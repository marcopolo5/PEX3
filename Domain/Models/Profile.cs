namespace Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public UserStatus Status { get; set; }
        public byte[] Image { get; set; }
        public int Reputation { get; set; } = 0;
        public string StatusMessage { get; set; }
    }
}
