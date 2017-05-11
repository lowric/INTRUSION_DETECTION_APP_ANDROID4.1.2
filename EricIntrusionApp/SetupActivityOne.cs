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
	[Activity (Label = "Registration")]			
	public class SetupActivityOne : Activity
	{
		private DataAccessObject DAO;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SetupOne);

			DAO = new DataAccessObject ();

			var saveDetails = FindViewById<Button>(Resource.Id.saveDetailsButton);
			saveDetails.Click += (sender, e) => {  

				Log.Debug("SetUp",FindViewById<TextView>(Resource.Id.nameEditText).Text+" text");         
			
				//VALIDATION ON THE FIELDS
				if(FindViewById<TextView>(Resource.Id.nameEditText).Text != null && FindViewById<TextView>(Resource.Id.nameEditText).Text.Count<Char>() > 1)
				{
					if(FindViewById<TextView>(Resource.Id.emailEditText).Text != null && FindViewById<TextView>(Resource.Id.emailEditText).Text.Count<Char>() > 7)
					{
						if(FindViewById<TextView>(Resource.Id.emailEditText).Text.Contains("@") && FindViewById<TextView>(Resource.Id.emailEditText).Text.Contains(".")) 
						{
							if(FindViewById<TextView>(Resource.Id.remailEditText).Text == FindViewById<TextView>(Resource.Id.emailEditText).Text)
							{
								if(FindViewById<TextView>(Resource.Id.passwordEditText).Text != null)
								{
									if(FindViewById<TextView>(Resource.Id.rpasswordEditText).Text == FindViewById<TextView>(Resource.Id.passwordEditText).Text)
									{
										DAO.insertUserDetails(FindViewById<TextView>(Resource.Id.nameEditText).Text,FindViewById<TextView>(Resource.Id.remailEditText).Text,FindViewById<TextView>(Resource.Id.passwordEditText).Text);
										StartActivity (typeof(MainActivity));
									}
									else{ Toast.MakeText(this,"Passwords do not match!!", ToastLength.Short).Show();}

								}else{Toast.MakeText(this,"Please complete Password!!", ToastLength.Short).Show(); }

							}else{Toast.MakeText(this,"Emails do not match!!", ToastLength.Short).Show(); }

						}else{Toast.MakeText(this,"Wrong Email Format!!", ToastLength.Short).Show(); }

					}else{Toast.MakeText(this,"Please complete Email!!", ToastLength.Short).Show(); }

				}else{Toast.MakeText(this,"Please complete name field!!", ToastLength.Short).Show(); }

			};

		}

		// opens the device home screen when the back button is pressed from this activity
		public override void OnBackPressed()
		{
			Intent intent = new Intent();
			intent.SetAction(Intent.ActionMain);
			intent.AddCategory(Intent.CategoryHome);

			StartActivity(intent);
		}
	}
}

