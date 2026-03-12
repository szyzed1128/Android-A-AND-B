using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.UserControls;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000202 RID: 514
	internal class MainPageConfigurationModel
	{
		// Token: 0x06001A55 RID: 6741 RVA: 0x001227AC File Offset: 0x001209AC
		public static MainPageButton BuildButton(MainPageButtons proxyButton)
		{
			switch (proxyButton)
			{
			case MainPageButtons.Dashboard:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileDashboard"), "DashboardActiveImage", "DashboardInactiveImage", new EventHandler(SimpleMainPage.Instance.btnDashboard_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.LiveData:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileLiveData"), "LiveDataActiveImage", "LiveDataInactiveImage", new EventHandler(SimpleMainPage.Instance.btnLiveData_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.AllSensors:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileAllSensors"), "LiveDataListActiveImage", "LiveDataListInactiveImage", new EventHandler(SimpleMainPage.Instance.btnLiveDataTable_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.DTC:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileDtcErrors"), "DTCActiveImage", "DTCInactiveImage", new EventHandler(SimpleMainPage.Instance.btnDTC_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.FreezeFrame:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileFreezeFrame"), "FreezeFrameActiveImage", "FreezeFrameInactiveImage", new EventHandler(SimpleMainPage.Instance.btnFreezeFrame_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.Mode06:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_Mode06Page_Title"), "Mode06ActiveImage", "Mode06InactiveImage", new EventHandler(SimpleMainPage.Instance.btnMode06_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.Acceleration:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileSpeedTest"), "SpeedTestActiveImage", "SpeedTestInactiveImage", new EventHandler(SimpleMainPage.Instance.btnSpeedTest_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.EcoTests:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileEmissionTests"), "EcoTestsActiveImage", "EcoTestsInactiveImage", new EventHandler(SimpleMainPage.Instance.btnEmissionTests_Tapped), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.DataRecords:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("Settings_Control_DataRecording.Content"), "RecordsImage", "RecordsImage", new EventHandler(SimpleMainPage.Instance.btnRecords_Clicked), null, proxyButton);
			case MainPageButtons.Settings:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_Settings"), "SettingsImage", "SettingsImage", new EventHandler(SimpleMainPage.Instance.btnSettings_Clicked), null, proxyButton);
			case MainPageButtons.FuelStatistics:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_FuelStatisticsPage.Text"), "FuelStatisticsImage", "FuelStatisticsImage", new EventHandler(SimpleMainPage.Instance.btnFuelStatistics_Clicked), null, proxyButton);
			case MainPageButtons.MyCars:
			{
				string text;
				if (string.IsNullOrEmpty(SharedSettings.Current.CurrentCarName) || SharedSettings.Current.CurrentCarName == Translate.GetString("ios_MY_CAR"))
				{
					text = Translate.GetString("ios_GarageTitle");
				}
				else
				{
					text = SharedSettings.Current.CurrentCarName;
					if (text.Length > 10)
					{
						text = text.Substring(0, 10) + "...";
					}
				}
				return MainPageConfigurationModel.BuildButton(text, "GarageActiveImage", "GarageInactiveImage", new EventHandler(SimpleMainPage.Instance.btnGarage_Clicked), new OBDReaderDisconnectedStatusToTrue(), proxyButton);
			}
			case MainPageButtons.Terminal:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_MainPage_TileTerminal"), "TerminalActiveImage", "TerminalInactiveImage", new EventHandler(SimpleMainPage.Instance.btnTerminal_Clicked), new OBDReaderDisconnectedOrConnectedToELMorECUStatusToTrue(), proxyButton);
			case MainPageButtons.Purchase:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ios_BuyPro"), "PurchaseImage", "PurchaseImage", new EventHandler(SimpleMainPage.Instance.btnPurchase_Clicked), null, proxyButton);
			case MainPageButtons.Coding:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("coding_Coding"), "CodingActiveImage", "CodingInactiveImage", new EventHandler(SimpleMainPage.Instance.btnCoding_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			case MainPageButtons.VersionInfo:
				return MainPageConfigurationModel.BuildButton(Translate.GetString("ecuIdentsTitle"), "VersionsActiveImage", "VersionsInactiveImage", new EventHandler(SimpleMainPage.Instance.btnVersions_Clicked), new OBDReaderConnectedToECUStatusToTrue(), proxyButton);
			default:
				throw new Exception("BuildButtonException");
			}
		}

		// Token: 0x06001A56 RID: 6742 RVA: 0x00122B58 File Offset: 0x00120D58
		internal void Save()
		{
			string text = JsonConvert.SerializeObject(this.Configuration);
			SharedSettings.Current.MainPageButtonsConfiguration = text;
		}

		// Token: 0x06001A57 RID: 6743 RVA: 0x00122B7C File Offset: 0x00120D7C
		private static MainPageButton BuildButton(string Text, string ActiveImage, string InactiveImage, EventHandler TappedEvent, IValueConverter OBDStatusToActiveConverter, MainPageButtons ButtonType)
		{
			MainPageButton mainPageButton = new MainPageButton();
			mainPageButton.SetDynamicResource(MainPageButton.ActiveImageProperty, ActiveImage);
			mainPageButton.SetDynamicResource(MainPageButton.InactiveImageProperty, InactiveImage);
			mainPageButton.BindingContext = App.OBDReader;
			mainPageButton.Text = Text;
			mainPageButton.Tapped -= TappedEvent;
			mainPageButton.Tapped += TappedEvent;
			mainPageButton.ButtonType = ButtonType;
			if (OBDStatusToActiveConverter != null)
			{
				Binding binding = new Binding("CurrentStatus", 2, OBDStatusToActiveConverter, null, null, null);
				mainPageButton.SetBinding(MainPageButton.IsActiveProperty, binding);
			}
			return mainPageButton;
		}

		// Token: 0x06001A58 RID: 6744 RVA: 0x00122BF4 File Offset: 0x00120DF4
		public MainPageConfigurationModel()
		{
			string mainPageButtonsConfiguration = SharedSettings.Current.MainPageButtonsConfiguration;
			if (string.IsNullOrEmpty(mainPageButtonsConfiguration))
			{
				this.ResetToDefault();
				return;
			}
			try
			{
				this.Configuration = JsonConvert.DeserializeObject<ObservableCollection<MainPageConfigurationProxyItem>>(mainPageButtonsConfiguration);
				this.AddMissingEntries();
			}
			catch (Exception)
			{
				this.ResetToDefault();
			}
		}

		// Token: 0x06001A59 RID: 6745 RVA: 0x00122C5C File Offset: 0x00120E5C
		private void AddMissingEntries()
		{
			try
			{
				foreach (object obj in Enum.GetValues(typeof(MainPageButtons)))
				{
					MainPageButtons buttonType = (MainPageButtons)obj;
					if (buttonType != MainPageButtons.Coding && !this.Configuration.Any((MainPageConfigurationProxyItem x) => x.ButtonType == buttonType))
					{
						this.Configuration.Add(new MainPageConfigurationProxyItem(buttonType, false));
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001A5A RID: 6746 RVA: 0x00122D10 File Offset: 0x00120F10
		public void ResetToDefault()
		{
			this.Configuration.Clear();
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.Dashboard, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.LiveData, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.AllSensors, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.DTC, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.FreezeFrame, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.Mode06, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.Purchase, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.MyCars, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.Settings, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.FuelStatistics, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.VersionInfo, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.DataRecords, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.Acceleration, true));
			this.Configuration.Add(new MainPageConfigurationProxyItem(MainPageButtons.EcoTests, true));
			this.AddMissingEntries();
		}

		// Token: 0x17000F99 RID: 3993
		// (get) Token: 0x06001A5B RID: 6747 RVA: 0x00122E2F File Offset: 0x0012102F
		// (set) Token: 0x06001A5C RID: 6748 RVA: 0x00122E37 File Offset: 0x00121037
		public ObservableCollection<MainPageConfigurationProxyItem> Configuration
		{
			[CompilerGenerated]
			get
			{
				return this.<Configuration>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Configuration>k__BackingField = value;
			}
		} = new ObservableCollection<MainPageConfigurationProxyItem>();

		// Token: 0x04000BA9 RID: 2985
		[CompilerGenerated]
		private ObservableCollection<MainPageConfigurationProxyItem> <Configuration>k__BackingField;

		// Token: 0x02000203 RID: 515
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06001A5D RID: 6749 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06001A5E RID: 6750 RVA: 0x00122E40 File Offset: 0x00121040
			internal bool <AddMissingEntries>b__0(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == this.buttonType;
			}

			// Token: 0x04000BAA RID: 2986
			public MainPageButtons buttonType;
		}
	}
}
