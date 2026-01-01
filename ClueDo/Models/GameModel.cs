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
        protected IndexButton[,] gameButtons = new IndexButton[15, 15];
        [Ignored]
        public EventHandler? OnGameChanged;
        [Ignored]
        public EventHandler? OnGameDeleted;
        [Ignored]
        public EventHandler? GameError;
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
        [Ignored]
        public string MyName { get; set; } = new User().Name;
        [Ignored]
        public bool IsHostUser { get; set; }
        [Ignored]
        public int MyIndex { get; protected set; } = 0;
        public int CurrentPlayers { get; set; } = 1;
        public int NextPlay { get; set; }
        [Ignored]
        public abstract string JoinStatus { get; }
        public List<string> PlayersNames { get; set; } = [];
        public string DiceResult { get; set; } = string.Empty;
        public Players Players { get; set; } = new();
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void RemoveSnapshotListener();
        public abstract void AddSnapshotListener();
        public abstract void JoinGame();
        public abstract Position GetPlayerPosition(int playerIndex);
        public abstract Color GetPlayerColor(int playerIndex);
        public abstract string GetPlayerName(int playerIndex);
        public abstract void DeleteDocument(Action<System.Threading.Tasks.Task> OnComplete);
        public abstract void Init(Grid board);
        protected abstract void OnButtonClicked(object? sender, EventArgs e);
        protected abstract void Play(int rowIndex, int columnIndex);
        protected abstract void UpdateStatus();
        protected abstract void UpdateFbMove();
        public abstract bool AddPlayer(string name);
        public abstract bool IsMyTurn();
        public abstract bool IsOponnentTurn(int oponnentIndex);
        protected abstract void OnChange(IDocumentSnapshot? snapshot, Exception? error);
    }
}
