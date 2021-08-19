using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AccountModule.Controllers
{
    class ReputationController
    {
        private readonly ProfileRepository _profileRepository = new();
        private readonly MessageRepository _messageRepository = new();
        private readonly UserRepository _userRepository = new();

        public async Task RateUp(int messageid)
        {
            var message = await _messageRepository.ReadAsync(messageid);
            var user = await _userRepository.ReadAsync(message.SenderId);
            user.Profile.Reputation += 1;
            await _profileRepository.UpdateAsync(user.Profile);
        }

        public async Task RateDown(int messageid)
        {
            var message = await _messageRepository.ReadAsync(messageid);
            var user = await _userRepository.ReadAsync(message.SenderId);
            user.Profile.Reputation -= 1;
            await _profileRepository.UpdateAsync(user.Profile);
        }

    }
}
