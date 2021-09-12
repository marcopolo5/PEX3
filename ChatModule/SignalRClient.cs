using Domain.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using AccountModule.Controllers;
using System.Linq;
using System.Collections.Generic;
using Domain.DTO;
using Domain.Repositories;

namespace SignalRClientModule
{
    public class SignalRClient : IAsyncDisposable
    {
        private static SignalRClient _signalRClient;

        private readonly LocationAPIController _locationAPIController = new();
        private readonly UserRepository _userRepository = new();
        private readonly SettingsRepository _settingsRepository = new();
        private readonly FriendRequestRepository _friendRequestRepository = new();
        private readonly ConversationRepository _conversationRepository = new();
        private HubConnection _connection;


        // EVENTS: 
        public event Action<Message> MessageReceived;

        public event Action<StatusModel> StatusChanged;

        public event Action<IEnumerable<Conversation>> ConversationsReceived;

        public event Action<Conversation> ConversationReceived;

        public event Action<string> ConversationRemoved;

        /// <summary>
        /// Takes the friend user model and a boolean that tells if the friend needs to be removed or added (if true remove the friend)
        /// CurrentUser.Friends was already updated, use this only to update the UI
        /// </summary>
        public event Action<User, bool> FriendshipUpdated;
        public event Action<FriendRequest> FriendRequestDenied;
       
        public event Action<User, FriendRequest> FriendRequestReceivedd;

        public static SignalRClient GetInstance()
        {
            if (_signalRClient == null)
            {
                _signalRClient = new SignalRClient();
                return _signalRClient;
            }
            return _signalRClient;
        }

        private SignalRClient()
        {
            // SINGLETON 
        }

        public async Task InitializeConnectionAsync(int userId, string token)
        {
            //var url = @"http://79.113.39.95:5000/chat";
            var url = @"http://localhost:5000/chat";
            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Headers.Add("userId", userId.ToString());
                    options.Headers.Add("loginToken", token);
                })
                .Build();

            await _connection.StartAsync();

            _connection.On<Message>("ReceiveMessage", async (message) => {
                var conversation = ApplicationUserController
                        .CurrentUser
                        .Conversations
                        .FirstOrDefault(c => c.Id == message.ConversationId);
                if (conversation == null)
                {
                    return;
                }

                await BindUsersNameToMessage(message);

                conversation.Messages.Add(message);
                MessageReceived?.Invoke(message);
            });

            _connection.On<StatusModel>("ChangeStatus", (status) =>
            {
                var friend = ApplicationUserController.CurrentUser.Friends.Where(f => f.Id == status.FriendId).FirstOrDefault();
                if (friend != null)
                {
                    friend.Profile.Status = status.NewStatus;
                    StatusChanged?.Invoke(status);
                }
            });

            _connection.On("UpdateProximityChats", async () => {
                await UpdateProximityChats();
            });

            _connection.On<IEnumerable<ServerConversationDTO>>("ReceiveConversations", async (conversations) => {
                var convs = new List<Conversation>();
                foreach (var servConv in conversations)
                {
                    Parallel.ForEach(servConv.Messages, async message =>
                    {
                        await BindUsersNameToMessage(message);
                    });

                    if (ApplicationUserController.CurrentUser.Conversations.Where(conv => conv.Id == servConv.Id).Count() == 0)
                    {
                        convs.Add(new Conversation
                        {
                            Id = servConv.Id,
                            Title = servConv.Title,
                            Messages = servConv.Messages,
                            Type = servConv.Type,
                            CreatedAt = servConv.CreatedAt,
                            UpdatedAt = servConv.UpdatedAt
                        });
                    }
                }
                // add convs to current user
                ApplicationUserController.CurrentUser.Conversations.AddRange(convs);
                // trigger the event
                ConversationsReceived?.Invoke(convs);
            });

            _connection.On<int>("FriendRequestDenied", (friendRequestId) =>
            {
                var friendRequest = ApplicationUserController.CurrentUser
                                                    .FriendRequests
                                                    .FirstOrDefault(fr => fr.Id == friendRequestId);
                if (friendRequest == null)
                {
                    return;
                }
                ApplicationUserController.CurrentUser.FriendRequests.Remove(friendRequest);
                FriendRequestDenied?.Invoke(friendRequest);
            });

            _connection.On<int, int>("ReceiveFriendRequest", async (senderId, receiverId) =>
            {
                var friendRequest = await _friendRequestRepository.ReadAsync(senderId, receiverId);
                ApplicationUserController.CurrentUser.FriendRequests.Add(friendRequest);
                var friend = await _userRepository.ReadAsync(senderId);
                FriendRequestReceivedd?.Invoke(friend, friendRequest);
            });

            _connection.On<int, bool>("UpdateFriendship", async (friendId, removeFriend) =>
            {
                User friend;
                if (removeFriend)
                {
                    friend = ApplicationUserController.CurrentUser.Friends.FirstOrDefault(f => f.Id == friendId);
                    ApplicationUserController.CurrentUser.Friends.Remove(friend);
                    ConversationRemoved?.Invoke(friend.Email);
                }
                else
                {
                    friend = await _userRepository.ReadAsync(friendId);
                    ApplicationUserController.CurrentUser.Friends.Add(friend);
                    var conversation = await _conversationRepository.ReadAsync(ApplicationUserController.CurrentUser.Id, friendId);
                    ConversationReceived?.Invoke(conversation);
                }
                FriendshipUpdated?.Invoke(friend, removeFriend);
            });
        }

        /// <summary>
        /// Calls AddOrRemoveFriend on server. Use this to let other user know that this user added or removed him/her
        /// </summary>
        /// <param name="friendUserId">Friend's USER id</param>
        /// <param name="wasFriendRemoved">Set this to true if friend was removed</param>
        /// <returns></returns>
        public async Task AddOrRemoveFriend(int friendUserId, bool wasFriendRemoved)
        {
            await _connection.SendAsync("AddOrRemoveFriend", ApplicationUserController.CurrentUser.Id, friendUserId, wasFriendRemoved);
        }

        public async Task SendFriendRequest(int senderId, int receiverId)
        {
            await _connection.SendAsync("SendFriendRequest", senderId, receiverId);
        }

        public async Task DenyFriendRequest(int friendRequestId)
        {
            await _connection.SendAsync("DenyFriendRequest", friendRequestId);
        }


        /// <summary>
        /// Calls GetProximityConversationsList on server. Use this to check if there are new proximity chats in your area.
        /// </summary>
        /// <returns>A Task</returns>
        public async Task UpdateProximityChats()
        {
            var id = ApplicationUserController.CurrentUser.Id;
            var apiResult = await _locationAPIController.CallApiAsync();

            var location = $"{apiResult.CountryName}, {apiResult.RegionName}, {apiResult.City}";
            var userLocation = new UserLocationDTO
            {
                UserId = id,
                Location = location,
                Latitude = apiResult.Latitude,
                Longitude = apiResult.Longitude
            };
            await _connection.SendAsync("GetProximityConversationsList", userLocation);
        }

        /// <summary>
        /// Calls SendMessage method on server. Use this to add a new message.
        /// </summary>
        /// <param name="conversationID">Conversation's id</param>
        /// <param name="textMessage">Message that needs to be sent</param>
        /// <returns>A Task</returns>
        public async Task SendMessageAsync(int conversationID, string textMessage)
        {
            var message = new Message()
            {
                SenderId = ApplicationUserController.CurrentUser.Id,
                ConversationId = conversationID,
                CreatedAt = DateTime.Now,
                TextMessage = textMessage
            };

            await _connection.SendAsync("SendMessage", message);
        }

        /// <summary>
        /// Calls CreateProximityConversation on server. Use this to create a new proximity chat
        /// </summary>
        /// <param name="title">Conversation's title</param>
        /// <returns>A Task</returns>
        public async Task CreateProximityConversationAsync(string title)
        {
            var location = await _locationAPIController.CallApiAsync();
            var createConversationDto = new ConversationCreateDTO
            {
                CreatorsId = ApplicationUserController.CurrentUser.Id,
                Type = Domain.ConversationTypes.ProximityGroup,
                Title = $"{title} - {new Random().Next()}",
                Location = $"{location.CountryName}, {location.RegionName}, {location.City}",
                Longitude = location.Longitude,
                Latitude = location.Latitude
            };
            await _connection.SendAsync("CreateProximityConversation", createConversationDto);
        }

        /// <summary>
        /// It checks owner's settings and it binds owner's username or abbreviation to the message
        /// </summary>
        /// <param name="message">Message that needs to be updated with the display name</param>
        private async Task BindUsersNameToMessage(Message message)
        {
            var settings = await _settingsRepository.ReadByUserIdAsync(message.SenderId);
            var user = await _userRepository.ReadAsync(message.SenderId);
            if (settings.Anonymity)
            {
                message.UsersDisplayName = $"{user.LastName[0]}.{user.FirstName[0]}.";
            }
            else
            {
                message.UsersDisplayName = $"{user.Profile.DisplayName}";
            }
        }

        /// <summary>
        /// Use this to close the SignalR connection
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _signalRClient = null;
        }
    }
}