using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface IApplicationUserController
    {
        /// <summary>
        /// Searches and logs in the user if the credentials are valid
        /// </summary>
        /// <param name="email">User's email in the login form</param>
        /// <param name="password">User's password in the login form</param>
        /// <param name="rememberMe">remember me feature (data retrieved from checkbox value)</param>
        /// <returns>true if credentials are valid, false otherwise</returns>
        Task<bool> Login(string email, string password, bool rememberMe);


        /// <summary>
        /// Create and insert a new user into database
        /// </summary>
        /// <param name="firstName">User's first name in the signup form</param>
        /// <param name="lastName">User's last name in the signup form</param>
        /// <param name="email">User's email in the signup form</param>
        /// <param name="password">Users password in the signup form</param>
        /// <returns>true if sign-up was successful, false otherwise</returns>
        Task<bool> Register(string firstName, string lastName, string email, string password);


        /// <summary>
        /// Deletes all the information from ram and file about the CurrentUser
        /// </summary>
        /// <returns>True if logout was successful, false otherwise</returns>
        Task<bool> Logout();

        /// <summary>
        /// Searches into database and check if the user corresponding to the email given exists
        /// </summary>
        /// <param name="email">email of the users that is going to be searched</param>
        /// <returns>true if the user is found into database, false otherwise</returns>
        Task<bool> UserExists(string email);

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

        /// <summary>
        /// Check if the password matches the security requirements
        /// </summary>
        /// <param name="password"></param>
        /// <param name="retypedPassword"></param>
        /// <returns>A string with the error message, if everyting was successfull returns an empty string</returns>
        public string CheckPasswordConstraints(string password, string retypedPassword);

        /// <summary>
        /// Check if remember me was active and checks if the token saved in the file matches the one in the DB
        /// </summary>
        /// <returns>Returns true if the token saved in the file matches the one in the DB, false otherwise</returns>
        Task<bool> CheckIfUserIsLoggedIn();

        /// <summary>
        /// Update the current user's information
        /// </summary>
        /// <returns>A task</returns>
        Task UpdateCurrentUserInformation();

        /// <summary>
        /// Registers (if necessary) and logs in an user
        /// with their Google account data. (email, first name, last name)
        /// </summary>
        /// <param name="rememberMe">Remember me option</param>
        Task AuthenticateWithGoogle(bool rememberMe);

    }
}
