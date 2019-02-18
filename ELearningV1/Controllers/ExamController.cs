using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ELearningV1.Controllers
{
    public class ExamController : Controller
    {
        public ActionResult ExamList()
        {
            return View();
        }

        // GET: Exam
        public ActionResult Exam()
        {
            return View();
        }

        public ActionResult CreateExam()
        {
            return View();
        }

        public ActionResult ExamQuestionList()
        {
            return View();
        }
    }
}