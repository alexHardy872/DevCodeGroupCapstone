using DevCodeGroupCapstone.Models;
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
            // todo: intended to be query strings, but putting them as parameters didn't work
            string beginningCalendarDate = "2019-11-12T21:13:52.460Z"; // this should be set to the beginning of the month and adjust evry month change

            DateTime beginningCalendarDateTime = DateTime.Parse(beginningCalendarDate);

            switch (generateForView)
            {
                case "teacherSchedule":
                    return await ReturnTeacherScheduleForView(teacherIdInt, beginningCalendarDateTime);
                case "lessonOptions":
                    return await ReturnStudentLessonOptionsForView(teacherIdInt, beginningCalendarDateTime);
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
                List<Event> eventList = await GenerateAvailableSlotsForStudents(teacherIdInt, beginningCalendarDateTime);
                return Ok(eventList);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        private async Task<List<Event>> GenerateAvailableSlotsForStudents(int teacherIdInt, DateTime beginningCalendarDateTime)
        {
            List<Event> eventList = await GenerateTeacherCalendarView(teacherIdInt, beginningCalendarDateTime);
            List<Event> lessons = eventList.Where(evnt => evnt.groupId == "Lesson").ToList();
            List<Event> availabilities = eventList.Where(evnt => evnt.groupId == "Availability").ToList();
            List<Event> availabilitiesToReturnToStudent = new List<Event>();

            var preferences = await Task.Run(() => context.Preferences
                .Where(p => p.teacherId == teacherIdInt)
                .SingleOrDefault()
            );

            if(preferences.NumberOfProximalLessons == null || (1440 > preferences.NumberOfProximalLessons * preferences.defaultLessonLength))
            {
                return availabilities;
            }

            foreach (Event availability in availabilities)
            {
                if(IsEventBeforeOrAfterALessonWithInProximalTolerance(availability, lessons, preferences))
                {
                    availabilitiesToReturnToStudent.Add(availability);
                }

            }

            return availabilitiesToReturnToStudent;
        }

        private bool IsEventBeforeOrAfterALessonWithInProximalTolerance(Event evnt, List<Event> lessons, TeacherPreference preferences)
        {
            bool IsIt = true;

            foreach (Event lesson in lessons)
            {
                DateTime beforeLessonStart = GetBeforeTimeForProximalLessons(lesson, preferences);
                DateTime afterLessonEnd = GetAfterTimeForProximalLessons(lesson, preferences);

                if ((evnt.start >= beforeLessonStart && evnt.end <= lesson.start) || (evnt.start >= lesson.start && evnt.end <= afterLessonEnd))
                {
                    IsIt = true;
                }
                else
                {
                    return false;
                }

            }
            return IsIt;
        }

        private DateTime GetAfterTimeForProximalLessons(Event lesson, TeacherPreference preferences)
        {
            DateTime timeBeforeProximalLessons = lesson.end + GetTimeSpanAroundALesson(lesson, preferences);
            return timeBeforeProximalLessons;
        }

        private DateTime GetBeforeTimeForProximalLessons(Event lesson, TeacherPreference preferences)
        {
            DateTime timeBeforeProximalLessons = lesson.start - GetTimeSpanAroundALesson(lesson, preferences);
            return timeBeforeProximalLessons;
        }

        private TimeSpan GetTimeSpanAroundALesson(Event lesson, TeacherPreference preferences)
        {
            int numberOfProximalLessons = preferences.NumberOfProximalLessons ?? 0;
            double lengthOfTimeToCoverProximalLessonsInMinutes = preferences.defaultLessonLength * numberOfProximalLessons;
            return TimeSpan.FromMinutes(lengthOfTimeToCoverProximalLessonsInMinutes);
        }

        private async Task<List<Event>> GenerateTeacherCalendarView(int teacherIdInt, DateTime beginningCalendarDateTime)
        {
            List<Event> eventList = new List<Event>();

                var lessons = await Task.Run(() => context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == teacherIdInt)
                    .ToList()
                    );
                TeacherAvail teacherAvail = new TeacherAvail();



                eventList = GenerateEventsFromLessons(lessons);

                var availabilities = await Task.Run(() => context.TeacherAvailabilities
                    .Where(a => a.PersonId == teacherIdInt)
                    .ToList()
                    );


                var preferences = await Task.Run(() => context.Preferences
                    .Where(p => p.teacherId == teacherIdInt)
                    .SingleOrDefault()
                    //.FirstOrDefault()
                );

                double convertedLessonLength = Convert.ToDouble(preferences.defaultLessonLength);
                TimeSpan timeSpanOfLesson = TimeSpan.FromMinutes(convertedLessonLength);

                foreach (var availableTimeSpan in availabilities)
                {
                    DayOfWeek dayOfWeek = availableTimeSpan.weekDay;
                    DateTime workingDate = GetNextDayOfWeekForDateTime(dayOfWeek, beginningCalendarDateTime);
                    DateTime workingStartTime = availableTimeSpan.start;
                    DateTime finishedTime = workingStartTime + timeSpanOfLesson;
                    TimeSpan endTimeOfLastEvent = finishedTime.TimeOfDay;
                    TimeSpan endTimeOfFinalAvailableTimeSlot = availableTimeSpan.end.TimeOfDay;


                    while (endTimeOfLastEvent <= endTimeOfFinalAvailableTimeSlot)
                    {
                        Event currentEvent = new Event();
                        currentEvent.start = CombineDateAndTime(workingDate, workingStartTime);
                        currentEvent.end = CombineDateAndTime(workingDate, workingStartTime + timeSpanOfLesson);
                        currentEvent.backgroundColor = "#dbd4d3";
                        currentEvent.textColor = "#000000";
                        currentEvent.title = "Available";
                        currentEvent.groupId = "Availability";
                    // current event.url

                        if (IsTimeAvailable(lessons, currentEvent))
                        {
                            eventList.Add(currentEvent);
                        }

                        workingStartTime = currentEvent.end;
                        finishedTime = workingStartTime + timeSpanOfLesson;
                        endTimeOfLastEvent = finishedTime.TimeOfDay;
                    }
                }
                return eventList;
        }
        

        private List<Event> GenerateEventsFromLessons(List<Lesson> lessons)
        {
            List<Event> events = new List<Event>();

            foreach (Lesson lesson in lessons)
            {
                StringBuilder titleBuild = new StringBuilder();
                titleBuild.Append(lesson.Student.firstName);
                titleBuild.Append(" @ ");
                titleBuild.Append(lesson.Location.address1);
                titleBuild.Append(", ");
                titleBuild.Append(lesson.Location.zip);
                string title = titleBuild.ToString();

                Event currentEvent = new Event();
                currentEvent.start = lesson.start;
                currentEvent.end = lesson.end;
                currentEvent.backgroundColor = "#f7a072";
                currentEvent.textColor = "#000000";
                currentEvent.title = title;
                currentEvent.groupId = "Lesson";

                events.Add(currentEvent);
            }

            return events;
        }

        private bool IsTimeAvailable(List<Lesson> lessons, Event availableTimeSlotEvent)
        {
            bool IsTimeAvailable = false;

            foreach (Lesson lesson in lessons)
            {
                if (lesson.start >= availableTimeSlotEvent.end || lesson.end <= availableTimeSlotEvent.start)
                {
                    IsTimeAvailable = true;
                }
                else
                {
                    return false;
                }
            }

            return IsTimeAvailable;
        }

        private DateTime GetNextDayOfWeekForDateTime(DayOfWeek dayOfWeek, DateTime dateTime)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
            {
                dateTime = dateTime.AddDays(1);
            }

            return dateTime.Date; // time is zeroed out
        }

        private DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return date + time.TimeOfDay;
        }

        private int GetNumberOfDaysForView(string view)
        {
            switch (view)
            {
                case "timeGridWeek":
                    return 7;
                default:
                    return 0;
            }
        }


    }
}
