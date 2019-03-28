using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models.ViewModel;
using ELearningV1.Models;
using KioskVersion3.Models.ViewModel;

namespace ELearningV1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //Nice ONE BROTHER 2334
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult GetSession()
        {
            var userID = Session["EmployeeNumber"].ToString();
            var userName = Session["EmployeeName"].ToString();
            var userDept = Session["EmployeeDeptName"].ToString();
            var userPos = Session["EmployeePositionName"].ToString();
            var userReportTo = Session["EmployeeReportTo"].ToString();

            var response = new JsonResult();
            response.Data = new
            {
                ID = userID.Trim(),
                Name = userName,
                Dep = userDept,
                Pos = userPos,
                ReportTo = userReportTo
            };

            return response;
        }



        public ActionResult ViewCourse()
        {

            return View();
        }

        public ActionResult LoadCourseData()
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            List<VMViewCourses> cList = SQLcon.ViewCourses(userID).Select(x => new VMViewCourses
            {
                ID = x.ID,
                Course = x.Course,
                Description = x.Description,
                Image = x.Image,
                DateCreated = x.DateCreated
            }).ToList();

            return View("ViewCourse", cList);
        }

        public ActionResult LoadCourseDataByCourseName(String CName)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            List<VMViewCourses> cList = SQLcon.ViewCoursesByName(userID, CName).Select(x => new VMViewCourses
            {
                ID = x.ID,
                Course = x.Course,
                Description = x.Description,
                Image = x.Image,
                DateCreated = x.DateCreated
            }).ToList();



            return View("ViewCourse", cList);
        }


        [HttpPost]
        public ActionResult LoadUserLogInHistoryByID1()
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
            var response = new JsonResult();

            try
            {
                response.Data = new
                {
                    _s = SQLcon.LoadLogInHistoryByIDandDate(userID, Convert.ToString(sunday)).Select(x => x.Status1)
                };
            }
            catch (Exception ex) { }




            return response;
        }

        public JsonResult LoadUserCurrentCourse(DataTablesParam param)
        {
            List<VMELearningCousesProgress> QuestList = new List<VMELearningCousesProgress>();
            var userID = Session["EmployeeNumber"].ToString();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.ViewCurrentEmployeeCoursebyEmployeeNumber(userID).Count();
            QuestList = SQLcon.ViewCurrentEmployeeCoursebyEmployeeNumber(userID).Select(x => new VMELearningCousesProgress
            {
                ID = x.ID,
                EmployeeNumber = x.EmployeeNumber,
                CourseID = x.CourseID,
                Course = x.Course,
                Progress = x.Progress,
                Score = x.Score,
                EnrolledDate = x.EnrolledDate,
                CompletionDate = x.CompletionDate,
                ConsumedTime = x.ConsumedTime
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = QuestList,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = QuestList.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadUserProgressAndStatus()
        {
            List<VMELearningCousesProgress> QuestList = new List<VMELearningCousesProgress>();
            var userID = Session["EmployeeNumber"].ToString();
            DAL SQLcon = new DAL();

            QuestList = SQLcon.ViewCurrentEmployeeCoursebyEmployeeNumber(userID).Select(x => new VMELearningCousesProgress
            {
                ID = x.ID,
                EmployeeNumber = x.EmployeeNumber,
                CourseID = x.CourseID,
                Course = x.Course,
                Progress = x.Progress,
                Score = x.Score,
                EnrolledDate = x.EnrolledDate,
                CompletionDate = x.CompletionDate,
                ConsumedTime = x.ConsumedTime
            }).AsEnumerable().ToList();

            int GetCount = QuestList.Count();
            double PercentMust = GetCount * 100;
            double ProPercent = QuestList.Sum(x => x.Progress);

            double GetPointPercentage = ProPercent / PercentMust;
            double GetCurrentPercentage = Math.Round(GetPointPercentage * 100, MidpointRounding.AwayFromZero);
            double GetRaminingPercentage = Math.Round((1 - GetPointPercentage) * 100, MidpointRounding.AwayFromZero);

            var response = new JsonResult();
            response.Data = new
            {
                _tCurrentPer = GetCurrentPercentage,
                _tRemnainPer = GetRaminingPercentage
            };

            return response;

        }

        public ActionResult LoadPanelBoxData() {

            DAL SQLcon = new DAL();
            String sDate = DateTime.Now.Year.ToString();
            var data = SQLcon.ViewUserDataByYear(sDate);

            var userID = Session["EmployeeNumber"].ToString();

            var CIP = data.Where(x => x.Progress < 100 && x.EmployeeNumber == userID).Count();
            var CC = data.Where(x => x.Progress == 100 && x.EmployeeNumber == userID).Count();
            var PA = data.Where(x => x.Progress == 100).Count();
            var INC = data.Where(x => x.Progress < 100 && x.CompletionDate < DateTime.Now).Count();

            var response = new JsonResult();
            response.Data = new {
                _cip = CIP,
                _cc = CC,
                _pa = PA,
                _inc = INC
            };


            return response;
        }

        public ActionResult CheckCurrentStatus(string CourseID) {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            var userID = Session["EmployeeNumber"].ToString();
            try {
                response.Data = new { res = SQLcon.SelectEmployeeProgressByEmpIDAndCourseID(userID, CourseID).Select(x => x.Progress).SingleOrDefault()
            };
            } catch (Exception ex) { }
            return response;
        }

        public ActionResult ResetUserCourseProgress(string CourseID)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            var userID = Session["EmployeeNumber"].ToString();
            var UserStats = SQLcon.SelectEmployeeProgressByEmpIDAndCourseID(userID, CourseID).Select(x => x.Status1).SingleOrDefault();

            try
            {
                if (UserStats != "")
                {
                    response.Data = new{ res = false};
                }
                else {
                    response.Data = new {  res = SQLcon.ResetCourseProgressByCourseIDAndUserID(userID, CourseID) };
                }
               
            }
            catch (Exception ex) { }
            return response;
        }

        public ActionResult EraseEmployeeAswerByCourseID(string CourseID) {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            var userID = Session["EmployeeNumber"].ToString();

            try {
                response.Data = new { res = SQLcon.EraseEmployeeAnswerByCourseIDAndUserID(userID, CourseID) };
            } catch (Exception ex) { }

            return response;
        }

        public ActionResult ResetEmployeeScoreByCourseID(string CourseID)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            var userID = Session["EmployeeNumber"].ToString();
            try
            {
                response.Data = new { res = SQLcon.ResetEmployeeScoreByEmpIDAndCourseID(userID, CourseID) };
            }
            catch (Exception ex) { }

            return response;
        }
    }
}
