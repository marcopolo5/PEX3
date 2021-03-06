using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Domain.Models;

namespace Domain.Repositories
{
    /// <summary>
    /// Data access layer class for 'Profile' model. 
    /// Corresponding to 'Profiles' table.
    /// </summary>
    public class ProfileRepository : GenericRepository<Profile>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProfileRepository() : base("Profiles") { }
        public new async Task<Profile> ReadAsync(int userid)
        {
            using (var connection = await CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<Profile>($"SELECT * FROM {TableName} WHERE userid={userid}");
                if (entity == null)
                    throw new KeyNotFoundException($"{TableName} with id {userid} was not found");
                return entity;
            }
        }
        
    }
}
