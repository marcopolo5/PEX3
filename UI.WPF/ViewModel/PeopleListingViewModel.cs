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
    public class PeopleListingViewModel: ViewModelBase
    {
        private readonly List<FriendViewModel> _peopleViewModels;
        private readonly UsersController _usersController = new();
        public ICollectionView FriendsCollectionView { get; }
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
        public PeopleListingViewModel()
        {
            _peopleViewModels = new List<FriendViewModel>();
            foreach (FriendViewModel friendViewModel in GetPersonViewModels())
            {
                _peopleViewModels.Add(friendViewModel);
            }

            FriendsCollectionView = CollectionViewSource.GetDefaultView(_peopleViewModels);

            //FriendsCollectionView.Filter = FilterFriends;
        }
        private IEnumerable<FriendViewModel> GetPersonViewModels()
        {
            var users = _usersController.SearchUsers("t").Result;
            //yield return new FriendViewModel("Friend Test 8", "email8@test.com", "Some status message ...", "Assets/profile.png", Domain.UserStatus.Offline);
            foreach (User user in users)
            {
                yield return new FriendViewModel(user.LastName + user.FirstName, user.Email, "Trebuie modificata baza de date sa suporte status message", "Assets/profile.png", user.Profile.Status);

            }
        }
        private bool FilterFriends(object obj)
        {
            if (obj is FriendViewModel friendViewModel)
            {
                return friendViewModel.DisplayName.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase) ||
                    friendViewModel.Email.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }
    }
}
