using AccountModule.Controllers;
using System;
using System.Windows;
using System.Windows.Controls;
using UI.WPF.ViewModel;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for FriendListControl.xaml
    /// </summary>
    public partial class FriendListControl : UserControl
    {
        private readonly FriendController _friendController = new();

        public FriendListingViewModel FriendListingViewModel { get; }

        public FriendListControl()
        {
            InitializeComponent();
                FriendListingViewModel = new FriendListingViewModel();
            DataContext = FriendListingViewModel;
        }

        // TODO: Create new conversation or open existing one 
        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedUserToBeRemoved = button.DataContext as FriendViewModel;
            await _friendController.DeleteFriend(selectedUserToBeRemoved.Email);
            // e nevoie de reload CurrentUser.Friends !!!!
        }

        // TODO: Show friend's profile (backend + frontend)
        private void ShowProfileButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
