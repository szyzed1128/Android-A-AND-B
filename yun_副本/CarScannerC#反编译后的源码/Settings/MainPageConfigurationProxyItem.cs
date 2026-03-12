using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000204 RID: 516
	internal class MainPageConfigurationProxyItem : INotifyPropertyChanged
	{
		// Token: 0x06001A5F RID: 6751 RVA: 0x00122E50 File Offset: 0x00121050
		public MainPageConfigurationProxyItem()
		{
		}

		// Token: 0x06001A60 RID: 6752 RVA: 0x00122E5F File Offset: 0x0012105F
		public MainPageConfigurationProxyItem(MainPageButtons ButtonType)
		{
			this.ButtonType = ButtonType;
		}

		// Token: 0x06001A61 RID: 6753 RVA: 0x00122E75 File Offset: 0x00121075
		public MainPageConfigurationProxyItem(MainPageButtons ButtonType, bool IsVisible)
			: this(ButtonType)
		{
			this.IsVisible = IsVisible;
		}

		// Token: 0x17000F9A RID: 3994
		// (get) Token: 0x06001A62 RID: 6754 RVA: 0x00122E85 File Offset: 0x00121085
		// (set) Token: 0x06001A63 RID: 6755 RVA: 0x00122E8D File Offset: 0x0012108D
		public MainPageButtons ButtonType
		{
			[CompilerGenerated]
			get
			{
				return this.<ButtonType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ButtonType>k__BackingField = value;
			}
		}

		// Token: 0x17000F9B RID: 3995
		// (get) Token: 0x06001A64 RID: 6756 RVA: 0x00122E96 File Offset: 0x00121096
		// (set) Token: 0x06001A65 RID: 6757 RVA: 0x00122E9E File Offset: 0x0012109E
		public bool IsVisible
		{
			get
			{
				return this._IsVisible;
			}
			set
			{
				this._IsVisible = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("IsVisible"));
			}
		}

		// Token: 0x17000F9C RID: 3996
		// (get) Token: 0x06001A66 RID: 6758 RVA: 0x00122EC4 File Offset: 0x001210C4
		[JsonIgnore]
		public string Title
		{
			get
			{
				switch (this.ButtonType)
				{
				case MainPageButtons.Dashboard:
					return Translate.GetString("ios_MainPage_TileDashboard");
				case MainPageButtons.LiveData:
					return Translate.GetString("ios_MainPage_TileLiveData");
				case MainPageButtons.AllSensors:
					return Translate.GetString("ios_MainPage_TileAllSensors");
				case MainPageButtons.DTC:
					return Translate.GetString("ios_MainPage_TileDtcErrors");
				case MainPageButtons.FreezeFrame:
					return Translate.GetString("ios_MainPage_TileFreezeFrame");
				case MainPageButtons.Mode06:
					return Translate.GetString("ios_Mode06Page_Title");
				case MainPageButtons.Acceleration:
					return Translate.GetString("ios_MainPage_TileSpeedTest");
				case MainPageButtons.EcoTests:
					return Translate.GetString("ios_MainPage_TileEmissionTests");
				case MainPageButtons.DataRecords:
					return Translate.GetString("Settings_Control_DataRecording.Content");
				case MainPageButtons.Settings:
					return Translate.GetString("ios_MainPage_Settings");
				case MainPageButtons.FuelStatistics:
					return Translate.GetString("ios_FuelStatisticsPage.Text");
				case MainPageButtons.MyCars:
					return Translate.GetString("ios_GarageTitle");
				case MainPageButtons.Terminal:
					return Translate.GetString("ios_MainPage_TileTerminal");
				case MainPageButtons.Purchase:
					return Translate.GetString("ios_BuyPro");
				default:
					return this.ButtonType.ToString();
				}
			}
		}

		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001A67 RID: 6759 RVA: 0x00122FCC File Offset: 0x001211CC
		// (remove) Token: 0x06001A68 RID: 6760 RVA: 0x00123004 File Offset: 0x00121204
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

		// Token: 0x04000BAB RID: 2987
		[CompilerGenerated]
		private MainPageButtons <ButtonType>k__BackingField;

		// Token: 0x04000BAC RID: 2988
		private bool _IsVisible = true;

		// Token: 0x04000BAD RID: 2989
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;
	}
}
