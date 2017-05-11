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

namespace EricIntrusionApp
{
	[Activity (Label = "Intrusion Detector", MainLauncher = true)]			
	public class StartupActivity : Activity
	{
		private DataAccessObject DAO;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			SetContentView (Resource.Layout.StartupActivity);
			DAO = new DataAccessObject ();

			if (DAO.getDetails (0) == null) {
				StartActivity (typeof(SetupActivityOne));
			} else {
				StartActivity (typeof(MainActivity));
			}
			// Create your application here


		}
	}
}

