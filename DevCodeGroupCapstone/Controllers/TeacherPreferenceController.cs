using DevCodeGroupCapstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace DevCodeGroupCapstone.Controllers
{
    public class TeacherPreferenceController : Controller
    {
        ApplicationDbContext context;
        public TeacherPreferenceController()
        {
            context = new ApplicationDbContext();
        }
        // GET: TeacherPreference
        public ActionResult Index()
        {
            return View();
        }

        // GET: TeacherPreference/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TeacherPreference/Create
        public ActionResult Create()
        {
            // if preferences exist send to edit
            string userId = User.Identity.GetUserId();
            Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
            int countOfPref = context.Preferences.Where(pref => pref.teacherId == teacher.PersonId).Count();
            if (countOfPref != 0)
            {
                return RedirectToAction("Edit"); // make Edit for Preferences
            }


            TeacherPreference preferences = new TeacherPreference();

            return View(preferences);
        }

        // POST: TeacherPreference/Create
        [HttpPost]
        public ActionResult Create(TeacherPreference preferences)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();

                preferences.teacherId = teacher.PersonId;
                context.Preferences.Add(preferences);
                context.SaveChanges();
    
                return RedirectToAction("Index", "Person");
            }
            catch
            {
                return View();
            }
        }

        // GET: TeacherPreference/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TeacherPreference/Edit/5
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

        // GET: TeacherPreference/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TeacherPreference/Delete/5
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
