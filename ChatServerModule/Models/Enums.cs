using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServerModule.Models
{
    /// <summary>
    /// User status:
    /// -online
    /// -away
    /// -offline
    /// </summary>
    public enum UserStatus
    {
        Offline,
        Away,
        Online
    }

    /// <summary>
    /// Conversation types:
    /// -private
    /// -group
    /// -proximity group
    /// </summary>
    public enum ConversationTypes
    {
        Private,
        Group,
        ProximityGroup
    }
}
