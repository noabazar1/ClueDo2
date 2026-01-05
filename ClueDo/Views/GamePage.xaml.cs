using ClueDo.ModelsLogic;
using ClueDo.ViewModels;
using System.Security.Cryptography.Xml;

namespace ClueDo.Views;

public partial class GamePage : ContentPage
{
    private readonly GamePageVM gpVM;

    public GamePage(Game game)
    {
        InitializeComponent();

        gpVM = new GamePageVM(game, grdOponnents, grdBoard);
        BindingContext = gpVM;

        game.InitBoard(grdBoard);
        game.DrawAllPlayers();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        gpVM.AddSnapshotListener();
    }

    protected override void OnDisappearing()
    {
        gpVM.RemoveSnapshotListener();
        base.OnDisappearing();
    }
}

