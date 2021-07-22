using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }

        [Description("ignore")]
        public User Sender { get; set; }
        
        [Description("ignore")]
        public Conversation Conversation { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TextMessage { get; set; }
    }
}
