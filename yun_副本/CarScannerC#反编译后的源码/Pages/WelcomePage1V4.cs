using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200066C RID: 1644
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\WelcomePage1V4.xaml")]
	public class WelcomePage1V4 : ContentPage
	{
		// Token: 0x0600386A RID: 14442 RVA: 0x002A9AFC File Offset: 0x002A7CFC
		public WelcomePage1V4()
		{
			this.InitializeComponent();
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				base.Appearing += this.WelcomePage1V4_AppearingRUS;
			}
			this.gridInterface.BindingContext = SharedSettings.Current;
			this.gridUnits.BindingContext = SharedSettings.Current;
			this.gridFuel.BindingContext = SharedSettings.Current;
			this.gridPrivacy.BindingContext = SharedSettings.Current;
			if (PlatformHelper.IsiOS)
			{
				try
				{
					this.setEULA.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setEULA));
					this.setAdapter.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setAdapter));
					this.setInterface.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setInterface));
					this.setUnits.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setUnits));
					this.setFuel.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setFuel));
					this.setPrivacy.SetBinding(VisualElement.HeightRequestProperty, new Binding("VisibleContentHeight", 2, null, null, null, this.setPrivacy));
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x0600386B RID: 14443 RVA: 0x002A9C68 File Offset: 0x002A7E68
		private void WelcomePage1V4_AppearingRUS(object sender, EventArgs e)
		{
			if (!this.eulaUpdated)
			{
				this.SetEULARus();
			}
		}

		// Token: 0x0600386C RID: 14444 RVA: 0x002A9C78 File Offset: 0x002A7E78
		private async void SetEULARus()
		{
			this.eulaActivityFrame.IsVisible = true;
			this.lbEULA.Text = "";
			string text = await HttpDownloader.Get("https://ru.carscanner.info/eula_ru.txt", 10);
			if (!string.IsNullOrEmpty(text))
			{
				this.lbEULA.Text = text;
				this.eulaUpdated = true;
			}
			else
			{
				this.lbEULA.Text = Translate.GetString("rus_EULA");
			}
			this.eulaActivityFrame.IsVisible = false;
		}

		// Token: 0x0600386D RID: 14445 RVA: 0x002A9CB0 File Offset: 0x002A7EB0
		private async void WelcomePage1V3_SizeChanged(object sender, EventArgs e)
		{
			DisplayOrientation orientation = DeviceDisplay.MainDisplayInfo.Orientation;
			await Task.Delay(1000);
			if (DeviceDisplay.MainDisplayInfo.Orientation == orientation)
			{
				if (DeviceDisplay.MainDisplayInfo.Orientation == 2)
				{
					NavigationPage.SetHasNavigationBar(this, false);
				}
				else
				{
					NavigationPage.SetHasNavigationBar(this, true);
				}
			}
		}

		// Token: 0x0600386E RID: 14446 RVA: 0x002A9CE8 File Offset: 0x002A7EE8
		private async Task AnimateNextElement(VisualElement appearing, VisualElement dissapearing)
		{
			appearing.IsVisible = true;
			await ViewExtensions.FadeTo(appearing, 0.0, 0U, null);
			await Task.WhenAll<bool>(new Task<bool>[]
			{
				ViewExtensions.FadeTo(appearing, 1.0, 200U, null),
				ViewExtensions.FadeTo(dissapearing, 0.0, 200U, null)
			});
			dissapearing.IsVisible = false;
			dissapearing.Opacity = 1.0;
		}

		// Token: 0x0600386F RID: 14447 RVA: 0x002A9D34 File Offset: 0x002A7F34
		private async void btnAgree_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.FirstTimeLaunch = false;
			await this.AnimateNextElement(this.gridAdapter, this.gridEULA);
			base.Title = Translate.GetString("welcome_Adapter");
			this.progressBar.IsVisible = true;
			OnlinePatch.GetPatch();
		}

		// Token: 0x06003870 RID: 14448 RVA: 0x002A9D6B File Offset: 0x002A7F6B
		private void btnDisagree_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.FirstTimeLaunch = true;
			PlatformHelper.CommonService.QuitApp();
		}

		// Token: 0x06003871 RID: 14449 RVA: 0x0024A913 File Offset: 0x00248B13
		private void cellChoosingAdapter_Tapped(object sender, EventArgs e)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/choosing-obdii-adapter/");
		}

		// Token: 0x06003872 RID: 14450 RVA: 0x002A9D84 File Offset: 0x002A7F84
		private async void btnAdapterNext_Clicked(object sender, EventArgs e)
		{
			this.gridInterface.BindingContext = SharedSettings.Current;
			await this.AnimateNextElement(this.gridInterface, this.gridAdapter);
			base.Title = Translate.GetString("welcome_InitialSetup");
			this.lbPageSubtitle.Text = Translate.GetString("settings_Interface");
			this.lbPageTitleSeparator.Text = " | ";
			this.progressBar.Progress = 0.3;
			this.UpdatePreview();
			this.DroidConnectionSettingsDetector();
		}

		// Token: 0x06003873 RID: 14451 RVA: 0x002A9DBC File Offset: 0x002A7FBC
		private async void DashboardThemeItem_Tapped(object sender, EventArgs e)
		{
			await this.UpdatePreview();
			await Task.Delay(300);
			await this.UpdatePreview();
		}

		// Token: 0x06003874 RID: 14452 RVA: 0x002A9DF4 File Offset: 0x002A7FF4
		private async Task UpdatePreview()
		{
			PIDWithFloatValueFormula pid = null;
			this.gridDashPreview.Children.Clear();
			await Task.Run(delegate
			{
				try
				{
					pid = PIDWithFloatValueFormula.PID010C_EngineRPM();
					this.dashItem = new DashboardItem();
					this.dashItem.HeightRequest = 150.0;
					this.dashItem.ItemType = DashboardItemTypes.Gauge;
					this.dashItem.ShowAvg = false;
					this.dashItem.Minimum = 0.0;
					this.dashItem.Maximum = 7000.0;
					this.dashItem.GaugeShowRedLine = true;
					this.dashItem.GaugeRedLineStart = 6000.0;
					this.dashItem.GaugeRedLineFinish = 7000.0;
					this.dashItem.HorizontalOptions = LayoutOptions.Center;
					this.dashItem.Model = new LiveDataPIDModel();
					this.dashItem.Model.SelectedPID = pid;
					this.dashItem.TitleFontSize = 0.0;
					this.dashItem.UnitsFontSize = 0.0;
					this.dashItem.SelectAndAddControl();
				}
				catch (Exception)
				{
				}
			});
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				try
				{
					this.gridDashPreview.Children.Add(this.dashItem);
					DashboardItem.ApplyThemeToItem(this.dashItem);
					PIDWithFloatValueFormula pid2 = pid;
					if (pid2 != null)
					{
						pid2.SetValue(3000.0);
					}
				}
				catch (Exception)
				{
				}
			});
		}

		// Token: 0x06003875 RID: 14453 RVA: 0x002A9E38 File Offset: 0x002A8038
		private async void btnInterfaceNext_Clicked(object sender, EventArgs e)
		{
			await this.AnimateNextElement(this.gridUnits, this.gridInterface);
			this.progressBar.Progress = 0.4;
			new Binding("TitleText", 2, null, null, null, null);
			this.lbPageSubtitle.Text = Translate.GetString("settings_Units");
			this.lbPageTitleSeparator.Text = " | ";
		}

		// Token: 0x06003876 RID: 14454 RVA: 0x002A9E70 File Offset: 0x002A8070
		private async void btnUnitsNext_Clicked(object sender, EventArgs e)
		{
			await this.AnimateNextElement(this.gridProfileSelector, this.gridUnits);
			this.progressBar.Progress = 0.5;
			Binding binding = new Binding("TitleText", 2, null, null, null, null);
			this.lbPageSubtitle.BindingContext = this.profileSelector;
			this.lbPageSubtitle.SetBinding(Span.TextProperty, binding);
		}

		// Token: 0x06003877 RID: 14455 RVA: 0x002A9EA8 File Offset: 0x002A80A8
		private async void ProfileSelector_NextRequested(object sender, bool profileSelected)
		{
			this.lbPageSubtitle.Text = Translate.GetString("Settings_Control_FuelFlowItem.Content");
			await this.AnimateNextElement(this.gridFuel, this.gridProfileSelector);
			this.progressBar.Progress = 0.8;
		}

		// Token: 0x06003878 RID: 14456 RVA: 0x00182281 File Offset: 0x00180481
		private void btnPrivacyPolicy_Tapped(object sender, EventArgs e)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/privacy-policy/");
		}

		// Token: 0x06003879 RID: 14457 RVA: 0x002A9EE0 File Offset: 0x002A80E0
		private async void btnFuelNext_Clicked(object sender, EventArgs e)
		{
			RegionInfo regionInfo = null;
			try
			{
				regionInfo = RegionInfo.CurrentRegion;
			}
			catch (Exception)
			{
				regionInfo = null;
			}
			string[] array = new string[]
			{
				"AT", "BE", "BG", "CY", "CZ", "DE", "DK", "ES", "EE", "FI",
				"FR", "GB", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV",
				"MT", "NL", "PL", "PT", "RO", "SK", "SI", "SE", "UK", "US"
			};
			bool flag = false;
			bool adsProductPurchased = SharedSettings.Current.AdsProductPurchased;
			bool flag2 = regionInfo == null || array.Contains(regionInfo.TwoLetterISORegionName);
			bool isiOS = PlatformHelper.IsiOS;
			if (adsProductPurchased)
			{
				flag = false;
			}
			else if (isiOS && PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
			{
				flag = true;
				this.panelAdsSelection1.IsVisible = false;
				this.panelAdsSelection2.IsVisible = false;
			}
			else if (flag2)
			{
				flag = true;
			}
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				flag = false;
			}
			if (PlatformHelper.AppMarket == Markets.GooglePlay && !adsProductPurchased)
			{
				DependencyService.Get<IUMPConsent>(0).DisplayConsentIfRequired();
				flag = false;
			}
			if (flag)
			{
				this.progressBar.Progress = 0.99;
				this.lbPageSubtitle.Text = Translate.GetString("welcome_Privacy");
				await this.AnimateNextElement(this.gridPrivacy, this.gridFuel);
				if (PlatformHelper.IsiOS)
				{
					DependencyService.Get<IUMPConsent>(0).DisplayConsentIfRequired();
				}
			}
			else
			{
				App.Instance.ChangeLanguage();
			}
		}

		// Token: 0x0600387A RID: 14458 RVA: 0x002A9F18 File Offset: 0x002A8118
		private async void btnPrivacyNext_Clicked(object sender, EventArgs e)
		{
			this.btnPrivacyNext.IsEnabled = false;
			if (PlatformHelper.IsiOS && PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
			{
				await PlatformHelper.IOSService.TrackingRequestHelper_DisplayTrackingRequest();
			}
			App.Instance.ChangeLanguage();
		}

		// Token: 0x0600387B RID: 14459 RVA: 0x002A9F50 File Offset: 0x002A8150
		private async Task DroidConnectionSettingsDetector()
		{
			if (PlatformHelper.IsAndroid)
			{
				ConnectionDetector connectionDetector = new ConnectionDetector();
				connectionDetector.FinishedWithSuccess += delegate(object d, EventArgs args)
				{
					SharedSettings.Current.FirstConnectionAttempted = true;
				};
				await connectionDetector.DetectAndSetSettings();
			}
		}

		// Token: 0x0600387C RID: 14460 RVA: 0x002A9F8C File Offset: 0x002A818C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(WelcomePage1V4).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/WelcomePage1V4.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			CustomFuelToTrueConverter customFuelToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(customFuelToTrueConverter = new CustomFuelToTrueConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			MultiBooleanToTrueConverter multiBooleanToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(multiBooleanToTrueConverter = new MultiBooleanToTrueConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			DecimalFuelPriceForLitreToStringConverter decimalFuelPriceForLitreToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(decimalFuelPriceForLitreToStringConverter = new DecimalFuelPriceForLitreToStringConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes = FuelFlowCalculationSchemes.Auto;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes2 = FuelFlowCalculationSchemes.LOAD_ABS;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes3 = FuelFlowCalculationSchemes.MAP;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes4 = FuelFlowCalculationSchemes.Injector;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes5 = FuelFlowCalculationSchemes.CycleConsumption;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			FuelTypes fuelTypes = FuelTypes.Gasoline;
			FuelTypes fuelTypes2 = FuelTypes.Diesel;
			FuelTypes fuelTypes3 = FuelTypes.Ethanol;
			FuelTypes fuelTypes4 = FuelTypes.Methanol;
			FuelTypes fuelTypes5 = FuelTypes.Propan;
			FuelTypes fuelTypes6 = FuelTypes.Methan;
			FuelTypes fuelTypes7 = FuelTypes.FlexFuelOBDII;
			FuelTypes fuelTypes8 = FuelTypes.Custom;
			EnumToBoolConverter enumToBoolConverter2;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter2 = new EnumToBoolConverter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 52);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 45);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Button)), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 13);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 18);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 10);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 17);
			ProgressBar progressBar;
			VisualDiagnostics.RegisterSourceInfo(progressBar = new ProgressBar(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 14);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 33);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 36);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 30);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 26);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 22);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 18);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 21);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 14);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 30);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 26);
			OnPlatformExtension onPlatformExtension;
			VisualDiagnostics.RegisterSourceInfo(onPlatformExtension = new OnPlatformExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 40);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 36);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 78);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 30);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 26);
			OnPlatformExtension onPlatformExtension2;
			VisualDiagnostics.RegisterSourceInfo(onPlatformExtension2 = new OnPlatformExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 40);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 36);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 78);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 30);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 29);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 29);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 26);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 22);
			SettingsView settingsView2;
			VisualDiagnostics.RegisterSourceInfo(settingsView2 = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 18);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 33);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 83);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 29);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 26);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 29);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 29);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 29);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 29);
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 26);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 29);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 29);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 29);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 26);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 57);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 119);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 34);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 26);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 22);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 33);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 89);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 34);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 30);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 26);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 39);
			int num = 2;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 26);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 39);
			int num2 = 0;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 26);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 39);
			int num3 = 1;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 26);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 22);
			SettingsView settingsView3;
			VisualDiagnostics.RegisterSourceInfo(settingsView3 = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 18);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 294, 21);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 21);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 18);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 14);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 33);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 119);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 39);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 120);
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 26);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 39);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 121);
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 26);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 22);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 81);
			FuelConsumptionUnits fuelConsumptionUnits = FuelConsumptionUnits.LitersPer100km;
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 26);
			FuelConsumptionUnits fuelConsumptionUnits2 = FuelConsumptionUnits.KmPerLiter;
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 26);
			FuelConsumptionUnits fuelConsumptionUnits3 = FuelConsumptionUnits.MilesPerGallon;
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 321, 26);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 22);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 324, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 324, 80);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 39);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 81);
			RadioCell radioCell11;
			VisualDiagnostics.RegisterSourceInfo(radioCell11 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 325, 26);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 39);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 82);
			RadioCell radioCell12;
			VisualDiagnostics.RegisterSourceInfo(radioCell12 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 26);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 324, 22);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 331, 25);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 332, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 25);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 39);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 112);
			RadioCell radioCell13;
			VisualDiagnostics.RegisterSourceInfo(radioCell13 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 26);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 335, 39);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 335, 113);
			RadioCell radioCell14;
			VisualDiagnostics.RegisterSourceInfo(radioCell14 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 335, 26);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 22);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 338, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 338, 108);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 39);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 117);
			RadioCell radioCell15;
			VisualDiagnostics.RegisterSourceInfo(radioCell15 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 26);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 39);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 118);
			RadioCell radioCell16;
			VisualDiagnostics.RegisterSourceInfo(radioCell16 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 26);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 338, 22);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 33);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 104);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 39);
			StaticResourceExtension staticResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 113);
			RadioCell radioCell17;
			VisualDiagnostics.RegisterSourceInfo(radioCell17 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 26);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 39);
			StaticResourceExtension staticResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 114);
			RadioCell radioCell18;
			VisualDiagnostics.RegisterSourceInfo(radioCell18 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 26);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 22);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 33);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 111);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 39);
			StaticResourceExtension staticResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension19 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 120);
			RadioCell radioCell19;
			VisualDiagnostics.RegisterSourceInfo(radioCell19 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 26);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 39);
			StaticResourceExtension staticResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension20 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 121);
			RadioCell radioCell20;
			VisualDiagnostics.RegisterSourceInfo(radioCell20 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 26);
			Section section11;
			VisualDiagnostics.RegisterSourceInfo(section11 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 22);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 33);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 112);
			StaticResourceExtension staticResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension21 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 49);
			RadioCell radioCell21;
			VisualDiagnostics.RegisterSourceInfo(radioCell21 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 26);
			StaticResourceExtension staticResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension22 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 355, 55);
			RadioCell radioCell22;
			VisualDiagnostics.RegisterSourceInfo(radioCell22 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 355, 26);
			Section section12;
			VisualDiagnostics.RegisterSourceInfo(section12 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 22);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 33);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 105);
			StaticResourceExtension staticResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension23 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 50);
			RadioCell radioCell23;
			VisualDiagnostics.RegisterSourceInfo(radioCell23 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 26);
			StaticResourceExtension staticResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension24 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 50);
			RadioCell radioCell24;
			VisualDiagnostics.RegisterSourceInfo(radioCell24 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 26);
			Section section13;
			VisualDiagnostics.RegisterSourceInfo(section13 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 22);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 33);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 106);
			StaticResourceExtension staticResourceExtension25;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension25 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 50);
			RadioCell radioCell25;
			VisualDiagnostics.RegisterSourceInfo(radioCell25 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 26);
			StaticResourceExtension staticResourceExtension26;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension26 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 54);
			RadioCell radioCell26;
			VisualDiagnostics.RegisterSourceInfo(radioCell26 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 26);
			Section section14;
			VisualDiagnostics.RegisterSourceInfo(section14 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 22);
			SettingsView settingsView4;
			VisualDiagnostics.RegisterSourceInfo(settingsView4 = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 18);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 21);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 21);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 18);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 14);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 391, 22);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 399, 21);
			ProfileSelectorV3 profileSelectorV;
			VisualDiagnostics.RegisterSourceInfo(profileSelectorV = new ProfileSelectorV3(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 18);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 386, 14);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 415, 33);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 415, 75);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 39);
			StaticResourceExtension staticResourceExtension27;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension27 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 108);
			RadioCell radioCell27;
			VisualDiagnostics.RegisterSourceInfo(radioCell27 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 26);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 39);
			StaticResourceExtension staticResourceExtension28;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension28 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 103);
			RadioCell radioCell28;
			VisualDiagnostics.RegisterSourceInfo(radioCell28 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 26);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 419, 29);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 420, 29);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 418, 26);
			Section section15;
			VisualDiagnostics.RegisterSourceInfo(section15 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 415, 22);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 33);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 100);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 39);
			FuelTypes fuelTypes9 = FuelTypes.Gasoline;
			RadioCell radioCell29;
			VisualDiagnostics.RegisterSourceInfo(radioCell29 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 26);
			Translate translate49;
			VisualDiagnostics.RegisterSourceInfo(translate49 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 426, 39);
			FuelTypes fuelTypes10 = FuelTypes.Diesel;
			RadioCell radioCell30;
			VisualDiagnostics.RegisterSourceInfo(radioCell30 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 426, 26);
			Translate translate50;
			VisualDiagnostics.RegisterSourceInfo(translate50 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 39);
			FuelTypes fuelTypes11 = FuelTypes.EvNoFuel;
			RadioCell radioCell31;
			VisualDiagnostics.RegisterSourceInfo(radioCell31 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 26);
			Translate translate51;
			VisualDiagnostics.RegisterSourceInfo(translate51 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 39);
			FuelTypes fuelTypes12 = FuelTypes.Ethanol;
			RadioCell radioCell32;
			VisualDiagnostics.RegisterSourceInfo(radioCell32 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 26);
			Translate translate52;
			VisualDiagnostics.RegisterSourceInfo(translate52 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 39);
			FuelTypes fuelTypes13 = FuelTypes.Methanol;
			RadioCell radioCell33;
			VisualDiagnostics.RegisterSourceInfo(radioCell33 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 26);
			Translate translate53;
			VisualDiagnostics.RegisterSourceInfo(translate53 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 39);
			FuelTypes fuelTypes14 = FuelTypes.Propan;
			RadioCell radioCell34;
			VisualDiagnostics.RegisterSourceInfo(radioCell34 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 26);
			Translate translate54;
			VisualDiagnostics.RegisterSourceInfo(translate54 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 431, 39);
			FuelTypes fuelTypes15 = FuelTypes.Methan;
			RadioCell radioCell35;
			VisualDiagnostics.RegisterSourceInfo(radioCell35 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 431, 26);
			Translate translate55;
			VisualDiagnostics.RegisterSourceInfo(translate55 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 39);
			FuelTypes fuelTypes16 = FuelTypes.FlexFuelOBDII;
			RadioCell radioCell36;
			VisualDiagnostics.RegisterSourceInfo(radioCell36 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 26);
			Translate translate56;
			VisualDiagnostics.RegisterSourceInfo(translate56 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 39);
			FuelTypes fuelTypes17 = FuelTypes.Custom;
			RadioCell radioCell37;
			VisualDiagnostics.RegisterSourceInfo(radioCell37 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 26);
			Section section16;
			VisualDiagnostics.RegisterSourceInfo(section16 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 22);
			Translate translate57;
			VisualDiagnostics.RegisterSourceInfo(translate57 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 33);
			StaticResourceExtension staticResourceExtension29;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension29 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 81);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 81);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 438, 65);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 438, 30);
			CustomCell customCell6;
			VisualDiagnostics.RegisterSourceInfo(customCell6 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 26);
			Section section17;
			VisualDiagnostics.RegisterSourceInfo(section17 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 22);
			Translate translate58;
			VisualDiagnostics.RegisterSourceInfo(translate58 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 444, 33);
			StaticResourceExtension staticResourceExtension30;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension30 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 444, 86);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 444, 86);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 446, 65);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 446, 30);
			CustomCell customCell7;
			VisualDiagnostics.RegisterSourceInfo(customCell7 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 445, 26);
			Section section18;
			VisualDiagnostics.RegisterSourceInfo(section18 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 444, 22);
			Translate translate59;
			VisualDiagnostics.RegisterSourceInfo(translate59 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 33);
			StaticResourceExtension staticResourceExtension31;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension31 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 453, 43);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 454, 34);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 458, 37);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 455, 34);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 453, 30);
			StaticResourceExtension staticResourceExtension32;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension32 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 36);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 36);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 30);
			CustomCell customCell8;
			VisualDiagnostics.RegisterSourceInfo(customCell8 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 462, 26);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 466, 36);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 466, 30);
			CustomCell customCell9;
			VisualDiagnostics.RegisterSourceInfo(customCell9 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 26);
			Section section19;
			VisualDiagnostics.RegisterSourceInfo(section19 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 22);
			Translate translate60;
			VisualDiagnostics.RegisterSourceInfo(translate60 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 33);
			StaticResourceExtension staticResourceExtension33;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension33 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 473, 43);
			StaticResourceExtension staticResourceExtension34;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension34 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 475, 37);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 474, 34);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 481, 37);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 478, 34);
			MultiBinding multiBinding2;
			VisualDiagnostics.RegisterSourceInfo(multiBinding2 = new MultiBinding(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 473, 30);
			StaticResourceExtension staticResourceExtension35;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension35 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 36);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 36);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 30);
			CustomCell customCell10;
			VisualDiagnostics.RegisterSourceInfo(customCell10 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 26);
			Section section20;
			VisualDiagnostics.RegisterSourceInfo(section20 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 22);
			Translate translate61;
			VisualDiagnostics.RegisterSourceInfo(translate61 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 25);
			StaticResourceExtension staticResourceExtension36;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension36 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 25);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 25);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 506, 37);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 506, 82);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(double), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 508, 46);
			double num4 = 0.1;
			double num5 = 0.2;
			double num6 = 0.3;
			double num7 = 0.4;
			double num8 = 0.5;
			double num9 = 0.6;
			double num10 = 0.7;
			double num11 = 0.8;
			double num12 = 0.9;
			double num13 = 1.0;
			double num14 = 1.1;
			double num15 = 1.2;
			double num16 = 1.3;
			double num17 = 1.4;
			double num18 = 1.5;
			double num19 = 1.6;
			double num20 = 1.7;
			double num21 = 1.8;
			double num22 = 1.9;
			double num23 = 2.0;
			double num24 = 2.1;
			double num25 = 2.2;
			double num26 = 2.3;
			double num27 = 2.4;
			double num28 = 2.5;
			double num29 = 2.6;
			double num30 = 2.7;
			double num31 = 2.8;
			double num32 = 2.9;
			double num33 = 3.0;
			double num34 = 3.1;
			double num35 = 3.2;
			double num36 = 3.3;
			double num37 = 3.4;
			double num38 = 3.5;
			double num39 = 3.6;
			double num40 = 3.7;
			double num41 = 3.8;
			double num42 = 3.9;
			double num43 = 4.0;
			double num44 = 4.1;
			double num45 = 4.2;
			double num46 = 4.3;
			double num47 = 4.4;
			double num48 = 4.5;
			double num49 = 4.6;
			double num50 = 4.7;
			double num51 = 4.8;
			double num52 = 4.9;
			double num53 = 5.0;
			double num54 = 5.1;
			double num55 = 5.2;
			double num56 = 5.3;
			double num57 = 5.4;
			double num58 = 5.5;
			double num59 = 5.6;
			double num60 = 5.7;
			double num61 = 5.8;
			double num62 = 5.9;
			double num63 = 6.0;
			double num64 = 6.1;
			double num65 = 6.2;
			double num66 = 6.3;
			double num67 = 6.4;
			double num68 = 6.5;
			double num69 = 6.6;
			double num70 = 6.7;
			double num71 = 6.8;
			double num72 = 6.9;
			double num73 = 7.0;
			double num74 = 7.1;
			double num75 = 7.2;
			double num76 = 7.3;
			double num77 = 7.4;
			double num78 = 7.5;
			double num79 = 7.6;
			double num80 = 7.7;
			double num81 = 7.8;
			double num82 = 7.9;
			double num83 = 8.0;
			double num84 = 8.1;
			double num85 = 8.2;
			double num86 = 8.3;
			double num87 = 8.4;
			double num88 = 8.5;
			double num89 = 8.6;
			double num90 = 8.7;
			double num91 = 8.8;
			double num92 = 8.9;
			double num93 = 9.0;
			double num94 = 9.1;
			double num95 = 9.2;
			double num96 = 9.3;
			double num97 = 9.4;
			double num98 = 9.5;
			double num99 = 9.6;
			double num100 = 9.7;
			double num101 = 9.8;
			double num102 = 9.9;
			double num103 = 10.0;
			double num104 = 10.1;
			double num105 = 10.2;
			double num106 = 10.3;
			double num107 = 10.4;
			double num108 = 10.5;
			double num109 = 10.6;
			double num110 = 10.7;
			double num111 = 10.8;
			double num112 = 10.9;
			double num113 = 11.0;
			double num114 = 11.1;
			double num115 = 11.2;
			double num116 = 11.3;
			double num117 = 11.4;
			double num118 = 11.5;
			double num119 = 11.6;
			double num120 = 11.7;
			double num121 = 11.8;
			double num122 = 11.9;
			double num123 = 12.0;
			double num124 = 12.1;
			double num125 = 12.2;
			double num126 = 12.3;
			double num127 = 12.4;
			double num128 = 12.5;
			double num129 = 12.6;
			double num130 = 12.7;
			double num131 = 12.8;
			double num132 = 12.9;
			ArrayExtension arrayExtension;
			(arrayExtension = new ArrayExtension()).Type = typeFromHandle;
			arrayExtension.Items.Add(num4);
			arrayExtension.Items.Add(num5);
			arrayExtension.Items.Add(num6);
			arrayExtension.Items.Add(num7);
			arrayExtension.Items.Add(num8);
			arrayExtension.Items.Add(num9);
			arrayExtension.Items.Add(num10);
			arrayExtension.Items.Add(num11);
			arrayExtension.Items.Add(num12);
			arrayExtension.Items.Add(num13);
			arrayExtension.Items.Add(num14);
			arrayExtension.Items.Add(num15);
			arrayExtension.Items.Add(num16);
			arrayExtension.Items.Add(num17);
			arrayExtension.Items.Add(num18);
			arrayExtension.Items.Add(num19);
			arrayExtension.Items.Add(num20);
			arrayExtension.Items.Add(num21);
			arrayExtension.Items.Add(num22);
			arrayExtension.Items.Add(num23);
			arrayExtension.Items.Add(num24);
			arrayExtension.Items.Add(num25);
			arrayExtension.Items.Add(num26);
			arrayExtension.Items.Add(num27);
			arrayExtension.Items.Add(num28);
			arrayExtension.Items.Add(num29);
			arrayExtension.Items.Add(num30);
			arrayExtension.Items.Add(num31);
			arrayExtension.Items.Add(num32);
			arrayExtension.Items.Add(num33);
			arrayExtension.Items.Add(num34);
			arrayExtension.Items.Add(num35);
			arrayExtension.Items.Add(num36);
			arrayExtension.Items.Add(num37);
			arrayExtension.Items.Add(num38);
			arrayExtension.Items.Add(num39);
			arrayExtension.Items.Add(num40);
			arrayExtension.Items.Add(num41);
			arrayExtension.Items.Add(num42);
			arrayExtension.Items.Add(num43);
			arrayExtension.Items.Add(num44);
			arrayExtension.Items.Add(num45);
			arrayExtension.Items.Add(num46);
			arrayExtension.Items.Add(num47);
			arrayExtension.Items.Add(num48);
			arrayExtension.Items.Add(num49);
			arrayExtension.Items.Add(num50);
			arrayExtension.Items.Add(num51);
			arrayExtension.Items.Add(num52);
			arrayExtension.Items.Add(num53);
			arrayExtension.Items.Add(num54);
			arrayExtension.Items.Add(num55);
			arrayExtension.Items.Add(num56);
			arrayExtension.Items.Add(num57);
			arrayExtension.Items.Add(num58);
			arrayExtension.Items.Add(num59);
			arrayExtension.Items.Add(num60);
			arrayExtension.Items.Add(num61);
			arrayExtension.Items.Add(num62);
			arrayExtension.Items.Add(num63);
			arrayExtension.Items.Add(num64);
			arrayExtension.Items.Add(num65);
			arrayExtension.Items.Add(num66);
			arrayExtension.Items.Add(num67);
			arrayExtension.Items.Add(num68);
			arrayExtension.Items.Add(num69);
			arrayExtension.Items.Add(num70);
			arrayExtension.Items.Add(num71);
			arrayExtension.Items.Add(num72);
			arrayExtension.Items.Add(num73);
			arrayExtension.Items.Add(num74);
			arrayExtension.Items.Add(num75);
			arrayExtension.Items.Add(num76);
			arrayExtension.Items.Add(num77);
			arrayExtension.Items.Add(num78);
			arrayExtension.Items.Add(num79);
			arrayExtension.Items.Add(num80);
			arrayExtension.Items.Add(num81);
			arrayExtension.Items.Add(num82);
			arrayExtension.Items.Add(num83);
			arrayExtension.Items.Add(num84);
			arrayExtension.Items.Add(num85);
			arrayExtension.Items.Add(num86);
			arrayExtension.Items.Add(num87);
			arrayExtension.Items.Add(num88);
			arrayExtension.Items.Add(num89);
			arrayExtension.Items.Add(num90);
			arrayExtension.Items.Add(num91);
			arrayExtension.Items.Add(num92);
			arrayExtension.Items.Add(num93);
			arrayExtension.Items.Add(num94);
			arrayExtension.Items.Add(num95);
			arrayExtension.Items.Add(num96);
			arrayExtension.Items.Add(num97);
			arrayExtension.Items.Add(num98);
			arrayExtension.Items.Add(num99);
			arrayExtension.Items.Add(num100);
			arrayExtension.Items.Add(num101);
			arrayExtension.Items.Add(num102);
			arrayExtension.Items.Add(num103);
			arrayExtension.Items.Add(num104);
			arrayExtension.Items.Add(num105);
			arrayExtension.Items.Add(num106);
			arrayExtension.Items.Add(num107);
			arrayExtension.Items.Add(num108);
			arrayExtension.Items.Add(num109);
			arrayExtension.Items.Add(num110);
			arrayExtension.Items.Add(num111);
			arrayExtension.Items.Add(num112);
			arrayExtension.Items.Add(num113);
			arrayExtension.Items.Add(num114);
			arrayExtension.Items.Add(num115);
			arrayExtension.Items.Add(num116);
			arrayExtension.Items.Add(num117);
			arrayExtension.Items.Add(num118);
			arrayExtension.Items.Add(num119);
			arrayExtension.Items.Add(num120);
			arrayExtension.Items.Add(num121);
			arrayExtension.Items.Add(num122);
			arrayExtension.Items.Add(num123);
			arrayExtension.Items.Add(num124);
			arrayExtension.Items.Add(num125);
			arrayExtension.Items.Add(num126);
			arrayExtension.Items.Add(num127);
			arrayExtension.Items.Add(num128);
			arrayExtension.Items.Add(num129);
			arrayExtension.Items.Add(num130);
			arrayExtension.Items.Add(num131);
			arrayExtension.Items.Add(num132);
			double[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new double[]
			{
				num4, num5, num6, num7, num8, num9, num10, num11, num12, num13,
				num14, num15, num16, num17, num18, num19, num20, num21, num22, num23,
				num24, num25, num26, num27, num28, num29, num30, num31, num32, num33,
				num34, num35, num36, num37, num38, num39, num40, num41, num42, num43,
				num44, num45, num46, num47, num48, num49, num50, num51, num52, num53,
				num54, num55, num56, num57, num58, num59, num60, num61, num62, num63,
				num64, num65, num66, num67, num68, num69, num70, num71, num72, num73,
				num74, num75, num76, num77, num78, num79, num80, num81, num82, num83,
				num84, num85, num86, num87, num88, num89, num90, num91, num92, num93,
				num94, num95, num96, num97, num98, num99, num100, num101, num102, num103,
				num104, num105, num106, num107, num108, num109, num110, num111, num112, num113,
				num114, num115, num116, num117, num118, num119, num120, num121, num122, num123,
				num124, num125, num126, num127, num128, num129, num130, num131, num132
			}, new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 508, 38);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 506, 30);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 505, 26);
			Translate translate62;
			VisualDiagnostics.RegisterSourceInfo(translate62 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 645, 29);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 648, 29);
			NumberPickerCell numberPickerCell;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell = new NumberPickerCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 644, 26);
			Translate translate63;
			VisualDiagnostics.RegisterSourceInfo(translate63 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 649, 57);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 649, 107);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 649, 26);
			Section section21;
			VisualDiagnostics.RegisterSourceInfo(section21 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 22);
			SettingsView settingsView5;
			VisualDiagnostics.RegisterSourceInfo(settingsView5 = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 18);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 668, 21);
			Translate translate64;
			VisualDiagnostics.RegisterSourceInfo(translate64 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 670, 21);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 665, 18);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 14);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 690, 33);
			StaticResourceExtension staticResourceExtension37;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension37 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 692, 36);
			Translate translate65;
			VisualDiagnostics.RegisterSourceInfo(translate65 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 692, 78);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 692, 30);
			CustomCell customCell11;
			VisualDiagnostics.RegisterSourceInfo(customCell11 = new CustomCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 691, 26);
			Translate translate66;
			VisualDiagnostics.RegisterSourceInfo(translate66 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 696, 29);
			StaticResourceExtension staticResourceExtension38;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension38 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 697, 29);
			RadioCell radioCell38;
			VisualDiagnostics.RegisterSourceInfo(radioCell38 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 694, 26);
			Translate translate67;
			VisualDiagnostics.RegisterSourceInfo(translate67 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 700, 29);
			StaticResourceExtension staticResourceExtension39;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension39 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 701, 29);
			RadioCell radioCell39;
			VisualDiagnostics.RegisterSourceInfo(radioCell39 = new RadioCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 698, 26);
			Translate translate68;
			VisualDiagnostics.RegisterSourceInfo(translate68 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 703, 29);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 705, 29);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 702, 26);
			Section section22;
			VisualDiagnostics.RegisterSourceInfo(section22 = new Section(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 690, 22);
			SettingsView settingsView6;
			VisualDiagnostics.RegisterSourceInfo(settingsView6 = new SettingsView(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 684, 18);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 721, 21);
			Translate translate69;
			VisualDiagnostics.RegisterSourceInfo(translate69 = new Translate(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 723, 21);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 719, 18);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 679, 14);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\WelcomePage1V4.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			nameScope.RegisterName("lbFormattedTitle", nonScalableLabel);
			if (nonScalableLabel.StyleId == null)
			{
				nonScalableLabel.StyleId = "lbFormattedTitle";
			}
			nameScope.RegisterName("lbPageTitle", span);
			if (span.StyleId == null)
			{
				span.StyleId = "lbPageTitle";
			}
			nameScope.RegisterName("lbPageTitleSeparator", span2);
			if (span2.StyleId == null)
			{
				span2.StyleId = "lbPageTitleSeparator";
			}
			nameScope.RegisterName("lbPageSubtitle", span3);
			if (span3.StyleId == null)
			{
				span3.StyleId = "lbPageSubtitle";
			}
			nameScope.RegisterName("progressBar", progressBar);
			if (progressBar.StyleId == null)
			{
				progressBar.StyleId = "progressBar";
			}
			nameScope.RegisterName("gridEULA", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridEULA";
			}
			nameScope.RegisterName("setEULA", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "setEULA";
			}
			nameScope.RegisterName("lbEULA", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbEULA";
			}
			nameScope.RegisterName("eulaActivityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "eulaActivityFrame";
			}
			nameScope.RegisterName("gridAdapter", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridAdapter";
			}
			nameScope.RegisterName("setAdapter", settingsView2);
			if (settingsView2.StyleId == null)
			{
				settingsView2.StyleId = "setAdapter";
			}
			nameScope.RegisterName("gridInterface", grid4);
			if (grid4.StyleId == null)
			{
				grid4.StyleId = "gridInterface";
			}
			nameScope.RegisterName("setInterface", settingsView3);
			if (settingsView3.StyleId == null)
			{
				settingsView3.StyleId = "setInterface";
			}
			nameScope.RegisterName("gridDashPreview", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "gridDashPreview";
			}
			nameScope.RegisterName("gridUnits", grid5);
			if (grid5.StyleId == null)
			{
				grid5.StyleId = "gridUnits";
			}
			nameScope.RegisterName("setUnits", settingsView4);
			if (settingsView4.StyleId == null)
			{
				settingsView4.StyleId = "setUnits";
			}
			nameScope.RegisterName("gridProfileSelector", grid6);
			if (grid6.StyleId == null)
			{
				grid6.StyleId = "gridProfileSelector";
			}
			nameScope.RegisterName("profileSelector", profileSelectorV);
			if (profileSelectorV.StyleId == null)
			{
				profileSelectorV.StyleId = "profileSelector";
			}
			nameScope.RegisterName("gridFuel", grid7);
			if (grid7.StyleId == null)
			{
				grid7.StyleId = "gridFuel";
			}
			nameScope.RegisterName("setFuel", settingsView5);
			if (settingsView5.StyleId == null)
			{
				settingsView5.StyleId = "setFuel";
			}
			nameScope.RegisterName("panelEngineVolume", section21);
			if (section21.StyleId == null)
			{
				section21.StyleId = "panelEngineVolume";
			}
			nameScope.RegisterName("btnPrivacyNext", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnPrivacyNext";
			}
			nameScope.RegisterName("gridPrivacy", grid8);
			if (grid8.StyleId == null)
			{
				grid8.StyleId = "gridPrivacy";
			}
			nameScope.RegisterName("setPrivacy", settingsView6);
			if (settingsView6.StyleId == null)
			{
				settingsView6.StyleId = "setPrivacy";
			}
			nameScope.RegisterName("panelAdsSelection1", radioCell38);
			if (radioCell38.StyleId == null)
			{
				radioCell38.StyleId = "panelAdsSelection1";
			}
			nameScope.RegisterName("panelAdsSelection2", radioCell39);
			if (radioCell39.StyleId == null)
			{
				radioCell39.StyleId = "panelAdsSelection2";
			}
			this.page = this;
			this.lbFormattedTitle = nonScalableLabel;
			this.lbPageTitle = span;
			this.lbPageTitleSeparator = span2;
			this.lbPageSubtitle = span3;
			this.progressBar = progressBar;
			this.gridEULA = grid;
			this.setEULA = settingsView;
			this.lbEULA = label;
			this.eulaActivityFrame = activityFrame;
			this.gridAdapter = grid2;
			this.setAdapter = settingsView2;
			this.gridInterface = grid4;
			this.setInterface = settingsView3;
			this.gridDashPreview = grid3;
			this.gridUnits = grid5;
			this.setUnits = settingsView4;
			this.gridProfileSelector = grid6;
			this.profileSelector = profileSelectorV;
			this.gridFuel = grid7;
			this.setFuel = settingsView5;
			this.panelEngineVolume = section21;
			this.btnPrivacyNext = button6;
			this.gridPrivacy = grid8;
			this.setPrivacy = settingsView6;
			this.panelAdsSelection1 = radioCell38;
			this.panelAdsSelection2 = radioCell39;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("CustomFuelToTrueConverter", customFuelToTrueConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("MultiBooleanToTrueConverter", multiBooleanToTrueConverter);
			resourceDictionary.Add("DecimalFuelPriceForLitreToStringConverter", decimalFuelPriceForLitreToStringConverter);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes2);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes3);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes4);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes5);
			IMarkupExtension<IValueConverter> markupExtension = enumToBoolConverter;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = resourceDictionary;
			array2[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, null, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("EngineDisplacementToVisibleTrueConverter", valueConverter);
			enumToBoolConverter2.TrueValues.Add(fuelTypes);
			enumToBoolConverter2.TrueValues.Add(fuelTypes2);
			enumToBoolConverter2.TrueValues.Add(fuelTypes3);
			enumToBoolConverter2.TrueValues.Add(fuelTypes4);
			enumToBoolConverter2.TrueValues.Add(fuelTypes5);
			enumToBoolConverter2.TrueValues.Add(fuelTypes6);
			enumToBoolConverter2.TrueValues.Add(fuelTypes7);
			enumToBoolConverter2.TrueValues.Add(fuelTypes8);
			IMarkupExtension<IValueConverter> markupExtension2 = enumToBoolConverter2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 2];
			array3[0] = resourceDictionary;
			array3[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle4, obj2 = new SimpleValueTargetProvider(array3, null, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 14)));
			IValueConverter valueConverter2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			resourceDictionary.Add("CombustionEngineFuelTypeToTrueConverter", valueConverter2);
			setter.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension2.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = setter;
			array4[1] = style;
			array4[2] = resourceDictionary;
			array4[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle6, obj3 = new SimpleValueTargetProvider(array4, typeof(Setter).GetRuntimeProperty("Value"), nameScope2));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 52)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			setter.Value = dynamicResource;
			style.Setters.Add(setter);
			setter2.Property = Button.TextColorProperty;
			setter2.Value = "White";
			setter2.Value = Color.White;
			style.Setters.Add(setter2);
			setter3.Property = Button.FontSizeProperty;
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = setter3;
			array5[1] = style;
			array5[2] = resourceDictionary;
			array5[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle8, obj4 = new SimpleValueTargetProvider(array5, typeof(Setter).GetRuntimeProperty("Value"), nameScope4));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 45)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			setter3.Value = obj5;
			style.Setters.Add(setter3);
			setter4.Property = Button.FontAttributesProperty;
			setter4.Value = "Bold";
			setter4.Value = new FontAttributesConverter().ConvertFromInvariantString("Bold");
			style.Setters.Add(setter4);
			resourceDictionary.Add("NextButtonStyle", style);
			translate.Text = "welcome_CarScanner";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 1];
			array6[0] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle10, obj6 = new SimpleValueTargetProvider(array6, Page.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			this.Title = obj7;
			this.SetValue(Application.EnableAccessibilityScalingForNamedFontSizesProperty, false);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 1];
			array7[0] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle12, obj8 = new SimpleValueTargetProvider(array7, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 5)));
			DynamicResource dynamicResource2 = markupExtension6.ProvideValue(xamlServiceProvider6);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SizeChanged += this.WelcomePage1V3_SizeChanged;
			this.Resources = resourceDictionary;
			nonScalableLabel.SetValue(Label.LineBreakModeProperty, 1);
			dynamicResourceExtension3.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 2];
			array8[0] = nonScalableLabel;
			array8[1] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle14, obj9 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 13)));
			DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
			nonScalableLabel.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = nonScalableLabel;
			array9[3] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle16, obj10 = new SimpleValueTargetProvider(array9, Span.FontSizeProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 25)));
			DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.SetDynamicResource(Span.FontSizeProperty, dynamicResource4.Key);
			bindingExtension.Path = "Title";
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension9 = referenceExtension;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = bindingExtension;
			array10[1] = span;
			array10[2] = formattedString;
			array10[3] = nonScalableLabel;
			array10[4] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle18, obj11 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 25)));
			object obj12 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension.Source = obj12;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, "");
			formattedString.Spans.Add(span2);
			span3.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension5.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = span3;
			array11[1] = formattedString;
			array11[2] = nonScalableLabel;
			array11[3] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle20, obj13 = new SimpleValueTargetProvider(array11, Span.FontSizeProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 25)));
			DynamicResource dynamicResource5 = markupExtension10.ProvideValue(xamlServiceProvider10);
			span3.SetDynamicResource(Span.FontSizeProperty, dynamicResource5.Key);
			span3.SetValue(Span.TextProperty, "");
			formattedString.Spans.Add(span3);
			nonScalableLabel.SetValue(Label.FormattedTextProperty, formattedString);
			this.SetValue(NavigationPage.TitleViewProperty, nonScalableLabel);
			grid9.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto, *"));
			grid9.SetValue(Grid.RowSpacingProperty, 0.0);
			progressBar.SetValue(Grid.RowProperty, 0);
			progressBar.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			progressBar.SetValue(ProgressBar.ProgressProperty, 0.1);
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = progressBar;
			array12[1] = grid9;
			array12[2] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle22, obj14 = new SimpleValueTargetProvider(array12, ProgressBar.ProgressColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 17)));
			DynamicResource dynamicResource6 = markupExtension11.ProvideValue(xamlServiceProvider11);
			progressBar.SetDynamicResource(ProgressBar.ProgressColorProperty, dynamicResource6.Key);
			grid9.Children.Add(progressBar);
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto, Auto"));
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			settingsView.SetValue(SettingsView.SeparatorColorProperty, Color.Transparent);
			settingsView.SetValue(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand);
			staticResourceExtension2.Key = "BaseFontSize+";
			IMarkupExtension markupExtension12 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = label;
			array13[1] = section;
			array13[2] = settingsView;
			array13[3] = grid;
			array13[4] = grid9;
			array13[5] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle24, obj15 = new SimpleValueTargetProvider(array13, Label.FontSizeProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 33)));
			object obj16 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label.FontSize = (double)obj16;
			translate2.Text = "ios_EULA";
			IMarkupExtension markupExtension13 = translate2;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = label;
			array14[1] = section;
			array14[2] = settingsView;
			array14[3] = grid;
			array14[4] = grid9;
			array14[5] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle26, obj17 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 33)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label.Text = obj18;
			section.SetValue(Section.FooterViewProperty, label);
			dynamicResourceExtension7.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = image;
			array15[1] = customCell;
			array15[2] = section;
			array15[3] = settingsView;
			array15[4] = grid;
			array15[5] = grid9;
			array15[6] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle28, obj19 = new SimpleValueTargetProvider(array15, Image.SourceProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 36)));
			DynamicResource dynamicResource7 = markupExtension14.ProvideValue(xamlServiceProvider14);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource7.Key);
			customCell.SetValue(CustomCell.ContentProperty, image);
			section.Add(customCell);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			button.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension8.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = button;
			array16[1] = grid;
			array16[2] = grid9;
			array16[3] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle30, obj20 = new SimpleValueTargetProvider(array16, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 21)));
			DynamicResource dynamicResource8 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			button.Clicked += this.btnAgree_Clicked;
			translate3.Text = "ios_AGREE";
			IMarkupExtension markupExtension16 = translate3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = button;
			array17[1] = grid;
			array17[2] = grid9;
			array17[3] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle32, obj21 = new SimpleValueTargetProvider(array17, Button.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 21)));
			object obj22 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button.Text = obj22;
			button.SetValue(Button.TextColorProperty, Color.White);
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 2);
			dynamicResourceExtension9.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = button2;
			array18[1] = grid;
			array18[2] = grid9;
			array18[3] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle34, obj23 = new SimpleValueTargetProvider(array18, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 21)));
			DynamicResource dynamicResource9 = markupExtension17.ProvideValue(xamlServiceProvider17);
			button2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource9.Key);
			button2.Clicked += this.btnDisagree_Clicked;
			translate4.Text = "ios_DISAGREE";
			IMarkupExtension markupExtension18 = translate4;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = button2;
			array19[1] = grid;
			array19[2] = grid9;
			array19[3] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle36, obj24 = new SimpleValueTargetProvider(array19, Button.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 21)));
			object obj25 = markupExtension18.ProvideValue(xamlServiceProvider18);
			button2.Text = obj25;
			button2.SetValue(Button.TextColorProperty, Color.White);
			button2.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(button2);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			grid9.Children.Add(grid);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView2.SetValue(Grid.RowProperty, 0);
			settingsView2.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView2.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			settingsView2.SetValue(SettingsView.SeparatorColorProperty, Color.Transparent);
			image2.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("elm327.png"));
			customCell2.SetValue(CustomCell.ContentProperty, image2);
			section2.Add(customCell2);
			onPlatformExtension.Default = "False";
			onPlatformExtension.iOS = "True";
			IMarkupExtension markupExtension19 = onPlatformExtension;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = customCell3;
			array20[1] = section2;
			array20[2] = settingsView2;
			array20[3] = grid2;
			array20[4] = grid9;
			array20[5] = this;
			object obj26;
			xamlServiceProvider19.Add(typeFromHandle38, obj26 = new SimpleValueTargetProvider(array20, CellBase.IsVisibleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 40)));
			object obj27 = markupExtension19.ProvideValue(xamlServiceProvider19);
			customCell3.IsVisible = (bool)obj27;
			staticResourceExtension3.Key = "BaseFontSize+";
			IMarkupExtension markupExtension20 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = label2;
			array21[1] = customCell3;
			array21[2] = section2;
			array21[3] = settingsView2;
			array21[4] = grid2;
			array21[5] = grid9;
			array21[6] = this;
			object obj28;
			xamlServiceProvider20.Add(typeFromHandle40, obj28 = new SimpleValueTargetProvider(array21, Label.FontSizeProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 36)));
			object obj29 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label2.FontSize = (double)obj29;
			translate5.Text = "ios_OBDADAPTER";
			IMarkupExtension markupExtension21 = translate5;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = label2;
			array22[1] = customCell3;
			array22[2] = section2;
			array22[3] = settingsView2;
			array22[4] = grid2;
			array22[5] = grid9;
			array22[6] = this;
			object obj30;
			xamlServiceProvider21.Add(typeFromHandle42, obj30 = new SimpleValueTargetProvider(array22, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 78)));
			object obj31 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label2.Text = obj31;
			customCell3.SetValue(CustomCell.ContentProperty, label2);
			section2.Add(customCell3);
			onPlatformExtension2.Default = "False";
			onPlatformExtension2.Android = "True";
			IMarkupExtension markupExtension22 = onPlatformExtension2;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = customCell4;
			array23[1] = section2;
			array23[2] = settingsView2;
			array23[3] = grid2;
			array23[4] = grid9;
			array23[5] = this;
			object obj32;
			xamlServiceProvider22.Add(typeFromHandle44, obj32 = new SimpleValueTargetProvider(array23, CellBase.IsVisibleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 40)));
			object obj33 = markupExtension22.ProvideValue(xamlServiceProvider22);
			customCell4.IsVisible = (bool)obj33;
			staticResourceExtension4.Key = "BaseFontSize+";
			IMarkupExtension markupExtension23 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = label3;
			array24[1] = customCell4;
			array24[2] = section2;
			array24[3] = settingsView2;
			array24[4] = grid2;
			array24[5] = grid9;
			array24[6] = this;
			object obj34;
			xamlServiceProvider23.Add(typeFromHandle46, obj34 = new SimpleValueTargetProvider(array24, Label.FontSizeProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 36)));
			object obj35 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label3.FontSize = (double)obj35;
			translate6.Text = "droid_OBDADAPTER";
			IMarkupExtension markupExtension24 = translate6;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 7];
			array25[0] = label3;
			array25[1] = customCell4;
			array25[2] = section2;
			array25[3] = settingsView2;
			array25[4] = grid2;
			array25[5] = grid9;
			array25[6] = this;
			object obj36;
			xamlServiceProvider24.Add(typeFromHandle48, obj36 = new SimpleValueTargetProvider(array25, Label.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 78)));
			object obj37 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label3.Text = obj37;
			customCell4.SetValue(CustomCell.ContentProperty, label3);
			section2.Add(customCell4);
			translate7.Text = "ios_ReadMoreAtCarscannerInfo";
			IMarkupExtension markupExtension25 = translate7;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = buttonCell;
			array26[1] = section2;
			array26[2] = settingsView2;
			array26[3] = grid2;
			array26[4] = grid9;
			array26[5] = this;
			object obj38;
			xamlServiceProvider25.Add(typeFromHandle50, obj38 = new SimpleValueTargetProvider(array26, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(201, 29)));
			object obj39 = markupExtension25.ProvideValue(xamlServiceProvider25);
			buttonCell.Title = obj39;
			buttonCell.Tapped += this.cellChoosingAdapter_Tapped;
			dynamicResourceExtension10.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = buttonCell;
			array27[1] = section2;
			array27[2] = settingsView2;
			array27[3] = grid2;
			array27[4] = grid9;
			array27[5] = this;
			object obj40;
			xamlServiceProvider26.Add(typeFromHandle52, obj40 = new SimpleValueTargetProvider(array27, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 29)));
			DynamicResource dynamicResource10 = markupExtension26.ProvideValue(xamlServiceProvider26);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource10.Key);
			section2.Add(buttonCell);
			settingsView2.Root.Add(section2);
			grid2.Children.Add(settingsView2);
			button3.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension11.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = button3;
			array28[1] = grid2;
			array28[2] = grid9;
			array28[3] = this;
			object obj41;
			xamlServiceProvider27.Add(typeFromHandle54, obj41 = new SimpleValueTargetProvider(array28, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			DynamicResource dynamicResource11 = markupExtension27.ProvideValue(xamlServiceProvider27);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource11.Key);
			button3.Clicked += this.btnAdapterNext_Clicked;
			translate8.Text = "ios_NEXT";
			IMarkupExtension markupExtension28 = translate8;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = button3;
			array29[1] = grid2;
			array29[2] = grid9;
			array29[3] = this;
			object obj42;
			xamlServiceProvider28.Add(typeFromHandle56, obj42 = new SimpleValueTargetProvider(array29, Button.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(215, 21)));
			object obj43 = markupExtension28.ProvideValue(xamlServiceProvider28);
			button3.Text = obj43;
			button3.SetValue(Button.TextColorProperty, Color.White);
			grid2.Children.Add(button3);
			grid9.Children.Add(grid2);
			grid4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid4.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView3.SetValue(Grid.RowProperty, 0);
			settingsView3.SetValue(TableView.HasUnevenRowsProperty, true);
			translate9.Text = "ios_InterfaceTheme";
			IMarkupExtension markupExtension29 = translate9;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = section3;
			array30[1] = settingsView3;
			array30[2] = grid4;
			array30[3] = grid9;
			array30[4] = this;
			object obj44;
			xamlServiceProvider29.Add(typeFromHandle58, obj44 = new SimpleValueTargetProvider(array30, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(235, 33)));
			object obj45 = markupExtension29.ProvideValue(xamlServiceProvider29);
			section3.Title = obj45;
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "DarkMode";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DarkMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DarkMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DarkMode")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section3.SetBinding(RadioCell.SelectedValueProperty, bindingBase2);
			translate10.Text = "ios_AutomaticallySwitchTheme";
			IMarkupExtension markupExtension30 = translate10;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = settingsCheckBoxCellPatched;
			array31[1] = section3;
			array31[2] = settingsView3;
			array31[3] = grid4;
			array31[4] = grid9;
			array31[5] = this;
			object obj46;
			xamlServiceProvider30.Add(typeFromHandle60, obj46 = new SimpleValueTargetProvider(array31, CellBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(237, 29)));
			object obj47 = markupExtension30.ProvideValue(xamlServiceProvider30);
			settingsCheckBoxCellPatched.Title = obj47;
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "AutomaticallySwitchTheme";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AutomaticallySwitchTheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchTheme")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "AutomaticallySwitchThemeAvailable";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchThemeAvailable, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchThemeAvailable")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CellBase.IsVisibleProperty, bindingBase4);
			section3.Add(settingsCheckBoxCellPatched);
			translate11.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension31 = translate11;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = radioCell;
			array32[1] = section3;
			array32[2] = settingsView3;
			array32[3] = grid4;
			array32[4] = grid9;
			array32[5] = this;
			object obj48;
			xamlServiceProvider31.Add(typeFromHandle62, obj48 = new SimpleValueTargetProvider(array32, CellBase.TitleProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(241, 29)));
			object obj49 = markupExtension31.ProvideValue(xamlServiceProvider31);
			radioCell.Title = obj49;
			bindingExtension5.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension32 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle64 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 7];
			array33[0] = bindingExtension5;
			array33[1] = radioCell;
			array33[2] = section3;
			array33[3] = settingsView3;
			array33[4] = grid4;
			array33[5] = grid9;
			array33[6] = this;
			object obj50;
			xamlServiceProvider32.Add(typeFromHandle64, obj50 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle65 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle65, new XamlTypeResolver(xmlNamespaceResolver32, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 29)));
			object obj51 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension5.Converter = obj51;
			bindingExtension5.Path = "AutomaticallySwitchTheme";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchTheme")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			radioCell.SetBinding(CellBase.IsVisibleProperty, bindingBase5);
			staticResourceExtension6.Key = "FalseValue";
			IMarkupExtension markupExtension33 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle66 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 6];
			array34[0] = radioCell;
			array34[1] = section3;
			array34[2] = settingsView3;
			array34[3] = grid4;
			array34[4] = grid9;
			array34[5] = this;
			object obj52;
			xamlServiceProvider33.Add(typeFromHandle66, obj52 = new SimpleValueTargetProvider(array34, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle67 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle67, new XamlTypeResolver(xmlNamespaceResolver33, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(243, 29)));
			object obj53 = markupExtension33.ProvideValue(xamlServiceProvider33);
			radioCell.SetValue(RadioCell.ValueProperty, obj53);
			section3.Add(radioCell);
			translate12.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension34 = translate12;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle68 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 6];
			array35[0] = radioCell2;
			array35[1] = section3;
			array35[2] = settingsView3;
			array35[3] = grid4;
			array35[4] = grid9;
			array35[5] = this;
			object obj54;
			xamlServiceProvider34.Add(typeFromHandle68, obj54 = new SimpleValueTargetProvider(array35, CellBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle69 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle69, new XamlTypeResolver(xmlNamespaceResolver34, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 29)));
			object obj55 = markupExtension34.ProvideValue(xamlServiceProvider34);
			radioCell2.Title = obj55;
			bindingExtension6.Mode = 2;
			staticResourceExtension7.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension35 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle70 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 7];
			array36[0] = bindingExtension6;
			array36[1] = radioCell2;
			array36[2] = section3;
			array36[3] = settingsView3;
			array36[4] = grid4;
			array36[5] = grid9;
			array36[6] = this;
			object obj56;
			xamlServiceProvider35.Add(typeFromHandle70, obj56 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle71 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle71, new XamlTypeResolver(xmlNamespaceResolver35, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(246, 29)));
			object obj57 = markupExtension35.ProvideValue(xamlServiceProvider35);
			bindingExtension6.Converter = obj57;
			bindingExtension6.Path = "AutomaticallySwitchTheme";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchTheme")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			radioCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase6);
			staticResourceExtension8.Key = "TrueValue";
			IMarkupExtension markupExtension36 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle72 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 6];
			array37[0] = radioCell2;
			array37[1] = section3;
			array37[2] = settingsView3;
			array37[3] = grid4;
			array37[4] = grid9;
			array37[5] = this;
			object obj58;
			xamlServiceProvider36.Add(typeFromHandle72, obj58 = new SimpleValueTargetProvider(array37, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle73 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle73, new XamlTypeResolver(xmlNamespaceResolver36, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(247, 29)));
			object obj59 = markupExtension36.ProvideValue(xamlServiceProvider36);
			radioCell2.SetValue(RadioCell.ValueProperty, obj59);
			section3.Add(radioCell2);
			translate13.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension37 = translate13;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle74 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 6];
			array38[0] = settingsCheckBoxCellPatched2;
			array38[1] = section3;
			array38[2] = settingsView3;
			array38[3] = grid4;
			array38[4] = grid9;
			array38[5] = this;
			object obj60;
			xamlServiceProvider37.Add(typeFromHandle74, obj60 = new SimpleValueTargetProvider(array38, CellBase.TitleProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle75 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle75, new XamlTypeResolver(xmlNamespaceResolver37, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 57)));
			object obj61 = markupExtension37.ProvideValue(xamlServiceProvider37);
			settingsCheckBoxCellPatched2.Title = obj61;
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "AndroidRecolorNavBar";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidRecolorNavBar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidRecolorNavBar")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase7);
			onPlatform.Android = true;
			onPlatform.iOS = false;
			settingsCheckBoxCellPatched2.SetValue(CellBase.IsVisibleProperty, onPlatform);
			section3.Add(settingsCheckBoxCellPatched2);
			settingsView3.Root.Add(section3);
			translate14.Text = "ios_DashboardChooseTheme";
			IMarkupExtension markupExtension38 = translate14;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle76 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 5];
			array39[0] = section4;
			array39[1] = settingsView3;
			array39[2] = grid4;
			array39[3] = grid9;
			array39[4] = this;
			object obj62;
			xamlServiceProvider38.Add(typeFromHandle76, obj62 = new SimpleValueTargetProvider(array39, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle77 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle77, new XamlTypeResolver(xmlNamespaceResolver38, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 33)));
			object obj63 = markupExtension38.ProvideValue(xamlServiceProvider38);
			section4.Title = obj63;
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "DashboardTheme";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DashboardTheme, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.DashboardTheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DashboardTheme")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			section4.SetBinding(RadioCell.SelectedValueProperty, bindingBase8);
			frame.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			frame.SetValue(Frame.BorderColorProperty, Color.Transparent);
			frame.SetValue(VisualElement.HeightRequestProperty, 150.0);
			frame.SetValue(VisualElement.WidthRequestProperty, 150.0);
			grid3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			frame.SetValue(ContentView.ContentProperty, grid3);
			customCell5.SetValue(CustomCell.ContentProperty, frame);
			section4.Add(customCell5);
			translate15.Text = "ios_DashboardThemeCarScanner";
			IMarkupExtension markupExtension39 = translate15;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle78 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 6];
			array40[0] = radioCell3;
			array40[1] = section4;
			array40[2] = settingsView3;
			array40[3] = grid4;
			array40[4] = grid9;
			array40[5] = this;
			object obj64;
			xamlServiceProvider39.Add(typeFromHandle78, obj64 = new SimpleValueTargetProvider(array40, CellBase.TitleProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle79 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle79, new XamlTypeResolver(xmlNamespaceResolver39, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 39)));
			object obj65 = markupExtension39.ProvideValue(xamlServiceProvider39);
			radioCell3.Title = obj65;
			radioCell3.Tapped += this.DashboardThemeItem_Tapped;
			radioCell3.SetValue(RadioCell.ValueProperty, num);
			section4.Add(radioCell3);
			translate16.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension40 = translate16;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle80 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 6];
			array41[0] = radioCell4;
			array41[1] = section4;
			array41[2] = settingsView3;
			array41[3] = grid4;
			array41[4] = grid9;
			array41[5] = this;
			object obj66;
			xamlServiceProvider40.Add(typeFromHandle80, obj66 = new SimpleValueTargetProvider(array41, CellBase.TitleProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle81 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle81, new XamlTypeResolver(xmlNamespaceResolver40, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(273, 39)));
			object obj67 = markupExtension40.ProvideValue(xamlServiceProvider40);
			radioCell4.Title = obj67;
			radioCell4.Tapped += this.DashboardThemeItem_Tapped;
			radioCell4.SetValue(RadioCell.ValueProperty, num2);
			section4.Add(radioCell4);
			translate17.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension41 = translate17;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle82 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 6];
			array42[0] = radioCell5;
			array42[1] = section4;
			array42[2] = settingsView3;
			array42[3] = grid4;
			array42[4] = grid9;
			array42[5] = this;
			object obj68;
			xamlServiceProvider41.Add(typeFromHandle82, obj68 = new SimpleValueTargetProvider(array42, CellBase.TitleProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle83 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle83, new XamlTypeResolver(xmlNamespaceResolver41, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(278, 39)));
			object obj69 = markupExtension41.ProvideValue(xamlServiceProvider41);
			radioCell5.Title = obj69;
			radioCell5.Tapped += this.DashboardThemeItem_Tapped;
			radioCell5.SetValue(RadioCell.ValueProperty, num3);
			section4.Add(radioCell5);
			settingsView3.Root.Add(section4);
			grid4.Children.Add(settingsView3);
			button4.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension12.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension42 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle84 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = button4;
			array43[1] = grid4;
			array43[2] = grid9;
			array43[3] = this;
			object obj70;
			xamlServiceProvider42.Add(typeFromHandle84, obj70 = new SimpleValueTargetProvider(array43, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle85 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver42.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle85, new XamlTypeResolver(xmlNamespaceResolver42, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(294, 21)));
			DynamicResource dynamicResource12 = markupExtension42.ProvideValue(xamlServiceProvider42);
			button4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource12.Key);
			button4.Clicked += this.btnInterfaceNext_Clicked;
			translate18.Text = "ios_NEXT";
			IMarkupExtension markupExtension43 = translate18;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle86 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = button4;
			array44[1] = grid4;
			array44[2] = grid9;
			array44[3] = this;
			object obj71;
			xamlServiceProvider43.Add(typeFromHandle86, obj71 = new SimpleValueTargetProvider(array44, Button.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle87 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver43.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle87, new XamlTypeResolver(xmlNamespaceResolver43, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(296, 21)));
			object obj72 = markupExtension43.ProvideValue(xamlServiceProvider43);
			button4.Text = obj72;
			button4.SetValue(Button.TextColorProperty, Color.White);
			grid4.Children.Add(button4);
			grid9.Children.Add(grid4);
			grid5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid5.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView4.SetValue(Grid.RowProperty, 0);
			settingsView4.SetValue(TableView.HasUnevenRowsProperty, true);
			translate19.Text = "Settings_Control_tbSpeedDistanceTemperatureVolume.Text";
			IMarkupExtension markupExtension44 = translate19;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle88 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 5];
			array45[0] = section5;
			array45[1] = settingsView4;
			array45[2] = grid5;
			array45[3] = grid9;
			array45[4] = this;
			object obj73;
			xamlServiceProvider44.Add(typeFromHandle88, obj73 = new SimpleValueTargetProvider(array45, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle89 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver44.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle89, new XamlTypeResolver(xmlNamespaceResolver44, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(313, 33)));
			object obj74 = markupExtension44.ProvideValue(xamlServiceProvider44);
			section5.Title = obj74;
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "Use_km";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_km, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_km = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_km")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			section5.SetBinding(RadioCell.SelectedValueProperty, bindingBase9);
			translate20.Text = "Settings_Control_ToggleSpeedAndDistance.OnContent";
			IMarkupExtension markupExtension45 = translate20;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle90 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 6];
			array46[0] = radioCell6;
			array46[1] = section5;
			array46[2] = settingsView4;
			array46[3] = grid5;
			array46[4] = grid9;
			array46[5] = this;
			object obj75;
			xamlServiceProvider45.Add(typeFromHandle90, obj75 = new SimpleValueTargetProvider(array46, CellBase.TitleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle91 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver45.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle91, new XamlTypeResolver(xmlNamespaceResolver45, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 39)));
			object obj76 = markupExtension45.ProvideValue(xamlServiceProvider45);
			radioCell6.Title = obj76;
			staticResourceExtension9.Key = "TrueValue";
			IMarkupExtension markupExtension46 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle92 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 6];
			array47[0] = radioCell6;
			array47[1] = section5;
			array47[2] = settingsView4;
			array47[3] = grid5;
			array47[4] = grid9;
			array47[5] = this;
			object obj77;
			xamlServiceProvider46.Add(typeFromHandle92, obj77 = new SimpleValueTargetProvider(array47, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle93 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver46.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle93, new XamlTypeResolver(xmlNamespaceResolver46, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 120)));
			object obj78 = markupExtension46.ProvideValue(xamlServiceProvider46);
			radioCell6.SetValue(RadioCell.ValueProperty, obj78);
			section5.Add(radioCell6);
			translate21.Text = "Settings_Control_ToggleSpeedAndDistance.OffContent";
			IMarkupExtension markupExtension47 = translate21;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle94 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 6];
			array48[0] = radioCell7;
			array48[1] = section5;
			array48[2] = settingsView4;
			array48[3] = grid5;
			array48[4] = grid9;
			array48[5] = this;
			object obj79;
			xamlServiceProvider47.Add(typeFromHandle94, obj79 = new SimpleValueTargetProvider(array48, CellBase.TitleProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle95 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver47.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle95, new XamlTypeResolver(xmlNamespaceResolver47, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(315, 39)));
			object obj80 = markupExtension47.ProvideValue(xamlServiceProvider47);
			radioCell7.Title = obj80;
			staticResourceExtension10.Key = "FalseValue";
			IMarkupExtension markupExtension48 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle96 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 6];
			array49[0] = radioCell7;
			array49[1] = section5;
			array49[2] = settingsView4;
			array49[3] = grid5;
			array49[4] = grid9;
			array49[5] = this;
			object obj81;
			xamlServiceProvider48.Add(typeFromHandle96, obj81 = new SimpleValueTargetProvider(array49, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle97 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver48.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle97, new XamlTypeResolver(xmlNamespaceResolver48, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(315, 121)));
			object obj82 = markupExtension48.ProvideValue(xamlServiceProvider48);
			radioCell7.SetValue(RadioCell.ValueProperty, obj82);
			section5.Add(radioCell7);
			settingsView4.Root.Add(section5);
			translate22.Text = "ios_UnitsForFuel";
			IMarkupExtension markupExtension49 = translate22;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle98 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 5];
			array50[0] = section6;
			array50[1] = settingsView4;
			array50[2] = grid5;
			array50[3] = grid9;
			array50[4] = this;
			object obj83;
			xamlServiceProvider49.Add(typeFromHandle98, obj83 = new SimpleValueTargetProvider(array50, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle99 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver49.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle99, new XamlTypeResolver(xmlNamespaceResolver49, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(318, 33)));
			object obj84 = markupExtension49.ProvideValue(xamlServiceProvider49);
			section6.Title = obj84;
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "FuelConsumptionUnit";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, FuelConsumptionUnits>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
				}
				return default(ValueTuple<FuelConsumptionUnits, bool>);
			}, delegate(SharedSettings A_0, FuelConsumptionUnits A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelConsumptionUnit = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelConsumptionUnit")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			section6.SetBinding(RadioCell.SelectedValueProperty, bindingBase10);
			radioCell8.SetValue(CellBase.TitleProperty, "l/100km");
			radioCell8.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits);
			section6.Add(radioCell8);
			radioCell9.SetValue(CellBase.TitleProperty, "km/L");
			radioCell9.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits2);
			section6.Add(radioCell9);
			radioCell10.SetValue(CellBase.TitleProperty, "MPG");
			radioCell10.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits3);
			section6.Add(radioCell10);
			settingsView4.Root.Add(section6);
			translate23.Text = "ios_VolumeUnits";
			IMarkupExtension markupExtension50 = translate23;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle100 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 5];
			array51[0] = section7;
			array51[1] = settingsView4;
			array51[2] = grid5;
			array51[3] = grid9;
			array51[4] = this;
			object obj85;
			xamlServiceProvider50.Add(typeFromHandle100, obj85 = new SimpleValueTargetProvider(array51, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle101 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver50.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver50.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider50.Add(typeFromHandle101, new XamlTypeResolver(xmlNamespaceResolver50, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(324, 33)));
			object obj86 = markupExtension50.ProvideValue(xamlServiceProvider50);
			section7.Title = obj86;
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "UseLitersForVolume";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseLitersForVolume = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section7.SetBinding(RadioCell.SelectedValueProperty, bindingBase11);
			translate24.Text = "ios_Liters";
			IMarkupExtension markupExtension51 = translate24;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle102 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 6];
			array52[0] = radioCell11;
			array52[1] = section7;
			array52[2] = settingsView4;
			array52[3] = grid5;
			array52[4] = grid9;
			array52[5] = this;
			object obj87;
			xamlServiceProvider51.Add(typeFromHandle102, obj87 = new SimpleValueTargetProvider(array52, CellBase.TitleProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle103 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver51.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver51.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider51.Add(typeFromHandle103, new XamlTypeResolver(xmlNamespaceResolver51, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(325, 39)));
			object obj88 = markupExtension51.ProvideValue(xamlServiceProvider51);
			radioCell11.Title = obj88;
			staticResourceExtension11.Key = "TrueValue";
			IMarkupExtension markupExtension52 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle104 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 6];
			array53[0] = radioCell11;
			array53[1] = section7;
			array53[2] = settingsView4;
			array53[3] = grid5;
			array53[4] = grid9;
			array53[5] = this;
			object obj89;
			xamlServiceProvider52.Add(typeFromHandle104, obj89 = new SimpleValueTargetProvider(array53, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle105 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver52.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver52.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver52.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider52.Add(typeFromHandle105, new XamlTypeResolver(xmlNamespaceResolver52, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(325, 81)));
			object obj90 = markupExtension52.ProvideValue(xamlServiceProvider52);
			radioCell11.SetValue(RadioCell.ValueProperty, obj90);
			section7.Add(radioCell11);
			translate25.Text = "ios_Gallons";
			IMarkupExtension markupExtension53 = translate25;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle106 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 6];
			array54[0] = radioCell12;
			array54[1] = section7;
			array54[2] = settingsView4;
			array54[3] = grid5;
			array54[4] = grid9;
			array54[5] = this;
			object obj91;
			xamlServiceProvider53.Add(typeFromHandle106, obj91 = new SimpleValueTargetProvider(array54, CellBase.TitleProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle107 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver53.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver53.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver53.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider53.Add(typeFromHandle107, new XamlTypeResolver(xmlNamespaceResolver53, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(326, 39)));
			object obj92 = markupExtension53.ProvideValue(xamlServiceProvider53);
			radioCell12.Title = obj92;
			staticResourceExtension12.Key = "FalseValue";
			IMarkupExtension markupExtension54 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle108 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 6];
			array55[0] = radioCell12;
			array55[1] = section7;
			array55[2] = settingsView4;
			array55[3] = grid5;
			array55[4] = grid9;
			array55[5] = this;
			object obj93;
			xamlServiceProvider54.Add(typeFromHandle108, obj93 = new SimpleValueTargetProvider(array55, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj93);
			Type typeFromHandle109 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver54.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver54.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver54.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider54.Add(typeFromHandle109, new XamlTypeResolver(xmlNamespaceResolver54, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(326, 82)));
			object obj94 = markupExtension54.ProvideValue(xamlServiceProvider54);
			radioCell12.SetValue(RadioCell.ValueProperty, obj94);
			section7.Add(radioCell12);
			settingsView4.Root.Add(section7);
			translate26.Text = "Settings_Control_ToggleUSGallon.Header";
			IMarkupExtension markupExtension55 = translate26;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle110 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 5];
			array56[0] = section8;
			array56[1] = settingsView4;
			array56[2] = grid5;
			array56[3] = grid9;
			array56[4] = this;
			object obj95;
			xamlServiceProvider55.Add(typeFromHandle110, obj95 = new SimpleValueTargetProvider(array56, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj95);
			Type typeFromHandle111 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver55.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver55.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver55.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider55.Add(typeFromHandle111, new XamlTypeResolver(xmlNamespaceResolver55, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(331, 25)));
			object obj96 = markupExtension55.ProvideValue(xamlServiceProvider55);
			section8.Title = obj96;
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "UseUSGallon";
			bindingExtension12.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseUSGallon = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseUSGallon")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			section8.SetBinding(RadioCell.SelectedValueProperty, bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "ShowUSGallonSelector";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowUSGallonSelector")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			section8.SetBinding(Section.IsVisibleProperty, bindingBase13);
			translate27.Text = "Settings_Control_ToggleUSGallon.OnContent";
			IMarkupExtension markupExtension56 = translate27;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle112 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 6];
			array57[0] = radioCell13;
			array57[1] = section8;
			array57[2] = settingsView4;
			array57[3] = grid5;
			array57[4] = grid9;
			array57[5] = this;
			object obj97;
			xamlServiceProvider56.Add(typeFromHandle112, obj97 = new SimpleValueTargetProvider(array57, CellBase.TitleProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj97);
			Type typeFromHandle113 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver56.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver56.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver56.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider56.Add(typeFromHandle113, new XamlTypeResolver(xmlNamespaceResolver56, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(334, 39)));
			object obj98 = markupExtension56.ProvideValue(xamlServiceProvider56);
			radioCell13.Title = obj98;
			staticResourceExtension13.Key = "TrueValue";
			IMarkupExtension markupExtension57 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle114 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 6];
			array58[0] = radioCell13;
			array58[1] = section8;
			array58[2] = settingsView4;
			array58[3] = grid5;
			array58[4] = grid9;
			array58[5] = this;
			object obj99;
			xamlServiceProvider57.Add(typeFromHandle114, obj99 = new SimpleValueTargetProvider(array58, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj99);
			Type typeFromHandle115 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver57.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver57.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver57.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider57.Add(typeFromHandle115, new XamlTypeResolver(xmlNamespaceResolver57, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(334, 112)));
			object obj100 = markupExtension57.ProvideValue(xamlServiceProvider57);
			radioCell13.SetValue(RadioCell.ValueProperty, obj100);
			section8.Add(radioCell13);
			translate28.Text = "Settings_Control_ToggleUSGallon.OffContent";
			IMarkupExtension markupExtension58 = translate28;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle116 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 6];
			array59[0] = radioCell14;
			array59[1] = section8;
			array59[2] = settingsView4;
			array59[3] = grid5;
			array59[4] = grid9;
			array59[5] = this;
			object obj101;
			xamlServiceProvider58.Add(typeFromHandle116, obj101 = new SimpleValueTargetProvider(array59, CellBase.TitleProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj101);
			Type typeFromHandle117 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver58.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver58.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver58.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider58.Add(typeFromHandle117, new XamlTypeResolver(xmlNamespaceResolver58, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(335, 39)));
			object obj102 = markupExtension58.ProvideValue(xamlServiceProvider58);
			radioCell14.Title = obj102;
			staticResourceExtension14.Key = "FalseValue";
			IMarkupExtension markupExtension59 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle118 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 6];
			array60[0] = radioCell14;
			array60[1] = section8;
			array60[2] = settingsView4;
			array60[3] = grid5;
			array60[4] = grid9;
			array60[5] = this;
			object obj103;
			xamlServiceProvider59.Add(typeFromHandle118, obj103 = new SimpleValueTargetProvider(array60, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj103);
			Type typeFromHandle119 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver59.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver59.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver59.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider59.Add(typeFromHandle119, new XamlTypeResolver(xmlNamespaceResolver59, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(335, 113)));
			object obj104 = markupExtension59.ProvideValue(xamlServiceProvider59);
			radioCell14.SetValue(RadioCell.ValueProperty, obj104);
			section8.Add(radioCell14);
			settingsView4.Root.Add(section8);
			translate29.Text = "Settings_Control_TogglePressureUnits.Header";
			IMarkupExtension markupExtension60 = translate29;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle120 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 5];
			array61[0] = section9;
			array61[1] = settingsView4;
			array61[2] = grid5;
			array61[3] = grid9;
			array61[4] = this;
			object obj105;
			xamlServiceProvider60.Add(typeFromHandle120, obj105 = new SimpleValueTargetProvider(array61, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj105);
			Type typeFromHandle121 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver60.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver60.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver60.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider60.Add(typeFromHandle121, new XamlTypeResolver(xmlNamespaceResolver60, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(338, 33)));
			object obj106 = markupExtension60.ProvideValue(xamlServiceProvider60);
			section9.Title = obj106;
			bindingExtension14.Path = "Pressure_use_kpa";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Pressure_use_kpa = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Pressure_use_kpa")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			section9.SetBinding(RadioCell.SelectedValueProperty, bindingBase14);
			translate30.Text = "Settings_Control_TogglePressureUnits.OnContent";
			IMarkupExtension markupExtension61 = translate30;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle122 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 6];
			array62[0] = radioCell15;
			array62[1] = section9;
			array62[2] = settingsView4;
			array62[3] = grid5;
			array62[4] = grid9;
			array62[5] = this;
			object obj107;
			xamlServiceProvider61.Add(typeFromHandle122, obj107 = new SimpleValueTargetProvider(array62, CellBase.TitleProperty, nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj107);
			Type typeFromHandle123 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver61.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver61.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver61.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider61.Add(typeFromHandle123, new XamlTypeResolver(xmlNamespaceResolver61, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(339, 39)));
			object obj108 = markupExtension61.ProvideValue(xamlServiceProvider61);
			radioCell15.Title = obj108;
			staticResourceExtension15.Key = "TrueValue";
			IMarkupExtension markupExtension62 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle124 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 6];
			array63[0] = radioCell15;
			array63[1] = section9;
			array63[2] = settingsView4;
			array63[3] = grid5;
			array63[4] = grid9;
			array63[5] = this;
			object obj109;
			xamlServiceProvider62.Add(typeFromHandle124, obj109 = new SimpleValueTargetProvider(array63, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj109);
			Type typeFromHandle125 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver62.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver62.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver62.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider62.Add(typeFromHandle125, new XamlTypeResolver(xmlNamespaceResolver62, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(339, 117)));
			object obj110 = markupExtension62.ProvideValue(xamlServiceProvider62);
			radioCell15.SetValue(RadioCell.ValueProperty, obj110);
			section9.Add(radioCell15);
			translate31.Text = "Settings_Control_TogglePressureUnits.OffContent";
			IMarkupExtension markupExtension63 = translate31;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle126 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 6];
			array64[0] = radioCell16;
			array64[1] = section9;
			array64[2] = settingsView4;
			array64[3] = grid5;
			array64[4] = grid9;
			array64[5] = this;
			object obj111;
			xamlServiceProvider63.Add(typeFromHandle126, obj111 = new SimpleValueTargetProvider(array64, CellBase.TitleProperty, nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj111);
			Type typeFromHandle127 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver63.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver63.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver63.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider63.Add(typeFromHandle127, new XamlTypeResolver(xmlNamespaceResolver63, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(340, 39)));
			object obj112 = markupExtension63.ProvideValue(xamlServiceProvider63);
			radioCell16.Title = obj112;
			staticResourceExtension16.Key = "FalseValue";
			IMarkupExtension markupExtension64 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle128 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 6];
			array65[0] = radioCell16;
			array65[1] = section9;
			array65[2] = settingsView4;
			array65[3] = grid5;
			array65[4] = grid9;
			array65[5] = this;
			object obj113;
			xamlServiceProvider64.Add(typeFromHandle128, obj113 = new SimpleValueTargetProvider(array65, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj113);
			Type typeFromHandle129 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver64.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver64.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver64.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider64.Add(typeFromHandle129, new XamlTypeResolver(xmlNamespaceResolver64, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(340, 118)));
			object obj114 = markupExtension64.ProvideValue(xamlServiceProvider64);
			radioCell16.SetValue(RadioCell.ValueProperty, obj114);
			section9.Add(radioCell16);
			settingsView4.Root.Add(section9);
			translate32.Text = "Settings_Control_ToggleFlowUnits.Header";
			IMarkupExtension markupExtension65 = translate32;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle130 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 5];
			array66[0] = section10;
			array66[1] = settingsView4;
			array66[2] = grid5;
			array66[3] = grid9;
			array66[4] = this;
			object obj115;
			xamlServiceProvider65.Add(typeFromHandle130, obj115 = new SimpleValueTargetProvider(array66, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj115);
			Type typeFromHandle131 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver65.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver65.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver65.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider65.Add(typeFromHandle131, new XamlTypeResolver(xmlNamespaceResolver65, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(343, 33)));
			object obj116 = markupExtension65.ProvideValue(xamlServiceProvider65);
			section10.Title = obj116;
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "Flow_use_grams_sec";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Flow_use_grams_sec = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Flow_use_grams_sec")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			section10.SetBinding(RadioCell.SelectedValueProperty, bindingBase15);
			translate33.Text = "Settings_Control_ToggleFlowUnits.OnContent";
			IMarkupExtension markupExtension66 = translate33;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle132 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 6];
			array67[0] = radioCell17;
			array67[1] = section10;
			array67[2] = settingsView4;
			array67[3] = grid5;
			array67[4] = grid9;
			array67[5] = this;
			object obj117;
			xamlServiceProvider66.Add(typeFromHandle132, obj117 = new SimpleValueTargetProvider(array67, CellBase.TitleProperty, nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj117);
			Type typeFromHandle133 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver66.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver66.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver66.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider66.Add(typeFromHandle133, new XamlTypeResolver(xmlNamespaceResolver66, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(344, 39)));
			object obj118 = markupExtension66.ProvideValue(xamlServiceProvider66);
			radioCell17.Title = obj118;
			staticResourceExtension17.Key = "TrueValue";
			IMarkupExtension markupExtension67 = staticResourceExtension17;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle134 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 6];
			array68[0] = radioCell17;
			array68[1] = section10;
			array68[2] = settingsView4;
			array68[3] = grid5;
			array68[4] = grid9;
			array68[5] = this;
			object obj119;
			xamlServiceProvider67.Add(typeFromHandle134, obj119 = new SimpleValueTargetProvider(array68, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj119);
			Type typeFromHandle135 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver67.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver67.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver67.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider67.Add(typeFromHandle135, new XamlTypeResolver(xmlNamespaceResolver67, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(344, 113)));
			object obj120 = markupExtension67.ProvideValue(xamlServiceProvider67);
			radioCell17.SetValue(RadioCell.ValueProperty, obj120);
			section10.Add(radioCell17);
			translate34.Text = "Settings_Control_ToggleFlowUnits.OffContent";
			IMarkupExtension markupExtension68 = translate34;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle136 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 6];
			array69[0] = radioCell18;
			array69[1] = section10;
			array69[2] = settingsView4;
			array69[3] = grid5;
			array69[4] = grid9;
			array69[5] = this;
			object obj121;
			xamlServiceProvider68.Add(typeFromHandle136, obj121 = new SimpleValueTargetProvider(array69, CellBase.TitleProperty, nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj121);
			Type typeFromHandle137 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver68.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver68.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver68.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider68.Add(typeFromHandle137, new XamlTypeResolver(xmlNamespaceResolver68, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(345, 39)));
			object obj122 = markupExtension68.ProvideValue(xamlServiceProvider68);
			radioCell18.Title = obj122;
			staticResourceExtension18.Key = "FalseValue";
			IMarkupExtension markupExtension69 = staticResourceExtension18;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle138 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 6];
			array70[0] = radioCell18;
			array70[1] = section10;
			array70[2] = settingsView4;
			array70[3] = grid5;
			array70[4] = grid9;
			array70[5] = this;
			object obj123;
			xamlServiceProvider69.Add(typeFromHandle138, obj123 = new SimpleValueTargetProvider(array70, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj123);
			Type typeFromHandle139 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver69.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver69.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver69.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider69.Add(typeFromHandle139, new XamlTypeResolver(xmlNamespaceResolver69, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(345, 114)));
			object obj124 = markupExtension69.ProvideValue(xamlServiceProvider69);
			radioCell18.SetValue(RadioCell.ValueProperty, obj124);
			section10.Add(radioCell18);
			settingsView4.Root.Add(section10);
			translate35.Text = "Settings_Control_ToggleTemperatureUnits.Header";
			IMarkupExtension markupExtension70 = translate35;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle140 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 5];
			array71[0] = section11;
			array71[1] = settingsView4;
			array71[2] = grid5;
			array71[3] = grid9;
			array71[4] = this;
			object obj125;
			xamlServiceProvider70.Add(typeFromHandle140, obj125 = new SimpleValueTargetProvider(array71, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj125);
			Type typeFromHandle141 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver70.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver70.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver70.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider70.Add(typeFromHandle141, new XamlTypeResolver(xmlNamespaceResolver70, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(348, 33)));
			object obj126 = markupExtension70.ProvideValue(xamlServiceProvider70);
			section11.Title = obj126;
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "Use_celcium";
			bindingExtension16.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_celcium = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_celcium")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			section11.SetBinding(RadioCell.SelectedValueProperty, bindingBase16);
			translate36.Text = "Settings_Control_ToggleTemperatureUnits.OnContent";
			IMarkupExtension markupExtension71 = translate36;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle142 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 6];
			array72[0] = radioCell19;
			array72[1] = section11;
			array72[2] = settingsView4;
			array72[3] = grid5;
			array72[4] = grid9;
			array72[5] = this;
			object obj127;
			xamlServiceProvider71.Add(typeFromHandle142, obj127 = new SimpleValueTargetProvider(array72, CellBase.TitleProperty, nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj127);
			Type typeFromHandle143 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver71.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver71.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver71.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider71.Add(typeFromHandle143, new XamlTypeResolver(xmlNamespaceResolver71, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(349, 39)));
			object obj128 = markupExtension71.ProvideValue(xamlServiceProvider71);
			radioCell19.Title = obj128;
			staticResourceExtension19.Key = "TrueValue";
			IMarkupExtension markupExtension72 = staticResourceExtension19;
			XamlServiceProvider xamlServiceProvider72 = new XamlServiceProvider();
			Type typeFromHandle144 = typeof(IProvideValueTarget);
			object[] array73 = new object[0 + 6];
			array73[0] = radioCell19;
			array73[1] = section11;
			array73[2] = settingsView4;
			array73[3] = grid5;
			array73[4] = grid9;
			array73[5] = this;
			object obj129;
			xamlServiceProvider72.Add(typeFromHandle144, obj129 = new SimpleValueTargetProvider(array73, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider72.Add(typeof(IReferenceProvider), obj129);
			Type typeFromHandle145 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver72 = new XmlNamespaceResolver();
			xmlNamespaceResolver72.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver72.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver72.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver72.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver72.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver72.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver72.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver72.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver72.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver72.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider72.Add(typeFromHandle145, new XamlTypeResolver(xmlNamespaceResolver72, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider72.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(349, 120)));
			object obj130 = markupExtension72.ProvideValue(xamlServiceProvider72);
			radioCell19.SetValue(RadioCell.ValueProperty, obj130);
			section11.Add(radioCell19);
			translate37.Text = "Settings_Control_ToggleTemperatureUnits.OffContent";
			IMarkupExtension markupExtension73 = translate37;
			XamlServiceProvider xamlServiceProvider73 = new XamlServiceProvider();
			Type typeFromHandle146 = typeof(IProvideValueTarget);
			object[] array74 = new object[0 + 6];
			array74[0] = radioCell20;
			array74[1] = section11;
			array74[2] = settingsView4;
			array74[3] = grid5;
			array74[4] = grid9;
			array74[5] = this;
			object obj131;
			xamlServiceProvider73.Add(typeFromHandle146, obj131 = new SimpleValueTargetProvider(array74, CellBase.TitleProperty, nameScope));
			xamlServiceProvider73.Add(typeof(IReferenceProvider), obj131);
			Type typeFromHandle147 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver73 = new XmlNamespaceResolver();
			xmlNamespaceResolver73.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver73.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver73.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver73.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver73.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver73.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver73.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver73.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver73.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver73.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider73.Add(typeFromHandle147, new XamlTypeResolver(xmlNamespaceResolver73, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider73.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(350, 39)));
			object obj132 = markupExtension73.ProvideValue(xamlServiceProvider73);
			radioCell20.Title = obj132;
			staticResourceExtension20.Key = "FalseValue";
			IMarkupExtension markupExtension74 = staticResourceExtension20;
			XamlServiceProvider xamlServiceProvider74 = new XamlServiceProvider();
			Type typeFromHandle148 = typeof(IProvideValueTarget);
			object[] array75 = new object[0 + 6];
			array75[0] = radioCell20;
			array75[1] = section11;
			array75[2] = settingsView4;
			array75[3] = grid5;
			array75[4] = grid9;
			array75[5] = this;
			object obj133;
			xamlServiceProvider74.Add(typeFromHandle148, obj133 = new SimpleValueTargetProvider(array75, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider74.Add(typeof(IReferenceProvider), obj133);
			Type typeFromHandle149 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver74 = new XmlNamespaceResolver();
			xmlNamespaceResolver74.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver74.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver74.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver74.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver74.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver74.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver74.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver74.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver74.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver74.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider74.Add(typeFromHandle149, new XamlTypeResolver(xmlNamespaceResolver74, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider74.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(350, 121)));
			object obj134 = markupExtension74.ProvideValue(xamlServiceProvider74);
			radioCell20.SetValue(RadioCell.ValueProperty, obj134);
			section11.Add(radioCell20);
			settingsView4.Root.Add(section11);
			translate38.Text = "Settings_Control_ToggleAccelerationUnits.Header";
			IMarkupExtension markupExtension75 = translate38;
			XamlServiceProvider xamlServiceProvider75 = new XamlServiceProvider();
			Type typeFromHandle150 = typeof(IProvideValueTarget);
			object[] array76 = new object[0 + 5];
			array76[0] = section12;
			array76[1] = settingsView4;
			array76[2] = grid5;
			array76[3] = grid9;
			array76[4] = this;
			object obj135;
			xamlServiceProvider75.Add(typeFromHandle150, obj135 = new SimpleValueTargetProvider(array76, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider75.Add(typeof(IReferenceProvider), obj135);
			Type typeFromHandle151 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver75 = new XmlNamespaceResolver();
			xmlNamespaceResolver75.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver75.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver75.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver75.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver75.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver75.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver75.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver75.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver75.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver75.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider75.Add(typeFromHandle151, new XamlTypeResolver(xmlNamespaceResolver75, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider75.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(353, 33)));
			object obj136 = markupExtension75.ProvideValue(xamlServiceProvider75);
			section12.Title = obj136;
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "AccelerationUseG";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AccelerationUseG = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationUseG")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			section12.SetBinding(RadioCell.SelectedValueProperty, bindingBase17);
			radioCell21.SetValue(CellBase.TitleProperty, "g");
			staticResourceExtension21.Key = "TrueValue";
			IMarkupExtension markupExtension76 = staticResourceExtension21;
			XamlServiceProvider xamlServiceProvider76 = new XamlServiceProvider();
			Type typeFromHandle152 = typeof(IProvideValueTarget);
			object[] array77 = new object[0 + 6];
			array77[0] = radioCell21;
			array77[1] = section12;
			array77[2] = settingsView4;
			array77[3] = grid5;
			array77[4] = grid9;
			array77[5] = this;
			object obj137;
			xamlServiceProvider76.Add(typeFromHandle152, obj137 = new SimpleValueTargetProvider(array77, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider76.Add(typeof(IReferenceProvider), obj137);
			Type typeFromHandle153 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver76 = new XmlNamespaceResolver();
			xmlNamespaceResolver76.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver76.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver76.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver76.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver76.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver76.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver76.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver76.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver76.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver76.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider76.Add(typeFromHandle153, new XamlTypeResolver(xmlNamespaceResolver76, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider76.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(354, 49)));
			object obj138 = markupExtension76.ProvideValue(xamlServiceProvider76);
			radioCell21.SetValue(RadioCell.ValueProperty, obj138);
			section12.Add(radioCell21);
			radioCell22.SetValue(CellBase.TitleProperty, "m/sec^2");
			staticResourceExtension22.Key = "FalseValue";
			IMarkupExtension markupExtension77 = staticResourceExtension22;
			XamlServiceProvider xamlServiceProvider77 = new XamlServiceProvider();
			Type typeFromHandle154 = typeof(IProvideValueTarget);
			object[] array78 = new object[0 + 6];
			array78[0] = radioCell22;
			array78[1] = section12;
			array78[2] = settingsView4;
			array78[3] = grid5;
			array78[4] = grid9;
			array78[5] = this;
			object obj139;
			xamlServiceProvider77.Add(typeFromHandle154, obj139 = new SimpleValueTargetProvider(array78, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider77.Add(typeof(IReferenceProvider), obj139);
			Type typeFromHandle155 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver77 = new XmlNamespaceResolver();
			xmlNamespaceResolver77.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver77.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver77.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver77.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver77.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver77.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver77.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver77.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver77.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver77.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider77.Add(typeFromHandle155, new XamlTypeResolver(xmlNamespaceResolver77, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider77.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(355, 55)));
			object obj140 = markupExtension77.ProvideValue(xamlServiceProvider77);
			radioCell22.SetValue(RadioCell.ValueProperty, obj140);
			section12.Add(radioCell22);
			settingsView4.Root.Add(section12);
			translate39.Text = "Settings_Control_TogglePowerUnits.Header";
			IMarkupExtension markupExtension78 = translate39;
			XamlServiceProvider xamlServiceProvider78 = new XamlServiceProvider();
			Type typeFromHandle156 = typeof(IProvideValueTarget);
			object[] array79 = new object[0 + 5];
			array79[0] = section13;
			array79[1] = settingsView4;
			array79[2] = grid5;
			array79[3] = grid9;
			array79[4] = this;
			object obj141;
			xamlServiceProvider78.Add(typeFromHandle156, obj141 = new SimpleValueTargetProvider(array79, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider78.Add(typeof(IReferenceProvider), obj141);
			Type typeFromHandle157 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver78 = new XmlNamespaceResolver();
			xmlNamespaceResolver78.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver78.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver78.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver78.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver78.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver78.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver78.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver78.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver78.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver78.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider78.Add(typeFromHandle157, new XamlTypeResolver(xmlNamespaceResolver78, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider78.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(358, 33)));
			object obj142 = markupExtension78.ProvideValue(xamlServiceProvider78);
			section13.Title = obj142;
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "UseHoursePower";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseHoursePower = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseHoursePower")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			section13.SetBinding(RadioCell.SelectedValueProperty, bindingBase18);
			radioCell23.SetValue(CellBase.TitleProperty, "hp");
			staticResourceExtension23.Key = "TrueValue";
			IMarkupExtension markupExtension79 = staticResourceExtension23;
			XamlServiceProvider xamlServiceProvider79 = new XamlServiceProvider();
			Type typeFromHandle158 = typeof(IProvideValueTarget);
			object[] array80 = new object[0 + 6];
			array80[0] = radioCell23;
			array80[1] = section13;
			array80[2] = settingsView4;
			array80[3] = grid5;
			array80[4] = grid9;
			array80[5] = this;
			object obj143;
			xamlServiceProvider79.Add(typeFromHandle158, obj143 = new SimpleValueTargetProvider(array80, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider79.Add(typeof(IReferenceProvider), obj143);
			Type typeFromHandle159 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver79 = new XmlNamespaceResolver();
			xmlNamespaceResolver79.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver79.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver79.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver79.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver79.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver79.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver79.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver79.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver79.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver79.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider79.Add(typeFromHandle159, new XamlTypeResolver(xmlNamespaceResolver79, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider79.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(359, 50)));
			object obj144 = markupExtension79.ProvideValue(xamlServiceProvider79);
			radioCell23.SetValue(RadioCell.ValueProperty, obj144);
			section13.Add(radioCell23);
			radioCell24.SetValue(CellBase.TitleProperty, "kW");
			staticResourceExtension24.Key = "FalseValue";
			IMarkupExtension markupExtension80 = staticResourceExtension24;
			XamlServiceProvider xamlServiceProvider80 = new XamlServiceProvider();
			Type typeFromHandle160 = typeof(IProvideValueTarget);
			object[] array81 = new object[0 + 6];
			array81[0] = radioCell24;
			array81[1] = section13;
			array81[2] = settingsView4;
			array81[3] = grid5;
			array81[4] = grid9;
			array81[5] = this;
			object obj145;
			xamlServiceProvider80.Add(typeFromHandle160, obj145 = new SimpleValueTargetProvider(array81, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider80.Add(typeof(IReferenceProvider), obj145);
			Type typeFromHandle161 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver80 = new XmlNamespaceResolver();
			xmlNamespaceResolver80.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver80.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver80.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver80.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver80.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver80.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver80.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver80.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver80.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver80.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider80.Add(typeFromHandle161, new XamlTypeResolver(xmlNamespaceResolver80, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider80.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(360, 50)));
			object obj146 = markupExtension80.ProvideValue(xamlServiceProvider80);
			radioCell24.SetValue(RadioCell.ValueProperty, obj146);
			section13.Add(radioCell24);
			settingsView4.Root.Add(section13);
			translate40.Text = "Settings_Control_ToggleTorqueUnits.Header";
			IMarkupExtension markupExtension81 = translate40;
			XamlServiceProvider xamlServiceProvider81 = new XamlServiceProvider();
			Type typeFromHandle162 = typeof(IProvideValueTarget);
			object[] array82 = new object[0 + 5];
			array82[0] = section14;
			array82[1] = settingsView4;
			array82[2] = grid5;
			array82[3] = grid9;
			array82[4] = this;
			object obj147;
			xamlServiceProvider81.Add(typeFromHandle162, obj147 = new SimpleValueTargetProvider(array82, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider81.Add(typeof(IReferenceProvider), obj147);
			Type typeFromHandle163 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver81 = new XmlNamespaceResolver();
			xmlNamespaceResolver81.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver81.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver81.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver81.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver81.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver81.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver81.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver81.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver81.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver81.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider81.Add(typeFromHandle163, new XamlTypeResolver(xmlNamespaceResolver81, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider81.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(363, 33)));
			object obj148 = markupExtension81.ProvideValue(xamlServiceProvider81);
			section14.Title = obj148;
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "UseNmForTorque";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseNmForTorque = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseNmForTorque")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			section14.SetBinding(RadioCell.SelectedValueProperty, bindingBase19);
			radioCell25.SetValue(CellBase.TitleProperty, "Nm");
			staticResourceExtension25.Key = "TrueValue";
			IMarkupExtension markupExtension82 = staticResourceExtension25;
			XamlServiceProvider xamlServiceProvider82 = new XamlServiceProvider();
			Type typeFromHandle164 = typeof(IProvideValueTarget);
			object[] array83 = new object[0 + 6];
			array83[0] = radioCell25;
			array83[1] = section14;
			array83[2] = settingsView4;
			array83[3] = grid5;
			array83[4] = grid9;
			array83[5] = this;
			object obj149;
			xamlServiceProvider82.Add(typeFromHandle164, obj149 = new SimpleValueTargetProvider(array83, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider82.Add(typeof(IReferenceProvider), obj149);
			Type typeFromHandle165 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver82 = new XmlNamespaceResolver();
			xmlNamespaceResolver82.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver82.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver82.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver82.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver82.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver82.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver82.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver82.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver82.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver82.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider82.Add(typeFromHandle165, new XamlTypeResolver(xmlNamespaceResolver82, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider82.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(364, 50)));
			object obj150 = markupExtension82.ProvideValue(xamlServiceProvider82);
			radioCell25.SetValue(RadioCell.ValueProperty, obj150);
			section14.Add(radioCell25);
			radioCell26.SetValue(CellBase.TitleProperty, "ft*lbs");
			staticResourceExtension26.Key = "FalseValue";
			IMarkupExtension markupExtension83 = staticResourceExtension26;
			XamlServiceProvider xamlServiceProvider83 = new XamlServiceProvider();
			Type typeFromHandle166 = typeof(IProvideValueTarget);
			object[] array84 = new object[0 + 6];
			array84[0] = radioCell26;
			array84[1] = section14;
			array84[2] = settingsView4;
			array84[3] = grid5;
			array84[4] = grid9;
			array84[5] = this;
			object obj151;
			xamlServiceProvider83.Add(typeFromHandle166, obj151 = new SimpleValueTargetProvider(array84, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider83.Add(typeof(IReferenceProvider), obj151);
			Type typeFromHandle167 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver83 = new XmlNamespaceResolver();
			xmlNamespaceResolver83.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver83.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver83.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver83.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver83.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver83.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver83.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver83.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver83.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver83.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider83.Add(typeFromHandle167, new XamlTypeResolver(xmlNamespaceResolver83, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider83.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(365, 54)));
			object obj152 = markupExtension83.ProvideValue(xamlServiceProvider83);
			radioCell26.SetValue(RadioCell.ValueProperty, obj152);
			section14.Add(radioCell26);
			settingsView4.Root.Add(section14);
			grid5.Children.Add(settingsView4);
			button5.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension13.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension84 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider84 = new XamlServiceProvider();
			Type typeFromHandle168 = typeof(IProvideValueTarget);
			object[] array85 = new object[0 + 4];
			array85[0] = button5;
			array85[1] = grid5;
			array85[2] = grid9;
			array85[3] = this;
			object obj153;
			xamlServiceProvider84.Add(typeFromHandle168, obj153 = new SimpleValueTargetProvider(array85, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider84.Add(typeof(IReferenceProvider), obj153);
			Type typeFromHandle169 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver84 = new XmlNamespaceResolver();
			xmlNamespaceResolver84.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver84.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver84.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver84.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver84.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver84.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver84.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver84.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver84.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver84.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider84.Add(typeFromHandle169, new XamlTypeResolver(xmlNamespaceResolver84, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider84.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(380, 21)));
			DynamicResource dynamicResource13 = markupExtension84.ProvideValue(xamlServiceProvider84);
			button5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource13.Key);
			button5.Clicked += this.btnUnitsNext_Clicked;
			translate41.Text = "ios_NEXT";
			IMarkupExtension markupExtension85 = translate41;
			XamlServiceProvider xamlServiceProvider85 = new XamlServiceProvider();
			Type typeFromHandle170 = typeof(IProvideValueTarget);
			object[] array86 = new object[0 + 4];
			array86[0] = button5;
			array86[1] = grid5;
			array86[2] = grid9;
			array86[3] = this;
			object obj154;
			xamlServiceProvider85.Add(typeFromHandle170, obj154 = new SimpleValueTargetProvider(array86, Button.TextProperty, nameScope));
			xamlServiceProvider85.Add(typeof(IReferenceProvider), obj154);
			Type typeFromHandle171 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver85 = new XmlNamespaceResolver();
			xmlNamespaceResolver85.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver85.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver85.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver85.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver85.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver85.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver85.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver85.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver85.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver85.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider85.Add(typeFromHandle171, new XamlTypeResolver(xmlNamespaceResolver85, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider85.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(382, 21)));
			object obj155 = markupExtension85.ProvideValue(xamlServiceProvider85);
			button5.Text = obj155;
			button5.SetValue(Button.TextColorProperty, Color.White);
			grid5.Children.Add(button5);
			grid9.Children.Add(grid5);
			grid6.SetValue(Grid.RowProperty, 1);
			grid6.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			profileSelectorV.SetValue(Grid.RowProperty, 0);
			profileSelectorV.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			profileSelectorV.SetValue(ProfileSelectorV3.BackButtonVisibleProperty, false);
			profileSelectorV.SetValue(ProfileSelectorV3.CreateBackItemProperty, true);
			translate42.Text = "ios_ChooseCarBrand";
			IMarkupExtension markupExtension86 = translate42;
			XamlServiceProvider xamlServiceProvider86 = new XamlServiceProvider();
			Type typeFromHandle172 = typeof(IProvideValueTarget);
			object[] array87 = new object[0 + 4];
			array87[0] = profileSelectorV;
			array87[1] = grid6;
			array87[2] = grid9;
			array87[3] = this;
			object obj156;
			xamlServiceProvider86.Add(typeFromHandle172, obj156 = new SimpleValueTargetProvider(array87, ProfileSelectorV3.TitleTextProperty, nameScope));
			xamlServiceProvider86.Add(typeof(IReferenceProvider), obj156);
			Type typeFromHandle173 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver86 = new XmlNamespaceResolver();
			xmlNamespaceResolver86.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver86.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver86.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver86.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver86.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver86.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver86.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver86.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver86.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver86.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider86.Add(typeFromHandle173, new XamlTypeResolver(xmlNamespaceResolver86, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider86.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(399, 21)));
			object obj157 = markupExtension86.ProvideValue(xamlServiceProvider86);
			profileSelectorV.TitleText = obj157;
			profileSelectorV.SetValue(ProfileSelectorV3.TitleVisibleProperty, false);
			profileSelectorV.WindowCloseRequested += this.ProfileSelector_NextRequested;
			grid6.Children.Add(profileSelectorV);
			grid9.Children.Add(grid6);
			grid7.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid7.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView5.SetValue(Grid.RowProperty, 0);
			settingsView5.SetValue(TableView.HasUnevenRowsProperty, true);
			translate43.Text = "ios_Image8";
			IMarkupExtension markupExtension87 = translate43;
			XamlServiceProvider xamlServiceProvider87 = new XamlServiceProvider();
			Type typeFromHandle174 = typeof(IProvideValueTarget);
			object[] array88 = new object[0 + 5];
			array88[0] = section15;
			array88[1] = settingsView5;
			array88[2] = grid7;
			array88[3] = grid9;
			array88[4] = this;
			object obj158;
			xamlServiceProvider87.Add(typeFromHandle174, obj158 = new SimpleValueTargetProvider(array88, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider87.Add(typeof(IReferenceProvider), obj158);
			Type typeFromHandle175 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver87 = new XmlNamespaceResolver();
			xmlNamespaceResolver87.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver87.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver87.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver87.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver87.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver87.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver87.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver87.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver87.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver87.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider87.Add(typeFromHandle175, new XamlTypeResolver(xmlNamespaceResolver87, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider87.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(415, 33)));
			object obj159 = markupExtension87.ProvideValue(xamlServiceProvider87);
			section15.Title = obj159;
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "AlwaysRecordFuelConsumption";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AlwaysRecordFuelConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AlwaysRecordFuelConsumption")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			section15.SetBinding(RadioCell.SelectedValueProperty, bindingBase20);
			translate44.Text = "ios_RecordFuelConsumptionWhenSelected";
			IMarkupExtension markupExtension88 = translate44;
			XamlServiceProvider xamlServiceProvider88 = new XamlServiceProvider();
			Type typeFromHandle176 = typeof(IProvideValueTarget);
			object[] array89 = new object[0 + 6];
			array89[0] = radioCell27;
			array89[1] = section15;
			array89[2] = settingsView5;
			array89[3] = grid7;
			array89[4] = grid9;
			array89[5] = this;
			object obj160;
			xamlServiceProvider88.Add(typeFromHandle176, obj160 = new SimpleValueTargetProvider(array89, CellBase.TitleProperty, nameScope));
			xamlServiceProvider88.Add(typeof(IReferenceProvider), obj160);
			Type typeFromHandle177 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver88 = new XmlNamespaceResolver();
			xmlNamespaceResolver88.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver88.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver88.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver88.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver88.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver88.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver88.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver88.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver88.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver88.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider88.Add(typeFromHandle177, new XamlTypeResolver(xmlNamespaceResolver88, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider88.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(416, 39)));
			object obj161 = markupExtension88.ProvideValue(xamlServiceProvider88);
			radioCell27.Title = obj161;
			staticResourceExtension27.Key = "FalseValue";
			IMarkupExtension markupExtension89 = staticResourceExtension27;
			XamlServiceProvider xamlServiceProvider89 = new XamlServiceProvider();
			Type typeFromHandle178 = typeof(IProvideValueTarget);
			object[] array90 = new object[0 + 6];
			array90[0] = radioCell27;
			array90[1] = section15;
			array90[2] = settingsView5;
			array90[3] = grid7;
			array90[4] = grid9;
			array90[5] = this;
			object obj162;
			xamlServiceProvider89.Add(typeFromHandle178, obj162 = new SimpleValueTargetProvider(array90, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider89.Add(typeof(IReferenceProvider), obj162);
			Type typeFromHandle179 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver89 = new XmlNamespaceResolver();
			xmlNamespaceResolver89.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver89.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver89.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver89.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver89.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver89.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver89.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver89.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver89.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver89.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider89.Add(typeFromHandle179, new XamlTypeResolver(xmlNamespaceResolver89, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider89.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(416, 108)));
			object obj163 = markupExtension89.ProvideValue(xamlServiceProvider89);
			radioCell27.SetValue(RadioCell.ValueProperty, obj163);
			section15.Add(radioCell27);
			translate45.Text = "Settings_tbAlwaysRecordFuel.Text";
			IMarkupExtension markupExtension90 = translate45;
			XamlServiceProvider xamlServiceProvider90 = new XamlServiceProvider();
			Type typeFromHandle180 = typeof(IProvideValueTarget);
			object[] array91 = new object[0 + 6];
			array91[0] = radioCell28;
			array91[1] = section15;
			array91[2] = settingsView5;
			array91[3] = grid7;
			array91[4] = grid9;
			array91[5] = this;
			object obj164;
			xamlServiceProvider90.Add(typeFromHandle180, obj164 = new SimpleValueTargetProvider(array91, CellBase.TitleProperty, nameScope));
			xamlServiceProvider90.Add(typeof(IReferenceProvider), obj164);
			Type typeFromHandle181 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver90 = new XmlNamespaceResolver();
			xmlNamespaceResolver90.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver90.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver90.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver90.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver90.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver90.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver90.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver90.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver90.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver90.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider90.Add(typeFromHandle181, new XamlTypeResolver(xmlNamespaceResolver90, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider90.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(417, 39)));
			object obj165 = markupExtension90.ProvideValue(xamlServiceProvider90);
			radioCell28.Title = obj165;
			staticResourceExtension28.Key = "TrueValue";
			IMarkupExtension markupExtension91 = staticResourceExtension28;
			XamlServiceProvider xamlServiceProvider91 = new XamlServiceProvider();
			Type typeFromHandle182 = typeof(IProvideValueTarget);
			object[] array92 = new object[0 + 6];
			array92[0] = radioCell28;
			array92[1] = section15;
			array92[2] = settingsView5;
			array92[3] = grid7;
			array92[4] = grid9;
			array92[5] = this;
			object obj166;
			xamlServiceProvider91.Add(typeFromHandle182, obj166 = new SimpleValueTargetProvider(array92, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider91.Add(typeof(IReferenceProvider), obj166);
			Type typeFromHandle183 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver91 = new XmlNamespaceResolver();
			xmlNamespaceResolver91.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver91.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver91.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver91.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver91.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver91.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver91.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver91.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver91.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver91.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider91.Add(typeFromHandle183, new XamlTypeResolver(xmlNamespaceResolver91, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider91.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(417, 103)));
			object obj167 = markupExtension91.ProvideValue(xamlServiceProvider91);
			radioCell28.SetValue(RadioCell.ValueProperty, obj167);
			section15.Add(radioCell28);
			translate46.Text = "Settings_tbAlwaysRecordFuelWarning.Text";
			IMarkupExtension markupExtension92 = translate46;
			XamlServiceProvider xamlServiceProvider92 = new XamlServiceProvider();
			Type typeFromHandle184 = typeof(IProvideValueTarget);
			object[] array93 = new object[0 + 6];
			array93[0] = labelCell;
			array93[1] = section15;
			array93[2] = settingsView5;
			array93[3] = grid7;
			array93[4] = grid9;
			array93[5] = this;
			object obj168;
			xamlServiceProvider92.Add(typeFromHandle184, obj168 = new SimpleValueTargetProvider(array93, CellBase.TitleProperty, nameScope));
			xamlServiceProvider92.Add(typeof(IReferenceProvider), obj168);
			Type typeFromHandle185 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver92 = new XmlNamespaceResolver();
			xmlNamespaceResolver92.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver92.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver92.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver92.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver92.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver92.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver92.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver92.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver92.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver92.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider92.Add(typeFromHandle185, new XamlTypeResolver(xmlNamespaceResolver92, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider92.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(419, 29)));
			object obj169 = markupExtension92.ProvideValue(xamlServiceProvider92);
			labelCell.Title = obj169;
			bindingExtension21.Path = "AlwaysRecordFuelConsumption";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AlwaysRecordFuelConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AlwaysRecordFuelConsumption")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			labelCell.SetBinding(CellBase.IsVisibleProperty, bindingBase21);
			labelCell.SetValue(CellBase.TitleColorProperty, Color.Red);
			section15.Add(labelCell);
			settingsView5.Root.Add(section15);
			translate47.Text = "Settings_Control_tbVehicleFuel.Text";
			IMarkupExtension markupExtension93 = translate47;
			XamlServiceProvider xamlServiceProvider93 = new XamlServiceProvider();
			Type typeFromHandle186 = typeof(IProvideValueTarget);
			object[] array94 = new object[0 + 5];
			array94[0] = section16;
			array94[1] = settingsView5;
			array94[2] = grid7;
			array94[3] = grid9;
			array94[4] = this;
			object obj170;
			xamlServiceProvider93.Add(typeFromHandle186, obj170 = new SimpleValueTargetProvider(array94, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider93.Add(typeof(IReferenceProvider), obj170);
			Type typeFromHandle187 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver93 = new XmlNamespaceResolver();
			xmlNamespaceResolver93.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver93.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver93.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver93.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver93.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver93.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver93.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver93.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver93.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver93.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider93.Add(typeFromHandle187, new XamlTypeResolver(xmlNamespaceResolver93, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider93.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(424, 33)));
			object obj171 = markupExtension93.ProvideValue(xamlServiceProvider93);
			section16.Title = obj171;
			bindingExtension22.Path = "FuelType";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, delegate(SharedSettings A_0, FuelTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelType = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			section16.SetBinding(RadioCell.SelectedValueProperty, bindingBase22);
			translate48.Text = "Settings_Control_Gasoline.Content";
			IMarkupExtension markupExtension94 = translate48;
			XamlServiceProvider xamlServiceProvider94 = new XamlServiceProvider();
			Type typeFromHandle188 = typeof(IProvideValueTarget);
			object[] array95 = new object[0 + 6];
			array95[0] = radioCell29;
			array95[1] = section16;
			array95[2] = settingsView5;
			array95[3] = grid7;
			array95[4] = grid9;
			array95[5] = this;
			object obj172;
			xamlServiceProvider94.Add(typeFromHandle188, obj172 = new SimpleValueTargetProvider(array95, CellBase.TitleProperty, nameScope));
			xamlServiceProvider94.Add(typeof(IReferenceProvider), obj172);
			Type typeFromHandle189 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver94 = new XmlNamespaceResolver();
			xmlNamespaceResolver94.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver94.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver94.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver94.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver94.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver94.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver94.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver94.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver94.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver94.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider94.Add(typeFromHandle189, new XamlTypeResolver(xmlNamespaceResolver94, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider94.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(425, 39)));
			object obj173 = markupExtension94.ProvideValue(xamlServiceProvider94);
			radioCell29.Title = obj173;
			radioCell29.SetValue(RadioCell.ValueProperty, fuelTypes9);
			section16.Add(radioCell29);
			translate49.Text = "Settings_Control_Diesel.Content";
			IMarkupExtension markupExtension95 = translate49;
			XamlServiceProvider xamlServiceProvider95 = new XamlServiceProvider();
			Type typeFromHandle190 = typeof(IProvideValueTarget);
			object[] array96 = new object[0 + 6];
			array96[0] = radioCell30;
			array96[1] = section16;
			array96[2] = settingsView5;
			array96[3] = grid7;
			array96[4] = grid9;
			array96[5] = this;
			object obj174;
			xamlServiceProvider95.Add(typeFromHandle190, obj174 = new SimpleValueTargetProvider(array96, CellBase.TitleProperty, nameScope));
			xamlServiceProvider95.Add(typeof(IReferenceProvider), obj174);
			Type typeFromHandle191 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver95 = new XmlNamespaceResolver();
			xmlNamespaceResolver95.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver95.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver95.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver95.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver95.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver95.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver95.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver95.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver95.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver95.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider95.Add(typeFromHandle191, new XamlTypeResolver(xmlNamespaceResolver95, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider95.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(426, 39)));
			object obj175 = markupExtension95.ProvideValue(xamlServiceProvider95);
			radioCell30.Title = obj175;
			radioCell30.SetValue(RadioCell.ValueProperty, fuelTypes10);
			section16.Add(radioCell30);
			translate50.Text = "fuelType_EV";
			IMarkupExtension markupExtension96 = translate50;
			XamlServiceProvider xamlServiceProvider96 = new XamlServiceProvider();
			Type typeFromHandle192 = typeof(IProvideValueTarget);
			object[] array97 = new object[0 + 6];
			array97[0] = radioCell31;
			array97[1] = section16;
			array97[2] = settingsView5;
			array97[3] = grid7;
			array97[4] = grid9;
			array97[5] = this;
			object obj176;
			xamlServiceProvider96.Add(typeFromHandle192, obj176 = new SimpleValueTargetProvider(array97, CellBase.TitleProperty, nameScope));
			xamlServiceProvider96.Add(typeof(IReferenceProvider), obj176);
			Type typeFromHandle193 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver96 = new XmlNamespaceResolver();
			xmlNamespaceResolver96.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver96.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver96.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver96.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver96.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver96.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver96.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver96.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver96.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver96.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider96.Add(typeFromHandle193, new XamlTypeResolver(xmlNamespaceResolver96, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider96.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(427, 39)));
			object obj177 = markupExtension96.ProvideValue(xamlServiceProvider96);
			radioCell31.Title = obj177;
			radioCell31.SetValue(RadioCell.ValueProperty, fuelTypes11);
			section16.Add(radioCell31);
			translate51.Text = "ios_Ethanol";
			IMarkupExtension markupExtension97 = translate51;
			XamlServiceProvider xamlServiceProvider97 = new XamlServiceProvider();
			Type typeFromHandle194 = typeof(IProvideValueTarget);
			object[] array98 = new object[0 + 6];
			array98[0] = radioCell32;
			array98[1] = section16;
			array98[2] = settingsView5;
			array98[3] = grid7;
			array98[4] = grid9;
			array98[5] = this;
			object obj178;
			xamlServiceProvider97.Add(typeFromHandle194, obj178 = new SimpleValueTargetProvider(array98, CellBase.TitleProperty, nameScope));
			xamlServiceProvider97.Add(typeof(IReferenceProvider), obj178);
			Type typeFromHandle195 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver97 = new XmlNamespaceResolver();
			xmlNamespaceResolver97.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver97.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver97.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver97.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver97.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver97.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver97.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver97.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver97.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver97.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider97.Add(typeFromHandle195, new XamlTypeResolver(xmlNamespaceResolver97, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider97.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(428, 39)));
			object obj179 = markupExtension97.ProvideValue(xamlServiceProvider97);
			radioCell32.Title = obj179;
			radioCell32.SetValue(RadioCell.ValueProperty, fuelTypes12);
			section16.Add(radioCell32);
			translate52.Text = "ios_Methanol";
			IMarkupExtension markupExtension98 = translate52;
			XamlServiceProvider xamlServiceProvider98 = new XamlServiceProvider();
			Type typeFromHandle196 = typeof(IProvideValueTarget);
			object[] array99 = new object[0 + 6];
			array99[0] = radioCell33;
			array99[1] = section16;
			array99[2] = settingsView5;
			array99[3] = grid7;
			array99[4] = grid9;
			array99[5] = this;
			object obj180;
			xamlServiceProvider98.Add(typeFromHandle196, obj180 = new SimpleValueTargetProvider(array99, CellBase.TitleProperty, nameScope));
			xamlServiceProvider98.Add(typeof(IReferenceProvider), obj180);
			Type typeFromHandle197 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver98 = new XmlNamespaceResolver();
			xmlNamespaceResolver98.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver98.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver98.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver98.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver98.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver98.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver98.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver98.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver98.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver98.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider98.Add(typeFromHandle197, new XamlTypeResolver(xmlNamespaceResolver98, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider98.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(429, 39)));
			object obj181 = markupExtension98.ProvideValue(xamlServiceProvider98);
			radioCell33.Title = obj181;
			radioCell33.SetValue(RadioCell.ValueProperty, fuelTypes13);
			section16.Add(radioCell33);
			translate53.Text = "ios_Propan";
			IMarkupExtension markupExtension99 = translate53;
			XamlServiceProvider xamlServiceProvider99 = new XamlServiceProvider();
			Type typeFromHandle198 = typeof(IProvideValueTarget);
			object[] array100 = new object[0 + 6];
			array100[0] = radioCell34;
			array100[1] = section16;
			array100[2] = settingsView5;
			array100[3] = grid7;
			array100[4] = grid9;
			array100[5] = this;
			object obj182;
			xamlServiceProvider99.Add(typeFromHandle198, obj182 = new SimpleValueTargetProvider(array100, CellBase.TitleProperty, nameScope));
			xamlServiceProvider99.Add(typeof(IReferenceProvider), obj182);
			Type typeFromHandle199 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver99 = new XmlNamespaceResolver();
			xmlNamespaceResolver99.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver99.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver99.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver99.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver99.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver99.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver99.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver99.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver99.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver99.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider99.Add(typeFromHandle199, new XamlTypeResolver(xmlNamespaceResolver99, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider99.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(430, 39)));
			object obj183 = markupExtension99.ProvideValue(xamlServiceProvider99);
			radioCell34.Title = obj183;
			radioCell34.SetValue(RadioCell.ValueProperty, fuelTypes14);
			section16.Add(radioCell34);
			translate54.Text = "ios_Methan";
			IMarkupExtension markupExtension100 = translate54;
			XamlServiceProvider xamlServiceProvider100 = new XamlServiceProvider();
			Type typeFromHandle200 = typeof(IProvideValueTarget);
			object[] array101 = new object[0 + 6];
			array101[0] = radioCell35;
			array101[1] = section16;
			array101[2] = settingsView5;
			array101[3] = grid7;
			array101[4] = grid9;
			array101[5] = this;
			object obj184;
			xamlServiceProvider100.Add(typeFromHandle200, obj184 = new SimpleValueTargetProvider(array101, CellBase.TitleProperty, nameScope));
			xamlServiceProvider100.Add(typeof(IReferenceProvider), obj184);
			Type typeFromHandle201 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver100 = new XmlNamespaceResolver();
			xmlNamespaceResolver100.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver100.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver100.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver100.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver100.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver100.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver100.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver100.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver100.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver100.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider100.Add(typeFromHandle201, new XamlTypeResolver(xmlNamespaceResolver100, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider100.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(431, 39)));
			object obj185 = markupExtension100.ProvideValue(xamlServiceProvider100);
			radioCell35.Title = obj185;
			radioCell35.SetValue(RadioCell.ValueProperty, fuelTypes15);
			section16.Add(radioCell35);
			translate55.Text = "ios_FlexFuelOBDII";
			IMarkupExtension markupExtension101 = translate55;
			XamlServiceProvider xamlServiceProvider101 = new XamlServiceProvider();
			Type typeFromHandle202 = typeof(IProvideValueTarget);
			object[] array102 = new object[0 + 6];
			array102[0] = radioCell36;
			array102[1] = section16;
			array102[2] = settingsView5;
			array102[3] = grid7;
			array102[4] = grid9;
			array102[5] = this;
			object obj186;
			xamlServiceProvider101.Add(typeFromHandle202, obj186 = new SimpleValueTargetProvider(array102, CellBase.TitleProperty, nameScope));
			xamlServiceProvider101.Add(typeof(IReferenceProvider), obj186);
			Type typeFromHandle203 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver101 = new XmlNamespaceResolver();
			xmlNamespaceResolver101.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver101.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver101.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver101.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver101.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver101.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver101.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver101.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver101.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver101.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider101.Add(typeFromHandle203, new XamlTypeResolver(xmlNamespaceResolver101, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider101.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(432, 39)));
			object obj187 = markupExtension101.ProvideValue(xamlServiceProvider101);
			radioCell36.Title = obj187;
			radioCell36.SetValue(RadioCell.ValueProperty, fuelTypes16);
			section16.Add(radioCell36);
			translate56.Text = "ios_Custom";
			IMarkupExtension markupExtension102 = translate56;
			XamlServiceProvider xamlServiceProvider102 = new XamlServiceProvider();
			Type typeFromHandle204 = typeof(IProvideValueTarget);
			object[] array103 = new object[0 + 6];
			array103[0] = radioCell37;
			array103[1] = section16;
			array103[2] = settingsView5;
			array103[3] = grid7;
			array103[4] = grid9;
			array103[5] = this;
			object obj188;
			xamlServiceProvider102.Add(typeFromHandle204, obj188 = new SimpleValueTargetProvider(array103, CellBase.TitleProperty, nameScope));
			xamlServiceProvider102.Add(typeof(IReferenceProvider), obj188);
			Type typeFromHandle205 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver102 = new XmlNamespaceResolver();
			xmlNamespaceResolver102.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver102.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver102.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver102.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver102.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver102.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver102.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver102.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver102.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver102.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider102.Add(typeFromHandle205, new XamlTypeResolver(xmlNamespaceResolver102, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider102.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(433, 39)));
			object obj189 = markupExtension102.ProvideValue(xamlServiceProvider102);
			radioCell37.Title = obj189;
			radioCell37.SetValue(RadioCell.ValueProperty, fuelTypes17);
			section16.Add(radioCell37);
			settingsView5.Root.Add(section16);
			translate57.Text = "ios_CustomFuelAF";
			IMarkupExtension markupExtension103 = translate57;
			XamlServiceProvider xamlServiceProvider103 = new XamlServiceProvider();
			Type typeFromHandle206 = typeof(IProvideValueTarget);
			object[] array104 = new object[0 + 5];
			array104[0] = section17;
			array104[1] = settingsView5;
			array104[2] = grid7;
			array104[3] = grid9;
			array104[4] = this;
			object obj190;
			xamlServiceProvider103.Add(typeFromHandle206, obj190 = new SimpleValueTargetProvider(array104, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider103.Add(typeof(IReferenceProvider), obj190);
			Type typeFromHandle207 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver103 = new XmlNamespaceResolver();
			xmlNamespaceResolver103.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver103.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver103.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver103.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver103.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver103.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver103.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver103.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver103.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver103.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider103.Add(typeFromHandle207, new XamlTypeResolver(xmlNamespaceResolver103, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider103.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(436, 33)));
			object obj191 = markupExtension103.ProvideValue(xamlServiceProvider103);
			section17.Title = obj191;
			bindingExtension23.Mode = 2;
			staticResourceExtension29.Key = "CustomFuelToTrueConverter";
			IMarkupExtension markupExtension104 = staticResourceExtension29;
			XamlServiceProvider xamlServiceProvider104 = new XamlServiceProvider();
			Type typeFromHandle208 = typeof(IProvideValueTarget);
			object[] array105 = new object[0 + 6];
			array105[0] = bindingExtension23;
			array105[1] = section17;
			array105[2] = settingsView5;
			array105[3] = grid7;
			array105[4] = grid9;
			array105[5] = this;
			object obj192;
			xamlServiceProvider104.Add(typeFromHandle208, obj192 = new SimpleValueTargetProvider(array105, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider104.Add(typeof(IReferenceProvider), obj192);
			Type typeFromHandle209 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver104 = new XmlNamespaceResolver();
			xmlNamespaceResolver104.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver104.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver104.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver104.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver104.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver104.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver104.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver104.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver104.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver104.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider104.Add(typeFromHandle209, new XamlTypeResolver(xmlNamespaceResolver104, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider104.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(436, 81)));
			object obj193 = markupExtension104.ProvideValue(xamlServiceProvider104);
			bindingExtension23.Converter = obj193;
			bindingExtension23.Path = "FuelType";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			section17.SetBinding(Section.IsVisibleProperty, bindingBase23);
			customCell6.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "CustomFuelAF";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CustomFuelAF, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomFuelAF = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CustomFuelAF")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase24);
			customCell6.SetValue(CustomCell.ContentProperty, numericEntryV);
			section17.Add(customCell6);
			settingsView5.Root.Add(section17);
			translate58.Text = "ios_CustomFuelDensity";
			IMarkupExtension markupExtension105 = translate58;
			XamlServiceProvider xamlServiceProvider105 = new XamlServiceProvider();
			Type typeFromHandle210 = typeof(IProvideValueTarget);
			object[] array106 = new object[0 + 5];
			array106[0] = section18;
			array106[1] = settingsView5;
			array106[2] = grid7;
			array106[3] = grid9;
			array106[4] = this;
			object obj194;
			xamlServiceProvider105.Add(typeFromHandle210, obj194 = new SimpleValueTargetProvider(array106, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider105.Add(typeof(IReferenceProvider), obj194);
			Type typeFromHandle211 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver105 = new XmlNamespaceResolver();
			xmlNamespaceResolver105.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver105.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver105.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver105.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver105.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver105.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver105.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver105.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver105.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver105.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider105.Add(typeFromHandle211, new XamlTypeResolver(xmlNamespaceResolver105, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider105.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(444, 33)));
			object obj195 = markupExtension105.ProvideValue(xamlServiceProvider105);
			section18.Title = obj195;
			bindingExtension25.Mode = 2;
			staticResourceExtension30.Key = "CustomFuelToTrueConverter";
			IMarkupExtension markupExtension106 = staticResourceExtension30;
			XamlServiceProvider xamlServiceProvider106 = new XamlServiceProvider();
			Type typeFromHandle212 = typeof(IProvideValueTarget);
			object[] array107 = new object[0 + 6];
			array107[0] = bindingExtension25;
			array107[1] = section18;
			array107[2] = settingsView5;
			array107[3] = grid7;
			array107[4] = grid9;
			array107[5] = this;
			object obj196;
			xamlServiceProvider106.Add(typeFromHandle212, obj196 = new SimpleValueTargetProvider(array107, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider106.Add(typeof(IReferenceProvider), obj196);
			Type typeFromHandle213 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver106 = new XmlNamespaceResolver();
			xmlNamespaceResolver106.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver106.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver106.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver106.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver106.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver106.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver106.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver106.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver106.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver106.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider106.Add(typeFromHandle213, new XamlTypeResolver(xmlNamespaceResolver106, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider106.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(444, 86)));
			object obj197 = markupExtension106.ProvideValue(xamlServiceProvider106);
			bindingExtension25.Converter = obj197;
			bindingExtension25.Path = "FuelType";
			bindingExtension25.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			section18.SetBinding(Section.IsVisibleProperty, bindingBase25);
			customCell7.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "CustomFuelDensity";
			bindingExtension26.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CustomFuelDensity, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomFuelDensity = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CustomFuelDensity")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase26);
			customCell7.SetValue(CustomCell.ContentProperty, numericEntryV2);
			section18.Add(customCell7);
			settingsView5.Root.Add(section18);
			translate59.Text = "Settings_tbFuelPriceL.Text";
			IMarkupExtension markupExtension107 = translate59;
			XamlServiceProvider xamlServiceProvider107 = new XamlServiceProvider();
			Type typeFromHandle214 = typeof(IProvideValueTarget);
			object[] array108 = new object[0 + 5];
			array108[0] = section19;
			array108[1] = settingsView5;
			array108[2] = grid7;
			array108[3] = grid9;
			array108[4] = this;
			object obj198;
			xamlServiceProvider107.Add(typeFromHandle214, obj198 = new SimpleValueTargetProvider(array108, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider107.Add(typeof(IReferenceProvider), obj198);
			Type typeFromHandle215 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver107 = new XmlNamespaceResolver();
			xmlNamespaceResolver107.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver107.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver107.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver107.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver107.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver107.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver107.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver107.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver107.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver107.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider107.Add(typeFromHandle215, new XamlTypeResolver(xmlNamespaceResolver107, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider107.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(451, 33)));
			object obj199 = markupExtension107.ProvideValue(xamlServiceProvider107);
			section19.Title = obj199;
			staticResourceExtension31.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension108 = staticResourceExtension31;
			XamlServiceProvider xamlServiceProvider108 = new XamlServiceProvider();
			Type typeFromHandle216 = typeof(IProvideValueTarget);
			object[] array109 = new object[0 + 6];
			array109[0] = multiBinding;
			array109[1] = section19;
			array109[2] = settingsView5;
			array109[3] = grid7;
			array109[4] = grid9;
			array109[5] = this;
			object obj200;
			xamlServiceProvider108.Add(typeFromHandle216, obj200 = new SimpleValueTargetProvider(array109, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider108.Add(typeof(IReferenceProvider), obj200);
			Type typeFromHandle217 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver108 = new XmlNamespaceResolver();
			xmlNamespaceResolver108.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver108.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver108.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver108.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver108.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver108.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver108.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver108.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver108.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver108.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider108.Add(typeFromHandle217, new XamlTypeResolver(xmlNamespaceResolver108, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider108.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(453, 43)));
			object obj201 = markupExtension108.ProvideValue(xamlServiceProvider108);
			multiBinding.Converter = obj201;
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "UseLitersForVolume";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase27);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "IsVisible";
			referenceExtension2.Name = "panelEngineVolume";
			IMarkupExtension markupExtension109 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider109 = new XamlServiceProvider();
			Type typeFromHandle218 = typeof(IProvideValueTarget);
			object[] array110 = new object[0 + 7];
			array110[0] = bindingExtension28;
			array110[1] = multiBinding;
			array110[2] = section19;
			array110[3] = settingsView5;
			array110[4] = grid7;
			array110[5] = grid9;
			array110[6] = this;
			object obj202;
			xamlServiceProvider109.Add(typeFromHandle218, obj202 = new SimpleValueTargetProvider(array110, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider109.Add(typeof(IReferenceProvider), obj202);
			Type typeFromHandle219 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver109 = new XmlNamespaceResolver();
			xmlNamespaceResolver109.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver109.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver109.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver109.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver109.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver109.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver109.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver109.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver109.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver109.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider109.Add(typeFromHandle219, new XamlTypeResolver(xmlNamespaceResolver109, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider109.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(458, 37)));
			object obj203 = markupExtension109.ProvideValue(xamlServiceProvider109);
			bindingExtension28.Source = obj203;
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase28);
			section19.SetBinding(Section.IsVisibleProperty, multiBinding);
			customCell8.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension29.Mode = 1;
			staticResourceExtension32.Key = "DecimalFuelPriceForLitreToStringConverter";
			IMarkupExtension markupExtension110 = staticResourceExtension32;
			XamlServiceProvider xamlServiceProvider110 = new XamlServiceProvider();
			Type typeFromHandle220 = typeof(IProvideValueTarget);
			object[] array111 = new object[0 + 8];
			array111[0] = bindingExtension29;
			array111[1] = entry;
			array111[2] = customCell8;
			array111[3] = section19;
			array111[4] = settingsView5;
			array111[5] = grid7;
			array111[6] = grid9;
			array111[7] = this;
			object obj204;
			xamlServiceProvider110.Add(typeFromHandle220, obj204 = new SimpleValueTargetProvider(array111, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider110.Add(typeof(IReferenceProvider), obj204);
			Type typeFromHandle221 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver110 = new XmlNamespaceResolver();
			xmlNamespaceResolver110.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver110.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver110.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver110.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver110.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver110.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver110.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver110.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver110.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver110.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider110.Add(typeFromHandle221, new XamlTypeResolver(xmlNamespaceResolver110, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider110.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(463, 36)));
			object obj205 = markupExtension110.ProvideValue(xamlServiceProvider110);
			bindingExtension29.Converter = obj205;
			bindingExtension29.Path = "FuelPriceForLitre";
			bindingExtension29.TypedBinding = new TypedBinding<SharedSettings, decimal>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
				}
				return default(ValueTuple<decimal, bool>);
			}, delegate(SharedSettings A_0, decimal A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelPriceForLitre = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelPriceForLitre")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase29);
			customCell8.SetValue(CustomCell.ContentProperty, entry);
			section19.Add(customCell8);
			customCell9.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "Currency";
			bindingExtension30.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Currency, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Currency = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Currency")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase30);
			customCell9.SetValue(CustomCell.ContentProperty, entry2);
			section19.Add(customCell9);
			settingsView5.Root.Add(section19);
			translate60.Text = "Settings_tbFuelPriceG.Text";
			IMarkupExtension markupExtension111 = translate60;
			XamlServiceProvider xamlServiceProvider111 = new XamlServiceProvider();
			Type typeFromHandle222 = typeof(IProvideValueTarget);
			object[] array112 = new object[0 + 5];
			array112[0] = section20;
			array112[1] = settingsView5;
			array112[2] = grid7;
			array112[3] = grid9;
			array112[4] = this;
			object obj206;
			xamlServiceProvider111.Add(typeFromHandle222, obj206 = new SimpleValueTargetProvider(array112, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider111.Add(typeof(IReferenceProvider), obj206);
			Type typeFromHandle223 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver111 = new XmlNamespaceResolver();
			xmlNamespaceResolver111.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver111.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver111.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver111.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver111.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver111.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver111.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver111.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver111.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver111.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider111.Add(typeFromHandle223, new XamlTypeResolver(xmlNamespaceResolver111, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider111.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(471, 33)));
			object obj207 = markupExtension111.ProvideValue(xamlServiceProvider111);
			section20.Title = obj207;
			staticResourceExtension33.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension112 = staticResourceExtension33;
			XamlServiceProvider xamlServiceProvider112 = new XamlServiceProvider();
			Type typeFromHandle224 = typeof(IProvideValueTarget);
			object[] array113 = new object[0 + 6];
			array113[0] = multiBinding2;
			array113[1] = section20;
			array113[2] = settingsView5;
			array113[3] = grid7;
			array113[4] = grid9;
			array113[5] = this;
			object obj208;
			xamlServiceProvider112.Add(typeFromHandle224, obj208 = new SimpleValueTargetProvider(array113, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider112.Add(typeof(IReferenceProvider), obj208);
			Type typeFromHandle225 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver112 = new XmlNamespaceResolver();
			xmlNamespaceResolver112.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver112.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver112.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver112.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver112.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver112.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver112.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver112.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver112.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver112.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider112.Add(typeFromHandle225, new XamlTypeResolver(xmlNamespaceResolver112, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider112.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(473, 43)));
			object obj209 = markupExtension112.ProvideValue(xamlServiceProvider112);
			multiBinding2.Converter = obj209;
			staticResourceExtension34.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension113 = staticResourceExtension34;
			XamlServiceProvider xamlServiceProvider113 = new XamlServiceProvider();
			Type typeFromHandle226 = typeof(IProvideValueTarget);
			object[] array114 = new object[0 + 7];
			array114[0] = bindingExtension31;
			array114[1] = multiBinding2;
			array114[2] = section20;
			array114[3] = settingsView5;
			array114[4] = grid7;
			array114[5] = grid9;
			array114[6] = this;
			object obj210;
			xamlServiceProvider113.Add(typeFromHandle226, obj210 = new SimpleValueTargetProvider(array114, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider113.Add(typeof(IReferenceProvider), obj210);
			Type typeFromHandle227 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver113 = new XmlNamespaceResolver();
			xmlNamespaceResolver113.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver113.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver113.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver113.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver113.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver113.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver113.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver113.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver113.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver113.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider113.Add(typeFromHandle227, new XamlTypeResolver(xmlNamespaceResolver113, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider113.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(475, 37)));
			object obj211 = markupExtension113.ProvideValue(xamlServiceProvider113);
			bindingExtension31.Converter = obj211;
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "UseLitersForVolume";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase31);
			bindingExtension32.Mode = 2;
			bindingExtension32.Path = "IsVisible";
			referenceExtension3.Name = "panelEngineVolume";
			IMarkupExtension markupExtension114 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider114 = new XamlServiceProvider();
			Type typeFromHandle228 = typeof(IProvideValueTarget);
			object[] array115 = new object[0 + 7];
			array115[0] = bindingExtension32;
			array115[1] = multiBinding2;
			array115[2] = section20;
			array115[3] = settingsView5;
			array115[4] = grid7;
			array115[5] = grid9;
			array115[6] = this;
			object obj212;
			xamlServiceProvider114.Add(typeFromHandle228, obj212 = new SimpleValueTargetProvider(array115, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider114.Add(typeof(IReferenceProvider), obj212);
			Type typeFromHandle229 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver114 = new XmlNamespaceResolver();
			xmlNamespaceResolver114.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver114.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver114.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver114.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver114.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver114.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver114.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver114.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver114.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver114.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider114.Add(typeFromHandle229, new XamlTypeResolver(xmlNamespaceResolver114, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider114.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(481, 37)));
			object obj213 = markupExtension114.ProvideValue(xamlServiceProvider114);
			bindingExtension32.Source = obj213;
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase32);
			section20.SetBinding(Section.IsVisibleProperty, multiBinding2);
			customCell10.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension33.Mode = 1;
			staticResourceExtension35.Key = "DecimalFuelPriceForLitreToStringConverter";
			IMarkupExtension markupExtension115 = staticResourceExtension35;
			XamlServiceProvider xamlServiceProvider115 = new XamlServiceProvider();
			Type typeFromHandle230 = typeof(IProvideValueTarget);
			object[] array116 = new object[0 + 8];
			array116[0] = bindingExtension33;
			array116[1] = entry3;
			array116[2] = customCell10;
			array116[3] = section20;
			array116[4] = settingsView5;
			array116[5] = grid7;
			array116[6] = grid9;
			array116[7] = this;
			object obj214;
			xamlServiceProvider115.Add(typeFromHandle230, obj214 = new SimpleValueTargetProvider(array116, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider115.Add(typeof(IReferenceProvider), obj214);
			Type typeFromHandle231 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver115 = new XmlNamespaceResolver();
			xmlNamespaceResolver115.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver115.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver115.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver115.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver115.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver115.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver115.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver115.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver115.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver115.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider115.Add(typeFromHandle231, new XamlTypeResolver(xmlNamespaceResolver115, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider115.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(485, 36)));
			object obj215 = markupExtension115.ProvideValue(xamlServiceProvider115);
			bindingExtension33.Converter = obj215;
			bindingExtension33.Path = "FuelPriceForLitre";
			bindingExtension33.TypedBinding = new TypedBinding<SharedSettings, decimal>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
				}
				return default(ValueTuple<decimal, bool>);
			}, delegate(SharedSettings A_0, decimal A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelPriceForLitre = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelPriceForLitre")
			});
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase33);
			customCell10.SetValue(CustomCell.ContentProperty, entry3);
			section20.Add(customCell10);
			settingsView5.Root.Add(section20);
			translate61.Text = "Settings_Control_tbEngineDisplacement.Text";
			IMarkupExtension markupExtension116 = translate61;
			XamlServiceProvider xamlServiceProvider116 = new XamlServiceProvider();
			Type typeFromHandle232 = typeof(IProvideValueTarget);
			object[] array117 = new object[0 + 5];
			array117[0] = section21;
			array117[1] = settingsView5;
			array117[2] = grid7;
			array117[3] = grid9;
			array117[4] = this;
			object obj216;
			xamlServiceProvider116.Add(typeFromHandle232, obj216 = new SimpleValueTargetProvider(array117, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider116.Add(typeof(IReferenceProvider), obj216);
			Type typeFromHandle233 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver116 = new XmlNamespaceResolver();
			xmlNamespaceResolver116.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver116.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver116.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver116.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver116.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver116.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver116.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver116.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver116.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver116.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider116.Add(typeFromHandle233, new XamlTypeResolver(xmlNamespaceResolver116, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider116.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(492, 25)));
			object obj217 = markupExtension116.ProvideValue(xamlServiceProvider116);
			section21.Title = obj217;
			staticResourceExtension36.Key = "CombustionEngineFuelTypeToTrueConverter";
			IMarkupExtension markupExtension117 = staticResourceExtension36;
			XamlServiceProvider xamlServiceProvider117 = new XamlServiceProvider();
			Type typeFromHandle234 = typeof(IProvideValueTarget);
			object[] array118 = new object[0 + 6];
			array118[0] = bindingExtension34;
			array118[1] = section21;
			array118[2] = settingsView5;
			array118[3] = grid7;
			array118[4] = grid9;
			array118[5] = this;
			object obj218;
			xamlServiceProvider117.Add(typeFromHandle234, obj218 = new SimpleValueTargetProvider(array118, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider117.Add(typeof(IReferenceProvider), obj218);
			Type typeFromHandle235 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver117 = new XmlNamespaceResolver();
			xmlNamespaceResolver117.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver117.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver117.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver117.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver117.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver117.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver117.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver117.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver117.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver117.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider117.Add(typeFromHandle235, new XamlTypeResolver(xmlNamespaceResolver117, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider117.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(493, 25)));
			object obj219 = markupExtension117.ProvideValue(xamlServiceProvider117);
			bindingExtension34.Converter = obj219;
			bindingExtension34.Mode = 2;
			bindingExtension34.Path = "FuelType";
			bindingExtension34.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			section21.SetBinding(Section.IsVisibleProperty, bindingBase34);
			dynamicResourceExtension14.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension118 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider118 = new XamlServiceProvider();
			Type typeFromHandle236 = typeof(IProvideValueTarget);
			object[] array119 = new object[0 + 7];
			array119[0] = picker;
			array119[1] = settingsCustomCellForPicker;
			array119[2] = section21;
			array119[3] = settingsView5;
			array119[4] = grid7;
			array119[5] = grid9;
			array119[6] = this;
			object obj220;
			xamlServiceProvider118.Add(typeFromHandle236, obj220 = new SimpleValueTargetProvider(array119, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider118.Add(typeof(IReferenceProvider), obj220);
			Type typeFromHandle237 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver118 = new XmlNamespaceResolver();
			xmlNamespaceResolver118.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver118.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver118.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver118.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver118.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver118.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver118.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver118.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver118.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver118.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider118.Add(typeFromHandle237, new XamlTypeResolver(xmlNamespaceResolver118, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider118.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(506, 37)));
			DynamicResource dynamicResource14 = markupExtension118.ProvideValue(xamlServiceProvider118);
			picker.SetDynamicResource(Picker.FontSizeProperty, dynamicResource14.Key);
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "EngineDisplacement";
			bindingExtension35.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.EngineDisplacement, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.EngineDisplacement = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "EngineDisplacement")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase35);
			picker.SetValue(Picker.ItemsSourceProperty, array);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, picker);
			section21.Add(settingsCustomCellForPicker);
			translate62.Text = "Settings_Control_tbNumberOfCylinders.Text";
			IMarkupExtension markupExtension119 = translate62;
			XamlServiceProvider xamlServiceProvider119 = new XamlServiceProvider();
			Type typeFromHandle238 = typeof(IProvideValueTarget);
			object[] array120 = new object[0 + 6];
			array120[0] = numberPickerCell;
			array120[1] = section21;
			array120[2] = settingsView5;
			array120[3] = grid7;
			array120[4] = grid9;
			array120[5] = this;
			object obj221;
			xamlServiceProvider119.Add(typeFromHandle238, obj221 = new SimpleValueTargetProvider(array120, CellBase.TitleProperty, nameScope));
			xamlServiceProvider119.Add(typeof(IReferenceProvider), obj221);
			Type typeFromHandle239 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver119 = new XmlNamespaceResolver();
			xmlNamespaceResolver119.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver119.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver119.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver119.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver119.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver119.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver119.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver119.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver119.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver119.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider119.Add(typeFromHandle239, new XamlTypeResolver(xmlNamespaceResolver119, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider119.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(645, 29)));
			object obj222 = markupExtension119.ProvideValue(xamlServiceProvider119);
			numberPickerCell.Title = obj222;
			numberPickerCell.SetValue(NumberPickerCell.MaxProperty, 16);
			numberPickerCell.SetValue(NumberPickerCell.MinProperty, 1);
			bindingExtension36.Mode = 1;
			bindingExtension36.Path = "EngineCylinders";
			bindingExtension36.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.EngineCylinders, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.EngineCylinders = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "EngineCylinders")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			numberPickerCell.SetBinding(NumberPickerCell.NumberProperty, bindingBase36);
			section21.Add(numberPickerCell);
			translate63.Text = "sett_FuelHybridCar";
			IMarkupExtension markupExtension120 = translate63;
			XamlServiceProvider xamlServiceProvider120 = new XamlServiceProvider();
			Type typeFromHandle240 = typeof(IProvideValueTarget);
			object[] array121 = new object[0 + 6];
			array121[0] = settingsCheckBoxCellPatched3;
			array121[1] = section21;
			array121[2] = settingsView5;
			array121[3] = grid7;
			array121[4] = grid9;
			array121[5] = this;
			object obj223;
			xamlServiceProvider120.Add(typeFromHandle240, obj223 = new SimpleValueTargetProvider(array121, CellBase.TitleProperty, nameScope));
			xamlServiceProvider120.Add(typeof(IReferenceProvider), obj223);
			Type typeFromHandle241 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver120 = new XmlNamespaceResolver();
			xmlNamespaceResolver120.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver120.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver120.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver120.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver120.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver120.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver120.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver120.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver120.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver120.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider120.Add(typeFromHandle241, new XamlTypeResolver(xmlNamespaceResolver120, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider120.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(649, 57)));
			object obj224 = markupExtension120.ProvideValue(xamlServiceProvider120);
			settingsCheckBoxCellPatched3.Title = obj224;
			bindingExtension37.Mode = 1;
			bindingExtension37.Path = "FuelHybridCar";
			bindingExtension37.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.FuelHybridCar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelHybridCar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelHybridCar")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase37);
			section21.Add(settingsCheckBoxCellPatched3);
			settingsView5.Root.Add(section21);
			grid7.Children.Add(settingsView5);
			button6.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension15.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension121 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider121 = new XamlServiceProvider();
			Type typeFromHandle242 = typeof(IProvideValueTarget);
			object[] array122 = new object[0 + 4];
			array122[0] = button6;
			array122[1] = grid7;
			array122[2] = grid9;
			array122[3] = this;
			object obj225;
			xamlServiceProvider121.Add(typeFromHandle242, obj225 = new SimpleValueTargetProvider(array122, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider121.Add(typeof(IReferenceProvider), obj225);
			Type typeFromHandle243 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver121 = new XmlNamespaceResolver();
			xmlNamespaceResolver121.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver121.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver121.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver121.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver121.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver121.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver121.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver121.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver121.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver121.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider121.Add(typeFromHandle243, new XamlTypeResolver(xmlNamespaceResolver121, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider121.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(668, 21)));
			DynamicResource dynamicResource15 = markupExtension121.ProvideValue(xamlServiceProvider121);
			button6.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource15.Key);
			button6.Clicked += this.btnFuelNext_Clicked;
			translate64.Text = "ios_NEXT";
			IMarkupExtension markupExtension122 = translate64;
			XamlServiceProvider xamlServiceProvider122 = new XamlServiceProvider();
			Type typeFromHandle244 = typeof(IProvideValueTarget);
			object[] array123 = new object[0 + 4];
			array123[0] = button6;
			array123[1] = grid7;
			array123[2] = grid9;
			array123[3] = this;
			object obj226;
			xamlServiceProvider122.Add(typeFromHandle244, obj226 = new SimpleValueTargetProvider(array123, Button.TextProperty, nameScope));
			xamlServiceProvider122.Add(typeof(IReferenceProvider), obj226);
			Type typeFromHandle245 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver122 = new XmlNamespaceResolver();
			xmlNamespaceResolver122.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver122.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver122.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver122.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver122.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver122.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver122.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver122.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver122.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver122.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider122.Add(typeFromHandle245, new XamlTypeResolver(xmlNamespaceResolver122, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider122.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(670, 21)));
			object obj227 = markupExtension122.ProvideValue(xamlServiceProvider122);
			button6.Text = obj227;
			button6.SetValue(Button.TextColorProperty, Color.White);
			grid7.Children.Add(button6);
			grid9.Children.Add(grid7);
			grid8.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid8.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView6.SetValue(Grid.RowProperty, 0);
			settingsView6.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView6.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			settingsView6.SetValue(SettingsView.SeparatorColorProperty, Color.Transparent);
			bindingExtension38.Path = "GDPR_ShowPersonalyzed";
			bindingExtension38.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GDPR_ShowPersonalyzed, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GDPR_ShowPersonalyzed = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "GDPR_ShowPersonalyzed")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			section22.SetBinding(RadioCell.SelectedValueProperty, bindingBase38);
			customCell11.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension37.Key = "BaseFontSize+";
			IMarkupExtension markupExtension123 = staticResourceExtension37;
			XamlServiceProvider xamlServiceProvider123 = new XamlServiceProvider();
			Type typeFromHandle246 = typeof(IProvideValueTarget);
			object[] array124 = new object[0 + 7];
			array124[0] = label4;
			array124[1] = customCell11;
			array124[2] = section22;
			array124[3] = settingsView6;
			array124[4] = grid8;
			array124[5] = grid9;
			array124[6] = this;
			object obj228;
			xamlServiceProvider123.Add(typeFromHandle246, obj228 = new SimpleValueTargetProvider(array124, Label.FontSizeProperty, nameScope));
			xamlServiceProvider123.Add(typeof(IReferenceProvider), obj228);
			Type typeFromHandle247 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver123 = new XmlNamespaceResolver();
			xmlNamespaceResolver123.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver123.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver123.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver123.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver123.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver123.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver123.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver123.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver123.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver123.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider123.Add(typeFromHandle247, new XamlTypeResolver(xmlNamespaceResolver123, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider123.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(692, 36)));
			object obj229 = markupExtension123.ProvideValue(xamlServiceProvider123);
			label4.FontSize = (double)obj229;
			translate65.Text = "GDPR_Consent_Text";
			IMarkupExtension markupExtension124 = translate65;
			XamlServiceProvider xamlServiceProvider124 = new XamlServiceProvider();
			Type typeFromHandle248 = typeof(IProvideValueTarget);
			object[] array125 = new object[0 + 7];
			array125[0] = label4;
			array125[1] = customCell11;
			array125[2] = section22;
			array125[3] = settingsView6;
			array125[4] = grid8;
			array125[5] = grid9;
			array125[6] = this;
			object obj230;
			xamlServiceProvider124.Add(typeFromHandle248, obj230 = new SimpleValueTargetProvider(array125, Label.TextProperty, nameScope));
			xamlServiceProvider124.Add(typeof(IReferenceProvider), obj230);
			Type typeFromHandle249 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver124 = new XmlNamespaceResolver();
			xmlNamespaceResolver124.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver124.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver124.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver124.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver124.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver124.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver124.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver124.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver124.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver124.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider124.Add(typeFromHandle249, new XamlTypeResolver(xmlNamespaceResolver124, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider124.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(692, 78)));
			object obj231 = markupExtension124.ProvideValue(xamlServiceProvider124);
			label4.Text = obj231;
			customCell11.SetValue(CustomCell.ContentProperty, label4);
			section22.Add(customCell11);
			translate66.Text = "GDPR_PersonalizedAds";
			IMarkupExtension markupExtension125 = translate66;
			XamlServiceProvider xamlServiceProvider125 = new XamlServiceProvider();
			Type typeFromHandle250 = typeof(IProvideValueTarget);
			object[] array126 = new object[0 + 6];
			array126[0] = radioCell38;
			array126[1] = section22;
			array126[2] = settingsView6;
			array126[3] = grid8;
			array126[4] = grid9;
			array126[5] = this;
			object obj232;
			xamlServiceProvider125.Add(typeFromHandle250, obj232 = new SimpleValueTargetProvider(array126, CellBase.TitleProperty, nameScope));
			xamlServiceProvider125.Add(typeof(IReferenceProvider), obj232);
			Type typeFromHandle251 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver125 = new XmlNamespaceResolver();
			xmlNamespaceResolver125.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver125.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver125.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver125.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver125.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver125.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver125.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver125.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver125.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver125.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider125.Add(typeFromHandle251, new XamlTypeResolver(xmlNamespaceResolver125, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider125.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(696, 29)));
			object obj233 = markupExtension125.ProvideValue(xamlServiceProvider125);
			radioCell38.Title = obj233;
			staticResourceExtension38.Key = "TrueValue";
			IMarkupExtension markupExtension126 = staticResourceExtension38;
			XamlServiceProvider xamlServiceProvider126 = new XamlServiceProvider();
			Type typeFromHandle252 = typeof(IProvideValueTarget);
			object[] array127 = new object[0 + 6];
			array127[0] = radioCell38;
			array127[1] = section22;
			array127[2] = settingsView6;
			array127[3] = grid8;
			array127[4] = grid9;
			array127[5] = this;
			object obj234;
			xamlServiceProvider126.Add(typeFromHandle252, obj234 = new SimpleValueTargetProvider(array127, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider126.Add(typeof(IReferenceProvider), obj234);
			Type typeFromHandle253 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver126 = new XmlNamespaceResolver();
			xmlNamespaceResolver126.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver126.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver126.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver126.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver126.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver126.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver126.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver126.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver126.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver126.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider126.Add(typeFromHandle253, new XamlTypeResolver(xmlNamespaceResolver126, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider126.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(697, 29)));
			object obj235 = markupExtension126.ProvideValue(xamlServiceProvider126);
			radioCell38.SetValue(RadioCell.ValueProperty, obj235);
			section22.Add(radioCell38);
			translate67.Text = "GDPR_NonPersonalizedAds";
			IMarkupExtension markupExtension127 = translate67;
			XamlServiceProvider xamlServiceProvider127 = new XamlServiceProvider();
			Type typeFromHandle254 = typeof(IProvideValueTarget);
			object[] array128 = new object[0 + 6];
			array128[0] = radioCell39;
			array128[1] = section22;
			array128[2] = settingsView6;
			array128[3] = grid8;
			array128[4] = grid9;
			array128[5] = this;
			object obj236;
			xamlServiceProvider127.Add(typeFromHandle254, obj236 = new SimpleValueTargetProvider(array128, CellBase.TitleProperty, nameScope));
			xamlServiceProvider127.Add(typeof(IReferenceProvider), obj236);
			Type typeFromHandle255 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver127 = new XmlNamespaceResolver();
			xmlNamespaceResolver127.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver127.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver127.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver127.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver127.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver127.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver127.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver127.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver127.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver127.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider127.Add(typeFromHandle255, new XamlTypeResolver(xmlNamespaceResolver127, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider127.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(700, 29)));
			object obj237 = markupExtension127.ProvideValue(xamlServiceProvider127);
			radioCell39.Title = obj237;
			staticResourceExtension39.Key = "FalseValue";
			IMarkupExtension markupExtension128 = staticResourceExtension39;
			XamlServiceProvider xamlServiceProvider128 = new XamlServiceProvider();
			Type typeFromHandle256 = typeof(IProvideValueTarget);
			object[] array129 = new object[0 + 6];
			array129[0] = radioCell39;
			array129[1] = section22;
			array129[2] = settingsView6;
			array129[3] = grid8;
			array129[4] = grid9;
			array129[5] = this;
			object obj238;
			xamlServiceProvider128.Add(typeFromHandle256, obj238 = new SimpleValueTargetProvider(array129, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider128.Add(typeof(IReferenceProvider), obj238);
			Type typeFromHandle257 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver128 = new XmlNamespaceResolver();
			xmlNamespaceResolver128.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver128.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver128.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver128.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver128.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver128.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver128.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver128.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver128.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver128.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider128.Add(typeFromHandle257, new XamlTypeResolver(xmlNamespaceResolver128, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider128.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(701, 29)));
			object obj239 = markupExtension128.ProvideValue(xamlServiceProvider128);
			radioCell39.SetValue(RadioCell.ValueProperty, obj239);
			section22.Add(radioCell39);
			translate68.Text = "ios_PrivacyPolicy";
			IMarkupExtension markupExtension129 = translate68;
			XamlServiceProvider xamlServiceProvider129 = new XamlServiceProvider();
			Type typeFromHandle258 = typeof(IProvideValueTarget);
			object[] array130 = new object[0 + 6];
			array130[0] = buttonCell2;
			array130[1] = section22;
			array130[2] = settingsView6;
			array130[3] = grid8;
			array130[4] = grid9;
			array130[5] = this;
			object obj240;
			xamlServiceProvider129.Add(typeFromHandle258, obj240 = new SimpleValueTargetProvider(array130, CellBase.TitleProperty, nameScope));
			xamlServiceProvider129.Add(typeof(IReferenceProvider), obj240);
			Type typeFromHandle259 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver129 = new XmlNamespaceResolver();
			xmlNamespaceResolver129.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver129.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver129.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver129.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver129.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver129.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver129.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver129.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver129.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver129.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider129.Add(typeFromHandle259, new XamlTypeResolver(xmlNamespaceResolver129, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider129.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(703, 29)));
			object obj241 = markupExtension129.ProvideValue(xamlServiceProvider129);
			buttonCell2.Title = obj241;
			buttonCell2.Tapped += this.btnPrivacyPolicy_Tapped;
			dynamicResourceExtension16.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension130 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider130 = new XamlServiceProvider();
			Type typeFromHandle260 = typeof(IProvideValueTarget);
			object[] array131 = new object[0 + 6];
			array131[0] = buttonCell2;
			array131[1] = section22;
			array131[2] = settingsView6;
			array131[3] = grid8;
			array131[4] = grid9;
			array131[5] = this;
			object obj242;
			xamlServiceProvider130.Add(typeFromHandle260, obj242 = new SimpleValueTargetProvider(array131, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider130.Add(typeof(IReferenceProvider), obj242);
			Type typeFromHandle261 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver130 = new XmlNamespaceResolver();
			xmlNamespaceResolver130.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver130.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver130.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver130.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver130.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver130.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver130.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver130.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver130.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver130.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider130.Add(typeFromHandle261, new XamlTypeResolver(xmlNamespaceResolver130, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider130.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(705, 29)));
			DynamicResource dynamicResource16 = markupExtension130.ProvideValue(xamlServiceProvider130);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource16.Key);
			section22.Add(buttonCell2);
			settingsView6.Root.Add(section22);
			grid8.Children.Add(settingsView6);
			button7.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension17.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension131 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider131 = new XamlServiceProvider();
			Type typeFromHandle262 = typeof(IProvideValueTarget);
			object[] array132 = new object[0 + 4];
			array132[0] = button7;
			array132[1] = grid8;
			array132[2] = grid9;
			array132[3] = this;
			object obj243;
			xamlServiceProvider131.Add(typeFromHandle262, obj243 = new SimpleValueTargetProvider(array132, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider131.Add(typeof(IReferenceProvider), obj243);
			Type typeFromHandle263 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver131 = new XmlNamespaceResolver();
			xmlNamespaceResolver131.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver131.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver131.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver131.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver131.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver131.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver131.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver131.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver131.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver131.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider131.Add(typeFromHandle263, new XamlTypeResolver(xmlNamespaceResolver131, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider131.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(721, 21)));
			DynamicResource dynamicResource17 = markupExtension131.ProvideValue(xamlServiceProvider131);
			button7.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource17.Key);
			button7.Clicked += this.btnPrivacyNext_Clicked;
			translate69.Text = "ios_NEXT";
			IMarkupExtension markupExtension132 = translate69;
			XamlServiceProvider xamlServiceProvider132 = new XamlServiceProvider();
			Type typeFromHandle264 = typeof(IProvideValueTarget);
			object[] array133 = new object[0 + 4];
			array133[0] = button7;
			array133[1] = grid8;
			array133[2] = grid9;
			array133[3] = this;
			object obj244;
			xamlServiceProvider132.Add(typeFromHandle264, obj244 = new SimpleValueTargetProvider(array133, Button.TextProperty, nameScope));
			xamlServiceProvider132.Add(typeof(IReferenceProvider), obj244);
			Type typeFromHandle265 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver132 = new XmlNamespaceResolver();
			xmlNamespaceResolver132.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver132.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver132.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver132.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver132.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver132.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver132.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver132.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver132.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver132.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider132.Add(typeFromHandle265, new XamlTypeResolver(xmlNamespaceResolver132, typeof(WelcomePage1V4).GetTypeInfo().Assembly));
			xamlServiceProvider132.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(723, 21)));
			object obj245 = markupExtension132.ProvideValue(xamlServiceProvider132);
			button7.Text = obj245;
			button7.SetValue(Button.TextColorProperty, Color.White);
			grid8.Children.Add(button7);
			grid9.Children.Add(grid8);
			this.SetValue(ContentPage.ContentProperty, grid9);
		}

		// Token: 0x0600387D RID: 14461 RVA: 0x002BE698 File Offset: 0x002BC898
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<WelcomePage1V4>(this, typeof(WelcomePage1V4));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.lbFormattedTitle = NameScopeExtensions.FindByName<NonScalableLabel>(this, "lbFormattedTitle");
			this.lbPageTitle = NameScopeExtensions.FindByName<Span>(this, "lbPageTitle");
			this.lbPageTitleSeparator = NameScopeExtensions.FindByName<Span>(this, "lbPageTitleSeparator");
			this.lbPageSubtitle = NameScopeExtensions.FindByName<Span>(this, "lbPageSubtitle");
			this.progressBar = NameScopeExtensions.FindByName<ProgressBar>(this, "progressBar");
			this.gridEULA = NameScopeExtensions.FindByName<Grid>(this, "gridEULA");
			this.setEULA = NameScopeExtensions.FindByName<SettingsView>(this, "setEULA");
			this.lbEULA = NameScopeExtensions.FindByName<Label>(this, "lbEULA");
			this.eulaActivityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "eulaActivityFrame");
			this.gridAdapter = NameScopeExtensions.FindByName<Grid>(this, "gridAdapter");
			this.setAdapter = NameScopeExtensions.FindByName<SettingsView>(this, "setAdapter");
			this.gridInterface = NameScopeExtensions.FindByName<Grid>(this, "gridInterface");
			this.setInterface = NameScopeExtensions.FindByName<SettingsView>(this, "setInterface");
			this.gridDashPreview = NameScopeExtensions.FindByName<Grid>(this, "gridDashPreview");
			this.gridUnits = NameScopeExtensions.FindByName<Grid>(this, "gridUnits");
			this.setUnits = NameScopeExtensions.FindByName<SettingsView>(this, "setUnits");
			this.gridProfileSelector = NameScopeExtensions.FindByName<Grid>(this, "gridProfileSelector");
			this.profileSelector = NameScopeExtensions.FindByName<ProfileSelectorV3>(this, "profileSelector");
			this.gridFuel = NameScopeExtensions.FindByName<Grid>(this, "gridFuel");
			this.setFuel = NameScopeExtensions.FindByName<SettingsView>(this, "setFuel");
			this.panelEngineVolume = NameScopeExtensions.FindByName<Section>(this, "panelEngineVolume");
			this.btnPrivacyNext = NameScopeExtensions.FindByName<Button>(this, "btnPrivacyNext");
			this.gridPrivacy = NameScopeExtensions.FindByName<Grid>(this, "gridPrivacy");
			this.setPrivacy = NameScopeExtensions.FindByName<SettingsView>(this, "setPrivacy");
			this.panelAdsSelection1 = NameScopeExtensions.FindByName<RadioCell>(this, "panelAdsSelection1");
			this.panelAdsSelection2 = NameScopeExtensions.FindByName<RadioCell>(this, "panelAdsSelection2");
		}

		// Token: 0x0600387E RID: 14462 RVA: 0x002BE884 File Offset: 0x002BCA84
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1432(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DarkMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600387F RID: 14463 RVA: 0x002BE8B4 File Offset: 0x002BCAB4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1433(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DarkMode = A_1;
				return;
			}
		}

		// Token: 0x06003880 RID: 14464 RVA: 0x002BE8D0 File Offset: 0x002BCAD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1434(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003881 RID: 14465 RVA: 0x002BE8E0 File Offset: 0x002BCAE0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1435(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003882 RID: 14466 RVA: 0x002BE910 File Offset: 0x002BCB10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1436(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AutomaticallySwitchTheme = A_1;
				return;
			}
		}

		// Token: 0x06003883 RID: 14467 RVA: 0x002BE92C File Offset: 0x002BCB2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1437(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003884 RID: 14468 RVA: 0x002BE93C File Offset: 0x002BCB3C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1438(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchThemeAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003885 RID: 14469 RVA: 0x002BE96C File Offset: 0x002BCB6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1439(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x002BE97C File Offset: 0x002BCB7C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1440(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x002BE9AC File Offset: 0x002BCBAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1441(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x002BE9BC File Offset: 0x002BCBBC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1442(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x002BE9EC File Offset: 0x002BCBEC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1443(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x002BE9FC File Offset: 0x002BCBFC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1444(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x002BEA2C File Offset: 0x002BCC2C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1445(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidRecolorNavBar = A_1;
				return;
			}
		}

		// Token: 0x0600388C RID: 14476 RVA: 0x002BEA48 File Offset: 0x002BCC48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1446(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600388D RID: 14477 RVA: 0x002BEA58 File Offset: 0x002BCC58
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1447(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DashboardTheme, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600388E RID: 14478 RVA: 0x002BEA88 File Offset: 0x002BCC88
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1448(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.DashboardTheme = A_1;
				return;
			}
		}

		// Token: 0x0600388F RID: 14479 RVA: 0x002BEAA4 File Offset: 0x002BCCA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1449(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003890 RID: 14480 RVA: 0x002BEAB4 File Offset: 0x002BCCB4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1450(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_km, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003891 RID: 14481 RVA: 0x002BEAE4 File Offset: 0x002BCCE4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1451(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_km = A_1;
				return;
			}
		}

		// Token: 0x06003892 RID: 14482 RVA: 0x002BEB00 File Offset: 0x002BCD00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1452(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003893 RID: 14483 RVA: 0x002BEB10 File Offset: 0x002BCD10
		[CompilerGenerated]
		private static ValueTuple<FuelConsumptionUnits, bool> <InitializeComponent>typedBindingsM__1453(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
			}
			return default(ValueTuple<FuelConsumptionUnits, bool>);
		}

		// Token: 0x06003894 RID: 14484 RVA: 0x002BEB40 File Offset: 0x002BCD40
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1454(SharedSettings A_0, FuelConsumptionUnits A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelConsumptionUnit = A_1;
				return;
			}
		}

		// Token: 0x06003895 RID: 14485 RVA: 0x002BEB5C File Offset: 0x002BCD5C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1455(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003896 RID: 14486 RVA: 0x002BEB6C File Offset: 0x002BCD6C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1456(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003897 RID: 14487 RVA: 0x002BEB9C File Offset: 0x002BCD9C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1457(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseLitersForVolume = A_1;
				return;
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x002BEBB8 File Offset: 0x002BCDB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1458(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x002BEBC8 File Offset: 0x002BCDC8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1459(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600389A RID: 14490 RVA: 0x002BEBF8 File Offset: 0x002BCDF8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1460(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseUSGallon = A_1;
				return;
			}
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x002BEC14 File Offset: 0x002BCE14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1461(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x002BEC24 File Offset: 0x002BCE24
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1462(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x002BEC54 File Offset: 0x002BCE54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1463(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x002BEC64 File Offset: 0x002BCE64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1464(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x002BEC94 File Offset: 0x002BCE94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1465(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Pressure_use_kpa = A_1;
				return;
			}
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x002BECB0 File Offset: 0x002BCEB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1466(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x002BECC0 File Offset: 0x002BCEC0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1467(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x002BECF0 File Offset: 0x002BCEF0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1468(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Flow_use_grams_sec = A_1;
				return;
			}
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x002BED0C File Offset: 0x002BCF0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1469(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x002BED1C File Offset: 0x002BCF1C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1470(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x002BED4C File Offset: 0x002BCF4C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1471(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_celcium = A_1;
				return;
			}
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x002BED68 File Offset: 0x002BCF68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1472(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038A7 RID: 14503 RVA: 0x002BED78 File Offset: 0x002BCF78
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1473(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038A8 RID: 14504 RVA: 0x002BEDA8 File Offset: 0x002BCFA8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1474(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AccelerationUseG = A_1;
				return;
			}
		}

		// Token: 0x060038A9 RID: 14505 RVA: 0x002BEDC4 File Offset: 0x002BCFC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1475(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038AA RID: 14506 RVA: 0x002BEDD4 File Offset: 0x002BCFD4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1476(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038AB RID: 14507 RVA: 0x002BEE04 File Offset: 0x002BD004
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1477(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseHoursePower = A_1;
				return;
			}
		}

		// Token: 0x060038AC RID: 14508 RVA: 0x002BEE20 File Offset: 0x002BD020
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1478(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x002BEE30 File Offset: 0x002BD030
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1479(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x002BEE60 File Offset: 0x002BD060
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1480(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseNmForTorque = A_1;
				return;
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x002BEE7C File Offset: 0x002BD07C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1481(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x002BEE8C File Offset: 0x002BD08C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1482(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038B1 RID: 14513 RVA: 0x002BEEBC File Offset: 0x002BD0BC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1483(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AlwaysRecordFuelConsumption = A_1;
				return;
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x002BEED8 File Offset: 0x002BD0D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1484(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x002BEEE8 File Offset: 0x002BD0E8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1485(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x002BEF18 File Offset: 0x002BD118
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1486(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AlwaysRecordFuelConsumption = A_1;
				return;
			}
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x002BEF34 File Offset: 0x002BD134
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1487(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x002BEF44 File Offset: 0x002BD144
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1488(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x002BEF74 File Offset: 0x002BD174
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1489(SharedSettings A_0, FuelTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelType = A_1;
				return;
			}
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x002BEF90 File Offset: 0x002BD190
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1490(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x002BEFA0 File Offset: 0x002BD1A0
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1491(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x002BEFD0 File Offset: 0x002BD1D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1492(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x002BEFE0 File Offset: 0x002BD1E0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1493(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CustomFuelAF, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x002BF010 File Offset: 0x002BD210
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1494(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomFuelAF = A_1;
				return;
			}
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x002BF02C File Offset: 0x002BD22C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1495(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x002BF03C File Offset: 0x002BD23C
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1496(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x002BF06C File Offset: 0x002BD26C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1497(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x002BF07C File Offset: 0x002BD27C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1498(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CustomFuelDensity, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x002BF0AC File Offset: 0x002BD2AC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1499(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomFuelDensity = A_1;
				return;
			}
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x002BF0C8 File Offset: 0x002BD2C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1500(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x002BF0D8 File Offset: 0x002BD2D8
		[CompilerGenerated]
		private static ValueTuple<decimal, bool> <InitializeComponent>typedBindingsM__1501(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
			}
			return default(ValueTuple<decimal, bool>);
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x002BF108 File Offset: 0x002BD308
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1502(SharedSettings A_0, decimal A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelPriceForLitre = A_1;
				return;
			}
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x002BF124 File Offset: 0x002BD324
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1503(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x002BF134 File Offset: 0x002BD334
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1504(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Currency, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x002BF164 File Offset: 0x002BD364
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1505(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Currency = A_1;
				return;
			}
		}

		// Token: 0x060038C8 RID: 14536 RVA: 0x002BF180 File Offset: 0x002BD380
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1506(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038C9 RID: 14537 RVA: 0x002BF190 File Offset: 0x002BD390
		[CompilerGenerated]
		private static ValueTuple<decimal, bool> <InitializeComponent>typedBindingsM__1507(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
			}
			return default(ValueTuple<decimal, bool>);
		}

		// Token: 0x060038CA RID: 14538 RVA: 0x002BF1C0 File Offset: 0x002BD3C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1508(SharedSettings A_0, decimal A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelPriceForLitre = A_1;
				return;
			}
		}

		// Token: 0x060038CB RID: 14539 RVA: 0x002BF1DC File Offset: 0x002BD3DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1509(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038CC RID: 14540 RVA: 0x002BF1EC File Offset: 0x002BD3EC
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1510(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x060038CD RID: 14541 RVA: 0x002BF21C File Offset: 0x002BD41C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1511(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038CE RID: 14542 RVA: 0x002BF22C File Offset: 0x002BD42C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1512(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.EngineDisplacement, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060038CF RID: 14543 RVA: 0x002BF25C File Offset: 0x002BD45C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1513(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.EngineDisplacement = A_1;
				return;
			}
		}

		// Token: 0x060038D0 RID: 14544 RVA: 0x002BF278 File Offset: 0x002BD478
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1514(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038D1 RID: 14545 RVA: 0x002BF288 File Offset: 0x002BD488
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1515(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.EngineCylinders, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060038D2 RID: 14546 RVA: 0x002BF2B8 File Offset: 0x002BD4B8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1516(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.EngineCylinders = A_1;
				return;
			}
		}

		// Token: 0x060038D3 RID: 14547 RVA: 0x002BF2D4 File Offset: 0x002BD4D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1517(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038D4 RID: 14548 RVA: 0x002BF2E4 File Offset: 0x002BD4E4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1518(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.FuelHybridCar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038D5 RID: 14549 RVA: 0x002BF314 File Offset: 0x002BD514
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1519(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelHybridCar = A_1;
				return;
			}
		}

		// Token: 0x060038D6 RID: 14550 RVA: 0x002BF330 File Offset: 0x002BD530
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1520(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060038D7 RID: 14551 RVA: 0x002BF340 File Offset: 0x002BD540
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1521(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GDPR_ShowPersonalyzed, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060038D8 RID: 14552 RVA: 0x002BF370 File Offset: 0x002BD570
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1522(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GDPR_ShowPersonalyzed = A_1;
				return;
			}
		}

		// Token: 0x060038D9 RID: 14553 RVA: 0x002BF38C File Offset: 0x002BD58C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1523(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0400224B RID: 8779
		private bool eulaUpdated;

		// Token: 0x0400224C RID: 8780
		private DashboardItem dashItem;

		// Token: 0x0400224D RID: 8781
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x0400224E RID: 8782
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel lbFormattedTitle;

		// Token: 0x0400224F RID: 8783
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageTitle;

		// Token: 0x04002250 RID: 8784
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageTitleSeparator;

		// Token: 0x04002251 RID: 8785
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageSubtitle;

		// Token: 0x04002252 RID: 8786
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProgressBar progressBar;

		// Token: 0x04002253 RID: 8787
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridEULA;

		// Token: 0x04002254 RID: 8788
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setEULA;

		// Token: 0x04002255 RID: 8789
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbEULA;

		// Token: 0x04002256 RID: 8790
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame eulaActivityFrame;

		// Token: 0x04002257 RID: 8791
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridAdapter;

		// Token: 0x04002258 RID: 8792
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setAdapter;

		// Token: 0x04002259 RID: 8793
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridInterface;

		// Token: 0x0400225A RID: 8794
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setInterface;

		// Token: 0x0400225B RID: 8795
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridDashPreview;

		// Token: 0x0400225C RID: 8796
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridUnits;

		// Token: 0x0400225D RID: 8797
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setUnits;

		// Token: 0x0400225E RID: 8798
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridProfileSelector;

		// Token: 0x0400225F RID: 8799
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProfileSelectorV3 profileSelector;

		// Token: 0x04002260 RID: 8800
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridFuel;

		// Token: 0x04002261 RID: 8801
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setFuel;

		// Token: 0x04002262 RID: 8802
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section panelEngineVolume;

		// Token: 0x04002263 RID: 8803
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnPrivacyNext;

		// Token: 0x04002264 RID: 8804
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridPrivacy;

		// Token: 0x04002265 RID: 8805
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setPrivacy;

		// Token: 0x04002266 RID: 8806
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioCell panelAdsSelection1;

		// Token: 0x04002267 RID: 8807
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioCell panelAdsSelection2;

		// Token: 0x0200066D RID: 1645
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060038DA RID: 14554 RVA: 0x002BF39A File Offset: 0x002BD59A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060038DB RID: 14555 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060038DC RID: 14556 RVA: 0x002BF3A6 File Offset: 0x002BD5A6
			internal void <DroidConnectionSettingsDetector>b__19_0(object d, EventArgs args)
			{
				SharedSettings.Current.FirstConnectionAttempted = true;
			}

			// Token: 0x04002268 RID: 8808
			public static readonly WelcomePage1V4.<>c <>9 = new WelcomePage1V4.<>c();

			// Token: 0x04002269 RID: 8809
			public static EventHandler <>9__19_0;
		}

		// Token: 0x0200066E RID: 1646
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x060038DD RID: 14557 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x060038DE RID: 14558 RVA: 0x002BF3B4 File Offset: 0x002BD5B4
			internal void <UpdatePreview>b__0()
			{
				try
				{
					this.pid = PIDWithFloatValueFormula.PID010C_EngineRPM();
					this.<>4__this.dashItem = new DashboardItem();
					this.<>4__this.dashItem.HeightRequest = 150.0;
					this.<>4__this.dashItem.ItemType = DashboardItemTypes.Gauge;
					this.<>4__this.dashItem.ShowAvg = false;
					this.<>4__this.dashItem.Minimum = 0.0;
					this.<>4__this.dashItem.Maximum = 7000.0;
					this.<>4__this.dashItem.GaugeShowRedLine = true;
					this.<>4__this.dashItem.GaugeRedLineStart = 6000.0;
					this.<>4__this.dashItem.GaugeRedLineFinish = 7000.0;
					this.<>4__this.dashItem.HorizontalOptions = LayoutOptions.Center;
					this.<>4__this.dashItem.Model = new LiveDataPIDModel();
					this.<>4__this.dashItem.Model.SelectedPID = this.pid;
					this.<>4__this.dashItem.TitleFontSize = 0.0;
					this.<>4__this.dashItem.UnitsFontSize = 0.0;
					this.<>4__this.dashItem.SelectAndAddControl();
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x060038DF RID: 14559 RVA: 0x002BF534 File Offset: 0x002BD734
			internal void <UpdatePreview>b__1()
			{
				try
				{
					this.<>4__this.gridDashPreview.Children.Add(this.<>4__this.dashItem);
					DashboardItem.ApplyThemeToItem(this.<>4__this.dashItem);
					PIDWithFloatValueFormula pidwithFloatValueFormula = this.pid;
					if (pidwithFloatValueFormula != null)
					{
						pidwithFloatValueFormula.SetValue(3000.0);
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0400226A RID: 8810
			public PIDWithFloatValueFormula pid;

			// Token: 0x0400226B RID: 8811
			public WelcomePage1V4 <>4__this;
		}

		// Token: 0x0200066F RID: 1647
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AnimateNextElement>d__5 : IAsyncStateMachine
		{
			// Token: 0x060038E0 RID: 14560 RVA: 0x002BF5A0 File Offset: 0x002BD7A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<bool[]> taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool[]> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool[]>);
							num2 = -1;
							goto IL_0119;
						}
						appearing.IsVisible = true;
						taskAwaiter3 = ViewExtensions.FadeTo(appearing, 0.0, 0U, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, WelcomePage1V4.<AnimateNextElement>d__5>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = Task.WhenAll<bool>(new Task<bool>[]
					{
						ViewExtensions.FadeTo(appearing, 1.0, 200U, null),
						ViewExtensions.FadeTo(dissapearing, 0.0, 200U, null)
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool[]> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool[]>, WelcomePage1V4.<AnimateNextElement>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_0119:
					taskAwaiter.GetResult();
					dissapearing.IsVisible = false;
					dissapearing.Opacity = 1.0;
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

			// Token: 0x060038E1 RID: 14561 RVA: 0x002BF738 File Offset: 0x002BD938
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400226C RID: 8812
			public int <>1__state;

			// Token: 0x0400226D RID: 8813
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400226E RID: 8814
			public VisualElement appearing;

			// Token: 0x0400226F RID: 8815
			public VisualElement dissapearing;

			// Token: 0x04002270 RID: 8816
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002271 RID: 8817
			private TaskAwaiter<bool[]> <>u__2;
		}

		// Token: 0x02000670 RID: 1648
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DashboardThemeItem_Tapped>d__10 : IAsyncStateMachine
		{
			// Token: 0x060038E2 RID: 14562 RVA: 0x002BF748 File Offset: 0x002BD948
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
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
						goto IL_012B;
					}
					default:
						taskAwaiter = welcomePage1V.UpdatePreview().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<DashboardThemeItem_Tapped>d__10>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(300).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<DashboardThemeItem_Tapped>d__10>(ref taskAwaiter, ref this);
						return;
					}
					IL_00D3:
					taskAwaiter.GetResult();
					taskAwaiter = welcomePage1V.UpdatePreview().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<DashboardThemeItem_Tapped>d__10>(ref taskAwaiter, ref this);
						return;
					}
					IL_012B:
					taskAwaiter.GetResult();
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

			// Token: 0x060038E3 RID: 14563 RVA: 0x002BF8D0 File Offset: 0x002BDAD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002272 RID: 8818
			public int <>1__state;

			// Token: 0x04002273 RID: 8819
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002274 RID: 8820
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002275 RID: 8821
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000671 RID: 1649
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DroidConnectionSettingsDetector>d__19 : IAsyncStateMachine
		{
			// Token: 0x060038E4 RID: 14564 RVA: 0x002BF8E0 File Offset: 0x002BDAE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_0095;
						}
						ConnectionDetector connectionDetector = new ConnectionDetector();
						connectionDetector.FinishedWithSuccess += delegate(object d, EventArgs args)
						{
							SharedSettings.Current.FirstConnectionAttempted = true;
						};
						taskAwaiter = connectionDetector.DetectAndSetSettings().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<DroidConnectionSettingsDetector>d__19>(ref taskAwaiter, ref this);
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

			// Token: 0x060038E5 RID: 14565 RVA: 0x002BF9C0 File Offset: 0x002BDBC0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002276 RID: 8822
			public int <>1__state;

			// Token: 0x04002277 RID: 8823
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002278 RID: 8824
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000672 RID: 1650
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ProfileSelector_NextRequested>d__15 : IAsyncStateMachine
		{
			// Token: 0x060038E6 RID: 14566 RVA: 0x002BF9D0 File Offset: 0x002BDBD0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						welcomePage1V.lbPageSubtitle.Text = Translate.GetString("Settings_Control_FuelFlowItem.Content");
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridFuel, welcomePage1V.gridProfileSelector).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<ProfileSelector_NextRequested>d__15>(ref taskAwaiter, ref this);
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
					welcomePage1V.progressBar.Progress = 0.8;
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

			// Token: 0x060038E7 RID: 14567 RVA: 0x002BFAB8 File Offset: 0x002BDCB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002279 RID: 8825
			public int <>1__state;

			// Token: 0x0400227A RID: 8826
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400227B RID: 8827
			public WelcomePage1V4 <>4__this;

			// Token: 0x0400227C RID: 8828
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000673 RID: 1651
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetEULARus>d__3 : IAsyncStateMachine
		{
			// Token: 0x060038E8 RID: 14568 RVA: 0x002BFAC8 File Offset: 0x002BDCC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						welcomePage1V.eulaActivityFrame.IsVisible = true;
						welcomePage1V.lbEULA.Text = "";
						taskAwaiter = HttpDownloader.Get("https://ru.carscanner.info/eula_ru.txt", 10).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, WelcomePage1V4.<SetEULARus>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					if (!string.IsNullOrEmpty(result))
					{
						welcomePage1V.lbEULA.Text = result;
						welcomePage1V.eulaUpdated = true;
					}
					else
					{
						welcomePage1V.lbEULA.Text = Translate.GetString("rus_EULA");
					}
					welcomePage1V.eulaActivityFrame.IsVisible = false;
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

			// Token: 0x060038E9 RID: 14569 RVA: 0x002BFBE0 File Offset: 0x002BDDE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400227D RID: 8829
			public int <>1__state;

			// Token: 0x0400227E RID: 8830
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400227F RID: 8831
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002280 RID: 8832
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000674 RID: 1652
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdatePreview>d__12 : IAsyncStateMachine
		{
			// Token: 0x060038EA RID: 14570 RVA: 0x002BFBF0 File Offset: 0x002BDDF0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new WelcomePage1V4.<>c__DisplayClass12_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.pid = null;
						welcomePage1V.gridDashPreview.Children.Clear();
						taskAwaiter = Task.Run(delegate
						{
							try
							{
								CS$<>8__locals1.pid = PIDWithFloatValueFormula.PID010C_EngineRPM();
								CS$<>8__locals1.<>4__this.dashItem = new DashboardItem();
								CS$<>8__locals1.<>4__this.dashItem.HeightRequest = 150.0;
								CS$<>8__locals1.<>4__this.dashItem.ItemType = DashboardItemTypes.Gauge;
								CS$<>8__locals1.<>4__this.dashItem.ShowAvg = false;
								CS$<>8__locals1.<>4__this.dashItem.Minimum = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.Maximum = 7000.0;
								CS$<>8__locals1.<>4__this.dashItem.GaugeShowRedLine = true;
								CS$<>8__locals1.<>4__this.dashItem.GaugeRedLineStart = 6000.0;
								CS$<>8__locals1.<>4__this.dashItem.GaugeRedLineFinish = 7000.0;
								CS$<>8__locals1.<>4__this.dashItem.HorizontalOptions = LayoutOptions.Center;
								CS$<>8__locals1.<>4__this.dashItem.Model = new LiveDataPIDModel();
								CS$<>8__locals1.<>4__this.dashItem.Model.SelectedPID = CS$<>8__locals1.pid;
								CS$<>8__locals1.<>4__this.dashItem.TitleFontSize = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.UnitsFontSize = 0.0;
								CS$<>8__locals1.<>4__this.dashItem.SelectAndAddControl();
							}
							catch (Exception)
							{
							}
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<UpdatePreview>d__12>(ref taskAwaiter, ref this);
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
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						try
						{
							CS$<>8__locals1.<>4__this.gridDashPreview.Children.Add(CS$<>8__locals1.<>4__this.dashItem);
							DashboardItem.ApplyThemeToItem(CS$<>8__locals1.<>4__this.dashItem);
							PIDWithFloatValueFormula pid = CS$<>8__locals1.pid;
							if (pid != null)
							{
								pid.SetValue(3000.0);
							}
						}
						catch (Exception)
						{
						}
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

			// Token: 0x060038EB RID: 14571 RVA: 0x002BFD10 File Offset: 0x002BDF10
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002281 RID: 8833
			public int <>1__state;

			// Token: 0x04002282 RID: 8834
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002283 RID: 8835
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002284 RID: 8836
			private WelcomePage1V4.<>c__DisplayClass12_0 <>8__1;

			// Token: 0x04002285 RID: 8837
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000675 RID: 1653
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WelcomePage1V3_SizeChanged>d__4 : IAsyncStateMachine
		{
			// Token: 0x060038EC RID: 14572 RVA: 0x002BFD20 File Offset: 0x002BDF20
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						orientation = DeviceDisplay.MainDisplayInfo.Orientation;
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<WelcomePage1V3_SizeChanged>d__4>(ref taskAwaiter, ref this);
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
					if (DeviceDisplay.MainDisplayInfo.Orientation == orientation)
					{
						if (DeviceDisplay.MainDisplayInfo.Orientation == 2)
						{
							NavigationPage.SetHasNavigationBar(welcomePage1V, false);
						}
						else
						{
							NavigationPage.SetHasNavigationBar(welcomePage1V, true);
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

			// Token: 0x060038ED RID: 14573 RVA: 0x002BFE24 File Offset: 0x002BE024
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002286 RID: 8838
			public int <>1__state;

			// Token: 0x04002287 RID: 8839
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002288 RID: 8840
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002289 RID: 8841
			private DisplayOrientation <orientation>5__2;

			// Token: 0x0400228A RID: 8842
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000676 RID: 1654
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnAdapterNext_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x060038EE RID: 14574 RVA: 0x002BFE34 File Offset: 0x002BE034
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						welcomePage1V.gridInterface.BindingContext = SharedSettings.Current;
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridInterface, welcomePage1V.gridAdapter).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<btnAdapterNext_Clicked>d__9>(ref taskAwaiter, ref this);
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
					welcomePage1V.Title = Translate.GetString("welcome_InitialSetup");
					welcomePage1V.lbPageSubtitle.Text = Translate.GetString("settings_Interface");
					welcomePage1V.lbPageTitleSeparator.Text = " | ";
					welcomePage1V.progressBar.Progress = 0.3;
					welcomePage1V.UpdatePreview();
					welcomePage1V.DroidConnectionSettingsDetector();
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

			// Token: 0x060038EF RID: 14575 RVA: 0x002BFF5C File Offset: 0x002BE15C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400228B RID: 8843
			public int <>1__state;

			// Token: 0x0400228C RID: 8844
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400228D RID: 8845
			public WelcomePage1V4 <>4__this;

			// Token: 0x0400228E RID: 8846
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000677 RID: 1655
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnAgree_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x060038F0 RID: 14576 RVA: 0x002BFF6C File Offset: 0x002BE16C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						SharedSettings.Current.FirstTimeLaunch = false;
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridAdapter, welcomePage1V.gridEULA).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<btnAgree_Clicked>d__6>(ref taskAwaiter, ref this);
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
					welcomePage1V.Title = Translate.GetString("welcome_Adapter");
					welcomePage1V.progressBar.IsVisible = true;
					OnlinePatch.GetPatch();
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

			// Token: 0x060038F1 RID: 14577 RVA: 0x002C0058 File Offset: 0x002BE258
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400228F RID: 8847
			public int <>1__state;

			// Token: 0x04002290 RID: 8848
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002291 RID: 8849
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002292 RID: 8850
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000678 RID: 1656
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnFuelNext_Clicked>d__17 : IAsyncStateMachine
		{
			// Token: 0x060038F2 RID: 14578 RVA: 0x002C0068 File Offset: 0x002BE268
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						RegionInfo regionInfo = null;
						try
						{
							regionInfo = RegionInfo.CurrentRegion;
						}
						catch (Exception)
						{
							regionInfo = null;
						}
						string[] array = new string[]
						{
							"AT", "BE", "BG", "CY", "CZ", "DE", "DK", "ES", "EE", "FI",
							"FR", "GB", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV",
							"MT", "NL", "PL", "PT", "RO", "SK", "SI", "SE", "UK", "US"
						};
						bool flag = false;
						bool adsProductPurchased = SharedSettings.Current.AdsProductPurchased;
						bool flag2 = regionInfo == null || array.Contains(regionInfo.TwoLetterISORegionName);
						bool isiOS = PlatformHelper.IsiOS;
						if (adsProductPurchased)
						{
							flag = false;
						}
						else if (isiOS && PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
						{
							flag = true;
							welcomePage1V.panelAdsSelection1.IsVisible = false;
							welcomePage1V.panelAdsSelection2.IsVisible = false;
						}
						else if (flag2)
						{
							flag = true;
						}
						if (PlatformHelper.AppMarket == Markets.RUS)
						{
							flag = false;
						}
						if (PlatformHelper.AppMarket == Markets.GooglePlay && !adsProductPurchased)
						{
							DependencyService.Get<IUMPConsent>(0).DisplayConsentIfRequired();
							flag = false;
						}
						if (!flag)
						{
							App.Instance.ChangeLanguage();
							goto IL_0271;
						}
						welcomePage1V.progressBar.Progress = 0.99;
						welcomePage1V.lbPageSubtitle.Text = Translate.GetString("welcome_Privacy");
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridPrivacy, welcomePage1V.gridFuel).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<btnFuelNext_Clicked>d__17>(ref taskAwaiter, ref this);
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
					if (PlatformHelper.IsiOS)
					{
						DependencyService.Get<IUMPConsent>(0).DisplayConsentIfRequired();
					}
					IL_0271:;
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

			// Token: 0x060038F3 RID: 14579 RVA: 0x002C0348 File Offset: 0x002BE548
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002293 RID: 8851
			public int <>1__state;

			// Token: 0x04002294 RID: 8852
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002295 RID: 8853
			public WelcomePage1V4 <>4__this;

			// Token: 0x04002296 RID: 8854
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000679 RID: 1657
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnInterfaceNext_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x060038F4 RID: 14580 RVA: 0x002C0358 File Offset: 0x002BE558
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridUnits, welcomePage1V.gridInterface).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<btnInterfaceNext_Clicked>d__13>(ref taskAwaiter, ref this);
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
					welcomePage1V.progressBar.Progress = 0.4;
					new Binding("TitleText", 2, null, null, null, null);
					welcomePage1V.lbPageSubtitle.Text = Translate.GetString("settings_Units");
					welcomePage1V.lbPageTitleSeparator.Text = " | ";
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

			// Token: 0x060038F5 RID: 14581 RVA: 0x002C0464 File Offset: 0x002BE664
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002297 RID: 8855
			public int <>1__state;

			// Token: 0x04002298 RID: 8856
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002299 RID: 8857
			public WelcomePage1V4 <>4__this;

			// Token: 0x0400229A RID: 8858
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200067A RID: 1658
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPrivacyNext_Clicked>d__18 : IAsyncStateMachine
		{
			// Token: 0x060038F6 RID: 14582 RVA: 0x002C0474 File Offset: 0x002BE674
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						welcomePage1V.btnPrivacyNext.IsEnabled = false;
						if (!PlatformHelper.IsiOS || !PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
						{
							goto IL_008D;
						}
						taskAwaiter = PlatformHelper.IOSService.TrackingRequestHelper_DisplayTrackingRequest().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, WelcomePage1V4.<btnPrivacyNext_Clicked>d__18>(ref taskAwaiter, ref this);
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
					taskAwaiter.GetResult();
					IL_008D:
					App.Instance.ChangeLanguage();
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

			// Token: 0x060038F7 RID: 14583 RVA: 0x002C0554 File Offset: 0x002BE754
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400229B RID: 8859
			public int <>1__state;

			// Token: 0x0400229C RID: 8860
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400229D RID: 8861
			public WelcomePage1V4 <>4__this;

			// Token: 0x0400229E RID: 8862
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200067B RID: 1659
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnUnitsNext_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x060038F8 RID: 14584 RVA: 0x002C0564 File Offset: 0x002BE764
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V4 welcomePage1V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = welcomePage1V.AnimateNextElement(welcomePage1V.gridProfileSelector, welcomePage1V.gridUnits).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage1V4.<btnUnitsNext_Clicked>d__14>(ref taskAwaiter, ref this);
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
					welcomePage1V.progressBar.Progress = 0.5;
					Binding binding = new Binding("TitleText", 2, null, null, null, null);
					welcomePage1V.lbPageSubtitle.BindingContext = welcomePage1V.profileSelector;
					welcomePage1V.lbPageSubtitle.SetBinding(Span.TextProperty, binding);
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

			// Token: 0x060038F9 RID: 14585 RVA: 0x002C0670 File Offset: 0x002BE870
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400229F RID: 8863
			public int <>1__state;

			// Token: 0x040022A0 RID: 8864
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022A1 RID: 8865
			public WelcomePage1V4 <>4__this;

			// Token: 0x040022A2 RID: 8866
			private TaskAwaiter <>u__1;
		}
	}
}
