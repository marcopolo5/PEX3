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
        Task SendMessageAsync(int conversationID, string textMessage);
        Task InitializeConnectionAsync(int userId, string token);

        ///// todo: create some kinda of event
    }
}
