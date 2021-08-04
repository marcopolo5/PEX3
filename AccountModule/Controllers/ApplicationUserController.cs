using System;
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
using Domain.Helpers;
using Domain;
using System.Text.RegularExpressions;

namespace AccountModule.Controllers
{
    public class ApplicationUserController : IApplicationUserController
    {
        private readonly UserRepository _userRepository = new();
        private readonly ProfileRepository _profileRepository = new();
        private readonly AppConfiguration _appConfiguration = new();
        public static CurrentUser CurrentUser;


        public ApplicationUserController()
        {
        }

        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            UserLoginModel userLoginModel = new UserLoginModel
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            };

            (int id, string token) = _userRepository.ValidateCredentials(userLoginModel).Result;
            if (string.IsNullOrEmpty(token) || token.Equals("0"))
            {
                return false;
            }

            // save token to file
            if (rememberMe)
            {
                _appConfiguration.SaveToken(token);
            }


            //CurrentUser.rememberMe = rememberMe;

            // initialize user's attributes
            User user = await _userRepository.ReadAsync(email);
            // ReadCurrentUserAsync needs inspection
            /*var viewUser = await _userRepository.ReadCurrentUserAsync(user.Id);
            CurrentUser.InitializeFields(viewUser.Profile, viewUser.Settings);*/

            return true;
        }

        public async Task<bool> Logout()
        {
            // moved to user repository
            //string queryString = "UPDATE [Users] SET token='0' WHERE id="+_currentUser.Id;

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    conn.Open();
            //    SqlCommand cmd = new SqlCommand(queryString, conn);
            //    int result = cmd.ExecuteNonQuery();
            //    if (result == 0) // Query execution failed
            //    {
            //        return false;
            //    }
            //}

            // deleting token from DB:
            if (_userRepository.LogoutUser(CurrentUser.Id) == false)
                return false;

            // deleting token from disk and memory:
            if (CurrentUser.ClearData() == false)
                return false;

            return true;
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string password)
        {
            UserRegisterModel userRegisterModel = new UserRegisterModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            if (await UserExists(userRegisterModel.Email))
            {
                return false;
            }
            else
            {
                await _userRepository.CreateAsync(userRegisterModel);
                Profile profile = new Profile
                {
                    UserId = (await _userRepository.GetAvailableId()) - 1,
                    DisplayName = firstName + " " + lastName,
                    Status = UserStatus.Offline,
                    // TODO: a default path for a default profile picture is needed
                    Image = "default_user_profile_picture.img"
                };
                await _profileRepository.CreateAsync(profile);
                return true;
            }
        }

        private async Task<bool> UserExists(string email)
        {
            var user = await _userRepository.ReadAsync(email);
            // email has not been used already -> user does not exist
            if (user == null)
                return false;
            return true;
        }

        public string CheckLoginConstraints(string email, string password)
        {
            if (email.Length == 0)
            {
                return "Enter e-mail";
            }
            else if (password.Length == 0)
            {
                return "Enter password";
            }
            return "";
        }

        public string CheckRegisterConstraints(string firstName, string lastName, string email, string password, string retypedPassword)
        {
            if (firstName.Length == 0 || lastName.Length == 0)
            {
                return "Enter a valid Name";
            }
            else if (
                    !Regex.IsMatch(email,
                                    @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")
                    ||
                        email.Length == 0)
            {
                return "Enter a valid e-mail";
            }
            else if (password.Length == 0 || retypedPassword.Length == 0)
            {
                return "The password cannot be empty";
            }
            else if (password != retypedPassword)
            {
                return "Passwords don't match";
            }
            else if (!Regex.IsMatch(
                                    password,
                                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$")
                )
            {
                return "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character";
            }
            return "";
        }
    }
}
