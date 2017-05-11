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
using SQLite;

namespace EricIntrusionApp
{
	[Table("SettingsItem")]
	class SettingsItems
	{
	
		[PrimaryKey]
		public int Id {get; set;}

		public string SettingName{get; set;}

		public string SettingDescription {get; set;}

		public bool checkBox {get; set;}
	}
}

