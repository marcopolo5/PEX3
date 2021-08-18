using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.ChatContracts
{
    public interface IChatController
    {
        bool CreatePrivateConversation(int userIdOne, int userIdTwo);
        bool CreateGroupConversation(int userId, string conversationTitle);
        bool AddUserToGroupConversation(int conversationId, int userId);
    }
}
