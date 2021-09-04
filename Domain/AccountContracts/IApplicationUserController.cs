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
        Task Login(string email, string password, bool rememberMe);


        /// <summary>
        /// Create and insert a new user into database
        /// </summary>
        /// <param name="firstName">User's first name in the signup form</param>
        /// <param name="lastName">User's last name in the signup form</param>
        /// <param name="email">User's email in the signup form</param>
        /// <param name="password">Users password in the signup form</param>
        Task Register(string firstName, string lastName, string email, string password, string retypedPasword);


        /// <summary>
        /// Deletes all the information from ram and file about the CurrentUser
        /// </summary>
        /// <returns>True if logout was successful, false otherwise</returns>
        Task<bool> Logout();


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
