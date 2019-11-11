using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models
{
    public class TeacherDetails
    {
        [Key]
        public int TeacherDetailsId { get; set; }

        public TimeSpan defaultLessonLength { get; set; }

    }
}