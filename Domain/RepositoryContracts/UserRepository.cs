using Dapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'User' and 'UserLoginModel' entities. 
    /// Corresponding to 'Users' table.
    /// </summary>
    public class UserRepository : GenericRepository<User>
    {
        private readonly ProfileRepository ProfileRepository = new();

        /// <summary>
        /// Constructor
        /// </summary>
        public UserRepository() : base("Users") { }


        /// <summary>
        /// Parameterized constructor. Used in DAL.Tests project.
        /// </summary>
        /// <param name="tablename">Database's table name</param>
        /// <param name="connectionstring">Database's connection string</param>
        public UserRepository(string tablename, string connectionstring) : base(tablename, connectionstring) { }


        /// <summary>
        /// Async method. Inserts a user into the database.
        /// </summary>
        /// <param name="user">User type entity to be inserted into the database</param>
        [Obsolete("Call CreateAsync(UserRegisterModel user) instead")]
        public new async Task CreateAsync(User user)
        {
            await base.CreateAsync(user);
        }


        /// <summary>
        /// Async method. Inserts a user into the database using spRegisterUser stored procedure
        /// </summary>
        /// <param name="user">UserRegisterModel type entity to be added into the database</param>
        public async Task CreateAsync(UserRegisterModel user)
        {
            using (var connection = CreateConnection())
            {
                var u = new UserRegisterModel
                {
                    FirstName=user.FirstName,
                    LastName=user.LastName,
                    Email=user.Email,
                    Password=user.Password
                };
                string query = @"exec spRegisterUser @Email, @Password, @FirstName, @LastName";
                await connection.QueryAsync(query, u);
            }
        }


        /// <summary>
        /// Async method. Reads all users from the database.
        /// Maps their corresponding profiles.
        /// </summary>
        /// <returns>All users</returns>
        public new async Task<IEnumerable<User>> ReadAllAsync()
        {
            string sql = @"select Users.*, Profiles.*
                          from Users left join Profiles
                            on Users.Id=Profiles.UserId";
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User, Profile, User>(sql,
                    (user, profile) => { user.Profile = profile; return user; });
                return users;
            }
        }


        /// <summary>
        /// Async method. Reads all users from the database with 'displayname' as substring
        /// in their profile DisplayName.
        /// </summary>
        /// <param name="displayname">Name to search for</param>
        /// <returns>All users with 'displayname' as substring</returns>
        public async Task<IEnumerable<User>> ReadAllAsync(string displayname)
        {
            string sql = $@"select Users.*, Profiles.*
                        from Users inner join Profiles
                        on Users.Id=Profiles.UserId and Profiles.DisplayName like '%{displayname}%'";
            using (var connection = CreateConnection())
            {
                var users = await connection.QueryAsync<User, Profile, User>(sql,
                    (user, profile) => { user.Profile = profile; return user; });
                return users;
            }
        }


        /// <summary>
        /// Async method. Reads a single user from the database with 'email'
        /// as their Email address.
        /// </summary>
        /// <param name="email">Email to search for</param>
        /// <returns>The user with specified email</returns>
        /// <remarks>KeyNotFoundException is thrown if no such user exists</remarks>
        public async Task<User> ReadAsync(string email)
        {
            using (var connection = CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<User>($"SELECT * FROM {TableName} WHERE {TableName}.Email='{email}'");
                if (entity == null)
                    throw new KeyNotFoundException($"User with email {email} was not found.");
                return entity;
            }
        }


        /// <summary>
        /// Checks if an user with userLoginModel's credentials exists
        /// </summary>
        /// <param name="userLoginModel">reference entity to look for in Database's 'Users' table</param>
        /// <returns>User's login token: "0" if login false, valid token otherwise.</returns>
        /// TOOD: Fix async
        public async Task<(int,string)> ValidateCredentials(UserLoginModel userLoginModel)
        {
            using (var connection = CreateConnection())
            {
                string sqlLogIn = $@"exec spLoginUser @Email, @Password";
                string sqlGetId = $@"select id from Users where Users.Email=@Email";
                return (
                    connection.QueryFirstOrDefaultAsync<int>(sqlGetId, new { userLoginModel.Email }).Result,
                    connection.QueryFirstOrDefaultAsync<string>(sqlLogIn, new { userLoginModel.Email, userLoginModel.Password }).Result
                    );
            }
        }


        /// <summary>
        /// Reads an user by it's ID. Maps CurrentUser's attributes.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>CurrentUser entity</returns>
        /// TODO: Fix asnync
        public async Task<CurrentUser> ReadCurrentUserAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                string sql = $@"select Users.*,Profiles.*,Settings.* from Users 
                                 inner join Profiles on Profiles.UserId=@Id 
                                  inner join Settings on Settings.UserId=@Id";
                //select FriendId from Friends where UserId={id};
                //select SenderId from Friend_Requests where ReceiverId={id};
                //select BlockedUserId from Blocked_Users where UserId={id};
                //select ConversationId from Group_Members where UserId={id}";
                //TODO: map all attritubtes. soon™

                var currentUser = connection.QueryAsync<CurrentUser, Profile, Settings, CurrentUser>(sql,
                    (user, profile, settings) => { user.Profile = profile; user.Settings = settings; return user; }, new { Id = id }).Result.First();
                return currentUser;
            }
        }

        /// <summary>
        /// Async user-profile mapper
        /// </summary>
        /// <param name="users">IEnumerable of users to be mapped</param>
        public async Task BindUserProfilesAsync(IEnumerable<User> users) {
            var profiles = await ProfileRepository.ReadAllAsync();
            foreach(User user in users)
            {
                user.Profile = profiles.FirstOrDefault(profile => profile.UserId == user.Id);
            }
        }

        /// <summary>
        /// Logs out the user by resetting his/her token to 0
        /// </summary>
        /// <param name="id">User's id</param>
        /// <returns>True if logout is successful, false otherwise</returns>
        public bool LogoutUser(int id)
        {
            string queryString = "UPDATE [Users] SET token='0' WHERE id=" + id;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(queryString, conn);
                int result = cmd.ExecuteNonQuery();
                if (result == 0) // Query execution failed
                {
                    return false;
                }
            }
            return true;
        }
    }
}
