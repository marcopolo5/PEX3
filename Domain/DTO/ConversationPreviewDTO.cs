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
