using System;
using System.ComponentModel;

namespace Domain.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ConversationId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string TextMessage { get; set; }

        [Description("ignore")]
        public string UsersDisplayName { get; set; }
    }
}
