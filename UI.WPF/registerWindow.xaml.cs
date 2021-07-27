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

        public registerWindow()
        {
           InitializeComponent();

            // Getting the service from DI
            _accountService = DependencyInjectionHelper.ServiceProvider.GetService<IAccountService>();
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
            this.Hide();
        }

        private void returnTextBox_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow main = new MainWindow();
            main.Show();
            this.Hide();
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
