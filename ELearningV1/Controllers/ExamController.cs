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
        string CourseSectionID = "";
        string Section_OrderNo = "";

        #region Done by boss Tiqius
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

        public ActionResult SaveVideoData(CourseImage model, string CID, string Title)
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
                        result = SQLcon.UploadNewFile(Title, "Video", "/UploadedFiles/" + file.FileName, CID);
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
                        result = SQLcon.UploadNewFile(Title, "PDF", "/UploadedFiles/" + file.FileName, CID);
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

        public JsonResult LoadSectionData(DataTablesParam param, string courseID)
        {
            List<VMCourseSection> sectionData = new List<VMCourseSection>();
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
                result = SQLcon.UpdateSectionOrdering(SID, OrderVal);
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
        #endregion

        #region Creating Test Name and Question
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

        public ActionResult SaveTestTitle(string CID, string Title)
        {
            var result = "";
            var user = Session["EmployeeNumber"].ToString();
            try
            {
                DAL SQLcon = new DAL();
                result = SQLcon.SaveTestName(Title, "Test", CID);
            }
            catch (Exception ex)
            {
                result = "false";
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult saveQuestion(string Question, string QuestionType, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAns1, string CourseSecID, string CourseID)
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

            string result = SQLcon.SaveExamAndQuestion(Question, QuestionType, Answer1, Answer2, Answer3, Answer4, CorrectAns1, EmployeeNumber ,CourseSecID , CourseID);

            var response = new JsonResult();
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult deleteQuestion(string ID)
        {
            var result = "";
            try
            {   result = SQLcon.DeleteQuestion(ID); }
            catch (Exception ex)
            {   result = "false";   }

            var response = new JsonResult();
            response.Data = new
            {   res = result    };
            return response;
        }

        public ActionResult updateQuestion(string ID, string Question, string C1, string C2, string C3, string C4, string CorAns)
        {
            var result = "";
            try
            { result = SQLcon.UpdateQuestion(ID, Question, C1, C2, C3, C4, CorAns); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateQuestionOrderNumber(string ID, string OrderNo)
        {
            var result = "";
            try
            { result = SQLcon.UpdateQuestionOrderNo(ID, OrderNo); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateTestName(string ID, string TestName)
        {
            var result = "";
            try
            { result = SQLcon.UpdateTestName(ID, TestName); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }
        #endregion

        public JsonResult getQuestionList(DataTablesParam param, string CourSecID)
        {
            List<VMGetExamQuestions> QuestList = new List<VMGetExamQuestions>();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getQuestionList(CourSecID).Count();
            QuestList = SQLcon.getQuestionList(CourSecID).OrderBy(x => x.OrderNumber).Select(x => new VMGetExamQuestions
            {
                ID = x.ID,
                Question = x.Question,
                QuestionType = x.QuestionType,
                C1 = x.C1,
                C2 = x.C2,
                C3 = x.C3,
                C4 = x.C4,
                CAnswer = x.CAnswer,
                CourseSectionID = x.CourseSectionID,
                CourseID = x.CourseID,
                OrderNumber = x.OrderNumber,
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = QuestList,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = QuestList.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        #region Exam
        public ActionResult GetFirstDataToLoad(string CourseID)
        { 
            var loadData = SQLcon.getDataToLoad(CourseID).OrderBy(x => x.OrderSec).Select(x => x.Type).FirstOrDefault().ToString(); 
            CourseSectionID = SQLcon.getDataToLoad(CourseID).OrderBy(x => x.CourSecID).Select(x => x.CourSecID).FirstOrDefault().ToString();
            var response = new JsonResult();
            response.Data = new
            {   res = loadData, };
            return response;
        }

        public JsonResult loadVideo(string CourseID)
        {
            var vidPath = SQLcon.getVideoPath(CourseID).Select(x => x.VideoPath).FirstOrDefault().ToString();

            var responseVideo = new JsonResult();
            responseVideo.Data = new
            {   resVideo = vidPath, };
            return responseVideo;
        }

        public JsonResult loadPDF(string CourseID)
        {
            var PDFpath = SQLcon.getPDFPath(CourseID).Select(x => x.PDFPath).FirstOrDefault().ToString();

            var responsePDF = new JsonResult();
            responsePDF.Data = new
            {   resPDF = PDFpath,  };
            return responsePDF;
        }
        #endregion
    }
}