using Domain.ChatContracts;
using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Domain.RepositoryContracts;
using AccountModule.Controllers;
using System.Linq;

namespace ChatModule
{
    public class TextChat : ITextChat
    {
        private HubConnection _connection;

        private readonly MessageRepository _messageRepository = new();

        public event Action<Message> MessageReceived;

        public event Action<StatusModel> StatusChanged;

        public async Task InitializeConnectionAsync(int userId, string token)
        {
            var url = "http://localhost:5000/chat";

            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Headers.Add("userId", userId.ToString());
                    options.Headers.Add("loginToken", token);
                })
                .Build();

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
                if(friend!=null)
                {
                    friend.Profile.Status = status.NewStatus;
                    StatusChanged?.Invoke(status);
                }
            });

            await _connection.StartAsync();
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

            await _messageRepository.CreateAsync(message);

            await _connection.SendAsync("SendMessage", message);
        }

    }
}
