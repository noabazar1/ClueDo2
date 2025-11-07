
using System.Collections.ObjectModel;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    internal class GamesModel
    {
        protected FbData fbd = new();
        protected Game? currentGame;
        public bool IsBusy { get; set; }
        public Game? CurrentGame { get; set; }
        public ObservableCollection<Game>? GamesList {  get; set; }
        public IList<GameSize>? GameSizes { get; set; } = [new GameSize(3), new GameSize(4), new GameSize(5)];
        public GameSize SelectedGameSize { get; set; } = new GameSize();
        public EventHandler<Game>? OnGameAdded;
    }
}
