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
    /// Data access layer class for 'FriendRequest' model. 
    /// Corresponding to 'Friend_Requests' table.
    /// </summary>
    public class FriendRequestRepository : GenericRepository<FriendRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FriendRequestRepository() : base("Friend_Requests") { }


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
