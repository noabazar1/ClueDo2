using ClueDo.Models;
using ClueDo.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace ClueDo.ViewModels
{
    public class FriendsPageVM
    {
        private readonly IContactsService _contactsService;
        private readonly IFriendsService _friendsService;

        public ObservableCollection<FriendContact> Contacts { get; set; }

        public ICommand AddFriendCommand { get; }
        public ICommand LoadFriendsCommand { get; }

        public FriendsPageVM(IContactsService contactsService,
                             IFriendsService friendsService)
        {
            _contactsService = contactsService;
            _friendsService = friendsService;

            Contacts = new ObservableCollection<FriendContact>();

            AddFriendCommand = new Command(async () => await AddFriend());
            LoadFriendsCommand = new Command(async () => await LoadFriends());
        }

        public async Task LoadFriends()
        {
            List<FriendContact> friends =
                await _friendsService.GetFriendsAsync();

            Contacts.Clear();

            foreach (FriendContact friend in friends)
            {
                Contacts.Add(friend);
            }
        }

        private async Task AddFriend()
        {
            FriendContact? friend =
                await _contactsService.PickContactAsync();

            if (friend == null)
                return;

            bool alreadyExists =
                Contacts.Any(f => f.Phone == friend.Phone);

            if (alreadyExists)
                return;

            Contacts.Add(friend);

            await _friendsService.AddFriendAsync(friend);
        }
    }
}
