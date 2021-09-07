using ChatServerModule.Models;

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
