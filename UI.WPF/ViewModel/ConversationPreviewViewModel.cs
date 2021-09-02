using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Domain;
using System.Windows.Media.Imaging;
using UI.WPF.View;
using UI.WPF.Common;

namespace UI.WPF.ViewModel
{
    public class ConversationPreviewViewModel : ViewModelBase
    {
        private int _conversationId;
        private string _conversationName;
        private string _lastMessage;
        private byte[] _accountProfilePictureArray;
        private string _statusMessage;
        private bool _isMessageUnread;
        private UserStatus _userStatus;

        public string StatusMessage
        { 
            get
            {
                return _statusMessage;
            }
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }


        public int ConversationId
        {
            get
            {
                return _conversationId;
            }
            set
            {
                _conversationId = value;
                OnPropertyChanged(nameof(ConversationId));
            }
        }

        public string ConversationName
        {
            get
            {
                return _conversationName;
            }
            set
            {
                _conversationName = value;
                OnPropertyChanged(nameof(ConversationName));
            }
        }

        public string LastMessage
        {
            get
            {
                return _lastMessage;
            }
            set
            {
                _lastMessage = value;
                OnPropertyChanged(nameof(LastMessage));
            }
        }

        public BitmapImage AccountProfilePicture 
        { 
            get
            {
                return BitmapImageLoader.LoadImage(_accountProfilePictureArray);
            }
        }

        public byte[] AccountProfilePictureArray
        {
            get
            {
                return _accountProfilePictureArray;
            }
            set
            {
                _accountProfilePictureArray = value;
                OnPropertyChanged(nameof(AccountProfilePictureArray));
                OnPropertyChanged(nameof(AccountProfilePicture));
            }
        }


        public bool UnreadMessage 
        {
            get 
            {
                return _isMessageUnread;
            }
            set 
            {
                _isMessageUnread = value;
                OnPropertyChanged(nameof(UnreadMessage));
                OnPropertyChanged(nameof(MessageColour));
            }
        }

        public UserStatus UserStatus
        {
            get
            {
                return _userStatus;
            }
            set
            {
                _userStatus = value;
                OnPropertyChanged(nameof(UserStatus));
                OnPropertyChanged(nameof(NameColour));
            }
        }

        public string NameColour
        {
            get
            {
                if (_userStatus == UserStatus.Offline)
                {
                    return "#FF838383";//gray
                }
                if (_userStatus == UserStatus.Online)
                {
                    return "#00FF00";//green
                }
                return "#fae12c"; //yellow
            }
        }

        public string MessageColour
        {
            get
            {
                if (_isMessageUnread)
                {
                    return "#FFFFFF";//white
                }
                return "#FF838383";//gray
            }
        }
    }
}
