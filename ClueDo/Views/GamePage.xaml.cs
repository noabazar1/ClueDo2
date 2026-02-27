using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Services;
using ClueDo.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.Views;

public partial class GamePage : ContentPage
{
    private readonly GamePageVM gpVM;
    private readonly Game game;
    bool popupOpen = false;
    private EventHandler? gameChangedHandler;
    private readonly IFriendsService friendsService = new FriendsService();

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
                if (roomCorrect && weaponCorrect && suspectCorrect)
                {
                    game.EndGame(game.MyName);
                    return;
                }
                else
                {
                    await this.ShowPopupAsync(new CheckPopup(roomCorrect, weaponCorrect, suspectCorrect));
                    game.EndTurnAfterSuggestion();
                }
                    
            }
        };
        gameChangedHandler = async (_, __) =>
        {
            if (!game.IsGameOver)
                return;

            if (popupOpen)
                return;

            popupOpen = true;

            if (game.WinnerName == game.MyName)
                await this.ShowPopupAsync(new VictoryPopup());
            else
                await this.ShowPopupAsync(new LosePopup(game.WinnerName!));
        };

        game.OnGameChanged += gameChangedHandler;
        WeakReferenceMessenger.Default.Register<AppMessage<string>>(this, async (r, m) =>
        {
            string incomingNumber = m.Value;

            if (!game.IsStarted || game.IsGameOver)
                return;

            if (popupOpen)
                return;

            popupOpen = true;

            object? result = await this.ShowPopupAsync(new ChallengePopup());

            popupOpen = false;

            if (result is bool success && !success)
            {
                string winner = game.Players
                    .PlayersList
                    .First(p => p.Name != game.MyName)
                    .Name;

                game.EndGame(winner);
            }
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        gpVM.AddSnapshotListener();
    }

    protected override void OnDisappearing()
    {
        gpVM.RemoveSnapshotListener();
        if (gameChangedHandler != null)
            game.OnGameChanged -= gameChangedHandler;
        WeakReferenceMessenger.Default.UnregisterAll(this);
        base.OnDisappearing();
    }
}

