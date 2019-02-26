using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;
using KioskVersion3.Models.ViewModel;

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
            var result = "";
            var user = Session["EmployeeNumber"].ToString();
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.SaveTestName(Title, user);
            }
            catch (Exception ex) {
                result = "false";
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public JsonResult LoadSectionData(DataTablesParam param, string courseID)
        {
            List<VMCourseSection> sectionData = new List<VMCourseSection>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;
          
            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

          
                totalCount = SQLcon.ViewCourseSectionByID(courseID).Count();
                sectionData = SQLcon.ViewCourseSectionByID(courseID).OrderBy(x => x.OrderSec).Select(x => new VMCourseSection
                {
                    ID = x.ID,
                    Title = x.Title,
                    Type = x.Type,
                    SrcFile = x.SrcFile,
                    CourseID = x.CourseID,
                    OrderSec = x.OrderSec
                }).AsEnumerable().ToList();
            
            return Json(new
            {
                aaData = sectionData,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = sectionData.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSectionOrderingByID(string SID, string OrderVal)
        {
            var result = false;
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.UpdateSectionOrdering(SID,OrderVal);
            }
            catch (Exception ex)
            {
                result = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult DeleteSectionByID(string SID)
        {
            var result = false;
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.DeleteSectionById(SID);
            }
            catch (Exception ex)
            {
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

            string result = SQLcon.SaveExamAndQuestion(Question, Answer1, Answer2, Answer3, Answer4, CorrectAns1, EmployeeNumber);

            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }
    }
}