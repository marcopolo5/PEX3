using ChatServerModule.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public class ConversationRepo : GenericRepo, IConversationRepo
    {
        public ConversationRepo(IConfiguration configuration)
            :base(configuration)
        {
            //empty ctor
        }

        public IEnumerable<int> GetConversationsParticipants(int conversationId)
        {
            IEnumerable<int> result;
            string sql = $"SELECT userid FROM [Group_Members] WHERE conversationid=@ConversationId;";

            using (var conn = new SqlConnection(_connectionString))
            {
                result = conn.Query<int>(sql, new { ConversationId = conversationId });
            }
            return result;
        }

        public IEnumerable<Conversation> GetConversationsCloseToLocation(string location)
        {
            IEnumerable<Conversation> conversations;
            using (var connection = new SqlConnection(_connectionString))
            {
                conversations = connection.Query<Conversation>($"SELECT * FROM [Conversations] WHERE location=@Location", new { Location = location });
                foreach (var conversation in conversations)
                {
                    MapConversationMessages(conversation);
                    MapConversationParticipants(conversation);
                }
            }
            return conversations;
        }

        public int CreateConversation(Conversation conversation)
        {
            int conversationId = 0;

            var insertSql = "INSERT INTO [Conversations](createdat, updatedat, title, type, location, longitude, latitude) VALUES(@CreatedAt, @UpdatedAt, @Title, @Type, @Location, @Longitude, @Latitude)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(insertSql,
                    new
                    {
                        CreatedAt = conversation.CreatedAt,
                        UpdatedAt = conversation.UpdatedAt,
                        Title = conversation.Title,
                        Type = conversation.Type,
                        Location = conversation.Location,
                        Longitude = conversation.Longitude,
                        Latitude = conversation.Latitude
                    });
                conversationId = connection.QueryFirstOrDefault<int>(@"SELECT id FROM [Conversations] WHERE title=@Title", new { Title = conversation.Title });
                
            }
            //using (var connection = new SqlConnection(_connectionString))
            //{
            //    //conversationId = connection.QueryFirstOrDefault<int>(selectSql,
            //    conversationId = connection.Query<int>(selectSql,
            //      new
            //      {
            //          Title = conversation.Title
            //      }).FirstOrDefault();
            //}
            return conversationId;
        }

        public void AddUserToConversation(int userId, int conversationId)
        {
            var checkSql = "SELECT COUNT(*) FROM [Group_Members] WHERE userid=@UserId AND conversationid=@ConversationId";
            var checkResult = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
               checkResult = connection.QueryFirst<int>(checkSql, new { UserId = userId, ConversationId = conversationId });
            }
            if (checkResult != 0)
            {
                return;
            }
            var sql = "INSERT INTO [Group_Members](userid, conversationid) VALUES(@UserId, @ConversationId)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new
                {
                    UserId = userId,
                    ConversationId = conversationId
                });
            }
        }

        public void RemoveUserFromConversation(int userId, int conversationId)
        {
            var sql = "DELETE FROM [Group_Members] WHERE userid=@UserId AND conversationid=@ConversationId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { UserId = userId, ConversationId = conversationId });
            }
        }

        public void AddMessageToConversation(Message message)
        {
            var sql = "INSERT INTO [Messages](conversationid, senderid, textmessage, createdat) VALUES(@ConversationId, @SenderId, @TextMessage, @CreatedAt)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql,
                    new
                    {
                        ConversationId = message.ConversationId,
                        SenderId = message.SenderId,
                        TextMessage = message.TextMessage,
                        CreatedAt = message.CreatedAt
                    });
            }
        }
        public void RemoveConversation(int id)
        {
            var msgsSql = "DELETE FROM [Messages] WHERE conversationid=@Id";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(msgsSql, new { Id = id });
            }
            var sql = "DELETE FROM [Conversations] WHERE id=@Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(sql, new { Id = id });
            }
        }
        public IEnumerable<int> GetProximityConversationsByUserId(int userId)
        {
            var sql = "SELECT Conversations.id FROM [Conversations] JOIN [Group_Members] ON conversationid=Conversations.id WHERE userid=@UserId AND type=2";
            IEnumerable<int> result;
            using (var connection = new SqlConnection(_connectionString))
            {
                result = connection.Query<int>(sql, new { UserId = userId });
            }
            return result;
        }



        // private methods:
        /// <summary>
        /// Maps conversation's messages to itself.
        /// </summary>
        /// <param name="conversation">Conversation to be mapped</param>
        private void MapConversationMessages(Conversation conversation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var messages = connection.Query<(int id, int conversation_id, int sender_id, string textmessage, DateTime created_at)>
                            ($"SELECT Messages.* FROM Messages INNER JOIN Conversations ON Messages.ConversationId=@ConversationId ORDER BY createdat ASC", new { ConversationId = conversation.Id } );
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
        /// Maps a conversation's participants to itself.
        /// </summary>
        /// <param name="conversation"></param>
        private void MapConversationParticipants(Conversation conversation)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var participants = connection.Query<int>($"SELECT Users.id FROM Users INNER JOIN Group_Members ON Users.Id=Group_Members.UserId AND Group_Members.ConversationId=@Id", new { Id = conversation.Id });

                conversation.Participants = participants.ToList();
            }
        }

    }
}
