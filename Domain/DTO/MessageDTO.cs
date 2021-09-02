namespace Domain.DTO
{
    public class MessageDTO : BaseModel
    {
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
