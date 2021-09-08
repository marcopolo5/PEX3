using Domain;
using System.Windows;
using System.Windows.Media.Imaging;

namespace UI.WPF.ViewModel
{
    public class FriendViewModel : ViewModelBase
    {
        private bool _isSelected;
        private string _statusColor;
        private string _displayName;
        private string _email;
        private string _statusMessage;
        private BitmapImage _profilePicture;
        private UserStatus _status;
        private int _friendRequestId;
        private Visibility _friendRequestExists;
        private Visibility _candAddFriend;
        private Visibility _friendRequestSent;
        private Visibility _isFriend;
        private string _joinDate;
        private int _reputation;
        private string _availability;

        public FriendViewModel(string displayName, string email, string statusMessage, BitmapImage image, UserStatus status, string joinDate, int reputation)
        {
            _status = status;
            UpdateStatusColor();
            UpdateAvailability();
            _displayName = displayName;
            _email = email;
            _statusMessage = statusMessage;
            _profilePicture = image;
            _joinDate = joinDate;
            _reputation = reputation;
        }

        public FriendViewModel(string displayName, string email, string statusMessage, BitmapImage image, UserStatus status, int friendRequestId)
        {
            _status = status;
            UpdateStatusColor();
            _displayName = displayName;
            _email = email;
            _statusMessage = statusMessage;
            _profilePicture = image;
            _friendRequestId = friendRequestId;
        }

        public string Availability
        {
            get
            {
                return _availability;
            }
            set
            {
                _availability = value;
                OnPropertyChanged(nameof(Availability));
            }
        }

        public Visibility FriendRequestExists
        {
            get{
                return _friendRequestExists;
            }
            set
            {
                _friendRequestExists = value;
                OnPropertyChanged(nameof(FriendRequestExists));
            }
        }

        public Visibility CanAddFriend
        {
            get
            {
                return _candAddFriend;
            }
            set
            {
                _candAddFriend = value;
                OnPropertyChanged(nameof(CanAddFriend));
            }
        }

        public Visibility FriendRequestSent
        {
            get
            {
                return _friendRequestSent;
            }
            set
            {
                _friendRequestSent = value;
                OnPropertyChanged(nameof(FriendRequestSent));
            }
        }

        public Visibility IsFriend
        {
            get
            {
                return _isFriend;
            }
            set
            {
                _isFriend = value;
                OnPropertyChanged(nameof(IsFriend));
            }
        }

        public int FriendRequestId
        {
            get { return _friendRequestId; }
            set
            {
                _friendRequestId = value;
                OnPropertyChanged(nameof(FriendRequestId));
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public string StatusColor
        {
            get { return _statusColor; }
            set
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public string DisplayName
        {
            get { return _displayName; }
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }
        
        public BitmapImage ProfilePicture
        {
            get { return _profilePicture; }
            set
            {
                _profilePicture = value;
                OnPropertyChanged(nameof(ProfilePicture));
            }
        }
        
        public UserStatus Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string JoinDate
        {
            get { return _joinDate; }
            set
            {
                _joinDate = value;
                OnPropertyChanged(nameof(JoinDate));
            }
        }

        public int Reputation
        {
            get { return _reputation; }
            set
            {
                _reputation = value;
                OnPropertyChanged(nameof(Reputation));
            }
        }

        public void UpdateStatusColor()
        {
            StatusColor = Status switch
            {
                UserStatus.Online => "GreenYellow",
                UserStatus.Away => "#FFC074",    // pale orange
                UserStatus.Offline => "#6E7582", // gray
                _ => "#6E7582",                  // default gray
            };
        }

        public void UpdateAvailability()
        {
            switch (_status)
            {
                case UserStatus.Online: _availability = "Online";
                    break;
                case UserStatus.Away: _availability = "Away";
                    break;
                case UserStatus.Offline: _availability = "Offline";
                    break;
            }
        }
    }
}
