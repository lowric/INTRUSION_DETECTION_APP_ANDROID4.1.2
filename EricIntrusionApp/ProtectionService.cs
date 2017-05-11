using System;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using System.Collections.Generic;
using Android.Util;
using Android.Graphics;
using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Android.Hardware;
using Xamarin.Media;
using Android.Locations;
using Android.Media;
using Xamarin.Geolocation;
//using System.Runtime.Remoting.Contexts;
using Android.Content;
using Android.OS;
using Java.IO;
using System.IO;
using Android.Views;
using Android.Content.PM;
using System.Drawing;

namespace EricIntrusionApp
{
	[Service (Label = "ProtectionService")]			
	public class ProtectionService : Service, ISurfaceHolderCallback, Android.Hardware.Camera.IPictureCallback
	{
		private SmtpClient client;
		private MailMessage mail;
		private Attachment attach;
		private string picturePath = null;
		private Position position;
		private bool flag;
		private Thread emailThread;

		private SurfaceView sv;

		// use to access database
		private DataAccessObject DAO;

		//a bitmap to display the captured image

		//Camera variables
		//a surface holder
		private ISurfaceHolder sHolder;
		//a variable to control the camera
		private Android.Hardware.Camera mCamera;
		//the camera parameters
		private Android.Hardware.Camera.Parameters parameters;

		private FileOutputStream fileWritter; 
		private Java.IO.File afile;


		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);

			Log.Debug ("SimpleService", "SimpleService started");

			flag = false;
			DAO = new DataAccessObject ();

			sv = new SurfaceView (this.BaseContext);

			position = new Position ();

			getLatLong();

			//Get a surface
			sHolder = sv.Holder;

			//add the callback interface methods defined below as the Surface View callbacks
			sHolder.AddCallback(this);

			//cslls the camera
			SurfaceCreated (sHolder);

			//set up the camera and take picture
			SurfaceChanged (sHolder, Android.Graphics.Format.Jpeg, 0, 0);

			emailThread = new Thread (StartProtection);
			emailThread.Start ();

			Log.Debug("",DAO.getDetails(1));
	
		}

		public override void OnDestroy ()
		{
			base.OnDestroy ();

			Log.Debug ("SimpleService", "SimpleService stopped"); 

		}
		
		// gets the location of the device
		public async void getLatLong()
		{
			var locator = new Geolocator(this.BaseContext) { DesiredAccuracy = 50 };

			try
			{
				 await locator.GetPositionAsync(timeout: 60000).ContinueWith(t =>{

					position.Latitude = t.Result.Latitude;
					position.Longitude = t.Result.Longitude;
					Log.Debug ("Test", "latitude: "+ position.Latitude);
					Log.Debug ("Test", "logitude: "+ position.Longitude);
				}, TaskScheduler.FromCurrentSynchronizationContext());

			}
			catch(System.Threading.Tasks.TaskCanceledException g)
			{
				Log.Debug ("ProtectionService","Coordinates not available!!"+ g.Message);
			}

		}

		public void OnPictureTaken(byte[] data, Android.Hardware.Camera camera)
		{
			// Creates a data access object to record the intrusion in the history			
			DAO = new DataAccessObject ();
			DAO.insertHistoryItems (DateTime.Now.ToString (), "Intrusion Detected", data);

			Bitmap bmp;
			BitmapFactory.Options bmpOptions = new BitmapFactory.Options {InJustDecodeBounds = true};
			bmpOptions.InSampleSize = 4;

			bmpOptions.InJustDecodeBounds = false;

			//decode the data obtained by the camera into a Bitmap
			bmp = BitmapFactory.DecodeByteArray(data, 0, data.Length, bmpOptions);

			int width= bmp.Width;
			int height = bmp.Height;
			Matrix matrix = new Matrix ();

			matrix.PostRotate (270);

			Bitmap resized = Bitmap.CreateBitmap (bmp, 0, 0, width, height, matrix, false);

			afile = new Java.IO.File (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "picture.jpeg");
			fileWritter = new FileOutputStream (afile);
			fileWritter.Write (data, 0, data.Length);
		
			attach = new Attachment (afile.Path);

			picturePath = System.IO.Path.Combine (System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal), "picture.jpeg");
		}
		

		public void SurfaceChanged(ISurfaceHolder arg0, Android.Graphics.Format arg1, int arg2, int arg3)
		{
			//get camera parameters
			parameters = mCamera.GetParameters();

			//set camera parameters
			mCamera.SetParameters(parameters);

			//mCamera.SetDisplayOrientation (90);
			mCamera.StartPreview();

			//Take the picture
			mCamera.TakePicture(null, null, this);
			//Log.Debug ("","picture taken!!!");
		}

		public void SurfaceCreated(ISurfaceHolder holder)
		{
			// The Surface has been created, acquire the camera and tell it where
			// to draw the preview.
		    
			mCamera = Android.Hardware.Camera.Open(1);
			Log.Debug ("","camera opened!!!");

			try 
			{
				mCamera.SetPreviewDisplay(holder);

			} catch (Java.IO.IOException exception) {
				mCamera.Release();
				mCamera = null;
			}
		}

		public void SurfaceDestroyed(ISurfaceHolder holder)
		{
			//stop the preview
			mCamera.StopPreview();
			//release the camera
			mCamera.Release();
			//unbind the camera from this object
			mCamera = null;
		}
		
		public  void StartProtection ()
		{
			try 
			{
				//Attachment attach = new Attachment("picture.png");

				mail = new MailMessage ();
				//mail.Attachments.Add(attach);

				string address = DAO.getDetails(1);
				Log.Debug("",address);

				// set up email info
				mail.From = new MailAddress("ericwatat@gmail.com");
				mail.To.Add (new MailAddress(address));
				mail.IsBodyHtml = true;
				mail.DeliveryNotificationOptions = DeliveryNotificationOptions.Never;
				//mail.Body = "<!DOCTYPE html><html>  <head><H1>Just Trying</H1></head>  <body><img src =\"http://maps.googleapis.com/maps/api/staticmap?zoom=13&size=600x300&maptype=roadmap&markers=color:red%7Ccolor:red%7Clabel:C%7C"+position.Latitude+","+position.Longitude+"&sensor=false\" height = \"420\" width = \"620\"></body></html>";

				//create a client account for sendind the email
				client = new SmtpClient ();
				client.UseDefaultCredentials = false;
				client.Credentials = new NetworkCredential("ericwatat","ripapyrus23");
				client.Port = 587;
				client.EnableSsl = true;
				client.Host = "smtp.gmail.com";
				client.Timeout = 60000;

				//authenticate credentials
				ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors Sslpolicy) {return true;};

				while (!flag) // performs until location is available
				{
					if(position.Latitude != 0 && position.Longitude != 0 && picturePath != null)
					{
						// Adds map to the email if Location
						// is activated in the settings
						if(new DataAccessObject().getSettingStatus(4))mail.Body = "<!DOCTYPE html><html>  <head><H1>Intrusion Detected</H1></head>  <body><p>Dear "+DAO.getDetails(0)+",</p><p align = \"justify\"> You received that email because the person in the attached picture\n" +
							                                                      "has attempted to access your device without your permission.</p>\n" +
							                                                      " <p align = \"justify\">The map below pinpoints the location of the unauthorised access\n\n\n." +
							                                                      "</p><p>If you trigger that email and forgot your details, your password is: "+DAO.getDetails(2)+"</p>\n<img src = \""+picturePath+"\"><img src =\"http://maps.googleapis.com/maps/api/staticmap?zoom=13&size=600x300&maptype=roadmap&markers=color:red%7Ccolor:red%7Clabel:C%7C"+position.Latitude+","+position.Longitude+"&sensor=false\" height = \"420\" width = \"620\"><br><p align = \"justify\">If you received this email by error please accept our appology and ignore it.</p></body></html>";


						else mail.Body = "<!DOCTYPE html><html>  <head><H1>Intrusion Detected</H1></head>  <body><p>Dear "+DAO.getDetails(0)+",</p><p align = \"justify\"> You received that email because the person in the attached picture\n" +
							             "has attempted to access your device without your permission.</p><br>" +
							             "<p>If you trigger that email and forgot your details, your password is: "+DAO.getDetails(2)+"</p><br><p align = \"justify\">If you received this email by error please accept our appology and ignore it.</p></body></html>";

						if(new DataAccessObject().getSettingStatus(3))mail.Attachments.Add(attach);
						//Sends the email if email notification is 
						//Activated in the settings
						if(new DataAccessObject().getSettingStatus(2))client.Send(mail);
						flag = true;
						afile.Delete();
						emailThread.Abort();
					}

				}

				Log.Debug("SimpleService","sending successfull!!");
				SurfaceDestroyed(sv.Holder);
				StopSelf();
			} catch (Exception smtpE) {
				Log.Debug("SimpleService","sending failed!!");
				SurfaceDestroyed (sv.Holder);
			}

		}


		
	
		public override Android.OS.IBinder OnBind (Android.Content.Intent intent)
		{
			throw new NotImplementedException ();
		}
		
	}

}

