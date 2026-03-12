using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000724 RID: 1828
	public class CarInfoViewModel : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x17001450 RID: 5200
		// (get) Token: 0x06003E1D RID: 15901 RVA: 0x0032B615 File Offset: 0x00329815
		// (set) Token: 0x06003E1E RID: 15902 RVA: 0x0032B61C File Offset: 0x0032981C
		public static CarInfoViewModel Instance
		{
			[CompilerGenerated]
			get
			{
				return CarInfoViewModel.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				CarInfoViewModel.<Instance>k__BackingField = value;
			}
		} = new CarInfoViewModel();

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06003E1F RID: 15903 RVA: 0x0032B624 File Offset: 0x00329824
		// (remove) Token: 0x06003E20 RID: 15904 RVA: 0x0032B65C File Offset: 0x0032985C
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

		// Token: 0x06003E21 RID: 15905 RVA: 0x0032B694 File Offset: 0x00329894
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
				});
			}
		}

		// Token: 0x17001451 RID: 5201
		// (get) Token: 0x06003E22 RID: 15906 RVA: 0x0032B6DA File Offset: 0x003298DA
		// (set) Token: 0x06003E23 RID: 15907 RVA: 0x0032B6E2 File Offset: 0x003298E2
		public bool IsRefreshing
		{
			[CompilerGenerated]
			get
			{
				return this.<IsRefreshing>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsRefreshing>k__BackingField = value;
			}
		}

		// Token: 0x17001452 RID: 5202
		// (get) Token: 0x06003E24 RID: 15908 RVA: 0x0032B6EB File Offset: 0x003298EB
		public ICommand FullRefreshCommand
		{
			get
			{
				return new Command(async delegate(object obj)
				{
					this.IsRefreshing = true;
					this.NotifyPropertyChanged("IsRefreshing");
					await this.Load(true);
					this.IsRefreshing = false;
					this.NotifyPropertyChanged("IsRefreshing");
				});
			}
		}

		// Token: 0x17001453 RID: 5203
		// (get) Token: 0x06003E25 RID: 15909 RVA: 0x0032B6FE File Offset: 0x003298FE
		// (set) Token: 0x06003E26 RID: 15910 RVA: 0x0032B708 File Offset: 0x00329908
		public string VIN
		{
			get
			{
				return this._VIN;
			}
			set
			{
				this._VIN = value;
				if (string.IsNullOrEmpty(value))
				{
					this.IsVINAvailable = false;
				}
				else
				{
					this.IsVINAvailable = true;
				}
				this.NotifyPropertyChanged("VIN");
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.OnVINLoaded(this.VIN);
			}
		}

		// Token: 0x17001454 RID: 5204
		// (get) Token: 0x06003E27 RID: 15911 RVA: 0x0032B754 File Offset: 0x00329954
		// (set) Token: 0x06003E28 RID: 15912 RVA: 0x0032B75C File Offset: 0x0032995C
		public bool IsVINAvailable
		{
			get
			{
				return this._IsVINAvailable;
			}
			set
			{
				this._IsVINAvailable = value;
				this.NotifyPropertyChanged("IsVINAvailable");
			}
		}

		// Token: 0x17001455 RID: 5205
		// (get) Token: 0x06003E29 RID: 15913 RVA: 0x0032B770 File Offset: 0x00329970
		// (set) Token: 0x06003E2A RID: 15914 RVA: 0x0032B778 File Offset: 0x00329978
		public string CalibrationId
		{
			get
			{
				return this._CalibrationId;
			}
			set
			{
				this._CalibrationId = value;
				if (string.IsNullOrEmpty(value))
				{
					this.IsCalibrationIdAvailable = false;
				}
				else
				{
					this.IsCalibrationIdAvailable = true;
				}
				this.NotifyPropertyChanged("CalibrationId");
			}
		}

		// Token: 0x17001456 RID: 5206
		// (get) Token: 0x06003E2B RID: 15915 RVA: 0x0032B7A4 File Offset: 0x003299A4
		// (set) Token: 0x06003E2C RID: 15916 RVA: 0x0032B7AC File Offset: 0x003299AC
		public bool IsCalibrationIdAvailable
		{
			get
			{
				return this._IsCalibrationIdAvailable;
			}
			set
			{
				this._IsCalibrationIdAvailable = value;
				this.NotifyPropertyChanged("IsCalibrationIdAvailable");
			}
		}

		// Token: 0x17001457 RID: 5207
		// (get) Token: 0x06003E2D RID: 15917 RVA: 0x0032B7C0 File Offset: 0x003299C0
		// (set) Token: 0x06003E2E RID: 15918 RVA: 0x0032B7C8 File Offset: 0x003299C8
		public string ECUName
		{
			get
			{
				return this._ECUName;
			}
			set
			{
				this._ECUName = value;
				if (string.IsNullOrEmpty(value))
				{
					this.IsECUNameAvailable = false;
				}
				else
				{
					this.IsECUNameAvailable = true;
				}
				this.NotifyPropertyChanged("ECUName");
			}
		}

		// Token: 0x17001458 RID: 5208
		// (get) Token: 0x06003E2F RID: 15919 RVA: 0x0032B7F4 File Offset: 0x003299F4
		// (set) Token: 0x06003E30 RID: 15920 RVA: 0x0032B7FC File Offset: 0x003299FC
		public bool IsECUNameAvailable
		{
			get
			{
				return this._IsECUNameAvailable;
			}
			set
			{
				this._IsECUNameAvailable = value;
				this.NotifyPropertyChanged("IsECUNameAvailable");
			}
		}

		// Token: 0x17001459 RID: 5209
		// (get) Token: 0x06003E31 RID: 15921 RVA: 0x0032B810 File Offset: 0x00329A10
		// (set) Token: 0x06003E32 RID: 15922 RVA: 0x0032B818 File Offset: 0x00329A18
		public string OBDProtocol
		{
			get
			{
				return this._OBDProtocol;
			}
			set
			{
				this._OBDProtocol = value;
				this.NotifyPropertyChanged("OBDProtocol");
			}
		}

		// Token: 0x1700145A RID: 5210
		// (get) Token: 0x06003E33 RID: 15923 RVA: 0x0032B82C File Offset: 0x00329A2C
		// (set) Token: 0x06003E34 RID: 15924 RVA: 0x0032B834 File Offset: 0x00329A34
		public bool CurrentDriveCycleSupported
		{
			get
			{
				return this._CurrentDriveCycleSupported;
			}
			set
			{
				this._CurrentDriveCycleSupported = value;
				this.NotifyPropertyChanged("CurrentDriveCycleSupported");
			}
		}

		// Token: 0x1700145B RID: 5211
		// (get) Token: 0x06003E35 RID: 15925 RVA: 0x0032B848 File Offset: 0x00329A48
		// (set) Token: 0x06003E36 RID: 15926 RVA: 0x0032B850 File Offset: 0x00329A50
		public bool StatusSinceDTCSupported
		{
			get
			{
				return this._StatusSinceDTCSupported;
			}
			set
			{
				this._StatusSinceDTCSupported = value;
				this.NotifyPropertyChanged("StatusSinceDTCSupported");
			}
		}

		// Token: 0x1700145C RID: 5212
		// (get) Token: 0x06003E37 RID: 15927 RVA: 0x0032B864 File Offset: 0x00329A64
		// (set) Token: 0x06003E38 RID: 15928 RVA: 0x0032B86C File Offset: 0x00329A6C
		public string VehicleManufacturerECUHardwareNumber
		{
			get
			{
				return this._VehicleManufacturerECUHardwareNumber;
			}
			set
			{
				this._VehicleManufacturerECUHardwareNumber = value;
				this.NotifyPropertyChanged("VehicleManufacturerECUHardwareNumber");
			}
		}

		// Token: 0x1700145D RID: 5213
		// (get) Token: 0x06003E39 RID: 15929 RVA: 0x0032B880 File Offset: 0x00329A80
		// (set) Token: 0x06003E3A RID: 15930 RVA: 0x0032B888 File Offset: 0x00329A88
		public string SystemSupplierECUHardwareNumber
		{
			get
			{
				return this._SystemSupplierECUHardwareNumber;
			}
			set
			{
				this._SystemSupplierECUHardwareNumber = value;
				this.NotifyPropertyChanged("SystemSupplierECUHardwareNumber");
			}
		}

		// Token: 0x1700145E RID: 5214
		// (get) Token: 0x06003E3B RID: 15931 RVA: 0x0032B89C File Offset: 0x00329A9C
		// (set) Token: 0x06003E3C RID: 15932 RVA: 0x0032B8A4 File Offset: 0x00329AA4
		public string SystemSupplierECUHardwareVersion
		{
			get
			{
				return this._SystemSupplierECUHardwareVersion;
			}
			set
			{
				this._SystemSupplierECUHardwareVersion = value;
				this.NotifyPropertyChanged("SystemSupplierECUHardwareVersion");
			}
		}

		// Token: 0x1700145F RID: 5215
		// (get) Token: 0x06003E3D RID: 15933 RVA: 0x0032B8B8 File Offset: 0x00329AB8
		// (set) Token: 0x06003E3E RID: 15934 RVA: 0x0032B8C0 File Offset: 0x00329AC0
		public string SystemSupplierECUSoftwareNumber
		{
			get
			{
				return this._SystemSupplierECUSoftwareNumber;
			}
			set
			{
				this._SystemSupplierECUSoftwareNumber = value;
				this.NotifyPropertyChanged("SystemSupplierECUSoftwareNumber");
			}
		}

		// Token: 0x17001460 RID: 5216
		// (get) Token: 0x06003E3F RID: 15935 RVA: 0x0032B8D4 File Offset: 0x00329AD4
		// (set) Token: 0x06003E40 RID: 15936 RVA: 0x0032B8DC File Offset: 0x00329ADC
		public string SystemSupplierECUSoftwareVersion
		{
			get
			{
				return this._SystemSupplierECUSoftwareVersion;
			}
			set
			{
				this._SystemSupplierECUSoftwareVersion = value;
				this.NotifyPropertyChanged("SystemSupplierECUSoftwareVersion");
			}
		}

		// Token: 0x17001461 RID: 5217
		// (get) Token: 0x06003E41 RID: 15937 RVA: 0x0032B8F0 File Offset: 0x00329AF0
		// (set) Token: 0x06003E42 RID: 15938 RVA: 0x0032B8F8 File Offset: 0x00329AF8
		public string ExhaustRegulationOrTypeApprovalNumber
		{
			get
			{
				return this._ExhaustRegulationOrTypeApprovalNumber;
			}
			set
			{
				this._ExhaustRegulationOrTypeApprovalNumber = value;
				this.NotifyPropertyChanged("ExhaustRegulationOrTypeApprovalNumber");
			}
		}

		// Token: 0x17001462 RID: 5218
		// (get) Token: 0x06003E43 RID: 15939 RVA: 0x0032B90C File Offset: 0x00329B0C
		// (set) Token: 0x06003E44 RID: 15940 RVA: 0x0032B914 File Offset: 0x00329B14
		public string SystemNameOrEngineType
		{
			get
			{
				return this._SystemNameOrEngineType;
			}
			set
			{
				this._SystemNameOrEngineType = value;
				this.NotifyPropertyChanged("SystemNameOrEngineType");
			}
		}

		// Token: 0x17001463 RID: 5219
		// (get) Token: 0x06003E45 RID: 15941 RVA: 0x0032B928 File Offset: 0x00329B28
		// (set) Token: 0x06003E46 RID: 15942 RVA: 0x0032B930 File Offset: 0x00329B30
		public string RepairShopCodeOrTesterSerialNumber
		{
			get
			{
				return this._RepairShopCodeOrTesterSerialNumber;
			}
			set
			{
				this._RepairShopCodeOrTesterSerialNumber = value;
				this.NotifyPropertyChanged("RepairShopCodeOrTesterSerialNumber");
			}
		}

		// Token: 0x17001464 RID: 5220
		// (get) Token: 0x06003E47 RID: 15943 RVA: 0x0032B944 File Offset: 0x00329B44
		// (set) Token: 0x06003E48 RID: 15944 RVA: 0x0032B94C File Offset: 0x00329B4C
		public string ProgrammingDate
		{
			get
			{
				return this._ProgrammingDate;
			}
			set
			{
				this._ProgrammingDate = value;
				this.NotifyPropertyChanged("ProgrammingDate");
			}
		}

		// Token: 0x17001465 RID: 5221
		// (get) Token: 0x06003E49 RID: 15945 RVA: 0x0032B960 File Offset: 0x00329B60
		// (set) Token: 0x06003E4A RID: 15946 RVA: 0x0032B968 File Offset: 0x00329B68
		public bool IsStatisticsAvailable
		{
			get
			{
				return this._IsStatisticsAvailable;
			}
			set
			{
				if (this._IsStatisticsAvailable != value)
				{
					this._IsStatisticsAvailable = value;
					this.NotifyPropertyChanged("IsStatisticsAvailable");
				}
			}
		}

		// Token: 0x17001466 RID: 5222
		// (get) Token: 0x06003E4B RID: 15947 RVA: 0x0032B985 File Offset: 0x00329B85
		public ObservableCollection<TestGroup> TestsCollection
		{
			get
			{
				return this._TestsCollection;
			}
		}

		// Token: 0x17001467 RID: 5223
		// (get) Token: 0x06003E4C RID: 15948 RVA: 0x0032B98D File Offset: 0x00329B8D
		public TestGroup CurrentCycleTestInfo
		{
			get
			{
				return this._CurrentCycleTestInfo;
			}
		}

		// Token: 0x17001468 RID: 5224
		// (get) Token: 0x06003E4D RID: 15949 RVA: 0x0032B995 File Offset: 0x00329B95
		public TestGroup SinceDTCClearedTestInfo
		{
			get
			{
				return this._SinceDTCClearedTestInfo;
			}
		}

		// Token: 0x06003E4E RID: 15950 RVA: 0x0032B9A0 File Offset: 0x00329BA0
		public CarInfoViewModel()
		{
			this.TestsCollection.Add(this.SinceDTCClearedTestInfo);
			this.VIN = "";
			this.ECUName = "";
			this.CalibrationId = "";
			try
			{
				this.OBDProtocol = StaticLists.Protocols[App.OBDReader.CurrentProtocolNumber];
			}
			catch (Exception)
			{
				this.OBDProtocol = this.GetProtocolStringFromId(SharedSettings.Current.ProtocolNumber);
			}
		}

		// Token: 0x06003E4F RID: 15951 RVA: 0x0032BAF0 File Offset: 0x00329CF0
		public void Reset()
		{
			this.TestsCollection.Clear();
			this.TestsCollection.Add(this.SinceDTCClearedTestInfo);
			this.VIN = "";
			this.ECUName = "";
			this.CalibrationId = "";
			this.VehicleManufacturerECUHardwareNumber = "";
			this.SystemNameOrEngineType = "";
			this.SystemSupplierECUHardwareNumber = "";
			this.SystemSupplierECUHardwareVersion = "";
			this.SystemSupplierECUSoftwareNumber = "";
			this.SystemSupplierECUSoftwareVersion = "";
			this.ExhaustRegulationOrTypeApprovalNumber = "";
			this.RepairShopCodeOrTesterSerialNumber = "";
			this.ProgrammingDate = "";
			this.IsStatisticsAvailable = false;
			this.OBDProtocol = this.GetProtocolStringFromId(SharedSettings.Current.ProtocolNumber);
			this.IsStatisticsAvailable = false;
		}

		// Token: 0x06003E50 RID: 15952 RVA: 0x0032BBC1 File Offset: 0x00329DC1
		private void CarInfoViewModel_ValueChanged(object sender, PID e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				try
				{
					if (e is PIDWithStringValue)
					{
						PIDWithStringValue pidwithStringValue = e as PIDWithStringValue;
						pidwithStringValue.ValueChanged -= this.CarInfoViewModel_ValueChanged;
						string command = pidwithStringValue.Command;
						if (!(command == "0902"))
						{
							if (!(command == "0904"))
							{
								if (!(command == "090A"))
								{
									if (command == "1A90")
									{
										if (string.IsNullOrEmpty(this.VIN))
										{
											this.VIN = pidwithStringValue.Value;
										}
									}
								}
								else
								{
									this.ECUName = pidwithStringValue.Value;
								}
							}
							else
							{
								this.CalibrationId = pidwithStringValue.Value;
							}
						}
						else
						{
							this.VIN = pidwithStringValue.Value;
						}
					}
					else if (e is PID_Status)
					{
						PID_Status pid_Status = e as PID_Status;
						if (pid_Status.Id == 2)
						{
							this.SinceDTCClearedTestInfo.Clear();
							foreach (ECUTest ecutest in pid_Status.Value.ECUTests)
							{
								this.SinceDTCClearedTestInfo.Add(ecutest);
							}
							this.StatusSinceDTCSupported = true;
						}
						else
						{
							if (!this.TestsCollection.Contains(this.CurrentCycleTestInfo))
							{
								this.TestsCollection.Add(this.CurrentCycleTestInfo);
							}
							this.CurrentCycleTestInfo.Clear();
							foreach (ECUTest ecutest2 in pid_Status.Value.ECUTests)
							{
								this.CurrentCycleTestInfo.Add(ecutest2);
							}
							this.CurrentDriveCycleSupported = true;
						}
					}
					e.ValueChanged -= this.CarInfoViewModel_ValueChanged;
				}
				catch (Exception)
				{
				}
			});
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x0032BBE8 File Offset: 0x00329DE8
		public async Task Load(bool LoadTests)
		{
			if (!OBDReaderSimulator.Current.IsActive)
			{
				this.IsVINAvailable = false;
				this.IsECUNameAvailable = false;
				this.IsCalibrationIdAvailable = false;
				this.ECUName = "";
				this.VIN = "";
				this.CalibrationId = "";
				if (!SharedSettings.Current.DaihatsuKLine)
				{
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged -= this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged -= this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged -= this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged -= this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged -= this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged += this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged += this.CarInfoViewModel_ValueChanged;
					App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged += this.CarInfoViewModel_ValueChanged;
					App.OBDReader.AddRequestToQueue("0902");
					App.OBDReader.AddRequestToQueue("0904");
					App.OBDReader.AddRequestToQueue("090A");
					this.IsStatisticsAvailable = false;
					if (LoadTests)
					{
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged += this.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged += this.CarInfoViewModel_ValueChanged;
						App.OBDReader.AddRequestToQueue(SharedSettings.Current.Mode01Prefix + "01");
						App.OBDReader.AddRequestToQueue("0141");
						App.OBDReader.AddRequestToQueue(SharedSettings.Current.Mode01Prefix + "41");
					}
					this.OBDProtocol = this.GetProtocolStringFromId(App.OBDReader.CurrentProtocolNumber);
					await App.OBDReader.WaitForCommandQueue();
				}
			}
			else
			{
				this.VIN = "WP0ZZZ99ZTS392124";
				this.ECUName = "Random engine ECU";
				this.CalibrationId = "1234567890A";
				this.OBDProtocol = this.GetProtocolStringFromId(SharedSettings.Current.ProtocolNumber);
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged -= this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged -= this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged -= this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged -= this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged -= this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged += this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged += this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged += this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged += this.CarInfoViewModel_ValueChanged;
				App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged += this.CarInfoViewModel_ValueChanged;
			}
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x0032BC34 File Offset: 0x00329E34
		private string GetProtocolStringFromId(int id)
		{
			switch (id)
			{
			case 0:
				return "Auto";
			case 1:
				return "SAE J1850 PWM (41.6 kbaud)";
			case 2:
				return "SAE J1850 VPM (10.4 kbaud)";
			case 3:
				return "ISO 9141-2 (5 baud init, 10.4 kbaud)";
			case 4:
				return "ISO 14230-4 KWP (5 baud init, 10.4 kbaud)";
			case 5:
				return "ISO 14230-4 KWP (fast baud init, 10.4 kbaud)";
			case 6:
				return "ISO 15765-4 CAN (11 bit ID, 500 kbaud)";
			case 7:
				return "ISO 15765-4 CAN (29 bit ID, 500 kbaud)";
			case 8:
				return "ISO 15765-4 CAN (11 bit ID, 250 kbaud)";
			case 9:
				return "ISO 15765-4 CAN (29 bit ID, 250 kbaud)";
			case 10:
				return "SAE J1939 CAN (11 bit ID, 125 kbaud)";
			default:
				return "Unknown";
			}
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x0032BCBC File Offset: 0x00329EBC
		public void Dispose()
		{
			App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged -= this.CarInfoViewModel_ValueChanged;
			App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged -= this.CarInfoViewModel_ValueChanged;
			App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged -= this.CarInfoViewModel_ValueChanged;
			App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged -= this.CarInfoViewModel_ValueChanged;
			App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged -= this.CarInfoViewModel_ValueChanged;
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x0032BE1D File Offset: 0x0032A01D
		private void CarInfoViewModel_PID0908_ValueChanged(object sender, PID e)
		{
			if (e is IPIDFloatValue && !double.IsNaN(((IPIDFloatValue)e).Value))
			{
				this.IsStatisticsAvailable = true;
			}
		}

		// Token: 0x06003E55 RID: 15957 RVA: 0x0032BE40 File Offset: 0x0032A040
		// Note: this type is marked as 'beforefieldinit'.
		static CarInfoViewModel()
		{
		}

		// Token: 0x06003E56 RID: 15958 RVA: 0x0032BE4C File Offset: 0x0032A04C
		[CompilerGenerated]
		private async void <get_FullRefreshCommand>b__13_0(object obj)
		{
			this.IsRefreshing = true;
			this.NotifyPropertyChanged("IsRefreshing");
			await this.Load(true);
			this.IsRefreshing = false;
			this.NotifyPropertyChanged("IsRefreshing");
		}

		// Token: 0x04002614 RID: 9748
		[CompilerGenerated]
		private static CarInfoViewModel <Instance>k__BackingField;

		// Token: 0x04002615 RID: 9749
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002616 RID: 9750
		[CompilerGenerated]
		private bool <IsRefreshing>k__BackingField;

		// Token: 0x04002617 RID: 9751
		private string _VIN = "";

		// Token: 0x04002618 RID: 9752
		private bool _IsVINAvailable;

		// Token: 0x04002619 RID: 9753
		private string _CalibrationId = "";

		// Token: 0x0400261A RID: 9754
		private bool _IsCalibrationIdAvailable;

		// Token: 0x0400261B RID: 9755
		private string _ECUName = "";

		// Token: 0x0400261C RID: 9756
		private bool _IsECUNameAvailable;

		// Token: 0x0400261D RID: 9757
		private string _OBDProtocol = "";

		// Token: 0x0400261E RID: 9758
		private bool _CurrentDriveCycleSupported;

		// Token: 0x0400261F RID: 9759
		private bool _StatusSinceDTCSupported;

		// Token: 0x04002620 RID: 9760
		private string _VehicleManufacturerECUHardwareNumber = "";

		// Token: 0x04002621 RID: 9761
		private string _SystemSupplierECUHardwareNumber = "";

		// Token: 0x04002622 RID: 9762
		private string _SystemSupplierECUHardwareVersion = "";

		// Token: 0x04002623 RID: 9763
		private string _SystemSupplierECUSoftwareNumber = "";

		// Token: 0x04002624 RID: 9764
		private string _SystemSupplierECUSoftwareVersion = "";

		// Token: 0x04002625 RID: 9765
		private string _ExhaustRegulationOrTypeApprovalNumber = "";

		// Token: 0x04002626 RID: 9766
		private string _SystemNameOrEngineType = "";

		// Token: 0x04002627 RID: 9767
		private string _RepairShopCodeOrTesterSerialNumber = "";

		// Token: 0x04002628 RID: 9768
		private string _ProgrammingDate = "";

		// Token: 0x04002629 RID: 9769
		private bool _IsStatisticsAvailable;

		// Token: 0x0400262A RID: 9770
		private ObservableCollection<TestGroup> _TestsCollection = new ObservableCollection<TestGroup>();

		// Token: 0x0400262B RID: 9771
		private TestGroup _CurrentCycleTestInfo = new TestGroup(Translate.GetString("ios_EmissionTests_CurrentDriveCycle"));

		// Token: 0x0400262C RID: 9772
		private TestGroup _SinceDTCClearedTestInfo = new TestGroup(Translate.GetString("ios_EmissionTests_SinceDTCReset"));

		// Token: 0x02000725 RID: 1829
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_FullRefreshCommand>b__13_0>d : IAsyncStateMachine
		{
			// Token: 0x06003E57 RID: 15959 RVA: 0x0032BE84 File Offset: 0x0032A084
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarInfoViewModel carInfoViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						carInfoViewModel.IsRefreshing = true;
						carInfoViewModel.NotifyPropertyChanged("IsRefreshing");
						taskAwaiter = carInfoViewModel.Load(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarInfoViewModel.<<get_FullRefreshCommand>b__13_0>d>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					carInfoViewModel.IsRefreshing = false;
					carInfoViewModel.NotifyPropertyChanged("IsRefreshing");
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

			// Token: 0x06003E58 RID: 15960 RVA: 0x0032BF5C File Offset: 0x0032A15C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400262D RID: 9773
			public int <>1__state;

			// Token: 0x0400262E RID: 9774
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400262F RID: 9775
			public CarInfoViewModel <>4__this;

			// Token: 0x04002630 RID: 9776
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000726 RID: 1830
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003E59 RID: 15961 RVA: 0x0032BF6A File Offset: 0x0032A16A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003E5A RID: 15962 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003E5B RID: 15963 RVA: 0x0032BF76 File Offset: 0x0032A176
			internal bool <Load>b__102_0(PID x)
			{
				return x.Id == 242;
			}

			// Token: 0x06003E5C RID: 15964 RVA: 0x0032BF85 File Offset: 0x0032A185
			internal bool <Load>b__102_1(PID x)
			{
				return x.Id == 243;
			}

			// Token: 0x06003E5D RID: 15965 RVA: 0x0032BF94 File Offset: 0x0032A194
			internal bool <Load>b__102_2(PID x)
			{
				return x.Id == 244;
			}

			// Token: 0x06003E5E RID: 15966 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Load>b__102_3(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x06003E5F RID: 15967 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Load>b__102_4(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x06003E60 RID: 15968 RVA: 0x0032BF76 File Offset: 0x0032A176
			internal bool <Load>b__102_5(PID x)
			{
				return x.Id == 242;
			}

			// Token: 0x06003E61 RID: 15969 RVA: 0x0032BF85 File Offset: 0x0032A185
			internal bool <Load>b__102_6(PID x)
			{
				return x.Id == 243;
			}

			// Token: 0x06003E62 RID: 15970 RVA: 0x0032BF94 File Offset: 0x0032A194
			internal bool <Load>b__102_7(PID x)
			{
				return x.Id == 244;
			}

			// Token: 0x06003E63 RID: 15971 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Load>b__102_8(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x06003E64 RID: 15972 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Load>b__102_9(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x06003E65 RID: 15973 RVA: 0x0032BF76 File Offset: 0x0032A176
			internal bool <Load>b__102_10(PID x)
			{
				return x.Id == 242;
			}

			// Token: 0x06003E66 RID: 15974 RVA: 0x0032BF85 File Offset: 0x0032A185
			internal bool <Load>b__102_11(PID x)
			{
				return x.Id == 243;
			}

			// Token: 0x06003E67 RID: 15975 RVA: 0x0032BF94 File Offset: 0x0032A194
			internal bool <Load>b__102_12(PID x)
			{
				return x.Id == 244;
			}

			// Token: 0x06003E68 RID: 15976 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Load>b__102_13(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x06003E69 RID: 15977 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Load>b__102_14(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x06003E6A RID: 15978 RVA: 0x0032BF76 File Offset: 0x0032A176
			internal bool <Load>b__102_15(PID x)
			{
				return x.Id == 242;
			}

			// Token: 0x06003E6B RID: 15979 RVA: 0x0032BF85 File Offset: 0x0032A185
			internal bool <Load>b__102_16(PID x)
			{
				return x.Id == 243;
			}

			// Token: 0x06003E6C RID: 15980 RVA: 0x0032BF94 File Offset: 0x0032A194
			internal bool <Load>b__102_17(PID x)
			{
				return x.Id == 244;
			}

			// Token: 0x06003E6D RID: 15981 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Load>b__102_18(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x06003E6E RID: 15982 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Load>b__102_19(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x06003E6F RID: 15983 RVA: 0x0032BF76 File Offset: 0x0032A176
			internal bool <Dispose>b__104_0(PID x)
			{
				return x.Id == 242;
			}

			// Token: 0x06003E70 RID: 15984 RVA: 0x0032BF85 File Offset: 0x0032A185
			internal bool <Dispose>b__104_1(PID x)
			{
				return x.Id == 243;
			}

			// Token: 0x06003E71 RID: 15985 RVA: 0x0032BF94 File Offset: 0x0032A194
			internal bool <Dispose>b__104_2(PID x)
			{
				return x.Id == 244;
			}

			// Token: 0x06003E72 RID: 15986 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Dispose>b__104_3(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x06003E73 RID: 15987 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Dispose>b__104_4(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x04002631 RID: 9777
			public static readonly CarInfoViewModel.<>c <>9 = new CarInfoViewModel.<>c();

			// Token: 0x04002632 RID: 9778
			public static Func<PID, bool> <>9__102_0;

			// Token: 0x04002633 RID: 9779
			public static Func<PID, bool> <>9__102_1;

			// Token: 0x04002634 RID: 9780
			public static Func<PID, bool> <>9__102_2;

			// Token: 0x04002635 RID: 9781
			public static Func<PID, bool> <>9__102_3;

			// Token: 0x04002636 RID: 9782
			public static Func<PID, bool> <>9__102_4;

			// Token: 0x04002637 RID: 9783
			public static Func<PID, bool> <>9__102_5;

			// Token: 0x04002638 RID: 9784
			public static Func<PID, bool> <>9__102_6;

			// Token: 0x04002639 RID: 9785
			public static Func<PID, bool> <>9__102_7;

			// Token: 0x0400263A RID: 9786
			public static Func<PID, bool> <>9__102_8;

			// Token: 0x0400263B RID: 9787
			public static Func<PID, bool> <>9__102_9;

			// Token: 0x0400263C RID: 9788
			public static Func<PID, bool> <>9__102_10;

			// Token: 0x0400263D RID: 9789
			public static Func<PID, bool> <>9__102_11;

			// Token: 0x0400263E RID: 9790
			public static Func<PID, bool> <>9__102_12;

			// Token: 0x0400263F RID: 9791
			public static Func<PID, bool> <>9__102_13;

			// Token: 0x04002640 RID: 9792
			public static Func<PID, bool> <>9__102_14;

			// Token: 0x04002641 RID: 9793
			public static Func<PID, bool> <>9__102_15;

			// Token: 0x04002642 RID: 9794
			public static Func<PID, bool> <>9__102_16;

			// Token: 0x04002643 RID: 9795
			public static Func<PID, bool> <>9__102_17;

			// Token: 0x04002644 RID: 9796
			public static Func<PID, bool> <>9__102_18;

			// Token: 0x04002645 RID: 9797
			public static Func<PID, bool> <>9__102_19;

			// Token: 0x04002646 RID: 9798
			public static Func<PID, bool> <>9__104_0;

			// Token: 0x04002647 RID: 9799
			public static Func<PID, bool> <>9__104_1;

			// Token: 0x04002648 RID: 9800
			public static Func<PID, bool> <>9__104_2;

			// Token: 0x04002649 RID: 9801
			public static Func<PID, bool> <>9__104_3;

			// Token: 0x0400264A RID: 9802
			public static Func<PID, bool> <>9__104_4;
		}

		// Token: 0x02000727 RID: 1831
		[CompilerGenerated]
		private sealed class <>c__DisplayClass101_0
		{
			// Token: 0x06003E74 RID: 15988 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass101_0()
			{
			}

			// Token: 0x06003E75 RID: 15989 RVA: 0x0032BFA4 File Offset: 0x0032A1A4
			internal void <CarInfoViewModel_ValueChanged>b__0()
			{
				try
				{
					if (this.e is PIDWithStringValue)
					{
						PIDWithStringValue pidwithStringValue = this.e as PIDWithStringValue;
						pidwithStringValue.ValueChanged -= this.<>4__this.CarInfoViewModel_ValueChanged;
						string command = pidwithStringValue.Command;
						if (!(command == "0902"))
						{
							if (!(command == "0904"))
							{
								if (!(command == "090A"))
								{
									if (command == "1A90")
									{
										if (string.IsNullOrEmpty(this.<>4__this.VIN))
										{
											this.<>4__this.VIN = pidwithStringValue.Value;
										}
									}
								}
								else
								{
									this.<>4__this.ECUName = pidwithStringValue.Value;
								}
							}
							else
							{
								this.<>4__this.CalibrationId = pidwithStringValue.Value;
							}
						}
						else
						{
							this.<>4__this.VIN = pidwithStringValue.Value;
						}
					}
					else if (this.e is PID_Status)
					{
						PID_Status pid_Status = this.e as PID_Status;
						if (pid_Status.Id == 2)
						{
							this.<>4__this.SinceDTCClearedTestInfo.Clear();
							foreach (ECUTest ecutest in pid_Status.Value.ECUTests)
							{
								this.<>4__this.SinceDTCClearedTestInfo.Add(ecutest);
							}
							this.<>4__this.StatusSinceDTCSupported = true;
						}
						else
						{
							if (!this.<>4__this.TestsCollection.Contains(this.<>4__this.CurrentCycleTestInfo))
							{
								this.<>4__this.TestsCollection.Add(this.<>4__this.CurrentCycleTestInfo);
							}
							this.<>4__this.CurrentCycleTestInfo.Clear();
							foreach (ECUTest ecutest2 in pid_Status.Value.ECUTests)
							{
								this.<>4__this.CurrentCycleTestInfo.Add(ecutest2);
							}
							this.<>4__this.CurrentDriveCycleSupported = true;
						}
					}
					this.e.ValueChanged -= this.<>4__this.CarInfoViewModel_ValueChanged;
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0400264B RID: 9803
			public PID e;

			// Token: 0x0400264C RID: 9804
			public CarInfoViewModel <>4__this;
		}

		// Token: 0x02000728 RID: 1832
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06003E76 RID: 15990 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06003E77 RID: 15991 RVA: 0x0032C1D4 File Offset: 0x0032A3D4
			internal void <NotifyPropertyChanged>b__0()
			{
				this.propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.PropertyName));
			}

			// Token: 0x0400264D RID: 9805
			public PropertyChangedEventHandler propertyChanged;

			// Token: 0x0400264E RID: 9806
			public CarInfoViewModel <>4__this;

			// Token: 0x0400264F RID: 9807
			public string PropertyName;
		}

		// Token: 0x02000729 RID: 1833
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Load>d__102 : IAsyncStateMachine
		{
			// Token: 0x06003E78 RID: 15992 RVA: 0x0032C1F4 File Offset: 0x0032A3F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarInfoViewModel carInfoViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (OBDReaderSimulator.Current.IsActive)
						{
							carInfoViewModel.VIN = "WP0ZZZ99ZTS392124";
							carInfoViewModel.ECUName = "Random engine ECU";
							carInfoViewModel.CalibrationId = "1234567890A";
							carInfoViewModel.OBDProtocol = carInfoViewModel.GetProtocolStringFromId(SharedSettings.Current.ProtocolNumber);
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							goto IL_06F5;
						}
						carInfoViewModel.IsVINAvailable = false;
						carInfoViewModel.IsECUNameAvailable = false;
						carInfoViewModel.IsCalibrationIdAvailable = false;
						carInfoViewModel.ECUName = "";
						carInfoViewModel.VIN = "";
						carInfoViewModel.CalibrationId = "";
						if (SharedSettings.Current.DaihatsuKLine)
						{
							goto IL_070E;
						}
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged -= carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 242).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 243).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 244).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
						App.OBDReader.AddRequestToQueue("0902");
						App.OBDReader.AddRequestToQueue("0904");
						App.OBDReader.AddRequestToQueue("090A");
						carInfoViewModel.IsStatisticsAvailable = false;
						if (LoadTests)
						{
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89).ValueChanged += carInfoViewModel.CarInfoViewModel_ValueChanged;
							App.OBDReader.AddRequestToQueue(SharedSettings.Current.Mode01Prefix + "01");
							App.OBDReader.AddRequestToQueue("0141");
							App.OBDReader.AddRequestToQueue(SharedSettings.Current.Mode01Prefix + "41");
						}
						carInfoViewModel.OBDProtocol = carInfoViewModel.GetProtocolStringFromId(App.OBDReader.CurrentProtocolNumber);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarInfoViewModel.<Load>d__102>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_06F5:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_070E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003E79 RID: 15993 RVA: 0x0032C940 File Offset: 0x0032AB40
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002650 RID: 9808
			public int <>1__state;

			// Token: 0x04002651 RID: 9809
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002652 RID: 9810
			public CarInfoViewModel <>4__this;

			// Token: 0x04002653 RID: 9811
			public bool LoadTests;

			// Token: 0x04002654 RID: 9812
			private TaskAwaiter <>u__1;
		}
	}
}
