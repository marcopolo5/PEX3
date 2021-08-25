using Domain.AccountContracts;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    class SettingsController: ISettingsController
    {
        private readonly SettingsRepository _settingsRepository = new();

        public SettingsController()
        {
        }

        public Task<bool> ChangePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetAnonimity(bool anonimity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetProximityRadius(int proximityRadius)
        {
            throw new NotImplementedException();
        }
    }
}
