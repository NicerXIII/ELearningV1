using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models.ViewModel;
using ELearningV1.Models;

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
            List<VMViewCourses> cList = SQLcon.ViewCourses().Select(x => new VMViewCourses
            {
                ID = x.ID,
                Course = x.Course,
                Description = x.Description,
                Image = x.Image,
                DateCreated = x.DateCreated
            }).ToList();

            

            return View("ViewCourse",cList);
        }

        public ActionResult LoadCourseDataByCourseName(String CName)
        {
            DAL SQLcon = new DAL();
            List<VMViewCourses> cList = SQLcon.ViewCoursesByName(CName).Select(x => new VMViewCourses
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

            try {
                response.Data = new {
                    _s = SQLcon.LoadLogInHistoryByIDandDate(userID,Convert.ToString(sunday)).Select(x=>x.Status1)
                };
            } catch (Exception ex) { }




            return response;
        }
    }
}
