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
        public string APIurl = "http://192.168.1.7/machines/api/";

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


        public string GetIp()
        {
            string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "settingsk.db3");
           
            WebClient wc = new WebClient();
            string ip = wc.DownloadString("https://api.ipify.org");
            
            return ip;
        }

    }
}