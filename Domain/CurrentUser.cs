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

        private readonly ITokenFileSaver _tokenFileSaver;
        public CurrentUser(ITokenFileSaver tokenFileSaver)
        {
            _tokenFileSaver = tokenFileSaver;
        }

    }
}
