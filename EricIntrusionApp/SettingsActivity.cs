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
	[Activity (Label = "Settings")]			
	public class SettingsActivity : Activity
	{
		List<SettingsItems> settingsItems = new DataAccessObject().getAllSettings();

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Settings);

			ListView settingsView = FindViewById<ListView> (Resource.Id.settingslistView);
			settingsView.Adapter = new SettingsLayoutAdapter (this,settingsItems);

//			settingsView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => //OnListItemClick
//			{
//				e.Position;
//
//			};

		}

		public override bool OnCreateOptionsMenu(IMenu myMenu)
		{
			var options1 = myMenu.Add (0, 0, 1, Resource.String.ViewOptions);
			var options2 = myMenu.Add (1, 1, 1, Resource.String.Account);
			var options4 = myMenu.Add (1, 3, 2, Resource.String.History);
			var options5 = myMenu.Add (1, 4, 3, Resource.String.Lock);
			options1.SetIcon (Resource.Drawable.home);
			options1.SetShowAsAction (ShowAsAction.IfRoom);
			options2.SetShowAsAction (ShowAsAction.CollapseActionView);
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

				case 1: StartActivity (typeof(AccountActivity));
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

