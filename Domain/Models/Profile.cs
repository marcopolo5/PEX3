using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string DisplayName { get; set; }
        public UserStatus Status { get; set; }
    }
}
