using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;
public partial class GamePage : ContentPage
{
    private GamePageVM? vm;
    private GameBoard? board;

    public GamePage(Game game)
    {
        InitializeComponent();

        board = new GameBoard();
        board.Build(grdBoard, game.OnButtonClicked);
        game.InitBoard(grdBoard);
        game.DoorClicked += OnDoorClicked;
        vm = new GamePageVM(game, grdOponnents, board);
        BindingContext = vm;

        vm.Initialize();
    }
    public GamePage()
    {
        InitializeComponent ();
    }
    private async void OnDoorClicked(string roomName)
    {
        if (BindingContext is GamePageVM vm)
            await vm.HandleDoorAsync(roomName);
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        vm?.Initialize();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        if (BindingContext is GamePageVM vm)
            vm.Cleanup();
    }
}