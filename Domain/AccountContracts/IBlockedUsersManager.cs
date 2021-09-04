using Domain.Models;

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
