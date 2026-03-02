using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Services;
using ClueDo.Views;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : Models.ObservableObject
    {
        private readonly Game game;
        private readonly GameBoard grdBoard;
        private readonly OpponentsGrid grdOponnents;
        private readonly List<Label> lstOponnentsLabels = [];

        private bool popupOpen = false;
        private EventHandler? gameChangedHandler;

        public string MyName => game.MyName;
        public bool IsMyTurn => game.IsMyTurn();
        public string StatusMessage => game.StatusMessage;
        public bool IsStarted => game.IsStarted;
        public bool IsHostUser => game.IsHostUser;
        public bool IsStartButtonVisible => IsHostUser && !game.IsStarted;

        private readonly TimerSettings animationTimer = new TimerSettings(600, 30);

        public string diceImage = "Dice/dice1c.png";
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
                Player me = game.Players.PlayersList[game.Players.MyIndex];
                return me.DiceValue > 0 ? me.DiceValue.ToString() : "";
            }
        }
        public GamePageVM(Game game, Grid grdOpponentsGrid, GameBoard board)
        {
            this.game = game;
            this.grdBoard = board;
            grdOponnents = new OpponentsGrid(grdOpponentsGrid, game);
        }
        public void Initialize()
        {
            gameChangedHandler = OnGameChanged;
            game.OnGameChanged += gameChangedHandler;

            game.OnGameDeleted += OnGameDeleted;

            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
            WeakReferenceMessenger.Default.UnregisterAll(this);
            WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
            {
                OnIncomingCall();
            });

            game.AddSnapshotListener();
        }
        public void Cleanup()
        {
            if (gameChangedHandler != null)
                game.OnGameChanged -= gameChangedHandler;

            game.OnGameDeleted -= OnGameDeleted;

            WeakReferenceMessenger.Default.UnregisterAll(this);

            game.RemoveSnapshotListener();
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
            if (!IsHostUser)
                return;

            game.IsStarted = true;
            game.SetDocument(_ => { });

            OnPropertyChanged(nameof(IsStartButtonVisible));
        }
        private void OnIncomingCall()
        {
            if (!IsStarted || popupOpen)
                return;

            popupOpen = true;

            WeakReferenceMessenger.Default.Send(
                new AppMessage<TimerSettings>(
                    new TimerSettings(10000, 1000)));

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                var popup = new ChallengePopup();
                await Shell.Current.CurrentPage.ShowPopupAsync(popup);
                popupOpen = false;
            });
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
            grdBoard.RestoreColors();

            for (int i = 0; i < game.PlayersCount; i++)
                grdBoard.UpdateButton(
                    game.GetPlayerPosition(i),
                    game.GetPlayerColor(i));
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
        private void InitOpponentsGrid(Grid grdOponnents)
        {
            int opponentsCount = game.Players.TotalPlayers - 1;

            for (int i = 0; i < opponentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(
                    new ColumnDefinition { Width = GridLength.Star });

                lstOponnentsLabels.Add(new Label
                {
                    Text = string.Empty,
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Padding = new Thickness(12)
                });

                grdOponnents.Add(lstOponnentsLabels[i], i, 0);
            }
        }
        private async Task PlayDiceAnimation()
        {
            if (!game.IsMyTurn() || !IsStarted)
                return;

            int totalFrames = 30;
            int iterations =
                (int)(animationTimer.TotalTimeInMilliseconds /
                animationTimer.IntervalInMilliseconds);

            double step = (double)totalFrames / iterations;
            double frameIndex = 0;

            for (int i = 0; i < iterations; i++)
            {
                int currentFrame =
                    Math.Min((int)frameIndex + 1, totalFrames);

                DiceImage = $"Dice/dice{currentFrame}c.png";

                await Task.Delay(
                    (int)animationTimer.IntervalInMilliseconds);

                frameIndex += step;
            }
        }
        public async Task HandleDoorAsync(string roomName)
        {
            SuggestionPopup suggestionPopup = new SuggestionPopup(roomName);

            object? result =
                await Shell.Current.CurrentPage.ShowPopupAsync(suggestionPopup);

            Accusation? accusation = result as Accusation;
            if (accusation == null)
                return;

            bool roomCorrect = game.CheckRoom(accusation.Room);
            bool weaponCorrect = game.CheckWeapon(accusation.Weapon);
            bool suspectCorrect = game.CheckSuspect(accusation.Suspect);

            if (suspectCorrect && roomCorrect && weaponCorrect)
            {
                game.EndGame();
                return;
            }

            CheckPopup checkPopup =
                new CheckPopup(roomCorrect, weaponCorrect, suspectCorrect);

            await Shell.Current.CurrentPage.ShowPopupAsync(checkPopup);

            game.EndTurnAfterSuggestion(); 
        }
        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError,
                    CommunityToolkit.Maui.Core.ToastDuration.Long, 14);
        }
    }
}