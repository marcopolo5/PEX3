using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; } 
        public string TextMessage { get; set; }
    }
}
