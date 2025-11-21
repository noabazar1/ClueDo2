using ClueDo.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace ClueDo.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        protected string[,]? gameBoard;
        protected string? nextPlay;
        protected IndexButton[,]? gameButtons;
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        protected abstract GameStatus Status { get; }
        [Ignored]
        public string StatusMessage => Status.StatusMessage;
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public bool IsFull { get; set; }
        public bool IsHostTurn { get; set; } = false;
        public List<int> Move { get; set; } = [-1, -1];
        public GamePiece? PlayerPiece { get; set; }
        [Ignored]
        public abstract string? Player1 { get; }
        [Ignored]
        public abstract string? Player2 { get; }
        [Ignored]
        public abstract string? Player3 { get; }
        [Ignored]
        public abstract string? Player4 { get; }
        [Ignored]
        public abstract string? Player5 { get; }
        public List<string> Players { get; set; } = new List<string>();
        [Ignored]
        public string MyName { get; set; } = new User().Name;
        [Ignored]
        public bool IsHostUser { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void Init(Grid board);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void Play(int rowIndex, int columnIndx, bool MyMove);
        protected abstract void UpdateStatus();
        protected abstract void UpdateFbMove();


    }
}
