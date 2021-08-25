using ChatServerModule.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public class ConversationRepo : GenericRepo, IConversationRepo
    {
        public ConversationRepo(IConfiguration configuration)
            :base(configuration)
        {
            //empty ctor
        }

        public IEnumerable<int> GetConversationsParticipants(int conversationId)
        {
            IEnumerable<int> result;
            string sql = $"SELECT userid FROM [Group_Members] WHERE conversationid = {conversationId};";

            using (var conn = new SqlConnection(_connectionString))
            {
                result = conn.Query<int>(sql);
            }
            return result;
        }

        public Conversation GetConversation(int conversationId)
        {
            throw new NotImplementedException();
        }
    }
}
