using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Models.View_Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;//tlc
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
            ViewBag.outOfRange = false;
            var lesson = context.Lessons.Where(l => l.LessonId == id).SingleOrDefault();

            SetLessonDetailsMapCoordinates(lesson);

            if (lesson != null && lesson.LessonType == "In-Home")
            {
                ViewBag.outOfRange = TravelDurationIsGreaterThanMaxDistance(lesson);
            }

            return View(lesson);
        }

        // GET: Lesson/Create
        public ActionResult Create(int teacherId)
        {
            ViewBag.LessonType = new SelectList(lessonLocation);
            Lesson lesson = new Lesson();
            lesson.start = DateTime.Now;
            lesson.end = DateTime.Now;
            lesson.teacherId = teacherId;
            return View(lesson);
        }

        public ActionResult CreateMakeup()
        {
            string userId = User.Identity.GetUserId();
            Person teacher = context.People.Where(t => t.ApplicationId == userId).FirstOrDefault();
            List<Person> allStudents = context.People.Where(p => p.PersonId != teacher.PersonId).ToList();

            LessonAndStudentsViewModel group = new LessonAndStudentsViewModel();
            Lesson makeup = new Lesson();
            makeup.requiresMakeup = true;
            makeup.teacherId = teacher.PersonId;

            group.lesson = makeup;
            group.students = allStudents;

            return View(group);
        }
        [HttpPost]
        public async Task<ActionResult> CreateMakeup(LessonAndStudentsViewModel group)
        {
            try
            {
                string userId = User.Identity.GetUserId();
                Person teacher = context.People.Where(t => t.ApplicationId == userId).FirstOrDefault();
                Lesson makeup = group.lesson;
                makeup.teacherId = teacher.PersonId;
                makeup.requiresMakeup = true;
                makeup.start = DateTime.Now;
                makeup.end = DateTime.Now;
               
               // Person student = context.People.Where(p => p.PersonId == makeup.studentId).FirstOrDefault();
               // makeup.studentId = student.PersonId;
                context.Lessons.Add(makeup);
                await context.SaveChangesAsync();
                return RedirectToAction("TeacherIndex", "Person");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return View();
            }   
        }

        // POST: Lesson/Create
        [HttpPost]
        public async Task<ActionResult> Create(Lesson lesson) //public ActionResult Create(Lesson lesson)
        {
            try
            {
                int? teacherId = lesson.teacherId;
                var teacher = context.People.Where(t => t.PersonId == teacherId).FirstOrDefault();
                var lessonType = new SelectList(new[]
{
                    new {value = 1, text = "In-Studio"},
                    new {value = 2, text = "In-Home"},
                    new {value = 3, text = "Online"}
                });
                ViewBag.LessonType = lessonType;
                string id = User.Identity.GetUserId();
                Person currentUser = context.People.FirstOrDefault(u => u.ApplicationId == id);
                lesson.teacherId = teacherId;
                lesson.studentId = currentUser.PersonId;
                var preferences = context.Preferences.FirstOrDefault(p => p.teacherId == teacherId);
                lesson.Price = preferences.PerHourRate;
                lesson.Length = preferences.defaultLessonLength;
                lesson.subject = teacher.subjects;
                if (lesson.LessonType == "In-Studio" || lesson.LessonType == "Online")
                {
                    //var person = context.People.FirstOrDefault(p => p.ApplicationId == id);
                    lesson.LocationId = teacher.LocationId;
                    decimal cost = lesson.Price / 60 * lesson.Length;
                    cost = Math.Round(cost, 2);
                    lesson.cost = cost;
                }
                else //tlc "In-Home"
                {
                    //calculate travel duration
                    if (lesson.teacherId != null && lesson.studentId != null && lesson.LocationId != null)
                    {
                        //var teacher = context.People.Include("Location").Where(t => t.PersonId == lesson.teacherId).SingleOrDefault();
                        var location = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

                        lesson = await Service_Classes.DistanceMatrix.GetTravelInfo(lesson);

                        try
                        {
                            ViewBag.outOfRange = TravelDurationIsGreaterThanMaxDistance(lesson);
                            ViewBag.lessonLat = lesson.Location.lat;
                            ViewBag.lessonLng = lesson.Location.lat;
                        }
                        catch (Exception e)
                        {
                            ViewBag.outOfRange = false;
                            Console.WriteLine(e.Message);
                        }
                    }
                }
                context.Lessons.Add(lesson);
                context.SaveChanges();

                return RedirectToAction("AlertRequest", "SMS", new { id = lesson.LessonId });
                //return RedirectToAction("StudentIndex", "Person");
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
            //ViewBag.LessonType = new SelectList(lessonLocation);
            Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
            return View(lesson);
        }

        // POST: Lesson/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Lesson lesson)//tlc made async
        {
            try
            {

                Lesson lessonFromDb = context.Lessons.FirstOrDefault(l => l.LessonId == id);
          
                lessonFromDb.teacherApproval = lesson.teacherApproval;
                if (lessonFromDb.LessonType == "In-Studio" || lessonFromDb.LessonType == "Online")
                {
                    var userId = User.Identity.GetUserId();
                    var user = context.People.FirstOrDefault(p => p.ApplicationId == userId);
                    decimal cost = lesson.Price / 60 * lesson.Length;
                    cost = Math.Round(cost, 2);
             
                    lesson.LocationId = user.LocationId;
                   
                    lesson.cost = cost;
                    lesson.travelDuration = 0;//tlc
                    
                }
                else //"In-Home"
                {
                    //lessonFromDb.LocationId = null;
                    //lessonFromDb.cost = 0;
                    if (lesson.travelDuration < 1)
                    {
                        var tempTeacher = context.Preferences.Where(p => p.teacherId == lessonFromDb.teacherId).SingleOrDefault();
                        if (tempTeacher != null)
                        {
                            lessonFromDb = await Service_Classes.DistanceMatrix.GetTravelInfo(lessonFromDb);
                        }
                    }
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

        public ActionResult Delete(int id)
        {
            Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
            return View(lesson);
        }

        // GET: Lesson/Delete/5
        public async Task<ActionResult> Approve(int id)
        {
            Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
            lesson.teacherApproval = true;
            await context.SaveChangesAsync();
            return RedirectToAction("TeacherIndex", "Person");
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

        
        public ActionResult QuickDelete(int id)
        {
            try
            {
                Lesson lesson = context.Lessons.FirstOrDefault(l => l.LessonId == id);
                context.Lessons.Remove(lesson);
                context.SaveChanges();
                return RedirectToAction("TeacherIndex","Person");
            }
            catch
            {
                return View();
            }
        }

        public Boolean TravelDurationIsGreaterThanMaxDistance(Lesson lesson) //tlc
        {
            bool result = false;
            var teacherPreference = context.Preferences.Where(p => p.teacherId == lesson.teacherId).SingleOrDefault();
            var location = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

            if (teacherPreference != null && location != null && teacherPreference.distanceType == RadiusOptions.Miles)
            {
                result = (lesson.travelDuration > teacherPreference.maxDistance);
            }

            return result;
        }

        public void SetLessonDetailsMapCoordinates(Lesson lesson)
        {
            var teacher = context.People.Include("Location").Where(p => p.PersonId == lesson.teacherId).SingleOrDefault();
            var teacherPreference = context.Preferences.Where(p => p.teacherId == teacher.PersonId).FirstOrDefault();
            var lessonLocation = context.Locations.Where(l => l.LocationId == lesson.LocationId).SingleOrDefault();

            ViewBag.lessonLat = lessonLocation.lat;
            ViewBag.lessonLng = lessonLocation.lng;
            ViewBag.teacherLat = teacher.Location.lat;
            ViewBag.teacherLng = teacher.Location.lng;

            try
            {
                if (teacherPreference.distanceType == RadiusOptions.Miles)
                {
                    ViewBag.radius = teacherPreference.maxDistance * Service_Classes.DistanceMatrix.metersToMiles;
                }
                else
                {
                    ViewBag.radius = teacherPreference.maxDistance;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                ViewBag.radius = 0;
            }
        }
    }
}
