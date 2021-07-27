﻿using Domain.Models;
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
    }
}
