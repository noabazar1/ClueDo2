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
            IndexButton white = new IndexButton(15, 11);
            white.BackgroundColor = Colors.White;
            board.Add(15, 11);
        }
    }
}
