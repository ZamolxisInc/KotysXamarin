using System;
using System.Net;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace KotysAndroidCsharp2
{
    [Activity(Label = "KotysClient", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public bool loggedIn = false;
        public bool active = true;
        public string devID = "devm02";
        public int timer = 5000;
        API apiCall = new API();


        public string APIurl = "http://192.168.1.2/machines/api/";
 


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            
            string response = "Default";

            button.Click += delegate 
            {
                string uss = FindViewById<EditText>(Resource.Id.editText1).Text;
                string psw = FindViewById<EditText>(Resource.Id.editText2).Text;


                if (apiCall.LogIn(uss,psw) == true)
                {
                    response = "Logged in";
                    loggedIn = true;
                    Toast.MakeText(this, response, ToastLength.Long).Show();
                    FindViewById<EditText>(Resource.Id.editText1).Text = FindViewById<EditText>(Resource.Id.editText2).Text = "";
                 
                    StartActivity(typeof(LoginActivity));
                    
                }
                else
                {
                    response = "Incorrect psw/uss";
                    loggedIn = false;
                    Toast.MakeText(this, response, ToastLength.Long).Show();
                }
                
            };

           
           

        }

       


    }
}

