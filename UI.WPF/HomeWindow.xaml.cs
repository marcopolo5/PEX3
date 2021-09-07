﻿using AccountModule.Controllers;
using ChatModule;
using System;
using System.Windows;
using System.Windows.Input;
using UI.WPF.View;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly SignalRClient _signalRClient = SignalRClient.GetInstance();
        private readonly ProfileControl _profileControl = new();
        private readonly AboutControl _aboutControl = new();
        private readonly ChatControl _chatControl = new();
        private readonly ProximityChatControl _proximityChatControl; //dont initialize here, check in the ctor first if the user wasnt banned  
        private readonly AddFriendControl _addFriendControl = new();
        private readonly ApplicationUserController _userController = new();
        private readonly SettingsControl _settingsControl = new();

        private bool closeButtonVisibilityFlag = true;
        private bool showFriendListFlag = false;

        public string AppFontFamily { get; set; }

        public HomeWindow()
        {
            AppFontFamily = "Times New Roman";

            InitializeComponent();
            _signalRClient.InitializeConnectionAsync(ApplicationUserController.CurrentUser.Id, ApplicationUserController.CurrentUser.Token)
                .ContinueWith(task =>
                {
                    if (task.Exception != null)
                    {
                        MessageBox.Show(task.Exception.ToString());
                    }
                });
            mainContentControl.Content = _aboutControl;

            // Check if the user is banned from proximity chat
            if (ApplicationUserController.CurrentUser.IsBannedFromProximity)
            {
                // if so disable the proximity btn
                ProximityChatContentButton.IsEnabled = false;
            }
            else
            {
                // if not initialize the proximity chat control
                _proximityChatControl = new();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void SignOut_Selected(object sender, RoutedEventArgs e)
        {
            Hide();
            await _userController.Logout();
            _chatControl?.Dispose();
            _proximityChatControl?.Dispose();
            await _signalRClient.DisposeAsync();
            //new MainWindow().ShowDialog();
            //ShowDialog();
        }

        private void HomeContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _aboutControl;
            closeButtonVisibilityFlag = true;
            ShowHideElements();
        }

        private void ProfileContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _profileControl;
            closeButtonVisibilityFlag = true;
            ShowHideElements();
        }

        private void AddFriendContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _addFriendControl;
            closeButtonVisibilityFlag = false;
            ShowHideElements();
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _chatControl.Dispose();
            _proximityChatControl?.Dispose();
            await _signalRClient.DisposeAsync();
            Environment.Exit(0);
        }

        private void ChatContent_Selected(object sender, RoutedEventArgs e)
        {
            closeButtonVisibilityFlag = true;
            mainContentControl.Content = _chatControl;
            ShowHideElements();
        }

        private void ProximityChatContent_Selected(object sender, RoutedEventArgs e)
        {
            closeButtonVisibilityFlag = true;
            mainContentControl.Content = _proximityChatControl;
            ShowHideElements();

        }

        private void SettingsContent_Selected(object sender, RoutedEventArgs e)
        {
            closeButtonVisibilityFlag = true;
            mainContentControl.Content = _settingsControl;
            ShowHideElements();
        }

        private void ShowHideElements()
        {
            if(closeButtonVisibilityFlag == true)
                closeButton.Visibility = Visibility.Visible;
            else
                closeButton.Visibility = Visibility.Hidden;
            HideFriendList();
        }

        private void DrawerEffectBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideFriendList();
        }

        private void FriendList_Selected(object sender, RoutedEventArgs e)
        {
            showFriendListFlag = !showFriendListFlag;
            if (showFriendListFlag)
                ShowFriendList();
            else
                HideFriendList();
        }

        private void HideFriendList()
        {
            DrawerEffectBorder.Visibility = Visibility.Hidden;
            FriendListGrid.Visibility = Visibility.Hidden;
            showFriendListFlag = false;
            MainMenuListView.SelectedItem = null;
        }

        private void ShowFriendList()
        {
            DrawerEffectBorder.Visibility = Visibility.Visible;
            FriendListGrid.Visibility = Visibility.Visible;
            showFriendListFlag = true;
        }
    }
}
