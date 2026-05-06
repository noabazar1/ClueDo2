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