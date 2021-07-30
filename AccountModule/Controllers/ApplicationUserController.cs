﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading.Tasks;
using Domain.Models;
using System.Data;
using System.Data.SqlClient;
using Domain.AccountContracts;
using Domain.RepositoryContracts;
using AccountModule.Helpers;
using Domain;

namespace AccountModule.Controllers
{
    public class ApplicationUserController : IAccountService
    {
        private string connectionString = @"Data Source=.\MSSQLSERVER02;Initial Catalog=GeoChat_DB;Integrated Security=True";
        private readonly UserRepository userRepository = new();
        private readonly static AppConfiguration appConfiguration = new();
        private readonly CurrentUser _currentUser;

        public ApplicationUserController(CurrentUser currentUser) 
        {
            _currentUser = currentUser;
        }

        public bool Login(UserLoginModel userLoginModel)
        {
            //TODO
            (int id, string token) = userRepository.ValidateCredentials(userLoginModel).Result;
            if (string.IsNullOrEmpty(token) || token.Equals("0"))
                return false;
            //var currentUser = userRepository.ReadCurrentUserAsync(id).Result; // TODO: poor guy remains unused
            return true;
        }

        public bool Logout()
        {
            string queryString = "UPDATE [Users] SET token='0' WHERE id="+_currentUser.Id;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(queryString, conn);
                int result = cmd.ExecuteNonQuery();
                if (result == 0) // Query execution failed
                {
                    return false;
                }
            }



            return false;
        }

        public bool Register(UserRegisterModel userRegisterModel)
        {
            //TODO
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
            //TODO
            string queryString = "SELECT [email] FROM [Users] WHERE email='" + email + "'";

            using (SqlConnection conn = new SqlConnection(connectionString))
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
