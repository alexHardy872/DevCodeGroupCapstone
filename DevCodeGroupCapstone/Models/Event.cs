using DevCodeGroupCapstone.Service_Classes;
using System;

namespace DevCodeGroupCapstone.Models
{
    public class Event : IComparable<Event>
    {
        public Guid sortId { get; set; }

        public DateTime start { get; set; }

        public DateTime end { get; set; }

        public DateTime officialStart { get; set; }

        public DateTime officialEnd { get; set; }

        public string title { get; set; }

        public string backgroundColor { get; set; }

        public string textColor { get; set; }
        public string groupId { get; set; }

        public decimal price { get; set; }

        public TeacherPreference preferences { get; private set; }

        public Event()
        {

        }

        public Event(TeacherPreference preferences, Lesson lesson)
        {
            // todo: build lesson event creation here
            this.preferences = preferences;
        }

        public Event(TeacherPreference preferences, DateTime availabilityStart, DateTime availabilityEnd, int travelDuration = 0)
        {
            this.sortId = Guid.NewGuid();
            this.preferences = preferences;
            this.backgroundColor = "#dbd4d3";
            this.textColor = "#000000";
            this.title = "Available";
            this.groupId = "Availability";
            this.start = availabilityStart;
            this.end = availabilityEnd;
            this.officialStart = availabilityStart + SchedService.ConvertIntToTimeSpan(travelDuration);
            this.officialEnd = availabilityEnd - SchedService.ConvertIntToTimeSpan(travelDuration);
            this.price = SchedService.CreatePrice(preferences.PerHourRate, this.start, this.end);
        }

        public int CompareTo(Event other)
        {
            if (start < other.start)
            {
                return -1;
            }
            else if (start > other.start)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static Event CreateAvailableTimeSlot(TeacherPreference preferences, DateTime start, DateTime end, int travelDuration = 0)
        {
            return new Event(preferences, start, end, travelDuration);
        }



    }
}