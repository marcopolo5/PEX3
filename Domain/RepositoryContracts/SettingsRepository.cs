using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'Settings' model. 
    /// Corresponding to 'Settings' table.
    /// </summary>
    class SettingsRepository : GenericRepository<Models.Settings>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsRepository() : base("Settings") { }
    }
}
