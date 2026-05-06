using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Services;
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
        private readonly GameBoard grdBoard;
        private readonly OpponentsGrid grdOponnents;
        private readonly List<Label> lstOponnentsLabels = [];
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private readonly IFriendsService friendsService = new FriendsService();
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
        /// <param name="board"></param>
        public GamePageVM(Game game, Grid grdOpponentsGrid, GameBoard board)
        {
            this.game = game;
            this.grdBoard = board;
            this.grdOponnents = new OpponentsGrid(grdOpponentsGrid, game);
            game.GameEnded += OnGameEnded;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
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
        public void Cleanup()
        {
            if (gameChangedHandler != null)
                game.OnGameChanged -= gameChangedHandler;
            game.OnGameDeleted -= OnGameDeleted;
            WeakReferenceMessenger.Default.UnregisterAll(this);
            game.RemoveSnapshotListener();
        }
        private static async void GoHome()
        {
            await Shell.Current.GoToAsync(Keys.MainArea);
        }
        [RelayCommand]
        private async Task RollDice()
        {
            await PlayDiceAnimation();
            game.RollDiceForCurrentPlayer();
        }
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
                    if (!stillInGame)
                        GamePageVM.GoHome();
                    popupOpen = false;
                });
            }
        }
        private void OnGameChanged(object? sender, EventArgs e)
        {
            grdOponnents.DisplayOponnentsNames();
            UpdateGameGrid();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(DiceResult));
            RollDiceCommand.NotifyCanExecuteChanged();
        }
        private void UpdateGameGrid()
        {
            grdBoard.DrawPlayers(game);
        }
        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
                Toast.Make(Strings.GameDeleted,
                    CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
            });
        }
        private async Task PlayDiceAnimation()
        {
            if (game.IsMyTurn() && IsStarted)
            {
                List<int> frames = game.GenerateDiceFrames( 30, animationTimer.TotalTimeInMilliseconds,
                    animationTimer.IntervalInMilliseconds);
                foreach (int frame in frames)
                {
                    DiceImage = string.Format(Keys.DiceImageFormat, frame);
                    await Task.Delay((int)animationTimer.IntervalInMilliseconds);
                }
            }
        }
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
        private async void OnGameEnded(bool isWinner)
        {
            if (isWinner)
                await Shell.Current.CurrentPage.ShowPopupAsync(new VictoryPopup());
            else
                await Shell.Current.CurrentPage.ShowPopupAsync(new LosePopup());
        }
        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError,
                    CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
        }
    }
}