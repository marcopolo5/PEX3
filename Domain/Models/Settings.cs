using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public User User { get; set; }
        public bool IsAnonymous { get; set; }

    }
}
