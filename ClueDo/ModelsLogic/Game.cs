using ClueDo.Models;
using Plugin.CloudFirestore;
using System.Diagnostics;
using CommunityToolkit.Maui.Views;
using ClueDo.Views;
using System.Threading.Tasks;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        private readonly Grid grdBoard;
        private GameBoard? boardLogic;
        private readonly Dice dice = new();
        private bool _gameOverPopupShown = false;
        public event Action<string>? DoorClicked;
        public override string JoinStatus => $"{CurrentPlayers}/{Players.TotalPlayers}";
        protected override GameStatus Status => _status;
        public string? CurrentRoom { get; private set; }
        public Game(Grid grdBoard)
        {
            Created = DateTime.Now;
            this.grdBoard = grdBoard;
            Player p = new(new User().Name, 0);
            Players.Add(p);
            InitBoard(grdBoard);
        }
        public Game()
        {
            grdBoard = [];
            Players.TotalPlayers = 0;
        }
        public override void EnsureAnswerGenerated(string myUserId)
        {
            if (Answer == null && myUserId == HostId && !string.IsNullOrEmpty(Id))
            {
                Answer = Answer.Generate();
                fbd.UpdateField(Keys.GamesCollection, Id, nameof(Answer), Answer, OnComplete);
            }
        }
        public override bool AddPlayer(string playerName)
        {
            int index = Players.Count;
            if (boardLogic != null)
            {
                Player tempPlayer = new("", index, null!);
                Position startPos = tempPlayer.Position;
                IndexButton btn = boardLogic.GetButton(startPos);
                Player player = new(playerName, index, btn);
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
            fbd.BatchUpdateField(Keys.GamesCollection, Id, nameof(Players), Players);
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
        public override void DrawAllPlayers()
        {
            foreach (Player player in Players.PlayersList)
            {
                DrawPlayer(player);
            }
        }
        public override void SetDocument(Action<Task> OnComplete)
        {
            Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }
        public override void UpdateGuestUser(Action<Task> OnComplete)
        {
            IsFull = Players.Count >= 5;
            UpdateFbJoinGame(OnComplete);
        }
        public override void AddSnapshotListener()
        {
            if (ilr == null && !string.IsNullOrEmpty(Id))
            {
                ilr = fbd.AddSnapshotListener(
                    Keys.GamesCollection,
                    Id,
                    OnChange);
            }
        }
        public override void RemoveSnapshotListener()
        {
            ilr?.Remove();
            DeleteDocument(OnComplete);
        }
        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public override void PlacePlayer(int playerIndex, int row, int col)
        {
            Players.PlayersList[playerIndex].Position = new Position(row, col);
        }
        public override void InitBoard(Grid grid)
        {
            if (boardLogic == null)
            {
                boardLogic = new GameBoard();
                boardLogic.Build(grid, OnButtonClicked);
            }
        }
        public override void OnButtonClicked(object? sender, EventArgs e)
        {
            if (IsStarted)
            {
                if (sender is IndexButton btn)
                {
                    if (btn.IsDoor)
                    {
                        Player me = Players.PlayersList[Players.MyIndex];

                        if (me.MovesLeft > 0 &&
                            Game.CanMoveTo(me, btn.Row, btn.Column))
                        {
                            CurrentRoom = btn.RoomName;
                            me.IsInRoom = true;
                            DoorClicked?.Invoke(btn.RoomName!);
                        }
                    }
                    else
                    {
                        Play(btn.Row, btn.Column);
                    }
                }
            }
        }
        public override void EndTurnAfterSuggestion()
        {
            if (IsMyTurn())
            {
                Player me = Players.PlayersList[Players.MyIndex];
                me.MovesLeft = 0;

                CurrentTurnIndex++;
                if (CurrentTurnIndex >= Players.PlayersList.Count)
                    CurrentTurnIndex = 0;

                SyncStatus();
                UpdateFbMove();
                OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public override void RollDiceForCurrentPlayer()
        {
            if (IsStarted && IsMyTurn())
            {
                Player me = Players.PlayersList[Players.MyIndex];
                if (me.MovesLeft == 0)
                {
                    dice.RollDice();
                    int total = dice.Die1 + dice.Die2;
                    me.DiceValue = total;
                    me.MovesLeft = total;

                    SyncStatus();
                    UpdateFbMove();
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public override void EndGame()
        {
            IsGameOver = true;
            WinnerName = Players.PlayersList[MyIndex].Name;

            Dictionary<string, object> dict = new()
            {
                { nameof(IsGameOver), IsGameOver },
                { nameof(WinnerName), WinnerName }
            };

            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        public override bool CheckRoom(string room)
        {
            return room == Answer!.Room;
        }
        public override bool CheckWeapon(string weapon)
        {
            return weapon == Answer!.Weapon;
        }
        public override bool CheckSuspect(string suspect)
        {
            return suspect == Answer!.Suspect;
        }
        public override bool IsMyTurn()
        {
            return Players.MyIndex == CurrentTurnIndex;
        }
        public override bool IsOponnentTurn(int oponnentIndex)
        {
            return Players.IsOponnentTurn(oponnentIndex);
        }
        public override void EliminateCurrentPlayer()
        {
            Player me = Players.PlayersList[Players.MyIndex];
            Players.PlayersList.Remove(me);
            me.IsEliminated = true;

            if (Players.MyIndex >= Players.PlayersList.Count)
                Players.MyIndex = 0;

            if (Players.PlayersList.Count == 1)
            {
                WinnerName = Players.PlayersList[0].Name;
                IsGameOver = true;
            }

            Dictionary<string, object> dict = new()
            {
                { nameof(Players), Players },
                { nameof(IsGameOver), IsGameOver },

            };
            if (IsGameOver && WinnerName != null)
                dict.Add(nameof(WinnerName), WinnerName);

            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        protected override void UpdateStatus()
        {
            _status.CurrentStatus = IsMyTurn()
                ? GameStatus.Status.Play
                : GameStatus.Status.Wait;
        }
        protected override void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            if (snapshot != null && error == null)
            {
                Game? game = snapshot.ToObject<Game>();
                if (game != null)
                {
                    int myIndex = Players.MyIndex;
                    Players = game.Players;
                    Players.MyIndex = myIndex;
                    IsStarted = game.IsStarted;
                    IsHostTurn = game.IsHostTurn;
                    NextPlay = game.NextPlay;
                    CurrentPlayers = game.CurrentPlayers;
                    CurrentTurnIndex = game.CurrentTurnIndex;
                    IsGameOver = game.IsGameOver;
                    WinnerName = game.WinnerName;

                    if (IsGameOver && !_gameOverPopupShown)
                    {
                        _gameOverPopupShown = true;

                        MainThread.BeginInvokeOnMainThread(async () =>
                        {
                            if (WinnerName == Players.PlayersList[Players.MyIndex].Name)
                                await Shell.Current.CurrentPage.ShowPopupAsync(new VictoryPopup());
                            else
                                await Shell.Current.CurrentPage.ShowPopupAsync(new LosePopup());
                        });
                    }

                    SyncStatus();

                    if (boardLogic != null)
                    {
                        boardLogic.ResetBoardColors();
                        DrawAllPlayers();
                    }

                    OnGameChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        protected override void Play(int rowIndex, int columnIndex)
        {
            if (boardLogic != null && Players?.PlayersList != null &&
                Players.PlayersList.Count > 0 &&
                Players.MyIndex >= 0 &&
                Players.MyIndex < Players.PlayersList.Count)
            {
                IndexButton targetBtn = boardLogic.GetButton(new Position(rowIndex, columnIndex));
                if (targetBtn != null)
                {
                    Player currentPlayer = Players.PlayersList[Players.MyIndex];
                    if (currentPlayer.MovesLeft > 0 &&
                        Game.CanMoveTo(currentPlayer, rowIndex, columnIndex))
                    {
                        if (currentPlayer.Button != null)
                            currentPlayer.Button.BackgroundColor = Colors.Transparent;

                        currentPlayer.Button = targetBtn;
                        currentPlayer.Position = new Position(rowIndex, columnIndex);
                        currentPlayer.MovesLeft--;

                        boardLogic.ResetBoardColors();
                        DrawAllPlayers();
                        boardLogic.MyTurn();

                        if (currentPlayer.MovesLeft == 0)
                        {
                            CurrentTurnIndex++;
                            if (CurrentTurnIndex >= Players.PlayersList.Count)
                                CurrentTurnIndex = 0;
                        }

                        UpdateFbMove();
                        fbd.UpdateField(
                            Keys.GamesCollection,
                            Id,
                            nameof(Players),
                            Players,
                            OnComplete);
                    }
                }
            }
        }
        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(CurrentTurnIndex), CurrentTurnIndex },
                { nameof(Players), Players }
            };

            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
        private void DrawPlayer(Player player)
        {
            if (boardLogic != null)
            {
                IndexButton btn = boardLogic.GetButton(player.Position);
                if (btn != null)
                {
                    btn.BackgroundColor = player.Color;
                    player.Button = btn;
                }
            }
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
        private void OnComplete(Task task)
        {
            if (task.IsCompletedSuccessfully)
            {
                if (action == Actions.Deleted)
                    OnGameDeleted?.Invoke(this, EventArgs.Empty);
                else
                    OnGameChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        private void SyncStatus()
        {
            _status.CurrentStatus = IsMyTurn()
                ? GameStatus.Status.Play
                : GameStatus.Status.Wait;
        }
        private static bool CanMoveTo(Player player, int targetRow, int targetCol)
        {
            int dRow = Math.Abs(player.Position.Row - targetRow);
            int dCol = Math.Abs(player.Position.Column - targetCol);
            return dRow + dCol == 1;
        }
    }
}