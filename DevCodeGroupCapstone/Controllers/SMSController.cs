using DevCodeGroupCapstone.Models;
using DevCodeGroupCapstone.Private;
using System.Web.Mvc;
using Twilio;
using Twilio.AspNet.Mvc;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace DevCodeGroupCapstone.Controllers
{
    public class SMSController : TwilioController
    {
        public ActionResult SendSMS(Person person, string text)
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
                body: text);

            return Content(message.Body); // needs to return to the same view it came from??

        }

        public string FormatNumber(string phonenumber)
        {
            return "+1" + phonenumber;
        }



    }
}