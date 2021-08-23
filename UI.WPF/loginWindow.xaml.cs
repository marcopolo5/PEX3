using AccountModule.Controllers;
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
using System.Windows.Shapes;

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
            CustomMessageBox messageBox = new CustomMessageBox();
            string _loginErrorMessage = _applicationUserController.CheckLoginConstraints(loginEmail.Text, loginPassword.Password);
            // If login error message is empty, that means the data from the form is valid
            bool validData = (_loginErrorMessage == "");
            if (validData)
            {
                if (await _applicationUserController.Login(
                    loginEmail.Text, loginPassword.Password, false)) //remeber me from UI
                {

                    //loginErrorMessage.Foreground = Brushes.Green;
                    //loginErrorMessage.Content = "Logged in successfully";
                    //CustomMessageBox messageBox = new CustomMessageBox();
                    //messageBox.Show("Logged in successfully");
                    Hide();
                    new HomeWindow().ShowDialog();
                    ShowDialog();
                    
                }
                else
                {
                    //loginErrorMessage.Content = "Incorrect e-mail or password";
                    //CustomMessageBox messageBox = new CustomMessageBox();
                    messageBox.Show("Incorrect e-mail or password");
                }
            }
            else
            {

                //loginErrorMessage.Content = _loginErrorMessage;
                //CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show(_loginErrorMessage);

            }
            
            //messageBox.Show(_loginErrorMessage);
            // loginErrorMessage.Content = _loginErrorMessage;
        }


        private async void ButtonAuthenticateWithGoogle_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _applicationUserController.AuthenticateWithGoogle(true);
                //loginErrorMessage.Foreground = Brushes.Green;
                //loginErrorMessage.Content = "Logged in successfully";
                CustomMessageBox messageBox = new CustomMessageBox();
                //messageBox.Show("Logged in successfully");
                Hide();
                new HomeWindow().ShowDialog();
                ShowDialog();
            }
            catch(Domain.Exceptions.GoogleAuthenticationException exception)
            {
                //loginErrorMessage.Foreground = Brushes.Red;
                //loginErrorMessage.Content = exception.Message;
                CustomMessageBox messageBox = new CustomMessageBox();
                messageBox.Show(exception.Message);
            }
            //catch(Exception exception) { } Commented - Not catching unexpected exceptions while in development
            finally
            {
                Topmost = true;
                Topmost = false;
            }
        }
    }
}
