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

        public ActionResult saveQuestion(string Question, string QuestionType, string Answer1, string Answer2, string Answer3, string Answer4)
        {


            var response = new JsonResult();
            return response;
        }
    }
}