using System.Collections.ObjectModel;
using System.Windows.Input;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ViewModels
{
    internal class MainPageVM:ObservableObject
    {
        private Games games = new();
        public bool IsBusy => games.IsBusy;
        public  IList<GameSize>? GameSizes { get => games.GameSizes; set => games.GameSizes = value; }
        public GameSize SelectedGameSize { get => games.SelectedGameSize; set => games.SelectedGameSize = value; }
        public ICommand AddGameCommand => new Command(AddGame);

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

        private void OnGameAdded(object? sender, bool e)
        {
            OnPropertyChanged(nameof(IsBusy));
        }
    }
}
