using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;

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

        public JsonResult ImageUpload(CourseImage model,string CName,string CDesc)
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
                        result = SQLcon.AddNewCourse(CName, CDesc,file.FileName,dateNow);
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

        public ActionResult EmployeeCourseDetail()
        {

            return View();
        }
    }
}