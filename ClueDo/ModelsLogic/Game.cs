using ClueDo.Models;
using Plugin.CloudFirestore;
using System.Diagnostics;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        public override string JoinStatus => $"{CurrentPlayers}/{Players.TotalPlayers}";
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
            new GameStatus { CurrentStatus = GameStatus.Status.Play } : new GameStatus { CurrentStatus = GameStatus.Status.Wait };
        private Grid grdBoard;
        private GameBoard? boardLogic;
        public Game(Grid grdBoard)
        {
            Created = DateTime.Now;
            this.grdBoard = grdBoard;
            Player p = new(new User().Name, 0);
            Players.Add(p);
            Players.NextPlay = Players.TotalPlayers - 1;
        }
        public Game()
        {
            grdBoard = new Grid();
            Players.TotalPlayers = 0;
        }

        public override bool AddPlayer(string playerName)
        {
            int index = Players.Count;
            if (boardLogic != null)
            {
                Player tempPlayer = new Player("", index, null!);
                Position startPos = tempPlayer.Position;  

                IndexButton btn = boardLogic.GetButton(startPos);
                Player player = new Player(playerName, index, btn);
                Players.Add(player);
                PlayersNames.Add(playerName);
                return true;
            }
            return false;
        }

        public override void JoinGame()
        {
            if (CurrentPlayers + 1 == Players.TotalPlayers)
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
            if (boardLogic == null)
            {
                return;
            }
            IndexButton btn = boardLogic.GetButton(player.Position);
            if (btn != null)
            {
                btn.BackgroundColor = player.Color;
                player.Button = btn;
            }
        }
        public void DrawAllPlayers()
        {
            foreach (Player player in Players.PlayersList)
            {
                DrawPlayer(player);
            }
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
            if (ilr != null)
                return;

            if (string.IsNullOrEmpty(Id))
                return;

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
            if (snapshot != null && error == null)
            {
                Game? game = snapshot?.ToObject<Game>();
                if (game != null)
                {
                    int myIndex = Players.MyIndex;
                    Players = game.Players;
                    Players.MyIndex = myIndex;
                    IsHostTurn = game.IsHostTurn;
                    NextPlay = game.NextPlay;
                    CurrentPlayers = game.CurrentPlayers;
                    OnGameChanged?.Invoke(this,EventArgs.Empty);
                }
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public void PlacePlayer(int playerIndex, int row, int col)
        {
            Players.PlayersList[playerIndex].Position = new Position(row, col);
        }
        public void InitBoard(Grid grid)
        {
            if (boardLogic != null)
                return; 
            boardLogic = new GameBoard();
            boardLogic.Build(grid, OnButtonClicked);
        }

        public override void OnButtonClicked(object? sender, EventArgs e)
        {
            if (sender is not IndexButton btn)
                return;

            Play(btn.Row, btn.Column);
        }

        protected override void Play(int rowIndex, int columnIndex)
        {
            if (boardLogic == null || Players?.PlayersList == null)
                return;

            if (Players.PlayersList.Count == 0)
                return;

            if (Players.MyIndex < 0 || Players.MyIndex >= Players.PlayersList.Count)
                return;

            IndexButton targetBtn = boardLogic.GetButton(new Position(rowIndex, columnIndex));
            if (targetBtn == null)
                return;

            Player currentPlayer = Players.PlayersList[Players.MyIndex];

            if (currentPlayer.Button != null)
            {
                currentPlayer.Button.BackgroundColor = Color.FromArgb("#F7D275");
            }

            currentPlayer.Button = targetBtn;
            currentPlayer.Position = new Position(rowIndex, columnIndex);

            boardLogic.ResetBoardColors();
            DrawAllPlayers();


            boardLogic.MyTurn();
            _status.UpdateStatus();
            IsHostTurn = !IsHostTurn;

            UpdateFbMove();
            fbd.UpdateField(
                Keys.GamesCollection,
                Id,
                nameof(Players),
                Players,
                OnComplete
            );
        }

        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                {nameof(IsHostTurn), IsHostTurn },
                {nameof(Players), Players}
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
