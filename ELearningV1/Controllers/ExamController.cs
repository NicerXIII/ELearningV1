using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;
using KioskVersion3.Models.ViewModel;
//
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ELearningV1.Controllers
{
    public class ExamController : Controller
    {
        DAL SQLcon = new DAL();

        string QuestOrder = "";

        #region Done by boss Tiqius
        [HttpGet]
        public ActionResult ExamList(string CourseID)
        {
            VMViewCourses courseData = new VMViewCourses();
            try
            { courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault(); }
            catch (Exception ex) { }

            return View(courseData);
        }

        public ActionResult SaveVideoData(CourseImage model, string CID, string Title,string Type1)
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
                        if (Type1 == "Vid")
                        {
                            result = SQLcon.UploadNewFile(Title, "Video", "/UploadedFiles/" + file.FileName, CID);
                        }
                        else {
                            result = SQLcon.UploadNewFile(Title, "VideoExam", "/UploadedFiles/" + file.FileName, CID);
                        }
                       
                    }
                    catch (Exception ex) { }
                }
                else
                { result = false; }
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
                { result = false; }
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
            { result = false; }
            var response = new JsonResult();
            response.Data = new
            { res = result };
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
            { result = false; }
            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }
        #endregion

        #region Creating Test Name and Question
        public ActionResult CreateExam()
        { return View(); }

        public ActionResult ExamQuestionList()
        { return View(); }

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
            { result = "false"; }
            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult saveQuestion(string Question, string QuestionType, string Answer1, string Answer2, string Answer3, string Answer4, string CorrectAns, string CourseSecID, string CourseID)
        {
            string EmployeeNumber = Session["EmployeeNumber"].ToString();
            string result = "";

            #region Condition
            if (Question.Contains("'"))
            {   Question = Question.Replace("'","\\'");  }
            /**
            if (Answer1 == "" || Answer1 == null)
            { }
            else if (Answer2 == "" || Answer2 == null)
            { }
            else if (Answer3 == "" || Answer3 == null)
            { }
            else if (Answer4 == "" || Answer4 == null)
            { }
            **/
            #endregion

            string[] Arraydata = new string[] { };
            Arraydata = CorrectAns.Split(',');

            List<string> CorrectData = new List<string>() { };

            foreach (string a in Arraydata)
            { CorrectData.Add(a); }

            if (CorrectData.ElementAtOrDefault(1) == null)
            { CorrectData.Add("NULL");/**CorrectData[1] = "Null";**/ }
            if (CorrectData.ElementAtOrDefault(2) == null)
            { CorrectData.Add("NULL");/**CorrectData[2] = "Null";**/ }
            if (CorrectData.ElementAtOrDefault(3) == null)
            { CorrectData.Add("NULL");/**CorrectData[3] = "Null";**/ }

            result = SQLcon.SaveExamAndQuestion(Question, QuestionType, Answer1, Answer2, Answer3, Answer4, CorrectData[0].ToString(), CorrectData[1].ToString(), CorrectData[2].ToString(), CorrectData[3].ToString(), EmployeeNumber, CourseSecID, CourseID);

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult deleteQuestion(string ID)
        {
            var result = "";
            try
            { result = SQLcon.DeleteQuestion(ID); }
            catch (Exception ex)
            { result = "false"; }

            var response = new JsonResult();
            response.Data = new
            { res = result };
            return response;
        }

        public ActionResult updateQuestion(string ID, string Question, string C1, string C2, string C3, string C4, string CorAns1, string CorAns2, string CorAns3, string CorAns4)
        {
            var result = "";
            try
            {
                if (Question.Contains("'"))
                {   Question = Question.Replace("'", "\\'"); }
                if (CorAns1 == "" || CorAns1 == null)
                { CorAns1 = "NULL"; }
                if (CorAns2 == "" || CorAns2 == null)
                { CorAns2 = "NULL"; }
                if (CorAns3 == "" || CorAns3 == null)
                { CorAns3 = "NULL"; }
                if (CorAns4 == "" || CorAns4 == null)
                { CorAns4 = "NULL"; }

                result = SQLcon.UpdateQuestion(ID, Question, C1, C2, C3, C4, CorAns1, CorAns2, CorAns3, CorAns4);
            }
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

        public JsonResult getQuestionList(DataTablesParam param, string CourSecID)
        {
            List<VMGetExamQuestions> QuestList = new List<VMGetExamQuestions>();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getQuestionList(CourSecID).Count();

            if (totalCount == 0)
            { totalCount = 0; }

            QuestList = SQLcon.getQuestionList(CourSecID).OrderBy(x => x.OrderNumber).Select(x => new VMGetExamQuestions
            {
                ID = x.ID,
                Question = x.Question,
                QuestionType = x.QuestionType,
                C1 = x.C1,
                C2 = x.C2,
                C3 = x.C3,
                C4 = x.C4,
                CAnswer1 = x.CAnswer1,
                CAnswer2 = x.CAnswer2,
                CAnswer3 = x.CAnswer3,
                CAnswer4 = x.CAnswer4,
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
        #endregion

        #region Exam
        public ActionResult Exam(string CourseID)
        {
            DAL SQLcon = new DAL();
            //List<VMGetExamQuestion> cList = SQLcon.getQuest(CourseID).Select(x => new VMGetExamQuestion
            //{
            //    ID = x.ID,
            //    CourseID = x.CourseID,
            //    CourseSectionID = x.CourseSectionID,
            //    OrderNumber = x.OrderNumber,
            //    Question = x.Question,
            //    QuestionType = x.QuestionType,
            //    C1 = x.C1,
            //    C2 = x.C2,
            //    C3 = x.C3,
            //    C4 = x.C4,
            //    CAnswer1 = x.CAnswer1,
            //    CAnswer2 = x.CAnswer2,
            //    CAnswer3 = x.CAnswer3,
            //    CAnswer4 = x.CAnswer4,
            //}).ToList();


            List<VMElearningCourseSection> cList = SQLcon.ViewCourseSectionByID(CourseID).OrderBy(x => x.OrderSec).Select(x => new VMElearningCourseSection
            {
                ID = x.ID,
                Title = x.Title,
                Type = x.Type,
                SourceFile = x.SrcFile,
                CourseID = x.CourseID,
                OrderSec = x.OrderSec,
                Quiz = SQLcon.GetQuestByCourseIDAndSectionID(CourseID, x.ID.ToString())
            }).ToList();

            return View("Exam", cList);
        }

        public ActionResult GetDataToLoad(string CourseID, int PreviousOrderSec)
        {
            var user = Session["EmployeeNumber"].ToString();
            var loadData = "";
            var CourseSectionOrder = "";
            decimal Progress = 0;
            decimal totalProgress = 0;
            int totalCourseSection = SQLcon.CountCourseSection(CourseID).Count();
            int dataCount = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).Count();

            if (dataCount > 0)
            {
                loadData = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).OrderBy(x => x.OrderSec).Select(x => x.Type).FirstOrDefault().ToString();
                CourseSectionOrder = SQLcon.getDataToLoad(CourseID, PreviousOrderSec).OrderBy(x => x.OrderSec).Select(x => x.OrderSec).FirstOrDefault().ToString();

                Progress = Decimal.Divide(Int32.Parse(CourseSectionOrder), totalCourseSection);
                totalProgress = Progress * 100;
                double totalA = Math.Round(Convert.ToDouble(totalProgress));

                SQLcon.SaveCourseProgress(user, CourseID, totalA.ToString());
            }
            else
            { loadData = "Scoring"; CourseSectionOrder = null; }

            var response = new JsonResult();
            response.Data = new
            {
                res = loadData,
                _currentSectionOrder = CourseSectionOrder
            };
            return response;
        }

        public JsonResult loadVideo(string CourseID)
        {
            var vidTitle = SQLcon.getVideoPath(CourseID).Select(x => x.Title).FirstOrDefault().ToString();
            var vidPath = SQLcon.getVideoPath(CourseID).Select(x => x.VideoPath).FirstOrDefault().ToString();

            var responseVideo = new JsonResult();
            responseVideo.Data = new
            {
                resTitleVideo = vidTitle,
                resVideo = vidPath,
            };
            return responseVideo;
        }

        public JsonResult loadPDF(string CourseID)
        {
            var PDFTitle = SQLcon.getPDFPath(CourseID).Select(x => x.Title).FirstOrDefault().ToString();
            var PDFpath = SQLcon.getPDFPath(CourseID).Select(x => x.PDFPath).FirstOrDefault().ToString();

            var responsePDF = new JsonResult();
            responsePDF.Data = new
            {
                resTitlePDF = PDFTitle,
                resPDF = PDFpath,
            };
            return responsePDF;
        }

        public ActionResult SaveEmployeeAnswers(string[] QuestionID, string[] EmployeeAnswers)
        {
            var user = Session["EmployeeNumber"].ToString();
            var result = "";

            List<string> GetAnswer1 = new List<string>();
            List<string> EmpAnswer1 = new List<string>();
            var response = new JsonResult();
            try
            {
                //Get answer by question ID
                foreach (var qID in QuestionID)
                {
                    GetAnswer1.Add(SQLcon.getAnsList(qID).Select(x => x.Answer1).SingleOrDefault());
                    GetAnswer1.Add(SQLcon.getAnsList(qID).Select(x => x.Answer2).SingleOrDefault());
                    GetAnswer1.Add(SQLcon.getAnsList(qID).Select(x => x.Answer3).SingleOrDefault());
                    GetAnswer1.Add(SQLcon.getAnsList(qID).Select(x => x.Answer4).SingleOrDefault());

                    //Get all null values to remove
                    var countNull = 0;
                    foreach (var i in GetAnswer1.Where(x => x.Contains("NULL")))
                    {
                        countNull += 1;
                    }
                    while (countNull > 0)
                    {
                        GetAnswer1.Remove("NULL");
                        countNull--;
                    }

                    //Get all EmployeeAnswer and Remove its ID
                    if (EmployeeAnswers != null)
                    {
                        foreach (var ans in EmployeeAnswers)
                        {
                            if (ans.IndexOf(qID.ToString()) > -1)
                            {
                                EmpAnswer1.Add(ans.Replace(qID.ToString(), ""));
                            }
                        }
                    }
                    else
                    {
                        response.Data = new
                        {
                            res = "No Data"
                        };
                        return response;
                    }

                    var ansResultStatus = "";
                    var empAnsToBeSave = "";

                    //Store all employee answer in one string variable and identify if the answer is correct if not it 
                    //will store "N" char in ansResultStatus variable;

                    foreach (var i in EmpAnswer1)
                    {
                        empAnsToBeSave += i + ",";
                        if (GetAnswer1.Contains(i))
                        {
                            ansResultStatus += "Y";
                        }
                        else
                        {
                            ansResultStatus += "N";
                        }
                    }

                    //identfy if user answert is incomplete
                    if (EmpAnswer1.Count() < GetAnswer1.Count())
                    {
                        empAnsToBeSave += "InComplete";
                        ansResultStatus += "N";
                    }

                    //Identify if in ansResultStatus variable has a "N" character
                    //if it has an "N" char then it will save in IsCorrect comlumn N
                    if (ansResultStatus.IndexOf("N") > -1)
                    {
                        result = SQLcon.saveAnswers(qID, user, empAnsToBeSave, "N");
                    }
                    else
                    {
                        result = SQLcon.saveAnswers(qID, user, empAnsToBeSave, "Y");
                    }

                    GetAnswer1.Clear();
                    EmpAnswer1.Clear();

                }

                //Get 
            }
            catch (Exception ex) { result = ex.Message; }
            response.Data = new
            {
                res = result
            };
            return response;
        }

        public ActionResult EmpResetAnswer(string CourseSecID)
        {
            var user = Session["EmployeeNumber"].ToString();
            var result = "";
            try
            { result = SQLcon.EmployeeResetAnswer(user, CourseSecID); }
            catch (Exception ex)
            { result = "false"; }

            var responseReset = new JsonResult();
            responseReset.Data = new
            { res = result };
            return responseReset;
        }

        public ActionResult GetDateEnrolled()
        {
            string user = Session["EmployeeNumber"].ToString();
            string result = "";
            DateTime dateEnrolledresult = DateTime.Parse(SQLcon.getEnrolledDate(user).Select(x => x.EnrolledDate).FirstOrDefault().ToString());
            DateTime dateCompletionresult = DateTime.Parse(SQLcon.getEnrolledDate(user).Select(x => x.CompletionDate).FirstOrDefault().ToString());
            //DateTime Add2Days = dateEnrolledresult.AddHours(48);

            if (DateTime.Now > dateCompletionresult)
            { result = "Expired"; }
            else
            { result = dateEnrolledresult.AddHours(48).ToString("MMMM dd, yyyy HH:mm:ss"); }

            var response = new JsonResult();
            response.Data = new
            { _res = result };
            return response;
        }

        public ActionResult GetSectionDataByCourseIDAndOrderSec(string CourseID, string OrderSec)
        {
            var response = new JsonResult();
            DAL SQLcon = new DAL();
            response.Data = SQLcon.ViewCourseSectionByID(CourseID).Where(x => x.OrderSec == Convert.ToInt32(OrderSec)).OrderBy(x => x.OrderSec).SingleOrDefault();
            return response;
        }
        //////////////////////////////////
        public ActionResult GetQuizRadioIDbyCourseID(string CourseID, string SectionID)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            if (CourseID != "")
            {
                response.Data = SQLcon.GetRadioIDByCourseIDAndQType(CourseID, SectionID).Select(x => new VMGetExamQuestion
                {
                    ID = x.ID,
                    QuestionType = x.QuestionType
                }).ToList();
            }
            return response;
        }

        public ActionResult UpdateCourseProgess(string CourseID, string SectionCount,string CurSection)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            var userID = Session["EmployeeNumber"].ToString();
            var secPercent = (int)1 / Convert.ToDouble(SectionCount);
            var CurMustPer = (int) Convert.ToDouble(CurSection) / Convert.ToDouble(SectionCount);
            
            var secResult = secPercent * 100;
            var CurMustPerResult = CurMustPer * 100;

            var currentProgress = SQLcon.SelectEmployeeProgressByEmpIDAndCourseID(userID,CourseID).Select(x=>x.Progress).SingleOrDefault();
            var TotalPercent = 0.0;

            if (CurMustPerResult > currentProgress)
            {   TotalPercent = (double)currentProgress + secResult; }
            else
            {   TotalPercent = currentProgress; }

            if (CourseID != "" && SectionCount != "")
            {
                try
                {   response.Data = new { res = SQLcon.SaveCourseProgress(userID, CourseID,Convert.ToString(TotalPercent)) };   }
                catch (Exception ex) { }
            }
            else
            {   response.Data = new { res = "false" };  }
            return response;
        }

        public ActionResult UpdateUserConsumedTime(string CourseID, string StrHours,string StrMinutes,string StrSeconds)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var TimeFormat = "";
            var result1 = false;
            if (StrHours != "0")
            {
                if (StrHours.Length > 1)
                {   TimeFormat = StrHours;  }
                else
                {   TimeFormat = "0" + StrHours;    }
            }
            else
            {   TimeFormat = "00";  }

            if (StrMinutes != "0")
            {
                if (StrMinutes.Length > 1)
                {   TimeFormat = TimeFormat + ":" + StrMinutes; }
                else
                {   TimeFormat = TimeFormat + ":0" + StrMinutes;    }
            }
            else
            {   TimeFormat = TimeFormat + ":00";    }

            if (StrSeconds != "0")
            {
                if (StrSeconds.Length > 1)
                {   TimeFormat = TimeFormat + ":" + StrSeconds; }
                else
                {   TimeFormat = TimeFormat + ":0" + StrSeconds;    }
            }
            else
            {   TimeFormat = TimeFormat + ":00";    }

            try
            {   result1 = SQLcon.UpdateEMployeeTimeConsumed(userID,CourseID,TimeFormat);    }
            catch (Exception ex) { }

            var response = new JsonResult();
            response.Data = new
            {   res = result1   };

            return response;
        }

        public ActionResult UpdateEmployeeScoreByCourseIDSectionIDAndEmployeeNumber(string CourseID)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var result = false;

            List<VMEmpAnswer> EmpAnswerList = SQLcon.GetEmpAnswersByEmployeeNumberAndCourseID(userID, CourseID).Select(x => new VMEmpAnswer {
                ID = x.ID,
                CourseID = x.CourseID,
                CourseSecID = x.CourseSecID,
                QuestionID = x.QuestionID,
                EmployeeNumber = x.EmployeeNumber,
                EmployeeAnswer = x.EmployeeAnswer,
                IsCorrect = x.IsCorrect,
                DateAnswered = x.DateAnswered
            }).ToList();

            List<VMGetExamQuestions> QuizList = SQLcon.GetQuestionListByCourseID(CourseID).ToList();

            //Get Total Employee Score
            var userScore = EmpAnswerList.Where(x => x.IsCorrect == "Y").Count();
            var ScoreMust = EmpAnswerList.Count();
            var Score = Convert.ToDouble(userScore) / Convert.ToDouble(ScoreMust);
            var TotalScore = (int) Math.Round(Score * 100);
            var Status1 = "";

            //Compare EmpAnswer table with Quiz table by section number
            var SectionCountEmpAnswer = EmpAnswerList.GroupBy(x => x.CourseSecID).Count();
            var SectionCountQuiz = QuizList.GroupBy(x => x.CourseSectionID).Count();

            if (SectionCountEmpAnswer == SectionCountQuiz)
            {
               //Identify the status
                if (TotalScore >= 92)
                {   Status1 = "PASSED"; }
                else
                {   Status1 = "FAILED"; }
                //Save Score and Status
                result = SQLcon.UpdateEmployeeCourseProgressScoreAndStatus(userID,CourseID, TotalScore, Status1);
            }
            else {
                //Save score
                result = SQLcon.UpdateEmployeeCourseProgressScoreAndStatus(userID, CourseID, TotalScore, Status1);
            }

            /**
            try
            {   }
            catch (Exception ex) { }
            **/

            var response = new JsonResult();
            response.Data = new {
                res = result,
                tscore = TotalScore,
                mscore = ScoreMust
            };

            return response;
        }

        public ActionResult SavePersonalityTest(string E, string A, string C, string N, string O)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var userName = Session["EmployeeName"].ToString();
            var response = new JsonResult();
            try
            {
                response.Data = new
                {   res = SQLcon.SaveEmployeePersonalityExam(userID, userName, E, A, C, N, O, DateTime.Now.ToString())  };
            }
            catch (Exception ex) { }
            return response;
        }

        public ActionResult UpdatePersonalityProgress(string CourseID, string StrHours, string StrMinutes, string StrSeconds)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var response = new JsonResult();
            var TimeFormat = "";

            if (StrHours != "0")
            {
                if (StrHours.Length > 1)
                {   TimeFormat = StrHours;  }
                else
                {   TimeFormat = "0" + StrHours;    }
            }
            else
            {   TimeFormat = "00";  }

            if (StrMinutes != "0")
            {
                if (StrMinutes.Length > 1)
                {   TimeFormat = TimeFormat + ":" + StrMinutes; }
                else
                {   TimeFormat = TimeFormat + ":0" + StrMinutes;    }
            }
            else
            {   TimeFormat = TimeFormat + ":00";    }

            if (StrSeconds != "0")
            {
                if (StrSeconds.Length > 1)
                {   TimeFormat = TimeFormat + ":" + StrSeconds; }
                else
                {   TimeFormat = TimeFormat + ":0" + StrSeconds;    }
            }
            else
            {   TimeFormat = TimeFormat + ":00";    }

            try
            {
                response.Data = new
                {   res = SQLcon.UpdateEmployeePersonalityProgress(userID, CourseID, TimeFormat)    };
            }
            catch (Exception ex) { }
            return response;
        }

        public ActionResult getPersonalityTestResult()
        {
            var response = new JsonResult();
            try
            {
                var userID = Session["EmployeeNumber"].ToString();
                var data = SQLcon.GetEmployeePersonalityResult(userID).SingleOrDefault();

                response.Data = new
                {
                    _EmpNo = data.EmployeeNumber,
                    _name = data.Name,
                    _e = data.Extroversion,
                    _a = data.Agreeableness,
                    _c = data.Conscientiousness,
                    _n = data.Neuroticism,
                    _o = data.Openness
                };
            }
            catch (Exception ex)
            {
                response.Data = new
                {   _error = ex.Message };
            }            
            return response;
        }

        public JsonResult getQuestionAndEmpAnswers(DataTablesParam param, string CourseID, string CourseSecID)
        {
            List<VMGetQuestionsAndEmployeeAnswers> QuestList = new List<VMGetQuestionsAndEmployeeAnswers>();
            var sessionEmp = Session["EmployeeNumber"].ToString();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getQuestAndEmpAns(sessionEmp,CourseID,CourseSecID).Count();

            if (totalCount == 0)
            { totalCount = 0; }

            QuestList = SQLcon.getQuestAndEmpAns(sessionEmp, CourseID, CourseSecID).OrderBy(x => x.OrderNumber).Select(x => new VMGetQuestionsAndEmployeeAnswers
            {
                CourseID  = x.CourseID,
                CourseSecID = x.CourseSecID,
                QuestionID = x.QuestionID,
                AnswerID = x.AnswerID,
                Question = x.Question,
                EmployeeAnswer = x.EmployeeAnswer,
                IsCorrect = x.IsCorrect,
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
        #endregion

        public ActionResult GetQuestionDetails(DataTablesParam param, string ID)
        {
            List<VMGetQuestionDetails> QuestList = new List<VMGetQuestionDetails>();
            /**
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getQuestDetail(ID).Count();

            if (totalCount == 0)
            { totalCount = 0; }
            **/
            QuestList = SQLcon.getQuestDetail(ID).Select(x => new VMGetQuestionDetails
            {
                Question = x.Question,
                C1 = x.C1,
                C2 = x.C2,
                C3 = x.C3,
                C4 = x.C4,
                CAnswer1 = x.CAnswer1,
                CAnswer2 = x.CAnswer2,
                CAnswer3 = x.CAnswer3,
                CAnswer4 = x.CAnswer4
            }).AsEnumerable().ToList();

            //return Json(new
            //{
            //    aaData = QuestList,
            //    eEcho = param.sEcho,
            //    iTotalDisplayRecords = totalCount,
            //    iTotalRecords = QuestList.Count()
            //}, JsonRequestBehavior.AllowGet);
            var response = new JsonResult();
            response.Data = new
            { res = QuestList };
            return response;
        }
    }
}