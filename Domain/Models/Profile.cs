using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public UserStatus Status { get; set; }
        public string Image { get; set; }
        public int Reputation { get; set; } = 0;
    }
}
