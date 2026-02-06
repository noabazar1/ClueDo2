using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;

namespace ClueDo.Views;

public partial class GamePage : ContentPage
{
    private readonly GamePageVM gpVM;
    private readonly Game game;
    bool popupOpen = false;

    public GamePage(Game game)
    {
        InitializeComponent();
        this.game = game;
        gpVM = new GamePageVM(game, grdOponnents, grdBoard);
        BindingContext = gpVM;

        game.InitBoard(grdBoard);
        game.DrawAllPlayers();

        game.DoorClicked += async (string roomName) =>
        {
            if (popupOpen)
                return;

            popupOpen = true;

            SuggestionPopup popup = new SuggestionPopup(roomName);
            object? result = await this.ShowPopupAsync(popup);

            popupOpen = false;

            if (result is Accusation accusation)
            {
                bool roomCorrect = game.Answer!.Room == accusation.Room;
                bool weaponCorrect = game.Answer!.Weapon == accusation.Weapon;
                bool suspectCorrect = game.Answer!.Suspect == accusation.Suspect;
                await this.ShowPopupAsync(new CheckPopup(roomCorrect, weaponCorrect, suspectCorrect));
                game.EndTurnAfterSuggestion();
            }
        };
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

