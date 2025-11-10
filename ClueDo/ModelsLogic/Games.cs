using ClueDo.Models;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    internal class Games : GamesModel
    {
        internal void AddGame()
        {
            IsBusy = true;
            currentGame = new()
            {
                IsHostUser = true
            };
            currentGame.OnGameDeleted += OnGameDeleted;
            currentGame.SetDocument(OnComplete);
        }

        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Toast.Make(Strings.GameDeleted, CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
            });
        }

        private void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, currentGame!);
        }
        private void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereEqualTo(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange?);
        }

        public override void RemoveSnapshotListener()
        {
            
        }
    }
}
