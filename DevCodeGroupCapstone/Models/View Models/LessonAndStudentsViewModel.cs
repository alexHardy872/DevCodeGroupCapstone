using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevCodeGroupCapstone.Models.View_Models
{
    public class LessonAndStudentsViewModel
    {
        public List<Person> students { get; set; }

        public Lesson lesson { get; set; }


    }
}