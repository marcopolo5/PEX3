using Domain;
using Domain.Helpers;
using Domain.Models;
using Domain.RepositoryContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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