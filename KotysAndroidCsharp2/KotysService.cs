using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KotysAndroidCsharp2
{
    [Service]
    public class KotysService : Service
    {
        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            var t = new Thread(() => {
                Log.Debug("DemoService", "");
                Thread.Sleep(5000);
                Log.Debug("DemoService", "");
                StopSelf();
            }
            );
            t.Start();
            return StartCommandResult.Sticky;
        }
}