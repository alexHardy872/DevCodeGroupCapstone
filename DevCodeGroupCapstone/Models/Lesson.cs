using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevCodeGroupCapstone.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }

        public string subject { get; set; }
        public double startTime { get; set; }
        public double endTime { get; set; }
        public double cost { get; set; }
        public bool teacherApproval { get; set; }

        [ForeignKey("Student")]
        public string studentId { get; set; }
        public Person Student { get; set; }

        [ForeignKey("Teacher")]
        public string teacherId { get; set; }
        public Person Teacher { get; set; }


        [ForeignKey("Location")]//fk attr
        [Column(Order =2)]
        public int LocationId { get; set; } //fk's spot at the table
        public Location Location { get; set; }//the class the fk attr is referencing
        

    }
}