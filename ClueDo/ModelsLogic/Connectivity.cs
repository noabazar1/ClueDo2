using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    /// <summary>
    /// class that holds the data of the connectivity to the internet, and the method to listen to changes
    /// in the connectivity. 
    /// </summary>
    public class Connectivity : ConnectivityModel
    {
        /// <summary>
        /// constructor for the Connectivity class, which initializes the properties of the connectivity,
        /// such as the current connectivity status, and subscribes to the ConnectivityChanged event to 
        /// listen to changes in the connectivity.
        /// </summary>
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }
        /// <summary>
        /// method that is called when the connectivity changes, which updates the IsConnected property 
        /// based on the current connectivity status. This method is subscribed to the ConnectivityChanged 
        /// event in the constructor, and it is called whenever the connectivity changes, such as when the
        /// device connects or disconnects from the internet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
