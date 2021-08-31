using AccountModule.Controllers;
using Domain;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using UI.WPF.ViewModel;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for AddFriendControl.xaml
    /// </summary>
    public partial class AddFriendControl : UserControl
    {
        private readonly UserController _userController = new();
        private readonly ProfileController _profileController = new();
        private readonly FriendController _friendController = new();
        private readonly FriendRequestController _friendRequestController = new();

        private ObservableCollection<FriendViewModel> Users { get; set; }
        private ObservableCollection<FriendViewModel> PendingFriendRequests { get; set; }

        public AddFriendControl()
        {
            InitializeComponent();
            Users = new ObservableCollection<FriendViewModel>();
            SearchUserList.ItemsSource = Users;

            PendingFriendRequests = new ObservableCollection<FriendViewModel>();
            GetPendingFriendRequests();
            PendindRequestsList.ItemsSource = PendingFriendRequests;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }


        private async void GetPendingFriendRequests()
        {
                var friendRequests = await _friendRequestController.GetUsersFriendRequests();
            foreach (FriendRequest friendRequest in friendRequests.ToList())
            {
                try
                {
                    if (friendRequest.ReceiverId != ApplicationUserController.CurrentUser.Id)
                    {
                        continue;
                    }
                    var profile = await _profileController.GetProfile(friendRequest.SenderId);
                    var user = await _userController.GetUser(friendRequest.SenderId);
                    FriendViewModel userUIModel = new FriendViewModel(profile.DisplayName, user.Email, profile.StatusMessage, ProfileControl.LoadImage(profile.Image), profile.Status, friendRequest.Id);
                    PendingFriendRequests.Add(userUIModel);
                }
                catch (KeyNotFoundException) { }
            }
            
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Users.Clear();
            string filter = searchUserTextBox.Text;
            if(filter.Length < 2)
            {
                return;
            }
            var filteredUsers = await _userController.GetUsers(filter);
            foreach (User user in filteredUsers.ToList())
            {
                if (user.Id == ApplicationUserController.CurrentUser.Id)
                    continue;
                try
                {
                    user.Profile = await _profileController.GetProfile(user.Id);
                }
                catch (KeyNotFoundException) 
                {
                    user.Profile = new();
                    user.Profile.StatusMessage = "Profile Not Found!";
                    user.Profile.Image = ApplicationUserController.GetImageBytes("../../../Assets/warning.png");
                    user.Profile.Status = UserStatus.Offline;
                }
                finally
                {
                    FriendViewModel userUIModel = new FriendViewModel(user.FirstName + " " + user.LastName, user.Email, user.Profile.StatusMessage, ProfileControl.LoadImage(user.Profile.Image), user.Profile.Status);
                    Users.Add(userUIModel);
                }
            }
        }

        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FriendViewModel selectedUser = button.DataContext as FriendViewModel;
            await _friendController.SendFriendRequest(ApplicationUserController.CurrentUser.Email, selectedUser.Email);
            button.IsEnabled = false;
            ((Border)button.Template.FindName("BG", button)).Background = Brushes.SteelBlue;
        }

        private async void AcceptFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FriendViewModel selectedUser = button.DataContext as FriendViewModel;
            await _friendController.AcceptFriendRequest(selectedUser.FriendRequestId);
            PendingFriendRequests.Remove(selectedUser);
        }

        private async void DenyFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            FriendViewModel selectedUser = button.DataContext as FriendViewModel;
            await _friendController.DenyFriendRequest(selectedUser.FriendRequestId);
            PendingFriendRequests.Remove(selectedUser);
        }
    }
}
