using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Domain.RepositoryContracts;
using AccountModule.Controllers;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Domain.DTO;

namespace ChatModule
{
    public class SignalRClient : IAsyncDisposable
    {
        private static SignalRClient _signalRClient;

        private HubConnection _connection;

        public event Action<Message> MessageReceived;

        public event Action<StatusModel> StatusChanged;

        public event Action<IEnumerable<Conversation>> ConversationsReceived;


        public static SignalRClient GetInstance()
        {
            if (_signalRClient == null)
            {
                _signalRClient = new SignalRClient();
                return _signalRClient;
            }
            return _signalRClient;
        }

        private SignalRClient()
        {
           // SINGLETON 
        }

        public async Task InitializeConnectionAsync(int userId, string token)
        {
            var url = @"http://localhost:5000/chat";

            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Headers.Add("userId", userId.ToString());
                    options.Headers.Add("loginToken", token);
                })
                .Build();

            await _connection.StartAsync();

            _connection.On<Message>("ReceiveMessage", (message) => {
                var conversation = ApplicationUserController
                        .CurrentUser
                        .Conversations
                        .FirstOrDefault(c => c.Id == message.ConversationId);
                if (conversation == null)
                {
                    return;
                }
                conversation.Messages.Add(message);
                MessageReceived?.Invoke(message);
            });


            _connection.On<StatusModel>("ChangeStatus", (status) =>
            {
                var friend = ApplicationUserController.CurrentUser.Friends.Where(f => f.Id == status.FriendId).FirstOrDefault();
                if(friend != null)
                {
                    friend.Profile.Status = status.NewStatus;
                    StatusChanged?.Invoke(status);
                }
            });

            _connection.On("UpdateProximityChats", async () => {
                await UpdateProximityChats();
            });

            _connection.On<IEnumerable<Conversation>>("ReceiveConversations", (conversations) => {
                // add convs to current user
                ApplicationUserController.CurrentUser.Conversations.AddRange(conversations);
                // trigger the event
                ConversationsReceived?.Invoke(conversations);
            });
        }

        public async Task UpdateProximityChats()
        {
            var id = ApplicationUserController.CurrentUser.Id;
            UserLocationDTO userLocation = new UserLocationDTO
            {
                UserId = id,
            };
            await _connection.SendAsync("GetProximityConversationsList", userLocation);
        }

        public async Task SendMessageAsync(int conversationID, string textMessage)
        {
            var message = new Message()
            {
                SenderId = ApplicationUserController.CurrentUser.Id,
                ConversationId = conversationID,
                CreatedAt = DateTime.Now,
                TextMessage = textMessage
            };

            await _connection.SendAsync("SendMessage", message);
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _signalRClient = null;
        }
    }
}
