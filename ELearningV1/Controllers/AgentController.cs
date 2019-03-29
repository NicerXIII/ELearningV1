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
    public class AgentController : Controller
    {
        // GET: Agent
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDateRange()
        {
            var DateFrom = "";
            var DateTo = "";
            if ((DateTime.Now.Day >= 1) && (DateTime.Now.Day <= 15))
            {
                DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString("MM/dd/yyyy");
                DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 15).ToString("MM/dd/yyyy");
            }
            else
            {
                DateFrom = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 16).ToString("MM/dd/yyyy");
                DateTo = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1).ToString("MM/dd/yyyy");

            }
            var response = new JsonResult();
            response.Data = new
            {
                _dateFrom = DateFrom,
                _dateTo = DateTo
            };
            return response;
        }

        public ActionResult LoadEmployeeCourseStatus(DataTablesParam param)
        {
            List<VMViewEmployeeCourseStatus> EmpCStats = new List<VMViewEmployeeCourseStatus>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            if (param.sSearch != null)
            {
                totalCount = SQLcon.ViewEmployeeCourseStatus().Where(x => x.EmpName.ToString().Contains(param.sSearch) || x.Course.Contains(param.sSearch)).Count();
                EmpCStats = SQLcon.ViewEmployeeCourseStatus().Where(x => x.EmpName.ToString().Contains(param.sSearch) || x.Course.Contains(param.sSearch)).Select(x => new VMViewEmployeeCourseStatus
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName,
                    Course = x.Course,
                    Progress = Math.Round(x.Progress),
                    Score = x.Score,
                    Status1 = x.Status1,
                    EnrolledDate = x.EnrolledDate,
                    CompletionDate = x.CompletionDate,
                }).AsEnumerable().ToList();
            }
            else
            {
                totalCount = SQLcon.ViewEmployeeCourseStatus().Count();
                EmpCStats = SQLcon.ViewEmployeeCourseStatus().Select(x => new VMViewEmployeeCourseStatus
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName,
                    Course = x.Course,
                    Progress = Math.Round(x.Progress),
                    Score = x.Score,
                    Status1 = x.Status1,
                    EnrolledDate = x.EnrolledDate,
                    CompletionDate = x.CompletionDate,
                }).AsEnumerable().ToList();
            }


            return Json(new
            {
                aaData = EmpCStats,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = EmpCStats.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadPanelBoxDataOfAllAgents()
        {

            DAL SQLcon = new DAL();
            String sDate = DateTime.Now.Year.ToString();
            var data = SQLcon.ViewUserDataByYear(sDate);

            var userID = Session["EmployeeNumber"].ToString();

            var CIP = data.Where(x => x.Progress < 100).Count();
            var CC = data.Where(x => x.Progress == 100).Count();
            var PA = data.Where(x => x.Progress == 100).Count();
            var INC = data.Where(x => x.Progress < 100 && x.CompletionDate < DateTime.Now).Count();

            var response = new JsonResult();
            response.Data = new
            {
                _cip = CIP,
                _cc = CC,
                _pa = PA,
                _inc = INC
            };
            return response;
        }

        public ActionResult GetEmployeeStatusForBarChart() {
            DAL SQLcon = new DAL();

            var data = SQLcon.GetEmployeeStatusCount().SingleOrDefault();
            var response = new JsonResult();
            response.Data = new {
                passed = data.Passed,
                failed = data.Failed,
                inprog = data.InProgress
            };
            return response;
        }


        public ActionResult LoadEmployeeByStatusAndDateRange(DataTablesParam param,string Status,string DFrom,string DTo)
        {
            List<VMViewEmployeeCourseStatus> EmpCStats = new List<VMViewEmployeeCourseStatus>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            if (param.sSearch != null)
            {
                totalCount = SQLcon.ViewEmployeeByStatusAndDateRange(Status,DFrom,DTo).Where(x => x.EmpName.ToString().Contains(param.sSearch) || x.CampiagnName.Contains(param.sSearch)).Count();
                EmpCStats = SQLcon.ViewEmployeeByStatusAndDateRange(Status, DFrom, DTo).Where(x => x.EmpName.ToString().Contains(param.sSearch) || x.CampiagnName.Contains(param.sSearch)).Select(x => new VMViewEmployeeCourseStatus
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName,
                    Score = x.Score,
                    Status1 = x.Status1,
                    CampiagnName = x.CampiagnName,
                    EnrolledDate = x.EnrolledDate
                }).AsEnumerable().ToList();
            }
            else
            {
                totalCount = SQLcon.ViewEmployeeByStatusAndDateRange(Status, DFrom, DTo).Count();
                EmpCStats = SQLcon.ViewEmployeeByStatusAndDateRange(Status, DFrom, DTo).Select(x => new VMViewEmployeeCourseStatus
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName,
                    Score = x.Score,
                    Status1 = x.Status1,
                    CampiagnName = x.CampiagnName,
                    EnrolledDate = x.EnrolledDate
                }).AsEnumerable().ToList();
            }
            return Json(new
            {
                aaData = EmpCStats,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = EmpCStats.Count()
            }, JsonRequestBehavior.AllowGet);
        }
    }
}