using ClueDo.Models;
using ClueDo.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    public partial class FriendsPageVM : ObservableObject
    {
        private readonly IContactsService _contactsService;
        private readonly IFriendsService _friendsService;
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool isAlertShown = false;
        public ObservableCollection<FriendContact> Contacts { get; set; }
        public ICommand AddFriendCommand { get; }
        public ICommand LoadFriendsCommand { get; }
        public ICommand ShowNoInternetCommand { get; }
        public bool IsConnected => _connectivity.IsConnected;
        public FriendsPageVM(IContactsService contactsService, IFriendsService friendsService)
        {
            _contactsService = contactsService;
            _friendsService = friendsService;
            Contacts = [];
            AddFriendCommand = new Command(async () => await AddFriend());
            LoadFriendsCommand = new Command(async () => await LoadFriends());
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        public async Task LoadFriends()
        {
            List<FriendContact> friends = await _friendsService.GetFriendsAsync();
            Contacts.Clear();
            foreach (FriendContact friend in friends)
                Contacts.Add(friend);
        }
        private async Task AddFriend()
        {
            FriendContact? friend = await _contactsService.PickContactAsync();
            if (friend != null)
            {
                bool alreadyExists = Contacts.Any(f => f.Phone == friend.Phone);
                if (!alreadyExists)
                {
                    Contacts.Add(friend);
                    await _friendsService.AddFriendAsync(friend);
                }
            }
        }
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
    }
}
