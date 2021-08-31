using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Domain.DTO;

namespace Domain.Models
{

    /// <summary>
    /// The difference between this model and the database table is that this model doesn't include the password/passwordHash or token
    /// These will be stored in the CurrentUserModel.
    /// </summary>
    public class User
    {
        // Basic info:
        public int Id { get; set; }

        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Password { get; set; }
        public string Email { get; set; }
        
        public bool Verified { get; set; }

        public DateTime JoinDate { get; set; }

        public DateTime LastUpdate { get; set; }
 
        [Description("ignore")]
        public Profile Profile { get; set; }

        public string? Token { get; set; }
    }
}
