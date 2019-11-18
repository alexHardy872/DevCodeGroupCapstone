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


            // maybe redirect if teacher not setup?

            //int preferencesCheck = context.Preferences.Where(p => p.teacherId == userFound.PersonId).Count();
            //int availsCheck = context.TeacherAvailabilities.Where(p => p.PersonId == userFound.PersonId).Count();

            //if (userFound.subjects == null || preferencesCheck == 0 || availsCheck == 0)
            //{
            //    return RedirectToAction("Index"); // redirect to index/ or info page
            //}


            List<Lesson> teacherLessons = context.Lessons
                    .Include("Student")
                    .Include("Location")
                    .Where(lesson => lesson.teacherId == userFound.PersonId && lesson.teacherApproval == true && lesson.requiresMakeup == false && lesson.studentId != null).ToList();

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
        public async Task<ActionResult> Details(int id)
        {
            string head = User.Identity.GetUserId();

            Person student = context.People
                .Include("Location")
                .Where(per => per.ApplicationId == head)
                .FirstOrDefault()
                ;

            //this is the teacher!
            PersonAndLocationViewModel personLocationDetails = new PersonAndLocationViewModel();
            personLocationDetails.person = context.People
                .Include("Location")
                .Where(p => p.PersonId == id)
                .Single();
            //this is the teacher's location!
            personLocationDetails.location = context.Locations.Where(l => l.LocationId == personLocationDetails.person.LocationId).Single();

            personLocationDetails.studentLocationId = student.LocationId;
            personLocationDetails.studentId = student.PersonId;

            TeacherPreference tpreffer = context.Preferences.Where(pref => pref.teacherId == id).FirstOrDefault();

            int range = tpreffer.maxDistance;
            RadiusOptions type = tpreffer.distanceType;
            decimal increment = tpreffer.incrementalCost;

            decimal inHomeCost;

            //student.PersonId is still ok to use here!
            //string userId = User.Identity.GetUserId();
            //Person studentA = context.People.Where(peop => peop.ApplicationId == userId).FirstOrDefault();
            //int tempDistance = await Service_Classes.DistanceMatrix.GetTravelInfo(studentA, personLocationDetails.person);
            int tempDuration = await Service_Classes.DistanceMatrix.GetTravelInfo(student, personLocationDetails.person);
            inHomeCost = tpreffer.PerHourRate + (tpreffer.incrementalCost * tempDuration);


            personLocationDetails.outOfRange = tempDuration > tpreffer.maxDistance ? true : false;
            personLocationDetails.outOfRangeNum = personLocationDetails.outOfRange ? 1 : 0;
            personLocationDetails.inHomeCost = inHomeCost;
            personLocationDetails.studioCost = tpreffer.PerHourRate;


            var tempTeacher = context.Preferences.Where(p => p.teacherId == personLocationDetails.person.PersonId).SingleOrDefault();//tlc

            //this is the teacher's preferences
            var teacherPreferences = context.Preferences.Where(p => p.teacherId == personLocationDetails.person.PersonId).SingleOrDefault();//tlc

            //if (teacherPreferences != null)
            //{
            //    if (teacherPreferences.distanceType == RadiusOptions.Miles)
            //    {
            //        //string userId = User.Identity.GetUserId();

            //        double teacherPreferenceRadius = teacherPreferences.maxDistance;//tlc
            //        ViewBag.radius = teacherPreferenceRadius * Service_Classes.DistanceMatrix.metersToMiles;//tlc

            //        var tempStudent = context.People.Include("Location").Where(p => p.ApplicationId == userId).SingleOrDefault();
                    
            //        if (tempStudent.PersonId != teacherPreferences.teacherId)
            //        {
            //            Lesson tempLesson = new Lesson();
            //            tempLesson.Teacher = context.People.Where(t=>t.PersonId == teacherPreferences.teacherId).SingleOrDefault();
            //            tempLesson.Student = tempStudent;
            //            tempLesson = await Service_Classes.DistanceMatrix.GetTravelInfo(tempLesson);

            //            //ViewBag.lessonPrice =                         
            //            ViewBag.outOfRange = teacherPreferences.maxDistance > tempLesson.travelDuration;
            //        }
                    
            //    }
            //    else if (tempTeacher.distanceType == RadiusOptions.Minutes)
            //    {
            //        ViewBag.radius = teacherPreferences.maxDistance;
            //    }
            //}

            

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
        public ActionResult Edit()
        {
            try//tlc
            {
               
                    string userId = User.Identity.GetUserId();
                    Person current = context.People.Where(p => p.ApplicationId == userId).FirstOrDefault();
                    int id = current.PersonId;
                

                var tempPerson = context.People
                        .Include("Location")
                        .Where(p => p.PersonId == id).SingleOrDefault();//tlc
                return View(tempPerson);
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }

            //tlc return View();

        }

        // POST: Person/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Person personToEdit)
        {
            string userId = User.Identity.GetUserId();
            Person current = context.People.Where(p => p.ApplicationId == userId).FirstOrDefault();
            int id = current.PersonId;

            Location locationfromDb = context.Locations.Where(l => l.LocationId == current.LocationId).FirstOrDefault();
            try
            {
                // TODO: Add update logic here
                if (current != null)
                {              
                    Location locationToEdit = personToEdit.Location;

                    current.PersonId = personToEdit.PersonId;
                    current.LocationId = personToEdit.LocationId;
                    current.firstName = personToEdit.firstName;
                    current.lastName = personToEdit.lastName;
                    current.subjects = personToEdit.subjects;
                    current.phoneNumber = personToEdit.phoneNumber;
                    current.ApplicationId = personToEdit.ApplicationId;

                    locationfromDb.LocationId = current.Location.LocationId;
                    locationfromDb.address1 = locationToEdit.address1;
                    locationfromDb.address2 = locationToEdit.address2;
                    locationfromDb.city = locationToEdit.city;
                    locationfromDb.state = locationToEdit.state;
                    locationfromDb.zip = locationToEdit.zip;

                    string[] latLng = await GeoCode.GetLatLongFromApi(locationToEdit);
                    locationToEdit.lat = latLng[0];
                    locationToEdit.lng = latLng[1];

                    locationfromDb.lat = locationToEdit.lat;
                    locationfromDb.lng = locationToEdit.lng;
                    context.SaveChanges();
                }

                 
            }
            catch
            {
                return View();
            }

            return RedirectToAction("Index");
        }

      



    }
}
