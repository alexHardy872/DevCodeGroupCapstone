using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Private;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using DevCodeGroupCapstone.Private;
using DevCodeGroupCapstone.Models;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Collections.Generic;

namespace DevCodeGroupCapstone.Controllers
{
    public class SMSController : TwilioController
    {

        ApplicationDbContext context;
        public SMSController()
        {
            context = new ApplicationDbContext();
        }



        public async Task<ActionResult> SendSMSToTeacher(int id) // alert is cancel or opening
        {
            Lesson lesson = context.Lessons.Where(les => les.LessonId == id).FirstOrDefault();
            Person student = context.People.Where(stu => stu.PersonId == lesson.studentId).FirstOrDefault();
            Person teacher = context.People.Where(tea => tea.PersonId == lesson.teacherId).FirstOrDefault();
            string textM = DetermineAlertTeacher(student, teacher, lesson).ToString();
            var to = FormatNumber(teacher.phoneNumber);
            var from = new PhoneNumber(ApiKey.fromNum);
            bool success = await Task.Run(() => SendMessage(to, from, textM));
            lesson.studentId = null;
            await context.SaveChangesAsync();

            // send to teachrs others students 
            
            List<Lesson> remainingStudentsFromLessons = context.Lessons
                        .Include("Student")
                        .Where(less => less.teacherId == teacher.PersonId && less.studentId != lesson.studentId && lesson.requiresMakeup == true).ToList();
            
            foreach(Lesson item in remainingStudentsFromLessons)
            {
                // send text to each student with new message
                string openingText = BuildOpeningMessage(item.Student, teacher, lesson);
                PhoneNumber studNum = FormatNumber(item.Student.phoneNumber);
                SendMessage(studNum, from, openingText);
            }

            
            return RedirectToAction("TeacherIndex", "Person"); // needs to return to the same view it came from??
        }

        public string BuildOpeningMessage(Person student, Person teacher, Lesson lesson)
        {
            StringBuilder message = new StringBuilder();
            message.Append(teacher.firstName+" "+teacher.lastName+"'s schedule has a new opening for a lesson ");
            message.Append("on " + lesson.start.Date + " at " + lesson.start.TimeOfDay + ".");
            message.Append("Log into your account to claim this lesson!");
            return message.ToString();
        }


         


        public async Task<ActionResult> SendSMSToStudent(int id, string alert) // alert is cancel or opening
        {
            Lesson lesson = context.Lessons.Where(les => les.LessonId == id).FirstOrDefault();
            Person student = context.People.Where(stu => stu.PersonId == lesson.studentId).FirstOrDefault();
            Person teacher = context.People.Where(tea => tea.PersonId == lesson.teacherId).FirstOrDefault();
            string textM = DetermineAlertStudent(student, teacher, lesson, alert).ToString();
            var to = FormatNumber(student.phoneNumber);
            var from = new PhoneNumber(ApiKey.fromNum);
            bool success = await Task.Run( () => SendMessage(to, from, textM));
            lesson.requiresMakeup = true;
            await context.SaveChangesAsync();
            return RedirectToAction("StudentIndex", "Person"); // needs to return to the same view it came from??
        }

        public string FormatNumber(string phonenumber)
        {
            string result = "+1" + phonenumber.ToString();
            return result;
        }

        public bool SendMessage(PhoneNumber to, PhoneNumber from, string text)
        {
            var accountSid = ApiKey.twillioAccountSID;
            var authToken = ApiKey.twillioAuthToken;
            TwilioClient.Init(accountSid, authToken);

            try
            {
                var message = MessageResource.Create(
                                to: to,
                                from: from,
                                body: text);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

        }

        public StringBuilder DetermineAlertStudent(Person student, Person teacher, Lesson lesson, string RecieverType)
        {
            StringBuilder finalMessage = new StringBuilder();
            switch (RecieverType)
            {
                case "cancel":
                    finalMessage.Append("Hello " + student.firstName + "... Your (" + lesson.subject + ") lesson with " + teacher.firstName + " " + teacher.lastName + " on " + lesson.start.Date + " at " + lesson.start.TimeOfDay + " has been cancelled");
                    break;
                case "open":
                    finalMessage.Append("Hello " + student.firstName + "... There is an opening for a (" + lesson.subject + ") lesson with " + teacher.firstName + " " + teacher.lastName + " on " + lesson.start.Date + " at " + lesson.start.TimeOfDay + "... if you are interested in picking up this opening, log into your WeTeachToday account and schedule with the instructor!");
                    break;
            }
            return finalMessage;
        }

        public StringBuilder DetermineAlertTeacher(Person student, Person teacher, Lesson lesson)
        {
            StringBuilder finalMessage = new StringBuilder();
            finalMessage.Append("Hello " + teacher.firstName + "... Your student " + student.firstName + " " + student.lastName + " has canceled their " + lesson.subject + " lesson schduled for " + lesson.start.Date + " at " + lesson.start.TimeOfDay + "... log on to your WeTeachToday account to alert available students about the opening");
            return finalMessage;
        }



    }
}