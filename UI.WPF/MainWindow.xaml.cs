using Domain.Helpers;
using Domain.HelpersContracts;
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
using AccountModule.Controllers;
using Domain.RepositoryContracts;
using Domain;

namespace UI.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IAppConfiguration _appConfig = new AppConfiguration();
        private readonly UserRepository _userRepo;
        public MainWindow()
        {
            InitializeComponent();
            _userRepo = new UserRepository();
            var token = _appConfig.GetToken();
            int id = _appConfig.GetId();
            //int id = 2;
            if (string.IsNullOrWhiteSpace(token) == false && token != "0" && id != 0)
            //if(true)
            {
                Hide();
                //_userRepo.ReadCurrentUserAsync(id).ContinueWith(task =>
                //{
                //    ApplicationUserController.CurrentUser = task.Result;

                //});
                ApplicationUserController.CurrentUser =  Task<CurrentUser>.Run(() => _userRepo.ReadCurrentUserAsync(id)).Result;
                var homeWindow = new HomeWindow();
                homeWindow.ShowDialog();
                ShowDialog();
            }
        }

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new RegisterWindow().ShowDialog();
            ShowDialog();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginWindow().ShowDialog();
            ShowDialog();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        } 

        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
