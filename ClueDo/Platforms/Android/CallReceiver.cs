using Android.App;
using Android.Content;
using Android.Telephony;
using ClueDo.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.Platforms.Android
{
    /// <summary>
    /// class that listens for incoming phone calls and sends a message to the application when a call is
    /// received. The CallReceiver class inherits from the BroadcastReceiver class, which allows it to 
    /// receive broadcast intents from the Android system. The class is decorated with the BroadcastReceiver
    /// attribute to specify that it is a broadcast receiver, and the IntentFilter attribute to specify
    /// that it should listen for the ActionPhoneStateChanged intent, which is broadcasted when the phone
    /// state changes (e.g., when a call is received).
    /// </summary>
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter([TelephonyManager.ActionPhoneStateChanged])]
    public class CallReceiver : BroadcastReceiver
    {
        /// <summary>
        /// method that is called when the CallReceiver receives a broadcast intent. It checks the state of 
        /// the phone call by retrieving the ExtraState extra from the intent. If the state is
        /// ExtraStateRinging (which indicates that a call is incoming), it sends a message using the 
        /// WeakReferenceMessenger to notify the application that a call has been received. The message 
        /// sent is an instance of AppMessage with the value set to true, indicating that a call is 
        /// currently ringing.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="intent"></param>
        public override void OnReceive(Context? context, Intent? intent)
        {
            string? state = intent?.GetStringExtra(TelephonyManager.ExtraState);

            if (state == TelephonyManager.ExtraStateRinging)
            {
                WeakReferenceMessenger.Default.Send(
                    new AppMessage<bool>(true));
            }
        }
    }
}