using ChatServerModule.Hubs;
using ChatServerModule.MiniRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.Mocks
{
    public class FakeConversationRepo : IConversationRepo
    {
        public IEnumerable<int> GetUserIds(int conversationId)
        {
            foreach (var userId in ChatHub.ConnectedUsers.Keys)
            {
                yield return userId;
            }
        }
    }
}
