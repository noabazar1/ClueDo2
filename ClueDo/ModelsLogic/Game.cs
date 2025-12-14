using ClueDo.Models;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        public List<PlayerPiece> PlayerPieces { get; private set; } = [];
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
    new GameStatus { CurrentStatus = GameStatus.Status.Play } :
    new GameStatus { CurrentStatus = GameStatus.Status.Wait };
        private readonly Grid grdBoard;
        private GameBoard? boardLogic;
        public Game(Grid grdBoard)
        {
            HostName = new User().Name;
            Created = DateTime.Now;
            this.grdBoard = grdBoard;
        }
        public Game()
        {
            grdBoard = new Grid();
        }
        public override string JoinStatus => CurrentPlayers + "/" + TotalPlayers;
        private readonly List<Position> startPositions = new()
        {
            new Position(9, 14),
            new Position(4, 14),
            new Position(0, 10),
            new Position(0, 4),
            new Position(10, 0),
            new Position(14, 5)
        };
        public override bool AddPlayer(string playerName)
        {
            int index = PlayersNames.Count;
            if (index >= startPositions.Count)
                return false;
            Position startPosition = startPositions[index];
            PlayersNames.Add(playerName);
            Color playerColor = GetPlayerColor(index);
            if (boardLogic != null)
            {
                boardLogic.PlacePlayer(startPosition, playerColor);
            }
            return true;
        }
        private Color GetPlayerColor(int index)
        {
            if (index == 0) return Colors.White;
            if (index == 1) return Color.FromArgb(Keys.Green);
            if (index == 2) return Color.FromArgb(Keys.Blue);
            if (index == 3) return Color.FromArgb(Keys.Plum);
            if (index == 4) return Color.FromArgb(Keys.Red);
            return Color.FromArgb(Keys.Mustard);
        }
        public override void JoinGame()
        {
            if (CurrentPlayers + 1 == TotalPlayers)
                fbd.UpdateField(Keys.GamesCollection, Id, nameof(IsFull), true, OnComplete);
            MyIndex = CurrentPlayers;
            PlayersNames.Add(MyName);
            fbd.StartBatch();
            fbd.BatchIncrementField(Keys.GamesCollection, Id, nameof(CurrentPlayers), 1);
            fbd.BatchUpdateField(Keys.GamesCollection, Id, nameof(PlayersNames), PlayersNames);
            fbd.CommitBatch(OnComplete);
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
            Players.Add(MyName);
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
                PlayersNames = game.PlayersNames;
                NextPlay = game.NextPlay;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
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
                _status.UpdateStatus();
                IsHostTurn = !IsHostTurn;
                UpdateFbMove();
            }
            else
                OnGameChanged?.Invoke(this, EventArgs.Empty);
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
            return NextPlay == MyIndex;
        }
        public override bool IsOponnentTurn(int oponnentIndex)
        {
            return oponnentIndex == NextPlay;
        }
    }
}
