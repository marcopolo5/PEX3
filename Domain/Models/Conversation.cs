using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    /// <summary>
    /// Includes basic informations about the chat, it doesnt include the messages.
    /// </summary>
    public class Conversation
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Title { get; set; }
        public ConversationTypes Type { get; set; }

        public IEnumerable<User> Participants { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
}
