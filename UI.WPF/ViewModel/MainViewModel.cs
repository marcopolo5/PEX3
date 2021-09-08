namespace UI.WPF.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public FriendListingViewModel FriendListingViewModel { get; }

        public MainViewModel()
        {
            FriendListingViewModel = new FriendListingViewModel();
        }

    }
}
