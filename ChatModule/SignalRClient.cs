﻿using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR.Client;
using Domain.RepositoryContracts;
using AccountModule.Controllers;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Domain.DTO;

namespace ChatModule
{
    public class SignalRClient : IAsyncDisposable
    {
        private static SignalRClient _signalRClient;

        public readonly LocationAPIController _locationAPIController = new();

        private HubConnection _connection;

        public event Action<Message> MessageReceived;

        public event Action<StatusModel> StatusChanged;

        public event Action<IEnumerable<Conversation>> ConversationsReceived;


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
            var url = @"http://localhost:5000/chat";

            _connection = new HubConnectionBuilder()
                .WithUrl(url, options =>
                {
                    options.Headers.Add("userId", userId.ToString());
                    options.Headers.Add("loginToken", token);
                })
                .Build();

            await _connection.StartAsync();

            _connection.On<Message>("ReceiveMessage", (message) => {
                var conversation = ApplicationUserController
                        .CurrentUser
                        .Conversations
                        .FirstOrDefault(c => c.Id == message.ConversationId);
                if (conversation == null)
                {
                    return;
                }
                conversation.Messages.Add(message);
                MessageReceived?.Invoke(message);
            });


            _connection.On<StatusModel>("ChangeStatus", (status) =>
            {
                var friend = ApplicationUserController.CurrentUser.Friends.Where(f => f.Id == status.FriendId).FirstOrDefault();
                if(friend != null)
                {
                    friend.Profile.Status = status.NewStatus;
                    StatusChanged?.Invoke(status);
                }
            });

            _connection.On("UpdateProximityChats", async () => {
                await UpdateProximityChats();
            });

            _connection.On<IEnumerable<ServerConversationDTO>>("ReceiveConversations", (conversations) => {
                var convs = new List<Conversation>();
                foreach(var servConv in conversations)
                {
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
        }

        public async Task UpdateProximityChats()
        {
            var id = ApplicationUserController.CurrentUser.Id;
            IpstackApiResult apiResult = _locationAPIController.CallApi();
            string location = $"{apiResult.CountryName}, {apiResult.RegionName}, {apiResult.City}";
            UserLocationDTO userLocation = new UserLocationDTO
            {
                UserId = id,
                Location = location,
                Latitude = apiResult.Latitude,
                Longitude = apiResult.Longitude
            };
            await _connection.SendAsync("GetProximityConversationsList", userLocation);
        }

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
        public async Task CreateProximityConversation()
        {
            var location = _locationAPIController.CallApi();
            var createConversationDto = new ConversationCreateDTO
            {
                CreatorsId = ApplicationUserController.CurrentUser.Id,
                Type = Domain.ConversationTypes.ProximityGroup,
                Title = $"{location.RegionName}, {location.City} - {new Random().Next()}",
                Location = $"{location.CountryName}, {location.RegionName}, {location.City}",
                Longitude = location.Longitude,
                Latitude = location.Latitude
            };
            await _connection.SendAsync("CreateProximityConversation", createConversationDto);
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.StopAsync();
            await _connection.DisposeAsync();
            _signalRClient = null;
        }
    }
}
