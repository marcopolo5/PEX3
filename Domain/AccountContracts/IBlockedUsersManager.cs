using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.AccountContracts
{
    public interface IBlockedUsersManager
    {
        bool BlockUser(User user);
        bool BlockUser(object userId);

        bool UnblockUser(User user);
        bool UnblockUser(object userId);
    }
}
