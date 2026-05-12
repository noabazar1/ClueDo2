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
        public Answer? Answer { get; set; } = new Answer();
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
        /// <summary>
        /// abstract method for checking the validity of a suspect in the game, it takes a string parameter
        /// representing the suspect to be checked and returns a boolean indicating whether the provided
        /// string is valid or not. The method will be implemented in the Game class to check if the
        /// provided string matches any of the valid suspects in the game.
        /// </summary>
        /// <param name="suspect"></param>
        /// <returns></returns>
        public abstract bool CheckSuspect(string suspect);
        /// <summary>
        /// abstract method for drawing all the players on the game board, it will be called to update the
        /// UI. it will be implemented in the Game class to draw all the players on the game board based
        /// on their current positions and colors. 
        /// </summary>
        public abstract void DrawAllPlayers();
        /// <summary>
        /// abstract method for updating the guest user's information in the game, it takes an Action as
        /// a parameter that will be called when the operation is complete. The method will be implemented
        /// in the Game class to update the guest user's information in the game document in the database,
        /// allowing for any necessary updates to the game state or UI to be made after the database
        /// operation is complete. 
        /// </summary>
        /// <param name="OnComplete"></param>
        public abstract void UpdateGuestUser(Action<Task> OnComplete);
        /// <summary>
        /// abstract method for initializing the game board, it takes a Grid as a parameter. The method
        /// will be implemented in the Game class to set up the game board UI by populating the provided
        /// Grid with the initial layout of the game board, including rooms, hallways, and any other
        /// relevant elements.
        /// </summary>
        /// <param name="grid"></param>
        public abstract void InitBoard(Grid grid);
        /// <summary>
        /// abstract method for ending the current player's turn after making a suggestion, it will be called
        /// when the player has finished making their suggestion and the game needs to process the suggestion
        /// and update the game state accordingly. It will be implemented in the Game class to handle the
        /// necessary logic for ending the current player's turn after making a suggestion, including 
        /// checking the validity of the suggestion, updating the game state based on the suggestion, 
        /// and determining if any players need to respond to the suggestion. 
        /// </summary>
        public abstract void EndTurnAfterSuggestion();
        /// <summary>
        /// abstract method for rolling the dice for the current player, it will be called at the beginning
        /// of the player's turn. The method will be implemented in the Game class to simulate rolling the
        /// dice for the current player, generating a random number between 1 and 6 to determine how many
        /// spaces the player can move on their turn. 
        /// </summary>
        public abstract void RollDiceForCurrentPlayer();
        /// <summary>
        /// abstract method for ending the game, it will be called when a player wins the game. The method 
        /// will be implemented in the Game class to handle the necessary logic for ending the game, 
        /// including updating the game state to reflect that the game is over, determining the winner of
        /// the game if applicable, and performing any necessary cleanup of resources.
        /// </summary>
        public abstract void EndGame();
        /// <summary>
        /// abstract method for handling the result of an incoming call, it takes a boolean parameter
        /// indicating whether the call was successful or not, and it returns a boolean indicating whether
        /// the game should continue or not based on the result of the call. The method will be implemented
        /// in the Game class to handle the result of incoming calls.
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public abstract bool HandleIncomingCallResult(bool success);
        /// <summary>
        /// abstract method for generating the frames for the dice animation, it takes the total number of
        /// frames, the total time for the animation, and the interval between frames as parameters, and 
        /// it returns a list of integers representing the values of the dice for each frame of the 
        /// animation. The method will be implemented in the Game class to generate the frames for the
        /// dice animation based on the total number of frames, the total time for the animation, and the 
        /// interval between frames, allowing for a visually appealing and dynamic dice rolling animation
        /// in the game.
        /// </summary>
        /// <param name="totalFrames"></param>
        /// <param name="totalTime"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public abstract List<int> GenerateDiceFrames(int totalFrames, long totalTime, long interval);
        /// <summary>
        /// abstract method for checking the player's accusation, it takes an Accusation object as a 
        /// parameter and returns a tuple containing booleans indicating whether the room, weapon, and 
        /// suspect in the accusation are correct, as well as a boolean indicating whether the player 
        /// wins the game based on the accusation. The method will be implemented in the Game class to 
        /// check the player's accusation against the correct answer for the game and determine the outcome
        /// of the accusation, including whether the player wins the game if all parts of the accusation 
        /// are correct. The method will return a tuple with the results of the accusation, allowing for 
        /// appropriate updates to the game state and UI based on the outcome of the accusation.
        /// </summary>
        /// <param name="accusation"></param>
        /// <returns></returns>
        public abstract (bool roomCorrect, bool weaponCorrect, bool suspectCorrect, bool isWin) CheckAccusation
    (Accusation accusation);
        /// <summary>
        /// abstract method for handling a player's move, it takes the row and column indices of the move
        /// as parameters. The method will be implemented in the Game class to handle a player's move on
        /// the game board, updating the player's position based on the provided row and column indices,
        /// and performing any necessary checks or updates to the game state as a result of the move. 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        protected abstract void Play(int rowIndex, int columnIndex);
        /// <summary>
        /// abstract method for updating the game status, it will be called whenever there is a change in
        /// the game turns state that requires an update to the game status. The method will be implemented
        /// in the Game class to update the game status based on the current state of the game.
        /// </summary>
        protected abstract void UpdateStatus();
        /// <summary>
        /// abstract method for updating the firebase data after a player makes a move, it will be called
        /// at the end of the Play method to update the game. The method will be implemented in the Game 
        /// class to update the game document in the database with the new state of the game after a 
        /// player makes a move, ensuring that all players have the most up-to-date information about 
        /// the game state. 
        /// </summary>
        protected abstract void UpdateFbMove();
        /// <summary>
        /// abstract method for handling changes in the game document snapshot, it takes the snapshot and
        /// an optional error as parameters. The method will be implemented in the Game class to handle 
        /// changes in the game document snapshot from the database, allowing the game to respond to 
        /// updates in the game state and update the UI accordingly. The snapshot parameter will provide
        /// the updated game document data, while the error parameter will provide information about any
        /// errors that may have occurred during the snapshot retrieval. The method will process the 
        /// snapshot data to update the game state and trigger any necessary events or UI updates based 
        /// on the changes detected in the snapshot. If an error is present, the method will handle the 
        /// error appropriately, such as by logging the error or notifying the user of the issue.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="error"></param>
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
    }
}