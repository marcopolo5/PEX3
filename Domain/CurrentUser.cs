using Domain.HelpersContracts;
using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// A singleton class that contains the information about the current logged in user.
    /// </summary>
    public class CurrentUser : User 
    {
        public List<User> Friends { get; set; } = new();
        public List<FriendRequest> FriendRequests { get; set; } = new();
        public List<Conversation> Conversations { get; set; } = new();
        public List<User> BlockedUsers { get; set; } = new();


        public Settings Settings { get; set; }
        public string LoginToken { get; set; }
        public bool RememberMe { get; set; }


        private readonly IAppConfiguration _appConfiguration;

        public CurrentUser() { }

        public CurrentUser(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;

        }

        public void InitializeFields(Profile profile, Settings settings)
        {

            // TODO: 
            // initialize Friends
            // initialize FriendRequests
            // initialize Conversations
            // initialize BlockedUsers
            // OBS: not finished yet in UserRepository (see  UserRepository.ReadCurrentUserAsync(int id)  )
            // soon™
            Profile = profile;
            Settings = settings;
        }

        /// <summary>
        /// Resets all the fields and deletes the token from the file
        /// </summary>
        /// <returns></returns>
        public bool ClearData()
        {
            throw new NotImplementedException();
        }
    }
}
