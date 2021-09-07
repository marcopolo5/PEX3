using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.Repositories
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
            using (var connection = await CreateConnection())
            {
                var sqlQueryForward = $@"DELETE FROM {TableName} WHERE userId=@UserId and friendId=@FriendId";
                var sqlQueryBackward = $@"DELETE FROM {TableName} WHERE userId=@FriendId and friendId=@UserId";

                await connection.ExecuteAsync(sqlQueryForward, new { UserId = userId, FriendId = friendId});
                await connection.ExecuteAsync(sqlQueryBackward, new { UserId = userId, FriendId = friendId});
            }
        }

        public async Task<bool> FriendshipExists(int userId, int friendId)
        {
            using (var connection = await CreateConnection())
            {
                var sqlQuery = $@"SELECT * FROM {TableName} WHERE userId=@UserId and friendId=@FriendId";
                return await connection.QueryFirstOrDefaultAsync<bool>(sqlQuery, new { UserId = userId, FriendId = friendId });
            }
        }

    }
}
