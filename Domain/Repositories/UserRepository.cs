using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.Repositories
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
        public UserRepository() : base("Users")
        {
        }


        /// <summary>
        /// Parameterized constructor. Used in DAL.Tests project.
        /// </summary>
        /// <param name="tablename">Database's table name</param>
        /// <param name="connectionstring">Database's connection string</param>
        public UserRepository(string tablename, string connectionstring) : base(tablename, connectionstring)
        {
        }

        public async Task LogoutAsync(int id)
        {
            var url = "UPDATE Users SET token=0, lastupdate=@Now WHERE id=@Id";
            using (var connection = await CreateConnection())
            {
                await connection.ExecuteAsync(url, new { Now = DateTime.Now, Id = id });
            }
        }

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
            using (var connection = await CreateConnection())
            {
                var u = new UserRegisterModel
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };
                var query = @"exec spRegisterUser @Email, @Password, @FirstName, @LastName";
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
            var sql = @"select Users.*, Profiles.*
                          from Users left join Profiles
                            on Users.Id=Profiles.UserId";
            using (var connection = await CreateConnection())
            {
                var users = await connection.QueryAsync<User, Profile, User>(sql,
                    (user, profile) =>
                    {
                        user.Profile = profile;
                        return user;
                    });
                return users;
            }
        }


        public async Task<IEnumerable<User>> ReadAllWithFilterAsync(string filter)
        {
            var sqlQuery =
                $@"select * from {TableName} where Users.firstname like CONCAT('%',@Filter,'%') OR Users.lastname like CONCAT('%',@Filter,'%') OR Users.email like CONCAT('%',@Filter,'%')";
            using (var connection = await CreateConnection())
            {
                var users = await connection.QueryAsync<User>(sqlQuery, new {Filter = filter});
                return users;
            }
        }


        /// <summary>
        /// Async method. Reads a single user from the database with 'id'
        /// as their Id.
        /// </summary>
        /// <param name="id">Id to search for</param>
        /// <returns>The user with specified email</returns>
        /// <remarks>KeyNotFoundException is thrown if no such user exists</remarks>
        public new async Task<User> ReadAsync(int id)
        {
            using (var connection = await CreateConnection())
            {
                var sqlGetUser = @$"select * from {TableName} where Users.id=@Id";
                var sqlGetProfile = @"select * from Profiles where Profiles.UserId=@Id";
                var entity = await connection.QuerySingleOrDefaultAsync<User>(sqlGetUser, new {Id = id});
                entity.Profile = await connection.QuerySingleOrDefaultAsync<Profile>(sqlGetProfile, new {Id = id});
                // if (entity == null)
                //     throw new KeyNotFoundException($"User with id {id} was not found"); //TODO: Unhandled in UI
                return entity;
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
            using (var connection = await CreateConnection())
            {
                var entity =
                    await connection.QuerySingleOrDefaultAsync<User>($"SELECT * FROM {TableName} WHERE Email=@Email",
                        new {Email = email});
                // if (entity == null)
                //     throw new KeyNotFoundException($"User with email {email} was not found."); //TODO: Unhandled in UI
                return entity;
            }
        }


        /// <summary>
        /// Checks if an user with userLoginModel's credentials exists
        /// </summary>
        /// <param name="userLoginModel">reference entity to look for in Database's 'Users' table</param>
        /// <returns>Tuple of user's id and login token: (0,"0") if login false, (id,valid_token) otherwise.</returns>
        public async Task<(int, string)> ValidateCredentials(UserLoginModel userLoginModel)
        {
            using (var connection = await CreateConnection())
            {
                var sqlLogIn = @"exec spLoginUser @Email, @Password";
                var sqlGetId = @"select id from Users where Users.Email=@Email";

                //TODO: Set DB to allow multiple connection threads from the same user, then configure tasks to run in parallel
                //see https://stackoverflow.com/questions/6062192/there-is-already-an-open-datareader-associated-with-this-command-which-must-be-c

                //var idTask = connection.QueryFirstOrDefaultAsync<int>(sqlGetId, new { userLoginModel.Email });
                //var tokenTask = connection.QueryFirstOrDefaultAsync<string>(sqlLogIn, new { userLoginModel.Email, userLoginModel.Password });

                //await Task.WhenAll(idTask, tokenTask);

                //int task = await idTask;
                //string token = await tokenTask;

                return (
                    await connection.QueryFirstOrDefaultAsync<int>(sqlGetId, new {userLoginModel.Email}),
                    await connection.QueryFirstOrDefaultAsync<string>(sqlLogIn,
                        new {userLoginModel.Email, userLoginModel.Password})
                );
            }
        }


        /// <summary>
        /// Reads an user by it's ID. Maps CurrentUser's attributes.
        /// </summary>
        /// <param name="id">User's id</param>
        /// <param name="token">User's token</param>
        /// <returns>CurrentUser entity</returns>
        public async Task<CurrentUser> ReadCurrentUserAsync(int id, string token)
        {
            using (var connection = await CreateConnection())
            {
                var sqlViewUser = @"exec spViewUser @Id, @Token";
                var sqlFriends = @"select FriendId from Friends where UserId=@Id";
                var sqlFriendRequests = @"select * from Friend_Requests where ReceiverId=@Id";
                var sqlBlockedUsers = @"select BlockedUserId from Blocked_Users where UserId=@Id";
                var sqlConversations = @"select Conversations.* from Conversations 
                                            inner join Group_Members on Group_Members.ConversationId = Conversations.Id
                                            where Group_Members.userid=@Id";
                var sqlGetStrikes = @"select * from Strikes where userid=@Id";

                var currentUserArray = await connection.QueryAsync<CurrentUser, Profile, Settings, CurrentUser>(
                    sqlViewUser,
                    (user, profile, settings) =>
                    {
                        user.Profile = profile;
                        user.Settings = settings;
                        return user;
                    }, new {Id = id, Token = token});
                var currentUser = currentUserArray.FirstOrDefault();

                if (currentUser == null)
                {
                    throw new ArgumentException("Token isn't valid");
                }


                // Mapping the proximity ban
                var strikeobj = await connection.QueryFirstOrDefaultAsync<Strikes>(sqlGetStrikes, new { Id = id });
                var isBanned = false;

                if (strikeobj != null && strikeobj.UnbanDate > DateTime.Now)
                {
                    isBanned = true;
                }
                currentUser.IsBannedFromProximity = isBanned;
                //


                var friendIds = await connection.QueryAsync<int>(sqlFriends, new {Id = id});
                foreach (var friendId in friendIds)
                {
                    var friend = await ReadAsync(friendId);
                    friend.Password = null;
                    friend.Token = null;
                    currentUser.Friends.Add(friend);
                }

                var friendRequests = await connection.QueryAsync<FriendRequest>(sqlFriendRequests, new {Id = id});
                foreach (var friendRequest in friendRequests)
                {
                    currentUser.FriendRequests.Add(friendRequest);
                }

                var blockedUsersIds = await connection.QueryAsync<int>(sqlBlockedUsers, new {Id = id});
                foreach (var blockedUserId in blockedUsersIds)
                {
                    var blockedUser = await ReadAsync(blockedUserId);
                    blockedUser.Password = null;
                    blockedUser.Token = null;
                    currentUser.BlockedUsers.Add(blockedUser);
                }

                var conversations = await connection.QueryAsync<Conversation>(sqlConversations, new {Id = id});
                foreach (var conversation in conversations)
                {
                    //Map messages
                    var messages = await connection
                        .QueryAsync<(int id, int conversation_id, int sender_id, string textmessage, DateTime created_at
                            )>
                        (@"select Messages.* from Messages inner join Conversations on Messages.ConversationId = Conversations.id WHERE Conversations.id = @Id ORDER BY createdat ASC",
                            new {Id = conversation.Id});
                    foreach (var message in messages)
                    {
                        conversation.Messages.Add(new Message
                        {
                            Id = message.id,
                            SenderId = message.sender_id,
                            CreatedAt = message.created_at,
                            TextMessage = message.textmessage
                        });
                    }

                    //Map participants
                    var participants = await connection.QueryAsync<User>(
                        "SELECT Users.* FROM Users INNER JOIN Group_Members ON Users.Id=Group_Members.UserId AND Group_Members.ConversationId=@Id",
                        new {Id = conversation.Id});
                    await MapUserProfilesAsync(participants);
                    conversation.Participants = (List<User>) participants;

                    currentUser.Conversations.Add(conversation);
                }

                return currentUser;
            }
        }


        /// <summary>
        /// Async user-profile mapper
        /// </summary>
        /// <param name="users">IEnumerable of users to be mapped</param>
        private async Task MapUserProfilesAsync(IEnumerable<User> users)
        {
            var profiles = await ProfileRepository.ReadAllAsync();
            foreach (var user in users)
            {
                user.Profile = profiles.FirstOrDefault(profile => profile.UserId == user.Id);
            }
        }
    }
}