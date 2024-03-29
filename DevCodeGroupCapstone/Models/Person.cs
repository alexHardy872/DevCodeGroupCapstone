﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevCodeGroupCapstone.Models
{
    public class Person
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string subjects { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        [ForeignKey("ApplicationUser")]//fk attr
        public string ApplicationId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }//the class the fk attr is referencing

        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public Location Location { get; set; }


    }
}
