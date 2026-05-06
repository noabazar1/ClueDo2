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
        /// <summary>
        /// method to add a new game to the games collection in the database. This method is called when 
        /// the user clicks the "Add Game" button, and it creates a new game document in the games 
        /// collection with the current user's name as the host. The method sets the IsBusy property to true
        /// while the game is being created, and it calls the OnComplete method when the task is completed 
        /// to handle the result of the operation. 
        /// </summary>
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
        /// <summary>
        /// method to handle changes in the games collection in the database. This method is called when a 
        /// change is detected in the games collection, and it updates the GamesList property with the new 
        /// data from the snapshot and raises the OnGamesChanged event to notify the UI of the changes. 
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="error"></param>
        protected override void OnChange(IQuerySnapshot snapshot, Exception error)
        {
            OnComplete(snapshot);
        }
        /// <summary>
        /// method to handle the completion of tasks related to adding games and listening for changes in 
        /// the games collection. This method is called when a task is completed, and it checks the result
        /// of the task to determine whether the operation was successful or if there was an error. If the
        /// task is completed successfully and a new game was added, it raises the OnGameAdded event to 
        /// notify the UI of the new game. If there was an error, it retrieves the error message and 
        /// displays it as a toast notification to the user. The method also sets the IsBusy property to
        /// false to indicate that the operation has completed.
        /// </summary>
        /// <param name="task"></param>
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
        /// <summary>
        /// method to handle the completion of the initial snapshot listener for the games collection. This
        /// method is called when the initial snapshot is received, and it updates the GamesList property 
        /// with the data from the snapshot and raises the OnGamesChanged event to notify the UI that the 
        /// initial data has been loaded.
        /// </summary>
        /// <param name="qs"></param>
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