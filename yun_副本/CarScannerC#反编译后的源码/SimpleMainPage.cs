using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Coding;
using CarScannerXamarinForms.Coding.PagesV2;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.DTC.VagDTC;
using CarScannerXamarinForms.DTCv2;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Settings.SettingsV3;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000101 RID: 257
	[XamlFilePath("Pages\\SimpleMainPage.xaml")]
	public class SimpleMainPage : ContentPage
	{
		// Token: 0x060004F1 RID: 1265 RVA: 0x0004D9BC File Offset: 0x0004BBBC
		public SimpleMainPage()
		{
			SimpleMainPage.Instance = this;
			this.InitializeComponent();
			this.btnBuy = new ToolbarItem("", "icons8_shopping_cart.png", delegate
			{
				this.btnPurchase_Clicked(this.btnBuy, EventArgs.Empty);
			}, 0, 0);
			this.btnStats = new ToolbarItem("", "gas_station.png", delegate
			{
				this.btnFuelStatistics_Clicked(this.btnStats, EventArgs.Empty);
			}, 0, 0);
			this.btnSettings = new ToolbarItem("", (string)App.Instance.Resources["NB_settings"], delegate
			{
				this.btnSettings_Clicked(this.btnSettings, EventArgs.Empty);
			}, 0, 0);
			this.UpdateMainButtons();
			this.Initialize();
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060004F2 RID: 1266 RVA: 0x0004DA64 File Offset: 0x0004BC64
		// (set) Token: 0x060004F3 RID: 1267 RVA: 0x0004DA6B File Offset: 0x0004BC6B
		public static SimpleMainPage Instance
		{
			[CompilerGenerated]
			get
			{
				return SimpleMainPage.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				SimpleMainPage.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0004DA74 File Offset: 0x0004BC74
		internal async void btnSettings_Clicked(object sender, EventArgs e)
		{
			this.DisableMainButtons();
			SettingsRoot settingsRoot = new SettingsRoot();
			await base.Navigation.PushAsync(settingsRoot);
			this.EnableMainButtons();
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x0004DAAC File Offset: 0x0004BCAC
		internal async void btnFuelStatistics_Clicked(object sender, EventArgs e)
		{
			this.DisableMainButtons();
			FuelStatisticsPage fuelStatisticsPage = new FuelStatisticsPage();
			await base.Navigation.PushAsync(fuelStatisticsPage);
			this.EnableMainButtons();
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0004DAE4 File Offset: 0x0004BCE4
		internal async void btnPurchase_Clicked(object sender, EventArgs e)
		{
			this.DisableMainButtons();
			Page inAppPage = InAppManager.GetInAppPage();
			await base.Navigation.PushAsync(inAppPage);
			this.EnableMainButtons();
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0004DB1C File Offset: 0x0004BD1C
		internal async void btnGarage_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				this.DisableMainButtons();
				SettingsGarage settingsGarage = new SettingsGarage();
				await base.Navigation.PushAsync(settingsGarage);
				this.EnableMainButtons();
			}
			else
			{
				await base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x0004DB54 File Offset: 0x0004BD54
		private async void Initialize()
		{
			SimpleMainPage.<>c__DisplayClass12_0 CS$<>8__locals1 = new SimpleMainPage.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			App.OBDReader.StatusChanged -= this.OBD_StatusChanged;
			App.OBDReader.StatusChanged += this.OBD_StatusChanged;
			this.cbECUID.SelectedIndexChanged -= this.cbECUID_SelectedIndexChanged;
			this.cbECUID.SelectedIndexChanged += this.cbECUID_SelectedIndexChanged;
			this.ad.IsVisible = false;
			CS$<>8__locals1.requires_update = false;
			this.btnStartSimulation.IsEnabled = false;
			this.btnConnect.IsEnabled = false;
			this.btnSettings.IsEnabled = false;
			this.activityFrame.IsVisible = true;
			await Task.Run(delegate
			{
				Task.Delay(500).Wait();
				try
				{
					DashboardItem.RegisterDashboardItemTypes();
				}
				catch
				{
				}
				try
				{
					CS$<>8__locals1.<>4__this.UpdateVINInfo();
				}
				catch
				{
				}
				bool isMainThread = MainThread.IsMainThread;
				CarData currentCarData = App.OBDReader.CurrentCarData;
				if (currentCarData != null)
				{
					currentCarData.CreateEmptyPIDS();
				}
				try
				{
					LiveDataPIDModel.LoadCustomPIDS();
				}
				catch (Exception)
				{
				}
				CS$<>8__locals1.requires_update = ProfileUpdater.CheckRequiresUpdate();
				if (CS$<>8__locals1.requires_update)
				{
					if (!VersionChecker.IsVersionNewer(SharedSettings.Current.LatestVersion, "1.95.1"))
					{
						MainPageConfigurationModel mainPageConfigurationModel = new MainPageConfigurationModel();
						mainPageConfigurationModel.Configuration.FirstOrDefault((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.VersionInfo).IsVisible = true;
						mainPageConfigurationModel.Save();
						CS$<>8__locals1.<>4__this.UpdateMainButtons();
					}
					Action action;
					if ((action = CS$<>8__locals1.<>9__3) == null)
					{
						action = (CS$<>8__locals1.<>9__3 = delegate
						{
							CS$<>8__locals1.<>4__this.activityFrame.IsVisible = true;
						});
					}
					Device.BeginInvokeOnMainThread(action);
					ProfileUpdater.PerformUpdate();
					VagDTCDecoder.DeleteContainers();
					Action action2;
					if ((action2 = CS$<>8__locals1.<>9__4) == null)
					{
						action2 = (CS$<>8__locals1.<>9__4 = delegate
						{
							CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
						});
					}
					Device.BeginInvokeOnMainThread(action2);
				}
				CS$<>8__locals1.<>4__this.RemoveCSV();
			});
			this.btnStartSimulation.IsEnabled = true;
			this.btnConnect.IsEnabled = true;
			this.btnSettings.IsEnabled = true;
			this.activityFrame.IsVisible = false;
			try
			{
				this.AskForReview();
			}
			catch
			{
			}
			if (SharedSettings.Current.ConnectOnLaunch)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					CS$<>8__locals1.<>4__this.ConnectOnLaunch();
				});
			}
			Task.Run(delegate
			{
				SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d <<Initialize>b__2>d;
				<<Initialize>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Initialize>b__2>d.<>4__this = CS$<>8__locals1;
				<<Initialize>b__2>d.<>1__state = -1;
				<<Initialize>b__2>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d>(ref <<Initialize>b__2>d);
				return <<Initialize>b__2>d.<>t__builder.Task;
			});
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0004DB8C File Offset: 0x0004BD8C
		private async Task CheckForNewVersion()
		{
			await VersionChecker.CheckVersion();
			Device.BeginInvokeOnMainThread(delegate
			{
				if (VersionChecker.ShouldShowNewVersionAvailable)
				{
					this.labelNewVersion.IsVisible = true;
					return;
				}
				this.labelNewVersion.IsVisible = false;
			});
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0004DBD0 File Offset: 0x0004BDD0
		private void LabelNewVersion_Clicked(object sender, EventArgs e)
		{
			try
			{
				switch (PlatformHelper.AppMarket)
				{
				case Markets.AppStore:
				{
					string text = "1259933623";
					Device.OpenUri(new Uri(string.Format("itms-apps://itunes.apple.com/app/id{0}", text), UriKind.Absolute));
					break;
				}
				case Markets.GooglePlay:
					Device.OpenUri(new Uri("market://details?id=com.ovz.carscanner", UriKind.Absolute));
					break;
				case Markets.Rustore:
					Device.OpenUri(new Uri("https://apps.rustore.ru/app/com.ovz.carscanner.rustore", UriKind.Absolute));
					break;
				case Markets.RUS:
					Device.OpenUri(new Uri("https://ru.carscanner.info/install/", UriKind.Absolute));
					break;
				case Markets.Sideload:
					Device.OpenUri(new Uri("https://www.carscanner.info/get-apk"));
					break;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0004DC7C File Offset: 0x0004BE7C
		internal async void btnVersions_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				this.DisableMainButtons();
				await base.Navigation.PushAsync(new EcuInfoPageV2());
				this.EnableMainButtons();
			}
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0004DCB4 File Offset: 0x0004BEB4
		internal async void btnCoding_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				if (!CodingListModel.IsCodingAvailable(false))
				{
					await base.DisplayAlert(Translate.GetString("coding_ModeNotSupportedTitle"), Translate.GetString("coding_ModeNotSupportedText"), "OK");
				}
				else
				{
					this.DisableMainButtons();
					this.activityFrame.IsVisible = true;
					App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
					Page page = null;
					if (App.UseLegacyUI)
					{
						if (PlatformHelper.IsiOS)
						{
							page = new CodingModeSelection();
						}
					}
					else
					{
						page = new CodingModeSelectionV2();
					}
					RequestProducerStatic.CurrentWorkingMode = WorkingModes.DTC;
					await base.Navigation.PushAsync(page);
					this.EnableMainButtons();
					this.activityFrame.IsVisible = false;
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0004DCEB File Offset: 0x0004BEEB
		private void AdsInitialize()
		{
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
				return;
			}
			Device.BeginInvokeOnMainThread(async delegate
			{
				this.ad.IsVisible = true;
			});
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0004DD18 File Offset: 0x0004BF18
		private async void RemoveCSV()
		{
			await Task.Delay(TimeSpan.FromSeconds(5.0));
			Task.Run(delegate
			{
				Brc2CsvConverter.RemoveTemporaryCSV();
			});
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0004DD47 File Offset: 0x0004BF47
		protected override bool OnBackButtonPressed()
		{
			if (PlatformHelper.IsAndroid)
			{
				this.AskForExit();
				return true;
			}
			return base.OnBackButtonPressed();
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0004DD60 File Offset: 0x0004BF60
		private async void AskForExit()
		{
			if (PlatformHelper.IsAndroid)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("droid_Exit"), "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					SharedSettings.Current.LastException = "";
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
					{
						this.btnDisconnect_Clicked(this, new EventArgs());
					}
					Task.Run(delegate
					{
						IPlatformSpecificServiceDroid droidService2 = PlatformHelper.DroidService;
						if (droidService2 == null)
						{
							return;
						}
						droidService2.AndroidHelper_StopService();
					});
					await Task.Delay(300);
					IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
					if (droidService != null)
					{
						droidService.KillApp();
					}
				}
			}
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0004DD98 File Offset: 0x0004BF98
		private async void ConnectOnLaunch()
		{
			if (SharedSettings.Current.FirstConnectionAttempted)
			{
				try
				{
					await this.StartConnection(true);
					if (SharedSettings.Current.OpenDashboardOnLaunch && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						this.btnDashboard_Clicked(null, null);
					}
					return;
				}
				catch (Exception)
				{
					return;
				}
			}
			await this.ShowWelcomePage2();
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0004DDD0 File Offset: 0x0004BFD0
		private async Task ShowWelcomePage2()
		{
			if (App.UseLegacyUI && PlatformHelper.IsiOS)
			{
				WelcomePage2 welcomePage = new WelcomePage2();
				await base.Navigation.PushAsync(welcomePage, true);
			}
			else
			{
				WelcomePage2V3 welcomePage2V = new WelcomePage2V3();
				await base.Navigation.PushAsync(welcomePage2V, true);
			}
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0004DE14 File Offset: 0x0004C014
		private void Page_SizeChanged(object sender, EventArgs e)
		{
			if (DeviceDisplay.MainDisplayInfo.Orientation == 2)
			{
				if (this.gridBottomContainer.ColumnDefinitions.Count == 1)
				{
					this.gridBottomContainer.ColumnDefinitions.Add(new ColumnDefinition
					{
						Width = GridLength.Star
					});
					Grid.SetColumn(this.gridConnectButtons, 1);
					Grid.SetRow(this.gridConnectButtons, 1);
					Grid.SetColumnSpan(this.ad, 2);
					return;
				}
			}
			else if (this.gridBottomContainer.ColumnDefinitions.Count == 2)
			{
				Grid.SetColumn(this.gridConnectButtons, 0);
				Grid.SetRow(this.gridConnectButtons, 2);
				Grid.SetColumnSpan(this.ad, 1);
				this.gridBottomContainer.ColumnDefinitions.RemoveAt(1);
			}
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0004DED4 File Offset: 0x0004C0D4
		public async Task RefreshPIDs()
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				if (App.OBDSimulator.IsActive)
				{
					LiveDataPIDModel.GetSupportedPIDsTEST(App.OBDReader);
				}
				else
				{
					LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
				}
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0004DF10 File Offset: 0x0004C110
		private void ConnectPage_Appearing(object sender, EventArgs e)
		{
			MainAppRequestProducer.Delegate = null;
			RequestProducerStatic.CurrentWorkingMode = WorkingModes.Normal;
			if (PlatformHelper.IsiOS && SharedSettings.Current.DarkMode)
			{
				Color color = (Color)Application.Current.Resources["NavigationBarBackgroundColor"];
				Application.Current.Resources["NavigationBarBackgroundColor"] = Color.Gray;
				Application.Current.Resources["NavigationBarBackgroundColor"] = color;
			}
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
			DependencyService.Get<IStatusBar>(0).ShowStatusBar();
			if (SharedSettings.Current.AdsProductPurchased && base.ToolbarItems.Contains(this.btnBuy))
			{
				base.ToolbarItems.Remove(this.btnBuy);
			}
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				if (PlatformHelper.AppMarket == Markets.GooglePlay || PlatformHelper.AppMarket == Markets.AppStore)
				{
					DependencyService.Get<IUMPConsent>(0).DisplayConsentIfRequired();
				}
				this.ad.IsVisible = true;
			}
			base.Title = App.AppTitle;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
			{
				foreach (SensorPID sensorPID in from x in new List<PID>(App.OBDReader.CurrentCarData.LiveDataPIDs)
					where x is SensorPID
					select x as SensorPID)
				{
					sensorPID.Stop();
				}
			}
			CarPlayManager instance = CarPlayManager.Instance;
			if (instance != null)
			{
				instance.HideNonDismissableAlert();
			}
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive && !ProfileSupportedPidsTester.IsTesting)
			{
				RequestProducerStatic.UpdateOBDReaderRequests();
			}
			this.AdsInitialize();
			this.SetButtonsDependingOnOBDStatus();
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ConnectPage_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x0004E114 File Offset: 0x0004C314
		private void OBD_StatusChanged(OBDDataReaderStatus NewStatus)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.SetButtonsDependingOnOBDStatus();
			});
		}

		// Token: 0x06000508 RID: 1288 RVA: 0x0004E127 File Offset: 0x0004C327
		private void cbECUID_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && this.cbECUID.IsVisible)
			{
				this.GetPidsForEcuSelected();
			}
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0004E149 File Offset: 0x0004C349
		private void GetPidsForEcuSelected()
		{
			Device.BeginInvokeOnMainThread(async delegate
			{
				this.btnDisconnect.IsEnabled = false;
				this.btnConnect.IsEnabled = false;
				this.btnStopSimulation.IsEnabled = false;
				this.btnStartSimulation.IsEnabled = false;
				this.tbPleaseWait.IsVisible = true;
				await App.OBDReader.ChangeECU();
				await this.UpdateVINInfo();
				this.btnDisconnect.IsEnabled = true;
				this.btnConnect.IsEnabled = true;
				this.btnStopSimulation.IsEnabled = true;
				this.btnStartSimulation.IsEnabled = true;
				this.tbPleaseWait.IsVisible = false;
			});
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0004E15C File Offset: 0x0004C35C
		public async void btnConnect_Clicked(object sender, EventArgs e)
		{
			this.btnConnect.IsEnabled = false;
			if (!SharedSettings.Current.FirstConnectionAttempted || string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
			{
				await this.ShowWelcomePage2();
			}
			else
			{
				try
				{
					await this.StartConnection(true);
				}
				catch (Exception)
				{
				}
			}
			this.btnConnect.IsEnabled = true;
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0004E194 File Offset: 0x0004C394
		public async void btnDisconnect_Clicked(object sender, EventArgs e)
		{
			this.btnDisconnect.IsVisible = false;
			this.btnConnect.IsVisible = true;
			this.activityFrame.IsVisible = true;
			this.connectionProgressFrame.IsVisible = false;
			if (PlatformHelper.IsAndroid)
			{
				IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
				if (droidService != null)
				{
					droidService.AndroidHelper_StopService();
				}
			}
			App.OBDReader.DisconnectRequested = true;
			await App.OBDReader.Disconnect("UserClickbtnDisconnect");
			DataRecorderV2.StopRecording(OBDDataReaderStatus.Disconnected);
			this.btnDisconnect.IsVisible = false;
			this.connectionProgressFrame.IsVisible = false;
			this.activityFrame.IsVisible = false;
			this.btnDisconnect.IsEnabled = true;
			BluetoothHelper.TurnOffBluetoothIfNeedTo();
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0004E1CC File Offset: 0x0004C3CC
		private async void btnStartSimulation_Clicked(object sender, EventArgs e)
		{
			Task pid_load = null;
			bool flag = !string.IsNullOrEmpty(SharedSettings.Current.LastCarAvailableSensors);
			if (flag)
			{
				flag = await base.DisplayAlert(Translate.GetString("ios_DemoModeSelectorTitle"), Translate.GetString("ios_DemoModeSelectorText"), Translate.GetString("ios_DemoModeLastCar"), Translate.GetString("ios_DemoModeAllSensors"));
			}
			if (flag)
			{
				this.activityFrame.IsVisible = true;
				if (pid_load != null)
				{
					await Task.WhenAny(new Task[] { pid_load });
				}
				App.OBDSimulator.Start(true);
				this.activityFrame.IsVisible = false;
			}
			else
			{
				this.activityFrame.IsVisible = true;
				if (pid_load != null)
				{
					await Task.WhenAny(new Task[] { pid_load });
				}
				App.OBDSimulator.Start(false);
				this.activityFrame.IsVisible = false;
			}
			LiveDataPIDModel.GetSupportedPIDsTEST(App.OBDReader);
			App.OBDReader.SetStatusForTest(OBDDataReaderStatus.ConnectedToECU);
			this.UpdateVINInfo();
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0004E203 File Offset: 0x0004C403
		private void btnStopSimulation_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
				if (droidService != null)
				{
					droidService.AndroidHelper_StopService();
				}
			}
			App.OBDReader.SetStatusForTest(OBDDataReaderStatus.Disconnected);
			App.OBDSimulator.Stop();
			LiveDataPIDModel._PIDCollection.Clear();
			this.UpdateVINInfo();
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0004E244 File Offset: 0x0004C444
		private void SetButtonsDependingOnOBDStatus()
		{
			if (!MainThread.IsMainThread)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					this.SetButtonsDependingOnOBDStatus();
				});
				return;
			}
			try
			{
				switch (App.OBDReader.CurrentStatus)
				{
				case OBDDataReaderStatus.Disconnected:
					this.gridVIN.IsVisible = false;
					this.tbProto.IsVisible = false;
					this.tbProtoValue.IsVisible = false;
					this.cbECUID.IsEnabled = false;
					this.btnConnect.IsEnabled = true;
					this.btnStartSimulation.IsEnabled = true;
					this.btnDisconnect.BackgroundColor = Color.Red;
					this.btnDisconnect.IsEnabled = true;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.btnConnect.IsVisible = true;
					this.btnDisconnect.IsVisible = false;
					this.btnStartSimulation.IsVisible = true;
					this.btnStopSimulation.IsVisible = false;
					this.btnDisconnect.IsEnabled = true;
					if (SharedSettings.Current.ShowCodingAndService)
					{
						if (this.MainButtonsGrid.Children.Any((View x) => x is MainPageButton && (x as MainPageButton).ButtonType == MainPageButtons.Coding))
						{
							this.UpdateMainButtons();
						}
					}
					break;
				case OBDDataReaderStatus.ConnectingToELM:
					this.gridVIN.IsVisible = false;
					this.tbProto.IsVisible = false;
					this.tbProtoValue.IsVisible = false;
					this.cbECUID.IsEnabled = false;
					this.btnDisconnect.BackgroundColor = Color.Red;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusConnecting");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["YellowTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.btnConnect.IsVisible = false;
					this.btnDisconnect.IsVisible = true;
					this.btnStartSimulation.IsVisible = false;
					this.btnStopSimulation.IsVisible = false;
					this.btnDisconnect.IsEnabled = true;
					break;
				case OBDDataReaderStatus.ConnectedToELM:
					this.gridVIN.IsVisible = false;
					this.tbProto.IsVisible = false;
					this.tbProtoValue.IsVisible = false;
					this.cbECUID.IsEnabled = false;
					this.btnDisconnect.BackgroundColor = Color.Red;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusConnected");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["GreenTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.btnConnect.IsVisible = false;
					this.btnDisconnect.IsVisible = true;
					this.btnStartSimulation.IsVisible = false;
					this.btnStopSimulation.IsVisible = false;
					this.btnDisconnect.IsEnabled = true;
					break;
				case OBDDataReaderStatus.ConnectingToECU:
					this.gridVIN.IsVisible = true;
					this.tbProto.IsVisible = true;
					this.tbProtoValue.IsVisible = true;
					this.cbECUID.IsEnabled = false;
					this.btnDisconnect.BackgroundColor = Color.Red;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusConnected");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["GreenTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusConnecting");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["YellowTextColor"];
					this.btnConnect.IsVisible = false;
					this.btnDisconnect.IsVisible = true;
					this.btnStartSimulation.IsVisible = false;
					this.btnStopSimulation.IsVisible = false;
					this.btnDisconnect.IsEnabled = true;
					break;
				case OBDDataReaderStatus.ConnectedToECU:
					this.gridVIN.IsVisible = true;
					this.tbProto.IsVisible = true;
					this.tbProtoValue.IsVisible = true;
					this.cbECUID.IsEnabled = true;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusConnected");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["GreenTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusConnected");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["GreenTextColor"];
					if (App.OBDSimulator.IsActive)
					{
						this.btnDisconnect.BackgroundColor = Color.Red;
						this.btnDisconnect.IsEnabled = true;
						this.btnDisconnect.IsVisible = false;
						this.btnStopSimulation.IsVisible = true;
						this.btnConnect.IsVisible = false;
						this.btnStartSimulation.IsVisible = false;
					}
					else
					{
						this.btnDisconnect.BackgroundColor = Color.Red;
						this.btnDisconnect.IsEnabled = true;
						this.btnDisconnect.IsVisible = true;
						this.btnStopSimulation.IsVisible = false;
						this.btnConnect.IsVisible = false;
						this.btnStartSimulation.IsVisible = false;
						if (SharedSettings.Current.ShowCodingAndService && CodingListModel.IsCodingAvailable(false))
						{
							this.UpdateMainButtons();
						}
					}
					break;
				case OBDDataReaderStatus.Disconnecting:
					this.gridVIN.IsVisible = false;
					this.tbProto.IsVisible = false;
					this.tbProtoValue.IsVisible = false;
					this.cbECUID.IsEnabled = false;
					this.btnDisconnect.BackgroundColor = Color.Red;
					this.tbELMStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbELMStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.tbECUStatus.Text = Translate.GetString("MainPage_StatusDisconnected");
					this.tbECUStatus.TextColor = (Color)Application.Current.Resources["RedTextColor"];
					this.btnConnect.IsVisible = false;
					this.btnDisconnect.IsVisible = true;
					this.btnStartSimulation.IsVisible = false;
					this.btnStopSimulation.IsVisible = false;
					this.btnDisconnect.IsEnabled = false;
					break;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0004E948 File Offset: 0x0004CB48
		public static void ShowActivityFrame()
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.IsVisible = true;
				}
			});
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0004E96E File Offset: 0x0004CB6E
		public static void HideActivityFrame()
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.IsVisible = false;
				}
			});
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0004E994 File Offset: 0x0004CB94
		public static void SetActivityFrameText(string text)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.Text = text;
				}
			});
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0004E9B4 File Offset: 0x0004CBB4
		public async Task StartConnection(bool InitECU = true)
		{
			SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = new SimpleMainPage.<>c__DisplayClass38_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.InitECU = InitECU;
			TaskAwaiter<PermissionStatus> taskAwaiter3;
			TaskAwaiter<PermissionStatus> taskAwaiter4;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				this.connectionProgressFrame.ResetTextToDefault();
				this.connectionProgressFrame.IsCancelVisible = false;
				if (!VersionChecker.IsValidVersion)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("main_VersionOutdatedTitle"), Translate.GetString("main_VersionOutdatedText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.LabelNewVersion_Clicked(this.labelNewVersion, EventArgs.Empty);
					}
				}
				else
				{
					CS$<>8__locals1.wifi_name = "";
					App.OBDReader.DisconnectRequested = false;
					App.OBDReader.stopwatch.Restart();
					SharedSettings.Current.OptimizedRequestStuckCounter = 0;
					if (PlatformHelper.IsAndroid && (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE || SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth) && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
					{
						taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							await taskAwaiter3;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<PermissionStatus>);
						}
						if (taskAwaiter3.GetResult() != 3)
						{
							await base.Navigation.PushAsync(new DroidPermissionRequestPage(true, false));
							return;
						}
					}
					IL_036A:
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
					{
						try
						{
							await base.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK");
							if (App.GetCurrentPage() == this)
							{
								await base.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true);
							}
						}
						catch
						{
						}
					}
					else if ((SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
					{
						try
						{
							await base.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK");
							if (App.GetCurrentPage() == this)
							{
								await base.Navigation.PushAsync(new BTDeviceSelectorPage(), true);
							}
						}
						catch
						{
						}
					}
					else
					{
						if (PlatformHelper.IsiOS)
						{
							try
							{
								if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
								{
									await base.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK");
									PlatformHelper.IOSService.OpenSystemSettings();
									return;
								}
							}
							catch (Exception)
							{
							}
							try
							{
								if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
								{
									PlatformHelper.IOSService.LocalNetworkPermissionService_EasyWayRequest();
								}
							}
							catch (Exception)
							{
							}
						}
						if (PlatformHelper.IsAndroid)
						{
							if (SharedSettings.Current.CheckBluetoothTurnedOn && (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE))
							{
								this.connectionProgressFrame.Text = Translate.GetString("droid_TurningOnBluetooth");
								this.connectionProgressFrame.IsVisible = true;
								if (SharedSettings.Current.DroidTryTurnOnBluetooth && !DependencyService.Get<IBluetooth2Manager>(0).IsOn)
								{
									await PlatformHelper.DroidService.AndroidHelper_RequestBluetoothPowerOn();
									BluetoothHelper.BluetoothWasTurnedOnByTheApp = true;
								}
							}
							else
							{
								BluetoothHelper.BluetoothWasTurnedOnByTheApp = false;
							}
						}
						CS$<>8__locals1.wifiHelper = DependencyService.Get<IWiFiHelper>(0);
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
						{
							try
							{
								CS$<>8__locals1.wifi_name = await this.CheckWiFiConnection(CS$<>8__locals1.wifiHelper);
							}
							catch (Exception)
							{
								CS$<>8__locals1.wifi_name = "<unknown ssid>";
							}
							this.connectionProgressFrame.IsVisible = false;
						}
						if (SharedSettings.Current.ForceProfileUpdateScheduled || (SharedSettings.Current.ProfileHasPids && CustomPIDViewModel.CurrentProfile.PidCollection.Count == 0))
						{
							this.connectionProgressFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							await Task.Run(delegate
							{
								ProfileUpdater.ForceUpdate();
							});
						}
						await MainThread.InvokeOnMainThreadAsync(delegate
						{
							CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = Translate.GetString("ios_ConnectingToELM");
							if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
							{
								if (string.IsNullOrEmpty(CS$<>8__locals1.wifi_name))
								{
									if (CS$<>8__locals1.wifiHelper.HasLocationPermission())
									{
										CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("ios_NoWiFi");
									}
								}
								else
								{
									CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nWiFi: " + CS$<>8__locals1.wifi_name;
								}
								if (PlatformHelper.IsAndroid)
								{
									CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("droid_DisableMobileData");
								}
							}
							else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
							{
								if (!string.IsNullOrEmpty(SharedSettings.Current.BTDeviceName))
								{
									CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTDeviceName;
								}
							}
							else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && !string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
							{
								CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTLEDeviceName;
							}
							CS$<>8__locals1.<>4__this.connectionProgressFrame.IsVisible = true;
						});
						CS$<>8__locals1.connected = false;
						OBDDataReader obdreader = App.OBDReader;
						if (obdreader != null)
						{
							ELMState elmstatus = obdreader.ELMStatus;
							if (elmstatus != null)
							{
								elmstatus.ResetErrorsState();
							}
						}
						try
						{
							if (SharedSettings.Current.SpeedCalibrationTaskPending && SpeedCalibrationModelV2.Instance == null)
							{
								SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
							}
							OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
							await Task.Run(delegate
							{
								SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__2>d <<StartConnection>b__2>d;
								<<StartConnection>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
								<<StartConnection>b__2>d.<>4__this = CS$<>8__locals1;
								<<StartConnection>b__2>d.<>1__state = -1;
								<<StartConnection>b__2>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__2>d>(ref <<StartConnection>b__2>d);
								return <<StartConnection>b__2>d.<>t__builder.Task;
							}).ConfigureAwait(true);
						}
						catch (Exception)
						{
						}
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && !CS$<>8__locals1.connected && SharedSettings.Current.SearchForBTLEIfConnectionFailed && !App.OBDReader.DisconnectRequested)
						{
							TaskAwaiter<bool> taskAwaiter = BTLEDeviceSelectorViewModel.FindDeviceWithRandomizedUUID().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter.GetResult())
							{
								try
								{
									OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
									await Task.Run(delegate
									{
										SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__4>d <<StartConnection>b__4>d;
										<<StartConnection>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
										<<StartConnection>b__4>d.<>4__this = CS$<>8__locals1;
										<<StartConnection>b__4>d.<>1__state = -1;
										<<StartConnection>b__4>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__4>d>(ref <<StartConnection>b__4>d);
										return <<StartConnection>b__4>d.<>t__builder.Task;
									}).ConfigureAwait(true);
								}
								catch (Exception)
								{
								}
							}
						}
						string text = "";
						switch (SharedSettings.Current.ConnectionType)
						{
						case ConnectionTypes.WiFi:
							text = SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
							break;
						case ConnectionTypes.BluetoothLE:
							text = SharedSettings.Current.BTLEDeviceID;
							break;
						case ConnectionTypes.Bluetooth:
							text = SharedSettings.Current.BTDeviceID;
							break;
						}
						ScanXChecker.CheckDeviceAtConnection(text);
						this.connectionProgressFrame.IsVisible = false;
						if (CS$<>8__locals1.connected && !App.OBDReader.DisconnectRequested)
						{
							if (SharedSettings.Current.CheckIsELMWhenConnecting && SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
							{
								if (PlatformHelper.IsAndroid)
								{
									if (CS$<>8__locals1.wifi_name != "<unknown ssid>")
									{
										SharedSettings.Current.LastWiFiName = CS$<>8__locals1.wifi_name;
									}
								}
								else
								{
									SharedSettings.Current.LastWiFiName = CS$<>8__locals1.wifi_name;
								}
							}
							if (CS$<>8__locals1.InitECU)
							{
								SimpleMainPage.<>c__DisplayClass38_1 CS$<>8__locals2 = new SimpleMainPage.<>c__DisplayClass38_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								if (App.OBDReader.DisconnectRequested)
								{
									return;
								}
								this.connectionProgressFrame.Text = Translate.GetString("ios_ConnectingToECU");
								this.connectionProgressFrame.IsVisible = true;
								CS$<>8__locals2.initprogress = new Progress<string>();
								CS$<>8__locals2.initprogress.ProgressChanged += this.Initprogress_ProgressChanged;
								CS$<>8__locals2.attempts = 3;
								if (SharedSettings.Current.ProtocolNumber == 0 && SharedSettings.Current.UseDefaultInit)
								{
									CS$<>8__locals2.attempts = 2;
								}
								CarInfoViewModel.Instance.Reset();
								CS$<>8__locals2.initResult = false;
								await Task.Run(delegate
								{
									SimpleMainPage.<>c__DisplayClass38_1.<<StartConnection>b__5>d <<StartConnection>b__5>d;
									<<StartConnection>b__5>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<StartConnection>b__5>d.<>4__this = CS$<>8__locals2;
									<<StartConnection>b__5>d.<>1__state = -1;
									<<StartConnection>b__5>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_1.<<StartConnection>b__5>d>(ref <<StartConnection>b__5>d);
									return <<StartConnection>b__5>d.<>t__builder.Task;
								}).ConfigureAwait(true);
								if (CS$<>8__locals2.initResult)
								{
									SharedSettings.Current.FreePeriodGoodConnections++;
									if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
									{
										if (PlatformHelper.IsAndroid)
										{
											if (CS$<>8__locals2.CS$<>8__locals1.wifi_name != "<unknown ssid>")
											{
												SharedSettings.Current.LastWiFiName = CS$<>8__locals2.CS$<>8__locals1.wifi_name;
											}
										}
										else
										{
											SharedSettings.Current.LastWiFiName = CS$<>8__locals2.CS$<>8__locals1.wifi_name;
										}
									}
									if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidStartBackgroundService)
									{
										IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
										if (droidService != null)
										{
											droidService.AndroidHelper_StartService();
										}
									}
									this.connectionProgressFrame.IsVisible = false;
									await this.UpdateVINInfo();
									if (SharedSettings.Current.ShouldCheckProfilePIDs && !SharedSettings.Current.IgnoreProfilePidsTestScheduled && (App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit))
									{
										SimpleMainPage.<>c__DisplayClass38_2 CS$<>8__locals3 = new SimpleMainPage.<>c__DisplayClass38_2();
										CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
										CS$<>8__locals3.detectingString = Translate.GetString("profile_CheckingSupportedPids");
										this.connectionProgressFrame.Text = CS$<>8__locals3.detectingString;
										this.connectionProgressFrame.CancelText = Translate.GetString("ios_Skip");
										this.connectionProgressFrame.IsCancelVisible = true;
										CS$<>8__locals3.cts = new CancellationTokenSource();
										EventHandler eventHandler = delegate(object cancelSender, EventArgs cancelArgs)
										{
											SimpleMainPage.<>c__DisplayClass38_2.<<StartConnection>b__8>d <<StartConnection>b__8>d;
											<<StartConnection>b__8>d.<>t__builder = AsyncVoidMethodBuilder.Create();
											<<StartConnection>b__8>d.<>4__this = CS$<>8__locals3;
											<<StartConnection>b__8>d.<>1__state = -1;
											<<StartConnection>b__8>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_2.<<StartConnection>b__8>d>(ref <<StartConnection>b__8>d);
										};
										ActivityFrame activityFrame = this.connectionProgressFrame;
										activityFrame.CancelClicked = (EventHandler)Delegate.Remove(activityFrame.CancelClicked, eventHandler);
										ActivityFrame activityFrame2 = this.connectionProgressFrame;
										activityFrame2.CancelClicked = (EventHandler)Delegate.Combine(activityFrame2.CancelClicked, eventHandler);
										this.connectionProgressFrame.IsVisible = true;
										IProgress<int> progress = new Progress<int>(delegate(int i)
										{
											MainThreadHelper.InvokeOnMainThread(delegate
											{
												CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = string.Concat(new string[]
												{
													CS$<>8__locals3.detectingString,
													"\n",
													SharedSettings.Current.SelectedBrand,
													" ",
													SharedSettings.Current.SelectedProfileV2Name,
													"\n",
													i.ToString(),
													"%"
												});
											});
										});
										this.DisableMainButtons();
										string text2 = await ProfileSupportedPidsTester.PerformTest(progress, CS$<>8__locals3.cts.Token);
										this.EnableMainButtons();
										this.connectionProgressFrame.IsVisible = false;
										this.connectionProgressFrame.ResetTextToDefault();
										this.connectionProgressFrame.IsCancelVisible = false;
										if (!string.IsNullOrEmpty(text2))
										{
											await base.DisplayAlert(Translate.GetString("profile_PidDetectionInterruptedTitle"), text2, "OK");
										}
									}
									DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
									if (recorder != null)
									{
										recorder.Save();
									}
									if (SharedSettings.Current.RecordData)
									{
										DataRecorderV2.StartRecording();
									}
									else
									{
										App.OBDReader.CurrentCarData.Recorder = null;
									}
									RequestProducerStatic.UpdateOBDReaderRequests();
									if ((App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit || App.OBDReader.CurrentELMFormat == ELMFormat.CAN29bit) && !SharedSettings.Current.CANOptimizationWarningShowed && !SharedSettings.Current.CANOptimizeRequests && SharedSettings.Current.FreePeriodGoodConnections > 1 && !(SharedSettings.Current.SelectedBrand == "BYD"))
									{
										SharedSettings.Current.CANOptimizationWarningShowed = true;
										await MainThread.InvokeOnMainThreadAsync(delegate
										{
											SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__6>d <<StartConnection>b__6>d;
											<<StartConnection>b__6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
											<<StartConnection>b__6>d.<>4__this = CS$<>8__locals2.CS$<>8__locals1;
											<<StartConnection>b__6>d.<>1__state = -1;
											<<StartConnection>b__6>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__6>d>(ref <<StartConnection>b__6>d);
											return <<StartConnection>b__6>d.<>t__builder.Task;
										});
									}
									if (SharedSettings.Current.UseRPMFix && !SharedSettings.Current.RPMFixWarningShowed)
									{
										SharedSettings.Current.RPMFixWarningShowed = true;
										await MainThread.InvokeOnMainThreadAsync(delegate
										{
											SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__7>d <<StartConnection>b__7>d;
											<<StartConnection>b__7>d.<>t__builder = AsyncTaskMethodBuilder.Create();
											<<StartConnection>b__7>d.<>4__this = CS$<>8__locals2.CS$<>8__locals1;
											<<StartConnection>b__7>d.<>1__state = -1;
											<<StartConnection>b__7>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__7>d>(ref <<StartConnection>b__7>d);
											return <<StartConnection>b__7>d.<>t__builder.Task;
										});
									}
								}
								CS$<>8__locals2 = null;
							}
							else
							{
								App.OBDReader.Start("MainPage->StartConnection/InitECUFailed");
							}
							this.connectionProgressFrame.IsVisible = false;
						}
						this.connectionProgressFrame.IsVisible = false;
						await MainThread.InvokeOnMainThreadAsync(delegate
						{
							SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d <<StartConnection>b__3>d;
							<<StartConnection>b__3>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<StartConnection>b__3>d.<>4__this = CS$<>8__locals1;
							<<StartConnection>b__3>d.<>1__state = -1;
							<<StartConnection>b__3>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref <<StartConnection>b__3>d);
							return <<StartConnection>b__3>d.<>t__builder.Task;
						});
					}
				}
			}
			return;
			taskAwaiter3 = taskAwaiter4;
			taskAwaiter4 = default(TaskAwaiter<PermissionStatus>);
			if (taskAwaiter3.GetResult() == 3)
			{
				goto IL_036A;
			}
			await base.DisplayAlert(Translate.GetString("droid_Android12BluetoothPermissionMissing_Title"), Translate.GetString("droid_NearbyDevicesExplanation") + "\n" + Translate.GetString("droid_Android12BluetoothPermissionMissing_Text"), "OK");
			PermissionHelper.OpenPermissionsSettings();
		}

		// Token: 0x06000513 RID: 1299 RVA: 0x0004EA00 File Offset: 0x0004CC00
		private async Task<string> CheckWiFiConnection(IWiFiHelper wifiHelper)
		{
			string wifi_name = wifiHelper.GetWiFiName();
			string text;
			if (SharedSettings.Current.NoWiFiWarning || !SharedSettings.Current.TryConnectToLastWiFiNetwork)
			{
				text = wifi_name;
			}
			else
			{
				if (wifiHelper.HasLocationPermission() && string.IsNullOrEmpty(wifi_name) && string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName))
				{
					if (SharedSettings.Current.NoWiFiWarning)
					{
						await base.DisplayAlert(Translate.GetString("ios_NoWiFiWarning_Title"), Translate.GetString("ios_NoWiFiWarning_Text"), "OK");
					}
				}
				else if (string.IsNullOrEmpty(wifi_name) && !string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) && SharedSettings.Current.TryConnectToLastWiFiNetwork)
				{
					this.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
					this.connectionProgressFrame.IsVisible = true;
					TaskAwaiter<bool> taskAwaiter = wifiHelper.ConnectToNetwork(SharedSettings.Current.LastWiFiName).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						await base.DisplayAlert(Translate.GetString("ios_NoWiFiWarning_Title"), Translate.GetString("ios_NoWiFiWarning_Text"), "OK");
					}
				}
				else if (!string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) && !string.IsNullOrEmpty(wifi_name) && wifi_name != "<unknown ssid>" && SharedSettings.Current.LastWiFiName != wifi_name)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ChangeWiFiQuestion"), wifi_name, SharedSettings.Current.LastWiFiName), Translate.GetString("ios_ChangeWiFiYes"), Translate.GetString("ios_ChangeWiFiNo")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
						this.connectionProgressFrame.IsVisible = true;
						await wifiHelper.ConnectToNetwork(SharedSettings.Current.LastWiFiName);
						wifi_name = wifiHelper.GetWiFiName();
					}
				}
				else if (!string.IsNullOrEmpty(wifi_name) && wifi_name != "<unknown ssid>" && string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) && !wifi_name.Contains("obd", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("vlink", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("vgate", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("v-link", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("konnwei", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("nexpeak", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("scantool", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("elm", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("viecar", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("carly", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("obdclick", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("kiwi", StringComparison.InvariantCultureIgnoreCase) && !wifi_name.Contains("ancel", StringComparison.InvariantCultureIgnoreCase))
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_WrongNetwork"), wifi_name, SharedSettings.Current.LastWiFiName), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
						this.connectionProgressFrame.IsVisible = true;
						await wifiHelper.ConnectToNetwork("WiFi_OBDII");
						wifi_name = wifiHelper.GetWiFiName();
					}
				}
				text = wifi_name;
			}
			return text;
		}

		// Token: 0x06000514 RID: 1300 RVA: 0x0004EA4B File Offset: 0x0004CC4B
		private void Initprogress_ProgressChanged(object sender, string e)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.connectionProgressFrame.Text = e;
			});
		}

		// Token: 0x06000515 RID: 1301 RVA: 0x0004EA70 File Offset: 0x0004CC70
		private async Task AskForReview()
		{
			if (!SharedSettings.Current.ReviewReceived && SharedSettings.Current.FreePeriodGoodConnections > SharedSettings.Current.AskForReviewGoodConnections)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_AskForRating_Title"), Translate.GetString("ios_AboutRatings"), Translate.GetString("ios_Yes"), Translate.GetString("ios_Later")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					SharedSettings.Current.ReviewReceived = true;
					try
					{
						DependencyService.Get<IRequestReview>(0).Request(false);
						return;
					}
					catch (Exception)
					{
						return;
					}
				}
				SharedSettings.Current.AskForReviewGoodConnections += 12;
			}
		}

		// Token: 0x06000516 RID: 1302 RVA: 0x0004EAB4 File Offset: 0x0004CCB4
		public async Task<bool> UpdateVINInfo()
		{
			bool flag;
			if (!SharedSettings.Current.UseOBD2)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					this.gridVIN.IsVisible = true;
				});
				flag = false;
			}
			else
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					this.gridVIN.IsVisible = true;
				});
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
				{
					if (App.OBDReader.CurrentProtocolNumber < 3)
					{
						flag = false;
					}
					else if (SharedSettings.Current.RequestECUInfo)
					{
						await CarInfoViewModel.Instance.Load(false);
						flag = true;
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06000517 RID: 1303 RVA: 0x0004EAF8 File Offset: 0x0004CCF8
		public bool CheckConnected(bool terminal = false)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectingToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectingToELM || App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnecting)
			{
				base.DisplayAlert(Translate.GetString("ios_ConnectionInProgressTitle"), Translate.GetString("ios_ConnectionInProgressText"), "OK");
				return false;
			}
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
			{
				if (terminal)
				{
					return true;
				}
				base.DisplayAlert(Translate.GetString("ios_ConnectedToElmOnlyTitle"), Translate.GetString("ios_ConnectedToElmOnlyText"), "OK");
				return false;
			}
			else
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
				{
					base.DisplayAlert(Translate.GetString("ios_DisconnectedFromELMTitle"), Translate.GetString("ios_DisconnectedFromELMText"), "OK");
					return false;
				}
				return true;
			}
		}

		// Token: 0x06000518 RID: 1304 RVA: 0x0004EBB4 File Offset: 0x0004CDB4
		internal async void btnTerminal_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
			{
				this.DisableMainButtons();
				TerminalPage terminalPage = new TerminalPage();
				await base.Navigation.PushAsync(terminalPage);
				this.EnableMainButtons();
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ConnectToELMOnlyTitle"), Translate.GetString("ios_ConnectToELMOnlyText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					await SimpleMainPage.Instance.StartConnection(false);
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
					{
						await App.GetCurrentPage().Navigation.PopAsync();
					}
				}
			}
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x0004EBEC File Offset: 0x0004CDEC
		internal async void btnLiveData_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				this.DisableMainButtons();
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				int chartsView = SharedSettings.Current.ChartsView;
				if (chartsView != 1)
				{
					if (chartsView != 2)
					{
						TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_LiveDataMode"), Translate.GetString("ios_LiveDataMode_Text"), Translate.GetString("ios_LiveDataMode_Combined"), Translate.GetString("ios_LiveDataMode_Separate")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							await this.ShowCombinedChart();
						}
						else
						{
							await this.ShowSeparateCharts();
						}
					}
					else
					{
						await this.ShowSeparateCharts();
					}
				}
				else
				{
					await this.ShowCombinedChart();
				}
				this.EnableMainButtons();
			}
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x0004EC24 File Offset: 0x0004CE24
		private async Task ShowSeparateCharts()
		{
			this.DisableMainButtons();
			this.activityFrame.IsVisible = true;
			await Task.Delay(100);
			await base.Navigation.PushAsync(new LiveDataChartPage());
			this.activityFrame.IsVisible = false;
			this.EnableMainButtons();
		}

		// Token: 0x0600051B RID: 1307 RVA: 0x0004EC68 File Offset: 0x0004CE68
		private async Task ShowCombinedChart()
		{
			this.DisableMainButtons();
			this.activityFrame.IsVisible = true;
			await Task.Delay(100);
			await base.Navigation.PushAsync(new LiveDataMultiChartSelector());
			this.activityFrame.IsVisible = false;
			this.EnableMainButtons();
		}

		// Token: 0x0600051C RID: 1308 RVA: 0x0004ECAC File Offset: 0x0004CEAC
		internal async void btnLiveDataTable_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				this.DisableMainButtons();
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				LiveDataListPage liveDataListPage = new LiveDataListPage();
				this.activityFrame.IsVisible = true;
				await base.Navigation.PushAsync(liveDataListPage);
				this.EnableMainButtons();
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x0600051D RID: 1309 RVA: 0x0004ECE4 File Offset: 0x0004CEE4
		internal async void btnDTC_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				this.DisableMainButtons();
				this.activityFrame.IsVisible = true;
				bool flag = false;
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && ((SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCReadingSequence)) || (SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCClearingSequence))))
				{
					flag = true;
				}
				RequestProducerStatic.CurrentWorkingMode = WorkingModes.DTC;
				if (!App.OBDSimulator.IsActive && DTCv2Model.IsDTCv2Available && !flag)
				{
					if (App.UseLegacyUI)
					{
						DTCv2Page dtcv2page = new DTCv2Page();
						await dtcv2page.LoadModel();
						await base.Navigation.PushAsync(dtcv2page);
						dtcv2page = null;
					}
					else
					{
						DTCv3Page dtcv3page = new DTCv3Page();
						await dtcv3page.LoadModel();
						await base.Navigation.PushAsync(dtcv3page);
						dtcv3page = null;
					}
				}
				else
				{
					Page page = new DTCWizardStartPage();
					await base.Navigation.PushAsync(page);
				}
				this.EnableMainButtons();
				this.activityFrame.IsVisible = false;
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance != null)
				{
					instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInDTC"));
				}
			}
		}

		// Token: 0x0600051E RID: 1310 RVA: 0x0004ED1C File Offset: 0x0004CF1C
		internal async void btnMode06_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				this.DisableMainButtons();
				View v = sender as View;
				if (v != null)
				{
					await ViewExtensions.FadeTo(v, 0.5, 100U, null);
				}
				await base.Navigation.PushAsync(new Mode06Page());
				if (v != null)
				{
					await ViewExtensions.FadeTo(v, 1.0, 100U, null);
				}
				this.EnableMainButtons();
				v = null;
			}
		}

		// Token: 0x0600051F RID: 1311 RVA: 0x0004ED5C File Offset: 0x0004CF5C
		internal async void btnDashboard_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				this.DisableMainButtons();
				this.activityFrame.IsVisible = true;
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				DashboardXamlPage dashboardXamlPage = new DashboardXamlPage();
				await base.Navigation.PushAsync(dashboardXamlPage);
				this.EnableMainButtons();
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x06000520 RID: 1312 RVA: 0x0004ED94 File Offset: 0x0004CF94
		internal async void btnFreezeFrame_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				this.DisableMainButtons();
				FreezeFramePage freezeFramePage = new FreezeFramePage();
				await base.Navigation.PushAsync(freezeFramePage);
				this.EnableMainButtons();
			}
		}

		// Token: 0x06000521 RID: 1313 RVA: 0x0004EDCC File Offset: 0x0004CFCC
		internal async void btnSpeedTest_Clicked(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				this.DisableMainButtons();
				SpeedTestPage speedTestPage = new SpeedTestPage();
				await base.Navigation.PushAsync(speedTestPage);
				this.EnableMainButtons();
			}
		}

		// Token: 0x06000522 RID: 1314 RVA: 0x0004EE04 File Offset: 0x0004D004
		internal async void btnRecords_Clicked(object sender, EventArgs e)
		{
			this.DisableMainButtons();
			SettingsRecording settingsRecording = new SettingsRecording();
			await base.Navigation.PushAsync(settingsRecording);
			this.EnableMainButtons();
		}

		// Token: 0x06000523 RID: 1315 RVA: 0x0004EE3C File Offset: 0x0004D03C
		internal async void btnEmissionTests_Tapped(object sender, EventArgs e)
		{
			if (this.CheckConnected(false))
			{
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				this.DisableMainButtons();
				Page page;
				if (App.UseLegacyUI)
				{
					page = new EcoTestPage();
				}
				else
				{
					page = new EcoTestPageV2();
				}
				await base.Navigation.PushAsync(page);
				this.EnableMainButtons();
			}
		}

		// Token: 0x06000524 RID: 1316 RVA: 0x0004EE74 File Offset: 0x0004D074
		public void UpdateMainButtons()
		{
			try
			{
				if (!MainThread.IsMainThread)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.UpdateMainButtons();
					});
				}
				else
				{
					MainPageConfigurationModel mainPageConfigurationModel = new MainPageConfigurationModel();
					this.MainButtonsGrid.Children.Clear();
					List<MainPageConfigurationProxyItem> list = mainPageConfigurationModel.Configuration.Where((MainPageConfigurationProxyItem x) => x.IsVisible).ToList<MainPageConfigurationProxyItem>();
					if (CodingListModel.IsCodingAvailable(false) && SharedSettings.Current.ShowCodingAndService)
					{
						list.Add(new MainPageConfigurationProxyItem(MainPageButtons.Coding));
					}
					if (!SharedSettings.Current.AdsProductPurchased || (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated))
					{
						base.Title = App.AppTitle;
						if (!list.Any((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.Purchase))
						{
							if (!base.ToolbarItems.Contains(this.btnBuy))
							{
								base.ToolbarItems.Insert(0, this.btnBuy);
							}
						}
						else if (base.ToolbarItems.Contains(this.btnBuy))
						{
							base.ToolbarItems.Remove(this.btnBuy);
						}
					}
					else
					{
						base.Title = App.AppTitle;
						if (base.ToolbarItems.Contains(this.btnBuy))
						{
							base.ToolbarItems.Remove(this.btnBuy);
						}
						MainPageConfigurationProxyItem mainPageConfigurationProxyItem = list.FirstOrDefault((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.Purchase);
						if (mainPageConfigurationProxyItem != null)
						{
							list.Remove(mainPageConfigurationProxyItem);
						}
					}
					this.AdsInitialize();
					int count = list.Count;
					int num = count / 3;
					if (count % 3 > 0)
					{
						num++;
					}
					if (this.MainButtonsGrid.RowDefinitions.Count != num)
					{
						this.MainButtonsGrid.RowDefinitions.Clear();
						for (int i = 0; i < num; i++)
						{
							RowDefinition rowDefinition = new RowDefinition
							{
								Height = GridLength.Auto
							};
							this.MainButtonsGrid.RowDefinitions.Add(rowDefinition);
						}
					}
					for (int j = 0; j < count; j++)
					{
						int num2 = j / 3;
						int num3 = j - num2 * 3;
						try
						{
							MainPageButton mainPageButton = MainPageConfigurationModel.BuildButton(list[j].ButtonType);
							Grid.SetRow(mainPageButton, num2);
							Grid.SetColumn(mainPageButton, num3);
							this.MainButtonsGrid.Children.Add(mainPageButton);
						}
						catch (Exception)
						{
						}
					}
					if (!list.Any((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.FuelStatistics) && !base.ToolbarItems.Contains(this.btnStats))
					{
						base.ToolbarItems.Add(this.btnStats);
					}
					else if (list.Any((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.FuelStatistics) && base.ToolbarItems.Contains(this.btnStats))
					{
						base.ToolbarItems.Remove(this.btnStats);
					}
					if (!list.Any((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.Settings) && !base.ToolbarItems.Contains(this.btnSettings))
					{
						base.ToolbarItems.Add(this.btnSettings);
					}
					else if (list.Any((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.Settings) && base.ToolbarItems.Contains(this.btnSettings))
					{
						base.ToolbarItems.Remove(this.btnSettings);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000525 RID: 1317 RVA: 0x0004F250 File Offset: 0x0004D450
		private void DisableMainButtons()
		{
			foreach (View view in this.MainButtonsGrid.Children)
			{
				view.IsEnabled = false;
			}
			this.cbECUID.IsEnabled = false;
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0004F2AC File Offset: 0x0004D4AC
		private void EnableMainButtons()
		{
			foreach (View view in this.MainButtonsGrid.Children)
			{
				view.IsEnabled = true;
			}
			this.cbECUID.IsEnabled = true;
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0004F308 File Offset: 0x0004D508
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SimpleMainPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/SimpleMainPage.xaml",
				Instance = this
			}))
			{
				this.__InitComponentRuntime();
				return;
			}
			if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
			{
				this.__InitComponentRuntime();
				return;
			}
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			ExcludeInfinityFromDoubleConverter excludeInfinityFromDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			OBDReaderConnectedToECUStatusToTrue obdreaderConnectedToECUStatusToTrue;
			VisualDiagnostics.RegisterSourceInfo(obdreaderConnectedToECUStatusToTrue = new OBDReaderConnectedToECUStatusToTrue(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			MultiBooleanToTrueConverter multiBooleanToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(multiBooleanToTrueConverter = new MultiBooleanToTrueConverter(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 10);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 13);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 26);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 26);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 26);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 30);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 30);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 30);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 29);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 29);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 34);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			OBDDataReader obdreader;
			VisualDiagnostics.RegisterSourceInfo(obdreader = App.OBDReader, new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 29);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 29);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 29);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 22);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 22);
			ActivityFrame activityFrame2;
			VisualDiagnostics.RegisterSourceInfo(activityFrame2 = new ActivityFrame(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 22);
			CarInfoViewModel instance;
			VisualDiagnostics.RegisterSourceInfo(instance = CarInfoViewModel.Instance, new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 25);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 34);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 34);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 33);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 33);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 30);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 33);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 33);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 30);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 26);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 31);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 291, 34);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 34);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 34);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 33);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 295, 30);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 303, 33);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 306, 33);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 30);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 26);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 31);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 34);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 34);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 34);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 321, 33);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 322, 33);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 30);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 33);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 328, 33);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 329, 33);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 323, 30);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 26);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 31);
			ColumnDefinition columnDefinition10;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition10 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 335, 34);
			ColumnDefinition columnDefinition11;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition11 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 34);
			ColumnDefinition columnDefinition12;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition12 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 34);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 33);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 33);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 30);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 33);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 352, 33);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 346, 30);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 22);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 14);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 389, 22);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 391, 22);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 22);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 395, 22);
			ColumnDefinition columnDefinition13;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition13 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 398, 22);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 404, 26);
			RowDefinition rowDefinition15;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition15 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 26);
			RowDefinition rowDefinition16;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition16 = new RowDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 406, 26);
			ColumnDefinition columnDefinition14;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition14 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 409, 26);
			ColumnDefinition columnDefinition15;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition15 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 26);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 25);
			DynamicResourceExtension dynamicResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension18 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 418, 25);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 22);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 25);
			DynamicResourceExtension dynamicResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension19 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 426, 25);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 419, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 25);
			DynamicResourceExtension dynamicResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension20 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 25);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 22);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 25);
			DynamicResourceExtension dynamicResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension21 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 441, 25);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 448, 25);
			DynamicResourceExtension dynamicResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension22 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 449, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 43);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 455, 37);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 452, 34);
			OBDDataReader obdreader2;
			VisualDiagnostics.RegisterSourceInfo(obdreader2 = App.OBDReader, new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 459, 37);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 456, 34);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 30);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 442, 22);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 402, 18);
			ColumnDefinition columnDefinition16;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition16 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 481, 30);
			ColumnDefinition columnDefinition17;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition17 = new ColumnDefinition(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 482, 30);
			DynamicResourceExtension dynamicResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension23 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 487, 29);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 29);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 26);
			DynamicResourceExtension dynamicResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension24 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 499, 29);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 504, 29);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 496, 26);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 479, 22);
			DynamicResourceExtension dynamicResourceExtension25;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension25 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 516, 25);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 521, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 511, 22);
			DynamicResourceExtension dynamicResourceExtension26;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension26 = new DynamicResourceExtension(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 530, 25);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 535, 25);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 525, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 18);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 540, 18);
			Grid grid10;
			VisualDiagnostics.RegisterSourceInfo(grid10 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 14);
			Grid grid11;
			VisualDiagnostics.RegisterSourceInfo(grid11 = new Grid(), new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\SimpleMainPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("contentGrid", grid11);
			if (grid11.StyleId == null)
			{
				grid11.StyleId = "contentGrid";
			}
			nameScope.RegisterName("labelNewVersion", label);
			if (label.StyleId == null)
			{
				label.StyleId = "labelNewVersion";
			}
			nameScope.RegisterName("cbECUID", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "cbECUID";
			}
			nameScope.RegisterName("tbPleaseWait", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "tbPleaseWait";
			}
			nameScope.RegisterName("MainButtonsGrid", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "MainButtonsGrid";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("connectionProgressFrame", activityFrame2);
			if (activityFrame2.StyleId == null)
			{
				activityFrame2.StyleId = "connectionProgressFrame";
			}
			nameScope.RegisterName("gridVIN", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "gridVIN";
			}
			nameScope.RegisterName("tbProto", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "tbProto";
			}
			nameScope.RegisterName("tbProtoValue", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "tbProtoValue";
			}
			nameScope.RegisterName("gridBottomContainer", grid10);
			if (grid10.StyleId == null)
			{
				grid10.StyleId = "gridBottomContainer";
			}
			nameScope.RegisterName("gridConnectionStatus", grid8);
			if (grid8.StyleId == null)
			{
				grid8.StyleId = "gridConnectionStatus";
			}
			nameScope.RegisterName("tbELMStatus", label12);
			if (label12.StyleId == null)
			{
				label12.StyleId = "tbELMStatus";
			}
			nameScope.RegisterName("tbECUStatus", label14);
			if (label14.StyleId == null)
			{
				label14.StyleId = "tbECUStatus";
			}
			nameScope.RegisterName("tbBadELM", label15);
			if (label15.StyleId == null)
			{
				label15.StyleId = "tbBadELM";
			}
			nameScope.RegisterName("gridConnectButtons", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "gridConnectButtons";
			}
			nameScope.RegisterName("btnConnect", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnConnect";
			}
			nameScope.RegisterName("btnStartSimulation", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnStartSimulation";
			}
			nameScope.RegisterName("btnDisconnect", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnDisconnect";
			}
			nameScope.RegisterName("btnStopSimulation", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnStopSimulation";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.page = this;
			this.contentGrid = grid11;
			this.labelNewVersion = label;
			this.cbECUID = picker;
			this.tbPleaseWait = label2;
			this.MainButtonsGrid = grid2;
			this.activityFrame = activityFrame;
			this.connectionProgressFrame = activityFrame2;
			this.gridVIN = stackLayout;
			this.tbProto = label3;
			this.tbProtoValue = label4;
			this.gridBottomContainer = grid10;
			this.gridConnectionStatus = grid8;
			this.tbELMStatus = label12;
			this.tbECUStatus = label14;
			this.tbBadELM = label15;
			this.gridConnectButtons = stackLayout2;
			this.btnConnect = button;
			this.btnStartSimulation = button2;
			this.btnDisconnect = button3;
			this.btnStopSimulation = button4;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("OBDReaderConnectedToECUStatusToTrue", obdreaderConnectedToECUStatusToTrue);
			resourceDictionary.Add("MultiBooleanToTrueConverter", multiBooleanToTrueConverter);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.ConnectPage_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.ConnectPage_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Page_SizeChanged;
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(0.0, 0.0, 0.0, 0.0);
			onPlatform.iOS = new Thickness(0.0, 0.0, 0.0, 0.0);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid11.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension2 = referenceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = grid11;
			array2[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 13)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			grid11.SetValue(BindableObject.BindingContextProperty, obj3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid11.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid11.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			scrollView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			grid.SetValue(Grid.RowProperty, 0);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = label;
			array3[1] = grid;
			array3[2] = grid7;
			array3[3] = scrollView;
			array3[4] = grid11;
			array3[5] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = grid7;
			array4[3] = scrollView;
			array4[4] = grid11;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 29)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate.Text = "main_NewVersionAvailable";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = grid7;
			array5[3] = scrollView;
			array5[4] = grid11;
			array5[5] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 29)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj7;
			label.SetValue(Label.TextColorProperty, Color.Red);
			label.SetValue(Label.TextDecorationsProperty, new TextDecorationConverter().ConvertFromInvariantString("Underline"));
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.LabelNewVersion_Clicked;
			label.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(label);
			picker.SetValue(Grid.RowProperty, 1);
			picker.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			picker.SetValue(BindableObject.BindingContextProperty, obdreader);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ECUSelectorVisible";
			bindingExtension.TypedBinding = new TypedBinding<OBDDataReader, bool>(delegate(OBDDataReader A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ECUSelectorVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<OBDDataReader, object>, string>[]
			{
				new Tuple<Func<OBDDataReader, object>, string>((OBDDataReader A_0) => A_0, "ECUSelectorVisible")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			bindingExtension2.Path = "FriendlyName";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.ItemDisplayBinding = bindingBase2;
			bindingExtension3.Path = "ECUHeaders";
			bindingExtension3.TypedBinding = new TypedBinding<OBDDataReader, ObservableCollection<ECUHeader>>(delegate(OBDDataReader A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ObservableCollection<ECUHeader>, bool>(A_0.ECUHeaders, true);
				}
				return default(ValueTuple<ObservableCollection<ECUHeader>, bool>);
			}, null, new Tuple<Func<OBDDataReader, object>, string>[]
			{
				new Tuple<Func<OBDDataReader, object>, string>((OBDDataReader A_0) => A_0, "ECUHeaders")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase3);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "SelectedECU";
			bindingExtension4.TypedBinding = new TypedBinding<OBDDataReader, int>(delegate(OBDDataReader A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SelectedECU, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(OBDDataReader A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.SelectedECU = A_1;
					return;
				}
			}, new Tuple<Func<OBDDataReader, object>, string>[]
			{
				new Tuple<Func<OBDDataReader, object>, string>((OBDDataReader A_0) => A_0, "SelectedECU")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase4);
			picker.SelectedIndexChanged += this.cbECUID_SelectedIndexChanged;
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = picker;
			array6[1] = grid;
			array6[2] = grid7;
			array6[3] = scrollView;
			array6[4] = grid11;
			array6[5] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Picker.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 29)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			picker.SetDynamicResource(Picker.TextColorProperty, dynamicResource4.Key);
			grid.Children.Add(picker);
			label2.SetValue(Grid.RowProperty, 2);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate2.Text = "MainPage_tbPleaseWait.Text";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = label2;
			array7[1] = grid;
			array7[2] = grid7;
			array7[3] = scrollView;
			array7[4] = grid11;
			array7[5] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 29)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label2.Text = obj10;
			dynamicResourceExtension5.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label2;
			array8[1] = grid;
			array8[2] = grid7;
			array8[3] = scrollView;
			array8[4] = grid11;
			array8[5] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 29)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label2.SetDynamicResource(Label.TextColorProperty, dynamicResource5.Key);
			grid.Children.Add(label2);
			grid7.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(Grid.ColumnSpacingProperty, 2.0);
			grid2.SetValue(Grid.RowSpacingProperty, 15.0);
			grid7.Children.Add(grid2);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid7.Children.Add(activityFrame);
			activityFrame2.SetValue(Grid.RowProperty, 1);
			activityFrame2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid7.Children.Add(activityFrame2);
			stackLayout.SetValue(Grid.RowProperty, 2);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0, 4.0));
			stackLayout.SetValue(BindableObject.BindingContextProperty, instance);
			stackLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			label3.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension6.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = label3;
			array9[1] = grid3;
			array9[2] = stackLayout;
			array9[3] = grid7;
			array9[4] = scrollView;
			array9[5] = grid11;
			array9[6] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(273, 33)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			label3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate3.Text = "CarInfoPage_tbOBDProtocol.Text";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = label3;
			array10[1] = grid3;
			array10[2] = stackLayout;
			array10[3] = grid7;
			array10[4] = scrollView;
			array10[5] = grid11;
			array10[6] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(277, 33)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label3.Text = obj14;
			grid3.Children.Add(label3);
			label4.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = label4;
			array11[1] = grid3;
			array11[2] = stackLayout;
			array11[3] = grid7;
			array11[4] = scrollView;
			array11[5] = grid11;
			array11[6] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, Label.FontSizeProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(281, 33)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "OBDProtocol";
			bindingExtension5.TypedBinding = new TypedBinding<CarInfoViewModel, string>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.OBDProtocol, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "OBDProtocol")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label4.SetBinding(Label.TextProperty, bindingBase5);
			grid3.Children.Add(label4);
			stackLayout.Children.Add(grid3);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "IsVINAvailable";
			bindingExtension6.TypedBinding = new TypedBinding<CarInfoViewModel, bool>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsVINAvailable, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "IsVINAvailable")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			grid4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			label5.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension8.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 7];
			array12[0] = label5;
			array12[1] = grid4;
			array12[2] = stackLayout;
			array12[3] = grid7;
			array12[4] = scrollView;
			array12[5] = grid11;
			array12[6] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, Label.FontSizeProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(297, 33)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			label5.SetValue(Label.TextProperty, "VIN:");
			grid4.Children.Add(label5);
			label6.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension9.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = label6;
			array13[1] = grid4;
			array13[2] = stackLayout;
			array13[3] = grid7;
			array13[4] = scrollView;
			array13[5] = grid11;
			array13[6] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array13, Label.FontSizeProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(303, 33)));
			DynamicResource dynamicResource9 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource9.Key);
			label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "VIN";
			bindingExtension7.TypedBinding = new TypedBinding<CarInfoViewModel, string>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.VIN, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "VIN")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase7);
			grid4.Children.Add(label6);
			stackLayout.Children.Add(grid4);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "IsECUNameAvailable";
			bindingExtension8.TypedBinding = new TypedBinding<CarInfoViewModel, bool>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsECUNameAvailable, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "IsECUNameAvailable")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			grid5.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
			label7.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension10.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 7];
			array14[0] = label7;
			array14[1] = grid5;
			array14[2] = stackLayout;
			array14[3] = grid7;
			array14[4] = scrollView;
			array14[5] = grid11;
			array14[6] = this;
			object obj18;
			xamlServiceProvider14.Add(typeFromHandle27, obj18 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(318, 33)));
			DynamicResource dynamicResource10 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource10.Key);
			label7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label7.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			translate4.Text = "CarInfoPage_tbECUName.Text";
			IMarkupExtension markupExtension15 = translate4;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = label7;
			array15[1] = grid5;
			array15[2] = stackLayout;
			array15[3] = grid7;
			array15[4] = scrollView;
			array15[5] = grid11;
			array15[6] = this;
			object obj19;
			xamlServiceProvider15.Add(typeFromHandle29, obj19 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(321, 33)));
			object obj20 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label7.Text = obj20;
			dynamicResourceExtension11.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = label7;
			array16[1] = grid5;
			array16[2] = stackLayout;
			array16[3] = grid7;
			array16[4] = scrollView;
			array16[5] = grid11;
			array16[6] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle31, obj21 = new SimpleValueTargetProvider(array16, Label.TextColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(322, 33)));
			DynamicResource dynamicResource11 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label7.SetDynamicResource(Label.TextColorProperty, dynamicResource11.Key);
			grid5.Children.Add(label7);
			label8.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension12.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 7];
			array17[0] = label8;
			array17[1] = grid5;
			array17[2] = stackLayout;
			array17[3] = grid7;
			array17[4] = scrollView;
			array17[5] = grid11;
			array17[6] = this;
			object obj22;
			xamlServiceProvider17.Add(typeFromHandle33, obj22 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(325, 33)));
			DynamicResource dynamicResource12 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource12.Key);
			label8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label8.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "ECUName";
			bindingExtension9.TypedBinding = new TypedBinding<CarInfoViewModel, string>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ECUName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "ECUName")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label8.SetBinding(Label.TextProperty, bindingBase9);
			dynamicResourceExtension13.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = label8;
			array18[1] = grid5;
			array18[2] = stackLayout;
			array18[3] = grid7;
			array18[4] = scrollView;
			array18[5] = grid11;
			array18[6] = this;
			object obj23;
			xamlServiceProvider18.Add(typeFromHandle35, obj23 = new SimpleValueTargetProvider(array18, Label.TextColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(329, 33)));
			DynamicResource dynamicResource13 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label8.SetDynamicResource(Label.TextColorProperty, dynamicResource13.Key);
			grid5.Children.Add(label8);
			stackLayout.Children.Add(grid5);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "IsCalibrationIdAvailable";
			bindingExtension10.TypedBinding = new TypedBinding<CarInfoViewModel, bool>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsCalibrationIdAvailable, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "IsCalibrationIdAvailable")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			grid6.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			columnDefinition10.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition10);
			columnDefinition11.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition11);
			columnDefinition12.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition12);
			label9.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension14.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 7];
			array19[0] = label9;
			array19[1] = grid6;
			array19[2] = stackLayout;
			array19[3] = grid7;
			array19[4] = scrollView;
			array19[5] = grid11;
			array19[6] = this;
			object obj24;
			xamlServiceProvider19.Add(typeFromHandle37, obj24 = new SimpleValueTargetProvider(array19, Label.FontSizeProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(341, 33)));
			DynamicResource dynamicResource14 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource14.Key);
			label9.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label9.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			translate5.Text = "CarInfoPage_tbCalibrationId.Text";
			IMarkupExtension markupExtension20 = translate5;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = label9;
			array20[1] = grid6;
			array20[2] = stackLayout;
			array20[3] = grid7;
			array20[4] = scrollView;
			array20[5] = grid11;
			array20[6] = this;
			object obj25;
			xamlServiceProvider20.Add(typeFromHandle39, obj25 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(344, 33)));
			object obj26 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label9.Text = obj26;
			dynamicResourceExtension15.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = label9;
			array21[1] = grid6;
			array21[2] = stackLayout;
			array21[3] = grid7;
			array21[4] = scrollView;
			array21[5] = grid11;
			array21[6] = this;
			object obj27;
			xamlServiceProvider21.Add(typeFromHandle41, obj27 = new SimpleValueTargetProvider(array21, Label.TextColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(345, 33)));
			DynamicResource dynamicResource15 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label9.SetDynamicResource(Label.TextColorProperty, dynamicResource15.Key);
			grid6.Children.Add(label9);
			label10.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension16.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = label10;
			array22[1] = grid6;
			array22[2] = stackLayout;
			array22[3] = grid7;
			array22[4] = scrollView;
			array22[5] = grid11;
			array22[6] = this;
			object obj28;
			xamlServiceProvider22.Add(typeFromHandle43, obj28 = new SimpleValueTargetProvider(array22, Label.FontSizeProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(348, 33)));
			DynamicResource dynamicResource16 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource16.Key);
			label10.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label10.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "CalibrationId";
			bindingExtension11.TypedBinding = new TypedBinding<CarInfoViewModel, string>(delegate(CarInfoViewModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.CalibrationId, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<CarInfoViewModel, object>, string>[]
			{
				new Tuple<Func<CarInfoViewModel, object>, string>((CarInfoViewModel A_0) => A_0, "CalibrationId")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label10.SetBinding(Label.TextProperty, bindingBase11);
			dynamicResourceExtension17.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 7];
			array23[0] = label10;
			array23[1] = grid6;
			array23[2] = stackLayout;
			array23[3] = grid7;
			array23[4] = scrollView;
			array23[5] = grid11;
			array23[6] = this;
			object obj29;
			xamlServiceProvider23.Add(typeFromHandle45, obj29 = new SimpleValueTargetProvider(array23, Label.TextColorProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(352, 33)));
			DynamicResource dynamicResource17 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label10.SetDynamicResource(Label.TextColorProperty, dynamicResource17.Key);
			grid6.Children.Add(label10);
			stackLayout.Children.Add(grid6);
			grid7.Children.Add(stackLayout);
			scrollView.Content = grid7;
			grid11.Children.Add(scrollView);
			grid10.SetValue(Grid.RowProperty, 1);
			grid10.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			columnDefinition13.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition13);
			grid8.SetValue(Grid.RowProperty, 1);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			rowDefinition15.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition15);
			rowDefinition16.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition16);
			columnDefinition14.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition14);
			columnDefinition15.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition15);
			label11.SetValue(Grid.RowProperty, 0);
			label11.SetValue(Grid.ColumnProperty, 0);
			label11.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			label11.SetValue(Label.LineBreakModeProperty, 1);
			translate6.Text = "MainPage_ELM_Connection.Text";
			IMarkupExtension markupExtension24 = translate6;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = label11;
			array24[1] = grid8;
			array24[2] = grid10;
			array24[3] = grid11;
			array24[4] = this;
			object obj30;
			xamlServiceProvider24.Add(typeFromHandle47, obj30 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(417, 25)));
			object obj31 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label11.Text = obj31;
			dynamicResourceExtension18.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension18;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = label11;
			array25[1] = grid8;
			array25[2] = grid10;
			array25[3] = grid11;
			array25[4] = this;
			object obj32;
			xamlServiceProvider25.Add(typeFromHandle49, obj32 = new SimpleValueTargetProvider(array25, Label.TextColorProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(418, 25)));
			DynamicResource dynamicResource18 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label11.SetDynamicResource(Label.TextColorProperty, dynamicResource18.Key);
			grid8.Children.Add(label11);
			label12.SetValue(Grid.RowProperty, 0);
			label12.SetValue(Grid.ColumnProperty, 1);
			label12.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label12.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			translate7.Text = "MainPage_StatusDisconnected";
			IMarkupExtension markupExtension26 = translate7;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 5];
			array26[0] = label12;
			array26[1] = grid8;
			array26[2] = grid10;
			array26[3] = grid11;
			array26[4] = this;
			object obj33;
			xamlServiceProvider26.Add(typeFromHandle51, obj33 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(425, 25)));
			object obj34 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label12.Text = obj34;
			dynamicResourceExtension19.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension19;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = label12;
			array27[1] = grid8;
			array27[2] = grid10;
			array27[3] = grid11;
			array27[4] = this;
			object obj35;
			xamlServiceProvider27.Add(typeFromHandle53, obj35 = new SimpleValueTargetProvider(array27, Label.TextColorProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(426, 25)));
			DynamicResource dynamicResource19 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label12.SetDynamicResource(Label.TextColorProperty, dynamicResource19.Key);
			grid8.Children.Add(label12);
			label13.SetValue(Grid.RowProperty, 1);
			label13.SetValue(Grid.ColumnProperty, 0);
			label13.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			label13.SetValue(Label.LineBreakModeProperty, 1);
			translate8.Text = "MainPage_ECU_Connection.Text";
			IMarkupExtension markupExtension28 = translate8;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = label13;
			array28[1] = grid8;
			array28[2] = grid10;
			array28[3] = grid11;
			array28[4] = this;
			object obj36;
			xamlServiceProvider28.Add(typeFromHandle55, obj36 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(432, 25)));
			object obj37 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label13.Text = obj37;
			dynamicResourceExtension20.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension20;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 5];
			array29[0] = label13;
			array29[1] = grid8;
			array29[2] = grid10;
			array29[3] = grid11;
			array29[4] = this;
			object obj38;
			xamlServiceProvider29.Add(typeFromHandle57, obj38 = new SimpleValueTargetProvider(array29, Label.TextColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(433, 25)));
			DynamicResource dynamicResource20 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label13.SetDynamicResource(Label.TextColorProperty, dynamicResource20.Key);
			grid8.Children.Add(label13);
			label14.SetValue(Grid.RowProperty, 1);
			label14.SetValue(Grid.ColumnProperty, 1);
			label14.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label14.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			translate9.Text = "MainPage_StatusDisconnected";
			IMarkupExtension markupExtension30 = translate9;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = label14;
			array30[1] = grid8;
			array30[2] = grid10;
			array30[3] = grid11;
			array30[4] = this;
			object obj39;
			xamlServiceProvider30.Add(typeFromHandle59, obj39 = new SimpleValueTargetProvider(array30, Label.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(440, 25)));
			object obj40 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label14.Text = obj40;
			dynamicResourceExtension21.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension31 = dynamicResourceExtension21;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 5];
			array31[0] = label14;
			array31[1] = grid8;
			array31[2] = grid10;
			array31[3] = grid11;
			array31[4] = this;
			object obj41;
			xamlServiceProvider31.Add(typeFromHandle61, obj41 = new SimpleValueTargetProvider(array31, Label.TextColorProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(441, 25)));
			DynamicResource dynamicResource21 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label14.SetDynamicResource(Label.TextColorProperty, dynamicResource21.Key);
			grid8.Children.Add(label14);
			label15.SetValue(Grid.RowProperty, 2);
			label15.SetValue(Grid.ColumnProperty, 0);
			label15.SetValue(Grid.ColumnSpanProperty, 2);
			label15.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate10.Text = "tbBadELM.Text";
			IMarkupExtension markupExtension32 = translate10;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 5];
			array32[0] = label15;
			array32[1] = grid8;
			array32[2] = grid10;
			array32[3] = grid11;
			array32[4] = this;
			object obj42;
			xamlServiceProvider32.Add(typeFromHandle63, obj42 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(448, 25)));
			object obj43 = markupExtension32.ProvideValue(xamlServiceProvider32);
			label15.Text = obj43;
			dynamicResourceExtension22.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension33 = dynamicResourceExtension22;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 5];
			array33[0] = label15;
			array33[1] = grid8;
			array33[2] = grid10;
			array33[3] = grid11;
			array33[4] = this;
			object obj44;
			xamlServiceProvider33.Add(typeFromHandle65, obj44 = new SimpleValueTargetProvider(array33, Label.TextColorProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(449, 25)));
			DynamicResource dynamicResource22 = markupExtension33.ProvideValue(xamlServiceProvider33);
			label15.SetDynamicResource(Label.TextColorProperty, dynamicResource22.Key);
			staticResourceExtension.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension34 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 6];
			array34[0] = multiBinding;
			array34[1] = label15;
			array34[2] = grid8;
			array34[3] = grid10;
			array34[4] = grid11;
			array34[5] = this;
			object obj45;
			xamlServiceProvider34.Add(typeFromHandle67, obj45 = new SimpleValueTargetProvider(array34, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(451, 43)));
			object obj46 = markupExtension34.ProvideValue(xamlServiceProvider34);
			multiBinding.Converter = obj46;
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "ShowBadELMWarning";
			bindingExtension12.Source = sharedSettings;
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "BadELM";
			bindingExtension13.Source = obdreader2;
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase13);
			label15.SetBinding(VisualElement.IsVisibleProperty, multiBinding);
			grid8.Children.Add(label15);
			grid10.Children.Add(grid8);
			stackLayout2.SetValue(Grid.RowProperty, 2);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			columnDefinition16.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.7*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition16);
			columnDefinition17.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.3*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition17);
			button.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension23.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension35 = dynamicResourceExtension23;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 6];
			array35[0] = button;
			array35[1] = grid9;
			array35[2] = stackLayout2;
			array35[3] = grid10;
			array35[4] = grid11;
			array35[5] = this;
			object obj47;
			xamlServiceProvider35.Add(typeFromHandle69, obj47 = new SimpleValueTargetProvider(array35, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(487, 29)));
			DynamicResource dynamicResource23 = markupExtension35.ProvideValue(xamlServiceProvider35);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource23.Key);
			button.SetValue(Button.BorderColorProperty, Color.Black);
			button.Clicked += this.btnConnect_Clicked;
			button.SetValue(Button.CornerRadiusProperty, 9);
			button.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			translate11.Text = "MainPage_TileConnect.Text";
			IMarkupExtension markupExtension36 = translate11;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 6];
			array36[0] = button;
			array36[1] = grid9;
			array36[2] = stackLayout2;
			array36[3] = grid10;
			array36[4] = grid11;
			array36[5] = this;
			object obj48;
			xamlServiceProvider36.Add(typeFromHandle71, obj48 = new SimpleValueTargetProvider(array36, Button.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(492, 29)));
			object obj49 = markupExtension36.ProvideValue(xamlServiceProvider36);
			button.Text = obj49;
			button.SetValue(Button.TextColorProperty, Color.White);
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid9.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension24.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension37 = dynamicResourceExtension24;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 6];
			array37[0] = button2;
			array37[1] = grid9;
			array37[2] = stackLayout2;
			array37[3] = grid10;
			array37[4] = grid11;
			array37[5] = this;
			object obj50;
			xamlServiceProvider37.Add(typeFromHandle73, obj50 = new SimpleValueTargetProvider(array37, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(499, 29)));
			DynamicResource dynamicResource24 = markupExtension37.ProvideValue(xamlServiceProvider37);
			button2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource24.Key);
			button2.SetValue(Button.BorderColorProperty, Color.Black);
			button2.Clicked += this.btnStartSimulation_Clicked;
			button2.SetValue(Button.CornerRadiusProperty, 9);
			button2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			translate12.Text = "ios_ConnectPage_StartDemo";
			IMarkupExtension markupExtension38 = translate12;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 6];
			array38[0] = button2;
			array38[1] = grid9;
			array38[2] = stackLayout2;
			array38[3] = grid10;
			array38[4] = grid11;
			array38[5] = this;
			object obj51;
			xamlServiceProvider38.Add(typeFromHandle75, obj51 = new SimpleValueTargetProvider(array38, Button.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(504, 29)));
			object obj52 = markupExtension38.ProvideValue(xamlServiceProvider38);
			button2.Text = obj52;
			button2.SetValue(Button.TextColorProperty, Color.White);
			button2.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid9.Children.Add(button2);
			stackLayout2.Children.Add(grid9);
			button3.SetValue(Grid.RowProperty, 0);
			button3.SetValue(Grid.ColumnProperty, 0);
			button3.SetValue(Grid.ColumnSpanProperty, 2);
			dynamicResourceExtension25.Key = "ButtonRedColor";
			IMarkupExtension<DynamicResource> markupExtension39 = dynamicResourceExtension25;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 5];
			array39[0] = button3;
			array39[1] = stackLayout2;
			array39[2] = grid10;
			array39[3] = grid11;
			array39[4] = this;
			object obj53;
			xamlServiceProvider39.Add(typeFromHandle77, obj53 = new SimpleValueTargetProvider(array39, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(516, 25)));
			DynamicResource dynamicResource25 = markupExtension39.ProvideValue(xamlServiceProvider39);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource25.Key);
			button3.SetValue(Button.BorderColorProperty, Color.Black);
			button3.Clicked += this.btnDisconnect_Clicked;
			button3.SetValue(Button.CornerRadiusProperty, 9);
			button3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate13.Text = "MainPage_TileDisconnect.Text";
			IMarkupExtension markupExtension40 = translate13;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 5];
			array40[0] = button3;
			array40[1] = stackLayout2;
			array40[2] = grid10;
			array40[3] = grid11;
			array40[4] = this;
			object obj54;
			xamlServiceProvider40.Add(typeFromHandle79, obj54 = new SimpleValueTargetProvider(array40, Button.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(521, 25)));
			object obj55 = markupExtension40.ProvideValue(xamlServiceProvider40);
			button3.Text = obj55;
			button3.SetValue(Button.TextColorProperty, Color.White);
			button3.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			stackLayout2.Children.Add(button3);
			button4.SetValue(Grid.RowProperty, 0);
			button4.SetValue(Grid.ColumnProperty, 0);
			button4.SetValue(Grid.ColumnSpanProperty, 2);
			dynamicResourceExtension26.Key = "ButtonRedColor";
			IMarkupExtension<DynamicResource> markupExtension41 = dynamicResourceExtension26;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 5];
			array41[0] = button4;
			array41[1] = stackLayout2;
			array41[2] = grid10;
			array41[3] = grid11;
			array41[4] = this;
			object obj56;
			xamlServiceProvider41.Add(typeFromHandle81, obj56 = new SimpleValueTargetProvider(array41, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(530, 25)));
			DynamicResource dynamicResource26 = markupExtension41.ProvideValue(xamlServiceProvider41);
			button4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource26.Key);
			button4.SetValue(Button.BorderColorProperty, Color.Black);
			button4.Clicked += this.btnStopSimulation_Clicked;
			button4.SetValue(Button.CornerRadiusProperty, 9);
			button4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate14.Text = "ios_ConnectPage_StopDemo";
			IMarkupExtension markupExtension42 = translate14;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 5];
			array42[0] = button4;
			array42[1] = stackLayout2;
			array42[2] = grid10;
			array42[3] = grid11;
			array42[4] = this;
			object obj57;
			xamlServiceProvider42.Add(typeFromHandle83, obj57 = new SimpleValueTargetProvider(array42, Button.TextProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SimpleMainPage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(535, 25)));
			object obj58 = markupExtension42.ProvideValue(xamlServiceProvider42);
			button4.Text = obj58;
			button4.SetValue(Button.TextColorProperty, Color.White);
			button4.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			stackLayout2.Children.Add(button4);
			grid10.Children.Add(stackLayout2);
			complexAdView.SetValue(Grid.RowProperty, 3);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid10.Children.Add(complexAdView);
			grid11.Children.Add(grid10);
			this.SetValue(ContentPage.ContentProperty, grid11);
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x00056161 File Offset: 0x00054361
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.btnPurchase_Clicked(this.btnBuy, EventArgs.Empty);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x00056174 File Offset: 0x00054374
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.btnFuelStatistics_Clicked(this.btnStats, EventArgs.Empty);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x00056187 File Offset: 0x00054387
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.btnSettings_Clicked(this.btnSettings, EventArgs.Empty);
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0005619A File Offset: 0x0005439A
		[CompilerGenerated]
		private void <CheckForNewVersion>b__13_0()
		{
			if (VersionChecker.ShouldShowNewVersionAvailable)
			{
				this.labelNewVersion.IsVisible = true;
				return;
			}
			this.labelNewVersion.IsVisible = false;
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x000561BC File Offset: 0x000543BC
		[CompilerGenerated]
		private async void <AdsInitialize>b__17_0()
		{
			this.ad.IsVisible = true;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x000561F3 File Offset: 0x000543F3
		[CompilerGenerated]
		private void <OBD_StatusChanged>b__27_0()
		{
			this.SetButtonsDependingOnOBDStatus();
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000561FC File Offset: 0x000543FC
		[CompilerGenerated]
		private async void <GetPidsForEcuSelected>b__29_0()
		{
			this.btnDisconnect.IsEnabled = false;
			this.btnConnect.IsEnabled = false;
			this.btnStopSimulation.IsEnabled = false;
			this.btnStartSimulation.IsEnabled = false;
			this.tbPleaseWait.IsVisible = true;
			await App.OBDReader.ChangeECU();
			await this.UpdateVINInfo();
			this.btnDisconnect.IsEnabled = true;
			this.btnConnect.IsEnabled = true;
			this.btnStopSimulation.IsEnabled = true;
			this.btnStartSimulation.IsEnabled = true;
			this.tbPleaseWait.IsVisible = false;
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x000561F3 File Offset: 0x000543F3
		[CompilerGenerated]
		private void <SetButtonsDependingOnOBDStatus>b__34_0()
		{
			this.SetButtonsDependingOnOBDStatus();
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00056233 File Offset: 0x00054433
		[CompilerGenerated]
		private void <UpdateVINInfo>b__42_0()
		{
			this.gridVIN.IsVisible = true;
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00056233 File Offset: 0x00054433
		[CompilerGenerated]
		private void <UpdateVINInfo>b__42_1()
		{
			this.gridVIN.IsVisible = true;
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x00056241 File Offset: 0x00054441
		[CompilerGenerated]
		private void <UpdateMainButtons>b__56_0()
		{
			this.UpdateMainButtons();
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0005624C File Offset: 0x0005444C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SimpleMainPage>(this, typeof(SimpleMainPage));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.contentGrid = NameScopeExtensions.FindByName<Grid>(this, "contentGrid");
			this.labelNewVersion = NameScopeExtensions.FindByName<Label>(this, "labelNewVersion");
			this.cbECUID = NameScopeExtensions.FindByName<Picker>(this, "cbECUID");
			this.tbPleaseWait = NameScopeExtensions.FindByName<Label>(this, "tbPleaseWait");
			this.MainButtonsGrid = NameScopeExtensions.FindByName<Grid>(this, "MainButtonsGrid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.connectionProgressFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "connectionProgressFrame");
			this.gridVIN = NameScopeExtensions.FindByName<StackLayout>(this, "gridVIN");
			this.tbProto = NameScopeExtensions.FindByName<Label>(this, "tbProto");
			this.tbProtoValue = NameScopeExtensions.FindByName<Label>(this, "tbProtoValue");
			this.gridBottomContainer = NameScopeExtensions.FindByName<Grid>(this, "gridBottomContainer");
			this.gridConnectionStatus = NameScopeExtensions.FindByName<Grid>(this, "gridConnectionStatus");
			this.tbELMStatus = NameScopeExtensions.FindByName<Label>(this, "tbELMStatus");
			this.tbECUStatus = NameScopeExtensions.FindByName<Label>(this, "tbECUStatus");
			this.tbBadELM = NameScopeExtensions.FindByName<Label>(this, "tbBadELM");
			this.gridConnectButtons = NameScopeExtensions.FindByName<StackLayout>(this, "gridConnectButtons");
			this.btnConnect = NameScopeExtensions.FindByName<Button>(this, "btnConnect");
			this.btnStartSimulation = NameScopeExtensions.FindByName<Button>(this, "btnStartSimulation");
			this.btnDisconnect = NameScopeExtensions.FindByName<Button>(this, "btnDisconnect");
			this.btnStopSimulation = NameScopeExtensions.FindByName<Button>(this, "btnStopSimulation");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x000563E0 File Offset: 0x000545E0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1410(OBDDataReader A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ECUSelectorVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x00056410 File Offset: 0x00054610
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1411(OBDDataReader A_0)
		{
			return A_0;
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00056420 File Offset: 0x00054620
		[CompilerGenerated]
		private static ValueTuple<ObservableCollection<ECUHeader>, bool> <InitializeComponent>typedBindingsM__1412(OBDDataReader A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ObservableCollection<ECUHeader>, bool>(A_0.ECUHeaders, true);
			}
			return default(ValueTuple<ObservableCollection<ECUHeader>, bool>);
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x00056450 File Offset: 0x00054650
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1414(OBDDataReader A_0)
		{
			return A_0;
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x00056460 File Offset: 0x00054660
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1415(OBDDataReader A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.SelectedECU, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00056490 File Offset: 0x00054690
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1416(OBDDataReader A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.SelectedECU = A_1;
				return;
			}
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x000564AC File Offset: 0x000546AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1417(OBDDataReader A_0)
		{
			return A_0;
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x000564BC File Offset: 0x000546BC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1418(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.OBDProtocol, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x000564EC File Offset: 0x000546EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1419(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x000564FC File Offset: 0x000546FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1420(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsVINAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0005652C File Offset: 0x0005472C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1421(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0005653C File Offset: 0x0005473C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1422(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.VIN, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0005656C File Offset: 0x0005476C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1423(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0005657C File Offset: 0x0005477C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1424(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsECUNameAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x000565AC File Offset: 0x000547AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1425(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x000565BC File Offset: 0x000547BC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1426(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ECUName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x000565EC File Offset: 0x000547EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1427(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x000565FC File Offset: 0x000547FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1428(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsCalibrationIdAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0005662C File Offset: 0x0005482C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1429(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0005663C File Offset: 0x0005483C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1430(CarInfoViewModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.CalibrationId, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0005666C File Offset: 0x0005486C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1431(CarInfoViewModel A_0)
		{
			return A_0;
		}

		// Token: 0x040003B7 RID: 951
		[CompilerGenerated]
		private static SimpleMainPage <Instance>k__BackingField;

		// Token: 0x040003B8 RID: 952
		private ToolbarItem btnBuy;

		// Token: 0x040003B9 RID: 953
		private ToolbarItem btnSettings;

		// Token: 0x040003BA RID: 954
		private ToolbarItem btnStats;

		// Token: 0x040003BB RID: 955
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x040003BC RID: 956
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid contentGrid;

		// Token: 0x040003BD RID: 957
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelNewVersion;

		// Token: 0x040003BE RID: 958
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker cbECUID;

		// Token: 0x040003BF RID: 959
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbPleaseWait;

		// Token: 0x040003C0 RID: 960
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid MainButtonsGrid;

		// Token: 0x040003C1 RID: 961
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x040003C2 RID: 962
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame connectionProgressFrame;

		// Token: 0x040003C3 RID: 963
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout gridVIN;

		// Token: 0x040003C4 RID: 964
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbProto;

		// Token: 0x040003C5 RID: 965
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbProtoValue;

		// Token: 0x040003C6 RID: 966
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridBottomContainer;

		// Token: 0x040003C7 RID: 967
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridConnectionStatus;

		// Token: 0x040003C8 RID: 968
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbELMStatus;

		// Token: 0x040003C9 RID: 969
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbECUStatus;

		// Token: 0x040003CA RID: 970
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbBadELM;

		// Token: 0x040003CB RID: 971
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout gridConnectButtons;

		// Token: 0x040003CC RID: 972
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnConnect;

		// Token: 0x040003CD RID: 973
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnStartSimulation;

		// Token: 0x040003CE RID: 974
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDisconnect;

		// Token: 0x040003CF RID: 975
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnStopSimulation;

		// Token: 0x040003D0 RID: 976
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000102 RID: 258
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<AdsInitialize>b__17_0>d : IAsyncStateMachine
		{
			// Token: 0x06000549 RID: 1353 RVA: 0x0005667C File Offset: 0x0005487C
			void IAsyncStateMachine.MoveNext()
			{
				SimpleMainPage simpleMainPage = this;
				try
				{
					simpleMainPage.ad.IsVisible = true;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600054A RID: 1354 RVA: 0x000566D8 File Offset: 0x000548D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040003D1 RID: 977
			public int <>1__state;

			// Token: 0x040003D2 RID: 978
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040003D3 RID: 979
			public SimpleMainPage <>4__this;
		}

		// Token: 0x02000103 RID: 259
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<GetPidsForEcuSelected>b__29_0>d : IAsyncStateMachine
		{
			// Token: 0x0600054B RID: 1355 RVA: 0x000566E8 File Offset: 0x000548E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_0107;
						}
						simpleMainPage.btnDisconnect.IsEnabled = false;
						simpleMainPage.btnConnect.IsEnabled = false;
						simpleMainPage.btnStopSimulation.IsEnabled = false;
						simpleMainPage.btnStartSimulation.IsEnabled = false;
						simpleMainPage.tbPleaseWait.IsVisible = true;
						taskAwaiter3 = App.OBDReader.ChangeECU().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<<GetPidsForEcuSelected>b__29_0>d>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = simpleMainPage.UpdateVINInfo().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<<GetPidsForEcuSelected>b__29_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_0107:
					taskAwaiter.GetResult();
					simpleMainPage.btnDisconnect.IsEnabled = true;
					simpleMainPage.btnConnect.IsEnabled = true;
					simpleMainPage.btnStopSimulation.IsEnabled = true;
					simpleMainPage.btnStartSimulation.IsEnabled = true;
					simpleMainPage.tbPleaseWait.IsVisible = false;
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

			// Token: 0x0600054C RID: 1356 RVA: 0x0005688C File Offset: 0x00054A8C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040003D4 RID: 980
			public int <>1__state;

			// Token: 0x040003D5 RID: 981
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040003D6 RID: 982
			public SimpleMainPage <>4__this;

			// Token: 0x040003D7 RID: 983
			private TaskAwaiter <>u__1;

			// Token: 0x040003D8 RID: 984
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000104 RID: 260
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600054D RID: 1357 RVA: 0x0005689A File Offset: 0x00054A9A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600054E RID: 1358 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600054F RID: 1359 RVA: 0x000568A6 File Offset: 0x00054AA6
			internal bool <Initialize>b__12_5(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.VersionInfo;
			}

			// Token: 0x06000550 RID: 1360 RVA: 0x000568B2 File Offset: 0x00054AB2
			internal void <RemoveCSV>b__18_0()
			{
				Brc2CsvConverter.RemoveTemporaryCSV();
			}

			// Token: 0x06000551 RID: 1361 RVA: 0x000568B9 File Offset: 0x00054AB9
			internal void <AskForExit>b__20_0()
			{
				IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
				if (droidService == null)
				{
					return;
				}
				droidService.AndroidHelper_StopService();
			}

			// Token: 0x06000552 RID: 1362 RVA: 0x000568CA File Offset: 0x00054ACA
			internal bool <ConnectPage_Appearing>b__25_0(PID x)
			{
				return x is SensorPID;
			}

			// Token: 0x06000553 RID: 1363 RVA: 0x000568D5 File Offset: 0x00054AD5
			internal SensorPID <ConnectPage_Appearing>b__25_1(PID x)
			{
				return x as SensorPID;
			}

			// Token: 0x06000554 RID: 1364 RVA: 0x000568DD File Offset: 0x00054ADD
			internal bool <SetButtonsDependingOnOBDStatus>b__34_1(View x)
			{
				return x is MainPageButton && (x as MainPageButton).ButtonType == MainPageButtons.Coding;
			}

			// Token: 0x06000555 RID: 1365 RVA: 0x000568F8 File Offset: 0x00054AF8
			internal void <ShowActivityFrame>b__35_0()
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.IsVisible = true;
				}
			}

			// Token: 0x06000556 RID: 1366 RVA: 0x0005691D File Offset: 0x00054B1D
			internal void <HideActivityFrame>b__36_0()
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.IsVisible = false;
				}
			}

			// Token: 0x06000557 RID: 1367 RVA: 0x00056942 File Offset: 0x00054B42
			internal void <StartConnection>b__38_0()
			{
				ProfileUpdater.ForceUpdate();
			}

			// Token: 0x06000558 RID: 1368 RVA: 0x00056949 File Offset: 0x00054B49
			internal bool <StartConnection>b__38_11(PID x)
			{
				return x.IsAvailable && !(x is PID_SupportedPids);
			}

			// Token: 0x06000559 RID: 1369 RVA: 0x00056961 File Offset: 0x00054B61
			internal bool <UpdateMainButtons>b__56_1(MainPageConfigurationProxyItem x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600055A RID: 1370 RVA: 0x00056969 File Offset: 0x00054B69
			internal bool <UpdateMainButtons>b__56_2(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.Purchase;
			}

			// Token: 0x0600055B RID: 1371 RVA: 0x00056969 File Offset: 0x00054B69
			internal bool <UpdateMainButtons>b__56_7(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.Purchase;
			}

			// Token: 0x0600055C RID: 1372 RVA: 0x00056975 File Offset: 0x00054B75
			internal bool <UpdateMainButtons>b__56_3(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.FuelStatistics;
			}

			// Token: 0x0600055D RID: 1373 RVA: 0x00056975 File Offset: 0x00054B75
			internal bool <UpdateMainButtons>b__56_4(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.FuelStatistics;
			}

			// Token: 0x0600055E RID: 1374 RVA: 0x00056981 File Offset: 0x00054B81
			internal bool <UpdateMainButtons>b__56_5(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.Settings;
			}

			// Token: 0x0600055F RID: 1375 RVA: 0x00056981 File Offset: 0x00054B81
			internal bool <UpdateMainButtons>b__56_6(MainPageConfigurationProxyItem x)
			{
				return x.ButtonType == MainPageButtons.Settings;
			}

			// Token: 0x040003D9 RID: 985
			public static readonly SimpleMainPage.<>c <>9 = new SimpleMainPage.<>c();

			// Token: 0x040003DA RID: 986
			public static Func<MainPageConfigurationProxyItem, bool> <>9__12_5;

			// Token: 0x040003DB RID: 987
			public static Action <>9__18_0;

			// Token: 0x040003DC RID: 988
			public static Action <>9__20_0;

			// Token: 0x040003DD RID: 989
			public static Func<PID, bool> <>9__25_0;

			// Token: 0x040003DE RID: 990
			public static Func<PID, SensorPID> <>9__25_1;

			// Token: 0x040003DF RID: 991
			public static Func<View, bool> <>9__34_1;

			// Token: 0x040003E0 RID: 992
			public static Action <>9__35_0;

			// Token: 0x040003E1 RID: 993
			public static Action <>9__36_0;

			// Token: 0x040003E2 RID: 994
			public static Action <>9__38_0;

			// Token: 0x040003E3 RID: 995
			public static Func<PID, bool> <>9__38_11;

			// Token: 0x040003E4 RID: 996
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_1;

			// Token: 0x040003E5 RID: 997
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_2;

			// Token: 0x040003E6 RID: 998
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_7;

			// Token: 0x040003E7 RID: 999
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_3;

			// Token: 0x040003E8 RID: 1000
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_4;

			// Token: 0x040003E9 RID: 1001
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_5;

			// Token: 0x040003EA RID: 1002
			public static Func<MainPageConfigurationProxyItem, bool> <>9__56_6;
		}

		// Token: 0x02000105 RID: 261
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06000560 RID: 1376 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06000561 RID: 1377 RVA: 0x00056990 File Offset: 0x00054B90
			internal void <Initialize>b__0()
			{
				Task.Delay(500).Wait();
				try
				{
					DashboardItem.RegisterDashboardItemTypes();
				}
				catch
				{
				}
				try
				{
					this.<>4__this.UpdateVINInfo();
				}
				catch
				{
				}
				bool isMainThread = MainThread.IsMainThread;
				CarData currentCarData = App.OBDReader.CurrentCarData;
				if (currentCarData != null)
				{
					currentCarData.CreateEmptyPIDS();
				}
				try
				{
					LiveDataPIDModel.LoadCustomPIDS();
				}
				catch (Exception)
				{
				}
				this.requires_update = ProfileUpdater.CheckRequiresUpdate();
				if (this.requires_update)
				{
					if (!VersionChecker.IsVersionNewer(SharedSettings.Current.LatestVersion, "1.95.1"))
					{
						MainPageConfigurationModel mainPageConfigurationModel = new MainPageConfigurationModel();
						mainPageConfigurationModel.Configuration.FirstOrDefault((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.VersionInfo).IsVisible = true;
						mainPageConfigurationModel.Save();
						this.<>4__this.UpdateMainButtons();
					}
					Action action;
					if ((action = this.<>9__3) == null)
					{
						action = (this.<>9__3 = delegate
						{
							this.<>4__this.activityFrame.IsVisible = true;
						});
					}
					Device.BeginInvokeOnMainThread(action);
					ProfileUpdater.PerformUpdate();
					VagDTCDecoder.DeleteContainers();
					Action action2;
					if ((action2 = this.<>9__4) == null)
					{
						action2 = (this.<>9__4 = delegate
						{
							this.<>4__this.activityFrame.IsVisible = false;
						});
					}
					Device.BeginInvokeOnMainThread(action2);
				}
				this.<>4__this.RemoveCSV();
			}

			// Token: 0x06000562 RID: 1378 RVA: 0x00056AE4 File Offset: 0x00054CE4
			internal void <Initialize>b__3()
			{
				this.<>4__this.activityFrame.IsVisible = true;
			}

			// Token: 0x06000563 RID: 1379 RVA: 0x00056AF7 File Offset: 0x00054CF7
			internal void <Initialize>b__4()
			{
				this.<>4__this.activityFrame.IsVisible = false;
			}

			// Token: 0x06000564 RID: 1380 RVA: 0x00056B0A File Offset: 0x00054D0A
			internal void <Initialize>b__1()
			{
				this.<>4__this.ConnectOnLaunch();
			}

			// Token: 0x06000565 RID: 1381 RVA: 0x00056B18 File Offset: 0x00054D18
			internal async Task <Initialize>b__2()
			{
				await this.<>4__this.CheckForNewVersion();
				await OnlinePatch.GetPatch();
				if (SharedSettings.Current.AllowProfilesBackgroundUpdate)
				{
					await ProfileV2Model.DoBackgroundUpdate();
				}
			}

			// Token: 0x040003EB RID: 1003
			public SimpleMainPage <>4__this;

			// Token: 0x040003EC RID: 1004
			public bool requires_update;

			// Token: 0x040003ED RID: 1005
			public Action <>9__3;

			// Token: 0x040003EE RID: 1006
			public Action <>9__4;

			// Token: 0x02000106 RID: 262
			[StructLayout(LayoutKind.Auto)]
			private struct <<Initialize>b__2>d : IAsyncStateMachine
			{
				// Token: 0x06000566 RID: 1382 RVA: 0x00056B5C File Offset: 0x00054D5C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass12_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00D3;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0136;
						}
						default:
							taskAwaiter = CS$<>8__locals1.<>4__this.CheckForNewVersion().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter = OnlinePatch.GetPatch().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00D3:
						taskAwaiter.GetResult();
						if (!SharedSettings.Current.AllowProfilesBackgroundUpdate)
						{
							goto IL_013D;
						}
						taskAwaiter = ProfileV2Model.DoBackgroundUpdate().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_0136:
						taskAwaiter.GetResult();
						IL_013D:;
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

				// Token: 0x06000567 RID: 1383 RVA: 0x00056CF0 File Offset: 0x00054EF0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040003EF RID: 1007
				public int <>1__state;

				// Token: 0x040003F0 RID: 1008
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040003F1 RID: 1009
				public SimpleMainPage.<>c__DisplayClass12_0 <>4__this;

				// Token: 0x040003F2 RID: 1010
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x02000107 RID: 263
		[CompilerGenerated]
		private sealed class <>c__DisplayClass37_0
		{
			// Token: 0x06000568 RID: 1384 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass37_0()
			{
			}

			// Token: 0x06000569 RID: 1385 RVA: 0x00056CFE File Offset: 0x00054EFE
			internal void <SetActivityFrameText>b__0()
			{
				if (SimpleMainPage.Instance != null && SimpleMainPage.Instance.activityFrame != null)
				{
					SimpleMainPage.Instance.activityFrame.Text = this.text;
				}
			}

			// Token: 0x040003F3 RID: 1011
			public string text;
		}

		// Token: 0x02000108 RID: 264
		[CompilerGenerated]
		private sealed class <>c__DisplayClass38_0
		{
			// Token: 0x0600056A RID: 1386 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass38_0()
			{
			}

			// Token: 0x0600056B RID: 1387 RVA: 0x00056D28 File Offset: 0x00054F28
			internal void <StartConnection>b__1()
			{
				this.<>4__this.connectionProgressFrame.Text = Translate.GetString("ios_ConnectingToELM");
				if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
				{
					if (string.IsNullOrEmpty(this.wifi_name))
					{
						if (this.wifiHelper.HasLocationPermission())
						{
							this.<>4__this.connectionProgressFrame.Text = this.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("ios_NoWiFi");
						}
					}
					else
					{
						this.<>4__this.connectionProgressFrame.Text = this.<>4__this.connectionProgressFrame.Text + "\r\nWiFi: " + this.wifi_name;
					}
					if (PlatformHelper.IsAndroid)
					{
						this.<>4__this.connectionProgressFrame.Text = this.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("droid_DisableMobileData");
					}
				}
				else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
				{
					if (!string.IsNullOrEmpty(SharedSettings.Current.BTDeviceName))
					{
						this.<>4__this.connectionProgressFrame.Text = this.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTDeviceName;
					}
				}
				else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && !string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
				{
					this.<>4__this.connectionProgressFrame.Text = this.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTLEDeviceName;
				}
				this.<>4__this.connectionProgressFrame.IsVisible = true;
			}

			// Token: 0x0600056C RID: 1388 RVA: 0x00056EDC File Offset: 0x000550DC
			internal async Task <StartConnection>b__2()
			{
				bool flag = await App.OBDReader.Connect(true);
				this.connected = flag;
			}

			// Token: 0x0600056D RID: 1389 RVA: 0x00056F20 File Offset: 0x00055120
			internal async Task <StartConnection>b__4()
			{
				bool flag = await App.OBDReader.Connect(true);
				this.connected = flag;
			}

			// Token: 0x0600056E RID: 1390 RVA: 0x00056F64 File Offset: 0x00055164
			internal async Task <StartConnection>b__6()
			{
				TaskAwaiter<bool> taskAwaiter = this.<>4__this.DisplayAlert(Translate.GetString("ios_CANOptimizationTitle"), Translate.GetString("ios_CANOptimizationText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					SharedSettings.Current.CANOptimizeRequests = true;
					SharedSettings.Current.CANOptimizeMode22 = true;
				}
			}

			// Token: 0x0600056F RID: 1391 RVA: 0x00056FA8 File Offset: 0x000551A8
			internal async Task <StartConnection>b__7()
			{
				try
				{
					await this.<>4__this.DisplayAlert(Translate.GetString("ios_RPMFixWarning_Title"), Translate.GetString("ios_RPMFixWarning_Text"), "OK");
				}
				catch
				{
				}
			}

			// Token: 0x06000570 RID: 1392 RVA: 0x00056FEC File Offset: 0x000551EC
			internal async Task <StartConnection>b__3()
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM && !App.OBDReader.DisconnectRequested && this.InitECU)
				{
					try
					{
						await this.<>4__this.DisplayAlert(Translate.GetString("MainPage_ConnectionTroubleshootingTitle"), Translate.GetString("MainPage_ConnectionTroubleshootingTextNoECU"), "OK", Translate.GetString("btnCancel.Content"));
					}
					catch
					{
					}
				}
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected && !App.OBDReader.DisconnectRequested)
				{
					try
					{
						string text = Translate.GetString("MainPage_ConnectionTroubleshootingTextNoELM");
						if (PlatformHelper.IsiOS)
						{
							if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi && PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
							{
								text = string.Concat(new string[]
								{
									string.Format(Translate.GetString("ios14Warning"), DeviceInfo.VersionString),
									"\n",
									Translate.GetString("ios14WarningWiFi"),
									"\n",
									text
								});
							}
							else if ((SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0))
							{
								text = string.Concat(new string[]
								{
									string.Format(Translate.GetString("ios14Warning"), DeviceInfo.VersionString),
									"\n",
									Translate.GetString("ios14WarningBluetooth"),
									"\n",
									text
								});
							}
						}
						TaskAwaiter<bool> taskAwaiter = this.<>4__this.DisplayAlert(Translate.GetString("MainPage_ConnectionTroubleshootingTitle"), text, "OK", Translate.GetString("ios_WiFiTroubleshooting")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
							{
								Launcher.TryOpenAsync("https://www.carscanner.info/ios-wifi-troubleshooting/");
							}
							else
							{
								Launcher.TryOpenAsync("https://www.carscanner.info/bluetooth-troubleshooting/");
							}
						}
					}
					catch
					{
					}
					BluetoothHelper.TurnOffBluetoothIfNeedTo();
				}
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && SharedSettings.Current.UseDefaultInit && (SharedSettings.Current.SelectedBrand == "Nissan" || SharedSettings.Current.SelectedBrand == "Infiniti") && (App.OBDReader.CurrentProtocolNumber == 3 || App.OBDReader.CurrentProtocolNumber == 4 || App.OBDReader.CurrentProtocolNumber == 5))
				{
					if (App.OBDReader.CurrentCarData.LiveDataPIDs.Count((PID x) => x.IsAvailable && !(x is PID_SupportedPids)) <= 2)
					{
						try
						{
							await this.<>4__this.DisplayAlert("Nissan Consult II", Translate.GetString("MainPage_NissanDetected"), "OK");
						}
						catch
						{
						}
					}
				}
				if ((App.OBDReader.CurrentELMFormat == ELMFormat.KWP) & SharedSettings.Current.BrandAndProfile.Contains("CAN"))
				{
					try
					{
						await this.<>4__this.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_CanWarningText"), StaticLists.Protocols[App.OBDReader.CurrentProtocolNumber]), "OK");
					}
					catch
					{
					}
				}
			}

			// Token: 0x040003F4 RID: 1012
			public SimpleMainPage <>4__this;

			// Token: 0x040003F5 RID: 1013
			public string wifi_name;

			// Token: 0x040003F6 RID: 1014
			public IWiFiHelper wifiHelper;

			// Token: 0x040003F7 RID: 1015
			public bool connected;

			// Token: 0x040003F8 RID: 1016
			public bool InitECU;

			// Token: 0x02000109 RID: 265
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__2>d : IAsyncStateMachine
			{
				// Token: 0x06000571 RID: 1393 RVA: 0x00057030 File Offset: 0x00055230
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.OBDReader.Connect(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__2>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.connected = result;
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

				// Token: 0x06000572 RID: 1394 RVA: 0x000570F4 File Offset: 0x000552F4
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040003F9 RID: 1017
				public int <>1__state;

				// Token: 0x040003FA RID: 1018
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040003FB RID: 1019
				public SimpleMainPage.<>c__DisplayClass38_0 <>4__this;

				// Token: 0x040003FC RID: 1020
				private TaskAwaiter<bool> <>u__1;
			}

			// Token: 0x0200010A RID: 266
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__3>d : IAsyncStateMachine
			{
				// Token: 0x06000573 RID: 1395 RVA: 0x00057104 File Offset: 0x00055304
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = this;
					try
					{
						switch (num)
						{
						case 0:
							break;
						case 1:
							IL_00FD:
							try
							{
								TaskAwaiter<bool> taskAwaiter3;
								if (num != 1)
								{
									string text = Translate.GetString("MainPage_ConnectionTroubleshootingTextNoELM");
									if (PlatformHelper.IsiOS)
									{
										if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi && PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
										{
											text = string.Concat(new string[]
											{
												string.Format(Translate.GetString("ios14Warning"), DeviceInfo.VersionString),
												"\n",
												Translate.GetString("ios14WarningWiFi"),
												"\n",
												text
											});
										}
										else if ((SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0))
										{
											text = string.Concat(new string[]
											{
												string.Format(Translate.GetString("ios14Warning"), DeviceInfo.VersionString),
												"\n",
												Translate.GetString("ios14WarningBluetooth"),
												"\n",
												text
											});
										}
									}
									taskAwaiter3 = CS$<>8__locals1.<>4__this.DisplayAlert(Translate.GetString("MainPage_ConnectionTroubleshootingTitle"), text, "OK", Translate.GetString("ios_WiFiTroubleshooting")).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 1);
										taskAwaiter2 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter3 = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
									num = (num2 = -1);
								}
								if (!taskAwaiter3.GetResult())
								{
									if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
									{
										Launcher.TryOpenAsync("https://www.carscanner.info/ios-wifi-troubleshooting/");
									}
									else
									{
										Launcher.TryOpenAsync("https://www.carscanner.info/bluetooth-troubleshooting/");
									}
								}
							}
							catch
							{
							}
							BluetoothHelper.TurnOffBluetoothIfNeedTo();
							goto IL_0288;
						case 2:
							IL_0339:
							try
							{
								TaskAwaiter taskAwaiter4;
								if (num != 2)
								{
									taskAwaiter4 = CS$<>8__locals1.<>4__this.DisplayAlert("Nissan Consult II", Translate.GetString("MainPage_NissanDetected"), "OK").GetAwaiter();
									if (!taskAwaiter4.IsCompleted)
									{
										num = (num2 = 2);
										TaskAwaiter taskAwaiter5 = taskAwaiter4;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref taskAwaiter4, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter taskAwaiter5;
									taskAwaiter4 = taskAwaiter5;
									taskAwaiter5 = default(TaskAwaiter);
									num = (num2 = -1);
								}
								taskAwaiter4.GetResult();
							}
							catch
							{
							}
							goto IL_03BA;
						case 3:
							IL_03E1:
							try
							{
								TaskAwaiter taskAwaiter4;
								if (num != 3)
								{
									taskAwaiter4 = CS$<>8__locals1.<>4__this.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_CanWarningText"), StaticLists.Protocols[App.OBDReader.CurrentProtocolNumber]), "OK").GetAwaiter();
									if (!taskAwaiter4.IsCompleted)
									{
										num = (num2 = 3);
										TaskAwaiter taskAwaiter5 = taskAwaiter4;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref taskAwaiter4, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter taskAwaiter5;
									taskAwaiter4 = taskAwaiter5;
									taskAwaiter5 = default(TaskAwaiter);
									num = (num2 = -1);
								}
								taskAwaiter4.GetResult();
							}
							catch
							{
							}
							goto IL_047D;
						default:
							if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToELM || App.OBDReader.DisconnectRequested || !CS$<>8__locals1.InitECU)
							{
								goto IL_00DD;
							}
							break;
						}
						try
						{
							TaskAwaiter<bool> taskAwaiter3;
							if (num != 0)
							{
								taskAwaiter3 = CS$<>8__locals1.<>4__this.DisplayAlert(Translate.GetString("MainPage_ConnectionTroubleshootingTitle"), Translate.GetString("MainPage_ConnectionTroubleshootingTextNoECU"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 0);
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num = (num2 = -1);
							}
							taskAwaiter3.GetResult();
						}
						catch
						{
						}
						IL_00DD:
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected && !App.OBDReader.DisconnectRequested)
						{
							goto IL_00FD;
						}
						IL_0288:
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && SharedSettings.Current.UseDefaultInit && (SharedSettings.Current.SelectedBrand == "Nissan" || SharedSettings.Current.SelectedBrand == "Infiniti") && (App.OBDReader.CurrentProtocolNumber == 3 || App.OBDReader.CurrentProtocolNumber == 4 || App.OBDReader.CurrentProtocolNumber == 5))
						{
							if (App.OBDReader.CurrentCarData.LiveDataPIDs.Count((PID x) => x.IsAvailable && !(x is PID_SupportedPids)) <= 2)
							{
								goto IL_0339;
							}
						}
						IL_03BA:
						if ((App.OBDReader.CurrentELMFormat == ELMFormat.KWP) & SharedSettings.Current.BrandAndProfile.Contains("CAN"))
						{
							goto IL_03E1;
						}
						IL_047D:;
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

				// Token: 0x06000574 RID: 1396 RVA: 0x00057638 File Offset: 0x00055838
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040003FD RID: 1021
				public int <>1__state;

				// Token: 0x040003FE RID: 1022
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040003FF RID: 1023
				public SimpleMainPage.<>c__DisplayClass38_0 <>4__this;

				// Token: 0x04000400 RID: 1024
				private TaskAwaiter<bool> <>u__1;

				// Token: 0x04000401 RID: 1025
				private TaskAwaiter <>u__2;
			}

			// Token: 0x0200010B RID: 267
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__4>d : IAsyncStateMachine
			{
				// Token: 0x06000575 RID: 1397 RVA: 0x00057648 File Offset: 0x00055848
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.OBDReader.Connect(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__4>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.connected = result;
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

				// Token: 0x06000576 RID: 1398 RVA: 0x0005770C File Offset: 0x0005590C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000402 RID: 1026
				public int <>1__state;

				// Token: 0x04000403 RID: 1027
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000404 RID: 1028
				public SimpleMainPage.<>c__DisplayClass38_0 <>4__this;

				// Token: 0x04000405 RID: 1029
				private TaskAwaiter<bool> <>u__1;
			}

			// Token: 0x0200010C RID: 268
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__6>d : IAsyncStateMachine
			{
				// Token: 0x06000577 RID: 1399 RVA: 0x0005771C File Offset: 0x0005591C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter3;
						if (num != 0)
						{
							taskAwaiter3 = CS$<>8__locals1.<>4__this.DisplayAlert(Translate.GetString("ios_CANOptimizationTitle"), Translate.GetString("ios_CANOptimizationText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__6>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						if (taskAwaiter3.GetResult())
						{
							SharedSettings.Current.CANOptimizeRequests = true;
							SharedSettings.Current.CANOptimizeMode22 = true;
						}
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

				// Token: 0x06000578 RID: 1400 RVA: 0x00057814 File Offset: 0x00055A14
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000406 RID: 1030
				public int <>1__state;

				// Token: 0x04000407 RID: 1031
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000408 RID: 1032
				public SimpleMainPage.<>c__DisplayClass38_0 <>4__this;

				// Token: 0x04000409 RID: 1033
				private TaskAwaiter<bool> <>u__1;
			}

			// Token: 0x0200010D RID: 269
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__7>d : IAsyncStateMachine
			{
				// Token: 0x06000579 RID: 1401 RVA: 0x00057824 File Offset: 0x00055A24
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = CS$<>8__locals1.<>4__this.DisplayAlert(Translate.GetString("ios_RPMFixWarning_Title"), Translate.GetString("ios_RPMFixWarning_Text"), "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__7>d>(ref taskAwaiter, ref this);
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
						}
						catch
						{
						}
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

				// Token: 0x0600057A RID: 1402 RVA: 0x00057908 File Offset: 0x00055B08
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400040A RID: 1034
				public int <>1__state;

				// Token: 0x0400040B RID: 1035
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x0400040C RID: 1036
				public SimpleMainPage.<>c__DisplayClass38_0 <>4__this;

				// Token: 0x0400040D RID: 1037
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x0200010E RID: 270
		[CompilerGenerated]
		private sealed class <>c__DisplayClass38_1
		{
			// Token: 0x0600057B RID: 1403 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass38_1()
			{
			}

			// Token: 0x0600057C RID: 1404 RVA: 0x00057918 File Offset: 0x00055B18
			internal async Task <StartConnection>b__5()
			{
				bool flag = await App.OBDReader.Initialize(this.attempts, OBDDataReader.InitModes.Default, this.initprogress);
				this.initResult = flag;
			}

			// Token: 0x0400040E RID: 1038
			public int attempts;

			// Token: 0x0400040F RID: 1039
			public Progress<string> initprogress;

			// Token: 0x04000410 RID: 1040
			public bool initResult;

			// Token: 0x04000411 RID: 1041
			public SimpleMainPage.<>c__DisplayClass38_0 CS$<>8__locals1;

			// Token: 0x0200010F RID: 271
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__5>d : IAsyncStateMachine
			{
				// Token: 0x0600057D RID: 1405 RVA: 0x0005795C File Offset: 0x00055B5C
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SimpleMainPage.<>c__DisplayClass38_1 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.OBDReader.Initialize(CS$<>8__locals1.attempts, OBDDataReader.InitModes.Default, CS$<>8__locals1.initprogress).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<>c__DisplayClass38_1.<<StartConnection>b__5>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.initResult = result;
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

				// Token: 0x0600057E RID: 1406 RVA: 0x00057A2C File Offset: 0x00055C2C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000412 RID: 1042
				public int <>1__state;

				// Token: 0x04000413 RID: 1043
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000414 RID: 1044
				public SimpleMainPage.<>c__DisplayClass38_1 <>4__this;

				// Token: 0x04000415 RID: 1045
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x02000110 RID: 272
		[CompilerGenerated]
		private sealed class <>c__DisplayClass38_2
		{
			// Token: 0x0600057F RID: 1407 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass38_2()
			{
			}

			// Token: 0x06000580 RID: 1408 RVA: 0x00057A3C File Offset: 0x00055C3C
			internal async void <StartConnection>b__8(object cancelSender, EventArgs cancelArgs)
			{
				this.cts.Cancel();
				App.OBDReader.ReplaceQueue(new OBDRequest[0]);
			}

			// Token: 0x06000581 RID: 1409 RVA: 0x00057A73 File Offset: 0x00055C73
			internal void <StartConnection>b__9(int i)
			{
				MainThreadHelper.InvokeOnMainThread(new Action(new SimpleMainPage.<>c__DisplayClass38_3
				{
					CS$<>8__locals3 = this,
					i = i
				}.<StartConnection>b__10));
			}

			// Token: 0x04000416 RID: 1046
			public CancellationTokenSource cts;

			// Token: 0x04000417 RID: 1047
			public string detectingString;

			// Token: 0x04000418 RID: 1048
			public SimpleMainPage.<>c__DisplayClass38_1 CS$<>8__locals2;

			// Token: 0x02000111 RID: 273
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnection>b__8>d : IAsyncStateMachine
			{
				// Token: 0x06000582 RID: 1410 RVA: 0x00057A98 File Offset: 0x00055C98
				void IAsyncStateMachine.MoveNext()
				{
					SimpleMainPage.<>c__DisplayClass38_2 CS$<>8__locals1 = this;
					try
					{
						CS$<>8__locals1.cts.Cancel();
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
					catch (Exception ex)
					{
						this.<>1__state = -2;
						this.<>t__builder.SetException(ex);
						return;
					}
					this.<>1__state = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x06000583 RID: 1411 RVA: 0x00057B04 File Offset: 0x00055D04
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000419 RID: 1049
				public int <>1__state;

				// Token: 0x0400041A RID: 1050
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x0400041B RID: 1051
				public SimpleMainPage.<>c__DisplayClass38_2 <>4__this;
			}
		}

		// Token: 0x02000112 RID: 274
		[CompilerGenerated]
		private sealed class <>c__DisplayClass38_3
		{
			// Token: 0x06000584 RID: 1412 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass38_3()
			{
			}

			// Token: 0x06000585 RID: 1413 RVA: 0x00057B14 File Offset: 0x00055D14
			internal void <StartConnection>b__10()
			{
				this.CS$<>8__locals3.CS$<>8__locals2.CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = string.Concat(new string[]
				{
					this.CS$<>8__locals3.detectingString,
					"\n",
					SharedSettings.Current.SelectedBrand,
					" ",
					SharedSettings.Current.SelectedProfileV2Name,
					"\n",
					this.i.ToString(),
					"%"
				});
			}

			// Token: 0x0400041C RID: 1052
			public int i;

			// Token: 0x0400041D RID: 1053
			public SimpleMainPage.<>c__DisplayClass38_2 CS$<>8__locals3;
		}

		// Token: 0x02000113 RID: 275
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x06000586 RID: 1414 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x06000587 RID: 1415 RVA: 0x00057BA1 File Offset: 0x00055DA1
			internal void <Initprogress_ProgressChanged>b__0()
			{
				this.<>4__this.connectionProgressFrame.Text = this.e;
			}

			// Token: 0x0400041E RID: 1054
			public SimpleMainPage <>4__this;

			// Token: 0x0400041F RID: 1055
			public string e;
		}

		// Token: 0x02000114 RID: 276
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskForExit>d__20 : IAsyncStateMachine
		{
			// Token: 0x06000588 RID: 1416 RVA: 0x00057BBC File Offset: 0x00055DBC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
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
							goto IL_0141;
						}
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_0158;
						}
						taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("droid_Exit"), "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<AskForExit>d__20>(ref taskAwaiter5, ref this);
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
						goto IL_0158;
					}
					SharedSettings.Current.LastException = "";
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
					{
						simpleMainPage.btnDisconnect_Clicked(simpleMainPage, new EventArgs());
					}
					Task.Run(delegate
					{
						IPlatformSpecificServiceDroid droidService2 = PlatformHelper.DroidService;
						if (droidService2 == null)
						{
							return;
						}
						droidService2.AndroidHelper_StopService();
					});
					taskAwaiter3 = Task.Delay(300).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<AskForExit>d__20>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0141:
					taskAwaiter3.GetResult();
					IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
					if (droidService != null)
					{
						droidService.KillApp();
					}
					IL_0158:;
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

			// Token: 0x06000589 RID: 1417 RVA: 0x00057D6C File Offset: 0x00055F6C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000420 RID: 1056
			public int <>1__state;

			// Token: 0x04000421 RID: 1057
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000422 RID: 1058
			public SimpleMainPage <>4__this;

			// Token: 0x04000423 RID: 1059
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000424 RID: 1060
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000115 RID: 277
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskForReview>d__41 : IAsyncStateMachine
		{
			// Token: 0x0600058A RID: 1418 RVA: 0x00057D7C File Offset: 0x00055F7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (SharedSettings.Current.ReviewReceived || SharedSettings.Current.FreePeriodGoodConnections <= SharedSettings.Current.AskForReviewGoodConnections)
						{
							goto IL_00F0;
						}
						taskAwaiter3 = simpleMainPage.DisplayAlert(Translate.GetString("ios_AskForRating_Title"), Translate.GetString("ios_AboutRatings"), Translate.GetString("ios_Yes"), Translate.GetString("ios_Later")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<AskForReview>d__41>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						SharedSettings.Current.ReviewReceived = true;
						try
						{
							DependencyService.Get<IRequestReview>(0).Request(false);
							goto IL_00F0;
						}
						catch (Exception)
						{
							goto IL_00F0;
						}
					}
					SharedSettings.Current.AskForReviewGoodConnections += 12;
					IL_00F0:;
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

			// Token: 0x0600058B RID: 1419 RVA: 0x00057EC4 File Offset: 0x000560C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000425 RID: 1061
			public int <>1__state;

			// Token: 0x04000426 RID: 1062
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000427 RID: 1063
			public SimpleMainPage <>4__this;

			// Token: 0x04000428 RID: 1064
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000116 RID: 278
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckForNewVersion>d__13 : IAsyncStateMachine
		{
			// Token: 0x0600058C RID: 1420 RVA: 0x00057ED4 File Offset: 0x000560D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = VersionChecker.CheckVersion().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<CheckForNewVersion>d__13>(ref taskAwaiter, ref this);
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
					Device.BeginInvokeOnMainThread(delegate
					{
						if (VersionChecker.ShouldShowNewVersionAvailable)
						{
							simpleMainPage.labelNewVersion.IsVisible = true;
							return;
						}
						simpleMainPage.labelNewVersion.IsVisible = false;
					});
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

			// Token: 0x0600058D RID: 1421 RVA: 0x00057F98 File Offset: 0x00056198
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000429 RID: 1065
			public int <>1__state;

			// Token: 0x0400042A RID: 1066
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400042B RID: 1067
			public SimpleMainPage <>4__this;

			// Token: 0x0400042C RID: 1068
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000117 RID: 279
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckWiFiConnection>d__39 : IAsyncStateMachine
		{
			// Token: 0x0600058E RID: 1422 RVA: 0x00057FA8 File Offset: 0x000561A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01EA;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0263;
					}
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0356;
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_03F8;
					case 5:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0601;
					case 6:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_069B;
					default:
						wifi_name = wifiHelper.GetWiFiName();
						if (SharedSettings.Current.NoWiFiWarning || !SharedSettings.Current.TryConnectToLastWiFiNetwork)
						{
							text = wifi_name;
							goto IL_06DD;
						}
						if (wifiHelper.HasLocationPermission() && string.IsNullOrEmpty(wifi_name) && string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName))
						{
							if (!SharedSettings.Current.NoWiFiWarning)
							{
								goto IL_06B4;
							}
							taskAwaiter3 = simpleMainPage.DisplayAlert(Translate.GetString("ios_NoWiFiWarning_Title"), Translate.GetString("ios_NoWiFiWarning_Text"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (string.IsNullOrEmpty(wifi_name) && !string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) && SharedSettings.Current.TryConnectToLastWiFiNetwork)
						{
							simpleMainPage.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
							simpleMainPage.connectionProgressFrame.IsVisible = true;
							taskAwaiter5 = wifiHelper.ConnectToNetwork(SharedSettings.Current.LastWiFiName).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_01EA;
						}
						else if (!string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) && !string.IsNullOrEmpty(wifi_name) && wifi_name != "<unknown ssid>" && SharedSettings.Current.LastWiFiName != wifi_name)
						{
							taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ChangeWiFiQuestion"), wifi_name, SharedSettings.Current.LastWiFiName), Translate.GetString("ios_ChangeWiFiYes"), Translate.GetString("ios_ChangeWiFiNo")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 3;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0356;
						}
						else
						{
							if (string.IsNullOrEmpty(wifi_name) || !(wifi_name != "<unknown ssid>") || !string.IsNullOrEmpty(SharedSettings.Current.LastWiFiName) || wifi_name.Contains("obd", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("vlink", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("vgate", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("v-link", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("konnwei", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("nexpeak", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("scantool", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("elm", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("viecar", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("carly", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("obdclick", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("kiwi", StringComparison.InvariantCultureIgnoreCase) || wifi_name.Contains("ancel", StringComparison.InvariantCultureIgnoreCase))
							{
								goto IL_06B4;
							}
							taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_WrongNetwork"), wifi_name, SharedSettings.Current.LastWiFiName), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 5;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0601;
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_06B4;
					IL_01EA:
					if (taskAwaiter5.GetResult())
					{
						goto IL_06B4;
					}
					taskAwaiter3 = simpleMainPage.DisplayAlert(Translate.GetString("ios_NoWiFiWarning_Title"), Translate.GetString("ios_NoWiFiWarning_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0263:
					taskAwaiter3.GetResult();
					goto IL_06B4;
					IL_0356:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_06B4;
					}
					simpleMainPage.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
					simpleMainPage.connectionProgressFrame.IsVisible = true;
					taskAwaiter5 = wifiHelper.ConnectToNetwork(SharedSettings.Current.LastWiFiName).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 4;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter5, ref this);
						return;
					}
					IL_03F8:
					taskAwaiter5.GetResult();
					wifi_name = wifiHelper.GetWiFiName();
					goto IL_06B4;
					IL_0601:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_06B4;
					}
					simpleMainPage.connectionProgressFrame.Text = string.Format(Translate.GetString("ios_ConnectingToWiFiNetwork"), SharedSettings.Current.LastWiFiName);
					simpleMainPage.connectionProgressFrame.IsVisible = true;
					taskAwaiter5 = wifiHelper.ConnectToNetwork("WiFi_OBDII").GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 6;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<CheckWiFiConnection>d__39>(ref taskAwaiter5, ref this);
						return;
					}
					IL_069B:
					taskAwaiter5.GetResult();
					wifi_name = wifiHelper.GetWiFiName();
					IL_06B4:
					text = wifi_name;
				}
				catch (Exception ex)
				{
					num2 = -2;
					wifi_name = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_06DD:
				num2 = -2;
				wifi_name = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600058F RID: 1423 RVA: 0x000586CC File Offset: 0x000568CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400042D RID: 1069
			public int <>1__state;

			// Token: 0x0400042E RID: 1070
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400042F RID: 1071
			public IWiFiHelper wifiHelper;

			// Token: 0x04000430 RID: 1072
			public SimpleMainPage <>4__this;

			// Token: 0x04000431 RID: 1073
			private string <wifi_name>5__2;

			// Token: 0x04000432 RID: 1074
			private TaskAwaiter <>u__1;

			// Token: 0x04000433 RID: 1075
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000118 RID: 280
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ConnectOnLaunch>d__21 : IAsyncStateMachine
		{
			// Token: 0x06000590 RID: 1424 RVA: 0x000586DC File Offset: 0x000568DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00FE;
						}
						if (!SharedSettings.Current.FirstConnectionAttempted)
						{
							goto IL_00AD;
						}
					}
					try
					{
						if (num != 0)
						{
							taskAwaiter = simpleMainPage.StartConnection(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ConnectOnLaunch>d__21>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						if (SharedSettings.Current.OpenDashboardOnLaunch && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
						{
							simpleMainPage.btnDashboard_Clicked(null, null);
						}
						goto IL_0105;
					}
					catch (Exception)
					{
						goto IL_0105;
					}
					IL_00AD:
					taskAwaiter = simpleMainPage.ShowWelcomePage2().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ConnectOnLaunch>d__21>(ref taskAwaiter, ref this);
						return;
					}
					IL_00FE:
					taskAwaiter.GetResult();
					IL_0105:;
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

			// Token: 0x06000591 RID: 1425 RVA: 0x00058838 File Offset: 0x00056A38
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000434 RID: 1076
			public int <>1__state;

			// Token: 0x04000435 RID: 1077
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000436 RID: 1078
			public SimpleMainPage <>4__this;

			// Token: 0x04000437 RID: 1079
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000119 RID: 281
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Initialize>d__12 : IAsyncStateMachine
		{
			// Token: 0x06000592 RID: 1426 RVA: 0x00058848 File Offset: 0x00056A48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new SimpleMainPage.<>c__DisplayClass12_0();
						CS$<>8__locals1.<>4__this = this;
						App.OBDReader.StatusChanged -= simpleMainPage.OBD_StatusChanged;
						App.OBDReader.StatusChanged += simpleMainPage.OBD_StatusChanged;
						simpleMainPage.cbECUID.SelectedIndexChanged -= simpleMainPage.cbECUID_SelectedIndexChanged;
						simpleMainPage.cbECUID.SelectedIndexChanged += simpleMainPage.cbECUID_SelectedIndexChanged;
						simpleMainPage.ad.IsVisible = false;
						CS$<>8__locals1.requires_update = false;
						simpleMainPage.btnStartSimulation.IsEnabled = false;
						simpleMainPage.btnConnect.IsEnabled = false;
						simpleMainPage.btnSettings.IsEnabled = false;
						simpleMainPage.activityFrame.IsVisible = true;
						taskAwaiter = Task.Run(delegate
						{
							Task.Delay(500).Wait();
							try
							{
								DashboardItem.RegisterDashboardItemTypes();
							}
							catch
							{
							}
							try
							{
								CS$<>8__locals1.<>4__this.UpdateVINInfo();
							}
							catch
							{
							}
							bool isMainThread = MainThread.IsMainThread;
							CarData currentCarData = App.OBDReader.CurrentCarData;
							if (currentCarData != null)
							{
								currentCarData.CreateEmptyPIDS();
							}
							try
							{
								LiveDataPIDModel.LoadCustomPIDS();
							}
							catch (Exception)
							{
							}
							CS$<>8__locals1.requires_update = ProfileUpdater.CheckRequiresUpdate();
							if (CS$<>8__locals1.requires_update)
							{
								if (!VersionChecker.IsVersionNewer(SharedSettings.Current.LatestVersion, "1.95.1"))
								{
									MainPageConfigurationModel mainPageConfigurationModel = new MainPageConfigurationModel();
									mainPageConfigurationModel.Configuration.FirstOrDefault((MainPageConfigurationProxyItem x) => x.ButtonType == MainPageButtons.VersionInfo).IsVisible = true;
									mainPageConfigurationModel.Save();
									CS$<>8__locals1.<>4__this.UpdateMainButtons();
								}
								Action action;
								if ((action = CS$<>8__locals1.<>9__3) == null)
								{
									action = (CS$<>8__locals1.<>9__3 = delegate
									{
										CS$<>8__locals1.<>4__this.activityFrame.IsVisible = true;
									});
								}
								Device.BeginInvokeOnMainThread(action);
								ProfileUpdater.PerformUpdate();
								VagDTCDecoder.DeleteContainers();
								Action action2;
								if ((action2 = CS$<>8__locals1.<>9__4) == null)
								{
									action2 = (CS$<>8__locals1.<>9__4 = delegate
									{
										CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
									});
								}
								Device.BeginInvokeOnMainThread(action2);
							}
							CS$<>8__locals1.<>4__this.RemoveCSV();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<Initialize>d__12>(ref taskAwaiter, ref this);
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
					simpleMainPage.btnStartSimulation.IsEnabled = true;
					simpleMainPage.btnConnect.IsEnabled = true;
					simpleMainPage.btnSettings.IsEnabled = true;
					simpleMainPage.activityFrame.IsVisible = false;
					try
					{
						simpleMainPage.AskForReview();
					}
					catch
					{
					}
					if (SharedSettings.Current.ConnectOnLaunch)
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals1.<>4__this.ConnectOnLaunch();
						});
					}
					Task.Run(delegate
					{
						SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d <<Initialize>b__2>d;
						<<Initialize>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<Initialize>b__2>d.<>4__this = CS$<>8__locals1;
						<<Initialize>b__2>d.<>1__state = -1;
						<<Initialize>b__2>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass12_0.<<Initialize>b__2>d>(ref <<Initialize>b__2>d);
						return <<Initialize>b__2>d.<>t__builder.Task;
					});
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000593 RID: 1427 RVA: 0x00058A78 File Offset: 0x00056C78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000438 RID: 1080
			public int <>1__state;

			// Token: 0x04000439 RID: 1081
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400043A RID: 1082
			public SimpleMainPage <>4__this;

			// Token: 0x0400043B RID: 1083
			private SimpleMainPage.<>c__DisplayClass12_0 <>8__1;

			// Token: 0x0400043C RID: 1084
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200011A RID: 282
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RefreshPIDs>d__24 : IAsyncStateMachine
		{
			// Token: 0x06000594 RID: 1428 RVA: 0x00058A88 File Offset: 0x00056C88
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						if (App.OBDSimulator.IsActive)
						{
							LiveDataPIDModel.GetSupportedPIDsTEST(App.OBDReader);
						}
						else
						{
							LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
						}
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000595 RID: 1429 RVA: 0x00058B00 File Offset: 0x00056D00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400043D RID: 1085
			public int <>1__state;

			// Token: 0x0400043E RID: 1086
			public AsyncTaskMethodBuilder <>t__builder;
		}

		// Token: 0x0200011B RID: 283
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RemoveCSV>d__18 : IAsyncStateMachine
		{
			// Token: 0x06000596 RID: 1430 RVA: 0x00058B10 File Offset: 0x00056D10
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Task.Delay(TimeSpan.FromSeconds(5.0)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<RemoveCSV>d__18>(ref taskAwaiter, ref this);
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
					Task.Run(delegate
					{
						Brc2CsvConverter.RemoveTemporaryCSV();
					});
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

			// Token: 0x06000597 RID: 1431 RVA: 0x00058BF0 File Offset: 0x00056DF0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400043F RID: 1087
			public int <>1__state;

			// Token: 0x04000440 RID: 1088
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000441 RID: 1089
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200011C RID: 284
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowCombinedChart>d__47 : IAsyncStateMachine
		{
			// Token: 0x06000598 RID: 1432 RVA: 0x00058C00 File Offset: 0x00056E00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00E1;
						}
						simpleMainPage.DisableMainButtons();
						simpleMainPage.activityFrame.IsVisible = true;
						taskAwaiter = Task.Delay(100).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowCombinedChart>d__47>(ref taskAwaiter, ref this);
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
					taskAwaiter = simpleMainPage.Navigation.PushAsync(new LiveDataMultiChartSelector()).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowCombinedChart>d__47>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E1:
					taskAwaiter.GetResult();
					simpleMainPage.activityFrame.IsVisible = false;
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x06000599 RID: 1433 RVA: 0x00058D44 File Offset: 0x00056F44
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000442 RID: 1090
			public int <>1__state;

			// Token: 0x04000443 RID: 1091
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000444 RID: 1092
			public SimpleMainPage <>4__this;

			// Token: 0x04000445 RID: 1093
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200011D RID: 285
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowSeparateCharts>d__46 : IAsyncStateMachine
		{
			// Token: 0x0600059A RID: 1434 RVA: 0x00058D54 File Offset: 0x00056F54
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00E1;
						}
						simpleMainPage.DisableMainButtons();
						simpleMainPage.activityFrame.IsVisible = true;
						taskAwaiter = Task.Delay(100).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowSeparateCharts>d__46>(ref taskAwaiter, ref this);
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
					taskAwaiter = simpleMainPage.Navigation.PushAsync(new LiveDataChartPage()).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowSeparateCharts>d__46>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E1:
					taskAwaiter.GetResult();
					simpleMainPage.activityFrame.IsVisible = false;
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x0600059B RID: 1435 RVA: 0x00058E98 File Offset: 0x00057098
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000446 RID: 1094
			public int <>1__state;

			// Token: 0x04000447 RID: 1095
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000448 RID: 1096
			public SimpleMainPage <>4__this;

			// Token: 0x04000449 RID: 1097
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200011E RID: 286
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShowWelcomePage2>d__22 : IAsyncStateMachine
		{
			// Token: 0x0600059C RID: 1436 RVA: 0x00058EA8 File Offset: 0x000570A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.UseLegacyUI && PlatformHelper.IsiOS)
							{
								WelcomePage2 welcomePage = new WelcomePage2();
								taskAwaiter = simpleMainPage.Navigation.PushAsync(welcomePage, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowWelcomePage2>d__22>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_008A;
							}
							else
							{
								WelcomePage2V3 welcomePage2V = new WelcomePage2V3();
								taskAwaiter = simpleMainPage.Navigation.PushAsync(welcomePage2V, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<ShowWelcomePage2>d__22>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0119;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_008A:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0119:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600059D RID: 1437 RVA: 0x00058FF4 File Offset: 0x000571F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400044A RID: 1098
			public int <>1__state;

			// Token: 0x0400044B RID: 1099
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400044C RID: 1100
			public SimpleMainPage <>4__this;

			// Token: 0x0400044D RID: 1101
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200011F RID: 287
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartConnection>d__38 : IAsyncStateMachine
		{
			// Token: 0x0600059E RID: 1438 RVA: 0x00059004 File Offset: 0x00057204
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter<PermissionStatus> taskAwaiter6;
					TaskAwaiter taskAwaiter7;
					TaskAwaiter<string> taskAwaiter9;
					ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter;
					switch (num)
					{
					case 0:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						break;
					case 1:
						taskAwaiter6 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						goto IL_022F;
					case 2:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_029F;
					}
					case 3:
						taskAwaiter6 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						if (taskAwaiter6.GetResult() == 3)
						{
							goto IL_036A;
						}
						taskAwaiter7 = simpleMainPage.DisplayAlert(Translate.GetString("droid_Android12BluetoothPermissionMissing_Title"), Translate.GetString("droid_NearbyDevicesExplanation") + "\n" + Translate.GetString("droid_Android12BluetoothPermissionMissing_Text"), "OK").GetAwaiter();
						if (!taskAwaiter7.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter8 = taskAwaiter7;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
							return;
						}
						goto IL_0359;
					case 4:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0359;
					}
					case 5:
					case 6:
						IL_038E:
						try
						{
							if (num != 5)
							{
								if (num == 6)
								{
									TaskAwaiter taskAwaiter8;
									taskAwaiter7 = taskAwaiter8;
									taskAwaiter8 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_047B;
								}
								taskAwaiter7 = simpleMainPage.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK").GetAwaiter();
								if (!taskAwaiter7.IsCompleted)
								{
									num = (num2 = 5);
									TaskAwaiter taskAwaiter8 = taskAwaiter7;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter8;
								taskAwaiter7 = taskAwaiter8;
								taskAwaiter8 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter7.GetResult();
							if (App.GetCurrentPage() != simpleMainPage)
							{
								goto IL_0482;
							}
							taskAwaiter7 = simpleMainPage.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num = (num2 = 6);
								TaskAwaiter taskAwaiter8 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
								return;
							}
							IL_047B:
							taskAwaiter7.GetResult();
							IL_0482:;
						}
						catch
						{
						}
						goto IL_1342;
					case 7:
					case 8:
						IL_04BD:
						try
						{
							if (num != 7)
							{
								if (num == 8)
								{
									TaskAwaiter taskAwaiter8;
									taskAwaiter7 = taskAwaiter8;
									taskAwaiter8 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_05AA;
								}
								taskAwaiter7 = simpleMainPage.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK").GetAwaiter();
								if (!taskAwaiter7.IsCompleted)
								{
									num = (num2 = 7);
									TaskAwaiter taskAwaiter8 = taskAwaiter7;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter8;
								taskAwaiter7 = taskAwaiter8;
								taskAwaiter8 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter7.GetResult();
							if (App.GetCurrentPage() != simpleMainPage)
							{
								goto IL_05B1;
							}
							taskAwaiter7 = simpleMainPage.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num = (num2 = 8);
								TaskAwaiter taskAwaiter8 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
								return;
							}
							IL_05AA:
							taskAwaiter7.GetResult();
							IL_05B1:;
						}
						catch
						{
						}
						goto IL_1342;
					case 9:
						IL_05C5:
						try
						{
							if (num != 9)
							{
								if (SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE || !PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
								{
									goto IL_0676;
								}
								taskAwaiter7 = simpleMainPage.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK").GetAwaiter();
								if (!taskAwaiter7.IsCompleted)
								{
									num = (num2 = 9);
									TaskAwaiter taskAwaiter8 = taskAwaiter7;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter8;
								taskAwaiter7 = taskAwaiter8;
								taskAwaiter8 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter7.GetResult();
							PlatformHelper.IOSService.OpenSystemSettings();
							goto IL_1342;
						}
						catch (Exception)
						{
						}
						IL_0676:
						try
						{
							if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
							{
								PlatformHelper.IOSService.LocalNetworkPermissionService_EasyWayRequest();
							}
						}
						catch (Exception)
						{
						}
						goto IL_0692;
					case 10:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_075E;
					}
					case 11:
						IL_0793:
						try
						{
							if (num != 11)
							{
								taskAwaiter9 = simpleMainPage.CheckWiFiConnection(CS$<>8__locals1.wifiHelper).GetAwaiter();
								if (!taskAwaiter9.IsCompleted)
								{
									num = (num2 = 11);
									TaskAwaiter<string> taskAwaiter10 = taskAwaiter9;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter9, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<string> taskAwaiter10;
								taskAwaiter9 = taskAwaiter10;
								taskAwaiter10 = default(TaskAwaiter<string>);
								num = (num2 = -1);
							}
							string result = taskAwaiter9.GetResult();
							CS$<>8__locals1.wifi_name = result;
						}
						catch (Exception)
						{
							CS$<>8__locals1.wifi_name = "<unknown ssid>";
						}
						simpleMainPage.connectionProgressFrame.IsVisible = false;
						goto IL_0833;
					case 12:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_08E8;
					}
					case 13:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0957;
					}
					case 14:
						IL_0985:
						try
						{
							if (num != 14)
							{
								if (SharedSettings.Current.SpeedCalibrationTaskPending && SpeedCalibrationModelV2.Instance == null)
								{
									SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
								}
								OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
								configuredTaskAwaiter = Task.Run(delegate
								{
									SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__2>d <<StartConnection>b__2>d;
									<<StartConnection>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<StartConnection>b__2>d.<>4__this = CS$<>8__locals1;
									<<StartConnection>b__2>d.<>1__state = -1;
									<<StartConnection>b__2>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__2>d>(ref <<StartConnection>b__2>d);
									return <<StartConnection>b__2>d.<>t__builder.Task;
								}).ConfigureAwait(true).GetAwaiter();
								if (!configuredTaskAwaiter.IsCompleted)
								{
									num = (num2 = 14);
									ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref configuredTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2;
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
								num = (num2 = -1);
							}
							configuredTaskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						if (SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE || CS$<>8__locals1.connected || !SharedSettings.Current.SearchForBTLEIfConnectionFailed || App.OBDReader.DisconnectRequested)
						{
							goto IL_0B54;
						}
						taskAwaiter5 = BTLEDeviceSelectorViewModel.FindDeviceWithRandomizedUUID().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 15);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_0ABF;
					case 15:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_0ABF;
					case 16:
						IL_0ACB:
						try
						{
							if (num != 16)
							{
								OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
								configuredTaskAwaiter = Task.Run(delegate
								{
									SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__4>d <<StartConnection>b__4>d;
									<<StartConnection>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<StartConnection>b__4>d.<>4__this = CS$<>8__locals1;
									<<StartConnection>b__4>d.<>1__state = -1;
									<<StartConnection>b__4>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__4>d>(ref <<StartConnection>b__4>d);
									return <<StartConnection>b__4>d.<>t__builder.Task;
								}).ConfigureAwait(true).GetAwaiter();
								if (!configuredTaskAwaiter.IsCompleted)
								{
									num = (num2 = 16);
									ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref configuredTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2;
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
								num = (num2 = -1);
							}
							configuredTaskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						goto IL_0B54;
					case 17:
					{
						ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						configuredTaskAwaiter = configuredTaskAwaiter2;
						configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
						num = (num2 = -1);
						goto IL_0D8E;
					}
					case 18:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_0EA4;
					case 19:
					{
						TaskAwaiter<string> taskAwaiter10;
						taskAwaiter9 = taskAwaiter10;
						taskAwaiter10 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_101B;
					}
					case 20:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_10BF;
					}
					case 21:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_11E3;
					}
					case 22:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_127D;
					}
					case 23:
					{
						TaskAwaiter taskAwaiter8;
						taskAwaiter7 = taskAwaiter8;
						taskAwaiter8 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_1319;
					}
					default:
						CS$<>8__locals1 = new SimpleMainPage.<>c__DisplayClass38_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.InitECU = InitECU;
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							goto IL_1342;
						}
						simpleMainPage.connectionProgressFrame.ResetTextToDefault();
						simpleMainPage.connectionProgressFrame.IsCancelVisible = false;
						if (!VersionChecker.IsValidVersion)
						{
							taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("main_VersionOutdatedTitle"), Translate.GetString("main_VersionOutdatedText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter5, ref this);
								return;
							}
						}
						else
						{
							CS$<>8__locals1.wifi_name = "";
							App.OBDReader.DisconnectRequested = false;
							App.OBDReader.stopwatch.Restart();
							SharedSettings.Current.OptimizedRequestStuckCounter = 0;
							if (!PlatformHelper.IsAndroid || (SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE && SharedSettings.Current.ConnectionType != ConnectionTypes.Bluetooth) || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
							{
								goto IL_036A;
							}
							taskAwaiter6 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num = (num2 = 1);
								taskAwaiter4 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_022F;
						}
						break;
					}
					if (taskAwaiter5.GetResult())
					{
						simpleMainPage.LabelNewVersion_Clicked(simpleMainPage.labelNewVersion, EventArgs.Empty);
					}
					goto IL_1342;
					IL_022F:
					if (taskAwaiter6.GetResult() == 3)
					{
						goto IL_036A;
					}
					taskAwaiter7 = simpleMainPage.Navigation.PushAsync(new DroidPermissionRequestPage(true, false)).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_029F:
					taskAwaiter7.GetResult();
					goto IL_1342;
					IL_0359:
					taskAwaiter7.GetResult();
					PermissionHelper.OpenPermissionsSettings();
					goto IL_1342;
					IL_036A:
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
					{
						goto IL_038E;
					}
					if ((SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
					{
						goto IL_04BD;
					}
					if (PlatformHelper.IsiOS)
					{
						goto IL_05C5;
					}
					IL_0692:
					if (!PlatformHelper.IsAndroid)
					{
						goto IL_0773;
					}
					if (!SharedSettings.Current.CheckBluetoothTurnedOn || (SharedSettings.Current.ConnectionType != ConnectionTypes.Bluetooth && SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE))
					{
						BluetoothHelper.BluetoothWasTurnedOnByTheApp = false;
						goto IL_0773;
					}
					simpleMainPage.connectionProgressFrame.Text = Translate.GetString("droid_TurningOnBluetooth");
					simpleMainPage.connectionProgressFrame.IsVisible = true;
					if (!SharedSettings.Current.DroidTryTurnOnBluetooth || DependencyService.Get<IBluetooth2Manager>(0).IsOn)
					{
						goto IL_0773;
					}
					taskAwaiter7 = PlatformHelper.DroidService.AndroidHelper_RequestBluetoothPowerOn().GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 10);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_075E:
					taskAwaiter7.GetResult();
					BluetoothHelper.BluetoothWasTurnedOnByTheApp = true;
					IL_0773:
					CS$<>8__locals1.wifiHelper = DependencyService.Get<IWiFiHelper>(0);
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
					{
						goto IL_0793;
					}
					IL_0833:
					if (!SharedSettings.Current.ForceProfileUpdateScheduled && (!SharedSettings.Current.ProfileHasPids || CustomPIDViewModel.CurrentProfile.PidCollection.Count != 0))
					{
						goto IL_08EF;
					}
					simpleMainPage.connectionProgressFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					taskAwaiter7 = Task.Run(delegate
					{
						ProfileUpdater.ForceUpdate();
					}).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 12);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_08E8:
					taskAwaiter7.GetResult();
					IL_08EF:
					taskAwaiter7 = MainThread.InvokeOnMainThreadAsync(delegate
					{
						CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = Translate.GetString("ios_ConnectingToELM");
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
						{
							if (string.IsNullOrEmpty(CS$<>8__locals1.wifi_name))
							{
								if (CS$<>8__locals1.wifiHelper.HasLocationPermission())
								{
									CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("ios_NoWiFi");
								}
							}
							else
							{
								CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nWiFi: " + CS$<>8__locals1.wifi_name;
							}
							if (PlatformHelper.IsAndroid)
							{
								CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\n" + Translate.GetString("droid_DisableMobileData");
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
						{
							if (!string.IsNullOrEmpty(SharedSettings.Current.BTDeviceName))
							{
								CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTDeviceName;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && !string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceName))
						{
							CS$<>8__locals1.<>4__this.connectionProgressFrame.Text = CS$<>8__locals1.<>4__this.connectionProgressFrame.Text + "\r\nBluetooth: " + SharedSettings.Current.BTLEDeviceName;
						}
						CS$<>8__locals1.<>4__this.connectionProgressFrame.IsVisible = true;
					}).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 13);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_0957:
					taskAwaiter7.GetResult();
					CS$<>8__locals1.connected = false;
					OBDDataReader obdreader = App.OBDReader;
					if (obdreader == null)
					{
						goto IL_0985;
					}
					ELMState elmstatus = obdreader.ELMStatus;
					if (elmstatus == null)
					{
						goto IL_0985;
					}
					elmstatus.ResetErrorsState();
					goto IL_0985;
					IL_0ABF:
					if (taskAwaiter5.GetResult())
					{
						goto IL_0ACB;
					}
					IL_0B54:
					string text = "";
					switch (SharedSettings.Current.ConnectionType)
					{
					case ConnectionTypes.WiFi:
						text = SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
						break;
					case ConnectionTypes.BluetoothLE:
						text = SharedSettings.Current.BTLEDeviceID;
						break;
					case ConnectionTypes.Bluetooth:
						text = SharedSettings.Current.BTDeviceID;
						break;
					}
					ScanXChecker.CheckDeviceAtConnection(text);
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					if (!CS$<>8__locals1.connected || App.OBDReader.DisconnectRequested)
					{
						goto IL_12A8;
					}
					if (SharedSettings.Current.CheckIsELMWhenConnecting && SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
					{
						if (PlatformHelper.IsAndroid)
						{
							if (CS$<>8__locals1.wifi_name != "<unknown ssid>")
							{
								SharedSettings.Current.LastWiFiName = CS$<>8__locals1.wifi_name;
							}
						}
						else
						{
							SharedSettings.Current.LastWiFiName = CS$<>8__locals1.wifi_name;
						}
					}
					if (!CS$<>8__locals1.InitECU)
					{
						App.OBDReader.Start("MainPage->StartConnection/InitECUFailed");
						goto IL_129C;
					}
					CS$<>8__locals2 = new SimpleMainPage.<>c__DisplayClass38_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					if (App.OBDReader.DisconnectRequested)
					{
						goto IL_1342;
					}
					simpleMainPage.connectionProgressFrame.Text = Translate.GetString("ios_ConnectingToECU");
					simpleMainPage.connectionProgressFrame.IsVisible = true;
					CS$<>8__locals2.initprogress = new Progress<string>();
					CS$<>8__locals2.initprogress.ProgressChanged += simpleMainPage.Initprogress_ProgressChanged;
					CS$<>8__locals2.attempts = 3;
					if (SharedSettings.Current.ProtocolNumber == 0 && SharedSettings.Current.UseDefaultInit)
					{
						CS$<>8__locals2.attempts = 2;
					}
					CarInfoViewModel.Instance.Reset();
					CS$<>8__locals2.initResult = false;
					configuredTaskAwaiter = Task.Run(delegate
					{
						SimpleMainPage.<>c__DisplayClass38_1.<<StartConnection>b__5>d <<StartConnection>b__5>d;
						<<StartConnection>b__5>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<StartConnection>b__5>d.<>4__this = CS$<>8__locals2;
						<<StartConnection>b__5>d.<>1__state = -1;
						<<StartConnection>b__5>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_1.<<StartConnection>b__5>d>(ref <<StartConnection>b__5>d);
						return <<StartConnection>b__5>d.<>t__builder.Task;
					}).ConfigureAwait(true).GetAwaiter();
					if (!configuredTaskAwaiter.IsCompleted)
					{
						num = (num2 = 17);
						ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2 = configuredTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref configuredTaskAwaiter, ref this);
						return;
					}
					IL_0D8E:
					configuredTaskAwaiter.GetResult();
					if (!CS$<>8__locals2.initResult)
					{
						goto IL_1284;
					}
					SharedSettings sharedSettings = SharedSettings.Current;
					int freePeriodGoodConnections = sharedSettings.FreePeriodGoodConnections;
					sharedSettings.FreePeriodGoodConnections = freePeriodGoodConnections + 1;
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
					{
						if (PlatformHelper.IsAndroid)
						{
							if (CS$<>8__locals2.CS$<>8__locals1.wifi_name != "<unknown ssid>")
							{
								SharedSettings.Current.LastWiFiName = CS$<>8__locals2.CS$<>8__locals1.wifi_name;
							}
						}
						else
						{
							SharedSettings.Current.LastWiFiName = CS$<>8__locals2.CS$<>8__locals1.wifi_name;
						}
					}
					if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidStartBackgroundService)
					{
						IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
						if (droidService != null)
						{
							droidService.AndroidHelper_StartService();
						}
					}
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					taskAwaiter5 = simpleMainPage.UpdateVINInfo().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 18);
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0EA4:
					taskAwaiter5.GetResult();
					if (!SharedSettings.Current.ShouldCheckProfilePIDs || SharedSettings.Current.IgnoreProfilePidsTestScheduled || (App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit && App.OBDReader.CurrentELMFormat != ELMFormat.CAN29bit))
					{
						goto IL_10C6;
					}
					SimpleMainPage.<>c__DisplayClass38_2 CS$<>8__locals3 = new SimpleMainPage.<>c__DisplayClass38_2();
					CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals2;
					CS$<>8__locals3.detectingString = Translate.GetString("profile_CheckingSupportedPids");
					simpleMainPage.connectionProgressFrame.Text = CS$<>8__locals3.detectingString;
					simpleMainPage.connectionProgressFrame.CancelText = Translate.GetString("ios_Skip");
					simpleMainPage.connectionProgressFrame.IsCancelVisible = true;
					CS$<>8__locals3.cts = new CancellationTokenSource();
					EventHandler eventHandler = delegate(object cancelSender, EventArgs cancelArgs)
					{
						SimpleMainPage.<>c__DisplayClass38_2.<<StartConnection>b__8>d <<StartConnection>b__8>d;
						<<StartConnection>b__8>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<StartConnection>b__8>d.<>4__this = CS$<>8__locals3;
						<<StartConnection>b__8>d.<>1__state = -1;
						<<StartConnection>b__8>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_2.<<StartConnection>b__8>d>(ref <<StartConnection>b__8>d);
					};
					ActivityFrame connectionProgressFrame = simpleMainPage.connectionProgressFrame;
					connectionProgressFrame.CancelClicked = (EventHandler)Delegate.Remove(connectionProgressFrame.CancelClicked, eventHandler);
					ActivityFrame connectionProgressFrame2 = simpleMainPage.connectionProgressFrame;
					connectionProgressFrame2.CancelClicked = (EventHandler)Delegate.Combine(connectionProgressFrame2.CancelClicked, eventHandler);
					simpleMainPage.connectionProgressFrame.IsVisible = true;
					IProgress<int> progress = new Progress<int>(delegate(int i)
					{
						MainThreadHelper.InvokeOnMainThread(new Action(new SimpleMainPage.<>c__DisplayClass38_3
						{
							CS$<>8__locals3 = CS$<>8__locals3,
							i = i
						}.<StartConnection>b__10));
					});
					simpleMainPage.DisableMainButtons();
					taskAwaiter9 = ProfileSupportedPidsTester.PerformTest(progress, CS$<>8__locals3.cts.Token).GetAwaiter();
					if (!taskAwaiter9.IsCompleted)
					{
						num = (num2 = 19);
						TaskAwaiter<string> taskAwaiter10 = taskAwaiter9;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter9, ref this);
						return;
					}
					IL_101B:
					string result2 = taskAwaiter9.GetResult();
					simpleMainPage.EnableMainButtons();
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					simpleMainPage.connectionProgressFrame.ResetTextToDefault();
					simpleMainPage.connectionProgressFrame.IsCancelVisible = false;
					if (string.IsNullOrEmpty(result2))
					{
						goto IL_10C6;
					}
					taskAwaiter7 = simpleMainPage.DisplayAlert(Translate.GetString("profile_PidDetectionInterruptedTitle"), result2, "OK").GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 20);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_10BF:
					taskAwaiter7.GetResult();
					IL_10C6:
					DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
					if (recorder != null)
					{
						recorder.Save();
					}
					if (SharedSettings.Current.RecordData)
					{
						DataRecorderV2.StartRecording();
					}
					else
					{
						App.OBDReader.CurrentCarData.Recorder = null;
					}
					RequestProducerStatic.UpdateOBDReaderRequests();
					if ((App.OBDReader.CurrentELMFormat != ELMFormat.CAN11bit && App.OBDReader.CurrentELMFormat != ELMFormat.CAN29bit) || SharedSettings.Current.CANOptimizationWarningShowed || SharedSettings.Current.CANOptimizeRequests || SharedSettings.Current.FreePeriodGoodConnections <= 1 || SharedSettings.Current.SelectedBrand == "BYD")
					{
						goto IL_11EA;
					}
					SharedSettings.Current.CANOptimizationWarningShowed = true;
					taskAwaiter7 = MainThread.InvokeOnMainThreadAsync(delegate
					{
						SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__6>d <<StartConnection>b__6>d;
						<<StartConnection>b__6>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<StartConnection>b__6>d.<>4__this = CS$<>8__locals2.CS$<>8__locals1;
						<<StartConnection>b__6>d.<>1__state = -1;
						<<StartConnection>b__6>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__6>d>(ref <<StartConnection>b__6>d);
						return <<StartConnection>b__6>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 21);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_11E3:
					taskAwaiter7.GetResult();
					IL_11EA:
					if (!SharedSettings.Current.UseRPMFix || SharedSettings.Current.RPMFixWarningShowed)
					{
						goto IL_1284;
					}
					SharedSettings.Current.RPMFixWarningShowed = true;
					taskAwaiter7 = MainThread.InvokeOnMainThreadAsync(delegate
					{
						SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__7>d <<StartConnection>b__7>d;
						<<StartConnection>b__7>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<StartConnection>b__7>d.<>4__this = CS$<>8__locals2.CS$<>8__locals1;
						<<StartConnection>b__7>d.<>1__state = -1;
						<<StartConnection>b__7>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__7>d>(ref <<StartConnection>b__7>d);
						return <<StartConnection>b__7>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 22);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_127D:
					taskAwaiter7.GetResult();
					IL_1284:
					CS$<>8__locals2 = null;
					IL_129C:
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					IL_12A8:
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					taskAwaiter7 = MainThread.InvokeOnMainThreadAsync(delegate
					{
						SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d <<StartConnection>b__3>d;
						<<StartConnection>b__3>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<StartConnection>b__3>d.<>4__this = CS$<>8__locals1;
						<<StartConnection>b__3>d.<>1__state = -1;
						<<StartConnection>b__3>d.<>t__builder.Start<SimpleMainPage.<>c__DisplayClass38_0.<<StartConnection>b__3>d>(ref <<StartConnection>b__3>d);
						return <<StartConnection>b__3>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num = (num2 = 23);
						TaskAwaiter taskAwaiter8 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<StartConnection>d__38>(ref taskAwaiter7, ref this);
						return;
					}
					IL_1319:
					taskAwaiter7.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_1342:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600059F RID: 1439 RVA: 0x0005A434 File Offset: 0x00058634
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400044E RID: 1102
			public int <>1__state;

			// Token: 0x0400044F RID: 1103
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000450 RID: 1104
			public SimpleMainPage <>4__this;

			// Token: 0x04000451 RID: 1105
			public bool InitECU;

			// Token: 0x04000452 RID: 1106
			private SimpleMainPage.<>c__DisplayClass38_0 <>8__1;

			// Token: 0x04000453 RID: 1107
			private SimpleMainPage.<>c__DisplayClass38_1 <>8__2;

			// Token: 0x04000454 RID: 1108
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000455 RID: 1109
			private TaskAwaiter<PermissionStatus> <>u__2;

			// Token: 0x04000456 RID: 1110
			private TaskAwaiter <>u__3;

			// Token: 0x04000457 RID: 1111
			private TaskAwaiter<string> <>u__4;

			// Token: 0x04000458 RID: 1112
			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter <>u__5;
		}

		// Token: 0x02000120 RID: 288
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateVINInfo>d__42 : IAsyncStateMachine
		{
			// Token: 0x060005A0 RID: 1440 RVA: 0x0005A444 File Offset: 0x00058644
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!SharedSettings.Current.UseOBD2)
						{
							Device.BeginInvokeOnMainThread(delegate
							{
								simpleMainPage.gridVIN.IsVisible = true;
							});
							flag = false;
							goto IL_00FB;
						}
						Device.BeginInvokeOnMainThread(delegate
						{
							simpleMainPage.gridVIN.IsVisible = true;
						});
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
						{
							flag = false;
							goto IL_00FB;
						}
						if (App.OBDReader.CurrentProtocolNumber < 3)
						{
							flag = false;
							goto IL_00FB;
						}
						if (!SharedSettings.Current.RequestECUInfo)
						{
							flag = false;
							goto IL_00FB;
						}
						taskAwaiter = CarInfoViewModel.Instance.Load(false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<UpdateVINInfo>d__42>(ref taskAwaiter, ref this);
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
					flag = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00FB:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060005A1 RID: 1441 RVA: 0x0005A570 File Offset: 0x00058770
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000459 RID: 1113
			public int <>1__state;

			// Token: 0x0400045A RID: 1114
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400045B RID: 1115
			public SimpleMainPage <>4__this;

			// Token: 0x0400045C RID: 1116
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000121 RID: 289
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCoding_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x060005A2 RID: 1442 RVA: 0x0005A580 File Offset: 0x00058780
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (!simpleMainPage.CheckConnected(false))
							{
								goto IL_0156;
							}
							if (!CodingListModel.IsCodingAvailable(false))
							{
								taskAwaiter = simpleMainPage.DisplayAlert(Translate.GetString("coding_ModeNotSupportedTitle"), Translate.GetString("coding_ModeNotSupportedText"), "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnCoding_Clicked>d__16>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0099;
							}
							else
							{
								simpleMainPage.DisableMainButtons();
								simpleMainPage.activityFrame.IsVisible = true;
								App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
								Page page = null;
								if (App.UseLegacyUI)
								{
									if (PlatformHelper.IsiOS)
									{
										page = new CodingModeSelection();
									}
								}
								else
								{
									page = new CodingModeSelectionV2();
								}
								RequestProducerStatic.CurrentWorkingMode = WorkingModes.DTC;
								taskAwaiter = simpleMainPage.Navigation.PushAsync(page).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnCoding_Clicked>d__16>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						simpleMainPage.EnableMainButtons();
						simpleMainPage.activityFrame.IsVisible = false;
						IL_0156:
						goto IL_0171;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_0099:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0171:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060005A3 RID: 1443 RVA: 0x0005A730 File Offset: 0x00058930
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400045D RID: 1117
			public int <>1__state;

			// Token: 0x0400045E RID: 1118
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400045F RID: 1119
			public SimpleMainPage <>4__this;

			// Token: 0x04000460 RID: 1120
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000122 RID: 290
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnConnect_Clicked>d__30 : IAsyncStateMachine
		{
			// Token: 0x060005A4 RID: 1444 RVA: 0x0005A740 File Offset: 0x00058940
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							simpleMainPage.btnConnect.IsEnabled = false;
							if (!SharedSettings.Current.FirstConnectionAttempted || string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
							{
								taskAwaiter = simpleMainPage.ShowWelcomePage2().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnConnect_Clicked>d__30>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0095;
							}
						}
						try
						{
							if (num != 1)
							{
								taskAwaiter = simpleMainPage.StartConnection(true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnConnect_Clicked>d__30>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num2 = -1;
							}
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						goto IL_0101;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_0095:
					taskAwaiter.GetResult();
					IL_0101:
					simpleMainPage.btnConnect.IsEnabled = true;
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

			// Token: 0x060005A5 RID: 1445 RVA: 0x0005A8BC File Offset: 0x00058ABC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000461 RID: 1121
			public int <>1__state;

			// Token: 0x04000462 RID: 1122
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000463 RID: 1123
			public SimpleMainPage <>4__this;

			// Token: 0x04000464 RID: 1124
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000123 RID: 291
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDTC_Clicked>d__49 : IAsyncStateMachine
		{
			// Token: 0x060005A6 RID: 1446 RVA: 0x0005A8CC File Offset: 0x00058ACC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_018C;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0203;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0269;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02D8;
					}
					default:
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_030B;
						}
						simpleMainPage.DisableMainButtons();
						simpleMainPage.activityFrame.IsVisible = true;
						bool flag = false;
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP && ((SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCReadingSequence)) || (SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.ReplaceAuto && !string.IsNullOrEmpty(SharedSettings.Current.DTCClearingSequence))))
						{
							flag = true;
						}
						RequestProducerStatic.CurrentWorkingMode = WorkingModes.DTC;
						if (!App.OBDSimulator.IsActive && DTCv2Model.IsDTCv2Available && !flag)
						{
							if (App.UseLegacyUI)
							{
								dtcv2page = new DTCv2Page();
								taskAwaiter = dtcv2page.LoadModel().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDTC_Clicked>d__49>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								dtcv3page = new DTCv3Page();
								taskAwaiter = dtcv3page.LoadModel().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 2;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDTC_Clicked>d__49>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0203;
							}
						}
						else
						{
							Page page = new DTCWizardStartPage();
							taskAwaiter = simpleMainPage.Navigation.PushAsync(page).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDTC_Clicked>d__49>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_02D8;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					taskAwaiter = simpleMainPage.Navigation.PushAsync(dtcv2page).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDTC_Clicked>d__49>(ref taskAwaiter, ref this);
						return;
					}
					IL_018C:
					taskAwaiter.GetResult();
					dtcv2page = null;
					goto IL_02DF;
					IL_0203:
					taskAwaiter.GetResult();
					taskAwaiter = simpleMainPage.Navigation.PushAsync(dtcv3page).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDTC_Clicked>d__49>(ref taskAwaiter, ref this);
						return;
					}
					IL_0269:
					taskAwaiter.GetResult();
					dtcv3page = null;
					goto IL_02DF;
					IL_02D8:
					taskAwaiter.GetResult();
					IL_02DF:
					simpleMainPage.EnableMainButtons();
					simpleMainPage.activityFrame.IsVisible = false;
					CarPlayManager instance = CarPlayManager.Instance;
					if (instance != null)
					{
						instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInDTC"));
					}
					IL_030B:;
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

			// Token: 0x060005A7 RID: 1447 RVA: 0x0005AC30 File Offset: 0x00058E30
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000465 RID: 1125
			public int <>1__state;

			// Token: 0x04000466 RID: 1126
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000467 RID: 1127
			public SimpleMainPage <>4__this;

			// Token: 0x04000468 RID: 1128
			private DTCv2Page <dtcv2page>5__2;

			// Token: 0x04000469 RID: 1129
			private TaskAwaiter <>u__1;

			// Token: 0x0400046A RID: 1130
			private DTCv3Page <dtcv3page>5__3;
		}

		// Token: 0x02000124 RID: 292
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDashboard_Clicked>d__51 : IAsyncStateMachine
		{
			// Token: 0x060005A8 RID: 1448 RVA: 0x0005AC40 File Offset: 0x00058E40
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_00B0;
						}
						simpleMainPage.DisableMainButtons();
						simpleMainPage.activityFrame.IsVisible = true;
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						DashboardXamlPage dashboardXamlPage = new DashboardXamlPage();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(dashboardXamlPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDashboard_Clicked>d__51>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					simpleMainPage.activityFrame.IsVisible = false;
					IL_00B0:;
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

			// Token: 0x060005A9 RID: 1449 RVA: 0x0005AD3C File Offset: 0x00058F3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400046B RID: 1131
			public int <>1__state;

			// Token: 0x0400046C RID: 1132
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400046D RID: 1133
			public SimpleMainPage <>4__this;

			// Token: 0x0400046E RID: 1134
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000125 RID: 293
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDisconnect_Clicked>d__31 : IAsyncStateMachine
		{
			// Token: 0x060005AA RID: 1450 RVA: 0x0005AD4C File Offset: 0x00058F4C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						simpleMainPage.btnDisconnect.IsVisible = false;
						simpleMainPage.btnConnect.IsVisible = true;
						simpleMainPage.activityFrame.IsVisible = true;
						simpleMainPage.connectionProgressFrame.IsVisible = false;
						if (PlatformHelper.IsAndroid)
						{
							IPlatformSpecificServiceDroid droidService = PlatformHelper.DroidService;
							if (droidService != null)
							{
								droidService.AndroidHelper_StopService();
							}
						}
						App.OBDReader.DisconnectRequested = true;
						taskAwaiter = App.OBDReader.Disconnect("UserClickbtnDisconnect").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnDisconnect_Clicked>d__31>(ref taskAwaiter, ref this);
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
					DataRecorderV2.StopRecording(OBDDataReaderStatus.Disconnected);
					simpleMainPage.btnDisconnect.IsVisible = false;
					simpleMainPage.connectionProgressFrame.IsVisible = false;
					simpleMainPage.activityFrame.IsVisible = false;
					simpleMainPage.btnDisconnect.IsEnabled = true;
					BluetoothHelper.TurnOffBluetoothIfNeedTo();
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

			// Token: 0x060005AB RID: 1451 RVA: 0x0005AE9C File Offset: 0x0005909C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400046F RID: 1135
			public int <>1__state;

			// Token: 0x04000470 RID: 1136
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000471 RID: 1137
			public SimpleMainPage <>4__this;

			// Token: 0x04000472 RID: 1138
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000126 RID: 294
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnEmissionTests_Tapped>d__55 : IAsyncStateMachine
		{
			// Token: 0x060005AC RID: 1452 RVA: 0x0005AEAC File Offset: 0x000590AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_00A7;
						}
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						simpleMainPage.DisableMainButtons();
						Page page;
						if (App.UseLegacyUI)
						{
							page = new EcoTestPage();
						}
						else
						{
							page = new EcoTestPageV2();
						}
						taskAwaiter = simpleMainPage.Navigation.PushAsync(page).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnEmissionTests_Tapped>d__55>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					IL_00A7:;
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

			// Token: 0x060005AD RID: 1453 RVA: 0x0005AFA0 File Offset: 0x000591A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000473 RID: 1139
			public int <>1__state;

			// Token: 0x04000474 RID: 1140
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000475 RID: 1141
			public SimpleMainPage <>4__this;

			// Token: 0x04000476 RID: 1142
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000127 RID: 295
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnFreezeFrame_Clicked>d__52 : IAsyncStateMachine
		{
			// Token: 0x060005AE RID: 1454 RVA: 0x0005AFB0 File Offset: 0x000591B0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_0095;
						}
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						simpleMainPage.DisableMainButtons();
						FreezeFramePage freezeFramePage = new FreezeFramePage();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(freezeFramePage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnFreezeFrame_Clicked>d__52>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					IL_0095:;
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

			// Token: 0x060005AF RID: 1455 RVA: 0x0005B090 File Offset: 0x00059290
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000477 RID: 1143
			public int <>1__state;

			// Token: 0x04000478 RID: 1144
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000479 RID: 1145
			public SimpleMainPage <>4__this;

			// Token: 0x0400047A RID: 1146
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000128 RID: 296
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnFuelStatistics_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x060005B0 RID: 1456 RVA: 0x0005B0A0 File Offset: 0x000592A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						simpleMainPage.DisableMainButtons();
						FuelStatisticsPage fuelStatisticsPage = new FuelStatisticsPage();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(fuelStatisticsPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnFuelStatistics_Clicked>d__9>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x060005B1 RID: 1457 RVA: 0x0005B16C File Offset: 0x0005936C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400047B RID: 1147
			public int <>1__state;

			// Token: 0x0400047C RID: 1148
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400047D RID: 1149
			public SimpleMainPage <>4__this;

			// Token: 0x0400047E RID: 1150
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000129 RID: 297
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnGarage_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x060005B2 RID: 1458 RVA: 0x0005B17C File Offset: 0x0005937C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
							{
								simpleMainPage.DisableMainButtons();
								SettingsGarage settingsGarage = new SettingsGarage();
								taskAwaiter = simpleMainPage.Navigation.PushAsync(settingsGarage).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnGarage_Clicked>d__11>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_008A;
							}
							else
							{
								taskAwaiter = simpleMainPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnGarage_Clicked>d__11>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_010A;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_008A:
					taskAwaiter.GetResult();
					simpleMainPage.EnableMainButtons();
					IL_010A:;
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

			// Token: 0x060005B3 RID: 1459 RVA: 0x0005B2D4 File Offset: 0x000594D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400047F RID: 1151
			public int <>1__state;

			// Token: 0x04000480 RID: 1152
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000481 RID: 1153
			public SimpleMainPage <>4__this;

			// Token: 0x04000482 RID: 1154
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200012A RID: 298
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnLiveDataTable_Clicked>d__48 : IAsyncStateMachine
		{
			// Token: 0x060005B4 RID: 1460 RVA: 0x0005B2E4 File Offset: 0x000594E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_00B0;
						}
						simpleMainPage.DisableMainButtons();
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						LiveDataListPage liveDataListPage = new LiveDataListPage();
						simpleMainPage.activityFrame.IsVisible = true;
						taskAwaiter = simpleMainPage.Navigation.PushAsync(liveDataListPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnLiveDataTable_Clicked>d__48>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					simpleMainPage.activityFrame.IsVisible = false;
					IL_00B0:;
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

			// Token: 0x060005B5 RID: 1461 RVA: 0x0005B3E0 File Offset: 0x000595E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000483 RID: 1155
			public int <>1__state;

			// Token: 0x04000484 RID: 1156
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000485 RID: 1157
			public SimpleMainPage <>4__this;

			// Token: 0x04000486 RID: 1158
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200012B RID: 299
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnLiveData_Clicked>d__45 : IAsyncStateMachine
		{
			// Token: 0x060005B6 RID: 1462 RVA: 0x0005B3F0 File Offset: 0x000595F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0111;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_019C;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01F9;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0253;
					}
					default:
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_0260;
						}
						simpleMainPage.DisableMainButtons();
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						int chartsView = SharedSettings.Current.ChartsView;
						if (chartsView != 1)
						{
							if (chartsView != 2)
							{
								taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("ios_LiveDataMode"), Translate.GetString("ios_LiveDataMode_Text"), Translate.GetString("ios_LiveDataMode_Combined"), Translate.GetString("ios_LiveDataMode_Separate")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 2;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<btnLiveData_Clicked>d__45>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_019C;
							}
							else
							{
								taskAwaiter3 = simpleMainPage.ShowSeparateCharts().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnLiveData_Clicked>d__45>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0111;
							}
						}
						else
						{
							taskAwaiter3 = simpleMainPage.ShowCombinedChart().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnLiveData_Clicked>d__45>(ref taskAwaiter3, ref this);
								return;
							}
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_025A;
					IL_0111:
					taskAwaiter3.GetResult();
					goto IL_025A;
					IL_019C:
					if (taskAwaiter5.GetResult())
					{
						taskAwaiter3 = simpleMainPage.ShowCombinedChart().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnLiveData_Clicked>d__45>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = simpleMainPage.ShowSeparateCharts().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnLiveData_Clicked>d__45>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0253;
					}
					IL_01F9:
					taskAwaiter3.GetResult();
					goto IL_025A;
					IL_0253:
					taskAwaiter3.GetResult();
					IL_025A:
					simpleMainPage.EnableMainButtons();
					IL_0260:;
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

			// Token: 0x060005B7 RID: 1463 RVA: 0x0005B6A8 File Offset: 0x000598A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000487 RID: 1159
			public int <>1__state;

			// Token: 0x04000488 RID: 1160
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000489 RID: 1161
			public SimpleMainPage <>4__this;

			// Token: 0x0400048A RID: 1162
			private TaskAwaiter <>u__1;

			// Token: 0x0400048B RID: 1163
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x0200012C RID: 300
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnMode06_Clicked>d__50 : IAsyncStateMachine
		{
			// Token: 0x060005B8 RID: 1464 RVA: 0x0005B6B8 File Offset: 0x000598B8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0121;
					}
					case 2:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0192;
					}
					default:
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_01A7;
						}
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						simpleMainPage.DisableMainButtons();
						v = sender as View;
						if (v == null)
						{
							goto IL_00C3;
						}
						taskAwaiter = ViewExtensions.FadeTo(v, 0.5, 100U, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<btnMode06_Clicked>d__50>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00C3:
					taskAwaiter3 = simpleMainPage.Navigation.PushAsync(new Mode06Page()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnMode06_Clicked>d__50>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0121:
					taskAwaiter3.GetResult();
					if (v == null)
					{
						goto IL_019A;
					}
					taskAwaiter = ViewExtensions.FadeTo(v, 1.0, 100U, null).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<btnMode06_Clicked>d__50>(ref taskAwaiter, ref this);
						return;
					}
					IL_0192:
					taskAwaiter.GetResult();
					IL_019A:
					simpleMainPage.EnableMainButtons();
					v = null;
					IL_01A7:;
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

			// Token: 0x060005B9 RID: 1465 RVA: 0x0005B8B8 File Offset: 0x00059AB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400048C RID: 1164
			public int <>1__state;

			// Token: 0x0400048D RID: 1165
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400048E RID: 1166
			public SimpleMainPage <>4__this;

			// Token: 0x0400048F RID: 1167
			public object sender;

			// Token: 0x04000490 RID: 1168
			private View <v>5__2;

			// Token: 0x04000491 RID: 1169
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000492 RID: 1170
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200012D RID: 301
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPurchase_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x060005BA RID: 1466 RVA: 0x0005B8C8 File Offset: 0x00059AC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						simpleMainPage.DisableMainButtons();
						Page inAppPage = InAppManager.GetInAppPage();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(inAppPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnPurchase_Clicked>d__10>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x060005BB RID: 1467 RVA: 0x0005B994 File Offset: 0x00059B94
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000493 RID: 1171
			public int <>1__state;

			// Token: 0x04000494 RID: 1172
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000495 RID: 1173
			public SimpleMainPage <>4__this;

			// Token: 0x04000496 RID: 1174
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200012E RID: 302
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRecords_Clicked>d__54 : IAsyncStateMachine
		{
			// Token: 0x060005BC RID: 1468 RVA: 0x0005B9A4 File Offset: 0x00059BA4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						simpleMainPage.DisableMainButtons();
						SettingsRecording settingsRecording = new SettingsRecording();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(settingsRecording).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnRecords_Clicked>d__54>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x060005BD RID: 1469 RVA: 0x0005BA70 File Offset: 0x00059C70
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000497 RID: 1175
			public int <>1__state;

			// Token: 0x04000498 RID: 1176
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000499 RID: 1177
			public SimpleMainPage <>4__this;

			// Token: 0x0400049A RID: 1178
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200012F RID: 303
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSettings_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x060005BE RID: 1470 RVA: 0x0005BA80 File Offset: 0x00059C80
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						simpleMainPage.DisableMainButtons();
						SettingsRoot settingsRoot = new SettingsRoot();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(settingsRoot).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnSettings_Clicked>d__8>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
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

			// Token: 0x060005BF RID: 1471 RVA: 0x0005BB4C File Offset: 0x00059D4C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400049B RID: 1179
			public int <>1__state;

			// Token: 0x0400049C RID: 1180
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400049D RID: 1181
			public SimpleMainPage <>4__this;

			// Token: 0x0400049E RID: 1182
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000130 RID: 304
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSpeedTest_Clicked>d__53 : IAsyncStateMachine
		{
			// Token: 0x060005C0 RID: 1472 RVA: 0x0005BB5C File Offset: 0x00059D5C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_0095;
						}
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						simpleMainPage.DisableMainButtons();
						SpeedTestPage speedTestPage = new SpeedTestPage();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(speedTestPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnSpeedTest_Clicked>d__53>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					IL_0095:;
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

			// Token: 0x060005C1 RID: 1473 RVA: 0x0005BC3C File Offset: 0x00059E3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400049F RID: 1183
			public int <>1__state;

			// Token: 0x040004A0 RID: 1184
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004A1 RID: 1185
			public SimpleMainPage <>4__this;

			// Token: 0x040004A2 RID: 1186
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000131 RID: 305
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnStartSimulation_Clicked>d__32 : IAsyncStateMachine
		{
			// Token: 0x060005C2 RID: 1474 RVA: 0x0005BC4C File Offset: 0x00059E4C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter<Task> taskAwaiter3;
					bool flag;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<Task> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Task>);
						num2 = -1;
						goto IL_0143;
					}
					case 2:
					{
						TaskAwaiter<Task> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Task>);
						num2 = -1;
						goto IL_01E1;
					}
					default:
						pid_load = null;
						flag = !string.IsNullOrEmpty(SharedSettings.Current.LastCarAvailableSensors);
						if (!flag)
						{
							goto IL_00C4;
						}
						taskAwaiter = simpleMainPage.DisplayAlert(Translate.GetString("ios_DemoModeSelectorTitle"), Translate.GetString("ios_DemoModeSelectorText"), Translate.GetString("ios_DemoModeLastCar"), Translate.GetString("ios_DemoModeAllSensors")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<btnStartSimulation_Clicked>d__32>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					flag = taskAwaiter.GetResult();
					IL_00C4:
					if (flag)
					{
						simpleMainPage.activityFrame.IsVisible = true;
						if (pid_load == null)
						{
							goto IL_014B;
						}
						taskAwaiter3 = Task.WhenAny(new Task[] { pid_load }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, SimpleMainPage.<btnStartSimulation_Clicked>d__32>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						simpleMainPage.activityFrame.IsVisible = true;
						if (pid_load == null)
						{
							goto IL_01E9;
						}
						taskAwaiter3 = Task.WhenAny(new Task[] { pid_load }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, SimpleMainPage.<btnStartSimulation_Clicked>d__32>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01E1;
					}
					IL_0143:
					taskAwaiter3.GetResult();
					IL_014B:
					App.OBDSimulator.Start(true);
					simpleMainPage.activityFrame.IsVisible = false;
					goto IL_0201;
					IL_01E1:
					taskAwaiter3.GetResult();
					IL_01E9:
					App.OBDSimulator.Start(false);
					simpleMainPage.activityFrame.IsVisible = false;
					IL_0201:
					LiveDataPIDModel.GetSupportedPIDsTEST(App.OBDReader);
					App.OBDReader.SetStatusForTest(OBDDataReaderStatus.ConnectedToECU);
					simpleMainPage.UpdateVINInfo();
				}
				catch (Exception ex)
				{
					num2 = -2;
					pid_load = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				pid_load = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060005C3 RID: 1475 RVA: 0x0005BED0 File Offset: 0x0005A0D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004A3 RID: 1187
			public int <>1__state;

			// Token: 0x040004A4 RID: 1188
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004A5 RID: 1189
			public SimpleMainPage <>4__this;

			// Token: 0x040004A6 RID: 1190
			private Task <pid_load>5__2;

			// Token: 0x040004A7 RID: 1191
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040004A8 RID: 1192
			private TaskAwaiter<Task> <>u__2;
		}

		// Token: 0x02000132 RID: 306
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnTerminal_Clicked>d__44 : IAsyncStateMachine
		{
			// Token: 0x060005C4 RID: 1476 RVA: 0x0005BEE0 File Offset: 0x0005A0E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter<Page> taskAwaiter6;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0130;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0195;
					}
					case 3:
					{
						TaskAwaiter<Page> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_0213;
					}
					default:
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
						{
							simpleMainPage.DisableMainButtons();
							TerminalPage terminalPage = new TerminalPage();
							taskAwaiter3 = simpleMainPage.Navigation.PushAsync(terminalPage).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnTerminal_Clicked>d__44>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = simpleMainPage.DisplayAlert(Translate.GetString("ios_ConnectToELMOnlyTitle"), Translate.GetString("ios_ConnectToELMOnlyText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SimpleMainPage.<btnTerminal_Clicked>d__44>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0130;
						}
						break;
					}
					taskAwaiter3.GetResult();
					simpleMainPage.EnableMainButtons();
					goto IL_021B;
					IL_0130:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_021B;
					}
					taskAwaiter3 = SimpleMainPage.Instance.StartConnection(false).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnTerminal_Clicked>d__44>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0195:
					taskAwaiter3.GetResult();
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU && App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToELM)
					{
						goto IL_021B;
					}
					taskAwaiter6 = App.GetCurrentPage().Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<Page> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SimpleMainPage.<btnTerminal_Clicked>d__44>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0213:
					taskAwaiter6.GetResult();
					IL_021B:;
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

			// Token: 0x060005C5 RID: 1477 RVA: 0x0005C154 File Offset: 0x0005A354
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004A9 RID: 1193
			public int <>1__state;

			// Token: 0x040004AA RID: 1194
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004AB RID: 1195
			public SimpleMainPage <>4__this;

			// Token: 0x040004AC RID: 1196
			private TaskAwaiter <>u__1;

			// Token: 0x040004AD RID: 1197
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x040004AE RID: 1198
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x02000133 RID: 307
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnVersions_Clicked>d__15 : IAsyncStateMachine
		{
			// Token: 0x060005C6 RID: 1478 RVA: 0x0005C164 File Offset: 0x0005A364
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SimpleMainPage simpleMainPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!simpleMainPage.CheckConnected(false))
						{
							goto IL_0088;
						}
						simpleMainPage.DisableMainButtons();
						taskAwaiter = simpleMainPage.Navigation.PushAsync(new EcuInfoPageV2()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SimpleMainPage.<btnVersions_Clicked>d__15>(ref taskAwaiter, ref this);
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
					simpleMainPage.EnableMainButtons();
					IL_0088:;
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

			// Token: 0x060005C7 RID: 1479 RVA: 0x0005C238 File Offset: 0x0005A438
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004AF RID: 1199
			public int <>1__state;

			// Token: 0x040004B0 RID: 1200
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004B1 RID: 1201
			public SimpleMainPage <>4__this;

			// Token: 0x040004B2 RID: 1202
			private TaskAwaiter <>u__1;
		}
	}
}
