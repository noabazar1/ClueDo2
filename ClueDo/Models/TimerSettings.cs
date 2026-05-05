namespace ClueDo.Models
{
    /// <summary>
    /// class that holds the settings of the timer, which are the total time of the game and the interval
    /// of the timer. The TimerSettings class is used to configure the timer for the game, and to determine
    /// how long the game will last and how often the timer will update. With primary constructor.
    /// </summary>
    /// <param name="totalTimeInMilliseconds"></param>
    /// <param name="intervalInMilliseconds"></param>
    public class TimerSettings(long totalTimeInMilliseconds, long intervalInMilliseconds)
    {
        public long TotalTimeInMilliseconds { get; set; } = totalTimeInMilliseconds;
        public long IntervalInMilliseconds { get; set; } = intervalInMilliseconds;
    }
}
