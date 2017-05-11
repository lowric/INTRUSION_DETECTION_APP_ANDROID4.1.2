using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;
using Android.Graphics;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace EricIntrusionApp
{
	[Table("HistoryItems")]
	class HistoryItems
	{
		[PrimaryKey, AutoIncrement]
		public int Id 
		{
			get;
			set;
		}

		public string date
		{
			get;
			set;
		}

		public string description
		{
			get;
			set;
		}
		

		public byte[] image
		{
			get;
			set;
		}
	}
}

