using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.AccountContracts;
using Domain.RepositoryContracts;
using Domain.Helpers;
using Domain;
using System.Text.RegularExpressions;
using System.IO;

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

        public ApplicationUserController()
        {
        }

        public async Task<bool> Login(string email, string password, bool rememberMe)
        {
            UserLoginModel userLoginModel = new UserLoginModel
            {
                Email = email,
                Password = password,
                RememberMe = rememberMe
            };

            (int id, string token) = await _userRepository.ValidateCredentials(userLoginModel);
            if (string.IsNullOrEmpty(token) || token.Equals("0"))
            {
                return false;
            }

            // save token to file
            if (rememberMe)
            {
                _appConfiguration.SaveId(id);
                _appConfiguration.SaveToken(token);
            }


            // initialize user's attributes
            User user = await _userRepository.ReadAsync(email);
            // ReadCurrentUserAsync needs inspection
            /*var viewUser = await _userRepository.ReadCurrentUserAsync(user.Id);
            CurrentUser.InitializeFields(viewUser.Profile, viewUser.Settings);*/

            CurrentUser = await _userRepository.ReadCurrentUserAsync(id, token);
            
            return true;
        }

        public async Task<bool> Register(string firstName, string lastName, string email, string password)
        {
            UserRegisterModel userRegisterModel = new UserRegisterModel
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };

            if (await UserExists(userRegisterModel.Email))
            {
                return false;
            }
            else
            {
                await _userRepository.CreateAsync(userRegisterModel);
                Profile profile = new Profile
                {
                    UserId = (await _userRepository.GetAvailableId()) - 1,  //TODO: Buggy in this version. Should read the object from DB.
                    DisplayName = firstName + " " + lastName,
                    Status = UserStatus.Offline,
                    Image = GetImageBytes(default_user_picture_path),
                    StatusMessage = "Hi there!"
                };
                Settings settings = new Settings
                {
                    Id = await _settingsRepository.GetAvailableId(),
                    UserId = await _userRepository.GetAvailableId()-1, //TODO: Buggy in this version. Should read the object from DB.
                    Anonymity = true
                };
                Strikes strikeobj = new Strikes
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
                await _strikesRepository.CreateAsync(strikeobj);
                return true;
            }
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

        public async Task<bool> UserExists(string email)
        {
            var user = await _userRepository.ReadAsync(email);
            // email has not been used already -> user does not exist
            if (user == null)
                return false;
            return true;
        }

        public string CheckLoginConstraints(string email, string password)
        {
            if (email.Length == 0)
            {
                return "Enter e-mail";
            }
            else if (password.Length == 0)
            {
                return "Enter password";
            }
            return "";
        }

        public string CheckRegisterConstraints(string firstName, string lastName, string email, string password, string retypedPassword)
        {
            if (firstName.Length == 0 || lastName.Length == 0)
            {
                return "Enter a valid Name";
            }
            else if (
                    !Regex.IsMatch(email,
                                    @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$")
                    ||
                        email.Length == 0)
            {
                return "Enter a valid e-mail";
            }
            return CheckPasswordConstraints(password, retypedPassword);
        }

        public string CheckPasswordConstraints(string password, string retypedPassword)
        {
            if (password.Length == 0 || retypedPassword.Length == 0)
            {
                return "The password cannot be empty";
            }
            else if (password != retypedPassword)
            {
                return "Passwords don't match";
            }
            else if (!Regex.IsMatch(
                                    password,
                                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$!%*?&])[A-Za-z\d@#$!%*?&]{8,}$")
                )
            {
                return "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character";
            }
            return "";
        }

        public async Task<bool> CheckIfUserIsLoggedIn()
        {
            var token = _appConfiguration.GetToken();
            int id = _appConfiguration.GetId();
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
            int id = _appConfiguration.GetId();
            string token = _appConfiguration.GetToken();
            CurrentUser = await _userRepository.ReadCurrentUserAsync(id, token);
        }

        public async Task AuthenticateWithGoogle(bool rememberMe)
        {
            Dictionary<string,string> userInfo = await _googleAuthenticatorController.GetGoogleAccountInfo();
            if(await _userRepository.ReadAsync(userInfo["email"])==null)
                await Register(userInfo["given_name"], userInfo["family_name"], userInfo["email"], null);
            await Login(userInfo["email"], null, rememberMe);
        }

        /// <summary>
        /// Convert memory image into a binary array
        /// </summary>
        /// <param name="imagePath">Path to the image</param>
        /// <returns>A binary array corresponding to the image</returns>
        public static byte[] GetImageBytes(string imagePath)
        {
            byte[] _imageBytes = null;
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                _imageBytes = new byte[fileStream.Length];
                _ = fileStream.Read(_imageBytes, 0, System.Convert.ToInt32(fileStream.Length));
            }
            return _imageBytes;
        }
    }
    
}
