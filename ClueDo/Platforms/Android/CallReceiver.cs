using Android.App;
using Android.Content;
using Android.Telephony;
using ClueDo.Models;
using CommunityToolkit.Mvvm.Messaging;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { TelephonyManager.ActionPhoneStateChanged })]
public class CallReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        var state = intent?.GetStringExtra(TelephonyManager.ExtraState);

        if (state == TelephonyManager.ExtraStateRinging)
        {
            System.Console.WriteLine("### RINGING DETECTED ###");

            WeakReferenceMessenger.Default.Send(
                new AppMessage<bool>(true));
        }
    }
}