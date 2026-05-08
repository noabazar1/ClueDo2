using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class for the lose popup, which is shown when the player loses the game. It has a button to go back
    /// to the main menu.
    /// </summary>
    /// <param name="popup"></param>
    public partial class LosePopupVM(Popup popup) : Models.ObservableObject
    {
        private readonly Popup popup = popup;
        /// <summary>
        /// method that is called when the player clicks the back button. It closes the popup and navigates
        /// back to the main menu.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task Back()
        {
            popup.Close();
            await Shell.Current.GoToAsync(Keys.MainArea);
        }
    }
}
