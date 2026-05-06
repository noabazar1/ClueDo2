using Android.OS;
using ClueDo.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.Platforms.Android
{
    /// <summary>
    /// class that extends the CountDownTimer class to create a custom timer for the application. The 
    /// MyTimer class takes two parameters in its constructor: millisInFuture, which specifies the total 
    /// time for the countdown in milliseconds, and countDownInterval, which specifies the interval at 
    /// which the onTick method should be called in milliseconds. The MyTimer class overrides the onFinish
    /// and onTick methods of the CountDownTimer class.
    /// </summary>
    /// <param name="millisInFuture"></param>
    /// <param name="countDownInterval"></param>
    public class MyTimer(long millisInFuture, long countDownInterval) : CountDownTimer(millisInFuture, countDownInterval)
    {
        /// <summary>
        /// method that is called when the countdown timer finishes. It sends a message using the 
        /// WeakReferenceMessenger to notify the application that the timer has finished. The message sent
        /// is an instance of AppMessage with the value set to the FinishedSignal constant, which can be 
        /// used by the application to identify that the timer has completed its countdown. This allows the
        /// application to perform any necessary actions when the timer finishes, such as updating the UI 
        /// or triggering other events.
        /// </summary>
        public override void OnFinish()
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(Keys.FinishedSignal));
        }
        /// <summary>
        /// method that is called at regular intervals defined by the countDownInterval parameter. It sends
        /// a message using the WeakReferenceMessenger to notify the application of the remaining time in 
        /// milliseconds until the timer finishes. The message sent is an instance of AppMessage with the 
        /// value set to the millisUntilFinished parameter, which represents the remaining time in 
        /// milliseconds. This allows the application to update the UI or perform other actions based on 
        /// the remaining time of the countdown timer, such as displaying a countdown or triggering events
        /// at specific intervals.
        /// </summary>
        /// <param name="millisUntilFinished"></param>
        public override void OnTick(long millisUntilFinished)
        {
            WeakReferenceMessenger.Default.Send(new AppMessage<long>(millisUntilFinished));
        }
    }
}
