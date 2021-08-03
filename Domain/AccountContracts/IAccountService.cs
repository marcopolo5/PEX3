using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    // TODO: update documentation
    public interface IAccountService
    {
        /// <summary>
        /// Searches and logs in the user if the credentials are valid
        /// </summary>
        /// <param name="userLoginModel">The data received from the login form</param>
        /// <returns>Returns false if wrong credentials, true otherwise - meaning the user logged in successfully</returns>
        Task<bool> Login(string email, string password, bool rememberMe);


        /// <summary>
        /// Create and insert a new user into database
        /// </summary>
        /// <param name="userRegisterModel">The data received from the registration form</param>
        /// <returns>Returns false if creating the user failed, else true - meaning registration was successful</returns>
        Task<bool> Register(string firstName, string lastName, string email, string password);


        /// <summary>
        /// Deletes all the information from ram and file about the CurrentUser
        /// </summary>
        /// <returns>True if logout was successful, false otherwise</returns>
        Task<bool> Logout();


        /// <summary>
        /// Executes a check for the purpose of data validation on the login form
        /// </summary>
        /// <param name="email">User's email in the login form</param>
        /// <param name="password">User's password in the login form</param>
        /// <returns>A string with the error message, if everyting was successfull returns an empty string</returns>
        public string CheckLoginConstraints(string email, string password);


        /// <summary>
        /// Executes a check for the purpose of data validation on the registration form
        /// </summary>
        /// <param name="firstName">User's first name in the registration form</param>
        /// <param name="lastName">User's last name in the registration form</param>
        /// <param name="email">User's email in the registration form</param>
        /// <param name="password1">User's password name in the registration form</param>
        /// <param name="password2">User's retyped password in the registration form</param>
        /// <returns>A string with the error message, if everyting was successfull returns an empty string</returns>
        public string CheckRegisterConstraints(string firstName, string lastName, string email, string password, string retypedPassword);
    }
}
