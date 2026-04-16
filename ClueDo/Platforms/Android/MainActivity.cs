using Android.App;
using Android.Content.PM;
using Android.OS;
using ClueDo.Models;
using CommunityToolkit.Mvvm.Messaging;

namespace ClueDo.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        MyTimer? mTimer;
        protected override async void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            PermissionStatus status = await Permissions.RequestAsync<Permissions.Phone>();

            if (status == PermissionStatus.Granted)
            {
                WeakReferenceMessenger.Default.Register<AppMessage<TimerSettings>>(this, (r, m) =>
                {
                    OnMessageReceived(m.Value);
                });

                WeakReferenceMessenger.Default.Register<AppMessage<bool>>(this, (r, m) =>
                {
                    OnMessageReceived(m.Value);
                });
            }
        }
        private void OnMessageReceived(bool value)
        {
            if (value)
            {
                mTimer?.Cancel();
                mTimer = null;
            }
        }
        private void OnMessageReceived(TimerSettings value)
        {
            mTimer = new MyTimer(value.TotalTimeInMilliseconds, value.IntervalInMilliseconds);
            mTimer.Start();
        }
    }
}
