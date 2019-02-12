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
            //Nice ONE BROTHER 2334
            ViewBag.Title = "Home Page Test12";

            return View();
        }

        public ActionResult ViewCourse()
        {
           
            return View();
        }
    }
}
