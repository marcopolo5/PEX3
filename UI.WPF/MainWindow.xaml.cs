using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AccountModule.Controllers;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationUserController _userController = new();
        public MainWindow()
        {
            InitializeComponent();

            var isUserLoggedIn = Task<bool>.Run(() => _userController.CheckIfUserIsLoggedIn()).Result;
            if (isUserLoggedIn)
            {
                Hide();
                Task.Run(() => _userController.UpdateCurrentUserInformation()).Wait();
                var homeWindow = new HomeWindow();
                homeWindow.ShowDialog();
                ShowDialog();
            }
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RegisterWindow().ShowDialog();
            ShowDialog();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow().ShowDialog();
            ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        } 

        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
