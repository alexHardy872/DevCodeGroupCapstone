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

        public static List<Event> GenerateEventsFromLessons(List<Lesson> lessons)
        {
            List<Event> events = new List<Event>();

            foreach (Lesson lesson in lessons)
            {
                // todo: create private function CreateTitle(Lesson lesson)
                StringBuilder titleBuild = new StringBuilder();
                titleBuild.Append(lesson.Student.firstName);
                titleBuild.Append(" @ ");
                if (lesson.travelDuration == 0)
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

                events.Add(currentEvent);
            }

            return events;
        }

        public static List<TeacherAvail> AddDatesToAvailabilities(List<TeacherAvail> availabilities, DateTime beginningDate, DateTime endingDate)
        {

            DateTime workingDate = beginningDate;

            List<TeacherAvail> newTa = new List<TeacherAvail>();

            while (workingDate <= endingDate)
            {
                List<TeacherAvail> ta = availabilities.Where(t => t.weekDay == workingDate.DayOfWeek).ToList();

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

                workingDate.AddDays(1);

            }

            return newTa;

        }

        private static DateTime GetNextDateForDateTime(DayOfWeek dayOfWeek, DateTime dateTime)
        {
            while (dateTime.DayOfWeek != dayOfWeek)
            {
                dateTime = dateTime.AddDays(1);
            }

            return dateTime.Date; // time is zeroed out
        }

        private static DateTime CombineDateAndTime(DateTime date, DateTime time)
        {
            return date + time.TimeOfDay;
        }


        public static List<Event> CreatePriorAvailabilities(TeacherPreference preferences, DateTime lowerLimit, DateTime lessonStart)
        {
            List<Event> availabilityEvents = new List<Event>();
            TimeSpan LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);

            DateTime workingStart = lessonStart - LessonLength;
            DateTime workingEnd = lessonStart;

            // loop through each until start is before the availableTimespan
            while (workingStart >= lowerLimit)
            {
                // check: is start and end inside timespan
                if (IsStartAndEndInsideTimeSpan(workingStart, workingEnd, lowerLimit, DateTime.MaxValue))
                {
                    // create available timeslot
                    Event availableSlot = Event.CreateAvailableTimeSlot(preferences, workingStart, workingEnd);

                    // add it
                    availabilityEvents.Add(availableSlot);
                }

                // update workingStart
                workingStart -= LessonLength;

                // update workingEnd
                workingEnd -= LessonLength;
            }

            return availabilityEvents;
        }

        public static List<Event> CreateNextAvailabilities(TeacherPreference preferences, DateTime upperLimit, DateTime lessonEnd)
        {
            List<Event> availabilityEvents = new List<Event>();
            TimeSpan LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);

            DateTime workingStart = lessonEnd;
            DateTime workingEnd = workingStart + LessonLength;

            // loop through each until start is before the availableTimespan
            while (workingEnd <= upperLimit)
            {
                // check: is start and end inside timespan
                if (IsStartAndEndInsideTimeSpan(workingStart, workingEnd, DateTime.MinValue, upperLimit))
                {
                    // create available timeslot
                    Event availableSlot = Event.CreateAvailableTimeSlot(preferences, workingStart, workingEnd);

                    // add it
                    availabilityEvents.Add(availableSlot);
                }

                // update workingStart
                workingStart += LessonLength;

                // update workingEnd
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