using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;


namespace KotysAndroidCsharp2
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        public string devId = "";
        public bool isRegistered = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create your application here
            SetContentView(Resource.Layout.LoggedIn);

            Button buttonToReg = FindViewById<Button>(Resource.Id.buttonToReg);
            buttonToReg.Click += (sender, e) =>
            {

                var intent = new Intent(this, typeof(RegDevActivity));
                
                StartActivityForResult(intent,0);

            };

            Button button1 = FindViewById<Button>(Resource.Id.button1);
            button1.Click += (sender, e) =>
            {
                Toast.MakeText(this, "Logged out", ToastLength.Long).Show();
               
                Finish();
              
            };

            Button buttonUpdate = FindViewById<Button>(Resource.Id.buttonUpdate);
            buttonUpdate.Click += (sender, e) =>
            {
                API apicall = new API();
                EditText textDevID = FindViewById<EditText>(Resource.Id.editTextDevice);

                string oldDevId = devId;

                if (devId != textDevID.Text)
                {
                    if(apicall.UpdateDevId(devId, textDevID.Text)== true)
                    {
                        Toast.MakeText(this, "The deviceId has been updated", ToastLength.Long).Show();
                        
                        devId = textDevID.Text;

                        //update to SqlLite

                        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
                        var db = new SQLiteConnection(dbPath);
                        db.CreateTable<Settings>();
                        db.Query<Settings>("update Settings SET devID='"+devId+"' WHERE devID='"+oldDevId+"'");
                        isLogged();
                        //

                    }
                    else
                    {
                        Toast.MakeText(this, "The deviceId was NOT updated", ToastLength.Long).Show();
                    }
                }else
                {
                    Toast.MakeText(this, "The deviceID is unchanged!!", ToastLength.Long).Show();
                }
            };

            isLogged();
          
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                
                devId = data.GetStringExtra("id");
                EditText textDev = FindViewById<EditText>(Resource.Id.editTextDevice);
                textDev.Text = devId;

                //Nu merge ->
                Button buttonToReg = FindViewById<Button>(Resource.Id.buttonToReg);
                buttonToReg.Visibility = ViewStates.Invisible;
                //
            }
            isLogged();
        }

     

        protected string isLogged()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Settings>();

            //test
            int nrOfRows = getTableDevRegCount();

            

            //testEnd

            if (nrOfRows > 0)
            {
                isRegistered = true;
                //var myId = from s in db.Table<Settings>()
                //           where s.identificator.Equals("1607")
                //           select s.devID;


                List<Settings> myId = db.Query<Settings>("select devID from Settings where identificator = '1607'");

                string gotID = "";
                foreach(Settings s in myId)
                {
                    gotID = s.devID.ToString();
                }
                gotID = myId.Last().devID.ToString();
              


                Button buttonToReg = FindViewById<Button>(Resource.Id.buttonToReg);
                buttonToReg.Visibility = ViewStates.Invisible;
                TextView textViewRegOrNot = FindViewById<TextView>(Resource.Id.textViewRegOrNot);
                textViewRegOrNot.Text = gotID;
                devId = gotID;
                EditText textDevID = FindViewById<EditText>(Resource.Id.editTextDevice);
                textDevID.Text = devId;
                return gotID;

            }
            else
            {
                isRegistered = false;
                Button buttonToReg = FindViewById<Button>(Resource.Id.buttonToReg);
                buttonToReg.Visibility = ViewStates.Visible;
                TextView textViewRegOrNot = FindViewById<TextView>(Resource.Id.textViewRegOrNot);
                textViewRegOrNot.Text = "This device is not registered";

                return "This device is not registered";
            }
           
        }

        public int getTableDevRegCount()
        {
            int count = 0;
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Settings>();
            var query = db.Table<Settings>().Where(v => v.identificator.StartsWith("1607"));

            foreach (var stock in query)
                count++;
            return count;
        }
       
    }
}