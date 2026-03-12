using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002C1 RID: 705
	public class OBDReaderProfileV2 : INotifyPropertyChanged
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x001A8F2C File Offset: 0x001A712C
		public OBDReaderProfileV2()
		{
			this.UpdateAliases = new List<string>();
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06002236 RID: 8758 RVA: 0x001A9004 File Offset: 0x001A7204
		// (remove) Token: 0x06002237 RID: 8759 RVA: 0x001A903C File Offset: 0x001A723C
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

		// Token: 0x06002238 RID: 8760 RVA: 0x001A9071 File Offset: 0x001A7271
		public void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x170010E7 RID: 4327
		// (get) Token: 0x06002239 RID: 8761 RVA: 0x001A908A File Offset: 0x001A728A
		// (set) Token: 0x0600223A RID: 8762 RVA: 0x001A9092 File Offset: 0x001A7292
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

		// Token: 0x170010E8 RID: 4328
		// (get) Token: 0x0600223B RID: 8763 RVA: 0x001A90A6 File Offset: 0x001A72A6
		// (set) Token: 0x0600223C RID: 8764 RVA: 0x001A90AE File Offset: 0x001A72AE
		public string BrandForDTC
		{
			get
			{
				return this._BrandForDTC;
			}
			set
			{
				this._BrandForDTC = value;
				this.OnPropertyChanged("BrandForDTC");
			}
		}

		// Token: 0x170010E9 RID: 4329
		// (get) Token: 0x0600223D RID: 8765 RVA: 0x001A90C2 File Offset: 0x001A72C2
		// (set) Token: 0x0600223E RID: 8766 RVA: 0x001A90CA File Offset: 0x001A72CA
		public string Description
		{
			get
			{
				return this._Description;
			}
			set
			{
				this._Description = value;
				this.OnPropertyChanged("Description");
			}
		}

		// Token: 0x170010EA RID: 4330
		// (get) Token: 0x0600223F RID: 8767 RVA: 0x001A90DE File Offset: 0x001A72DE
		// (set) Token: 0x06002240 RID: 8768 RVA: 0x001A90E6 File Offset: 0x001A72E6
		public List<string> Brands
		{
			get
			{
				return this._Brands;
			}
			set
			{
				this._Brands = value;
				this.OnPropertyChanged("Brands");
			}
		}

		// Token: 0x170010EB RID: 4331
		// (get) Token: 0x06002241 RID: 8769 RVA: 0x001A90FA File Offset: 0x001A72FA
		// (set) Token: 0x06002242 RID: 8770 RVA: 0x001A9102 File Offset: 0x001A7302
		public List<string> UpdateAliases
		{
			get
			{
				return this._Aliases;
			}
			set
			{
				this._Aliases = value;
				this.OnPropertyChanged("Aliases");
			}
		}

		// Token: 0x170010EC RID: 4332
		// (get) Token: 0x06002243 RID: 8771 RVA: 0x001A9116 File Offset: 0x001A7316
		// (set) Token: 0x06002244 RID: 8772 RVA: 0x001A911E File Offset: 0x001A731E
		public string TesterPresentCommand
		{
			get
			{
				return this._TesterPresentCommand;
			}
			set
			{
				this._TesterPresentCommand = value;
				this.OnPropertyChanged("TesterPresentCommand");
			}
		}

		// Token: 0x170010ED RID: 4333
		// (get) Token: 0x06002245 RID: 8773 RVA: 0x001A9132 File Offset: 0x001A7332
		// (set) Token: 0x06002246 RID: 8774 RVA: 0x001A913A File Offset: 0x001A733A
		public bool SendTesterPresent
		{
			get
			{
				return this._SendTesterPresent;
			}
			set
			{
				this._SendTesterPresent = value;
				this.OnPropertyChanged("SendTesterPresent");
			}
		}

		// Token: 0x170010EE RID: 4334
		// (get) Token: 0x06002247 RID: 8775 RVA: 0x001A914E File Offset: 0x001A734E
		// (set) Token: 0x06002248 RID: 8776 RVA: 0x001A9156 File Offset: 0x001A7356
		public string InitSequence
		{
			get
			{
				return this._InitSequence;
			}
			set
			{
				this._InitSequence = value;
				this.OnPropertyChanged("InitSequence");
			}
		}

		// Token: 0x170010EF RID: 4335
		// (get) Token: 0x06002249 RID: 8777 RVA: 0x001A916A File Offset: 0x001A736A
		// (set) Token: 0x0600224A RID: 8778 RVA: 0x001A9172 File Offset: 0x001A7372
		public string CheckConnectionCommand
		{
			get
			{
				return this._CheckConnectionCommand;
			}
			set
			{
				this._CheckConnectionCommand = value;
				this.OnPropertyChanged("CheckConnectionCommand");
			}
		}

		// Token: 0x170010F0 RID: 4336
		// (get) Token: 0x0600224B RID: 8779 RVA: 0x001A9186 File Offset: 0x001A7386
		// (set) Token: 0x0600224C RID: 8780 RVA: 0x001A918E File Offset: 0x001A738E
		public bool UseOBD2
		{
			get
			{
				return this._UseOBD2;
			}
			set
			{
				this._UseOBD2 = value;
				this.OnPropertyChanged("UseOBD2");
			}
		}

		// Token: 0x170010F1 RID: 4337
		// (get) Token: 0x0600224D RID: 8781 RVA: 0x001A91A2 File Offset: 0x001A73A2
		// (set) Token: 0x0600224E RID: 8782 RVA: 0x001A91AA File Offset: 0x001A73AA
		public string DefaultHeader
		{
			get
			{
				return this._DefaultHeader;
			}
			set
			{
				this._DefaultHeader = value;
				this.OnPropertyChanged("DefaultHeader");
			}
		}

		// Token: 0x170010F2 RID: 4338
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x001A91BE File Offset: 0x001A73BE
		// (set) Token: 0x06002250 RID: 8784 RVA: 0x001A91C6 File Offset: 0x001A73C6
		public string ProfilePIDs
		{
			get
			{
				return this._ProfilePIDs;
			}
			set
			{
				this._ProfilePIDs = value;
				this.OnPropertyChanged("ProfilePIDs");
			}
		}

		// Token: 0x170010F3 RID: 4339
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x001A91DA File Offset: 0x001A73DA
		// (set) Token: 0x06002252 RID: 8786 RVA: 0x001A91E2 File Offset: 0x001A73E2
		public DTCModeV2 DTCReadingModeV2
		{
			get
			{
				return this._DTCReadingModeV2;
			}
			set
			{
				this._DTCReadingModeV2 = value;
				this.OnPropertyChanged("DTCReadingModeV2");
			}
		}

		// Token: 0x170010F4 RID: 4340
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x001A91F6 File Offset: 0x001A73F6
		// (set) Token: 0x06002254 RID: 8788 RVA: 0x001A91FE File Offset: 0x001A73FE
		public string DTCReadingSequence
		{
			get
			{
				return this._DTCReadingSequence;
			}
			set
			{
				this._DTCReadingSequence = value;
				this.OnPropertyChanged("DTCReadingSequence");
			}
		}

		// Token: 0x170010F5 RID: 4341
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x001A9212 File Offset: 0x001A7412
		// (set) Token: 0x06002256 RID: 8790 RVA: 0x001A921A File Offset: 0x001A741A
		public DTCModeV2 DTCClearingModeV2
		{
			get
			{
				return this._DTCClearingModeV2;
			}
			set
			{
				this._DTCClearingModeV2 = value;
				this.OnPropertyChanged("DTCClearingModeV2");
			}
		}

		// Token: 0x170010F6 RID: 4342
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x001A922E File Offset: 0x001A742E
		// (set) Token: 0x06002258 RID: 8792 RVA: 0x001A9236 File Offset: 0x001A7436
		public string DTCClearingSequence
		{
			get
			{
				return this._DTCClearingSequence;
			}
			set
			{
				this._DTCClearingSequence = value;
				this.OnPropertyChanged("DTCClearingSequence");
			}
		}

		// Token: 0x170010F7 RID: 4343
		// (get) Token: 0x06002259 RID: 8793 RVA: 0x001A924A File Offset: 0x001A744A
		// (set) Token: 0x0600225A RID: 8794 RVA: 0x001A9252 File Offset: 0x001A7452
		public bool UseCustomInit
		{
			get
			{
				return this._UseCustomInit;
			}
			set
			{
				this._UseCustomInit = value;
				this.OnPropertyChanged("UseCustomInit");
			}
		}

		// Token: 0x170010F8 RID: 4344
		// (get) Token: 0x0600225B RID: 8795 RVA: 0x001A9266 File Offset: 0x001A7466
		// (set) Token: 0x0600225C RID: 8796 RVA: 0x001A926E File Offset: 0x001A746E
		public int DefaultProtocolSelected
		{
			get
			{
				return this._DefaultProtocolSelected;
			}
			set
			{
				this._DefaultProtocolSelected = value;
				this.OnPropertyChanged("DefaultProtocolSelected");
			}
		}

		// Token: 0x170010F9 RID: 4345
		// (get) Token: 0x0600225D RID: 8797 RVA: 0x001A9282 File Offset: 0x001A7482
		// (set) Token: 0x0600225E RID: 8798 RVA: 0x001A928A File Offset: 0x001A748A
		public bool IsExperimental
		{
			get
			{
				return this._IsExperimental;
			}
			set
			{
				this._IsExperimental = value;
				this.OnPropertyChanged("IsExperimental");
			}
		}

		// Token: 0x170010FA RID: 4346
		// (get) Token: 0x0600225F RID: 8799 RVA: 0x001A929E File Offset: 0x001A749E
		// (set) Token: 0x06002260 RID: 8800 RVA: 0x001A92A6 File Offset: 0x001A74A6
		public bool ShouldCheckProfilePIDs
		{
			get
			{
				return this._ShouldCheckProfilePIDs;
			}
			set
			{
				this._ShouldCheckProfilePIDs = value;
				this.OnPropertyChanged("ShouldCheckProfilePIDs");
			}
		}

		// Token: 0x170010FB RID: 4347
		// (get) Token: 0x06002261 RID: 8801 RVA: 0x001A92BA File Offset: 0x001A74BA
		// (set) Token: 0x06002262 RID: 8802 RVA: 0x001A92C2 File Offset: 0x001A74C2
		public bool ForceOnlyOneProtocol
		{
			get
			{
				return this._ForceOnlyOneProtocol;
			}
			set
			{
				this._ForceOnlyOneProtocol = value;
				this.OnPropertyChanged("ForceOnlyOneProtocol");
			}
		}

		// Token: 0x170010FC RID: 4348
		// (get) Token: 0x06002263 RID: 8803 RVA: 0x001A92D6 File Offset: 0x001A74D6
		// (set) Token: 0x06002264 RID: 8804 RVA: 0x001A92DE File Offset: 0x001A74DE
		public bool DaihatsuMode
		{
			get
			{
				return this._DaihatsuMode;
			}
			set
			{
				this._DaihatsuMode = value;
				this.OnPropertyChanged("DaihatsuMode");
			}
		}

		// Token: 0x170010FD RID: 4349
		// (get) Token: 0x06002265 RID: 8805 RVA: 0x001A92F2 File Offset: 0x001A74F2
		// (set) Token: 0x06002266 RID: 8806 RVA: 0x001A92FA File Offset: 0x001A74FA
		public string Mode01Prefix
		{
			get
			{
				return this._Mode01Prefix;
			}
			set
			{
				this._Mode01Prefix = value;
				this.OnPropertyChanged("Mode01Prefix");
			}
		}

		// Token: 0x170010FE RID: 4350
		// (get) Token: 0x06002267 RID: 8807 RVA: 0x001A930E File Offset: 0x001A750E
		// (set) Token: 0x06002268 RID: 8808 RVA: 0x001A9316 File Offset: 0x001A7516
		public int ResponseMarkerLength
		{
			get
			{
				return this._ResponseMarkerLength;
			}
			set
			{
				this._ResponseMarkerLength = value;
				this.OnPropertyChanged("ResponseMarkerLength");
			}
		}

		// Token: 0x170010FF RID: 4351
		// (get) Token: 0x06002269 RID: 8809 RVA: 0x001A932A File Offset: 0x001A752A
		// (set) Token: 0x0600226A RID: 8810 RVA: 0x001A9332 File Offset: 0x001A7532
		public bool RequestECUInfo
		{
			get
			{
				return this._RequestECUInfo;
			}
			set
			{
				this._RequestECUInfo = value;
				this.OnPropertyChanged("RequestECUInfo");
			}
		}

		// Token: 0x17001100 RID: 4352
		// (get) Token: 0x0600226B RID: 8811 RVA: 0x001A9346 File Offset: 0x001A7546
		// (set) Token: 0x0600226C RID: 8812 RVA: 0x001A934E File Offset: 0x001A754E
		public int PIDsIndexStart
		{
			get
			{
				return this._PIDsIndexStart;
			}
			set
			{
				this._PIDsIndexStart = value;
				this.OnPropertyChanged("PIDsIndexStart");
			}
		}

		// Token: 0x17001101 RID: 4353
		// (get) Token: 0x0600226D RID: 8813 RVA: 0x001A9362 File Offset: 0x001A7562
		// (set) Token: 0x0600226E RID: 8814 RVA: 0x001A936A File Offset: 0x001A756A
		public string Tweaks
		{
			get
			{
				return this._Tweaks;
			}
			set
			{
				this._Tweaks = value;
				this.OnPropertyChanged("Tweaks");
			}
		}

		// Token: 0x17001102 RID: 4354
		// (get) Token: 0x0600226F RID: 8815 RVA: 0x001A937E File Offset: 0x001A757E
		// (set) Token: 0x06002270 RID: 8816 RVA: 0x001A9386 File Offset: 0x001A7586
		public bool ProfileHasPids
		{
			[CompilerGenerated]
			get
			{
				return this.<ProfileHasPids>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProfileHasPids>k__BackingField = value;
			}
		}

		// Token: 0x06002271 RID: 8817 RVA: 0x001A9390 File Offset: 0x001A7590
		public void Apply(string brand)
		{
			SharedSettings.Current.SelectedProfileV2Name = this.Name;
			SharedSettings.Current.SelectedBrand = brand;
			SharedSettings.Current.TesterPresentCommand = this.TesterPresentCommand;
			SharedSettings.Current.AlwaysPingECU = this.SendTesterPresent;
			SharedSettings.Current.CustomInitString = this.InitSequence;
			SharedSettings.Current.DetectECUConnectionPID = this.CheckConnectionCommand;
			SharedSettings.Current.UseOBD2 = this.UseOBD2;
			SharedSettings.Current.DefaultFunctionalHeader = this.DefaultHeader;
			SharedSettings.Current.ProfilePIDsCollection = this.ProfilePIDs;
			SharedSettings.Current.DTCReadingModeV2 = this.DTCReadingModeV2;
			SharedSettings.Current.DTCReadingSequence = this.DTCReadingSequence;
			SharedSettings.Current.DTCClearingModeV2 = this.DTCClearingModeV2;
			SharedSettings.Current.DTCClearingSequence = this.DTCClearingSequence;
			SharedSettings.Current.UseDefaultInit = !this.UseCustomInit;
			SharedSettings.Current.ProtocolNumber = this.DefaultProtocolSelected;
			SharedSettings.Current.ForceOnlyOneProtocol = this.ForceOnlyOneProtocol;
			SharedSettings.Current.ShouldCheckProfilePIDs = this.ShouldCheckProfilePIDs;
			SharedSettings.Current.DaihatsuKLine = this.DaihatsuMode;
			SharedSettings.Current.Mode01Prefix = this.Mode01Prefix;
			SharedSettings.Current.ResponseMarkerLength = this.ResponseMarkerLength;
			SharedSettings.Current.RequestECUInfo = this.RequestECUInfo;
			SharedSettings.Current.PerformSensorsScanByTesting = false;
			SharedSettings.Current.BrandForDTC = this.BrandForDTC;
			SharedSettings.Current.ProfileHasPids = this.ProfileHasPids;
			OBDReaderProfileV2.ImportProfilePids(this);
			OBDReaderProfileV2.SetEnabled();
			OBDDataReader obdreader = App.OBDReader;
			if (obdreader != null)
			{
				CarData currentCarData = obdreader.CurrentCarData;
				if (currentCarData != null)
				{
					currentCarData.CreateEmptyPIDS();
				}
			}
			SharedSettings.Current.ProfileUpdateAlias = this.UpdateAliases[0];
			CustomPIDViewModel.CurrentProfile.Save();
			if (!string.IsNullOrEmpty(this.Tweaks))
			{
				try
				{
					this.ApplyTweaks(false);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002272 RID: 8818 RVA: 0x001A9588 File Offset: 0x001A7788
		public void ApplyTweaks(bool onlyDashboard = false)
		{
			string[] array = this.Tweaks.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (string text in array)
			{
				try
				{
					string[] array3 = text.Split(new char[] { '=' }, 2, StringSplitOptions.RemoveEmptyEntries);
					if (array3.Length == 2)
					{
						dictionary[array3[0]] = array3[1];
					}
				}
				catch (Exception)
				{
				}
			}
			Type typeFromHandle = typeof(SharedSettings);
			List<string> list = new List<string>(new string[]
			{
				"CustomPidLastId", "SpeedTestDB", "AndroidStartBackgroundService", "OpenDashboardOnLaunch", "ConnectOnLaunch", "IOTimeout", "SendDelay", "WiFiServer", "WiFiPort", "ShowBadELMWarning",
				"FuelConsumptionUnit", "UseLitersForVolume", "FirstTimeLaunch", "FirstConnectionAttempted", "UseL100ForFuel", "UseHoursePower", "UseNmForTorque", "AccelerationUseG", "Use_km", "Pressure_use_kpa",
				"Flow_use_grams_sec", "Use_celcium", "UseUSGallon", "FuelPriceForLitre", "Currency", "AlwaysRecordFuelConsumption", "ShowAirFuelBasedOnStoichiometric", "ChartsView", "PIDSortingMode", "ShowPing",
				"RecordData", "LiveDataShowTime", "ChartShowAverageValue", "SetChartMinMaxOnlyVisibleArea", "ChartsVisible", "LiveDataPIDId0", "LiveDataPIDId1", "LiveDataPIDId2", "LiveDataPIDId3", "SelectedProfileV2Name",
				"SelectedBrand", "ProfileUpdateAlias", "DTCReadingModeV2", "DTCReadingSequence", "DTCClearingModeV2", "DTCClearingSequence", "ForceOnlyOneProtocol", "ShouldCheckProfilePIDs", "DaihatsuKLine", "Mode01Prefix",
				"ResponseMarkerLength", "ZeroConsumptionWhenZero", "ZeroConsumptionPIDId", "UseCustomPIDForZeroConsumption", "FuelFlowCalculationSchemeIdx", "BoostCalculationMethodIdx", "IgnoreSupportedFlagForPIDs0166_0183", "CANOptimizeRequests", "ATSTIdx", "CheckOnlyPositiveResponseMarker",
				"SensorsSearchOrderDefault"
			});
			List<string> list2 = new List<string>(new string[]
			{
				"Current", "ProfilePIDsCollection", "EncryptedProfilePIDsCollection", "CustomPIDsCollection", "SpeedTestDB", "BrandAndProfile", "MigratedToSettingsV2", "LicenseFinishDate", "ShowWhenTrialExpires", "TrialExtentions",
				"AdsProductPurchased", "FreePeriodFinishTime", "AskedForUpdateToVersion", "GDPR_Asked", "GDPR_ShowPersonalyzed", "LastException", "FlushLog", "IsTrialExpired", "ReviewReceived", "AskForReviewGoodConnections",
				"FreePeriodGoodConnections", "ZeroConsumptionPIDCollection", "InjectorPIDCollection", "CurbWeight", "PassengersWeight", "AdditionalWeight", "DragCoefficient", "DragArea", "TireResistance", "AmbientTemperature",
				"BarometricPressure", "DriverWeight", "FuelInTankVolume", "LanguageChanged", "FirstTimeLaunch", "InfoShowed_Dashboard", "InfoShowed_SpeedTest", "InfoShowed_Mode06", "InfoShowed_AllSensors", "InfoShowed_EcoTests",
				"InfoShowed_FreezeFrame", "NissanWarningShowed", "Mode06AttentionShow", "LiveData_InfoShowed", "InfoShowed_DTCPage", "Dashboard", "DTCReadingModeV2", "DTCClearingModeV2", "DTCReadingSequence", "DTCClearingSequence",
				"DisabledProfilePIDsCollection", "CANOptimizeMode22DataLengthDictionary"
			});
			if (onlyDashboard)
			{
				list.Clear();
			}
			if (string.IsNullOrEmpty(SharedSettings.Current.Dashboard))
			{
				list2.Remove("Dashboard");
				list.Add("Dashboard");
			}
			foreach (string text2 in dictionary.Keys)
			{
				try
				{
					if (list.Contains(text2) && !list2.Contains(text2))
					{
						string text3 = dictionary[text2];
						PropertyInfo property = typeFromHandle.GetProperty(text2);
						Type propertyType = property.PropertyType;
						if (propertyType == typeof(int))
						{
							int num = int.Parse(text3, CultureInfo.InvariantCulture.NumberFormat);
							property.SetValue(SharedSettings.Current, num);
						}
						else if (propertyType == typeof(string))
						{
							property.SetValue(SharedSettings.Current, text3);
						}
						else if (propertyType == typeof(Enum))
						{
							int num2 = int.Parse(text3, CultureInfo.InvariantCulture.NumberFormat);
							object obj = Enum.ToObject(property.PropertyType, num2);
							property.SetValue(SharedSettings.Current, obj);
						}
						else if (propertyType == typeof(bool))
						{
							int num3 = int.Parse(text3, CultureInfo.InvariantCulture.NumberFormat);
							if (num3 == 0)
							{
								property.SetValue(SharedSettings.Current, false);
							}
							else if (num3 == 1)
							{
								property.SetValue(SharedSettings.Current, true);
							}
						}
						else if (propertyType == typeof(long))
						{
							long num4 = long.Parse(text3, CultureInfo.InvariantCulture.NumberFormat);
							property.SetValue(SharedSettings.Current, num4);
						}
						else if (propertyType == typeof(double))
						{
							double num5 = double.Parse(text3, CultureInfo.InvariantCulture.NumberFormat);
							property.SetValue(SharedSettings.Current, num5);
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002273 RID: 8819 RVA: 0x001A9C80 File Offset: 0x001A7E80
		public static void ImportProfilePids(OBDReaderProfileV2 profile)
		{
			CustomPID[] array = CustomPIDViewModel.CurrentProfile.PidCollection.ToArray<CustomPID>();
			if (!CustomPIDViewModel.DisabledProfile.Loaded)
			{
				CustomPIDViewModel.DisabledProfile.Load();
			}
			CustomPID[] array2 = CustomPIDViewModel.DisabledProfile.PidCollection.ToArray<CustomPID>();
			CustomPIDViewModel.CurrentProfile.PidCollection.Clear();
			CustomPIDViewModel.DisabledProfile.PidCollection.Clear();
			try
			{
				CustomPIDViewModel.CurrentProfile.AddFromString(profile.ProfilePIDs);
			}
			catch (Exception)
			{
				foreach (CustomPID customPID in array)
				{
					CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID);
				}
				foreach (CustomPID customPID2 in array2)
				{
					CustomPIDViewModel.DisabledProfile.PidCollection.Add(customPID2);
				}
				return;
			}
			CustomPID[] array4 = CustomPIDViewModel.CurrentProfile.PidCollection.ToArray<CustomPID>();
			CustomPIDViewModel.CurrentProfile.PidCollection.Clear();
			CustomPID[] array3 = array4;
			for (int i = 0; i < array3.Length; i++)
			{
				CustomPID new_pid = array3[i];
				CustomPID customPID3 = array2.FirstOrDefault((CustomPID x) => x.Equals(new_pid) || x.Id == new_pid.Id);
				if (customPID3 != null)
				{
					new_pid.Id = customPID3.Id;
					CustomPIDViewModel.DisabledProfile.PidCollection.Add(new_pid);
				}
				else
				{
					customPID3 = array.FirstOrDefault((CustomPID x) => x.Equals(new_pid));
					if (customPID3 != null)
					{
						new_pid.Id = customPID3.Id;
					}
					CustomPIDViewModel.CurrentProfile.PidCollection.Add(new_pid);
				}
			}
			CustomPIDViewModel.CurrentProfile.Save();
			CustomPIDViewModel.DisabledProfile.Save();
			foreach (CustomPID customPID4 in CustomPIDViewModel.CurrentProfile.PidCollection)
			{
				if (customPID4.Type == CustomPIDType.Formula && (customPID4.Formula.Contains('{', StringComparison.Ordinal) || customPID4.Formula.Contains("PID(", StringComparison.OrdinalIgnoreCase)))
				{
					customPID4.ReloadFormula();
				}
			}
		}

		// Token: 0x06002274 RID: 8820 RVA: 0x001A9EA8 File Offset: 0x001A80A8
		private static void SetEnabled()
		{
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentProfile.PidCollection)
			{
				customPID.IsAvailable = true;
			}
		}

		// Token: 0x04001053 RID: 4179
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001054 RID: 4180
		private string _Name = "";

		// Token: 0x04001055 RID: 4181
		private string _BrandForDTC = "";

		// Token: 0x04001056 RID: 4182
		private string _Description = "";

		// Token: 0x04001057 RID: 4183
		private List<string> _Brands = new List<string>();

		// Token: 0x04001058 RID: 4184
		private List<string> _Aliases = new List<string>();

		// Token: 0x04001059 RID: 4185
		private string _TesterPresentCommand = "0100";

		// Token: 0x0400105A RID: 4186
		private bool _SendTesterPresent = true;

		// Token: 0x0400105B RID: 4187
		private string _InitSequence = "ATZ\nATE0\nATH1\nATSP0\nATS0\nATM0\nATAT1";

		// Token: 0x0400105C RID: 4188
		private string _CheckConnectionCommand = "0100";

		// Token: 0x0400105D RID: 4189
		private bool _UseOBD2 = true;

		// Token: 0x0400105E RID: 4190
		private string _DefaultHeader = "";

		// Token: 0x0400105F RID: 4191
		private string _ProfilePIDs = "";

		// Token: 0x04001060 RID: 4192
		private DTCModeV2 _DTCReadingModeV2;

		// Token: 0x04001061 RID: 4193
		private string _DTCReadingSequence = "";

		// Token: 0x04001062 RID: 4194
		private DTCModeV2 _DTCClearingModeV2;

		// Token: 0x04001063 RID: 4195
		private string _DTCClearingSequence = "";

		// Token: 0x04001064 RID: 4196
		private bool _UseCustomInit;

		// Token: 0x04001065 RID: 4197
		private int _DefaultProtocolSelected;

		// Token: 0x04001066 RID: 4198
		private bool _IsExperimental;

		// Token: 0x04001067 RID: 4199
		private bool _ShouldCheckProfilePIDs;

		// Token: 0x04001068 RID: 4200
		private bool _ForceOnlyOneProtocol;

		// Token: 0x04001069 RID: 4201
		private bool _DaihatsuMode;

		// Token: 0x0400106A RID: 4202
		private string _Mode01Prefix = "01";

		// Token: 0x0400106B RID: 4203
		private int _ResponseMarkerLength;

		// Token: 0x0400106C RID: 4204
		private bool _RequestECUInfo = true;

		// Token: 0x0400106D RID: 4205
		private int _PIDsIndexStart = 10000000;

		// Token: 0x0400106E RID: 4206
		private string _Tweaks = "";

		// Token: 0x0400106F RID: 4207
		[CompilerGenerated]
		private bool <ProfileHasPids>k__BackingField;

		// Token: 0x020002C2 RID: 706
		[CompilerGenerated]
		private sealed class <>c__DisplayClass119_0
		{
			// Token: 0x06002275 RID: 8821 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass119_0()
			{
			}

			// Token: 0x06002276 RID: 8822 RVA: 0x001A9EF8 File Offset: 0x001A80F8
			internal bool <ImportProfilePids>b__0(CustomPID x)
			{
				return x.Equals(this.new_pid) || x.Id == this.new_pid.Id;
			}

			// Token: 0x06002277 RID: 8823 RVA: 0x001A9F1D File Offset: 0x001A811D
			internal bool <ImportProfilePids>b__1(CustomPID x)
			{
				return x.Equals(this.new_pid);
			}

			// Token: 0x04001070 RID: 4208
			public CustomPID new_pid;
		}
	}
}
