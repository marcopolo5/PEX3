using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ConversationPreviewDTO : BaseModel
    {
        private int _conversationId;
        private string _conversationName;
        private string _lastMessage;
        private string _accountProfilePicPath;
        private bool _isMessageUnread;
        private UserStatus _userStatus;

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

        public string AccountProfilePicPath 
        { 
            get
            {
                return _accountProfilePicPath;
            }
            set
            {
                _accountProfilePicPath = value;
                OnPropertyChanged(nameof(AccountProfilePicPath));
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
            }
        }
    }
}
