using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Repositories;

namespace AccountModule.Controllers
{
    public class FriendRequestController
    {
        private readonly FriendRequestRepository _friendRequestRepository = new();

        public async Task<FriendRequest> GetFriendRequest(int id)
        {
            return await _friendRequestRepository.ReadAsync(id);
        }

        public async Task<FriendRequest> FriendRequestExists(int senderId, int receiverId)
        {
            return await _friendRequestRepository.FriendRequestExists(senderId, receiverId);
        }

        public async Task<IEnumerable<FriendRequest>> GetUsersFriendRequests()
        {
            return await _friendRequestRepository.ReadAllAsync();
        }
        
    }
}
