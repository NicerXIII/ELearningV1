using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;

namespace ELearningV1.Controllers
{
    public class ExamController : Controller
    {
        [HttpGet]
        public ActionResult ExamList(string CourseID)
        {
            DAL SQLcon = new DAL();
            VMViewCourses courseData = new VMViewCourses();
            try
            {
                courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault();
            }
            catch (Exception ex) { }

            return View(courseData);
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