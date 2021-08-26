using ChatServerModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.DTOs
{
    public class ConversationCreateDTO
    {
        public int CreatorsId { get; set; }

        public string Title { get; set; }

        public ConversationTypes Type { get; set; }

        public string Location { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
