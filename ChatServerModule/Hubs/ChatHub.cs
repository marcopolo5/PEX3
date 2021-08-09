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
        public ChatHub(ITokenValidator tokenValidator, IConversationRepo conversationRepo)
        {
            _tokenValidator = tokenValidator;
            _conversationRepo = conversationRepo;
        }

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
            await base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // get the userId
            var userId = ConnectedUsers.FirstOrDefault(cd => cd.Value == Context.ConnectionId).Key;
            // remove user from the connected user lists
            ConnectedUsers.Remove(userId, out string _); //discarding the out param
            return base.OnDisconnectedAsync(exception);
        }

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

    }
}
