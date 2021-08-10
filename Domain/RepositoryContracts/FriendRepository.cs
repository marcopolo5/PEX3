using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'Friend' model.
    /// Corresponding to 'Friends' table.
    /// </summary>
    public class FriendRepository : GenericRepository<Friend>
    {
        public FriendRepository(): base("Friends") { }
    }
}
