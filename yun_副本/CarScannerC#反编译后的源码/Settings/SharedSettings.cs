using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Styles;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000274 RID: 628
	public class SharedSettings : INotifyPropertyChanged
	{
		// Token: 0x06001C33 RID: 7219 RVA: 0x00144CF8 File Offset: 0x00142EF8
		public SharedSettings()
		{
		}

		// Token: 0x06001C34 RID: 7220 RVA: 0x00144D2D File Offset: 0x00142F2D
		public static void Reload()
		{
			SharedSettings._Current = new SharedSettings();
			CustomPIDViewModel.Reload();
		}

		// Token: 0x06001C35 RID: 7221 RVA: 0x00144D40 File Offset: 0x00142F40
		public static void ResetCarSettingsToDefault()
		{
			IEnumerable<PropertyInfo> enumerable = from x in typeof(SharedSettings).GetProperties()
				where !x.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any<object>()
				select x;
			SettingsV2 settingsV = new SettingsV2();
			if (settingsV.Contains("ProfilePIDsCollection"))
			{
				settingsV.Remove("ProfilePIDsCollection");
			}
			if (settingsV.Contains("ProfilePIDsCollection2"))
			{
				settingsV.Remove("ProfilePIDsCollection2");
			}
			if (settingsV.Contains("CustomPIDsCollection"))
			{
				settingsV.Remove("CustomPIDsCollection");
			}
			CustomPIDViewModel.Reload();
			foreach (PropertyInfo propertyInfo in enumerable)
			{
				try
				{
					if (settingsV.Contains(propertyInfo.Name))
					{
						settingsV.Remove(propertyInfo.Name);
					}
				}
				catch (Exception)
				{
				}
			}
			SharedSettings.Reload();
		}

		// Token: 0x17000FAC RID: 4012
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x00144E38 File Offset: 0x00143038
		public static SharedSettings Current
		{
			get
			{
				if (SharedSettings._Current == null)
				{
					SharedSettings._Current = new SharedSettings();
				}
				return SharedSettings._Current;
			}
		}

		// Token: 0x06001C37 RID: 7223 RVA: 0x00144E50 File Offset: 0x00143050
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06001C38 RID: 7224 RVA: 0x00144E6C File Offset: 0x0014306C
		// (remove) Token: 0x06001C39 RID: 7225 RVA: 0x00144EA4 File Offset: 0x001430A4
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

		// Token: 0x17000FAD RID: 4013
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x00144ED9 File Offset: 0x001430D9
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x00144F10 File Offset: 0x00143110
		[JsonIgnore]
		public bool MigratedToSettingsV2
		{
			get
			{
				if (this._MigratedToSettingsV2 == null)
				{
					this._MigratedToSettingsV2 = new bool?(this.AppSettings.GetValueOrDefault<bool>("MigratedToSettingsV2", false, null));
				}
				return this._MigratedToSettingsV2.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("MigratedToSettingsV2", value, null);
				this._MigratedToSettingsV2 = new bool?(value);
				this.NotifyPropertyChanged("MigratedToSettingsV2");
			}
		}

		// Token: 0x17000FAE RID: 4014
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x00144F3C File Offset: 0x0014313C
		// (set) Token: 0x06001C3D RID: 7229 RVA: 0x00144F8C File Offset: 0x0014318C
		[JsonIgnore]
		public DateTime LicenseFinishDate
		{
			get
			{
				if (this._LicenseFinishDate == null)
				{
					this._LicenseFinishDate = new DateTime?(new DateTime(this.AppSettings.GetValueOrDefault<long>("LicenseFinishDate", DateTime.MinValue.Ticks, null)));
				}
				return this._LicenseFinishDate.Value;
			}
			set
			{
				if (value == this.LicenseFinishDate)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LicenseFinishDate", value.Ticks, null);
				this._LicenseFinishDate = new DateTime?(value);
				this.NotifyPropertyChanged("LicenseFinishDate");
			}
		}

		// Token: 0x17000FAF RID: 4015
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x00144FCC File Offset: 0x001431CC
		// (set) Token: 0x06001C3F RID: 7231 RVA: 0x00145003 File Offset: 0x00143203
		[JsonIgnore]
		public bool ShowWhenTrialExpires
		{
			get
			{
				if (this._ShowWhenTrialExpires == null)
				{
					this._ShowWhenTrialExpires = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowWhenTrialExpires", true, null));
				}
				return this._ShowWhenTrialExpires.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ShowWhenTrialExpires", value, null);
				this._ShowWhenTrialExpires = new bool?(value);
				this.NotifyPropertyChanged("ShowWhenTrialExpires");
			}
		}

		// Token: 0x17000FB0 RID: 4016
		// (get) Token: 0x06001C40 RID: 7232 RVA: 0x0014502E File Offset: 0x0014322E
		// (set) Token: 0x06001C41 RID: 7233 RVA: 0x00145065 File Offset: 0x00143265
		[JsonIgnore]
		public int TrialExtentions
		{
			get
			{
				if (this._TrialExtentions == null)
				{
					this._TrialExtentions = new int?(this.AppSettings.GetValueOrDefault<int>("TrialExtentions", 0, null));
				}
				return this._TrialExtentions.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("TrialExtentions", value, null);
				this._TrialExtentions = new int?(value);
				this.NotifyPropertyChanged("TrialExtentions");
			}
		}

		// Token: 0x17000FB1 RID: 4017
		// (get) Token: 0x06001C42 RID: 7234 RVA: 0x00145090 File Offset: 0x00143290
		[JsonIgnore]
		public bool ManageSubscriptionsVisible
		{
			get
			{
				return PlatformHelper.IsAndroid && PlatformHelper.AppMarket != Markets.RUS && PlatformHelper.AppMarket != Markets.HMS && (this.AdsProductPurchased && this.LicenseFinishDate != DateTime.MinValue && this.LicenseFinishDate != DateTime.MaxValue && this.LicenseFinishDate >= DateTime.UtcNow);
			}
		}

		// Token: 0x17000FB2 RID: 4018
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x001450FC File Offset: 0x001432FC
		// (set) Token: 0x06001C44 RID: 7236 RVA: 0x00145198 File Offset: 0x00143398
		[JsonIgnore]
		public bool AdsProductPurchased
		{
			get
			{
				if (this._AdsProductPurchased == null)
				{
					this._AdsProductPurchased = new bool?(this.AppSettings.GetValueOrDefault<bool>("AdsProductPurchased", false, null));
				}
				if (!this._AdsProductPurchased.GetValueOrDefault())
				{
					return this._AdsProductPurchased.Value;
				}
				if (this.LicenseFinishDate == DateTime.MinValue || this.LicenseFinishDate == DateTime.MaxValue || DateTime.UtcNow <= this.LicenseFinishDate)
				{
					return true;
				}
				if (PlatformHelper.IsiOS)
				{
					PlatformHelper.IOSService.StoreReceiptParser_LoadReceipt();
				}
				return false;
			}
			set
			{
				bool? adsProductPurchased = this._AdsProductPurchased;
				if (!((adsProductPurchased.GetValueOrDefault() == value) & (adsProductPurchased != null)))
				{
					if (this._AdsProductPurchased.GetValueOrDefault() && !value && this.FreePeriodGoodConnections < 25)
					{
						this.FreePeriodGoodConnections = 25;
					}
					this.AppSettings.AddOrUpdateValue("AdsProductPurchased", value, null);
					this._AdsProductPurchased = new bool?(value);
					this.NotifyPropertyChanged("AdsProductPurchased");
					if (!value)
					{
						this.LastTimeLicenceChecked = 0L;
						this.PurchaseResponseCached = "";
					}
				}
			}
		}

		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x06001C45 RID: 7237 RVA: 0x00145223 File Offset: 0x00143423
		// (set) Token: 0x06001C46 RID: 7238 RVA: 0x0014525B File Offset: 0x0014345B
		[JsonIgnore]
		public long FreePeriodFinishTime
		{
			get
			{
				if (this._FreePeriodFinishTime == null)
				{
					this._FreePeriodFinishTime = new long?(this.AppSettings.GetValueOrDefault<long>("FreePeriodFinishTime", 0L, null));
				}
				return this._FreePeriodFinishTime.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("FreePeriodFinishTime", value, null);
				this._FreePeriodFinishTime = new long?(value);
				this.NotifyPropertyChanged("FreePeriodFinishTime");
			}
		}

		// Token: 0x17000FB4 RID: 4020
		// (get) Token: 0x06001C47 RID: 7239 RVA: 0x00145286 File Offset: 0x00143486
		// (set) Token: 0x06001C48 RID: 7240 RVA: 0x001452BD File Offset: 0x001434BD
		[Backupable]
		[JsonIgnore]
		public bool SendStatistics
		{
			get
			{
				if (this._SendStatistics == null)
				{
					this._SendStatistics = new bool?(this.AppSettings.GetValueOrDefault<bool>("SendStatistics", true, null));
				}
				return this._SendStatistics.Value;
			}
			set
			{
				if (value == this.SendStatistics)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendStatistics", value, null);
				this._SendStatistics = new bool?(value);
				this.NotifyPropertyChanged("SendStatistics");
			}
		}

		// Token: 0x17000FB5 RID: 4021
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x001452F2 File Offset: 0x001434F2
		// (set) Token: 0x06001C4A RID: 7242 RVA: 0x0014531E File Offset: 0x0014351E
		[JsonIgnore]
		public string AskedForUpdateToVersion
		{
			get
			{
				if (this._AskedForUpdateToVersion == null)
				{
					this._AskedForUpdateToVersion = this.AppSettings.GetValueOrDefault<string>("AskedForUpdateToVersion", App.Version, null);
				}
				return this._AskedForUpdateToVersion;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("AskedForUpdateToVersion", value, null);
				this._AskedForUpdateToVersion = value;
				this.NotifyPropertyChanged("AskedForUpdateToVersion");
			}
		}

		// Token: 0x17000FB6 RID: 4022
		// (get) Token: 0x06001C4B RID: 7243 RVA: 0x00145344 File Offset: 0x00143544
		// (set) Token: 0x06001C4C RID: 7244 RVA: 0x00145370 File Offset: 0x00143570
		[JsonIgnore]
		public string LatestVersion
		{
			get
			{
				if (this._LatestVersion == null)
				{
					this._LatestVersion = this.AppSettings.GetValueOrDefault<string>("LatestVersion", "", null);
				}
				return this._LatestVersion;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("LatestVersion", value, null);
				this._LatestVersion = value;
				this.NotifyPropertyChanged("LatestVersion");
			}
		}

		// Token: 0x17000FB7 RID: 4023
		// (get) Token: 0x06001C4D RID: 7245 RVA: 0x00145396 File Offset: 0x00143596
		// (set) Token: 0x06001C4E RID: 7246 RVA: 0x001453C2 File Offset: 0x001435C2
		public string MinVersion
		{
			get
			{
				if (this._MinVersion == null)
				{
					this._MinVersion = this.AppSettings.GetValueOrDefault<string>("MinVersion", App.Version, null);
				}
				return this._MinVersion;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("MinVersion", value, null);
				this._MinVersion = value;
				this.NotifyPropertyChanged("MinVersion");
			}
		}

		// Token: 0x17000FB8 RID: 4024
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x001453E8 File Offset: 0x001435E8
		// (set) Token: 0x06001C50 RID: 7248 RVA: 0x00145414 File Offset: 0x00143614
		public string AvailableVersion
		{
			get
			{
				if (this._AvailableVersion == null)
				{
					this._AvailableVersion = this.AppSettings.GetValueOrDefault<string>("AvailableVersion", App.Version, null);
				}
				return this._AvailableVersion;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("AvailableVersion", value, null);
				this._AvailableVersion = value;
				this.NotifyPropertyChanged("AvailableVersion");
			}
		}

		// Token: 0x17000FB9 RID: 4025
		// (get) Token: 0x06001C51 RID: 7249 RVA: 0x0014543C File Offset: 0x0014363C
		// (set) Token: 0x06001C52 RID: 7250 RVA: 0x00145488 File Offset: 0x00143688
		[Backupable]
		[JsonIgnore]
		public bool GDPR_ShowPersonalyzed
		{
			get
			{
				if (this.AdsProductPurchased)
				{
					return false;
				}
				if (this._GDPR_ShowPersonalyzed == null)
				{
					this._GDPR_ShowPersonalyzed = new bool?(this.AppSettings.GetValueOrDefault<bool>("GDPR_ShowPersonalyzed", true, null));
				}
				return this._GDPR_ShowPersonalyzed.Value;
			}
			set
			{
				if (value == this.GDPR_ShowPersonalyzed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("GDPR_ShowPersonalyzed", value, null);
				this._GDPR_ShowPersonalyzed = new bool?(value);
				this.NotifyPropertyChanged("GDPR_ShowPersonalyzed");
			}
		}

		// Token: 0x17000FBA RID: 4026
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x001454BD File Offset: 0x001436BD
		// (set) Token: 0x06001C54 RID: 7252 RVA: 0x001454EC File Offset: 0x001436EC
		[JsonIgnore]
		public string LastException
		{
			get
			{
				if (this._LastException == null)
				{
					this._LastException = this.AppSettings.GetValueOrDefault<string>("LastException", "", null);
				}
				return this._LastException;
			}
			set
			{
				string text = "";
				string text2 = "";
				if (!string.IsNullOrEmpty(value))
				{
					try
					{
						if (App.GetCurrentPage() != null)
						{
							string text3 = "\r\nCurrent page = " + App.GetCurrentPage().GetType().Name;
							if (App.OBDReader != null)
							{
								text = "\r\nConnection state = " + App.OBDReader.CurrentStatus.ToString() + "\r\n";
							}
							if (App.OBDSimulator != null)
							{
								text2 = "\r\nSimulator active state = " + App.OBDSimulator.IsActive.ToString() + "\r\n";
							}
							value = value + text3 + text + text2;
							if (PlatformHelper.IsiOS)
							{
								value = value + "\r\nApp Startuplog: " + PlatformHelper.IOSService.AppDelegate_StartupLog;
							}
							if (text3 == "DashboardXamlPage")
							{
								value = value + "\r\nDashboard configuration:\r\n" + this.Dashboard;
							}
						}
					}
					catch (Exception)
					{
					}
				}
				this.AppSettings.AddOrUpdateValue("LastException", value, null);
				this._LastException = value;
				this.NotifyPropertyChanged("LastException");
			}
		}

		// Token: 0x17000FBB RID: 4027
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x00145618 File Offset: 0x00143818
		// (set) Token: 0x06001C56 RID: 7254 RVA: 0x00145620 File Offset: 0x00143820
		[JsonIgnore]
		public bool FlushLog
		{
			[CompilerGenerated]
			get
			{
				return this.<FlushLog>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FlushLog>k__BackingField = value;
			}
		}

		// Token: 0x17000FBC RID: 4028
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x00145629 File Offset: 0x00143829
		// (set) Token: 0x06001C58 RID: 7256 RVA: 0x00145660 File Offset: 0x00143860
		[Backupable]
		public bool UseGPS
		{
			get
			{
				if (this._UseGPS == null)
				{
					this._UseGPS = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseGPS", false, null));
				}
				return this._UseGPS.Value;
			}
			set
			{
				if (value != this.UseGPS)
				{
					this.AppSettings.AddOrUpdateValue("UseGPS", value, null);
					this._UseGPS = new bool?(value);
					this.NotifyPropertyChanged("UseGPS");
					try
					{
						if (!value && App.OBDReader != null && App.OBDReader.CurrentCarData != null && App.OBDReader.CurrentCarData.LiveDataPIDs != null)
						{
							PID pid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed);
							if (pid != null)
							{
								(pid as SensorPID).Stop();
								pid.IsAvailable = false;
								if (LiveDataPIDModel._PIDCollection.Contains(pid))
								{
									LiveDataPIDModel._PIDCollection.Remove(pid);
								}
								pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAvgSpeedGPS);
								if (pid != null)
								{
									LiveDataPIDModel._PIDCollection.Remove(pid);
								}
							}
							PID pid2 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSAltitude);
							if (pid2 != null)
							{
								(pid2 as SensorPID).Stop();
								pid2.IsAvailable = false;
								if (LiveDataPIDModel._PIDCollection.Contains(pid2))
								{
									LiveDataPIDModel._PIDCollection.Remove(pid2);
								}
							}
						}
						if (value)
						{
							if (PlatformHelper.IsAndroid)
							{
								PlatformHelper.DroidService.CheckLocationPermission();
								if (!PlatformHelper.DroidService.HasLocationPermission)
								{
									MainThread.BeginInvokeOnMainThread(async delegate
									{
										TaskAwaiter<bool> taskAwaiter3 = App.GetCurrentPage().DisplayAlert(Translate.GetString("droid_BackgroundGPSTitle"), Translate.GetString("droid_BackgroundGPS"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											await taskAwaiter3;
											taskAwaiter3 = taskAwaiter2;
											taskAwaiter2 = default(TaskAwaiter<bool>);
										}
										if (taskAwaiter3.GetResult())
										{
											await this.RequestLocationPermissionAsync();
										}
										else
										{
											this.UseGPS = false;
										}
									});
								}
							}
							else if (PlatformHelper.IsiOS)
							{
								this.RequestLocationPermissionAsync();
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x00145834 File Offset: 0x00143A34
		private async Task RequestLocationPermissionAsync()
		{
			TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
			TaskAwaiter<PermissionStatus> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
			}
			if (taskAwaiter.GetResult() != 3)
			{
				try
				{
					taskAwaiter = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() != 3)
					{
						this.UseGPS = false;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x17000FBD RID: 4029
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x00145878 File Offset: 0x00143A78
		// (set) Token: 0x06001C5B RID: 7259 RVA: 0x001458CE File Offset: 0x00143ACE
		[Backupable]
		public bool UseGPSForFuelConsumption
		{
			get
			{
				if (!this.UseGPS)
				{
					return false;
				}
				if (!this.ShowExperimental)
				{
					return false;
				}
				if (this._UseGPSForFuelConsumption == null)
				{
					this._UseGPSForFuelConsumption = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseGPSForFuelConsumption", false, null));
				}
				return this._UseGPSForFuelConsumption.Value;
			}
			set
			{
				if (value == this.UseGPSForFuelConsumption)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseGPSForFuelConsumption", value, null);
				this._UseGPSForFuelConsumption = new bool?(value);
				this.NotifyPropertyChanged("UseGPSForFuelConsumption");
			}
		}

		// Token: 0x17000FBE RID: 4030
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x00145903 File Offset: 0x00143B03
		// (set) Token: 0x06001C5D RID: 7261 RVA: 0x0014593A File Offset: 0x00143B3A
		[Backupable]
		[JsonIgnore]
		public bool ShowExperimental
		{
			get
			{
				if (this._ShowExperimental == null)
				{
					this._ShowExperimental = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowExperimental", false, null));
				}
				return this._ShowExperimental.Value;
			}
			set
			{
				if (value == this.ShowExperimental)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowExperimental", value, null);
				this._ShowExperimental = new bool?(value);
				this.NotifyPropertyChanged("ShowExperimental");
				if (!value)
				{
					this.DeveloperMode = false;
				}
			}
		}

		// Token: 0x17000FBF RID: 4031
		// (get) Token: 0x06001C5E RID: 7262 RVA: 0x00145979 File Offset: 0x00143B79
		// (set) Token: 0x06001C5F RID: 7263 RVA: 0x001459A5 File Offset: 0x00143BA5
		[JsonIgnore]
		public string PurchaseToken
		{
			get
			{
				if (this._PurchaseToken == null)
				{
					this._PurchaseToken = this.AppSettings.GetValueOrDefault<string>("PurchaseToken", "", null);
				}
				return this._PurchaseToken;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("PurchaseToken", value, null);
				this._PurchaseToken = value;
				this.NotifyPropertyChanged("PurchaseToken");
			}
		}

		// Token: 0x17000FC0 RID: 4032
		// (get) Token: 0x06001C60 RID: 7264 RVA: 0x001459CB File Offset: 0x00143BCB
		// (set) Token: 0x06001C61 RID: 7265 RVA: 0x001459F7 File Offset: 0x00143BF7
		[JsonIgnore]
		public string PurchaseProductId
		{
			get
			{
				if (this._PurchaseProductId == null)
				{
					this._PurchaseProductId = this.AppSettings.GetValueOrDefault<string>("PurchaseProductId", "", null);
				}
				return this._PurchaseProductId;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("PurchaseProductId", value, null);
				this._PurchaseProductId = value;
				this.NotifyPropertyChanged("PurchaseProductId");
			}
		}

		// Token: 0x17000FC1 RID: 4033
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x00145A1D File Offset: 0x00143C1D
		// (set) Token: 0x06001C63 RID: 7267 RVA: 0x00145A49 File Offset: 0x00143C49
		[JsonIgnore]
		public string PurchaseOrderId
		{
			get
			{
				if (this._PurchaseOrderId == null)
				{
					this._PurchaseOrderId = this.AppSettings.GetValueOrDefault<string>("PurchaseOrderId", "", null);
				}
				return this._PurchaseOrderId;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("PurchaseOrderId", value, null);
				this._PurchaseOrderId = value;
				this.NotifyPropertyChanged("PurchaseOrderId");
			}
		}

		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x00145A6F File Offset: 0x00143C6F
		// (set) Token: 0x06001C65 RID: 7269 RVA: 0x00145AA6 File Offset: 0x00143CA6
		[JsonIgnore]
		public bool PurchaseDebug
		{
			get
			{
				if (this._PurchaseDebug == null)
				{
					this._PurchaseDebug = new bool?(this.AppSettings.GetValueOrDefault<bool>("PurchaseDebug", false, null));
				}
				return this._PurchaseDebug.Value;
			}
			set
			{
				if (value == this.PurchaseDebug)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PurchaseDebug", value, null);
				this._PurchaseDebug = new bool?(value);
				this.NotifyPropertyChanged("PurchaseDebug");
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x06001C66 RID: 7270 RVA: 0x00145ADC File Offset: 0x00143CDC
		// (set) Token: 0x06001C67 RID: 7271 RVA: 0x00145B14 File Offset: 0x00143D14
		[JsonIgnore]
		public bool IsTrialExpired
		{
			get
			{
				if (this.AdsProductPurchased)
				{
					return false;
				}
				IKeyPairStorage keyPairStorage = DependencyService.Get<IKeyPairStorage>(0);
				return keyPairStorage.KeyExists("IsTrialExpired") && keyPairStorage.GetBool("IsTrialExpired");
			}
			set
			{
				DependencyService.Get<IKeyPairStorage>(0).SetBool("IsTrialExpired", value);
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x06001C68 RID: 7272 RVA: 0x00145B27 File Offset: 0x00143D27
		// (set) Token: 0x06001C69 RID: 7273 RVA: 0x00145B5E File Offset: 0x00143D5E
		[JsonIgnore]
		public bool ReviewReceived
		{
			get
			{
				if (this._ReviewReceived == null)
				{
					this._ReviewReceived = new bool?(this.AppSettings.GetValueOrDefault<bool>("ReviewReceived", false, null));
				}
				return this._ReviewReceived.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ReviewReceived", value, null);
				this._ReviewReceived = new bool?(value);
				this.NotifyPropertyChanged("ReviewReceived");
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x06001C6A RID: 7274 RVA: 0x00145B89 File Offset: 0x00143D89
		// (set) Token: 0x06001C6B RID: 7275 RVA: 0x00145BC0 File Offset: 0x00143DC0
		[JsonIgnore]
		public int AskForReviewGoodConnections
		{
			get
			{
				if (this._AskForReviewGoodConnections == null)
				{
					this._AskForReviewGoodConnections = new int?(this.AppSettings.GetValueOrDefault<int>("AskForReviewGoodConnections", 2, null));
				}
				return this._AskForReviewGoodConnections.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("AskForReviewGoodConnections", value, null);
				this._AskForReviewGoodConnections = new int?(value);
				this.NotifyPropertyChanged("AskForReviewGoodConnections");
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06001C6C RID: 7276 RVA: 0x00145BEC File Offset: 0x00143DEC
		// (set) Token: 0x06001C6D RID: 7277 RVA: 0x00145C45 File Offset: 0x00143E45
		public string ProfilePIDsCollection
		{
			get
			{
				string valueOrDefault = this.AppSettings.GetValueOrDefault<string>("ProfilePIDsCollection", "", "pr.png");
				if (!string.IsNullOrEmpty(valueOrDefault))
				{
					this.EncryptedProfilePIDsCollection = valueOrDefault;
					this.AppSettings.AddOrUpdateValue("ProfilePIDsCollection", "", "pr.png");
					return valueOrDefault;
				}
				return this.EncryptedProfilePIDsCollection;
			}
			set
			{
				this.EncryptedProfilePIDsCollection = value;
				this.NotifyPropertyChanged("ProfilePIDsCollection");
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06001C6E RID: 7278 RVA: 0x00145C5C File Offset: 0x00143E5C
		// (set) Token: 0x06001C6F RID: 7279 RVA: 0x00145CB4 File Offset: 0x00143EB4
		private string EncryptedProfilePIDsCollection
		{
			get
			{
				string text;
				try
				{
					text = SharedSettings.DecodeViaBytes(this.AppSettings.GetValueOrDefault<string>("ProfilePIDsCollection2", "", "pr.png"), (byte)"Car Scanner"[3]);
				}
				catch (Exception)
				{
					text = "";
				}
				return text;
			}
			set
			{
				string text = SharedSettings.EncodeViaBytes(value, (byte)"Car Scanner"[3]);
				this.AppSettings.AddOrUpdateValue("ProfilePIDsCollection2", text, "pr.png");
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00145CEC File Offset: 0x00143EEC
		// (set) Token: 0x06001C71 RID: 7281 RVA: 0x00145D44 File Offset: 0x00143F44
		public string DisabledProfilePIDsCollection
		{
			get
			{
				string text;
				try
				{
					text = SharedSettings.DecodeViaBytes(this.AppSettings.GetValueOrDefault<string>("DisabledProfilePIDsCollection", "", "pr.png"), (byte)"Car Scanner"[4]);
				}
				catch (Exception)
				{
					text = "";
				}
				return text;
			}
			set
			{
				string text = SharedSettings.EncodeViaBytes(value, (byte)"Car Scanner"[4]);
				this.AppSettings.AddOrUpdateValue("DisabledProfilePIDsCollection", text, "pr.png");
			}
		}

		// Token: 0x06001C72 RID: 7282 RVA: 0x00145D7C File Offset: 0x00143F7C
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

		// Token: 0x06001C73 RID: 7283 RVA: 0x00145DD0 File Offset: 0x00143FD0
		private static string DecodeViaBytes(string input, byte key)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
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

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x00145E2E File Offset: 0x0014402E
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x00145E4A File Offset: 0x0014404A
		public string CustomPIDsCollection
		{
			get
			{
				return this.AppSettings.GetValueOrDefault<string>("CustomPIDsCollection", "", "cs.png");
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("CustomPIDsCollection", value, "cs.png");
				this.NotifyPropertyChanged("CustomPIDsCollection");
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06001C76 RID: 7286 RVA: 0x00145E6D File Offset: 0x0014406D
		// (set) Token: 0x06001C77 RID: 7287 RVA: 0x00145EA8 File Offset: 0x001440A8
		[JsonIgnore]
		[Backupable]
		public int CustomPidLastId
		{
			get
			{
				if (this._CustomPidLastId == null)
				{
					this._CustomPidLastId = new int?(this.AppSettings.GetValueOrDefault<int>("CustomPidLastId", 1001, null));
				}
				return this._CustomPidLastId.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("CustomPidLastId", value, null);
				this._CustomPidLastId = new int?(value);
				this.NotifyPropertyChanged("CustomPidLastId");
			}
		}

		// Token: 0x17000FCB RID: 4043
		// (get) Token: 0x06001C78 RID: 7288 RVA: 0x00145ED4 File Offset: 0x001440D4
		// (set) Token: 0x06001C79 RID: 7289 RVA: 0x00145F26 File Offset: 0x00144126
		[JsonIgnore]
		[Backupable]
		public string SpeedTestDB
		{
			get
			{
				if (this._SpeedTestDB == null)
				{
					if (this.AppSettings.Contains("SpeedTestDB"))
					{
						this._SpeedTestDB = this.AppSettings.GetValueOrDefault<string>("SpeedTestDB", string.Empty, null);
					}
					else
					{
						this._SpeedTestDB = null;
					}
				}
				return this._SpeedTestDB;
			}
			set
			{
				if (value == this.SpeedTestDB)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SpeedTestDB", value, null);
				this._SpeedTestDB = value;
				this.NotifyPropertyChanged("SpeedTestDB");
			}
		}

		// Token: 0x17000FCC RID: 4044
		// (get) Token: 0x06001C7A RID: 7290 RVA: 0x00145F5B File Offset: 0x0014415B
		// (set) Token: 0x06001C7B RID: 7291 RVA: 0x00145F93 File Offset: 0x00144193
		public int NoDataLimit
		{
			get
			{
				if (this._NoDataLimit == null)
				{
					this._NoDataLimit = new int?(this.AppSettings.GetValueOrDefault<int>("NoDataLimit", 40, null));
				}
				return this._NoDataLimit.Value;
			}
			set
			{
				if (value == this.NoDataLimit)
				{
					return;
				}
				if (value < 1)
				{
					value = 1;
				}
				this.AppSettings.AddOrUpdateValue("NoDataLimit", value, null);
				this._NoDataLimit = new int?(value);
				this.NotifyPropertyChanged("NoDataLimit");
			}
		}

		// Token: 0x17000FCD RID: 4045
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x00145FCF File Offset: 0x001441CF
		// (set) Token: 0x06001C7D RID: 7293 RVA: 0x00146006 File Offset: 0x00144206
		public int DelayBeforeReconnect
		{
			get
			{
				if (this._DelayBeforeReconnect == null)
				{
					this._DelayBeforeReconnect = new int?(this.AppSettings.GetValueOrDefault<int>("DelayBeforeReconnect", 0, null));
				}
				return this._DelayBeforeReconnect.Value;
			}
			set
			{
				if (value == this.DelayBeforeReconnect)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DelayBeforeReconnect", value, null);
				this._DelayBeforeReconnect = new int?(value);
				this.NotifyPropertyChanged("DelayBeforeReconnect");
			}
		}

		// Token: 0x17000FCE RID: 4046
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x0014603B File Offset: 0x0014423B
		// (set) Token: 0x06001C7F RID: 7295 RVA: 0x00146072 File Offset: 0x00144272
		[Backupable]
		public bool DisableOptimizationIfItFails
		{
			get
			{
				if (this._DisableOptimizationIfItFails == null)
				{
					this._DisableOptimizationIfItFails = new bool?(this.AppSettings.GetValueOrDefault<bool>("DisableOptimizationIfItFails", true, null));
				}
				return this._DisableOptimizationIfItFails.Value;
			}
			set
			{
				if (value == this.DisableOptimizationIfItFails)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DisableOptimizationIfItFails", value, null);
				this._DisableOptimizationIfItFails = new bool?(value);
				this.NotifyPropertyChanged("DisableOptimizationIfItFails");
			}
		}

		// Token: 0x17000FCF RID: 4047
		// (get) Token: 0x06001C80 RID: 7296 RVA: 0x001460A7 File Offset: 0x001442A7
		// (set) Token: 0x06001C81 RID: 7297 RVA: 0x001460D3 File Offset: 0x001442D3
		[Backupable]
		public string SelectedProfileV2Name
		{
			get
			{
				if (this._SelectedProfileV2Name == null)
				{
					this._SelectedProfileV2Name = this.AppSettings.GetValueOrDefault<string>("SelectedProfileV2Name", "", null);
				}
				return this._SelectedProfileV2Name;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("SelectedProfileV2Name", value, null);
				this._SelectedProfileV2Name = value;
				this.NotifyPropertyChanged("SelectedProfileV2Name");
				this.SelectedProfileName = "";
				this.NotifyPropertyChanged("BrandAndProfile");
			}
		}

		// Token: 0x17000FD0 RID: 4048
		// (get) Token: 0x06001C82 RID: 7298 RVA: 0x0014610F File Offset: 0x0014430F
		// (set) Token: 0x06001C83 RID: 7299 RVA: 0x0014613B File Offset: 0x0014433B
		[Backupable]
		public string SelectedBrand
		{
			get
			{
				if (this._SelectedBrand == null)
				{
					this._SelectedBrand = this.AppSettings.GetValueOrDefault<string>("SelectedBrand", "", null);
				}
				return this._SelectedBrand;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("SelectedBrand", value, null);
				this._SelectedBrand = value;
				this.NotifyPropertyChanged("SelectedBrand");
				this.NotifyPropertyChanged("BrandAndProfile");
			}
		}

		// Token: 0x17000FD1 RID: 4049
		// (get) Token: 0x06001C84 RID: 7300 RVA: 0x0014616C File Offset: 0x0014436C
		// (set) Token: 0x06001C85 RID: 7301 RVA: 0x001461C0 File Offset: 0x001443C0
		public string BrandForDTC
		{
			get
			{
				if (this._BrandForDTC == null)
				{
					this._BrandForDTC = this.AppSettings.GetValueOrDefault<string>("BrandForDTC", "", null);
				}
				if (string.IsNullOrEmpty(this._BrandForDTC))
				{
					this._BrandForDTC = SharedSettings.Current.SelectedBrand;
				}
				return this._BrandForDTC;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("BrandForDTC", value, null);
				this._BrandForDTC = value;
				this.NotifyPropertyChanged("BrandForDTC");
			}
		}

		// Token: 0x17000FD2 RID: 4050
		// (get) Token: 0x06001C86 RID: 7302 RVA: 0x001461E6 File Offset: 0x001443E6
		[JsonIgnore]
		public string BrandAndProfile
		{
			get
			{
				return this.SelectedBrand + " " + this.SelectedProfileV2Name;
			}
		}

		// Token: 0x17000FD3 RID: 4051
		// (get) Token: 0x06001C87 RID: 7303 RVA: 0x001461FE File Offset: 0x001443FE
		// (set) Token: 0x06001C88 RID: 7304 RVA: 0x0014622A File Offset: 0x0014442A
		[Backupable]
		public string ProfileUpdateAlias
		{
			get
			{
				if (this._ProfileUpdateAlias == null)
				{
					this._ProfileUpdateAlias = this.AppSettings.GetValueOrDefault<string>("ProfileUpdateAlias", "", null);
				}
				return this._ProfileUpdateAlias;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ProfileUpdateAlias", value, null);
				this._ProfileUpdateAlias = value;
				this.NotifyPropertyChanged("ProfileUpdateAlias");
			}
		}

		// Token: 0x17000FD4 RID: 4052
		// (get) Token: 0x06001C89 RID: 7305 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06001C8A RID: 7306 RVA: 0x00146250 File Offset: 0x00144450
		[Backupable]
		public bool CheckIsELMWhenConnecting
		{
			get
			{
				return false;
			}
			set
			{
				if (value == this.CheckIsELMWhenConnecting)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CheckIsELMWhenConnecting", value, null);
				this._CheckIsELMWhenConnecting = new bool?(value);
				this.NotifyPropertyChanged("CheckIsELMWhenConnecting");
			}
		}

		// Token: 0x17000FD5 RID: 4053
		// (get) Token: 0x06001C8B RID: 7307 RVA: 0x00146285 File Offset: 0x00144485
		// (set) Token: 0x06001C8C RID: 7308 RVA: 0x001462BC File Offset: 0x001444BC
		[Backupable]
		public bool BindSocketToWiFiNetwork
		{
			get
			{
				if (this._BindSocketToWiFiNetwork == null)
				{
					this._BindSocketToWiFiNetwork = new bool?(this.AppSettings.GetValueOrDefault<bool>("BindSocketToWiFiNetwork", true, null));
				}
				return this._BindSocketToWiFiNetwork.Value;
			}
			set
			{
				if (value == this.BindSocketToFirstWiFiNetwork)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BindSocketToWiFiNetwork", value, null);
				this._BindSocketToWiFiNetwork = new bool?(value);
				this.NotifyPropertyChanged("BindSocketToWiFiNetwork");
			}
		}

		// Token: 0x17000FD6 RID: 4054
		// (get) Token: 0x06001C8D RID: 7309 RVA: 0x001462F1 File Offset: 0x001444F1
		// (set) Token: 0x06001C8E RID: 7310 RVA: 0x00146328 File Offset: 0x00144528
		[Backupable]
		public bool BindSocketToFirstWiFiNetwork
		{
			get
			{
				if (this._BindSocketToFirstWiFiNetwork == null)
				{
					this._BindSocketToFirstWiFiNetwork = new bool?(this.AppSettings.GetValueOrDefault<bool>("BindSocketToFirstWiFiNetwork", true, null));
				}
				return this._BindSocketToFirstWiFiNetwork.Value;
			}
			set
			{
				if (value == this.BindSocketToFirstWiFiNetwork)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BindSocketToFirstWiFiNetwork", value, null);
				this._BindSocketToFirstWiFiNetwork = new bool?(value);
				this.NotifyPropertyChanged("BindSocketToFirstWiFiNetwork");
			}
		}

		// Token: 0x17000FD7 RID: 4055
		// (get) Token: 0x06001C8F RID: 7311 RVA: 0x0014635D File Offset: 0x0014455D
		// (set) Token: 0x06001C90 RID: 7312 RVA: 0x00146394 File Offset: 0x00144594
		[Backupable]
		public bool CheckBluetoothTurnedOn
		{
			get
			{
				if (this._CheckBluetoothTurnedOn == null)
				{
					this._CheckBluetoothTurnedOn = new bool?(this.AppSettings.GetValueOrDefault<bool>("CheckBluetoothTurnedOn", true, null));
				}
				return this._CheckBluetoothTurnedOn.Value;
			}
			set
			{
				if (value == this.CheckBluetoothTurnedOn)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CheckBluetoothTurnedOn", value, null);
				this._CheckBluetoothTurnedOn = new bool?(value);
				this.NotifyPropertyChanged("CheckBluetoothTurnedOn");
			}
		}

		// Token: 0x17000FD8 RID: 4056
		// (get) Token: 0x06001C91 RID: 7313 RVA: 0x001463C9 File Offset: 0x001445C9
		// (set) Token: 0x06001C92 RID: 7314 RVA: 0x00146400 File Offset: 0x00144600
		public bool TurnOffBluetoothIfWasTurnedOn
		{
			get
			{
				if (this._TurnOffBluetoothIfWasTurnedOn == null)
				{
					this._TurnOffBluetoothIfWasTurnedOn = new bool?(this.AppSettings.GetValueOrDefault<bool>("TurnOffBluetoothIfWasTurnedOn", false, null));
				}
				return this._TurnOffBluetoothIfWasTurnedOn.Value;
			}
			set
			{
				if (value == this.TurnOffBluetoothIfWasTurnedOn)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("TurnOffBluetoothIfWasTurnedOn", value, null);
				this._TurnOffBluetoothIfWasTurnedOn = new bool?(value);
				this.NotifyPropertyChanged("TurnOffBluetoothIfWasTurnedOn");
			}
		}

		// Token: 0x17000FD9 RID: 4057
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x00146438 File Offset: 0x00144638
		// (set) Token: 0x06001C94 RID: 7316 RVA: 0x0014647C File Offset: 0x0014467C
		public AndroidWiFiConnectionModes AndroidWiFiMode
		{
			get
			{
				if (this._AndroidWiFiMode == null)
				{
					int valueOrDefault = this.AppSettings.GetValueOrDefault<int>("AndroidWiFiMode", 0, null);
					this._AndroidWiFiMode = new AndroidWiFiConnectionModes?((AndroidWiFiConnectionModes)valueOrDefault);
				}
				return this._AndroidWiFiMode.Value;
			}
			set
			{
				if (value == this.AndroidWiFiMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidWiFiMode", (int)value, null);
				this._AndroidWiFiMode = new AndroidWiFiConnectionModes?(value);
				this.NotifyPropertyChanged("AndroidWiFiMode");
			}
		}

		// Token: 0x17000FDA RID: 4058
		// (get) Token: 0x06001C95 RID: 7317 RVA: 0x001464B1 File Offset: 0x001446B1
		// (set) Token: 0x06001C96 RID: 7318 RVA: 0x001464E8 File Offset: 0x001446E8
		[Backupable]
		public bool RequestECUInfo
		{
			get
			{
				if (this._RequestECUInfo == null)
				{
					this._RequestECUInfo = new bool?(this.AppSettings.GetValueOrDefault<bool>("RequestECUInfo", true, null));
				}
				return this._RequestECUInfo.Value;
			}
			set
			{
				if (value == this.RequestECUInfo)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RequestECUInfo", value, null);
				this._RequestECUInfo = new bool?(value);
				this.NotifyPropertyChanged("RequestECUInfo");
			}
		}

		// Token: 0x17000FDB RID: 4059
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x0014651D File Offset: 0x0014471D
		// (set) Token: 0x06001C98 RID: 7320 RVA: 0x00146554 File Offset: 0x00144754
		[Backupable]
		public bool DroidTryTurnOnBluetooth
		{
			get
			{
				if (this._DroidTryTurnOnBluetooth == null)
				{
					this._DroidTryTurnOnBluetooth = new bool?(this.AppSettings.GetValueOrDefault<bool>("DroidTryTurnOnBluetooth", true, null));
				}
				return this._DroidTryTurnOnBluetooth.Value;
			}
			set
			{
				if (value == this.DroidTryTurnOnBluetooth)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DroidTryTurnOnBluetooth", value, null);
				this._DroidTryTurnOnBluetooth = new bool?(value);
				this.NotifyPropertyChanged("DroidTryTurnOnBluetooth");
			}
		}

		// Token: 0x17000FDC RID: 4060
		// (get) Token: 0x06001C99 RID: 7321 RVA: 0x00146589 File Offset: 0x00144789
		// (set) Token: 0x06001C9A RID: 7322 RVA: 0x001465C0 File Offset: 0x001447C0
		[Backupable]
		public bool DroidAskForBluetoothPermissions
		{
			get
			{
				if (this._DroidAskForBluetoothPermissions == null)
				{
					this._DroidAskForBluetoothPermissions = new bool?(this.AppSettings.GetValueOrDefault<bool>("DroidAskForBluetoothPermissions", true, null));
				}
				return this._DroidAskForBluetoothPermissions.Value;
			}
			set
			{
				if (value == this.DroidAskForBluetoothPermissions)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DroidAskForBluetoothPermissions", value, null);
				this._DroidAskForBluetoothPermissions = new bool?(value);
				this.NotifyPropertyChanged("DroidAskForBluetoothPermissions");
			}
		}

		// Token: 0x17000FDD RID: 4061
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x001465F5 File Offset: 0x001447F5
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x0014662C File Offset: 0x0014482C
		[JsonIgnore]
		public bool AndroidStartBackgroundService
		{
			get
			{
				if (this._AndroidStartBackgroundService == null)
				{
					this._AndroidStartBackgroundService = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidStartBackgroundService", true, null));
				}
				return this._AndroidStartBackgroundService.Value;
			}
			set
			{
				if (value == this.AndroidStartBackgroundService)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidStartBackgroundService", value, null);
				this._AndroidStartBackgroundService = new bool?(value);
				this.NotifyPropertyChanged("AndroidStartBackgroundService");
			}
		}

		// Token: 0x17000FDE RID: 4062
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x00146661 File Offset: 0x00144861
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x00146698 File Offset: 0x00144898
		public bool DroidDisplayConnectionStatusInStatusBar
		{
			get
			{
				if (this._DroidDisplayConnectionStatusInStatusBar == null)
				{
					this._DroidDisplayConnectionStatusInStatusBar = new bool?(this.AppSettings.GetValueOrDefault<bool>("DroidDisplayConnectionStatusInStatusBar", false, null));
				}
				return this._DroidDisplayConnectionStatusInStatusBar.Value;
			}
			set
			{
				if (value == this.DroidDisplayConnectionStatusInStatusBar)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DroidDisplayConnectionStatusInStatusBar", value, null);
				this._DroidDisplayConnectionStatusInStatusBar = new bool?(value);
				this.NotifyPropertyChanged("DroidDisplayConnectionStatusInStatusBar");
			}
		}

		// Token: 0x17000FDF RID: 4063
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x001466CD File Offset: 0x001448CD
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x00146704 File Offset: 0x00144904
		[Backupable]
		public bool SendDefaultFunctionalHeader
		{
			get
			{
				if (this._SendDefaultFunctionalHeader == null)
				{
					this._SendDefaultFunctionalHeader = new bool?(this.AppSettings.GetValueOrDefault<bool>("SendDefaultFunctionalHeader", true, null));
				}
				return this._SendDefaultFunctionalHeader.Value;
			}
			set
			{
				if (value == this.SendDefaultFunctionalHeader)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendDefaultFunctionalHeader", value, null);
				this._SendDefaultFunctionalHeader = new bool?(value);
				this.NotifyPropertyChanged("SendDefaultFunctionalHeader");
			}
		}

		// Token: 0x17000FE0 RID: 4064
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x00146739 File Offset: 0x00144939
		// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x00146770 File Offset: 0x00144970
		[JsonIgnore]
		public int AndroidBluetoothConnectionMethod
		{
			get
			{
				if (this._AndroidBluetoothConnectionMethod == null)
				{
					this._AndroidBluetoothConnectionMethod = new int?(this.AppSettings.GetValueOrDefault<int>("AndroidBluetoothConnectionMethod", 0, null));
				}
				return this._AndroidBluetoothConnectionMethod.Value;
			}
			set
			{
				if (value == this.AndroidBluetoothConnectionMethod)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidBluetoothConnectionMethod", value, null);
				this._AndroidBluetoothConnectionMethod = new int?(value);
				this.NotifyPropertyChanged("AndroidBluetoothConnectionMethod");
			}
		}

		// Token: 0x17000FE1 RID: 4065
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x001467A5 File Offset: 0x001449A5
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x001467DC File Offset: 0x001449DC
		[JsonIgnore]
		[Backupable]
		public bool NoDelayELM327Init
		{
			get
			{
				if (this._NoDelayELM327Init == null)
				{
					this._NoDelayELM327Init = new bool?(this.AppSettings.GetValueOrDefault<bool>("NoDelayELM327Init", false, null));
				}
				return this._NoDelayELM327Init.Value;
			}
			set
			{
				if (value == this.NoDelayELM327Init)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("NoDelayELM327Init", value, null);
				this._NoDelayELM327Init = new bool?(value);
				this.NotifyPropertyChanged("NoDelayELM327Init");
			}
		}

		// Token: 0x17000FE2 RID: 4066
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x00146811 File Offset: 0x00144A11
		// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x00146848 File Offset: 0x00144A48
		public bool ToyotaJDMMode21
		{
			get
			{
				if (this._ToyotaJDMMode21 == null)
				{
					this._ToyotaJDMMode21 = new bool?(this.AppSettings.GetValueOrDefault<bool>("ToyotaJDMMode21", false, null));
				}
				return this._ToyotaJDMMode21.Value;
			}
			set
			{
				if (value == this.ToyotaJDMMode21)
				{
					return;
				}
				if (value)
				{
					this.Mode01Prefix = "21";
				}
				else
				{
					this.Mode01Prefix = "01";
				}
				this.AppSettings.AddOrUpdateValue("ToyotaJDMMode21", value, null);
				this._ToyotaJDMMode21 = new bool?(value);
				this.NotifyPropertyChanged("ToyotaJDMMode21");
			}
		}

		// Token: 0x17000FE3 RID: 4067
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x001468A3 File Offset: 0x00144AA3
		// (set) Token: 0x06001CA8 RID: 7336 RVA: 0x001468DC File Offset: 0x00144ADC
		[Backupable]
		public bool DaihatsuKLine
		{
			get
			{
				if (this._DaihatsuKLine == null)
				{
					this._DaihatsuKLine = new bool?(this.AppSettings.GetValueOrDefault<bool>("DaihatsuKLine", false, null));
				}
				return this._DaihatsuKLine.Value;
			}
			set
			{
				if (value != this.DaihatsuKLine)
				{
					this.AppSettings.AddOrUpdateValue("DaihatsuKLine", value, null);
					this._DaihatsuKLine = new bool?(value);
					this.NotifyPropertyChanged("DaihatsuKLine");
					OBDDataReader obdreader = App.OBDReader;
					if (obdreader == null)
					{
						return;
					}
					CarData currentCarData = obdreader.CurrentCarData;
					if (currentCarData == null)
					{
						return;
					}
					currentCarData.InitializeCalculatedPIDs();
				}
			}
		}

		// Token: 0x17000FE4 RID: 4068
		// (get) Token: 0x06001CA9 RID: 7337 RVA: 0x00146934 File Offset: 0x00144B34
		// (set) Token: 0x06001CAA RID: 7338 RVA: 0x00146960 File Offset: 0x00144B60
		public string BTDeviceID
		{
			get
			{
				if (this._BTDeviceID == null)
				{
					this._BTDeviceID = this.AppSettings.GetValueOrDefault<string>("BTDeviceID", "", null);
				}
				return this._BTDeviceID;
			}
			set
			{
				if (value == this.BTDeviceID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTDeviceID", value, null);
				this._BTDeviceID = value;
				this.NotifyPropertyChanged("BTDeviceID");
			}
		}

		// Token: 0x17000FE5 RID: 4069
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x00146995 File Offset: 0x00144B95
		// (set) Token: 0x06001CAC RID: 7340 RVA: 0x001469C1 File Offset: 0x00144BC1
		public string BTDeviceName
		{
			get
			{
				if (this._BTDeviceName == null)
				{
					this._BTDeviceName = this.AppSettings.GetValueOrDefault<string>("BTDeviceName", "", null);
				}
				return this._BTDeviceName;
			}
			set
			{
				if (value == this.BTDeviceName)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTDeviceName", value, null);
				this._BTDeviceName = value;
				this.NotifyPropertyChanged("BTDeviceName");
			}
		}

		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06001CAD RID: 7341 RVA: 0x001469F6 File Offset: 0x00144BF6
		// (set) Token: 0x06001CAE RID: 7342 RVA: 0x00146A2D File Offset: 0x00144C2D
		[JsonIgnore]
		public int OptimizedRequestStuckCounter
		{
			get
			{
				if (this._OptimizedRequestStuckCounter == null)
				{
					this._OptimizedRequestStuckCounter = new int?(this.AppSettings.GetValueOrDefault<int>("OptimizedRequestStuckCounter", 0, null));
				}
				return this._OptimizedRequestStuckCounter.Value;
			}
			set
			{
				if (value == this.OptimizedRequestStuckCounter)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("OptimizedRequestStuckCounter", value, null);
				this._OptimizedRequestStuckCounter = new int?(value);
				this.NotifyPropertyChanged("OptimizedRequestStuckCounter");
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06001CAF RID: 7343 RVA: 0x00146A62 File Offset: 0x00144C62
		// (set) Token: 0x06001CB0 RID: 7344 RVA: 0x00146A99 File Offset: 0x00144C99
		[JsonIgnore]
		public bool CANOptimizationWarningShowed
		{
			get
			{
				if (this._CANOptimizationWarningShowed == null)
				{
					this._CANOptimizationWarningShowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("CANOptimizationWarningShowed", false, null));
				}
				return this._CANOptimizationWarningShowed.Value;
			}
			set
			{
				if (value == this.CANOptimizationWarningShowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizationWarningShowed", value, null);
				this._CANOptimizationWarningShowed = new bool?(value);
				this.NotifyPropertyChanged("CANOptimizationWarningShowed");
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06001CB1 RID: 7345 RVA: 0x00146ACE File Offset: 0x00144CCE
		// (set) Token: 0x06001CB2 RID: 7346 RVA: 0x00146B05 File Offset: 0x00144D05
		[Backupable]
		public int CANOptimizeMaxInRequest
		{
			get
			{
				if (this._CANOptimizeMaxInRequest == null)
				{
					this._CANOptimizeMaxInRequest = new int?(this.AppSettings.GetValueOrDefault<int>("CANOptimizeMaxInRequest", 6, null));
				}
				return this._CANOptimizeMaxInRequest.Value;
			}
			set
			{
				if (value == this.CANOptimizeMaxInRequest)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizeMaxInRequest", value, null);
				this._CANOptimizeMaxInRequest = new int?(value);
				this.NotifyPropertyChanged("CANOptimizeMaxInRequest");
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06001CB3 RID: 7347 RVA: 0x00146B3A File Offset: 0x00144D3A
		// (set) Token: 0x06001CB4 RID: 7348 RVA: 0x00146B71 File Offset: 0x00144D71
		public int CANOptimizeMode22MaxInRequest
		{
			get
			{
				if (this._CANOptimizeMode22MaxInRequest == null)
				{
					this._CANOptimizeMode22MaxInRequest = new int?(this.AppSettings.GetValueOrDefault<int>("CANOptimizeMode22MaxInRequest", 3, null));
				}
				return this._CANOptimizeMode22MaxInRequest.Value;
			}
			set
			{
				if (value == this.CANOptimizeMode22MaxInRequest)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizeMode22MaxInRequest", value, null);
				this._CANOptimizeMode22MaxInRequest = new int?(value);
				this.NotifyPropertyChanged("CANOptimizeMode22MaxInRequest");
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06001CB5 RID: 7349 RVA: 0x00146BA6 File Offset: 0x00144DA6
		// (set) Token: 0x06001CB6 RID: 7350 RVA: 0x00146BDD File Offset: 0x00144DDD
		[Backupable]
		public bool CANOptimizeRequests
		{
			get
			{
				if (this._CANOptimizeRequests == null)
				{
					this._CANOptimizeRequests = new bool?(this.AppSettings.GetValueOrDefault<bool>("CANOptimizeRequests", false, null));
				}
				return this._CANOptimizeRequests.Value;
			}
			set
			{
				if (value == this.CANOptimizeRequests)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizeRequests", value, null);
				this._CANOptimizeRequests = new bool?(value);
				this.NotifyPropertyChanged("CANOptimizeRequests");
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x00146C12 File Offset: 0x00144E12
		// (set) Token: 0x06001CB8 RID: 7352 RVA: 0x00146C49 File Offset: 0x00144E49
		public bool CANOptimizeMode22
		{
			get
			{
				if (this._CANOptimizeMode22 == null)
				{
					this._CANOptimizeMode22 = new bool?(this.AppSettings.GetValueOrDefault<bool>("CANOptimizeMode22", false, null));
				}
				return this._CANOptimizeMode22.Value;
			}
			set
			{
				if (value == this.CANOptimizeMode22)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizeMode22", value, null);
				this._CANOptimizeMode22 = new bool?(value);
				this.NotifyPropertyChanged("CANOptimizeMode22");
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x00146C7E File Offset: 0x00144E7E
		// (set) Token: 0x06001CBA RID: 7354 RVA: 0x00146CB5 File Offset: 0x00144EB5
		public bool CANOptimizeMode22SelfLearningMode
		{
			get
			{
				if (this._CANOptimizeMode22SelfLearningMode == null)
				{
					this._CANOptimizeMode22SelfLearningMode = new bool?(this.AppSettings.GetValueOrDefault<bool>("CANOptimizeMode22SelfLearningMode", true, null));
				}
				return this._CANOptimizeMode22SelfLearningMode.Value;
			}
			set
			{
				if (value == this.CANOptimizeMode22SelfLearningMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANOptimizeMode22SelfLearningMode", value, null);
				this._CANOptimizeMode22SelfLearningMode = new bool?(value);
				this.NotifyPropertyChanged("CANOptimizeMode22SelfLearningMode");
			}
		}

		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06001CBB RID: 7355 RVA: 0x00146CEC File Offset: 0x00144EEC
		// (set) Token: 0x06001CBC RID: 7356 RVA: 0x00146D44 File Offset: 0x00144F44
		public string CANOptimizeMode22DataLengthDictionary
		{
			get
			{
				string text;
				try
				{
					text = SharedSettings.DecodeViaBytes(this.AppSettings.GetValueOrDefault<string>("CANOptimizeMode22DataLengthDictionary", "", "pr.png"), (byte)"Car Scanner"[4]);
				}
				catch (Exception)
				{
					text = "";
				}
				return text;
			}
			set
			{
				string text = SharedSettings.EncodeViaBytes(value, (byte)"Car Scanner"[4]);
				this.AppSettings.AddOrUpdateValue("CANOptimizeMode22DataLengthDictionary", text, "pr.png");
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06001CBD RID: 7357 RVA: 0x00146D7C File Offset: 0x00144F7C
		// (set) Token: 0x06001CBE RID: 7358 RVA: 0x00146DC8 File Offset: 0x00144FC8
		[Backupable]
		public bool ShouldCheckProfilePIDs
		{
			get
			{
				if (this.IgnoreProfilePidsTestScheduled)
				{
					return false;
				}
				if (this._ShouldCheckProfilePIDs == null)
				{
					this._ShouldCheckProfilePIDs = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShouldCheckProfilePIDs", false, null));
				}
				return this._ShouldCheckProfilePIDs.Value;
			}
			set
			{
				if (value == this.ShouldCheckProfilePIDs)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShouldCheckProfilePIDs", value, null);
				this._ShouldCheckProfilePIDs = new bool?(value);
				this.NotifyPropertyChanged("ShouldCheckProfilePIDs");
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06001CBF RID: 7359 RVA: 0x00146E00 File Offset: 0x00145000
		// (set) Token: 0x06001CC0 RID: 7360 RVA: 0x00146E4C File Offset: 0x0014504C
		[Backupable]
		public bool PerformSensorsScanByTesting
		{
			get
			{
				if (!this.ShowExperimental)
				{
					return false;
				}
				if (this._PerformSensorsScanByTesting == null)
				{
					this._PerformSensorsScanByTesting = new bool?(this.AppSettings.GetValueOrDefault<bool>("PerformSensorsScanByTesting", false, null));
				}
				return this._PerformSensorsScanByTesting.Value;
			}
			set
			{
				if (value == this.PerformSensorsScanByTesting)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PerformSensorsScanByTesting", value, null);
				this._PerformSensorsScanByTesting = new bool?(value);
				this.NotifyPropertyChanged("PerformSensorsScanByTesting");
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06001CC1 RID: 7361 RVA: 0x00146E81 File Offset: 0x00145081
		// (set) Token: 0x06001CC2 RID: 7362 RVA: 0x00146EAD File Offset: 0x001450AD
		public string BTLEDeviceID
		{
			get
			{
				if (this._BTLEDeviceID == null)
				{
					this._BTLEDeviceID = this.AppSettings.GetValueOrDefault<string>("BTLEDeviceID", string.Empty, null);
				}
				return this._BTLEDeviceID;
			}
			set
			{
				if (value == this.BTLEDeviceID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEDeviceID", value, null);
				this._BTLEDeviceID = value;
				this.NotifyPropertyChanged("BTLEDeviceID");
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06001CC3 RID: 7363 RVA: 0x00146EE2 File Offset: 0x001450E2
		// (set) Token: 0x06001CC4 RID: 7364 RVA: 0x00146F0E File Offset: 0x0014510E
		public string BTLEDeviceName
		{
			get
			{
				if (this._BTLEDeviceName == null)
				{
					this._BTLEDeviceName = this.AppSettings.GetValueOrDefault<string>("BTLEDeviceName", string.Empty, null);
				}
				return this._BTLEDeviceName;
			}
			set
			{
				if (value == this.BTLEDeviceName)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEDeviceName", value, null);
				this._BTLEDeviceName = value;
				this.NotifyPropertyChanged("BTLEDeviceName");
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06001CC5 RID: 7365 RVA: 0x00146F43 File Offset: 0x00145143
		// (set) Token: 0x06001CC6 RID: 7366 RVA: 0x00146F6F File Offset: 0x0014516F
		public string BTLEServiceID
		{
			get
			{
				if (this._BTLEServiceID == null)
				{
					this._BTLEServiceID = this.AppSettings.GetValueOrDefault<string>("BTLEServiceID", string.Empty, null);
				}
				return this._BTLEServiceID;
			}
			set
			{
				if (value == this.BTLEServiceID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEServiceID", value, null);
				this._BTLEServiceID = value;
				this.NotifyPropertyChanged("BTLEServiceID");
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00146FA4 File Offset: 0x001451A4
		// (set) Token: 0x06001CC8 RID: 7368 RVA: 0x00146FD0 File Offset: 0x001451D0
		public string BTLEOutputID
		{
			get
			{
				if (this._BTLEOutputID == null)
				{
					this._BTLEOutputID = this.AppSettings.GetValueOrDefault<string>("BTLEOutputID", string.Empty, null);
				}
				return this._BTLEOutputID;
			}
			set
			{
				if (value == this.BTLEOutputID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEOutputID", value, null);
				this._BTLEOutputID = value;
				this.NotifyPropertyChanged("BTLEOutputID");
			}
		}

		// Token: 0x17000FF4 RID: 4084
		// (get) Token: 0x06001CC9 RID: 7369 RVA: 0x00147005 File Offset: 0x00145205
		// (set) Token: 0x06001CCA RID: 7370 RVA: 0x00147031 File Offset: 0x00145231
		public string BTLEInputID
		{
			get
			{
				if (this._BTLEInputID == null)
				{
					this._BTLEInputID = this.AppSettings.GetValueOrDefault<string>("BTLEInputID", string.Empty, null);
				}
				return this._BTLEInputID;
			}
			set
			{
				if (value == this.BTLEInputID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEInputID", value, null);
				this._BTLEInputID = value;
				this.NotifyPropertyChanged("BTLEInputID");
			}
		}

		// Token: 0x17000FF5 RID: 4085
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x00147068 File Offset: 0x00145268
		// (set) Token: 0x06001CCC RID: 7372 RVA: 0x001470D0 File Offset: 0x001452D0
		public ConnectionTypes ConnectionType
		{
			get
			{
				if (this._ConnectionType == null)
				{
					if (PlatformHelper.IsAndroid)
					{
						this._ConnectionType = new ConnectionTypes?((ConnectionTypes)this.AppSettings.GetValueOrDefault<int>("ConnectionType", 2, null));
					}
					else
					{
						this._ConnectionType = new ConnectionTypes?((ConnectionTypes)this.AppSettings.GetValueOrDefault<int>("ConnectionType", 0, null));
					}
				}
				return this._ConnectionType.Value;
			}
			set
			{
				if (value == this.ConnectionType)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ConnectionType", (int)value, null);
				this._ConnectionType = new ConnectionTypes?(value);
				this.NotifyPropertyChanged("ConnectionType");
				if (PlatformHelper.IsiOS)
				{
					try
					{
						if (value == ConnectionTypes.MFI_OBDLinkMXPlus && string.IsNullOrEmpty(this.BTDeviceID))
						{
							IBluetooth2Device bluetooth2Device = PlatformHelper.IOSService.MFIDevicesManager_FindFirstSupportedMFIDevice();
							if (bluetooth2Device != null)
							{
								this.BTDeviceID = bluetooth2Device.Id;
								this.BTDeviceName = bluetooth2Device.Name;
							}
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x17000FF6 RID: 4086
		// (get) Token: 0x06001CCD RID: 7373 RVA: 0x00147164 File Offset: 0x00145364
		// (set) Token: 0x06001CCE RID: 7374 RVA: 0x0014719B File Offset: 0x0014539B
		[Backupable]
		[JsonIgnore]
		public bool FirstConnectionAttempted
		{
			get
			{
				if (this._FirstConnectionAttempted == null)
				{
					this._FirstConnectionAttempted = new bool?(this.AppSettings.GetValueOrDefault<bool>("FirstConnectionAttempted", false, null));
				}
				return this._FirstConnectionAttempted.Value;
			}
			set
			{
				if (value == this.FirstConnectionAttempted)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FirstConnectionAttempted", value, null);
				this._FirstConnectionAttempted = new bool?(value);
				this.NotifyPropertyChanged("FirstConnectionAttempted");
			}
		}

		// Token: 0x17000FF7 RID: 4087
		// (get) Token: 0x06001CCF RID: 7375 RVA: 0x001471D0 File Offset: 0x001453D0
		// (set) Token: 0x06001CD0 RID: 7376 RVA: 0x00147207 File Offset: 0x00145407
		[Backupable]
		public bool DecodeMUT2Compatible
		{
			get
			{
				if (this._DecodeMUT2Compatible == null)
				{
					this._DecodeMUT2Compatible = new bool?(this.AppSettings.GetValueOrDefault<bool>("DecodeMUT2Compatible", true, null));
				}
				return this._DecodeMUT2Compatible.Value;
			}
			set
			{
				if (value == this.DecodeMUT2Compatible)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DecodeMUT2Compatible", value, null);
				this._DecodeMUT2Compatible = new bool?(value);
				this.NotifyPropertyChanged("DecodeMUT2Compatible");
			}
		}

		// Token: 0x17000FF8 RID: 4088
		// (get) Token: 0x06001CD1 RID: 7377 RVA: 0x0014723C File Offset: 0x0014543C
		// (set) Token: 0x06001CD2 RID: 7378 RVA: 0x00147273 File Offset: 0x00145473
		[JsonIgnore]
		public int FreePeriodGoodConnections
		{
			get
			{
				if (this._FreePeriodGoodConnections == null)
				{
					this._FreePeriodGoodConnections = new int?(this.AppSettings.GetValueOrDefault<int>("FreePeriodGoodConnections", 0, null));
				}
				return this._FreePeriodGoodConnections.Value;
			}
			set
			{
				if (value == this.FreePeriodGoodConnections)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FreePeriodGoodConnections", value, null);
				this._FreePeriodGoodConnections = new int?(value);
				this.NotifyPropertyChanged("FreePeriodGoodConnections");
			}
		}

		// Token: 0x17000FF9 RID: 4089
		// (get) Token: 0x06001CD3 RID: 7379 RVA: 0x001472A8 File Offset: 0x001454A8
		// (set) Token: 0x06001CD4 RID: 7380 RVA: 0x001472DF File Offset: 0x001454DF
		[JsonIgnore]
		public int CodingsCounter
		{
			get
			{
				if (this._CodingsCounter == null)
				{
					this._CodingsCounter = new int?(this.AppSettings.GetValueOrDefault<int>("CodingsCounter", 0, null));
				}
				return this._CodingsCounter.Value;
			}
			set
			{
				if (value == this.CodingsCounter)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CodingsCounter", value, null);
				this._CodingsCounter = new int?(value);
				this.NotifyPropertyChanged("CodingsCounter");
			}
		}

		// Token: 0x17000FFA RID: 4090
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x00147314 File Offset: 0x00145514
		// (set) Token: 0x06001CD6 RID: 7382 RVA: 0x0014734B File Offset: 0x0014554B
		[JsonIgnore]
		public bool OpenDashboardOnLaunch
		{
			get
			{
				if (this._OpenDashboardOnLaunch == null)
				{
					this._OpenDashboardOnLaunch = new bool?(this.AppSettings.GetValueOrDefault<bool>("OpenDashboardOnLaunch", false, null));
				}
				return this._OpenDashboardOnLaunch.Value;
			}
			set
			{
				if (value == this.OpenDashboardOnLaunch)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("OpenDashboardOnLaunch", value, null);
				this._OpenDashboardOnLaunch = new bool?(value);
				this.NotifyPropertyChanged("OpenDashboardOnLaunch");
			}
		}

		// Token: 0x17000FFB RID: 4091
		// (get) Token: 0x06001CD7 RID: 7383 RVA: 0x00147380 File Offset: 0x00145580
		// (set) Token: 0x06001CD8 RID: 7384 RVA: 0x001473B7 File Offset: 0x001455B7
		[JsonIgnore]
		public bool ConnectOnLaunch
		{
			get
			{
				if (this._ConnectOnLaunch == null)
				{
					this._ConnectOnLaunch = new bool?(this.AppSettings.GetValueOrDefault<bool>("ConnectOnLaunch", false, null));
				}
				return this._ConnectOnLaunch.Value;
			}
			set
			{
				if (value == this.ConnectOnLaunch)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ConnectOnLaunch", value, null);
				this._ConnectOnLaunch = new bool?(value);
				this.NotifyPropertyChanged("ConnectOnLaunch");
			}
		}

		// Token: 0x17000FFC RID: 4092
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x001473EC File Offset: 0x001455EC
		// (set) Token: 0x06001CDA RID: 7386 RVA: 0x0014741D File Offset: 0x0014561D
		public string SelectedProfileName
		{
			get
			{
				if (this._SelectedProfileName == null)
				{
					this._SelectedProfileName = this.AppSettings.GetValueOrDefault<string>("SelectedProfileName", PID.GetResourceString("Profile_OBD2_Name"), null);
				}
				return this._SelectedProfileName;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("SelectedProfileName", value, null);
				this._SelectedProfileName = value;
				this.NotifyPropertyChanged("SelectedProfileName");
			}
		}

		// Token: 0x17000FFD RID: 4093
		// (get) Token: 0x06001CDB RID: 7387 RVA: 0x00147443 File Offset: 0x00145643
		// (set) Token: 0x06001CDC RID: 7388 RVA: 0x0014747A File Offset: 0x0014567A
		public bool UseDefaultInit
		{
			get
			{
				if (this._UseDefaultInit == null)
				{
					this._UseDefaultInit = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseDefaultInit", true, null));
				}
				return this._UseDefaultInit.Value;
			}
			set
			{
				if (value == this.UseDefaultInit)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseDefaultInit", value, null);
				this._UseDefaultInit = new bool?(value);
				this.NotifyPropertyChanged("UseDefaultInit");
			}
		}

		// Token: 0x17000FFE RID: 4094
		// (get) Token: 0x06001CDD RID: 7389 RVA: 0x001474B0 File Offset: 0x001456B0
		// (set) Token: 0x06001CDE RID: 7390 RVA: 0x00147503 File Offset: 0x00145703
		[JsonIgnore]
		public int IOTimeout
		{
			get
			{
				if (this._IOTimeout == null)
				{
					this._IOTimeout = new int?(this.AppSettings.GetValueOrDefault<int>("IOTimeout", 10, null));
				}
				if (this._IOTimeout.Value < 2)
				{
					return 2;
				}
				return this._IOTimeout.Value;
			}
			set
			{
				if (value == this.IOTimeout)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("IOTimeout", value, null);
				this._IOTimeout = new int?(value);
				this.NotifyPropertyChanged("IOTimeout");
			}
		}

		// Token: 0x17000FFF RID: 4095
		// (get) Token: 0x06001CDF RID: 7391 RVA: 0x00147538 File Offset: 0x00145738
		// (set) Token: 0x06001CE0 RID: 7392 RVA: 0x0014756F File Offset: 0x0014576F
		public bool UseOBD2
		{
			get
			{
				if (this._UseOBD2 == null)
				{
					this._UseOBD2 = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseOBD2", true, null));
				}
				return this._UseOBD2.Value;
			}
			set
			{
				if (value == this.UseOBD2)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseOBD2", value, null);
				this._UseOBD2 = new bool?(value);
				this.NotifyPropertyChanged("UseOBD2");
			}
		}

		// Token: 0x17001000 RID: 4096
		// (get) Token: 0x06001CE1 RID: 7393 RVA: 0x001475A4 File Offset: 0x001457A4
		// (set) Token: 0x06001CE2 RID: 7394 RVA: 0x001475D0 File Offset: 0x001457D0
		public string DefaultFunctionalHeader
		{
			get
			{
				if (this._DefaultFunctionalHeader == null)
				{
					this._DefaultFunctionalHeader = this.AppSettings.GetValueOrDefault<string>("DefaultFunctionalHeader", "", null);
				}
				return this._DefaultFunctionalHeader;
			}
			set
			{
				if (value == this.DefaultFunctionalHeader)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DefaultFunctionalHeader", value, null);
				this._DefaultFunctionalHeader = value;
				this.NotifyPropertyChanged("DefaultFunctionalHeader");
			}
		}

		// Token: 0x17001001 RID: 4097
		// (get) Token: 0x06001CE3 RID: 7395 RVA: 0x00147608 File Offset: 0x00145808
		// (set) Token: 0x06001CE4 RID: 7396 RVA: 0x00147658 File Offset: 0x00145858
		[JsonIgnore]
		public int SendDelay
		{
			get
			{
				if (this._SendDelay == null)
				{
					this._SendDelay = new int?(this.AppSettings.GetValueOrDefault<int>("SendDelay", PlatformHelper.IsAndroid ? 20 : 40, null));
				}
				return this._SendDelay.Value;
			}
			set
			{
				if (value == this.SendDelay)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendDelay", value, null);
				this._SendDelay = new int?(value);
				this.NotifyPropertyChanged("SendDelay");
				if (App.OBDReader != null)
				{
					App.OBDReader.SendDelay = value;
				}
			}
		}

		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x06001CE5 RID: 7397 RVA: 0x001476AA File Offset: 0x001458AA
		// (set) Token: 0x06001CE6 RID: 7398 RVA: 0x001476D6 File Offset: 0x001458D6
		public string TesterPresentCommand
		{
			get
			{
				if (this._TesterPresentCommand == null)
				{
					this._TesterPresentCommand = this.AppSettings.GetValueOrDefault<string>("TesterPresentCommand", "0100", null);
				}
				return this._TesterPresentCommand;
			}
			set
			{
				if (value == this.TesterPresentCommand)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("TesterPresentCommand", value, null);
				this._TesterPresentCommand = value;
				this.NotifyPropertyChanged("TesterPresentCommand");
			}
		}

		// Token: 0x17001003 RID: 4099
		// (get) Token: 0x06001CE7 RID: 7399 RVA: 0x0014770B File Offset: 0x0014590B
		// (set) Token: 0x06001CE8 RID: 7400 RVA: 0x00147742 File Offset: 0x00145942
		public bool AlwaysPingECU
		{
			get
			{
				if (this._AlwaysPingECU == null)
				{
					this._AlwaysPingECU = new bool?(this.AppSettings.GetValueOrDefault<bool>("AlwaysPingECU", false, null));
				}
				return this._AlwaysPingECU.Value;
			}
			set
			{
				if (value == this.AlwaysPingECU)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AlwaysPingECU", value, null);
				this._AlwaysPingECU = new bool?(value);
				this.NotifyPropertyChanged("AlwaysPingECU");
			}
		}

		// Token: 0x17001004 RID: 4100
		// (get) Token: 0x06001CE9 RID: 7401 RVA: 0x00147777 File Offset: 0x00145977
		// (set) Token: 0x06001CEA RID: 7402 RVA: 0x001477AE File Offset: 0x001459AE
		public int ProtocolNumber
		{
			get
			{
				if (this._ProtocolNumber == null)
				{
					this._ProtocolNumber = new int?(this.AppSettings.GetValueOrDefault<int>("ProtocolNumber", 0, null));
				}
				return this._ProtocolNumber.Value;
			}
			set
			{
				if (value == this.ProtocolNumber)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ProtocolNumber", value, null);
				this._ProtocolNumber = new int?(value);
				this.NotifyPropertyChanged("ProtocolNumber");
			}
		}

		// Token: 0x17001005 RID: 4101
		// (get) Token: 0x06001CEB RID: 7403 RVA: 0x001477E3 File Offset: 0x001459E3
		// (set) Token: 0x06001CEC RID: 7404 RVA: 0x0014780F File Offset: 0x00145A0F
		public string CustomInitString
		{
			get
			{
				if (this._CustomInitString == null)
				{
					this._CustomInitString = this.AppSettings.GetValueOrDefault<string>("CustomInitString", "ATZ\nATE0\nATH1\nATSP0\nATS0\nATM0\nATAT1\nATST64", null);
				}
				return this._CustomInitString;
			}
			set
			{
				if (value == this.CustomInitString)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CustomInitString", value, null);
				this._CustomInitString = value;
				this.NotifyPropertyChanged("CustomInitString");
			}
		}

		// Token: 0x17001006 RID: 4102
		// (get) Token: 0x06001CED RID: 7405 RVA: 0x00147844 File Offset: 0x00145A44
		// (set) Token: 0x06001CEE RID: 7406 RVA: 0x00147870 File Offset: 0x00145A70
		public string DetectECUConnectionPID
		{
			get
			{
				if (this._DetectECUConnectionPID == null)
				{
					this._DetectECUConnectionPID = this.AppSettings.GetValueOrDefault<string>("DetectECUConnectionPID", "0100", null);
				}
				return this._DetectECUConnectionPID;
			}
			set
			{
				if (value == this.DetectECUConnectionPID)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DetectECUConnectionPID", value, null);
				this._DetectECUConnectionPID = value;
				this.NotifyPropertyChanged("DetectECUConnectionPID");
			}
		}

		// Token: 0x17001007 RID: 4103
		// (get) Token: 0x06001CEF RID: 7407 RVA: 0x001478A5 File Offset: 0x00145AA5
		// (set) Token: 0x06001CF0 RID: 7408 RVA: 0x001478DC File Offset: 0x00145ADC
		public int ATSTIdx
		{
			get
			{
				if (this._ATSTIdx == null)
				{
					this._ATSTIdx = new int?(this.AppSettings.GetValueOrDefault<int>("ATSTIdx", 6, null));
				}
				return this._ATSTIdx.Value;
			}
			set
			{
				if (value == this.ATSTIdx)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ATSTIdx", value, null);
				this._ATSTIdx = new int?(value);
				this.NotifyPropertyChanged("ATSTIdx");
			}
		}

		// Token: 0x06001CF1 RID: 7409 RVA: 0x00147914 File Offset: 0x00145B14
		public string GetATST()
		{
			switch (this.ATSTIdx)
			{
			case 0:
				return "";
			case 1:
				return "00";
			case 2:
				return "08";
			case 3:
				return "16";
			case 4:
				return "32";
			case 5:
				return "48";
			case 6:
				return "64";
			case 7:
				return "96";
			case 8:
				return "FF";
			default:
				return "";
			}
		}

		// Token: 0x17001008 RID: 4104
		// (get) Token: 0x06001CF2 RID: 7410 RVA: 0x0014798F File Offset: 0x00145B8F
		// (set) Token: 0x06001CF3 RID: 7411 RVA: 0x001479C6 File Offset: 0x00145BC6
		public int AdaptiveTimings
		{
			get
			{
				if (this._AdaptiveTimings == null)
				{
					this._AdaptiveTimings = new int?(this.AppSettings.GetValueOrDefault<int>("AdaptiveTimings", 1, null));
				}
				return this._AdaptiveTimings.Value;
			}
			set
			{
				if (value == this.AdaptiveTimings)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AdaptiveTimings", value, null);
				this._AdaptiveTimings = new int?(value);
				this.NotifyPropertyChanged("AdaptiveTimings");
			}
		}

		// Token: 0x17001009 RID: 4105
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x001479FC File Offset: 0x00145BFC
		// (set) Token: 0x06001CF5 RID: 7413 RVA: 0x00147A40 File Offset: 0x00145C40
		public DTCReadingMode DTCMode
		{
			get
			{
				if (this._DTCMode == null)
				{
					int valueOrDefault = this.AppSettings.GetValueOrDefault<int>("DTCMode", 0, null);
					this._DTCMode = new DTCReadingMode?((DTCReadingMode)valueOrDefault);
				}
				return this._DTCMode.Value;
			}
			set
			{
				if (value == this.DTCMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DTCMode", (int)value, null);
				this._DTCMode = new DTCReadingMode?(value);
				this.NotifyPropertyChanged("DTCMode");
			}
		}

		// Token: 0x1700100A RID: 4106
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x00147A75 File Offset: 0x00145C75
		// (set) Token: 0x06001CF7 RID: 7415 RVA: 0x00147AA1 File Offset: 0x00145CA1
		public string WiFiServer
		{
			get
			{
				if (this._WiFiServer == null)
				{
					this._WiFiServer = this.AppSettings.GetValueOrDefault<string>("WiFiServer", "192.168.0.10", null);
				}
				return this._WiFiServer;
			}
			set
			{
				if (value == this.WiFiServer)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("WiFiServer", value, null);
				this._WiFiServer = value;
				this.NotifyPropertyChanged("WiFiServer");
			}
		}

		// Token: 0x1700100B RID: 4107
		// (get) Token: 0x06001CF8 RID: 7416 RVA: 0x00147AD6 File Offset: 0x00145CD6
		// (set) Token: 0x06001CF9 RID: 7417 RVA: 0x00147B04 File Offset: 0x00145D04
		public string WiFiPort
		{
			get
			{
				if (this._WiFiPort == null)
				{
					this._WiFiPort = this.AppSettings.GetValueOrDefault<string>("WiFiPort", "35000", null);
				}
				return this._WiFiPort;
			}
			set
			{
				if (value == this.WiFiPort)
				{
					return;
				}
				int num;
				if (string.IsNullOrEmpty(value))
				{
					this.AppSettings.AddOrUpdateValue("WiFiPort", "0", null);
					this._WiFiPort = "0";
				}
				else if (int.TryParse(value, out num))
				{
					this.AppSettings.AddOrUpdateValue("WiFiPort", value, null);
					this._WiFiPort = num.ToString(CultureInfo.InvariantCulture);
				}
				this.NotifyPropertyChanged("WiFiPort");
			}
		}

		// Token: 0x1700100C RID: 4108
		// (get) Token: 0x06001CFA RID: 7418 RVA: 0x00147B84 File Offset: 0x00145D84
		// (set) Token: 0x06001CFB RID: 7419 RVA: 0x00147BBB File Offset: 0x00145DBB
		[JsonIgnore]
		public bool ShowBadELMWarning
		{
			get
			{
				if (this._ShowBadELMWarning == null)
				{
					this._ShowBadELMWarning = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowBadELMWarning", false, null));
				}
				return this._ShowBadELMWarning.Value;
			}
			set
			{
				if (value == this.ShowBadELMWarning)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowBadELMWarning", value, null);
				this._ShowBadELMWarning = new bool?(value);
				this.NotifyPropertyChanged("ShowBadELMWarning");
			}
		}

		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06001CFC RID: 7420 RVA: 0x00147BF0 File Offset: 0x00145DF0
		// (set) Token: 0x06001CFD RID: 7421 RVA: 0x00147C3D File Offset: 0x00145E3D
		[JsonIgnore]
		public FuelConsumptionUnits FuelConsumptionUnit
		{
			get
			{
				if (this._FuelConsumptionUnit == null)
				{
					this._FuelConsumptionUnit = new FuelConsumptionUnits?((FuelConsumptionUnits)this.AppSettings.GetValueOrDefault<int>("FuelConsumptionUnit", this.UseL100ForFuel ? 0 : 2, null));
				}
				return this._FuelConsumptionUnit.Value;
			}
			set
			{
				if (value == this.FuelConsumptionUnit)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelConsumptionUnit", (int)value, null);
				this._FuelConsumptionUnit = new FuelConsumptionUnits?(value);
				this.NotifyPropertyChanged("FuelConsumptionUnit");
				this.NotifyPropertyChanged("ShowUSGallonSelector");
			}
		}

		// Token: 0x1700100E RID: 4110
		// (get) Token: 0x06001CFE RID: 7422 RVA: 0x00147C7D File Offset: 0x00145E7D
		// (set) Token: 0x06001CFF RID: 7423 RVA: 0x00147CB9 File Offset: 0x00145EB9
		[JsonIgnore]
		public bool UseLitersForVolume
		{
			get
			{
				if (this._UseLitersForVolume == null)
				{
					this._UseLitersForVolume = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseLitersForVolume", this.UseL100ForFuel, null));
				}
				return this._UseLitersForVolume.Value;
			}
			set
			{
				if (value == this.UseLitersForVolume)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseLitersForVolume", value, null);
				this._UseLitersForVolume = new bool?(value);
				this.NotifyPropertyChanged("UseLitersForVolume");
				this.NotifyPropertyChanged("ShowUSGallonSelector");
			}
		}

		// Token: 0x1700100F RID: 4111
		// (get) Token: 0x06001D00 RID: 7424 RVA: 0x00147CF9 File Offset: 0x00145EF9
		[JsonIgnore]
		public bool ShowUSGallonSelector
		{
			get
			{
				return this.FuelConsumptionUnit == FuelConsumptionUnits.MilesPerGallon || !this.UseLitersForVolume;
			}
		}

		// Token: 0x17001010 RID: 4112
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x00147D0F File Offset: 0x00145F0F
		// (set) Token: 0x06001D02 RID: 7426 RVA: 0x00147D4B File Offset: 0x00145F4B
		[JsonIgnore]
		public bool UseL100ForFuel
		{
			get
			{
				if (this._UseL100ForFuel == null)
				{
					this._UseL100ForFuel = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseL100ForFuel", this.Use_km, null));
				}
				return this._UseL100ForFuel.Value;
			}
			set
			{
				if (value == this.UseL100ForFuel)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseL100ForFuel", value, null);
				this._UseL100ForFuel = new bool?(value);
				this.NotifyPropertyChanged("UseL100ForFuel");
			}
		}

		// Token: 0x17001011 RID: 4113
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x00147D80 File Offset: 0x00145F80
		// (set) Token: 0x06001D04 RID: 7428 RVA: 0x00147DB7 File Offset: 0x00145FB7
		[JsonIgnore]
		public bool UseHoursePower
		{
			get
			{
				if (this._UseHoursePower == null)
				{
					this._UseHoursePower = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseHoursePower", true, null));
				}
				return this._UseHoursePower.Value;
			}
			set
			{
				if (value == this.UseHoursePower)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseHoursePower", value, null);
				this._UseHoursePower = new bool?(value);
				this.NotifyPropertyChanged("UseHoursePower");
			}
		}

		// Token: 0x17001012 RID: 4114
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x00147DEC File Offset: 0x00145FEC
		// (set) Token: 0x06001D06 RID: 7430 RVA: 0x00147E23 File Offset: 0x00146023
		[JsonIgnore]
		public bool UseNmForTorque
		{
			get
			{
				if (this._UseNmForTorque == null)
				{
					this._UseNmForTorque = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseNmForTorque", true, null));
				}
				return this._UseNmForTorque.Value;
			}
			set
			{
				if (value == this.UseNmForTorque)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseNmForTorque", value, null);
				this._UseNmForTorque = new bool?(value);
				this.NotifyPropertyChanged("UseNmForTorque");
			}
		}

		// Token: 0x17001013 RID: 4115
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x00147E58 File Offset: 0x00146058
		// (set) Token: 0x06001D08 RID: 7432 RVA: 0x00147E8F File Offset: 0x0014608F
		[JsonIgnore]
		public bool AccelerationUseG
		{
			get
			{
				if (this._AccelerationUseG == null)
				{
					this._AccelerationUseG = new bool?(this.AppSettings.GetValueOrDefault<bool>("AccelerationUseG", true, null));
				}
				return this._AccelerationUseG.Value;
			}
			set
			{
				if (value == this.AccelerationUseG)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AccelerationUseG", value, null);
				this._AccelerationUseG = new bool?(value);
				this.NotifyPropertyChanged("AccelerationUseG");
			}
		}

		// Token: 0x17001014 RID: 4116
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x00147EC4 File Offset: 0x001460C4
		// (set) Token: 0x06001D0A RID: 7434 RVA: 0x00147EFB File Offset: 0x001460FB
		[JsonIgnore]
		public bool Use_km
		{
			get
			{
				if (this._Use_km == null)
				{
					this._Use_km = new bool?(this.AppSettings.GetValueOrDefault<bool>("Use_km", true, null));
				}
				return this._Use_km.Value;
			}
			set
			{
				if (value == this.Use_km)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Use_km", value, null);
				this._Use_km = new bool?(value);
				this.NotifyPropertyChanged("Use_km");
			}
		}

		// Token: 0x17001015 RID: 4117
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x00147F30 File Offset: 0x00146130
		// (set) Token: 0x06001D0C RID: 7436 RVA: 0x00147F67 File Offset: 0x00146167
		[JsonIgnore]
		public bool Pressure_use_kpa
		{
			get
			{
				if (this._Pressure_use_kpa == null)
				{
					this._Pressure_use_kpa = new bool?(this.AppSettings.GetValueOrDefault<bool>("Pressure_use_kpa", true, null));
				}
				return this._Pressure_use_kpa.Value;
			}
			set
			{
				if (value == this.Pressure_use_kpa)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Pressure_use_kpa", value, null);
				this._Pressure_use_kpa = new bool?(value);
				this.NotifyPropertyChanged("Pressure_use_kpa");
			}
		}

		// Token: 0x17001016 RID: 4118
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x00147F9C File Offset: 0x0014619C
		// (set) Token: 0x06001D0E RID: 7438 RVA: 0x00147FD3 File Offset: 0x001461D3
		[JsonIgnore]
		public bool Flow_use_grams_sec
		{
			get
			{
				if (this._Flow_use_grams_sec == null)
				{
					this._Flow_use_grams_sec = new bool?(this.AppSettings.GetValueOrDefault<bool>("Flow_use_grams_sec", true, null));
				}
				return this._Flow_use_grams_sec.Value;
			}
			set
			{
				if (value == this.Flow_use_grams_sec)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Flow_use_grams_sec", value, null);
				this._Flow_use_grams_sec = new bool?(value);
				this.NotifyPropertyChanged("Flow_use_grams_sec");
			}
		}

		// Token: 0x17001017 RID: 4119
		// (get) Token: 0x06001D0F RID: 7439 RVA: 0x00148008 File Offset: 0x00146208
		// (set) Token: 0x06001D10 RID: 7440 RVA: 0x0014803F File Offset: 0x0014623F
		[JsonIgnore]
		public bool Use_celcium
		{
			get
			{
				if (this._Use_celcium == null)
				{
					this._Use_celcium = new bool?(this.AppSettings.GetValueOrDefault<bool>("Use_celcium", true, null));
				}
				return this._Use_celcium.Value;
			}
			set
			{
				if (value == this.Use_celcium)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Use_celcium", value, null);
				this._Use_celcium = new bool?(value);
				this.NotifyPropertyChanged("Use_celcium");
			}
		}

		// Token: 0x17001018 RID: 4120
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x00148074 File Offset: 0x00146274
		// (set) Token: 0x06001D12 RID: 7442 RVA: 0x001480AB File Offset: 0x001462AB
		[JsonIgnore]
		public bool UseUSGallon
		{
			get
			{
				if (this._UseUSGallon == null)
				{
					this._UseUSGallon = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseUSGallon", true, null));
				}
				return this._UseUSGallon.Value;
			}
			set
			{
				if (value == this.UseUSGallon)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseUSGallon", value, null);
				this._UseUSGallon = new bool?(value);
				this.NotifyPropertyChanged("UseUSGallon");
			}
		}

		// Token: 0x17001019 RID: 4121
		// (get) Token: 0x06001D13 RID: 7443 RVA: 0x001480E0 File Offset: 0x001462E0
		// (set) Token: 0x06001D14 RID: 7444 RVA: 0x00148117 File Offset: 0x00146317
		public bool ShowCodingAndService
		{
			get
			{
				if (this._ShowCodingAndService == null)
				{
					this._ShowCodingAndService = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowCodingAndService", true, null));
				}
				return this._ShowCodingAndService.Value;
			}
			set
			{
				if (value == this.ShowCodingAndService)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowCodingAndService", value, null);
				this._ShowCodingAndService = new bool?(value);
				this.NotifyPropertyChanged("ShowCodingAndService");
			}
		}

		// Token: 0x1700101A RID: 4122
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x0014814C File Offset: 0x0014634C
		// (set) Token: 0x06001D16 RID: 7446 RVA: 0x00148183 File Offset: 0x00146383
		public int AccelerationItems
		{
			get
			{
				if (this._AccelerationItems == null)
				{
					this._AccelerationItems = new int?(this.AppSettings.GetValueOrDefault<int>("AccelerationItems", 5, null));
				}
				return this._AccelerationItems.Value;
			}
			set
			{
				if (value == this.AccelerationItems)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AccelerationItems", value, null);
				this._AccelerationItems = new int?(value);
				this.NotifyPropertyChanged("AccelerationItems");
			}
		}

		// Token: 0x1700101B RID: 4123
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x001481B8 File Offset: 0x001463B8
		// (set) Token: 0x06001D18 RID: 7448 RVA: 0x00148204 File Offset: 0x00146404
		public bool SpeedPID2Bytes
		{
			get
			{
				if (!this.ShowExperimental)
				{
					return false;
				}
				if (this._SpeedPID2Bytes == null)
				{
					this._SpeedPID2Bytes = new bool?(this.AppSettings.GetValueOrDefault<bool>("SpeedPID2Bytes", false, null));
				}
				return this._SpeedPID2Bytes.Value;
			}
			set
			{
				if (value == this.SpeedPID2Bytes)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SpeedPID2Bytes", value, null);
				this._SpeedPID2Bytes = new bool?(value);
				OBDRequestQueueOptimizer.RefreshOBD2Dictionary();
				this.NotifyPropertyChanged("SpeedPID2Bytes");
			}
		}

		// Token: 0x1700101C RID: 4124
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x0014823E File Offset: 0x0014643E
		// (set) Token: 0x06001D1A RID: 7450 RVA: 0x00148275 File Offset: 0x00146475
		public bool KWPConcatResponseLines
		{
			get
			{
				if (this._KWPConcatResponseLines == null)
				{
					this._KWPConcatResponseLines = new bool?(this.AppSettings.GetValueOrDefault<bool>("KWPConcatResponseLines", false, null));
				}
				return this._KWPConcatResponseLines.Value;
			}
			set
			{
				if (value == this.KWPConcatResponseLines)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("KWPConcatResponseLines", value, null);
				this._KWPConcatResponseLines = new bool?(value);
				this.NotifyPropertyChanged("KWPConcatResponseLines");
			}
		}

		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06001D1B RID: 7451 RVA: 0x001482AA File Offset: 0x001464AA
		// (set) Token: 0x06001D1C RID: 7452 RVA: 0x001482E9 File Offset: 0x001464E9
		public double CustomFuelAF
		{
			get
			{
				if (this._CustomFuelAF == null)
				{
					this._CustomFuelAF = new double?(this.AppSettings.GetValueOrDefault<double>("CustomFuelAF", 14.7, null));
				}
				return this._CustomFuelAF.Value;
			}
			set
			{
				if (value != this.CustomFuelAF)
				{
					this.AppSettings.AddOrUpdateValue("CustomFuelAF", value, null);
					this._CustomFuelAF = new double?(value);
					this.NotifyPropertyChanged("CustomFuelAF");
				}
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06001D1D RID: 7453 RVA: 0x0014831D File Offset: 0x0014651D
		// (set) Token: 0x06001D1E RID: 7454 RVA: 0x0014835C File Offset: 0x0014655C
		public double CustomFuelDensity
		{
			get
			{
				if (this._CustomFuelDensity == null)
				{
					this._CustomFuelDensity = new double?(this.AppSettings.GetValueOrDefault<double>("CustomFuelDensity", 0.73, null));
				}
				return this._CustomFuelDensity.Value;
			}
			set
			{
				if (this.CustomFuelDensity != value)
				{
					this.AppSettings.AddOrUpdateValue("CustomFuelDensity", value, null);
					this._CustomFuelDensity = new double?(value);
					this.NotifyPropertyChanged("CustomFuelDensity");
				}
			}
		}

		// Token: 0x1700101F RID: 4127
		// (get) Token: 0x06001D1F RID: 7455 RVA: 0x00148390 File Offset: 0x00146590
		// (set) Token: 0x06001D20 RID: 7456 RVA: 0x001483C7 File Offset: 0x001465C7
		public bool FilterRPM300
		{
			get
			{
				if (this._FilterRPM300 == null)
				{
					this._FilterRPM300 = new bool?(this.AppSettings.GetValueOrDefault<bool>("FilterRPM300", false, null));
				}
				return this._FilterRPM300.Value;
			}
			set
			{
				if (value == this.FilterRPM300)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FilterRPM300", value, null);
				this._FilterRPM300 = new bool?(value);
				this.NotifyPropertyChanged("FilterRPM300");
			}
		}

		// Token: 0x17001020 RID: 4128
		// (get) Token: 0x06001D21 RID: 7457 RVA: 0x001483FC File Offset: 0x001465FC
		// (set) Token: 0x06001D22 RID: 7458 RVA: 0x00148480 File Offset: 0x00146680
		public double SpeedCorrectionFactor
		{
			get
			{
				if (this._SpeedCorrectionFactor == null)
				{
					try
					{
						this._SpeedCorrectionFactor = new double?(this.AppSettings.GetValueOrDefault<double>("SpeedCorrectionFactor", 1.0, null));
					}
					catch
					{
						this.AppSettings.Remove("SpeedCorrectionFactor");
						this._SpeedCorrectionFactor = new double?(1.0);
					}
				}
				return this._SpeedCorrectionFactor.Value;
			}
			set
			{
				if (value > 0.0)
				{
					double? speedCorrectionFactor = this._SpeedCorrectionFactor;
					if (!((value == speedCorrectionFactor.GetValueOrDefault()) & (speedCorrectionFactor != null)))
					{
						this.AppSettings.AddOrUpdateValue("SpeedCorrectionFactor", value, null);
						this._SpeedCorrectionFactor = new double?(value);
						this.NotifyPropertyChanged("SpeedCorrectionFactor");
					}
				}
			}
		}

		// Token: 0x17001021 RID: 4129
		// (get) Token: 0x06001D23 RID: 7459 RVA: 0x001484E0 File Offset: 0x001466E0
		// (set) Token: 0x06001D24 RID: 7460 RVA: 0x0014855A File Offset: 0x0014675A
		public bool IgnoreSupportedFlagForPIDs0166_0183
		{
			get
			{
				if (this.SelectedBrand == "Hyundai" || this.SelectedBrand == "Kia" || this.SelectedBrand == "Genesis")
				{
					return true;
				}
				if (this._IgnoreSupportedFlagForPIDs0166_0183 == null)
				{
					this._IgnoreSupportedFlagForPIDs0166_0183 = new bool?(this.AppSettings.GetValueOrDefault<bool>("IgnoreSupportedFlagForPIDs0166_0183", false, null));
				}
				return this._IgnoreSupportedFlagForPIDs0166_0183.Value;
			}
			set
			{
				if (value == this.IgnoreSupportedFlagForPIDs0166_0183)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("IgnoreSupportedFlagForPIDs0166_0183", value, null);
				this._IgnoreSupportedFlagForPIDs0166_0183 = new bool?(value);
				this.NotifyPropertyChanged("IgnoreSupportedFlagForPIDs0166_0183");
			}
		}

		// Token: 0x17001022 RID: 4130
		// (get) Token: 0x06001D25 RID: 7461 RVA: 0x0014858F File Offset: 0x0014678F
		// (set) Token: 0x06001D26 RID: 7462 RVA: 0x00148597 File Offset: 0x00146797
		public int BoostCalculationMethodIdx
		{
			get
			{
				return (int)this.BoostCalculationMethod;
			}
			set
			{
				this.BoostCalculationMethod = (BoostCalculationMethods)value;
			}
		}

		// Token: 0x17001023 RID: 4131
		// (get) Token: 0x06001D27 RID: 7463 RVA: 0x001485A0 File Offset: 0x001467A0
		// (set) Token: 0x06001D28 RID: 7464 RVA: 0x001485D8 File Offset: 0x001467D8
		public BoostCalculationMethods BoostCalculationMethod
		{
			get
			{
				if (this._BoostCalculationMethod == null)
				{
					this._BoostCalculationMethod = new BoostCalculationMethods?((BoostCalculationMethods)this.AppSettings.GetValueOrDefault<int>("BoostCalculationMethod", 0, null));
				}
				return this._BoostCalculationMethod.Value;
			}
			set
			{
				BoostCalculationMethods? boostCalculationMethod = this._BoostCalculationMethod;
				if (!((boostCalculationMethod.GetValueOrDefault() == value) & (boostCalculationMethod != null)))
				{
					this.AppSettings.AddOrUpdateValue("BoostCalculationMethod", (int)value, null);
					this._BoostCalculationMethod = new BoostCalculationMethods?(value);
					this.NotifyPropertyChanged("BoostCalculationMethod");
					this.NotifyPropertyChanged("BoostCalculationMethodIdx");
					App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
					SimpleMainPage.Instance.RefreshPIDs();
				}
			}
		}

		// Token: 0x17001024 RID: 4132
		// (get) Token: 0x06001D29 RID: 7465 RVA: 0x00148650 File Offset: 0x00146850
		// (set) Token: 0x06001D2A RID: 7466 RVA: 0x00148687 File Offset: 0x00146887
		public bool HideDTCWithUncomplitedTests
		{
			get
			{
				if (this._HideDTCWithUncomplitedTests == null)
				{
					this._HideDTCWithUncomplitedTests = new bool?(this.AppSettings.GetValueOrDefault<bool>("HideDTCWithUncomplitedTests", true, null));
				}
				return this._HideDTCWithUncomplitedTests.Value;
			}
			set
			{
				if (value == this.HideDTCWithUncomplitedTests)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("HideDTCWithUncomplitedTests", value, null);
				this._HideDTCWithUncomplitedTests = new bool?(value);
				this.NotifyPropertyChanged("HideDTCWithUncomplitedTests");
			}
		}

		// Token: 0x17001025 RID: 4133
		// (get) Token: 0x06001D2B RID: 7467 RVA: 0x001486BC File Offset: 0x001468BC
		// (set) Token: 0x06001D2C RID: 7468 RVA: 0x001486E8 File Offset: 0x001468E8
		public string SkipHeadersDTC
		{
			get
			{
				if (this._SkipHeadersDTC == null)
				{
					this._SkipHeadersDTC = this.AppSettings.GetValueOrDefault<string>("SkipHeadersDTC", "", null);
				}
				return this._SkipHeadersDTC;
			}
			set
			{
				if (value == this.SkipHeadersDTC)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SkipHeadersDTC", value, null);
				this._SkipHeadersDTC = value;
				this.NotifyPropertyChanged("SkipHeadersDTC");
			}
		}

		// Token: 0x17001026 RID: 4134
		// (get) Token: 0x06001D2D RID: 7469 RVA: 0x0014871D File Offset: 0x0014691D
		public bool IsCalibrationStartTimeSet
		{
			get
			{
				return !(this.CalibrationStartTime == DateTime.MinValue);
			}
		}

		// Token: 0x17001027 RID: 4135
		// (get) Token: 0x06001D2E RID: 7470 RVA: 0x00148734 File Offset: 0x00146934
		// (set) Token: 0x06001D2F RID: 7471 RVA: 0x00148786 File Offset: 0x00146986
		public DateTime CalibrationStartTime
		{
			get
			{
				if (this._CalibrationStartTime == null)
				{
					long valueOrDefault = this.AppSettings.GetValueOrDefault<long>("CalibrationStartTime", DateTime.MinValue.Ticks, null);
					this._CalibrationStartTime = new DateTime?(new DateTime(valueOrDefault));
				}
				return this._CalibrationStartTime.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("CalibrationStartTime", value.Ticks, null);
				this._CalibrationStartTime = new DateTime?(value);
				this.NotifyPropertyChanged("CalibrationStartTime");
				this.NotifyPropertyChanged("IsCalibrationStartTimeSet");
			}
		}

		// Token: 0x17001028 RID: 4136
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x001487C2 File Offset: 0x001469C2
		// (set) Token: 0x06001D31 RID: 7473 RVA: 0x00148801 File Offset: 0x00146A01
		public double CalibrationStartOdometer
		{
			get
			{
				if (this._CalibrationStartOdometer == null)
				{
					this._CalibrationStartOdometer = new double?(this.AppSettings.GetValueOrDefault<double>("CalibrationStartOdometer", 0.0, null));
				}
				return this._CalibrationStartOdometer.Value;
			}
			set
			{
				if (value == this.CalibrationStartOdometer)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CalibrationStartOdometer", value, null);
				this._CalibrationStartOdometer = new double?(value);
				this.NotifyPropertyChanged("CalibrationStartOdometer");
			}
		}

		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x00148836 File Offset: 0x00146A36
		// (set) Token: 0x06001D33 RID: 7475 RVA: 0x0014886D File Offset: 0x00146A6D
		public bool ZeroConsumptionWhenZero
		{
			get
			{
				if (this._ZeroConsumptionWhenZero == null)
				{
					this._ZeroConsumptionWhenZero = new bool?(this.AppSettings.GetValueOrDefault<bool>("ZeroConsumptionWhenZero", false, null));
				}
				return this._ZeroConsumptionWhenZero.Value;
			}
			set
			{
				if (value == this.ZeroConsumptionWhenZero)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ZeroConsumptionWhenZero", value, null);
				this._ZeroConsumptionWhenZero = new bool?(value);
				this.NotifyPropertyChanged("ZeroConsumptionWhenZero");
			}
		}

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06001D34 RID: 7476 RVA: 0x001488A2 File Offset: 0x00146AA2
		// (set) Token: 0x06001D35 RID: 7477 RVA: 0x001488DC File Offset: 0x00146ADC
		public int ZeroConsumptionPIDId
		{
			get
			{
				if (this._ZeroConsumptionPIDId == null)
				{
					this._ZeroConsumptionPIDId = new int?(this.AppSettings.GetValueOrDefault<int>("ZeroConsumptionPIDId", -1, null));
				}
				return this._ZeroConsumptionPIDId.Value;
			}
			set
			{
				if (this._ZeroConsumptionPIDId == null || value != this._ZeroConsumptionPIDId.Value)
				{
					this.AppSettings.AddOrUpdateValue("ZeroConsumptionPIDId", value, null);
					this._ZeroConsumptionPIDId = new int?(value);
					this.NotifyPropertyChanged("ZeroConsumptionPIDId");
				}
			}
		}

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06001D36 RID: 7478 RVA: 0x00148930 File Offset: 0x00146B30
		// (set) Token: 0x06001D37 RID: 7479 RVA: 0x00148A70 File Offset: 0x00146C70
		[JsonIgnore]
		public List<PID> ZeroConsumptionPIDCollection
		{
			get
			{
				if (this._ZeroConsumptionPIDCollection == null)
				{
					List<PID> list = new List<PID>();
					list.Add(PID.Empty);
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						list.AddRange(LiveDataPIDModel._PIDCollection.Where((PID x) => !(x is CustomPID) && x is IPIDFloatValue && ((x as IPIDFloatValue).Units == UnitsHelper.Units.None || (x as IPIDFloatValue).Units == UnitsHelper.Units.percent)));
					}
					else
					{
						list.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x is IPIDFloatValue && ((x as IPIDFloatValue).Units == UnitsHelper.Units.None || (x as IPIDFloatValue).Units == UnitsHelper.Units.percent)).ToArray<PID>());
					}
					list.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Units == UnitsHelper.Units.None || x.Units == UnitsHelper.Units.percent));
					list.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.Units == UnitsHelper.Units.None || x.Units == UnitsHelper.Units.percent));
					list.RemoveAll((PID x) => x is PID_EconomizerFSSandThrottlePosition);
					this._ZeroConsumptionPIDCollection = list;
				}
				return this._ZeroConsumptionPIDCollection;
			}
			set
			{
				this._ZeroConsumptionPIDCollection = value;
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06001D38 RID: 7480 RVA: 0x00148A79 File Offset: 0x00146C79
		// (set) Token: 0x06001D39 RID: 7481 RVA: 0x00148AB0 File Offset: 0x00146CB0
		public bool UseCustomPIDForZeroConsumption
		{
			get
			{
				if (this._UseCustomPIDForZeroConsumption == null)
				{
					this._UseCustomPIDForZeroConsumption = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseCustomPIDForZeroConsumption", false, null));
				}
				return this._UseCustomPIDForZeroConsumption.Value;
			}
			set
			{
				if (value == this.UseCustomPIDForZeroConsumption)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseCustomPIDForZeroConsumption", value, null);
				this._UseCustomPIDForZeroConsumption = new bool?(value);
				this.NotifyPropertyChanged("UseCustomPIDForZeroConsumption");
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06001D3A RID: 7482 RVA: 0x00148AE8 File Offset: 0x00146CE8
		// (set) Token: 0x06001D3B RID: 7483 RVA: 0x00148B34 File Offset: 0x00146D34
		[JsonIgnore]
		public decimal FuelPriceForLitre
		{
			get
			{
				if (this._FuelPriceForLitre == null)
				{
					this._FuelPriceForLitre = new decimal?(this.AppSettings.GetValueOrDefault<decimal>("FuelPriceForLitre", 1.01m, null));
				}
				return this._FuelPriceForLitre.Value;
			}
			set
			{
				if (value == this.FuelPriceForLitre)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelPriceForLitre", value, null);
				this._FuelPriceForLitre = new decimal?(value);
				this.NotifyPropertyChanged("FuelPriceForLitre");
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06001D3C RID: 7484 RVA: 0x00148B6E File Offset: 0x00146D6E
		// (set) Token: 0x06001D3D RID: 7485 RVA: 0x00148B9A File Offset: 0x00146D9A
		[JsonIgnore]
		public string Currency
		{
			get
			{
				if (this._Currency == null)
				{
					this._Currency = this.AppSettings.GetValueOrDefault<string>("Currency", "$", null);
				}
				return this._Currency;
			}
			set
			{
				if (value == this.Currency)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Currency", value, null);
				this._Currency = value;
				this.NotifyPropertyChanged("Currency");
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06001D3E RID: 7486 RVA: 0x00148BCF File Offset: 0x00146DCF
		// (set) Token: 0x06001D3F RID: 7487 RVA: 0x00148C06 File Offset: 0x00146E06
		[JsonIgnore]
		public bool FilterWrongAFValues
		{
			get
			{
				if (this._FilterWrongAFValues == null)
				{
					this._FilterWrongAFValues = new bool?(this.AppSettings.GetValueOrDefault<bool>("FilterWrongAFValues", true, null));
				}
				return this._FilterWrongAFValues.Value;
			}
			set
			{
				if (value == this.FilterWrongAFValues)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FilterWrongAFValues", value, null);
				this._FilterWrongAFValues = new bool?(value);
				this.NotifyPropertyChanged("FilterWrongAFValues");
			}
		}

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06001D40 RID: 7488 RVA: 0x00148C3B File Offset: 0x00146E3B
		// (set) Token: 0x06001D41 RID: 7489 RVA: 0x00148C74 File Offset: 0x00146E74
		[JsonIgnore]
		public bool AlwaysRecordFuelConsumption
		{
			get
			{
				if (this._AlwaysRecordFuelConsumption == null)
				{
					this._AlwaysRecordFuelConsumption = new bool?(this.AppSettings.GetValueOrDefault<bool>("AlwaysRecordFuelConsumption", false, null));
				}
				return this._AlwaysRecordFuelConsumption.Value;
			}
			set
			{
				if (value == this.AlwaysRecordFuelConsumption)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AlwaysRecordFuelConsumption", value, null);
				bool? alwaysRecordFuelConsumption = this._AlwaysRecordFuelConsumption;
				if (!((alwaysRecordFuelConsumption.GetValueOrDefault() == value) & (alwaysRecordFuelConsumption != null)))
				{
					this._AlwaysRecordFuelConsumption = new bool?(value);
					this.NotifyPropertyChanged("AlwaysRecordFuelConsumption");
				}
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06001D42 RID: 7490 RVA: 0x00148CD1 File Offset: 0x00146ED1
		// (set) Token: 0x06001D43 RID: 7491 RVA: 0x00148D09 File Offset: 0x00146F09
		public int MergeDriveCyclesTime
		{
			get
			{
				if (this._MergeDriveCyclesTime == null)
				{
					this._MergeDriveCyclesTime = new int?(this.AppSettings.GetValueOrDefault<int>("MergeDriveCyclesTime", 20, null));
				}
				return this._MergeDriveCyclesTime.Value;
			}
			set
			{
				if (value == this.MergeDriveCyclesTime)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("MergeDriveCyclesTime", value, null);
				this._MergeDriveCyclesTime = new int?(value);
				this.NotifyPropertyChanged("MergeDriveCyclesTime");
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06001D44 RID: 7492 RVA: 0x00148D40 File Offset: 0x00146F40
		[JsonIgnore]
		public List<PID> InjectorPIDCollection
		{
			get
			{
				List<PID> list = new List<PID>();
				list.Add(App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 122));
				list.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection.Where((CustomPID x) => x.Units == UnitsHelper.Units.ms));
				list.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.Units == UnitsHelper.Units.ms));
				if (SharedSettings.Current.ProtocolNumber == 11)
				{
					App.OBDReader.CurrentCarData.AddOrRemoveNissanConsultPIDsV2(true);
					list.AddRange(App.OBDReader.CurrentCarData.NissanConsultPIDs.Where((PID x) => x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.ms));
				}
				return list;
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x00148E4C File Offset: 0x0014704C
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x00148E88 File Offset: 0x00147088
		[JsonIgnore]
		public double CurbWeight
		{
			get
			{
				if (this._CurbWeight == null)
				{
					this._CurbWeight = new double?((double)this.AppSettings.GetValueOrDefault<int>("CurbWeight", 1300, null));
				}
				return this._CurbWeight.Value;
			}
			set
			{
				if (value == this.CurbWeight)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CurbWeight", value, null);
				this._CurbWeight = new double?(value);
				this.NotifyPropertyChanged("CurbWeight");
			}
		}

		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x00148EBD File Offset: 0x001470BD
		// (set) Token: 0x06001D48 RID: 7496 RVA: 0x00148EF5 File Offset: 0x001470F5
		[JsonIgnore]
		public double PassengersWeight
		{
			get
			{
				if (this._PassengersWeight == null)
				{
					this._PassengersWeight = new double?((double)this.AppSettings.GetValueOrDefault<int>("PassengersWeight", 0, null));
				}
				return this._PassengersWeight.Value;
			}
			set
			{
				if (value == this.PassengersWeight)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PassengersWeight", value, null);
				this._PassengersWeight = new double?(value);
				this.NotifyPropertyChanged("PassengersWeight");
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06001D49 RID: 7497 RVA: 0x00148F2A File Offset: 0x0014712A
		// (set) Token: 0x06001D4A RID: 7498 RVA: 0x00148F62 File Offset: 0x00147162
		[JsonIgnore]
		public double AdditionalWeight
		{
			get
			{
				if (this._AdditionalWeight == null)
				{
					this._AdditionalWeight = new double?((double)this.AppSettings.GetValueOrDefault<int>("AdditionalWeight", 0, null));
				}
				return this._AdditionalWeight.Value;
			}
			set
			{
				if (value == this.AdditionalWeight)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AdditionalWeight", value, null);
				this._AdditionalWeight = new double?(value);
				this.NotifyPropertyChanged("AdditionalWeight");
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x00148F97 File Offset: 0x00147197
		// (set) Token: 0x06001D4C RID: 7500 RVA: 0x00148FD6 File Offset: 0x001471D6
		[JsonIgnore]
		public double DragCoefficient
		{
			get
			{
				if (this._DragCoefficient == null)
				{
					this._DragCoefficient = new double?(this.AppSettings.GetValueOrDefault<double>("DragCoefficient", 0.35, null));
				}
				return this._DragCoefficient.Value;
			}
			set
			{
				if (value == this.DragCoefficient)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DragCoefficient", value, null);
				this._DragCoefficient = new double?(value);
				this.NotifyPropertyChanged("DragCoefficient");
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06001D4D RID: 7501 RVA: 0x0014900B File Offset: 0x0014720B
		// (set) Token: 0x06001D4E RID: 7502 RVA: 0x00149043 File Offset: 0x00147243
		[JsonIgnore]
		public double DragArea
		{
			get
			{
				if (this._DragArea == null)
				{
					this._DragArea = new double?((double)this.AppSettings.GetValueOrDefault<int>("DragArea", 2, null));
				}
				return this._DragArea.Value;
			}
			set
			{
				if (value == this.DragArea)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DragArea", value, null);
				this._DragArea = new double?(value);
				this.NotifyPropertyChanged("DragArea");
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x00149078 File Offset: 0x00147278
		// (set) Token: 0x06001D50 RID: 7504 RVA: 0x001490B7 File Offset: 0x001472B7
		[JsonIgnore]
		public double TireResistance
		{
			get
			{
				if (this._TireResistance == null)
				{
					this._TireResistance = new double?(this.AppSettings.GetValueOrDefault<double>("TireResistance", 0.012, null));
				}
				return this._TireResistance.Value;
			}
			set
			{
				if (value == this.TireResistance)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("TireResistance", value, null);
				this._TireResistance = new double?(value);
				this.NotifyPropertyChanged("TireResistance");
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06001D51 RID: 7505 RVA: 0x001490EC File Offset: 0x001472EC
		// (set) Token: 0x06001D52 RID: 7506 RVA: 0x00149125 File Offset: 0x00147325
		[JsonIgnore]
		public double AmbientTemperature
		{
			get
			{
				if (this._AmbientTemperature == null)
				{
					this._AmbientTemperature = new double?((double)this.AppSettings.GetValueOrDefault<int>("AmbientTemperature", 20, null));
				}
				return this._AmbientTemperature.Value;
			}
			set
			{
				if (value == this.AmbientTemperature)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AmbientTemperature", value, null);
				this._AmbientTemperature = new double?(value);
				this.NotifyPropertyChanged("AmbientTemperature");
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06001D53 RID: 7507 RVA: 0x0014915A File Offset: 0x0014735A
		// (set) Token: 0x06001D54 RID: 7508 RVA: 0x00149199 File Offset: 0x00147399
		[JsonIgnore]
		public double BarometricPressure
		{
			get
			{
				if (this._BarometricPressure == null)
				{
					this._BarometricPressure = new double?(this.AppSettings.GetValueOrDefault<double>("BarometricPressure", 101.325, null));
				}
				return this._BarometricPressure.Value;
			}
			set
			{
				if (value == this.BarometricPressure)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BarometricPressure", value, null);
				this._BarometricPressure = new double?(value);
				this.NotifyPropertyChanged("BarometricPressure");
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06001D55 RID: 7509 RVA: 0x001491CE File Offset: 0x001473CE
		// (set) Token: 0x06001D56 RID: 7510 RVA: 0x0014920D File Offset: 0x0014740D
		[JsonIgnore]
		public double FuelInTankVolume
		{
			get
			{
				if (this._FuelInTankVolume == null)
				{
					this._FuelInTankVolume = new double?(this.AppSettings.GetValueOrDefault<double>("FuelInTankVolume", 100.0, null));
				}
				return this._FuelInTankVolume.Value;
			}
			set
			{
				if (value == this.FuelInTankVolume)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelInTankVolume", value, null);
				this._FuelInTankVolume = new double?(value);
				this.NotifyPropertyChanged("FuelInTankVolume");
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06001D57 RID: 7511 RVA: 0x00149242 File Offset: 0x00147442
		// (set) Token: 0x06001D58 RID: 7512 RVA: 0x0014927B File Offset: 0x0014747B
		[JsonIgnore]
		public double DriverWeight
		{
			get
			{
				if (this._DriverWeight == null)
				{
					this._DriverWeight = new double?((double)this.AppSettings.GetValueOrDefault<int>("DriverWeight", 70, null));
				}
				return this._DriverWeight.Value;
			}
			set
			{
				if (value == this.DriverWeight)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DriverWeight", value, null);
				this._DriverWeight = new double?(value);
				this.NotifyPropertyChanged("DriverWeight");
			}
		}

		// Token: 0x1700103D RID: 4157
		// (get) Token: 0x06001D59 RID: 7513 RVA: 0x001492B0 File Offset: 0x001474B0
		// (set) Token: 0x06001D5A RID: 7514 RVA: 0x001492E7 File Offset: 0x001474E7
		[JsonIgnore]
		public bool ShowAirFuelBasedOnStoichiometric
		{
			get
			{
				if (this._ShowAirFuelBasedOnStoichiometric == null)
				{
					this._ShowAirFuelBasedOnStoichiometric = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowAirFuelBasedOnStoichiometric", true, null));
				}
				return this._ShowAirFuelBasedOnStoichiometric.Value;
			}
			set
			{
				if (value == this.ShowAirFuelBasedOnStoichiometric)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowAirFuelBasedOnStoichiometric", value, null);
				this._ShowAirFuelBasedOnStoichiometric = new bool?(value);
				this.NotifyPropertyChanged("ShowAirFuelBasedOnStoichiometric");
			}
		}

		// Token: 0x1700103E RID: 4158
		// (get) Token: 0x06001D5B RID: 7515 RVA: 0x0014931C File Offset: 0x0014751C
		// (set) Token: 0x06001D5C RID: 7516 RVA: 0x0014935B File Offset: 0x0014755B
		public double InjectorFlow
		{
			get
			{
				if (this._InjectorFlow == null)
				{
					this._InjectorFlow = new double?(this.AppSettings.GetValueOrDefault<double>("InjectorFlow", 250.0, null));
				}
				return this._InjectorFlow.Value;
			}
			set
			{
				if (value == this.InjectorFlow)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InjectorFlow", value, null);
				this._InjectorFlow = new double?(value);
				this.NotifyPropertyChanged("InjectorFlow");
			}
		}

		// Token: 0x1700103F RID: 4159
		// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00149390 File Offset: 0x00147590
		// (set) Token: 0x06001D5E RID: 7518 RVA: 0x001493CF File Offset: 0x001475CF
		public double FuelFlowCorrectionFactor
		{
			get
			{
				if (this._FuelFlowCorrectionFactor == null)
				{
					this._FuelFlowCorrectionFactor = new double?(this.AppSettings.GetValueOrDefault<double>("FuelFlowCorrectionFactor", 1.0, null));
				}
				return this._FuelFlowCorrectionFactor.Value;
			}
			set
			{
				if (value == this.FuelFlowCorrectionFactor)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelFlowCorrectionFactor", value, null);
				this._FuelFlowCorrectionFactor = new double?(value);
				this.NotifyPropertyChanged("FuelFlowCorrectionFactor");
			}
		}

		// Token: 0x17001040 RID: 4160
		// (get) Token: 0x06001D5F RID: 7519 RVA: 0x00149404 File Offset: 0x00147604
		// (set) Token: 0x06001D60 RID: 7520 RVA: 0x0014943C File Offset: 0x0014763C
		public int InjectorPid
		{
			get
			{
				if (this._InjectorPid == null)
				{
					this._InjectorPid = new int?(this.AppSettings.GetValueOrDefault<int>("InjectorPid", 122, null));
				}
				return this._InjectorPid.Value;
			}
			set
			{
				if (value == this.InjectorPid)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InjectorPid", value, null);
				this._InjectorPid = new int?(value);
				this.NotifyPropertyChanged("InjectorPid");
			}
		}

		// Token: 0x06001D61 RID: 7521 RVA: 0x00149474 File Offset: 0x00147674
		public PID GetInjectorPID()
		{
			if (this.InjectorPid == 122)
			{
				return App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 122);
			}
			CustomPID customPID = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Id == this.InjectorPid);
			if (customPID != null)
			{
				return customPID;
			}
			customPID = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == this.InjectorPid);
			if (customPID != null)
			{
				return customPID;
			}
			return App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 122);
		}

		// Token: 0x17001041 RID: 4161
		// (get) Token: 0x06001D62 RID: 7522 RVA: 0x00149534 File Offset: 0x00147734
		// (set) Token: 0x06001D63 RID: 7523 RVA: 0x0014953C File Offset: 0x0014773C
		public int FuelFlowCalculationSchemeIdx
		{
			get
			{
				return (int)this.FuelFlowCalculationScheme;
			}
			set
			{
				this.FuelFlowCalculationScheme = (FuelFlowCalculationSchemes)value;
			}
		}

		// Token: 0x17001042 RID: 4162
		// (get) Token: 0x06001D64 RID: 7524 RVA: 0x00149545 File Offset: 0x00147745
		// (set) Token: 0x06001D65 RID: 7525 RVA: 0x0014957C File Offset: 0x0014777C
		public bool FuelFlowUseFixedAFR
		{
			get
			{
				if (this._FuelFlowUseFixedAFR == null)
				{
					this._FuelFlowUseFixedAFR = new bool?(this.AppSettings.GetValueOrDefault<bool>("FuelFlowUseFixedAFR", false, null));
				}
				return this._FuelFlowUseFixedAFR.Value;
			}
			set
			{
				if (value == this.FuelFlowUseFixedAFR)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelFlowUseFixedAFR", value, null);
				this._FuelFlowUseFixedAFR = new bool?(value);
				this.NotifyPropertyChanged("FuelFlowUseFixedAFR");
			}
		}

		// Token: 0x17001043 RID: 4163
		// (get) Token: 0x06001D66 RID: 7526 RVA: 0x001495B1 File Offset: 0x001477B1
		// (set) Token: 0x06001D67 RID: 7527 RVA: 0x001495E8 File Offset: 0x001477E8
		public FuelFlowCalculationSchemes FuelFlowCalculationScheme
		{
			get
			{
				if (this._FuelFlowCalculationScheme == null)
				{
					this._FuelFlowCalculationScheme = new FuelFlowCalculationSchemes?((FuelFlowCalculationSchemes)this.AppSettings.GetValueOrDefault<int>("FuelFlowCalculationScheme", 0, null));
				}
				return this._FuelFlowCalculationScheme.Value;
			}
			set
			{
				FuelFlowCalculationSchemes? fuelFlowCalculationScheme = this._FuelFlowCalculationScheme;
				if (!((fuelFlowCalculationScheme.GetValueOrDefault() == value) & (fuelFlowCalculationScheme != null)))
				{
					this.AppSettings.AddOrUpdateValue("FuelFlowCalculationScheme", (int)value, null);
					this._FuelFlowCalculationScheme = new FuelFlowCalculationSchemes?(value);
					this.NotifyPropertyChanged("FuelFlowCalculationScheme");
					App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
					SimpleMainPage.Instance.RefreshPIDs();
					this.NotifyPropertyChanged("FuelFlowCalculationScheme");
					this.NotifyPropertyChanged("FuelFlowCalculationSchemeIdx");
				}
			}
		}

		// Token: 0x17001044 RID: 4164
		// (get) Token: 0x06001D68 RID: 7528 RVA: 0x0014966C File Offset: 0x0014786C
		// (set) Token: 0x06001D69 RID: 7529 RVA: 0x001496DC File Offset: 0x001478DC
		public bool UseRPMFix
		{
			get
			{
				if (!this.ShowExperimental)
				{
					return false;
				}
				if (this._UseRPMFix == null)
				{
					try
					{
						this._UseRPMFix = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseRPMFix", false, null));
					}
					catch
					{
						this._UseRPMFix = new bool?(false);
					}
				}
				return this._UseRPMFix.Value;
			}
			set
			{
				if (value == this.UseRPMFix)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseRPMFix", value, null);
				this._UseRPMFix = new bool?(value);
				this.NotifyPropertyChanged("UseRPMFix");
			}
		}

		// Token: 0x17001045 RID: 4165
		// (get) Token: 0x06001D6A RID: 7530 RVA: 0x00149711 File Offset: 0x00147911
		// (set) Token: 0x06001D6B RID: 7531 RVA: 0x00149750 File Offset: 0x00147950
		public double FuelTankCapacity
		{
			get
			{
				if (this._FuelTankCapacity == null)
				{
					this._FuelTankCapacity = new double?(this.AppSettings.GetValueOrDefault<double>("FuelTankCapacity", 50.0, null));
				}
				return this._FuelTankCapacity.Value;
			}
			set
			{
				if (value == this.FuelTankCapacity)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelTankCapacity", value, null);
				this._FuelTankCapacity = new double?(value);
				this.NotifyPropertyChanged("FuelTankCapacity");
			}
		}

		// Token: 0x17001046 RID: 4166
		// (get) Token: 0x06001D6C RID: 7532 RVA: 0x00149785 File Offset: 0x00147985
		// (set) Token: 0x06001D6D RID: 7533 RVA: 0x001497BD File Offset: 0x001479BD
		public int VE1000
		{
			get
			{
				if (this._VE1000 == null)
				{
					this._VE1000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE1000", 70, null));
				}
				return this._VE1000.Value;
			}
			set
			{
				if (value == this.VE1000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE1000", value, null);
				this._VE1000 = new int?(value);
				this.NotifyPropertyChanged("VE1000");
			}
		}

		// Token: 0x17001047 RID: 4167
		// (get) Token: 0x06001D6E RID: 7534 RVA: 0x001497F2 File Offset: 0x001479F2
		// (set) Token: 0x06001D6F RID: 7535 RVA: 0x0014982A File Offset: 0x00147A2A
		public int VE2000
		{
			get
			{
				if (this._VE2000 == null)
				{
					this._VE2000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE2000", 75, null));
				}
				return this._VE2000.Value;
			}
			set
			{
				if (value == this.VE2000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE2000", value, null);
				this._VE2000 = new int?(value);
				this.NotifyPropertyChanged("VE2000");
			}
		}

		// Token: 0x17001048 RID: 4168
		// (get) Token: 0x06001D70 RID: 7536 RVA: 0x0014985F File Offset: 0x00147A5F
		// (set) Token: 0x06001D71 RID: 7537 RVA: 0x00149897 File Offset: 0x00147A97
		public int VE3000
		{
			get
			{
				if (this._VE3000 == null)
				{
					this._VE3000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE3000", 80, null));
				}
				return this._VE3000.Value;
			}
			set
			{
				if (value == this.VE3000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE3000", value, null);
				this._VE3000 = new int?(value);
				this.NotifyPropertyChanged("VE3000");
			}
		}

		// Token: 0x17001049 RID: 4169
		// (get) Token: 0x06001D72 RID: 7538 RVA: 0x001498CC File Offset: 0x00147ACC
		// (set) Token: 0x06001D73 RID: 7539 RVA: 0x00149904 File Offset: 0x00147B04
		public int VE4000
		{
			get
			{
				if (this._VE4000 == null)
				{
					this._VE4000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE4000", 85, null));
				}
				return this._VE4000.Value;
			}
			set
			{
				if (value == this.VE4000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE4000", value, null);
				this._VE4000 = new int?(value);
				this.NotifyPropertyChanged("VE4000");
			}
		}

		// Token: 0x1700104A RID: 4170
		// (get) Token: 0x06001D74 RID: 7540 RVA: 0x00149939 File Offset: 0x00147B39
		// (set) Token: 0x06001D75 RID: 7541 RVA: 0x00149971 File Offset: 0x00147B71
		public int VE5000
		{
			get
			{
				if (this._VE5000 == null)
				{
					this._VE5000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE5000", 85, null));
				}
				return this._VE5000.Value;
			}
			set
			{
				if (value == this.VE5000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE5000", value, null);
				this._VE5000 = new int?(value);
				this.NotifyPropertyChanged("VE5000");
			}
		}

		// Token: 0x1700104B RID: 4171
		// (get) Token: 0x06001D76 RID: 7542 RVA: 0x001499A6 File Offset: 0x00147BA6
		// (set) Token: 0x06001D77 RID: 7543 RVA: 0x001499DE File Offset: 0x00147BDE
		public int VE6000
		{
			get
			{
				if (this._VE6000 == null)
				{
					this._VE6000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE6000", 85, null));
				}
				return this._VE6000.Value;
			}
			set
			{
				if (value == this.VE6000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE6000", value, null);
				this._VE6000 = new int?(value);
				this.NotifyPropertyChanged("VE6000");
			}
		}

		// Token: 0x1700104C RID: 4172
		// (get) Token: 0x06001D78 RID: 7544 RVA: 0x00149A13 File Offset: 0x00147C13
		// (set) Token: 0x06001D79 RID: 7545 RVA: 0x00149A4B File Offset: 0x00147C4B
		public int VE7000
		{
			get
			{
				if (this._VE7000 == null)
				{
					this._VE7000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE7000", 80, null));
				}
				return this._VE7000.Value;
			}
			set
			{
				if (value == this.VE7000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE7000", value, null);
				this._VE7000 = new int?(value);
				this.NotifyPropertyChanged("VE7000");
			}
		}

		// Token: 0x1700104D RID: 4173
		// (get) Token: 0x06001D7A RID: 7546 RVA: 0x00149A80 File Offset: 0x00147C80
		// (set) Token: 0x06001D7B RID: 7547 RVA: 0x00149AB8 File Offset: 0x00147CB8
		public int VE8000
		{
			get
			{
				if (this._VE8000 == null)
				{
					this._VE8000 = new int?(this.AppSettings.GetValueOrDefault<int>("VE8000", 75, null));
				}
				return this._VE8000.Value;
			}
			set
			{
				if (value == this.VE8000)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VE8000", value, null);
				this._VE8000 = new int?(value);
				this.NotifyPropertyChanged("VE8000");
			}
		}

		// Token: 0x1700104E RID: 4174
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x00149AED File Offset: 0x00147CED
		// (set) Token: 0x06001D7D RID: 7549 RVA: 0x00149B24 File Offset: 0x00147D24
		public int EngineCylinders
		{
			get
			{
				if (this._EngineCylinders == null)
				{
					this._EngineCylinders = new int?(this.AppSettings.GetValueOrDefault<int>("EngineCylinders", 4, null));
				}
				return this._EngineCylinders.Value;
			}
			set
			{
				if (value != this.EngineCylinders)
				{
					this.AppSettings.AddOrUpdateValue("EngineCylinders", value, null);
					this._EngineCylinders = new int?(value);
					this.NotifyPropertyChanged("EngineCylinders");
				}
			}
		}

		// Token: 0x1700104F RID: 4175
		// (get) Token: 0x06001D7E RID: 7550 RVA: 0x00149B58 File Offset: 0x00147D58
		// (set) Token: 0x06001D7F RID: 7551 RVA: 0x00149B60 File Offset: 0x00147D60
		public int FuelTypeIdx
		{
			get
			{
				return (int)this.FuelType;
			}
			set
			{
				this.FuelType = (FuelTypes)value;
			}
		}

		// Token: 0x17001050 RID: 4176
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x00149B6C File Offset: 0x00147D6C
		// (set) Token: 0x06001D81 RID: 7553 RVA: 0x00149BB0 File Offset: 0x00147DB0
		public FuelTypes FuelType
		{
			get
			{
				if (this._FuelType == null)
				{
					int valueOrDefault = this.AppSettings.GetValueOrDefault<int>("FuelType", 0, null);
					this._FuelType = new FuelTypes?((FuelTypes)valueOrDefault);
				}
				return this._FuelType.Value;
			}
			set
			{
				FuelTypes? fuelType = this._FuelType;
				if (!((value == fuelType.GetValueOrDefault()) & (fuelType != null)) && value == FuelTypes.EvNoFuel)
				{
					this.UseHoursePower = false;
				}
				this.AppSettings.AddOrUpdateValue("FuelType", (int)value, null);
				this._FuelType = new FuelTypes?(value);
				this.NotifyPropertyChanged("FuelType");
				this.NotifyPropertyChanged("FuelTypeIdx");
			}
		}

		// Token: 0x17001051 RID: 4177
		// (get) Token: 0x06001D82 RID: 7554 RVA: 0x00149C18 File Offset: 0x00147E18
		// (set) Token: 0x06001D83 RID: 7555 RVA: 0x00149C7E File Offset: 0x00147E7E
		public double EngineDisplacement
		{
			get
			{
				if (this._EngineDisplacement == null)
				{
					this._EngineDisplacement = new double?(this.AppSettings.GetValueOrDefault<double>("EngineDisplacement", 1.6, null));
					this._EngineDisplacement = new double?(Math.Round(this._EngineDisplacement.Value, 1));
				}
				return this._EngineDisplacement.Value;
			}
			set
			{
				value = Math.Round(value, 1);
				if (value != this.EngineDisplacement)
				{
					this.AppSettings.AddOrUpdateValue("EngineDisplacement", value, null);
					this._EngineDisplacement = new double?(value);
					this.NotifyPropertyChanged("EngineDisplacement");
				}
			}
		}

		// Token: 0x17001052 RID: 4178
		// (get) Token: 0x06001D84 RID: 7556 RVA: 0x00149CBB File Offset: 0x00147EBB
		// (set) Token: 0x06001D85 RID: 7557 RVA: 0x00149CF2 File Offset: 0x00147EF2
		public bool DetectZeroFuelConsumption
		{
			get
			{
				if (this._DetectZeroFuelConsumption == null)
				{
					this._DetectZeroFuelConsumption = new bool?(this.AppSettings.GetValueOrDefault<bool>("DetectZeroFuelConsumption", true, null));
				}
				return this._DetectZeroFuelConsumption.Value;
			}
			set
			{
				if (value == this.DetectZeroFuelConsumption)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DetectZeroFuelConsumption", value, null);
				this._DetectZeroFuelConsumption = new bool?(value);
				this.NotifyPropertyChanged("DetectZeroFuelConsumption");
			}
		}

		// Token: 0x17001053 RID: 4179
		// (get) Token: 0x06001D86 RID: 7558 RVA: 0x00149D27 File Offset: 0x00147F27
		// (set) Token: 0x06001D87 RID: 7559 RVA: 0x00149D3F File Offset: 0x00147F3F
		[JsonIgnore]
		public string MainPageButtonsConfiguration
		{
			get
			{
				return this.AppSettings.GetValueOrDefault<string>("MainPageButtonsConfiguration", "", null);
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("MainPageButtonsConfiguration", value, null);
				this.NotifyPropertyChanged("MainPageButtonsConfiguration");
			}
		}

		// Token: 0x17001054 RID: 4180
		// (get) Token: 0x06001D88 RID: 7560 RVA: 0x00149D60 File Offset: 0x00147F60
		// (set) Token: 0x06001D89 RID: 7561 RVA: 0x00149DB9 File Offset: 0x00147FB9
		public bool AndroidChartRenderingSafeMode
		{
			get
			{
				if (PlatformHelper.IsAndroid)
				{
					if (this._AndroidChartRenderingSafeMode == null)
					{
						bool flag = false;
						if (PlatformHelper.IsPlatformVersionNewerOrEqual(33, 0))
						{
							flag = true;
						}
						this._AndroidChartRenderingSafeMode = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidChartRenderingSafeMode", flag, null));
					}
					return this._AndroidChartRenderingSafeMode.Value;
				}
				return false;
			}
			set
			{
				if (value == this.AndroidChartRenderingSafeMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidChartRenderingSafeMode", value, null);
				this._AndroidChartRenderingSafeMode = new bool?(value);
				this.NotifyPropertyChanged("AndroidChartRenderingSafeMode");
			}
		}

		// Token: 0x17001055 RID: 4181
		// (get) Token: 0x06001D8A RID: 7562 RVA: 0x00149DEE File Offset: 0x00147FEE
		// (set) Token: 0x06001D8B RID: 7563 RVA: 0x00149E28 File Offset: 0x00148028
		public bool DarkMode
		{
			get
			{
				if (this._DarkMode == null)
				{
					this._DarkMode = new bool?(this.AppSettings.GetValueOrDefault<bool>("DarkMode", false, null));
				}
				return this._DarkMode.Value;
			}
			set
			{
				bool value2 = value;
				bool? darkMode = this._DarkMode;
				if (!((value2 == darkMode.GetValueOrDefault()) & (darkMode != null)))
				{
					MainThread.InvokeOnMainThreadAsync(delegate
					{
						try
						{
							this.AppSettings.AddOrUpdateValue("DarkMode", value, null);
							this._DarkMode = new bool?(value);
							this.NotifyPropertyChanged("DarkMode");
							if (value)
							{
								foreach (ResourceDictionary resourceDictionary in Application.Current.Resources.MergedDictionaries.Where((ResourceDictionary x) => x is LightTheme).ToArray<ResourceDictionary>())
								{
									Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
								}
								Application.Current.Resources.Add(new DarkTheme());
								this.AndroidSetNavbarColor();
							}
							else
							{
								foreach (ResourceDictionary resourceDictionary2 in Application.Current.Resources.MergedDictionaries.Where((ResourceDictionary x) => x is DarkTheme).ToArray<ResourceDictionary>())
								{
									Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
								}
								Application.Current.Resources.Add(new LightTheme());
								if (PlatformHelper.IsAndroid)
								{
									this.AndroidSetNavbarColor();
								}
							}
						}
						catch (Exception)
						{
						}
					}).Wait();
				}
			}
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x00149E7F File Offset: 0x0014807F
		private void AndroidSetNavbarColor()
		{
			if (PlatformHelper.IsAndroid)
			{
				MainThread.BeginInvokeOnMainThread(delegate
				{
					if (this.AndroidRecolorNavBar)
					{
						if (PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
						{
							Color color = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
							PlatformHelper.DroidService.SetNavigationBarColor(color);
							return;
						}
					}
					else if (PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
					{
						PlatformHelper.DroidService.SetNavigationBarColor(App.OriginalNavBarColor);
					}
				});
			}
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06001D8D RID: 7565 RVA: 0x00149E9C File Offset: 0x0014809C
		// (set) Token: 0x06001D8E RID: 7566 RVA: 0x00149EE8 File Offset: 0x001480E8
		public bool AutomaticallySwitchTheme
		{
			get
			{
				if (!this.AutomaticallySwitchThemeAvailable)
				{
					return false;
				}
				if (this._AutomaticallySwitchTheme == null)
				{
					this._AutomaticallySwitchTheme = new bool?(this.AppSettings.GetValueOrDefault<bool>("AutomaticallySwitchTheme", true, null));
				}
				return this._AutomaticallySwitchTheme.Value;
			}
			set
			{
				if (value == this.AutomaticallySwitchTheme)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AutomaticallySwitchTheme", value, null);
				this._AutomaticallySwitchTheme = new bool?(value);
				this.NotifyPropertyChanged("AutomaticallySwitchTheme");
			}
		}

		// Token: 0x17001057 RID: 4183
		// (get) Token: 0x06001D8F RID: 7567 RVA: 0x00149F1D File Offset: 0x0014811D
		public bool AutomaticallySwitchThemeAvailable
		{
			get
			{
				if (PlatformHelper.IsAndroid)
				{
					return PlatformHelper.IsPlatformVersionNewerOrEqual(29, 0);
				}
				if (PlatformHelper.IsiOS)
				{
					return PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0);
				}
				throw new NotImplementedException("SharedSettings->AutomaticallySwitchThemeAvailable->UnknownPlatform=" + Device.RuntimePlatform);
			}
		}

		// Token: 0x17001058 RID: 4184
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x00149F5D File Offset: 0x0014815D
		// (set) Token: 0x06001D91 RID: 7569 RVA: 0x00149F94 File Offset: 0x00148194
		public bool AndroidRecolorNavBar
		{
			get
			{
				if (this._AndroidRecolorNavBar == null)
				{
					this._AndroidRecolorNavBar = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidRecolorNavBar", false, null));
				}
				return this._AndroidRecolorNavBar.Value;
			}
			set
			{
				if (value == this.AndroidRecolorNavBar)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidRecolorNavBar", value, null);
				this._AndroidRecolorNavBar = new bool?(value);
				this.NotifyPropertyChanged("AndroidRecolorNavBar");
				this.AndroidSetNavbarColor();
			}
		}

		// Token: 0x17001059 RID: 4185
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x00149FCF File Offset: 0x001481CF
		// (set) Token: 0x06001D93 RID: 7571 RVA: 0x0014A006 File Offset: 0x00148206
		public bool AndroidUseFullscreen
		{
			get
			{
				if (this._AndroidUseFullscreen == null)
				{
					this._AndroidUseFullscreen = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidUseFullscreen", true, null));
				}
				return this._AndroidUseFullscreen.Value;
			}
			set
			{
				if (value == this.AndroidUseFullscreen)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidUseFullscreen", value, null);
				this._AndroidUseFullscreen = new bool?(value);
				this.NotifyPropertyChanged("AndroidUseFullscreen");
			}
		}

		// Token: 0x1700105A RID: 4186
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x0014A03B File Offset: 0x0014823B
		// (set) Token: 0x06001D95 RID: 7573 RVA: 0x0014A072 File Offset: 0x00148272
		[JsonIgnore]
		public int ChartsView
		{
			get
			{
				if (this._ChartsView == null)
				{
					this._ChartsView = new int?(this.AppSettings.GetValueOrDefault<int>("ChartsView", 0, null));
				}
				return this._ChartsView.Value;
			}
			set
			{
				if (value == this.ChartsView)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ChartsView", value, null);
				this._ChartsView = new int?(value);
				this.NotifyPropertyChanged("ChartsView");
			}
		}

		// Token: 0x1700105B RID: 4187
		// (get) Token: 0x06001D96 RID: 7574 RVA: 0x0014A0A7 File Offset: 0x001482A7
		// (set) Token: 0x06001D97 RID: 7575 RVA: 0x0014A0DE File Offset: 0x001482DE
		[JsonIgnore]
		public bool MultiChartPauseHidden
		{
			get
			{
				if (this._MultiChartPauseHidden == null)
				{
					this._MultiChartPauseHidden = new bool?(this.AppSettings.GetValueOrDefault<bool>("MultiChartPauseHidden", true, null));
				}
				return this._MultiChartPauseHidden.Value;
			}
			set
			{
				if (value == this.MultiChartPauseHidden)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("MultiChartPauseHidden", value, null);
				this._MultiChartPauseHidden = new bool?(value);
				this.NotifyPropertyChanged("MultiChartPauseHidden");
			}
		}

		// Token: 0x1700105C RID: 4188
		// (get) Token: 0x06001D98 RID: 7576 RVA: 0x0014A113 File Offset: 0x00148313
		// (set) Token: 0x06001D99 RID: 7577 RVA: 0x0014A13F File Offset: 0x0014833F
		[JsonIgnore]
		public string MultiPidsSelected
		{
			get
			{
				if (this._MultiPidsSelected == null)
				{
					this._MultiPidsSelected = this.AppSettings.GetValueOrDefault<string>("MultiPidsSelected", "", null);
				}
				return this._MultiPidsSelected;
			}
			set
			{
				if (value == this.MultiPidsSelected)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("MultiPidsSelected", value, null);
				this._MultiPidsSelected = value;
				this.NotifyPropertyChanged("MultiPidsSelected");
			}
		}

		// Token: 0x1700105D RID: 4189
		// (get) Token: 0x06001D9A RID: 7578 RVA: 0x0014A174 File Offset: 0x00148374
		// (set) Token: 0x06001D9B RID: 7579 RVA: 0x0014A1AB File Offset: 0x001483AB
		[JsonIgnore]
		public SharedSettings.PIDSortingModes PIDSortingMode
		{
			get
			{
				if (this._PIDSortingMode == null)
				{
					this._PIDSortingMode = new SharedSettings.PIDSortingModes?((SharedSettings.PIDSortingModes)this.AppSettings.GetValueOrDefault<int>("PIDSortingMode", 0, null));
				}
				return this._PIDSortingMode.Value;
			}
			set
			{
				if (value == this.PIDSortingMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PIDSortingMode", (int)value, null);
				this._PIDSortingMode = new SharedSettings.PIDSortingModes?(value);
				this.NotifyPropertyChanged("PIDSortingMode");
			}
		}

		// Token: 0x1700105E RID: 4190
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x0014A1E0 File Offset: 0x001483E0
		// (set) Token: 0x06001D9D RID: 7581 RVA: 0x0014A217 File Offset: 0x00148417
		[JsonIgnore]
		public bool ShowPing
		{
			get
			{
				if (this._ShowPing == null)
				{
					this._ShowPing = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowPing", false, null));
				}
				return this._ShowPing.Value;
			}
			set
			{
				if (value == this.ShowPing)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowPing", value, null);
				this._ShowPing = new bool?(value);
				this.NotifyPropertyChanged("ShowPing");
			}
		}

		// Token: 0x1700105F RID: 4191
		// (get) Token: 0x06001D9E RID: 7582 RVA: 0x0014A24C File Offset: 0x0014844C
		// (set) Token: 0x06001D9F RID: 7583 RVA: 0x0014A283 File Offset: 0x00148483
		[JsonIgnore]
		public bool RecordData
		{
			get
			{
				if (this._RecordData == null)
				{
					this._RecordData = new bool?(this.AppSettings.GetValueOrDefault<bool>("RecordData", true, null));
				}
				return this._RecordData.Value;
			}
			set
			{
				if (value == this.RecordData)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RecordData", value, null);
				this._RecordData = new bool?(value);
				this.NotifyPropertyChanged("RecordData");
			}
		}

		// Token: 0x17001060 RID: 4192
		// (get) Token: 0x06001DA0 RID: 7584 RVA: 0x0014A2B8 File Offset: 0x001484B8
		// (set) Token: 0x06001DA1 RID: 7585 RVA: 0x0014A2D3 File Offset: 0x001484D3
		[JsonIgnore]
		public bool LanguageChanged
		{
			get
			{
				return this._LanguageChanged && this.originalLanguage != this.Language;
			}
			set
			{
				this._LanguageChanged = value;
				this.NotifyPropertyChanged("LanguageChanged");
			}
		}

		// Token: 0x17001061 RID: 4193
		// (get) Token: 0x06001DA2 RID: 7586 RVA: 0x0014A2E8 File Offset: 0x001484E8
		// (set) Token: 0x06001DA3 RID: 7587 RVA: 0x0014A33C File Offset: 0x0014853C
		[JsonIgnore]
		public int Language
		{
			get
			{
				if (this._Language == null)
				{
					this._Language = new int?(this.AppSettings.GetValueOrDefault<int>("Language", 0, null));
					this.originalLanguage = this._Language.Value;
				}
				return this._Language.Value;
			}
			set
			{
				int? num = this._Language;
				bool flag = (value == num.GetValueOrDefault()) & (num != null);
				this.AppSettings.AddOrUpdateValue("Language", value, null);
				num = this._Language;
				if (!((num.GetValueOrDefault() == value) & (num != null)))
				{
					this.LanguageChanged = true;
				}
				if (value == this.originalLanguage)
				{
					this.LanguageChanged = false;
				}
				this._Language = new int?(value);
				this.NotifyPropertyChanged("Language");
			}
		}

		// Token: 0x17001062 RID: 4194
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x0014A3C0 File Offset: 0x001485C0
		// (set) Token: 0x06001DA5 RID: 7589 RVA: 0x0014A3F8 File Offset: 0x001485F8
		[JsonIgnore]
		public int LiveDataShowTime
		{
			get
			{
				if (this._LiveDataShowTime == null)
				{
					this._LiveDataShowTime = new int?(this.AppSettings.GetValueOrDefault<int>("LiveDataShowTime", 15, null));
				}
				return this._LiveDataShowTime.Value;
			}
			set
			{
				if (value == this.LiveDataShowTime)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataShowTime", value, null);
				this._LiveDataShowTime = new int?(value);
				this.NotifyPropertyChanged("LiveDataShowTime");
			}
		}

		// Token: 0x17001063 RID: 4195
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x0014A42D File Offset: 0x0014862D
		// (set) Token: 0x06001DA7 RID: 7591 RVA: 0x0014A464 File Offset: 0x00148664
		[JsonIgnore]
		public bool ChartShowAverageValue
		{
			get
			{
				if (this._ChartShowAverageValue == null)
				{
					this._ChartShowAverageValue = new bool?(this.AppSettings.GetValueOrDefault<bool>("ChartShowAverageValue", false, null));
				}
				return this._ChartShowAverageValue.Value;
			}
			set
			{
				if (value == this.ChartShowAverageValue)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ChartShowAverageValue", value, null);
				this._ChartShowAverageValue = new bool?(value);
				this.NotifyPropertyChanged("ChartShowAverageValue");
			}
		}

		// Token: 0x17001064 RID: 4196
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0014A499 File Offset: 0x00148699
		// (set) Token: 0x06001DA9 RID: 7593 RVA: 0x0014A4D0 File Offset: 0x001486D0
		[JsonIgnore]
		public bool SetChartMinMaxOnlyVisibleArea
		{
			get
			{
				if (this._SetChartMinMaxOnlyVisibleArea == null)
				{
					this._SetChartMinMaxOnlyVisibleArea = new bool?(this.AppSettings.GetValueOrDefault<bool>("SetChartMinMaxOnlyVisibleArea", true, null));
				}
				return this._SetChartMinMaxOnlyVisibleArea.Value;
			}
			set
			{
				if (value == this.SetChartMinMaxOnlyVisibleArea)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SetChartMinMaxOnlyVisibleArea", value, null);
				this._SetChartMinMaxOnlyVisibleArea = new bool?(value);
				this.NotifyPropertyChanged("SetChartMinMaxOnlyVisibleArea");
			}
		}

		// Token: 0x17001065 RID: 4197
		// (get) Token: 0x06001DAA RID: 7594 RVA: 0x0014A505 File Offset: 0x00148705
		// (set) Token: 0x06001DAB RID: 7595 RVA: 0x0014A53C File Offset: 0x0014873C
		public bool ShowMinMaxValues
		{
			get
			{
				if (this._ShowMinMaxValues == null)
				{
					this._ShowMinMaxValues = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowMinMaxValues", false, null));
				}
				return this._ShowMinMaxValues.Value;
			}
			set
			{
				if (value == this.ShowMinMaxValues)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ShowMinMaxValues", value, null);
				this._ShowMinMaxValues = new bool?(value);
				this.NotifyPropertyChanged("ShowMinMaxValues");
			}
		}

		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06001DAC RID: 7596 RVA: 0x0014A571 File Offset: 0x00148771
		// (set) Token: 0x06001DAD RID: 7597 RVA: 0x0014A5A8 File Offset: 0x001487A8
		[JsonIgnore]
		public int ChartsVisible
		{
			get
			{
				if (this._ChartsVisible == null)
				{
					this._ChartsVisible = new int?(this.AppSettings.GetValueOrDefault<int>("ChartsVisible", 1, null));
				}
				return this._ChartsVisible.Value;
			}
			set
			{
				if (value == this.ChartsVisible)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ChartsVisible", value, null);
				this._ChartsVisible = new int?(value);
				this.NotifyPropertyChanged("ChartsVisible");
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06001DAE RID: 7598 RVA: 0x0014A5E0 File Offset: 0x001487E0
		// (set) Token: 0x06001DAF RID: 7599 RVA: 0x0014A668 File Offset: 0x00148868
		[JsonIgnore]
		public int LiveDataPIDId0
		{
			get
			{
				if (this._LiveDataPIDId0 == null)
				{
					this._LiveDataPIDId0 = new int?(this.AppSettings.GetValueOrDefault<int>("LiveDataPIDId0", 0, null));
					if (this._LiveDataPIDId0.GetValueOrDefault() == 544)
					{
						this._LiveDataPIDId0 = new int?(231);
					}
					else if (this._LiveDataPIDId0.GetValueOrDefault() == 545)
					{
						this._LiveDataPIDId0 = new int?(234);
					}
				}
				return this._LiveDataPIDId0.Value;
			}
			set
			{
				if (value == this.LiveDataPIDId0)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataPIDId0", value, null);
				this._LiveDataPIDId0 = new int?(value);
				this.NotifyPropertyChanged("LiveDataPIDId0");
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06001DB0 RID: 7600 RVA: 0x0014A6A0 File Offset: 0x001488A0
		// (set) Token: 0x06001DB1 RID: 7601 RVA: 0x0014A728 File Offset: 0x00148928
		[JsonIgnore]
		public int LiveDataPIDId1
		{
			get
			{
				if (this._LiveDataPIDId1 == null)
				{
					this._LiveDataPIDId1 = new int?(this.AppSettings.GetValueOrDefault<int>("LiveDataPIDId1", 0, null));
					if (this._LiveDataPIDId1.GetValueOrDefault() == 544)
					{
						this._LiveDataPIDId1 = new int?(231);
					}
					else if (this._LiveDataPIDId1.GetValueOrDefault() == 545)
					{
						this._LiveDataPIDId1 = new int?(234);
					}
				}
				return this._LiveDataPIDId1.Value;
			}
			set
			{
				if (value == this.LiveDataPIDId1)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataPIDId1", value, null);
				this._LiveDataPIDId1 = new int?(value);
				this.NotifyPropertyChanged("LiveDataPIDId1");
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06001DB2 RID: 7602 RVA: 0x0014A760 File Offset: 0x00148960
		// (set) Token: 0x06001DB3 RID: 7603 RVA: 0x0014A7E8 File Offset: 0x001489E8
		[JsonIgnore]
		public int LiveDataPIDId2
		{
			get
			{
				if (this._LiveDataPIDId2 == null)
				{
					this._LiveDataPIDId2 = new int?(this.AppSettings.GetValueOrDefault<int>("LiveDataPIDId2", 0, null));
					if (this._LiveDataPIDId2.GetValueOrDefault() == 544)
					{
						this._LiveDataPIDId2 = new int?(231);
					}
					else if (this._LiveDataPIDId2.GetValueOrDefault() == 545)
					{
						this._LiveDataPIDId2 = new int?(234);
					}
				}
				return this._LiveDataPIDId2.Value;
			}
			set
			{
				if (value == this.LiveDataPIDId2)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataPIDId2", value, null);
				this._LiveDataPIDId2 = new int?(value);
				this.NotifyPropertyChanged("LiveDataPIDId2");
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06001DB4 RID: 7604 RVA: 0x0014A820 File Offset: 0x00148A20
		// (set) Token: 0x06001DB5 RID: 7605 RVA: 0x0014A8A8 File Offset: 0x00148AA8
		[JsonIgnore]
		public int LiveDataPIDId3
		{
			get
			{
				if (this._LiveDataPIDId3 == null)
				{
					this._LiveDataPIDId3 = new int?(this.AppSettings.GetValueOrDefault<int>("LiveDataPIDId3", 0, null));
					if (this._LiveDataPIDId3.GetValueOrDefault() == 544)
					{
						this._LiveDataPIDId3 = new int?(231);
					}
					else if (this._LiveDataPIDId3.GetValueOrDefault() == 545)
					{
						this._LiveDataPIDId3 = new int?(234);
					}
				}
				return this._LiveDataPIDId3.Value;
			}
			set
			{
				if (value == this.LiveDataPIDId3)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataPIDId3", value, null);
				this._LiveDataPIDId3 = new int?(value);
				this.NotifyPropertyChanged("LiveDataPIDId3");
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06001DB6 RID: 7606 RVA: 0x0014A8DD File Offset: 0x00148ADD
		// (set) Token: 0x06001DB7 RID: 7607 RVA: 0x0014A914 File Offset: 0x00148B14
		[JsonIgnore]
		public bool NoWiFiWarning
		{
			get
			{
				if (this._NoWiFiWarning == null)
				{
					this._NoWiFiWarning = new bool?(this.AppSettings.GetValueOrDefault<bool>("NoWiFiWarning", true, null));
				}
				return this._NoWiFiWarning.Value;
			}
			set
			{
				if (value == this.NoWiFiWarning)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("NoWiFiWarning", value, null);
				this._NoWiFiWarning = new bool?(value);
				this.NotifyPropertyChanged("NoWiFiWarning");
			}
		}

		// Token: 0x1700106C RID: 4204
		// (get) Token: 0x06001DB8 RID: 7608 RVA: 0x0014A949 File Offset: 0x00148B49
		// (set) Token: 0x06001DB9 RID: 7609 RVA: 0x0014A980 File Offset: 0x00148B80
		[JsonIgnore]
		public bool TryConnectToLastWiFiNetwork
		{
			get
			{
				if (this._TryConnectToLastWiFiNetwork == null)
				{
					this._TryConnectToLastWiFiNetwork = new bool?(this.AppSettings.GetValueOrDefault<bool>("TryConnectToLastWiFiNetwork", true, null));
				}
				return this._TryConnectToLastWiFiNetwork.Value;
			}
			set
			{
				if (value == this.TryConnectToLastWiFiNetwork)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("TryConnectToLastWiFiNetwork", value, null);
				this._TryConnectToLastWiFiNetwork = new bool?(value);
				this.NotifyPropertyChanged("TryConnectToLastWiFiNetwork");
			}
		}

		// Token: 0x1700106D RID: 4205
		// (get) Token: 0x06001DBA RID: 7610 RVA: 0x0014A9B5 File Offset: 0x00148BB5
		// (set) Token: 0x06001DBB RID: 7611 RVA: 0x0014A9EC File Offset: 0x00148BEC
		[JsonIgnore]
		public bool RPMFixWarningShowed
		{
			get
			{
				if (this._RPMFixWarningShowed == null)
				{
					this._RPMFixWarningShowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("RPMFixWarningShowed", false, null));
				}
				return this._RPMFixWarningShowed.Value;
			}
			set
			{
				if (value == this.RPMFixWarningShowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RPMFixWarningShowed", value, null);
				this._RPMFixWarningShowed = new bool?(value);
				this.NotifyPropertyChanged("RPMFixWarningShowed");
			}
		}

		// Token: 0x1700106E RID: 4206
		// (get) Token: 0x06001DBC RID: 7612 RVA: 0x0014AA21 File Offset: 0x00148C21
		// (set) Token: 0x06001DBD RID: 7613 RVA: 0x0014AA58 File Offset: 0x00148C58
		[JsonIgnore]
		public bool FirstTimeLaunch
		{
			get
			{
				if (this._FirstTimeLaunch == null)
				{
					this._FirstTimeLaunch = new bool?(this.AppSettings.GetValueOrDefault<bool>("FirstTimeLaunch", true, null));
				}
				return this._FirstTimeLaunch.Value;
			}
			set
			{
				if (value == this.FirstTimeLaunch)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FirstTimeLaunch", value, null);
				this._FirstTimeLaunch = new bool?(value);
				this.NotifyPropertyChanged("FirstTimeLaunch");
			}
		}

		// Token: 0x1700106F RID: 4207
		// (get) Token: 0x06001DBE RID: 7614 RVA: 0x0014AA8D File Offset: 0x00148C8D
		// (set) Token: 0x06001DBF RID: 7615 RVA: 0x0014AAC4 File Offset: 0x00148CC4
		[JsonIgnore]
		public bool InfoShowed_Dashboard
		{
			get
			{
				if (this._InfoShowed_Dashboard == null)
				{
					this._InfoShowed_Dashboard = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_Dashboard", false, null));
				}
				return this._InfoShowed_Dashboard.Value;
			}
			set
			{
				if (value == this.InfoShowed_Dashboard)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_Dashboard", value, null);
				this._InfoShowed_Dashboard = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_Dashboard");
			}
		}

		// Token: 0x17001070 RID: 4208
		// (get) Token: 0x06001DC0 RID: 7616 RVA: 0x0014AAF9 File Offset: 0x00148CF9
		// (set) Token: 0x06001DC1 RID: 7617 RVA: 0x0014AB30 File Offset: 0x00148D30
		[JsonIgnore]
		public bool InfoShowed_SpeedTest
		{
			get
			{
				if (this._InfoShowed_SpeedTest == null)
				{
					this._InfoShowed_SpeedTest = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_SpeedTest", false, null));
				}
				return this._InfoShowed_SpeedTest.Value;
			}
			set
			{
				if (value == this.InfoShowed_SpeedTest)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_SpeedTest", value, null);
				this._InfoShowed_SpeedTest = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_SpeedTest");
			}
		}

		// Token: 0x17001071 RID: 4209
		// (get) Token: 0x06001DC2 RID: 7618 RVA: 0x0014AB65 File Offset: 0x00148D65
		// (set) Token: 0x06001DC3 RID: 7619 RVA: 0x0014AB9C File Offset: 0x00148D9C
		[JsonIgnore]
		public bool InfoShowed_Mode06
		{
			get
			{
				if (this._InfoShowed_Mode06 == null)
				{
					this._InfoShowed_Mode06 = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_Mode06", false, null));
				}
				return this._InfoShowed_Mode06.Value;
			}
			set
			{
				if (value == this.InfoShowed_Mode06)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_Mode06", value, null);
				this._InfoShowed_Mode06 = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_Mode06");
			}
		}

		// Token: 0x17001072 RID: 4210
		// (get) Token: 0x06001DC4 RID: 7620 RVA: 0x0014ABD1 File Offset: 0x00148DD1
		// (set) Token: 0x06001DC5 RID: 7621 RVA: 0x0014AC08 File Offset: 0x00148E08
		[JsonIgnore]
		public bool InfoShowed_AllSensors
		{
			get
			{
				if (this._InfoShowed_AllSensors == null)
				{
					this._InfoShowed_AllSensors = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_AllSensors", false, null));
				}
				return this._InfoShowed_AllSensors.Value;
			}
			set
			{
				if (value == this.InfoShowed_AllSensors)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_AllSensors", value, null);
				this._InfoShowed_AllSensors = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_AllSensors");
			}
		}

		// Token: 0x17001073 RID: 4211
		// (get) Token: 0x06001DC6 RID: 7622 RVA: 0x0014AC3D File Offset: 0x00148E3D
		// (set) Token: 0x06001DC7 RID: 7623 RVA: 0x0014AC74 File Offset: 0x00148E74
		[JsonIgnore]
		public bool InfoShowed_EcoTests
		{
			get
			{
				if (this._InfoShowed_EcoTests == null)
				{
					this._InfoShowed_EcoTests = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_EcoTests", false, null));
				}
				return this._InfoShowed_EcoTests.Value;
			}
			set
			{
				if (value == this.InfoShowed_EcoTests)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_EcoTests", value, null);
				this._InfoShowed_EcoTests = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_EcoTests");
			}
		}

		// Token: 0x17001074 RID: 4212
		// (get) Token: 0x06001DC8 RID: 7624 RVA: 0x0014ACA9 File Offset: 0x00148EA9
		// (set) Token: 0x06001DC9 RID: 7625 RVA: 0x0014ACE0 File Offset: 0x00148EE0
		[JsonIgnore]
		public bool InfoShowed_FreezeFrame
		{
			get
			{
				if (this._InfoShowed_FreezeFrame == null)
				{
					this._InfoShowed_FreezeFrame = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_FreezeFrame", false, null));
				}
				return this._InfoShowed_FreezeFrame.Value;
			}
			set
			{
				if (value == this.InfoShowed_FreezeFrame)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_FreezeFrame", value, null);
				this._InfoShowed_FreezeFrame = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_FreezeFrame");
			}
		}

		// Token: 0x17001075 RID: 4213
		// (get) Token: 0x06001DCA RID: 7626 RVA: 0x0014AD15 File Offset: 0x00148F15
		// (set) Token: 0x06001DCB RID: 7627 RVA: 0x0014AD4C File Offset: 0x00148F4C
		[JsonIgnore]
		public bool NissanWarningShowed
		{
			get
			{
				if (this._NissanWarningShowed == null)
				{
					this._NissanWarningShowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("NissanWarningShowed", false, null));
				}
				return this._NissanWarningShowed.Value;
			}
			set
			{
				if (value == this.NissanWarningShowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("NissanWarningShowed", value, null);
				this._NissanWarningShowed = new bool?(value);
				this.NotifyPropertyChanged("NissanWarningShowed");
			}
		}

		// Token: 0x17001076 RID: 4214
		// (get) Token: 0x06001DCC RID: 7628 RVA: 0x0014AD81 File Offset: 0x00148F81
		// (set) Token: 0x06001DCD RID: 7629 RVA: 0x0014ADB8 File Offset: 0x00148FB8
		[JsonIgnore]
		public bool Mode06AttentionShow
		{
			get
			{
				if (this._Mode06AttentionShow == null)
				{
					this._Mode06AttentionShow = new bool?(this.AppSettings.GetValueOrDefault<bool>("Mode06AttentionShow", false, null));
				}
				return this._Mode06AttentionShow.Value;
			}
			set
			{
				if (value == this.Mode06AttentionShow)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Mode06AttentionShow", value, null);
				this._Mode06AttentionShow = new bool?(value);
				this.NotifyPropertyChanged("Mode06AttentionShow");
			}
		}

		// Token: 0x17001077 RID: 4215
		// (get) Token: 0x06001DCE RID: 7630 RVA: 0x0014ADED File Offset: 0x00148FED
		// (set) Token: 0x06001DCF RID: 7631 RVA: 0x0014AE24 File Offset: 0x00149024
		[JsonIgnore]
		public bool LiveData_InfoShowed
		{
			get
			{
				if (this._LiveData_InfoShowed == null)
				{
					this._LiveData_InfoShowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("LiveData_InfoShowed", false, null));
				}
				return this._LiveData_InfoShowed.Value;
			}
			set
			{
				if (value == this.LiveData_InfoShowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveData_InfoShowed", value, null);
				this._LiveData_InfoShowed = new bool?(value);
				this.NotifyPropertyChanged("LiveData_InfoShowed");
			}
		}

		// Token: 0x17001078 RID: 4216
		// (get) Token: 0x06001DD0 RID: 7632 RVA: 0x0014AE59 File Offset: 0x00149059
		// (set) Token: 0x06001DD1 RID: 7633 RVA: 0x0014AE90 File Offset: 0x00149090
		[JsonIgnore]
		public bool InfoShowed_DTCPage
		{
			get
			{
				if (this._InfoShowed_DTCPage == null)
				{
					this._InfoShowed_DTCPage = new bool?(this.AppSettings.GetValueOrDefault<bool>("InfoShowed_DTCPage", false, null));
				}
				return this._InfoShowed_DTCPage.Value;
			}
			set
			{
				if (value == this.InfoShowed_DTCPage)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("InfoShowed_DTCPage", value, null);
				this._InfoShowed_DTCPage = new bool?(value);
				this.NotifyPropertyChanged("InfoShowed_DTCPage");
			}
		}

		// Token: 0x17001079 RID: 4217
		// (get) Token: 0x06001DD2 RID: 7634 RVA: 0x0014AEC5 File Offset: 0x001490C5
		// (set) Token: 0x06001DD3 RID: 7635 RVA: 0x0014AEFC File Offset: 0x001490FC
		public bool DashboardRearrangeOnRotation
		{
			get
			{
				if (this._DashboardRearrangeOnRotation == null)
				{
					this._DashboardRearrangeOnRotation = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardRearrangeOnRotation", true, null));
				}
				return this._DashboardRearrangeOnRotation.Value;
			}
			set
			{
				if (value == this.DashboardRearrangeOnRotation)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardRearrangeOnRotation", value, null);
				this._DashboardRearrangeOnRotation = new bool?(value);
				this.NotifyPropertyChanged("DashboardRearrangeOnRotation");
			}
		}

		// Token: 0x1700107A RID: 4218
		// (get) Token: 0x06001DD4 RID: 7636 RVA: 0x0014AF31 File Offset: 0x00149131
		// (set) Token: 0x06001DD5 RID: 7637 RVA: 0x0014AF68 File Offset: 0x00149168
		public bool DashboardHUDMode
		{
			get
			{
				if (this._DashboardHUDMode == null)
				{
					this._DashboardHUDMode = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardHUDMode", false, null));
				}
				return this._DashboardHUDMode.Value;
			}
			set
			{
				if (value == this.DashboardHUDMode)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardHUDMode", value, null);
				this._DashboardHUDMode = new bool?(value);
				this.NotifyPropertyChanged("DashboardHUDMode");
			}
		}

		// Token: 0x1700107B RID: 4219
		// (get) Token: 0x06001DD6 RID: 7638 RVA: 0x0014AF9D File Offset: 0x0014919D
		// (set) Token: 0x06001DD7 RID: 7639 RVA: 0x0014AFD4 File Offset: 0x001491D4
		public int DashboardTheme
		{
			get
			{
				if (this._DashboardTheme == null)
				{
					this._DashboardTheme = new int?(this.AppSettings.GetValueOrDefault<int>("DashboardTheme", 2, null));
				}
				return this._DashboardTheme.Value;
			}
			set
			{
				if (value == this.DashboardTheme)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardTheme", value, null);
				this._DashboardTheme = new int?(value);
				this.NotifyPropertyChanged("DashboardTheme");
			}
		}

		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06001DD8 RID: 7640 RVA: 0x0014B009 File Offset: 0x00149209
		// (set) Token: 0x06001DD9 RID: 7641 RVA: 0x0014B021 File Offset: 0x00149221
		public string Dashboard
		{
			get
			{
				return this.AppSettings.GetValueOrDefault<string>("Dashboard", "", null);
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("Dashboard", value, null);
				this.NotifyPropertyChanged("Dashboard");
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.OnDashboardConfigurationUpdated();
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06001DDA RID: 7642 RVA: 0x0014B04F File Offset: 0x0014924F
		// (set) Token: 0x06001DDB RID: 7643 RVA: 0x0014B086 File Offset: 0x00149286
		[JsonIgnore]
		public int DashboardLastPage
		{
			get
			{
				if (this._DashboardLastPage == null)
				{
					this._DashboardLastPage = new int?(this.AppSettings.GetValueOrDefault<int>("DashboardLastPage", 0, null));
				}
				return this._DashboardLastPage.Value;
			}
			set
			{
				if (value == this.DashboardLastPage)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardLastPage", value, null);
				this._DashboardLastPage = new int?(value);
				this.NotifyPropertyChanged("DashboardLastPage");
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06001DDC RID: 7644 RVA: 0x0014B0BB File Offset: 0x001492BB
		// (set) Token: 0x06001DDD RID: 7645 RVA: 0x0014B0E7 File Offset: 0x001492E7
		public string LastCarAvailableSensors
		{
			get
			{
				if (this._LastCarAvailableSensors == null)
				{
					this._LastCarAvailableSensors = this.AppSettings.GetValueOrDefault<string>("LastCarAvailableSensors", "", null);
				}
				return this._LastCarAvailableSensors;
			}
			set
			{
				if (value == this.LastCarAvailableSensors)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastCarAvailableSensors", value, null);
				this._LastCarAvailableSensors = value;
				this.NotifyPropertyChanged("LastCarAvailableSensors");
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06001DDE RID: 7646 RVA: 0x0014B11C File Offset: 0x0014931C
		// (set) Token: 0x06001DDF RID: 7647 RVA: 0x0014B154 File Offset: 0x00149354
		[JsonIgnore]
		public long CurrentCarId
		{
			get
			{
				if (this._CurrentCarId == null)
				{
					this._CurrentCarId = new long?(this.AppSettings.GetValueOrDefault<long>("CurrentCarId", 0L, null));
				}
				return this._CurrentCarId.Value;
			}
			set
			{
				if (value == this.CurrentCarId)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CurrentCarId", value, null);
				this._CurrentCarId = new long?(value);
				this.NotifyPropertyChanged("CurrentCarId");
			}
		}

		// Token: 0x17001080 RID: 4224
		// (get) Token: 0x06001DE0 RID: 7648 RVA: 0x0014B189 File Offset: 0x00149389
		// (set) Token: 0x06001DE1 RID: 7649 RVA: 0x0014B1BA File Offset: 0x001493BA
		public string CurrentCarName
		{
			get
			{
				if (this._CurrentCarName == null)
				{
					this._CurrentCarName = this.AppSettings.GetValueOrDefault<string>("CurrentCarName", Translate.GetString("ios_MY_CAR"), null);
				}
				return this._CurrentCarName;
			}
			set
			{
				if (value == this.CurrentCarName)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CurrentCarName", value, null);
				this._CurrentCarName = value;
				this.NotifyPropertyChanged("CurrentCarName");
				SimpleMainPage.Instance.UpdateMainButtons();
			}
		}

		// Token: 0x17001081 RID: 4225
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x0014B1F9 File Offset: 0x001493F9
		// (set) Token: 0x06001DE3 RID: 7651 RVA: 0x0014B230 File Offset: 0x00149430
		public DTCModeV2 DTCReadingModeV2
		{
			get
			{
				if (this._DTCReadingModeV2 == null)
				{
					this._DTCReadingModeV2 = new DTCModeV2?((DTCModeV2)this.AppSettings.GetValueOrDefault<int>("DTCReadingModeV2", 0, null));
				}
				return this._DTCReadingModeV2.Value;
			}
			set
			{
				if (value == this.DTCReadingModeV2)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DTCReadingModeV2", (int)value, null);
				this._DTCReadingModeV2 = new DTCModeV2?(value);
				this.NotifyPropertyChanged("DTCReadingModeV2");
			}
		}

		// Token: 0x17001082 RID: 4226
		// (get) Token: 0x06001DE4 RID: 7652 RVA: 0x0014B265 File Offset: 0x00149465
		// (set) Token: 0x06001DE5 RID: 7653 RVA: 0x0014B29C File Offset: 0x0014949C
		public DTCModeV2 DTCClearingModeV2
		{
			get
			{
				if (this._DTCClearingModeV2 == null)
				{
					this._DTCClearingModeV2 = new DTCModeV2?((DTCModeV2)this.AppSettings.GetValueOrDefault<int>("DTCClearingModeV2", 0, null));
				}
				return this._DTCClearingModeV2.Value;
			}
			set
			{
				if (value == this.DTCClearingModeV2)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DTCClearingModeV2", (int)value, null);
				this._DTCClearingModeV2 = new DTCModeV2?(value);
				this.NotifyPropertyChanged("DTCClearingModeV2");
			}
		}

		// Token: 0x17001083 RID: 4227
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x0014B2D1 File Offset: 0x001494D1
		// (set) Token: 0x06001DE7 RID: 7655 RVA: 0x0014B2FD File Offset: 0x001494FD
		public string DTCReadingSequence
		{
			get
			{
				if (this._DTCReadingSequence == null)
				{
					this._DTCReadingSequence = this.AppSettings.GetValueOrDefault<string>("DTCReadingSequence", "", null);
				}
				return this._DTCReadingSequence;
			}
			set
			{
				if (value == this.DTCReadingSequence)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DTCReadingSequence", value, null);
				this._DTCReadingSequence = value;
				this.NotifyPropertyChanged("DTCReadingSequence");
			}
		}

		// Token: 0x17001084 RID: 4228
		// (get) Token: 0x06001DE8 RID: 7656 RVA: 0x0014B332 File Offset: 0x00149532
		// (set) Token: 0x06001DE9 RID: 7657 RVA: 0x0014B35E File Offset: 0x0014955E
		public string DTCClearingSequence
		{
			get
			{
				if (this._DTCClearingSequence == null)
				{
					this._DTCClearingSequence = this.AppSettings.GetValueOrDefault<string>("DTCClearingSequence", "", null);
				}
				return this._DTCClearingSequence;
			}
			set
			{
				if (value == this.DTCClearingSequence)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DTCClearingSequence", value, null);
				this._DTCClearingSequence = value;
				this.NotifyPropertyChanged("DTCClearingSequence");
			}
		}

		// Token: 0x17001085 RID: 4229
		// (get) Token: 0x06001DEA RID: 7658 RVA: 0x0014B393 File Offset: 0x00149593
		// (set) Token: 0x06001DEB RID: 7659 RVA: 0x0014B3CA File Offset: 0x001495CA
		public bool ForceOnlyOneProtocol
		{
			get
			{
				if (this._ForceOnlyOneProtocol == null)
				{
					this._ForceOnlyOneProtocol = new bool?(this.AppSettings.GetValueOrDefault<bool>("ForceOnlyOneProtocol", false, null));
				}
				return this._ForceOnlyOneProtocol.Value;
			}
			set
			{
				if (value == this.ForceOnlyOneProtocol)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ForceOnlyOneProtocol", value, null);
				this._ForceOnlyOneProtocol = new bool?(value);
				this.NotifyPropertyChanged("ForceOnlyOneProtocol");
			}
		}

		// Token: 0x17001086 RID: 4230
		// (get) Token: 0x06001DEC RID: 7660 RVA: 0x0014B3FF File Offset: 0x001495FF
		// (set) Token: 0x06001DED RID: 7661 RVA: 0x0014B42C File Offset: 0x0014962C
		public string Mode01Prefix
		{
			get
			{
				if (this._Mode01Prefix == null)
				{
					this._Mode01Prefix = this.AppSettings.GetValueOrDefault<string>("Mode01Prefix", "01", null);
				}
				return this._Mode01Prefix;
			}
			set
			{
				if (value != this._Mode01Prefix)
				{
					this.AppSettings.AddOrUpdateValue("Mode01Prefix", value, null);
					this._Mode01Prefix = value;
					this.NotifyPropertyChanged("Mode01Prefix");
					OBDDataReader obdreader = App.OBDReader;
					if (obdreader != null)
					{
						CarData currentCarData = obdreader.CurrentCarData;
						if (currentCarData != null)
						{
							currentCarData.CreateEmptyPIDS();
						}
					}
					OBDDataReader obdreader2 = App.OBDReader;
					if (obdreader2 == null)
					{
						return;
					}
					CarData currentCarData2 = obdreader2.CurrentCarData;
					if (currentCarData2 == null)
					{
						return;
					}
					currentCarData2.InitializeCalculatedPIDs();
				}
			}
		}

		// Token: 0x17001087 RID: 4231
		// (get) Token: 0x06001DEE RID: 7662 RVA: 0x0014B49F File Offset: 0x0014969F
		// (set) Token: 0x06001DEF RID: 7663 RVA: 0x0014B4D6 File Offset: 0x001496D6
		public int ResponseMarkerLength
		{
			get
			{
				if (this._ResponseMarkerLength == null)
				{
					this._ResponseMarkerLength = new int?(this.AppSettings.GetValueOrDefault<int>("ResponseMarkerLength", 0, null));
				}
				return this._ResponseMarkerLength.Value;
			}
			set
			{
				if (value == this.ResponseMarkerLength)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ResponseMarkerLength", value, null);
				this._ResponseMarkerLength = new int?(value);
				this.NotifyPropertyChanged("ResponseMarkerLength");
			}
		}

		// Token: 0x17001088 RID: 4232
		// (get) Token: 0x06001DF0 RID: 7664 RVA: 0x0014B50B File Offset: 0x0014970B
		// (set) Token: 0x06001DF1 RID: 7665 RVA: 0x0014B537 File Offset: 0x00149737
		public string LastWiFiName
		{
			get
			{
				if (this._LastWiFiName == null)
				{
					this._LastWiFiName = this.AppSettings.GetValueOrDefault<string>("LastWiFiName", "", null);
				}
				return this._LastWiFiName;
			}
			set
			{
				if (value == this.LastWiFiName)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastWiFiName", value, null);
				this._LastWiFiName = value;
				this.NotifyPropertyChanged("LastWiFiName");
			}
		}

		// Token: 0x17001089 RID: 4233
		// (get) Token: 0x06001DF2 RID: 7666 RVA: 0x0014B56C File Offset: 0x0014976C
		// (set) Token: 0x06001DF3 RID: 7667 RVA: 0x0014B5A3 File Offset: 0x001497A3
		public int LastSuccessfulProtocol
		{
			get
			{
				if (this._LastSuccessfulProtocol == null)
				{
					this._LastSuccessfulProtocol = new int?(this.AppSettings.GetValueOrDefault<int>("LastSuccessfulProtocol", 0, null));
				}
				return this._LastSuccessfulProtocol.Value;
			}
			set
			{
				if (value == this.LastSuccessfulProtocol)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastSuccessfulProtocol", value, null);
				this._LastSuccessfulProtocol = new int?(value);
				this.NotifyPropertyChanged("LastSuccessfulProtocol");
			}
		}

		// Token: 0x1700108A RID: 4234
		// (get) Token: 0x06001DF4 RID: 7668 RVA: 0x0014B5D8 File Offset: 0x001497D8
		// (set) Token: 0x06001DF5 RID: 7669 RVA: 0x0014B60F File Offset: 0x0014980F
		public bool AndroidRequestedStoragePermission
		{
			get
			{
				if (this._AndroidRequestedStoragePermission == null)
				{
					this._AndroidRequestedStoragePermission = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidRequestedStoragePermission", false, null));
				}
				return this._AndroidRequestedStoragePermission.Value;
			}
			set
			{
				if (value == this.AndroidRequestedStoragePermission)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidRequestedStoragePermission", value, null);
				this._AndroidRequestedStoragePermission = new bool?(value);
				this.NotifyPropertyChanged("AndroidRequestedStoragePermission");
			}
		}

		// Token: 0x1700108B RID: 4235
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x0014B644 File Offset: 0x00149844
		// (set) Token: 0x06001DF7 RID: 7671 RVA: 0x0014B67B File Offset: 0x0014987B
		public bool AndroidRequestedLocationPermission
		{
			get
			{
				if (this._AndroidRequestedLocationPermission == null)
				{
					this._AndroidRequestedLocationPermission = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidRequestedLocationPermission", false, null));
				}
				return this._AndroidRequestedLocationPermission.Value;
			}
			set
			{
				if (value == this.AndroidRequestedLocationPermission)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidRequestedLocationPermission", value, null);
				this._AndroidRequestedLocationPermission = new bool?(value);
				this.NotifyPropertyChanged("AndroidRequestedLocationPermission");
			}
		}

		// Token: 0x1700108C RID: 4236
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x0014B6B0 File Offset: 0x001498B0
		// (set) Token: 0x06001DF9 RID: 7673 RVA: 0x0014B6DC File Offset: 0x001498DC
		public string CodingLastPlatformSelected
		{
			get
			{
				if (this._CodingLastPlatformSelected == null)
				{
					this._CodingLastPlatformSelected = this.AppSettings.GetValueOrDefault<string>("CodingLastPlatformSelected", "", null);
				}
				return this._CodingLastPlatformSelected;
			}
			set
			{
				if (value == this.CodingLastPlatformSelected)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CodingLastPlatformSelected", value, null);
				this._CodingLastPlatformSelected = value;
				this.NotifyPropertyChanged("CodingLastPlatformSelected");
			}
		}

		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x0014B711 File Offset: 0x00149911
		// (set) Token: 0x06001DFB RID: 7675 RVA: 0x0014B748 File Offset: 0x00149948
		public bool SendATZATE
		{
			get
			{
				if (this._SendATZATE == null)
				{
					this._SendATZATE = new bool?(this.AppSettings.GetValueOrDefault<bool>("SendATZATE", true, null));
				}
				return this._SendATZATE.Value;
			}
			set
			{
				if (value == this.SendATZATE)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendATZATE", value, null);
				this._SendATZATE = new bool?(value);
				this.NotifyPropertyChanged("SendATZATE");
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06001DFC RID: 7676 RVA: 0x0014B77D File Offset: 0x0014997D
		// (set) Token: 0x06001DFD RID: 7677 RVA: 0x0014B7B4 File Offset: 0x001499B4
		public bool HideArchiveDTC
		{
			get
			{
				if (this._HideArchiveDTC == null)
				{
					this._HideArchiveDTC = new bool?(this.AppSettings.GetValueOrDefault<bool>("HideArchiveDTC", false, null));
				}
				return this._HideArchiveDTC.Value;
			}
			set
			{
				if (value == this.HideArchiveDTC)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("HideArchiveDTC", value, null);
				this._HideArchiveDTC = new bool?(value);
				this.NotifyPropertyChanged("HideArchiveDTC");
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06001DFE RID: 7678 RVA: 0x0014B7E9 File Offset: 0x001499E9
		// (set) Token: 0x06001DFF RID: 7679 RVA: 0x0014B7F1 File Offset: 0x001499F1
		public bool IgnoreCodingErrors
		{
			get
			{
				return this._IgnoreCodingErrors;
			}
			set
			{
				if (value == this.IgnoreCodingErrors)
				{
					return;
				}
				this._IgnoreCodingErrors = value;
				this.NotifyPropertyChanged("IgnoreCodingErrors");
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06001E00 RID: 7680 RVA: 0x0014B80F File Offset: 0x00149A0F
		// (set) Token: 0x06001E01 RID: 7681 RVA: 0x0014B846 File Offset: 0x00149A46
		public bool VWTP20OpenSessionForDTCOperations
		{
			get
			{
				if (this._VWTP20OpenSessionForDTCOperations == null)
				{
					this._VWTP20OpenSessionForDTCOperations = new bool?(this.AppSettings.GetValueOrDefault<bool>("VWTP20OpenSessionForDTCOperations", true, null));
				}
				return this._VWTP20OpenSessionForDTCOperations.Value;
			}
			set
			{
				if (value == this.VWTP20OpenSessionForDTCOperations)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VWTP20OpenSessionForDTCOperations", value, null);
				this._VWTP20OpenSessionForDTCOperations = new bool?(value);
				this.NotifyPropertyChanged("VWTP20OpenSessionForDTCOperations");
			}
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x0014B87B File Offset: 0x00149A7B
		// (set) Token: 0x06001E03 RID: 7683 RVA: 0x0014B8A7 File Offset: 0x00149AA7
		[JsonIgnore]
		public string PIDOverridesData
		{
			get
			{
				if (this._PIDOverridesData == null)
				{
					this._PIDOverridesData = this.AppSettings.GetValueOrDefault<string>("PIDOverridesData", "", null);
				}
				return this._PIDOverridesData;
			}
			set
			{
				if (value == this.PIDOverridesData)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PIDOverridesData", value, null);
				this._PIDOverridesData = value;
				this.NotifyPropertyChanged("PIDOverridesData");
			}
		}

		// Token: 0x17001092 RID: 4242
		// (get) Token: 0x06001E04 RID: 7684 RVA: 0x0014B8DC File Offset: 0x00149ADC
		// (set) Token: 0x06001E05 RID: 7685 RVA: 0x0014B913 File Offset: 0x00149B13
		public bool ExpectedResponseCountOptimization
		{
			get
			{
				if (this._ExpectedResponseCountOptimization == null)
				{
					this._ExpectedResponseCountOptimization = new bool?(this.AppSettings.GetValueOrDefault<bool>("ExpectedResponseCountOptimization", true, null));
				}
				return this._ExpectedResponseCountOptimization.Value;
			}
			set
			{
				if (value == this.ExpectedResponseCountOptimization)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ExpectedResponseCountOptimization", value, null);
				this._ExpectedResponseCountOptimization = new bool?(value);
				this.NotifyPropertyChanged("ExpectedResponseCountOptimization");
			}
		}

		// Token: 0x17001093 RID: 4243
		// (get) Token: 0x06001E06 RID: 7686 RVA: 0x0014B948 File Offset: 0x00149B48
		// (set) Token: 0x06001E07 RID: 7687 RVA: 0x0014B97F File Offset: 0x00149B7F
		public bool ExpectedResponseCountOptimizationAlways1ForKWPMode01
		{
			get
			{
				if (this._ExpectedResponseCountOptimizationAlways1ForKWPMode01 == null)
				{
					this._ExpectedResponseCountOptimizationAlways1ForKWPMode01 = new bool?(this.AppSettings.GetValueOrDefault<bool>("ExpectedResponseCountOptimizationAlways1ForKWPMode01", false, null));
				}
				return this._ExpectedResponseCountOptimizationAlways1ForKWPMode01.Value;
			}
			set
			{
				if (value == this.ExpectedResponseCountOptimizationAlways1ForKWPMode01)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ExpectedResponseCountOptimizationAlways1ForKWPMode01", value, null);
				this._ExpectedResponseCountOptimizationAlways1ForKWPMode01 = new bool?(value);
				this.NotifyPropertyChanged("ExpectedResponseCountOptimizationAlways1ForKWPMode01");
			}
		}

		// Token: 0x17001094 RID: 4244
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0014B9B4 File Offset: 0x00149BB4
		// (set) Token: 0x06001E09 RID: 7689 RVA: 0x0014B9F8 File Offset: 0x00149BF8
		internal PIDEditorListModel.ViewByType PidListLastViewByType
		{
			get
			{
				if (this._PidListLastViewByType == null)
				{
					int valueOrDefault = this.AppSettings.GetValueOrDefault<int>("PidListLastViewByType", 0, null);
					this._PidListLastViewByType = new PIDEditorListModel.ViewByType?((PIDEditorListModel.ViewByType)valueOrDefault);
				}
				return this._PidListLastViewByType.Value;
			}
			set
			{
				if (value == this.PidListLastViewByType)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PidListLastViewByType", (int)value, null);
				this._PidListLastViewByType = new PIDEditorListModel.ViewByType?(value);
				this.NotifyPropertyChanged("PidListLastViewByType");
			}
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0014BA30 File Offset: 0x00149C30
		// (set) Token: 0x06001E0B RID: 7691 RVA: 0x0014BA74 File Offset: 0x00149C74
		public ReadPartialErrorActions ReadPartialErrorAction
		{
			get
			{
				if (this._ReadPartialErrorAction == null)
				{
					int valueOrDefault = this.AppSettings.GetValueOrDefault<int>("ReadPartialErrorAction", 2, null);
					this._ReadPartialErrorAction = new ReadPartialErrorActions?((ReadPartialErrorActions)valueOrDefault);
				}
				return this._ReadPartialErrorAction.Value;
			}
			set
			{
				if (value == this.ReadPartialErrorAction)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ReadPartialErrorAction", (int)value, null);
				this._ReadPartialErrorAction = new ReadPartialErrorActions?(value);
				this.NotifyPropertyChanged("ReadPartialErrorAction");
			}
		}

		// Token: 0x06001E0C RID: 7692 RVA: 0x0014BAAC File Offset: 0x00149CAC
		public async Task SendDebugEmail()
		{
			int num = 0;
			try
			{
				string text = " (" + Device.RuntimePlatform + ")";
				string text2 = await this.GetSettingsReport();
				EmailMessage message = new EmailMessage(text, text2, new string[] { "admin@carscanner.info" });
				text = null;
				message.BodyFormat = 0;
				if (!string.IsNullOrEmpty(this.LastException))
				{
					message.Subject = "Car Scanner Crash log " + message.Subject;
					message.Body = message.Body + "\r\n" + this.LastException;
				}
				else
				{
					message.Subject = "Car Scanner user feedback" + message.Subject;
				}
				try
				{
					await PCLDebugStream.CurrentInstance.Flush();
					PCLDebugStream.CurrentInstance.Close();
				}
				catch (Exception)
				{
				}
				string filepath = PCLDebugStream.GetFilepath();
				if (File.Exists(filepath))
				{
					message.Attachments.Add(new EmailAttachment(filepath));
				}
				await Email.ComposeAsync(message);
				message = null;
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception ex = (Exception)obj;
				try
				{
					await App.GetCurrentPage().DisplayAlert("Error creating email", "Please send email to:\nadmin@carscanner.info", "OK");
				}
				catch (Exception)
				{
				}
			}
			obj = null;
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06001E0D RID: 7693 RVA: 0x0014BAF0 File Offset: 0x00149CF0
		// (set) Token: 0x06001E0E RID: 7694 RVA: 0x0014BB48 File Offset: 0x00149D48
		public string DTCFoundECUs
		{
			get
			{
				string text;
				try
				{
					text = SharedSettings.DecodeViaBytes(this.AppSettings.GetValueOrDefault<string>("DTCFoundECUs", "", "pr.png"), (byte)"Car Scanner"[5]);
				}
				catch (Exception)
				{
					text = "";
				}
				return text;
			}
			set
			{
				string text = SharedSettings.EncodeViaBytes(value, (byte)"Car Scanner"[5]);
				this.AppSettings.AddOrUpdateValue("DTCFoundECUs", text, "pr.png");
			}
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x06001E0F RID: 7695 RVA: 0x0014BB7E File Offset: 0x00149D7E
		// (set) Token: 0x06001E10 RID: 7696 RVA: 0x0014BBAA File Offset: 0x00149DAA
		public string RecentColors
		{
			get
			{
				if (this._RecentColors == null)
				{
					this._RecentColors = this.AppSettings.GetValueOrDefault<string>("RecentColors", "", null);
				}
				return this._RecentColors;
			}
			set
			{
				if (value == this.RecentColors)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RecentColors", value, null);
				this._RecentColors = value;
				this.NotifyPropertyChanged("RecentColors");
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x06001E11 RID: 7697 RVA: 0x0014BBDF File Offset: 0x00149DDF
		// (set) Token: 0x06001E12 RID: 7698 RVA: 0x0014BC1D File Offset: 0x00149E1D
		public bool DashboardCircularGaugeAnimation
		{
			get
			{
				if (this._DashboardCircularGaugeAnimation == null)
				{
					this._DashboardCircularGaugeAnimation = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardCircularGaugeAnimation", !PlatformHelper.IsAndroid, null));
				}
				return this._DashboardCircularGaugeAnimation.Value;
			}
			set
			{
				if (value == this.DashboardCircularGaugeAnimation)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardCircularGaugeAnimation", value, null);
				this._DashboardCircularGaugeAnimation = new bool?(value);
				this.NotifyPropertyChanged("DashboardCircularGaugeAnimation");
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x0014BC52 File Offset: 0x00149E52
		// (set) Token: 0x06001E14 RID: 7700 RVA: 0x0014BC89 File Offset: 0x00149E89
		public bool ForceUseManualFlowControlForCodingOperations
		{
			get
			{
				if (this._ForceUseManualFlowControlForCodingOperations == null)
				{
					this._ForceUseManualFlowControlForCodingOperations = new bool?(this.AppSettings.GetValueOrDefault<bool>("ForceUseManualFlowControlForCodingOperations", true, null));
				}
				return this._ForceUseManualFlowControlForCodingOperations.Value;
			}
			set
			{
				if (value == this.ForceUseManualFlowControlForCodingOperations)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ForceUseManualFlowControlForCodingOperations", value, null);
				this._ForceUseManualFlowControlForCodingOperations = new bool?(value);
				this.NotifyPropertyChanged("ForceUseManualFlowControlForCodingOperations");
			}
		}

		// Token: 0x1700109A RID: 4250
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0014BCBE File Offset: 0x00149EBE
		// (set) Token: 0x06001E16 RID: 7702 RVA: 0x0014BCF9 File Offset: 0x00149EF9
		public int DatasetUploadMaxBlockSize
		{
			get
			{
				if (this._DatasetUploadMaxBlockSize == null)
				{
					this._DatasetUploadMaxBlockSize = new int?(this.AppSettings.GetValueOrDefault<int>("DatasetUploadMaxBlockSize", 4095, null));
				}
				return this._DatasetUploadMaxBlockSize.Value;
			}
			set
			{
				if (value == this.DatasetUploadMaxBlockSize)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DatasetUploadMaxBlockSize", value, null);
				this._DatasetUploadMaxBlockSize = new int?(value);
				this.NotifyPropertyChanged("DatasetUploadMaxBlockSize");
			}
		}

		// Token: 0x1700109B RID: 4251
		// (get) Token: 0x06001E17 RID: 7703 RVA: 0x0014BD2E File Offset: 0x00149F2E
		// (set) Token: 0x06001E18 RID: 7704 RVA: 0x0014BD69 File Offset: 0x00149F69
		public int SendTesterPresentWhileLongUploadTimeMs
		{
			get
			{
				if (this._SendTesterPresentWhileLongUploadTimeMs == null)
				{
					this._SendTesterPresentWhileLongUploadTimeMs = new int?(this.AppSettings.GetValueOrDefault<int>("SendTesterPresentWhileLongUploadTimeMs", 750, null));
				}
				return this._SendTesterPresentWhileLongUploadTimeMs.Value;
			}
			set
			{
				if (value == this.SendTesterPresentWhileLongUploadTimeMs)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendTesterPresentWhileLongUploadTimeMs", value, null);
				this._SendTesterPresentWhileLongUploadTimeMs = new int?(value);
				this.NotifyPropertyChanged("SendTesterPresentWhileLongUploadTimeMs");
			}
		}

		// Token: 0x1700109C RID: 4252
		// (get) Token: 0x06001E19 RID: 7705 RVA: 0x0014BD9E File Offset: 0x00149F9E
		// (set) Token: 0x06001E1A RID: 7706 RVA: 0x0014BDCA File Offset: 0x00149FCA
		public string CustomCodings
		{
			get
			{
				if (this._CustomCodings == null)
				{
					this._CustomCodings = this.AppSettings.GetValueOrDefault<string>("CustomCodings", "[]", null);
				}
				return this._CustomCodings;
			}
			set
			{
				if (value == this.CustomCodings)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CustomCodings", value, null);
				this._CustomCodings = value;
				this.NotifyPropertyChanged("CustomCodings");
			}
		}

		// Token: 0x1700109D RID: 4253
		// (get) Token: 0x06001E1B RID: 7707 RVA: 0x0014BDFF File Offset: 0x00149FFF
		// (set) Token: 0x06001E1C RID: 7708 RVA: 0x0014BE36 File Offset: 0x0014A036
		public bool AndroidRecolorStatusBarInDarkTheme
		{
			get
			{
				if (this._AndroidRecolorStatusBarInDarkTheme == null)
				{
					this._AndroidRecolorStatusBarInDarkTheme = new bool?(this.AppSettings.GetValueOrDefault<bool>("AndroidRecolorStatusBarInDarkTheme", true, null));
				}
				return this._AndroidRecolorStatusBarInDarkTheme.Value;
			}
			set
			{
				if (value == this.AndroidRecolorStatusBarInDarkTheme)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AndroidRecolorStatusBarInDarkTheme", value, null);
				this._AndroidRecolorStatusBarInDarkTheme = new bool?(value);
				this.NotifyPropertyChanged("AndroidRecolorStatusBarInDarkTheme");
			}
		}

		// Token: 0x1700109E RID: 4254
		// (get) Token: 0x06001E1D RID: 7709 RVA: 0x0014BE6B File Offset: 0x0014A06B
		// (set) Token: 0x06001E1E RID: 7710 RVA: 0x0014BEA2 File Offset: 0x0014A0A2
		public bool TraceLogs
		{
			get
			{
				if (this._TraceLogs == null)
				{
					this._TraceLogs = new bool?(this.AppSettings.GetValueOrDefault<bool>("TraceLogs", false, null));
				}
				return this._TraceLogs.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("TraceLogs", value, null);
				this._TraceLogs = new bool?(value);
				this.NotifyPropertyChanged("TraceLogs");
			}
		}

		// Token: 0x1700109F RID: 4255
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0014BECD File Offset: 0x0014A0CD
		// (set) Token: 0x06001E20 RID: 7712 RVA: 0x0014BF04 File Offset: 0x0014A104
		public bool DatasetAllowed
		{
			get
			{
				if (this._DatasetAllowed == null)
				{
					this._DatasetAllowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("DatasetAllowed", true, null));
				}
				return this._DatasetAllowed.Value;
			}
			set
			{
				if (value == this.DatasetAllowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DatasetAllowed", value, null);
				this._DatasetAllowed = new bool?(value);
				this.NotifyPropertyChanged("DatasetAllowed");
			}
		}

		// Token: 0x170010A0 RID: 4256
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0014BF39 File Offset: 0x0014A139
		// (set) Token: 0x06001E22 RID: 7714 RVA: 0x0014BF70 File Offset: 0x0014A170
		public bool SensorsSearchOrderDefault
		{
			get
			{
				if (this._SensorsSearchOrderDefault == null)
				{
					this._SensorsSearchOrderDefault = new bool?(this.AppSettings.GetValueOrDefault<bool>("SensorsSearchOrderDefault", true, null));
				}
				return this._SensorsSearchOrderDefault.Value;
			}
			set
			{
				if (value == this.SensorsSearchOrderDefault)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SensorsSearchOrderDefault", value, null);
				this._SensorsSearchOrderDefault = new bool?(value);
				this.NotifyPropertyChanged("SensorsSearchOrderDefault");
				OBDDataReader obdreader = App.OBDReader;
				if (obdreader == null)
				{
					return;
				}
				CarData currentCarData = obdreader.CurrentCarData;
				if (currentCarData == null)
				{
					return;
				}
				currentCarData.ClearFindCache();
			}
		}

		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x0014BFC9 File Offset: 0x0014A1C9
		// (set) Token: 0x06001E24 RID: 7716 RVA: 0x0014C000 File Offset: 0x0014A200
		public bool ForceUseManualFlowControlWhileReadingData
		{
			get
			{
				if (this._ForceUseManualFlowControlWhileReadingData == null)
				{
					this._ForceUseManualFlowControlWhileReadingData = new bool?(this.AppSettings.GetValueOrDefault<bool>("ForceUseManualFlowControlWhileReadingData", true, null));
				}
				return this._ForceUseManualFlowControlWhileReadingData.Value;
			}
			set
			{
				if (value == this.ForceUseManualFlowControlWhileReadingData)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ForceUseManualFlowControlWhileReadingData", value, null);
				this._ForceUseManualFlowControlWhileReadingData = new bool?(value);
				this.NotifyPropertyChanged("ForceUseManualFlowControlWhileReadingData");
			}
		}

		// Token: 0x170010A2 RID: 4258
		// (get) Token: 0x06001E25 RID: 7717 RVA: 0x0014C035 File Offset: 0x0014A235
		// (set) Token: 0x06001E26 RID: 7718 RVA: 0x0014C06C File Offset: 0x0014A26C
		public bool DashboardAnimation
		{
			get
			{
				if (this._DashboardAnimation == null)
				{
					this._DashboardAnimation = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardAnimation", false, null));
				}
				return this._DashboardAnimation.Value;
			}
			set
			{
				if (value == this.DashboardAnimation)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardAnimation", value, null);
				this._DashboardAnimation = new bool?(value);
				this.NotifyPropertyChanged("DashboardAnimation");
			}
		}

		// Token: 0x170010A3 RID: 4259
		// (get) Token: 0x06001E27 RID: 7719 RVA: 0x0014C0A1 File Offset: 0x0014A2A1
		// (set) Token: 0x06001E28 RID: 7720 RVA: 0x0014C0D8 File Offset: 0x0014A2D8
		public bool DashboardHideTopControls
		{
			get
			{
				if (this._DashboardHideTopControls == null)
				{
					this._DashboardHideTopControls = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardHideTopControls", true, null));
				}
				return this._DashboardHideTopControls.Value;
			}
			set
			{
				if (value == this.DashboardHideTopControls)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardHideTopControls", value, null);
				this._DashboardHideTopControls = new bool?(value);
				this.NotifyPropertyChanged("DashboardHideTopControls");
			}
		}

		// Token: 0x170010A4 RID: 4260
		// (get) Token: 0x06001E29 RID: 7721 RVA: 0x0014C10D File Offset: 0x0014A30D
		// (set) Token: 0x06001E2A RID: 7722 RVA: 0x0014C144 File Offset: 0x0014A344
		public bool DashboardAlignItemsToGrid
		{
			get
			{
				if (this._DashboardAlignItemsToGrid == null)
				{
					this._DashboardAlignItemsToGrid = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardAlignItemsToGrid", true, null));
				}
				return this._DashboardAlignItemsToGrid.Value;
			}
			set
			{
				if (value == this.DashboardAlignItemsToGrid)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardAlignItemsToGrid", value, null);
				this._DashboardAlignItemsToGrid = new bool?(value);
				this.NotifyPropertyChanged("DashboardAlignItemsToGrid");
			}
		}

		// Token: 0x170010A5 RID: 4261
		// (get) Token: 0x06001E2B RID: 7723 RVA: 0x0014C179 File Offset: 0x0014A379
		// (set) Token: 0x06001E2C RID: 7724 RVA: 0x0014C1B0 File Offset: 0x0014A3B0
		public bool DashboardResizeHintDisplayed
		{
			get
			{
				if (this._DashboardResizeHintDisplayed == null)
				{
					this._DashboardResizeHintDisplayed = new bool?(this.AppSettings.GetValueOrDefault<bool>("DashboardResizeHintDisplayed", false, null));
				}
				return this._DashboardResizeHintDisplayed.Value;
			}
			set
			{
				if (value == this.DashboardResizeHintDisplayed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("DashboardResizeHintDisplayed", value, null);
				this._DashboardResizeHintDisplayed = new bool?(value);
				this.NotifyPropertyChanged("DashboardResizeHintDisplayed");
			}
		}

		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x06001E2D RID: 7725 RVA: 0x0014C1E5 File Offset: 0x0014A3E5
		// (set) Token: 0x06001E2E RID: 7726 RVA: 0x0014C21C File Offset: 0x0014A41C
		public bool FuelHybridCar
		{
			get
			{
				if (this._FuelHybridCar == null)
				{
					this._FuelHybridCar = new bool?(this.AppSettings.GetValueOrDefault<bool>("FuelHybridCar", false, null));
				}
				return this._FuelHybridCar.Value;
			}
			set
			{
				if (value == this.FuelHybridCar)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("FuelHybridCar", value, null);
				this._FuelHybridCar = new bool?(value);
				this.NotifyPropertyChanged("FuelHybridCar");
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x06001E2F RID: 7727 RVA: 0x0014C251 File Offset: 0x0014A451
		// (set) Token: 0x06001E30 RID: 7728 RVA: 0x0014C288 File Offset: 0x0014A488
		public bool SearchForBTLEIfConnectionFailed
		{
			get
			{
				if (this._SearchForBTLEIfConnectionFailed == null)
				{
					this._SearchForBTLEIfConnectionFailed = new bool?(this.AppSettings.GetValueOrDefault<bool>("SearchForBTLEIfConnectionFailed", true, null));
				}
				return this._SearchForBTLEIfConnectionFailed.Value;
			}
			set
			{
				if (value == this.SearchForBTLEIfConnectionFailed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SearchForBTLEIfConnectionFailed", value, null);
				this._SearchForBTLEIfConnectionFailed = new bool?(value);
				this.NotifyPropertyChanged("SearchForBTLEIfConnectionFailed");
			}
		}

		// Token: 0x170010A8 RID: 4264
		// (get) Token: 0x06001E31 RID: 7729 RVA: 0x0014C2BD File Offset: 0x0014A4BD
		// (set) Token: 0x06001E32 RID: 7730 RVA: 0x0014C2F4 File Offset: 0x0014A4F4
		public bool VagSendRebootAfterClear
		{
			get
			{
				if (this._VagSendRebootAfterClear == null)
				{
					this._VagSendRebootAfterClear = new bool?(this.AppSettings.GetValueOrDefault<bool>("VagSendRebootAfterClear", true, null));
				}
				return this._VagSendRebootAfterClear.Value;
			}
			set
			{
				if (value == this.VagSendRebootAfterClear)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("VagSendRebootAfterClear", value, null);
				this._VagSendRebootAfterClear = new bool?(value);
				this.NotifyPropertyChanged("VagSendRebootAfterClear");
			}
		}

		// Token: 0x170010A9 RID: 4265
		// (get) Token: 0x06001E33 RID: 7731 RVA: 0x0014C329 File Offset: 0x0014A529
		// (set) Token: 0x06001E34 RID: 7732 RVA: 0x0014C360 File Offset: 0x0014A560
		public bool RecordLocationData
		{
			get
			{
				if (this._RecordLocationData == null)
				{
					this._RecordLocationData = new bool?(this.AppSettings.GetValueOrDefault<bool>("RecordLocationData", false, null));
				}
				return this._RecordLocationData.Value;
			}
			set
			{
				if (value == this.RecordLocationData)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RecordLocationData", value, null);
				this._RecordLocationData = new bool?(value);
				this.NotifyPropertyChanged("RecordLocationData");
			}
		}

		// Token: 0x170010AA RID: 4266
		// (get) Token: 0x06001E35 RID: 7733 RVA: 0x0014C395 File Offset: 0x0014A595
		// (set) Token: 0x06001E36 RID: 7734 RVA: 0x0014C3CC File Offset: 0x0014A5CC
		public bool SendTesterPresentWhileLongUploadTimeVag5f
		{
			get
			{
				if (this._SendTesterPresentWhileLongUploadTimeVag5f == null)
				{
					this._SendTesterPresentWhileLongUploadTimeVag5f = new bool?(this.AppSettings.GetValueOrDefault<bool>("SendTesterPresentWhileLongUploadTimeVag5f", true, null));
				}
				return this._SendTesterPresentWhileLongUploadTimeVag5f.Value;
			}
			set
			{
				if (value == this.SendTesterPresentWhileLongUploadTimeVag5f)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("SendTesterPresentWhileLongUploadTimeVag5f", value, null);
				this._SendTesterPresentWhileLongUploadTimeVag5f = new bool?(value);
				this.NotifyPropertyChanged("SendTesterPresentWhileLongUploadTimeVag5f");
			}
		}

		// Token: 0x170010AB RID: 4267
		// (get) Token: 0x06001E37 RID: 7735 RVA: 0x0014C401 File Offset: 0x0014A601
		// (set) Token: 0x06001E38 RID: 7736 RVA: 0x0014C438 File Offset: 0x0014A638
		public bool CANRequestSegmentationSTNLevel
		{
			get
			{
				if (this._CANRequestSegmentationSTNLevel == null)
				{
					this._CANRequestSegmentationSTNLevel = new bool?(this.AppSettings.GetValueOrDefault<bool>("CANRequestSegmentationSTNLevel", true, null));
				}
				return this._CANRequestSegmentationSTNLevel.Value;
			}
			set
			{
				if (value == this.CANRequestSegmentationSTNLevel)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANRequestSegmentationSTNLevel", value, null);
				this._CANRequestSegmentationSTNLevel = new bool?(value);
				this.NotifyPropertyChanged("CANRequestSegmentationSTNLevel");
			}
		}

		// Token: 0x170010AC RID: 4268
		// (get) Token: 0x06001E39 RID: 7737 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06001E3A RID: 7738 RVA: 0x0014C46D File Offset: 0x0014A66D
		public bool CANResponseSegmentationSTNLevel
		{
			get
			{
				return false;
			}
			set
			{
				if (value == this.CANResponseSegmentationSTNLevel)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("CANResponseSegmentationSTNLevel", value, null);
				this._CANResponseSegmentationSTNLevel = new bool?(value);
				this.NotifyPropertyChanged("CANResponseSegmentationSTNLevel");
			}
		}

		// Token: 0x170010AD RID: 4269
		// (get) Token: 0x06001E3B RID: 7739 RVA: 0x0014C4A2 File Offset: 0x0014A6A2
		// (set) Token: 0x06001E3C RID: 7740 RVA: 0x0014C4D9 File Offset: 0x0014A6D9
		public bool ReplaceATTAWithATCER
		{
			get
			{
				if (this._ReplaceATTAWithATCER == null)
				{
					this._ReplaceATTAWithATCER = new bool?(this.AppSettings.GetValueOrDefault<bool>("ReplaceATTAWithATCER", false, null));
				}
				return this._ReplaceATTAWithATCER.Value;
			}
			set
			{
				if (value == this.ReplaceATTAWithATCER)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ReplaceATTAWithATCER", value, null);
				this._ReplaceATTAWithATCER = new bool?(value);
				this.NotifyPropertyChanged("ReplaceATTAWithATCER");
			}
		}

		// Token: 0x170010AE RID: 4270
		// (get) Token: 0x06001E3D RID: 7741 RVA: 0x0014C50E File Offset: 0x0014A70E
		// (set) Token: 0x06001E3E RID: 7742 RVA: 0x0014C545 File Offset: 0x0014A745
		public ChartItemTypes ChartDisplayStyle
		{
			get
			{
				if (this._ChartDisplayStyle == null)
				{
					this._ChartDisplayStyle = new ChartItemTypes?((ChartItemTypes)this.AppSettings.GetValueOrDefault<int>("ChartDisplayStyle", 0, null));
				}
				return this._ChartDisplayStyle.Value;
			}
			set
			{
				if (value == this.ChartDisplayStyle)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ChartDisplayStyle", (int)value, null);
				this._ChartDisplayStyle = new ChartItemTypes?(value);
				this.NotifyPropertyChanged("ChartDisplayStyle");
			}
		}

		// Token: 0x170010AF RID: 4271
		// (get) Token: 0x06001E3F RID: 7743 RVA: 0x0014C57A File Offset: 0x0014A77A
		// (set) Token: 0x06001E40 RID: 7744 RVA: 0x0014C5B1 File Offset: 0x0014A7B1
		public bool ATCommandStateOptimization
		{
			get
			{
				if (this._ATCommandStateOptimization == null)
				{
					this._ATCommandStateOptimization = new bool?(this.AppSettings.GetValueOrDefault<bool>("ATCommandStateOptimization", true, null));
				}
				return this._ATCommandStateOptimization.Value;
			}
			set
			{
				if (value == this.ATCommandStateOptimization)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("ATCommandStateOptimization", value, null);
				this._ATCommandStateOptimization = new bool?(value);
				this.NotifyPropertyChanged("ATCommandStateOptimization");
			}
		}

		// Token: 0x170010B0 RID: 4272
		// (get) Token: 0x06001E41 RID: 7745 RVA: 0x0014C5E6 File Offset: 0x0014A7E6
		// (set) Token: 0x06001E42 RID: 7746 RVA: 0x0014C61D File Offset: 0x0014A81D
		public bool ATCRAOptimization
		{
			get
			{
				if (this._ATCRAOptimization == null)
				{
					this._ATCRAOptimization = new bool?(this.AppSettings.GetValueOrDefault<bool>("ATCRAOptimization", false, null));
				}
				return this._ATCRAOptimization.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ATCRAOptimization", value, null);
				this._ATCRAOptimization = new bool?(value);
				this.NotifyPropertyChanged("ATCRAOptimization");
			}
		}

		// Token: 0x170010B1 RID: 4273
		// (get) Token: 0x06001E43 RID: 7747 RVA: 0x0014C648 File Offset: 0x0014A848
		// (set) Token: 0x06001E44 RID: 7748 RVA: 0x0014C67F File Offset: 0x0014A87F
		public bool BluetoothLocationWarningShowed
		{
			get
			{
				if (this._BluetoothLocationWarningShowed == null)
				{
					this._BluetoothLocationWarningShowed = new bool?(this.AppSettings.GetValueOrDefault<bool>("BluetoothLocationWarningShowed", false, null));
				}
				return this._BluetoothLocationWarningShowed.Value;
			}
			set
			{
				if (value == this.BluetoothLocationWarningShowed)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BluetoothLocationWarningShowed", value, null);
				this._BluetoothLocationWarningShowed = new bool?(value);
				this.NotifyPropertyChanged("BluetoothLocationWarningShowed");
			}
		}

		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06001E45 RID: 7749 RVA: 0x0014C6B4 File Offset: 0x0014A8B4
		// (set) Token: 0x06001E46 RID: 7750 RVA: 0x0014C6EC File Offset: 0x0014A8EC
		[JsonIgnore]
		public long LastTimePatchUpdateChecked
		{
			get
			{
				if (this._LastTimePatchUpdateChecked == null)
				{
					this._LastTimePatchUpdateChecked = new long?(this.AppSettings.GetValueOrDefault<long>("LastTimePatchUpdateChecked", 0L, null));
				}
				return this._LastTimePatchUpdateChecked.Value;
			}
			set
			{
				if (value == this.LastTimePatchUpdateChecked)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastTimePatchUpdateChecked", value, null);
				this._LastTimePatchUpdateChecked = new long?(value);
				this.NotifyPropertyChanged("LastTimePatchUpdateChecked");
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x0014C721 File Offset: 0x0014A921
		// (set) Token: 0x06001E48 RID: 7752 RVA: 0x0014C759 File Offset: 0x0014A959
		[JsonIgnore]
		public long LastTimeNewVersionChecked
		{
			get
			{
				if (this._LastTimeNewVersionChecked == null)
				{
					this._LastTimeNewVersionChecked = new long?(this.AppSettings.GetValueOrDefault<long>("LastTimeNewVersionChecked", 0L, null));
				}
				return this._LastTimeNewVersionChecked.Value;
			}
			set
			{
				if (value == this.LastTimeNewVersionChecked)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastTimeNewVersionChecked", value, null);
				this._LastTimeNewVersionChecked = new long?(value);
				this.NotifyPropertyChanged("LastTimeNewVersionChecked");
			}
		}

		// Token: 0x170010B4 RID: 4276
		// (get) Token: 0x06001E49 RID: 7753 RVA: 0x0014C78E File Offset: 0x0014A98E
		// (set) Token: 0x06001E4A RID: 7754 RVA: 0x0014C7C6 File Offset: 0x0014A9C6
		[JsonIgnore]
		public long LastTimeDBUpdateChecked
		{
			get
			{
				if (this._LastTimeDBUpdateChecked == null)
				{
					this._LastTimeDBUpdateChecked = new long?(this.AppSettings.GetValueOrDefault<long>("LastTimeDBUpdateChecked", 0L, null));
				}
				return this._LastTimeDBUpdateChecked.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("LastTimeDBUpdateChecked", value, null);
				this._LastTimeDBUpdateChecked = new long?(value);
				this.NotifyPropertyChanged("LastTimeDBUpdateChecked");
			}
		}

		// Token: 0x170010B5 RID: 4277
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x0014C7F1 File Offset: 0x0014A9F1
		// (set) Token: 0x06001E4C RID: 7756 RVA: 0x0014C828 File Offset: 0x0014AA28
		[JsonIgnore]
		public bool ForceProfileUpdateScheduled
		{
			get
			{
				if (this._ForceProfileUpdateScheduled == null)
				{
					this._ForceProfileUpdateScheduled = new bool?(this.AppSettings.GetValueOrDefault<bool>("ForceProfileUpdateScheduled", false, null));
				}
				return this._ForceProfileUpdateScheduled.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ForceProfileUpdateScheduled", value, null);
				this._ForceProfileUpdateScheduled = new bool?(value);
				this.NotifyPropertyChanged("ForceProfileUpdateScheduled");
			}
		}

		// Token: 0x170010B6 RID: 4278
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0014C853 File Offset: 0x0014AA53
		// (set) Token: 0x06001E4E RID: 7758 RVA: 0x0014C88B File Offset: 0x0014AA8B
		[JsonIgnore]
		public long LastTimeLicenceChecked
		{
			get
			{
				if (this._LastTimeLicenceChecked == null)
				{
					this._LastTimeLicenceChecked = new long?(this.AppSettings.GetValueOrDefault<long>("LastTimeLicenceChecked", 0L, null));
				}
				return this._LastTimeLicenceChecked.Value;
			}
			set
			{
				if (value == this.LastTimeLicenceChecked)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LastTimeLicenceChecked", value, null);
				this._LastTimeLicenceChecked = new long?(value);
				this.NotifyPropertyChanged("LastTimeLicenceChecked");
			}
		}

		// Token: 0x170010B7 RID: 4279
		// (get) Token: 0x06001E4F RID: 7759 RVA: 0x0014C8C0 File Offset: 0x0014AAC0
		// (set) Token: 0x06001E50 RID: 7760 RVA: 0x0014C8C8 File Offset: 0x0014AAC8
		[JsonIgnore]
		public bool IsRuIPCached
		{
			[CompilerGenerated]
			get
			{
				return this.<IsRuIPCached>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsRuIPCached>k__BackingField = value;
			}
		}

		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x06001E51 RID: 7761 RVA: 0x0014C8D1 File Offset: 0x0014AAD1
		// (set) Token: 0x06001E52 RID: 7762 RVA: 0x0014C8D9 File Offset: 0x0014AAD9
		[JsonIgnore]
		public bool DisplayRokodil
		{
			[CompilerGenerated]
			get
			{
				return this.<DisplayRokodil>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DisplayRokodil>k__BackingField = value;
			}
		}

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x06001E53 RID: 7763 RVA: 0x0014C8E2 File Offset: 0x0014AAE2
		// (set) Token: 0x06001E54 RID: 7764 RVA: 0x0014C919 File Offset: 0x0014AB19
		[JsonIgnore]
		public bool DisplayRokodilV2
		{
			get
			{
				if (this._DisplayRokodilV2 == null)
				{
					this._DisplayRokodilV2 = new bool?(this.AppSettings.GetValueOrDefault<bool>("DisplayRokodilV2", true, null));
				}
				return this._DisplayRokodilV2.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("DisplayRokodilV2", value, null);
				this._DisplayRokodilV2 = new bool?(value);
				this.NotifyPropertyChanged("DisplayRokodilV2");
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x06001E55 RID: 7765 RVA: 0x0014C944 File Offset: 0x0014AB44
		// (set) Token: 0x06001E56 RID: 7766 RVA: 0x0014C970 File Offset: 0x0014AB70
		[JsonIgnore]
		public string RokodilURL
		{
			get
			{
				if (this._RokodilURL == null)
				{
					this._RokodilURL = this.AppSettings.GetValueOrDefault<string>("RokodilURL", "https://obd.rokodil.ru/product/obd-scan-tool-rokodil-scanx/", null);
				}
				return this._RokodilURL;
			}
			set
			{
				if (value == this.RokodilURL)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("RokodilURL", value, null);
				this._RokodilURL = value;
				this.NotifyPropertyChanged("RokodilURL");
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06001E57 RID: 7767 RVA: 0x0014C9A5 File Offset: 0x0014ABA5
		// (set) Token: 0x06001E58 RID: 7768 RVA: 0x0014C9D1 File Offset: 0x0014ABD1
		public string PurchaseResponseCached
		{
			get
			{
				if (this._PurchaseResponseCached == null)
				{
					this._PurchaseResponseCached = this.AppSettings.GetValueOrDefault<string>("PurchaseResponseCached", "", null);
				}
				return this._PurchaseResponseCached;
			}
			set
			{
				if (value == this.PurchaseResponseCached)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("PurchaseResponseCached", value, null);
				this._PurchaseResponseCached = value;
				this.NotifyPropertyChanged("PurchaseResponseCached");
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06001E59 RID: 7769 RVA: 0x0014CA06 File Offset: 0x0014AC06
		// (set) Token: 0x06001E5A RID: 7770 RVA: 0x0014CA3D File Offset: 0x0014AC3D
		public bool UseServiceResponseForPositiveResponseMarker
		{
			get
			{
				if (this._UseServiceResponseForPositiveResponseMarker == null)
				{
					this._UseServiceResponseForPositiveResponseMarker = new bool?(this.AppSettings.GetValueOrDefault<bool>("UseServiceResponseForPositiveResponseMarker", true, null));
				}
				return this._UseServiceResponseForPositiveResponseMarker.Value;
			}
			set
			{
				if (value == this.UseServiceResponseForPositiveResponseMarker)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("UseServiceResponseForPositiveResponseMarker", value, null);
				this._UseServiceResponseForPositiveResponseMarker = new bool?(value);
				this.NotifyPropertyChanged("UseServiceResponseForPositiveResponseMarker");
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x0014CA72 File Offset: 0x0014AC72
		// (set) Token: 0x06001E5C RID: 7772 RVA: 0x0014CAA9 File Offset: 0x0014ACA9
		public bool BTLEShowDevicesWithoutName
		{
			get
			{
				if (this._BTLEShowDevicesWithoutName == null)
				{
					this._BTLEShowDevicesWithoutName = new bool?(this.AppSettings.GetValueOrDefault<bool>("BTLEShowDevicesWithoutName", false, null));
				}
				return this._BTLEShowDevicesWithoutName.Value;
			}
			set
			{
				if (value == this.BTLEShowDevicesWithoutName)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("BTLEShowDevicesWithoutName", value, null);
				this._BTLEShowDevicesWithoutName = new bool?(value);
				this.NotifyPropertyChanged("BTLEShowDevicesWithoutName");
			}
		}

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x0014CADE File Offset: 0x0014ACDE
		// (set) Token: 0x06001E5E RID: 7774 RVA: 0x0014CB15 File Offset: 0x0014AD15
		public bool AutomaticReoptimization
		{
			get
			{
				if (this._AutomaticReoptimization == null)
				{
					this._AutomaticReoptimization = new bool?(this.AppSettings.GetValueOrDefault<bool>("AutomaticReoptimization", true, null));
				}
				return this._AutomaticReoptimization.Value;
			}
			set
			{
				if (value == this.AutomaticReoptimization)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("AutomaticReoptimization", value, null);
				this._AutomaticReoptimization = new bool?(value);
				this.NotifyPropertyChanged("AutomaticReoptimization");
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06001E5F RID: 7775 RVA: 0x0014CB4A File Offset: 0x0014AD4A
		// (set) Token: 0x06001E60 RID: 7776 RVA: 0x0014CB81 File Offset: 0x0014AD81
		public bool LiveDataListPageUpdateOnlyVisible
		{
			get
			{
				if (this._LiveDataListPageUpdateOnlyVisible == null)
				{
					this._LiveDataListPageUpdateOnlyVisible = new bool?(this.AppSettings.GetValueOrDefault<bool>("LiveDataListPageUpdateOnlyVisible", true, null));
				}
				return this._LiveDataListPageUpdateOnlyVisible.Value;
			}
			set
			{
				if (value == this.LiveDataListPageUpdateOnlyVisible)
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("LiveDataListPageUpdateOnlyVisible", value, null);
				this._LiveDataListPageUpdateOnlyVisible = new bool?(value);
				this.NotifyPropertyChanged("LiveDataListPageUpdateOnlyVisible");
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06001E61 RID: 7777 RVA: 0x0014CBB6 File Offset: 0x0014ADB6
		// (set) Token: 0x06001E62 RID: 7778 RVA: 0x0014CBE2 File Offset: 0x0014ADE2
		public string VWTP_ChannelSetupATST
		{
			get
			{
				if (this._VWTP_ChannelSetupATST == null)
				{
					this._VWTP_ChannelSetupATST = this.AppSettings.GetValueOrDefault<string>("VWTP_ChannelSetupATST", "0A", null);
				}
				return this._VWTP_ChannelSetupATST;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("VWTP_ChannelSetupATST", value, null);
				this._VWTP_ChannelSetupATST = value;
				this.NotifyPropertyChanged("VWTP_ChannelSetupATST");
			}
		}

		// Token: 0x170010C1 RID: 4289
		// (get) Token: 0x06001E63 RID: 7779 RVA: 0x0014CC08 File Offset: 0x0014AE08
		// (set) Token: 0x06001E64 RID: 7780 RVA: 0x0014CC43 File Offset: 0x0014AE43
		public int VWTP_ActiveConnectionTestTimeout
		{
			get
			{
				if (this._VWTP_ActiveConnectionTestTimeout == null)
				{
					this._VWTP_ActiveConnectionTestTimeout = new int?(this.AppSettings.GetValueOrDefault<int>("VWTP_ActiveConnectionTestTimeout", 1000, null));
				}
				return this._VWTP_ActiveConnectionTestTimeout.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("VWTP_ActiveConnectionTestTimeout", value, null);
				this._VWTP_ActiveConnectionTestTimeout = new int?(value);
				this.NotifyPropertyChanged("VWTP_ActiveConnectionTestTimeout");
			}
		}

		// Token: 0x170010C2 RID: 4290
		// (get) Token: 0x06001E65 RID: 7781 RVA: 0x0014CC6E File Offset: 0x0014AE6E
		// (set) Token: 0x06001E66 RID: 7782 RVA: 0x0014CCA9 File Offset: 0x0014AEA9
		public int VWTP_SendConnectionConfirmationPeriod
		{
			get
			{
				if (this._VWTP_SendConnectionConfirmationPeriod == null)
				{
					this._VWTP_SendConnectionConfirmationPeriod = new int?(this.AppSettings.GetValueOrDefault<int>("VWTP_SendConnectionConfirmationPeriod", 600, null));
				}
				return this._VWTP_SendConnectionConfirmationPeriod.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("VWTP_SendConnectionConfirmationPeriod", value, null);
				this._VWTP_SendConnectionConfirmationPeriod = new int?(value);
				this.NotifyPropertyChanged("VWTP_SendConnectionConfirmationPeriod");
			}
		}

		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x0014CCD4 File Offset: 0x0014AED4
		// (set) Token: 0x06001E68 RID: 7784 RVA: 0x0014CD0C File Offset: 0x0014AF0C
		public int DataRecordLineSplitterTime
		{
			get
			{
				if (this._DataRecordLineSplitterTime == null)
				{
					this._DataRecordLineSplitterTime = new int?(this.AppSettings.GetValueOrDefault<int>("DataRecordLineSplitterTime", 30, null));
				}
				return this._DataRecordLineSplitterTime.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("DataRecordLineSplitterTime", value, null);
				this._DataRecordLineSplitterTime = new int?(value);
				this.NotifyPropertyChanged("DataRecordLineSplitterTime");
			}
		}

		// Token: 0x170010C4 RID: 4292
		// (get) Token: 0x06001E69 RID: 7785 RVA: 0x0014CD37 File Offset: 0x0014AF37
		// (set) Token: 0x06001E6A RID: 7786 RVA: 0x0014CD70 File Offset: 0x0014AF70
		public FlowControlOverrides FlowControlOverrideMode
		{
			get
			{
				if (this._FlowControlOverrideMode == null)
				{
					this._FlowControlOverrideMode = new FlowControlOverrides?((FlowControlOverrides)this.AppSettings.GetValueOrDefault<int>("FlowControlOverrideMode", 0, null));
				}
				return this._FlowControlOverrideMode.Value;
			}
			set
			{
				FlowControlOverrides? flowControlOverrideMode = this._FlowControlOverrideMode;
				if (!((flowControlOverrideMode.GetValueOrDefault() == value) & (flowControlOverrideMode != null)))
				{
					this.AppSettings.AddOrUpdateValue("FlowControlOverrideMode", (int)value, null);
					this._FlowControlOverrideMode = new FlowControlOverrides?(value);
					this.NotifyPropertyChanged("FlowControlOverrideMode");
				}
			}
		}

		// Token: 0x170010C5 RID: 4293
		// (get) Token: 0x06001E6B RID: 7787 RVA: 0x0014CDC3 File Offset: 0x0014AFC3
		// (set) Token: 0x06001E6C RID: 7788 RVA: 0x0014CDFC File Offset: 0x0014AFFC
		public bool Hstld4Apl
		{
			get
			{
				if (this._Hstld4Apl == null)
				{
					this._Hstld4Apl = new bool?(this.AppSettings.GetValueOrDefault<bool>("Hstld4Apl", false, null));
				}
				return this._Hstld4Apl.Value;
			}
			set
			{
				bool? hstld4Apl = this._Hstld4Apl;
				if ((value == hstld4Apl.GetValueOrDefault()) & (hstld4Apl != null))
				{
					return;
				}
				this.AppSettings.AddOrUpdateValue("Hstld4Apl", value, null);
				this._Hstld4Apl = new bool?(value);
				this.NotifyPropertyChanged("Hstld4Apl");
			}
		}

		// Token: 0x170010C6 RID: 4294
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x0014CE4E File Offset: 0x0014B04E
		// (set) Token: 0x06001E6E RID: 7790 RVA: 0x0014CE85 File Offset: 0x0014B085
		public int ELM327ConnectionAttempts
		{
			get
			{
				if (this._ELM327ConnectionAttempts == null)
				{
					this._ELM327ConnectionAttempts = new int?(this.AppSettings.GetValueOrDefault<int>("ELM327ConnectionAttempts", 3, null));
				}
				return this._ELM327ConnectionAttempts.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ELM327ConnectionAttempts", value, null);
				this._ELM327ConnectionAttempts = new int?(value);
				this.NotifyPropertyChanged("ELM327ConnectionAttempts");
			}
		}

		// Token: 0x170010C7 RID: 4295
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x0014CEB0 File Offset: 0x0014B0B0
		// (set) Token: 0x06001E70 RID: 7792 RVA: 0x0014CEE7 File Offset: 0x0014B0E7
		public bool EV_Power_InvertValue
		{
			get
			{
				if (this._EV_Power_InvertValue == null)
				{
					this._EV_Power_InvertValue = new bool?(this.AppSettings.GetValueOrDefault<bool>("EV_Power_InvertValue", false, null));
				}
				return this._EV_Power_InvertValue.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("EV_Power_InvertValue", value, null);
				this._EV_Power_InvertValue = new bool?(value);
				this.NotifyPropertyChanged("EV_Power_InvertValue");
			}
		}

		// Token: 0x170010C8 RID: 4296
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x0014CF12 File Offset: 0x0014B112
		// (set) Token: 0x06001E72 RID: 7794 RVA: 0x0014CF49 File Offset: 0x0014B149
		public bool SpeedCalibrationTaskPending
		{
			get
			{
				if (this._SpeedCalibrationTaskPending == null)
				{
					this._SpeedCalibrationTaskPending = new bool?(this.AppSettings.GetValueOrDefault<bool>("SpeedCalibrationTaskPending", false, null));
				}
				return this._SpeedCalibrationTaskPending.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("SpeedCalibrationTaskPending", value, null);
				this._SpeedCalibrationTaskPending = new bool?(value);
				this.NotifyPropertyChanged("SpeedCalibrationTaskPending");
			}
		}

		// Token: 0x170010C9 RID: 4297
		// (get) Token: 0x06001E73 RID: 7795 RVA: 0x0014CF74 File Offset: 0x0014B174
		// (set) Token: 0x06001E74 RID: 7796 RVA: 0x0014CFAC File Offset: 0x0014B1AC
		[JsonIgnore]
		public int FontSizePatch
		{
			get
			{
				if (this._FontSizePatch == null)
				{
					this._FontSizePatch = new int?(this.AppSettings.GetValueOrDefault<int>("FontSizePatch", 0, null));
				}
				return this._FontSizePatch.Value;
			}
			set
			{
				int? fontSizePatch = this._FontSizePatch;
				if (!((fontSizePatch.GetValueOrDefault() == value) & (fontSizePatch != null)))
				{
					this.AppSettings.AddOrUpdateValue("FontSizePatch", value, null);
					this._FontSizePatch = new int?(value);
					MainThread.BeginInvokeOnMainThread(delegate
					{
						this.NotifyPropertyChanged("FontSizePatch");
						App.ApplyFontSizePatch();
					});
				}
			}
		}

		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x06001E75 RID: 7797 RVA: 0x0014D008 File Offset: 0x0014B208
		// (set) Token: 0x06001E76 RID: 7798 RVA: 0x0014D080 File Offset: 0x0014B280
		public int AccelerationTestSpeedSourcePid
		{
			get
			{
				if (this._AccelerationTestSpeedSourcePid == null)
				{
					int num = 13;
					try
					{
						IPIDFloatValue ipidfloatValue = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
						if (ipidfloatValue != null)
						{
							num = ipidfloatValue.Id;
						}
					}
					catch (Exception)
					{
					}
					this._AccelerationTestSpeedSourcePid = new int?(this.AppSettings.GetValueOrDefault<int>("AccelerationTestSpeedSourcePid", num, null));
				}
				return this._AccelerationTestSpeedSourcePid.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("AccelerationTestSpeedSourcePid", value, null);
				this._AccelerationTestSpeedSourcePid = new int?(value);
				this.NotifyPropertyChanged("AccelerationTestSpeedSourcePid");
			}
		}

		// Token: 0x170010CB RID: 4299
		// (get) Token: 0x06001E77 RID: 7799 RVA: 0x0014D0AB File Offset: 0x0014B2AB
		// (set) Token: 0x06001E78 RID: 7800 RVA: 0x0014D0E2 File Offset: 0x0014B2E2
		public bool CheckOnlyPositiveResponseMarker
		{
			get
			{
				if (this._CheckOnlyPositiveResponseMarker == null)
				{
					this._CheckOnlyPositiveResponseMarker = new bool?(this.AppSettings.GetValueOrDefault<bool>("CheckOnlyPositiveResponseMarker", false, null));
				}
				return this._CheckOnlyPositiveResponseMarker.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("CheckOnlyPositiveResponseMarker", value, null);
				this._CheckOnlyPositiveResponseMarker = new bool?(value);
				this.NotifyPropertyChanged("CheckOnlyPositiveResponseMarker");
			}
		}

		// Token: 0x170010CC RID: 4300
		// (get) Token: 0x06001E79 RID: 7801 RVA: 0x0014D10D File Offset: 0x0014B30D
		// (set) Token: 0x06001E7A RID: 7802 RVA: 0x0014D144 File Offset: 0x0014B344
		public bool PIDSelectorWithValuePreview
		{
			get
			{
				if (this._PIDSelectorWithValuePreview == null)
				{
					this._PIDSelectorWithValuePreview = new bool?(this.AppSettings.GetValueOrDefault<bool>("PIDSelectorWithValuePreview", true, null));
				}
				return this._PIDSelectorWithValuePreview.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("PIDSelectorWithValuePreview", value, null);
				this._PIDSelectorWithValuePreview = new bool?(value);
				this.NotifyPropertyChanged("PIDSelectorWithValuePreview");
			}
		}

		// Token: 0x170010CD RID: 4301
		// (get) Token: 0x06001E7B RID: 7803 RVA: 0x0014D16F File Offset: 0x0014B36F
		// (set) Token: 0x06001E7C RID: 7804 RVA: 0x0014D1A8 File Offset: 0x0014B3A8
		public int StopConnectionAttemptsAfterFailsMinutes
		{
			get
			{
				if (this._StopConnectionAttemptsAfterFailsMinutes == null)
				{
					this._StopConnectionAttemptsAfterFailsMinutes = new int?(this.AppSettings.GetValueOrDefault<int>("StopConnectionAttemptsAfterFailsMinutes", 25, null));
				}
				return this._StopConnectionAttemptsAfterFailsMinutes.Value;
			}
			set
			{
				int? stopConnectionAttemptsAfterFailsMinutes = this._StopConnectionAttemptsAfterFailsMinutes;
				if (!((value == stopConnectionAttemptsAfterFailsMinutes.GetValueOrDefault()) & (stopConnectionAttemptsAfterFailsMinutes != null)))
				{
					this.AppSettings.AddOrUpdateValue("StopConnectionAttemptsAfterFailsMinutes", value, null);
					this._StopConnectionAttemptsAfterFailsMinutes = new int?(value);
					this.NotifyPropertyChanged("StopConnectionAttemptsAfterFailsMinutes");
				}
			}
		}

		// Token: 0x170010CE RID: 4302
		// (get) Token: 0x06001E7D RID: 7805 RVA: 0x0014D1F9 File Offset: 0x0014B3F9
		// (set) Token: 0x06001E7E RID: 7806 RVA: 0x0014D230 File Offset: 0x0014B430
		public bool AllowProfilesBackgroundUpdate
		{
			get
			{
				if (this._AllowProfilesBackgroundUpdate == null)
				{
					this._AllowProfilesBackgroundUpdate = new bool?(this.AppSettings.GetValueOrDefault<bool>("AllowProfilesBackgroundUpdate", true, null));
				}
				return this._AllowProfilesBackgroundUpdate.Value;
			}
			set
			{
				if (value != this.AllowProfilesBackgroundUpdate)
				{
					this.AppSettings.AddOrUpdateValue("AllowProfilesBackgroundUpdate", value, null);
					this._AllowProfilesBackgroundUpdate = new bool?(value);
					if (!value)
					{
						ProfileV2Model.DeleteUpdated();
					}
					SharedSettings.Current.ForceProfileUpdateScheduled = true;
					this.NotifyPropertyChanged("AllowProfilesBackgroundUpdate");
				}
			}
		}

		// Token: 0x170010CF RID: 4303
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x0014D282 File Offset: 0x0014B482
		// (set) Token: 0x06001E80 RID: 7808 RVA: 0x0014D2B9 File Offset: 0x0014B4B9
		public bool iOSDashboardUseFullscreen
		{
			get
			{
				if (this._DashboardiOSUseFullscreen == null)
				{
					this._DashboardiOSUseFullscreen = new bool?(this.AppSettings.GetValueOrDefault<bool>("iOSDashboardUseFullscreen", false, null));
				}
				return this._DashboardiOSUseFullscreen.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("iOSDashboardUseFullscreen", value, null);
				this._DashboardiOSUseFullscreen = new bool?(value);
				this.NotifyPropertyChanged("iOSDashboardUseFullscreen");
			}
		}

		// Token: 0x170010D0 RID: 4304
		// (get) Token: 0x06001E81 RID: 7809 RVA: 0x0014D2E4 File Offset: 0x0014B4E4
		// (set) Token: 0x06001E82 RID: 7810 RVA: 0x0014D34D File Offset: 0x0014B54D
		public bool AddNissanConsult3Pids
		{
			get
			{
				if (this._AddNissanConsult3Pids == null)
				{
					this._AddNissanConsult3Pids = new bool?(this.AppSettings.GetValueOrDefault<bool>("AddNissanConsult3Pids", this.SelectedBrand == "Nissan" || this.SelectedBrand == "Infiniti", null));
				}
				return this._AddNissanConsult3Pids.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("AddNissanConsult3Pids", value, null);
				this._AddNissanConsult3Pids = new bool?(value);
				this.NotifyPropertyChanged("AddNissanConsult3Pids");
			}
		}

		// Token: 0x170010D1 RID: 4305
		// (get) Token: 0x06001E83 RID: 7811 RVA: 0x0014D378 File Offset: 0x0014B578
		// (set) Token: 0x06001E84 RID: 7812 RVA: 0x0014D3AF File Offset: 0x0014B5AF
		[JsonIgnore]
		public bool WhitelistDeviceActivated
		{
			get
			{
				if (this._WhitelistDeviceActivated == null)
				{
					this._WhitelistDeviceActivated = new bool?(this.AppSettings.GetValueOrDefault<bool>("WhitelistDeviceActivated", false, null));
				}
				return this._WhitelistDeviceActivated.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("WhitelistDeviceActivated", value, null);
				this._WhitelistDeviceActivated = new bool?(value);
				this.NotifyPropertyChanged("WhitelistDeviceActivated");
			}
		}

		// Token: 0x170010D2 RID: 4306
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x0014D3DA File Offset: 0x0014B5DA
		// (set) Token: 0x06001E86 RID: 7814 RVA: 0x0014D406 File Offset: 0x0014B606
		[JsonIgnore]
		public string WhitelistDeviceSN
		{
			get
			{
				if (this._WhitelistDeviceSN == null)
				{
					this._WhitelistDeviceSN = this.AppSettings.GetValueOrDefault<string>("WhitelistDeviceSN", "", null);
				}
				return this._WhitelistDeviceSN;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("WhitelistDeviceSN", value, null);
				this._WhitelistDeviceSN = value;
				this.NotifyPropertyChanged("WhitelistDeviceSN");
			}
		}

		// Token: 0x170010D3 RID: 4307
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x0014D42C File Offset: 0x0014B62C
		// (set) Token: 0x06001E88 RID: 7816 RVA: 0x0014D463 File Offset: 0x0014B663
		public bool IgnoreProfilePidsTestScheduled
		{
			get
			{
				if (this._IgnoreProfilePidsTestScheduled == null)
				{
					this._IgnoreProfilePidsTestScheduled = new bool?(this.AppSettings.GetValueOrDefault<bool>("IgnoreProfilePidsTestScheduled", false, null));
				}
				return this._IgnoreProfilePidsTestScheduled.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("IgnoreProfilePidsTestScheduled", value, null);
				this._IgnoreProfilePidsTestScheduled = new bool?(value);
				this.NotifyPropertyChanged("IgnoreProfilePidsTestScheduled");
			}
		}

		// Token: 0x170010D4 RID: 4308
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x0014D490 File Offset: 0x0014B690
		// (set) Token: 0x06001E8A RID: 7818 RVA: 0x0014D4E8 File Offset: 0x0014B6E8
		public int CarPlayUpdateInterval
		{
			get
			{
				if (this._CarPlayUpdateInterval == null)
				{
					int num = (PlatformHelper.IsiOS ? 300 : 500);
					this._CarPlayUpdateInterval = new int?(this.AppSettings.GetValueOrDefault<int>("CarPlayUpdateInterval", num, null));
				}
				return this._CarPlayUpdateInterval.Value;
			}
			set
			{
				if (value < 50)
				{
					return;
				}
				int? carPlayUpdateInterval = this._CarPlayUpdateInterval;
				if (!((value == carPlayUpdateInterval.GetValueOrDefault()) & (carPlayUpdateInterval != null)))
				{
					this.AppSettings.AddOrUpdateValue("CarPlayUpdateInterval", value, null);
					this._CarPlayUpdateInterval = new int?(value);
					this.NotifyPropertyChanged("CarPlayUpdateInterval");
				}
			}
		}

		// Token: 0x170010D5 RID: 4309
		// (get) Token: 0x06001E8B RID: 7819 RVA: 0x0014D53F File Offset: 0x0014B73F
		// (set) Token: 0x06001E8C RID: 7820 RVA: 0x0014D576 File Offset: 0x0014B776
		public int CarPlayDashboardSelectedIndex
		{
			get
			{
				if (this._CarPlayDashboardSelectedIndex == null)
				{
					this._CarPlayDashboardSelectedIndex = new int?(this.AppSettings.GetValueOrDefault<int>("CarPlayDashboardSelectedIndex", 0, null));
				}
				return this._CarPlayDashboardSelectedIndex.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("CarPlayDashboardSelectedIndex", value, null);
				this._CarPlayDashboardSelectedIndex = new int?(value);
				this.NotifyPropertyChanged("CarPlayDashboardSelectedIndex");
			}
		}

		// Token: 0x170010D6 RID: 4310
		// (get) Token: 0x06001E8D RID: 7821 RVA: 0x0014D5A1 File Offset: 0x0014B7A1
		// (set) Token: 0x06001E8E RID: 7822 RVA: 0x0014D5CD File Offset: 0x0014B7CD
		public string BannedVersions
		{
			get
			{
				if (this._BannedVersions == null)
				{
					this._BannedVersions = this.AppSettings.GetValueOrDefault<string>("BannedVersions", "", null);
				}
				return this._BannedVersions;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("BannedVersions", value, null);
				this._BannedVersions = value;
				this.NotifyPropertyChanged("BannedVersions");
			}
		}

		// Token: 0x170010D7 RID: 4311
		// (get) Token: 0x06001E8F RID: 7823 RVA: 0x0014D5F3 File Offset: 0x0014B7F3
		// (set) Token: 0x06001E90 RID: 7824 RVA: 0x0014D62A File Offset: 0x0014B82A
		public bool NissanConsult3OpenCloseSession
		{
			get
			{
				if (this._NissanConsult3OpenCloseSession == null)
				{
					this._NissanConsult3OpenCloseSession = new bool?(this.AppSettings.GetValueOrDefault<bool>("NissanConsult3OpenCloseSession", true, null));
				}
				return this._NissanConsult3OpenCloseSession.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("NissanConsult3OpenCloseSession", value, null);
				this._NissanConsult3OpenCloseSession = new bool?(value);
				this.NotifyPropertyChanged("NissanConsult3OpenCloseSession");
			}
		}

		// Token: 0x170010D8 RID: 4312
		// (get) Token: 0x06001E91 RID: 7825 RVA: 0x0014D655 File Offset: 0x0014B855
		// (set) Token: 0x06001E92 RID: 7826 RVA: 0x0014D68C File Offset: 0x0014B88C
		public bool ProfileHasPids
		{
			get
			{
				if (this._ProfileHasPids == null)
				{
					this._ProfileHasPids = new bool?(this.AppSettings.GetValueOrDefault<bool>("ProfileHasPids", false, null));
				}
				return this._ProfileHasPids.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ProfileHasPids", value, null);
				this._ProfileHasPids = new bool?(value);
				this.NotifyPropertyChanged("ProfileHasPids");
			}
		}

		// Token: 0x170010D9 RID: 4313
		// (get) Token: 0x06001E93 RID: 7827 RVA: 0x0014D6B7 File Offset: 0x0014B8B7
		// (set) Token: 0x06001E94 RID: 7828 RVA: 0x0014D6EE File Offset: 0x0014B8EE
		public bool DroidGPCheckCurrencyR
		{
			get
			{
				if (this._DroidGPCheckCurrencyR == null)
				{
					this._DroidGPCheckCurrencyR = new bool?(this.AppSettings.GetValueOrDefault<bool>("DroidGPCheckCurrencyR", true, null));
				}
				return this._DroidGPCheckCurrencyR.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("DroidGPCheckCurrencyR", value, null);
				this._DroidGPCheckCurrencyR = new bool?(value);
				this.NotifyPropertyChanged("DroidGPCheckCurrencyR");
			}
		}

		// Token: 0x170010DA RID: 4314
		// (get) Token: 0x06001E95 RID: 7829 RVA: 0x0014D719 File Offset: 0x0014B919
		// (set) Token: 0x06001E96 RID: 7830 RVA: 0x0014D750 File Offset: 0x0014B950
		public bool ShowConnectionStatusOverlay
		{
			get
			{
				if (this._ShowConnectionStatusOverlay == null)
				{
					this._ShowConnectionStatusOverlay = new bool?(this.AppSettings.GetValueOrDefault<bool>("ShowConnectionStatusOverlay", false, null));
				}
				return this._ShowConnectionStatusOverlay.Value;
			}
			set
			{
				this.AppSettings.AddOrUpdateValue("ShowConnectionStatusOverlay", value, null);
				this._ShowConnectionStatusOverlay = new bool?(value);
				this.NotifyPropertyChanged("ShowConnectionStatusOverlay");
			}
		}

		// Token: 0x06001E97 RID: 7831 RVA: 0x0014D77C File Offset: 0x0014B97C
		public async Task<string> GetSettingsReport()
		{
			StringBuilder sb = new StringBuilder(256);
			sb.Append("[Car Scanner settings:]\n");
			foreach (PropertyInfo propertyInfo in from x in base.GetType().GetProperties()
				orderby x.Name
				select x)
			{
				try
				{
					if (!SharedSettings.SkipListBase.Contains(propertyInfo.Name))
					{
						if (!PlatformHelper.IsiOS || (!propertyInfo.Name.StartsWith("android", StringComparison.InvariantCultureIgnoreCase) && !propertyInfo.Name.StartsWith("droid", StringComparison.InvariantCultureIgnoreCase)))
						{
							sb.Append(propertyInfo.Name);
							sb.Append("=");
							object value = propertyInfo.GetValue(this);
							sb.Append((value == null) ? "null" : value.ToString());
							sb.Append("\r\n");
						}
					}
				}
				catch (Exception)
				{
				}
			}
			try
			{
				string text = "\r\nOverride_roles=";
				foreach (int num in PIDOverrideDictionary.Instance.Keys)
				{
					PIDOverride pidoverride = PIDOverrideDictionary.Instance[num];
					if (pidoverride != null)
					{
						text += string.Format("{0}={1};", pidoverride.ID, pidoverride.Role);
					}
				}
				foreach (CustomPID customPID in CustomPIDViewModel.CurrentCustom.PidCollection)
				{
					if (customPID != null && customPID.Role != Roles.UNDEFINED)
					{
						text += string.Format("{0}={1};", customPID.Id, customPID.Role);
					}
				}
				text += "\r\n";
				sb.Append(text);
			}
			catch (Exception)
			{
			}
			try
			{
				if (!CustomPIDViewModel.DisabledProfile.Loaded)
				{
					CustomPIDViewModel.DisabledProfile.Load();
				}
				if (CustomPIDViewModel.DisabledProfile.PidCollection != null && CustomPIDViewModel.DisabledProfile.PidCollection.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder(CustomPIDViewModel.DisabledProfile.PidCollection.Count + 1);
					stringBuilder.Append("\r\nDisabledPIDs: ");
					foreach (CustomPID customPID2 in CustomPIDViewModel.DisabledProfile.PidCollection)
					{
						stringBuilder.Append(customPID2.Id.ToString() + ";");
					}
					sb.Append(stringBuilder.ToString());
				}
			}
			catch (Exception)
			{
			}
			sb.Append("ATST=");
			sb.Append(this.GetATST());
			sb.Append("\r\n");
			sb.Append("P=");
			sb.Append(this.AdsProductPurchased.ToString());
			sb.Append("\r\n");
			sb.Append("FPGC=");
			sb.Append(this.FreePeriodGoodConnections.ToString());
			sb.Append("\r\n");
			sb.Append("CC=");
			sb.Append(this.CodingsCounter.ToString());
			sb.Append("\r\n");
			sb.Append("\r\nOS version=" + DeviceInfo.VersionString);
			sb.Append("\r\nPlatform=" + DeviceInfo.Platform.ToString());
			sb.Append("\r\nIdiom=" + DeviceInfo.Idiom.ToString());
			sb.Append("\r\nDeviceModel=" + DeviceInfo.Model);
			sb.Append("\r\nDeviceManufacturer=" + DeviceInfo.Manufacturer + "\r\n");
			try
			{
				sb.Append("\r\nOrigCulture=" + CarScannerXamarinForms.Language.origCulture.ToString());
				sb.Append("\r\nOrigUICulture=" + CarScannerXamarinForms.Language.origUICulture.ToString());
			}
			catch (Exception)
			{
			}
			if (PlatformHelper.IsAndroid)
			{
				sb.Append("\r\nLocationPermission=" + PlatformHelper.DroidService.HasLocationPermission.ToString() + "\r\n");
				sb.Append("StoragePermission=" + PlatformHelper.DroidService.HasStoragePermission.ToString() + "\r\n");
				sb.Append("BluetoothPermission=" + PlatformHelper.DroidService.HasBluetoothPermission.ToString() + "\r\n");
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					sb.Append("NearbyDevicesPermission=" + (await PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async()).ToString());
				}
			}
			else
			{
				sb.Append("\r\nRunningOnMac=" + PlatformHelper.IOSService.IsiOSApplicationOnMac.ToString());
			}
			sb.Append(string.Concat(new string[]
			{
				"\r\nVersion=",
				App.Version,
				"/",
				App.Build,
				"\r\n"
			}));
			sb.Append("\r\nPrVersion=" + ProfileV2Model.GetCurrentVersion() + "\r\n");
			switch (PlatformHelper.AppMarket)
			{
			case Markets.AppStore:
				sb.Append("#iOS");
				break;
			case Markets.GooglePlay:
				sb.Append("#Gplay");
				break;
			case Markets.HMS:
				sb.Append("#HUAWEI");
				break;
			case Markets.Rustore:
				sb.Append("#RuStore");
				break;
			case Markets.RUS:
				sb.Append("#RUS");
				break;
			case Markets.Sideload:
				sb.Append("#SL");
				break;
			}
			return sb.ToString();
		}

		// Token: 0x06001E98 RID: 7832 RVA: 0x0014D7C0 File Offset: 0x0014B9C0
		// Note: this type is marked as 'beforefieldinit'.
		static SharedSettings()
		{
		}

		// Token: 0x06001E99 RID: 7833 RVA: 0x0014DAC4 File Offset: 0x0014BCC4
		[CompilerGenerated]
		private async void <set_UseGPS>b__74_0()
		{
			TaskAwaiter<bool> taskAwaiter = App.GetCurrentPage().DisplayAlert(Translate.GetString("droid_BackgroundGPSTitle"), Translate.GetString("droid_BackgroundGPS"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				await this.RequestLocationPermissionAsync();
			}
			else
			{
				this.UseGPS = false;
			}
		}

		// Token: 0x06001E9A RID: 7834 RVA: 0x0014DAFB File Offset: 0x0014BCFB
		[CompilerGenerated]
		private bool <GetInjectorPID>b__593_1(CustomPID x)
		{
			return x.Id == this.InjectorPid;
		}

		// Token: 0x06001E9B RID: 7835 RVA: 0x0014DAFB File Offset: 0x0014BCFB
		[CompilerGenerated]
		private bool <GetInjectorPID>b__593_2(CustomPID x)
		{
			return x.Id == this.InjectorPid;
		}

		// Token: 0x06001E9C RID: 7836 RVA: 0x0014DB0C File Offset: 0x0014BD0C
		[CompilerGenerated]
		private void <AndroidSetNavbarColor>b__674_0()
		{
			if (this.AndroidRecolorNavBar)
			{
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
				{
					Color color = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
					PlatformHelper.DroidService.SetNavigationBarColor(color);
					return;
				}
			}
			else if (PlatformHelper.IsPlatformVersionNewerOrEqual(21, 0))
			{
				PlatformHelper.DroidService.SetNavigationBarColor(App.OriginalNavBarColor);
			}
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0014DB6A File Offset: 0x0014BD6A
		[CompilerGenerated]
		private void <set_FontSizePatch>b__1142_0()
		{
			this.NotifyPropertyChanged("FontSizePatch");
			App.ApplyFontSizePatch();
		}

		// Token: 0x04000DAA RID: 3498
		private static SharedSettings _Current = null;

		// Token: 0x04000DAB RID: 3499
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04000DAC RID: 3500
		private bool? _MigratedToSettingsV2;

		// Token: 0x04000DAD RID: 3501
		[JsonIgnore]
		public SettingsV2 AppSettings = new SettingsV2();

		// Token: 0x04000DAE RID: 3502
		private DateTime? _LicenseFinishDate;

		// Token: 0x04000DAF RID: 3503
		[JsonIgnore]
		public const int FPGC_LIMIT = 15;

		// Token: 0x04000DB0 RID: 3504
		[JsonIgnore]
		public const int CODING_LIMIT = 3;

		// Token: 0x04000DB1 RID: 3505
		private bool? _ShowWhenTrialExpires;

		// Token: 0x04000DB2 RID: 3506
		private int? _TrialExtentions;

		// Token: 0x04000DB3 RID: 3507
		private bool? _AdsProductPurchased;

		// Token: 0x04000DB4 RID: 3508
		private long? _FreePeriodFinishTime;

		// Token: 0x04000DB5 RID: 3509
		private bool? _SendStatistics;

		// Token: 0x04000DB6 RID: 3510
		private string _AskedForUpdateToVersion;

		// Token: 0x04000DB7 RID: 3511
		private string _LatestVersion;

		// Token: 0x04000DB8 RID: 3512
		private string _MinVersion;

		// Token: 0x04000DB9 RID: 3513
		private string _AvailableVersion;

		// Token: 0x04000DBA RID: 3514
		private bool? _GDPR_Asked;

		// Token: 0x04000DBB RID: 3515
		private bool? _GDPR_ShowPersonalyzed;

		// Token: 0x04000DBC RID: 3516
		private string _LastException;

		// Token: 0x04000DBD RID: 3517
		[CompilerGenerated]
		private bool <FlushLog>k__BackingField;

		// Token: 0x04000DBE RID: 3518
		private bool? _UseGPS;

		// Token: 0x04000DBF RID: 3519
		private bool? _UseGPSForFuelConsumption;

		// Token: 0x04000DC0 RID: 3520
		private bool? _ShowExperimental;

		// Token: 0x04000DC1 RID: 3521
		private string _PurchaseToken;

		// Token: 0x04000DC2 RID: 3522
		private string _PurchaseProductId;

		// Token: 0x04000DC3 RID: 3523
		private string _PurchaseOrderId;

		// Token: 0x04000DC4 RID: 3524
		private bool? _PurchaseDebug;

		// Token: 0x04000DC5 RID: 3525
		private bool? _ReviewReceived;

		// Token: 0x04000DC6 RID: 3526
		private int? _AskForReviewGoodConnections;

		// Token: 0x04000DC7 RID: 3527
		private int? _CustomPidLastId;

		// Token: 0x04000DC8 RID: 3528
		private string _SpeedTestDB;

		// Token: 0x04000DC9 RID: 3529
		private int? _NoDataLimit;

		// Token: 0x04000DCA RID: 3530
		private int? _DelayBeforeReconnect;

		// Token: 0x04000DCB RID: 3531
		private bool? _DisableOptimizationIfItFails;

		// Token: 0x04000DCC RID: 3532
		private string _SelectedProfileV2Name;

		// Token: 0x04000DCD RID: 3533
		private string _SelectedBrand;

		// Token: 0x04000DCE RID: 3534
		private string _BrandForDTC;

		// Token: 0x04000DCF RID: 3535
		private string _ProfileUpdateAlias;

		// Token: 0x04000DD0 RID: 3536
		private bool? _CheckIsELMWhenConnecting;

		// Token: 0x04000DD1 RID: 3537
		private bool? _BindSocketToWiFiNetwork;

		// Token: 0x04000DD2 RID: 3538
		private bool? _BindSocketToFirstWiFiNetwork;

		// Token: 0x04000DD3 RID: 3539
		private bool? _TurnOffBluetoothIfWasTurnedOn;

		// Token: 0x04000DD4 RID: 3540
		private AndroidWiFiConnectionModes? _AndroidWiFiMode;

		// Token: 0x04000DD5 RID: 3541
		private bool? _CheckBluetoothTurnedOn;

		// Token: 0x04000DD6 RID: 3542
		private bool? _RequestECUInfo;

		// Token: 0x04000DD7 RID: 3543
		private bool? _DroidTryTurnOnBluetooth;

		// Token: 0x04000DD8 RID: 3544
		private bool? _DroidAskForBluetoothPermissions;

		// Token: 0x04000DD9 RID: 3545
		private bool? _AndroidStartBackgroundService;

		// Token: 0x04000DDA RID: 3546
		private bool? _DroidDisplayConnectionStatusInStatusBar;

		// Token: 0x04000DDB RID: 3547
		private bool? _SendDefaultFunctionalHeader;

		// Token: 0x04000DDC RID: 3548
		private int? _AndroidBluetoothConnectionMethod;

		// Token: 0x04000DDD RID: 3549
		private bool? _NoDelayELM327Init;

		// Token: 0x04000DDE RID: 3550
		private bool? _ToyotaJDMMode21;

		// Token: 0x04000DDF RID: 3551
		private bool? _DaihatsuKLine;

		// Token: 0x04000DE0 RID: 3552
		private string _BTDeviceID;

		// Token: 0x04000DE1 RID: 3553
		private string _BTDeviceName;

		// Token: 0x04000DE2 RID: 3554
		private int? _OptimizedRequestStuckCounter;

		// Token: 0x04000DE3 RID: 3555
		private bool? _CANOptimizationWarningShowed;

		// Token: 0x04000DE4 RID: 3556
		private int? _CANOptimizeMode22MaxInRequest;

		// Token: 0x04000DE5 RID: 3557
		private int? _CANOptimizeMaxInRequest;

		// Token: 0x04000DE6 RID: 3558
		private bool? _CANOptimizeRequests;

		// Token: 0x04000DE7 RID: 3559
		private bool? _CANOptimizeMode22;

		// Token: 0x04000DE8 RID: 3560
		private bool? _CANOptimizeMode22SelfLearningMode;

		// Token: 0x04000DE9 RID: 3561
		private bool? _ShouldCheckProfilePIDs;

		// Token: 0x04000DEA RID: 3562
		private bool? _PerformSensorsScanByTesting;

		// Token: 0x04000DEB RID: 3563
		private string _BTLEDeviceID;

		// Token: 0x04000DEC RID: 3564
		private string _BTLEDeviceName;

		// Token: 0x04000DED RID: 3565
		private string _BTLEServiceID;

		// Token: 0x04000DEE RID: 3566
		private string _BTLEOutputID;

		// Token: 0x04000DEF RID: 3567
		private string _BTLEInputID;

		// Token: 0x04000DF0 RID: 3568
		private ConnectionTypes? _ConnectionType;

		// Token: 0x04000DF1 RID: 3569
		private bool? _FirstConnectionAttempted;

		// Token: 0x04000DF2 RID: 3570
		private bool? _DecodeMUT2Compatible;

		// Token: 0x04000DF3 RID: 3571
		private int? _CodingsCounter;

		// Token: 0x04000DF4 RID: 3572
		private int? _FreePeriodGoodConnections;

		// Token: 0x04000DF5 RID: 3573
		private bool? _OpenDashboardOnLaunch;

		// Token: 0x04000DF6 RID: 3574
		private bool? _ConnectOnLaunch;

		// Token: 0x04000DF7 RID: 3575
		private string _SelectedProfileName;

		// Token: 0x04000DF8 RID: 3576
		private bool? _UseDefaultInit;

		// Token: 0x04000DF9 RID: 3577
		private int? _IOTimeout;

		// Token: 0x04000DFA RID: 3578
		private bool? _UseOBD2;

		// Token: 0x04000DFB RID: 3579
		private string _DefaultFunctionalHeader;

		// Token: 0x04000DFC RID: 3580
		private int? _SendDelay;

		// Token: 0x04000DFD RID: 3581
		private string _TesterPresentCommand;

		// Token: 0x04000DFE RID: 3582
		private bool? _AlwaysPingECU;

		// Token: 0x04000DFF RID: 3583
		private int? _ProtocolNumber;

		// Token: 0x04000E00 RID: 3584
		private string _CustomInitString;

		// Token: 0x04000E01 RID: 3585
		private string _DetectECUConnectionPID;

		// Token: 0x04000E02 RID: 3586
		private int? _ATSTIdx;

		// Token: 0x04000E03 RID: 3587
		private int? _AdaptiveTimings;

		// Token: 0x04000E04 RID: 3588
		private DTCReadingMode? _DTCMode;

		// Token: 0x04000E05 RID: 3589
		private string _WiFiServer;

		// Token: 0x04000E06 RID: 3590
		private string _WiFiPort;

		// Token: 0x04000E07 RID: 3591
		private bool? _ShowBadELMWarning;

		// Token: 0x04000E08 RID: 3592
		private FuelConsumptionUnits? _FuelConsumptionUnit;

		// Token: 0x04000E09 RID: 3593
		private bool? _UseLitersForVolume;

		// Token: 0x04000E0A RID: 3594
		private bool? _UseL100ForFuel;

		// Token: 0x04000E0B RID: 3595
		private bool? _UseHoursePower;

		// Token: 0x04000E0C RID: 3596
		private bool? _UseNmForTorque;

		// Token: 0x04000E0D RID: 3597
		private bool? _AccelerationUseG;

		// Token: 0x04000E0E RID: 3598
		private bool? _Use_km;

		// Token: 0x04000E0F RID: 3599
		private bool? _Pressure_use_kpa;

		// Token: 0x04000E10 RID: 3600
		private bool? _Flow_use_grams_sec;

		// Token: 0x04000E11 RID: 3601
		private bool? _Use_celcium;

		// Token: 0x04000E12 RID: 3602
		private bool? _UseUSGallon;

		// Token: 0x04000E13 RID: 3603
		private bool? _ShowCodingAndService;

		// Token: 0x04000E14 RID: 3604
		private int? _AccelerationItems;

		// Token: 0x04000E15 RID: 3605
		private bool? _SpeedPID2Bytes;

		// Token: 0x04000E16 RID: 3606
		private bool? _KWPConcatResponseLines;

		// Token: 0x04000E17 RID: 3607
		private double? _CustomFuelAF;

		// Token: 0x04000E18 RID: 3608
		private double? _CustomFuelDensity;

		// Token: 0x04000E19 RID: 3609
		private bool? _FilterRPM300;

		// Token: 0x04000E1A RID: 3610
		private double? _SpeedCorrectionFactor;

		// Token: 0x04000E1B RID: 3611
		private bool? _IgnoreSupportedFlagForPIDs0166_0183;

		// Token: 0x04000E1C RID: 3612
		private BoostCalculationMethods? _BoostCalculationMethod;

		// Token: 0x04000E1D RID: 3613
		private bool? _HideDTCWithUncomplitedTests;

		// Token: 0x04000E1E RID: 3614
		private string _SkipHeadersDTC;

		// Token: 0x04000E1F RID: 3615
		private DateTime? _CalibrationStartTime;

		// Token: 0x04000E20 RID: 3616
		private double? _CalibrationStartOdometer;

		// Token: 0x04000E21 RID: 3617
		private bool? _ZeroConsumptionWhenZero;

		// Token: 0x04000E22 RID: 3618
		private int? _ZeroConsumptionPIDId;

		// Token: 0x04000E23 RID: 3619
		private List<PID> _ZeroConsumptionPIDCollection;

		// Token: 0x04000E24 RID: 3620
		private bool? _UseCustomPIDForZeroConsumption;

		// Token: 0x04000E25 RID: 3621
		private decimal? _FuelPriceForLitre;

		// Token: 0x04000E26 RID: 3622
		private string _Currency;

		// Token: 0x04000E27 RID: 3623
		private bool? _FilterWrongAFValues;

		// Token: 0x04000E28 RID: 3624
		private bool? _AlwaysRecordFuelConsumption;

		// Token: 0x04000E29 RID: 3625
		private int? _MergeDriveCyclesTime;

		// Token: 0x04000E2A RID: 3626
		private double? _CurbWeight;

		// Token: 0x04000E2B RID: 3627
		private double? _PassengersWeight;

		// Token: 0x04000E2C RID: 3628
		private double? _AdditionalWeight;

		// Token: 0x04000E2D RID: 3629
		private double? _DragCoefficient;

		// Token: 0x04000E2E RID: 3630
		private double? _DragArea;

		// Token: 0x04000E2F RID: 3631
		private double? _TireResistance;

		// Token: 0x04000E30 RID: 3632
		private double? _AmbientTemperature;

		// Token: 0x04000E31 RID: 3633
		private double? _BarometricPressure;

		// Token: 0x04000E32 RID: 3634
		private double? _FuelInTankVolume;

		// Token: 0x04000E33 RID: 3635
		private double? _DriverWeight;

		// Token: 0x04000E34 RID: 3636
		private bool? _ShowAirFuelBasedOnStoichiometric;

		// Token: 0x04000E35 RID: 3637
		private double? _InjectorFlow;

		// Token: 0x04000E36 RID: 3638
		private double? _FuelFlowCorrectionFactor;

		// Token: 0x04000E37 RID: 3639
		private int? _InjectorPid;

		// Token: 0x04000E38 RID: 3640
		private bool? _FuelFlowUseFixedAFR;

		// Token: 0x04000E39 RID: 3641
		private FuelFlowCalculationSchemes? _FuelFlowCalculationScheme;

		// Token: 0x04000E3A RID: 3642
		private bool? _UseRPMFix;

		// Token: 0x04000E3B RID: 3643
		private double? _FuelTankCapacity;

		// Token: 0x04000E3C RID: 3644
		private int? _VE1000;

		// Token: 0x04000E3D RID: 3645
		private int? _VE2000;

		// Token: 0x04000E3E RID: 3646
		private int? _VE3000;

		// Token: 0x04000E3F RID: 3647
		private int? _VE4000;

		// Token: 0x04000E40 RID: 3648
		private int? _VE5000;

		// Token: 0x04000E41 RID: 3649
		private int? _VE6000;

		// Token: 0x04000E42 RID: 3650
		private int? _VE7000;

		// Token: 0x04000E43 RID: 3651
		private int? _VE8000;

		// Token: 0x04000E44 RID: 3652
		private int? _EngineCylinders;

		// Token: 0x04000E45 RID: 3653
		private FuelTypes? _FuelType;

		// Token: 0x04000E46 RID: 3654
		private double? _EngineDisplacement;

		// Token: 0x04000E47 RID: 3655
		private bool? _DetectZeroFuelConsumption;

		// Token: 0x04000E48 RID: 3656
		private bool? _AndroidChartRenderingSafeMode;

		// Token: 0x04000E49 RID: 3657
		private bool? _DarkMode;

		// Token: 0x04000E4A RID: 3658
		private bool? _AutomaticallySwitchTheme;

		// Token: 0x04000E4B RID: 3659
		private bool? _AndroidRecolorNavBar;

		// Token: 0x04000E4C RID: 3660
		private bool? _AndroidUseFullscreen;

		// Token: 0x04000E4D RID: 3661
		private int? _ChartsView;

		// Token: 0x04000E4E RID: 3662
		private bool? _MultiChartPauseHidden;

		// Token: 0x04000E4F RID: 3663
		private string _MultiPidsSelected;

		// Token: 0x04000E50 RID: 3664
		private SharedSettings.PIDSortingModes? _PIDSortingMode;

		// Token: 0x04000E51 RID: 3665
		private bool? _ShowPing;

		// Token: 0x04000E52 RID: 3666
		private bool? _RecordData;

		// Token: 0x04000E53 RID: 3667
		private bool _LanguageChanged;

		// Token: 0x04000E54 RID: 3668
		private int originalLanguage = -1;

		// Token: 0x04000E55 RID: 3669
		private int? _Language;

		// Token: 0x04000E56 RID: 3670
		private int? _LiveDataShowTime;

		// Token: 0x04000E57 RID: 3671
		private bool? _ChartShowAverageValue;

		// Token: 0x04000E58 RID: 3672
		private bool? _SetChartMinMaxOnlyVisibleArea;

		// Token: 0x04000E59 RID: 3673
		private bool? _ShowMinMaxValues;

		// Token: 0x04000E5A RID: 3674
		private int? _ChartsVisible;

		// Token: 0x04000E5B RID: 3675
		private int? _LiveDataPIDId0;

		// Token: 0x04000E5C RID: 3676
		private int? _LiveDataPIDId1;

		// Token: 0x04000E5D RID: 3677
		private int? _LiveDataPIDId2;

		// Token: 0x04000E5E RID: 3678
		private int? _LiveDataPIDId3;

		// Token: 0x04000E5F RID: 3679
		private bool? _NoWiFiWarning;

		// Token: 0x04000E60 RID: 3680
		private bool? _TryConnectToLastWiFiNetwork;

		// Token: 0x04000E61 RID: 3681
		private bool? _RPMFixWarningShowed;

		// Token: 0x04000E62 RID: 3682
		private bool? _FirstTimeLaunch;

		// Token: 0x04000E63 RID: 3683
		private bool? _InfoShowed_Dashboard;

		// Token: 0x04000E64 RID: 3684
		private bool? _InfoShowed_SpeedTest;

		// Token: 0x04000E65 RID: 3685
		private bool? _InfoShowed_Mode06;

		// Token: 0x04000E66 RID: 3686
		private bool? _InfoShowed_AllSensors;

		// Token: 0x04000E67 RID: 3687
		private bool? _InfoShowed_EcoTests;

		// Token: 0x04000E68 RID: 3688
		private bool? _InfoShowed_FreezeFrame;

		// Token: 0x04000E69 RID: 3689
		private bool? _NissanWarningShowed;

		// Token: 0x04000E6A RID: 3690
		private bool? _Mode06AttentionShow;

		// Token: 0x04000E6B RID: 3691
		private bool? _LiveData_InfoShowed;

		// Token: 0x04000E6C RID: 3692
		private bool? _InfoShowed_DTCPage;

		// Token: 0x04000E6D RID: 3693
		private bool? _DashboardRearrangeOnRotation;

		// Token: 0x04000E6E RID: 3694
		private bool? _DashboardHUDMode;

		// Token: 0x04000E6F RID: 3695
		private int? _DashboardTheme;

		// Token: 0x04000E70 RID: 3696
		private int? _DashboardLastPage;

		// Token: 0x04000E71 RID: 3697
		private string _LastCarAvailableSensors;

		// Token: 0x04000E72 RID: 3698
		private long? _CurrentCarId;

		// Token: 0x04000E73 RID: 3699
		private DTCModeV2? _DTCReadingModeV2;

		// Token: 0x04000E74 RID: 3700
		private DTCModeV2? _DTCClearingModeV2;

		// Token: 0x04000E75 RID: 3701
		private string _CurrentCarName;

		// Token: 0x04000E76 RID: 3702
		private string _DTCReadingSequence;

		// Token: 0x04000E77 RID: 3703
		private string _DTCClearingSequence;

		// Token: 0x04000E78 RID: 3704
		private bool? _ForceOnlyOneProtocol;

		// Token: 0x04000E79 RID: 3705
		private string _Mode01Prefix;

		// Token: 0x04000E7A RID: 3706
		private int? _ResponseMarkerLength;

		// Token: 0x04000E7B RID: 3707
		private string _LastWiFiName;

		// Token: 0x04000E7C RID: 3708
		private int? _LastSuccessfulProtocol;

		// Token: 0x04000E7D RID: 3709
		private bool? _AndroidRequestedStoragePermission;

		// Token: 0x04000E7E RID: 3710
		private bool? _AndroidRequestedLocationPermission;

		// Token: 0x04000E7F RID: 3711
		private string _CodingLastPlatformSelected;

		// Token: 0x04000E80 RID: 3712
		private bool? _SendATZATE;

		// Token: 0x04000E81 RID: 3713
		private bool? _HideArchiveDTC;

		// Token: 0x04000E82 RID: 3714
		private bool _IgnoreCodingErrors = true;

		// Token: 0x04000E83 RID: 3715
		private bool? _VWTP20OpenSessionForDTCOperations;

		// Token: 0x04000E84 RID: 3716
		private string _PIDOverridesData;

		// Token: 0x04000E85 RID: 3717
		private bool? _ExpectedResponseCountOptimization;

		// Token: 0x04000E86 RID: 3718
		private bool? _ExpectedResponseCountOptimizationAlways1ForKWPMode01;

		// Token: 0x04000E87 RID: 3719
		private PIDEditorListModel.ViewByType? _PidListLastViewByType;

		// Token: 0x04000E88 RID: 3720
		private ReadPartialErrorActions? _ReadPartialErrorAction;

		// Token: 0x04000E89 RID: 3721
		private string _RecentColors;

		// Token: 0x04000E8A RID: 3722
		private bool? _DashboardCircularGaugeAnimation;

		// Token: 0x04000E8B RID: 3723
		private bool? _ForceUseManualFlowControlForCodingOperations;

		// Token: 0x04000E8C RID: 3724
		private int? _DatasetUploadMaxBlockSize;

		// Token: 0x04000E8D RID: 3725
		private int? _SendTesterPresentWhileLongUploadTimeMs;

		// Token: 0x04000E8E RID: 3726
		private string _CustomCodings;

		// Token: 0x04000E8F RID: 3727
		private bool? _AndroidRecolorStatusBarInDarkTheme;

		// Token: 0x04000E90 RID: 3728
		private bool? _TraceLogs;

		// Token: 0x04000E91 RID: 3729
		internal static string[] SkipListBase = new string[]
		{
			"Current", "ProfilePIDsCollection", "EncryptedProfilePIDsCollection", "CustomPIDsCollection", "SpeedTestDB", "BrandAndProfile", "MigratedToSettingsV2", "LicenseFinishDate", "ShowWhenTrialExpires", "TrialExtentions",
			"AdsProductPurchased", "FreePeriodFinishTime", "AskedForUpdateToVersion", "GDPR_Asked", "GDPR_ShowPersonalyzed", "LastException", "FlushLog", "IsTrialExpired", "ReviewReceived", "AskForReviewGoodConnections",
			"FreePeriodGoodConnections", "ZeroConsumptionPIDCollection", "InjectorPIDCollection", "CurbWeight", "PassengersWeight", "AdditionalWeight", "DragCoefficient", "DragArea", "TireResistance", "AmbientTemperature",
			"BarometricPressure", "DriverWeight", "FuelInTankVolume", "LanguageChanged", "FirstTimeLaunch", "InfoShowed_Dashboard", "InfoShowed_SpeedTest", "InfoShowed_Mode06", "InfoShowed_AllSensors", "InfoShowed_EcoTests",
			"InfoShowed_FreezeFrame", "NissanWarningShowed", "Mode06AttentionShow", "LiveData_InfoShowed", "InfoShowed_DTCPage", "Dashboard", "DTCReadingModeV2", "DTCClearingModeV2", "DTCReadingSequence", "DTCClearingSequence",
			"DisabledProfilePIDsCollection", "CANOptimizeMode22DataLengthDictionary", "MinVersion", "AvailableVersion", "CodingsCounter", "DTCFoundECUs", "PIDOverridesData", "PurchaseOrderId", "PurchaseProductId", "PurchaseToken",
			"SendTesterPresentWhileLongUploadTimeVag5f", "DatasetAllowed", "CustomCodings", "PurchaseDebug", "VagSendRebootAfterClear", "LastTimePatchUpdateChecked", "LastTimeNewVersionChecked", "LastTimeNewVersionChecked", "RokodilURL", "DisplayRokodil",
			"DisplayRokodilV2", "PurchaseResponseCached", "WhitelistDeviceActivated", "WhitelistDeviceSN", "BannedVersions"
		};

		// Token: 0x04000E92 RID: 3730
		internal static string[] OnlinePatchAllowedList = new string[] { "DisplayRokodil", "DisplayRokodilV2", "RokodilURL", "VagSendRebootAfterClear", "PurchaseDebug", "DatasetAllowed", "SendTesterPresentWhileLongUploadTimeVag5f", "BannedVersions" };

		// Token: 0x04000E93 RID: 3731
		internal bool DeveloperMode;

		// Token: 0x04000E94 RID: 3732
		private bool? _DatasetAllowed;

		// Token: 0x04000E95 RID: 3733
		private bool? _SensorsSearchOrderDefault;

		// Token: 0x04000E96 RID: 3734
		private bool? _ForceUseManualFlowControlWhileReadingData;

		// Token: 0x04000E97 RID: 3735
		private bool? _DashboardAnimation;

		// Token: 0x04000E98 RID: 3736
		private bool? _DashboardHideTopControls;

		// Token: 0x04000E99 RID: 3737
		private bool? _DashboardAlignItemsToGrid;

		// Token: 0x04000E9A RID: 3738
		private bool? _DashboardResizeHintDisplayed;

		// Token: 0x04000E9B RID: 3739
		private bool? _FuelHybridCar;

		// Token: 0x04000E9C RID: 3740
		private bool? _VagSendRebootAfterClear;

		// Token: 0x04000E9D RID: 3741
		private bool? _SearchForBTLEIfConnectionFailed;

		// Token: 0x04000E9E RID: 3742
		private bool? _SendTesterPresentWhileLongUploadTimeVag5f;

		// Token: 0x04000E9F RID: 3743
		private bool? _RecordLocationData;

		// Token: 0x04000EA0 RID: 3744
		private bool? _CANRequestSegmentationSTNLevel;

		// Token: 0x04000EA1 RID: 3745
		private bool? _CANResponseSegmentationSTNLevel;

		// Token: 0x04000EA2 RID: 3746
		private bool? _ReplaceATTAWithATCER;

		// Token: 0x04000EA3 RID: 3747
		private ChartItemTypes? _ChartDisplayStyle;

		// Token: 0x04000EA4 RID: 3748
		private bool? _ATCommandStateOptimization;

		// Token: 0x04000EA5 RID: 3749
		private bool? _ATCRAOptimization;

		// Token: 0x04000EA6 RID: 3750
		private bool? _BluetoothLocationWarningShowed;

		// Token: 0x04000EA7 RID: 3751
		private long? _LastTimePatchUpdateChecked;

		// Token: 0x04000EA8 RID: 3752
		private long? _LastTimeNewVersionChecked;

		// Token: 0x04000EA9 RID: 3753
		private long? _LastTimeDBUpdateChecked;

		// Token: 0x04000EAA RID: 3754
		private bool? _ForceProfileUpdateScheduled;

		// Token: 0x04000EAB RID: 3755
		private long? _LastTimeLicenceChecked;

		// Token: 0x04000EAC RID: 3756
		public TimeSpan LicenseCheckPeriod = TimeSpan.FromMinutes(10.0);

		// Token: 0x04000EAD RID: 3757
		[CompilerGenerated]
		private bool <IsRuIPCached>k__BackingField;

		// Token: 0x04000EAE RID: 3758
		[CompilerGenerated]
		private bool <DisplayRokodil>k__BackingField;

		// Token: 0x04000EAF RID: 3759
		private bool? _DisplayRokodilV2;

		// Token: 0x04000EB0 RID: 3760
		private string _RokodilURL;

		// Token: 0x04000EB1 RID: 3761
		private string _PurchaseResponseCached;

		// Token: 0x04000EB2 RID: 3762
		private bool? _UseServiceResponseForPositiveResponseMarker;

		// Token: 0x04000EB3 RID: 3763
		private bool? _BTLEShowDevicesWithoutName;

		// Token: 0x04000EB4 RID: 3764
		private bool? _AutomaticReoptimization;

		// Token: 0x04000EB5 RID: 3765
		private bool? _LiveDataListPageUpdateOnlyVisible;

		// Token: 0x04000EB6 RID: 3766
		private string _VWTP_ChannelSetupATST;

		// Token: 0x04000EB7 RID: 3767
		private int? _VWTP_ActiveConnectionTestTimeout;

		// Token: 0x04000EB8 RID: 3768
		private int? _VWTP_SendConnectionConfirmationPeriod;

		// Token: 0x04000EB9 RID: 3769
		private int? _DataRecordLineSplitterTime;

		// Token: 0x04000EBA RID: 3770
		private FlowControlOverrides? _FlowControlOverrideMode;

		// Token: 0x04000EBB RID: 3771
		private bool? _Hstld4Apl;

		// Token: 0x04000EBC RID: 3772
		private int? _ELM327ConnectionAttempts;

		// Token: 0x04000EBD RID: 3773
		private bool? _EV_Power_InvertValue;

		// Token: 0x04000EBE RID: 3774
		private bool? _SpeedCalibrationTaskPending;

		// Token: 0x04000EBF RID: 3775
		private int? _FontSizePatch;

		// Token: 0x04000EC0 RID: 3776
		private int? _AccelerationTestSpeedSourcePid;

		// Token: 0x04000EC1 RID: 3777
		private bool? _CheckOnlyPositiveResponseMarker;

		// Token: 0x04000EC2 RID: 3778
		private bool? _PIDSelectorWithValuePreview;

		// Token: 0x04000EC3 RID: 3779
		private int? _StopConnectionAttemptsAfterFailsMinutes;

		// Token: 0x04000EC4 RID: 3780
		private bool? _AllowProfilesBackgroundUpdate;

		// Token: 0x04000EC5 RID: 3781
		private bool? _DashboardiOSUseFullscreen;

		// Token: 0x04000EC6 RID: 3782
		private bool? _AddNissanConsult3Pids;

		// Token: 0x04000EC7 RID: 3783
		private bool? _WhitelistDeviceActivated;

		// Token: 0x04000EC8 RID: 3784
		private string _WhitelistDeviceSN;

		// Token: 0x04000EC9 RID: 3785
		private bool? _IgnoreProfilePidsTestScheduled;

		// Token: 0x04000ECA RID: 3786
		private int? _CarPlayUpdateInterval;

		// Token: 0x04000ECB RID: 3787
		private int? _CarPlayDashboardSelectedIndex;

		// Token: 0x04000ECC RID: 3788
		private string _BannedVersions;

		// Token: 0x04000ECD RID: 3789
		private bool? _NissanConsult3OpenCloseSession;

		// Token: 0x04000ECE RID: 3790
		private bool? _ProfileHasPids;

		// Token: 0x04000ECF RID: 3791
		private bool? _DroidGPCheckCurrencyR;

		// Token: 0x04000ED0 RID: 3792
		private bool? _ShowConnectionStatusOverlay;

		// Token: 0x02000275 RID: 629
		private enum ChartViewTypes
		{
			// Token: 0x04000ED2 RID: 3794
			Ask,
			// Token: 0x04000ED3 RID: 3795
			Combined,
			// Token: 0x04000ED4 RID: 3796
			Separate
		}

		// Token: 0x02000276 RID: 630
		public enum PIDSortingModes
		{
			// Token: 0x04000ED6 RID: 3798
			Id,
			// Token: 0x04000ED7 RID: 3799
			NameAsc,
			// Token: 0x04000ED8 RID: 3800
			NameDesc
		}

		// Token: 0x02000277 RID: 631
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<set_UseGPS>b__74_0>d : IAsyncStateMachine
		{
			// Token: 0x06001E9E RID: 7838 RVA: 0x0014DB7C File Offset: 0x0014BD7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SharedSettings sharedSettings = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00ED;
						}
						taskAwaiter5 = App.GetCurrentPage().DisplayAlert(Translate.GetString("droid_BackgroundGPSTitle"), Translate.GetString("droid_BackgroundGPS"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SharedSettings.<<set_UseGPS>b__74_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						sharedSettings.UseGPS = false;
						goto IL_00FD;
					}
					taskAwaiter3 = sharedSettings.RequestLocationPermissionAsync().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SharedSettings.<<set_UseGPS>b__74_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00ED:
					taskAwaiter3.GetResult();
					IL_00FD:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001E9F RID: 7839 RVA: 0x0014DCC4 File Offset: 0x0014BEC4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000ED9 RID: 3801
			public int <>1__state;

			// Token: 0x04000EDA RID: 3802
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000EDB RID: 3803
			public SharedSettings <>4__this;

			// Token: 0x04000EDC RID: 3804
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000EDD RID: 3805
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000278 RID: 632
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001EA0 RID: 7840 RVA: 0x0014DCD2 File Offset: 0x0014BED2
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001EA1 RID: 7841 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001EA2 RID: 7842 RVA: 0x0014DCDE File Offset: 0x0014BEDE
			internal bool <ResetCarSettingsToDefault>b__2_0(PropertyInfo x)
			{
				return !x.GetCustomAttributes(typeof(JsonIgnoreAttribute), false).Any<object>();
			}

			// Token: 0x06001EA3 RID: 7843 RVA: 0x0014DCF9 File Offset: 0x0014BEF9
			internal bool <set_UseGPS>b__74_1(PID x)
			{
				return x is PID_GPSSpeed;
			}

			// Token: 0x06001EA4 RID: 7844 RVA: 0x0014DD04 File Offset: 0x0014BF04
			internal bool <set_UseGPS>b__74_2(PID x)
			{
				return x is PID_CalculatedAvgSpeedGPS;
			}

			// Token: 0x06001EA5 RID: 7845 RVA: 0x0014DD0F File Offset: 0x0014BF0F
			internal bool <set_UseGPS>b__74_3(PID x)
			{
				return x is PID_GPSAltitude;
			}

			// Token: 0x06001EA6 RID: 7846 RVA: 0x0014DD1A File Offset: 0x0014BF1A
			internal bool <get_ZeroConsumptionPIDCollection>b__508_0(PID x)
			{
				return !(x is CustomPID) && x is IPIDFloatValue && ((x as IPIDFloatValue).Units == UnitsHelper.Units.None || (x as IPIDFloatValue).Units == UnitsHelper.Units.percent);
			}

			// Token: 0x06001EA7 RID: 7847 RVA: 0x0014DD4C File Offset: 0x0014BF4C
			internal bool <get_ZeroConsumptionPIDCollection>b__508_1(PID x)
			{
				return x is IPIDFloatValue && ((x as IPIDFloatValue).Units == UnitsHelper.Units.None || (x as IPIDFloatValue).Units == UnitsHelper.Units.percent);
			}

			// Token: 0x06001EA8 RID: 7848 RVA: 0x0014DD76 File Offset: 0x0014BF76
			internal bool <get_ZeroConsumptionPIDCollection>b__508_2(CustomPID x)
			{
				return x.Units == UnitsHelper.Units.None || x.Units == UnitsHelper.Units.percent;
			}

			// Token: 0x06001EA9 RID: 7849 RVA: 0x0014DD76 File Offset: 0x0014BF76
			internal bool <get_ZeroConsumptionPIDCollection>b__508_3(CustomPID x)
			{
				return x.Units == UnitsHelper.Units.None || x.Units == UnitsHelper.Units.percent;
			}

			// Token: 0x06001EAA RID: 7850 RVA: 0x0014DD8C File Offset: 0x0014BF8C
			internal bool <get_ZeroConsumptionPIDCollection>b__508_4(PID x)
			{
				return x is PID_EconomizerFSSandThrottlePosition;
			}

			// Token: 0x06001EAB RID: 7851 RVA: 0x0014DD97 File Offset: 0x0014BF97
			internal bool <get_InjectorPIDCollection>b__536_0(PID x)
			{
				return x.Id == 122;
			}

			// Token: 0x06001EAC RID: 7852 RVA: 0x0014DDA3 File Offset: 0x0014BFA3
			internal bool <get_InjectorPIDCollection>b__536_1(CustomPID x)
			{
				return x.Units == UnitsHelper.Units.ms;
			}

			// Token: 0x06001EAD RID: 7853 RVA: 0x0014DDA3 File Offset: 0x0014BFA3
			internal bool <get_InjectorPIDCollection>b__536_2(CustomPID x)
			{
				return x.Units == UnitsHelper.Units.ms;
			}

			// Token: 0x06001EAE RID: 7854 RVA: 0x0014DDAF File Offset: 0x0014BFAF
			internal bool <get_InjectorPIDCollection>b__536_3(PID x)
			{
				return x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.ms;
			}

			// Token: 0x06001EAF RID: 7855 RVA: 0x0014DD97 File Offset: 0x0014BF97
			internal bool <GetInjectorPID>b__593_0(PID x)
			{
				return x.Id == 122;
			}

			// Token: 0x06001EB0 RID: 7856 RVA: 0x0014DD97 File Offset: 0x0014BF97
			internal bool <GetInjectorPID>b__593_3(PID x)
			{
				return x.Id == 122;
			}

			// Token: 0x06001EB1 RID: 7857 RVA: 0x0014DDCA File Offset: 0x0014BFCA
			internal bool <set_DarkMode>b__673_1(ResourceDictionary x)
			{
				return x is LightTheme;
			}

			// Token: 0x06001EB2 RID: 7858 RVA: 0x0014DDD5 File Offset: 0x0014BFD5
			internal bool <set_DarkMode>b__673_2(ResourceDictionary x)
			{
				return x is DarkTheme;
			}

			// Token: 0x06001EB3 RID: 7859 RVA: 0x0014DDE0 File Offset: 0x0014BFE0
			internal string <GetSettingsReport>b__1212_0(PropertyInfo x)
			{
				return x.Name;
			}

			// Token: 0x04000EDE RID: 3806
			public static readonly SharedSettings.<>c <>9 = new SharedSettings.<>c();

			// Token: 0x04000EDF RID: 3807
			public static Func<PropertyInfo, bool> <>9__2_0;

			// Token: 0x04000EE0 RID: 3808
			public static Func<PID, bool> <>9__74_1;

			// Token: 0x04000EE1 RID: 3809
			public static Func<PID, bool> <>9__74_2;

			// Token: 0x04000EE2 RID: 3810
			public static Func<PID, bool> <>9__74_3;

			// Token: 0x04000EE3 RID: 3811
			public static Func<PID, bool> <>9__508_0;

			// Token: 0x04000EE4 RID: 3812
			public static Func<PID, bool> <>9__508_1;

			// Token: 0x04000EE5 RID: 3813
			public static Func<CustomPID, bool> <>9__508_2;

			// Token: 0x04000EE6 RID: 3814
			public static Func<CustomPID, bool> <>9__508_3;

			// Token: 0x04000EE7 RID: 3815
			public static Predicate<PID> <>9__508_4;

			// Token: 0x04000EE8 RID: 3816
			public static Func<PID, bool> <>9__536_0;

			// Token: 0x04000EE9 RID: 3817
			public static Func<CustomPID, bool> <>9__536_1;

			// Token: 0x04000EEA RID: 3818
			public static Func<CustomPID, bool> <>9__536_2;

			// Token: 0x04000EEB RID: 3819
			public static Func<PID, bool> <>9__536_3;

			// Token: 0x04000EEC RID: 3820
			public static Func<PID, bool> <>9__593_0;

			// Token: 0x04000EED RID: 3821
			public static Func<PID, bool> <>9__593_3;

			// Token: 0x04000EEE RID: 3822
			public static Func<ResourceDictionary, bool> <>9__673_1;

			// Token: 0x04000EEF RID: 3823
			public static Func<ResourceDictionary, bool> <>9__673_2;

			// Token: 0x04000EF0 RID: 3824
			public static Func<PropertyInfo, string> <>9__1212_0;
		}

		// Token: 0x02000279 RID: 633
		[CompilerGenerated]
		private sealed class <>c__DisplayClass673_0
		{
			// Token: 0x06001EB4 RID: 7860 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass673_0()
			{
			}

			// Token: 0x06001EB5 RID: 7861 RVA: 0x0014DDE8 File Offset: 0x0014BFE8
			internal void <set_DarkMode>b__0()
			{
				try
				{
					this.<>4__this.AppSettings.AddOrUpdateValue("DarkMode", this.value, null);
					this.<>4__this._DarkMode = new bool?(this.value);
					this.<>4__this.NotifyPropertyChanged("DarkMode");
					if (this.value)
					{
						foreach (ResourceDictionary resourceDictionary in Application.Current.Resources.MergedDictionaries.Where((ResourceDictionary x) => x is LightTheme).ToArray<ResourceDictionary>())
						{
							Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
						}
						Application.Current.Resources.Add(new DarkTheme());
						this.<>4__this.AndroidSetNavbarColor();
					}
					else
					{
						foreach (ResourceDictionary resourceDictionary2 in Application.Current.Resources.MergedDictionaries.Where((ResourceDictionary x) => x is DarkTheme).ToArray<ResourceDictionary>())
						{
							Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary2);
						}
						Application.Current.Resources.Add(new LightTheme());
						if (PlatformHelper.IsAndroid)
						{
							this.<>4__this.AndroidSetNavbarColor();
						}
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04000EF1 RID: 3825
			public SharedSettings <>4__this;

			// Token: 0x04000EF2 RID: 3826
			public bool value;
		}

		// Token: 0x0200027A RID: 634
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetSettingsReport>d__1212 : IAsyncStateMachine
		{
			// Token: 0x06001EB6 RID: 7862 RVA: 0x0014DF70 File Offset: 0x0014C170
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SharedSettings sharedSettings = this;
				string text2;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					if (num != 0)
					{
						sb = new StringBuilder(256);
						sb.Append("[Car Scanner settings:]\n");
						IEnumerator<PropertyInfo> enumerator = (from x in sharedSettings.GetType().GetProperties()
							orderby x.Name
							select x).GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PropertyInfo propertyInfo = enumerator.Current;
								try
								{
									if (!SharedSettings.SkipListBase.Contains(propertyInfo.Name))
									{
										if (!PlatformHelper.IsiOS || (!propertyInfo.Name.StartsWith("android", StringComparison.InvariantCultureIgnoreCase) && !propertyInfo.Name.StartsWith("droid", StringComparison.InvariantCultureIgnoreCase)))
										{
											sb.Append(propertyInfo.Name);
											sb.Append("=");
											object value = propertyInfo.GetValue(sharedSettings);
											sb.Append((value == null) ? "null" : value.ToString());
											sb.Append("\r\n");
										}
									}
								}
								catch (Exception)
								{
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						try
						{
							string text = "\r\nOverride_roles=";
							Dictionary<int, PIDOverride>.KeyCollection.Enumerator enumerator2 = PIDOverrideDictionary.Instance.Keys.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									int num3 = enumerator2.Current;
									PIDOverride pidoverride = PIDOverrideDictionary.Instance[num3];
									if (pidoverride != null)
									{
										text += string.Format("{0}={1};", pidoverride.ID, pidoverride.Role);
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator2).Dispose();
								}
							}
							IEnumerator<CustomPID> enumerator3 = CustomPIDViewModel.CurrentCustom.PidCollection.GetEnumerator();
							try
							{
								while (enumerator3.MoveNext())
								{
									CustomPID customPID = enumerator3.Current;
									if (customPID != null && customPID.Role != Roles.UNDEFINED)
									{
										text += string.Format("{0}={1};", customPID.Id, customPID.Role);
									}
								}
							}
							finally
							{
								if (num < 0 && enumerator3 != null)
								{
									enumerator3.Dispose();
								}
							}
							text += "\r\n";
							sb.Append(text);
						}
						catch (Exception)
						{
						}
						try
						{
							if (!CustomPIDViewModel.DisabledProfile.Loaded)
							{
								CustomPIDViewModel.DisabledProfile.Load();
							}
							if (CustomPIDViewModel.DisabledProfile.PidCollection != null && CustomPIDViewModel.DisabledProfile.PidCollection.Count > 0)
							{
								StringBuilder stringBuilder = new StringBuilder(CustomPIDViewModel.DisabledProfile.PidCollection.Count + 1);
								stringBuilder.Append("\r\nDisabledPIDs: ");
								IEnumerator<CustomPID> enumerator3 = CustomPIDViewModel.DisabledProfile.PidCollection.GetEnumerator();
								try
								{
									while (enumerator3.MoveNext())
									{
										CustomPID customPID2 = enumerator3.Current;
										stringBuilder.Append(customPID2.Id.ToString() + ";");
									}
								}
								finally
								{
									if (num < 0 && enumerator3 != null)
									{
										enumerator3.Dispose();
									}
								}
								sb.Append(stringBuilder.ToString());
							}
						}
						catch (Exception)
						{
						}
						sb.Append("ATST=");
						sb.Append(sharedSettings.GetATST());
						sb.Append("\r\n");
						sb.Append("P=");
						sb.Append(sharedSettings.AdsProductPurchased.ToString());
						sb.Append("\r\n");
						sb.Append("FPGC=");
						sb.Append(sharedSettings.FreePeriodGoodConnections.ToString());
						sb.Append("\r\n");
						sb.Append("CC=");
						sb.Append(sharedSettings.CodingsCounter.ToString());
						sb.Append("\r\n");
						sb.Append("\r\nOS version=" + DeviceInfo.VersionString);
						sb.Append("\r\nPlatform=" + DeviceInfo.Platform.ToString());
						sb.Append("\r\nIdiom=" + DeviceInfo.Idiom.ToString());
						sb.Append("\r\nDeviceModel=" + DeviceInfo.Model);
						sb.Append("\r\nDeviceManufacturer=" + DeviceInfo.Manufacturer + "\r\n");
						try
						{
							sb.Append("\r\nOrigCulture=" + CarScannerXamarinForms.Language.origCulture.ToString());
							sb.Append("\r\nOrigUICulture=" + CarScannerXamarinForms.Language.origUICulture.ToString());
						}
						catch (Exception)
						{
						}
						if (!PlatformHelper.IsAndroid)
						{
							sb.Append("\r\nRunningOnMac=" + PlatformHelper.IOSService.IsiOSApplicationOnMac.ToString());
							goto IL_064B;
						}
						sb.Append("\r\nLocationPermission=" + PlatformHelper.DroidService.HasLocationPermission.ToString() + "\r\n");
						sb.Append("StoragePermission=" + PlatformHelper.DroidService.HasStoragePermission.ToString() + "\r\n");
						sb.Append("BluetoothPermission=" + PlatformHelper.DroidService.HasBluetoothPermission.ToString() + "\r\n");
						if (!PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							goto IL_064B;
						}
						taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SharedSettings.<GetSettingsReport>d__1212>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
					}
					PermissionStatus result = taskAwaiter.GetResult();
					sb.Append("NearbyDevicesPermission=" + result.ToString());
					IL_064B:
					sb.Append(string.Concat(new string[]
					{
						"\r\nVersion=",
						App.Version,
						"/",
						App.Build,
						"\r\n"
					}));
					sb.Append("\r\nPrVersion=" + ProfileV2Model.GetCurrentVersion() + "\r\n");
					switch (PlatformHelper.AppMarket)
					{
					case Markets.AppStore:
						sb.Append("#iOS");
						break;
					case Markets.GooglePlay:
						sb.Append("#Gplay");
						break;
					case Markets.HMS:
						sb.Append("#HUAWEI");
						break;
					case Markets.Rustore:
						sb.Append("#RuStore");
						break;
					case Markets.RUS:
						sb.Append("#RUS");
						break;
					case Markets.Sideload:
						sb.Append("#SL");
						break;
					}
					text2 = sb.ToString();
				}
				catch (Exception ex)
				{
					num2 = -2;
					sb = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				sb = null;
				this.<>t__builder.SetResult(text2);
			}

			// Token: 0x06001EB7 RID: 7863 RVA: 0x0014E7E4 File Offset: 0x0014C9E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000EF3 RID: 3827
			public int <>1__state;

			// Token: 0x04000EF4 RID: 3828
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04000EF5 RID: 3829
			public SharedSettings <>4__this;

			// Token: 0x04000EF6 RID: 3830
			private StringBuilder <sb>5__2;

			// Token: 0x04000EF7 RID: 3831
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x0200027B RID: 635
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestLocationPermissionAsync>d__75 : IAsyncStateMachine
		{
			// Token: 0x06001EB8 RID: 7864 RVA: 0x0014E7F4 File Offset: 0x0014C9F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SharedSettings sharedSettings = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							goto IL_0072;
						}
						taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SharedSettings.<RequestLocationPermissionAsync>d__75>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						goto IL_00DD;
					}
					IL_0072:
					try
					{
						if (num != 1)
						{
							taskAwaiter3 = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SharedSettings.<RequestLocationPermissionAsync>d__75>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
						}
						if (taskAwaiter3.GetResult() != 3)
						{
							sharedSettings.UseGPS = false;
						}
					}
					catch (Exception)
					{
					}
					IL_00DD:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001EB9 RID: 7865 RVA: 0x0014E928 File Offset: 0x0014CB28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000EF8 RID: 3832
			public int <>1__state;

			// Token: 0x04000EF9 RID: 3833
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000EFA RID: 3834
			public SharedSettings <>4__this;

			// Token: 0x04000EFB RID: 3835
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x0200027C RID: 636
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendDebugEmail>d__932 : IAsyncStateMachine
		{
			// Token: 0x06001EBA RID: 7866 RVA: 0x0014E938 File Offset: 0x0014CB38
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SharedSettings sharedSettings = this;
				try
				{
					if (num2 > 2)
					{
						if (num2 == 3)
						{
							goto IL_0289;
						}
						num = 0;
					}
					TaskAwaiter taskAwaiter4;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						switch (num2)
						{
						case 0:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = (num3 = -1);
							break;
						}
						case 1:
						{
							IL_0154:
							try
							{
								if (num2 != 1)
								{
									taskAwaiter3 = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = (num3 = 1);
										taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SharedSettings.<SendDebugEmail>d__932>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
									num2 = (num3 = -1);
								}
								taskAwaiter3.GetResult();
								PCLDebugStream.CurrentInstance.Close();
							}
							catch (Exception)
							{
							}
							string filepath = PCLDebugStream.GetFilepath();
							if (File.Exists(filepath))
							{
								message.Attachments.Add(new EmailAttachment(filepath));
							}
							taskAwaiter3 = Email.ComposeAsync(message).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = (num3 = 2);
								taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SharedSettings.<SendDebugEmail>d__932>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_024A;
						}
						case 2:
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_024A;
						default:
							text = " (" + Device.RuntimePlatform + ")";
							taskAwaiter = sharedSettings.GetSettingsReport().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = (num3 = 0);
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SharedSettings.<SendDebugEmail>d__932>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						string result = taskAwaiter.GetResult();
						message = new EmailMessage(text, result, new string[] { "admin@carscanner.info" });
						text = null;
						message.BodyFormat = 0;
						if (!string.IsNullOrEmpty(sharedSettings.LastException))
						{
							message.Subject = "Car Scanner Crash log " + message.Subject;
							message.Body = message.Body + "\r\n" + sharedSettings.LastException;
							goto IL_0154;
						}
						message.Subject = "Car Scanner user feedback" + message.Subject;
						goto IL_0154;
						IL_024A:
						taskAwaiter3.GetResult();
						message = null;
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_0301;
					}
					Exception ex2 = (Exception)obj;
					IL_0289:
					try
					{
						TaskAwaiter taskAwaiter3;
						if (num2 != 3)
						{
							taskAwaiter3 = App.GetCurrentPage().DisplayAlert("Error creating email", "Please send email to:\nadmin@carscanner.info", "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = (num3 = 3);
								taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SharedSettings.<SendDebugEmail>d__932>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = (num3 = -1);
						}
						taskAwaiter3.GetResult();
					}
					catch (Exception)
					{
					}
					IL_0301:
					obj = null;
				}
				catch (Exception ex3)
				{
					num3 = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				num3 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001EBB RID: 7867 RVA: 0x0014ECE0 File Offset: 0x0014CEE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000EFC RID: 3836
			public int <>1__state;

			// Token: 0x04000EFD RID: 3837
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000EFE RID: 3838
			public SharedSettings <>4__this;

			// Token: 0x04000EFF RID: 3839
			private object <>7__wrap1;

			// Token: 0x04000F00 RID: 3840
			private int <>7__wrap2;

			// Token: 0x04000F01 RID: 3841
			private EmailMessage <message>5__4;

			// Token: 0x04000F02 RID: 3842
			private string <>7__wrap4;

			// Token: 0x04000F03 RID: 3843
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04000F04 RID: 3844
			private TaskAwaiter <>u__2;
		}
	}
}
