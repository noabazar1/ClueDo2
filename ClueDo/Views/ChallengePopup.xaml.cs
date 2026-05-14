using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;
public partial class ChallengePopup : Popup
{
    private readonly ChallengePopupVM vm;

    public ChallengePopup()
    {
        InitializeComponent();
        vm = new ChallengePopupVM(this);
        BindingContext = vm;
        this.Closed += (_, __) => vm.Cleanup();
    }
}