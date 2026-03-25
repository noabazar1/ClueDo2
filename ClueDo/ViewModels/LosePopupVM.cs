using ClueDo.Models;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClueDo.ViewModels
{
    public partial class LosePopupVM(Popup popup) : Models.ObservableObject
    {
        private readonly Popup popup = popup;

        [RelayCommand]
        private async Task Back()
        {
            popup.Close();
            await Shell.Current.GoToAsync(Keys.MainArea);
        }
    }
}
