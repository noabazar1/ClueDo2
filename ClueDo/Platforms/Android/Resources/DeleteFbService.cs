using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using ClueDo.Models;

namespace ClueDo.Platforms.Android.Resources
{
    public class DeleteFbService : Service
    {
        private bool isRunning = true;
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent? intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            ThreadStart threadStart = new ThreadStart(DeleteFbDocs);
            Thread thread = new Thread(threadStart);
            thread.Start();
            return base.OnStartCommand(intent, flags, startId);
        }

        private void DeleteFbDocs()
        {
            while (isRunning)
            {
                Thread.Sleep(Keys.OneHourInMilliseconds);
            }
            StopSelf();
        }

        public override IBinder? OnBind(Intent? intent)
        {
            return null;
        }
        public override void OnDestroy()
        {
            isRunning = false;
            base.OnDestroy();
        }
    }
}
