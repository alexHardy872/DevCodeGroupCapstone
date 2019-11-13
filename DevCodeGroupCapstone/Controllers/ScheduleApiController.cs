using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DevCodeGroupCapstone.Models;
using System.Reflection;


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
            //////////////////////

            DateTime beginningCalendarDateTime = DateTime.Parse(beginningCalendarDate); // use this for the date
            int teacherIdInt = int.Parse(TeacherId);

            List<Event> eventList = new List<Event>();

            try
            {
                var availabilities = await Task.Run(() => context.TeacherAvailabilities
                    .Where(a => a.PersonId == teacherIdInt)
                    .ToList()
                    );

                TeacherAvail teacherAvail = new TeacherAvail();


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
                        eventList.Add(currentEvent);

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
;        }

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
