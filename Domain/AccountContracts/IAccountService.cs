using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.AccountContracts
{
    public interface IAccountService
    {
        /// <summary>
        /// Searches and logs in the user if the credentials are valid
        /// </summary>
        /// <param name="userLoginModel">The data received from the login form</param>
        /// <returns>Returns false if wrong credentials, true otherwise - meaning the user logged in successfully</returns>
        bool Login(UserLoginModel userLoginModel);


        /// <summary>
        /// Create and insert a new user into database
        /// </summary>
        /// <param name="userRegisterModel">The data received from the registration form</param>
        /// <returns>Returns false if creating the user failed, else true - meaning registration was successful</returns>
        bool Register(UserRegisterModel userRegisterModel);


        /// <summary>
        /// Deletes all the information from ram and file about the CurrentUser
        /// </summary>
        /// <returns>True if logout was successful, false otherwise</returns>
        bool Logout();
    }
}
