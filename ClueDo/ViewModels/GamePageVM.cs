using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public string MyName => game.MyName;
        public string OpponentName => game.OpponentName;
        public GamePageVM(Game game)
        {
            game.OnGameChanged += OnGameChanged;
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(OpponentName));
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
