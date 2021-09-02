using ChatServerModule.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ChatServerModule.MiniRepo
{
    public class UsersRepo : GenericRepo, IUsersRepo
    {
        public UsersRepo(IConfiguration configuration)
            :base(configuration)
        {
            //empty ctor
        }
        public void ChangeUserStatus(int userId, UserStatus newUserStatus)
        {

            var profileId = GetProfileId(userId);
            byte status = (byte)newUserStatus;
            string sql = $"UPDATE [Profiles] SET status=@Status WHERE id=@ProfileId";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Execute(sql, new { Status = status, ProfileId = profileId });
            }
        }

        public IEnumerable<int> GetFriendsIds(int userId)
        {
            IEnumerable<int> result;
            string sql = $"SELECT friendid FROM [Friends] WHERE userid=@UserId;";

            using (var conn = new SqlConnection(_connectionString))
            {
                result = conn.Query<int>(sql, new { UserId = userId });
            }
            return result;
        }

        public int GetProximityRadius(int userId)
        {
            var sql = "SELECT Settings.proximityRadius FROM [Settings] WHERE userid=@UserId";
            int result = 5;
            using (var conn = new SqlConnection(_connectionString))
            {
                int? querryResult = conn.QueryFirstOrDefault<int?>(sql, new { UserId = userId });
                if (querryResult != null)
                {
                    result = querryResult.Value;
                }
            }
            return result;
        }

        private int GetProfileId(int userId)
        {
            string sql = $"SELECT p.id FROM [Users] AS u JOIN [Profiles] AS p ON p.userid=u.id WHERE u.id=@UserId ";
            int profileId;

            using (var conn = new SqlConnection(_connectionString))
            {
                profileId = conn.QueryFirstOrDefault<int>(sql, new { UserId = userId });
            }
            return profileId;
        }
    }
}
