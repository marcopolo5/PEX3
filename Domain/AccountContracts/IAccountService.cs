using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AccountContracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Tries to login the user. If login is successful it'll save the user's token to CurrentUser. 
        /// If remember_me is true it'll save a copy of the token in a local file
        /// </summary>
        /// <param name="userToLogInModel">Contains the details about the user(email/password/remember_me)</param>
        /// <returns>True if login was successful, false otherwise.</returns>
        bool Login(UserLoginModel userLoginModel);


        /// <summary>
        /// This method will create a new user and it'll add it to the DB
        /// </summary>
        /// <param name="userRegisterModel">User to be created</param>
        /// <returns>True if creation was successful, false otherwise</returns>
        bool Register(UserRegisterModel userRegisterModel);


        /// <summary>
        /// Deletes all the information about the CurrentUser
        /// </summary>
        /// <returns>True if logout was successful, false otherwise</returns>
        bool Logout();
    }
}
