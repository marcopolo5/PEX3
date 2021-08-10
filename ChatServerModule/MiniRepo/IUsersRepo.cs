using ChatServerModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public interface IUsersRepo
    {
        /// <summary>
        /// Get all ids from friends
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>A list of friends' ids</returns>
        IEnumerable<int> GetFriendsIds(int userId);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userStatus"></param>
        void ChangeUserStatus(int userId, UserStatus newUserStatus);
    }
}
