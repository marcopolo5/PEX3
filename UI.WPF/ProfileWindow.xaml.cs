using Microsoft.Win32;
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
    /// Interaction logic for ProfileWindow.xaml
    /// </summary>
    public partial class ProfileWindow : Window
    {
        public ProfileWindow()
        {
            InitializeComponent();
        }
        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new HomeWindow().ShowDialog();
            ShowDialog();
        }

        private void photoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
               profilePicture.Fill = new ImageBrush(new BitmapImage(new Uri(openFileDialog.FileName)));
            }

        }
        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Hide();
            new HomeWindow().ShowDialog();
            ShowDialog();
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Hide();
            new ProfileWindow().ShowDialog();
            ShowDialog();
        }

        private void ListViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            Hide();
            new AddFriendsWindow().ShowDialog();
            ShowDialog();
        }

        
    }
}
