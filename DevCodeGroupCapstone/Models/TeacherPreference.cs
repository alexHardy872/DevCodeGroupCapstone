﻿using System;
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

        public int defaultLessonLength { get; set; }


        [Display(Name = "Increase Cost Per")]
        public RadiusOptions distanceType { get; set; }

        public int maxDistance { get; set; }

        [DataType(DataType.Currency)]
        public decimal incementalCost { get; set; }

    

        [ForeignKey("Teacher")]
        public int? teacherId { get; set; }
        public Person Teacher { get; set; }



    }
}