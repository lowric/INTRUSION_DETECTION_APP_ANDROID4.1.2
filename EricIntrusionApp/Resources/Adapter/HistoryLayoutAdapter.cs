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
using Android.Graphics;

namespace EricIntrusionApp
{
	class HistoryLayoutAdapter:BaseAdapter<HistoryItems>
	{
		List<HistoryItems> items;
		Activity context;

		public HistoryLayoutAdapter(Activity context, List<HistoryItems> items): base()
		{
			this.items = items;
			this.context = context;
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
			Bitmap bmp;
			BitmapFactory.Options bmpOptions = new BitmapFactory.Options {InJustDecodeBounds = true};
			bmpOptions.InSampleSize = 4;

			bmpOptions.InJustDecodeBounds = false;

			//decode the data obtained by the camera into a Bitmap
			bmp = BitmapFactory.DecodeByteArray(item.image, 0, item.image.Length, bmpOptions);

			int width= bmp.Width;
			int height = bmp.Height;
			Matrix matrix = new Matrix ();

			matrix.PostRotate (270);

			Bitmap resized = Bitmap.CreateBitmap (bmp, 0, 0, width, height, matrix, false);

			if (view == null) view = context.LayoutInflater.Inflate (Resource.Layout.HistoryLayout,null);

			view.FindViewById<TextView> (Resource.Id.dateTextView).Text = item.date;
			view.FindViewById<TextView> (Resource.Id.reportTextView).Text = item.description;
			view.FindViewById<ImageView> (Resource.Id.intruderImageView).SetImageBitmap (resized);

			return view;
		}

		public override int Count {
			get {return items.Count;}
		}

		#endregion

		#region implemented abstract members of BaseAdapter

		public override HistoryItems this [int index] {
			get {return items [index];}
		}

		#endregion

	}
}

