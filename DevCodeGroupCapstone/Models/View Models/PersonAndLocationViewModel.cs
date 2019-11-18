using System.Collections.Generic;

namespace DevCodeGroupCapstone.Models.View_Models
{
    public class PersonAndLocationViewModel
    {
        public Person person { get; set; }

        public Location location { get; set; }

        public List<Lesson> lessons { get; set; }

        public List<TeacherAvail> avails { get; set; }

        public int studentId { get; set; }

        public bool outOfRange { get; set; }

        public int outOfRangeNum { get; set; }

        public decimal inHomeCost { get; set; }

        public decimal studioCost { get; set; }


        public int? studentLocationId { get; set; }





    }
}