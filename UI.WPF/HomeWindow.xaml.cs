using AccountModule.Controllers;
using SignalRClientModule;
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


        /// <summary>
        /// Drag and move the window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        /// <summary>
        /// Closes the window and dispose data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _chatControl.Dispose();
            _proximityChatControl?.Dispose();
            await _signalRClient.DisposeAsync();
            Environment.Exit(0);
        }


        /// <summary>
        /// Minimizes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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
            ShowHideElements();
        }

        private void ProfileContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _profileControl;
            ShowHideElements();
        }

        private void AddFriendContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _addFriendControl;
            ShowHideElements();
        }

        private void ChatContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _chatControl;
            ShowHideElements();
        }

        private void ProximityChatContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _proximityChatControl;
            ShowHideElements();

        }

        private void SettingsContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _settingsControl;
            ShowHideElements();
        }


        /// <summary>
        /// Initially intended for responsive design and multiple elements to be shown/hidden
        /// Currently only hide the FriendList
        /// </summary>
        private void ShowHideElements()
        {
            HideFriendList();
        }


        /// <summary>
        /// Hide friend list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawerEffectBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HideFriendList();
        }


        /// <summary>
        /// Set the the visibility flag of the friend list
        /// The flag does not let the friend list to be opened again if it is already opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FriendList_Selected(object sender, RoutedEventArgs e)
        {
            showFriendListFlag = !showFriendListFlag;
            if (showFriendListFlag)
                ShowFriendList();
            else
                HideFriendList();
        }


        /// <summary>
        /// Hide friend list and remove the drawer effect
        /// </summary>
        private void HideFriendList()
        {
            DrawerEffectBorder.Visibility = Visibility.Hidden;
            FriendListGrid.Visibility = Visibility.Hidden;
            showFriendListFlag = false;
            MainMenuListView.SelectedItem = null;
        }


        /// <summary>
        /// Show friend list and make the drawer effect visible
        /// </summary>
        private void ShowFriendList()
        {
            DrawerEffectBorder.Visibility = Visibility.Visible;
            FriendListGrid.Visibility = Visibility.Visible;
            showFriendListFlag = true;
        }

        /// <summary>
        /// Open the chat with the selected friend
        /// Must only be called from friend list - "send message" button
        /// </summary>
        public void OpenChatWithFriend(string friendEmail)
        {
            mainContentControl.Content = _chatControl;
            _chatControl.GoToFriendConversation(friendEmail);
            ShowHideElements();
        }
    }
}
