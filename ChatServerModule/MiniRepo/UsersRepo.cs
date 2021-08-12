using ChatServerModule.Models;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public class UsersRepo : GenericRepo, IUsersRepo
    {
        public void ChangeUserStatus(int userId, UserStatus newUserStatus)
        {

            var profileId = GetProfileId(userId);
            byte status = (byte)newUserStatus;
            string sql = $"UPDATE [Profiles] SET status={status} WHERE id={profileId}";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql);
            }
        }

        public IEnumerable<int> GetFriendsIds(int userId)
        {
            IEnumerable<int> result;
            string sql = $"SELECT friendid FROM [Friends] WHERE userid={userId};";

            using (var conn = new SqlConnection(_connectionString))
            {
                result = conn.Query<int>(sql);
            }
            return result;
        }

        private int GetProfileId(int userId)
        {
            string sql = $"SELECT p.id FROM [Users] AS u JOIN [Profiles] AS p ON p.userid=u.id WHERE u.id={userId} ";//////////
            int profileId;

            using (var conn = new SqlConnection(_connectionString))
            {
                profileId = conn.QueryFirstOrDefault<int>(sql);
            }
            return profileId;
        }
    }
}
