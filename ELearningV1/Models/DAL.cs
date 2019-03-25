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

        #region LogIn
        public VMKioskLogInUserList KioskLogInUserData(string EmpNum, string Password)
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
                    { }
                    finally
                    { con.Close(); }
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
                    { }
                    finally
                    { con.Close(); }
                }
            }
            return null;
        }

        public bool SaveLogInLogHistory(string EmployeeNumber, string Date1)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ElearningLogInHistory VALUES('" + EmployeeNumber + "','" + Date1 + "')", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }
        #endregion

        #region Home
        public VMLogHistoryList LoadLogInHistoryByIDandDate(string EmpID, string Date1)
        {
            VMLogHistoryList LogsList = new VMLogHistoryList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("eLearningGetLogInHistory", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeNumber", EmpID);
                        cmd.Parameters.AddWithValue("@DateStart", Date1);

                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMLogHistory logs = new VMLogHistory();
                            logs.Status1 = Convert.ToInt32(dr["Status1"]);
                            logs.Date1 = Convert.ToDateTime(dr["Date1"]);
                            LogsList.Add(logs);
                        }
                        return LogsList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }
        #endregion

        #region Course
        public String AddNewCourse(string CName, string Desc, string ImageName, string Date1, string MinDay)
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
                        cmd.Parameters.AddWithValue("@Day", MinDay);

                        cmd.ExecuteNonQuery();
                        return "True";
                    }
                    catch (Exception ex)
                    { return ex.ToString(); }
                    finally
                    { con.Close(); }
                }
            }
        }

        public bool UploadNewFile(string Title, string Type, string SrcFile, string CourseID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningAddSourceFile", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@Title", Title);
                        cmd.Parameters.AddWithValue("@Type", Type);
                        cmd.Parameters.AddWithValue("@SrcFile", SrcFile);
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);

                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public bool DeleteSectionById(string SecID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From ELearningCourseSection Where ID='" + SecID + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public VMViewCoursesList ViewCourses(string UserID)
        {
            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ViewCoursesByUserID", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeNumber", UserID);
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
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMViewCourseUserList ViewCourseUserByCourseID(string CourseID)
        {
            VMViewCourseUserList CourseUserList = new VMViewCourseUserList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("GetCourseEnrolledUsers", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewCourseUser courseUser = new VMViewCourseUser();
                            courseUser.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            courseUser.EmpName = Convert.ToString(dr["EmpName"]);
                            courseUser.Department = Convert.ToString(dr["Department"]);
                            courseUser.CompletionDate = Convert.ToDateTime(dr["CompletionDate"]);
                            CourseUserList.Add(courseUser);
                        }
                        return CourseUserList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMViewCoursesList ViewCoursesByName(string UserID, string CName)
        {
            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ViewCoursesByUserIDandCName", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeNumber", UserID);
                        cmd.Parameters.AddWithValue("@CourseName", CName);
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
                    { }
                    finally
                    { con.Close(); }
                }
            }
            return null;
        }

        public VMViewCoursesList ViewCoursesByID(string CID)
        {
            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourse Where ID='" + CID + "'", con))
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
                            course.IsActive = Convert.ToBoolean(dr["IsActive"]);
                            course.Days1 = Convert.ToInt32(dr["Day1"]);
                            CourseList.Add(course);
                        }
                        return CourseList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMCourseSectionList ViewCourseSectionByID(string CID)
        {
            VMCourseSectionList SectionList = new VMCourseSectionList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourseSection Where CourseID='" + CID + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMCourseSection section = new VMCourseSection();
                            section.ID = Convert.ToInt32(dr["ID"]);
                            section.Title = Convert.ToString(dr["Title"]);
                            section.Type = Convert.ToString(dr["Type"]);
                            section.SrcFile = Convert.ToString(dr["SrcFile"]);
                            section.CourseID = Convert.ToString(dr["CourseID"]);
                            section.OrderSec = Convert.ToInt32(dr["OrderSec"]);
                            SectionList.Add(section);
                        }
                        return SectionList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public bool UpdateCourse(string CID, string CName, string Desc, bool IsActive)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Update ELearningCourse SET COurse='" + CName + "', Description='" + Desc + "',IsActive='" + IsActive + "' Where ID = '" + CID + "' ", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public bool UpdateSectionOrdering(string SectionID, string OderVal)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Update ELearningCourseSection SET OrderSec='" + OderVal + "' Where ID='" + SectionID + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public bool ApplyEmployeebyCourseID(string CourseID, string EmployeeNumber, string Date1, string CompDate)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ELearningCourseProgress(EmployeeNumber,CourseID,Progress,Score,EnrolledDate,CompletionDate,ConsumedTime) VALUES('" + EmployeeNumber + "','" + CourseID + "','0','0','" + Date1 + "','" + CompDate + "','0')", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public VMELearningCousesProgressList ViewCurrentEmployeeCoursebyEmployeeNumber(string EmployeeNumber)
        {
            VMELearningCousesProgressList CourseProgList = new VMELearningCousesProgressList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("eLearningSelectCurrentCourseofEmployee", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMELearningCousesProgress courseprog = new VMELearningCousesProgress();
                            courseprog.ID = Convert.ToInt32(dr["ID"]);
                            courseprog.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            courseprog.CourseID = Convert.ToInt32(dr["CourseID"]);
                            courseprog.Course = Convert.ToString(dr["Course"]);
                            courseprog.Progress = Convert.ToInt32(dr["Progress"]);
                            courseprog.Score = (float)Convert.ToDouble(dr["Score"]);
                            courseprog.EnrolledDate = Convert.ToDateTime(dr["EnrolledDate"]);
                            courseprog.CompletionDate = Convert.ToDateTime(dr["CompletionDate"]);
                            courseprog.ConsumedTime = (float)Convert.ToDouble(dr["ConsumedTime"]);
                            CourseProgList.Add(courseprog);
                        }
                        return CourseProgList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public bool CheckCourseIfAlreadyApplied(string CourseID, string UserID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourseProgress Where CourseID='" + CourseID + "' and EmployeeNumber='" + UserID + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        if (dr.HasRows)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                    finally
                    { con.Close(); }
                }
            }
        }

        public VMELearningCousesProgressList ViewUserDataByYear(string Year1)
        {
            VMELearningCousesProgressList CourseProgressList = new VMELearningCousesProgressList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ELearningCourseProgress Where YEAR(EnrolledDate) = '" + Year1 + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMELearningCousesProgress courseProg = new VMELearningCousesProgress();
                            courseProg.ID = Convert.ToInt32(dr["ID"]);
                            courseProg.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            courseProg.CourseID = Convert.ToInt32(dr["CourseID"]);
                            courseProg.Progress = Convert.ToInt32(dr["Progress"]);
                            courseProg.Score = Convert.ToInt32(dr["Score"]);
                            courseProg.EnrolledDate = Convert.ToDateTime(dr["EnrolledDate"]);
                            courseProg.CompletionDate = Convert.ToDateTime(dr["CompletionDate"]);
                            courseProg.ConsumedTime = Convert.ToInt32(dr["ConsumedTime"]);
                            CourseProgressList.Add(courseProg);
                        }
                        return CourseProgressList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMViewCoursesList ViewCoursesList()
        {
            VMViewCoursesList CourseList = new VMViewCoursesList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select ID,Course,Description From ELearningCourse", con))
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
                            CourseList.Add(course);
                        }
                        return CourseList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMViewEmployeeCourseStatusList ViewEmployeeCourseTakenByCourseID(string CourseID)
        {
            VMViewEmployeeCourseStatusList EmployeeCourseList = new VMViewEmployeeCourseStatusList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeEnrolledBySelectedCourse", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewEmployeeCourseStatus empcourse = new VMViewEmployeeCourseStatus();
                            empcourse.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            empcourse.EmpName = Convert.ToString(dr["EmpName"]);
                            empcourse.CampiagnName = Convert.ToString(dr["CampaignName"]);
                            empcourse.Progress = Convert.ToInt32(dr["Progress"]);
                            empcourse.Score = Convert.ToInt32(dr["Score"]);
                            EmployeeCourseList.Add(empcourse);
                        }
                        return EmployeeCourseList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public bool DeleteCourseByCourseID(string CourseID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningDeleteCourseByCourseID", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CourseID", CourseID);
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        public bool RemoveEmployeeFromCourseByEmpID(string EmployeeNumber)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Delete From ELearningCourseProgress Where EmployeeNumber='" + EmployeeNumber + "'", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                    catch (Exception ex)
                    { return false; }
                    finally
                    { con.Close(); }
                }
            }
        }

        #endregion

        #region Creating Exam
        public string SaveExamAndQuestion(string Question, string QuestType, string Ans1, string Ans2, string Ans3, string Ans4, string CorAns1, string CorAn2, string CorAn3, string CorAn4, string EmployeeNumber, string CourseSecID, string CourseID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningSaveQuestion", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        com.Parameters.AddWithValue("@CourseSecID", CourseSecID);
                        com.Parameters.AddWithValue("@CourseID", CourseID);
                        com.Parameters.AddWithValue("@Question", Question);
                        com.Parameters.AddWithValue("@QuestionType", QuestType);
                        com.Parameters.AddWithValue("@Choice1", Ans1);
                        com.Parameters.AddWithValue("@Choice2", Ans2);
                        com.Parameters.AddWithValue("@Choice3", Ans3);
                        com.Parameters.AddWithValue("@Choice4", Ans4);
                        com.Parameters.AddWithValue("@ChoiceAns", CorAns1);
                        com.Parameters.AddWithValue("@ChoiceAns2", CorAn2);
                        com.Parameters.AddWithValue("@ChoiceAns3", CorAn3);
                        com.Parameters.AddWithValue("@ChoiceAns4", CorAn4);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string SaveTestName(string TestName, string Type, string CourseID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningSaveTestName", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@TestName", TestName);
                        com.Parameters.AddWithValue("@Type", Type);
                        com.Parameters.AddWithValue("@CourseID", CourseID);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string DeleteQuestion(string ID)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningDeleteQuestion", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@QuestionID", ID);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string UpdateQuestion(string ID, string Question, string C1, string C2, string C3, string C4, string CorAns1, string CorAns2, string CorAns3, string CorAns4)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningUpdateQuestion", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@Question", Question);
                        com.Parameters.AddWithValue("@C1", C1);
                        com.Parameters.AddWithValue("@C2", C2);
                        com.Parameters.AddWithValue("@C3", C3);
                        com.Parameters.AddWithValue("@C4", C4);
                        com.Parameters.AddWithValue("@CAnswer1", CorAns1);
                        com.Parameters.AddWithValue("@CAnswer2", CorAns2);
                        com.Parameters.AddWithValue("@CAnswer3", CorAns3);
                        com.Parameters.AddWithValue("@CAnswer4", CorAns4);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string UpdateQuestionOrderNo(string ID, string OrderNo)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningUpdateQuestionOrderNumber", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@OrderNo", OrderNo);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string UpdateTestName(string ID, string TestName)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningUpdateTestName", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@ID", ID);
                        com.Parameters.AddWithValue("@TestName", TestName);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public getExamQuestionList getQuestionList(string CourSecID)
        {
            getExamQuestionList QuestionList = new getExamQuestionList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetQuestions", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CouSecID", CourSecID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetExamQuestions question = new VMGetExamQuestions();
                                question.ID = dr["ID"].ToString();
                                question.Question = dr["Question"].ToString();
                                question.QuestionType = dr["QuestionType"].ToString();
                                question.C1 = dr["C1"].ToString();
                                question.C2 = dr["C2"].ToString();
                                question.C3 = dr["C3"].ToString();
                                question.C4 = dr["C4"].ToString();
                                question.CAnswer1 = dr["CAnswer1"].ToString();
                                question.CAnswer2 = dr["CAnswer2"].ToString();
                                question.CAnswer3 = dr["CAnswer3"].ToString();
                                question.CAnswer4 = dr["CAnswer4"].ToString();
                                question.CourseSectionID = dr["CourseSectionID"].ToString();
                                question.CourseID = dr["CourseID"].ToString();
                                question.OrderNumber = Int32.Parse(dr["OrderNumber"].ToString());
                                QuestionList.Add(question);
                            }
                            return QuestionList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }

            return null;
        }

        public string EmployeeResetAnswer(string EmployeeNumber, string CourseSec)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningResetEmployeeAnswer", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        com.Parameters.AddWithValue("@CourseSection", CourseSec);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "true";
        }
        #endregion

        #region Getting Questions and answers
        public getDataToLoadList getDataToLoad(string CourseID, int PreviousOrderSec)
        {
            getDataToLoadList loadList = new getDataToLoadList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetDataToLoad", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                            cmd.Parameters.AddWithValue("@PreviousOrderSec", PreviousOrderSec);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    VMGetDataToLoadOneByOne loadData = new VMGetDataToLoadOneByOne();
                                    loadData.ID = dr["ID"].ToString();
                                    loadData.Title = dr["Title"].ToString();
                                    loadData.Type = dr["Type"].ToString();
                                    loadData.SourceFile = dr["SrcFile"].ToString();
                                    loadData.CourSecID = dr["CourseID"].ToString();
                                    loadData.OrderSec = dr["OrderSec"].ToString();
                                    loadList.Add(loadData);
                                }
                            }
                            return loadList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getDataToLoadList CountCourseSection(string CourseID)
        {
            getDataToLoadList loadList = new getDataToLoadList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningCountCourseSection", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dt.Rows)
                                {
                                    VMGetDataToLoadOneByOne loadData = new VMGetDataToLoadOneByOne();
                                    loadData.ID = dr["ID"].ToString();
                                    loadList.Add(loadData);
                                }
                            }
                            return loadList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getVideoPathList getVideoPath(string CourseID)
        {
            getVideoPathList videoPathList = new getVideoPathList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetVideoPath", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetVideoPath vidPATH = new VMGetVideoPath();
                                vidPATH.ID = dr["ID"].ToString();
                                vidPATH.Title = dr["Title"].ToString();
                                vidPATH.Type = dr["Type"].ToString();
                                vidPATH.VideoPath = dr["SrcFile"].ToString();
                                vidPATH.CourseID = dr["CourseID"].ToString();
                                vidPATH.OrderSec = dr["OrderSec"].ToString();
                                videoPathList.Add(vidPATH);
                            }
                            return videoPathList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getPDFPathList getPDFPath(string CourseID)
        {
            getPDFPathList PDFPathList = new getPDFPathList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetPDFPath", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetPDFPath pdfPATH = new VMGetPDFPath();
                                pdfPATH.ID = dr["ID"].ToString();
                                pdfPATH.Title = dr["Title"].ToString();
                                pdfPATH.Type = dr["Type"].ToString();
                                pdfPATH.PDFPath = dr["SrcFile"].ToString();
                                pdfPATH.CourseID = dr["CourseID"].ToString();
                                pdfPATH.OrderSec = dr["OrderSec"].ToString();
                                PDFPathList.Add(pdfPATH);
                            }
                            return PDFPathList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getQuestionList getQuest(string CourseID)
        {
            getQuestionList PDFPathList = new getQuestionList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetQuestionOneByOne", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CourseID", CourseID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetExamQuestion quest = new VMGetExamQuestion();
                                quest.ID = dr["ID"].ToString();
                                quest.CourseID = dr["CourseID"].ToString();
                                quest.CourseSectionID = dr["CourseSectionID"].ToString();
                                quest.OrderNumber = Int32.Parse(dr["OrderNumber"].ToString());
                                quest.Question = dr["Question"].ToString();
                                quest.QuestionType = dr["QuestionType"].ToString();
                                quest.C1 = dr["C1"].ToString();
                                quest.C2 = dr["C2"].ToString();
                                quest.C3 = dr["C3"].ToString();
                                quest.C4 = dr["C4"].ToString();
                                quest.CAnswer1 = dr["CAnswer1"].ToString();
                                quest.CAnswer2 = dr["CAnswer2"].ToString();
                                quest.CAnswer3 = dr["CAnswer3"].ToString();
                                quest.CAnswer4 = dr["CAnswer4"].ToString();
                                PDFPathList.Add(quest);
                            }
                            return PDFPathList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public QuestionIDList getQuestID(string Answer)
        {
            QuestionIDList questID = new QuestionIDList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetQuestionIDbyEmployeeAnswer", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Answer", Answer);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetQuestionIDbyEmployeeAnswer quest = new VMGetQuestionIDbyEmployeeAnswer();
                                quest.QuestionID = dr["ID"].ToString();
                                quest.CAnswer = dr["CAnswer"].ToString();
                                questID.Add(quest);
                            }
                            return questID;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getAnswersList getAnsList(string QuestionID)
        {
            getAnswersList questID = new getAnswersList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetAnswers", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@QuestionID", QuestionID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetAnswers quest = new VMGetAnswers();
                                quest.Answer1 = dr["CAnswer1"].ToString();
                                quest.Answer2 = dr["CAnswer2"].ToString();
                                quest.Answer3 = dr["CAnswer3"].ToString();
                                quest.Answer4 = dr["CAnswer4"].ToString();

                                questID.Add(quest);
                            }
                            return questID;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public string saveAnswers(string QuestionID, string EmployeeNumber, string Answers, string isCorrect)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningSaveAnswers", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@QuestionID", QuestionID);
                        com.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        com.Parameters.AddWithValue("@Answers", Answers);
                        com.Parameters.AddWithValue("@IsCorrect", isCorrect);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public getQuestIDList getQuestListByCourseSec(string CourSecID)
        {
            getQuestIDList QuestionList = new getQuestIDList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetQuestions", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@CouSecID", CourSecID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetQuestionID question = new VMGetQuestionID();
                                question.QuestionID = dr["ID"].ToString();
                                QuestionList.Add(question);
                            }
                            return QuestionList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public EmployeeAnswersList getEmployeeAnswer(string EmployeeNumber, string QID)
        {
            EmployeeAnswersList AnswerList = new EmployeeAnswersList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetEmployeeAnswers", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                            cmd.Parameters.AddWithValue("@QuestionID", QID);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetEmployeeAnswers question = new VMGetEmployeeAnswers();
                                question.QuestionID = dr["QuestionID"].ToString();
                                question.EmployeeAnswer = dr["EmployeeAnswer"].ToString();
                                question.IsCorrect = dr["IsCorrect"].ToString();
                                AnswerList.Add(question);
                            }
                            return AnswerList;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public getEmployeeDateEnrolledList getEnrolledDate(string EmployeeNumber)
        {
            getEmployeeDateEnrolledList listDateEnrolled = new getEmployeeDateEnrolledList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetEnrolledDate", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                            con.Open();
                            cmd.ExecuteNonQuery();

                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            foreach (DataRow dr in dt.Rows)
                            {
                                VMGetEmployeeEnrollDate date = new VMGetEmployeeEnrollDate();
                                date.EnrolledDate = DateTime.Parse(dr["EnrolledDate"].ToString());
                                date.CompletionDate = DateTime.Parse(dr["CompletionDate"].ToString());
                                listDateEnrolled.Add(date);
                            }
                            return listDateEnrolled;
                        }
                        catch (Exception ex)
                        { }
                        finally
                        { con.Close(); }
                    }
                }
            }
            return null;
        }

        public string SaveEmployeeScore(string EmployeeNumber, string CourseID, string CourseSecID, string Score)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningSaveScore", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        com.Parameters.AddWithValue("@CourseID", CourseID);
                        com.Parameters.AddWithValue("@CourseSectionID", CourseSecID);
                        com.Parameters.AddWithValue("@Score", Score);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }

        public string SaveCourseProgress(string EmployeeNumber, string CourseID, string Progress)
        {
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand com = new SqlCommand("ELearningSaveEmployeeProgress", con))
                {
                    try
                    {
                        com.CommandType = CommandType.StoredProcedure;
                        com.Parameters.AddWithValue("@EmployeeNumber", EmployeeNumber);
                        com.Parameters.AddWithValue("@CourseID", CourseID);
                        com.Parameters.AddWithValue("@Progress", Progress);
                        con.Open();
                        com.ExecuteNonQuery();
                        con.Close();
                    }
                    catch (Exception ex)
                    { return ex.Message; }
                }
            }
            return "Success";
        }
        #endregion

        #region Agent Status
        public VMViewEmployeeCourseStatusList ViewEmployeeCourseStatus()
        {
            VMViewEmployeeCourseStatusList EmployeeCourseStatusList = new VMViewEmployeeCourseStatusList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("Select * From ViewEmployeeListStatus", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.Text;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewEmployeeCourseStatus empstatus = new VMViewEmployeeCourseStatus();
                            empstatus.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            empstatus.EmpName = Convert.ToString(dr["EmpName"]);
                            empstatus.Course = Convert.ToString(dr["Course"]);
                            empstatus.Progress = Convert.ToInt32(dr["Progress"]);
                            empstatus.Score = Convert.ToInt32(dr["Score"]);
                            empstatus.Status1 = Convert.ToString(dr["Status1"]);
                            empstatus.EnrolledDate = Convert.ToDateTime(dr["EnrolledDate"]);
                            empstatus.CompletionDate = Convert.ToDateTime(dr["CompletionDate"]);
                            EmployeeCourseStatusList.Add(empstatus);
                        }
                        return EmployeeCourseStatusList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMEmpStatsCountList GetEmployeeStatusCount()
        {
            VMEmpStatsCountList EmpCourseStatusCount = new VMEmpStatsCountList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("ELearningGetCourseExamStatus", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMEmpStatsCount empstatus = new VMEmpStatsCount();
                            empstatus.Passed = Convert.ToInt32(dr["Passed"]);
                            empstatus.Failed = Convert.ToInt32(dr["Failed"]);
                            empstatus.InProgress = Convert.ToInt32(dr["InProgress"]);
                            EmpCourseStatusCount.Add(empstatus);
                        }
                        return EmpCourseStatusCount;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        public VMViewEmployeeCourseStatusList ViewEmployeeByStatusAndDateRange(string Status,string DFrom,string DTo)
        {
            VMViewEmployeeCourseStatusList EmployeeCourseStatusList = new VMViewEmployeeCourseStatusList();
            using (SqlConnection con = new SqlConnection(Cons))
            {
                using (SqlCommand cmd = new SqlCommand("GetEmployeeByStatusAndDateRange", con))
                {
                    try
                    {
                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@DFrom", DFrom);
                        cmd.Parameters.AddWithValue("@DTo", DTo);
                        SqlDataReader dr = cmd.ExecuteReader();

                        while (dr.Read())
                        {
                            VMViewEmployeeCourseStatus empstatus = new VMViewEmployeeCourseStatus();
                            empstatus.EmployeeNumber = Convert.ToString(dr["EmployeeNumber"]);
                            empstatus.EmpName = Convert.ToString(dr["EmpName"]);
                            empstatus.Score = Convert.ToInt32(dr["Score"]);
                            empstatus.Status1 = Convert.ToString(dr["Status1"]);
                            empstatus.CampiagnName = Convert.ToString(dr["CampaignName"]);
                            empstatus.EnrolledDate = Convert.ToDateTime(dr["EnrolledDate"]);
                            EmployeeCourseStatusList.Add(empstatus);
                        }
                        return EmployeeCourseStatusList;
                    }
                    catch (Exception ex)
                    { }
                    finally
                    { con.Close(); }
                }
            }

            return null;
        }

        #endregion
    }
}