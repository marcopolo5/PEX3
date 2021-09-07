using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.Repositories
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
		}


		/// <summary>
		/// Parameterized constructor. Used in DAL.Tests project.
		/// </summary>
		/// <param name="tablename">Database's table name</param>
		/// <param name="connectionstring">Database's connection string</param>
		public ConversationRepository(string tablename, string connectionstring) : base(tablename, connectionstring)
		{
		}


		/// <summary>
		/// Async method. Inserts a Conversation object into the database.
		/// </summary>
		/// <param name="conversation">Conversation to be inserted</param>
		public new async Task CreateAsync(Conversation conversation)
		{
			using (var connection = await CreateConnection())
			{
				await base.CreateAsync(conversation);
				var conversationId = await GetAvailableId() - 1;
				foreach (var participant in conversation.Participants)
				{
					var sql = $@"insert into Group_Members values(@ParticipantId, @ConversationId)";
					await connection.QueryAsync(sql,
						new {ParticipantId = participant.Id, ConversationId = conversationId});
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
			var conversation = await base.ReadAsync(id);
			await MapConversationParticipants(conversation);
			await MapConversationMessages(conversation);
			return conversation;
		}


		/// <summary>
		/// Async method. 
		/// Reads all conversations from the database.
		/// Maps it's participants and messages.
		/// </summary>
		/// <returns>All conversations</returns>
		public new async Task<IEnumerable<Conversation>> ReadAllAsync()
		{
			using (var connection = await CreateConnection())
			{
				var conversations = await base.ReadAllAsync();
				foreach (var conversation in conversations)
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
			const string sql =
				@"SELECT Messages.* FROM Messages INNER JOIN Conversations ON Messages.ConversationId=@ConversationId ORDER BY createdat ASC";
			using (var connection = await CreateConnection())
			{
				var messages = await connection
					.QueryAsync<(int id, int conversation_id, int sender_id, string textmessage, DateTime created_at)>
						(sql, new {ConversationId = conversation.Id});
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
			using (var connection = await CreateConnection())
			{
				var participants = await connection.QueryAsync<User>(
					$"SELECT Users.* FROM Users INNER JOIN Group_Members ON Users.Id=Group_Members.UserId AND Group_Members.ConversationId=@Id",
					new {Id = conversation.Id});
				var profiles = await connection.QueryAsync<Profile>(@"SELECT * FROM Profiles");
				foreach (var user in participants)
				{
					user.Profile = profiles.FirstOrDefault(profile => profile.UserId == user.Id);
				}

				conversation.Participants = (List<User>) participants;
			}
		}
	}
}