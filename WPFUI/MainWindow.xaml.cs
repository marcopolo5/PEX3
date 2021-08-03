using AccountModule.Controllers;
using Domain;
using Domain.AccountContracts;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace WPFUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationUserController _applicationUserController = new();

        public MainWindow()
        {
            InitializeComponent();
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the loginButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string _loginErrorMessage = _applicationUserController.CheckLoginConstraints(loginEmail.Text, loginPassword.Password);
            bool validData = (_loginErrorMessage == "");
            if (validData)
            {
                /*var userLoginModel = new UserLoginModel
                {
                    Email = loginEmail.Text,
                    Password = loginPassword.Password,
                    // Obs: no check-box for remember me option
                    RememberMe = false  
                };*/
                if (await _applicationUserController.Login(
                    loginEmail.Text, loginPassword.Password, false)) // rememberMe default = false (missing check-box in UI)
                {
                    loginErrorMessage.Foreground = Brushes.Green;
                    loginErrorMessage.Content = "Logged in successfully";
                }
                else
                {
                    loginErrorMessage.Content = "Incorrect e-mail or password";
                }
            }
            else
            {
                loginErrorMessage.Content = _loginErrorMessage;
            }
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            string _registerErrorMessage = _applicationUserController.CheckRegisterConstraints(firstName.Text, lastName.Text, registerEmail.Text, passwordBox1.Password, passwordBox2.Password);
            bool validData = (_registerErrorMessage == "");
            if (validData)
            {
                if (await _applicationUserController.Register(
                    firstName.Text, lastName.Text, registerEmail.Text, passwordBox1.Password))
                {
                    registerErrorMessage.Foreground = Brushes.Green;
                    registerErrorMessage.Content = "You have registered successfully!";
                }
                else
                {
                    registerErrorMessage.Content = "This e-mail has already been used";
                }
            }
            else
            {
                registerErrorMessage.Content = _registerErrorMessage;
            }
        }
    }
}
