using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ServerConversationDTO
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string Title { get; set; }

        public ConversationTypes Type { get; set; }

        public string Location { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        [Description("ignore")]
        public List<int> Participants { get; set; } = new();

        [Description("ignore")]
        public List<Message> Messages { get; set; } = new();

    }
}
