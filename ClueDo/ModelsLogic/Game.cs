using ClueDo.Models;
using ClueDo.Views;
using CommunityToolkit.Maui.Alerts;
using Plugin.CloudFirestore;
using System.Xml.Linq;

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
        public override void InitGrid(Grid board)
        {
            for (int i = 0; i < 15; i++)
            {
                board.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                board.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            }
            for (int i = 0; i < 15; i++)
                for (int j = 0; j < 15; j++)
                {
                    board.Add(new IndexButton(i, j), j, i);
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
            for (int i = 11; i < 15; i++)
                for (int j = 11; j < 15; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Conservatory
            for (int i = 0; i < 4; i++) 
                for (int j = 11; j < 15; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Study
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 3; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Lounge
            for (int i = 11; i < 15; i++)
                for (int j = 0; j < 4; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Dining Room
            for (int i = 10; i < 15; i++)
                for (int j = 6; j < 10; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Ballroom
            for (int i = 6; i < 9; i++)
            {
                int j = 14;
                var btn = new IndexButton(i, j);
                btn.IsEnabled = false;
                btn.BackgroundColor = Colors.LightCoral;
                board.Add(btn, i, j);
            }
            for (int i = 5; i < 10; i++)
                for (int j = 11; j < 14; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            for (int i = 5; i < 9; i++)
            {
                int j = 10;
                var btn = new IndexButton(i, j);
                btn.IsEnabled = false;
                btn.BackgroundColor = Colors.LightCoral;
                board.Add(btn, i, j);
            }
            //Billiard Room
            for (int i = 0; i < 4; i++)
                for (int j = 8; j < 10; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Library
            for (int i = 0; i < 5; i++)
                for (int j = 5; j < 7; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
            //Hall
            for (int i = 6; i < 10; i++)
                for (int j = 0; j < 5; j++)
                {
                    var btn = new IndexButton(i, j);
                    btn.IsEnabled = false;
                    btn.BackgroundColor = Colors.LightCoral;
                    board.Add(btn, i, j);
                }
        }
    }
}
