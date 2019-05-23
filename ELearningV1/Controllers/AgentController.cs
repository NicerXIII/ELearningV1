using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;
using KioskVersion3.Models.ViewModel;
using ELearningV1.Views.Agent.Reports;

namespace ELearningV1.Controllers
{
    public class AgentController : Controller
    {
        DAL SQLcon = new DAL();
        // GET: Agent
        public ActionResult Index()
        {
            List<VMGetAppliedEmployees> AppliedEmployees = new List<VMGetAppliedEmployees>();

            AppliedEmployees = SQLcon.getAppliedEmployees().Select(x=> new VMGetAppliedEmployees {
                EmployeeNumber = x.EmployeeNumber,
                Name = x.Name
            }).AsEnumerable().ToList();

            ViewBag.AppliedEmployeelist = AppliedEmployees;
            
            return View("Index");
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
            var PA = data.Where(x => x.Progress == 100 && x.Score >= 92).Count();
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

        public ActionResult getPersonalityTestResult(string EmployeeNumber)
        {
            var data = SQLcon.GetEmployeePersonalityResult(EmployeeNumber).SingleOrDefault();
            var response = new JsonResult();
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
            return response;

        }

        public JsonResult LoadRetakeRequest(DataTablesParam param)
        {
            List<VMGetEmployeeRequestRetake> QuestList = new List<VMGetEmployeeRequestRetake>();
            var userID = Session["EmployeeNumber"].ToString();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.getEmpRequest().Count();
            QuestList = SQLcon.getEmpRequest().Select(x => new VMGetEmployeeRequestRetake
            {
                ID = x.ID,
                EmployeeNumber = x.EmployeeNumber,
                Name = x.Name,
                Course = x.Course,
                EnrolledDate = x.EnrolledDate,
                CompletionDate = x.CompletionDate
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = QuestList,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = QuestList.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveDecision(string ID, string Decision)
        {
            var response = new JsonResult();
            try
            { response.Data = new { res = SQLcon.saveDecision(ID, Decision) }; }
            catch (Exception ex) { }
            return response;
        }

        public ActionResult ExportELearningCourseProgress(/**DataTablesParam param,**/ string fileType)
        {
            Stream oStream;
            EmployeeCourseProgress rpt = new EmployeeCourseProgress();
            string contentType = "";
            if (fileType == "CSV")
            {
                rpt.Refresh();
                rpt.SetDatabaseLogon("sa", "s3cur3d");
                oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelRecord);
                contentType = "application/vnd.xls";
                rpt.Dispose();
                    
            }
            else
            {
                rpt.Refresh();
                rpt.SetDatabaseLogon("sa", "s3cur3d");
                oStream = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelRecord);
                contentType = "application/vnd.ms-excel";
                rpt.Dispose();
            }
            return new FileStreamResult(oStream, contentType);
            //return null;
        }
    }
}