﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int FriendId { get; set; }
    }
}
