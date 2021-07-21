using Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryContracts
{
    public interface IUserRepository
    {
        User GetUser(object userId);
        IEnumerable<User> FindUsersByDisplayName(string displayName);
        IEnumerable<User> FindUsersByEmail(string email);
        IEnumerable<User> GetAllUsers();
    }
}
