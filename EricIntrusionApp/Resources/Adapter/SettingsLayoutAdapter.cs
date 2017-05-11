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
	 class SettingsLayoutAdapter: BaseAdapter<SettingsItems>, CompoundButton.IOnCheckedChangeListener
	{
		List<SettingsItems> items;
		Activity context;
		int[] myPosition;
		DataAccessObject DAO;

	     public SettingsLayoutAdapter(Activity context, List<SettingsItems> items): base()
		{
			this.items = items;
			this.context = context;
			DAO = new DataAccessObject();
			myPosition = new int[4];
		}

		#region implemented abstract members of BaseAdapter

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var item = items [position];
			View view = convertView;

			if (view == null) view = context.LayoutInflater.Inflate (Resource.Layout.Settingslayout,null);

			view.FindViewById<TextView> (Resource.Id.settingNameTextView).Text = item.SettingName;
			view.FindViewById<TextView> (Resource.Id.settingDescriptionTextView).Text = item.SettingDescription;
			myPosition[position] = position + 1;

			Log.Debug ("Setting", item.SettingName+"" + position);
			//int i = position++;
			//if (myPosition [position] == 0)myPosition [position]++; 
			if (DAO.getSettingStatus (myPosition[position]))
				view.FindViewById<CheckBox> (Resource.Id.checkBox).Checked = true;
			else
				view.FindViewById<CheckBox> (Resource.Id.checkBox).Checked = false;

		    view.FindViewById<CheckBox> (Resource.Id.checkBox).SetOnCheckedChangeListener(this);
			view.FindViewById<CheckBox> (Resource.Id.checkBox).Id = position;

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override SettingsItems this [int index] {
			get {return items [index];}
		}

		#endregion

		#region IOnCheckedChangeListener implementation

		public void OnCheckedChanged (CompoundButton buttonView, bool isChecked)
		{
			//buttonView.Id;
			int index = myPosition [buttonView.Id];
			//if (index == 0)index++;
			DAO.updateSettings (index, isChecked);
			//throw new NotImplementedException ();
		}

		#endregion


	}
}

