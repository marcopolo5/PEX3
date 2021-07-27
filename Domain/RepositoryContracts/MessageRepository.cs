using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'Message' model. 
    /// Corresponding to 'Messages' table.
    /// </summary>
    class MessageRepository : GenericRepository<Message> { 
    
        /// <summary>
        /// Constructor
        /// </summary>
        public MessageRepository() : base("Messages") { }


        /// <summary>
        /// Async method. Reads all messages from the database.
        /// </summary>
        /// <returns>IEnumerable of messages</returns>
        public new async Task<IEnumerable<Message>> ReadAllAsync()
        {
            using (var connection = CreateConnection()) {
                List<Message> messages_result = new();
                var messages = await connection.QueryAsync<(int id, int conversation_id, int sender_id, int receiver_id, string message, DateTime created_at)>
                    ($"SELECT * FROM Messages ORDER BY Created_At ASC");
                foreach(var message in messages)
                {
                    messages_result.Append(new Message
                    {
                        Id = message.id,
                        SenderId = message.sender_id,
                        CreatedAt = message.created_at,
                        TextMessage = message.message
                    });
                }
                return messages_result;
            }
        }
    }
}
