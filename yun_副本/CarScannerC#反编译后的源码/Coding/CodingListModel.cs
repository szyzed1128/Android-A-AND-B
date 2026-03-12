using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Coding.DB.Haval;
using CarScannerXamarinForms.Coding.DB.HyundaiKia;
using CarScannerXamarinForms.Coding.DB.MLB;
using CarScannerXamarinForms.Coding.DB.MLB_A4B9;
using CarScannerXamarinForms.Coding.DB.MLB_EVO;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Coding.DB.Nissan;
using CarScannerXamarinForms.Coding.DB.PQ26;
using CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020;
using CarScannerXamarinForms.Coding.DB.PQ35;
using CarScannerXamarinForms.Coding.DB.Renault;
using CarScannerXamarinForms.Coding.DB.Subaru;
using CarScannerXamarinForms.Coding.DB.Toyota;
using CarScannerXamarinForms.Coding.DB.VAG_OTHER;
using CarScannerXamarinForms.Coding.DB.Volvo;
using CarScannerXamarinForms.Coding.DB.VWTP;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000851 RID: 2129
	public class CodingListModel : INotifyPropertyChanged
	{
		// Token: 0x060048A0 RID: 18592 RVA: 0x00371064 File Offset: 0x0036F264
		public static bool IsCodingAvailable(bool checkVin)
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				CustomCodingsListViewModel customCodingsListViewModel = new CustomCodingsListViewModel();
				customCodingsListViewModel.Load();
				if (customCodingsListViewModel.Count > 0)
				{
					return true;
				}
			}
			try
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive)
				{
					string text = SharedSettings.Current.BrandForDTC;
					if (string.IsNullOrEmpty(text))
					{
						text = SharedSettings.Current.SelectedBrand;
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && !string.IsNullOrEmpty(text) && VagUnitHelper.IsVag(text))
					{
						string vin = CarInfoViewModel.Instance.VIN;
						if (!checkVin)
						{
							return true;
						}
						if (!string.IsNullOrEmpty(vin) && vin.Length >= 10 && vin[9] >= 'C')
						{
							return true;
						}
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && SharedSettings.Current.ShowExperimental && !string.IsNullOrEmpty(text) && VagUnitHelper.IsVag(text))
					{
						return true;
					}
					if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && checkVin && !string.IsNullOrEmpty(text) && VagUnitHelper.IsVag(text))
					{
						string vin2 = CarInfoViewModel.Instance.VIN;
						if (!string.IsNullOrEmpty(vin2) && vin2.Length >= 10 && vin2[9] >= '9')
						{
							return true;
						}
					}
					else
					{
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && (SharedSettings.Current.ProfileUpdateAlias == "c83763c04180489992fd1fedd17eaec5" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (EN)" || SharedSettings.Current.ProfileUpdateAlias == "6d7cef6bb26b48bb8f31cdfeba9e289f" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (RU)"))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (text == "Opel" || text == "Chevrolet" || text == "Vauxhall" || text == "Daewoo" || text == "Holden"))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Mitsubishi")
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Nissan" && SharedSettings.Current.ShowExperimental)
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Nissan" && SharedSettings.Current.SelectedProfileV2Name.Contains("Leaf"))
						{
							return true;
						}
						if (new string[] { "6e121b9dbd814f6bbf48df31c44bbcf5", "Nissan X-Trail T31 2.0", "fe6c577e35d3481cb0224323bbf90860", "698734883a5d461ca3e016bc3ba1fe3b", "Nissan X-Trail T31 2.5", "293952d48d9240c588356e57a0d9b94a", "Nissan X-Trail T31 CVT" }.Contains(SharedSettings.Current.ProfileUpdateAlias) && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.KWP))
						{
							return true;
						}
						if (new string[]
						{
							"Qashqai J11", "d5c487cd83354b52a8245759aa2ffce4", "Qashqai J11 1.5dci K9K 636 [EN]", "896225c9ee8f48abbdb9b48bf135f2da", "Qashqai J11 1.5dci K9K 636 [RU]", "c3f57cd35c1e4fd9aa3cc7b57cf254a9", "Rogue T32 2014-2020", "513ecb4ac8eb4df5addf1acf6887f8f8", "X-Trail T32", "d4d7c86928de468c9259bcff2c2d95fe",
							"X-Trail T32 (NT32) 2.0 MR20DD [EN]", "X-Trail T32 (NT32) 2.0 MR20DD [RU]", "aa016831f2ce473483f6c02463038a47", "X-Trail T32, Qashqai 1.6 dCi R9M [EN]", "02316b1b16344ee7922a4f9e7140aa24", "X-Trail T32, Qashqai 1.6 dCi R9M [RU]", "03fa97678ac0457fa6bdd8780903875f"
						}.Contains(SharedSettings.Current.ProfileUpdateAlias) && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (text == "Hyundai" || text == "Kia"))
						{
							return true;
						}
						if ((text == "Renault" || text == "Dacia") && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit || App.OBDReader.CurrentELMFormat == ELMFormat.KWP))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (text == "Lada" || text == "Лада" || text == "ВАЗ" || text == "VAZ"))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Toyota")
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Lexus")
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Volvo")
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && (text == "Honda" || text == "Acura"))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (text == "Haval" || text == "Hover" || text == "Great Wall") && (SharedSettings.Current.ProfileUpdateAlias == "17e35e3c0b974724b2a7d6533c755aac" || SharedSettings.Current.ProfileUpdateAlias == "6dcaa7bc7c5948ddb5aab6530fbd6575" || SharedSettings.Current.ProfileUpdateAlias == "6cc38fcd23fc4017a78f18ba2b45b352"))
						{
							return true;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && text == "Subaru")
						{
							return true;
						}
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return false;
		}

		// Token: 0x17001650 RID: 5712
		// (get) Token: 0x060048A1 RID: 18593 RVA: 0x00371630 File Offset: 0x0036F830
		// (set) Token: 0x060048A2 RID: 18594 RVA: 0x00371638 File Offset: 0x0036F838
		public string WarningSpecific
		{
			[CompilerGenerated]
			get
			{
				return this.<WarningSpecific>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WarningSpecific>k__BackingField = value;
			}
		} = "";

		// Token: 0x060048A3 RID: 18595 RVA: 0x00371644 File Offset: 0x0036F844
		public CodingListModel()
		{
			this.SetBrandSpecificValues();
		}

		// Token: 0x17001651 RID: 5713
		// (get) Token: 0x060048A4 RID: 18596 RVA: 0x003716B5 File Offset: 0x0036F8B5
		// (set) Token: 0x060048A5 RID: 18597 RVA: 0x003716BD File Offset: 0x0036F8BD
		public bool IsPlatformSelectorVisible
		{
			get
			{
				return this._IsPlatformSelectorVisible;
			}
			set
			{
				this._IsPlatformSelectorVisible = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("IsPlatformSelectorVisible"));
			}
		}

		// Token: 0x17001652 RID: 5714
		// (get) Token: 0x060048A6 RID: 18598 RVA: 0x003716E1 File Offset: 0x0036F8E1
		// (set) Token: 0x060048A7 RID: 18599 RVA: 0x003716E9 File Offset: 0x0036F8E9
		public ValueItemWithTranslation CurrentPlatform
		{
			get
			{
				return this._CurrentPlatform;
			}
			set
			{
				this._CurrentPlatform = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CurrentPlatform"));
			}
		}

		// Token: 0x060048A8 RID: 18600 RVA: 0x00371710 File Offset: 0x0036F910
		private void SetBrandSpecificValues()
		{
			string selectedBrand = SharedSettings.Current.SelectedBrand;
			if (selectedBrand == "Audi" || selectedBrand == "Skoda" || selectedBrand == "Seat" || selectedBrand == "Volkswagen" || selectedBrand == "Jetta")
			{
				this.WarningSpecific = Translate.GetString("coding_WarningVAG");
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged(this, new PropertyChangedEventArgs("WarningSpecific"));
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					this.IsPlatformSelectorVisible = true;
				}
				else if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
				{
					this.IsPlatformSelectorVisible = false;
				}
				List<ValueItemWithTranslation> list = PackageFileReader.DeserilzeFromEmbeddedFile<List<ValueItemWithTranslation>>("vagplatforms.db");
				this.Platforms.Clear();
				foreach (ValueItemWithTranslation valueItemWithTranslation in list)
				{
					this.Platforms.Add(valueItemWithTranslation);
				}
				ValueItemWithTranslation valueItemWithTranslation2 = new ValueItemWithTranslation
				{
					Title = Translate.GetString("coding_Group_Other"),
					Description = Translate.GetString("coding_vag_other_platform"),
					Value = "VAG_OTHER"
				};
				this.Platforms.Add(valueItemWithTranslation2);
				return;
			}
			this.WarningSpecific = "";
			PropertyChangedEventHandler propertyChanged2 = this.PropertyChanged;
			if (propertyChanged2 != null)
			{
				propertyChanged2(this, new PropertyChangedEventArgs("WarningSpecific"));
			}
			this.IsPlatformSelectorVisible = false;
			this.Platforms.Clear();
		}

		// Token: 0x17001653 RID: 5715
		// (get) Token: 0x060048A9 RID: 18601 RVA: 0x00371894 File Offset: 0x0036FA94
		// (set) Token: 0x060048AA RID: 18602 RVA: 0x0037189C File Offset: 0x0036FA9C
		public ObservableCollection<ValueItemWithTranslation> Platforms
		{
			[CompilerGenerated]
			get
			{
				return this.<Platforms>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Platforms>k__BackingField = value;
			}
		} = new ObservableCollection<ValueItemWithTranslation>();

		// Token: 0x17001654 RID: 5716
		// (get) Token: 0x060048AB RID: 18603 RVA: 0x003718A5 File Offset: 0x0036FAA5
		// (set) Token: 0x060048AC RID: 18604 RVA: 0x003718AD File Offset: 0x0036FAAD
		public ObservableCollection<ICodingContainer> FilteredCollection
		{
			[CompilerGenerated]
			get
			{
				return this.<FilteredCollection>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FilteredCollection>k__BackingField = value;
			}
		} = new ObservableCollection<ICodingContainer>();

		// Token: 0x17001655 RID: 5717
		// (get) Token: 0x060048AD RID: 18605 RVA: 0x003718B6 File Offset: 0x0036FAB6
		// (set) Token: 0x060048AE RID: 18606 RVA: 0x003718C0 File Offset: 0x0036FAC0
		public string Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				if (this._Filter != value)
				{
					this._Filter = value;
					if (string.IsNullOrEmpty(this._Filter))
					{
						this.SetFilteredCollection(this.CodingCollection);
						return;
					}
					List<ICodingContainer> list = new List<ICodingContainer>(this.CodingCollection);
					string[] array = this.Filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
					for (int i = 0; i < array.Length; i++)
					{
						string word = array[i];
						list = list.Where((ICodingContainer x) => (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<ICodingContainer>();
					}
					this.SetFilteredCollection(list);
				}
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x060048AF RID: 18607 RVA: 0x00371960 File Offset: 0x0036FB60
		// (remove) Token: 0x060048B0 RID: 18608 RVA: 0x00371998 File Offset: 0x0036FB98
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

		// Token: 0x060048B1 RID: 18609 RVA: 0x003719D0 File Offset: 0x0036FBD0
		private void SetFilteredCollection(IEnumerable<ICodingContainer> input)
		{
			this.FilteredCollection.Clear();
			foreach (ICodingContainer codingContainer in input)
			{
				this.FilteredCollection.Add(codingContainer);
			}
		}

		// Token: 0x17001656 RID: 5718
		// (get) Token: 0x060048B2 RID: 18610 RVA: 0x00371A28 File Offset: 0x0036FC28
		// (set) Token: 0x060048B3 RID: 18611 RVA: 0x00371A30 File Offset: 0x0036FC30
		public ObservableCollection<CodingGroupCollection> Groups
		{
			[CompilerGenerated]
			get
			{
				return this.<Groups>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Groups>k__BackingField = value;
			}
		} = new ObservableCollection<CodingGroupCollection>();

		// Token: 0x060048B4 RID: 18612 RVA: 0x00371A3C File Offset: 0x0036FC3C
		private void LoadVAG_MLB_A4B9()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("a4b9adaptations.db", true)
				select (x)).ToList<ICodingContainer>();
			ICodingContainer[] array = new ICodingContainer[]
			{
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Assistance.HighBeamAssistance(),
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Assistance.LaneAssist(),
				Service.ResetOilService(),
				Service.ResetInspectionService(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DrivingSchool(),
				new MQBMultimediaRestart(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_OilTemperatureDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_FreeSpaceInFuelTankDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_AccelerationDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ConfirmInstallationChanges(),
				new ThrottleAdaptationRoutine(),
				new VagUDSMassErrorReset(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ResetToFactorySettings_FactorySettings(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayCharismaDriveMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayStartStopInfo(),
				new FuelPumpTest(),
				new VIM_MIB2(),
				new VIM_MIB3v2(),
				new MIM_MIB2(),
				new MIM_MIB3v2(),
				new DPFServiceRegenerationWhileDriving(" #2")
			};
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(array);
			this.LoadedCollection.Add(new DPFServiceRegeneration());
			this.LoadedCollection.Add(new DPFServiceRegenerationCVMD());
			this.LoadedCollection.AddRange(VagBrakeBleeding.BuildBrakeBleed_MLBEVO_A4B9());
			this.LoadedCollection.AddRange(VagBrakeBleeding.BuildParkingBrake_MLBEVO_A4B9());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.UserSelectionScreen_MMI_MIB3());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.PrivacyWarningScreen_MMI_MIB3());
			List<ICodingContainer> list2 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("q7mq8.db", true)
				select (x)).ToList<ICodingContainer>();
			this.LoadedCollection.AddRange(list2);
			this.LoadedCollection.Add(new ParkingBrakeRoutine_MLBEVO());
			this.LoadedCollection.Add(new MH2P_PartitionFormat());
			this.LoadedCollection.AddRange(MQBUnitBackupCreator.BuildBackupCreators("mlbevo"));
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MLB_EVO.Brakes.BrakePressureSensorBasicAdaptation());
			this.LoadedCollection.Add(AT.ZH8HP_QuickAdaptation());
			this.LoadedCollection.Add(AT.ZH8HP_ResetSystemSpecificAdaptationValues());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.MIB2SoundDataset_audio_parameter_sound_0x3000());
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.AddRange(DatasetDumps.Create5FDatasetDumps());
				this.LoadedCollection.AddRange(new ICodingContainer[]
				{
					DatasetDumps.DataSet_65_0x7E0800(),
					DatasetDumps.DataSet_47(),
					new MQBMode22ParametrizeSilentDump("13", "7200"),
					new MQBMode22ParametrizeSilentDump("13", "7201"),
					new MQBMode22ParametrizeSilentDump("13", "7202"),
					new MQBMode22ParametrizeSilentDump("13", "7203"),
					new MQBMode22ParametrizeSilentDump("5F", "7201"),
					new MQBMode22ParametrizeSilentDump("5F", "7202"),
					new MQBMode22ParametrizeSilentDump("5F", "7203"),
					new MQBMode22ParametrizeSilentDump("5F", "7204"),
					new MQBMode22ParametrizeSilentDump("5F", "7205"),
					new MQBMode22ParametrizeSilentDump("5F", "7206"),
					new MQBMode22ParametrizeSilentDump("5F", "720B"),
					new MQBMode22ParametrizeSilentDump("5F", "720C"),
					new MQBMode22ParametrizeSilentDump("5F", "720D"),
					new MQBMode22ParametrizeSilentDump("5F", "7210"),
					new MQBMode22ParametrizeSilentDump("5F", "7211"),
					new MQBMode22ParametrizeSilentDump("5F", "7212"),
					new MQBMode22ParametrizeSilentDump("5F", "7213"),
					new MQBMode22ParametrizeSilentDump("5F", "7214"),
					new MQBMode22ParametrizeSilentDump("5F", "7215"),
					new MQBMode22ParametrizeSilentDump("5F", "7216")
				});
			}
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x00371E40 File Offset: 0x00370040
		private void LoadVAG_MQB()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("mqbadaptations.db", true)
				select (x)).ToList<ICodingContainer>();
			List<ICodingContainer> list2 = new List<ICodingContainer>
			{
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_A5_AdaptavieLaneAssist(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_A5_AdaptavieLaneAssistPatch(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_A5_HighBeamAssistVariants(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_13_OvertakingRightPrevention(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_17_RoadSignDetectionCamera_3Q0_980_654_Navi(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_17_RoadSignDetectionCamera_3Q0_980_654_NoNavi(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_LaneAssist_3Q0980654(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_13_ActiveCruiseControlAlgorythmSelection(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_LaneAssist_2Q0980654(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.HighBeamAssistantSaveState(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_LaneAssist_5Q0980653(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_17_RoadSignDetectionCamera_5Q0(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_13_ACC_OperationMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_A5_LaneAssistSaveState(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_09_HighBeamAssistStep2(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_09_HighBeamAssistantMenu(),
				CarScannerXamarinForms.Coding.DB.MQB.Brakes.MQB_03_ESCMenu(),
				CarScannerXamarinForms.Coding.DB.MQB.Brakes.MQB_03_HillHoldControlActivation(),
				CarScannerXamarinForms.Coding.DB.MQB.Brakes.MQB_03_XDS_Activation(),
				CarScannerXamarinForms.Coding.DB.MQB.Brakes.MQB_ForceEnableAutoHoldWithoutButtonControl(),
				CarScannerXamarinForms.Coding.DB.MQB.Brakes.BrakePressureSensorBasicAdaptation(),
				CarScannerXamarinForms.Coding.DB.MQB.TPMS.MQB_TPMS_Indirect(),
				CarScannerXamarinForms.Coding.DB.MQB.TPMS.MQB_TPMS_Indirect_PatchForParkAssist(),
				CarScannerXamarinForms.Coding.DB.MQB.TPMS.TPMSDisplayInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.TPMS.MQB_DisableDirectTPMSSystem(),
				new TPMSResetRoutine(),
				CarScannerXamarinForms.Coding.DB.MQB.Climate.MQB_08_BlowerDisplayInAuto(),
				CarScannerXamarinForms.Coding.DB.MQB.Climate.MQB_08_AutomaticWheelHeat(),
				CarScannerXamarinForms.Coding.DB.MQB.Climate.MQB_08_AutomaticWheelHeat2021PatchOnly(),
				CarScannerXamarinForms.Coding.DB.MQB.Climate.FlapAdaptations(),
				CarScannerXamarinForms.Coding.DB.MQB.Climate.WindshieldHeaterAutoOnThreshold(),
				WebastoProcedures.DisengageHeater(),
				WebastoProcedures.UnlockHeater(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.DisableTurnOnLightWarning_09(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_FreeSpaceInFuelTankDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_PointerTest(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_Time24HoursFormat(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_LapTimerDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_OilTemperatureDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_AccelerationDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_OutsideTemperatureDisplay(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_WarningWhen120kmh(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_WarnAboutRearFogLightsSpeedLimit(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.MQB_17_DisplaySoC(),
				EngineAndPowertrain.MQB_44_TorqueSteeringCompensation(),
				EngineAndPowertrain.IdleRPMAdaptationDiesel(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_RearLightsWhenDayLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_CornerActivation_NOT_LED(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_FogLightWithHighBeam_LED_Only(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_FogLightBlinkWithHighBeamBlink_LED_Only(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_FogLightOnAndBlinkWithHighBeam_LED_Only(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.BlinkRearTurnLightsWithRearLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOffDRLWhenParkingBrakeOn(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnLightsPoliteBlinks(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOffDRLWhenLightSwitchOff(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.DRLSteupInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MirrorLightsWithAreaView(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.SkodaKodiaqDRLLigthsWithStripes(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_Kodiaq_RearSideLightAudiStyle(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.BlinkFrontTurnLightsWithDRL(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.DimmDRLWhenFrontTurnLight_AudiStyle(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.CornerUpperSpeedThreshold(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.CornerRegulation(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.MQB_09_LightSwitchWithAutoMode(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.OctaviaA7_SecondsRearFogLight(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.KodiaqHalogenDisableFrontSideLightWhenHeadlightsEngaged(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.KodiaqBlinkStripesWithTurnLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TiguanIIBlinkRearSideLightsWithTurnLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.Tiguan3DLED_RearLightsWhenDayLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TiguanBASIC_LED_RearLightsWhenDayLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.EmergencyBrakingLightsWithTurnSignals(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.EmergencyBrakingLightsWithBrakeLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.EmergencyBrakingLightsWithBrakeLightsPhase2(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOnFogLightsWhenReversing(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.SkodaSuperB_MK3_2019(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.Tiguan3DLED_RearLightsAudiStyle(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.FogLights_ActivateAndBlinkWithHighBeam(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.FogLights_ActivateWithHighBeam(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.FogLights_BlinkWithHighBeam(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LimitMaxFrontLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnLights_USStyleHalogenOnly(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.ComingHomeLamps(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LicensePlate_LampsType(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.SeatLeon5F_BlinkRearSideLightWithTurnSignals(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.ComingHomeActivationEvents(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.ComingHomeActivation(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.ReduceDRLBrightness(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.DrivingLightsByDay(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.DynamicTurnLights_Tiguan2FL(),
				InteriorLights.SmoothButtonsLight(),
				InteriorLights.MQB_09_AmbientLight30Colors_Alt_DashMMI(),
				InteriorLights.MQB_09_AmbientLight30Colors_Alt_Doors(),
				InteriorLights.MQB_09_AmbientLight30ColorsVar2_DashMMI(),
				InteriorLights.MQB_09_AmbientLight30ColorsVar2_Doors(),
				InteriorLights.MQB_09_AmbientLight10Colors_DashboardAndMMI(),
				InteriorLights.MQB_09_AmbientLight10Colors_Doors(),
				InteriorLights.StartButtonHeartBit(),
				InteriorLights.DontTurnOnInteriorLightsWhenTailgateOpened(),
				InteriorLights.MQB_AmbientColorWithDriveModeSelectionVar1(),
				InteriorLights.MQB_AmbientColorWithDriveModeSelectionVar2(),
				InteriorLights.AmbientLightColorsList1(),
				InteriorLights.AmbientLightColorsList1LIN(),
				InteriorLights.AmbientLightColorsList2(),
				InteriorLights.AmbientLightColorsList2LIN(),
				InteriorLights.FootwellLightsInstalled(),
				Mirrors.CloseMirrorsHoldingKeylessDoor(),
				Mirrors.MQB_09_OpenSideMirrorsWithUnlocking(),
				Mirrors.MQB_09_RightMirrorGoDownWhenReversWithMemmory(),
				Mirrors.MQB_09_RightMirrorGoDownWhenReversWithoutMemmory(),
				Mirrors.MQB_MirrorLightsWhenFolded(),
				Mirrors.Mirrors_FoldingMirrorsWhenRepeatLocking(),
				Mirrors.Mirrors_FoldingOptionsInMMI(),
				Mirrors.Mirrors_FunkSpiegelanklappen(),
				Mirrors.Mirrors_ProfilfunctionForFoldingMirrors(),
				Mirrors.Mirrors_SyncMenu(),
				Mirrors.Mirrors_HeaterAlwasyActive(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_RearCameraInstalled(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_RearCameraInstalledMIB3(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_AMRadio(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_AUXInEnable(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DashboardDisplayPictures(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_OffroadModeDisplayInMMI_Var2(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_TripComputerAvailableWithIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DeveloperModeActivation(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_EcoDriving(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_OilLevel(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DrivingSchool(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MIB3_WirelessAppleCarPlay(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_NHTSA_NoSoftKeyboard(),
				CarScannerXamarinForms.Coding.DB.MQB.Dashboard.DigitalDashboardConfigurationInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_RadioArtDB(),
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.UserSelectionScreen_MMI_MIB3(),
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.PrivacyWarningScreen_MMI_MIB3(),
				new MQBMultimediaRestart(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ConfirmInstallationChanges(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_SecondPhoneSupport(),
				new MH2P_PartitionFormat(),
				CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MQB_5F_BetterSoundBolero(),
				CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MQB_47_AcousticSystemCoding(),
				new VIM_MIB2(),
				new VIM_MIB3v2(),
				new MIM_MIB2(),
				new MIM_MIB3v2(),
				CarScannerXamarinForms.Coding.DB.MQB.Other.CloseWindowsWhenRainy(),
				CarScannerXamarinForms.Coding.DB.MQB.Other.PersonalizationActivation(),
				CarScannerXamarinForms.Coding.DB.MQB.Other.PersonalizationSeatsPatch(),
				CarScannerXamarinForms.Coding.DB.MQB.Other.LightsSwitchWithFogLightsInstalled(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LightSensorInstalled(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LightSensorSensivity(),
				Seats.MQB_36_ComfortEntryDriverSide(),
				Seats.MQB_06_ComfortEntryPassengerSide(),
				Seats.BasicSettingsDriverSeat(),
				Seats.BasicSettingsPassengerSeat(),
				MassageProcedure.MassageRoutineDriverSeat(),
				MassageProcedure.MassageRoutinePassengerSeat(),
				CarScannerXamarinForms.Coding.DB.MQB.SoundsAndAlarm.MQB_6D_TailgateSound(),
				Washer.ReduceWasherDelay(),
				Washer.DropsTear_FrontWiper(),
				Washer.DropsTear_RearWiper(),
				Washer.MQB_09_ComfortRearWiper(),
				Washer.MQB_09_AutoRearWiper(),
				Washer.WipersServicePositionMenu(),
				Washer.ParkWipersAfterIgnitionTurnedOff(),
				Washer.MQB_HeadlightWasherInterval(),
				Washer.MQB_DelayBeforeHeadlightWasher(),
				Washer.MQB_HeadlightWasherDutyDuration(),
				Washer.MQB_StopWiperWhenHoodOpened(),
				Washer.MQB_ParkWiperWhenHoodOpened(),
				Washer.MIB3_DisplayWiperMenu(),
				Washer.MQB_09_RearCameraWashHMI(),
				Washer.LowWasherLevelSensorInstalled(),
				Washer.MQB_HeadlightWasher_DelayBetweenFirstAndSecondWash(),
				Washer.MQB_HeadlightWasher_SecondWashDuration(),
				Washer.MQB_HeadlightWasher_UseWasherWithLowLevel(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.EasyOpenPart3_Kodiaq(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.MQB_05_DisableKessyFrontDoors(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.MQB_05_DisableKessyAllDoors(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.MQB_05_AutomaticallyLockCarDoors(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.EnableKeyFobWhileEngineRunning(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_AutolockAtSpeed(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_AutolockRear(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_AutoUnlock(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_AutoUnlockWhenSelectorInParking_NAR(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_LockMenu(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_UnlockDoorsVariants(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.Lock_Unlockmenu(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.RaiseAndLowerWindowsWithKeyFob(),
				new ParkingBrakeRoutineMQB(),
				new DSGAdaptationRoutine(),
				new DSGDQ381AdaptationRoutine(),
				new DPFServiceRegeneration(),
				new MQBAisinProcedures(),
				new FuelPumpTest(),
				new Haldex5FuelPumpTest(),
				Service.ResetOilService(),
				Service.ResetInspectionService(),
				Roof.AutomaticallyOpenAndCloseSunroofHoldingKeyFobKeys(),
				FeedbackSignals.AcousticFeedbackDurationOfAcousticFeedbackFromSimpleHorn(),
				FeedbackSignals.AcousticFeedbackForTheSecondCloseCommand(),
				FeedbackSignals.AcousticFeedbackGlobal(),
				FeedbackSignals.AcousticFeedbackLockAcousticFeedback(),
				FeedbackSignals.AcousticFeedbackMenu(),
				FeedbackSignals.AcousticFeedbackSignalHorn(),
				FeedbackSignals.AcousticFeedbackWhenUnlocking(),
				FeedbackSignals.MQB_09_SoundConfirmationOfLockAndUnlock(),
				FeedbackSignals.OpticalFeedback3rdBrakeLight(),
				FeedbackSignals.OpticalFeedbackComfortClosing(),
				FeedbackSignals.OpticalFeedbackWhenLocking(),
				AntiTheft.Alarm_ActivateAntiTheftAlarm(),
				AntiTheft.Alarm_AlarmSignal(),
				AntiTheft.Alarm_CamperMode(),
				AntiTheft.Alarm_DeactivateAntiTheftByUnlockCylinder(),
				AntiTheft.Alarm_DeactivationSensorsHMI(),
				AntiTheft.Alarm_DelayBeforeActivation(),
				AntiTheft.Alarm_DisableCabinAlarmSensor(),
				AntiTheft.Alarm_MonitoringInsideLockingLeaver(),
				AntiTheft.Alarm_PanicAlarmDelay(),
				AntiTheft.Alarm_ParkingHeaterInstalled(),
				AntiTheft.Alarm_RearWindowBreakSensor(),
				AntiTheft.Alarm_RearWindowBreakSensorForInteriorMonitoring(),
				AntiTheft.Alarm_SirenAlarms(),
				AntiTheft.Alarm_SoundOut(),
				AntiTheft.Alarm_TiltSensor(),
				AntiTheft.Alarm_UseSignalhorn(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_13_SimpleCruiseControlActivation(),
				Roof.EnableRoofInteriorLights(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.DRL_Mode(),
				new DieselExhaustFlapAdaptation_Koidaq(),
				new ThrottleAdaptationRoutine(),
				new VagUDSMassErrorReset(),
				new FPACoding(),
				new FPAUserFriendlyCoding(),
				new A5Parametrize(),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.EmergencyAssist(),
				new RKDSDatasetCoding(),
				new DPFServiceRegenerationWhileDriving(" #2"),
				CarScannerXamarinForms.Coding.DB.MQB.Assistance.MQB_A5_2Q0LaneAssistTJAActivation(),
				new A5ParametrizeV2(),
				new EngineAutomaticTestsProcedure(),
				new RDKSSensorPositionLearning(1),
				new RDKSSensorPositionLearning(2),
				new RDKSSensorPositionLearning(3),
				new RDKSSensorPositionLearning(4),
				new RDKSSensorIDLearning(0),
				new RDKSSensorIDLearning(1),
				new RDKSSensorIDLearning(2),
				new RDKSSensorIDLearning(3),
				new EA288_LowTemperatureCircuitBleeding(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ResetToFactorySettings_FactorySettings(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayCharismaDriveMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayStartStopInfo(),
				CarScannerXamarinForms.Coding.DB.MQB.Doors.EasyOpen_Unit09_Part(),
				new ControlSupplyVoltageTest()
			};
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(list2);
			this.LoadedCollection.AddRange(MQB_LightConfigurationCoding.GetLightConfigurationMQB());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_sound_0x3000());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x0700());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x7100());
			this.LoadedCollection.Add(new MQBAudioDataSetMIB3());
			this.LoadedCollection.Add(new TailGateParametrizeCustomizationCoding());
			this.LoadedCollection.Add(new MQBSwapActivation());
			this.LoadedCollection.AddRange((from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("mqb_camera_fusion.db", true)
				select (x)).ToList<ICodingContainer>());
			if (!SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.RemoveAll((ICodingContainer x) => x.Group == CodingGroup.DashboardDimming);
			}
			this.LoadedCollection.AddRange(MQBUnitBackupCreator.BuildBackupCreators("mqb"));
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.Add(new MQBCantonDataSetMIB2());
				this.LoadedCollection.AddRange(DatasetDumps.Create5FDatasetDumps());
				this.LoadedCollection.AddRange(MIB2PowerManagement.GetMIB2PowerManagementCollection());
				this.LoadedCollection.AddRange(new ICodingContainer[]
				{
					DatasetDumps.DataSet_65_0x7E0800(),
					DatasetDumps.DataSet_47(),
					new MQBMode22ParametrizeSilentDump("13", "7200"),
					new MQBMode22ParametrizeSilentDump("13", "7201"),
					new MQBMode22ParametrizeSilentDump("13", "7202"),
					new MQBMode22ParametrizeSilentDump("13", "7203"),
					new MQBMode22ParametrizeSilentDump("5F", "7201"),
					new MQBMode22ParametrizeSilentDump("5F", "7202"),
					new MQBMode22ParametrizeSilentDump("5F", "7203"),
					new MQBMode22ParametrizeSilentDump("5F", "7204"),
					new MQBMode22ParametrizeSilentDump("5F", "7205"),
					new MQBMode22ParametrizeSilentDump("5F", "7206"),
					new MQBMode22ParametrizeSilentDump("5F", "720B"),
					new MQBMode22ParametrizeSilentDump("5F", "720C"),
					new MQBMode22ParametrizeSilentDump("5F", "720D"),
					new MQBMode22ParametrizeSilentDump("5F", "7210"),
					new MQBMode22ParametrizeSilentDump("5F", "7211"),
					new MQBMode22ParametrizeSilentDump("5F", "7212"),
					new MQBMode22ParametrizeSilentDump("5F", "7213"),
					new MQBMode22ParametrizeSilentDump("5F", "7214"),
					new MQBMode22ParametrizeSilentDump("5F", "7215"),
					new MQBMode22ParametrizeSilentDump("5F", "7216"),
					CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB1SoundDataset_audio_parameter_sound_0x1000()
				});
				this.LoadedCollection.AddRange(SFDBuilder.BuildSFD());
			}
			if (SharedSettings.Current.SelectedBrand == "Audi")
			{
				this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.Dashboard.AudiVC1GenSportLayout());
			}
			this.LoadedCollection.AddRange(VagBrakeBleeding.BuildBrakeBleed_MQB());
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x00372CD4 File Offset: 0x00370ED4
		public async Task LoadEasyCodingCollection(IProgress<string> progress)
		{
			string text = SharedSettings.Current.BrandForDTC;
			if (string.IsNullOrEmpty(text))
			{
				text = SharedSettings.Current.SelectedBrand;
			}
			this.CodingCollection.Clear();
			this.LoadedCollection.Clear();
			if (text != null)
			{
				int i = text.Length;
				switch (i)
				{
				case 3:
				{
					char c = text[0];
					if (c != 'K')
					{
						if (c != 'V')
						{
							if (c != 'В')
							{
								goto IL_1607;
							}
							if (!(text == "ВАЗ"))
							{
								goto IL_1607;
							}
							goto IL_0FBA;
						}
						else
						{
							if (!(text == "VAZ"))
							{
								goto IL_1607;
							}
							goto IL_0FBA;
						}
					}
					else
					{
						if (!(text == "Kia"))
						{
							goto IL_1607;
						}
						goto IL_06C9;
					}
					break;
				}
				case 4:
				{
					char c = text[0];
					if (c <= 'L')
					{
						if (c != 'A')
						{
							if (c != 'L')
							{
								goto IL_1607;
							}
							if (!(text == "Lada"))
							{
								goto IL_1607;
							}
							goto IL_0FBA;
						}
						else if (!(text == "Audi"))
						{
							goto IL_1607;
						}
					}
					else if (c != 'O')
					{
						if (c != 'S')
						{
							if (c != 'Л')
							{
								goto IL_1607;
							}
							if (!(text == "Лада"))
							{
								goto IL_1607;
							}
							goto IL_0FBA;
						}
						else if (!(text == "Seat"))
						{
							goto IL_1607;
						}
					}
					else
					{
						if (!(text == "Opel"))
						{
							goto IL_1607;
						}
						goto IL_0627;
					}
					break;
				}
				case 5:
				{
					char c = text[2];
					if (c != 'c')
					{
						switch (c)
						{
						case 'l':
							if (!(text == "Volvo"))
							{
								goto IL_1607;
							}
							goto IL_141E;
						case 'm':
						case 'p':
						case 'q':
						case 'r':
						case 's':
						case 'w':
							goto IL_1607;
						case 'n':
							if (!(text == "Honda"))
							{
								goto IL_1607;
							}
							break;
						case 'o':
							if (!(text == "Skoda"))
							{
								goto IL_1607;
							}
							goto IL_0457;
						case 't':
							if (!(text == "Jetta"))
							{
								goto IL_1607;
							}
							goto IL_0457;
						case 'u':
							if (!(text == "Acura"))
							{
								goto IL_1607;
							}
							break;
						case 'v':
							if (!(text == "Haval") && !(text == "Hover"))
							{
								goto IL_1607;
							}
							goto IL_1486;
						case 'x':
							if (!(text == "Lexus"))
							{
								goto IL_1607;
							}
							goto IL_1355;
						default:
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
						{
							List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("hondareset.db", true)
								select (x)).ToList<ICodingContainer>();
							this.LoadedCollection.AddRange(list);
							goto IL_1607;
						}
						goto IL_1607;
					}
					else
					{
						if (!(text == "Dacia"))
						{
							goto IL_1607;
						}
						goto IL_0CAA;
					}
					break;
				}
				case 6:
				{
					char c = text[0];
					if (c <= 'H')
					{
						if (c != 'D')
						{
							if (c != 'H')
							{
								goto IL_1607;
							}
							if (!(text == "Holden"))
							{
								goto IL_1607;
							}
							goto IL_0627;
						}
						else
						{
							if (!(text == "Daewoo"))
							{
								goto IL_1607;
							}
							goto IL_0627;
						}
					}
					else if (c != 'N')
					{
						if (c != 'S')
						{
							if (c != 'T')
							{
								goto IL_1607;
							}
							if (!(text == "Toyota"))
							{
								goto IL_1607;
							}
							goto IL_1355;
						}
						else
						{
							if (!(text == "Subaru"))
							{
								goto IL_1607;
							}
							this.LoadedCollection.Add(new SubaruResetSaS());
							goto IL_1607;
						}
					}
					else
					{
						if (!(text == "Nissan"))
						{
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.SelectedProfileV2Name.Contains("Leaf"))
						{
							this.LoadedCollection.Add(new NissanLeafBattery());
							List<ICodingContainer> list2 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("leafze0.db", true)
								select (x)).ToList<ICodingContainer>();
							this.LoadedCollection.AddRange(list2);
							goto IL_1607;
						}
						if (new string[] { "6e121b9dbd814f6bbf48df31c44bbcf5", "Nissan X-Trail T31 2.0", "fe6c577e35d3481cb0224323bbf90860", "698734883a5d461ca3e016bc3ba1fe3b", "Nissan X-Trail T31 2.5", "293952d48d9240c588356e57a0d9b94a", "Nissan X-Trail T31 CVT" }.Contains(SharedSettings.Current.ProfileUpdateAlias) && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.KWP))
						{
							this.LoadedCollection.Add(new NissanCVTReset());
							goto IL_1607;
						}
						if (new string[]
						{
							"Qashqai J11", "d5c487cd83354b52a8245759aa2ffce4", "Qashqai J11 1.5dci K9K 636 [EN]", "896225c9ee8f48abbdb9b48bf135f2da", "Qashqai J11 1.5dci K9K 636 [RU]", "c3f57cd35c1e4fd9aa3cc7b57cf254a9", "Rogue T32 2014-2020", "513ecb4ac8eb4df5addf1acf6887f8f8", "X-Trail T32", "d4d7c86928de468c9259bcff2c2d95fe",
							"X-Trail T32 (NT32) 2.0 MR20DD [EN]", "X-Trail T32 (NT32) 2.0 MR20DD [RU]", "aa016831f2ce473483f6c02463038a47", "X-Trail T32, Qashqai 1.6 dCi R9M [EN]", "02316b1b16344ee7922a4f9e7140aa24", "X-Trail T32, Qashqai 1.6 dCi R9M [RU]", "03fa97678ac0457fa6bdd8780903875f"
						}.Contains(SharedSettings.Current.ProfileUpdateAlias) && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
						{
							this.LoadedCollection.Clear();
							this.LoadedCollection.AddRange(PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("t32.db", true));
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.ShowExperimental)
						{
							this.LoadedCollection.Add(new NissanCVTReset());
							goto IL_1607;
						}
						goto IL_1607;
					}
					break;
				}
				case 7:
				{
					char c = text[0];
					if (c != 'H')
					{
						if (c != 'R')
						{
							goto IL_1607;
						}
						if (!(text == "Renault"))
						{
							goto IL_1607;
						}
						goto IL_0CAA;
					}
					else
					{
						if (!(text == "Hyundai"))
						{
							goto IL_1607;
						}
						goto IL_06C9;
					}
					break;
				}
				case 8:
					if (!(text == "Vauxhall"))
					{
						goto IL_1607;
					}
					goto IL_0627;
				case 9:
					if (!(text == "Chevrolet"))
					{
						goto IL_1607;
					}
					goto IL_0627;
				case 10:
				{
					char c = text[0];
					if (c != 'G')
					{
						if (c != 'M')
						{
							if (c != 'V')
							{
								goto IL_1607;
							}
							if (!(text == "Volkswagen"))
							{
								goto IL_1607;
							}
						}
						else
						{
							if (!(text == "Mitsubishi"))
							{
								goto IL_1607;
							}
							if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
							{
								this.LoadedCollection.Add(new MitsuCVTOilReset());
								goto IL_1607;
							}
							goto IL_1607;
						}
					}
					else
					{
						if (!(text == "Great Wall"))
						{
							goto IL_1607;
						}
						goto IL_1486;
					}
					break;
				}
				default:
					goto IL_1607;
				}
				IL_0457:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && SharedSettings.Current.ShowExperimental)
				{
					this.LoadVAG_MEB();
					goto IL_1607;
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					string value = this.CurrentPlatform.Value;
					if (value != null)
					{
						i = value.Length;
						switch (i)
						{
						case 3:
							if (value == "MQB")
							{
								this.LoadVAG_MQB();
								goto IL_1607;
							}
							break;
						case 4:
						{
							char c = value[0];
							if (c != 'P')
							{
								if (c == 'Y')
								{
									if (value == "YETI")
									{
										this.LoadVAG_Yeti();
										goto IL_1607;
									}
								}
							}
							else if (value == "PQ26")
							{
								this.LoadVAG_PQ26();
								goto IL_1607;
							}
							break;
						}
						case 5:
							if (value == "TNFFL")
							{
								this.LoadVAG_TouaregNFFL();
								goto IL_1607;
							}
							break;
						case 6:
						case 8:
						case 10:
						case 11:
							break;
						case 7:
							if (value == "FABIANJ")
							{
								this.LoadVAG_PQ26_FabiaNJ();
								goto IL_1607;
							}
							break;
						case 9:
							if (value == "TIGUAN1FL")
							{
								this.LoadVAG_Tiguan1FL();
								goto IL_1607;
							}
							break;
						case 12:
							if (value == "MLB-EVO-A4B9")
							{
								this.LoadVAG_MLB_A4B9();
								goto IL_1607;
							}
							break;
						default:
							if (i == 16)
							{
								if (value == "PQ26NEWRAPID2020")
								{
									this.LoadVAG_PQ26_NewRapid2020();
									goto IL_1607;
								}
							}
							break;
						}
					}
					this.LoadVAG_Other();
					goto IL_1607;
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					this.LoadedCollection.Clear();
					this.LoadedCollection.AddRange(VWLongCoding.BuildLongCodingList());
					goto IL_1607;
				}
				goto IL_1607;
				IL_0627:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					this.LoadedCollection.Add(new GMResetServiceCANVar1());
					goto IL_1607;
				}
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && (SharedSettings.Current.ProfileUpdateAlias == "c83763c04180489992fd1fedd17eaec5" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (EN)" || SharedSettings.Current.ProfileUpdateAlias == "6d7cef6bb26b48bb8f31cdfeba9e289f" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (RU)"))
				{
					this.LoadedCollection.Add(new SiriusD42ResetThrottle());
					goto IL_1607;
				}
				goto IL_1607;
				IL_06C9:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					this.LoadedCollection.Add(new HyundaiReset6AT());
					this.LoadedCollection.Add(new HyundaiReset4AT());
					this.LoadedCollection.Add(new ResetEngineAdaptation());
					this.LoadedCollection.Add(new HyundaKiaResetServiceReminder());
					this.LoadedCollection.Add(BrakeBleed.CeratoYD());
					this.LoadedCollection.Add(BrakeBleed.PicantoJA());
					this.LoadedCollection.Add(BrakeBleed.PicantoTA());
					this.LoadedCollection.Add(BrakeBleed.RioFB());
					this.LoadedCollection.Add(BrakeBleed.RioQBR());
					this.LoadedCollection.Add(new CRDIFuelPump_SorentoUMFL());
					this.LoadedCollection.Add(new CRDIFuelPump_SportageSL());
					this.LoadedCollection.Add(new SportageQLThrottleAdaptationReset());
					this.LoadedCollection.Add(new SportageQL_ESP_Wheel_Calibration());
					this.LoadedCollection.Add(new SportageQL_MDPS_Calibration());
					this.LoadedCollection.Add(new ElectricParkingBrakeService_SportageQL());
					this.LoadedCollection.Add(new ElectricParkingBrakeService_OptimaJF());
					this.LoadedCollection.Add(new ElectricParkingBrakeService_OptimaTF());
					this.LoadedCollection.Add(new ElectricParkingBrakeService_CeedJD());
					List<CustomizableCodingTemplate> list3 = await EngineActuatorTestsDetector.GetSupportedActuators();
					List<CustomizableCodingTemplate> engineActuators = list3;
					List<CustomizableCodingTemplate> list4 = await SmartJunktionUnitTestDetector.GetSupportedActuators();
					List<ICodingContainer> list5 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<HyundaiKiaUDSCoding>>("kiaservice.db", true)
						select (x)).ToList<ICodingContainer>();
					list5.AddRange(engineActuators);
					list5.AddRange(list4);
					this.LoadedCollection.AddRange(list5);
					this.LoadedCollection.AddRange((from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiadashboardvariant.db", true)
						select (x)).ToList<ICodingContainer>());
					this.LoadedCollection.AddRange((from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiatpms.db", true)
						select (x)).ToList<ICodingContainer>());
					List<CustomizableCodingTemplate> list6 = await KiaBCMSupportedDetector.GetSupportedIds(PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiabcm.db", true).ToList<CustomizableCodingTemplate>());
					if (list6 != null)
					{
						this.LoadedCollection.AddRange(list6);
					}
					List<CustomizableCodingTemplate> list7 = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("hyundaiparkingbrake.db", true).ToList<CustomizableCodingTemplate>();
					this.LoadedCollection.AddRange(list7);
					this.LoadedCollection.Add(new CretaTPMSCoding());
					this.LoadedCollection.Add(new HyundaiDPFRegen());
					if (SharedSettings.Current.ShowExperimental)
					{
						this.LoadedCollection.Add(DCTAdaptation.CreateDTCAdaptation());
					}
					engineActuators = null;
					goto IL_1607;
				}
				goto IL_1607;
				IL_0CAA:
				if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit && App.OBDReader.CurrentELMFormat != ELMFormat.CAN29bit && App.OBDReader.CurrentELMFormat != ELMFormat.KWP)
				{
					goto IL_1607;
				}
				foreach (string text2 in await new RenaultUnitsDetector().GetFileNames(progress))
				{
					if (text2 == "RenaultFapVer1")
					{
						this.LoadedCollection.Add(new RenaultFapVer1());
					}
					else if (text2 == "RenaultFapVer2")
					{
						this.LoadedCollection.Add(new RenaultFapVer2());
					}
					else if (text2 == "RenaultFapVer3")
					{
						this.LoadedCollection.Add(new RenaultFapVer3());
					}
					else
					{
						try
						{
							List<ICodingContainer> list8 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault." + text2, true)
								select (x)).ToList<ICodingContainer>();
							this.LoadedCollection.AddRange(list8);
						}
						catch (Exception)
						{
						}
						if (text2 == "ABSESC_X_ALL_10C0.db")
						{
							this.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("10C0"));
						}
						if (text2 == "ABSESC_X_ALL_1003.db")
						{
							this.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("1003"));
						}
						if (text2 == "CMFB_CEPS_JTEKT_V3.2_20181030T122041.db")
						{
							this.LoadedCollection.Add(new EPSBackup());
							this.LoadedCollection.Add(new ESPConfigAutoRestore());
						}
					}
				}
				if (this.LoadedCollection.Count <= 0)
				{
					goto IL_1607;
				}
				this.LoadedCollection.Add(new DaciaServiceReset());
				try
				{
					if (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper() == "RU")
					{
						CodingListModel.<>c__DisplayClass39_0 CS$<>8__locals1 = new CodingListModel.<>c__DisplayClass39_0();
						CS$<>8__locals1.hasAmtResponse = false;
						OBDRequest obdrequest = new OBDRequest("222E0C", "7E1", "", "", false);
						obdrequest.ResponseDecoded += delegate(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length == 2)
							{
								CS$<>8__locals1.hasAmtResponse = true;
							}
						};
						App.OBDReader.ReplaceQueue(obdrequest);
						await App.OBDReader.WaitForCommandQueue();
						if (CS$<>8__locals1.hasAmtResponse)
						{
							this.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
						}
						CS$<>8__locals1 = null;
					}
					goto IL_1607;
				}
				catch (Exception)
				{
					goto IL_1607;
				}
				IL_0FBA:
				if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
				{
					goto IL_1607;
				}
				foreach (string text3 in await new RenaultUnitsDetector().GetFileNames(progress))
				{
					try
					{
						List<ICodingContainer> list9 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault." + text3, true)
							select (x)).ToList<ICodingContainer>();
						this.LoadedCollection.AddRange(list9);
					}
					catch (Exception)
					{
					}
					if (text3 == "ABSESC_X_ALL_10C0.db")
					{
						this.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("10C0"));
					}
					if (text3 == "ABSESC_X_ALL_1003.db")
					{
						this.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("1003"));
					}
				}
				if (this.LoadedCollection.Count == 0)
				{
					CodingListModel.<>c__DisplayClass39_1 CS$<>8__locals2 = new CodingListModel.<>c__DisplayClass39_1();
					CS$<>8__locals2.hasAmtResponse = false;
					OBDRequest obdrequest2 = new OBDRequest("222E0C", "7E1", "", "", false);
					obdrequest2.ResponseDecoded += delegate(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length == 2)
						{
							CS$<>8__locals2.hasAmtResponse = true;
						}
					};
					App.OBDReader.ReplaceQueue(obdrequest2);
					await App.OBDReader.WaitForCommandQueue();
					if (CS$<>8__locals2.hasAmtResponse)
					{
						this.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
					}
					CS$<>8__locals2 = null;
				}
				else
				{
					this.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
				}
				if (this.LoadedCollection.Count > 0)
				{
					List<ICodingContainer> list10 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("vestam86.db", true)
						select (x)).ToList<ICodingContainer>();
					this.LoadedCollection.AddRange(list10);
				}
				if (SharedSettings.Current.ProfileUpdateAlias == "bdae95afb9d44d1b825011e7144af5ba" || SharedSettings.Current.ProfileUpdateAlias == "66ed536069554560ae1b6a27c27b31c4")
				{
					this.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTSetDateOfServiceProcedure());
					this.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTResetProcedure());
					this.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTResetECU());
				}
				if (SharedSettings.Current.ProfileUpdateAlias == "780429f87d044ba2ab5d00064a72367d" || SharedSettings.Current.ProfileUpdateAlias == "587c1d092acc4576a737d2f50287baa0")
				{
					this.LoadedCollection.Clear();
					List<ICodingContainer> list11 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("vestang.db", true)
						select (x)).ToList<ICodingContainer>();
					this.LoadedCollection.AddRange(list11);
					List<ICodingContainer> list12 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault.PP_vesta_ng.db", true)
						select (x)).ToList<ICodingContainer>();
					this.LoadedCollection.AddRange(list12);
					goto IL_1607;
				}
				goto IL_1607;
				IL_1355:
				if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
				{
					goto IL_1607;
				}
				try
				{
					List<CustomizableCodingTemplate> list13 = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("tlcprado150.db", true);
					list13.Add(new ToyotaTPMS2ACoding());
					list13.Add(new ToyotaTPMS2ACoding5Wheels());
					List<CustomizableCodingTemplate> list14 = await new ToyotaUnitsDetector().CheckAvailableCodings(list13, progress);
					this.LoadedCollection.AddRange(list14);
					goto IL_1607;
				}
				catch (Exception)
				{
					this.LoadedCollection.Clear();
					goto IL_1607;
				}
				IL_141E:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
				{
					List<ICodingContainer> list15 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("volvo.db", true)
						select (x)).ToList<ICodingContainer>();
					this.LoadedCollection.AddRange(list15);
					this.LoadedCollection.Add(new VolvoP3SetTimeDashboard());
					goto IL_1607;
				}
				goto IL_1607;
				IL_1486:
				if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (SharedSettings.Current.ProfileUpdateAlias == "17e35e3c0b974724b2a7d6533c755aac" || SharedSettings.Current.ProfileUpdateAlias == "6dcaa7bc7c5948ddb5aab6530fbd6575" || SharedSettings.Current.ProfileUpdateAlias == "6cc38fcd23fc4017a78f18ba2b45b352"))
				{
					List<ICodingContainer> list16 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("havalh5.db", true)
						select (x)).ToList<ICodingContainer>();
					this.LoadedCollection.AddRange(list16);
					if (SharedSettings.Current.ProfileUpdateAlias == "6cc38fcd23fc4017a78f18ba2b45b352")
					{
						this.LoadedCollection.Add(new DelphiInjectorCoding(1, "22FDE1", "2EFDE1"));
						this.LoadedCollection.Add(new DelphiInjectorCoding(2, "22FDE2", "2EFDE2"));
						this.LoadedCollection.Add(new DelphiInjectorCoding(3, "22FDE3", "2EFDE3"));
						this.LoadedCollection.Add(new DelphiInjectorCoding(4, "22FDE4", "2EFDE4"));
					}
				}
			}
			IL_1607:
			if (SharedSettings.Current.ShowExperimental)
			{
				CustomCodingsListViewModel customCodingsListViewModel = new CustomCodingsListViewModel();
				customCodingsListViewModel.Load();
				this.LoadedCollection.AddRange(customCodingsListViewModel);
			}
			this.CreateGroupCollections(this.LoadedCollection);
		}

		// Token: 0x060048B7 RID: 18615 RVA: 0x00372D20 File Offset: 0x00370F20
		private void LoadVAG_MEB()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("vagmeb.db", true)
				select (x)).ToList<ICodingContainer>();
			foreach (ICodingContainer codingContainer in list)
			{
				(codingContainer as MQBAdaptationTemplate).Protocol = "6";
			}
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.AddRange(SFDBuilder.BuildSFD());
			}
		}

		// Token: 0x060048B8 RID: 18616 RVA: 0x00372DE0 File Offset: 0x00370FE0
		private void LoadVAG_Yeti()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("yeti.db", true)
				select (x)).ToList<ICodingContainer>();
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.Add(Service.ResetOilService());
			this.LoadedCollection.Add(Service.ResetInspectionService());
			this.LoadedCollection.Add(new EngineAutomaticTestsProcedure());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
		}

		// Token: 0x060048B9 RID: 18617 RVA: 0x00372E7C File Offset: 0x0037107C
		private void LoadVAG_Tiguan1FL()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("tiguan1fl.db", true)
				select (x)).ToList<ICodingContainer>();
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.Add(Service.ResetOilService());
			this.LoadedCollection.Add(Service.ResetInspectionService());
			this.LoadedCollection.Add(new Haldex5FuelPumpTest());
			this.LoadedCollection.Add(new EngineAutomaticTestsProcedure());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
		}

		// Token: 0x060048BA RID: 18618 RVA: 0x00372F28 File Offset: 0x00371128
		private void LoadVAG_TouaregNFFL()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("tnffl.db", true)
				select (x)).ToList<ICodingContainer>();
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.Add(Service.ResetOilService());
			this.LoadedCollection.Add(Service.ResetInspectionService());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.Add(TouaregNF.TouaregNF_BrakeDiskDrying());
		}

		// Token: 0x060048BB RID: 18619 RVA: 0x00372FC4 File Offset: 0x003711C4
		private void LoadVAG_Other()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("pq35adaptations.db", true)
				select (x)).ToList<ICodingContainer>();
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.Add(Service.ResetOilService());
			this.LoadedCollection.Add(Service.ResetInspectionService());
			this.LoadedCollection.Add(new FuelPumpTest());
			this.LoadedCollection.Add(new ParkingBrakeRoutine_AudiQ3_5N0614109());
			this.LoadedCollection.Add(new MLB_DPF_ServiceReset());
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.AddRange(VWLongCoding.BuildLongCodingList());
			}
		}

		// Token: 0x060048BC RID: 18620 RVA: 0x0037309C File Offset: 0x0037129C
		private void LoadVAG_PQ26()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("pq26adaptations.db", true)
				select (x)).ToList<ICodingContainer>();
			ICodingContainer[] array = new ICodingContainer[]
			{
				new FuelPumpTest(),
				CarScannerXamarinForms.Coding.DB.PQ26.TPMS.TPMS_ActivationStep1(),
				CarScannerXamarinForms.Coding.DB.PQ26.TPMS.TPMS_ActivationStep2(),
				new TPMSResetRoutine(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionClimate(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionAC(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_PointerTest(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_Time24HoursFormat(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_LapTimerDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_RemoveKeyWarning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_ShowECOHints(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_FreeSpaceInFuelTankDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_WarnAboutRearFogLightsSpeedLimit(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_OilTemperatureDisplay(),
				Engine.MQB_01_AcceleratorPedalSensivity(),
				ExteriorLight.PQ26_DisableLicensePlateLightWhenTailgateIsOpened(),
				ExteriorLight.PQ26_Corner(),
				ExteriorLight.CornerRegulation(),
				ExteriorLight.PQ26_RearSideLightsAsDRL(),
				ExteriorLight.PQ26_StroboscopeEffectFogLightHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLowBeamHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLEDDRLHighBeam(),
				ExteriorLight.CornerUpperSpeedThreshold(),
				ExteriorLight.PQ26_LEDDRL_WithLowBeam(),
				ExteriorLight.PQ26_DimmingDRLWhenTurnLightsOn(),
				ExteriorLight.PQ26_FrontSideLightDimmingWithTurnLights(),
				ExteriorLight.PQ26_USStandlichts(),
				ExteriorLight.PQ26_USStandlichtsOnlyWhenLightInSideLightsPosition(),
				ExteriorLight.PQ26_TurnOnStopSignalWhenDoorOpened(),
				ExteriorLight.PQ26_TurnOnSideTurnlightsWhenTailgateOpened(),
				ExteriorLight.TurnOffDRLWhenParkingBrakeOn(),
				ExteriorLight.TurnOffDRLWhenLightSwitchOff(),
				ExteriorLight.DRLSteupInMMI(),
				ExteriorLight.LimitMaxFrontLights(),
				ExteriorLight.ComingHomeWithLightSensor(),
				ExteriorLight.ComingHomeWithoutLightSensor(),
				ExteriorLight.PQ26_09_LightSensorSensivity(),
				ExteriorLight.TurnLightsPoliteBlinks(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOnFogLightsWhenReversing(),
				ExteriorLight.PQ26_ComingHomeLeavingHomeReverseLight(),
				ExteriorLight.ComingHomeLamps(),
				ExteriorLight.EmergencyBrakingLightsWithTurnSignals(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LicensePlate_LampsType(),
				InteriorLights.SmoothButtonsLight(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_AMRadio(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_AUXInEnable(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_OffroadModeDisplayInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_OffroadModeDisplayInMMI_Var2(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.MQB_5F_BetterSoundBolero(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_DrivingSchool(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ConfirmInstallationChanges(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_TripComputerAvailableWithIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_DisplayComfortUsage(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.MQB_5F_RearSpeakersActivation_6_speakers(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.MQB_5F_RearSpeakersActivation_8_speakers(),
				new VIM_MIB2(),
				new VIM_MIB3v2(),
				new MIM_MIB2(),
				new MIM_MIB3v2(),
				new MQBMultimediaRestart(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockUnlockDoorsForLowTrims(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.EnableKeyFobWhileEngineRunning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_ElectricWindowsWithoutIgnition(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_CloseDriverWindowAfterClosingCarMY19(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.SoundsAndAlarm.PQ26_SignalWhenIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.SoundsAndAlarm.ActivateAntiTheftAlarm(),
				Washers.ParkWipersAfterIgnitionTurnedOff(),
				Washers.WipersServicePositionMenu(),
				Washers.TearDropWindshield(),
				Washers.TearDropRearGlass(),
				Washers.MQB_09_ComfortRearWiper(),
				Washers.MQB_09_AutoRearWiper(),
				Washers.MQB_HeadlightWasherInterval(),
				Washers.MQB_DelayBeforeHeadlightWasher(),
				Washers.MQB_HeadlightWasherDutyDuration(),
				Washers.PQ26_WipersDefrostPosition(),
				Washers.PQ26_StopWiperWhenHoodOpened(),
				Washers.PQ26_ParkWiperWhenHoodOpened(),
				Washer.LowWasherLevelSensorInstalled(),
				new DSGAdaptationRoutine(),
				new MQBAisinProcedures(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.PQ26_ElectricWindowsComfort(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_AutolockAtSpeed(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_AutolockRear(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_AutoUnlock(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_AutoUnlockWhenSelectorInParking_NAR(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_LockMenu(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_UnlockDoorsVariantsNewBCM(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.Lock_UnlockDoorsVariantsOldBCM(),
				Service.ResetOilService(),
				Service.ResetInspectionService(),
				FeedbackSignals.AcousticFeedbackDurationOfAcousticFeedbackFromSimpleHorn(),
				FeedbackSignals.AcousticFeedbackForTheSecondCloseCommand(),
				FeedbackSignals.AcousticFeedbackGlobal(),
				FeedbackSignals.AcousticFeedbackLockAcousticFeedback(),
				FeedbackSignals.AcousticFeedbackMenu(),
				FeedbackSignals.AcousticFeedbackSignalHorn(),
				FeedbackSignals.AcousticFeedbackWhenUnlocking(),
				FeedbackSignals.MQB_09_SoundConfirmationOfLockAndUnlock(),
				FeedbackSignals.OpticalFeedback3rdBrakeLight(),
				FeedbackSignals.OpticalFeedbackComfortClosing(),
				FeedbackSignals.OpticalFeedbackWhenLocking(),
				new ThrottleAdaptationRoutine(),
				new VagUDSMassErrorReset(),
				new EngineAutomaticTestsProcedure(),
				new RDKSSensorPositionLearning(1),
				new RDKSSensorPositionLearning(2),
				new RDKSSensorPositionLearning(3),
				new RDKSSensorPositionLearning(4),
				new RDKSSensorIDLearning(0),
				new RDKSSensorIDLearning(1),
				new RDKSSensorIDLearning(2),
				new RDKSSensorIDLearning(3),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ResetToFactorySettings_FactorySettings(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayCharismaDriveMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayStartStopInfo()
			};
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(array);
			this.LoadedCollection.AddRange(MQB_LightConfigurationCoding.GetLightConfigurationPQ26());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.AddRange(MQBUnitBackupCreator.BuildBackupCreators("pq26"));
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_sound_0x3000());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x0700());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x7100());
			this.LoadedCollection.Add(new MQBSwapActivation());
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.AddRange(DatasetDumps.Create5FDatasetDumps());
			}
		}

		// Token: 0x060048BD RID: 18621 RVA: 0x003735C0 File Offset: 0x003717C0
		private void LoadVAG_PQ26_FabiaNJ()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("fabianj.db", true)
				select (x)).ToList<ICodingContainer>();
			ICodingContainer[] array = new ICodingContainer[]
			{
				new FuelPumpTest(),
				new TPMSResetRoutine(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionClimate(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionAC(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_PointerTest(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_Time24HoursFormat(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_LapTimerDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_RemoveKeyWarning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_ShowECOHints(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_FreeSpaceInFuelTankDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_WarnAboutRearFogLightsSpeedLimit(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_OilTemperatureDisplay(),
				Engine.MQB_01_AcceleratorPedalSensivity(),
				ExteriorLight.PQ26_DisableLicensePlateLightWhenTailgateIsOpened(),
				ExteriorLight.PQ26_Corner(),
				ExteriorLight.CornerRegulation(),
				ExteriorLight.PQ26_RearSideLightsAsDRL(),
				ExteriorLight.PQ26_StroboscopeEffectFogLightHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLowBeamHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLEDDRLHighBeam(),
				ExteriorLight.CornerUpperSpeedThreshold(),
				ExteriorLight.PQ26_LEDDRL_WithLowBeam(),
				ExteriorLight.PQ26_DimmingDRLWhenTurnLightsOn(),
				ExteriorLight.PQ26_FrontSideLightDimmingWithTurnLights(),
				ExteriorLight.PQ26_TurnOnStopSignalWhenDoorOpened(),
				ExteriorLight.PQ26_TurnOnSideTurnlightsWhenTailgateOpened(),
				ExteriorLight.TurnOffDRLWhenParkingBrakeOn(),
				ExteriorLight.TurnOffDRLWhenLightSwitchOff(),
				ExteriorLight.DRLSteupInMMI(),
				ExteriorLight.LimitMaxFrontLights(),
				ExteriorLight.ComingHomeWithLightSensor(),
				ExteriorLight.ComingHomeWithoutLightSensor(),
				ExteriorLight.PQ26_09_LightSensorSensivity(),
				ExteriorLight.TurnLightsPoliteBlinks(),
				ExteriorLight.ComingHomeLamps(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOnFogLightsWhenReversing(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LicensePlate_LampsType(),
				ExteriorLight.EmergencyBrakingLightsWithTurnSignals(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_AMRadio(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_AUXInEnable(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_OffroadModeDisplayInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_OffroadModeDisplayInMMI_Var2(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.MQB_5F_BetterSoundBolero(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_DrivingSchool(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ConfirmInstallationChanges(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_TripComputerAvailableWithIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.Multimedia.PQ26_5F_DisplayComfortUsage(),
				new MQBMultimediaRestart(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockUnlockDoorsForLowTrims(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.EnableKeyFobWhileEngineRunning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_ElectricWindowsWithoutIgnition(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_CloseDriverWindowAfterClosingCarMY19(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.SoundsAndAlarm.PQ26_SignalWhenIgnitionOff(),
				Washers.WipersServicePositionMenu(),
				Washers.ParkWipersAfterIgnitionTurnedOff(),
				Washers.TearDropWindshield(),
				Washers.TearDropRearGlass(),
				Washers.MQB_09_ComfortRearWiper(),
				Washers.MQB_09_AutoRearWiper(),
				Washers.MQB_HeadlightWasherInterval(),
				Washers.MQB_DelayBeforeHeadlightWasher(),
				Washers.MQB_HeadlightWasherDutyDuration(),
				Washers.PQ26_WipersDefrostPosition(),
				Washers.PQ26_StopWiperWhenHoodOpened(),
				Washers.PQ26_ParkWiperWhenHoodOpened(),
				Washer.LowWasherLevelSensorInstalled(),
				new DSGAdaptationRoutine(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.PQ26_ElectricWindowsComfort(),
				new DPFServiceRegeneration(),
				new DPFServiceRegenerationWhileDriving(""),
				Service.ResetOilService(),
				Service.ResetInspectionService(),
				new VIM_MIB2(),
				new VIM_MIB3v2(),
				new MIM_MIB2(),
				new MIM_MIB3v2(),
				FeedbackSignals.AcousticFeedbackDurationOfAcousticFeedbackFromSimpleHorn(),
				FeedbackSignals.AcousticFeedbackForTheSecondCloseCommand(),
				FeedbackSignals.AcousticFeedbackGlobal(),
				FeedbackSignals.AcousticFeedbackLockAcousticFeedback(),
				FeedbackSignals.AcousticFeedbackMenu(),
				FeedbackSignals.AcousticFeedbackSignalHorn(),
				FeedbackSignals.AcousticFeedbackWhenUnlocking(),
				FeedbackSignals.MQB_09_SoundConfirmationOfLockAndUnlock(),
				FeedbackSignals.OpticalFeedback3rdBrakeLight(),
				FeedbackSignals.OpticalFeedbackComfortClosing(),
				FeedbackSignals.OpticalFeedbackWhenLocking(),
				new ThrottleAdaptationRoutine(),
				new VagUDSMassErrorReset(),
				new EngineAutomaticTestsProcedure(),
				new RDKSSensorPositionLearning(1),
				new RDKSSensorPositionLearning(2),
				new RDKSSensorPositionLearning(3),
				new RDKSSensorPositionLearning(4),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ResetToFactorySettings_FactorySettings(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayCharismaDriveMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayStartStopInfo()
			};
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(array);
			this.LoadedCollection.AddRange(MQB_LightConfigurationCoding.GetLightConfigurationPQ26());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.AddRange(MQBUnitBackupCreator.BuildBackupCreators("pq26"));
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_sound_0x3000());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x0700());
			this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.MQB.MultimediaSoundQuality.MIB2SoundDataset_audio_parameter_individual_sound_processing_0x7100());
			this.LoadedCollection.Add(new MQBSwapActivation());
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.AddRange(DatasetDumps.Create5FDatasetDumps());
			}
		}

		// Token: 0x060048BE RID: 18622 RVA: 0x00373A3C File Offset: 0x00371C3C
		private void LoadVAG_PQ26_NewRapid2020()
		{
			List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<MQBAdaptationTemplate>>("pq26newrapid2020.db", true)
				select (x)).ToList<ICodingContainer>();
			ICodingContainer[] array = new ICodingContainer[]
			{
				new FuelPumpTest(),
				new TPMSResetRoutine(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionClimate(),
				CarScannerXamarinForms.Coding.DB.PQ26.Climate.PQ26_08_RememberLastRecyrculationPositionAC(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_PointerTest(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_Time24HoursFormat(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_17_LapTimerDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_RemoveKeyWarning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.PQ26_ShowECOHints(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_FreeSpaceInFuelTankDisplay(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_WarnAboutRearFogLightsSpeedLimit(),
				CarScannerXamarinForms.Coding.DB.PQ26.Dashboard.MQB_17_OilTemperatureDisplay(),
				ExteriorLight.PQ26_DisableLicensePlateLightWhenTailgateIsOpened(),
				ExteriorLight.PQ26_Corner(),
				ExteriorLight.CornerRegulation(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_RearSideLightsAsDRL_Rapid2020(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_RearSideLightsAsDRL_VWPolo2020(),
				ExteriorLight.PQ26_StroboscopeEffectFogLightHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLowBeamHighBeam(),
				ExteriorLight.PQ26_StroboscopeEffectLEDDRLHighBeam(),
				ExteriorLight.CornerUpperSpeedThreshold(),
				ExteriorLight.PQ26_LEDDRL_WithLowBeam(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_DimmingDRLWhenTurnLightsOn(),
				ExteriorLight.PQ26_FrontSideLightDimmingWithTurnLights(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_TurnOnStopSignalWhenDoorOpened(),
				ExteriorLight.PQ26_TurnOnSideTurnlightsWhenTailgateOpened(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOffDRLWhenParkingBrakeOn(),
				ExteriorLight.TurnOffDRLWhenLightSwitchOff(),
				ExteriorLight.DRLSteupInMMI(),
				ExteriorLight.LimitMaxFrontLights(),
				ExteriorLight.ComingHomeWithLightSensor(),
				ExteriorLight.ComingHomeWithoutLightSensor(),
				ExteriorLight.PQ26_09_LightSensorSensivity(),
				ExteriorLight.TurnLightsPoliteBlinks(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.TurnOnFogLightsWhenReversing(),
				CarScannerXamarinForms.Coding.DB.MQB.ExteriorLights.LicensePlate_LampsType(),
				ExteriorLight.ComingHomeLamps(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_DimmRearSideLightsWithTurnLight(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.ExteriorLights.PQ26_BlinkRearSideLightsWithTurnLight(),
				ExteriorLight.EmergencyBrakingLightsWithTurnSignals(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.MQB_5F_AMRadio(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.PQ26_5F_AUXInEnable(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.PQ26_5F_OffroadModeDisplayInMMI(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.PQ262020_5F_DrivingSchool(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.MQB_5F_TripComputerAvailableWithIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.PQ26_5F_DisplayComfortUsage(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Multimedia.PQ262020_5F_EcoDriving(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ConfirmInstallationChanges(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MIB3_WirelessAppleCarPlay(),
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.UserSelectionScreen_MMI_MIB3(),
				CarScannerXamarinForms.Coding.DB.MLB_A4B9.Multimedia.PrivacyWarningScreen_MMI_MIB3(),
				new MQBMultimediaRestart(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockUnlockDoorsForLowTrims(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_AutomaticLockkDoorsAt15kmhUnlockKeyRemoved(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.EnableKeyFobWhileEngineRunning(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_ElectricWindowsWithoutIgnition(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.PQ26_CloseDriverWindowAfterClosingCarMY19(),
				CarScannerXamarinForms.Coding.DB.PQ26.Other.MQB_DriverElectricWindowTimeAfterIgnitionTurnedOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.SoundsAndAlarm.PQ26_SignalWhenIgnitionOff(),
				CarScannerXamarinForms.Coding.DB.PQ26.SoundsAndAlarm.ActivateAntiTheftAlarm(),
				Washers.WipersServicePositionMenu(),
				Washers.ParkWipersAfterIgnitionTurnedOff(),
				Washers.TearDropWindshield(),
				Washers.TearDropRearGlass(),
				Washers.MQB_09_ComfortRearWiper(),
				Washers.MQB_09_AutoRearWiper(),
				Washers.MQB_HeadlightWasherInterval(),
				Washers.MQB_DelayBeforeHeadlightWasher(),
				Washers.MQB_HeadlightWasherDutyDuration(),
				Washers.PQ26_WipersDefrostPosition(),
				Washers.PQ26_StopWiperWhenHoodOpened(),
				Washers.PQ26_ParkWiperWhenHoodOpened(),
				Washer.LowWasherLevelSensorInstalled(),
				new DSGAdaptationRoutine(),
				CarScannerXamarinForms.Coding.DB.PQ26.Doors.PQ26_ElectricWindowsComfort(),
				Service.ResetOilService(),
				Service.ResetInspectionService(),
				FeedbackSignals.AcousticFeedbackDurationOfAcousticFeedbackFromSimpleHorn(),
				FeedbackSignals.AcousticFeedbackForTheSecondCloseCommand(),
				FeedbackSignals.AcousticFeedbackGlobal(),
				FeedbackSignals.AcousticFeedbackLockAcousticFeedback(),
				FeedbackSignals.AcousticFeedbackMenu(),
				FeedbackSignals.AcousticFeedbackSignalHorn(),
				FeedbackSignals.AcousticFeedbackWhenUnlocking(),
				FeedbackSignals.MQB_09_SoundConfirmationOfLockAndUnlock(),
				FeedbackSignals.OpticalFeedback3rdBrakeLight(),
				FeedbackSignals.OpticalFeedbackComfortClosing(),
				FeedbackSignals.OpticalFeedbackWhenLocking(),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Brakes.PQ26_2020_HillHoldControlActivation(),
				new ThrottleAdaptationRoutine(),
				new VagUDSMassErrorReset(),
				new MIM_MIB3PQ26WriteOnly(),
				InteriorLights.SmoothButtonsLight(),
				new EngineAutomaticTestsProcedure(),
				new RDKSSensorPositionLearning(1),
				new RDKSSensorPositionLearning(2),
				new RDKSSensorPositionLearning(3),
				new RDKSSensorPositionLearning(4),
				new RDKSSensorIDLearning(0),
				new RDKSSensorIDLearning(1),
				new RDKSSensorIDLearning(2),
				new RDKSSensorIDLearning(3),
				CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Dashboard.DigitalDashboardConfigurationInMMI(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.ResetToFactorySettings_FactorySettings(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayCharismaDriveMode(),
				CarScannerXamarinForms.Coding.DB.MQB.Multimedia.MQB_5F_DisplayStartStopInfo()
			};
			this.LoadedCollection.Clear();
			this.LoadedCollection.AddRange(list);
			this.LoadedCollection.AddRange(array);
			this.LoadedCollection.AddRange(MQB_LightConfigurationCoding.GetLightConfigurationPQ26());
			this.LoadedCollection.AddRange(this.LoadMQBLongCodingCollection());
			this.LoadedCollection.AddRange(MQBUnitBackupCreator.BuildBackupCreators("pq262020"));
			this.LoadedCollection.Add(new MQBSwapActivation());
			if (SharedSettings.Current.ShowExperimental)
			{
				this.LoadedCollection.Add(new DPFServiceRegeneration());
				this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.Assistance.PQ26_SpeedLimiter());
				if (SharedSettings.Current.DeveloperMode)
				{
					this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.MultimediaSoundQuality.PQ26AudioDatasets());
					this.LoadedCollection.Add(CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020.MultimediaSoundQuality.Rapid2020SoundAnn7202());
				}
			}
			this.LoadedCollection.AddRange(VagBrakeBleeding.BuildBrakeBleed_PQ262020());
		}

		// Token: 0x060048BF RID: 18623 RVA: 0x00373F10 File Offset: 0x00372110
		public void SetGroup(CodingGroup group)
		{
			CodingGroupCollection codingGroupCollection = this.Groups.FirstOrDefault((CodingGroupCollection x) => x.Group == group);
			if (codingGroupCollection != null)
			{
				this.CodingCollection.Clear();
				this.CodingCollection.AddRange(codingGroupCollection);
				this.Filter = "";
				this.SetFilteredCollection(this.CodingCollection);
			}
		}

		// Token: 0x060048C0 RID: 18624 RVA: 0x00373F74 File Offset: 0x00372174
		public void CreateGroupCollections(IEnumerable<ICodingContainer> codings)
		{
			List<CodingGroupCollection> tempList = new List<CodingGroupCollection>();
			using (IEnumerator<ICodingContainer> enumerator = codings.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ICodingContainer coding = enumerator.Current;
					CodingGroupCollection codingGroupCollection = tempList.FirstOrDefault((CodingGroupCollection x) => x.Group == coding.Group);
					if (codingGroupCollection == null)
					{
						codingGroupCollection = new CodingGroupCollection();
						codingGroupCollection.Group = coding.Group;
						tempList.Add(codingGroupCollection);
					}
					codingGroupCollection.Add(coding);
				}
			}
			for (int i = 0; i < tempList.Count; i++)
			{
				if (tempList[i].Group != CodingGroup.ManualLightConfiguration || tempList[i].Group != CodingGroup.SFD)
				{
					CodingGroupCollection codingGroupCollection2 = new CodingGroupCollection();
					codingGroupCollection2.Group = tempList[i].Group;
					ICodingContainer[] array = tempList[i].OrderBy((ICodingContainer x) => x.Name).ToArray<ICodingContainer>();
					if (SharedSettings.Current.SelectedBrand == "Hyundai" || SharedSettings.Current.SelectedBrand == "Kia")
					{
						array = tempList[i].ToArray<ICodingContainer>();
					}
					foreach (ICodingContainer codingContainer in array)
					{
						codingGroupCollection2.Add(codingContainer);
					}
					tempList[i] = codingGroupCollection2;
				}
			}
			tempList = (from x in tempList
				orderby x.Group == CodingGroup.BackupCreator descending, x.Group == CodingGroup.Other, x.Name
				select x).ToList<CodingGroupCollection>();
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				try
				{
					this.Groups.Clear();
					foreach (CodingGroupCollection codingGroupCollection3 in tempList)
					{
						this.Groups.Add(codingGroupCollection3);
					}
				}
				catch (Exception)
				{
				}
			});
		}

		// Token: 0x060048C1 RID: 18625 RVA: 0x003741D0 File Offset: 0x003723D0
		public IEnumerable<ICodingContainer> LoadMQBLongCodingCollection()
		{
			MQBLongCoding[] array = new MQBLongCoding[]
			{
				new MQBLongCoding
				{
					Name = "01. Engine",
					RequestHeader = "7E0",
					ResponseHeader = "7E8",
					Password = "27971"
				},
				new MQBLongCoding
				{
					Name = "02. Transmission",
					RequestHeader = "7E1",
					ResponseHeader = "7E9",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "03. Braking system",
					RequestHeader = "713",
					ResponseHeader = "77D",
					Password = "11966",
					PasswordHint = "20103, 40168, 11966, 25004"
				},
				new MQBLongCoding
				{
					Name = "04. Power steering",
					RequestHeader = "712",
					ResponseHeader = "77C",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "05. Authorization system for access and starting the engine",
					RequestHeader = "732",
					ResponseHeader = "79C",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "06. Front passenger seat",
					RequestHeader = "74D",
					ResponseHeader = "7B7",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "08. Heater and climate",
					RequestHeader = "746",
					ResponseHeader = "7B0",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "09. Onboard supply control unit",
					RequestHeader = "70E",
					ResponseHeader = "778",
					Password = "31347"
				},
				new MQBLongCoding
				{
					Address = "6001",
					Name = "09. Onboard supply control unit -> Subsystem 1 (Wipers/Rain sensor)",
					RequestHeader = "70E",
					ResponseHeader = "778",
					Password = "31347"
				},
				new MQBLongCoding
				{
					Address = "6002",
					Name = "09. Onboard supply control unit -> Subsystem 2 (RLHS)",
					RequestHeader = "70E",
					ResponseHeader = "778",
					Password = "31347"
				},
				new MQBLongCoding
				{
					Name = "0E. Media player 1",
					RequestHeader = "770",
					ResponseHeader = "7DA",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "10. Parking Assistant 2",
					RequestHeader = "70A",
					ResponseHeader = "774",
					Password = "71679"
				},
				new MQBLongCoding
				{
					Name = "11. Engine Electronics No. 2",
					RequestHeader = "7E2",
					ResponseHeader = "84C",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "13. Active cruise control",
					RequestHeader = "757",
					ResponseHeader = "7C1",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "14. Email system / Damping controls",
					RequestHeader = "772",
					ResponseHeader = "7DC",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "15. Srs",
					RequestHeader = "715",
					ResponseHeader = "77F",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "16. Steering column",
					RequestHeader = "70C",
					ResponseHeader = "776",
					Password = "20103"
				},
				new MQBLongCoding
				{
					Name = "17. Dashboard",
					RequestHeader = "714",
					ResponseHeader = "77E",
					Password = "20103"
				},
				new MQBLongCoding
				{
					Name = "18. Additional heater",
					RequestHeader = "76A",
					ResponseHeader = "7D4",
					Password = "80782"
				},
				new MQBLongCoding
				{
					Name = "19. CAN Gateway",
					RequestHeader = "710",
					ResponseHeader = "77A",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "1B. Active steering",
					RequestHeader = "716",
					ResponseHeader = "780",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "20. FLA",
					RequestHeader = "730",
					ResponseHeader = "79A",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "21. Energy Management 2",
					RequestHeader = "728",
					ResponseHeader = "792",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "22. Four-wheel drive",
					RequestHeader = "70F",
					ResponseHeader = "779",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "23. Increased braking force",
					RequestHeader = "73B",
					ResponseHeader = "7A5",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "25. Immobilizer",
					RequestHeader = "711",
					ResponseHeader = "77B",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "26. Electric folding roof",
					RequestHeader = "72D",
					ResponseHeader = "797",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "28. Climate Control Panel",
					RequestHeader = "71A",
					ResponseHeader = "784",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "32. Differential lock electronics",
					RequestHeader = "71E",
					ResponseHeader = "788",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "34. Ride height control system",
					RequestHeader = "755",
					ResponseHeader = "7BF",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "36. Driver seat adjustment",
					RequestHeader = "74C",
					ResponseHeader = "7B6",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "37. Navigation system",
					RequestHeader = "76C",
					ResponseHeader = "7D6",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "3C. Lane change assistant",
					RequestHeader = "74E",
					ResponseHeader = "7B8",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "3D. Special function",
					RequestHeader = "72C",
					ResponseHeader = "796",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "42. Driver door electronic equipment",
					RequestHeader = "74A",
					ResponseHeader = "7B4",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "44. Power steering",
					RequestHeader = "712",
					ResponseHeader = "77C",
					Password = "19249"
				},
				new MQBLongCoding
				{
					Name = "47. Acoustic system",
					RequestHeader = "76F",
					ResponseHeader = "7D9",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "51. Electric drive",
					RequestHeader = "7E6",
					ResponseHeader = "850",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "52. Front passenger door electronics",
					RequestHeader = "74B",
					ResponseHeader = "7B5",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "53. Parking brake",
					RequestHeader = "752",
					ResponseHeader = "7BC",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "55. Headlights corrector",
					RequestHeader = "754",
					ResponseHeader = "7BE",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "57. Tv tuner",
					RequestHeader = "76D",
					ResponseHeader = "7D7",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "5F. Electronic Information System 1",
					RequestHeader = "773",
					ResponseHeader = "7DD",
					Password = "20103"
				},
				new MQBLongCoding
				{
					Name = "65. TPMS",
					RequestHeader = "70B",
					ResponseHeader = "775",
					Password = "20103"
				},
				new MQBLongCoding
				{
					Name = "69. Trailer functions",
					RequestHeader = "747",
					ResponseHeader = "7B1",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "6C. Rear view camera system var.1",
					RequestHeader = "769",
					ResponseHeader = "7D3",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "6С. Rear view camera system var.2",
					RequestHeader = "6B8",
					ResponseHeader = "722",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "6D. Boot lid",
					RequestHeader = "723",
					ResponseHeader = "78D",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "6F. Comfort system central module 2",
					RequestHeader = "745",
					ResponseHeader = "7AF",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "71. Charger",
					RequestHeader = "71D",
					ResponseHeader = "787",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "75. Emergency Call and Communication Module",
					RequestHeader = "767",
					ResponseHeader = "7D1",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "76. Parking assistant",
					RequestHeader = "70A",
					ResponseHeader = "774",
					Password = "71679"
				},
				new MQBLongCoding
				{
					Name = "77. Telephone",
					RequestHeader = "76B",
					ResponseHeader = "7D5",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "7F. Electronic Information System 2",
					RequestHeader = "75A",
					ResponseHeader = "7C4",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "82. HUD",
					RequestHeader = "71B",
					ResponseHeader = "785",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "94. Srs",
					RequestHeader = "6BC",
					ResponseHeader = "726",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "95. Esp",
					RequestHeader = "784",
					ResponseHeader = "7EE",
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "A5. Front Assistant A5",
					RequestHeader = "74F",
					ResponseHeader = "7B9",
					Password = "20103"
				},
				new MQBLongCoding
				{
					Name = "BB. Rear door behind driver",
					RequestHeader = "73E",
					ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader("73E", "Audi", null),
					Password = ""
				},
				new MQBLongCoding
				{
					Name = "BC. Rear door behind passenger",
					RequestHeader = "73F",
					ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader("73F", "Audi", null),
					Password = ""
				}
			};
			MQBLongCoding[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i].Group = CodingGroup.LongCoding;
			}
			return array;
		}

		// Token: 0x040029E9 RID: 10729
		[CompilerGenerated]
		private string <WarningSpecific>k__BackingField;

		// Token: 0x040029EA RID: 10730
		private bool _IsPlatformSelectorVisible;

		// Token: 0x040029EB RID: 10731
		private ValueItemWithTranslation _CurrentPlatform = new ValueItemWithTranslation();

		// Token: 0x040029EC RID: 10732
		[CompilerGenerated]
		private ObservableCollection<ValueItemWithTranslation> <Platforms>k__BackingField;

		// Token: 0x040029ED RID: 10733
		private List<ICodingContainer> LoadedCollection = new List<ICodingContainer>();

		// Token: 0x040029EE RID: 10734
		private List<ICodingContainer> CodingCollection = new List<ICodingContainer>();

		// Token: 0x040029EF RID: 10735
		[CompilerGenerated]
		private ObservableCollection<ICodingContainer> <FilteredCollection>k__BackingField;

		// Token: 0x040029F0 RID: 10736
		private string _Filter = "";

		// Token: 0x040029F1 RID: 10737
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040029F2 RID: 10738
		[CompilerGenerated]
		private ObservableCollection<CodingGroupCollection> <Groups>k__BackingField;

		// Token: 0x02000852 RID: 2130
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060048C2 RID: 18626 RVA: 0x00374E9A File Offset: 0x0037309A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060048C3 RID: 18627 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060048C4 RID: 18628 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_MLB_A4B9>b__37_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048C5 RID: 18629 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_MLB_A4B9>b__37_1(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048C6 RID: 18630 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_MQB>b__38_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048C7 RID: 18631 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_MQB>b__38_1(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048C8 RID: 18632 RVA: 0x00374EA6 File Offset: 0x003730A6
			internal bool <LoadVAG_MQB>b__38_2(ICodingContainer x)
			{
				return x.Group == CodingGroup.DashboardDimming;
			}

			// Token: 0x060048C9 RID: 18633 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_0(HyundaiKiaUDSCoding x)
			{
				return x;
			}

			// Token: 0x060048CA RID: 18634 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_1(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048CB RID: 18635 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_2(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048CC RID: 18636 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_3(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048CD RID: 18637 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_4(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048CE RID: 18638 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_6(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048CF RID: 18639 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_8(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D0 RID: 18640 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_9(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D1 RID: 18641 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_10(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D2 RID: 18642 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_11(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D3 RID: 18643 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_12(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D4 RID: 18644 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadEasyCodingCollection>b__39_13(CustomizableCodingTemplate x)
			{
				return x;
			}

			// Token: 0x060048D5 RID: 18645 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_MEB>b__40_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048D6 RID: 18646 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_Yeti>b__41_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048D7 RID: 18647 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_Tiguan1FL>b__42_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048D8 RID: 18648 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_TouaregNFFL>b__43_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048D9 RID: 18649 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_Other>b__44_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048DA RID: 18650 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_PQ26>b__45_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048DB RID: 18651 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_PQ26_FabiaNJ>b__46_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048DC RID: 18652 RVA: 0x00016849 File Offset: 0x00014A49
			internal ICodingContainer <LoadVAG_PQ26_NewRapid2020>b__47_0(MQBAdaptationTemplate x)
			{
				return x;
			}

			// Token: 0x060048DD RID: 18653 RVA: 0x00374EB2 File Offset: 0x003730B2
			internal string <CreateGroupCollections>b__49_5(ICodingContainer x)
			{
				return x.Name;
			}

			// Token: 0x060048DE RID: 18654 RVA: 0x00374EBA File Offset: 0x003730BA
			internal bool <CreateGroupCollections>b__49_0(CodingGroupCollection x)
			{
				return x.Group == CodingGroup.BackupCreator;
			}

			// Token: 0x060048DF RID: 18655 RVA: 0x00374EC6 File Offset: 0x003730C6
			internal bool <CreateGroupCollections>b__49_1(CodingGroupCollection x)
			{
				return x.Group == CodingGroup.Other;
			}

			// Token: 0x060048E0 RID: 18656 RVA: 0x00374ED1 File Offset: 0x003730D1
			internal string <CreateGroupCollections>b__49_2(CodingGroupCollection x)
			{
				return x.Name;
			}

			// Token: 0x040029F3 RID: 10739
			public static readonly CodingListModel.<>c <>9 = new CodingListModel.<>c();

			// Token: 0x040029F4 RID: 10740
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__37_0;

			// Token: 0x040029F5 RID: 10741
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__37_1;

			// Token: 0x040029F6 RID: 10742
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__38_0;

			// Token: 0x040029F7 RID: 10743
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__38_1;

			// Token: 0x040029F8 RID: 10744
			public static Predicate<ICodingContainer> <>9__38_2;

			// Token: 0x040029F9 RID: 10745
			public static Func<HyundaiKiaUDSCoding, ICodingContainer> <>9__39_0;

			// Token: 0x040029FA RID: 10746
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_1;

			// Token: 0x040029FB RID: 10747
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_2;

			// Token: 0x040029FC RID: 10748
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_3;

			// Token: 0x040029FD RID: 10749
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_4;

			// Token: 0x040029FE RID: 10750
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_6;

			// Token: 0x040029FF RID: 10751
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_8;

			// Token: 0x04002A00 RID: 10752
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_9;

			// Token: 0x04002A01 RID: 10753
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_10;

			// Token: 0x04002A02 RID: 10754
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_11;

			// Token: 0x04002A03 RID: 10755
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_12;

			// Token: 0x04002A04 RID: 10756
			public static Func<CustomizableCodingTemplate, ICodingContainer> <>9__39_13;

			// Token: 0x04002A05 RID: 10757
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__40_0;

			// Token: 0x04002A06 RID: 10758
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__41_0;

			// Token: 0x04002A07 RID: 10759
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__42_0;

			// Token: 0x04002A08 RID: 10760
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__43_0;

			// Token: 0x04002A09 RID: 10761
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__44_0;

			// Token: 0x04002A0A RID: 10762
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__45_0;

			// Token: 0x04002A0B RID: 10763
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__46_0;

			// Token: 0x04002A0C RID: 10764
			public static Func<MQBAdaptationTemplate, ICodingContainer> <>9__47_0;

			// Token: 0x04002A0D RID: 10765
			public static Func<ICodingContainer, string> <>9__49_5;

			// Token: 0x04002A0E RID: 10766
			public static Func<CodingGroupCollection, bool> <>9__49_0;

			// Token: 0x04002A0F RID: 10767
			public static Func<CodingGroupCollection, bool> <>9__49_1;

			// Token: 0x04002A10 RID: 10768
			public static Func<CodingGroupCollection, string> <>9__49_2;
		}

		// Token: 0x02000853 RID: 2131
		[CompilerGenerated]
		private sealed class <>c__DisplayClass27_0
		{
			// Token: 0x060048E1 RID: 18657 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass27_0()
			{
			}

			// Token: 0x060048E2 RID: 18658 RVA: 0x00374EDC File Offset: 0x003730DC
			internal bool <set_Filter>b__0(ICodingContainer x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002A11 RID: 10769
			public string word;
		}

		// Token: 0x02000854 RID: 2132
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_0
		{
			// Token: 0x060048E3 RID: 18659 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_0()
			{
			}

			// Token: 0x060048E4 RID: 18660 RVA: 0x00374F34 File Offset: 0x00373134
			internal void <LoadEasyCodingCollection>b__5(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length == 2)
				{
					this.hasAmtResponse = true;
				}
			}

			// Token: 0x04002A12 RID: 10770
			public bool hasAmtResponse;
		}

		// Token: 0x02000855 RID: 2133
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_1
		{
			// Token: 0x060048E5 RID: 18661 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_1()
			{
			}

			// Token: 0x060048E6 RID: 18662 RVA: 0x00374F46 File Offset: 0x00373146
			internal void <LoadEasyCodingCollection>b__7(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length == 2)
				{
					this.hasAmtResponse = true;
				}
			}

			// Token: 0x04002A13 RID: 10771
			public bool hasAmtResponse;
		}

		// Token: 0x02000856 RID: 2134
		[CompilerGenerated]
		private sealed class <>c__DisplayClass48_0
		{
			// Token: 0x060048E7 RID: 18663 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass48_0()
			{
			}

			// Token: 0x060048E8 RID: 18664 RVA: 0x00374F58 File Offset: 0x00373158
			internal bool <SetGroup>b__0(CodingGroupCollection x)
			{
				return x.Group == this.group;
			}

			// Token: 0x04002A14 RID: 10772
			public CodingGroup group;
		}

		// Token: 0x02000857 RID: 2135
		[CompilerGenerated]
		private sealed class <>c__DisplayClass49_0
		{
			// Token: 0x060048E9 RID: 18665 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass49_0()
			{
			}

			// Token: 0x060048EA RID: 18666 RVA: 0x00374F68 File Offset: 0x00373168
			internal void <CreateGroupCollections>b__3()
			{
				try
				{
					this.<>4__this.Groups.Clear();
					foreach (CodingGroupCollection codingGroupCollection in this.tempList)
					{
						this.<>4__this.Groups.Add(codingGroupCollection);
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04002A15 RID: 10773
			public CodingListModel <>4__this;

			// Token: 0x04002A16 RID: 10774
			public List<CodingGroupCollection> tempList;
		}

		// Token: 0x02000858 RID: 2136
		[CompilerGenerated]
		private sealed class <>c__DisplayClass49_1
		{
			// Token: 0x060048EB RID: 18667 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass49_1()
			{
			}

			// Token: 0x060048EC RID: 18668 RVA: 0x00374FE8 File Offset: 0x003731E8
			internal bool <CreateGroupCollections>b__4(CodingGroupCollection x)
			{
				return x.Group == this.coding.Group;
			}

			// Token: 0x04002A17 RID: 10775
			public ICodingContainer coding;
		}

		// Token: 0x02000859 RID: 2137
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadEasyCodingCollection>d__39 : IAsyncStateMachine
		{
			// Token: 0x060048ED RID: 18669 RVA: 0x00375000 File Offset: 0x00373200
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingListModel codingListModel = this;
				try
				{
					TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter;
					TaskAwaiter<string[]> taskAwaiter3;
					TaskAwaiter taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<CustomizableCodingTemplate>>);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<CustomizableCodingTemplate>>);
						num = (num2 = -1);
						goto IL_08B6;
					}
					case 2:
					{
						TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<List<CustomizableCodingTemplate>>);
						num = (num2 = -1);
						goto IL_09FC;
					}
					case 3:
					{
						TaskAwaiter<string[]> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string[]>);
						num = (num2 = -1);
						goto IL_0D35;
					}
					case 4:
						IL_0EA7:
						try
						{
							if (num != 4)
							{
								if (!(Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpper() == "RU"))
								{
									goto IL_0FAF;
								}
								CS$<>8__locals1 = new CodingListModel.<>c__DisplayClass39_0();
								CS$<>8__locals1.hasAmtResponse = false;
								OBDRequest obdrequest = new OBDRequest("222E0C", "7E1", "", "", false);
								obdrequest.ResponseDecoded += delegate(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
								{
									if (data != null && data.Length == 2)
									{
										CS$<>8__locals1.hasAmtResponse = true;
									}
								};
								App.OBDReader.ReplaceQueue(obdrequest);
								taskAwaiter5 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num = (num2 = 4);
									TaskAwaiter taskAwaiter6 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter5, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter6;
								taskAwaiter5 = taskAwaiter6;
								taskAwaiter6 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter5.GetResult();
							if (CS$<>8__locals1.hasAmtResponse)
							{
								codingListModel.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
							}
							CS$<>8__locals1 = null;
							IL_0FAF:
							goto IL_1607;
						}
						catch (Exception)
						{
							goto IL_1607;
						}
						goto IL_0FBA;
					case 5:
					{
						TaskAwaiter<string[]> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string[]>);
						num = (num2 = -1);
						goto IL_102B;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_11A6;
					}
					case 7:
						goto IL_1365;
					default:
					{
						string text = SharedSettings.Current.BrandForDTC;
						if (string.IsNullOrEmpty(text))
						{
							text = SharedSettings.Current.SelectedBrand;
						}
						codingListModel.CodingCollection.Clear();
						codingListModel.LoadedCollection.Clear();
						if (text == null)
						{
							goto IL_1607;
						}
						int i = text.Length;
						switch (i)
						{
						case 3:
						{
							char c = text[0];
							if (c != 'K')
							{
								if (c != 'V')
								{
									if (c != 'В')
									{
										goto IL_1607;
									}
									if (!(text == "ВАЗ"))
									{
										goto IL_1607;
									}
									goto IL_0FBA;
								}
								else
								{
									if (!(text == "VAZ"))
									{
										goto IL_1607;
									}
									goto IL_0FBA;
								}
							}
							else
							{
								if (!(text == "Kia"))
								{
									goto IL_1607;
								}
								goto IL_06C9;
							}
							break;
						}
						case 4:
						{
							char c = text[0];
							if (c <= 'L')
							{
								if (c != 'A')
								{
									if (c != 'L')
									{
										goto IL_1607;
									}
									if (!(text == "Lada"))
									{
										goto IL_1607;
									}
									goto IL_0FBA;
								}
								else if (!(text == "Audi"))
								{
									goto IL_1607;
								}
							}
							else if (c != 'O')
							{
								if (c != 'S')
								{
									if (c != 'Л')
									{
										goto IL_1607;
									}
									if (!(text == "Лада"))
									{
										goto IL_1607;
									}
									goto IL_0FBA;
								}
								else if (!(text == "Seat"))
								{
									goto IL_1607;
								}
							}
							else
							{
								if (!(text == "Opel"))
								{
									goto IL_1607;
								}
								goto IL_0627;
							}
							break;
						}
						case 5:
						{
							char c = text[2];
							if (c != 'c')
							{
								switch (c)
								{
								case 'l':
									if (!(text == "Volvo"))
									{
										goto IL_1607;
									}
									goto IL_141E;
								case 'm':
								case 'p':
								case 'q':
								case 'r':
								case 's':
								case 'w':
									goto IL_1607;
								case 'n':
									if (!(text == "Honda"))
									{
										goto IL_1607;
									}
									break;
								case 'o':
									if (!(text == "Skoda"))
									{
										goto IL_1607;
									}
									goto IL_0457;
								case 't':
									if (!(text == "Jetta"))
									{
										goto IL_1607;
									}
									goto IL_0457;
								case 'u':
									if (!(text == "Acura"))
									{
										goto IL_1607;
									}
									break;
								case 'v':
									if (!(text == "Haval") && !(text == "Hover"))
									{
										goto IL_1607;
									}
									goto IL_1486;
								case 'x':
									if (!(text == "Lexus"))
									{
										goto IL_1607;
									}
									goto IL_1355;
								default:
									goto IL_1607;
								}
								if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit)
								{
									List<ICodingContainer> list = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("hondareset.db", true)
										select (x)).ToList<ICodingContainer>();
									codingListModel.LoadedCollection.AddRange(list);
									goto IL_1607;
								}
								goto IL_1607;
							}
							else
							{
								if (!(text == "Dacia"))
								{
									goto IL_1607;
								}
								goto IL_0CAA;
							}
							break;
						}
						case 6:
						{
							char c = text[0];
							if (c <= 'H')
							{
								if (c != 'D')
								{
									if (c != 'H')
									{
										goto IL_1607;
									}
									if (!(text == "Holden"))
									{
										goto IL_1607;
									}
									goto IL_0627;
								}
								else
								{
									if (!(text == "Daewoo"))
									{
										goto IL_1607;
									}
									goto IL_0627;
								}
							}
							else if (c != 'N')
							{
								if (c != 'S')
								{
									if (c != 'T')
									{
										goto IL_1607;
									}
									if (!(text == "Toyota"))
									{
										goto IL_1607;
									}
									goto IL_1355;
								}
								else
								{
									if (!(text == "Subaru"))
									{
										goto IL_1607;
									}
									codingListModel.LoadedCollection.Add(new SubaruResetSaS());
									goto IL_1607;
								}
							}
							else
							{
								if (!(text == "Nissan"))
								{
									goto IL_1607;
								}
								if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.SelectedProfileV2Name.Contains("Leaf"))
								{
									codingListModel.LoadedCollection.Add(new NissanLeafBattery());
									List<ICodingContainer> list2 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("leafze0.db", true)
										select (x)).ToList<ICodingContainer>();
									codingListModel.LoadedCollection.AddRange(list2);
									goto IL_1607;
								}
								if (new string[] { "6e121b9dbd814f6bbf48df31c44bbcf5", "Nissan X-Trail T31 2.0", "fe6c577e35d3481cb0224323bbf90860", "698734883a5d461ca3e016bc3ba1fe3b", "Nissan X-Trail T31 2.5", "293952d48d9240c588356e57a0d9b94a", "Nissan X-Trail T31 CVT" }.Contains(SharedSettings.Current.ProfileUpdateAlias) && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.KWP))
								{
									codingListModel.LoadedCollection.Add(new NissanCVTReset());
									goto IL_1607;
								}
								if (new string[]
								{
									"Qashqai J11", "d5c487cd83354b52a8245759aa2ffce4", "Qashqai J11 1.5dci K9K 636 [EN]", "896225c9ee8f48abbdb9b48bf135f2da", "Qashqai J11 1.5dci K9K 636 [RU]", "c3f57cd35c1e4fd9aa3cc7b57cf254a9", "Rogue T32 2014-2020", "513ecb4ac8eb4df5addf1acf6887f8f8", "X-Trail T32", "d4d7c86928de468c9259bcff2c2d95fe",
									"X-Trail T32 (NT32) 2.0 MR20DD [EN]", "X-Trail T32 (NT32) 2.0 MR20DD [RU]", "aa016831f2ce473483f6c02463038a47", "X-Trail T32, Qashqai 1.6 dCi R9M [EN]", "02316b1b16344ee7922a4f9e7140aa24", "X-Trail T32, Qashqai 1.6 dCi R9M [RU]", "03fa97678ac0457fa6bdd8780903875f"
								}.Contains(SharedSettings.Current.ProfileUpdateAlias) && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
								{
									codingListModel.LoadedCollection.Clear();
									codingListModel.LoadedCollection.AddRange(PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("t32.db", true));
									goto IL_1607;
								}
								if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && SharedSettings.Current.ShowExperimental)
								{
									codingListModel.LoadedCollection.Add(new NissanCVTReset());
									goto IL_1607;
								}
								goto IL_1607;
							}
							break;
						}
						case 7:
						{
							char c = text[0];
							if (c != 'H')
							{
								if (c != 'R')
								{
									goto IL_1607;
								}
								if (!(text == "Renault"))
								{
									goto IL_1607;
								}
								goto IL_0CAA;
							}
							else
							{
								if (!(text == "Hyundai"))
								{
									goto IL_1607;
								}
								goto IL_06C9;
							}
							break;
						}
						case 8:
							if (!(text == "Vauxhall"))
							{
								goto IL_1607;
							}
							goto IL_0627;
						case 9:
							if (!(text == "Chevrolet"))
							{
								goto IL_1607;
							}
							goto IL_0627;
						case 10:
						{
							char c = text[0];
							if (c != 'G')
							{
								if (c != 'M')
								{
									if (c != 'V')
									{
										goto IL_1607;
									}
									if (!(text == "Volkswagen"))
									{
										goto IL_1607;
									}
								}
								else
								{
									if (!(text == "Mitsubishi"))
									{
										goto IL_1607;
									}
									if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
									{
										codingListModel.LoadedCollection.Add(new MitsuCVTOilReset());
										goto IL_1607;
									}
									goto IL_1607;
								}
							}
							else
							{
								if (!(text == "Great Wall"))
								{
									goto IL_1607;
								}
								goto IL_1486;
							}
							break;
						}
						default:
							goto IL_1607;
						}
						IL_0457:
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit && SharedSettings.Current.ShowExperimental)
						{
							codingListModel.LoadVAG_MEB();
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
						{
							string value = codingListModel.CurrentPlatform.Value;
							if (value != null)
							{
								i = value.Length;
								switch (i)
								{
								case 3:
									if (value == "MQB")
									{
										codingListModel.LoadVAG_MQB();
										goto IL_1607;
									}
									break;
								case 4:
								{
									char c = value[0];
									if (c != 'P')
									{
										if (c == 'Y')
										{
											if (value == "YETI")
											{
												codingListModel.LoadVAG_Yeti();
												goto IL_1607;
											}
										}
									}
									else if (value == "PQ26")
									{
										codingListModel.LoadVAG_PQ26();
										goto IL_1607;
									}
									break;
								}
								case 5:
									if (value == "TNFFL")
									{
										codingListModel.LoadVAG_TouaregNFFL();
										goto IL_1607;
									}
									break;
								case 6:
								case 8:
								case 10:
								case 11:
									break;
								case 7:
									if (value == "FABIANJ")
									{
										codingListModel.LoadVAG_PQ26_FabiaNJ();
										goto IL_1607;
									}
									break;
								case 9:
									if (value == "TIGUAN1FL")
									{
										codingListModel.LoadVAG_Tiguan1FL();
										goto IL_1607;
									}
									break;
								case 12:
									if (value == "MLB-EVO-A4B9")
									{
										codingListModel.LoadVAG_MLB_A4B9();
										goto IL_1607;
									}
									break;
								default:
									if (i == 16)
									{
										if (value == "PQ26NEWRAPID2020")
										{
											codingListModel.LoadVAG_PQ26_NewRapid2020();
											goto IL_1607;
										}
									}
									break;
								}
							}
							codingListModel.LoadVAG_Other();
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
						{
							codingListModel.LoadedCollection.Clear();
							codingListModel.LoadedCollection.AddRange(VWLongCoding.BuildLongCodingList());
							goto IL_1607;
						}
						goto IL_1607;
						IL_0627:
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
						{
							codingListModel.LoadedCollection.Add(new GMResetServiceCANVar1());
							goto IL_1607;
						}
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && (SharedSettings.Current.ProfileUpdateAlias == "c83763c04180489992fd1fedd17eaec5" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (EN)" || SharedSettings.Current.ProfileUpdateAlias == "6d7cef6bb26b48bb8f31cdfeba9e289f" || SharedSettings.Current.ProfileUpdateAlias == "Sirius D42 (RU)"))
						{
							codingListModel.LoadedCollection.Add(new SiriusD42ResetThrottle());
							goto IL_1607;
						}
						goto IL_1607;
						IL_06C9:
						if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
						{
							goto IL_1607;
						}
						codingListModel.LoadedCollection.Add(new HyundaiReset6AT());
						codingListModel.LoadedCollection.Add(new HyundaiReset4AT());
						codingListModel.LoadedCollection.Add(new ResetEngineAdaptation());
						codingListModel.LoadedCollection.Add(new HyundaKiaResetServiceReminder());
						codingListModel.LoadedCollection.Add(BrakeBleed.CeratoYD());
						codingListModel.LoadedCollection.Add(BrakeBleed.PicantoJA());
						codingListModel.LoadedCollection.Add(BrakeBleed.PicantoTA());
						codingListModel.LoadedCollection.Add(BrakeBleed.RioFB());
						codingListModel.LoadedCollection.Add(BrakeBleed.RioQBR());
						codingListModel.LoadedCollection.Add(new CRDIFuelPump_SorentoUMFL());
						codingListModel.LoadedCollection.Add(new CRDIFuelPump_SportageSL());
						codingListModel.LoadedCollection.Add(new SportageQLThrottleAdaptationReset());
						codingListModel.LoadedCollection.Add(new SportageQL_ESP_Wheel_Calibration());
						codingListModel.LoadedCollection.Add(new SportageQL_MDPS_Calibration());
						codingListModel.LoadedCollection.Add(new ElectricParkingBrakeService_SportageQL());
						codingListModel.LoadedCollection.Add(new ElectricParkingBrakeService_OptimaJF());
						codingListModel.LoadedCollection.Add(new ElectricParkingBrakeService_OptimaTF());
						codingListModel.LoadedCollection.Add(new ElectricParkingBrakeService_CeedJD());
						taskAwaiter = EngineActuatorTestsDetector.GetSupportedActuators().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CustomizableCodingTemplate>>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter, ref this);
							return;
						}
						break;
						IL_0CAA:
						if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit && App.OBDReader.CurrentELMFormat != ELMFormat.CAN29bit && App.OBDReader.CurrentELMFormat != ELMFormat.KWP)
						{
							goto IL_1607;
						}
						taskAwaiter3 = new RenaultUnitsDetector().GetFileNames(progress).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter<string[]> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string[]>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0D35;
						IL_1355:
						if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
						{
							goto IL_1365;
						}
						goto IL_1607;
						IL_1486:
						if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit || (!(SharedSettings.Current.ProfileUpdateAlias == "17e35e3c0b974724b2a7d6533c755aac") && !(SharedSettings.Current.ProfileUpdateAlias == "6dcaa7bc7c5948ddb5aab6530fbd6575") && !(SharedSettings.Current.ProfileUpdateAlias == "6cc38fcd23fc4017a78f18ba2b45b352")))
						{
							goto IL_1607;
						}
						List<ICodingContainer> list3 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("havalh5.db", true)
							select (x)).ToList<ICodingContainer>();
						codingListModel.LoadedCollection.AddRange(list3);
						if (SharedSettings.Current.ProfileUpdateAlias == "6cc38fcd23fc4017a78f18ba2b45b352")
						{
							codingListModel.LoadedCollection.Add(new DelphiInjectorCoding(1, "22FDE1", "2EFDE1"));
							codingListModel.LoadedCollection.Add(new DelphiInjectorCoding(2, "22FDE2", "2EFDE2"));
							codingListModel.LoadedCollection.Add(new DelphiInjectorCoding(3, "22FDE3", "2EFDE3"));
							codingListModel.LoadedCollection.Add(new DelphiInjectorCoding(4, "22FDE4", "2EFDE4"));
							goto IL_1607;
						}
						goto IL_1607;
					}
					}
					List<CustomizableCodingTemplate> result = taskAwaiter.GetResult();
					engineActuators = result;
					taskAwaiter = SmartJunktionUnitTestDetector.GetSupportedActuators().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CustomizableCodingTemplate>>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter, ref this);
						return;
					}
					IL_08B6:
					List<CustomizableCodingTemplate> result2 = taskAwaiter.GetResult();
					List<ICodingContainer> list4 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<HyundaiKiaUDSCoding>>("kiaservice.db", true)
						select (x)).ToList<ICodingContainer>();
					list4.AddRange(engineActuators);
					list4.AddRange(result2);
					codingListModel.LoadedCollection.AddRange(list4);
					codingListModel.LoadedCollection.AddRange((from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiadashboardvariant.db", true)
						select (x)).ToList<ICodingContainer>());
					codingListModel.LoadedCollection.AddRange((from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiatpms.db", true)
						select (x)).ToList<ICodingContainer>());
					taskAwaiter = KiaBCMSupportedDetector.GetSupportedIds(PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("kiabcm.db", true).ToList<CustomizableCodingTemplate>()).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CustomizableCodingTemplate>>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter, ref this);
						return;
					}
					IL_09FC:
					List<CustomizableCodingTemplate> result3 = taskAwaiter.GetResult();
					if (result3 != null)
					{
						codingListModel.LoadedCollection.AddRange(result3);
					}
					List<CustomizableCodingTemplate> list5 = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("hyundaiparkingbrake.db", true).ToList<CustomizableCodingTemplate>();
					codingListModel.LoadedCollection.AddRange(list5);
					codingListModel.LoadedCollection.Add(new CretaTPMSCoding());
					codingListModel.LoadedCollection.Add(new HyundaiDPFRegen());
					if (SharedSettings.Current.ShowExperimental)
					{
						codingListModel.LoadedCollection.Add(DCTAdaptation.CreateDTCAdaptation());
					}
					engineActuators = null;
					goto IL_1607;
					IL_0D35:
					foreach (string text2 in taskAwaiter3.GetResult())
					{
						if (text2 == "RenaultFapVer1")
						{
							codingListModel.LoadedCollection.Add(new RenaultFapVer1());
						}
						else if (text2 == "RenaultFapVer2")
						{
							codingListModel.LoadedCollection.Add(new RenaultFapVer2());
						}
						else if (text2 == "RenaultFapVer3")
						{
							codingListModel.LoadedCollection.Add(new RenaultFapVer3());
						}
						else
						{
							try
							{
								List<ICodingContainer> list6 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault." + text2, true)
									select (x)).ToList<ICodingContainer>();
								codingListModel.LoadedCollection.AddRange(list6);
							}
							catch (Exception)
							{
							}
							if (text2 == "ABSESC_X_ALL_10C0.db")
							{
								codingListModel.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("10C0"));
							}
							if (text2 == "ABSESC_X_ALL_1003.db")
							{
								codingListModel.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("1003"));
							}
							if (text2 == "CMFB_CEPS_JTEKT_V3.2_20181030T122041.db")
							{
								codingListModel.LoadedCollection.Add(new EPSBackup());
								codingListModel.LoadedCollection.Add(new ESPConfigAutoRestore());
							}
						}
					}
					if (codingListModel.LoadedCollection.Count > 0)
					{
						codingListModel.LoadedCollection.Add(new DaciaServiceReset());
						goto IL_0EA7;
					}
					goto IL_1607;
					IL_0FBA:
					if (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit)
					{
						goto IL_1607;
					}
					taskAwaiter3 = new RenaultUnitsDetector().GetFileNames(progress).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 5);
						TaskAwaiter<string[]> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string[]>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_102B:
					foreach (string text3 in taskAwaiter3.GetResult())
					{
						try
						{
							List<ICodingContainer> list7 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault." + text3, true)
								select (x)).ToList<ICodingContainer>();
							codingListModel.LoadedCollection.AddRange(list7);
						}
						catch (Exception)
						{
						}
						if (text3 == "ABSESC_X_ALL_10C0.db")
						{
							codingListModel.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("10C0"));
						}
						if (text3 == "ABSESC_X_ALL_1003.db")
						{
							codingListModel.LoadedCollection.Add(new VestaRenaultDriveWheelPositionSensor("1003"));
						}
					}
					if (codingListModel.LoadedCollection.Count != 0)
					{
						codingListModel.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
						goto IL_11E3;
					}
					CS$<>8__locals2 = new CodingListModel.<>c__DisplayClass39_1();
					CS$<>8__locals2.hasAmtResponse = false;
					OBDRequest obdrequest2 = new OBDRequest("222E0C", "7E1", "", "", false);
					obdrequest2.ResponseDecoded += delegate(OBDRequest clutchreq2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length == 2)
						{
							CS$<>8__locals2.hasAmtResponse = true;
						}
					};
					App.OBDReader.ReplaceQueue(obdrequest2);
					taskAwaiter5 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 6);
						TaskAwaiter taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter5, ref this);
						return;
					}
					IL_11A6:
					taskAwaiter5.GetResult();
					if (CS$<>8__locals2.hasAmtResponse)
					{
						codingListModel.LoadedCollection.AddRange(VestaAMTProcedure.VestaAMTProcedures());
					}
					CS$<>8__locals2 = null;
					IL_11E3:
					if (codingListModel.LoadedCollection.Count > 0)
					{
						List<ICodingContainer> list8 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("vestam86.db", true)
							select (x)).ToList<ICodingContainer>();
						codingListModel.LoadedCollection.AddRange(list8);
					}
					if (SharedSettings.Current.ProfileUpdateAlias == "bdae95afb9d44d1b825011e7144af5ba" || SharedSettings.Current.ProfileUpdateAlias == "66ed536069554560ae1b6a27c27b31c4")
					{
						codingListModel.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTSetDateOfServiceProcedure());
						codingListModel.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTResetProcedure());
						codingListModel.LoadedCollection.Add(VestaRenaultCVTReset.VestaRenaultCVTResetECU());
					}
					if (SharedSettings.Current.ProfileUpdateAlias == "780429f87d044ba2ab5d00064a72367d" || SharedSettings.Current.ProfileUpdateAlias == "587c1d092acc4576a737d2f50287baa0")
					{
						codingListModel.LoadedCollection.Clear();
						List<ICodingContainer> list9 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("vestang.db", true)
							select (x)).ToList<ICodingContainer>();
						codingListModel.LoadedCollection.AddRange(list9);
						List<ICodingContainer> list10 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("Renault.PP_vesta_ng.db", true)
							select (x)).ToList<ICodingContainer>();
						codingListModel.LoadedCollection.AddRange(list10);
						goto IL_1607;
					}
					goto IL_1607;
					IL_1365:
					try
					{
						if (num != 7)
						{
							List<CustomizableCodingTemplate> list11 = PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("tlcprado150.db", true);
							list11.Add(new ToyotaTPMS2ACoding());
							list11.Add(new ToyotaTPMS2ACoding5Wheels());
							taskAwaiter = new ToyotaUnitsDetector().CheckAvailableCodings(list11, progress).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 7);
								TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<List<CustomizableCodingTemplate>>, CodingListModel.<LoadEasyCodingCollection>d__39>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<List<CustomizableCodingTemplate>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<List<CustomizableCodingTemplate>>);
							num = (num2 = -1);
						}
						List<CustomizableCodingTemplate> result4 = taskAwaiter.GetResult();
						codingListModel.LoadedCollection.AddRange(result4);
						goto IL_1607;
					}
					catch (Exception)
					{
						codingListModel.LoadedCollection.Clear();
						goto IL_1607;
					}
					IL_141E:
					if (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit)
					{
						List<ICodingContainer> list12 = (from x in PackageFileReader.BinaryDeserilzeFromEmbeddedFileUsingCryptoStream2<List<CustomizableCodingTemplate>>("volvo.db", true)
							select (x)).ToList<ICodingContainer>();
						codingListModel.LoadedCollection.AddRange(list12);
						codingListModel.LoadedCollection.Add(new VolvoP3SetTimeDashboard());
					}
					IL_1607:
					if (SharedSettings.Current.ShowExperimental)
					{
						CustomCodingsListViewModel customCodingsListViewModel = new CustomCodingsListViewModel();
						customCodingsListViewModel.Load();
						codingListModel.LoadedCollection.AddRange(customCodingsListViewModel);
					}
					codingListModel.CreateGroupCollections(codingListModel.LoadedCollection);
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

			// Token: 0x060048EE RID: 18670 RVA: 0x003766F4 File Offset: 0x003748F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A18 RID: 10776
			public int <>1__state;

			// Token: 0x04002A19 RID: 10777
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002A1A RID: 10778
			public CodingListModel <>4__this;

			// Token: 0x04002A1B RID: 10779
			public IProgress<string> progress;

			// Token: 0x04002A1C RID: 10780
			private CodingListModel.<>c__DisplayClass39_0 <>8__1;

			// Token: 0x04002A1D RID: 10781
			private CodingListModel.<>c__DisplayClass39_1 <>8__2;

			// Token: 0x04002A1E RID: 10782
			private List<CustomizableCodingTemplate> <engineActuators>5__2;

			// Token: 0x04002A1F RID: 10783
			private TaskAwaiter<List<CustomizableCodingTemplate>> <>u__1;

			// Token: 0x04002A20 RID: 10784
			private TaskAwaiter<string[]> <>u__2;

			// Token: 0x04002A21 RID: 10785
			private TaskAwaiter <>u__3;
		}
	}
}
