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
        public int PersonId { get; set; }
        public string sunday { get; set; }
        public string monday { get; set; }
        public string tuesday { get; set; }
        public string wednesday { get; set; }
        public string thursday { get; set; }
        public string friday { get; set; }
        //tlc
        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; } //fk's spot at the table
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing
    }
}