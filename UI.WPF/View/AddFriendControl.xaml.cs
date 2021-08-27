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
using UI.WPF.ViewModel;

namespace UI.WPF.View
{
    /// <summary>
    /// Interaction logic for AddFriendControl.xaml
    /// </summary>
    public partial class AddFriendControl : UserControl
    {
        public PeopleListingViewModel PeopleListingViewModel { get; }
        public AddFriendControl()
        {
            InitializeComponent();
            PeopleListingViewModel = new PeopleListingViewModel();
            DataContext = PeopleListingViewModel;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ButtonSearch_Click(object sender, RoutedEventArgs e)
        {
            var users = AddFriendControl.SearchUsers();
            Print(users);
        }

        private static object SearchUsers()
        {
            throw new NotImplementedException();
        }

        private void Print(object users)
        {
            throw new NotImplementedException();
        }
    }
}
