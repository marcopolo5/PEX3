using AccountModule.Controllers;
using Domain.Models;
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


        private IEnumerable<FriendViewModel> GetFriendViewModels()
        {
            //yield return new FriendViewModel("Friend Test 8", "email8@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            foreach (User friend in ApplicationUserController.CurrentUser.Friends.ToList())
            {
                yield return new FriendViewModel(friend.LastName + friend.FirstName, friend.Email, "Trebuie modificata baza de date sa suporte status message", "Assets/profile.png", friend.Profile.Status);

            }
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
