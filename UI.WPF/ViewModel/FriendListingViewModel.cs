using AccountModule.Controllers;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using UI.WPF.Common;

namespace UI.WPF.ViewModel
{
    public class FriendListingViewModel : ViewModelBase
    {
        private readonly List<FriendViewModel> _friendViewModels;

        public ICollectionView FriendsCollectionView { get; }

        public FriendListingViewModel()
        {
            _friendViewModels = new List<FriendViewModel>();
            foreach (var friendViewModel in GetFriendViewModels())
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
                return  friendViewModel.DisplayName.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase) 
                        ||
                        friendViewModel.Email.Contains(FriendsFilter, StringComparison.InvariantCultureIgnoreCase);
            }
            return false;
        }

        private IEnumerable<FriendViewModel> GetFriendViewModels()
        {
            foreach (var friend in ApplicationUserController.CurrentUser.Friends.ToList())
            {
                yield return new FriendViewModel(friend.Id, friend.Profile.DisplayName, friend.Email, friend.Profile.StatusMessage, BitmapImageLoader.LoadImage(friend.Profile.Image), friend.Profile.Status, friend.JoinDate.ToString("dd.MM.yyyy"), friend.Profile.Reputation);
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
