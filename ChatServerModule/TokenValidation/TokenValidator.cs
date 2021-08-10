using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ChatServerModule.MiniRepo;
using Dapper;

namespace ChatServerModule.TokenValidation
{
    // TODO: fix some naming issues/maybe move to mini repos folder
    public class TokenValidator : GenericRepo, ITokenValidator
    {
        public bool IsValid(int userId, string token)
        {
            string sql = $"SELECT COUNT(*) FROM [Users] WHERE id={userId} AND token={token}";

            using (var conn  = new SqlConnection(_connectionString))
            {
                var count = conn.QueryFirst<int>(sql);
                if (count == 1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
