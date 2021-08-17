using AccountModule.Controllers;
using Domain;
using Domain.AccountContracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationUserController _applicationUserController = new();
        private readonly FriendController _friendController = new();
        public ObservableCollection<FriendUI> Friends { get; set; }
           

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            ShowFriends();
        }

        public void ShowFriends()
        {
            Friends = new();
            // hardcoded data
            FriendUI friend1 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 1",
                Status = UserStatus.Online,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend1.UpdateStatusColor();

            FriendUI friend2 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 2",
                Status = UserStatus.Away,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend2.UpdateStatusColor();

            FriendUI friend3 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 3",
                Status = UserStatus.Offline,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend3.UpdateStatusColor();

            FriendUI friend4 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 3",
                Status = UserStatus.Online,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend4.UpdateStatusColor();

            Friends.Add(friend1);
            Friends.Add(friend2);
            Friends.Add(friend1);
            Friends.Add(friend4);
            Friends.Add(friend3);
            Friends.Add(friend3);

            //FriendList.ItemsSource = Friends;
        }
        
        public async void SendFriendRequestClick(object sender, RoutedEventArgs e)
        {
            bool friendRequestFlag = await _friendController.SendFriendRequest(senderEmail.Text, receiverEmail.Text);
            if(friendRequestFlag == false)
            {
                friendRequestErrorMessage.Content = "Erroareeee";
            }
            else
            {
                friendRequestErrorMessage.Content = "Success";
            }
        }
        
        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the loginButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string _loginErrorMessage = _applicationUserController.CheckLoginConstraints(loginEmail.Text, loginPassword.Password);
            bool validData = (_loginErrorMessage == "");
            if (validData)
            {
                /*var userLoginModel = new UserLoginModel
                {
                    Email = loginEmail.Text,
                    Password = loginPassword.Password,
                    // Obs: no check-box for remember me option
                    RememberMe = false  
                };*/
                if (await _applicationUserController.Login(
                    loginEmail.Text, loginPassword.Password, false)) // rememberMe default = false (missing check-box in UI)
                {
                    loginErrorMessage.Foreground = Brushes.Green;
                    loginErrorMessage.Content = "Logged in successfully";
                }
                else
                {
                    loginErrorMessage.Foreground = Brushes.Red;
                    loginErrorMessage.Content = "Incorrect e-mail or password";
                }
            }
            else
            {
                loginErrorMessage.Content = _loginErrorMessage;
            }
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            string _registerErrorMessage = _applicationUserController.CheckRegisterConstraints(firstName.Text, lastName.Text, registerEmail.Text, passwordBox1.Password, passwordBox2.Password);
            bool validData = (_registerErrorMessage == "");
            if (validData)
            {
                if (await _applicationUserController.Register(
                    firstName.Text, lastName.Text, registerEmail.Text, passwordBox1.Password))
                {
                    registerErrorMessage.Foreground = Brushes.Green;
                    registerErrorMessage.Content = "You have registered successfully!";
                }
                else
                {
                    registerErrorMessage.Foreground = Brushes.Red;
                    registerErrorMessage.Content = "This e-mail has already been used";
                }
            }
            else
            {
                registerErrorMessage.Content = _registerErrorMessage;
            }
        }

        public void FadeBorderClick(object sender, RoutedEventArgs e)
        {
            FakeDrawerSimulator.Visibility = Visibility.Hidden;
        }

        public class FriendUI
        {
            public string StatusColor { get; set; }
            public string DisplayName { get; set; }
            public string StatusMessage { get; set; }
            public string ImagePath { get; set; }
            public UserStatus Status { get; set; }

            public ICollectionView fadsfds;

            public FriendUI()
            {
                UpdateStatusColor();
            }

            public void UpdateStatusColor()
            {
                StatusColor = Status switch
                {
                    UserStatus.Online => "GreenYellow",
                    UserStatus.Away => "#FFC074",// pale orange
                    UserStatus.Offline => "#6E7582",// gray
                    _ => "#6E7582",// gray
                };
            }
        }

    }
}
