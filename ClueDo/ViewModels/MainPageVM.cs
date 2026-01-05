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
        public ICommand AddGameCommand { get; }
        public bool IsBusy => games.IsBusy;
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
        }
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
            OnPropertyChanged(nameof(IsBusy));
        }
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
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
