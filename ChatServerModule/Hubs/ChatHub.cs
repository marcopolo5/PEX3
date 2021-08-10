using ChatServerModule.MiniRepo;
using ChatServerModule.Models;
using ChatServerModule.TokenValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.Hubs
{
    public class ChatHub : Hub
    {

        /// <summary>
        /// key - user id 
        /// value - connection
        /// </summary>
        public static readonly ConcurrentDictionary<int, string> ConnectedUsers = new();

        private readonly ITokenValidator _tokenValidator;
        private readonly IConversationRepo _conversationRepo;
        private readonly IUsersRepo _usersRepo;

        public ChatHub(ITokenValidator tokenValidator, 
            IConversationRepo conversationRepo,
            IUsersRepo usersRepo)
        {
            _tokenValidator = tokenValidator;
            _conversationRepo = conversationRepo;
            _usersRepo = usersRepo;
        }

        /// <summary>
        /// Called when user connects to the server
        /// </summary>
        /// <returns>A task</returns>
        public override async Task OnConnectedAsync()
        {
            string stringId = Context
                .GetHttpContext()
                .Request
                .Headers["userId"];

            string token = Context
                .GetHttpContext()
                .Request
                .Headers["loginToken"];

            if (int.TryParse(stringId, out int id) == false)
            {
                return;
            }
            
            if (_tokenValidator.IsValid(id, token) == false)
            {
                return;
            }

            ConnectedUsers[id] = Context.ConnectionId;
            // update status in DB
            _usersRepo.ChangeUserStatus(id, UserStatus.Online);

            // update status for friends
            await UpdateStatus(id, UserStatus.Online);

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when user disconnects from the server
        /// </summary>
        /// <param name="exception">An exception</param>
        /// <returns>A task</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // get the userId
            var userId = ConnectedUsers.FirstOrDefault(cd => cd.Value == Context.ConnectionId).Key;

            // update user status in DB:
            _usersRepo.ChangeUserStatus(userId, UserStatus.Online);

            // update status for friends
            await UpdateStatus(userId, UserStatus.Offline);

            // remove user from the connected user lists
            ConnectedUsers.Remove(userId, out string _); //discarding the out param
            await OnDisconnectedAsync(exception);
        }


        /// <summary>
        /// This method can be called from client to send a message
        /// </summary>
        /// <param name="message"> Message to be sent</param>
        /// <returns>A task</returns>
        public async Task SendMessage(Message message)
        {
            IEnumerable<int> userIds = _conversationRepo.GetUserIds(message.ConversationId);

            foreach(var userId in userIds)
            {
                // check if the user is connected
                if (ConnectedUsers.ContainsKey(userId) == false)
                {
                    continue; // if not jump to the next user
                }

                var connectionId = ConnectedUsers[userId];
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }


        /// <summary>
        /// Get connection ids from all the user's friends(if they are connected) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns>Returns a list of connection ids</returns>
        private List<string> GetFriendsConnectionIds(int currentUser)
        {
            var friendsIds = _usersRepo.GetFriendsIds(currentUser);
            var result = new List<string>();
            foreach (var id in friendsIds)
            {
                if (ConnectedUsers.TryGetValue(id, out string connectionId))
                {
                    result.Add(connectionId);
                }
            }
            return result;
        }


        /// <summary>
        /// Update this user status for all connected friends
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="newStatus">New status</param>
        /// <returns>A task</returns>
        private async Task UpdateStatus(int userId, UserStatus newStatus)
        {            
            // create a status model
            var statusModel = new StatusModel
            {
                FriendId = userId,
                NewStatus = newStatus
            };

            // get all online friends
            IReadOnlyList<string> allFriendsConnection = GetFriendsConnectionIds(userId);

            // if there's at least one friend connected call ChangeStatus on client
            if (allFriendsConnection.Count > 0)
            {
                await Clients.Clients(allFriendsConnection).SendAsync("ChangeStatus", statusModel);
            }
        }
    }
}
