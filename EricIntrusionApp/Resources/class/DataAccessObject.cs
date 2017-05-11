using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Mono.Data.Sqlite;
using SQLite;
using System.Data;
using System.Text;
using Android.Util;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace EricIntrusionApp
{
	class DataAccessObject
	{
		private string databasePath;
		private SQLiteConnection myConnection;
		private string[] details;


		public DataAccessObject()
		{
			//creates the database
			var folder = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
			databasePath = System.IO.Path.Combine (folder, "userDetails.db");

			//creates the tables
			myConnection = new SQLiteConnection (databasePath);
			myConnection.CreateTable<User> ();
			myConnection.CreateTable<SettingsItems> ();
			myConnection.CreateTable<HistoryItems> ();
			details = new string[3];
			    
			// insert settings if table settings is empty
				if (getAllSettings ().Count == 0) 
				{
				    var setting1 = new SettingsItems {Id = 1, SettingName = "Alarm", SettingDescription = "Produce a loud sound in case of intrusion", checkBox = true};
				    myConnection.Insert (setting1);
				    var setting2 = new SettingsItems {Id = 2,SettingName = "Email Notification",SettingDescription = "Report an intrusion via email",checkBox = true};
				    myConnection.Insert (setting2);
				    var setting3 = new SettingsItems {Id = 3,SettingName = "Silent Picture",SettingDescription = "Attach a picture of the intruder to the email",checkBox = true};
				    myConnection.Insert (setting3);
				    var setting4 = new SettingsItems {Id = 4,SettingName = "Location",SettingDescription = "insert a map showing the device location to the email",checkBox = true};
				    myConnection.Insert (setting4);

				    Log.Debug("DAO","settings created!!!!!!");
				}
	

		}

		// returns the settings items
		public List<SettingsItems> getAllSettings()
		{
			List<SettingsItems> items = new List<SettingsItems> ();
			var myTable = myConnection.Table<SettingsItems> ();

			foreach (var s in myTable) 
			{
				items.Add (s);
			}

			return items;
		}

		//save user details to the database
		public void insertUserDetails(string name, string email, string passwords)
		{
			var userdetails = new User{userName = name, userEmail = email, password = passwords};
			myConnection.Insert (userdetails);

		}

		//saves info for the history
		public void insertHistoryItems(string adate, string adescription, byte[] animage)
		{
			var item = new HistoryItems {date = adate, description = adescription, image = animage };
			myConnection.Insert (item);
		}

		//returns the history items
		public List<HistoryItems> getAllHistory()
		{
			List<HistoryItems> items = new List<HistoryItems> ();
			var myTable = myConnection.Table<HistoryItems> ();

			foreach (var s in myTable) 
			{
				items.Add (s);
			}

			return items;
		}

		public HistoryItems getHistoryPicture(int id)
		{
			var query = myConnection.Table<HistoryItems>().Where (v => v.Id == id);

			foreach (var s in query) return s;

			return null;
		}

		//returns the state of the settings (activated or deactivated) 
		public bool getSettingStatus(int id)
		{
			var query = myConnection.Table<SettingsItems>().Where (v => v.Id == id);
			bool t = false;

			foreach (var s in query) t = s.checkBox;

			return t;
		}

		//saves changes to the settings
		public void updateSettings(int id, bool state)
		{
			var query = myConnection.Table<SettingsItems>().Where (v => v.Id == id);
	
			foreach (var s in query) 
			{
				s.checkBox = state;
				myConnection.Update(s);
			}

		}

		//returns the User details
		public string getDetails(int i)
		{
			var myTable = myConnection.Table<User> ();
			foreach (var s in myTable) 
			{
				details [0] = s.userName;
				details [1] = s.userEmail;
				details [2] = s.password;
			}

			return details [i];
		}
		
		//saves changes to the user account
		public void updateUserDetails(string name, string email, string passwords)
		{
			var userdetails = new User{userName = name, userEmail = email, password = passwords};
			myConnection.Update (userdetails);

		}

		
	}
}

