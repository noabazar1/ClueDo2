
using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    internal class Game:GameModel
    {
        internal Game(GameSize selectedGameSize)
        {
            HostName = new User().Name;
            RowSize = selectedGameSize.Size;
            Created = DateTime.Now;
        }

        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
           Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

       
    }
}
