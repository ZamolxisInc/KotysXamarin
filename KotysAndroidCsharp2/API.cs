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
using System.Globalization;
using SQLite;

namespace KotysAndroidCsharp2
{
    class API
    {
        public string devID="";
        public string APIurl = "http://192.168.1.3/machines/api/";

        public int timer = 5000;

        public API()
        {

            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Settings>();
            List<Settings> myId = db.Query<Settings>("select devID from Settings where identificator = '1607'");

            string gotID = "";
            foreach (Settings s in myId)
            {
                gotID = s.devID.ToString();
            }
            //devID = myId.Last().devID.ToString();
            devID = gotID;
        }

        public bool LogIn(string user, string password)
        {
            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/login.php?t1=" + user + "&t2=" + password);

            if (int.Parse(messageApi) > 0)
            {
                addReport(user + ":" + password + " ~ Logged in");
                return true;
            }
            else
            {
                addReport(user + ":" + password + " ~ Failed to log in");
                return false;
            }
            
        }

        public bool UpdateDevId(string oldID, string newId)
        {
            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/updateDeviceID.php?t1=" + oldID + "&t2=" + newId);

            if (int.Parse(messageApi) == 406)
            {
                addReport("devID updated from" + oldID + " to " + newId);
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
                addReport("New device registered:t1=" + id + "&t2=" + name + "&t3=" + active + "&t4=" + ip + "&t5=" + lastseen + "&t6=" + type + "&t7=" + username);
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

        public bool addReport(string info)
        {
            DateTime now = DateTime.Now;
            
            string d = now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            string t = now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            WebClient wc = new WebClient();
            string messageApi = wc.DownloadString(APIurl + "/addReport.php?t1=" + devID + "&t2=" + info + "&t3=" + d + "&t4=" + t);

            if (int.Parse(messageApi) == 406)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}