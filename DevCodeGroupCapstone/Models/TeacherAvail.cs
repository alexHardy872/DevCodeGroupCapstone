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
        public string sundayStart { get; set; }
        public string sundayStop { get; set; }
        public string mondayStart { get; set; }
        public string mondayStop { get; set; }
        public string tuesdayStart { get; set; }
        public string tuesdayStop { get; set; }
        public string wednesdayStart { get; set; }

        public string wednesdayStop { get; set; }
        public string thursdayStart { get; set; }
        public string thursdayStop { get; set; }
        public string fridayStart { get; set; }
        public string fridayStop { get; set; }
        public string saturdayStart { get; set; }
        public string saturdayStop { get; set; }



    }
    }