using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using Twilio.TwiML;
using Twilio.AspNet.Mvc;
using DevCodeGroupCapstone.Private;
using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Models.View_Models;

namespace DevCodeGroupCapstone.Controllers
{
    public class SMSController : TwilioController
    {

        ApplicationDbContext context;
        public SMSController()
        {
            context = new ApplicationDbContext();
        }
      public ActionResult SendSMS()
        {

            List<Person> group = new List<Person>();

            group = context.People.ToList();


            foreach ( Person person in group)
            {
                var accountSid = ApiKey.twillioAccountSID;
                var authToken = ApiKey.twillioAuthToken;
                TwilioClient.Init(accountSid, authToken);

                //var to = new PhoneNumber(ApiKey.myNum);
                var to = FormatNumber(person.phoneNumber);
                var from = new PhoneNumber(ApiKey.fromNum);

                var message = MessageResource.Create(
                    to: to,
                    from: from,
                    body: "test group?");
            }
           

                return RedirectToAction("Index","Person"); // needs to return to the same view it came from??

        }

        public string FormatNumber(string phonenumber)
        {
            string result = "+1" + phonenumber.ToString();
            return result;
        }



    }
}