using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class SuggestionPopup : Popup
{
    public SuggestionPopup(string roomName)
    {
        InitializeComponent();
        BindingContext = new SuggestionPopupVM(roomName, this);
    }
}