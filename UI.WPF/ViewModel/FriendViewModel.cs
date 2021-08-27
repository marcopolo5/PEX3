using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FriendViewModel(string displayName, string email, string statusMessage, BitmapImage image, UserStatus status)
        {
            _status = status;
            UpdateStatusColor();
            _displayName = displayName;
            _email = email;
            _statusMessage = statusMessage;
            _profilePicture = image;
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
    }
}
