using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public User Sender { get; set; }
        public Conversation Conversation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TextMessage { get; set; }
    }
}
