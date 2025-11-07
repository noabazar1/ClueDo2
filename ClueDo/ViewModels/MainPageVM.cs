using System.Collections.ObjectModel;
using System.Windows.Input;
using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Views;

namespace ClueDo.ViewModels
{
    internal class MainPageVM:ObservableObject
    {
        private Games games = new();
        public bool IsBusy => games.IsBusy;
        public  IList<GameSize>? GameSizes { get => games.GameSizes; set => games.GameSizes = value; }
        public GameSize SelectedGameSize { get => games.SelectedGameSize; set => games.SelectedGameSize = value; }
        public ICommand AddGameCommand => new Command(AddGame);
        public Game SelectedItem
        {
            get => games.CurrentGame;
            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(value), true);
                    });
                }
            }
        }
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }
        public IList<Game>? GamesList => games.GamesList;
        public MainPageVM()
        {
            games.OnGameAdded += OnGameAdded;
        }

        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PushAsync(new GamePage(game), true);
            });
        }
    }
}
