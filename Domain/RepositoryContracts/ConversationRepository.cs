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
    /// Data access layer class for 'Conversation' model. 
    /// Corresponding to 'Conversations' table.
    /// </summary>
    public class ConversationRepository : GenericRepository<Conversation>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public ConversationRepository() : base("Conversations")
        {
            UserRepository = new();
        }


        /// <summary>
        /// Parameterized constructor. Used in DAL.Tests project.
        /// </summary>
        /// <param name="tablename">Database's table name</param>
        /// <param name="connectionstring">Database's connection string</param>
        public ConversationRepository(string tablename, string connectionstring) : base(tablename, connectionstring) { }


        /// <summary>
        /// Async method. Inserts a Conversation object into the database.
        /// </summary>
        /// <param name="conversation">Conversation to be inserted</param>
        public new async Task CreateAsync(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                await base.CreateAsync(conversation);
                foreach (User participant in conversation.Participants)
                {
                    string sql = $@"insert into Group_Members values({participant.Id},{conversation.Id})";
                    await connection.QueryAsync(sql);
                }
            }
        }


        /// <summary>
        /// Async method.
        /// Reads a conversation from the database.
        /// Maps it's participants and messages
        /// </summary>
        /// <param name="id">Conversation's ID</param>
        /// <returns>Read conversation</returns>
        public new async Task<Conversation> ReadAsync(int id)
        {
            using (var connection = CreateConnection())
            {
                var conversation = await base.ReadAsync(id);
                await MapConversationParticipants(conversation);
                await MapConversationMessages(conversation);
                return conversation;
            }
        }


        /// <summary>
        /// Async method. 
        /// Reads all conversations from the database.
        /// Maps it's participants and messages.
        /// </summary>
        /// <returns>All conversations</returns>
        public new async Task<IEnumerable<Conversation>> ReadAllAsync()
        {
            using (var connection = CreateConnection())
            {
                var conversations = await base.ReadAllAsync();
                foreach (Conversation conversation in conversations)
                {
                    await MapConversationParticipants(conversation);
                    await MapConversationMessages(conversation);
                }
                return conversations;
            }
        }


        /// <summary>
        /// Async method.
        /// Maps conversation's messages to itself.
        /// </summary>
        /// <param name="conversation">Conversation to be mapped</param>
        /// <returns></returns>
        private async Task MapConversationMessages(Conversation conversation)
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


        /// <summary>
        /// Async method.
        /// Maps a conversation's participants to itself.
        /// </summary>
        /// <param name="conversation"></param>
        /// <returns></returns>
        private async Task MapConversationParticipants(Conversation conversation)
        {
            using (var connection = CreateConnection())
            {
                var participants = await connection.QueryAsync<User>($"SELECT Users.* FROM Users INNER JOIN Group_Members ON Users.Id=Group_Members.UserId AND Group_Members.ConversationId=@Id", new { Id = conversation.Id });
                var profiles = await connection.QueryAsync<Profile>(@"SELECT * FROM Profiles");
                foreach (User user in participants)
                {
                    user.Profile = profiles.FirstOrDefault(profile => profile.UserId == user.Id);
                }
                conversation.Participants = (List<User>)participants;
            }
        }
    }
}
