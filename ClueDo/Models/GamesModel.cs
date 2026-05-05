using Plugin.CloudFirestore;
using System.Collections.ObjectModel;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the data of the games, and the methods to add and remove games, and to listen to
    /// changes in the games collection in the database.
    /// </summary>
    public abstract class GamesModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected Game? currentGame;
        public EventHandler<Game>? OnGameAdded;
        public EventHandler? OnGamesChanged;
        public bool IsBusy { get; set; }
        public Game? CurrentGame
        {
            get => currentGame;
            set => currentGame = value;
        }
        public ObservableCollection<Game>? GamesList { get; set; } = [];
        /// <summary>
        /// abstract method to add a new game to the database, which will be implemented in the Games
        /// class.
        /// </summary>
        public abstract void AddGame();
        /// <summary>
        /// abstract method to remove the snapshot listener from the games collection in the database,
        /// which will be implemented in the Games class. This method is used to stop listening to changes
        /// in the games collection when the user leaves the games page.
        /// </summary>
        public abstract void RemoveSnapshotListener();
        /// <summary>
        /// abstract method to add a snapshot listener to the games collection in the database, which will
        /// be implemented in the Games class. This method is used to listen to changes in the games 
        /// collection when the user enters the games page. When a change is detected, the OnChange method
        /// will be called, which will update the GamesList property and raise the OnGamesChanged event to 
        /// notify the UI of the changes. The OnComplete method will be called when the initial snapshot 
        /// is received, which will set the IsBusy property to false and raise the OnGamesChanged event 
        /// to notify the UI that the initial data has been loaded.
        /// </summary>
        public abstract void AddSnapshotListener();
        /// <summary>
        /// abstract method to handle changes in the games collection in the database, which will be 
        /// implemented in the Games class. This method will be called when a change is detected in the 
        /// games collection, and it will update the GamesList property and raise the OnGamesChanged event
        /// to notify the UI of the changes. 
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="error"></param>
        protected abstract void OnChange(IQuerySnapshot snapshot, Exception error);
        /// <summary>
        /// abstract method to handle the completion of the initial snapshot listener, which will be 
        /// implemented in the Games class. This method will be called when the initial snapshot is 
        /// received, and it will set the IsBusy property to false and raise the OnGamesChanged event to
        /// notify the UI that the initial data has been loaded.
        /// </summary>
        /// <param name="task"></param>
        protected abstract void OnComplete(Task task);
        /// <summary>
        /// abstract method to handle the completion of the GetDocumentsWhereEqualTo method, which will be
        /// implemented in the Games class. This method will be called when the documents are 
        /// successfully retrieved, and it will update the GamesList property and raise the OnGamesChanged
        /// event to notify the UI of the changes.
        /// </summary>
        /// <param name="qs"></param>
        protected abstract void OnComplete(IQuerySnapshot qs);
    }
}