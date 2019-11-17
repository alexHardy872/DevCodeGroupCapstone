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
         
            BigIndexViewModel bigModel = new BigIndexViewModel();
           
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
                    .Where(lesson => lesson.studentId == userFound.PersonId && lesson.teacherApproval == true && lesson.requiresMakeup == false).ToList();
            List<Lesson> lessonRequests = context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.studentId == userFound.PersonId && lesson.teacherApproval == false && lesson.requiresMakeup == false).ToList();

            List<Lesson> makeupLessons = context.Lessons
                    .Include("Teacher")
                    .Include("Location")
                    .Where(lesson => lesson.studentId == userFound.PersonId && lesson.requiresMakeup == true).ToList();

            BigIndexViewModel bigModel = new BigIndexViewModel();
            bigModel.teachersComp = teachers;
            bigModel.studentLessons = studentLessons;
            bigModel.currentUser = userFound;
            bigModel.requestsForStudent = lessonRequests;
            bigModel.makeups = makeupLessons;
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

            //// maybe redirect if teacher not setup?

            //int preferencesCheck = context.Preferences.Where(p => p.teacherId == userFound.PersonId).Count();
            //int availsCheck = context.TeacherAvailabilities.Where(p => p.PersonId == userFound.PersonId).Count();

            //if (userFound.subjects == null || preferencesCheck == 0 || availsCheck == 0)
            //{
            //    return RedirectToAction("Index"); // redirect to index/ or info page
            //}
            
            List<Lesson> teacherLessons = context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == userFound.PersonId && lesson.teacherApproval == true && lesson.requiresMakeup == false).ToList();

            List<Lesson> lessonRequests = context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == userFound.PersonId && lesson.teacherApproval == false && lesson.requiresMakeup == false).ToList();

            List<Lesson> makeupLessons = context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == userFound.PersonId && lesson.requiresMakeup == true).ToList();

            BigIndexViewModel bigModel = new BigIndexViewModel();        
            bigModel.teacherLessons = teacherLessons;
            bigModel.requestsForTeacher = lessonRequests;
            bigModel.currentUser = userFound;
            bigModel.makeups = makeupLessons;

            return View(bigModel);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            PersonAndLocationViewModel personLocationDetails = new PersonAndLocationViewModel();
            personLocationDetails.person = context.People.Include("Location").Where(p => p.PersonId == id).Single();
            personLocationDetails.location = context.Locations.Where(l => l.LocationId == personLocationDetails.person.LocationId).Single();

            var tempTeacher = context.Preferences.Where(p => p.teacherId == personLocationDetails.person.PersonId).SingleOrDefault();//tlc
            if (tempTeacher != null)                
            {
                if (tempTeacher.distanceType == RadiusOptions.Miles)
                {
                    double teacherPreferenceRadius = tempTeacher.maxDistance;//tlc
                    ViewBag.radius = teacherPreferenceRadius * Service_Classes.DistanceMatrix.metersToMiles;//tlc
                }
                else if (tempTeacher.distanceType == RadiusOptions.Miles)
                {
                    ViewBag.radius = tempTeacher.maxDistance;
                }
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
                var tempPerson = context.People
                        .Include("Location")
                        .Where(p => p.PersonId == id).SingleOrDefault();//tlc
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
        public async Task<ActionResult> Edit(int id, Person personToEdit)
        {
            Person personFromDb = context.People
                .Include("Location")
                .Where(p=>p.PersonId == id).SingleOrDefault();

            Location locationfromDb = context.Locations.Where(l => l.LocationId == personFromDb.LocationId).FirstOrDefault();
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
                    personFromDb.ApplicationId = personToEdit.ApplicationId;

                    locationfromDb.address1 = personToEdit.Location.address1;
                    locationfromDb.address2 = personToEdit.Location.address2;
                    locationfromDb.city = personToEdit.Location.city;
                    locationfromDb.state = personToEdit.Location.state;
                    locationfromDb.zip = personToEdit.Location.zip;

                    string[] latLng = await GeoCode.GetLatLongFromApi(personToEdit.Location);
                    personToEdit.Location.lat = latLng[0];
                    personToEdit.Location.lng = latLng[1];

                    locationfromDb.lat = personToEdit.Location.lat;
                    locationfromDb.lng = personToEdit.Location.lng;

                }

                context.SaveChanges();
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Index");
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
