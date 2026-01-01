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
        private readonly Grid grdBoard;
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
            Players.TotalPlayers = 0;
        }

        public override bool AddPlayer(string playerName)
        {
            int index = Players.Count;
            if (boardLogic != null)
            {
                IndexButton btn = boardLogic.GetButton(new Position(0, 0));
                Player player = new Player(new User().Name, 0, btn);
                Players.Add(player);
                PlayersNames.Add(playerName);

                DrawPlayer(player);
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
            if (boardLogic == null) return;

            IndexButton btn = boardLogic.GetButton(player.Position);
            btn.BackgroundColor = player.Color;
            player.Button = btn;
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
            if (sender is not IndexButton btn)
                return;

            Play(btn.Row, btn.Column, true);
        }

        protected override void Play(int rowIndex, int columnIndex, bool myMove)
        {
            if (boardLogic == null || Players == null)
                return;

            if (Players.PlayersList == null || Players.PlayersList.Count == 0)
                return;

            if (Players.MyIndex < 0 || Players.MyIndex >= Players.PlayersList.Count)
                return;

            IndexButton targetBtn =
                boardLogic.GetButton(new Position(rowIndex, columnIndex));

            if (targetBtn == null)
                return;

            Player currentPlayer = Players.PlayersList[Players.MyIndex];

            // ניקוי הכפתור הקודם של השחקן
            if (currentPlayer.Button != null)
            {
                currentPlayer.Button.Background = null;
                currentPlayer.Button.BackgroundColor = Color.FromArgb("#F7D275");
                currentPlayer.Button.Handler?.UpdateValue(nameof(Button.BackgroundColor));
            }

            // צביעת הכפתור החדש
            targetBtn.Background = null;
            targetBtn.BackgroundColor = currentPlayer.Color;
            targetBtn.Text = "X";

            // 🔴 זה קריטי לאנדרואיד (MaterialButton)
            targetBtn.Handler?.UpdateValue(nameof(Button.BackgroundColor));

            currentPlayer.Button = targetBtn;
            currentPlayer.Position = new Position(rowIndex, columnIndex);

            Players.Play(rowIndex, columnIndex);

            if (myMove)
            {
                boardLogic.MyTurn();
                _status.UpdateStatus();
                IsHostTurn = !IsHostTurn;
                UpdateFbMove();
            }
            else
            {
                boardLogic.OpponentTurn();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }

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
