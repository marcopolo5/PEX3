using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public interface IConversationRepo
    {
        IEnumerable<int> GetUserIds(int conversationId);
    }
}
