using DevCodeGroupCapstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using DevCodeGroupCapstone.Models.View_Models;
using DevCodeGroupCapstone.Service_Classes;
using System.Threading.Tasks;

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
            var userFound = context.People.Where(p => p.PersonId == userId).Count();

            if (userFound == 0)
            {
                return RedirectToAction("Create");
            }
            
            // if not registered user go to creat
            // else
            // in index generate list of all teachers in the dB
            // iterate through people where subject != null (teachers)
            // display subject and availabiliy?
            // have button to sign up with teach/ create lesson

            var teachers = context.People.ToList(); // temporary for view test
            return View(teachers);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
                var person = context.People.Where(p => p.PersonId == info.person.PersonId).Single();
                string userId = User.Identity.GetUserId();
                person.ApplicationId = userId;

                context.People.Add(info.person);

                string[] latLng = await GeoCode.GetLatLongFromApi(info.location);
                info.location.lat = latLng[0];
                info.location.lat = latLng[1];

                context.Locations.Add(info.location);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
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
