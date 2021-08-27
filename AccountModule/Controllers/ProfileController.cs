﻿using Domain;
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
       
        public async Task UpdateProfile(string displayName, string about, byte[] imageByteArray)
        {
            var userId = ApplicationUserController.CurrentUser.Id;
            var user = await _profileRepository.ReadAsyncProfile(userId);
            user.DisplayName = displayName;
            user.StatusMessage = about;
            user.Image = imageByteArray;
            await _profileRepository.UpdateAsync(user);
        }
    }
}
