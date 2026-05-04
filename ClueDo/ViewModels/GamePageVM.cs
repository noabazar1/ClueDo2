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
        public GamePageVM(Game game, Grid grdOpponentsGrid, GameBoard board)
        {
            this.game = game;
            this.grdBoard = board;
            this.grdOponnents = new OpponentsGrid(grdOpponentsGrid, game);
            game.GameEnded += OnGameEnded;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
        }
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
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
            WeakReferenceMessenger.Default.Register<AppMessage<string>>(this, (r, m) =>
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