using ClueDo.Models;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that holds the data of the game, and the methods to manage the game, such as adding players,
    /// joining the game, placing players on the board, handling button clicks, rolling the dice, ending 
    /// the turn after a suggestion, ending the game, checking the accusation, and handling incoming call 
    /// results. This class is used in the GamePage to manage the game logic, and to update the game's 
    /// information and status during the game. The Game class inherits from the GameModel class, which 
    /// defines the properties and methods that are common to all games, and it implements the specific 
    /// logic for the Clue game. The Game class also uses a GameBoard object to manage the grid of the game,
    /// and a Dice object to manage the dice rolls in the game. The Game class also has events for when a 
    /// door is clicked, when the game ends, when the game changes, and when the game is deleted, which are 
    /// used to notify the UI of changes in the game's state.   
    /// </summary>
    public class Game : GameModel
    {
        private GameBoard? boardLogic;
        private readonly Dice dice = new();
        private bool _gameOverPopupShown = false;
        public event Action<string>? DoorClicked;
        public override string JoinStatus => $"{CurrentPlayers}/{Players.TotalPlayers}";
        protected override GameStatus Status => _status;
        public string? CurrentRoom { get; private set; }
        public event Action<bool>? GameEnded;
        /// <summary>
        /// constructor for the Game class, which initializes the game with a grid for the board, and adds
        /// the host player to the game. The constructor takes a Grid object as a parameter, which is used
        /// to build the board of the game. The constructor also initializes the Created property to the
        /// current date and time, and it creates a new Player object for the host player, using the name
        /// of the current user and an index of 0. The host player is added to the Players list, and the 
        /// InitBoard method is called to build the board of the game using the provided grid. This
        /// constructor is used when creating a new game, and it sets up the initial state of the game with
        /// the host player and the game board.
        /// </summary>
        /// <param name="grdBoard"></param>
        public Game(Grid grdBoard)
        {
            Created = DateTime.Now;
            Player p = new(new User().Name, 0);
            Players.Add(p);
            InitBoard(grdBoard);
        }
        /// <summary>
        /// constructor for the Game class, which initializes the game with default values.
        /// </summary>
        public Game()
        {
            Players.TotalPlayers = 0;
        }
        /// <summary>
        /// method to ensure that the answer for the game is generated. This method checks if the answer is
        /// null, if the current user is the host, and if the game ID is not empty. If these conditions are
        /// met, it generates a new answer using the Answer.Generate() method, and it updates the Answer 
        /// field in the Firestore database using the UpdateField method of the FirestoreDatabase class. 
        /// The OnComplete callback is passed to handle the completion of the update operation. This method
        /// is called to ensure that the answer for the game is generated and stored in the database when 
        /// the host starts the game.
        /// </summary>
        /// <param name="myUserId"></param>
        public override void EnsureAnswerGenerated(string myUserId)
        {
            if (Answer == null && myUserId == HostId && !string.IsNullOrEmpty(Id))
            {
                Answer = Answer.Generate();
                fbd.UpdateField(Keys.GamesCollection, Id, nameof(Answer), Answer, OnComplete);
            }
        }
        /// <summary>
        /// method to add a player to the game. This method takes the name of the player as a parameter, 
        /// and it creates a new Player object with the provided name and an index based on the current
        /// number of players in the game. The new player is added to the Players list, and the player's 
        /// name is added to the PlayersNames list. The method returns a boolean value indicating whether
        /// the player was successfully added to the game or not. The player is added to the game only if
        /// the boardLogic is not null, which means that the game board has been initialized. This method 
        /// is called when a new player joins the game.
        /// </summary>
        /// <param name="playerName"></param>
        /// <returns></returns>
        public override bool AddPlayer(string playerName)
        {
            bool result = false;
            int index = Players.Count;
            if (boardLogic != null)
            {
                Player tempPlayer = new(string.Empty, index, null!);
                Position startPos = tempPlayer.Position;
                IndexButton btn = boardLogic.GetButton(startPos);
                Player player = new(playerName, index, btn);
                Players.Add(player);
                PlayersNames.Add(playerName);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// method to join the game. This method checks if there are available slots for players in the game,
        /// and if so, it creates a new Player object for the current user, adds it to the Players list, and
        /// updates the CurrentPlayers count. If the number of current players reaches the total number of
        /// players allowed in the game, it sets the IsFull property to true. The method then updates the 
        /// relevant fields in the Firestore database using the UpdateFields method of the FirestoreDatabase
        /// class, and it passes the OnComplete callback to handle the completion of the update operation. 
        /// This method is called when a user attempts to join the game, and it manages the process of 
        /// adding the player to the game and updating the game state in the database accordingly.
        /// </summary>
        public override void JoinGame()
        {
            if (Players.PlayersList.Count > 0)
            {
                int newIndex = Players.PlayersList.Count;
                Players.MyIndex = newIndex;
                Player p = new(MyName, newIndex);
                Players.Add(p);
                CurrentPlayers = Players.PlayersList.Count;
                if (CurrentPlayers >= Players.TotalPlayers)
                    IsFull = true;
                Dictionary<string, object> dict = new()
                {
                    { nameof(CurrentPlayers), CurrentPlayers },
                    { nameof(Players), Players },
                    { nameof(IsFull), IsFull }
                };
                fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
            }
        }
        /// <summary>
        /// method to get the position of a player based on their index in the players list. This method 
        /// takes an integer index as a parameter, and it returns the Position object of the player at that
        /// index in the players list. 
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public override Position GetPlayerPosition(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Position;
        }
        /// <summary>
        /// method to get the color of a player based on their index in the players list. This method takes
        /// an integer index as a parameter, and it returns the Color object of the player at that index in 
        /// the players list. The color of the player is determined by the player's index, and it is used 
        /// to visually represent the player on the game board. This method is called when drawing the 
        /// players on the board to get the appropriate color for each player based on their index in the
        /// players list.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public override Color GetPlayerColor(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Color;
        }
        /// <summary>
        /// method to get the name of a player based on their index in the players list. This method takes
        /// an integer index as a parameter, and it returns the name of the player at that index in the 
        /// players list. The name of the player is used to identify the player in the game.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public override string GetPlayerName(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Name;
        }
        /// <summary>
        /// method to draw all the players on the game board. This method iterates through the list of
        /// players in the Players list, and it calls the DrawPlayer method for each player to update their 
        /// position and color on the board. The DrawPlayer method uses the player's position to get the
        /// corresponding button on the board and sets the background color of the button to the player's 
        /// color. 
        /// </summary>
        public override void DrawAllPlayers()
        {
            foreach (Player player in Players.PlayersList)
                DrawPlayer(player);
        }
        /// <summary>
        /// method to set the document in the Firestore database for the game. This method calls the 
        /// SetDocument method of the FirestoreDatabase class, passing the current game object, the 
        /// collection name, the game ID, and the OnComplete callback to handle the completion of the 
        /// operation. The SetDocument method will create a new document in the specified collection with 
        /// the data from the current game object, and it will return the ID of the created document, which
        /// is then assigned to the Id property of the game. This method is called when creating a new game
        /// to save it in the database and get its unique ID for future reference.
        /// </summary>
        /// <param name="OnComplete"></param>
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        /// <summary>
        /// method to update the guest user in the game. This method checks if the number of players in the
        /// game has reached the maximum allowed, and if so, it sets the IsFull property to true. It then
        /// calls the UpdateFbJoinGame method to update the relevant fields in the Firestore database, 
        /// passing the OnComplete callback to handle the completion of the update operation. This method
        /// is called when a guest user joins the game, and it manages the process of updating the game 
        /// state in the database to reflect the new player and the current status of the game.
        /// </summary>
        /// <param name="OnComplete"></param>
        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            IsFull = Players.Count >= 5;
            UpdateFbJoinGame(OnComplete);
        }
        /// <summary>
        /// method to add a snapshot listener to the game document in the Firestore database. This method
        /// checks if the listener is not already added and if the game ID is not empty, and if so, it 
        /// calls the AddSnapshotListener method of the FirestoreDatabase class, passing the collection 
        /// name, the game ID, and the OnChange callback to handle changes in the document.
        /// </summary>
        public override void AddSnapshotListener()
        {
            if (ilr == null && !string.IsNullOrEmpty(Id))
                ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }
        /// <summary>
        /// method to remove the snapshot listener from the game document in the Firestore database. This
        /// method checks if the listener is not null, and if so, it calls the Remove method of the 
        /// listener to stop listening to changes in the document. It then calls the DeleteDocument method 
        /// to remove the game document from the database, passing the OnComplete callback to handle the 
        /// completion of the delete operation.
        /// </summary>
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }
        /// <summary>
        /// method to delete the game document from the Firestore database. This method calls the
        /// DeleteDocument method of the FirestoreDatabase class, passing the collection name, the game ID,
        /// and the OnComplete callback to handle the completion of the delete operation. This method is 
        /// called when the host player leaves the game, and it is used to remove the game from the 
        /// database since the host is responsible for managing the game.
        /// </summary>
        /// <param name="OnComplete"></param>
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        /// <summary>
        /// method to place a player on the board at the beginning of the game. This method takes the player's
        /// index, and the row and column indices as parameters, and it updates the position of the player in
        /// the Players list based on the provided row and column indices.
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        public override void PlacePlayer(int playerIndex, int row, int col)
        {
            Players.PlayersList[playerIndex].Position = new Position(row, col);
        }
        /// <summary>
        /// method to initialize the game board. This method takes a Grid object as a parameter, and it 
        /// checks if the boardLogic is null. If it is null, it creates a new GameBoard object and calls 
        /// the Build method of the GameBoard class, passing the grid and the OnButtonClicked callback to
        /// handle button clicks on the board. This method is called when creating a new game to set up the
        /// game board using the provided grid, and to enable interaction with the board through button 
        /// clicks.
        /// </summary>
        /// <param name="grid"></param>
        public override void InitBoard(Grid grid)
        {
            if (boardLogic == null)
            {
                boardLogic = new GameBoard();
                boardLogic.Build(grid, OnButtonClicked);
            }
        }
        /// <summary>
        /// method to handle button clicks on the game board. This method checks if the game has started, 
        /// and if the sender of the click event is an IndexButton. If the button represents a door, it 
        /// checks if the current player has moves left and if they can move to the button's position. If 
        /// so, it updates the CurrentRoom property to the room name of the button, sets the player's 
        /// IsInRoom property to true, and raises the DoorClicked event with the room name. If the button 
        /// does not represent a door, it calls the Play method with the row and column indices of the 
        /// button to handle a regular move on the board.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public override void OnButtonClicked(object? sender, EventArgs e)
        {
            if (IsStarted)
                if (sender is IndexButton btn)
                    if (btn.IsDoor)
                    {
                        Player me = Players.PlayersList[Players.MyIndex];
                        if (me.MovesLeft > 0 && CanMoveTo(me, btn.Row, btn.Column))
                        {
                            CurrentRoom = btn.RoomName;
                            me.IsInRoom = true;
                            DoorClicked?.Invoke(btn.RoomName!);
                        }
                    }
                    else
                        Play(btn.Row, btn.Column);
        }
        /// <summary>
        /// method to end the turn after a suggestion is made. This method checks if it is the current
        /// player's turn, and if so, it sets the moves left for the player to 0, increments the 
        /// CurrentTurnIndex to move to the next player's turn, and updates the game status and the 
        /// Firestore database with the new turn information. It also raises the OnGameChanged event to 
        /// notify the UI of the change in the game state. This method is called after a player makes a
        /// suggestion and finishes their turn, allowing the next player to take their turn. 
        /// </summary>
        public override void EndTurnAfterSuggestion()
        {
            if (IsMyTurn())
            {
                Player me = Players.PlayersList[Players.MyIndex];
                me.MovesLeft = 0;
                CurrentTurnIndex++;
                if (CurrentTurnIndex >= Players.PlayersList.Count)
                    CurrentTurnIndex = 0;
                UpdateStatus();
                UpdateFbMove();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        /// <summary>
        /// method to roll the dice for the current player. This method checks if the game has started and
        /// if it is the current player's turn. If so, it checks if the player has no moves left, and if 
        /// that's the case, it rolls the dice using the Dice object, calculates the total value of the 
        /// dice, and updates the player's DiceValue and MovesLeft properties with the total value. 
        /// </summary>
        public override void RollDiceForCurrentPlayer()
        {
            if (IsStarted && IsMyTurn())
            {
                Player me = Players.PlayersList[Players.MyIndex];
                if (me.MovesLeft == 0)
                {
                    dice.RollDice();
                    int total = dice.Die1 + dice.Die2;
                    me.DiceValue = total;
                    me.MovesLeft = total;
                    UpdateStatus();
                    UpdateFbMove();
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// method to end the game. This method sets the IsGameOver property to true, assigns the name of 
        /// the winner based on the current turn index, and updates the relevant fields in the Firestore 
        /// database using the UpdateFields method of the FirestoreDatabase class. The OnComplete callback
        /// is passed to handle the completion of the update operation. This method is called when a player
        /// wins the game, and it manages the process of updating the game state in the database to reflect
        /// that the game is over and to identify the winner. It also raises the GameEnded event to notify
        /// the UI of the end of the game and to indicate whether the current player is the winner or not.
        /// </summary>
        public override void EndGame()
        {
            IsGameOver = true;
            WinnerName = Players.PlayersList[CurrentTurnIndex].Name;
            Dictionary<string, object> dict = new()
            {
                { nameof(IsGameOver), IsGameOver },
                { nameof(WinnerName), WinnerName }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        /// <summary>
        /// method to check if the room in a suggestion or accusation is correct based on the answer for
        /// the game. This method takes a string representing the room as a parameter, and it compares it 
        /// to the Room property of the Answer object. It returns a boolean value indicating whether the 
        /// provided room is correct or not. 
        /// </summary>
        /// <param name="room"></param>
        /// <returns></returns>
        public override bool CheckRoom(string room)
        {
            return room == Answer!.Room;
        }
        /// <summary>
        /// method to check if the weapon in a suggestion or accusation is correct based on the answer for
        /// the game. This method takes a string representing the weapon as a parameter, and it compares it
        /// to the Weapon property of the Answer object. It returns a boolean value indicating whether the
        /// provided weapon is correct or not.
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        public override bool CheckWeapon(string weapon)
        {
            return weapon == Answer!.Weapon;
        }
        /// <summary>
        /// method to check if the suspect in a suggestion or accusation is correct based on the answer for
        /// the game. This method takes a string representing the suspect as a parameter, and it compares it
        /// to the Suspect property of the Answer object. It returns a boolean value indicating whether the
        /// provided suspect is correct or not.
        /// </summary>
        /// <param name="suspect"></param>
        /// <returns></returns>
        public override bool CheckSuspect(string suspect)
        {
            return suspect == Answer!.Suspect;
        }
        /// <summary>
        /// method to check if it is the current player's turn. This method compares the MyIndex property
        /// of the Players object to the CurrentTurnIndex property of the game. If they are equal, it means
        /// that it is the current player's turn, and the method returns true. Otherwise, it returns false.
        /// </summary>
        /// <returns></returns>
        public override bool IsMyTurn()
        {
            return Players.MyIndex == CurrentTurnIndex;
        }
        /// <summary>
        /// method to check if it is the opponent's turn based on their index in the players list. This 
        /// method takes an integer index as a parameter, and it compares it to the CurrentTurnIndex 
        /// property of the game. If the provided index is equal to the CurrentTurnIndex, it means that it
        /// is that opponent's turn, and the method returns true. Otherwise, it returns false. This method 
        /// is used to determine if the UI should indicate that it is the opponent's turn, and to manage
        /// the game logic accordingly based on whose turn it is in the game.
        /// </summary>
        /// <param name="opponentIndex"></param>
        /// <returns></returns>
        public override bool IsOpponentTurn(int opponentIndex)
        {
            return Players.IsOpponentTurn(opponentIndex);
        }
        /// <summary>
        /// method to set eliminate the current player from the game. This method removes the current player
        /// from the Players list, sets their IsEliminated property to true, and checks if the current turn
        /// index needs to be updated based on the new number of players in the game. If there is only one
        /// player left in the game after elimination, it sets the WinnerName to that player's name and
        /// marks the game as over by setting IsGameOver to true. It then updates the relevant fields in the
        /// Firestore database using the UpdateFields method of the FirestoreDatabase class, passing the
        /// OnComplete callback to handle the completion of the update operation. This method is called when
        /// a player is eliminated from the game, and it manages the process of updating the game state in
        /// the database to reflect the elimination and to check for a winner if necessary.
        /// </summary>
        public override void EliminateCurrentPlayer()
        {
            Player me = Players.PlayersList[Players.MyIndex];
            Players.PlayersList.Remove(me);
            me.IsEliminated = true;
            if (Players.MyIndex >= Players.PlayersList.Count)
                Players.MyIndex = 0;
            if (Players.PlayersList.Count == 1)
            {
                WinnerName = Players.PlayersList[0].Name;
                IsGameOver = true;
            }
            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players },
                { nameof(IsGameOver), IsGameOver },

            };
            if (IsGameOver && WinnerName != null)
                dict.Add(nameof(WinnerName), WinnerName);
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        /// <summary>
        /// method to handle the result of an incoming call related to a suggestion or accusation. This
        /// method takes a boolean value indicating the success or failure of the task that came with the call
        /// as a parameter, and it returns a boolean value indicating the result of the operation.
        /// </summary>
        /// <param name="success"></param>
        /// <returns></returns>
        public override bool HandleIncomingCallResult(bool success)
        {
            bool result = true;
            if (!success)
            {
                EliminateCurrentPlayer();
                result = false;
            }
            return result;
        }
        /// <summary>
        /// method to generate the frames for the dice roll animation. This method takes the total number of
        /// frames for the animation, the total time for the animation in milliseconds, and the interval 
        /// between frames in milliseconds as parameters. It calculates the number of iterations based on
        /// the total time and the interval, and it generates a list of frame indices to be used for the 
        /// dice roll animation. The frame indices are calculated to create a smooth animation effect over 
        /// the specified total time. This method is called when rolling the dice to determine which frames
        /// to display during the dice roll animation.
        /// </summary>
        /// <param name="totalFrames"></param>
        /// <param name="totalTime"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public override List<int> GenerateDiceFrames(int totalFrames, long totalTime, long interval)
        {
            List<int> frames = [];
            int iterations = (int)(totalTime / interval);
            double step = (double)totalFrames / iterations;
            double frameIndex = 0;
            for (int i = 0; i < iterations; i++)
            {
                int currentFrame = Math.Min((int)frameIndex + 1, totalFrames);
                frames.Add(currentFrame);
                frameIndex += step;
            }
            return frames;
        }
        /// <summary>
        /// method to check an accusation made by a player. This method takes an Accusation object as a parameter,
        /// and it returns a tuple containing the results of the check for the room, weapon, and suspect.
        /// </summary>
        /// <param name="accusation"></param>
        /// <returns></returns>
        public override (bool roomCorrect, bool weaponCorrect, bool suspectCorrect, bool isWin) CheckAccusation
            (Accusation accusation)
        {
            bool roomCorrect = CheckRoom(accusation.Room);
            bool weaponCorrect = CheckWeapon(accusation.Weapon);
            bool suspectCorrect = CheckSuspect(accusation.Suspect);
            bool isWin = roomCorrect && weaponCorrect && suspectCorrect;
            return (roomCorrect, weaponCorrect, suspectCorrect, isWin);
        }
        /// <summary>
        /// method to update the game status based on whether it is the current player's turn or not. This
        /// method sets the CurrentStatus property of the _status object to Play if it is the current 
        /// player's turn, or to Wait if it is not the current player's turn. This method is called 
        /// whenever there is a change in the game state that may affect whose turn it is, such as after a
        /// player makes a move, ends their turn, or when the game state is updated from the database.
        /// </summary>
        protected override void UpdateStatus()
        {
            _status.CurrentStatus = IsMyTurn()
                ? GameStatus.Status.Play
                : GameStatus.Status.Wait;
        }
        /// <summary>
        /// method to handle changes in the game document from the Firestore database. This method is
        /// called whenever there is a change in the game document, and it takes an optional 
        /// IDocumentSnapshot and an optional Exception as parameters. If the snapshot is not null and 
        /// there is no error, it converts the snapshot to a Game object and updates the properties of the 
        /// current game instance with the values from the snapshot.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="error"></param>
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            if (snapshot != null && error == null)
            {
                Game? game = snapshot.ToObject<Game>();
                if (game != null)
                {
                    int myIndex = Players.MyIndex;
                    Players = game.Players;
                    CurrentPlayers = Players.PlayersList.Count;
                    if (Players.MyIndex >= Players.PlayersList.Count)
                        Players.MyIndex = Players.PlayersList.Count - 1;
                    Players.MyIndex = myIndex;
                    IsStarted = game.IsStarted;
                    IsHostTurn = game.IsHostTurn;
                    NextPlay = game.NextPlay;
                    CurrentPlayers = game.CurrentPlayers;
                    CurrentTurnIndex = game.CurrentTurnIndex;
                    IsGameOver = game.IsGameOver;
                    WinnerName = game.WinnerName;
                    if (IsGameOver && !_gameOverPopupShown)
                    {
                        _gameOverPopupShown = true;
                        bool isWinner = WinnerName == MyName;
                        GameEnded?.Invoke(isWinner);
                    }
                    UpdateStatus();
                    if (boardLogic != null)
                    {
                        boardLogic.ResetBoardColors();
                        DrawAllPlayers();
                    }
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        /// <summary>
        /// method to handle a play on the game board. This method takes the row and column indices of the
        /// target button as parameters, and it checks if the boardLogic is not null, if the Players list 
        /// is not null and has players, and if the current player's index is valid. If these conditions are
        /// met it gets the target button from the boardLogic using the provided row and column indices, 
        /// and if the button is not null, it checks if the current player has moves left and if they can 
        /// move to the target position. If so, it updates the player's position, moves left, and the board
        /// colors accordingly, and it updates the game state in the Firestore database with the new move 
        /// information. This method is called when a player clicks on a button on the game board to make a 
        /// move, and it manages the logic for validating and processing that move. 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        protected override void Play(int rowIndex, int columnIndex)
        {
            if (boardLogic != null && Players?.PlayersList != null &&
                Players.PlayersList.Count > 0 &&
                Players.MyIndex >= 0 &&
                Players.MyIndex < Players.PlayersList.Count)
            {
                IndexButton targetBtn = boardLogic.GetButton(new Position(rowIndex, columnIndex));
                if (targetBtn != null)
                {
                    Player currentPlayer = Players.PlayersList[Players.MyIndex];
                    if (currentPlayer.MovesLeft > 0 && Game.CanMoveTo(currentPlayer, rowIndex, columnIndex))
                    {
                        if (currentPlayer.Button != null)
                            currentPlayer.Button.BackgroundColor = Colors.Transparent;
                        currentPlayer.Button = targetBtn;
                        currentPlayer.Position = new Position(rowIndex, columnIndex);
                        currentPlayer.MovesLeft--;
                        boardLogic.ResetBoardColors();
                        DrawAllPlayers();
                        boardLogic.MyTurn();
                        if (currentPlayer.MovesLeft == 0)
                        {
                            CurrentTurnIndex++;
                            if (CurrentTurnIndex >= Players.PlayersList.Count)
                                CurrentTurnIndex = 0;
                        }
                        UpdateFbMove();
                        fbd.UpdateField(
                            Keys.GamesCollection,
                            Id,
                            nameof(Players),
                            Players,
                            OnComplete);
                    }
                }
            }
        }
        /// <summary>
        /// method to update the game state in the Firestore database after a move is made. This method 
        /// creates a dictionary containing the current turn index and the players list, and it calls the 
        /// UpdateFields method of the FirestoreDatabase class, passing the collection name, the game ID, 
        /// the dictionary of fields to update and the OnComplete callback to handle the completion of the 
        /// update operation. This method is called after a player makes a move on the board to ensure that
        /// the game state in the database is updated with the new turn information and the updated players
        /// list, allowing other players to see the changes in real-time through their snapshot listeners.
        /// </summary>
        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentTurnIndex), CurrentTurnIndex },
                { nameof(Players), Players }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        /// <summary>
        /// method to draw a player on the game board. This method takes a Player object as a parameter, and
        /// it checks if the boardLogic is not null. If it is not null, it gets the button corresponding to
        /// the player's position using the GetButton method of the boardLogic, and if the button is not 
        /// null, it sets the background color of the button to the player's color and assigns the button to
        /// the player's Button property.
        /// </summary>
        /// <param name="player"></param>
        private void DrawPlayer(Player player)
        {
            if (boardLogic != null)
            {
                IndexButton btn = boardLogic.GetButton(player.Position);
                if (btn != null)
                {
                    btn.BackgroundColor = player.Color;
                    player.Button = btn;
                }
            }
        }
        /// <summary>
        /// method to update the game state in the Firestore database after a guest user joins the game. This method
        /// creates a dictionary containing the current game state information, and it calls the UpdateFields method
        /// of the FirestoreDatabase class, passing the collection name, the game ID, the dictionary of fields to
        /// update and the OnComplete callback to handle the completion of the update operation. This method is called
        /// when a guest user joins the game to ensure that the game state in the database is updated with the new
        /// player information, allowing other players to see the changes in real-time through their snapshot listeners.
        /// </summary>
        /// <param name="OnComplete"></param>
        private void UpdateFbJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(IsFull), IsFull },
                { nameof(Players), Players }
            };
            action = Actions.Changed;
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        /// <summary>
        /// method to handle the completion of an asynchronous operation related to updating the game state
        /// in the Firestore database. This method takes a Task object as a parameter, and it checks if the
        /// task was completed successfully. If it was successful, it checks the value of the action 
        /// variable to determine whether to raise the OnGameDeleted event or the OnGameChanged event. This
        /// method is used as a callback for various database operations to ensure that the UI is updated 
        /// accordingly based on the changes made to the game state in the database. 
        /// </summary>
        /// <param name="task"></param>
        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
        }
        /// <summary>
        /// method to check if a player can move to a target position on the game board. This method takes
        /// a Player object and the target row and column indices as parameters, and it calculates the 
        /// absolute difference in rows and columns between the player's current position and the target 
        /// position. It returns true if the sum of the absolute differences in rows and columns is equal 
        /// to 1, which means that the target position is adjacent to the player's current position, 
        /// allowing for a valid move. Otherwise, it returns false. This method is used to validate player
        /// moves on the game board, ensuring that players can only move to adjacent positions according to
        /// the rules of the game.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="targetRow"></param>
        /// <param name="targetCol"></param>
        /// <returns></returns>
        private static bool CanMoveTo(Player player, int targetRow, int targetCol)
        {
            int dRow = Math.Abs(player.Position.Row - targetRow);
            int dCol = Math.Abs(player.Position.Column - targetCol);
            return dRow + dCol == 1;
        }
    }
}