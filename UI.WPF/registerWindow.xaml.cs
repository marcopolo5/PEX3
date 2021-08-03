using Domain.AccountContracts;
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
using Microsoft.Extensions.DependencyInjection;
using AccountModule.Controllers;

namespace MainWindoww
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class registerWindow : Window
    {
        /// <summary>
        /// Can be used for login, register and logout
        /// </summary>
        private readonly IAccountService _accountService;
        private readonly ApplicationUserController _applicationUserController = new();


        public registerWindow()
        {
           InitializeComponent();

            // Getting the service from DI
            _accountService = DependencyInjectionHelper.ServiceProvider.GetService<IAccountService>();
        }

        // TODO: fix documentation (no longer matches the functionality)
        /// <summary>
        /// Called by the registerButton
        /// Checks if the data is valid and sends it to the ApplicationUserController for the purpose of registration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public async void ButtonRegisterClick(object sender, RoutedEventArgs e)
        {
            string _registerErrorMessage = _applicationUserController.CheckRegisterConstraints(firstNameText.Text, lastNameText.Text, emailText.Text, password1Text.Password, password2Text.Password);
            bool validData = (_registerErrorMessage == "");
            if (validData)
            {
                if (await _applicationUserController.Register(
                    firstNameText.Text, lastNameText.Text, emailText.Text, password1Text.Password))
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
            loginWindow login = new loginWindow();
            login.Show();
            this.Close();
        }

        private void returnTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Close();
        }

        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
      
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            
        }
    }
}
