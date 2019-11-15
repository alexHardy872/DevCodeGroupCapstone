using DevCodeGroupCapstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DevCodeGroupCapstone.Controllers
{
    public class TeacherAvailabilityController : Controller
    {
        ApplicationDbContext context;

        public TeacherAvailabilityController()
        {
            context = new ApplicationDbContext();
        }
        // GET: TeacherAvailability
        public ActionResult Index()
        {

            return RedirectToAction("List");
        }

        // GET: TeacherAvailability/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeacherAvailability/Create
        public ActionResult Create()
        {
            TeacherAvail avail = new TeacherAvail();
            return View(avail);
        }

        // POST: TeacherAvailability/Create
        [HttpPost]
        public ActionResult Create(TeacherAvail avail)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
                avail.PersonId = teacher.PersonId;
                context.TeacherAvailabilities.Add(avail);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // GET: TeacherAvailability/Create
        public ActionResult List()
        {

            string userId = User.Identity.GetUserId();
            Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
            List<TeacherAvail> avails = new List<TeacherAvail>();
            avails = context.TeacherAvailabilities.Where(a => a.PersonId == teacher.PersonId).ToList();

            return View(avails);
        }



        // GET: TeacherAvailability/Edit/5
        public ActionResult Edit(int id)
        {
            TeacherAvail avail = context.TeacherAvailabilities.Where(a => a.availId == id).Single();
            return View(avail);
        }

        // POST: TeacherAvailability/Edit/5
        [HttpPost]
        public ActionResult Edit(TeacherAvail editedAvail)
        {
            TeacherAvail availFromDb = null;
            try
            {
                availFromDb = context.TeacherAvailabilities.Where(ta => ta.availId == editedAvail.availId).SingleOrDefault();

                availFromDb.start = editedAvail.start;
                availFromDb.end = editedAvail.end;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception e)
            {
                //return View();
                Console.WriteLine(e.Message);
                return RedirectToAction("Index");
            }
        }

        // GET: TeacherAvailability/Delete/5
        public ActionResult Delete(int id)
        {
            TeacherAvail availToRemove = context.TeacherAvailabilities.Where(a => a.availId == id).FirstOrDefault();
            return View(availToRemove);
        }

        // POST: TeacherAvailability/Delete/5
        [HttpPost]
        public ActionResult Delete(TeacherAvail avail)
        {
            try
            {
                var availDb = context.TeacherAvailabilities.Where(a => a.availId == avail.availId).FirstOrDefault();
                context.TeacherAvailabilities.Remove(availDb);

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }
    }
}
