using DevCodeGroupCapstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

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
            return View();
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

        // GET: TeacherAvailability/Edit/5
        public ActionResult Edit(int id)
        {
            TeacherAvail avail = context.TeacherAvailabilities.Where(a => a.availId == id).Single();
            return View(avail);
        }

        // POST: TeacherAvailability/Edit/5
        [HttpPost]
        public ActionResult Edit(TeacherAvail avail)
        {
            try
            {



                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherAvailability/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeacherAvailability/Delete/5
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
