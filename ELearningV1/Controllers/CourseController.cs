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
    public class CourseController : Controller
    {
        // GET: Course
        [HttpGet]
        public ActionResult CourseList()
        {
            return View();
        }

     
        [HttpGet]
        public ActionResult CourseDetail(string CourseID)
        {
            
            DAL SQLcon = new DAL();
            VMViewCourses courseData = new VMViewCourses();
            try {
                courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault();
            } catch (Exception ex) { }

            return View(courseData);
        }

        public ActionResult UpdateCourse(string CourseID,string CourseName,string CourseDesc,bool IsActive)
        {
            DAL SQLcon = new DAL();
            bool stats = false;
            try
            {
                stats = SQLcon.UpdateCourse(CourseID,CourseName,CourseDesc,IsActive);
            }
            catch (Exception ex) {
                stats = false;
            }

            var response = new JsonResult();
            response.Data = new {
                res = stats
            };

            return response;
        }

        public ActionResult CourseDetailReport()
        {
            return View();
        }

        public JsonResult ImageUpload(CourseImage model,string CName,string CDesc,string Days1)
        {
           // var userID = Session["EmployeeNumber"].ToString().Trim();
            var result = "False";
            var file = model.ImageFile;
            var dateNow = DateTime.Now.ToString("MM/dd/yyyy");
            if (file != null)
            {
                if (!System.IO.File.Exists(Server.MapPath("/UpLoadedImages/" + file.FileName)))
                {
                    file.SaveAs(Server.MapPath("/UpLoadedImages/" + file.FileName));
                    try
                    {
                        DAL SQLcon = new DAL();
                        result = SQLcon.AddNewCourse(CName, CDesc,file.FileName,dateNow,Days1);
                        result = "True";
                    }
                    catch (Exception ex) { }
                }
                else
                {
                    result = "False";
                }
            }
            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                iName = file.FileName
            };
            return response;
        }

        public ActionResult CreateCourse() {

            return View();
        }

        public ActionResult EmployeeCourseDetail(string CourseID)
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

        public ActionResult ApplyEmployeebyCourseID(string CourseID)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var Stats = "";
            bool result = false;
            try {
                var Days1 = SQLcon.ViewCoursesByID(CourseID).Select(x => x.Days1).SingleOrDefault();
                DateTime CompDate = DateTime.Now.AddDays(Days1 - 1);

                var courseStats = SQLcon.CheckCourseIfAlreadyApplied(CourseID, userID);
                if (courseStats == false)
                {
                    result = SQLcon.ApplyEmployeebyCourseID(CourseID, userID, DateTime.Now.ToString("MM/dd/yyyy"), CompDate.ToString("MM/dd/yyyy"));
                }
                else {
                    Stats = "RE";
                }

            } catch (Exception ex) {
            }
            var response = new JsonResult();
            response.Data = new {
                res = result,
                sta = Stats
            };
            return response;
        }


        public JsonResult LoadUserEnrolledList(DataTablesParam param,string CourseID)
        {
            List<VMViewCourseUser> CourseUserList = new List<VMViewCourseUser>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.ViewCourseUserByCourseID(CourseID).Count();
            CourseUserList = SQLcon.ViewCourseUserByCourseID(CourseID).Select(x => new VMViewCourseUser
            {
                EmployeeNumber = x.EmployeeNumber,
                EmpName = x.EmpName,
                Department = x.Department,
                CompletionDate = x.CompletionDate
            }).AsEnumerable().ToList();

            return Json(new
            {
                aaData = CourseUserList,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = CourseUserList.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewCourseList()
        {

            return View();
        }



    }
}