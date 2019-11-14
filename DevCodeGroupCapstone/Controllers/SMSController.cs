using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Private;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using DevCodeGroupCapstone.Private;
using DevCodeGroupCapstone.Models;

namespace DevCodeGroupCapstone.Controllers
{
    public class SMSController : TwilioController
    {

        ApplicationDbContext context;
        public SMSController()
        {
            context = new ApplicationDbContext();
        }
      public ActionResult SendSMS(int id, string text, string RecieverType)
        {
            Person person = context.People.Where(p => p.PersonId == id).FirstOrDefault();
           
                var accountSid = ApiKey.twillioAccountSID;
                var authToken = ApiKey.twillioAuthToken;
                 TwilioClient.Init(accountSid, authToken);

                //var to = new PhoneNumber(ApiKey.myNum);
                var to = FormatNumber(person.phoneNumber);
                var from = new PhoneNumber(ApiKey.fromNum);
                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: text);
                return RedirectToAction("Index","Person"); // needs to return to the same view it came from??
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
            return RedirectToAction("Index", "Person"); // needs to return to the same view it came from??
        }

        public string FormatNumber(string phonenumber)
        {
            string result = "+1" + phonenumber.ToString();
            return result;
        }

        public bool SendMessage (PhoneNumber to, PhoneNumber from, string text)
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
            catch(Exception e)
            {
                return false;
            }
            
        }

        public StringBuilder DetermineAlertStudent(Person student, Person teacher, Lesson lesson, string RecieverType)
        {
           StringBuilder finalMessage = new StringBuilder();

            switch (RecieverType)
            {
                case "cancel":
                    finalMessage.Append("Hello "+student.firstName+"... Your ("+lesson.subject+") lesson with "+teacher.firstName+" "+teacher.lastName+" on "+lesson.start.Date+" at "+ lesson.start.TimeOfDay+" has been cancelled");
                        break;
                case "open":
                    finalMessage.Append("Hello "+student.firstName+"... There is an opening for a ("+lesson.subject+") lesson with "+teacher.firstName+" "+teacher.lastName+" on "+lesson.start.Date+" at "+ lesson.start.TimeOfDay+"... if you are interested in picking up this opening, log into your WeTeachToday account and schedule with the instructor!");
                    break;
            }

            return finalMessage;
        }



    }
}