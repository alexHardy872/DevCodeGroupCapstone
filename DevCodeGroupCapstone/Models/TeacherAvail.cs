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
        public DateTime sundayStart { get; set; }
        public DateTime sundayStop { get; set; }
        public DateTime mondayStart { get; set; }
        public DateTime mondayStop { get; set; }
        public DateTime tuesdayStart { get; set; }
        public DateTime tuesdayStop { get; set; }
        public DateTime wednesdayStart { get; set; }
               
        public DateTime wednesdayStop { get; set; }
        public DateTime thursdayStart { get; set; }
        public DateTime thursdayStop { get; set; }
        public DateTime fridayStart { get; set; }
        public DateTime fridayStop { get; set; }
        public DateTime saturdayStart { get; set; }
        public DateTime saturdayStop { get; set; }



    }
    }