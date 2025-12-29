using ClueDo.Models;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        public override string JoinStatus => CurrentPlayers + "/" + Players.TotalPlayers;
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
    new GameStatus { CurrentStatus = GameStatus.Status.Play } :
    new GameStatus { CurrentStatus = GameStatus.Status.Wait };
        private readonly Grid grdBoard;
        private readonly int totalPlayers;
        private GameBoard? boardLogic;
        public Game(Grid grdBoard, int totalPlayers)
        {
            Created = DateTime.Now;
            this.grdBoard = grdBoard;
            boardLogic = new GameBoard();
            Player p = new(new User().Name, 0);
            Players.Add(p);
            Players.TotalPlayers = totalPlayers;
            Players.NextPlay = totalPlayers - 1;
        }
        public Game()
        {
            grdBoard = new Grid();
            totalPlayers = 0;
        }
        private readonly List<Position> startPositions =
        [
            new Position(9, 14),
            new Position(4, 14),
            new Position(0, 10),
            new Position(0, 4),
            new Position(10, 0),
            new Position(14, 5)
        ];
        public override bool AddPlayer(string playerName)
        {
            int index = Players.Count;
            if (index >= startPositions.Count)
                return false;

            Position startPos = startPositions[index];
            Color color = PlayerColor(index);

            Player player = new(playerName, index);
            Players.Add(player);
            PlayersNames.Add(playerName);

            DrawPlayer(player);
            return true;
        }

        public static Color PlayerColor(int index)
        {
            if (index == 0) return Colors.White;
            if (index == 1) return Color.FromArgb("#46865D");
            if (index == 2) return Color.FromArgb("#2961184");
            if (index == 3) return Color.FromArgb("#7C436E");
            if (index == 4) return Color.FromArgb("#B0251A");
            return Color.FromArgb("#D9AD3B");
        }
        public override void JoinGame()
        {
            if (CurrentPlayers + 1 == TotalPlayers)
                fbd.UpdateField(Keys.GamesCollection, Id, nameof(IsFull), true, OnComplete);
            Players.MyIndex = CurrentPlayers;
            Player p = new(MyName, CurrentPlayers);
            Players.Add(p);
            fbd.StartBatch();
            fbd.BatchIncrementField(Keys.GamesCollection, Id, nameof(CurrentPlayers), 1);
            fbd.BatchUpdateField(Keys.GamesCollection, Id, nameof(PlayersNames), PlayersNames);
            fbd.CommitBatch(OnComplete);
        }
        public override Position GetPlayerPosition(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Position;
        }
        public override Color GetPlayerColor(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Color;
        }

        public override string GetPlayerName(int playerIndex)
        {
            return Players.PlayersList[playerIndex].Name;
        }
        private void DrawPlayer(Player player)
        {
            if (boardLogic == null) return;

            IndexButton btn = boardLogic.GetButton(player.Position);
            btn.BackgroundColor = player.Color;
        }

        protected override void UpdateStatus()
        {
            _status.CurrentStatus = IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
                GameStatus.Status.Play : GameStatus.Status.Wait;
        }

        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

        public void UpdateGuestUser(Action<Task> OnComplete)
        {
            IsFull = Players.Count >= 5; 
            UpdateFbJoinGame(OnComplete);
        }

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

        public override void AddSnapshotListener()
        {
            ilr = fbd.AddSnapshotListener(Keys.GamesCollection, Id, OnChange);
        }

        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }

        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? game = snapshot?.ToObject<Game>();
            if (game != null)
            {
                CurrentPlayers = game.CurrentPlayers;
                int myIndex = Players.MyIndex;
                Players = game.Players;
                Players.MyIndex = myIndex;
                NextPlay = game.NextPlay;
                if (CurrentPlayers == Players.Count)
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                else
                    GameError?.Invoke(this, EventArgs.Empty);
            }
            else
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public override void Init(Grid board)
        {
            boardLogic = new GameBoard();
            boardLogic.Build(board, OnButtonClicked);
        }


        protected override void OnButtonClicked(object? sender, EventArgs e)
        {
            IndexButton? btn = sender as IndexButton;
            if (btn != null)
            {
                Play(btn.Row, btn.Column, true);
            }
        }
        protected override void Play(int rowIndex, int columnIndex, bool MyMove)
        {
            if (MyMove)
            {
                if (boardLogic != null)
                    boardLogic.MyTurn();
                _status.UpdateStatus();
                IsHostTurn = !IsHostTurn;
                UpdateFbMove();
            }
            else
            {
                if (boardLogic != null)
                    boardLogic.OpponentTurn();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
            Players.Play(rowIndex, columnIndex);
            fbd.UpdateField(Keys.GamesCollection, Id, nameof(Players), Players, OnComplete);
        }
        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                {nameof(IsHostTurn), IsHostTurn }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        public override bool IsMyTurn()
        {
            return Players.IsMyTurn();
        }
        public override bool IsOponnentTurn(int oponnentIndex)
        {
            return Players.IsOponnentTurn(oponnentIndex);
        }
    }
}
