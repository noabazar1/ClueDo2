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
            string? state = intent?.GetStringExtra(TelephonyManager.ExtraState);

            if (state == TelephonyManager.ExtraStateRinging)
            {
                string? phone = intent?.Extras?.GetString("incoming_number");

                WeakReferenceMessenger.Default.Send(
                    new AppMessage<string>(phone ?? ""));
            }
        }
    }
}