using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class FriendDTO : BaseModel
    {
        private string _statusColor;
        private string _displayName;
        private string _statusMessage;
        private string _imagePath;
        private UserStatus _status;


        public string StatusColor
        {
            get
            {
                return _statusColor;
            }
            set
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string StatusMessage
        {
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = StatusMessage;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public string ImagePath
        {
            get
            {
                return _imagePath;
            }
            set
            {
                _imagePath = value;
                OnPropertyChanged(nameof(ImagePath));
            }
        }

        public UserStatus Status
        {
            get
            {
                return Status;
            }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }


    }
}
