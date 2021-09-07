using Domain.Models;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Validators;

namespace AccountModule.Controllers
{
    public class ProfileController
    {
        private readonly ProfileRepository _profileRepository = new();

        public async Task UpdateProfile(string displayName, string about, byte[] imageByteArray)
        {
            var profile = await _profileRepository.ReadAsync(ApplicationUserController.CurrentUser.Id);
            
            profile.DisplayName = displayName;
            profile.StatusMessage = about;
            profile.Image = imageByteArray;

            ProfileValidator.Validate(profile);
            await _profileRepository.UpdateAsync(profile);
        }

        public async Task<Profile> GetProfile(int userId)
        {
            return await _profileRepository.ReadAsync(userId);
        }

 
    }
}