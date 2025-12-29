using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    public abstract class GamesModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected Game? currentGame;

        public bool IsBusy { get; set; }
        public Game? CurrentGame { get => currentGame; set => currentGame = value; }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        public int SelectedTotalPlayers { get; set; }
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        protected abstract void OnComplete(Task task);
        protected abstract void OnComplete(IQuerySnapshot qs);
        public abstract void AddGame();
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
    }
}
