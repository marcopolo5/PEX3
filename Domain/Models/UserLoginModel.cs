﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class UserLoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
