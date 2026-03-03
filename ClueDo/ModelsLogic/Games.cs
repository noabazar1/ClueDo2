using Plugin.CloudFirestore;
using ClueDo.Models;
using ClueDo.ModelsLogic;

namespace ClueDo.ModelsLogic
{
    public class Games : GamesModel
    {
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            OnComplete(snapshot);
        }
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            if (task.IsCompletedSuccessfully)
                OnGameAdded?.Invoke(this, currentGame!);
            //else if (task.IsFaulted && task.Exception != null)
            //{
            //    MainThread.InvokeOnMainThreadAsync(() =>
            //    {
            //        Toast.Make(fbd.GetErrorMessage(task.Exception.Message), CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
            //    });
            //}
        }
        protected override void OnComplete(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            //if(qs.Documents.Count() >0)
            //{
            //    IDocumentSnapshot ds = qs.Documents.FirstOrDefault()!;
            //    Game? game = ds.ToObject<Game>();
            //}
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    if (game.IsStarted ||game.IsGameOver)
                        continue;

                    game.Id = ds.Id;
                    Console.WriteLine($"Game: {game!.Id}  IsGameOver: {game.IsGameOver}");
                    GamesList.Add(game);
                }
            }
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }
        public override void AddSnapshotListener()
        {
            if (ilr != null)
                return;

            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }

        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            ilr = null;
        }
        public override void AddGame()
        {
            IsBusy = true;

            string myUserId = new User().Name;

            currentGame = new Game([])
            {
                HostName = myUserId,
                HostId = myUserId,
                IsHostUser = true,
                Created = DateTime.Now
            };

            currentGame.SetDocument(task =>
            {
                if (task.IsCompletedSuccessfully)
                {
                    currentGame.EnsureAnswerGenerated(myUserId);
                }

                OnComplete(task);
            });
        }
    }
}