using System.Data.SqlClient;
using ChatServerModule.MiniRepo;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ChatServerModule.TokenValidation
{
    // TODO: fix some naming issues/maybe move to mini repos folder
    public class TokenValidator : GenericRepo, ITokenValidator
    {
        public TokenValidator(IConfiguration configuration)
            : base(configuration)
        {
            //empty ctor
        }
        public bool IsValid(int userId, string token)
        {
            var sql = $"SELECT COUNT(*) FROM [Users] WHERE id={userId} AND token='{token}'";

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
