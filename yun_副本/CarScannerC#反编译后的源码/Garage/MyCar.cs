using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Garage
{
	// Token: 0x020004B2 RID: 1202
	public class MyCar : INotifyPropertyChanged
	{
		// Token: 0x06002FC8 RID: 12232 RVA: 0x00213713 File Offset: 0x00211913
		public MyCar()
		{
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06002FC9 RID: 12233 RVA: 0x00213731 File Offset: 0x00211931
		// (set) Token: 0x06002FCA RID: 12234 RVA: 0x00213739 File Offset: 0x00211939
		public long Id
		{
			[CompilerGenerated]
			get
			{
				return this.<Id>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Id>k__BackingField = value;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06002FCB RID: 12235 RVA: 0x00213742 File Offset: 0x00211942
		// (set) Token: 0x06002FCC RID: 12236 RVA: 0x0021374A File Offset: 0x0021194A
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.OnPropertyChanged("Name");
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06002FCD RID: 12237 RVA: 0x0021375E File Offset: 0x0021195E
		// (set) Token: 0x06002FCE RID: 12238 RVA: 0x00213766 File Offset: 0x00211966
		public int Year
		{
			get
			{
				return this._Year;
			}
			set
			{
				this._Year = value;
				this.OnPropertyChanged("Year");
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06002FCF RID: 12239 RVA: 0x0021377A File Offset: 0x0021197A
		// (set) Token: 0x06002FD0 RID: 12240 RVA: 0x00213782 File Offset: 0x00211982
		public string SettingsString
		{
			[CompilerGenerated]
			get
			{
				return this.<SettingsString>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SettingsString>k__BackingField = value;
			}
		}

		// Token: 0x06002FD1 RID: 12241 RVA: 0x0021378C File Offset: 0x0021198C
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(name));
			}
		}

		// Token: 0x1400002B RID: 43
		// (add) Token: 0x06002FD2 RID: 12242 RVA: 0x002137B0 File Offset: 0x002119B0
		// (remove) Token: 0x06002FD3 RID: 12243 RVA: 0x002137E8 File Offset: 0x002119E8
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x06002FD4 RID: 12244 RVA: 0x00213820 File Offset: 0x00211A20
		public void LoadFromCurrentSettings()
		{
			string text = JsonConvert.SerializeObject(SharedSettings.Current);
			this.SettingsString = MyCar.EncodeViaBytes(text, 57);
			if (FileSystemHelper.LocalFileExists("drivecycles.bin"))
			{
				string localFilePath = FileSystemHelper.GetLocalFilePath("drivecycles.bin");
				string localFilePath2 = FileSystemHelper.GetLocalFilePath(this.Id.ToString() + "_drivecycles.bin");
				using (FileStream fileStream = File.OpenRead(localFilePath))
				{
					using (FileStream fileStream2 = File.Open(localFilePath2, FileMode.Create))
					{
						fileStream.CopyTo(fileStream2);
						fileStream2.Flush();
					}
				}
			}
		}

		// Token: 0x06002FD5 RID: 12245 RVA: 0x002138CC File Offset: 0x00211ACC
		public Tuple<bool, string> ApplyToSettings()
		{
			return MyCar.ApplyToSettings(this);
		}

		// Token: 0x06002FD6 RID: 12246 RVA: 0x002138D4 File Offset: 0x00211AD4
		private static Tuple<bool, string> ApplyToSettings(MyCar car)
		{
			try
			{
				bool adsProductPurchased = SharedSettings.Current.AdsProductPurchased;
				ConnectionTypes connectionType = SharedSettings.Current.ConnectionType;
				string btledeviceName = SharedSettings.Current.BTLEDeviceName;
				string btledeviceID = SharedSettings.Current.BTLEDeviceID;
				string btleserviceID = SharedSettings.Current.BTLEServiceID;
				string btleinputID = SharedSettings.Current.BTLEInputID;
				string btleoutputID = SharedSettings.Current.BTLEOutputID;
				string btdeviceName = SharedSettings.Current.BTDeviceName;
				string btdeviceID = SharedSettings.Current.BTDeviceID;
				SharedSettings.ResetCarSettingsToDefault();
				if (string.IsNullOrEmpty(car.SettingsString))
				{
					new SharedSettings().ConnectionType = connectionType;
				}
				else
				{
					JsonConvert.DeserializeObject<SharedSettings>(MyCar.DecodeViaBytes(car.SettingsString, 57));
				}
				SharedSettings.Reload();
				SharedSettings.Current.AdsProductPurchased = adsProductPurchased;
				if (string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID) && !string.IsNullOrEmpty(btledeviceID))
				{
					SharedSettings.Current.BTLEDeviceID = btledeviceID;
					SharedSettings.Current.BTLEDeviceName = btledeviceName;
					SharedSettings.Current.BTLEInputID = btleinputID;
					SharedSettings.Current.BTLEOutputID = btleoutputID;
					SharedSettings.Current.BTLEServiceID = btleserviceID;
				}
				if (string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID) && !string.IsNullOrEmpty(btdeviceID))
				{
					SharedSettings.Current.BTDeviceID = btdeviceID;
					SharedSettings.Current.BTDeviceName = btdeviceName;
				}
				int count = CustomPIDViewModel.CurrentProfile.PidCollection.Count;
				int count2 = CustomPIDViewModel.CurrentCustom.PidCollection.Count;
				CustomPIDViewModel.CheckIdIntegrity();
				SharedSettings.Current.CurrentCarId = car.Id;
				SharedSettings.Current.CurrentCarName = car.Name;
				ProfileUpdater.ForceUpdate();
				foreach (CustomPID customPID in CustomPIDViewModel.CurrentCustom.PidCollection)
				{
					customPID.ReloadFormula();
				}
				try
				{
					if (FileSystemHelper.LocalFileExists(car.Id.ToString() + "_drivecycles.bin"))
					{
						string localFilePath = FileSystemHelper.GetLocalFilePath("drivecycles.bin");
						using (FileStream fileStream = File.OpenRead(FileSystemHelper.GetLocalFilePath(car.Id.ToString() + "_drivecycles.bin")))
						{
							using (FileStream fileStream2 = File.Open(localFilePath, FileMode.Create))
							{
								fileStream.CopyTo(fileStream2);
								fileStream2.Flush();
							}
							goto IL_0252;
						}
					}
					if (FileSystemHelper.LocalFileExists("drivecycles.bin"))
					{
						File.Delete(FileSystemHelper.GetLocalFilePath("drivecycles.bin"));
					}
					IL_0252:;
				}
				catch (Exception)
				{
				}
				DriveCycleViewModel.Reload();
			}
			catch (Exception ex)
			{
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance != null)
				{
					instance.OnDashboardConfigurationUpdated();
				}
				return new Tuple<bool, string>(false, ex.ToString());
			}
			CarPlayManager instance2 = CarPlayManager.Instance;
			if (instance2 != null)
			{
				instance2.OnDashboardConfigurationUpdated();
			}
			return new Tuple<bool, string>(true, "");
		}

		// Token: 0x06002FD7 RID: 12247 RVA: 0x00213BFC File Offset: 0x00211DFC
		private static string EncodeViaBytes(string input, byte key)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(input);
			for (int i = 0; i < bytes.Length; i++)
			{
				if (i % 2 == 0)
				{
					byte b = bytes[i];
					b ^= key;
					bytes[i] = b;
				}
				else
				{
					byte b2 = bytes[i];
					b2 += key;
					b2 += (byte)i;
					bytes[i] = b2;
				}
			}
			return Convert.ToBase64String(bytes);
		}

		// Token: 0x06002FD8 RID: 12248 RVA: 0x00213C50 File Offset: 0x00211E50
		private static string DecodeViaBytes(string input, byte key)
		{
			byte[] array = Convert.FromBase64String(input);
			for (int i = 0; i < array.Length; i++)
			{
				if (i % 2 == 0)
				{
					byte b = array[i];
					b ^= key;
					array[i] = b;
				}
				else
				{
					byte b2 = array[i];
					b2 -= key;
					b2 -= (byte)i;
					array[i] = b2;
				}
			}
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x04001BC5 RID: 7109
		[CompilerGenerated]
		private long <Id>k__BackingField;

		// Token: 0x04001BC6 RID: 7110
		private string _Name = "";

		// Token: 0x04001BC7 RID: 7111
		private int _Year = 2020;

		// Token: 0x04001BC8 RID: 7112
		[CompilerGenerated]
		private string <SettingsString>k__BackingField;

		// Token: 0x04001BC9 RID: 7113
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
