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
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : Activity
    {
        public string devId = "";
        

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

                if (devId != textDevID.Text)
                {
                    if(apicall.UpdateDevId(devId, textDevID.Text)== true)
                    {
                        Toast.MakeText(this, "The deviceId has been updated", ToastLength.Long).Show();
                        devId = textDevID.Text;
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

           
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                
                devId = data.GetStringExtra("id");
                EditText textDev = FindViewById<EditText>(Resource.Id.editTextDevice);
                textDev.Text = devId;
                Button buttonToReg = FindViewById<Button>(Resource.Id.buttonToReg);
                buttonToReg.Activated = false;
            }
        }
       
    }
}