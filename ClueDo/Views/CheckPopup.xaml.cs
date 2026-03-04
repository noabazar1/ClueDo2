using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class CheckPopup : Popup
{
    public CheckPopup(bool roomCorrect, bool weaponCorrect, bool suspectCorrect)
    {
        InitializeComponent();

        BindingContext = new CheckPopupVM(roomCorrect, weaponCorrect, suspectCorrect, () => Close());
    }
}