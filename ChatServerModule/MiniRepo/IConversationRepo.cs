using ChatServerModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public interface IConversationRepo
    {
        IEnumerable<int> GetConversationsParticipants(int conversationId);

        IEnumerable<Conversation> GetConversationsCloseToLocation(string location);

        void CreateConversation(Conversation conversation);

        void AddUserToConversation(int userId, int conversationId);
        
        void RemoveUserFromConversation(int userId, int conversationId);

        void AddMessageToConversation(Message message);
    }
}
