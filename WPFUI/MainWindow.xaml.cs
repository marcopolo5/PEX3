using Domain;
using Domain.AccountContracts;
using Domain.Models;
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

        // Messy code
        public void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            string errorMessage = "";
            // does not check existing user :)
            bool validData = CheckConstraints(ref errorMessage);
            if (validData)
            {
                var userRegisterModel = new UserRegisterModel
                {
                    Email = Email.Text, // No regex to check Email yet :(
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    Password = PasswordBox1.Password // no hash, plain text !
                };
                _accountService.Register(userRegisterModel);
                ErrorMessage.Content = "You have registered successfully!";
            }
            else
            {
                ErrorMessage.Content = errorMessage;
            }
        }

        public bool CheckConstraints(ref string errorMessage)
        {
            if (FirstName.Text.Length == 0 || LastName.Text.Length == 0)
            {
                errorMessage = "Enter a valid Name";
            }
            else if (Email.Text.Length == 0)
            {
                errorMessage = "Enter a valid e-mail";
            }
            else if (PasswordBox1.Password.Length == 0 || PasswordBox2.Password.Length == 0)
            {
                errorMessage = "The password cannot be empty";
            }
            else if (PasswordBox1.Password != PasswordBox2.Password)
            {
                errorMessage = "Passwords don't match";
            }

            return errorMessage == "";
        }
    }
}
