using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public class ConversationRepo : IConversationRepo
    {

        private readonly string _connectionString;
        public ConversationRepo()
        {
        }
        public IEnumerable<int> GetUserIds(int conversationId)
        {
            IEnumerable<int> result;
            string sql = $"SELECT userid FROM Group_Members WHERE conversationid = {conversationId};";

            using (var conn = new SqlConnection(_connectionString))
            {
                result = conn.Query<int>(sql);
            }
            return result;
        }
    }
}
