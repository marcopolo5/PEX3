using AccountModule.Controllers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UI.WPF.Common;
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
        private ObservableCollection<FriendViewModel> FriendList { get; set; } = new();
        ObservableCollection<FriendViewModel> FriendListFiltered = new();


        public FriendListControl()
        {
            InitializeComponent();

            GetFriendList();
            FriendListFiltered = new ObservableCollection<FriendViewModel>(FriendList);
            FriendListView.ItemsSource = FriendListFiltered;
        }


        /******************* Friend List Filter Methods *******************/
        private void GetFriendList()
        {
            var friends = ApplicationUserController.CurrentUser.Friends;
            foreach (User friend in friends)
            {
                FriendList.Add(new FriendViewModel(friend.Profile.DisplayName, friend.Email, friend.Profile.StatusMessage, BitmapImageLoader.LoadImage(friend.Profile.Image), friend.Profile.Status, friend.JoinDate.ToString("dd.MM.yyyy"), friend.Profile.Reputation));
            }
        }

        private void FriendsFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = FriendList.Where(friend => Filter(friend));
            RemoveNonMatching(filtered);
            AddFriendBack(filtered);
        }

        public bool Filter(FriendViewModel friendViewModel)
        {
            return friendViewModel.DisplayName.Contains(FriendsFilter.Text, StringComparison.InvariantCultureIgnoreCase)
                   ||
                   friendViewModel.Email.Contains(FriendsFilter.Text, StringComparison.InvariantCultureIgnoreCase);
        }

        private void RemoveNonMatching(IEnumerable<FriendViewModel> filteredData)
        {
            for (int i = FriendListFiltered.Count - 1; i >= 0; i--)
            {
                var item = FriendListFiltered[i];
                // If friend is not in the filtered argument list, remove it from the ListView's source.
                if (!filteredData.Contains(item))
                {
                    FriendListFiltered.Remove(item);
                }
            }
        }

        private void AddFriendBack(IEnumerable<FriendViewModel> filteredData)
        {
            foreach (var item in filteredData)
            {
                // If item in filtered list is not currently in ListView's source collection, add it back in
                if (!FriendListFiltered.Contains(item))
                {
                    FriendListFiltered.Add(item);
                }
            }
        }

        /****************** End Friend List Filter Methos ****************/


        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedUserToBeRemoved = button.DataContext as FriendViewModel;
            HomeWindow win = (HomeWindow)Window.GetWindow(this);
            win.OpenChatWithFriend(selectedUserToBeRemoved.Email);
        }

        private async void RemoveFriendButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedUserToBeRemoved = button.DataContext as FriendViewModel;
            await _friendController.DeleteFriend(selectedUserToBeRemoved.Email);
            FriendList.Remove(selectedUserToBeRemoved);
            FriendListFiltered.Remove(selectedUserToBeRemoved);
        }

        private void ShowProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var selectedFriend = button.DataContext as FriendViewModel;
            FriendProfileGrid.DataContext = selectedFriend;

            FriendListGrid.Visibility = Visibility.Hidden;
            FriendProfileGrid.Visibility = Visibility.Visible;
        }

        private void CloseFriendProfile_Click(object sender, RoutedEventArgs e)
        {
            FriendProfileGrid.Visibility = Visibility.Hidden;
            FriendListGrid.Visibility = Visibility.Visible;
        }

    }
}
