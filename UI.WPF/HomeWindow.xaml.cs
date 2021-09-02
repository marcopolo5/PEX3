using AccountModule.Controllers;
using ChatModule;
using System;
using System.Collections.Generic;
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
using UI.WPF.View;
using UI.WPF.ViewModel;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private readonly SignalRClient _signalRClient = SignalRClient.GetInstance();
        private readonly ProfileControl _profileControl = new();
        private readonly HomeControl _homeControl = new();
        private readonly  ChatControl _chatControl = new();
        private  readonly ProximityChatControl _proximityChatControl = new();
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
                .ContinueWith(async task =>
                {
                    if (task.Exception != null)
                    {
                        MessageBox.Show(task.Exception.ToString());
                    }
                    // await _signalRClient.UpdateProximityChats(); // removed: already done server side
                });
            mainContentControl.Content = _homeControl;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void SignOut_Selected(object sender, RoutedEventArgs e)
        {
            Hide();
            await _userController.Logout();
            _chatControl.Dispose();
            await _signalRClient.DisposeAsync();
            //new MainWindow().ShowDialog();
            //ShowDialog();
        }

        private void HomeContent_Selected(object sender, RoutedEventArgs e)
        {
            mainContentControl.Content = _homeControl;
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
            mainContentControl.Content = _settingsControl;
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
