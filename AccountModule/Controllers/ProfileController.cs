using Domain;
using Domain.Helpers;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class ProfileController
    {
        private readonly ProfileRepository _profileRepository = new();
        private readonly SettingsRepository _settingsRepository = new();
        private readonly AppConfiguration _appConfiguration = new();
        public static CurrentUser CurrentUser = new();
        public async Task<bool> UpdateProfile(string displayName, string about, string photoPath)
        {
            return true; // TO BE implemented soon
        }
    }
}
