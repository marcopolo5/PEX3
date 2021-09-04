using System;

namespace Domain.Models
{
    public class Strikes
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool FirstStrike { get; set; }
        public bool SecondStrike { get; set; }
        public bool ThirdStrike { get; set; }
        public DateTime? UnbanDate { get; set; }
    }
}
