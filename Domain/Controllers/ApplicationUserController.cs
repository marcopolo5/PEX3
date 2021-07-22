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
        private int token = 234123;

        public ApplicationUserController() 
        {
        
        }

        public static void Register(UserRegisterModel userRegisterModel)
        {
            string connectionString = "Server=DESKTOP-GPVJ6T8; Database=GeoChat_DB; Trusted_Connection=True;";
            string queryString = "INSERT INTO Users([first_name], [last_name], [email], [password], [token], [verified], [join_date]) VALUES ('"+ userRegisterModel.FirstName + "', '" + userRegisterModel.LastName + "', '" + userRegisterModel.Email + "', '" + userRegisterModel.Password + "', 'no_token', 0, CURRENT_TIMESTAMP);"; // open to SQL Injection (testing purpose tho)

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(queryString, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();
                }
            }
            


            return;
        }


    }
}
