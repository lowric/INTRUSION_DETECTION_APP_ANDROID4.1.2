using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using EricIntrusionApp;
using Android.Widget;
using Android.Util;

namespace EricIntrusionApp
{
	[Activity (Label = "", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]
	//[IntentFilter (new[]{Intent.ActionMain},Categories = new string[] { Intent.CategoryHome, Intent.CategoryDefault })]	
	[IntentFilter (new[]{Intent.ActionMain},Categories = new string[] {Intent.CategoryHome })]			
	public class ProtectionActivity : Activity
	{
		private DataAccessObject DAO;
		private static int count = 0;
		private static int count2 = 0;
		private System.Threading.Timer timer;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.LockScreen);
			SetPersistent (true);

			DAO = new DataAccessObject ();

			var unlock = FindViewById<ImageView>(Resource.Id.UnnlockImageView);
			unlock.Click += (sender, e) => { 
				          
				if(FindViewById<TextView>(Resource.Id.unlockTextView).Text != DAO.getDetails(2))
				{
					count++;
					count2++;
					if(count ==3)
					{
						StartService(new Intent(this,typeof(ProtectionService)));
						count = 0;
					}

					if(count2 == 4)
					{
						if(new DataAccessObject().getSettingStatus(1))StartService(new Intent(this,typeof(SoundService)));
						count2 = 0;
					}
				}
				else
				{
					if(timer != null)timer.Dispose();
					StopService (new Intent(this,typeof(ProtectionService)));
					StopService (new Intent(this,typeof(SoundService)));
					StartActivity(typeof(MainActivity));
				}

			};

		}

//		protected override void OnUserLeaveHint ()
//		{
//			base.OnUserLeaveHint ();
//			StartActivity(typeof(ProtectionActivity));
//			Log.Debug (""," got through");
//		}

		public override bool OnKeyDown(Keycode code, KeyEvent e)
		{
			if (code == Keycode.Back || code == Keycode.VolumeDown || code == Keycode.Camera || code == Keycode.VolumeUp) 
			{
				Log.Debug ("",""+code);
				StartActivity(typeof(ProtectionActivity));
			}


			if (code == Keycode.Power|| code == Keycode.AppSwitch || code == Keycode.SoftLeft || code == Keycode.SoftRight) 
			{
				StartActivity(typeof(ProtectionActivity));
			}


			return base.OnKeyDown (code, e);
		}
		

		protected override void OnDestroy ()
		{
			if (FindViewById<TextView> (Resource.Id.unlockTextView).Text == DAO.getDetails (2)) 
			{
				StopService (new Intent (this, typeof(ProtectionService)));
				StopService (new Intent (this, typeof(SoundService)));
				timer.Dispose ();
			} else 
			{
				if (timer == null) 
				{
					timer = new System.Threading.Timer ((o) => {
						Log.Debug ("SimpleService", "hello from simple service");
						StartService (new Intent(this,typeof(ProtectionService)));}
						, null, 0, 3600000);
				}
			}

			base.OnDestroy ();
		}

		protected override void OnStop()
		{
			if (FindViewById<TextView> (Resource.Id.unlockTextView).Text == DAO.getDetails (2)) 
			{
				StopService (new Intent(this,typeof(ProtectionService)));
				StopService (new Intent(this,typeof(SoundService)));
				if(timer != null)timer.Dispose ();
			} else 
			{
				if (timer == null) 
				{
					timer = new System.Threading.Timer ((o) => {
						Log.Debug ("SimpleService", "hello from simple service");
						StartService (new Intent(this,typeof(ProtectionService)));}
						, null, 0, 3600000);
				}
			}

			base.OnStop ();
		}

//		protected override void OnPause()
//		{
//			StartActivity (typeof(ProtectionActivity));
//		}
		 
	}
}

