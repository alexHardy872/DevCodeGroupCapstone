using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models
{
    public class Person
    {
        [Key]       
        public string PersonId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string subjects { get; set; }

        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }

        [ForeignKey("Location")]
        public int LocationId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing
        public Location Location { get; set; }

    }
}
