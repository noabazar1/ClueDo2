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
                List<string> matches = game.Answer.GetMatchingParts(accusation);
                if (matches.Count == 3)
                {
                    await DisplayAlert(
                        "You win!",
                        "You solved the mystery!",
                        "Amazing"
                    );
                    return;
                }

                if (matches.Count == 0)
                {
                    await DisplayAlert(
                        "Wrong",
                        "No correct guesses",
                        "Okay"
                    );
                }
                else
                {
                    string message = "The correct parameters:\n";

                    if (matches.Contains("Room"))
                        message += "The room\n";

                    if (matches.Contains("Weapon"))
                        message += "The murder weapon\n";

                    if (matches.Contains("Suspect"))
                        message += "The suspect\n";

                    await DisplayAlert(
                        "Check",
                        message.TrimEnd(),
                        "Okay"
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

