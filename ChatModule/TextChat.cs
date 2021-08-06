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

        public async Task InitializeConnection(int userId, string token)
        {
            var url = "10.0.2.162:5002"; 

            _connection = new HubConnectionBuilder()
                .WithUrl($"http://{url}/chat?id={userId}&loginToken={token}")
                .Build();

            await _connection.StartAsync();
            _connection.On<Message>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));
        }

        public async Task SendMessage(Message message)
        {
            await _connection.SendAsync("SendMessage", message);
        }
    }
}
