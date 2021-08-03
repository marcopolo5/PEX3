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
        private static readonly ConcurrentDictionary<int, string> ConnectedUsers = new();
        private readonly ITokenValidator _tokenValidator;

        public ChatHub(ITokenValidator tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }

        public override Task OnConnectedAsync()
        {
            // Client side:
            //var connection = new HubConnectionBuilder()
            //.WithUrl($"http://10.0.2.162:5002/connection?id={id}")
            //.WithConsoleLogger()
            //.WithMessagePackProtocol()
            //.WithTransport(TransportType.WebSockets)
            //.Build();

            string stringId = Context.
                GetHttpContext()
                .Request
                .Query["id"]
                .ToString();

            if(int.TryParse(stringId, out int id))
                ConnectedUsers[id] = Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // get the userId
            var userId = ConnectedUsers.FirstOrDefault(cd => cd.Value == Context.ConnectionId).Key;
            // remove user from the connected user lists
            ConnectedUsers.Remove(userId, out string _); //discarding the out param
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Message message, string token)
        {
            if (_tokenValidator.IsValid(token) == false)
            {
                return; // if token isnt valid dont send the message
            }
            foreach(var userId in message.ListOfReceivers)
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
