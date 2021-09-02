using Domain.Models;

namespace Domain.AccountContracts
{
    public interface IFriendsManager
    {
        bool SendFriendRequest(object userId);
        bool SendFriendRequest(User user);

        bool RemoveFriend(User friend);
        bool RemoveFriend(object friendId);

        bool AcceptFriendRequest(int friendRequestId);
        bool DenyFriendRequest(int friendRequestId);
    }
}
