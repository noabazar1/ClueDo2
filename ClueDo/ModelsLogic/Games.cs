using ClueDo.Models;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    public class Games : GamesModel
    {
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            fbd.GetDocumentsWhereLessThan(Keys.GamesCollection, nameof(GameModel.IsFull), false, OnComplete);
        }
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            if (task.IsCompletedSuccessfully)
                OnGameAdded?.Invoke(this, currentGame!);
            else if (task.IsFaulted && task.Exception != null)
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(fbd.GetErrorMessage(task.Exception.Message), CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                });
            }
        }
        protected override void OnComplete(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            if (qs.Documents.Count() > 0)
            {
                IDocumentSnapshot ds = qs.Documents.FirstOrDefault()!;
                Game? game = ds.ToObject<Game>();
            }
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    game.Id = ds.Id;
                    GamesList.Add(game);
                }
            }
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
        }
        public override void AddGame()
        {
            IsBusy = true;
            currentGame = new()
            {
                IsHostUser = true
            };
            currentGame.SetDocument(OnComplete);
        }
    }
}