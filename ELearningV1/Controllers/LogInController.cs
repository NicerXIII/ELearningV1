using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ELearningV1.Models;
using ELearningV1.Models.ViewModel;

namespace ELearningV1.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LogInAccount(string unum, string upass)
        {
            DAL SQLcon = new DAL();
            bool stats = false;
            string path = "http://trecnetkiosk:94/UploadedImages/";
            try
            {
                var logInData = SQLcon.KioskLogInUserData(unum, upass).SingleOrDefault();
                if (logInData != null)
                {
                    //Session["EmployeeNumber"] = logInData.EmployeeNumber.ToString();
                    //Session["EmployeeName"] = logInData.EmployeeName.ToString();
                    //Session["EmployeeDeptName"] = logInData.Department.ToString();
                    //Session["EmployeePositionName"] = logInData.Position.ToString();
                    //Session["EmployeeReportTo"] = logInData.ReportTo.ToString();
                    //var data1 = SQLcon.FBLoadLeftPanel(logInData.EmployeeNumber).Select(x => new VMELearnEmpData
                    //{
                    //    EmpImage = (x.EmpImage != "") ? x.EmpImage : "avatar3.png",
                    //}).SingleOrDefault();



                    //Session["UserProfilePic"] = path + data1.EmpImage;
                    stats = true;
                }
                else
                {
                    stats = false;
                }
            }
            catch (Exception ex)
            {
                stats = false;
            }
            var response = new JsonResult();
            response.Data = new
            {
                result = stats
            };
            return response;
        }

    }
}