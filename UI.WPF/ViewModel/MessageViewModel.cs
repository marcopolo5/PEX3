using Domain.Repositories;

namespace UI.WPF.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        private int _id;
        private string _textMessage;
        private string _displayName;
        public bool IsSent { get; set; }

        public int Id
        { 
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string DisplayName
        {
            get
            {
                if (IsSent)
                {
                    return "Me";
                }
                return _displayName;
            }
            set
            {
                _displayName = value;
                OnPropertyChanged(nameof(DisplayName));
            }
        }

        public string TextMessage
        {
            get 
            { 
                return _textMessage;
            }
            set 
            {
                _textMessage = value;
                OnPropertyChanged(nameof(TextMessage));
            }
        }
        public string ToggleButtonVisibility
        {
            get
            {
                if (IsSent)
                {
                    return "Hidden";
                }
                return "Visible";
            }
        }
        public string Positioning
        {
            get
            {
                if (IsSent)
                    return "0,0 30 0 0,15"; // polygon on the right
                else
                    return "0,0 30 0 25,15"; // polygon on the left
            }
        }

        public string Margins
        {
            get
            {
                if (IsSent)
                    return "0,20,26,0"; // polygon on the right
                else
                    return "0,20,0,0"; // polygon on the left
            }
        }

        public string Alignment
        {
            get
            {
                if (IsSent)
                    return "Right"; // polygon on the right
                else
                    return "Left"; // polygon on the left
            }
        }

    }
}
