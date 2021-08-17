using AccountModule.Controllers;
using Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace WPFUI.TEST.View
{
    /// <summary>
    /// Interaction logic for FriendListUserControl.xaml
    /// </summary>
    public partial class FriendListUserControl : UserControl
    {
        private readonly ApplicationUserController _applicationUserController = new();
        private readonly FriendController _friendController = new();
        public ObservableCollection<FriendUI> Friends { get; set; }

        public FriendListUserControl()
        {
            InitializeComponent();
            DataContext = this;

            ShowFriends();
        }

        public void ShowFriends()
        {
            Friends = new();
            // hardcoded data
            FriendUI friend1 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 1",
                Status = UserStatus.Online,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend1.UpdateStatusColor();

            FriendUI friend2 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 2",
                Status = UserStatus.Away,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend2.UpdateStatusColor();

            FriendUI friend3 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 3",
                Status = UserStatus.Offline,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend3.UpdateStatusColor();

            FriendUI friend4 = new FriendUI
            {
                StatusColor = "",
                DisplayName = "Friend Test 3",
                Status = UserStatus.Online,
                ImagePath = "Assets/Images/user_default.png",
                StatusMessage = "some status message ... "
            };
            friend4.UpdateStatusColor();

            Friends.Add(friend1);
            Friends.Add(friend2);
            Friends.Add(friend1);
            Friends.Add(friend4);
            Friends.Add(friend3);
            Friends.Add(friend3);

            FriendList.ItemsSource = Friends;
        }

        public class FriendUI
        {
            public string StatusColor { get; set; }
            public string DisplayName { get; set; }
            public string StatusMessage { get; set; }
            public string ImagePath { get; set; }
            public UserStatus Status { get; set; }

            public FriendUI()
            {
                UpdateStatusColor();
            }

            public void UpdateStatusColor()
            {
                StatusColor = Status switch
                {
                    UserStatus.Online => "GreenYellow",
                    UserStatus.Away => "#FFC074",// pale orange
                    UserStatus.Offline => "#6E7582",// gray
                    _ => "#6E7582",// gray
                };
            }
        }
    }
}
