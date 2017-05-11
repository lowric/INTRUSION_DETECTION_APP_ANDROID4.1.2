using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace EricIntrusionApp
{
	[Activity (Label = "Intrusion Detector", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	public class MainActivity : Activity
	{ 

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
	
			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);



			// Get our button from the layout resource,
			// and attach an event to it
			var showSettings = FindViewById<ImageButton>(Resource.Id.settingsImageButton);
			showSettings.Click += (sender, e) => {           

				StartActivity (typeof(SettingsActivity));
			};

			var showAccountDetails = FindViewById<ImageButton>(Resource.Id.userImageButton);
			showAccountDetails.Click += (sender, e) => {           

				StartActivity (typeof(AccountActivity));

			};

			var lockScreen = FindViewById<ImageButton>(Resource.Id.lockImageButton);
			lockScreen.Click += (sender, e) => {           

				StartActivity (typeof(ProtectionActivity));

			};

			var showHistory = FindViewById<ImageButton>(Resource.Id.historyImageButton);
			showHistory.Click += (sender, e) => {           

				StartActivity (typeof(HistoryActivity));

			};


		}

		// opens the device home screen when the back button is pressed from this activity
		public override bool OnKeyDown (Keycode code, KeyEvent e)
		{
			if (code == Keycode.Back) 
			{
				Intent intent = new Intent();
				intent.SetAction (Intent.ActionMain);
				intent.AddCategory(Intent.CategoryHome);

				StartActivity(intent);

			}
			return base.OnKeyDown (code, e);
		}
       
	}

}

