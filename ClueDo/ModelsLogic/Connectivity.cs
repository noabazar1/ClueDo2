using ClueDo.Models;

namespace ClueDo.ModelsLogic
{
    public class Connectivity : ConnectivityModel
    {
        public Connectivity()
        {
            Microsoft.Maui.Networking.Connectivity.Current.ConnectivityChanged += OnConnectivityChanged;
            IsConnected = Microsoft.Maui.Networking.Connectivity.Current.NetworkAccess == NetworkAccess.Internet;
        }

        private void OnConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            IsConnected = e.NetworkAccess == NetworkAccess.Internet;
        }
    }
}
