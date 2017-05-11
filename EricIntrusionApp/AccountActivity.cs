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
using Android.Util;
using EricIntrusionApp;

namespace EricIntrusionApp
{
	[Activity (Label = "Account", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class AccountActivity : Activity
	{
		private DataAccessObject DAO = new DataAccessObject ();
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SetupTwo);
			FindViewById<TextView> (Resource.Id.PasswordText).Enabled = false;
			FindViewById<TextView>(Resource.Id.NameText).Text = DAO.getDetails(0);
			FindViewById<TextView>(Resource.Id.EmailText).Text = DAO.getDetails(1);
			FindViewById<TextView>(Resource.Id.PasswordText).Text = DAO.getDetails(2);

			var updateDetails = FindViewById<Button>(Resource.Id.updateAccountDetails);
			updateDetails.Click += (sender, e) => { 
				          
				if (FindViewById<TextView>(Resource.Id.NameText).Text.Count<Char>() > 1)
				{
					if (FindViewById<TextView>(Resource.Id.EmailText).Text.Count<Char>() > 7)
					{
						if (FindViewById<TextView>(Resource.Id.EmailText).Text.Contains("@") && FindViewById<TextView>(Resource.Id.EmailText).Text.Contains("."))
						{
							if(FindViewById<TextView>(Resource.Id.NameText).Text == DAO.getDetails(0) && FindViewById<TextView>(Resource.Id.EmailText).Text == DAO.getDetails(1))
							{
								Toast.MakeText(this,"No Change Made!!", ToastLength.Short).Show();
							}
							else
							{
								DAO.updateUserDetails(FindViewById<TextView>(Resource.Id.NameText).Text,
									FindViewById<TextView>(Resource.Id.EmailText).Text,
									FindViewById<TextView>(Resource.Id.PasswordText).Text);

								Toast.MakeText(this,"Account Details Saved!!", ToastLength.Short).Show();
							}
						}
						else Toast.MakeText(this,"Wrong Email Format!!", ToastLength.Short).Show();

					}
					else
					{
						Toast.MakeText(this,"Enter Email please!!", ToastLength.Short).Show();
					}
				}
				else
				{
					Toast.MakeText(this,"Enter Your Name Please!!", ToastLength.Short).Show();
				} 
					    
			};

		}

		public override bool OnCreateOptionsMenu(IMenu myMenu)
		{
			var options1 = myMenu.Add (0, 0, 1, Resource.String.ViewOptions);
			var options3 = myMenu.Add (1, 2, 1, Resource.String.Settings);
			var options4 = myMenu.Add (1, 3, 2, Resource.String.History);
			var options5 = myMenu.Add (1, 4, 3, Resource.String.Lock);
			options1.SetIcon (Resource.Drawable.home);
			options1.SetShowAsAction (ShowAsAction.IfRoom);
			options3.SetShowAsAction (ShowAsAction.CollapseActionView);
			options4.SetShowAsAction (ShowAsAction.CollapseActionView);
			options5.SetShowAsAction (ShowAsAction.CollapseActionView);

			return true;
		}

		public override bool OnOptionsItemSelected(IMenuItem item)
		{
			switch (item.ItemId)
			{
				case 0: StartActivity (typeof(MainActivity));
				return true;

				case 2: StartActivity (typeof(SettingsActivity));
				return true;

				case 3: StartActivity (typeof(HistoryActivity));
				return true;

			    case 4: StartActivity (typeof(ProtectionActivity));
				return true;

				default:
				return base.OnOptionsItemSelected(item);
			}

		}
	}
}

