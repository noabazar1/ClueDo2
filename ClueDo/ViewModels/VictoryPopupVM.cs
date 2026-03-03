using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ClueDo.ViewModels;

public partial class VictoryPopupVM : ObservableObject
{
    private readonly Popup popup;

    public VictoryPopupVM(Popup popup)
    {
        this.popup = popup;
    }

    [RelayCommand]
    private async Task Back()
    {
        popup.Close();
        await Shell.Current.GoToAsync("//MainArea");
    }
}