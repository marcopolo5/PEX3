using ChatServerModule.Hubs;
using ChatServerModule.MiniRepo;
using ChatServerModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.Mocks
{
    public class FakeConversationRepo : IConversationRepo
    {
        public Conversation GetConversation(int conversationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> GetConversationsParticipants(int conversationId)
        {
            foreach (var userId in ChatHub.ConnectedUsers.Keys)
            {
                yield return userId;
            }
        }
    }
}
