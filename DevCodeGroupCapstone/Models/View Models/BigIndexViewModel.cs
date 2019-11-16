using System.Collections.Generic;

namespace DevCodeGroupCapstone.Models.View_Models
{
    public class BigIndexViewModel
    {
        public List<PersonAndLocationViewModel> teachersComp;

        public List<Lesson> studentLessons;

        public List<Lesson> teacherLessons;

        public List<Lesson> requestsForStudent;

        public List<Lesson> requestsForTeacher;

        public List<Lesson> makeups;

        public Person currentUser;


    }
}