using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace Domain.Controllers
{
    public partial class ApplicationUserController
    {
        private string connectionString = "Server=DESKTOP-GPVJ6T8; Database=GeoChat_DB; Trusted_Connection=True;";

        public ApplicationUserController() 
        {
        
        }

        public bool Login(UserLoginModel userLoginModel)
        {
            string spName = @"dbo.[spLoginUser]";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = userLoginModel.Email;

                cmd.Parameters.Add("@password", SqlDbType.VarChar);
                cmd.Parameters["@password"].Value = userLoginModel.Password;

                // RememberMe option - client side 

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string result = reader.GetString(0);
                        if (result == "0")
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool Register(UserRegisterModel userRegisterModel)
        {
            if (UserExists(userRegisterModel.Email))
            {
                return false;
            }

            string spName = @"dbo.[spRegisterUser]";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(spName, conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@email", SqlDbType.VarChar);
                cmd.Parameters["@email"].Value = userRegisterModel.Email;

                cmd.Parameters.Add("@password", SqlDbType.VarChar);
                cmd.Parameters["@password"].Value = userRegisterModel.Password;

                cmd.Parameters.Add("@firstName", SqlDbType.VarChar);
                cmd.Parameters["@firstName"].Value = userRegisterModel.FirstName;

                cmd.Parameters.Add("@lastName", SqlDbType.VarChar);
                cmd.Parameters["@lastName"].Value = userRegisterModel.LastName;

                int result = cmd.ExecuteNonQuery();
                if(result == 0) // Query execution failed
                {
                    return false;
                }
            }
            
            return true;
        }

        private bool UserExists(string email)
        {
            string queryString = "SELECT [email] FROM [Users]";

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string result = reader.GetString(0);
                            if(result == email)
                            {
                                return true;
                            }

                            reader.NextResult();
                        }
                    }
                }
            }

            return false;
        }

    }
}
