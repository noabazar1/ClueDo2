using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that represents the view model for the game page, which is responsible for managing the state 
    /// of the game and providing the necessary data and commands to the game page view. It interacts with 
    /// the Game model to retrieve the current state of the game, such as the players, their positions, and
    /// the status of the game, and it provides commands for rolling the dice, starting the game, and
    /// handling incoming calls. It also listens to changes in the game state and updates the UI 
    /// accordingly, and it handles the end of the game by showing a victory or defeat popup to the user.
    /// Additionally, it listens to changes in the internet connectivity and shows an alert if the device 
    /// is not connected to the internet when trying to perform actions that require an internet connection. 
    /// </summary>
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        private readonly GameBoard board;
        private readonly OpponentsGrid grdOponnents;
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool popupOpen = false;
        private EventHandler? gameChangedHandler;
        private bool isAlertShown = false;
        public ICommand ShowNoInternetCommand { get; }
        public bool IsConnected => _connectivity.IsConnected;
        public string MyName => game.MyName;
        public bool IsMyTurn => game.IsMyTurn();
        public string StatusMessage => game.StatusMessage;
        public bool IsStarted => game.IsStarted;
        public bool IsHostUser => game.IsHostUser;
        public bool IsStartButtonVisible => IsHostUser && !game.IsStarted;

        private readonly TimerSettings animationTimer = new(600, 30);

        public string diceImage = Keys.DiceImage;
        public string DiceImage
        {
            get => diceImage;
            set
            {
                if (diceImage != value)
                {
                    diceImage = value;
                    OnPropertyChanged();
                }
            }
        }
        public string DiceResult
        {
            get
            {
                string result = string.Empty;
                if (game.Players.PlayersList.Count > 0 && game.Players.MyIndex >= 0 &&
                    game.Players.MyIndex < game.Players.PlayersList.Count)
                {
                    Player me = game.Players.PlayersList[game.Players.MyIndex];
                    if (me.DiceValue > 0)
                        result = me.DiceValue.ToString();
                }
                return result;
            }
        }
        /// <summary>
        /// constructor for the GamePageVM class, which initializes the game model, the game board, and the
        /// opponents grid. It also subscribes to the GameEnded event of the game model to handle the end 
        /// of the game and to the ConnectivityChanged event of the connectivity model to listen to changes 
        /// in the internet connectivity. It also initializes the ShowNoInternetCommand to show an alert 
        /// when the device is not connected to the internet.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="grdOpponentsGrid"></param>
        /// <param name="grdBoard"></param>
        public GamePageVM(Game game, Grid grdBoard, Grid grdOpponentsGrid)
        {
            this.game = game;
            board = new GameBoard();
            board.Build(grdBoard, game.OnButtonClicked);
            game.InitBoard(grdBoard);
            game.DoorClicked += async roomName =>
            {
                await HandleDoorAsync(roomName);
            };
            grdOponnents = new OpponentsGrid(grdOpponentsGrid, game);
            game.GameEnded += OnGameEnded;
            ShowNoInternetCommand =
                new Command(async () => await ShowNoInternet());
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }
        /// <summary>
        /// method that is called when the connectivity changes, which updates the IsConnected property and
        /// shows an alert if the device is not connected to the internet and the alert is not already 
        /// shown. This method is subscribed to the ConnectivityChanged event of the connectivity model in
        /// the constructor, and it is called whenever the connectivity changes, such as when the device 
        /// connects or disconnects from the internet. The alert is shown using the Shell.Current.DisplayAlert
        /// method, which displays a popup alert to the user with a title and a message, and an OK button to
        /// dismiss the alert. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        /// <summary>
        /// method that shows an alert to the user indicating that they need to check their internet 
        /// connection. This method is called when the user tries to perform an action that requires an
        /// internet connection while the device is not connected to the internet, and it is also called 
        /// when the connectivity changes and the device becomes disconnected from the internet. 
        /// </summary>
        /// <returns></returns>
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
        /// <summary>
        /// method that initializes the game page view model, which subscribes to the GameChanged and 
        /// GameDeleted events of the game model, adds a snapshot listener to the game model, and updates 
        /// the guest user information if the current user is not the host. It also registers a message 
        /// handler for incoming calls using the WeakReferenceMessenger from the CommunityToolkit.Mvvm 
        /// library. This method is called when the game page is navigated to, and it sets up the necessary
        /// event handlers and listeners to manage the state of the game and handle user interactions during
        /// the game.
        /// </summary>
        public void Initialize()
        {
            gameChangedHandler = OnGameChanged;
            game.OnGameChanged += gameChangedHandler;
            game.OnGameDeleted += OnGameDeleted;
            game.AddSnapshotListener();
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
            WeakReferenceMessenger.Default.UnregisterAll(this);
            WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
            {
                OnIncomingCall();
            });
        }
        /// <summary>
        /// method that cleans up the game page view model, which unsubscribes from the GameChanged and 
        /// GameDeleted events of the game model, removes the snapshot listener from the game model, and 
        /// unregisters all message handlers for the view model using the WeakReferenceMessenger from the 
        /// CommunityToolkit.Mvvm library.
        /// </summary>
        public void Cleanup()
        {
            if (gameChangedHandler != null)
                game.OnGameChanged -= gameChangedHandler;
            game.OnGameDeleted -= OnGameDeleted;
            WeakReferenceMessenger.Default.UnregisterAll(this);
            game.RemoveSnapshotListener();
        }
        /// <summary>
        /// method that rolls the dice for the current player, which is called when the user presses the
        /// dice button during their turn. The method first plays the dice animation by generating a list
        /// of frames for the dice roll and updating the DiceImage property with each frame, and then it 
        /// calls the RollDiceForCurrentPlayer method of the game model to update the game state with the 
        /// result of the dice roll. This method is decorated with the RelayCommand attribute from the 
        /// CommunityToolkit.Mvvm library, which allows it to be bound to a button in the game page view, 
        /// and it can only be executed when it is the current player's turn and the game has started.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task RollDice()
        {
            await PlayDiceAnimation();
            game.RollDiceForCurrentPlayer();
        }
        /// <summary>
        /// method that starts the game, which is called when the host user presses the start button. The 
        /// method checks if the current user is the host, and if so, it sets the IsStarted property of the
        /// game model to true and calls the SetDocument method of the game model to update the game state
        /// in the database. It also raises the PropertyChanged event for the IsStartButtonVisible property
        /// to update the visibility of the start button in the game page view. This method is decorated 
        /// with the RelayCommand attribute from the CommunityToolkit.Mvvm library, which allows it to be
        /// bound to a button in the game page view, and it can only be executed by the host user when the
        /// game has not started yet.
        /// </summary>
        [RelayCommand]
        private void StartGame()
        {
            if (IsHostUser)
            {
                game.IsStarted = true;
                game.SetDocument(_ => { });
                OnPropertyChanged(nameof(IsStartButtonVisible));
            }
        }
        /// <summary>
        /// method that handles an incoming call during the game, which is called when a message of type
        /// AppMessage is received using the WeakReferenceMessenger from the CommunityToolkit.Mvvm library.
        /// The method checks if the game has started and if there is no popup currently open, and if so, 
        /// it sets the popupOpen flag to true and sends a message to show a challenge popup with a timer.
        /// It then shows the ChallengePopup using the Shell.Current.CurrentPage.ShowPopupAsync method, and
        /// waits for the result of the popup. If the result indicates that the user successfully completed
        /// the challenge, it calls the HandleIncomingCallResult method of the game model to update the game
        /// state accordingly. If the user failed the challenge and is still in the game, it shows a toast 
        /// message indicating that they lost their turn. If the user failed the challenge and is no longer
        /// in the game, it navigates back to the main area of the app. 
        /// </summary>
        private void OnIncomingCall()
        {
            if (IsStarted && !popupOpen)
            {
                popupOpen = true;
                WeakReferenceMessenger.Default.Send(
                    new AppMessage<TimerSettings>(new TimerSettings(10000, 1000)));
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    ChallengePopup popup = new();
                    object? result = await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                    bool success = result is bool b && b;
                    bool stillInGame = game.HandleIncomingCallResult(success);
                    popupOpen = false;
                    if (!stillInGame)
                    {
                        await Shell.Current.GoToAsync(Keys.MainArea);
                    }
                });
            }
        }
        /// <summary>
        /// method that is called when there is a change in the game state, which updates the opponents grid,
        /// the game board, and the relevant properties in the view model. This method is subscribed to the
        /// GameChanged event of the game model in the Initialize method, and it is called whenever there is
        /// a change in the game state, such as when a player rolls the dice, moves, or makes an accusation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameChanged(object? sender, EventArgs e)
        {
            grdOponnents.DisplayOponnentsNames();
            UpdateGameGrid();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(DiceResult));
            OnPropertyChanged(nameof(IsStarted));
            RollDiceCommand.NotifyCanExecuteChanged();
        }
        /// <summary>
        /// method that updates the game grid with the current positions of the players, which is called 
        /// when there is a change in the game state. The method calls the DrawPlayers method of the 
        /// GameBoard class to update the visual representation of the game board with the current 
        /// positions of the players. This method is called from the OnGameChanged method whenever there is
        /// a change in the game state, such as when a player moves or makes an accusation, to ensure that
        /// the game board reflects the current state of the game.
        /// </summary>
        private void UpdateGameGrid()
        {
            board.DrawPlayers(game);
        }
        /// <summary>
        /// method that is called when the game is deleted, which navigates back to the previous page and 
        /// shows a toast message indicating that the game has been deleted. This method is subscribed to 
        /// the GameDeleted event of the game model in the Initialize method, and it is called when the 
        /// game is deleted, such as when the host user deletes the game or when the game ends and is 
        /// removed from the database. The method uses the Shell.Current.Navigation.PopAsync method to 
        /// navigate back to the previous page, and it uses the Toast.Make method from the 
        /// CommunityToolkit.Maui.Alerts library to show a toast message to the user indicating that the 
        /// game has been deleted. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
                Toast.Make(Strings.GameDeleted,
                    CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
            });
        }
        /// <summary>
        /// method that plays the dice animation when the user rolls the dice, which is called from the 
        /// RollDice method. The method generates a list of frames for the dice roll using the 
        /// GenerateDiceFrames method of the game model, and it updates the DiceImage property with each 
        /// frame to create the animation effect. The method uses the animationTimer to control the timing 
        /// of the animation, and it only plays the animation if it is the current player's turn and the 
        /// game has started. 
        /// </summary>
        /// <returns></returns>
        private async Task PlayDiceAnimation()
        {
            if (game.IsMyTurn() && IsStarted)
            {
                List<int> frames = game.GenerateDiceFrames(30, animationTimer.TotalTimeInMilliseconds,
                    animationTimer.IntervalInMilliseconds);
                foreach (int frame in frames)
                {
                    DiceImage = string.Format(Keys.DiceImageFormat, frame);
                    await Task.Delay((int)animationTimer.IntervalInMilliseconds);
                }
            }
        }
        /// <summary>
        /// method that handles the user entering a room through a door, which is called when the user 
        /// clicks on a door in the game board. The method shows a SuggestionPopup to the user, allowing 
        /// them to make a suggestion for the suspect, weapon, and room. If the user submits an accusation
        /// through the popup, the method checks the accusation against the hidden solution using the 
        /// CheckAccusation method of the game model. If the accusation is correct and the user wins the
        /// game, it calls the EndGame method of the game model. If the accusation is incorrect, it shows
        /// a CheckPopup with the results of the accusation, indicating which elements of the accusation 
        /// were correct and which were incorrect. After showing the results of the accusation.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public async Task HandleDoorAsync(string roomName)
        {
            SuggestionPopup suggestionPopup = new(roomName);
            object? result = await Shell.Current.CurrentPage.ShowPopupAsync(suggestionPopup);
            if (result is Accusation accusation)
            {
                (bool roomCorrect, bool weaponCorrect, bool suspectCorrect, bool isWin) =
                    game.CheckAccusation(accusation);
                if (isWin)
                    game.EndGame();
                else
                {
                    CheckPopup checkPopup = new(roomCorrect, weaponCorrect, suspectCorrect);
                    await Shell.Current.CurrentPage.ShowPopupAsync(checkPopup);
                    game.EndTurnAfterSuggestion();
                }
            }
        }
        /// <summary>
        /// method that is called when the game ends, which shows a victory popup if the user wins the game,
        /// or a defeat popup if the user loses the game. This method is subscribed to the GameEnded event
        /// of the game model in the constructor, and it is called when the game ends, such as when a player
        /// correctly identifies the hidden solution or when the game is otherwise concluded. The method 
        /// uses the Shell.Current.CurrentPage.ShowPopupAsync method to show the appropriate popup to the 
        /// user based on whether they won or lost the game. The VictoryPopup and LosePopup are custom 
        /// popups that display a message and possibly some animation to indicate the outcome of the game to
        /// the user.
        /// </summary>
        /// <param name="isWinner"></param>
        private async void OnGameEnded(bool isWinner)
        {
            if (isWinner)
                await Shell.Current.CurrentPage.ShowPopupAsync(new VictoryPopup());
            else
                await Shell.Current.CurrentPage.ShowPopupAsync(new LosePopup());
        }
        /// <summary>
        /// method that is called when the guest user information is updated, which shows a toast message 
        /// if there was an error updating the guest user information. This method is passed as a callback
        /// to the UpdateGuestUser method of the game model in the Initialize method, and it is called when
        /// the guest user information is updated in the database. If there was an error during the update,
        /// such as a network error or a database error, the method shows a toast message to the user 
        /// indicating that there was an error joining the game. The toast message is shown using the 
        /// Toast.Make method from the CommunityToolkit.Maui.Alerts library, which displays a brief message
        /// to the user at the bottom of the screen.
        /// </summary>
        /// <param name="task"></param>
        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError,
                    CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
        }
    }
}