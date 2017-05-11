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
using EricIntrusionApp;

namespace EricIntrusionApp
{
	[Activity (Label = "IntrusionReport")]			
	public class IntrusionReport : Activity
	{
		private DataAccessObject DAO;
		private int id;
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.PictureScreen);

			Bundle extra = this.Intent.Extras;
			id = extra.GetInt("id") + 1;

			DAO = new DataAccessObject ();
			Bitmap bmp;

			BitmapFactory.Options bmpOptions = new BitmapFactory.Options {InJustDecodeBounds = true};
			bmpOptions.InSampleSize = 4;

			bmpOptions.InJustDecodeBounds = false;

			//decode the data obtained by the camera into a Bitmap
			bmp = BitmapFactory.DecodeByteArray(DAO.getHistoryPicture(id).image, 0, DAO.getHistoryPicture(id).image.Length, bmpOptions);

			int width= bmp.Width;
			int height = bmp.Height;
			Matrix matrix = new Matrix ();

			matrix.PostRotate (270);

			Bitmap resized = Bitmap.CreateBitmap (bmp, 0, 0, width, height, matrix, false);

			FindViewById<ImageView> (Resource.Id.IntruderPictureView).SetImageBitmap(resized);

			FindViewById<TextView> (Resource.Id.PictureScreenTextView).Text = "The person in the above picture tried to access your device without your permission" +
			                                                                  " on the " + DAO.getHistoryPicture (id).date + ".";

			// Create your application here
		}
	}
}

