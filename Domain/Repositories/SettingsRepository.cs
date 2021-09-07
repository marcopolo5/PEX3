﻿using System.Threading.Tasks;
using Dapper;

namespace Domain.Repositories
{
    /// <summary>
    /// Data access layer class for 'Settings' model. 
    /// Corresponding to 'Settings' table.
    /// </summary>
    public class SettingsRepository : GenericRepository<Models.Settings>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsRepository() : base("Settings") { }

        public async Task ChangePassword(int userId, string newPassword)
        {
            using (var connection = await CreateConnection())
            {
                var query = @"exec spUpdatePassword @userId, @newPassword";
                await connection.QueryAsync(query, new { userId, newPassword });
            }
        }

        public async Task<int> CheckPassword(string email, string password)
        {
            using (var connection = await CreateConnection())
            {
                var query = @"exec spCheckPassword @email, @password";
                return await connection.QueryFirstOrDefaultAsync<int>(query, new { email, password });
            }
        }


    }
}