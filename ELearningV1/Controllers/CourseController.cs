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
        DAL SQLcon = new DAL();
        // GET: Course
        #region Views
        [HttpGet]
        public ActionResult CourseList()
        {   return View();  }

        [HttpGet]
        public ActionResult CourseDetail(string CourseID)
        {
            DAL SQLcon = new DAL();
            VMViewCourses courseData = new VMViewCourses();
            try
            { courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault(); }
            catch (Exception ex) { }

            return View(courseData);
        }

        public ActionResult CourseDetailReport()
        { return View(); }

        public ActionResult CreateCourse()
        { return View(); }

        public ActionResult ViewCourseList()
        { return View(); }

        public ActionResult IndexCourseAssign()
        { return View(); }

        public ActionResult EmployeeList()
        { return View(); }
        #endregion

        public ActionResult UpdateCourse(string CourseID, string CourseName, string CourseDesc, bool IsActive)
        {
            DAL SQLcon = new DAL();
            bool stats = false;
            try
            {   stats = SQLcon.UpdateCourse(CourseID, CourseName, CourseDesc, IsActive);    }
            catch (Exception ex)
            {   stats = false;  }

            var response = new JsonResult();
            response.Data = new
            {   res = stats };

            return response;
        }        

        public JsonResult ImageUpload(CourseImage model, string CName, string CDesc, string Days1)
        {
            var response = new JsonResult();
            var result = "False";
            var file = model.ImageFile;
            try
            {
                // var userID = Session["EmployeeNumber"].ToString().Trim();
                var dateNow = DateTime.Now.ToString("MM/dd/yyyy");
                if (file != null)
                {
                    if (!System.IO.File.Exists(Server.MapPath("/UpLoadedImages/" + file.FileName)))
                    {
                        file.SaveAs(Server.MapPath("/UpLoadedImages/" + file.FileName));
                        try
                        {
                            DAL SQLcon = new DAL();
                            result = SQLcon.AddNewCourse(CName, CDesc, file.FileName, dateNow, Days1);
                            result = "True";
                        }
                        catch (Exception ex)
                        {   result = ex.Message;    }
                    }
                    else
                    {   result = "False";   }
                }
            }
            catch (Exception ex)
            {   result = ex.Message;    }
            response.Data = new
            {
                res = result,
                iName = file.FileName
            };
            return response;
        }        

        public ActionResult EmployeeCourseDetail(string CourseID)
        {
            DAL SQLcon = new DAL();
            VMViewCourses courseData = new VMViewCourses();
            try
            {   courseData = SQLcon.ViewCoursesByID(CourseID).SingleOrDefault();    }
            catch (Exception ex) { }

            return View(courseData);
        }

        public ActionResult ApplyEmployeebyCourseID(string CourseID)
        {
            DAL SQLcon = new DAL();
            var userID = Session["EmployeeNumber"].ToString();
            var Stats = "";
            bool result = false;
            try
            {
                var Days1 = SQLcon.ViewCoursesByID(CourseID).Select(x => x.Days1).SingleOrDefault();
                DateTime CompDate = DateTime.Now.AddDays(Days1 - 1);

                var courseStats = SQLcon.CheckCourseIfAlreadyApplied(CourseID, userID);
                if (courseStats == false)
                {   result = SQLcon.ApplyEmployeebyCourseID(CourseID, userID, DateTime.Now.ToString("MM/dd/yyyy"), CompDate.ToString("MM/dd/yyyy"));   }
                else
                {   Stats = "RE";   }

            }
            catch (Exception ex)
            {   }

            var response = new JsonResult();
            response.Data = new
            {
                res = result,
                sta = Stats
            };
            return response;
        }
        
        public JsonResult LoadUserEnrolledList(DataTablesParam param, string CourseID)
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

        public ActionResult LoadCourseDetails(DataTablesParam param)
        {
            List<VMViewCourses> CoursesD = new List<VMViewCourses>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }
            
            totalCount = SQLcon.ViewCoursesList().Count();
            CoursesD = SQLcon.ViewCoursesList().Select(x => new VMViewCourses
            {
                ID = x.ID,
                Course = x.Course,
                Description = x.Description
            }).AsEnumerable().ToList();
            
            return Json(new
            {
                aaData = CoursesD,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = CoursesD.Count()
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadEmployeeByCourseID(DataTablesParam param, string CourseID)
        {
            List<VMViewEmployeeCourseStatus> EmployeeD = new List<VMViewEmployeeCourseStatus>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }

            totalCount = SQLcon.ViewEmployeeCourseTakenByCourseID(CourseID).Count();
            EmployeeD = SQLcon.ViewEmployeeCourseTakenByCourseID(CourseID).Select(x => new VMViewEmployeeCourseStatus
            {
                EmployeeNumber = x.EmployeeNumber,
                EmpName = x.EmpName,
                CampiagnName = x.CampiagnName,
                Progress = Math.Round(x.Progress),
                Score = x.Score
            }).AsEnumerable().ToList();
            
            return Json(new
            {
                aaData = EmployeeD,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = EmployeeD.Count()
            }, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DeleteCourseByCourseID(string CourseID)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            response.Data = new
            {   res = SQLcon.DeleteCourseByCourseID(CourseID)   };
            return response;
         }

        public ActionResult RemoveEmployeeFromCourseByEMployeeNumber(string EmployeeNumber,string CourseID)
        {
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            response.Data = new
            {   res = SQLcon.RemoveEmployeeFromCourseByEmpID(EmployeeNumber, CourseID)  };
            return response;
        }

        public ActionResult EraseEmployeeAswerByCourseID(string EmployeeNumber, string CourseID)
        {
            var response = new JsonResult();
            try
            {
                if (CourseID == "16")
                {   response.Data = new { res = SQLcon.RemoveEmployeePersonalityTestData(EmployeeNumber) }; }
                else
                {   response.Data = new { res = SQLcon.EraseEmployeeAnswerByCourseIDAndUserID(EmployeeNumber, CourseID) };  }
            }
            catch (Exception ex) { }

            return response;

            /** OLD CODE
            DAL SQLcon = new DAL();
            var response = new JsonResult();
            try
            {   response.Data = new { res = SQLcon.EraseEmployeeAnswerByCourseIDAndUserID(EmployeeNumber, CourseID) };  }
            catch (Exception ex) { }

            return response;
            **/
        }        

        public ActionResult GetAllEpmployees(DataTablesParam param)
        {
            List<VMELearnEmpData> Emps = new List<VMELearnEmpData>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            if (param.sSearch != null)
            {
                totalCount = SQLcon.ViewEmployee1().Where(x => x.EmpName.ToString().Contains(param.sSearch)).Count();
                Emps = SQLcon.ViewEmployee1().Where(x => x.EmpName.ToString().Contains(param.sSearch)).OrderBy(x=>x.EmpName).Select(x => new VMELearnEmpData
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName
                }).AsEnumerable().ToList();
            }
            else
            {
                totalCount = SQLcon.ViewEmployee1().Count();
                Emps = SQLcon.ViewEmployee1().Select(x => new VMELearnEmpData
                {
                    EmployeeNumber = x.EmployeeNumber,
                    EmpName = x.EmpName
                }).OrderBy(x => x.EmpName).AsEnumerable().ToList();
            }

            return Json(new
            {
                aaData = Emps,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = Emps.Count()
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetAllCourseAvailable(DataTablesParam param,string EmployeeNumber)
        {
            List<VMViewCourses> Course = new List<VMViewCourses>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            if (param.sSearch != null)
            {
                totalCount = SQLcon.ViewCourseAvailable(EmployeeNumber).Where(x => x.Course.ToString().Contains(param.sSearch)).Count();
                Course = SQLcon.ViewCourseAvailable(EmployeeNumber).Where(x => x.Course.ToString().Contains(param.sSearch)).Select(x => new VMViewCourses
                {
                    ID = x.ID,
                    Course = x.Course
                }).AsEnumerable().ToList();
            }
            else
            {
                totalCount = SQLcon.ViewCourseAvailable(EmployeeNumber).Count();
                Course = SQLcon.ViewCourseAvailable(EmployeeNumber).Select(x => new VMViewCourses
                {
                    ID = x.ID,
                    Course = x.Course
                }).AsEnumerable().ToList();
            }

            return Json(new
            {
                aaData = Course,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = Course.Count()
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult AddEmployeeToCourse(string EmployeeNumber,string CourseID)
        {
            DAL SQLcon = new DAL();
            var result = false;
            try
            {   result = SQLcon.AddEmployeeToCourse(EmployeeNumber, CourseID, DateTime.Now);    }
            catch (Exception ex)
            { }

            var response = new JsonResult();
            response.Data = new
            {   res = result   };
            return response;
        }

        public ActionResult GetAllCourseApplied(DataTablesParam param, string EmployeeNumber)
        {
            List<VMViewCourses> Course = new List<VMViewCourses>();
            DAL SQLcon = new DAL();
            int pageNo = 1;
            int totalCount = 0;

            if (param.iDisplayStart >= param.iDisplayLength)
            { pageNo = (param.iDisplayStart / param.iDisplayLength) + 1; }


            if (param.sSearch != null)
            {
                totalCount = SQLcon.ViewEmployeeCourseApplied(EmployeeNumber).Where(x => x.Course.ToString().Contains(param.sSearch)).Count();
                Course = SQLcon.ViewEmployeeCourseApplied(EmployeeNumber).Where(x => x.Course.ToString().Contains(param.sSearch)).Select(x => new VMViewCourses
                {
                    ID = x.ID,
                    Course = x.Course,
                    Status = x.Status
                }).AsEnumerable().ToList();
            }
            else
            {
                totalCount = SQLcon.ViewEmployeeCourseApplied(EmployeeNumber).Count();
                Course = SQLcon.ViewEmployeeCourseApplied(EmployeeNumber).Select(x => new VMViewCourses
                {
                    ID = x.ID,
                    Course = x.Course,
                    Status = x.Status
                }).AsEnumerable().ToList();
            }

            return Json(new
            {
                aaData = Course,
                eEcho = param.sEcho,
                iTotalDisplayRecords = totalCount,
                iTotalRecords = Course.Count()
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult RemoveEmployeeCourse(string ID)
        {
            DAL SQLcon = new DAL();
            var result = false;
            try
            {   result = SQLcon.RemoveCourseFromEmployeeCourseApplied(ID);  }
            catch (Exception ex)
            { }
            var response = new JsonResult();
            response.Data = new
            {   res = result    };
            return response;
        }

        public ActionResult UpdateEmployeeStatus(string EmployeeNumber, string Status)
        {
            bool result = SQLcon.SaveEmployeeStatus(EmployeeNumber,Status);

            var response = new JsonResult();
            response.Data = new
            {   res = result    };

            return response;
        }

    }
}