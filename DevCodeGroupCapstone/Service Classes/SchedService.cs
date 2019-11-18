using DevCodeGroupCapstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace DevCodeGroupCapstone.Service_Classes
{
    public static class SchedService
    {

        public static List<Event> GenerateEventsFromLessons(TeacherPreference preferences, List<Lesson> lessons)
        {
            List<Event> events = new List<Event>();

            foreach (Lesson lesson in lessons)
            {
                // todo: create private function CreateTitle(Lesson lesson)
                StringBuilder titleBuild = new StringBuilder();
                titleBuild.Append(lesson.Student.firstName);
                titleBuild.Append(" @ ");
                if (lesson.travelDuration != 0)
                {
                    titleBuild.Append(lesson.Location.address1);
                    titleBuild.Append(", ");
                    titleBuild.Append(lesson.Location.zip);
                }
                else
                {
                    titleBuild.Append("In Studio");
                }

                string title = titleBuild.ToString();

                Event currentEvent = new Event();
                currentEvent.start = AddDriveTimeBeforeLesson(lesson);
                currentEvent.end = AddDriveTimeAfterLesson(lesson);
                currentEvent.officialStart = lesson.start;
                currentEvent.officialEnd = lesson.end;
                currentEvent.backgroundColor = "#f7a072";
                currentEvent.textColor = "#000000";
                currentEvent.title = title;
                currentEvent.groupId = "Lesson";
                currentEvent.price = CreatePrice(preferences.PerHourRate, currentEvent.start, currentEvent.end);

                events.Add(currentEvent);
            }

            return events;
        }

        public static decimal CreatePrice(decimal perHourRate, DateTime start, DateTime end)
        {

            TimeSpan timeSpan = end - start;
            Double totalMinutes = timeSpan.TotalMinutes;
            Decimal costPerMinute = perHourRate / 60;
            Decimal totalCost = Convert.ToDecimal(totalMinutes) * costPerMinute;

            return totalCost;
        }

        public static List<TeacherAvail> AddDatesToAvailabilities(List<TeacherAvail> availabilities, DateTime beginningDate, DateTime endingDate)
        {

            DateTime workingDate = beginningDate;

            List<TeacherAvail> newTa = new List<TeacherAvail>();

            while (workingDate <= endingDate)
            {
                List<TeacherAvail> ta = availabilities.Where(t => (int)t.weekDay == Convert.ToInt32(workingDate.DayOfWeek)).ToList();

                if (ta != null && ta.Count > 0)
                {
                    foreach (TeacherAvail teachAvail in ta)
                    {
                        TeacherAvail newTeachAvail = new TeacherAvail
                        {
                            availId = teachAvail.availId,
                            weekDay = teachAvail.weekDay,
                            start = CombineDateAndTime(workingDate, teachAvail.start),
                            end = CombineDateAndTime(workingDate, teachAvail.end),
                            PersonId = teachAvail.PersonId,
                            TeacherId = teachAvail.TeacherId

                        };

                        newTa.Add(newTeachAvail);
                    }

                }

                workingDate = workingDate.AddDays(1);

            }

            return newTa;

        }

        private static DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return date + time.TimeOfDay;
        }


        public static List<Event> CreatePriorAvailabilities(TeacherPreference preferences, DateTime lowerLimit, DateTime lessonStart, int travelDuration = 0)
        {
            List<Event> availabilityEvents = new List<Event>();
            TimeSpan LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);

            if (travelDuration > 0)
            {
                LessonLength = LessonLength + ConvertIntToTimeSpan(travelDuration * 2);
            }

            DateTime workingStart = lessonStart - LessonLength;
            DateTime workingEnd = lessonStart;
            int ProximalLessonCheck = 0;

            // loop through each until start is before the availableTimespan
            while (workingStart >= lowerLimit)
            {
                // check: is start and end inside timespan
                if (IsStartAndEndInsideTimeSpan(workingStart, workingEnd, lowerLimit, DateTime.MaxValue))
                {
                    // create available timeslot
                    Event availableSlot = Event.CreateAvailableTimeSlot(preferences, workingStart, workingEnd, travelDuration);

                    // add it
                    availabilityEvents.Add(availableSlot);
                }

                ProximalLessonCheck += 1;
                if (ProximalLessonCheck == preferences.NumberOfProximalLessons)
                {
                    break;
                }
               

                // update workingStart
                workingStart -= LessonLength;

                // update workingEnd
                workingEnd -= LessonLength;
            }

            return availabilityEvents;
        }

        public static List<Event> CreateNextAvailabilities(TeacherPreference preferences, DateTime upperLimit, DateTime lessonEnd, bool proximalLessonCheck = true, int travelDuration = 0)
        {
            List<Event> availabilityEvents = new List<Event>();
            TimeSpan LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);

            if(travelDuration > 0)
            {
                LessonLength = LessonLength + ConvertIntToTimeSpan(travelDuration * 2);
            }

            DateTime workingStart = lessonEnd;
            DateTime workingEnd = workingStart + LessonLength;
            int ProximalLessonCheck = 0;

            // loop through each until start is before the availableTimespan
            while (workingEnd <= upperLimit)
            {

                // check: is start and end inside timespan
                // todo: do I even need to make this check?
                if (IsStartAndEndInsideTimeSpan(workingStart, workingEnd, DateTime.MinValue, upperLimit))
                {
                    // create available timeslot
                    Event availableSlot = Event.CreateAvailableTimeSlot(preferences, workingStart, workingEnd, travelDuration);

                    // add it
                    availabilityEvents.Add(availableSlot);
                }

                ProximalLessonCheck += 1;
                if (ProximalLessonCheck == preferences.NumberOfProximalLessons && proximalLessonCheck)
                {
                    break;
                }

                workingStart += LessonLength;
                workingEnd += LessonLength;
            }

            return availabilityEvents;
        }

        private static bool IsStartAndEndInsideTimeSpan(DateTime start, DateTime end, DateTime spanStart, DateTime spanEnd)
        {
            return start >= spanStart && end <= spanEnd;
        }

        public static TimeSpan ConvertIntToTimeSpan(int timeInMinutes)
        {
            Double MinutesInDouble = Convert.ToDouble(timeInMinutes);
            return TimeSpan.FromMinutes(MinutesInDouble);

        }

        private static DateTime AddDriveTimeBeforeLesson(Lesson lesson)
        {
            double convertedLessonTime = Convert.ToDouble(lesson.travelDuration);
            TimeSpan lengthOfTimeToSubtractFromStart = TimeSpan.FromMinutes(convertedLessonTime);
            return lesson.start - lengthOfTimeToSubtractFromStart;
        }

        private static DateTime AddDriveTimeAfterLesson(Lesson lesson)
        {
            double convertedLessonTime = Convert.ToDouble(lesson.travelDuration);
            TimeSpan lengthOfTimeToAddToEnd = TimeSpan.FromMinutes(convertedLessonTime);
            return lesson.end + lengthOfTimeToAddToEnd;
        }



    }
}