using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string TextMessage { get; set; }
    }
}
