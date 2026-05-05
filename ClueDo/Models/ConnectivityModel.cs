namespace ClueDo.Models
{
    /// <summary>
    /// class that represents the connectivity status of the app. It has a boolean property IsConnected
    /// that indicates whether the app is currently connected to the internet or not. It also has an event
    /// ConnectivityChanged that is triggered whenever the connectivity status changes. This class can be used
    /// to monitor the connectivity status of the app and update the UI accordingly.
    /// </summary>
    public class ConnectivityModel
    {
        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            protected set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    ConnectivityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public EventHandler? ConnectivityChanged { get; set; }
    }
}
