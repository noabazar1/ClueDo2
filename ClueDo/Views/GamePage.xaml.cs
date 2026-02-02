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
                List<string> matches = game.Answer!.GetMatchingParts(accusation);
                if (matches.Count == 0)
                {
                    await DisplayAlert(
                        Strings.Wrong,
                        Strings.NoCorrectGuesses,
                        Strings.Ok
                    );
                }
                else
                {
                    string message = Strings.CorrectParameters;

                    if (matches.Contains(Strings.Room))
                        message += Strings.TheRoom;

                    if (matches.Contains(Strings.Weapon))
                        message += Strings.TheMurderWeapon;

                    if (matches.Contains(Strings.Suspect))
                        message += Strings.TheSuspect;

                    await DisplayAlert(
                        Strings.Check,
                        message.TrimEnd(),
                        Strings.Ok
                    );
                }
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

