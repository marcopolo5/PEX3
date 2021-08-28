using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for FriendListControl.xaml
    /// </summary>
    public partial class FriendListControl : UserControl
    {
        public FriendListingViewModel FriendListingViewModel { get; }

        public FriendListControl()
        {
            InitializeComponent();
                FriendListingViewModel = new FriendListingViewModel();
            DataContext = FriendListingViewModel;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
