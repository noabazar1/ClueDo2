using ClueDo.Models;
using ClueDo.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : Models.ObservableObject
    {
        private readonly Game game;
        private readonly GameBoard grdBoard;
        private readonly OpponentsGrid grdOponnents;
        private readonly List<Label> lstOponnentsLabels = [];
        public string MyName => game.MyName;
        public bool IsMyTurn => game.IsMyTurn();
        public string StatusMessage => game.StatusMessage;
        public bool IsStarted => game.IsStarted;
        public bool IsHostUser => game.IsHostUser;
        public bool IsStartButtonVisible => IsHostUser && !game.IsStarted;
        private readonly TimerSettings animationTimer = new TimerSettings(600, 30);
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
        public string DiceResult
        {
            get
            {
                Player me = game.Players.PlayersList[game.Players.MyIndex];

                if (me.DiceValue > 0)
                    return me.DiceValue.ToString();

                return "";
            }
        }
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
        public GamePageVM(Game game,Grid grdOpponents, Grid board) 
        { 
            grdBoard = new GameBoard();
            this.game = game;
            this.grdOponnents = new OpponentsGrid(grdOpponents, game);
            game.OnGameChanged += OnGameChanged; 
            InitOpponentsGrid(board); 
            if (!game.IsHostUser) 
                game.UpdateGuestUser(OnComplete);
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            grdOponnents.DisplayOponnentsNames();
            UpdatGameGrid();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(DiceResult));
            RollDiceCommand.NotifyCanExecuteChanged();
        }
        private void UpdatGameGrid()
        {
            grdBoard.RestoreColors();
            for (int i = 0; i < game.PlayersCount; i++)
                grdBoard.UpdateButton(game.GetPlayerPosition(i), game.GetPlayerColor(i));
        }
        private void OnGameDeleted(object? sender, EventArgs e)
        {
            MainThread.InvokeOnMainThreadAsync(() =>
            {
                Shell.Current.Navigation.PopAsync();
                Toast.Make(Strings.GameDeleted, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
            });
        }
        
        private void InitOpponentsGrid(Grid grdOponnents)
        {
            int oponnentsCount = game.Players.TotalPlayers - 1;
            for (int i = 0; i < oponnentsCount; i++)
            {
                grdOponnents.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
                lstOponnentsLabels.Add(new Label
                {
                    Text = Strings.Waiting,
                    FontSize = 16,
                    Margin = new Thickness(5),
                    Padding = new Thickness(12)
                });
                grdOponnents.Add(lstOponnentsLabels[i], i, 0);
            }
        }
        private async Task PlayDiceAnimation()
        {
            if (!game.IsMyTurn())
                return;
            int totalFrames = 30;
            int interations = (int)(animationTimer.TotalTimeInMilliseconds / animationTimer.IntervalInMilliseconds);
            double step = (double)totalFrames / interations;
            double frameIndex = 0;
            for (int i = 0; i < interations; i++)
            {
                int currentFrame = Math.Min((int)frameIndex + 1, totalFrames);
                DiceImage = $"Dice/dice{currentFrame}c.png";
                await MainThread.InvokeOnMainThreadAsync(() => { });
                frameIndex += step;
                await Task.Delay((int)animationTimer.IntervalInMilliseconds);
            }
        }
        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError, CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

        }
        public void AddSnapshotListener()
        {
            game.AddSnapshotListener();
        }

        public void RemoveSnapshotListener()
        {
            game.RemoveSnapshotListener();
        }
    }
}
