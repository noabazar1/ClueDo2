using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class LosePopup : Popup
{
	public LosePopup()
	{
		InitializeComponent();
        BindingContext = new LosePopupVM(this);
    }
}