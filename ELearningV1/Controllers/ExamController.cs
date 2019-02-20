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
        DAL SQLcon = new DAL();

        [HttpGet]
        public ActionResult ExamList(string CourseID)
        {            
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

        public ActionResult saveQuestion(string Question, string QuestionType, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAns1, string CorrectAns2, string CorrectAns3, string CorrectAns4)
        {
            string EmployeeNumber = Session["EmployeeNumber"].ToString();
            if (Answer1 == "" || Answer1 == null)
            {

            }
            else if (Answer2 == "" || Answer2 == null)
            {

            }
            else if (Answer3 == "" || Answer3 == null)
            {

            }
            else if (Answer4 == "" || Answer4 == null)
            {

            }

            string result = SQLcon.SaveExamAndQuestion(Question, Answer1, Answer2, Answer3, Answer4, CorrectAns1, CorrectAns2, CorrectAns3, CorrectAns4, EmployeeNumber);

            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }
    }
}