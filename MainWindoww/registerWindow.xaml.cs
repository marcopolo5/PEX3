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

namespace MainWindoww
{
    /// <summary>
    /// Interaction logic for registerWindow.xaml
    /// </summary>
    public partial class registerWindow : Window
    {
        public registerWindow()
        {
           InitializeComponent();
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            firstNameText.Clear();
            lastNameText.Clear();
            emailText.Clear();
            password1Text.Clear();
            password2Text.Clear();
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            //mainWindow.Show()
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
