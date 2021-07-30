using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    /// <summary>
    /// Data access layer class for 'FriendRequest' model. 
    /// Corresponding to 'Friend_Requests' table.
    /// </summary>
    public class FriendRequestRepository : GenericRepository<Models.FriendRequest>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public FriendRequestRepository() : base("Friend_Requests") { }
    }
}
