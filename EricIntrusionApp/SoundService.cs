using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Media;
using Android.Widget;

namespace EricIntrusionApp
{
	[Service]			
	public class SoundService : Service
	{
		AudioManager soundManager;
		MediaPlayer alarm;

		public override void OnStart (Android.Content.Intent intent, int startId)
		{
			base.OnStart (intent, startId);
		
			soundManager = (AudioManager) GetSystemService (AudioService);
			soundManager.SetStreamVolume (Stream.Music, soundManager.GetStreamMaxVolume (Stream.Music),VolumeNotificationFlags.AllowRingerModes);
			alarm = MediaPlayer.Create (this, Resource.Raw.siren);
			alarm.SetAudioStreamType (Stream.Music);
			alarm.Looping = true;
			alarm.Start ();

		}
		
		public override void OnDestroy()
		{
			alarm.Release ();
			//alarm.Reset ();
			base.OnDestroy ();
		}

		public override IBinder OnBind (Intent intent)
		{
			throw new NotImplementedException ();
		}

	}
}

