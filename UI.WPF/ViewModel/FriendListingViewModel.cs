using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace UI.WPF.ViewModel
{
    public class FriendListingViewModel : ViewModelBase
    {
        private readonly List<FriendViewModel> _friendViewModels;
        public ICollectionView FriendsCollectionView { get; }

        public FriendListingViewModel()
        {
            _friendViewModels = new List<FriendViewModel>();
            foreach(FriendViewModel friendViewModel in GetFriendViewModels())
            {
                _friendViewModels.Add(friendViewModel);
            }
            
            FriendsCollectionView = CollectionViewSource.GetDefaultView(_friendViewModels);

            FriendsCollectionView.Filter = FilterFriends;
        }

        private bool FilterFriends(object obj)
        {
            if(obj is FriendViewModel friendViewModel)
            {
                return friendViewModel.DisplayName.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase) ||
                    friendViewModel.Email.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        // Dummy data
        private IEnumerable<FriendViewModel> GetFriendViewModels()
        {
            yield return new FriendViewModel("Friend Test 1", "email1@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 2", "email2@test.com", "Some status message dummy status message", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 2", "email2@test.com", "Some status message dummy status message", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 2", "email2@test.com", "Some status message dummy status message", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 2", "email2@test.com", "Some status message dummy status message", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 2", "email2@test.com", "Some status message dummy status message", "Assets/profile.png", Domain.UserStatus.Online);
            yield return new FriendViewModel("Friend Test 3", "email3@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Away);
            yield return new FriendViewModel("Friend Test 4", "email4@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Away);
            yield return new FriendViewModel("Friend Test 5", "email5@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Away);
            yield return new FriendViewModel("Friend Test 5", "email5@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Away);
            yield return new FriendViewModel("Friend Test 6", "email6@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            yield return new FriendViewModel("Friend Test 7", "email7@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            yield return new FriendViewModel("Friend Test 7", "email7@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            yield return new FriendViewModel("Friend Test 6", "email6@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            yield return new FriendViewModel("Friend Test 8", "email8@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
        }

        private string _friendsFilter = string.Empty;
        public string FriendsFilter
        {
            get { return _friendsFilter; }
            set
            {
                _friendsFilter = value;
                OnPropertyChanged(nameof(FriendsFilter));
                FriendsCollectionView.Refresh();
            }
        }
    }
}
