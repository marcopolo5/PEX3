using Dapper;
using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
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
            using(var connection = CreateConnection())
            {
                var friendRequests = await connection.QueryAsync<FriendRequest>(sqlQuery, new { ReceiverId = receiverId});
                return friendRequests;
            }
        }


        public async Task<FriendRequest> ReadAsync(int senderId, int receiverId)
        {
            using (var connection = CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<FriendRequest>($"SELECT * FROM {TableName} WHERE SenderId=@SenderId AND ReceiverID=@ReceiverId", new { SenderId = senderId, ReceiverId = receiverId });
                return entity;
            }
        }

    }
}
