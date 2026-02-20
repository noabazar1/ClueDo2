using ClueDo.Models;
using ClueDo.Services;
using System.Collections.ObjectModel;
using Microsoft.Maui.ApplicationModel.Communication;
using System.Windows.Input;
namespace ClueDo.ViewModels
{
    public class FriendsPageVM
    {
        private readonly IContactsService _contactsService;
        public ObservableCollection<FriendContact> Contacts { get; private set; }
        public ICommand AddFriendCommand { get; }
        public FriendsPageVM(IContactsService contactsService)
        {
            _contactsService = contactsService;
            Contacts = new ObservableCollection<FriendContact>();
            AddFriendCommand = new Command(async () => await AddFriend());
        }
        private async Task AddFriend()
        {
            FriendContact? friend = await _contactsService.PickContactAsync();
            if(friend == null) 
                return;
            if(Contacts.Any(f => f.Phone == friend.Phone))
                return;
            Contacts.Add(friend);
        }
    }
}
