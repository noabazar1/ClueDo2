using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    public partial class MainPageVM : ObservableObject
    {
        private readonly Games games = new();
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool isAlertShown = false;
        public ICommand AddGameCommand { get; }
        public ICommand ShowNoInternetCommand { get; }
        public bool IsBusy => games.IsBusy;
        public bool IsConnected => _connectivity.IsConnected;
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public Game? SelectedItem
        {
            get => games.CurrentGame;
            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    value.JoinGame();
                    MainThread.InvokeOnMainThreadAsync( () =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(value));
                    });
                }
            }
        }
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }
        public MainPageVM()
        {
            AddGameCommand = new Command(AddGame);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
            OnPropertyChanged(nameof(IsBusy));
        }
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new GamePage(game));
            });
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
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
    }
}
