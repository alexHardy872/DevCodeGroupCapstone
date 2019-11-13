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

        [System.Web.Http.HttpGet]
        public async Task<IHttpActionResult> Index()
        {
            // todo: these are intended to be query strings, but putting them as parameters didn't work
            string TeacherId = "1";
            string beginningCalendarDate = "2019-11-12T21:13:52.460Z";

            DateTime beginningCalendarDateTime = DateTime.Parse(beginningCalendarDate); // use this for the date
            int teacherIdInt = int.Parse(TeacherId);

            List<Event> eventList = new List<Event>();

            try
            {
                var lessons = await Task.Run(() => context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == teacherIdInt)
                    .ToList()
                    );


                eventList.AddRange(GenerateEventsFromLessons(lessons));

                var availabilities = await Task.Run(() => context.TeacherAvailabilities
                    .Where(a => a.PersonId == teacherIdInt)
                    .ToList()
                    );


                var preferences = await Task.Run(() => context.Preferences
                    .Where(p => p.teacherId == teacherIdInt)
                    .SingleOrDefault()
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

                        if (IsTimeAvailable(lessons, currentEvent))
                        {
                            eventList.Add(currentEvent);
                        }

                        workingStartTime = currentEvent.end;
                        finishedTime = workingStartTime + timeSpanOfLesson;
                        endTimeOfLastEvent = finishedTime.TimeOfDay;
                    }
                }
                return Ok(eventList);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
;
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

                events.Add(currentEvent);
            }

            return events;
        }

        private bool IsTimeAvailable(List<Lesson> lessons, Event newEvent) // newEvent is available timeslot
        {
            bool IsTimeAvailable = false;

            foreach (Lesson lesson in lessons)
            {
                if ((lesson.start <= newEvent.start && lesson.end <= newEvent.end) || (lesson.start >= newEvent.start && lesson.end >= newEvent.end))
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

        // private DateTime AdjustStartTime

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
