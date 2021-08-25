using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ChatServerModule.Models
{
    public class Conversation
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Title { get; set; }
        public ConversationTypes Type { get; set; }

        [Description("ignore")]
        public List<int> Participants { get; set; } = new();

        [Description("ignore")]
        public List<Message> Messages { get; set; } = new();
    }
}
