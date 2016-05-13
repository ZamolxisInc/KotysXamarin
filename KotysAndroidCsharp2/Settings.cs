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

namespace KotysAndroidCsharp2
{
    [Table("Settings")]
    public class Settings
    {

         [PrimaryKey, AutoIncrement]
         public int ID { get; set; }

         public string identificator { get; set; }

         public string devID { get; set; }

         public string active { get; set; }

         public override string ToString()
             {
             return string.Format("[Settings: ID={0}, identificator={1}, devID={2}, active={3}]", ID ,identificator, devID, active);
            }
    }

}
