using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class FriendController
    {
        private readonly UserRepository _userRepository = new();
        private readonly FriendRequestRepository _friendRequestRepository = new();
        private readonly FriendRepository _friendRepository = new();

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

        public async Task<bool> SendFriendRequest(string senderEmail, string receiverEmail)
        {
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
                return false;
            }
            await _friendRequestRepository.CreateAsync(friendRequest);

            return true;
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
                SenderId = friendRequest.SenderId,
                FriendId = friendRequest.ReceiverId
            };
            Friend friendBackward = new Friend()
            {
                SenderId = friendRequest.SenderId,
                FriendId = friendRequest.ReceiverId
            };

            await _friendRepository.CreateAsync(friendForward);
            await _friendRepository.CreateAsync(friendBackward);

            // Delete the pending friend request from DB
            await _friendRequestRepository.DeleteAsync(friendRequest.Id);

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

    }
}
