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
        private readonly HubConnection connection;
        public event Action<Message> MessageReceived;

        public TextChat(HubConnection _connection)
        {
            connection = _connection;
            connection.On<Message>("ReceiveMessage", (message) => MessageReceived?.Invoke(message));
        }

        public async Task InitializeConnection(int userId)
        {
            await connection.StartAsync();
        }

        public async Task SendMessage(Message message, string token)
        {
            await connection.SendAsync("SendMessage", message, token);
        }
    }
}
