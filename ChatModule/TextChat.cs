using Domain.ChatContracts;
using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Domain.RepositoryContracts;
using AccountModule.Controllers;

namespace ChatModule
{
    public class TextChat : ITextChat
    {
        private HubConnection _connection;

        private readonly MessageRepository messageRepository;

        public event Action<Message> MessageReceived;

        public async Task InitializeConnectionAsync(int userId, string token)
        {
            var url = "http://localhost:5000/chat";

            _connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat", options =>
                {
                    options.Headers.Add("userId", userId.ToString());
                    options.Headers.Add("loginToken", token);
                })
                .Build();

            _connection.On<Message>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));

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

            await messageRepository.CreateAsync(message);

            await _connection.SendAsync("SendMessage", message);
        }

    }
}
