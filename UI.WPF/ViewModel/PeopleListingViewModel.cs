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
        private List<FriendViewModel> _peopleViewModels;
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
            FriendsCollectionView = CollectionViewSource.GetDefaultView(_peopleViewModels);
        }

        public async Task UsersView(string searchquery)
        {
            var users = await GetPersonViewModels(searchquery);
            foreach (FriendViewModel user in users)
            {
                _peopleViewModels.Add(user);
            }
            //FriendsCollectionView.Filter = FilterFriends;
        }

        private async Task<IEnumerable<FriendViewModel>> GetPersonViewModels(string searchquery)
        {
            var users = await _usersController.SearchUsers(searchquery);
            List<FriendViewModel> friendViewModels = new();
            foreach (User user in users)
            {
                friendViewModels.Add(new FriendViewModel(user.LastName + user.FirstName, user.Email, "Trebuie modificata baza de date sa suporte status message", "Assets/profile.png", user.Profile.Status));
            }

            return friendViewModels;
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
