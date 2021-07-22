using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryContracts
{
    public class ProfileRepository : GenericRepository<Profile>
    {
        public ProfileRepository() : base("Profiles") { }
    }
}
