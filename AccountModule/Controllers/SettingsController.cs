using Domain.AccountContracts;
using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountModule.Controllers
{
    public class SettingsController: ISettingsController
    {
        private readonly SettingsRepository _settingsRepository = new();
        private readonly ApplicationUserController _userController = new();

        public SettingsController()
        {
        }

        public async Task<string> SaveChanges(string password, string newPassword, string newRetypedPassword)
        {
            if(await _settingsRepository.CheckPassword(ApplicationUserController.CurrentUser.Email, password)==1)
            {
                string validationPasswordMessage = _userController.CheckPasswordConstraints(newPassword, newRetypedPassword);
                if (validationPasswordMessage.Equals(""))
                {
                    await ChangePassword(newPassword);
                }
                return validationPasswordMessage;                
            }
            else if (await _settingsRepository.CheckPassword(ApplicationUserController.CurrentUser.Email, password) == 2)
                return "google";
            else
                return "wrong";
        }

        public async Task ChangePassword(string newPassword)
        {
            await _settingsRepository.ChangePassword(ApplicationUserController.CurrentUser.Id, newPassword);
        }

        public async void SetAnonimity(bool anonimity)
        {
            Settings settings = new Settings
            {
                Id = ApplicationUserController.CurrentUser.Settings.Id,
                UserId = ApplicationUserController.CurrentUser.Id,
                Anonymity = anonimity,
                ProximityRadius = ApplicationUserController.CurrentUser.Settings.ProximityRadius
            };
            await _settingsRepository.UpdateAsync(settings);
        }

        public async void SetProximityRadius(int proximityRadius)
        {
            Settings settings = new Settings
            {
                Id = ApplicationUserController.CurrentUser.Settings.Id,
                UserId = ApplicationUserController.CurrentUser.Id,
                Anonymity = ApplicationUserController.CurrentUser.Settings.Anonymity,
                ProximityRadius = proximityRadius
            };
            await _settingsRepository.UpdateAsync(settings);
        }
    }
}
