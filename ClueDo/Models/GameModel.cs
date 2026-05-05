using ClueDo.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace ClueDo.Models
{
    /// <summary>
    /// class that represents the game model, which holds all the properties and methods related to the
    /// game logic and state. It is an abstract class that will be implemented by the Game class in the
    /// ModelsLogic folder. The GameModel class serves as the base class for the game logic and state
    /// management, and it will be extended by the Game class to provide the specific implementation of
    /// the game mechanics and interactions with the database.
    /// </summary>
    public abstract class GameModel
    {
        protected enum Actions { Changed, Deleted }
        protected Actions action = Actions.Changed;
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        protected string[,]? gameBoard;
        protected IndexButton[,] gameButtons = new IndexButton[15, 15];
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? GameError;
        [Ignored]
        protected abstract GameStatus Status { get; }
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        [Ignored]
        public int PlayersCount => Players.Count;
        [Ignored]
        public int TotalPlayers => Players.TotalPlayers;
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public int CurrentTurnIndex { get; set; }
        public bool IsHostTurn { get; set; } = false;
        [Ignored]
        public string MyName { get; set; } = new User().Name;
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public int MyIndex { get; protected set; } = 0;
        public int CurrentPlayers { get; set; } = 1;
        public int NextPlay { get; set; }
        [Ignored]
        public abstract string JoinStatus { get; }
        public List<string> PlayersNames { get; set; } = [];
        public string DiceResult { get; set; } = string.Empty;
        public Players Players { get; set; } = new();
        public string HostId { get; set; } = string.Empty;
        public Answer? Answer { get; set; }
        public bool IsStarted { get; set; }
        public bool IsGameOver { get; set; }
        public string? WinnerName { get; set; }
        /// <summary>
        /// abstract method for setting the document in the database, it takes an Action as a parameter
        /// that will be called when the operation is complete. The method will be implemented in the Game
        /// class to set the game document in the database with the current state of the game. The Action
        /// parameter will allow the caller to specify a callback function that will be executed once the
        /// document is successfully set in the database, allowing for any necessary updates to the UI or
        /// game state to be made after the database operation is complete.
        /// </summary>
        /// <param name="OnComplete"></param>
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// abstract method for removing the snapshot listener from the database, it will be called when
        /// the game is deleted or when the user leaves the game. The method will be implemented in the
        /// Game class to remove the snapshot listener that was added to listen for changes in the game 
        /// document in the database.
        /// </summary>
        public abstract void RemoveSnapshotListener();
        /// <summary>
        /// abstract method for adding a snapshot listener to the game document in the database, it will
        /// be called when the game is created or when the user joins the game. The method will be
        /// implemented in the Game class to add a snapshot listener to the game document in the database,
        /// allowing the game to listen for changes in the game state and update the UI accordingly. The
        /// snapshot listener will be used to listen for changes in the game document, such as updates to
        /// the game state, player actions, and game status, and it will trigger the appropriate events
        /// and updates in the game logic and UI when changes are detected. 
        /// </summary>
        public abstract void AddSnapshotListener();
        /// <summary>
        /// abstract method for joining a game, it will be called when the user wants to join an existing
        /// game. The method will be implemented in the Game class to allow the user to join an existing
        /// game by adding their information to the game document in the database and updating the game 
        /// state accordingly. The method will also handle any necessary checks to ensure that the game
        /// is not already full and that the user is not already part of the game before allowing them to
        /// join. Once the user successfully joins the game, the method will update the game state and 
        /// trigger any necessary events to notify other players of the new player joining the game and 
        /// to update the UI accordingly. 
        /// </summary>
        public abstract void JoinGame();
        /// <summary>
        /// abstract method for getting the position of a player, it takes the player's index as a 
        /// parameter and returns the player's position on the game board. The method will be implemented
        /// in the Game class to retrieve the position of a player based on their index in the Players
        /// list. The player's position will be represented as a Position object, which contains the row
        /// and column coordinates of the player's current location on the game board. 
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public abstract Position GetPlayerPosition(int playerIndex);
        /// <summary>
        /// abstract method for getting the color of a player, it takes the player's index as a parameter and
        /// returns the player's color on the game board. The method will be implemented in the Game class to
        /// retrieve the color of a player based on their index in the Players list.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public abstract Color GetPlayerColor(int playerIndex);
        /// <summary>
        /// abstract method for getting the name of a player, it takes the player's index as a parameter and
        /// returns the player's name on the game board. The method will be implemented in the Game class to
        /// retrieve the name of a player based on their index in the Players list.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public abstract string GetPlayerName(int playerIndex);
        /// <summary>
        /// abstract method for deleting the game document from the database, it takes an Action as a 
        /// parameter that will be called when the operation is complete. The method will be implemented
        /// in the Game class to delete the game document from the database when the game is over or when
        /// the host decides to end the game. The Action parameter will allow the caller to specify a 
        /// callback function that will be executed once the document is successfully deleted from the
        /// database, allowing for any necessary cleanup of resources or updates to the UI to be made 
        /// after the database operation is complete. 
        /// </summary>
        /// <param name="OnComplete"></param>
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        /// <summary>
        /// abstract method for handling the click event of the buttons on the game board, it takes the
        /// sender and EventArgs as parameters. The method will be implemented in the Game class to handle
        /// the click events of the buttons on the game board, allowing players to interact with the game
        /// by clicking on the buttons to move their pieces, make suggestions, or perform other game
        /// actions. The sender parameter will provide information about which button was clicked, and the
        /// EventArgs parameter will provide any additional information about the event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public abstract void OnButtonClicked(object? sender, EventArgs e);
        /// <summary>
        /// abstract method for eliminating the current player from the game,  The method will handle
        /// the necessary logic to remove the player from the game, update the game state, and notify
        /// other players of the elimination.
        /// </summary>
        public abstract void EliminateCurrentPlayer();
        /// <summary>
        /// abstract method for adding a player to the game, it takes the player's name as a parameter
        /// and returns a boolean indicating whether the player was successfully added.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract bool AddPlayer(string name);
        /// <summary>
        /// abstract method for checking if it is the current player's turn, it returns a boolean indicating
        /// whether it is the current player's turn. The method will be implemented in the Game class to
        /// check if it is the current player's turn base on the game state and the player's index in 
        /// the Players list. The method will compare the player's index with the NextPlay property to
        /// determine if it is their turn, and it will return true if it is their turn and false otherwise. 
        /// </summary>
        /// <returns></returns>
        public abstract bool IsMyTurn();
        /// <summary>
        /// abstract method for setting the next player in the game, it will be called at the end of
        /// each turn to update the NextPlay property and determine which player's turn is next. 
        /// The method will be implemented in the Game class to update the NextPlay property based on the
        /// current player's index and the total number of players in the game. 
        /// </summary>
        /// <param name="oponnentIndex"></param>
        /// <returns></returns>
        public abstract bool IsOpponentTurn(int oponnentIndex);
        /// <summary>
        /// abstract method for checking the validity of a room, weapon, or suspect in the game, it takes
        /// a string parameter representing the room, weapon, or suspect to be checked and returns a 
        /// boolean indicating whether the provided string is valid or not. The method will be implemented
        /// in the Game class to check if the provided string matches any of the valid rooms, weapons,
        /// or suspects in the game. This method will be used to validate player suggestions and 
        /// accusations, ensuring that players can only suggest or accuse valid rooms, weapons, 
        /// and suspects according to the rules of the game.
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public abstract bool CheckRoom(string room);
        /// <summary>
        /// abstract method for checking the validity of a weapon in the game, it takes a string parameter
        /// representing the weapon to be checked and returns a boolean indicating whether the provided
        /// string is valid or not. The method will be implemented in the Game class to check if the 
        /// provided string matches any of the valid weapons in the game. 
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public abstract bool CheckWeapon(string weapon);
        public abstract bool CheckSuspect(string suspect);
        public abstract void EnsureAnswerGenerated(string myUserId);
        public abstract void DrawAllPlayers();
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        public abstract void PlacePlayer(int playerIndex, int row, int col);
        public abstract void InitBoard(Grid grid);
        public abstract void EndTurnAfterSuggestion();
        public abstract void RollDiceForCurrentPlayer();
        public abstract void EndGame();
        public abstract bool HandleIncomingCallResult(bool success);
        public abstract List<int> GenerateDiceFrames(int totalFrames, long totalTime, long interval);
        public abstract (bool roomCorrect, bool weaponCorrect, bool suspectCorrect, bool isWin) CheckAccusation
    (Accusation accusation);
        protected abstract void Play(int rowIndex, int columnIndex);
        protected abstract void UpdateStatus();
        protected abstract void UpdateFbMove();
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
    }
}