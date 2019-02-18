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
    }
}