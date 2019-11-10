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

        [ForeignKey("Student")]//fk attr
        [Column(Order =0)]
        public string StudentId { get; set; } //fk's spot at the table
        public Person Student { get; set; }

        [ForeignKey("Teacher")]//fk attr
        [Column(Order =1)]
        public string TeacherId { get; set; } //fk's spot at the table
        public Person Teacher { get; set; }

        public string subject { get; set; }
        public double startTime { get; set; }
        public double endTime { get; set; }
        public double cost { get; set; }
        public bool teacherApproval { get; set; }

        [ForeignKey("Location")]//fk attr
        [Column(Order =2)]
        public int LocationId { get; set; } //fk's spot at the table
        public Location Location { get; set; }//the class the fk attr is referencing

    }
}