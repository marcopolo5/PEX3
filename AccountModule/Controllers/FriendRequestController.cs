using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class FriendRequestController
    {
        private readonly FriendRequestRepository _friendRequestRepository = new();

        public async Task<FriendRequest> GetFriendRequest(int id)
        {
            return await _friendRequestRepository.ReadAsync(id);
        }

        public async Task<IEnumerable<FriendRequest>> GetUsersFriendRequests()
        {
            return await _friendRequestRepository.ReadAllAsync();
        }

    }
}
