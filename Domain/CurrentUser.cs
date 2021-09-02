using Domain.Helpers;
using Domain.HelpersContracts;
using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    /// <summary>
    /// A class that contains the information about the current logged in user.
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

        public int CurrentConversationId { get; set; }

        private readonly IAppConfiguration _appConfiguration = new AppConfiguration();
        

        /// <summary>
        /// Resets all the fields and deletes the token from the file
        /// </summary>
        /// <returns>True if the data was cleared successfuly, false otherwise</returns>
        public bool ClearData()
        {
            if (_appConfiguration.ResetToken() == false || _appConfiguration.ResetId() == false)
            {
                return false;
            }
            // TODO: reset the fields
            return true;
        }
    }
}
