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

namespace EricIntrusionApp
{
	[Activity (Label = "History")]			
	public class HistoryActivity : Activity
	{
		List<HistoryItems> historyItems = new DataAccessObject ().getAllHistory();

		private static int count = 0;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			count++;

			SetContentView (Resource.Layout.History);

			//historyItems.Add (new HistoryItems (count, DateTime.Now.ToString (), "Intrusion Report", this.BaseContext));

			ListView historyView = FindViewById<ListView> (Resource.Id.historyListView);
			historyView.Adapter = new HistoryLayoutAdapter (this,historyItems);

			historyView.ItemClick += OnListItemClick;

		}

		protected void OnListItemClick(Object sender, Android.Widget.AdapterView.ItemClickEventArgs e)
		{
//			var listView = sender as ListView;
			var historyItem = historyItems[e.Position];
			Intent i = new Intent(this,typeof(IntrusionReport));
			i.PutExtra ("id", e.Position);
			StartActivity (i);
		}

		public override bool OnCreateOptionsMenu(IMenu myMenu)
		{
			var options1 = myMenu.Add (0, 0, 1, Resource.String.ViewOptions);
			var options2 = myMenu.Add (1, 1, 1, Resource.String.Account);
			var options3 = myMenu.Add (1, 2, 1, Resource.String.Settings);
			var options5 = myMenu.Add (1, 3, 3, Resource.String.Lock);
			options1.SetIcon (Resource.Drawable.home);
			options1.SetShowAsAction (ShowAsAction.IfRoom);
			options2.SetShowAsAction (ShowAsAction.CollapseActionView);
			options3.SetShowAsAction (ShowAsAction.CollapseActionView);
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

				case 2: StartActivity (typeof(SettingsActivity));
				return true;

			    case 3: StartActivity (typeof(ProtectionActivity));
				return true;

				default:
				return base.OnOptionsItemSelected(item);
			}

		}

	}


}

