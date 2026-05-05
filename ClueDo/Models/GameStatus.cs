namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the status of the game, which can be either "wait" or "play". The status is used
    /// to determine the state of the game, and to display the appropriate message to the players. The
    /// status is stored as an enum, and the corresponding messages are stored in an array. The 
    /// StatusMessage property returns the message corresponding to the current status, which can be used
    /// in the UI to display the status of the game to the players.
    /// </summary>
    public class GameStatus
    {
        private readonly string[] msgs = [Strings.WaitMessage, Strings.PlayMessage];
        public enum Status { Wait, Play }
        public Status CurrentStatus { get; set; } = Status.Wait;
        public string StatusMessage => msgs[(int)CurrentStatus];
    }
}