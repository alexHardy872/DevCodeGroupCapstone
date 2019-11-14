using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Models.View_Models;
using DevCodeGroupCapstone.Service_Classes;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DevCodeGroupCapstone.Controllers
{
    public class PersonController : Controller
    {
        public ApplicationDbContext context;


        public PersonController()
        {
            context = new ApplicationDbContext();
        }


        // GET: Person
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var userFound = context.People.Where(p => p.ApplicationId == userId).FirstOrDefault();
            if (userFound == null)
            {
                return RedirectToAction("Create");
            }
            List<PersonAndLocationViewModel> teachers = new List<PersonAndLocationViewModel>();
            List<Person> eligibleTeachers = context.People.Where(s => s.subjects != null && s.PersonId != userFound.PersonId).ToList();
            foreach (Person teacher in eligibleTeachers)
            {
                PersonAndLocationViewModel info = new PersonAndLocationViewModel();
                info.person = teacher;
                info.location = context.Locations.Where(l => l.LocationId == teacher.LocationId).Single();
                info.lessons = context.Lessons.Where(lesson => lesson.teacherId == teacher.PersonId).ToList();
                info.avails = context.TeacherAvailabilities.Where(av => av.PersonId == teacher.PersonId).ToList();
                teachers.Add(info);
            }
            List<Lesson> studentLessons = context.Lessons
                    .Include("Teacher")
                    .Include("Location")
                    .Where(lesson => lesson.studentId == userFound.PersonId).ToList();
            if (teachers == null)
            {
                return RedirectToAction("Index");
            }
            BigIndexViewModel bigModel = new BigIndexViewModel();
            bigModel.teachersComp = teachers;
            bigModel.studentLessons = studentLessons;
            bigModel.currentUser = userFound;
   
            return View(bigModel);
        }

        public ActionResult StudentIndex()
        {
            string userId = User.Identity.GetUserId();
            var userFound = context.People.Where(p => p.ApplicationId == userId).FirstOrDefault();

            if (userFound == null)
            {
                return RedirectToAction("Create");
            }
            List<PersonAndLocationViewModel> teachers = new List<PersonAndLocationViewModel>();
            List<Person> eligibleTeachers = context.People.Where(s => s.subjects != null && s.PersonId != userFound.PersonId).ToList();
            foreach (Person teacher in eligibleTeachers)
            {
                PersonAndLocationViewModel info = new PersonAndLocationViewModel();
                info.person = teacher;
                info.location = context.Locations.Where(l => l.LocationId == teacher.LocationId).Single();
                info.lessons = context.Lessons.Where(lesson => lesson.teacherId == teacher.PersonId).ToList();
                info.avails = context.TeacherAvailabilities.Where(av => av.PersonId == teacher.PersonId).ToList();
                teachers.Add(info);
            }
            List<Lesson> studentLessons = context.Lessons
                    .Include("Teacher")
                    .Include("Location")
                    .Where(lesson => lesson.studentId == userFound.PersonId).ToList();
            if (teachers == null)
            {
                return RedirectToAction("Index");
            }
            BigIndexViewModel bigModel = new BigIndexViewModel();
            bigModel.teachersComp = teachers;
            bigModel.studentLessons = studentLessons;
            bigModel.currentUser = userFound;
            return View(bigModel);
        }

        public ActionResult TeacherIndex()
        {
            string userId = User.Identity.GetUserId();
            var userFound = context.People.Where(p => p.ApplicationId == userId).FirstOrDefault();
            if (userFound == null)
            {
                return RedirectToAction("Create");
            }
            List<PersonAndLocationViewModel> teachers = new List<PersonAndLocationViewModel>();
            List<Person> eligibleTeachers = context.People.Where(s => s.subjects != null && s.PersonId != userFound.PersonId).ToList();
            foreach (Person teacher in eligibleTeachers)
            {
                PersonAndLocationViewModel info = new PersonAndLocationViewModel();
                info.person = teacher;
                info.location = context.Locations.Where(l => l.LocationId == teacher.LocationId).Single();
                info.lessons = context.Lessons.Where(lesson => lesson.teacherId == teacher.PersonId).ToList();
                info.avails = context.TeacherAvailabilities.Where(av => av.PersonId == teacher.PersonId).ToList();
                teachers.Add(info);
            }
            List<Lesson> studentLessons = context.Lessons
                    .Include("Teacher")
                    .Include("Location")
                    .Where(lesson => lesson.studentId == userFound.PersonId).ToList();
            if (teachers == null)
            {
                return RedirectToAction("Index");
            }
            BigIndexViewModel bigModel = new BigIndexViewModel();
            bigModel.teachersComp = teachers;
            bigModel.studentLessons = studentLessons;
            bigModel.currentUser = userFound;
            return View(bigModel);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            //tlc double metersToMiles = 1609.34;
            PersonAndLocationViewModel personLocationDetails = new PersonAndLocationViewModel();
            personLocationDetails.person = context.People.Include("Location").Where(p => p.PersonId == id).Single();
            personLocationDetails.location = context.Locations.Where(l => l.LocationId == personLocationDetails.person.LocationId).Single();

            var tempTeacher = context.Preferences.Where(p => p.teacherId == personLocationDetails.person.PersonId).SingleOrDefault();//tlc
            if (tempTeacher != null)
            {
                //double teacherPreferenceRadius = tempTeacher.maxDistance * metersToMiles;//tlc
                double teacherPreferenceRadius = tempTeacher.maxDistance;//tlc
                ViewBag.radius = teacherPreferenceRadius;//tlc
            }
            

            return View(personLocationDetails);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            Person person = new Person();
            Location location = new Location();

            PersonAndLocationViewModel info = new PersonAndLocationViewModel();
            info.person = person;
            info.location = location;

            return View(info);
        }

        // POST: Person/Create
        [HttpPost]
        public async Task<ActionResult> Create(PersonAndLocationViewModel info)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                info.person.ApplicationId = userId;
                int nextLocationId = context.Database.ExecuteSqlCommand("SELECT IDENT_CURRENT('dbo.Locations')") + 1;
                info.person.LocationId = nextLocationId;

                context.People.Add(info.person);

                string[] latLng = await GeoCode.GetLatLongFromApi(info.location);
                info.location.lat = latLng[0];
                info.location.lng = latLng[1];

                context.Locations.Add(info.location);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return View();
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            try//tlc
            {
                var tempPerson = context.People.Where(p => p.PersonId == id).SingleOrDefault();//tlc
                return View(tempPerson);
            }
            catch(Exception)
            {
                return RedirectToAction("Index");
            }
            
            //tlc return View();
            
        }

        // POST: Person/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Person personToEdit)
        {
            var personFromDb = context.People.Where(p=>p.PersonId == id).SingleOrDefault();
            try
            {
                // TODO: Add update logic here
                if (personFromDb != null)
                {
                    personFromDb = personToEdit;
                    personFromDb.firstName = personToEdit.firstName;
                    personFromDb.lastName = personToEdit.lastName;
                    personFromDb.subjects = personToEdit.subjects;
                    personFromDb.phoneNumber = personToEdit.phoneNumber;

                }

                context.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Details", new { id = personFromDb.PersonId });
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Person/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
