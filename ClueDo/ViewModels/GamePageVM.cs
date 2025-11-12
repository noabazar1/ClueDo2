using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public string MyName => game.MyName;
        public string Player1 => game.Player1;
        public string Player2 => game.Player2;
        public string Player3 => game.Player3;
        public string Player4 => game.Player4;
        public string Player5 => game.Player5;

        public GamePageVM(Game game)
        {
            game.OnGameChanged += OnGameChanged;
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Player1));
            OnPropertyChanged(nameof(Player2));
            OnPropertyChanged(nameof(Player3));
            OnPropertyChanged(nameof(Player4));
            OnPropertyChanged(nameof(Player5));
        }

        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError, CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

        }

        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
