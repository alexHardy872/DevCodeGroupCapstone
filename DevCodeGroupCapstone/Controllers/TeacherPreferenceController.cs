﻿using DevCodeGroupCapstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

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
            TeacherPreference tempPreference = null;

            try
            {
                tempPreference = context.Preferences.Where(p => p.teacherId == id).SingleOrDefault();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return View(tempPreference);
            }
            return View();
        }

        // GET: TeacherPreference/Create
        public ActionResult Create()
        {
            // if preferences exist send to edit
            string userId = User.Identity.GetUserId();
            //tlc Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
            Person teacher = context.People.Include("Location").Where(p => p.ApplicationId == userId).Single(); //tlc
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
        public async Task<ActionResult> Create(TeacherPreference preferences)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();

                preferences.teacherId = teacher.PersonId;

                context.Preferences.Add(preferences);
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "Person");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: TeacherPreference/Edit/5
        public ActionResult Edit()
        {
            string userId = User.Identity.GetUserId();
            Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
            TeacherPreference preference = context.Preferences.Where(pref => pref.teacherId == teacher.PersonId).Single();
            return View(preference);
        }

        // POST: TeacherPreference/Edit/5
        [HttpPost]
        public ActionResult Edit(TeacherPreference preference)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Person teacher = context.People.Where(p => p.ApplicationId == userId).Single();
                TeacherPreference preferenceDb = context.Preferences.Where(pref => pref.teacherId == teacher.PersonId).Single();

                preferenceDb.PerHourRate = preference.PerHourRate;
                preferenceDb.maxDistance = preference.maxDistance;
                preferenceDb.incrementalCost = preference.incrementalCost;
                preferenceDb.distanceType = preference.distanceType;
                preferenceDb.defaultLessonLength = preference.defaultLessonLength;


                context.SaveChanges();
                return RedirectToAction("Index", "Person");
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
