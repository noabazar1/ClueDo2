using ClueDo.Models;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that manages the games collection in the database, allowing users to create new games and 
    /// listen for changes in the games collection. The Games class inherits from the GamesModel class, 
    /// which defines the properties and methods for managing the games collection. The Games class 
    /// implements the methods for adding a new game to the collection, adding and removing snapshot 
    /// listeners to listen for changes in the games collection, and handling the completion of tasks 
    /// related to adding games and listening for changes. The Games class also defines events for when a game is added and when the games collection changes, which can be used to update the user interface accordingly.
    /// </summary>
    public class Games : GamesModel
    {
        /// <summary>
        /// method to add a snapshot listener to the games collection in the database. This method is 
        /// called when the user enters the games page, and it listens for changes in the games collection.
        /// When a change is detected, the OnChange method is called, which updates the GamesList property 
        /// and raises the OnGamesChanged event to notify the UI of the changes. The OnComplete method is
        /// called when the initial snapshot is received, which sets the IsBusy property to false and raises
        /// the OnGamesChanged event to notify the UI that the initial data has been loaded.
        /// </summary>
        public override void AddSnapshotListener()
        {
            ilr ??= fbd.AddSnapshotListener(Keys.GamesCollection, OnChange!);
        }
        /// <summary>
        /// method to remove the snapshot listener from the games collection in the database. This method 
        /// is called when the user leaves the games page, and it stops listening for changes in the games
        /// collection to prevent memory leaks and unnecessary updates to the UI.  
        /// </summary>
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
                    currentGame.EnsureAnswerGenerated(myUserId);
                OnComplete(task);
            });
        }
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            OnComplete(snapshot);
        }
        protected override void OnComplete(Task task)
        {
            IsBusy = false;
            if (task.IsCompletedSuccessfully)
                OnGameAdded?.Invoke(this, currentGame!);
            else if (task.IsFaulted && task.Exception != null)
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Toast.Make(fbd.GetErrorMessage(task.Exception.Message), CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                });
        }
        protected override void OnComplete(IQuerySnapshot qs)
        {
            GamesList!.Clear();
            foreach (IDocumentSnapshot ds in qs.Documents)
            {
                Game? game = ds.ToObject<Game>();
                if (game != null)
                {
                    if (game.IsStarted || game.IsGameOver)
                        continue;
                    game.Id = ds.Id;
                    GamesList.Add(game);
                }
            }
            OnGamesChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}