using Domain.ChatContracts;
using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;

namespace ChatModule
{
    public class TextChat : ITextChat
    {
        private HubConnection _connection;
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

        public async Task SendMessageAsync(Message message)
        {
            await _connection.SendAsync("SendMessage", message);
        }
    }
}
