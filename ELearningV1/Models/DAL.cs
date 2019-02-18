using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ELearningV1.Models.ViewModel;

namespace ELearningV1.Models
{
    public class DAL
    {
        string Cons = ConfigurationManager.ConnectionStrings["PayRollCon"].ConnectionString;

        public String AddNewCourse(string CName,string Desc, string ImageName,string Date1)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningAddNewCourse", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@CName", CName);
                        cmd.Parameters.AddWithValue("@CDesc", Desc);
                        cmd.Parameters.AddWithValue("@CImage", ImageName);
                        cmd.Parameters.AddWithValue("@Date1", Date1);

                        cmd.ExecuteNonQuery();
                        return "True";
                    }
                    catch (Exception ex)
                    {
                        return ex.ToString();
                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }
        }

        public VMViewCoursesList ViewCourses()
        {

            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourse", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewCourses course = new VMViewCourses();
                            course.ID = Convert.ToInt32(dr["ID"]);
                            course.Course = Convert.ToString(dr["Course"]);
                            course.Description = Convert.ToString(dr["Description"]);
                            course.Image = Convert.ToString(dr["Image"]);
                            course.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
                            CourseList.Add(course);
                        }
                        return CourseList;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return null;
        }

        public VMViewCoursesList ViewCoursesByName(string CName)
        {

            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourse Where Course like '%" + CName + "%'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewCourses course = new VMViewCourses();
                            course.ID = Convert.ToInt32(dr["ID"]);
                            course.Course = Convert.ToString(dr["Course"]);
                            course.Description = Convert.ToString(dr["Description"]);
                            course.Image = Convert.ToString(dr["Image"]);
                            course.DateCreated = Convert.ToDateTime(dr["DateCreated"]);
                            CourseList.Add(course);
                        }
                        return CourseList;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return null;
        }

        #region LogIn
        public VMKioskLogInUserList KioskLogInUserData(string EmpNum,string Password)
        {

            VMKioskLogInUserList UserDataList = new VMKioskLogInUserList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("EmployeeKioskLoginUser", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeNumber", EmpNum);
                        cmd.Parameters.AddWithValue("@Password", Password);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMKioskLogInUser UserData = new VMKioskLogInUser();
                            UserData.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            UserData.EmployeeName = Convert.ToString(dr["EmployeeName"]);
                            UserData.Department = Convert.ToString(dr["Department"]);
                            UserData.Position = Convert.ToString(dr["Position_name"]);
                            UserData.ReportTo = Convert.ToString(dr["ReportTo"]);
                            UserDataList.Add(UserData);
                        }
                        return UserDataList;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return null;
        }


        public VMELearnEmpDataList FBLoadLeftPanel(string EmpID)
        {

            VMELearnEmpDataList EmpDataList = new VMELearnEmpDataList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From fuzeEmpData1 Where EmployeeNumber = '" + EmpID + "' ", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMELearnEmpData empData = new VMELearnEmpData();
                            empData.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            empData.EmpName = Convert.ToString(dr["EmpName"]);
                            empData.EmpImage = Convert.ToString(dr["EmpImage"]);
                            empData.PEarned = Convert.ToInt32(dr["PEarned"]);
                            empData.PGranted = Convert.ToInt32(dr["PGranted"]);
                            empData.PBalance = Convert.ToInt32(dr["PBalance"]);
                            empData.IsAdmin = Convert.ToBoolean(dr["IsAdmin"]);
                            EmpDataList.Add(empData);
                        }


                        return EmpDataList;
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                }
            }

            return null;
        }

        #endregion



    }
}