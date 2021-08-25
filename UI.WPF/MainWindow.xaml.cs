using Domain.Helpers;
using Domain.HelpersContracts;
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
using AccountModule.Controllers;
using Domain.RepositoryContracts;
using Domain;

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
