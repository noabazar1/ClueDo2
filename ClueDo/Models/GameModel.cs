using Plugin.CloudFirestore.Attributes;
using ClueDo.ModelsLogic;

namespace ClueDo.Models
{
    internal abstract class GameModel
    {
        protected FbData fbd = new();
        [Ignored]
        public string Id { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public int RowSize {  get; set; }
        [Ignored]
        public string RowSizeName => $"{RowSize} X {RowSize}";
        public abstract void SetDocument(Action<System.Threading.Tasks.Task> OnComplete);
    }
}
