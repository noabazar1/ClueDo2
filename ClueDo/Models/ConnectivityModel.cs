namespace ClueDo.Models
{
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
