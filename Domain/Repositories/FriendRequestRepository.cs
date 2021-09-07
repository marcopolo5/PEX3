using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.Repositories
{
    /// <summary>
    /// Data access layer class for 'FriendRequest' model. 
    /// Corresponding to 'Friend_Requests' table.
    /// </summary>
    public class FriendRequestRepository : GenericRepository<FriendRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FriendRequestRepository() : base("Friend_Requests") { }

        public async Task<IEnumerable<FriendRequest>> ReadAsyncByReceiverId(int receiverId)
        {
            var sqlQuery = $@"select * from {TableName} where Friend_Request.receiverid=@ReceiverId";
            using(var connection = await CreateConnection())
            {
                var friendRequests = await connection.QueryAsync<FriendRequest>(sqlQuery, new { ReceiverId = receiverId});
                return friendRequests;
            }
        }

        public async Task<FriendRequest> FriendRequestExists(int senderId, int receiverId)
        {
            using(var connection = await CreateConnection())
            {
                var sqlQuery = $@"SELECT * FROM {TableName} WHERE senderId=@SenderId and receiverId=@ReceiverId";
                var entity = await connection.QuerySingleOrDefaultAsync<FriendRequest>(sqlQuery, new { SenderId = senderId, ReceiverId = receiverId });
                return entity;
            }
        }

        public async Task<FriendRequest> ReadAsync(int senderId, int receiverId)
        {
            using (var connection = await CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<FriendRequest>($"SELECT * FROM {TableName} WHERE SenderId=@SenderId AND ReceiverID=@ReceiverId", new { SenderId = senderId, ReceiverId = receiverId });
                return entity;
            }
        }

    }
}
