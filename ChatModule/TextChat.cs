using Domain.ChatContracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule
{
    public class TextChat : ITextChat
    {
        public Task InitializeConnection(int userId)
        {
            throw new NotImplementedException();
        }

        public Task SendMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
