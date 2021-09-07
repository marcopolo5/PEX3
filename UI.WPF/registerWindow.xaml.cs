using System;
using System.Windows;
using System.Windows.Input;
using AccountModule.Controllers;
using Domain.Exceptions;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        /// <summary>
        /// Can be used for login, register and logout
        /// </summary>
        private readonly ApplicationUserController _applicationUserController = new();
        //private CustomMessageBox messageBox = new CustomMessageBox();

        public RegisterWindow()
        {
           InitializeComponent();
        }

        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegisterClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await _applicationUserController.Register(firstNameText.Text, lastNameText.Text, emailText.Text,
                    password1Text.Password, password2Text.Password);
                new CustomMessageBox().Show("You have registered successfully!");
            }
            catch (InvalidEntityException exception)
            {
                new CustomMessageBox().Show(exception.Message);
            }
            //catch(Exception exception){ } - Commented: Not catching unexpected exceptions while in development TODO: uncomment before release
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            firstNameText.Clear();
            lastNameText.Clear();
            emailText.Clear();
            password1Text.Clear();
            password2Text.Clear();
        }

        private void loginTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new LoginWindow().ShowDialog();
            ShowDialog();
        }

        private void returnTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
            ShowDialog();
        }

        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
      
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
    }
}
