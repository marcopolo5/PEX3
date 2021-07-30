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
        private readonly IAccountService _accountService;

        public MainWindow(IAccountService accountService)
        {
            _accountService = accountService;
            InitializeComponent();
        }

        /// <summary>
        /// Called by the loginButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string _loginErrorMessage = "";
            bool validData = CheckLoginConstraints(ref _loginErrorMessage);
            if (validData)
            {
                var userLoginModel = new UserLoginModel
                {
                    Email = loginEmail.Text,
                    Password = loginPassword.Password,
                    RememberMe = false  // Feature not implemented yet
                };
                if(new ApplicationUserController(null).Login(userLoginModel)) ///////am adaugat null aici doar sa scap de eroare
                {
                    loginErrorMessage.Foreground = System.Windows.Media.Brushes.Green;
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

        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            string _registerErrorMessage = "";
            bool validData = CheckRegisterConstraints(ref _registerErrorMessage);
            if (validData)
            {
                var userRegisterModel = new UserRegisterModel
                {
                    Email = Email.Text,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Password = PasswordBox1.Password // Mai e nevoie de un regex pentru constrangerea parolei
                };
                if (new ApplicationUserController(null).Register(userRegisterModel)) ///////am adaugat null aici doar sa scap de eroare
                {
                    registerErrorMessage.Foreground = System.Windows.Media.Brushes.Green;
                    registerErrorMessage.Content = "You have registered successfully!";
                }
                else
                {
                    registerErrorMessage.Content = "E-mail already used";
                }
            }
            else
            {
                registerErrorMessage.Content = _registerErrorMessage;
            }
        }

        /// <summary>
        /// Executes a check for the purpose of data validation on the login form
        /// </summary>
        /// <param name="_loginErrorMessage">Parameter which accumulates the error message</param>
        /// <returns>true - if all rules are passed, false - otherwise</returns>
        public bool CheckLoginConstraints(ref string _loginErrorMessage)
        {
            if(loginEmail.Text.Length == 0)
            {
                loginEmail.Select(0, loginEmail.Text.Length);
                loginEmail.Focus();
                _loginErrorMessage = "Enter e-mail";
            }
            else if(loginPassword.Password.Length == 0)
            {
                loginPassword.Focus();
                _loginErrorMessage = "Enter password";
            }
            return _loginErrorMessage == "";
        }

        /// <summary>
        /// Executes a check for the purpose of data validation on the registration form
        /// </summary>
        /// <param name="_registerErrorMessage">Parameter which accumulates the error message</param>
        /// <returns>true - if all rules are passed, false - otherwise</returns>
        public bool CheckRegisterConstraints(ref string _registerErrorMessage)
        {
            if (FirstName.Text.Length == 0 || LastName.Text.Length == 0)
            {
                _registerErrorMessage = "Enter a valid Name";
            }
            else if (
                    !Regex.IsMatch(
                        Email.Text, 
                        @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$") 
                    || 
                        Email.Text.Length == 0)
            {
                _registerErrorMessage = "Enter a valid e-mail";
                Email.Select(0, Email.Text.Length);
                Email.Focus();
            }
            else if (PasswordBox1.Password.Length == 0 || PasswordBox2.Password.Length == 0)
            {
                _registerErrorMessage = "The password cannot be empty";
            }
            else if (PasswordBox1.Password != PasswordBox2.Password)
            {
                _registerErrorMessage = "Passwords don't match";
            }
            else if(!Regex.IsMatch(
                                    PasswordBox1.Password, 
                                    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                )
            {
                _registerErrorMessage = "Password must contain minimum eight characters, at least one uppercase letter, one lowercase letter, one number and one special character";
            }

            return _registerErrorMessage == "";
        }
    }
}
