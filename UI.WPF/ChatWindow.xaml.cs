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
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        public ChatWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            Hide();
            new MainWindow().ShowDialog();
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
        private void ListViewItem_Selected_3(object sender, RoutedEventArgs e)
        {
            Hide();
            new ChatWindow().ShowDialog();
            ShowDialog();
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void sendMessage_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addConversation_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
