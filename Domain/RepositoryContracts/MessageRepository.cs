using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.RepositoryContracts
{
    class MessageRepository:GenericRepository<Message>
    {
        private readonly UserRepository UserRepository;
        private readonly ConversationRepository ConversationRepository;
        public MessageRepository() : base("Messages") {
            UserRepository = new();
            ConversationRepository = new();
        }

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
                        Sender = await UserRepository.ReadAsync(message.sender_id),
                        Conversation = await ConversationRepository.ReadAsync(message.conversation_id),
                        CreatedAt = message.created_at,
                        TextMessage = message.message
                    });
                }
                return messages_result;
            }
        }
    }
}
