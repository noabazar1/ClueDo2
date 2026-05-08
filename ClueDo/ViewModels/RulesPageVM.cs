using ClueDo.Models;
using System.Windows.Input;

namespace ClueDo.ViewModels
{
    /// <summary>
    /// class that manages the data and logic for the rules page of the application, allowing users to see the
    /// game rules and instructions.
    /// </summary>
    public partial class RulesPageVM : ObservableObject
    {
        private readonly ModelsLogic.Connectivity _connectivity = new();
        private bool isAlertShown = false;
        public ICommand ShowNoInternetCommand { get; }
        public bool IsConnected => _connectivity.IsConnected;
        /// <summary>
        /// constructor for the RulesPageVM class, which initializes the commands and subscribes to the 
        /// necessary events to manage the connectivity status. The constructor subscribes to the 
        /// ConnectivityChanged event of the Connectivity class to update the IsConnected property and show
        /// an alert if there is no internet connection.
        /// </summary>
        public RulesPageVM()
        {
            _connectivity.ConnectivityChanged += OnConnectivityChanged;
            ShowNoInternetCommand = new Command(async () => await ShowNoInternet());
        }
        /// <summary>
        /// method that is called when the connectivity changes, which updates the IsConnected property and 
        /// shows an alert if there is no internet connection. This method is subscribed to the 
        /// ConnectivityChanged event in the constructor, and it is called whenever the connectivity 
        /// changes, such as when the device connects or disconnects from the internet. If there is no 
        /// internet connection and an alert is not already shown, it executes the ShowNoInternetCommand to
        /// display an alert to the user about the lack of internet connectivity.
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
    }
}
