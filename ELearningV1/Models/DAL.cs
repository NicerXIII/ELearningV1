using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

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

    }
}