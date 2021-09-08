﻿using AccountModule.Controllers;
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
            // e nevoie de reload CurrentUser.Friends !!!!
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
