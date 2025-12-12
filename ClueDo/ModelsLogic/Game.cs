using ClueDo.Models;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        public override string Player1 => Players.Count > 0 ? Players[0] : "";
        public override string Player2 => Players.Count > 1 ? Players[1] : "";
        public override string Player3 => Players.Count > 2 ? Players[2] : "";
        public override string Player4 => Players.Count > 3 ? Players[3] : "";
        public override string Player5 => Players.Count > 4 ? Players[4] : "";
        public List<PlayerPiece> PlayerPieces { get; private set; } = [];
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
    new GameStatus { CurrentStatus = GameStatus.Status.Play } :
    new GameStatus { CurrentStatus = GameStatus.Status.Wait };
        private readonly Grid grdBoard;
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
        private readonly List<PlayerPiece> startPositions = new()
        {
            { new PlayerPiece("Mrs. White", Colors.White, new Position(9, 14)) },
            { new PlayerPiece("Reverend Green", Color.FromArgb(Keys.Green), new Position(4, 14)) },
            { new PlayerPiece("Mrs. Peacock", Color.FromArgb(Keys.Blue), new Position(0, 10)) },
            { new PlayerPiece("Professor Plum", Color.FromArgb(Keys.Plum), new Position(0, 4)) },
            { new PlayerPiece("Miss Scarlet", Color.FromArgb(Keys.Red), new Position(10, 0)) },
            { new PlayerPiece("Colonel Mustard", Color.FromArgb(Keys.Mustard), new Position(14, 5)) }
        };
        public override void AddPlayer()
        {
            if (startPositions.Count > 0)
            {
                IndexButton btn = new(startPositions[Players.Count - 1].CurrentPosition.x, startPositions[Players.Count - 1].CurrentPosition.y);
                btn.BackgroundColor = startPositions[Players.Count - 1].Color;
                grdBoard.Add(btn, btn.Row, btn.Column);
                startPositions.RemoveAt(Players.Count - 1);
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

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                GuestName = updatedGame.GuestName;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
                IsHostTurn = updatedGame.IsHostTurn;
                UpdateStatus();
                if (_status.CurrentStatus == GameStatus.Status.Play)
                { }
            }
            else
            {
                MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Shell.Current.Navigation.PopAsync();
                    Toast.Make(Strings.GameDeleted, CommunityToolkit.Maui.Core.ToastDuration.Long, 14).Show();
                });
            }
        }

        public override void DeleteDocument(Action<Task> OnComplete)
        {
            fbd.DeleteDocument(Keys.GamesCollection, Id, OnComplete);
        }
        public override void Init(Grid board)
        {
            int rowSize = 15;
            gameBoard = new string[rowSize, rowSize];
            gameButtons = new IndexButton[rowSize, rowSize];
            IndexButton btn;
            for (int i = 0; i < rowSize; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < rowSize; i++)
                for (int j = 0; j < rowSize; j++)
                {
                    btn = new IndexButton(i, j);
                    gameButtons[i, j] = btn;
                    btn.Clicked += OnButtonClicked;
                    board.Add(btn, j, i);
                }
            //Kitchen
            for (int i = 11; i < rowSize; i++)
                for (int j = 11; j < rowSize; j++)
                {
                    var kitchen = new IndexButton(i, j);
                    kitchen.IsEnabled = false;
                    kitchen.BackgroundColor = Colors.LightCoral;
                    board.Add(kitchen, i, j);
                }
            //Conservatory
            for (int i = 0; i < 4; i++) 
                for (int j = 11; j < rowSize; j++)
                {
                    var Conservatory = new IndexButton(i, j);
                    Conservatory.IsEnabled = false;
                    Conservatory.BackgroundColor = Colors.LightCoral;
                    board.Add(Conservatory, i, j);
                }
            //Study
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 3; j++)
                {
                    var Study = new IndexButton(i, j);
                    Study.IsEnabled = false;
                    Study.BackgroundColor = Colors.LightCoral;
                    board.Add(Study, i, j);
                }
            //Lounge
            for (int i = 11; i < rowSize; i++)
                for (int j = 0; j < 4; j++)
                {
                    var Lounge = new IndexButton(i, j);
                    Lounge.IsEnabled = false;
                    Lounge.BackgroundColor = Colors.LightCoral;
                    board.Add(Lounge, i, j);
                }
            //Dining Room
            for (int i = 10; i < rowSize; i++)
                for (int j = 6; j < 10; j++)
                {
                    var DiningRoom = new IndexButton(i, j);
                    DiningRoom.IsEnabled = false;
                    DiningRoom.BackgroundColor = Colors.LightCoral;
                    board.Add(DiningRoom, i, j);
                }
            //Ballroom
            for (int i = 6; i < 9; i++)
            {
                int j = 14;
                var Ballroom = new IndexButton(i, j);
                Ballroom.IsEnabled = false;
                Ballroom.BackgroundColor = Colors.LightCoral;
                board.Add(Ballroom, i, j);
            }
            for (int i = 5; i < 10; i++)
                for (int j = 11; j < 14; j++)
                {
                    var Ballroom = new IndexButton(i, j);
                    Ballroom.IsEnabled = false;
                    Ballroom.BackgroundColor = Colors.LightCoral;
                    board.Add(Ballroom, i, j);
                }
            for (int i = 5; i < 9; i++)
            {
                int j = 10;
                var Ballroom = new IndexButton(i, j);
                Ballroom.IsEnabled = false;
                Ballroom.BackgroundColor = Colors.LightCoral;
                board.Add(Ballroom, i, j);
            }
            //Billiard Room
            for (int i = 0; i < 4; i++)
                for (int j = 8; j < 10; j++)
                {
                    var BilliardRoom = new IndexButton(i, j);
                    BilliardRoom.IsEnabled = false;
                    BilliardRoom.BackgroundColor = Colors.LightCoral;
                    board.Add(BilliardRoom, i, j);
                }
            //Library
            for (int i = 0; i < 5; i++)
                for (int j = 5; j < 7; j++)
                {
                    var Library = new IndexButton(i, j);
                    Library.IsEnabled = false;
                    Library.BackgroundColor = Colors.LightCoral;
                    board.Add(Library, i, j);
                }
            //Hall
            for (int i = 6; i < 10; i++)
                for (int j = 0; j < 5; j++)
                {
                    var Hall = new IndexButton(i, j);
                    Hall.IsEnabled = false;
                    Hall.BackgroundColor = Colors.LightCoral;
                    board.Add(Hall, i, j);
                }
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
    }
}
