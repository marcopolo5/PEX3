using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerModule.Models
{
    public class StatusModel
    {
        public int FriendId { get; set; }
        public UserStatus NewStatus { get; set; }
    }
}

