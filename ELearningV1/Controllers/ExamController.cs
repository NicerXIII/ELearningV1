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

        public ActionResult SaveVideoData(CourseImage model, string CID, string Title) {

            var result = false;
            var file = model.ImageFile;
            if (file != null)
            {
                if (!System.IO.File.Exists(Server.MapPath("/UploadedFiles/" + file.FileName)))
                {
                    file.SaveAs(Server.MapPath("/UploadedFiles/" + file.FileName));
                    try
                    {
                        DAL SQLcon = new DAL();
                        result = SQLcon.UploadNewFile(Title,"Video", "/UploadedFiles/" + file.FileName,CID);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    result = false;
                }
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                vName = "/UploadedFiles/" + file.FileName
            };
            return response;
        }

        public ActionResult SavePPTData(CourseImage model, string CID, string Title)
        {

            var result = false;
            var file = model.ImageFile;
            if (file != null)
            {
                if (!System.IO.File.Exists(Server.MapPath("/UploadedFiles/" + file.FileName)))
                {
                    file.SaveAs(Server.MapPath("/UploadedFiles/" + file.FileName));
                    try
                    {
                        DAL SQLcon = new DAL();
                        result = SQLcon.UploadNewFile(Title, "PPT", "/UploadedFiles/" + file.FileName, CID);
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    result = false;
                }
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                pName = "/UploadedFiles/" + file.FileName
            };
            return response;
        }

        public ActionResult SaveTestTitle(string CID, string Title)
        {
            var result = false;
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.UploadNewFile(Title, "Test", "", CID);
            }
            catch (Exception ex) {
                result = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
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