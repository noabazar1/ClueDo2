using ClueDo.Models;
using ClueDo.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClueDo.ViewModels
{
    internal class GamePageVM(Game game) : ObservableObject 
    {
        private Game game = game;
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

        internal void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        internal void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
