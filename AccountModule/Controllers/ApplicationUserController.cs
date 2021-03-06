using System;
using System.Threading.Tasks;
using Domain.Models;
using Domain.AccountContracts;
using Domain.Helpers;
using Domain;
using Domain.Exceptions;
using Domain.Repositories;
using Domain.Validators;

namespace AccountModule.Controllers
{
    public class ApplicationUserController : IApplicationUserController
    {
        private const string default_user_picture_path = "../../../Assets/profile.png";
        private readonly GoogleAuthenticatorController _googleAuthenticatorController = new();
        private readonly UserRepository _userRepository = new();
        private readonly ProfileRepository _profileRepository = new();
        private readonly SettingsRepository _settingsRepository = new();
        private readonly StrikesRepository _strikesRepository = new();
        private readonly AppConfiguration _appConfiguration = new();
        public static CurrentUser CurrentUser = new();
        

        public async Task Login(string email, string password, bool rememberMe)
        {
            UserValidator.ValidateLogin(email,password);
            
            var userLoginModel = new UserLoginModel
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            };

            (var id, var token) = await _userRepository.ValidateCredentials(userLoginModel);
            if (string.IsNullOrEmpty(token) || token.Equals("0") || id==0)
            {
                throw new InvalidEntityException("Invalid credentials. ");
            }

            // save token to file
            if (rememberMe)
            {
                _appConfiguration.SaveId(id);
                _appConfiguration.SaveToken(token);
            }
            
            CurrentUser = await _userRepository.ReadCurrentUserAsync(id, token);
        }

        public async Task Register(string firstName, string lastName, string email, string password, string retypedPassword)
        {
            var userRegisterModel = new UserRegisterModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            
            UserValidator.Validate(userRegisterModel);
            UserValidator.ValidatePasswords(password,retypedPassword);
            
            if (await _userRepository.ReadAsync(userRegisterModel.Email)!=null)
            {
                throw new InvalidEntityException("Email already exists. ");
            }
            
            await _userRepository.CreateAsync(userRegisterModel);
            var profile = new Profile
            {
                UserId = (await _userRepository.GetAvailableId()) - 1,  //TODO: Buggy in this version. Should read the object from DB.
                DisplayName = firstName + " " + lastName,
                Status = UserStatus.Offline,
                Image = ImageHelper.GetImageBytes(default_user_picture_path),
                StatusMessage = "Hi there!"
            };
            var settings = new Settings
            {
                Id = await _settingsRepository.GetAvailableId(),
                UserId = await _userRepository.GetAvailableId()-1, //TODO: Buggy in this version. Should read the object from DB.
                Anonymity = true
            };
            var strikes = new Strikes
            {
                Id = await _strikesRepository.GetAvailableId(),
                UserId = await _userRepository.GetAvailableId() - 1,
                FirstStrike = false,
                SecondStrike = false,
                ThirdStrike = false,
                UnbanDate = null
            };
            await _profileRepository.CreateAsync(profile);
            await _settingsRepository.CreateAsync(settings);
            await _strikesRepository.CreateAsync(strikes);
        }

        public async Task AuthenticateWithGoogle(bool rememberMe)
        {
            var userInfo = await _googleAuthenticatorController.GetGoogleAccountInfo();
            if(await _userRepository.ReadAsync(userInfo["email"])==null)
                await Register(userInfo["given_name"], userInfo["family_name"], userInfo["email"], null,null);
            await Login(userInfo["email"], null, rememberMe);
        }
        
        public async Task<bool> Logout()
        {
            User user = CurrentUser;
            user.Token = "0";
            user.LastUpdate = DateTime.Now;
            await _userRepository.UpdateAsync(user);

            // deleting token from disk and memory:
            if (CurrentUser.ClearData() == false)
                return false;

            return true;
        }


        public async Task<bool> CheckIfUserIsLoggedIn()
        {
            var token = _appConfiguration.GetToken();
            var id = _appConfiguration.GetId();
            if (string.IsNullOrWhiteSpace(token) == true || token == "0" || id == 0)
            {
                return false;
            }
            var user = await _userRepository.ReadAsync(id);
            if (user.Token != token)
            {
                _appConfiguration.ResetToken();
                _appConfiguration.ResetId();
                return false;
            }
            return true;
        }

        public async Task UpdateCurrentUserInformation()
        {
            var id = _appConfiguration.GetId();
            var token = _appConfiguration.GetToken();
            CurrentUser = await _userRepository.ReadCurrentUserAsync(id, token);
        }
        
    }
    
}
