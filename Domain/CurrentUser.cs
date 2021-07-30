using Domain.HelpersContracts;
using Domain.Models;
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
        public byte[] LoginToken { get; set; }


        private readonly IAppConfiguration _appConfiguration;

        public CurrentUser(IAppConfiguration appConfiguration)
        {
            _appConfiguration = appConfiguration;

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
