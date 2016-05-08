using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace KotysAndroidCsharp2
{
    class API
    {
        public string APIurl = "http://192.168.1.3/machines/api/";

        public int timer = 5000;


        public bool LogIn(string user, string password)
        {
            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/login.php?t1=" + user + "&t2=" + password);

            if (int.Parse(messageApi) > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateDevId(string oldID, string newId)
        {
            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/updateDeviceID.php?t1=" + oldID + "&t2=" + newId);

            if (int.Parse(messageApi) == 406)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool RegisterNewDevice(string id, string name, string active, string ip, string lastseen, string type, string username)
        {
            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/registerDevice.php?t1=" + id + "&t2=" + name + "&t3=" + active + "&t4=" + ip + "&t5=" + lastseen + "&t6=" + type + "&t7=" + username);

            if (int.Parse(messageApi) == 406)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

       public  void SendLastSeen(string devID)
        {
            //var nMgr = (NotificationManager)GetSystemService(NotificationService);
            //var notification = new Notification(Resource.Drawable.Icon, "Message from demo service");
            //var pendingIntent = PendingIntent.GetActivity(this, 0, new Intent(this, typeof(MainActivity)), 0);
            //notification.SetLatestEventInfo(this, "Demo Service Notification", "Message from demo service", pendingIntent);
            //nMgr.Notify(0, notification);

            DateTime now = DateTime.Now;
            WebClient wc = new WebClient();
            wc.DownloadString(APIurl + "updateLastSeen.php?t1=" + devID + "&t2=" + now); // TREBUIE TESTAT
            //TREBUIE INITIALIZAT DEVID in onStartCommand

        }

      public void markItAsDone(string ident)
       {
           // apeleaza api si marchaza ca DONE 

           WebClient wc = new WebClient();
           wc.DownloadString(APIurl + "setCommandAsDone.php?t1=" + ident); // TREBUIE TESTAT
       }

        public string GetIp()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
           
            WebClient wc = new WebClient();
            string ip = wc.DownloadString("https://api.ipify.org");
            
            return ip;
        }

    }
}