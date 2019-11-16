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