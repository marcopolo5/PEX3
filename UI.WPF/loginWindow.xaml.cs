using AccountModule.Controllers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using Domain.Exceptions;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for loginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly ApplicationUserController _applicationUserController = new();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void goToRegisterButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new RegisterWindow().ShowDialog();
            ShowDialog();
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the loginButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLoginClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var rememberMe = rememberMeCheckBox.IsChecked ?? false;
                await _applicationUserController.Login(loginEmail.Text, loginPassword.Password, rememberMe);
                Hide();
                new HomeWindow().ShowDialog();
                ShowDialog();
            }
            catch (SqlException)
            {
                new CustomMessageBox().Show("Could not connect to the database.");
            }
            catch (InvalidEntityException exception)
            {
                new CustomMessageBox().Show(exception.Message);
            }
            // catch(Exception exception){ } - Commented: Not catching unexpected exceptions while in development TODO: uncomment before release
        }

        private async void ButtonAuthenticateWithGoogle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _applicationUserController.AuthenticateWithGoogle(true);
                Hide();
                var homeWindow = new HomeWindow();
                homeWindow.ShowDialog();
                ShowDialog();
            }
            catch (GoogleAuthenticationException exception)
            {
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show(exception.Message);
            }
            catch (SqlException)
            {
                new CustomMessageBox().Show("Could not connect to the database.");
            }
            //catch(Exception exception) { } - Commented: Not catching unexpected exceptions while in development TODO: uncomment before release
        }
    }
}