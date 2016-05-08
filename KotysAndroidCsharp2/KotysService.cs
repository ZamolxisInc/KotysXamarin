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
            //var t = new Thread(() =>
            //  {
            //      do
            //      {
            //          Thread.Sleep(5000);
            //          SendSastSeen();
            //      } while (1 != 2);
                
               
            //    StopSelf();
            //    }
            // );
           
            
            //start v2

            t2.Interval = 900000; // = 15 minutes in ms
            t2.Elapsed += new System.Timers.ElapsedEventHandler(t2_Elapsed);
            t2.Start();

            t3.Interval = 1000; // = 3 minutes in ms
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
           string[] aux = get.Split('~');
           string command = aux[0];
           string Identifier = aux[1];
           doCommand(command, Identifier);
           
       }

       void doCommand(string command, string Identifier)
       {
           switch (command.Substring(0, 2))
           {
               case "101": if (toastIT(command.Substring(3, command.Length)) == true) { apicall.markItAsDone(Identifier); } break;
               default: break;
           }

       }


       bool toastIT(string message)
       {
           try{
               Toast.MakeText(this, message, ToastLength.Long);
               return true;
           }catch(Exception e)
           {
               return false;
           }
          
       }

     
       

        
    }

}