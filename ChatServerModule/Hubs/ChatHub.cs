using ChatServerModule.DTOs;
using ChatServerModule.MiniRepo;
using ChatServerModule.Models;
using ChatServerModule.ProximityChatLogic;
using ChatServerModule.TokenValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.Hubs
{
    public class ChatHub : Hub
    {

        /// <summary>
        /// key - userid |
        /// value - connection
        /// </summary>
        public static readonly ConcurrentDictionary<int, string> ConnectedUsers = new();

        private readonly ITokenValidator _tokenValidator;
        private readonly IConversationRepo _conversationRepo;
        private readonly IUsersRepo _usersRepo;

        public ChatHub(ITokenValidator tokenValidator, 
            IConversationRepo conversationRepo,
            IUsersRepo usersRepo)
        {
            Console.WriteLine("\nChatHub init");
            _tokenValidator = tokenValidator;
            _conversationRepo = conversationRepo;
            _usersRepo = usersRepo;
        }

        /// <summary>
        /// Called when user connects to the server
        /// </summary>
        /// <returns>A task</returns>
        public override async Task OnConnectedAsync()
        {
            Console.WriteLine("Someone's trying to connect");
            // getting data from headers
            string stringId = Context
                .GetHttpContext()
                .Request
                .Headers["userId"];

            string token = Context
                .GetHttpContext()
                .Request
                .Headers["loginToken"];

            // parsing the data
            if (int.TryParse(stringId, out int id) == false)
            {
                return;
            }
            
            if (_tokenValidator.IsValid(id, token) == false)
            {
                return;
            }
            Console.WriteLine($"OnConnected: {id} | {token}\n");

            // add user to the concurrent dictionary
            ConnectedUsers[id] = Context.ConnectionId;

            // update status in DB
            _usersRepo.ChangeUserStatus(id, UserStatus.Online);

            // update status for friends
            await UpdateStatus(id, UserStatus.Online);

            // ask the client to update the proximity chats from the server (by calling GetProximityConversationsList from the client side)
            await Clients.Client(Context.ConnectionId).SendAsync("UpdateProximityChats");

            await base.OnConnectedAsync();
        }

        /// <summary>
        /// Called when user disconnects from the server
        /// </summary>
        /// <param name="exception">An exception</param>
        /// <returns>A task</returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            Console.Write("\nOnDisconnected: ");
            // get the userId
            var userId = ConnectedUsers.FirstOrDefault(cd => cd.Value == Context.ConnectionId).Key;

            // update user status in DB:
            _usersRepo.ChangeUserStatus(userId, UserStatus.Online);

            // update status for friends
            await UpdateStatus(userId, UserStatus.Offline);

            // remove user from the connected user lists
            ConnectedUsers.Remove(userId, out string _); //discarding the out param

            // remove user from every proximity chat
            RemoveUserFromAllProximityChats(userId); 

            Console.Write($"User {userId} with connection id {Context.ConnectionId} has disconnected at {DateTime.Now}");

            await base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// This method can be called from client to send a message
        /// </summary>
        /// <param name="message"> Message to be sent</param>
        /// <returns>A task</returns>
        public async Task SendMessage(Message message)
        {
            // add message to db
            _conversationRepo.AddMessageToConversation(message);

            IEnumerable<int> userIds = _conversationRepo.GetConversationsParticipants(message.ConversationId);

            foreach(var userId in userIds)
            {
                // check if the user is connected
                if (ConnectedUsers.ContainsKey(userId) == false)
                {
                    continue; // if not jump to the next user
                }
                Console.WriteLine($"From {message.SenderId} | To {message.ConversationId} | {message.TextMessage} | Date: {message.CreatedAt}");


                
                var connectionId = ConnectedUsers[userId];
                await Clients.Client(connectionId).SendAsync("ReceiveMessage", message);
            }
        }

        public void CreateProximityConversation(ConversationCreateDTO conversationDTO)
        {
            // map dto to a model
            var conversation = new Conversation 
            { 
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = conversationDTO.Title,
                Type = ConversationTypes.ProximityGroup,
                Location = conversationDTO.Location,
                Longitude = conversationDTO.Longitude,
                Latitude = conversationDTO.Latitude
            };

            // create conversation:
            int conversationId = _conversationRepo.CreateConversation(conversation).Value;
            // add the creator to it:
            _conversationRepo.AddUserToConversation(conversationDTO.CreatorsId, conversationId);
        }

        public async Task GetProximityConversationsList(UserLocationDTO locationDTO)
        {
            var conversations = _conversationRepo.GetConversationsCloseToLocation(locationDTO.Location);
            var conversationsFilteredByUserSettings = new List<Conversation>();
            int userProximityRadius = _usersRepo.GetProximityRadius(locationDTO.UserId); // distance in km

            foreach (var conversation in conversations)
            {
                // get distance in meters
                var distance = DistanceCalculator.CalculateDistance(locationDTO.Latitude, locationDTO.Longitude,
                                                                    conversation.Latitude, conversation.Longitude);
                // calculate distance in km and check if its higher than userProximityRadius, if so go to the next conversation
                if (distance / 1000 > userProximityRadius)
                {
                    continue;
                }

                // add current user to the conversation
                _conversationRepo.AddUserToConversation(locationDTO.UserId, conversation.Id);
                // add conversation to the result
                conversationsFilteredByUserSettings.Add(conversation);
            }
            var connectionId = ConnectedUsers[locationDTO.UserId];
            await Clients.Clients(connectionId).SendAsync("ReceiveConversations", conversationsFilteredByUserSettings);
        }

        /// <summary>
        /// Get connection ids from all the user's friends(if they are connected) 
        /// </summary>
        /// <param name="currentUser"></param>
        /// <returns>Returns a list of connection ids</returns>
        private List<string> GetFriendsConnectionIds(int currentUser)
        {
            var friendsIds = _usersRepo.GetFriendsIds(currentUser);
            var result = new List<string>();
            foreach (var id in friendsIds)
            {
                if (ConnectedUsers.TryGetValue(id, out string connectionId))
                {
                    result.Add(connectionId);
                }
            }
            return result;
        }

        /// <summary>
        /// Update this user status for all connected friends
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="newStatus">New status</param>
        /// <returns>A task</returns>
        private async Task UpdateStatus(int userId, UserStatus newStatus)
        {            
            // create a status model
            var statusModel = new StatusModel
            {
                FriendId = userId,
                NewStatus = newStatus
            };
            _usersRepo.ChangeUserStatus(userId, newStatus);
            // get all online friends
            IReadOnlyList<string> allFriendsConnection = GetFriendsConnectionIds(userId);

            // if there's at least one friend connected call ChangeStatus on client
            if (allFriendsConnection.Count > 0)
            {
                await Clients.Clients(allFriendsConnection).SendAsync("ChangeStatus", statusModel);
            }
        }

        private void RemoveUserFromAllProximityChats(int userId)
        {
            var conversationsIds = _conversationRepo.GetProximityConversationsByUserId(userId);
            foreach (var conversationId in conversationsIds)
            {
                _conversationRepo.RemoveUserFromConversation(userId, conversationId);
            }
        }
    }
}
