using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningV1.Controllers
{
    public class CourseController : Controller
    {
        // GET: Course
        public ActionResult CourseList()
        {
            return View();
        }
    }
}