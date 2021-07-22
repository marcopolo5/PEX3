using Dapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    public class ConversationRepository : GenericRepository<Conversation>
    {
        private readonly MessageRepository MessageRepository;
        private readonly UserRepository UserRepository;
        public ConversationRepository() : base("Conversations") {
            MessageRepository = new();
            UserRepository = new();
        }

        public new async Task<IEnumerable<Conversation>> ReadAllAsync()
        {
            using (var connection = CreateConnection())
            {
                var conversations = await base.ReadAllAsync();
                foreach(Conversation conversation in conversations)
                {
                    await BindConversationParticipants(conversation);
                    await BindConversationMessages(conversation);
                }
                return conversations;
            }
        }

        private async Task BindConversationMessages(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                var messages = await connection.QueryAsync<(int id, int conversation_id, int sender_id, int receiver_id, string message, DateTime created_at)>
                            ($"SELECT Messages.* FROM Messages INNER JOIN Conversations ON Messages.Conversation_Id={conversation.Id} ORDER BY created_at ASC");
                foreach (var message in messages)
                {
                    conversation.Messages.Append(new Message
                    {
                        Id = message.id,
                        Sender = await UserRepository.ReadAsync(message.sender_id),
                        Conversation = conversation,
                        CreatedAt = message.created_at,
                        TextMessage = message.message
                    });
                }
            }
        }

        private async Task BindConversationParticipants(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                var participants = await connection.QueryAsync<User>($"SELECT Users.* FROM Users INNER JOIN Group_Members ON User.Id=Group_Members.User_Id AND Group_Members.Conversation_Id={conversation.Id}");
                await UserRepository.BindUserProfilesAsync(participants);
                conversation.Participants = participants;
            }
        }
    }
}
