//using System;
//using System.Collections.Generic;
//using Mono.Data.Sqlite;
using SQLite;
//using System.Text;
//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;


namespace EricIntrusionApp
{
	[Table("Owner") ]
	public class User
	{

		public string userName {get; set;}
		[PrimaryKey]
		public string userEmail {get;set;}

		public string password { get; set; }				
	}
}

