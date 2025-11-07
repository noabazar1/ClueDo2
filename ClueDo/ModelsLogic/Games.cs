using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    internal class Games : GamesModel
    {
        internal void AddGame()
        {
            IsBusy = true;
            currentGame = new();
            currentGame.IsHost = true;
            currentGame.SetDocument(OnComplete);
        }
        private void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, currentGame!);
        }
    }
}
