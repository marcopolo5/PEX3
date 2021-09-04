namespace Domain.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Anonymity { get; set; }
        public int ProximityRadius { get; set; }

    }
}
