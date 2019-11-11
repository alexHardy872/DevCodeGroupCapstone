using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevCodeGroupCapstone.Controllers
{

    [RequireHttps]
    public class HomeController : Controller
    {

        public ActionResult Index()
        {

            // does applicaiton Id exist in customer table?
            // if it does they go to perspn controller home
            // if not they go to create person

           

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}