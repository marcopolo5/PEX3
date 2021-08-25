using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
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
            using (var connection = CreateConnection())
            {
                string query = @"exec spUpdatePassword @userId, @newPassword";
                await connection.QueryAsync(query);
            }
        }
    }
}
