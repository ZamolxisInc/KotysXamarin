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
    [Activity(Label = "RegDevActivity")]
    public class RegDevActivity : Activity
    {
        string dbPathG = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"settingsk.db3");

        public LoginActivity RefToLogIn { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RegDev);

            Button buttonRegister = FindViewById<Button>(Resource.Id.buttonRegister);
            buttonRegister.Click += (sender, e) =>
            {
                string id = FindViewById<EditText>(Resource.Id.editTextID).Text;
                string name = FindViewById<EditText>(Resource.Id.editTextNAME).Text;
                string username = FindViewById<EditText>(Resource.Id.editTextUSERNAME).Text;


                API apicall = new API();
                string myIP = apicall.GetIp();
                DateTime date = DateTime.Now;
                if (apicall.RegisterNewDevice(id, name, "1", myIP, date.ToString(), "Mobile-Android", username) == true)
                {
                    Settings set = new Settings();
                    set.active = "1";
                    set.devID = id;
                    set.identificator = "1607";
                    DbCreator(dbPathG, set);
                    Toast.MakeText(this, "Device was succesfully registered!", ToastLength.Long).Show();
                 
                    Intent myIntent = new Intent(this, typeof(LoginActivity));
                    myIntent.PutExtra("id", id);
                    SetResult(Result.Ok, myIntent);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "Device was NOT registered!", ToastLength.Long).Show();
                }
            };

        }


        public void DbCreator(string dbPath, Settings data)
        {
            var db = new SQLiteConnection(dbPath);
            db.CreateTable<Settings>();
            db.Insert(data); // after creating the newStock object
        }
        
    
    }
}