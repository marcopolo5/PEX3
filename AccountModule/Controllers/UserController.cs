using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
