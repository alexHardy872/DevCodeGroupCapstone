using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models
{
    public class TeacherPreference
    {
        [Key]
        public int TeacherPreferenceId { get; set; }

        public TimeSpan defaultLessonLength { get; set; }

        
        public int maxMiles { get; set; }

        public int maxMinutes { get; set; }

        [DataType(DataType.Currency)]
        public decimal incementalCostPerMile { get; set; }

        [DataType(DataType.Currency)]
        public decimal incementalCostPerMinute { get; set; }

        [ForeignKey("Teacher")]
        public int? teacherId { get; set; }
        public Person Teacher { get; set; }



    }
}