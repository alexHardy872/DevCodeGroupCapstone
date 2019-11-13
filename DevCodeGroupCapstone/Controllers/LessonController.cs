﻿using DevCodeGroupCapstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;//tlc
using System.Web;
using System.Web.Mvc;

namespace DevCodeGroupCapstone.Controllers
{
    public class LessonController : Controller
    {
        ApplicationDbContext context;
        string[] lessonLocation;

        public LessonController()
        {
            context = new ApplicationDbContext();
            lessonLocation = new string[3] { "In-Studio", "In-Home", "Online" };
        }
        // GET: Lesson
        public ActionResult List()
        {
            var id = User.Identity.GetUserId();
            var user = context.People.FirstOrDefault(p => p.ApplicationId == id);
            List<Lesson> lessons = context.Lessons.Where(l => l.teacherId == user.PersonId).ToList();
            return View(lessons);
        }

        // GET: Lesson/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Lesson/Create
        public ActionResult Create()
        {
            ViewBag.LessonType = new SelectList(lessonLocation);
            Lesson lesson = new Lesson();
            return View(lesson);
        }

        // POST: Lesson/Create
        [HttpPost]
        public async Task<ActionResult> Create(Lesson lesson) //public ActionResult Create(Lesson lesson)
        {
            try
            {
                var lessonType = new SelectList(new[]
{
                    new {value = 1, text = "In-Studio"},
                    new {value = 2, text = "In-Home"},
                    new {value = 3, text = "Online"}
                });
                ViewBag.LessonType = lessonType;
                string id = User.Identity.GetUserId();
                Person user = context.People.FirstOrDefault(u => u.ApplicationId == id);
                lesson.teacherId = user.PersonId;
                if (lesson.LessonType == "In-Studio" || lesson.LessonType == "Online")
                {
                    var person = context.People.FirstOrDefault(p => p.ApplicationId == id);
                    lesson.LocationId = person.LocationId;
                    decimal cost = lesson.Price / 60 * lesson.Length;
                    cost = Math.Round(cost, 2);
                    lesson.cost = cost;
                }
                else //tlc "In-Home"
                {
                    //calculate travel duration
                    if (lesson.teacherId != null && lesson.studentId != null && lesson.LocationId != null)
                    {
                        var teacher = context.People.Include("Location").Where(t => t.PersonId == lesson.teacherId).SingleOrDefault();
                        var location = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

                        lesson.travelDuration = await Service_Classes.DistanceMatrix.GetTravelDuration(teacher.Location.lat, teacher.Location.lng, location.lat, location.lng);
                        
                        //TLC CALL ALERT ROUTINE FROM HERE IF DURATION > PREFERENCE
                    }


                }
                context.Lessons.Add(lesson);
                context.SaveChanges();
                return RedirectToAction("List");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View();
            }
        }

        // GET: Lesson/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.LessonType = new SelectList(lessonLocation);
            Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
            return View(lesson);
        }

        // POST: Lesson/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Lesson lesson)//tlc made async
        {
            try
            {
                var lessonType = new SelectList(new[]
{
                    new {value = 1, text = "In-Studio"},
                    new {value = 2, text = "In-Home"},
                    new {value = 3, text = "Online"}
                });
                ViewBag.LessonType = lessonType;
                Lesson lessonFromDb = context.Lessons.FirstOrDefault(l => l.LessonId == id);
                lessonFromDb.subject = lesson.subject;
                lessonFromDb.Length = lesson.Length;
                lessonFromDb.Price = lesson.Price;
                lessonFromDb.LessonType = lesson.LessonType;
                if (lesson.LessonType == "In-Studio" || lesson.LessonType == "Online")
                {
                    var userId = User.Identity.GetUserId();
                    var user = context.People.FirstOrDefault(p => p.ApplicationId == userId);
                    decimal cost = lesson.Price / 60 * lesson.Length;
                    cost = Math.Round(cost, 2);
                    lessonFromDb.LocationId = user.LocationId;
                    lessonFromDb.cost = cost;                    

                    //return RedirectToAction("List");
                }
                else //"In-Home"
                {
                    //lessonFromDb.LocationId = null;
                    //lessonFromDb.cost = 0;
                    if (lessonFromDb.travelDuration < 1)
                    {
                        var teacher = context.People.Include("Location").Where(p => p.PersonId == lessonFromDb.teacherId).SingleOrDefault();
                        var tempLessonLocation = context.Locations.Where(l => l.LocationId == lessonFromDb.LocationId).SingleOrDefault();

                        lessonFromDb.travelDuration = await Service_Classes.DistanceMatrix.GetTravelDuration(teacher.Location.lat, teacher.Location.lng, tempLessonLocation.lat, tempLessonLocation.lng);

                        //TLC CALL ALERT ROUTINE FROM HERE IF DURATION > PREFERENCE
                    }

                    //return RedirectToAction("List");
                }   
            }
            catch
            {
                //return View();
                return RedirectToAction("List");
            }

            context.SaveChanges();
            return RedirectToAction("Details", "Lesson", new { id = id });//tlc
        }

        // GET: Lesson/Delete/5
        public ActionResult Delete(int id)
        {
            Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
            return View(lesson);
        }

        // POST: Lesson/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Lesson lesson)
        {
            try
            {
                lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
                context.Lessons.Remove(lesson);
                context.SaveChanges();
                return RedirectToAction("List");
            }
            catch
            {
                return View();
            }
        }
    }
}
