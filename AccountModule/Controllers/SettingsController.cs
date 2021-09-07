using Domain.AccountContracts;
using Domain.Models;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Validators;

namespace AccountModule.Controllers
{
    public class SettingsController : ISettingsController
    {
        private readonly SettingsRepository _settingsRepository = new();
        
        public async Task<string> SaveChanges(string password, string newPassword, string newRetypedPassword)
        {
            if (await _settingsRepository.CheckPassword(ApplicationUserController.CurrentUser.Email, password) == 1)
            {
                UserValidator.ValidatePasswords(newPassword, newRetypedPassword);
                await ChangePassword(newPassword);
                return "";
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

        public async void SetAnonymity(bool anonimity)
        {
            var settings = new Settings
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
            var settings = new Settings
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