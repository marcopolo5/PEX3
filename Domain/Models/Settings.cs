using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class Settings
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool Anonymity { get; set; }

    }
}
