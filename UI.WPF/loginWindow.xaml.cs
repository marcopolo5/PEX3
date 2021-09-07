using AccountModule.Controllers;
using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void GoToRegisterButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new RegisterWindow().ShowDialog();
            ShowDialog();
        }


        /// <summary>
        /// Handler for classic login use case.
        /// </summary>
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


        /// <summary>
        /// Handler for Google authentication login and/or register.
        /// </summary>
        private async void ButtonAuthenticateWithGoogle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _applicationUserController.AuthenticateWithGoogle(true);
                Hide();
                new HomeWindow().ShowDialog();
                ShowDialog();
            }
            catch (GoogleAuthenticationException exception)
            {
                new CustomMessageBox().Show(exception.Message);
            }
            catch (SqlException)
            {
                new CustomMessageBox().Show("Could not connect to the database.");
            }
            //catch(Exception exception) { } - Commented: Not catching unexpected exceptions while in development TODO: uncomment before release
        }


    }
}