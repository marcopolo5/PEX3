using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.WPF.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        public int Id { get; set; }
        public bool IsSent { get; set; }

        private string _textMessage;

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
