
using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class Game:GameModel
    {
        internal Game()
        {
            HostName = new User().Name;
            Created = DateTime.Now;
        }

        public override string OpponentName => IsHost ? GuestName : HostName;

        public override void SetDocument(Action<System.Threading.Tasks.Task> OnComplete)
        {
           Id = fbd.SetDocument(this, Keys.GamesCollection, Id, OnComplete);
        }

       
    }
}
