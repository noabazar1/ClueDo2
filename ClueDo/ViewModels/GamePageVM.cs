using ClueDo.Models;
using ClueDo.ModelsLogic;
using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;

namespace ClueDo.ViewModels
{
    public partial class GamePageVM : ObservableObject
    {
        private readonly Game game;
        public string MyName => game.MyName;
        public string Player1 => game.Player1;
        public string Player2 => game.Player2;
        public string Player3 => game.Player3;
        public string Player4 => game.Player4;
        public string Player5 => game.Player5;
        public string StatusMessage => game.StatusMessage;
        public GameBoard Board { get; set; } = new GameBoard();
        public GamePageVM(Game game, Grid board)
        {
            _ = new Game(board);
            game.OnGameChanged += OnGameChanged;
            game.Init(board);
            this.game = game;
            if (!game.IsHostUser)
                game.UpdateGuestUser(OnComplete);
        }

        private void OnGameChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(Player1));
            OnPropertyChanged(nameof(Player2));
            OnPropertyChanged(nameof(Player3));
            OnPropertyChanged(nameof(Player4));
            OnPropertyChanged(nameof(Player5));
        }

        private void OnComplete(Task task)
        {
            if (!task.IsCompletedSuccessfully)
                Toast.Make(Strings.JoinGameError, CommunityToolkit.Maui.Core.ToastDuration.Long, 14);

        }
        public void MovePiece(int x, int y)
        {
            int newX = Board.PlayerPiece.X + x;
            int newY = Board.PlayerPiece.Y + y;
            if (newX >= 0 && newX < Board.Columns &&
                newY >= 0 && newY < Board.Rows)
            {
                Board.PlayerPiece.X = newX;
                Board.PlayerPiece.Y = newY;
            }
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
