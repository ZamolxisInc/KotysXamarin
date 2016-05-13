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
using Android.Locations;
using Android.Util;

namespace KotysAndroidCsharp2
{   [Service]
    class GPSService : Android.App.Service, ILocationListener
    {
        LocationManager locMgr;
        string tag = "MainActivity";
        Button button;
        string latitude;
        string longitude;
        string provider;


        public override StartCommandResult OnStartCommand(Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
            
            locMgr = GetSystemService(Context.LocationService) as LocationManager;

            if (locMgr.AllProviders.Contains(LocationManager.NetworkProvider)
                && locMgr.IsProviderEnabled(LocationManager.NetworkProvider))
            {
                locMgr.RequestLocationUpdates(LocationManager.NetworkProvider, 2000, 1, this);
            }
            else
            {
                Toast.MakeText(this, "The Network Provider does not exist or is not enabled!", ToastLength.Long).Show();
            }


            return StartCommandResult.NotSticky;
        }

        public void OnLocationChanged(Android.Locations.Location location)
        {
            Log.Debug(tag, "Location changed");
            latitude = location.Latitude.ToString();
            longitude = location.Longitude.ToString();
            provider = location.Provider.ToString();
            //SendNotification(latitude);
            API apiCall = new API();
            apiCall.addReport("<a href=\"http://www.google.com/maps/place/" + latitude + "," + longitude + "/@" + latitude + "," + longitude + ",17z\">Location (" + latitude + ";" + longitude + ")</a>");
           
        }
        public void OnProviderDisabled(string provider)
        {
            Log.Debug(tag, provider + " disabled by user");
        }
        public void OnProviderEnabled(string provider)
        {
            Log.Debug(tag, provider + " enabled by user");
        }
        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(tag, provider + " availability has changed to " + status.ToString());
        }



        public override void OnDestroy()
        {
            base.OnDestroy();
            // cleanup code
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        private void SendNotification(string message)
        {

            var nMgr = (NotificationManager)this.GetSystemService(NotificationService);
            var notification = new Notification(Resource.Drawable.Icon, "Kotys Message");
            var intent = new Intent();
            intent.SetComponent(new ComponentName(this, "dart.androidapp.ContactsActivity"));
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, 0);
            notification.SetLatestEventInfo(this, message, message, pendingIntent);
            nMgr.Notify(0, notification);
        }


    }
}