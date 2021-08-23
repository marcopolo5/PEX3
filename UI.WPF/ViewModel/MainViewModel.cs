using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
