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

namespace MainWindoww
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

        private void signupButton_Click(object sender, RoutedEventArgs e)
        {
            registerWindow register = new registerWindow();
            this.Close();
            register.Show();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            loginWindow login = new loginWindow();
            this.Close();
            login.Show();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
            
        }
        private void grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

    }
}
