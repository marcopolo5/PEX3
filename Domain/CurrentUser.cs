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
        public IEnumerable<User> Friends { get; set; } = new List<User>();
        public IEnumerable<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();
        public IEnumerable<Conversation> Conversations { get; set; } = new List<Conversation>();
        public IEnumerable<User> BlockedUsers { get; set; } = new List<User>();

        public Settings Settings { get; set; }
        public byte[] LoginToken { get; set; }

        private readonly ITokenFileSaver _tokenFileSaver;
        public CurrentUser(ITokenFileSaver tokenFileSaver)
        {
            _tokenFileSaver = tokenFileSaver;
        }

    }
}
