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
        /// Change user status in db
        /// </summary>
        /// <param name="userId">User's id</param>
        /// <param name="userStatus">New status</param>
        void ChangeUserStatus(int userId, UserStatus newUserStatus);
    }
}
