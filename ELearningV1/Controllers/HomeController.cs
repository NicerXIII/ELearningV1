using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningV1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //This is just a test
            ViewBag.Title = "Home Page Test12";

            return View();
        }
    }
}
