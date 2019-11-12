using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using DevCodeGroupCapstone.Models;

namespace DevCodeGroupCapstone.Controllers
{
    public class ScheduleApiController : ApiController
    {
        public ApplicationDbContext context;

        public ScheduleApiController()
        {
            context = ApplicationDbContext.Create();
        }

        public async Task<IHttpActionResult> Index(string Id)
        {
            try
            {
                // todo: Index grab of events for Calendar
                var person = await Task.Run(() => context.People
                    .Include("avail")
                    .Where(p => p.ApplicationId == Id)
                    .SingleOrDefault()
                    );

                TeacherAvail teacherAvail = new TeacherAvail();

              


            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }


            return Ok();
;        }


    }
}
