using Dapper;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
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
        public async Task<Profile> ReadAsyncProfile(int userid)
        {
            using (var connection = CreateConnection())
            {
                var entity = await connection.QuerySingleOrDefaultAsync<Profile>($"SELECT * FROM {TableName} WHERE userid={userid}");
                if (entity == null)
                    throw new KeyNotFoundException($"{TableName} with id {userid} was not found");
                return entity;
            }
        }
        
    }
}
