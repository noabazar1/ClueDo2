using ClueDo.Models;
using ClueDo.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that represents the view model for the friends page, which is responsible for managing the
    /// user's friends list. It interacts with the IContactsService to allow the user to pick a contact 
    /// from their device's contact list and add it to their friends list, and with the IFriendsService to
    /// retrieve the list of friends and add new friends to it. 
    /// </summary>
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
        public ICommand DeleteFriendCommand { get; }
        public bool IsConnected => _connectivity.IsConnected;
        /// <summary>
        /// constructor for the FriendsPageVM class, which initializes the services, the contacts 
        /// collection, and the commands. It also subscribes to the ConnectivityChanged event of the 
        /// connectivity model to listen to changes in the internet connectivity and show an alert if the
        /// device is not connected to the internet when trying to load the friends list or add a new 
        /// friend.
        /// </summary>
        /// <param name="contactsService"></param>
        /// <param name="friendsService"></param>
        public FriendsPageVM(IContactsService contactsService, IFriendsService friendsService)
        {
            _contactsService = contactsService;
            _friendsService = friendsService;
            Contacts = [];
            AddFriendCommand = new Command(async () => await AddFriend());
            LoadFriendsCommand = new Command(async () => await LoadFriends());
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
            DeleteFriendCommand = new Command<FriendContact>(async (friend) => await DeleteFriend(friend));
        }
        /// <summary>
        /// method that loads the user's friends list by calling the GetFriendsAsync method of the 
        /// IFriendsService, and updates the Contacts collection with the retrieved friends. If the device
        /// is not connected to the internet, it shows an alert to the user indicating that they need to 
        /// check their connection. This method is called when the user navigates to the friends page, and
        /// it is also called when the user tries to add a new friend, to ensure that the friends list is
        /// always up to date. 
        /// </summary>
        /// <returns></returns>
        public async Task LoadFriends()
        {
            List<FriendContact> friends = await _friendsService.GetFriendsAsync();
            Contacts.Clear();
            foreach (FriendContact friend in friends)
                Contacts.Add(friend);
        }
        /// <summary>
        /// method that allows the user to add a new friend by picking a contact from their device's 
        /// contact list.
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// method that is called when the connectivity changes, which updates the IsConnected property and
        /// shows an alert if the device is not connected to the internet and the alert is not already 
        /// shown. This method is subscribed to the ConnectivityChanged event of the connectivity model in
        /// the constructor, and it is called whenever the connectivity changes, such as when the device 
        /// connects or disconnects from the internet. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        /// <summary>
        /// method that shows an alert to the user indicating that they need to check their internet 
        /// connection. This method is called when the user tries to load the friends list or add a new 
        /// friend while the device is not connected to the internet, and it is also called when the 
        /// connectivity changes and the device becomes disconnected from the internet. The method sets the
        /// isAlertShown flag to true to prevent multiple alerts from being shown at the same time, and it
        /// resets the flag to false after the alert is dismissed.
        /// </summary>
        /// <returns></returns>
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
        private async Task DeleteFriend(FriendContact friend)
        {
            if (friend == null)
                return;
            await _friendsService.DeleteFriendAsync(friend.Id!);
            Contacts.Remove(friend);
        }
    }
}
