using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models
{
    public class TeacherAvail
    {
        [Key]
        public int availId { get; set; }

        
        [Display(Name = "Day of Week")]
        public DayOfWeek weekDay { get; set; }

        [DataType(DataType.Time)]
        public DateTime start { get; set; }

        [DataType(DataType.Time)]
        public DateTime end { get; set; }


        [ForeignKey("TeacherId")]
        public int PersonId { get; set; }
        public Person TeacherId { get; set; }






    }
    }