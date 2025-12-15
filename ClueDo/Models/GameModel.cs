using ClueDo.ModelsLogic;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace ClueDo.Models
{
    public abstract class GameModel
    {
        protected enum Actions { Changed, Deleted }
        protected Actions action = Actions.Changed;
        protected FbData fbd = new();
        protected IListenerRegistration? ilr;
        protected GameStatus _status = new();
        protected string[,]? gameBoard;
        protected IndexButton[,]? gameButtons = new IndexButton[15, 15];
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
        public List<string> Players { get; set; } = new List<string>();
        [Ignored]
        public string MyName { get; set; } = new User().Name;
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public int MyIndex { get; protected set; } = 0;
        public int TotalPlayers { get; set; }
        public int CurrentPlayers { get; set; } = 1;
        public int NextPlay { get; set; }
        protected abstract string JoinStatus { get; }
        public List<string> PlayersNames { get; set; } = [];
        public string DiceResult { get; set; } = string.Empty;
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void JoinGame();
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void Init(Grid board);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void Play(int rowIndex, int columnIndx, bool MyMove);
        protected abstract void UpdateStatus();
        protected abstract void UpdateFbMove();
        public abstract bool AddPlayer(string name);
        public abstract bool IsMyTurn();
        public abstract bool IsOponnentTurn(int oponnentIndex);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
    }
}
