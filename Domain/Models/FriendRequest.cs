﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class FriendRequest
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
    }
}
