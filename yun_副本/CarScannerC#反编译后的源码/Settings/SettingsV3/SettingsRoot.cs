using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Coding;
using CarScannerXamarinForms.Coding.PagesV2;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.DTCv2;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002AA RID: 682
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsRoot.xaml")]
	public class SettingsRoot : ContentPage
	{
		// Token: 0x06002170 RID: 8560 RVA: 0x00190B38 File Offset: 0x0018ED38
		public SettingsRoot()
		{
			try
			{
				this.InitializeComponent();
				if (PlatformHelper.AppMarket == Markets.RUS)
				{
					this.cellRate.IsVisible = false;
				}
				if (!SharedSettings.Current.AdsProductPurchased || (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated))
				{
					this.buyCell.IsVisible = true;
				}
				else
				{
					this.buyCell.IsVisible = false;
				}
				if (App.UseLegacyUI && PlatformHelper.IsiOS)
				{
					this.cellInterface.IsVisible = false;
					this.cellUnits.IsVisible = false;
					this.cellInterfaceAndUnits.IsVisible = true;
				}
				base.Appearing += this.SettingsRoot_Appearing;
				base.Disappearing += this.SettingsRoot_Disappearing;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x170010E2 RID: 4322
		// (get) Token: 0x06002171 RID: 8561 RVA: 0x00190C10 File Offset: 0x0018EE10
		public bool IsCodingAvailable
		{
			get
			{
				return CodingListModel.IsCodingAvailable(true);
			}
		}

		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x06002172 RID: 8562 RVA: 0x00190C18 File Offset: 0x0018EE18
		public bool IsIdentsAvailable
		{
			get
			{
				return App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive && DTCv2Model.IsDTCv2Available;
			}
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x00190C40 File Offset: 0x0018EE40
		private void SettingsRoot_Disappearing(object sender, EventArgs e)
		{
			App.OBDReader.StatusChanged -= this.OBDReader_StatusChanged;
			try
			{
				this.connectionCell.BindingContext = null;
				this.connectionProfileCell.BindingContext = null;
				this.buyCell.BindingContext = null;
				this.customCodingsCell.BindingContext = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x00190CA8 File Offset: 0x0018EEA8
		private void SettingsRoot_Appearing(object sender, EventArgs e)
		{
			App.OBDReader.StatusChanged -= this.OBDReader_StatusChanged;
			App.OBDReader.StatusChanged += this.OBDReader_StatusChanged;
			this.OBDReader_StatusChanged(App.OBDReader.CurrentStatus);
			this.connectionCell.BindingContext = SharedSettings.Current;
			this.connectionProfileCell.BindingContext = SharedSettings.Current;
			this.buyCell.BindingContext = SharedSettings.Current;
			this.customCodingsCell.BindingContext = SharedSettings.Current;
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x00190D31 File Offset: 0x0018EF31
		private void OBDReader_StatusChanged(OBDDataReaderStatus NewStatus)
		{
			this.OnPropertyChanged("IsCodingAvailable");
			this.OnPropertyChanged("IsIdentsAvailable");
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x00190D4C File Offset: 0x0018EF4C
		private async void LaunchPage(Type pageV3, Type pageLegacy)
		{
			this.settingsLayoutRoot.IsEnabled = false;
			Type type = pageV3;
			if (App.UseLegacyUI && PlatformHelper.IsiOS)
			{
				type = pageLegacy;
			}
			int num = 0;
			try
			{
				Page page = (Page)Activator.CreateInstance(type);
				await base.Navigation.PushAsync(page);
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				await base.DisplayAlert("Error", ((Exception)obj).ToStringWithInnerExceptions(), "OK");
			}
			obj = null;
			this.settingsLayoutRoot.IsEnabled = true;
		}

		// Token: 0x06002177 RID: 8567 RVA: 0x00190D94 File Offset: 0x0018EF94
		private async void LaunchPage(Page page)
		{
			this.settingsLayoutRoot.IsEnabled = false;
			await base.Navigation.PushAsync(page);
			this.settingsLayoutRoot.IsEnabled = true;
		}

		// Token: 0x06002178 RID: 8568 RVA: 0x00190DD3 File Offset: 0x0018EFD3
		private void Cell_CodingTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(CodingModeSelectionV2), typeof(CodingModeSelection));
				return;
			}
			this.LaunchPage(new CodingModeSelectionV2());
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x00190E02 File Offset: 0x0018F002
		private void Cell_IdentsTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(EcuInfoPageV2), typeof(EcuInfoPageV2));
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x00190E1E File Offset: 0x0018F01E
		private void Cell_GarageTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(SettingsGarage), typeof(SettingsGarage));
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x00190E3A File Offset: 0x0018F03A
		private void Cell_ConnectionTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsConnectionPageV3), typeof(SettingsConnectionPage));
				return;
			}
			this.LaunchPage(new SettingsConnectionPageV3());
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x00190E69 File Offset: 0x0018F069
		private void Cell_InterfaceTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsInterfacePageV3), typeof(SettingsInterfacePage));
				return;
			}
			this.LaunchPage(new SettingsInterfacePageV3());
		}

		// Token: 0x0600217D RID: 8573 RVA: 0x00190E98 File Offset: 0x0018F098
		private void Cell_UnitsTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsUnitsV3), typeof(SettingsInterfacePage));
				return;
			}
			this.LaunchPage(new SettingsUnitsV3());
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x00190EC7 File Offset: 0x0018F0C7
		private void Cell_DashboardTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsDashboardV3), typeof(SettingsDashboard));
				return;
			}
			this.LaunchPage(new SettingsDashboardV3());
		}

		// Token: 0x0600217F RID: 8575 RVA: 0x00190EF6 File Offset: 0x0018F0F6
		private void Cell_VehicleOptionsTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsVehicleOptionsPageV3), typeof(SettingsVehicleOptions));
				return;
			}
			this.LaunchPage(new SettingsVehicleOptionsPageV3());
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x00190F25 File Offset: 0x0018F125
		private void Cell_FuelConsumptionTapped(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				this.LaunchPage(typeof(SettingsFuelRateV3), typeof(SettingsFuelRatePage));
				return;
			}
			this.LaunchPage(new SettingsFuelRateV3());
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x00190F54 File Offset: 0x0018F154
		private async void Cell_SensorsTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(SettingsPIDOverrideListPage), typeof(SettingsPIDOverrideListPage));
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x00190F8C File Offset: 0x0018F18C
		private async void Cell_RateTapped(object sender, EventArgs e)
		{
			string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), PlatformHelper.AppMarketTitle);
			bool flag = Device.RuntimePlatform == "iOS";
			if (!flag)
			{
				flag = await base.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content"));
			}
			if (flag)
			{
				IRequestReview requestReview = DependencyService.Get<IRequestReview>(0);
				if (requestReview != null)
				{
					requestReview.Request(true);
				}
			}
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x00190FC4 File Offset: 0x0018F1C4
		private void Cell_BuyTapped(object sender, EventArgs e)
		{
			this.settingsLayoutRoot.IsEnabled = false;
			Page inAppPage = InAppManager.GetInAppPage();
			base.Navigation.PushAsync(inAppPage);
			this.settingsLayoutRoot.IsEnabled = true;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x00190FFC File Offset: 0x0018F1FC
		private void Cell_BackupTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(SettingsBackup), typeof(SettingsBackup));
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x00191018 File Offset: 0x0018F218
		private async void Cell_TerminalTapped(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
			{
				await base.Navigation.PushAsync(new TerminalPage());
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
					SimpleMainPage.Instance.StartConnection(false);
					Page currentPage = App.GetCurrentPage();
					if (currentPage != null && currentPage != SimpleMainPage.Instance)
					{
						await App.GetCurrentPage().Navigation.PopAsync();
					}
				}
			}
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x00191050 File Offset: 0x0018F250
		private async void Cell_ContactDeveloper(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				await base.Navigation.PushAsync(new ContactDeveloperPage());
			}
			else
			{
				try
				{
					await base.DisplayAlert(Translate.GetString("ios_ContactDeveloper"), Translate.GetString("ios_PleaseDisconnectFirst_Title"), "OK");
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x00191087 File Offset: 0x0018F287
		private void Cell_InfoPageTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(SettingsInfoV3), typeof(InfoPage));
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x001910A3 File Offset: 0x0018F2A3
		private void Cell_CustomCodingsTapped(object sender, EventArgs e)
		{
			this.LaunchPage(typeof(SettingsCustomCodingsListPage), typeof(SettingsCustomCodingsListPage));
		}

		// Token: 0x06002189 RID: 8585 RVA: 0x001910C0 File Offset: 0x0018F2C0
		private void btnProfileSelector_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				ProfileSelectorV2Page profileSelectorV2Page = new ProfileSelectorV2Page();
				base.Navigation.PushAsync(profileSelectorV2Page);
				return;
			}
			base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x00191110 File Offset: 0x0018F310
		private void Cell_ManageSubscriptionsTapped(object sender, EventArgs e)
		{
			try
			{
				Launcher.TryOpenAsync("https://play.google.com/store/account/subscriptions");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600218B RID: 8587 RVA: 0x00191140 File Offset: 0x0018F340
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsRoot).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsRoot.xaml",
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
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			EmptyConverterParameterToNotSelectedStringConverter emptyConverterParameterToNotSelectedStringConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyConverterParameterToNotSelectedStringConverter = new EmptyConverterParameterToNotSelectedStringConverter(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			ConnectionSettingsToStringMultiConverter connectionSettingsToStringMultiConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionSettingsToStringMultiConverter = new ConnectionSettingsToStringMultiConverter(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 13);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 21);
			CommandCell commandCell;
			VisualDiagnostics.RegisterSourceInfo(commandCell = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 21);
			CommandCell commandCell2;
			VisualDiagnostics.RegisterSourceInfo(commandCell2 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 21);
			CommandCell commandCell3;
			VisualDiagnostics.RegisterSourceInfo(commandCell3 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 21);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 21);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 39);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 30);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 30);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 30);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 30);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 26);
			CommandCell commandCell4;
			VisualDiagnostics.RegisterSourceInfo(commandCell4 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 21);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 21);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 39);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 30);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 30);
			MultiBinding multiBinding2;
			VisualDiagnostics.RegisterSourceInfo(multiBinding2 = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 26);
			CommandCell commandCell5;
			VisualDiagnostics.RegisterSourceInfo(commandCell5 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 21);
			CommandCell commandCell6;
			VisualDiagnostics.RegisterSourceInfo(commandCell6 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 21);
			CommandCell commandCell7;
			VisualDiagnostics.RegisterSourceInfo(commandCell7 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 21);
			CommandCell commandCell8;
			VisualDiagnostics.RegisterSourceInfo(commandCell8 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 21);
			CommandCell commandCell9;
			VisualDiagnostics.RegisterSourceInfo(commandCell9 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 21);
			CommandCell commandCell10;
			VisualDiagnostics.RegisterSourceInfo(commandCell10 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 21);
			CommandCell commandCell11;
			VisualDiagnostics.RegisterSourceInfo(commandCell11 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 18);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 21);
			CommandCell commandCell12;
			VisualDiagnostics.RegisterSourceInfo(commandCell12 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 21);
			CommandCell commandCell13;
			VisualDiagnostics.RegisterSourceInfo(commandCell13 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 21);
			SharedSettings sharedSettings3;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings3 = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 21);
			CommandCell commandCell14;
			VisualDiagnostics.RegisterSourceInfo(commandCell14 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 18);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 21);
			SharedSettings sharedSettings4;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings4 = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 21);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 21);
			CommandCell commandCell15;
			VisualDiagnostics.RegisterSourceInfo(commandCell15 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 21);
			CommandCell commandCell16;
			VisualDiagnostics.RegisterSourceInfo(commandCell16 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 21);
			CommandCell commandCell17;
			VisualDiagnostics.RegisterSourceInfo(commandCell17 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 21);
			CommandCell commandCell18;
			VisualDiagnostics.RegisterSourceInfo(commandCell18 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 21);
			CommandCell commandCell19;
			VisualDiagnostics.RegisterSourceInfo(commandCell19 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 21);
			SharedSettings sharedSettings5;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings5 = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 21);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 21);
			CommandCell commandCell20;
			VisualDiagnostics.RegisterSourceInfo(commandCell20 = new CommandCell(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsRoot.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("sectConnection", section);
			if (section.StyleId == null)
			{
				section.StyleId = "sectConnection";
			}
			nameScope.RegisterName("connectionCell", commandCell4);
			if (commandCell4.StyleId == null)
			{
				commandCell4.StyleId = "connectionCell";
			}
			nameScope.RegisterName("connectionProfileCell", commandCell5);
			if (commandCell5.StyleId == null)
			{
				commandCell5.StyleId = "connectionProfileCell";
			}
			nameScope.RegisterName("cellInterfaceAndUnits", commandCell6);
			if (commandCell6.StyleId == null)
			{
				commandCell6.StyleId = "cellInterfaceAndUnits";
			}
			nameScope.RegisterName("cellInterface", commandCell7);
			if (commandCell7.StyleId == null)
			{
				commandCell7.StyleId = "cellInterface";
			}
			nameScope.RegisterName("cellUnits", commandCell8);
			if (commandCell8.StyleId == null)
			{
				commandCell8.StyleId = "cellUnits";
			}
			nameScope.RegisterName("cellRate", commandCell13);
			if (commandCell13.StyleId == null)
			{
				commandCell13.StyleId = "cellRate";
			}
			nameScope.RegisterName("buyCell", commandCell14);
			if (commandCell14.StyleId == null)
			{
				commandCell14.StyleId = "buyCell";
			}
			nameScope.RegisterName("manageSubscriptionsCell", commandCell15);
			if (commandCell15.StyleId == null)
			{
				commandCell15.StyleId = "manageSubscriptionsCell";
			}
			nameScope.RegisterName("customCodingsCell", commandCell20);
			if (commandCell20.StyleId == null)
			{
				commandCell20.StyleId = "customCodingsCell";
			}
			this.page = this;
			this.settingsLayoutRoot = settingsView;
			this.sectConnection = section;
			this.connectionCell = commandCell4;
			this.connectionProfileCell = commandCell5;
			this.cellInterfaceAndUnits = commandCell6;
			this.cellInterface = commandCell7;
			this.cellUnits = commandCell8;
			this.cellRate = commandCell13;
			this.buyCell = commandCell14;
			this.manageSubscriptionsCell = commandCell15;
			this.customCodingsCell = commandCell20;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EmptyConverterParameterToNotSelectedStringConverter", emptyConverterParameterToNotSelectedStringConverter);
			resourceDictionary.Add("ConnectionSettingsToStringMultiConverter", connectionSettingsToStringMultiConverter);
			translate.Text = "ios_MainPage_Settings";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension3 = referenceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 2];
			array3[0] = settingsView;
			array3[1] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 13)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			settingsView.SetValue(BindableObject.BindingContextProperty, obj5);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			section.SetValue(SectionBase.TitleProperty, "Connection");
			translate2.Text = "coding_Coding";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = commandCell;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, CellBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			commandCell.Title = obj7;
			commandCell.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_coding.png"));
			bindingExtension.Mode = 2;
			bindingExtension.Path = "IsCodingAvailable";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			commandCell.SetBinding(CellBase.IsVisibleProperty, bindingBase);
			commandCell.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell.Tapped += this.Cell_CodingTapped;
			section.Add(commandCell);
			translate3.Text = "ecuIdentsTitle";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = commandCell2;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, CellBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 21)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			commandCell2.Title = obj9;
			commandCell2.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_report.png"));
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "IsIdentsAvailable";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			commandCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase2);
			commandCell2.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell2.Tapped += this.Cell_IdentsTapped;
			section.Add(commandCell2);
			translate4.Text = "ios_GarageTitle";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = commandCell3;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, CellBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			commandCell3.Title = obj11;
			commandCell3.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_mycars.png"));
			commandCell3.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell3.Tapped += this.Cell_GarageTapped;
			section.Add(commandCell3);
			translate5.Text = "Settings_Adapter";
			IMarkupExtension markupExtension7 = translate5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = commandCell4;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, CellBase.TitleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			commandCell4.Title = obj13;
			commandCell4.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			commandCell4.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_connection.png"));
			commandCell4.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell4.Tapped += this.Cell_ConnectionTapped;
			staticResourceExtension.Key = "ConnectionSettingsToStringMultiConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = multiBinding;
			array8[1] = commandCell4;
			array8[2] = section;
			array8[3] = settingsView;
			array8[4] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 39)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			multiBinding.Converter = obj15;
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "ConnectionType";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "BTDeviceName";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase4);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "BTLEDeviceName";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "WiFiServer";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase6);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "WiFiPort";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase7);
			commandCell4.SetBinding(CellBase.DescriptionProperty, multiBinding);
			section.Add(commandCell4);
			translate6.Text = "settings_ConnectionProfile";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = commandCell5;
			array9[1] = section;
			array9[2] = settingsView;
			array9[3] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, CellBase.TitleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 21)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			commandCell5.Title = obj17;
			commandCell5.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			commandCell5.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_connectionprofile.png"));
			commandCell5.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell5.Tapped += this.btnProfileSelector_Clicked;
			staticResourceExtension2.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = multiBinding2;
			array10[1] = commandCell5;
			array10[2] = section;
			array10[3] = settingsView;
			array10[4] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 39)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			multiBinding2.Converter = obj19;
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "BrandAndProfile";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase8);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "SelectedProfileV2Name";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase9);
			commandCell5.SetBinding(CellBase.DescriptionProperty, multiBinding2);
			section.Add(commandCell5);
			translate7.Text = "SettingsPage_itemInterface.Content";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = commandCell6;
			array11[1] = section;
			array11[2] = settingsView;
			array11[3] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array11, CellBase.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 21)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			commandCell6.Title = obj21;
			commandCell6.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_interface.png"));
			commandCell6.SetValue(CellBase.IsVisibleProperty, false);
			commandCell6.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell6.Tapped += this.Cell_InterfaceTapped;
			section.Add(commandCell6);
			translate8.Text = "settings_Interface";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = commandCell7;
			array12[1] = section;
			array12[2] = settingsView;
			array12[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 21)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			commandCell7.Title = obj23;
			commandCell7.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_interface.png"));
			commandCell7.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell7.Tapped += this.Cell_InterfaceTapped;
			section.Add(commandCell7);
			translate9.Text = "settings_Units";
			IMarkupExtension markupExtension13 = translate9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = commandCell8;
			array13[1] = section;
			array13[2] = settingsView;
			array13[3] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle25, obj24 = new SimpleValueTargetProvider(array13, CellBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 21)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			commandCell8.Title = obj25;
			commandCell8.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_units.png"));
			commandCell8.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell8.Tapped += this.Cell_UnitsTapped;
			section.Add(commandCell8);
			translate10.Text = "ios_DashboardSettings";
			IMarkupExtension markupExtension14 = translate10;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = commandCell9;
			array14[1] = section;
			array14[2] = settingsView;
			array14[3] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle27, obj26 = new SimpleValueTargetProvider(array14, CellBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 21)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			commandCell9.Title = obj27;
			commandCell9.SetValue(CommandCell.HideArrowIndicatorProperty, false);
			commandCell9.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_dashboard.png"));
			commandCell9.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell9.Tapped += this.Cell_DashboardTapped;
			section.Add(commandCell9);
			translate11.Text = "Settings_Control_VehicleOptions.Header";
			IMarkupExtension markupExtension15 = translate11;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = commandCell10;
			array15[1] = section;
			array15[2] = settingsView;
			array15[3] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle29, obj28 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 21)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			commandCell10.Title = obj29;
			commandCell10.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_vehicleoptions.png"));
			commandCell10.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell10.Tapped += this.Cell_VehicleOptionsTapped;
			section.Add(commandCell10);
			translate12.Text = "Settings_Control_FuelFlowItem.Content";
			IMarkupExtension markupExtension16 = translate12;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = commandCell11;
			array16[1] = section;
			array16[2] = settingsView;
			array16[3] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle31, obj30 = new SimpleValueTargetProvider(array16, CellBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 21)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			commandCell11.Title = obj31;
			commandCell11.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_fuel.png"));
			commandCell11.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell11.Tapped += this.Cell_FuelConsumptionTapped;
			section.Add(commandCell11);
			translate13.Text = "ios_Sensors";
			IMarkupExtension markupExtension17 = translate13;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = commandCell12;
			array17[1] = section;
			array17[2] = settingsView;
			array17[3] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle33, obj32 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 21)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			commandCell12.Title = obj33;
			commandCell12.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_sensors.png"));
			commandCell12.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell12.Tapped += this.Cell_SensorsTapped;
			section.Add(commandCell12);
			translate14.Text = "ios_SettingsRate";
			IMarkupExtension markupExtension18 = translate14;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = commandCell13;
			array18[1] = section;
			array18[2] = settingsView;
			array18[3] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle35, obj34 = new SimpleValueTargetProvider(array18, CellBase.TitleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 21)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			commandCell13.Title = obj35;
			commandCell13.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_rate.png"));
			commandCell13.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell13.Tapped += this.Cell_RateTapped;
			section.Add(commandCell13);
			translate15.Text = "ios_Buy";
			IMarkupExtension markupExtension19 = translate15;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = commandCell14;
			array19[1] = section;
			array19[2] = settingsView;
			array19[3] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle37, obj36 = new SimpleValueTargetProvider(array19, CellBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 21)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			commandCell14.Title = obj37;
			commandCell14.SetValue(BindableObject.BindingContextProperty, sharedSettings3);
			commandCell14.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_purchase.png"));
			bindingExtension10.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = bindingExtension10;
			array20[1] = commandCell14;
			array20[2] = section;
			array20[3] = settingsView;
			array20[4] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle39, obj38 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 21)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension10.Converter = obj39;
			bindingExtension10.Path = "AdsProductPurchased";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			commandCell14.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			commandCell14.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell14.Tapped += this.Cell_BuyTapped;
			section.Add(commandCell14);
			translate16.Text = "settings_ManageSubscriptions";
			IMarkupExtension markupExtension21 = translate16;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = commandCell15;
			array21[1] = section;
			array21[2] = settingsView;
			array21[3] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle41, obj40 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 21)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			commandCell15.Title = obj41;
			commandCell15.SetValue(BindableObject.BindingContextProperty, sharedSettings4);
			commandCell15.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_purchase.png"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "ManageSubscriptionsVisible";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			commandCell15.SetBinding(CellBase.IsVisibleProperty, bindingBase11);
			commandCell15.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell15.Tapped += this.Cell_ManageSubscriptionsTapped;
			section.Add(commandCell15);
			translate17.Text = "ios_BackupTitle";
			IMarkupExtension markupExtension22 = translate17;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = commandCell16;
			array22[1] = section;
			array22[2] = settingsView;
			array22[3] = this;
			object obj42;
			xamlServiceProvider22.Add(typeFromHandle43, obj42 = new SimpleValueTargetProvider(array22, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 21)));
			object obj43 = markupExtension22.ProvideValue(xamlServiceProvider22);
			commandCell16.Title = obj43;
			commandCell16.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_backup.png"));
			commandCell16.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell16.Tapped += this.Cell_BackupTapped;
			section.Add(commandCell16);
			translate18.Text = "ios_MainPage_TileTerminal";
			IMarkupExtension markupExtension23 = translate18;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = commandCell17;
			array23[1] = section;
			array23[2] = settingsView;
			array23[3] = this;
			object obj44;
			xamlServiceProvider23.Add(typeFromHandle45, obj44 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 21)));
			object obj45 = markupExtension23.ProvideValue(xamlServiceProvider23);
			commandCell17.Title = obj45;
			commandCell17.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_terminal.png"));
			commandCell17.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell17.Tapped += this.Cell_TerminalTapped;
			section.Add(commandCell17);
			translate19.Text = "ios_ContactDeveloper";
			IMarkupExtension markupExtension24 = translate19;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = commandCell18;
			array24[1] = section;
			array24[2] = settingsView;
			array24[3] = this;
			object obj46;
			xamlServiceProvider24.Add(typeFromHandle47, obj46 = new SimpleValueTargetProvider(array24, CellBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 21)));
			object obj47 = markupExtension24.ProvideValue(xamlServiceProvider24);
			commandCell18.Title = obj47;
			commandCell18.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_contact.png"));
			commandCell18.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell18.Tapped += this.Cell_ContactDeveloper;
			section.Add(commandCell18);
			translate20.Text = "SettingsPage_itemHelpInfo.Content";
			IMarkupExtension markupExtension25 = translate20;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = commandCell19;
			array25[1] = section;
			array25[2] = settingsView;
			array25[3] = this;
			object obj48;
			xamlServiceProvider25.Add(typeFromHandle49, obj48 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 21)));
			object obj49 = markupExtension25.ProvideValue(xamlServiceProvider25);
			commandCell19.Title = obj49;
			commandCell19.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_info.png"));
			commandCell19.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell19.Tapped += this.Cell_InfoPageTapped;
			section.Add(commandCell19);
			translate21.Text = "settings_CustomCodings";
			IMarkupExtension markupExtension26 = translate21;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = commandCell20;
			array26[1] = section;
			array26[2] = settingsView;
			array26[3] = this;
			object obj50;
			xamlServiceProvider26.Add(typeFromHandle51, obj50 = new SimpleValueTargetProvider(array26, CellBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsRoot).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(224, 21)));
			object obj51 = markupExtension26.ProvideValue(xamlServiceProvider26);
			commandCell20.Title = obj51;
			commandCell20.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("setv3_customcodings.png"));
			bindingExtension12.Source = sharedSettings5;
			bindingExtension12.Path = "ShowExperimental";
			bindingExtension12.Mode = 2;
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			commandCell20.SetBinding(CellBase.IsVisibleProperty, bindingBase12);
			commandCell20.SetValue(CommandCell.KeepSelectedUntilBackProperty, false);
			commandCell20.Tapped += this.Cell_CustomCodingsTapped;
			section.Add(commandCell20);
			settingsView.Root.Add(section);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x00194814 File Offset: 0x00192A14
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsRoot>(this, typeof(SettingsRoot));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.sectConnection = NameScopeExtensions.FindByName<Section>(this, "sectConnection");
			this.connectionCell = NameScopeExtensions.FindByName<CommandCell>(this, "connectionCell");
			this.connectionProfileCell = NameScopeExtensions.FindByName<CommandCell>(this, "connectionProfileCell");
			this.cellInterfaceAndUnits = NameScopeExtensions.FindByName<CommandCell>(this, "cellInterfaceAndUnits");
			this.cellInterface = NameScopeExtensions.FindByName<CommandCell>(this, "cellInterface");
			this.cellUnits = NameScopeExtensions.FindByName<CommandCell>(this, "cellUnits");
			this.cellRate = NameScopeExtensions.FindByName<CommandCell>(this, "cellRate");
			this.buyCell = NameScopeExtensions.FindByName<CommandCell>(this, "buyCell");
			this.manageSubscriptionsCell = NameScopeExtensions.FindByName<CommandCell>(this, "manageSubscriptionsCell");
			this.customCodingsCell = NameScopeExtensions.FindByName<CommandCell>(this, "customCodingsCell");
		}

		// Token: 0x04000FEF RID: 4079
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04000FF0 RID: 4080
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000FF1 RID: 4081
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section sectConnection;

		// Token: 0x04000FF2 RID: 4082
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell connectionCell;

		// Token: 0x04000FF3 RID: 4083
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell connectionProfileCell;

		// Token: 0x04000FF4 RID: 4084
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell cellInterfaceAndUnits;

		// Token: 0x04000FF5 RID: 4085
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell cellInterface;

		// Token: 0x04000FF6 RID: 4086
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell cellUnits;

		// Token: 0x04000FF7 RID: 4087
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell cellRate;

		// Token: 0x04000FF8 RID: 4088
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell buyCell;

		// Token: 0x04000FF9 RID: 4089
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell manageSubscriptionsCell;

		// Token: 0x04000FFA RID: 4090
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CommandCell customCodingsCell;

		// Token: 0x020002AB RID: 683
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Cell_ContactDeveloper>d__24 : IAsyncStateMachine
		{
			// Token: 0x0600218D RID: 8589 RVA: 0x00194900 File Offset: 0x00192B00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRoot settingsRoot = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1 || App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							try
							{
								if (num != 1)
								{
									taskAwaiter = settingsRoot.DisplayAlert(Translate.GetString("ios_ContactDeveloper"), Translate.GetString("ios_PleaseDisconnectFirst_Title"), "OK").GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num2 = 1;
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<Cell_ContactDeveloper>d__24>(ref taskAwaiter, ref this);
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
							catch (Exception)
							{
							}
							goto IL_0103;
						}
						taskAwaiter = settingsRoot.Navigation.PushAsync(new ContactDeveloperPage()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<Cell_ContactDeveloper>d__24>(ref taskAwaiter, ref this);
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
					IL_0103:;
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

			// Token: 0x0600218E RID: 8590 RVA: 0x00194A58 File Offset: 0x00192C58
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FFB RID: 4091
			public int <>1__state;

			// Token: 0x04000FFC RID: 4092
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FFD RID: 4093
			public SettingsRoot <>4__this;

			// Token: 0x04000FFE RID: 4094
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002AC RID: 684
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Cell_RateTapped>d__20 : IAsyncStateMachine
		{
			// Token: 0x0600218F RID: 8591 RVA: 0x00194A68 File Offset: 0x00192C68
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRoot settingsRoot = this;
				try
				{
					bool flag;
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), PlatformHelper.AppMarketTitle);
						flag = Device.RuntimePlatform == "iOS";
						if (flag)
						{
							goto IL_00AF;
						}
						taskAwaiter = settingsRoot.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsRoot.<Cell_RateTapped>d__20>(ref taskAwaiter, ref this);
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
					flag = taskAwaiter.GetResult();
					IL_00AF:
					if (flag)
					{
						IRequestReview requestReview = DependencyService.Get<IRequestReview>(0);
						if (requestReview != null)
						{
							requestReview.Request(true);
						}
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

			// Token: 0x06002190 RID: 8592 RVA: 0x00194B78 File Offset: 0x00192D78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FFF RID: 4095
			public int <>1__state;

			// Token: 0x04001000 RID: 4096
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001001 RID: 4097
			public SettingsRoot <>4__this;

			// Token: 0x04001002 RID: 4098
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020002AD RID: 685
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Cell_SensorsTapped>d__19 : IAsyncStateMachine
		{
			// Token: 0x06002191 RID: 8593 RVA: 0x00194B88 File Offset: 0x00192D88
			void IAsyncStateMachine.MoveNext()
			{
				SettingsRoot settingsRoot = this;
				try
				{
					settingsRoot.LaunchPage(typeof(SettingsPIDOverrideListPage), typeof(SettingsPIDOverrideListPage));
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

			// Token: 0x06002192 RID: 8594 RVA: 0x00194BF4 File Offset: 0x00192DF4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001003 RID: 4099
			public int <>1__state;

			// Token: 0x04001004 RID: 4100
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001005 RID: 4101
			public SettingsRoot <>4__this;
		}

		// Token: 0x020002AE RID: 686
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Cell_TerminalTapped>d__23 : IAsyncStateMachine
		{
			// Token: 0x06002193 RID: 8595 RVA: 0x00194C04 File Offset: 0x00192E04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRoot settingsRoot = this;
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
						goto IL_011B;
					case 2:
					{
						TaskAwaiter<Page> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_01A4;
					}
					default:
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
						{
							taskAwaiter3 = settingsRoot.Navigation.PushAsync(new TerminalPage()).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<Cell_TerminalTapped>d__23>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = settingsRoot.DisplayAlert(Translate.GetString("ios_ConnectToELMOnlyTitle"), Translate.GetString("ios_ConnectToELMOnlyText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsRoot.<Cell_TerminalTapped>d__23>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_011B;
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_01AC;
					IL_011B:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_01AC;
					}
					SimpleMainPage.Instance.StartConnection(false);
					Page currentPage = App.GetCurrentPage();
					if (currentPage == null || currentPage == SimpleMainPage.Instance)
					{
						goto IL_01AC;
					}
					taskAwaiter6 = App.GetCurrentPage().Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<Page> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsRoot.<Cell_TerminalTapped>d__23>(ref taskAwaiter6, ref this);
						return;
					}
					IL_01A4:
					taskAwaiter6.GetResult();
					IL_01AC:;
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

			// Token: 0x06002194 RID: 8596 RVA: 0x00194E08 File Offset: 0x00193008
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001006 RID: 4102
			public int <>1__state;

			// Token: 0x04001007 RID: 4103
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001008 RID: 4104
			public SettingsRoot <>4__this;

			// Token: 0x04001009 RID: 4105
			private TaskAwaiter <>u__1;

			// Token: 0x0400100A RID: 4106
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x0400100B RID: 4107
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x020002AF RID: 687
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LaunchPage>d__8 : IAsyncStateMachine
		{
			// Token: 0x06002195 RID: 8597 RVA: 0x00194E18 File Offset: 0x00193018
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsRoot settingsRoot = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					Type type;
					if (num2 != 0)
					{
						if (num2 == 1)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num3 = -1;
							goto IL_014F;
						}
						settingsRoot.settingsLayoutRoot.IsEnabled = false;
						type = pageV3;
						if (App.UseLegacyUI && PlatformHelper.IsiOS)
						{
							type = pageLegacy;
						}
						num = 0;
					}
					try
					{
						if (num2 != 0)
						{
							Page page = (Page)Activator.CreateInstance(type);
							taskAwaiter = settingsRoot.Navigation.PushAsync(page).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<LaunchPage>d__8>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num3 = -1;
						}
						taskAwaiter.GetResult();
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_0156;
					}
					Exception ex2 = (Exception)obj;
					taskAwaiter = settingsRoot.DisplayAlert("Error", ex2.ToStringWithInnerExceptions(), "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num3 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<LaunchPage>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_014F:
					taskAwaiter.GetResult();
					IL_0156:
					obj = null;
					settingsRoot.settingsLayoutRoot.IsEnabled = true;
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

			// Token: 0x06002196 RID: 8598 RVA: 0x00194FF0 File Offset: 0x001931F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400100C RID: 4108
			public int <>1__state;

			// Token: 0x0400100D RID: 4109
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400100E RID: 4110
			public SettingsRoot <>4__this;

			// Token: 0x0400100F RID: 4111
			public Type pageV3;

			// Token: 0x04001010 RID: 4112
			public Type pageLegacy;

			// Token: 0x04001011 RID: 4113
			private object <>7__wrap1;

			// Token: 0x04001012 RID: 4114
			private int <>7__wrap2;

			// Token: 0x04001013 RID: 4115
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002B0 RID: 688
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LaunchPage>d__9 : IAsyncStateMachine
		{
			// Token: 0x06002197 RID: 8599 RVA: 0x00195000 File Offset: 0x00193200
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRoot settingsRoot = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsRoot.settingsLayoutRoot.IsEnabled = false;
						taskAwaiter = settingsRoot.Navigation.PushAsync(page).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRoot.<LaunchPage>d__9>(ref taskAwaiter, ref this);
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
					settingsRoot.settingsLayoutRoot.IsEnabled = true;
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

			// Token: 0x06002198 RID: 8600 RVA: 0x001950D8 File Offset: 0x001932D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001014 RID: 4116
			public int <>1__state;

			// Token: 0x04001015 RID: 4117
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001016 RID: 4118
			public SettingsRoot <>4__this;

			// Token: 0x04001017 RID: 4119
			public Page page;

			// Token: 0x04001018 RID: 4120
			private TaskAwaiter <>u__1;
		}
	}
}
