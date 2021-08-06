using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ChatContracts
{
    public interface ITextChat
    {
        // maybe change Message to primitive types
        Task SendMessageAsync(Message message);
        Task InitializeConnectionAsync(int userId, string token);

        event Action<Message> MessageReceived;
        ///// todo: create some kinda of event
    }
}
