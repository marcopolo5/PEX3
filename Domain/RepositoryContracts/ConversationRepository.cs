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
        private readonly UserRepository UserRepository;
        public ConversationRepository() : base("Conversations") {
            UserRepository = new();
        }

        public new async Task CreateAsync(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                await base.CreateAsync(conversation);
                foreach(User participant in conversation.Participants)
                {
                    string sql = $@"insert into Group_Members values({participant.Id},{conversation.Id})";
                    await connection.QueryAsync(sql);
                }
            }
        }

        public new async Task<Conversation> ReadAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var conversation = await base.ReadAsync(id);
                await BindConversationParticipants(conversation);
                await BindConversationMessages(conversation);
                return conversation;
            }
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
                var messages = await connection.QueryAsync<(int id, int conversation_id, int sender_id, string textmessage, DateTime created_at)>
                            ($"SELECT Messages.* FROM Messages INNER JOIN Conversations ON Messages.ConversationId={conversation.Id} ORDER BY createdat ASC");
                foreach (var message in messages)
                {
                    conversation.Messages.Add(new Message
                    {
                        Id = message.id,
                        SenderId = message.sender_id,
                        CreatedAt = message.created_at,
                        TextMessage = message.textmessage
                    });
                }
            }
        }

        private async Task BindConversationParticipants(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                var participants = await connection.QueryAsync<User>($"SELECT Users.* FROM Users INNER JOIN Group_Members ON Users.Id=Group_Members.UserId AND Group_Members.ConversationId={conversation.Id}");
                await UserRepository.BindUserProfilesAsync(participants);
                conversation.Participants = (List<User>)participants;
            }
        }
    }
}
