﻿using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Service_Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;


namespace DevCodeGroupCapstone.Controllers
{
    public class ScheduleApiController : ApiController
    {
        public ApplicationDbContext context;

        public ScheduleApiController()
        {
            context = ApplicationDbContext.Create();
        }

        [HttpGet]
        public async Task<IHttpActionResult> Index(string generateForView, int teacherIdInt, int studentIdInt)
        {

            switch (generateForView)
            {
                case "teacherSchedule":
                    return await ReturnTeacherScheduleForView(teacherIdInt);
                case "lessonOptions":
                    return await ReturnStudentLessonOptionsForView(teacherIdInt);
                case "inHomeLessonOptions":
                    return await ReturnStudentInHomeLessonOptions(teacherIdInt, studentIdInt);
            }

            return await Task.Run(() => Ok());
        }

        private async Task<IHttpActionResult> ReturnStudentInHomeLessonOptions (int teacherIdInt, int studentIdInt)
        {
            {

                try
                {
                    Lesson lesson = new Lesson
                    {
                        studentId = studentIdInt,
                        teacherId = teacherIdInt
                    };

                    Person student = await Task.Run(() => context.People
                        .Include("Location")
                        .Where(person => person.PersonId == studentIdInt)
                        .SingleOrDefault()
                        );

                    TeacherPreference preferences = await Task.Run(() => context.Preferences
                       .Include("Teacher")
                       .Where(pref => pref.teacherId == teacherIdInt)
                       .SingleOrDefault()
                       );

                    lesson.LocationId = student.LocationId;
                    lesson.Price = preferences.PerHourRate;

                    // use lesson to get drive time
                    lesson = await DistanceMatrix.GetTravelInfo(lesson);



                    // use the drive time to create availability events 
                    // add a boolean with a default to false for in-home lessons
                    // create special event constructor for in-home lessons
                    List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt, lesson.travelDuration);

                    return Ok(eventList);
                }
                catch (Exception e)
                {

                    List<Event> emptyList = new List<Event>();
                    return Ok(emptyList);
                }
            }


        }

        private async Task<IHttpActionResult> ReturnTeacherScheduleForView(int teacherIdInt)
        {

            try
            {
                List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt);
                return Ok(eventList);
            }
            catch (Exception e)
            {

                List<Event> emptyList = new List<Event>();
                return Ok(emptyList);
            }
        }

        private async Task<IHttpActionResult> ReturnStudentLessonOptionsForView(int teacherIdInt)
        {

            try
            {
                List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt);

                eventList = eventList
                    .Where(evnt => evnt.groupId == "Availability")
                    .ToList();

                return Ok(eventList);
            }
            catch (Exception e)
            {
                List<Event> emptyList = new List<Event>();
                return Ok(emptyList);
            }
        }

        private async Task<List<Event>> GenerateTeacherCalendarView(int teacherIdInt, int travelDuration = 0)
        {
            // arrange
            List<Event> eventList = new List<Event>();

            TeacherPreference preferences = await Task.Run(() => context.Preferences
            .Where(p => p.teacherId == teacherIdInt)
            .SingleOrDefault()
        );

            List<Lesson> lessons = await Task.Run(() => context.Lessons
                .Include("Student")
                .Include("Location")
                // todo: add in other lesson filter constraints
                .Where(lesson => lesson.teacherId == teacherIdInt && lesson.teacherApproval)
                .ToList()
                );

            eventList = SchedService.GenerateEventsFromLessons(preferences, lessons);
            List<Event> lessonEventList = SchedService.GenerateEventsFromLessons(preferences, lessons);

            List<TeacherAvail> availabilities = await Task.Run(() => context.TeacherAvailabilities
                    .Where(a => a.PersonId == teacherIdInt)
                    .ToList()
                    );

            // todo: make calendar views dynamic by changing this
            availabilities = SchedService.AddDatesToAvailabilities(availabilities, DateTime.Today.AddDays(-20), DateTime.Today.AddDays(30));

            double convertedLessonLength = Convert.ToDouble(preferences.defaultLessonLength);
            TimeSpan timeSpanOfLesson = TimeSpan.FromMinutes(convertedLessonLength);

            // act on Available Timespans
            foreach (var availableTimeSpan in availabilities)
            {

                // find the lesson events that are inclusive to that time span
                List<Event> filteredLessonList = lessonEventList
                    .Where(lessn => lessn.start >= availableTimeSpan.start && lessn.end <= availableTimeSpan.end)
                    .ToList();

                if (filteredLessonList.Count == 0)
                {
                    List<Event> listOfAfterEvents = SchedService.CreateNextAvailabilities(preferences, availableTimeSpan.end, availableTimeSpan.start, false);
                    eventList.AddRange(listOfAfterEvents);

                    continue;
                }

                // sort those lesson events
                filteredLessonList.Sort();

                // take the first one and create the new available events before it, checking if it fits in the available slot
                List<Event> listOfPreEvents = SchedService.CreatePriorAvailabilities(preferences, availableTimeSpan.start, filteredLessonList[0].start);
                eventList.AddRange(listOfPreEvents);

                int currentLessonNumber = 0;
                int nextLessonNumber = 1;

                // fill in the times between the lesson events, evenly
                while (nextLessonNumber < filteredLessonList.Count)
                {
                    TimeSpan timeSpan = filteredLessonList[nextLessonNumber].start - filteredLessonList[currentLessonNumber].end;

                    if (timeSpan.TotalMinutes > Convert.ToDouble(2 * preferences.defaultLessonLength))
                    {
                        // find the mid point between the lessons
                        TimeSpan timeBetweenLessons = filteredLessonList[nextLessonNumber].start - filteredLessonList[currentLessonNumber].end;
                        Double timeBetweenLessonsInMinutes = timeBetweenLessons.TotalMinutes;
                        Double midpointBetweenLessons = timeBetweenLessonsInMinutes / 2;
                        TimeSpan timeSpanOfMidpointFromEndOfCurrentLesson = TimeSpan.FromMinutes(midpointBetweenLessons);
                        DateTime midpoint = filteredLessonList[currentLessonNumber].end + timeSpanOfMidpointFromEndOfCurrentLesson;

                        List<Event> listOfAfterEvents = SchedService.CreateNextAvailabilities(preferences, midpoint, filteredLessonList[currentLessonNumber].end);
                        eventList.AddRange(listOfAfterEvents);

                        List<Event> listOfBeforeEvents = SchedService.CreatePriorAvailabilities(preferences, midpoint, filteredLessonList[nextLessonNumber].start);
                        eventList.AddRange(listOfBeforeEvents);

                    }
                    else
                    {
                        List<Event> listOfAfterEvents = SchedService.CreateNextAvailabilities(preferences, filteredLessonList[nextLessonNumber].start, filteredLessonList[currentLessonNumber].end);
                        eventList.AddRange(listOfAfterEvents);
                    }

                    currentLessonNumber += 1;
                    nextLessonNumber += 1;

                }

                // take the last lesson and fill in afterwards
                List<Event> listOfPostEvents = SchedService.CreateNextAvailabilities(preferences, availableTimeSpan.end, filteredLessonList[filteredLessonList.Count - 1].end);
                eventList.AddRange(listOfPostEvents);

            }

            return eventList;

        
        }
    }
}
