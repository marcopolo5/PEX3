using Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Repositories;

namespace AccountModule.Controllers
{
    public class UserController
    {
        private readonly UserRepository _userRepository = new();

        public async Task<IEnumerable<User>> GetUsers(string filter)
        {
            return await _userRepository.ReadAllWithFilterAsync(filter);
        }

        public async Task<User> GetUser(int id)
        {
            return await _userRepository.ReadAsync(id);
        }

    }
}
