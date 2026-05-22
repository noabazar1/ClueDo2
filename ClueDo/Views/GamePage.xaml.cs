using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class GamePage : ContentPage
{
    private readonly GamePageVM vm;
    public GamePage(Game game)
    {
        InitializeComponent();
        vm = new GamePageVM(game, grdBoard, grdOponnents);
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm.Initialize();
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        vm.Cleanup();
    }
}