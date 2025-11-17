using ClueDo.Models;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;
using System.ComponentModel.Design.Serialization;

namespace ClueDo.ModelsLogic
{
    public class Game : GameModel
    {
        public override string Player1 => IsHostUser ? GuestName : HostName;
        public override string Player2 => IsHostUser ? GuestName : HostName;
        public override string Player3 => IsHostUser ? GuestName : HostName;
        public override string Player4 => IsHostUser ? GuestName : HostName;
        public override string Player5 => IsHostUser ? GuestName : HostName;
        protected override GameStatus Status => IsHostUser && IsHostTurn || !IsHostUser && !IsHostTurn ?
    new GameStatus { CurrentStatus = GameStatus.Status.Play } :
    new GameStatus { CurrentStatus = GameStatus.Status.Wait };


        public Game()
        {
            HostName = new User().Name;
            Created = DateTime.Now;
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
            IsFull = true;
            GuestName = MyName;
            UpdateFbJoinGame(OnComplete);
        }

        private void UpdateFbJoinGame(Action<Task> OnComplete)
        {
            Dictionary<string, object> dict = new()
            {
                { nameof(IsFull), IsFull },
                { nameof(GuestName), GuestName }
            };
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
                OnGameDeleted?.Invoke(this, EventArgs.Empty);
        }

        private void OnChange(IDocumentSnapshot? snapshot, Exception? error)
        {
            Game? updatedGame = snapshot?.ToObject<Game>();
            if (updatedGame != null)
            {
                IsFull = updatedGame.IsFull;
                GuestName = updatedGame.GuestName;
                OnGameChanged?.Invoke(this, EventArgs.Empty);
                if (_status.CurrentStatus == GameStatus.Status.Play)
                    Play(updatedGame.Move[0], updatedGame.Move[1], false);
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
            //Mrs.White Start
            IndexButton white = new IndexButton(9, 14);
            white.BackgroundColor = Colors.White;
            board.Add(white, 9, 14);
            //Reverend Green Start
            IndexButton green = new IndexButton(4, 14);
            green.BackgroundColor = Color.FromArgb("#46865D");
            board.Add(green, 5, 14);
            //Mrs.Peacock Start
            IndexButton blue = new IndexButton(0, 10);
            blue.BackgroundColor = Color.FromArgb("#2961184");
            board.Add(blue, 0, 10);
            //Professor Plum Start
            IndexButton plum = new IndexButton(0, 4);
            plum.BackgroundColor = Color.FromArgb("#7C436E");
            board.Add(plum, 0, 4);
            //Miss Scarlet Start
            IndexButton red = new IndexButton(10, 0);
            red.BackgroundColor = Color.FromArgb("#B0251A");
            board.Add(red, 10, 0);
            //Colonel Mustard Start
            IndexButton yellow = new IndexButton(14, 5);
            yellow.BackgroundColor = Color.FromArgb("#D9AD3B");
            board.Add(yellow, 14, 5);
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
            if(_status.CurrentStatus == GameStatus.Status.Play)
            {
                IndexButton? btn = sender as IndexButton;
                if(btn!.Text == string.Empty)
                    Play(btn!.RowIndex, btn.ColumnIndex, true);
            }
        }
        protected override void Play(int rowIndex, int columnIndex, bool MyMove)
        {
            gameButtons![rowIndex, columnIndex].Text = nextPlay;
            gameBoard![rowIndex, columnIndex] = nextPlay!;
            if (MyMove)
            {
                Move[0] = rowIndex;
                Move[1] = columnIndex;
                _status.UpdateStatus();
                UpdateFbMove();
            }
        }
        protected override void UpdateFbMove()
        {
            Dictionary<string, object> dict = new()
            {
                {nameof(Move), Move },
                {nameof(IsHostTurn), IsHostTurn }
            };
            fbd.UpdateFields(Keys.GamesCollection, Id, dict, OnComplete);
        }
    }
}
