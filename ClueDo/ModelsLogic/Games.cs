using CommunityToolkit.Maui.Alerts;
using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    internal class Games : GamesModel
    {
        internal void AddGame()
        {
            IsBusy = true;
            Game game= new (SelectedGameSize);
            game.SetDocument(OnComplete);
        }
        private void OnComplete(Task task)
        {
            IsBusy = false;
            OnGameAdded?.Invoke(this, task.IsCompletedSuccessfully);
        }
    }
}
