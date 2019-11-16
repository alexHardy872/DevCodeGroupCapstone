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
                currentEvent.backgroundColor = "#f7a072";
                currentEvent.textColor = "#000000";
                currentEvent.title = title;
                currentEvent.groupId = "Lesson";

                events.Add(currentEvent);
            }

            return events;
        }

        public static List<Event> CreatePriorAvailabilities(TeacherPreference preferences, TeacherAvail availableTimeSpan, DateTime lessonStartTime, bool inHome)
        {
            List<Event> availabilityEvents = new List<Event>();
            TimeSpan LessonLength;

            if (inHome == false)
            {
                LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);
            }
            else
            {
                // todo: include drive time
                LessonLength = ConvertIntToTimeSpan(preferences.defaultLessonLength);
            }

            DateTime workingStart = lessonStartTime - LessonLength;
            DateTime workingEnd = lessonStartTime;

            // loop through each until start is before the availableTimespan
            while (workingStart >= availableTimeSpan.start)
            {
                // check: is start and end inside timespan
                if (IsStartAndEndInsideTimeSpan(workingStart, workingEnd, availableTimeSpan.start, availableTimeSpan.end))
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

        private static bool IsStartAndEndInsideTimeSpan(DateTime start, DateTime end, DateTime spanStart, DateTime spanEnd)
        {
            return start >= spanStart || end <= spanEnd;
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