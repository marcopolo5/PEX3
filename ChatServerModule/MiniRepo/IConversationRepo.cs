using ChatServerModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public interface IConversationRepo
    {
        /// <summary>
        /// Returns all the users' ids taking part to a conversation
        /// </summary>
        /// <param name="conversationId">Conversation's id</param>
        /// <returns>List of ids</returns>
        IEnumerable<int> GetConversationsParticipants(int conversationId);

        /// <summary>
        /// Returns a list of conversations close to a location
        /// </summary>
        /// <param name="location">Location</param>
        /// <returns>List of conversations</returns>
        IEnumerable<Conversation> GetConversationsCloseToLocation(string location);

        /// <summary>
        /// Creates a new conversation
        /// </summary>
        /// <param name="conversation">Conversation model</param>
        /// <returns>The id of the conversation</returns>
        int? CreateConversation(Conversation conversation);

        /// <summary>
        /// Add user to conversation
        /// </summary>
        /// <param name="userId">User to be added</param>
        /// <param name="conversationId">Conversation's id</param>
        void AddUserToConversation(int userId, int conversationId);

        /// <summary>
        /// Remove user to conversation
        /// </summary>
        /// <param name="userId">User to be removed</param>
        /// <param name="conversationId">Conversation's id</param>
        void RemoveUserFromConversation(int userId, int conversationId);

        /// <summary>
        /// Add message to conversation
        /// </summary>
        /// <param name="message">Message model</param>
        void AddMessageToConversation(Message message);

        /// <summary>
        /// Returns the ids of all proximity conversations by user's id
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <returns></returns>
        IEnumerable<int> GetProximityConversationsByUserId(int userId);
    }
}
