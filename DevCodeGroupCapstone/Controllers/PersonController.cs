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
            var userFound = context.People.Where(p => p.ApplicationId == userId).Count();

            if (userFound == 0)
            {
                return RedirectToAction("Create");
            }
            List<PersonAndLocationViewModel> teachers = new List<PersonAndLocationViewModel>();

            List<Person> eligibleTeachers = context.People.Where(s => s.subjects != null).ToList();

            foreach (Person teacher in eligibleTeachers)
            {
                PersonAndLocationViewModel info = new PersonAndLocationViewModel();
                info.person = teacher;
                info.location = context.Locations.Where(l => l.LocationId == teacher.LocationId).Single();
                info.lessons = context.Lessons.Where(lesson => lesson.teacherId == teacher.PersonId).ToList();
                info.avails = context.TeacherAvailabilities.Where(av => av.PersonId == teacher.PersonId).ToList();

                teachers.Add(info);
            }

            if (teachers == null)
            {
                return RedirectToAction("Index");
            }

<<<<<<< HEAD
            
=======
>>>>>>> 6208d4cc3333659bcbb89008b6a66e2a50f0c33d
            return View(teachers);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            double metersToMiles = 1609.34;
            PersonAndLocationViewModel personLocationDetails = new PersonAndLocationViewModel();
            personLocationDetails.person = context.People.Include("Location").Where(p => p.PersonId == id).Single();
            personLocationDetails.location = context.Locations.Where(l => l.LocationId == personLocationDetails.person.LocationId).Single();

            var tempTeacher = context.Preferences.Where(p => p.teacherId == personLocationDetails.person.PersonId).SingleOrDefault();//tlc
            if (tempTeacher != null)
            {
                double teacherPreferenceRadius = tempTeacher.maxDistance * metersToMiles;//tlc
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
            return View();
        }

        // POST: Person/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
