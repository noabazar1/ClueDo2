using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class VictoryPopup : Popup
{
    public VictoryPopup()
    {
        InitializeComponent();
        BindingContext = new VictoryPopupVM(this);
    }
}