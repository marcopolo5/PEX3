using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class UsersController
    {
        private readonly UserRepository _userRepository = new();
        public async Task<List<User>> SearchUsers(string displayName_email)
        {
            return await _userRepository.ReadAllAsync(displayName_email) as List<User>;
        }
     
    }

}
