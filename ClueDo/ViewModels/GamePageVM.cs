using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Views;
using CommunityToolkit.Maui.Alerts;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        private readonly GameBoard grdBoard;
        private readonly OpponentsGrid grdOponnents;
        private readonly List<Label> lstOponnentsLabels = [];
        public string MyName => game.MyName;
        public bool IsMyTurn => game.IsMyTurn();
        public string StatusMessage => game.StatusMessage;
        public ICommand RollDiceCommand { get; }
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
        public GamePageVM(Game game,Grid grdOpponents, Grid board) 
        { 
            grdBoard = new GameBoard();
            this.game = game;
            this.grdOponnents = new OpponentsGrid(grdOpponents, game);
            game.OnGameChanged += OnGameChanged; 
            InitOpponentsGrid(board); 
            if (!game.IsHostUser) 
                game.UpdateGuestUser(OnComplete);
            RollDiceCommand = new Command(() =>
            {
                game.RollDiceForCurrentPlayer();
            });
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            grdOponnents.DisplayOponnentsNames();
            UpdatGameGrid();
            OnPropertyChanged(nameof(IsMyTurn));
            OnPropertyChanged(nameof(StatusMessage));
            OnPropertyChanged(nameof(DiceResult));
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
