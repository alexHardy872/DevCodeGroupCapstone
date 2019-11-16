using System;

namespace DevCodeGroupCapstone.Models
{
    public class Event : IComparable<Event>
    {
        public DateTime start { get; set; }

        public DateTime end { get; set; }
        public string title { get; set; }

        public string backgroundColor { get; set; }

        public string textColor { get; set; }
        public string groupId { get; set; }

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
    }
}