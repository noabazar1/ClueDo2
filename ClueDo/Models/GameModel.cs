using Plugin.CloudFirestore.Attributes;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    public abstract class GameModel
    {
        protected FbData fbd = new();
        [Ignored]
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string GuestName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int RowSize {  get; set; }
        public abstract string OpponentName { get; }
        [Ignored]
        public string MyName { get; set; } = new User().Name;
        [Ignored]
        public string RowSizeName => $"{RowSize} X {RowSize}";
        [Ignored]
        public bool IsHost { get; set; }
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
