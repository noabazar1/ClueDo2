using Android.App;
using Android.Content;
using Android.Telephony;
using ClueDo.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.Platforms.Android
{
    [BroadcastReceiver(Enabled = true, Exported = true)]
    [IntentFilter([TelephonyManager.ActionPhoneStateChanged])]
    public class CallReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context? context, Intent? intent)
        {
            var state = intent?.GetStringExtra(TelephonyManager.ExtraState);

            if (state == TelephonyManager.ExtraStateRinging)
            {
                WeakReferenceMessenger.Default.Send(
                    new AppMessage<bool>(true));
            }
        }
    }
}