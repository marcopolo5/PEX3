using Domain;
using Domain.Controllers;
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
        public MainWindow()
        {
            InitializeComponent();
        }

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
                if(new ApplicationUserController().Login(userLoginModel))
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
                if (new ApplicationUserController().Register(userRegisterModel))
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

            return _registerErrorMessage == "";
        }
    }
}
