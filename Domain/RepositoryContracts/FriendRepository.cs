using Dapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'Friend' model.
    /// Corresponding to 'Friends' table.
    /// </summary>
    public class FriendRepository : GenericRepository<Friend>
    {
        public FriendRepository(): base("Friends") { }

        public async Task DeleteFriend(int userId, int friendId)
        {
            using (var connection = CreateConnection())
            {
                string sqlQueryForward = $@"DELETE FROM {TableName} WHERE userId=@UserId and friendId=@FriendId";
                string sqlQueryBackward = $@"DELETE FROM {TableName} WHERE userId=@FriendId and friendId=@UserId";

                await connection.ExecuteAsync(sqlQueryForward, new { UserId = userId, FriendId = friendId});
                await connection.ExecuteAsync(sqlQueryBackward, new { UserId = userId, FriendId = friendId});
            }
        }
    }
}
