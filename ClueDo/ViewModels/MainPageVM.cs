using ClueDo.Models;
using ClueDo.ModelsLogic;
using ClueDo.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that manages the data and logic for the main page of the application, allowing users to see a
    /// list of available games and join them. The MainPageVM class inherits from the ObservableObject 
    /// class, which provides the necessary functionality for property change notifications, allowing the 
    /// UI to update when the underlying data changes.
    /// </summary>
    public partial class MainPageVM : ObservableObject
    {
        private readonly Games games = new();
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool isAlertShown = false;
        public ICommand AddGameCommand { get; }
        public ICommand ShowNoInternetCommand { get; }
        public bool IsBusy => games.IsBusy;
        public bool IsConnected => _connectivity.IsConnected;
        public ObservableCollection<Game>? GamesList => games.GamesList;
        public Game? SelectedItem
        {
            get => games.CurrentGame;
            set
            {
                if (value != null)
                {
                    games.CurrentGame = value;
                    value.JoinGame();
                    MainThread.InvokeOnMainThreadAsync( () =>
                    {
                        Shell.Current.Navigation.PushAsync(new GamePage(value));
                    });
                }
            }
        }
        /// <summary>
        /// constructor for the MainPageVM class, which initializes the commands and subscribes to the 
        /// necessary events to manage the games list and connectivity status. The constructor initializes
        /// the AddGameCommand, which is executed when the user clicks the button to add a new game. It also
        /// subscribes to the OnGame Added and OnGamesChanged events of the Games class to update the UI 
        /// when a new game is added or when the games list changes. Additionally, it subscribes to the 
        /// ConnectivityChanged event of the Connectivity class to update the connectivity status and show
        /// an alert if there is no internet connection.
        /// </summary>
        public MainPageVM()
        {
            AddGameCommand = new Command(AddGame);
            games.OnGameAdded += OnGameAdded;
            games.OnGamesChanged += OnGamesChanged;
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        /// <summary>
        /// method that is called when the user clicks the button to add a new game. This method calls the
        /// AddGame method of the Games class to create a new game in the database. The AddGame method sets
        /// the IsBusy property to true while the game is being created, and it calls the OnComplete method 
        /// when the task is completed to handle the result of the operation. The OnGameAdded event is 
        /// raised when a new game is successfully added to the database, which triggers the OnGameAdded 
        /// method in the MainPageVM class to update the UI and navigate to the GamePage for the newly 
        /// created game.
        /// </summary>
        private void AddGame()
        {
            games.AddGame();
            OnPropertyChanged(nameof(IsBusy));
        }
        /// <summary>
        /// method that is called when the games list changes, which updates the GamesList property and 
        /// raises the OnPropertyChanged event to notify the UI of the changes. This method is subscribed
        /// to the OnGamesChanged event of the Games class, which is raised whenever there is a change in
        /// the games collection in the database, such as when a new game is added or when the games list
        /// is updated. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGamesChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(GamesList));
            OnPropertyChanged(nameof(IsBusy));
        }
        /// <summary>
        /// method that is called when a new game is added to the database, which updates the IsBusy
        /// property and navigates to the GamePage for the newly created game. This method is subscribed to 
        /// the OnGame Added event of the Games class, which is raised when a new game is successfully 
        /// added to the database. The method updates the IsBusy property to false to indicate that the 
        /// game creation process is complete, and it uses the MainThread.BeginInvokeOnMainThread method to
        /// navigate to the GamePage for the newly created game on the main thread, ensuring that the UI
        /// updates correctly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="game"></param>
        private void OnGameAdded(object? sender, Game game)
        {
            OnPropertyChanged(nameof(IsBusy));
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Shell.Current.Navigation.PushAsync(new GamePage(game));
            });
        }
        /// <summary>
        /// method that is called when the connectivity status changes, which updates the IsConnected 
        /// property and shows an alert if there is no internet connection. This method is subscribed to 
        /// the ConnectivityChanged event of the Connectivity class, which is raised whenever there is a 
        /// change in the connectivity status, such as when the device connects or disconnects from the 
        /// internet. The method updates the IsConnected property to reflect the current connectivity 
        /// status, and if the device is not connected to the internet and an alert is not already shown,
        /// it executes the ShowNoInternetCommand to display an alert to the user, informing them of the 
        /// lack of internet connection and prompting them to check their connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object? sender, EventArgs e)
        {
            OnPropertyChanged(nameof(IsConnected));
            if (!IsConnected && !isAlertShown)
                ShowNoInternetCommand.Execute(null);
        }
        /// <summary>
        /// method that displays an alert to the user when there is no internet connection. This method is 
        /// executed when the connectivity status changes and the device is not connected to the internet. 
        /// The method sets the isAlertShown flag to true to prevent multiple alerts from being shown 
        /// simultaneously, and it uses the Shell class to display an alert with a title, message, and an 
        /// "OK" button. 
        /// </summary>
        /// <returns></returns>
        private async Task ShowNoInternet()
        {
            isAlertShown = true;
            await Shell.Current.DisplayAlert(Strings.NoInternet, Strings.CheckConnection, Strings.Ok);
            isAlertShown = false;
        }
        /// <summary>
        /// method to add a snapshot listener to the games collection in the database. This method is called
        /// when the MainPageVM is initialized, and it allows the application to listen for changes in the
        /// games collection in real-time. When a change is detected in the games collection, such as when 
        /// a new game is added or when the games list is updated, the OnChange method of the Games class 
        /// is called to update the GamesList property and notify the UI of the changes.
        /// </summary>
        public void AddSnapshotListener()
        {
            games.AddSnapshotListener();
        }
        /// <summary>
        /// method to remove the snapshot listener from the games collection in the database. This method 
        /// is called when the MainPageVM is disposed or when the user navigates away from the main page, 
        /// and it stops listening for changes in the games collection to prevent memory leaks and 
        /// unnecessary updates to the UI.
        /// </summary>
        public void RemoveSnapshotListener()
        {
            games.RemoveSnapshotListener();
        }
    }
}
