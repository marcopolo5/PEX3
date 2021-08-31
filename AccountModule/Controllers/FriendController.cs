using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class FriendController
    {
        private readonly UserRepository _userRepository = new();
        private readonly FriendRequestRepository _friendRequestRepository = new();
        private readonly FriendRepository _friendRepository = new();
        private readonly ConversationRepository _conversationRepository = new();

        public async Task DeleteFriend(string friendEmail)
        {
            User friend = await _userRepository.ReadAsync(friendEmail);
            await _friendRepository.DeleteFriend(ApplicationUserController.CurrentUser.Id, friend.Id);
        }

        // TODO: move wrong placed methodes to FriendRequestController

        public async Task<bool> SendFriendRequest(string senderEmail, string receiverEmail)
        {
            // TODO: modify existing check (should already be checked in the search friends method)

            var senderUser = await _userRepository.ReadAsync(senderEmail);
            if (senderUser == null)
            {
                // TODO: needs an internal error message
                return false;
            }
            var receiverUser = await _userRepository.ReadAsync(receiverEmail);
            if (receiverUser == null)
            {
                // TODO: should send an error message "user not found"
                return false;
            }

            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = senderUser.Id,
                ReceiverId = receiverUser.Id
            };
            if (await FriendRequestExists(friendRequest))
            {
                // TODO: friend request already sent
                return false;
            }
            await _friendRequestRepository.CreateAsync(friendRequest);

            return true;
        }

        public async Task<bool> FriendRequestExists(FriendRequest request)
        {
            var friendRequest = await _friendRequestRepository.ReadAsync(request.SenderId, request.ReceiverId);
            if (friendRequest == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> AcceptFriendRequest(int requestId)
        {
            var friendRequest = await _friendRequestRepository.ReadAsync(requestId);
            if (friendRequest == null)
            {
                return false;
            }

            // Update friend connection into DB
            Friend friendForward = new Friend()
            {
                UserId = friendRequest.SenderId,
                FriendId = friendRequest.ReceiverId
            };
            Friend friendBackward = new Friend()
            {
                UserId = friendRequest.ReceiverId,
                FriendId = friendRequest.SenderId
            };

            await _friendRepository.CreateAsync(friendForward);
            await _friendRepository.CreateAsync(friendBackward);

            // Delete the pending friend request from DB
            await _friendRequestRepository.DeleteAsync(friendRequest.Id);

            // Create a conversation between users
            await CreateConversation(friendRequest.ReceiverId, friendRequest.SenderId);

            return true;
        }

        public async Task<bool> DenyFriendRequest(int requestId)
        {
            var friendRequest = await _friendRequestRepository.ReadAsync(requestId);
            if (friendRequest == null)
            {
                return false;
            }

            await _friendRequestRepository.DeleteAsync(friendRequest.Id);

            return true;
        }

        private async Task CreateConversation(int userIdOne, int userIdTwo)
        {
            var participants = new List<User>();

            var conversation = new Conversation
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Title = "private conversation", /// no title needed, we can use the friend's name
                Type = Domain.ConversationTypes.Private,
                Participants = participants
            };

            await _conversationRepository.CreateAsync(conversation);
        }
    }
}
