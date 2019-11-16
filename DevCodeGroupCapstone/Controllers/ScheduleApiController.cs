using DevCodeGroupCapstone.Models;
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
        public async Task<IHttpActionResult> Index(string generateForView, int teacherIdInt)
        {
            // todo: eliminate this
            string beginningCalendarDate = "2019-11-12T21:13:52.460Z"; // this should be set to the beginning of the month and adjust evry month change
            DateTime beginningCalendarDateTime = DateTime.Parse(beginningCalendarDate);


            switch (generateForView)
            {
                case "teacherSchedule":
                    return await ReturnTeacherScheduleForView(teacherIdInt, beginningCalendarDateTime);
                case "lessonOptions":
                    return await ReturnStudentLessonOptionsForView(teacherIdInt, beginningCalendarDateTime);
                    // case "inHomeLessonOptions"
            }

            return await Task.Run(() => Ok());
        }


        private async Task<IHttpActionResult> ReturnTeacherScheduleForView(int teacherIdInt, DateTime beginningCalendarDateTime)
        {

            try
            {
                List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt, beginningCalendarDateTime);
                return Ok(eventList);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private async Task<IHttpActionResult> ReturnStudentLessonOptionsForView(int teacherIdInt, DateTime beginningCalendarDateTime)
        {

            try
            {
                // todo: eliminate all remnants of GenerateAvailableSlotsForStudents
                //List<Event> eventList = await GenerateAvailableSlotsForStudents(teacherIdInt, beginningCalendarDateTime);

                List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt, beginningCalendarDateTime);

                eventList = eventList
                    .Where(evnt => evnt.groupId == "Availability")
                    .ToList();

                return Ok(eventList);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private async Task<List<Event>> GenerateTeacherCalendarView(int teacherIdInt, DateTime beginningCalendarDate)
        {
            // arrange
            List<Event> eventList = new List<Event>();

            List<Lesson> lessons = await Task.Run(() => context.Lessons
                .Include("Student")
                .Include("Location")
                // todo: add in other lesson filter constraints
                .Where(lesson => lesson.teacherId == teacherIdInt)
                .ToList()
                );

            eventList = SchedService.GenerateEventsFromLessons(lessons);
            List<Event> lessonEventList = SchedService.GenerateEventsFromLessons(lessons);

            List<TeacherAvail> availabilities = await Task.Run(() => context.TeacherAvailabilities
                    .Where(a => a.PersonId == teacherIdInt)
                    .ToList()
                    );

            // todo: make calendar views dynamic by changing this
            availabilities = SchedService.AddDatesToAvailabilities(availabilities, DateTime.Today.AddDays(-20), DateTime.Today.AddDays(30));

            TeacherPreference preferences = await Task.Run(() => context.Preferences
                .Where(p => p.teacherId == teacherIdInt)
                .SingleOrDefault()
            );

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
