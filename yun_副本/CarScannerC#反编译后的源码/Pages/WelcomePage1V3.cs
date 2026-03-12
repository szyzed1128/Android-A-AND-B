using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000683 RID: 1667
	[XamlCompilation(2)]
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml")]
	public class WelcomePage1V3 : ContentPage
	{
		// Token: 0x06003913 RID: 14611 RVA: 0x002C5F6C File Offset: 0x002C416C
		public WelcomePage1V3()
		{
			try
			{
				this.InitializeComponent();
				if (PlatformHelper.AppMarket == Markets.RUS)
				{
					base.Appearing += this.WelcomePage1V3_AppearingRUS;
				}
				switch (SharedSettings.Current.DashboardTheme)
				{
				case 0:
					this.btnDarkDash.IsChecked = new bool?(true);
					this.btnLightDash.IsChecked = new bool?(false);
					this.btnCarScannerDash.IsChecked = new bool?(false);
					break;
				case 1:
					this.btnDarkDash.IsChecked = new bool?(false);
					this.btnLightDash.IsChecked = new bool?(true);
					this.btnCarScannerDash.IsChecked = new bool?(false);
					break;
				case 2:
					this.btnDarkDash.IsChecked = new bool?(false);
					this.btnLightDash.IsChecked = new bool?(false);
					this.btnCarScannerDash.IsChecked = new bool?(true);
					break;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x002C6074 File Offset: 0x002C4274
		private void WelcomePage1V3_AppearingRUS(object sender, EventArgs e)
		{
			if (!this.eulaUpdated)
			{
				this.SetEULARus();
			}
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x002C6084 File Offset: 0x002C4284
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

		// Token: 0x06003916 RID: 14614 RVA: 0x002C60BC File Offset: 0x002C42BC
		private void WelcomePage1V3_SizeChanged(object sender, EventArgs e)
		{
			if (DeviceDisplay.MainDisplayInfo.Orientation == 2)
			{
				NavigationPage.SetHasNavigationBar(this, false);
				return;
			}
			NavigationPage.SetHasNavigationBar(this, true);
		}

		// Token: 0x06003917 RID: 14615 RVA: 0x002C60E8 File Offset: 0x002C42E8
		private void btnAgree_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.FirstTimeLaunch = false;
			this.gridEULA.IsVisible = false;
			this.gridAdapter.IsVisible = true;
			base.Title = Translate.GetString("welcome_Adapter");
			this.progressBar.IsVisible = true;
			OnlinePatch.GetPatch();
			if (PlatformHelper.IsAndroid)
			{
				ConnectionDetector connectionDetector = new ConnectionDetector();
				connectionDetector.FinishedWithSuccess += delegate(object d, EventArgs args)
				{
					SharedSettings.Current.FirstConnectionAttempted = true;
				};
				connectionDetector.DetectAndSetSettings();
			}
		}

		// Token: 0x06003918 RID: 14616 RVA: 0x002A9D6B File Offset: 0x002A7F6B
		private void btnDisagree_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.FirstTimeLaunch = true;
			PlatformHelper.CommonService.QuitApp();
		}

		// Token: 0x06003919 RID: 14617 RVA: 0x002C6174 File Offset: 0x002C4374
		private void btnAdapterNext_Clicked(object sender, EventArgs e)
		{
			this.gridAdapter.IsVisible = false;
			this.gridInterface.IsVisible = true;
			base.Title = Translate.GetString("welcome_InitialSetup");
			this.lbPageSubtitle.Text = Translate.GetString("SettingsPage_itemInterface.Content");
			this.lbPageTitleSeparator.Text = " | ";
			this.progressBar.Progress = 0.3;
		}

		// Token: 0x0600391A RID: 14618 RVA: 0x002C61E4 File Offset: 0x002C43E4
		private void btnInterfaceNext_Clicked(object sender, EventArgs e)
		{
			this.gridInterface.IsVisible = false;
			this.gridProfileSelector.IsVisible = true;
			this.progressBar.Progress = 0.5;
			Binding binding = new Binding("TitleText", 2, null, null, null, null);
			this.lbPageSubtitle.BindingContext = this.profileSelector;
			this.lbPageSubtitle.SetBinding(Span.TextProperty, binding);
		}

		// Token: 0x0600391B RID: 14619 RVA: 0x002C6250 File Offset: 0x002C4450
		private async void ProfileSelector_NextRequested(object sender, bool profileSelected)
		{
			this.lbPageSubtitle.Text = Translate.GetString("Settings_Control_FuelFlowItem.Content");
			this.gridProfileSelector.IsVisible = false;
			this.gridFuel.IsVisible = true;
			this.progressBar.Progress = 0.8;
		}

		// Token: 0x0600391C RID: 14620 RVA: 0x002C6288 File Offset: 0x002C4488
		private void btnFuelNext_Clicked(object sender, EventArgs e)
		{
			bool flag = false;
			bool flag2 = false;
			bool adsProductPurchased = SharedSettings.Current.AdsProductPurchased;
			try
			{
				RegionInfo currentRegion = RegionInfo.CurrentRegion;
				flag = new string[]
				{
					"AT", "BE", "BG", "CY", "CZ", "DE", "DK", "ES", "EE", "FI",
					"FR", "GB", "GR", "HR", "HU", "IE", "IT", "LT", "LU", "LV",
					"MT", "NL", "PL", "PT", "RO", "SK", "SI", "SE", "UK", "US"
				}.Contains(currentRegion.TwoLetterISORegionName);
			}
			catch (Exception)
			{
				flag = true;
			}
			if (adsProductPurchased)
			{
				flag2 = false;
			}
			else if (PlatformHelper.IsiOS && PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
			{
				flag2 = true;
				this.panelAdsSelection.IsVisible = false;
			}
			else if (flag)
			{
				flag2 = true;
			}
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				flag2 = false;
			}
			if (flag2)
			{
				this.progressBar.Progress = 0.99;
				this.lbPageSubtitle.Text = Translate.GetString("welcome_Privacy");
				this.gridFuel.IsVisible = false;
				this.gridPrivacy.IsVisible = true;
				return;
			}
			App.Instance.ChangeLanguage();
		}

		// Token: 0x0600391D RID: 14621 RVA: 0x002C6470 File Offset: 0x002C4670
		private async void btnPrivacyNext_Clicked(object sender, EventArgs e)
		{
			this.btnPrivacyNext.IsEnabled = false;
			if (PlatformHelper.IsiOS && PlatformHelper.IOSService.TrackingRequestHelper_ShouldRequestTracking())
			{
				await PlatformHelper.IOSService.TrackingRequestHelper_DisplayTrackingRequest();
			}
			App.Instance.ChangeLanguage();
		}

		// Token: 0x0600391E RID: 14622 RVA: 0x002C64A8 File Offset: 0x002C46A8
		private void RbTheme_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (this.btnDarkDash.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 0;
			}
			else if (this.btnLightDash.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 1;
			}
			else if (this.btnCarScannerDash.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.DashboardTheme = 2;
			}
			if (this.dashItem != null)
			{
				this.gridDashPreview.Children.Remove(this.dashItem);
			}
			this.dashItem = new DashboardItem();
			this.dashItem.HeightRequest = 50.0;
			this.dashItem.ItemType = DashboardItemTypes.Gauge;
			this.dashItem.ShowAvg = false;
			this.dashItem.Minimum = 0.0;
			this.dashItem.Maximum = 7000.0;
			this.dashItem.GaugeShowRedLine = true;
			this.dashItem.GaugeRedLineStart = 6000.0;
			this.dashItem.GaugeRedLineFinish = 7000.0;
			PIDWithFloatValueFormula pidwithFloatValueFormula = PIDWithFloatValueFormula.PID010C_EngineRPM();
			this.dashItem.Model = new LiveDataPIDModel();
			this.dashItem.Model.SelectedPID = pidwithFloatValueFormula;
			this.dashItem.TitleFontSize = 0.0;
			this.dashItem.UnitsFontSize = 0.0;
			this.dashItem.SelectAndAddControl();
			Grid.SetColumn(this.dashItem, 1);
			this.gridDashPreview.Children.Add(this.dashItem);
			DashboardItem.ApplyThemeToItem(this.dashItem);
		}

		// Token: 0x0600391F RID: 14623 RVA: 0x002C6654 File Offset: 0x002C4854
		private void numericEntry_SizeChanged(object sender, EventArgs e)
		{
			if (sender as Entry == this.entryL)
			{
				if (SharedSettings.Current.UseLitersForVolume)
				{
					this.entryL.Text = SharedSettings.Current.FuelPriceForLitre.ToString();
					return;
				}
				decimal num = (SharedSettings.Current.UseUSGallon ? 0.264172m : 0.2199692m);
				decimal num2 = SharedSettings.Current.FuelPriceForLitre / num;
				this.entryL.Text = num2.ToString("0.00");
			}
		}

		// Token: 0x06003920 RID: 14624 RVA: 0x002C66EC File Offset: 0x002C48EC
		private void numericEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = e.NewTextValue.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			Entry entry = sender as Entry;
			if (entry == this.entryL && SharedSettings.Current.UseLitersForVolume)
			{
				decimal num = 0m;
				if (e.NewTextValue == "" || e.NewTextValue == "-")
				{
					SharedSettings.Current.FuelPriceForLitre = 0m;
				}
				else if (decimal.TryParse(text, out num))
				{
					SharedSettings.Current.FuelPriceForLitre = num;
				}
				else
				{
					if (!decimal.TryParse(e.OldTextValue, out num))
					{
						num = 0m;
					}
					SharedSettings.Current.FuelPriceForLitre = num;
					entry.Text = e.OldTextValue;
				}
			}
			if (entry == this.entryL && !SharedSettings.Current.UseLitersForVolume)
			{
				decimal num2 = 0m;
				if (e.NewTextValue == "" || e.NewTextValue == "-")
				{
					SharedSettings.Current.FuelPriceForLitre = 0m;
					return;
				}
				if (decimal.TryParse(text, out num2))
				{
					decimal num3 = (SharedSettings.Current.UseUSGallon ? 0.264172m : 0.2199692m);
					decimal num4 = num2 * num3;
					SharedSettings.Current.FuelPriceForLitre = num4;
					return;
				}
				if (decimal.TryParse(e.OldTextValue, out num2))
				{
					decimal num5 = (SharedSettings.Current.UseUSGallon ? 0.264172m : 0.2199692m);
					decimal num6 = num2 * num5;
					SharedSettings.Current.FuelPriceForLitre = num6;
					entry.Text = e.OldTextValue;
					return;
				}
				num2 = 0m;
				SharedSettings.Current.FuelPriceForLitre = num2;
				entry.Text = "";
			}
		}

		// Token: 0x06003921 RID: 14625 RVA: 0x002C68F8 File Offset: 0x002C4AF8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(WelcomePage1V3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/WelcomePage1V3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			CustomFuelToTrueConverter customFuelToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(customFuelToTrueConverter = new CustomFuelToTrueConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 13);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 22);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 22);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 25);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 22);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 17);
			ProgressBar progressBar;
			VisualDiagnostics.RegisterSourceInfo(progressBar = new ProgressBar(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 14);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 22);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 22);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 32);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 29);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 22);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 18);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 14);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 22);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 22);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 26);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 29);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 26);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 29);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 29);
			HyperLinkLabel hyperLinkLabel;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel = new HyperLinkLabel(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 22);
			ScrollView scrollView2;
			VisualDiagnostics.RegisterSourceInfo(scrollView2 = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 21);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 14);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 22);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 29);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 29);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 36);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 30);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 33);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 33);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 33);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 33);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 33);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 33);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 30);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 33);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 33);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 33);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 33);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 33);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 30);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 44);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 100);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 34);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 26);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 32);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 34);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 37);
			SfRadioButton sfRadioButton3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton3 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 34);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 37);
			SfRadioButton sfRadioButton4;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton4 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 34);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 37);
			SfRadioButton sfRadioButton5;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton5 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 34);
			SfRadioGroup sfRadioGroup2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup2 = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 30);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 26);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 34);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 34);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 34);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 34);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 33);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 33);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 33);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 33);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 33);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 30);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 33);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 30);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 33);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 30);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 26);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 291, 29);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 26);
			List<string> fuelConsumptionUnits;
			VisualDiagnostics.RegisterSourceInfo(fuelConsumptionUnits = StaticLists.FuelConsumptionUnits, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 33);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 114);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 114);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 26);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 34);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 34);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 302, 34);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 303, 34);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 33);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 33);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 30);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 33);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 317, 33);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 30);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 323, 33);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 30);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 327, 33);
			Switch switch2;
			VisualDiagnostics.RegisterSourceInfo(switch2 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 324, 30);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 26);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 29);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 34);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 34);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 34);
			RowDefinition rowDefinition15;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition15 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 34);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 33);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 33);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 346, 30);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 357, 33);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 357, 33);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 33);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 30);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 33);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 30);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 368, 33);
			Switch switch3;
			VisualDiagnostics.RegisterSourceInfo(switch3 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 30);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 26);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 376, 34);
			ColumnDefinition columnDefinition10;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition10 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 377, 34);
			RowDefinition rowDefinition16;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition16 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 34);
			RowDefinition rowDefinition17;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition17 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 381, 34);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 387, 33);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 388, 33);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 30);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 394, 33);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 394, 33);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 395, 33);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 390, 30);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 33);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 398, 30);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 33);
			Switch switch4;
			VisualDiagnostics.RegisterSourceInfo(switch4 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 402, 30);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 374, 26);
			ColumnDefinition columnDefinition11;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition11 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 34);
			ColumnDefinition columnDefinition12;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition12 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 34);
			RowDefinition rowDefinition18;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition18 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 34);
			RowDefinition rowDefinition19;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition19 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 34);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 423, 33);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 33);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 419, 30);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 33);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 33);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 431, 33);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 426, 30);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 33);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 441, 33);
			Switch switch5;
			VisualDiagnostics.RegisterSourceInfo(switch5 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 438, 30);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 22);
			ScrollView scrollView3;
			VisualDiagnostics.RegisterSourceInfo(scrollView3 = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 18);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 449, 21);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 21);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 446, 18);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 14);
			RowDefinition rowDefinition20;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition20 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 461, 22);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 469, 21);
			ProfileSelectorV2 profileSelectorV;
			VisualDiagnostics.RegisterSourceInfo(profileSelectorV = new ProfileSelectorV2(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 18);
			Grid grid10;
			VisualDiagnostics.RegisterSourceInfo(grid10 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 456, 14);
			RowDefinition rowDefinition21;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition21 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 479, 22);
			RowDefinition rowDefinition22;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition22 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 480, 22);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 482, 29);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 32);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 26);
			List<string> fuelTypesList;
			VisualDiagnostics.RegisterSourceInfo(fuelTypesList = StaticLists.FuelTypesList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 33);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 33);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 107);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 26);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 488, 29);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 488, 29);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 36);
			Label label24;
			VisualDiagnostics.RegisterSourceInfo(label24 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 30);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 65);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 30);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 499, 36);
			Label label25;
			VisualDiagnostics.RegisterSourceInfo(label25 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 499, 30);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 500, 65);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 500, 30);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 486, 26);
			ColumnDefinition columnDefinition13;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition13 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 515, 34);
			ColumnDefinition columnDefinition14;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition14 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 516, 34);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 521, 33);
			Label label26;
			VisualDiagnostics.RegisterSourceInfo(label26 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 518, 30);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 528, 33);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 523, 30);
			Grid grid11;
			VisualDiagnostics.RegisterSourceInfo(grid11 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 513, 26);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 544, 29);
			Stepper stepper;
			VisualDiagnostics.RegisterSourceInfo(stepper = new Stepper(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 537, 26);
			ColumnDefinition columnDefinition15;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition15 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 551, 34);
			ColumnDefinition columnDefinition16;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition16 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 552, 34);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 557, 33);
			Label label27;
			VisualDiagnostics.RegisterSourceInfo(label27 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 554, 30);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 564, 33);
			NumericEntryV3 numericEntryV4;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV4 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 559, 30);
			Grid grid12;
			VisualDiagnostics.RegisterSourceInfo(grid12 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 549, 26);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 580, 29);
			Stepper stepper2;
			VisualDiagnostics.RegisterSourceInfo(stepper2 = new Stepper(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 573, 26);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 44);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 93);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 26);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 586, 36);
			Label label28;
			VisualDiagnostics.RegisterSourceInfo(label28 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 586, 30);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 588, 33);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 588, 33);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 590, 33);
			SfRadioButton sfRadioButton6;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton6 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 587, 30);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 592, 33);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 594, 33);
			SfRadioButton sfRadioButton7;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton7 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 591, 30);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 596, 33);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 597, 33);
			Label label29;
			VisualDiagnostics.RegisterSourceInfo(label29 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 595, 30);
			SfRadioGroup sfRadioGroup3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup3 = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 585, 26);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 605, 32);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 605, 74);
			Label label30;
			VisualDiagnostics.RegisterSourceInfo(label30 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 605, 26);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 32);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 32);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 126);
			Label label31;
			VisualDiagnostics.RegisterSourceInfo(label31 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 26);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 607, 26);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 612, 32);
			Label label32;
			VisualDiagnostics.RegisterSourceInfo(label32 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 612, 26);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 613, 32);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 613, 26);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 615, 44);
			Translate translate49;
			VisualDiagnostics.RegisterSourceInfo(translate49 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 615, 86);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 615, 26);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 483, 22);
			ScrollView scrollView4;
			VisualDiagnostics.RegisterSourceInfo(scrollView4 = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 482, 18);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 622, 21);
			Translate translate50;
			VisualDiagnostics.RegisterSourceInfo(translate50 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 624, 21);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 619, 18);
			Grid grid13;
			VisualDiagnostics.RegisterSourceInfo(grid13 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 474, 14);
			RowDefinition rowDefinition23;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition23 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 634, 22);
			RowDefinition rowDefinition24;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition24 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 635, 22);
			SharedSettings sharedSettings3;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings3 = SharedSettings.Current, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 637, 29);
			Translate translate51;
			VisualDiagnostics.RegisterSourceInfo(translate51 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 642, 32);
			Label label33;
			VisualDiagnostics.RegisterSourceInfo(label33 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 642, 26);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 645, 55);
			Translate translate52;
			VisualDiagnostics.RegisterSourceInfo(translate52 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 645, 112);
			SfRadioButton sfRadioButton8;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton8 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 645, 30);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 646, 55);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 646, 55);
			Translate translate53;
			VisualDiagnostics.RegisterSourceInfo(translate53 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 646, 164);
			SfRadioButton sfRadioButton9;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton9 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 646, 30);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 644, 26);
			Translate translate54;
			VisualDiagnostics.RegisterSourceInfo(translate54 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 653, 29);
			HyperLinkLabel hyperLinkLabel2;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel2 = new HyperLinkLabel(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 649, 26);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 638, 22);
			ScrollView scrollView5;
			VisualDiagnostics.RegisterSourceInfo(scrollView5 = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 637, 18);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 659, 21);
			Translate translate55;
			VisualDiagnostics.RegisterSourceInfo(translate55 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 661, 21);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 656, 18);
			Grid grid14;
			VisualDiagnostics.RegisterSourceInfo(grid14 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 629, 14);
			Grid grid15;
			VisualDiagnostics.RegisterSourceInfo(grid15 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\WelcomePage1V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("lbFormattedTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbFormattedTitle";
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
			nameScope.RegisterName("lbEULA", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "lbEULA";
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
			nameScope.RegisterName("iosELMLabel", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "iosELMLabel";
			}
			nameScope.RegisterName("droidELMLabel", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "droidELMLabel";
			}
			nameScope.RegisterName("btnAdapterNext", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnAdapterNext";
			}
			nameScope.RegisterName("gridInterface", grid9);
			if (grid9.StyleId == null)
			{
				grid9.StyleId = "gridInterface";
			}
			nameScope.RegisterName("interfaceThemeGroup", sfRadioGroup);
			if (sfRadioGroup.StyleId == null)
			{
				sfRadioGroup.StyleId = "interfaceThemeGroup";
			}
			nameScope.RegisterName("btnLight", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "btnLight";
			}
			nameScope.RegisterName("btnDark", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "btnDark";
			}
			nameScope.RegisterName("gridDashPreview", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "gridDashPreview";
			}
			nameScope.RegisterName("dashThemeGroup", sfRadioGroup2);
			if (sfRadioGroup2.StyleId == null)
			{
				sfRadioGroup2.StyleId = "dashThemeGroup";
			}
			nameScope.RegisterName("btnCarScannerDash", sfRadioButton3);
			if (sfRadioButton3.StyleId == null)
			{
				sfRadioButton3.StyleId = "btnCarScannerDash";
			}
			nameScope.RegisterName("btnLightDash", sfRadioButton4);
			if (sfRadioButton4.StyleId == null)
			{
				sfRadioButton4.StyleId = "btnLightDash";
			}
			nameScope.RegisterName("btnDarkDash", sfRadioButton5);
			if (sfRadioButton5.StyleId == null)
			{
				sfRadioButton5.StyleId = "btnDarkDash";
			}
			nameScope.RegisterName("btnInterfaceNext", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnInterfaceNext";
			}
			nameScope.RegisterName("gridProfileSelector", grid10);
			if (grid10.StyleId == null)
			{
				grid10.StyleId = "gridProfileSelector";
			}
			nameScope.RegisterName("profileSelector", profileSelectorV);
			if (profileSelectorV.StyleId == null)
			{
				profileSelectorV.StyleId = "profileSelector";
			}
			nameScope.RegisterName("gridFuel", grid13);
			if (grid13.StyleId == null)
			{
				grid13.StyleId = "gridFuel";
			}
			nameScope.RegisterName("panelCustomFuel", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "panelCustomFuel";
			}
			nameScope.RegisterName("alwaysRecordFuelGroup", sfRadioGroup3);
			if (sfRadioGroup3.StyleId == null)
			{
				sfRadioGroup3.StyleId = "alwaysRecordFuelGroup";
			}
			nameScope.RegisterName("entryL", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryL";
			}
			nameScope.RegisterName("btnFuelNext", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnFuelNext";
			}
			nameScope.RegisterName("gridPrivacy", grid14);
			if (grid14.StyleId == null)
			{
				grid14.StyleId = "gridPrivacy";
			}
			nameScope.RegisterName("panelAdsSelection", stackLayout6);
			if (stackLayout6.StyleId == null)
			{
				stackLayout6.StyleId = "panelAdsSelection";
			}
			nameScope.RegisterName("btnPrivacyNext", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnPrivacyNext";
			}
			this.page = this;
			this.lbFormattedTitle = label;
			this.lbPageTitle = span;
			this.lbPageTitleSeparator = span2;
			this.lbPageSubtitle = span3;
			this.progressBar = progressBar;
			this.gridEULA = grid;
			this.lbEULA = label2;
			this.eulaActivityFrame = activityFrame;
			this.gridAdapter = grid2;
			this.iosELMLabel = label3;
			this.droidELMLabel = label4;
			this.btnAdapterNext = button3;
			this.gridInterface = grid9;
			this.interfaceThemeGroup = sfRadioGroup;
			this.btnLight = sfRadioButton;
			this.btnDark = sfRadioButton2;
			this.gridDashPreview = grid3;
			this.dashThemeGroup = sfRadioGroup2;
			this.btnCarScannerDash = sfRadioButton3;
			this.btnLightDash = sfRadioButton4;
			this.btnDarkDash = sfRadioButton5;
			this.btnInterfaceNext = button4;
			this.gridProfileSelector = grid10;
			this.profileSelector = profileSelectorV;
			this.gridFuel = grid13;
			this.panelCustomFuel = stackLayout4;
			this.alwaysRecordFuelGroup = sfRadioGroup3;
			this.entryL = entry;
			this.btnFuelNext = button5;
			this.gridPrivacy = grid14;
			this.panelAdsSelection = stackLayout6;
			this.btnPrivacyNext = button6;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("CustomFuelToTrueConverter", customFuelToTrueConverter);
			translate.Text = "welcome_CarScanner";
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
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Application.EnableAccessibilityScalingForNamedFontSizesProperty, false);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
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
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SizeChanged += this.WelcomePage1V3_SizeChanged;
			this.Resources = resourceDictionary;
			label.SetValue(Label.LineBreakModeProperty, 1);
			dynamicResourceExtension2.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 2];
			array3[0] = label;
			array3[1] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 13)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = span;
			array4[1] = formattedString;
			array4[2] = label;
			array4[3] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Span.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 25)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			span.SetDynamicResource(Span.FontSizeProperty, dynamicResource3.Key);
			bindingExtension.Path = "Title";
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension5 = referenceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = bindingExtension;
			array5[1] = span;
			array5[2] = formattedString;
			array5[3] = label;
			array5[4] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 25)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension.Source = obj7;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, "");
			formattedString.Spans.Add(span2);
			span3.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension4.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = span3;
			array6[1] = formattedString;
			array6[2] = label;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Span.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 25)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			span3.SetDynamicResource(Span.FontSizeProperty, dynamicResource4.Key);
			span3.SetValue(Span.TextProperty, "");
			formattedString.Spans.Add(span3);
			label.SetValue(Label.FormattedTextProperty, formattedString);
			this.SetValue(NavigationPage.TitleViewProperty, label);
			grid15.SetValue(Grid.RowSpacingProperty, 0.0);
			onPlatform.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform.iOS = new Thickness(5.0, 5.0, 5.0, 5.0);
			grid15.SetValue(View.MarginProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid15.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid15.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			progressBar.SetValue(Grid.RowProperty, 0);
			progressBar.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			progressBar.SetValue(ProgressBar.ProgressProperty, 0.1);
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = progressBar;
			array7[1] = grid15;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, ProgressBar.ProgressColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			progressBar.SetDynamicResource(ProgressBar.ProgressColorProperty, dynamicResource5.Key);
			grid15.Children.Add(progressBar);
			grid.SetValue(Grid.RowProperty, 1);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension6.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = image;
			array8[1] = stackLayout;
			array8[2] = scrollView;
			array8[3] = grid;
			array8[4] = grid15;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Image.SourceProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 32)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource6.Key);
			stackLayout.Children.Add(image);
			label2.SetValue(Label.LineBreakModeProperty, 1);
			translate2.Text = "ios_EULA";
			IMarkupExtension markupExtension9 = translate2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = label2;
			array9[1] = stackLayout;
			array9[2] = scrollView;
			array9[3] = grid;
			array9[4] = grid15;
			array9[5] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 29)));
			object obj12 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label2.Text = obj12;
			stackLayout.Children.Add(label2);
			scrollView.Content = stackLayout;
			grid.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension7.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = button;
			array10[1] = grid;
			array10[2] = grid15;
			array10[3] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 21)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource7.Key);
			button.Clicked += this.btnAgree_Clicked;
			translate3.Text = "ios_AGREE";
			IMarkupExtension markupExtension11 = translate3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = button;
			array11[1] = grid;
			array11[2] = grid15;
			array11[3] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 21)));
			object obj15 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button.Text = obj15;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 2);
			button2.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			button2.Clicked += this.btnDisagree_Clicked;
			translate4.Text = "ios_DISAGREE";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = button2;
			array12[1] = grid;
			array12[2] = grid15;
			array12[3] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 21)));
			object obj17 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button2.Text = obj17;
			button2.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button2);
			grid15.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			scrollView2.SetValue(Grid.RowProperty, 0);
			scrollView2.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			image2.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("elm327.png"));
			stackLayout2.Children.Add(image2);
			label3.SetValue(Label.LineBreakModeProperty, 1);
			translate5.Text = "ios_OBDADAPTER";
			IMarkupExtension markupExtension13 = translate5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = label3;
			array13[1] = stackLayout2;
			array13[2] = scrollView2;
			array13[3] = grid2;
			array13[4] = grid15;
			array13[5] = this;
			object obj18;
			xamlServiceProvider13.Add(typeFromHandle25, obj18 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 29)));
			object obj19 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label3.Text = obj19;
			stackLayout2.Children.Add(label3);
			label4.SetValue(Label.LineBreakModeProperty, 1);
			translate6.Text = "droid_OBDADAPTER";
			IMarkupExtension markupExtension14 = translate6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = label4;
			array14[1] = stackLayout2;
			array14[2] = scrollView2;
			array14[3] = grid2;
			array14[4] = grid15;
			array14[5] = this;
			object obj20;
			xamlServiceProvider14.Add(typeFromHandle27, obj20 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 29)));
			object obj21 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label4.Text = obj21;
			stackLayout2.Children.Add(label4);
			hyperLinkLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel.SetValue(HyperLinkLabel.NavigateUriProperty, "http://carscanner.info/choosing-obdii-adapter/");
			translate7.Text = "ios_ReadMoreAtCarscannerInfo";
			IMarkupExtension markupExtension15 = translate7;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = hyperLinkLabel;
			array15[1] = stackLayout2;
			array15[2] = scrollView2;
			array15[3] = grid2;
			array15[4] = grid15;
			array15[5] = this;
			object obj22;
			xamlServiceProvider15.Add(typeFromHandle29, obj22 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 29)));
			object obj23 = markupExtension15.ProvideValue(xamlServiceProvider15);
			hyperLinkLabel.Text = obj23;
			stackLayout2.Children.Add(hyperLinkLabel);
			scrollView2.Content = stackLayout2;
			grid2.Children.Add(scrollView2);
			button3.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension8.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = button3;
			array16[1] = grid2;
			array16[2] = grid15;
			array16[3] = this;
			object obj24;
			xamlServiceProvider16.Add(typeFromHandle31, obj24 = new SimpleValueTargetProvider(array16, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 21)));
			DynamicResource dynamicResource8 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			button3.Clicked += this.btnAdapterNext_Clicked;
			translate8.Text = "ios_NEXT";
			IMarkupExtension markupExtension17 = translate8;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = button3;
			array17[1] = grid2;
			array17[2] = grid15;
			array17[3] = this;
			object obj25;
			xamlServiceProvider17.Add(typeFromHandle33, obj25 = new SimpleValueTargetProvider(array17, Button.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 21)));
			object obj26 = markupExtension17.ProvideValue(xamlServiceProvider17);
			button3.Text = obj26;
			button3.SetValue(Button.TextColorProperty, Color.White);
			grid2.Children.Add(button3);
			grid15.Children.Add(grid2);
			grid9.SetValue(Grid.RowProperty, 1);
			grid9.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			scrollView3.SetValue(Grid.RowProperty, 0);
			scrollView3.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			scrollView3.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "AutomaticallySwitchTheme";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase2);
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "AutomaticallySwitchThemeAvailable";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			translate9.Text = "ios_AutomaticallySwitchTheme";
			IMarkupExtension markupExtension18 = translate9;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = labelSwitch;
			array18[1] = stackLayout3;
			array18[2] = scrollView3;
			array18[3] = grid9;
			array18[4] = grid15;
			array18[5] = this;
			object obj27;
			xamlServiceProvider18.Add(typeFromHandle35, obj27 = new SimpleValueTargetProvider(array18, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 29)));
			object obj28 = markupExtension18.ProvideValue(xamlServiceProvider18);
			labelSwitch.Text = obj28;
			stackLayout3.Children.Add(labelSwitch);
			bindingExtension4.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension19 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 7];
			array19[0] = bindingExtension4;
			array19[1] = sfRadioGroup;
			array19[2] = stackLayout3;
			array19[3] = scrollView3;
			array19[4] = grid9;
			array19[5] = grid15;
			array19[6] = this;
			object obj29;
			xamlServiceProvider19.Add(typeFromHandle37, obj29 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 29)));
			object obj30 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension4.Converter = obj30;
			bindingExtension4.Path = "AutomaticallySwitchTheme";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			sfRadioGroup.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			translate10.Text = "ios_InterfaceTheme";
			IMarkupExtension markupExtension20 = translate10;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = label5;
			array20[1] = sfRadioGroup;
			array20[2] = stackLayout3;
			array20[3] = scrollView3;
			array20[4] = grid9;
			array20[5] = grid15;
			array20[6] = this;
			object obj31;
			xamlServiceProvider20.Add(typeFromHandle39, obj31 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 36)));
			object obj32 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label5.Text = obj32;
			sfRadioGroup.Children.Add(label5);
			dynamicResourceExtension9.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = sfRadioButton;
			array21[1] = sfRadioGroup;
			array21[2] = stackLayout3;
			array21[3] = scrollView3;
			array21[4] = grid9;
			array21[5] = grid15;
			array21[6] = this;
			object obj33;
			xamlServiceProvider21.Add(typeFromHandle41, obj33 = new SimpleValueTargetProvider(array21, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 33)));
			DynamicResource dynamicResource9 = markupExtension21.ProvideValue(xamlServiceProvider21);
			sfRadioButton.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource9.Key);
			bindingExtension5.Mode = 1;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension22 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 8];
			array22[0] = bindingExtension5;
			array22[1] = sfRadioButton;
			array22[2] = sfRadioGroup;
			array22[3] = stackLayout3;
			array22[4] = scrollView3;
			array22[5] = grid9;
			array22[6] = grid15;
			array22[7] = this;
			object obj34;
			xamlServiceProvider22.Add(typeFromHandle43, obj34 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(193, 33)));
			object obj35 = markupExtension22.ProvideValue(xamlServiceProvider22);
			bindingExtension5.Converter = obj35;
			bindingExtension5.Path = "DarkMode";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			sfRadioButton.SetBinding(ToggleButton.IsCheckedProperty, bindingBase5);
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate11.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension23 = translate11;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 7];
			array23[0] = sfRadioButton;
			array23[1] = sfRadioGroup;
			array23[2] = stackLayout3;
			array23[3] = scrollView3;
			array23[4] = grid9;
			array23[5] = grid15;
			array23[6] = this;
			object obj36;
			xamlServiceProvider23.Add(typeFromHandle45, obj36 = new SimpleValueTargetProvider(array23, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(195, 33)));
			object obj37 = markupExtension23.ProvideValue(xamlServiceProvider23);
			sfRadioButton.Text = obj37;
			dynamicResourceExtension10.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = sfRadioButton;
			array24[1] = sfRadioGroup;
			array24[2] = stackLayout3;
			array24[3] = scrollView3;
			array24[4] = grid9;
			array24[5] = grid15;
			array24[6] = this;
			object obj38;
			xamlServiceProvider24.Add(typeFromHandle47, obj38 = new SimpleValueTargetProvider(array24, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 33)));
			DynamicResource dynamicResource10 = markupExtension24.ProvideValue(xamlServiceProvider24);
			sfRadioButton.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource10.Key);
			dynamicResourceExtension11.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 7];
			array25[0] = sfRadioButton;
			array25[1] = sfRadioGroup;
			array25[2] = stackLayout3;
			array25[3] = scrollView3;
			array25[4] = grid9;
			array25[5] = grid15;
			array25[6] = this;
			object obj39;
			xamlServiceProvider25.Add(typeFromHandle49, obj39 = new SimpleValueTargetProvider(array25, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 33)));
			DynamicResource dynamicResource11 = markupExtension25.ProvideValue(xamlServiceProvider25);
			sfRadioButton.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource11.Key);
			sfRadioGroup.Children.Add(sfRadioButton);
			dynamicResourceExtension12.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 7];
			array26[0] = sfRadioButton2;
			array26[1] = sfRadioGroup;
			array26[2] = stackLayout3;
			array26[3] = scrollView3;
			array26[4] = grid9;
			array26[5] = grid15;
			array26[6] = this;
			object obj40;
			xamlServiceProvider26.Add(typeFromHandle51, obj40 = new SimpleValueTargetProvider(array26, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 33)));
			DynamicResource dynamicResource12 = markupExtension26.ProvideValue(xamlServiceProvider26);
			sfRadioButton2.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource12.Key);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "DarkMode";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			sfRadioButton2.SetBinding(ToggleButton.IsCheckedProperty, bindingBase6);
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate12.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension27 = translate12;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 7];
			array27[0] = sfRadioButton2;
			array27[1] = sfRadioGroup;
			array27[2] = stackLayout3;
			array27[3] = scrollView3;
			array27[4] = grid9;
			array27[5] = grid15;
			array27[6] = this;
			object obj41;
			xamlServiceProvider27.Add(typeFromHandle53, obj41 = new SimpleValueTargetProvider(array27, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 33)));
			object obj42 = markupExtension27.ProvideValue(xamlServiceProvider27);
			sfRadioButton2.Text = obj42;
			dynamicResourceExtension13.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 7];
			array28[0] = sfRadioButton2;
			array28[1] = sfRadioGroup;
			array28[2] = stackLayout3;
			array28[3] = scrollView3;
			array28[4] = grid9;
			array28[5] = grid15;
			array28[6] = this;
			object obj43;
			xamlServiceProvider28.Add(typeFromHandle55, obj43 = new SimpleValueTargetProvider(array28, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(204, 33)));
			DynamicResource dynamicResource13 = markupExtension28.ProvideValue(xamlServiceProvider28);
			sfRadioButton2.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource13.Key);
			dynamicResourceExtension14.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 7];
			array29[0] = sfRadioButton2;
			array29[1] = sfRadioGroup;
			array29[2] = stackLayout3;
			array29[3] = scrollView3;
			array29[4] = grid9;
			array29[5] = grid15;
			array29[6] = this;
			object obj44;
			xamlServiceProvider29.Add(typeFromHandle57, obj44 = new SimpleValueTargetProvider(array29, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 33)));
			DynamicResource dynamicResource14 = markupExtension29.ProvideValue(xamlServiceProvider29);
			sfRadioButton2.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource14.Key);
			sfRadioGroup.Children.Add(sfRadioButton2);
			stackLayout3.Children.Add(sfRadioGroup);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "AndroidRecolorNavBar";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase7);
			translate13.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension30 = translate13;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 6];
			array30[0] = labelSwitch2;
			array30[1] = stackLayout3;
			array30[2] = scrollView3;
			array30[3] = grid9;
			array30[4] = grid15;
			array30[5] = this;
			object obj45;
			xamlServiceProvider30.Add(typeFromHandle59, obj45 = new SimpleValueTargetProvider(array30, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(207, 100)));
			object obj46 = markupExtension30.ProvideValue(xamlServiceProvider30);
			labelSwitch2.Text = obj46;
			onPlatform2.Android = true;
			onPlatform2.iOS = false;
			labelSwitch2.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			stackLayout3.Children.Add(labelSwitch2);
			translate14.Text = "ios_DashboardThemeShort";
			IMarkupExtension markupExtension31 = translate14;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label6;
			array31[1] = stackLayout3;
			array31[2] = scrollView3;
			array31[3] = grid9;
			array31[4] = grid15;
			array31[5] = this;
			object obj47;
			xamlServiceProvider31.Add(typeFromHandle61, obj47 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(216, 32)));
			object obj48 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label6.Text = obj48;
			stackLayout3.Children.Add(label6);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			sfRadioGroup2.SetValue(Grid.ColumnProperty, 0);
			sfRadioGroup2.SetValue(StackLayout.OrientationProperty, 0);
			sfRadioButton3.StateChanged += this.RbTheme_StateChanged;
			translate15.Text = "ios_DashboardThemeCarScanner";
			IMarkupExtension markupExtension32 = translate15;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 8];
			array32[0] = sfRadioButton3;
			array32[1] = sfRadioGroup2;
			array32[2] = grid3;
			array32[3] = stackLayout3;
			array32[4] = scrollView3;
			array32[5] = grid9;
			array32[6] = grid15;
			array32[7] = this;
			object obj49;
			xamlServiceProvider32.Add(typeFromHandle63, obj49 = new SimpleValueTargetProvider(array32, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(229, 37)));
			object obj50 = markupExtension32.ProvideValue(xamlServiceProvider32);
			sfRadioButton3.Text = obj50;
			sfRadioGroup2.Children.Add(sfRadioButton3);
			sfRadioButton4.StateChanged += this.RbTheme_StateChanged;
			translate16.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension33 = translate16;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 8];
			array33[0] = sfRadioButton4;
			array33[1] = sfRadioGroup2;
			array33[2] = grid3;
			array33[3] = stackLayout3;
			array33[4] = scrollView3;
			array33[5] = grid9;
			array33[6] = grid15;
			array33[7] = this;
			object obj51;
			xamlServiceProvider33.Add(typeFromHandle65, obj51 = new SimpleValueTargetProvider(array33, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(233, 37)));
			object obj52 = markupExtension33.ProvideValue(xamlServiceProvider33);
			sfRadioButton4.Text = obj52;
			sfRadioGroup2.Children.Add(sfRadioButton4);
			sfRadioButton5.StateChanged += this.RbTheme_StateChanged;
			translate17.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension34 = translate17;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 8];
			array34[0] = sfRadioButton5;
			array34[1] = sfRadioGroup2;
			array34[2] = grid3;
			array34[3] = stackLayout3;
			array34[4] = scrollView3;
			array34[5] = grid9;
			array34[6] = grid15;
			array34[7] = this;
			object obj53;
			xamlServiceProvider34.Add(typeFromHandle67, obj53 = new SimpleValueTargetProvider(array34, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(237, 37)));
			object obj54 = markupExtension34.ProvideValue(xamlServiceProvider34);
			sfRadioButton5.Text = obj54;
			sfRadioGroup2.Children.Add(sfRadioButton5);
			grid3.Children.Add(sfRadioGroup2);
			stackLayout3.Children.Add(grid3);
			grid4.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid4.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			label7.SetValue(Grid.RowProperty, 1);
			label7.SetValue(Grid.ColumnProperty, 0);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "Use_km";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate18.Text = "Settings_Control_ToggleSpeedAndDistance.OnContent";
			IMarkupExtension markupExtension35 = translate18;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 7];
			array35[0] = label7;
			array35[1] = grid4;
			array35[2] = stackLayout3;
			array35[3] = scrollView3;
			array35[4] = grid9;
			array35[5] = grid15;
			array35[6] = this;
			object obj55;
			xamlServiceProvider35.Add(typeFromHandle69, obj55 = new SimpleValueTargetProvider(array35, Label.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(265, 33)));
			object obj56 = markupExtension35.ProvideValue(xamlServiceProvider35);
			label7.Text = obj56;
			label7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(label7);
			label8.SetValue(Grid.RowProperty, 1);
			label8.SetValue(Grid.ColumnProperty, 0);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension9.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension36 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 8];
			array36[0] = bindingExtension9;
			array36[1] = label8;
			array36[2] = grid4;
			array36[3] = stackLayout3;
			array36[4] = scrollView3;
			array36[5] = grid9;
			array36[6] = grid15;
			array36[7] = this;
			object obj57;
			xamlServiceProvider36.Add(typeFromHandle71, obj57 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(271, 33)));
			object obj58 = markupExtension36.ProvideValue(xamlServiceProvider36);
			bindingExtension9.Converter = obj58;
			bindingExtension9.Path = "Use_km";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label8.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate19.Text = "Settings_Control_ToggleSpeedAndDistance.OffContent";
			IMarkupExtension markupExtension37 = translate19;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 7];
			array37[0] = label8;
			array37[1] = grid4;
			array37[2] = stackLayout3;
			array37[3] = scrollView3;
			array37[4] = grid9;
			array37[5] = grid15;
			array37[6] = this;
			object obj59;
			xamlServiceProvider37.Add(typeFromHandle73, obj59 = new SimpleValueTargetProvider(array37, Label.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 33)));
			object obj60 = markupExtension37.ProvideValue(xamlServiceProvider37);
			label8.Text = obj60;
			label8.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(label8);
			label9.SetValue(Grid.RowProperty, 0);
			label9.SetValue(Grid.ColumnProperty, 0);
			translate20.Text = "Settings_Control_tbSpeedDistanceTemperatureVolume.Text";
			IMarkupExtension markupExtension38 = translate20;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 7];
			array38[0] = label9;
			array38[1] = grid4;
			array38[2] = stackLayout3;
			array38[3] = scrollView3;
			array38[4] = grid9;
			array38[5] = grid15;
			array38[6] = this;
			object obj61;
			xamlServiceProvider38.Add(typeFromHandle75, obj61 = new SimpleValueTargetProvider(array38, Label.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(278, 33)));
			object obj62 = markupExtension38.ProvideValue(xamlServiceProvider38);
			label9.Text = obj62;
			grid4.Children.Add(label9);
			@switch.SetValue(Grid.RowProperty, 1);
			@switch.SetValue(Grid.ColumnProperty, 1);
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "Use_km";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			@switch.SetBinding(Switch.IsToggledProperty, bindingBase10);
			grid4.Children.Add(@switch);
			stackLayout3.Children.Add(grid4);
			label10.SetValue(Grid.RowProperty, 0);
			label10.SetValue(Grid.ColumnProperty, 0);
			translate21.Text = "ios_UnitsForFuel";
			IMarkupExtension markupExtension39 = translate21;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 6];
			array39[0] = label10;
			array39[1] = stackLayout3;
			array39[2] = scrollView3;
			array39[3] = grid9;
			array39[4] = grid15;
			array39[5] = this;
			object obj63;
			xamlServiceProvider39.Add(typeFromHandle77, obj63 = new SimpleValueTargetProvider(array39, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(291, 29)));
			object obj64 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label10.Text = obj64;
			stackLayout3.Children.Add(label10);
			bindingExtension11.Source = fuelConsumptionUnits;
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase11);
			bindingExtension12.Mode = 1;
			staticResourceExtension4.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension40 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 7];
			array40[0] = bindingExtension12;
			array40[1] = picker;
			array40[2] = stackLayout3;
			array40[3] = scrollView3;
			array40[4] = grid9;
			array40[5] = grid15;
			array40[6] = this;
			object obj65;
			xamlServiceProvider40.Add(typeFromHandle79, obj65 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(292, 114)));
			object obj66 = markupExtension40.ProvideValue(xamlServiceProvider40);
			bindingExtension12.Converter = obj66;
			bindingExtension12.Path = "FuelConsumptionUnit";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase12);
			stackLayout3.Children.Add(picker);
			grid5.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid5.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			label11.SetValue(Grid.RowProperty, 1);
			label11.SetValue(Grid.ColumnProperty, 0);
			label11.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "UseLitersForVolume";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label11.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			translate22.Text = "ios_Liters";
			IMarkupExtension markupExtension41 = translate22;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 7];
			array41[0] = label11;
			array41[1] = grid5;
			array41[2] = stackLayout3;
			array41[3] = scrollView3;
			array41[4] = grid9;
			array41[5] = grid15;
			array41[6] = this;
			object obj67;
			xamlServiceProvider41.Add(typeFromHandle81, obj67 = new SimpleValueTargetProvider(array41, Label.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(310, 33)));
			object obj68 = markupExtension41.ProvideValue(xamlServiceProvider41);
			label11.Text = obj68;
			label11.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(label11);
			label12.SetValue(Grid.RowProperty, 1);
			label12.SetValue(Grid.ColumnProperty, 0);
			label12.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension14.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension42 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 8];
			array42[0] = bindingExtension14;
			array42[1] = label12;
			array42[2] = grid5;
			array42[3] = stackLayout3;
			array42[4] = scrollView3;
			array42[5] = grid9;
			array42[6] = grid15;
			array42[7] = this;
			object obj69;
			xamlServiceProvider42.Add(typeFromHandle83, obj69 = new SimpleValueTargetProvider(array42, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 33)));
			object obj70 = markupExtension42.ProvideValue(xamlServiceProvider42);
			bindingExtension14.Converter = obj70;
			bindingExtension14.Path = "UseLitersForVolume";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			label12.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			translate23.Text = "ios_Gallons";
			IMarkupExtension markupExtension43 = translate23;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 7];
			array43[0] = label12;
			array43[1] = grid5;
			array43[2] = stackLayout3;
			array43[3] = scrollView3;
			array43[4] = grid9;
			array43[5] = grid15;
			array43[6] = this;
			object obj71;
			xamlServiceProvider43.Add(typeFromHandle85, obj71 = new SimpleValueTargetProvider(array43, Label.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(317, 33)));
			object obj72 = markupExtension43.ProvideValue(xamlServiceProvider43);
			label12.Text = obj72;
			label12.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(label12);
			label13.SetValue(Grid.RowProperty, 0);
			label13.SetValue(Grid.ColumnProperty, 0);
			translate24.Text = "ios_VolumeUnits";
			IMarkupExtension markupExtension44 = translate24;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 7];
			array44[0] = label13;
			array44[1] = grid5;
			array44[2] = stackLayout3;
			array44[3] = scrollView3;
			array44[4] = grid9;
			array44[5] = grid15;
			array44[6] = this;
			object obj73;
			xamlServiceProvider44.Add(typeFromHandle87, obj73 = new SimpleValueTargetProvider(array44, Label.TextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(323, 33)));
			object obj74 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label13.Text = obj74;
			grid5.Children.Add(label13);
			switch2.SetValue(Grid.RowProperty, 1);
			switch2.SetValue(Grid.ColumnProperty, 1);
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "UseLitersForVolume";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			switch2.SetBinding(Switch.IsToggledProperty, bindingBase15);
			grid5.Children.Add(switch2);
			stackLayout3.Children.Add(grid5);
			grid6.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			bindingExtension16.Path = "ShowUSGallonSelector";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			grid6.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			grid6.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			rowDefinition15.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition15);
			label14.SetValue(Grid.RowProperty, 1);
			label14.SetValue(Grid.ColumnProperty, 0);
			label14.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "UseUSGallon";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			label14.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			translate25.Text = "Settings_Control_ToggleUSGallon.OnContent";
			IMarkupExtension markupExtension45 = translate25;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 7];
			array45[0] = label14;
			array45[1] = grid6;
			array45[2] = stackLayout3;
			array45[3] = scrollView3;
			array45[4] = grid9;
			array45[5] = grid15;
			array45[6] = this;
			object obj75;
			xamlServiceProvider45.Add(typeFromHandle89, obj75 = new SimpleValueTargetProvider(array45, Label.TextProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(351, 33)));
			object obj76 = markupExtension45.ProvideValue(xamlServiceProvider45);
			label14.Text = obj76;
			label14.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(label14);
			label15.SetValue(Grid.RowProperty, 1);
			label15.SetValue(Grid.ColumnProperty, 0);
			label15.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension18.Mode = 2;
			staticResourceExtension6.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension46 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 8];
			array46[0] = bindingExtension18;
			array46[1] = label15;
			array46[2] = grid6;
			array46[3] = stackLayout3;
			array46[4] = scrollView3;
			array46[5] = grid9;
			array46[6] = grid15;
			array46[7] = this;
			object obj77;
			xamlServiceProvider46.Add(typeFromHandle91, obj77 = new SimpleValueTargetProvider(array46, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(357, 33)));
			object obj78 = markupExtension46.ProvideValue(xamlServiceProvider46);
			bindingExtension18.Converter = obj78;
			bindingExtension18.Path = "UseUSGallon";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label15.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
			translate26.Text = "Settings_Control_ToggleUSGallon.OffContent";
			IMarkupExtension markupExtension47 = translate26;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 7];
			array47[0] = label15;
			array47[1] = grid6;
			array47[2] = stackLayout3;
			array47[3] = scrollView3;
			array47[4] = grid9;
			array47[5] = grid15;
			array47[6] = this;
			object obj79;
			xamlServiceProvider47.Add(typeFromHandle93, obj79 = new SimpleValueTargetProvider(array47, Label.TextProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(358, 33)));
			object obj80 = markupExtension47.ProvideValue(xamlServiceProvider47);
			label15.Text = obj80;
			label15.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(label15);
			label16.SetValue(Grid.RowProperty, 0);
			label16.SetValue(Grid.ColumnProperty, 0);
			translate27.Text = "Settings_Control_ToggleUSGallon.Header";
			IMarkupExtension markupExtension48 = translate27;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 7];
			array48[0] = label16;
			array48[1] = grid6;
			array48[2] = stackLayout3;
			array48[3] = scrollView3;
			array48[4] = grid9;
			array48[5] = grid15;
			array48[6] = this;
			object obj81;
			xamlServiceProvider48.Add(typeFromHandle95, obj81 = new SimpleValueTargetProvider(array48, Label.TextProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(364, 33)));
			object obj82 = markupExtension48.ProvideValue(xamlServiceProvider48);
			label16.Text = obj82;
			grid6.Children.Add(label16);
			switch3.SetValue(Grid.RowProperty, 1);
			switch3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "UseUSGallon";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			switch3.SetBinding(Switch.IsToggledProperty, bindingBase19);
			grid6.Children.Add(switch3);
			stackLayout3.Children.Add(grid6);
			grid7.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid7.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid7.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
			columnDefinition10.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition10);
			rowDefinition16.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition16);
			rowDefinition17.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition17);
			label17.SetValue(Grid.RowProperty, 1);
			label17.SetValue(Grid.ColumnProperty, 0);
			label17.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "Pressure_use_kpa";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label17.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
			translate28.Text = "Settings_Control_TogglePressureUnits.OnContent";
			IMarkupExtension markupExtension49 = translate28;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 7];
			array49[0] = label17;
			array49[1] = grid7;
			array49[2] = stackLayout3;
			array49[3] = scrollView3;
			array49[4] = grid9;
			array49[5] = grid15;
			array49[6] = this;
			object obj83;
			xamlServiceProvider49.Add(typeFromHandle97, obj83 = new SimpleValueTargetProvider(array49, Label.TextProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(388, 33)));
			object obj84 = markupExtension49.ProvideValue(xamlServiceProvider49);
			label17.Text = obj84;
			label17.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid7.Children.Add(label17);
			label18.SetValue(Grid.RowProperty, 1);
			label18.SetValue(Grid.ColumnProperty, 0);
			label18.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension21.Mode = 2;
			staticResourceExtension7.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension50 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 8];
			array50[0] = bindingExtension21;
			array50[1] = label18;
			array50[2] = grid7;
			array50[3] = stackLayout3;
			array50[4] = scrollView3;
			array50[5] = grid9;
			array50[6] = grid15;
			array50[7] = this;
			object obj85;
			xamlServiceProvider50.Add(typeFromHandle99, obj85 = new SimpleValueTargetProvider(array50, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(394, 33)));
			object obj86 = markupExtension50.ProvideValue(xamlServiceProvider50);
			bindingExtension21.Converter = obj86;
			bindingExtension21.Path = "Pressure_use_kpa";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			label18.SetBinding(VisualElement.IsVisibleProperty, bindingBase21);
			translate29.Text = "Settings_Control_TogglePressureUnits.OffContent";
			IMarkupExtension markupExtension51 = translate29;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 7];
			array51[0] = label18;
			array51[1] = grid7;
			array51[2] = stackLayout3;
			array51[3] = scrollView3;
			array51[4] = grid9;
			array51[5] = grid15;
			array51[6] = this;
			object obj87;
			xamlServiceProvider51.Add(typeFromHandle101, obj87 = new SimpleValueTargetProvider(array51, Label.TextProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(395, 33)));
			object obj88 = markupExtension51.ProvideValue(xamlServiceProvider51);
			label18.Text = obj88;
			label18.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid7.Children.Add(label18);
			label19.SetValue(Grid.RowProperty, 0);
			label19.SetValue(Grid.ColumnProperty, 0);
			translate30.Text = "Settings_Control_TogglePressureUnits.Header";
			IMarkupExtension markupExtension52 = translate30;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 7];
			array52[0] = label19;
			array52[1] = grid7;
			array52[2] = stackLayout3;
			array52[3] = scrollView3;
			array52[4] = grid9;
			array52[5] = grid15;
			array52[6] = this;
			object obj89;
			xamlServiceProvider52.Add(typeFromHandle103, obj89 = new SimpleValueTargetProvider(array52, Label.TextProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver52.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(401, 33)));
			object obj90 = markupExtension52.ProvideValue(xamlServiceProvider52);
			label19.Text = obj90;
			grid7.Children.Add(label19);
			switch4.SetValue(Grid.RowProperty, 1);
			switch4.SetValue(Grid.ColumnProperty, 1);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "Pressure_use_kpa";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			switch4.SetBinding(Switch.IsToggledProperty, bindingBase22);
			grid7.Children.Add(switch4);
			stackLayout3.Children.Add(grid7);
			grid8.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid8.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition11.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition11);
			columnDefinition12.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition12);
			rowDefinition18.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition18);
			rowDefinition19.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition19);
			label20.SetValue(Grid.RowProperty, 1);
			label20.SetValue(Grid.ColumnProperty, 0);
			label20.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "Use_celcium";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label20.SetBinding(VisualElement.IsVisibleProperty, bindingBase23);
			translate31.Text = "Settings_Control_ToggleTemperatureUnits.OnContent";
			IMarkupExtension markupExtension53 = translate31;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 7];
			array53[0] = label20;
			array53[1] = grid8;
			array53[2] = stackLayout3;
			array53[3] = scrollView3;
			array53[4] = grid9;
			array53[5] = grid15;
			array53[6] = this;
			object obj91;
			xamlServiceProvider53.Add(typeFromHandle105, obj91 = new SimpleValueTargetProvider(array53, Label.TextProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver53.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(424, 33)));
			object obj92 = markupExtension53.ProvideValue(xamlServiceProvider53);
			label20.Text = obj92;
			label20.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid8.Children.Add(label20);
			label21.SetValue(Grid.RowProperty, 1);
			label21.SetValue(Grid.ColumnProperty, 0);
			label21.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension24.Mode = 2;
			staticResourceExtension8.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension54 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 8];
			array54[0] = bindingExtension24;
			array54[1] = label21;
			array54[2] = grid8;
			array54[3] = stackLayout3;
			array54[4] = scrollView3;
			array54[5] = grid9;
			array54[6] = grid15;
			array54[7] = this;
			object obj93;
			xamlServiceProvider54.Add(typeFromHandle107, obj93 = new SimpleValueTargetProvider(array54, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj93);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver54.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(430, 33)));
			object obj94 = markupExtension54.ProvideValue(xamlServiceProvider54);
			bindingExtension24.Converter = obj94;
			bindingExtension24.Path = "Use_celcium";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			label21.SetBinding(VisualElement.IsVisibleProperty, bindingBase24);
			translate32.Text = "Settings_Control_ToggleTemperatureUnits.OffContent";
			IMarkupExtension markupExtension55 = translate32;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 7];
			array55[0] = label21;
			array55[1] = grid8;
			array55[2] = stackLayout3;
			array55[3] = scrollView3;
			array55[4] = grid9;
			array55[5] = grid15;
			array55[6] = this;
			object obj95;
			xamlServiceProvider55.Add(typeFromHandle109, obj95 = new SimpleValueTargetProvider(array55, Label.TextProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj95);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver55.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(431, 33)));
			object obj96 = markupExtension55.ProvideValue(xamlServiceProvider55);
			label21.Text = obj96;
			label21.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid8.Children.Add(label21);
			label22.SetValue(Grid.RowProperty, 0);
			label22.SetValue(Grid.ColumnProperty, 0);
			translate33.Text = "Settings_Control_ToggleTemperatureUnits.Header";
			IMarkupExtension markupExtension56 = translate33;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 7];
			array56[0] = label22;
			array56[1] = grid8;
			array56[2] = stackLayout3;
			array56[3] = scrollView3;
			array56[4] = grid9;
			array56[5] = grid15;
			array56[6] = this;
			object obj97;
			xamlServiceProvider56.Add(typeFromHandle111, obj97 = new SimpleValueTargetProvider(array56, Label.TextProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj97);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver56.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(437, 33)));
			object obj98 = markupExtension56.ProvideValue(xamlServiceProvider56);
			label22.Text = obj98;
			grid8.Children.Add(label22);
			switch5.SetValue(Grid.RowProperty, 1);
			switch5.SetValue(Grid.ColumnProperty, 1);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "Use_celcium";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			switch5.SetBinding(Switch.IsToggledProperty, bindingBase25);
			grid8.Children.Add(switch5);
			stackLayout3.Children.Add(grid8);
			scrollView3.Content = stackLayout3;
			grid9.Children.Add(scrollView3);
			button4.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension15.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension57 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 4];
			array57[0] = button4;
			array57[1] = grid9;
			array57[2] = grid15;
			array57[3] = this;
			object obj99;
			xamlServiceProvider57.Add(typeFromHandle113, obj99 = new SimpleValueTargetProvider(array57, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj99);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver57.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(449, 21)));
			DynamicResource dynamicResource15 = markupExtension57.ProvideValue(xamlServiceProvider57);
			button4.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource15.Key);
			button4.Clicked += this.btnInterfaceNext_Clicked;
			translate34.Text = "ios_NEXT";
			IMarkupExtension markupExtension58 = translate34;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 4];
			array58[0] = button4;
			array58[1] = grid9;
			array58[2] = grid15;
			array58[3] = this;
			object obj100;
			xamlServiceProvider58.Add(typeFromHandle115, obj100 = new SimpleValueTargetProvider(array58, Button.TextProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj100);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver58.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(451, 21)));
			object obj101 = markupExtension58.ProvideValue(xamlServiceProvider58);
			button4.Text = obj101;
			button4.SetValue(Button.TextColorProperty, Color.White);
			grid9.Children.Add(button4);
			grid15.Children.Add(grid9);
			grid10.SetValue(Grid.RowProperty, 1);
			grid10.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			rowDefinition20.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition20);
			profileSelectorV.SetValue(Grid.RowProperty, 0);
			profileSelectorV.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			profileSelectorV.SetValue(ProfileSelectorV2.BackButtonVisibleProperty, false);
			profileSelectorV.SetValue(ProfileSelectorV2.CreateBackItemProperty, true);
			translate35.Text = "ios_ChooseCarBrand";
			IMarkupExtension markupExtension59 = translate35;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 4];
			array59[0] = profileSelectorV;
			array59[1] = grid10;
			array59[2] = grid15;
			array59[3] = this;
			object obj102;
			xamlServiceProvider59.Add(typeFromHandle117, obj102 = new SimpleValueTargetProvider(array59, ProfileSelectorV2.TitleTextProperty, nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj102);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver59.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider59.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver59, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(469, 21)));
			object obj103 = markupExtension59.ProvideValue(xamlServiceProvider59);
			profileSelectorV.TitleText = obj103;
			profileSelectorV.SetValue(ProfileSelectorV2.TitleVisibleProperty, false);
			profileSelectorV.WindowCloseRequested += this.ProfileSelector_NextRequested;
			grid10.Children.Add(profileSelectorV);
			grid15.Children.Add(grid10);
			grid13.SetValue(Grid.RowProperty, 1);
			grid13.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition21.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid13.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition21);
			rowDefinition22.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid13.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition22);
			scrollView4.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			scrollView4.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			translate36.Text = "Settings_Control_tbVehicleFuel.Text";
			IMarkupExtension markupExtension60 = translate36;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle119 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 6];
			array60[0] = label23;
			array60[1] = stackLayout5;
			array60[2] = scrollView4;
			array60[3] = grid13;
			array60[4] = grid15;
			array60[5] = this;
			object obj104;
			xamlServiceProvider60.Add(typeFromHandle119, obj104 = new SimpleValueTargetProvider(array60, Label.TextProperty, nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj104);
			Type typeFromHandle120 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver60.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider60.Add(typeFromHandle120, new XamlTypeResolver(xmlNamespaceResolver60, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(484, 32)));
			object obj105 = markupExtension60.ProvideValue(xamlServiceProvider60);
			label23.Text = obj105;
			stackLayout5.Children.Add(label23);
			bindingExtension26.Source = fuelTypesList;
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			picker2.SetBinding(Picker.ItemsSourceProperty, bindingBase26);
			bindingExtension27.Mode = 1;
			bindingExtension27.Path = "FuelTypeIdx";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase27);
			stackLayout5.Children.Add(picker2);
			bindingExtension28.Mode = 2;
			staticResourceExtension9.Key = "CustomFuelToTrueConverter";
			IMarkupExtension markupExtension61 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle121 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 7];
			array61[0] = bindingExtension28;
			array61[1] = stackLayout4;
			array61[2] = stackLayout5;
			array61[3] = scrollView4;
			array61[4] = grid13;
			array61[5] = grid15;
			array61[6] = this;
			object obj106;
			xamlServiceProvider61.Add(typeFromHandle121, obj106 = new SimpleValueTargetProvider(array61, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj106);
			Type typeFromHandle122 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver61.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider61.Add(typeFromHandle122, new XamlTypeResolver(xmlNamespaceResolver61, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(488, 29)));
			object obj107 = markupExtension61.ProvideValue(xamlServiceProvider61);
			bindingExtension28.Converter = obj107;
			bindingExtension28.Path = "FuelType";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase28);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			translate37.Text = "ios_CustomFuelAF";
			IMarkupExtension markupExtension62 = translate37;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle123 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 7];
			array62[0] = label24;
			array62[1] = stackLayout4;
			array62[2] = stackLayout5;
			array62[3] = scrollView4;
			array62[4] = grid13;
			array62[5] = grid15;
			array62[6] = this;
			object obj108;
			xamlServiceProvider62.Add(typeFromHandle123, obj108 = new SimpleValueTargetProvider(array62, Label.TextProperty, nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj108);
			Type typeFromHandle124 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver62.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider62.Add(typeFromHandle124, new XamlTypeResolver(xmlNamespaceResolver62, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(490, 36)));
			object obj109 = markupExtension62.ProvideValue(xamlServiceProvider62);
			label24.Text = obj109;
			stackLayout4.Children.Add(label24);
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension29.Mode = 1;
			bindingExtension29.Path = "CustomFuelAF";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase29);
			stackLayout4.Children.Add(numericEntryV);
			translate38.Text = "ios_CustomFuelDensity";
			IMarkupExtension markupExtension63 = translate38;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle125 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 7];
			array63[0] = label25;
			array63[1] = stackLayout4;
			array63[2] = stackLayout5;
			array63[3] = scrollView4;
			array63[4] = grid13;
			array63[5] = grid15;
			array63[6] = this;
			object obj110;
			xamlServiceProvider63.Add(typeFromHandle125, obj110 = new SimpleValueTargetProvider(array63, Label.TextProperty, nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj110);
			Type typeFromHandle126 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver63.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider63.Add(typeFromHandle126, new XamlTypeResolver(xmlNamespaceResolver63, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(499, 36)));
			object obj111 = markupExtension63.ProvideValue(xamlServiceProvider63);
			label25.Text = obj111;
			stackLayout4.Children.Add(label25);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "CustomFuelDensity";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase30);
			stackLayout4.Children.Add(numericEntryV2);
			stackLayout5.Children.Add(stackLayout4);
			columnDefinition13.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition13);
			columnDefinition14.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition14);
			label26.SetValue(Grid.RowProperty, 0);
			label26.SetValue(Grid.ColumnProperty, 0);
			translate39.Text = "Settings_Control_tbEngineDisplacement.Text";
			IMarkupExtension markupExtension64 = translate39;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle127 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 7];
			array64[0] = label26;
			array64[1] = grid11;
			array64[2] = stackLayout5;
			array64[3] = scrollView4;
			array64[4] = grid13;
			array64[5] = grid15;
			array64[6] = this;
			object obj112;
			xamlServiceProvider64.Add(typeFromHandle127, obj112 = new SimpleValueTargetProvider(array64, Label.TextProperty, nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj112);
			Type typeFromHandle128 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver64.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider64.Add(typeFromHandle128, new XamlTypeResolver(xmlNamespaceResolver64, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(521, 33)));
			object obj113 = markupExtension64.ProvideValue(xamlServiceProvider64);
			label26.Text = obj113;
			label26.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid11.Children.Add(label26);
			numericEntryV3.SetValue(Grid.RowProperty, 0);
			numericEntryV3.SetValue(Grid.ColumnProperty, 1);
			numericEntryV3.SetValue(NumericEntryV3.DoubleFormatProperty, "0.0");
			numericEntryV3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			bindingExtension31.Mode = 1;
			bindingExtension31.Path = "EngineDisplacement";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase31);
			grid11.Children.Add(numericEntryV3);
			stackLayout5.Children.Add(grid11);
			stepper.SetValue(Grid.RowProperty, 1);
			stepper.SetValue(Grid.ColumnProperty, 1);
			stepper.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
			stepper.SetValue(Stepper.IncrementProperty, 0.1);
			stepper.SetValue(Stepper.MaximumProperty, 20.0);
			stepper.SetValue(Stepper.MinimumProperty, 0.1);
			bindingExtension32.Mode = 1;
			bindingExtension32.Path = "EngineDisplacement";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			stepper.SetBinding(Stepper.ValueProperty, bindingBase32);
			stackLayout5.Children.Add(stepper);
			columnDefinition15.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition15);
			columnDefinition16.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition16);
			label27.SetValue(Grid.RowProperty, 0);
			label27.SetValue(Grid.ColumnProperty, 0);
			translate40.Text = "Settings_Control_tbNumberOfCylinders.Text";
			IMarkupExtension markupExtension65 = translate40;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle129 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 7];
			array65[0] = label27;
			array65[1] = grid12;
			array65[2] = stackLayout5;
			array65[3] = scrollView4;
			array65[4] = grid13;
			array65[5] = grid15;
			array65[6] = this;
			object obj114;
			xamlServiceProvider65.Add(typeFromHandle129, obj114 = new SimpleValueTargetProvider(array65, Label.TextProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj114);
			Type typeFromHandle130 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver65.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider65.Add(typeFromHandle130, new XamlTypeResolver(xmlNamespaceResolver65, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(557, 33)));
			object obj115 = markupExtension65.ProvideValue(xamlServiceProvider65);
			label27.Text = obj115;
			label27.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid12.Children.Add(label27);
			numericEntryV4.SetValue(Grid.RowProperty, 0);
			numericEntryV4.SetValue(Grid.ColumnProperty, 1);
			numericEntryV4.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "EngineCylinders";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			numericEntryV4.SetBinding(NumericEntryV3.ValueProperty, bindingBase33);
			grid12.Children.Add(numericEntryV4);
			stackLayout5.Children.Add(grid12);
			stepper2.SetValue(Grid.RowProperty, 1);
			stepper2.SetValue(Grid.ColumnProperty, 1);
			stepper2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
			stepper2.SetValue(Stepper.IncrementProperty, 1.0);
			stepper2.SetValue(Stepper.MaximumProperty, 24.0);
			stepper2.SetValue(Stepper.MinimumProperty, 1.0);
			bindingExtension34.Mode = 1;
			bindingExtension34.Path = "EngineCylinders";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			stepper2.SetBinding(Stepper.ValueProperty, bindingBase34);
			stackLayout5.Children.Add(stepper2);
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "FuelHybridCar";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase35);
			translate41.Text = "sett_FuelHybridCar";
			IMarkupExtension markupExtension66 = translate41;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle131 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 6];
			array66[0] = labelSwitch3;
			array66[1] = stackLayout5;
			array66[2] = scrollView4;
			array66[3] = grid13;
			array66[4] = grid15;
			array66[5] = this;
			object obj116;
			xamlServiceProvider66.Add(typeFromHandle131, obj116 = new SimpleValueTargetProvider(array66, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj116);
			Type typeFromHandle132 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver66.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider66.Add(typeFromHandle132, new XamlTypeResolver(xmlNamespaceResolver66, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(582, 93)));
			object obj117 = markupExtension66.ProvideValue(xamlServiceProvider66);
			labelSwitch3.Text = obj117;
			stackLayout5.Children.Add(labelSwitch3);
			sfRadioGroup3.SetValue(StackLayout.OrientationProperty, 0);
			translate42.Text = "ios_Image8";
			IMarkupExtension markupExtension67 = translate42;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle133 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 7];
			array67[0] = label28;
			array67[1] = sfRadioGroup3;
			array67[2] = stackLayout5;
			array67[3] = scrollView4;
			array67[4] = grid13;
			array67[5] = grid15;
			array67[6] = this;
			object obj118;
			xamlServiceProvider67.Add(typeFromHandle133, obj118 = new SimpleValueTargetProvider(array67, Label.TextProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj118);
			Type typeFromHandle134 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver67.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider67.Add(typeFromHandle134, new XamlTypeResolver(xmlNamespaceResolver67, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(586, 36)));
			object obj119 = markupExtension67.ProvideValue(xamlServiceProvider67);
			label28.Text = obj119;
			sfRadioGroup3.Children.Add(label28);
			bindingExtension36.Mode = 1;
			staticResourceExtension10.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension68 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle135 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 8];
			array68[0] = bindingExtension36;
			array68[1] = sfRadioButton6;
			array68[2] = sfRadioGroup3;
			array68[3] = stackLayout5;
			array68[4] = scrollView4;
			array68[5] = grid13;
			array68[6] = grid15;
			array68[7] = this;
			object obj120;
			xamlServiceProvider68.Add(typeFromHandle135, obj120 = new SimpleValueTargetProvider(array68, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj120);
			Type typeFromHandle136 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver68.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider68.Add(typeFromHandle136, new XamlTypeResolver(xmlNamespaceResolver68, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(588, 33)));
			object obj121 = markupExtension68.ProvideValue(xamlServiceProvider68);
			bindingExtension36.Converter = obj121;
			bindingExtension36.Path = "AlwaysRecordFuelConsumption";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			sfRadioButton6.SetBinding(ToggleButton.IsCheckedProperty, bindingBase36);
			sfRadioButton6.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate43.Text = "ios_RecordFuelConsumptionWhenSelected";
			IMarkupExtension markupExtension69 = translate43;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle137 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 7];
			array69[0] = sfRadioButton6;
			array69[1] = sfRadioGroup3;
			array69[2] = stackLayout5;
			array69[3] = scrollView4;
			array69[4] = grid13;
			array69[5] = grid15;
			array69[6] = this;
			object obj122;
			xamlServiceProvider69.Add(typeFromHandle137, obj122 = new SimpleValueTargetProvider(array69, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj122);
			Type typeFromHandle138 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver69.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider69.Add(typeFromHandle138, new XamlTypeResolver(xmlNamespaceResolver69, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(590, 33)));
			object obj123 = markupExtension69.ProvideValue(xamlServiceProvider69);
			sfRadioButton6.Text = obj123;
			sfRadioGroup3.Children.Add(sfRadioButton6);
			bindingExtension37.Mode = 1;
			bindingExtension37.Path = "AlwaysRecordFuelConsumption";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			sfRadioButton7.SetBinding(ToggleButton.IsCheckedProperty, bindingBase37);
			sfRadioButton7.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate44.Text = "Settings_tbAlwaysRecordFuel.Text";
			IMarkupExtension markupExtension70 = translate44;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle139 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 7];
			array70[0] = sfRadioButton7;
			array70[1] = sfRadioGroup3;
			array70[2] = stackLayout5;
			array70[3] = scrollView4;
			array70[4] = grid13;
			array70[5] = grid15;
			array70[6] = this;
			object obj124;
			xamlServiceProvider70.Add(typeFromHandle139, obj124 = new SimpleValueTargetProvider(array70, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj124);
			Type typeFromHandle140 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver70.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider70.Add(typeFromHandle140, new XamlTypeResolver(xmlNamespaceResolver70, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(594, 33)));
			object obj125 = markupExtension70.ProvideValue(xamlServiceProvider70);
			sfRadioButton7.Text = obj125;
			sfRadioGroup3.Children.Add(sfRadioButton7);
			bindingExtension38.Path = "AlwaysRecordFuelConsumption";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			label29.SetBinding(VisualElement.IsVisibleProperty, bindingBase38);
			translate45.Text = "Settings_tbAlwaysRecordFuelWarning.Text";
			IMarkupExtension markupExtension71 = translate45;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle141 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 7];
			array71[0] = label29;
			array71[1] = sfRadioGroup3;
			array71[2] = stackLayout5;
			array71[3] = scrollView4;
			array71[4] = grid13;
			array71[5] = grid15;
			array71[6] = this;
			object obj126;
			xamlServiceProvider71.Add(typeFromHandle141, obj126 = new SimpleValueTargetProvider(array71, Label.TextProperty, nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj126);
			Type typeFromHandle142 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver71.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider71.Add(typeFromHandle142, new XamlTypeResolver(xmlNamespaceResolver71, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(597, 33)));
			object obj127 = markupExtension71.ProvideValue(xamlServiceProvider71);
			label29.Text = obj127;
			label29.SetValue(Label.TextColorProperty, Color.Red);
			sfRadioGroup3.Children.Add(label29);
			stackLayout5.Children.Add(sfRadioGroup3);
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "Use_km";
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			label30.SetBinding(VisualElement.IsVisibleProperty, bindingBase39);
			translate46.Text = "Settings_tbFuelPriceL.Text";
			IMarkupExtension markupExtension72 = translate46;
			XamlServiceProvider xamlServiceProvider72 = new XamlServiceProvider();
			Type typeFromHandle143 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 6];
			array72[0] = label30;
			array72[1] = stackLayout5;
			array72[2] = scrollView4;
			array72[3] = grid13;
			array72[4] = grid15;
			array72[5] = this;
			object obj128;
			xamlServiceProvider72.Add(typeFromHandle143, obj128 = new SimpleValueTargetProvider(array72, Label.TextProperty, nameScope));
			xamlServiceProvider72.Add(typeof(IReferenceProvider), obj128);
			Type typeFromHandle144 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver72 = new XmlNamespaceResolver();
			xmlNamespaceResolver72.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver72.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver72.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver72.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver72.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver72.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver72.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver72.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider72.Add(typeFromHandle144, new XamlTypeResolver(xmlNamespaceResolver72, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider72.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(605, 74)));
			object obj129 = markupExtension72.ProvideValue(xamlServiceProvider72);
			label30.Text = obj129;
			stackLayout5.Children.Add(label30);
			bindingExtension40.Mode = 2;
			staticResourceExtension11.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension73 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider73 = new XamlServiceProvider();
			Type typeFromHandle145 = typeof(IProvideValueTarget);
			object[] array73 = new object[0 + 7];
			array73[0] = bindingExtension40;
			array73[1] = label31;
			array73[2] = stackLayout5;
			array73[3] = scrollView4;
			array73[4] = grid13;
			array73[5] = grid15;
			array73[6] = this;
			object obj130;
			xamlServiceProvider73.Add(typeFromHandle145, obj130 = new SimpleValueTargetProvider(array73, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider73.Add(typeof(IReferenceProvider), obj130);
			Type typeFromHandle146 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver73 = new XmlNamespaceResolver();
			xmlNamespaceResolver73.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver73.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver73.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver73.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver73.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver73.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver73.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver73.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider73.Add(typeFromHandle146, new XamlTypeResolver(xmlNamespaceResolver73, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider73.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(606, 32)));
			object obj131 = markupExtension73.ProvideValue(xamlServiceProvider73);
			bindingExtension40.Converter = obj131;
			bindingExtension40.Path = "Use_km";
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			label31.SetBinding(VisualElement.IsVisibleProperty, bindingBase40);
			translate47.Text = "Settings_tbFuelPriceG.Text";
			IMarkupExtension markupExtension74 = translate47;
			XamlServiceProvider xamlServiceProvider74 = new XamlServiceProvider();
			Type typeFromHandle147 = typeof(IProvideValueTarget);
			object[] array74 = new object[0 + 6];
			array74[0] = label31;
			array74[1] = stackLayout5;
			array74[2] = scrollView4;
			array74[3] = grid13;
			array74[4] = grid15;
			array74[5] = this;
			object obj132;
			xamlServiceProvider74.Add(typeFromHandle147, obj132 = new SimpleValueTargetProvider(array74, Label.TextProperty, nameScope));
			xamlServiceProvider74.Add(typeof(IReferenceProvider), obj132);
			Type typeFromHandle148 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver74 = new XmlNamespaceResolver();
			xmlNamespaceResolver74.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver74.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver74.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver74.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver74.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver74.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver74.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver74.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider74.Add(typeFromHandle148, new XamlTypeResolver(xmlNamespaceResolver74, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider74.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(606, 126)));
			object obj133 = markupExtension74.ProvideValue(xamlServiceProvider74);
			label31.Text = obj133;
			stackLayout5.Children.Add(label31);
			entry.SizeChanged += this.numericEntry_SizeChanged;
			entry.TextChanged += this.numericEntry_TextChanged;
			stackLayout5.Children.Add(entry);
			translate48.Text = "Settings_tbFuelCurrency.Text";
			IMarkupExtension markupExtension75 = translate48;
			XamlServiceProvider xamlServiceProvider75 = new XamlServiceProvider();
			Type typeFromHandle149 = typeof(IProvideValueTarget);
			object[] array75 = new object[0 + 6];
			array75[0] = label32;
			array75[1] = stackLayout5;
			array75[2] = scrollView4;
			array75[3] = grid13;
			array75[4] = grid15;
			array75[5] = this;
			object obj134;
			xamlServiceProvider75.Add(typeFromHandle149, obj134 = new SimpleValueTargetProvider(array75, Label.TextProperty, nameScope));
			xamlServiceProvider75.Add(typeof(IReferenceProvider), obj134);
			Type typeFromHandle150 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver75 = new XmlNamespaceResolver();
			xmlNamespaceResolver75.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver75.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver75.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver75.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver75.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver75.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver75.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver75.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider75.Add(typeFromHandle150, new XamlTypeResolver(xmlNamespaceResolver75, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider75.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(612, 32)));
			object obj135 = markupExtension75.ProvideValue(xamlServiceProvider75);
			label32.Text = obj135;
			stackLayout5.Children.Add(label32);
			bindingExtension41.Mode = 1;
			bindingExtension41.Path = "Currency";
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase41);
			stackLayout5.Children.Add(entry2);
			bindingExtension42.Mode = 1;
			bindingExtension42.Path = "UseGPS";
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase42);
			translate49.Text = "ios_UseGPS";
			IMarkupExtension markupExtension76 = translate49;
			XamlServiceProvider xamlServiceProvider76 = new XamlServiceProvider();
			Type typeFromHandle151 = typeof(IProvideValueTarget);
			object[] array76 = new object[0 + 6];
			array76[0] = labelSwitch4;
			array76[1] = stackLayout5;
			array76[2] = scrollView4;
			array76[3] = grid13;
			array76[4] = grid15;
			array76[5] = this;
			object obj136;
			xamlServiceProvider76.Add(typeFromHandle151, obj136 = new SimpleValueTargetProvider(array76, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider76.Add(typeof(IReferenceProvider), obj136);
			Type typeFromHandle152 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver76 = new XmlNamespaceResolver();
			xmlNamespaceResolver76.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver76.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver76.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver76.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver76.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver76.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver76.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver76.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider76.Add(typeFromHandle152, new XamlTypeResolver(xmlNamespaceResolver76, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider76.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(615, 86)));
			object obj137 = markupExtension76.ProvideValue(xamlServiceProvider76);
			labelSwitch4.Text = obj137;
			stackLayout5.Children.Add(labelSwitch4);
			scrollView4.Content = stackLayout5;
			grid13.Children.Add(scrollView4);
			button5.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension16.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension77 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider77 = new XamlServiceProvider();
			Type typeFromHandle153 = typeof(IProvideValueTarget);
			object[] array77 = new object[0 + 4];
			array77[0] = button5;
			array77[1] = grid13;
			array77[2] = grid15;
			array77[3] = this;
			object obj138;
			xamlServiceProvider77.Add(typeFromHandle153, obj138 = new SimpleValueTargetProvider(array77, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider77.Add(typeof(IReferenceProvider), obj138);
			Type typeFromHandle154 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver77 = new XmlNamespaceResolver();
			xmlNamespaceResolver77.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver77.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver77.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver77.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver77.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver77.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver77.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver77.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider77.Add(typeFromHandle154, new XamlTypeResolver(xmlNamespaceResolver77, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider77.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(622, 21)));
			DynamicResource dynamicResource16 = markupExtension77.ProvideValue(xamlServiceProvider77);
			button5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource16.Key);
			button5.Clicked += this.btnFuelNext_Clicked;
			translate50.Text = "ios_NEXT";
			IMarkupExtension markupExtension78 = translate50;
			XamlServiceProvider xamlServiceProvider78 = new XamlServiceProvider();
			Type typeFromHandle155 = typeof(IProvideValueTarget);
			object[] array78 = new object[0 + 4];
			array78[0] = button5;
			array78[1] = grid13;
			array78[2] = grid15;
			array78[3] = this;
			object obj139;
			xamlServiceProvider78.Add(typeFromHandle155, obj139 = new SimpleValueTargetProvider(array78, Button.TextProperty, nameScope));
			xamlServiceProvider78.Add(typeof(IReferenceProvider), obj139);
			Type typeFromHandle156 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver78 = new XmlNamespaceResolver();
			xmlNamespaceResolver78.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver78.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver78.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver78.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver78.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver78.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver78.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver78.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider78.Add(typeFromHandle156, new XamlTypeResolver(xmlNamespaceResolver78, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider78.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(624, 21)));
			object obj140 = markupExtension78.ProvideValue(xamlServiceProvider78);
			button5.Text = obj140;
			button5.SetValue(Button.TextColorProperty, Color.White);
			grid13.Children.Add(button5);
			grid15.Children.Add(grid13);
			grid14.SetValue(Grid.RowProperty, 1);
			grid14.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition23.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid14.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition23);
			rowDefinition24.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid14.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition24);
			scrollView5.SetValue(BindableObject.BindingContextProperty, sharedSettings3);
			scrollView5.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			translate51.Text = "GDPR_Consent_Text";
			IMarkupExtension markupExtension79 = translate51;
			XamlServiceProvider xamlServiceProvider79 = new XamlServiceProvider();
			Type typeFromHandle157 = typeof(IProvideValueTarget);
			object[] array79 = new object[0 + 6];
			array79[0] = label33;
			array79[1] = stackLayout7;
			array79[2] = scrollView5;
			array79[3] = grid14;
			array79[4] = grid15;
			array79[5] = this;
			object obj141;
			xamlServiceProvider79.Add(typeFromHandle157, obj141 = new SimpleValueTargetProvider(array79, Label.TextProperty, nameScope));
			xamlServiceProvider79.Add(typeof(IReferenceProvider), obj141);
			Type typeFromHandle158 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver79 = new XmlNamespaceResolver();
			xmlNamespaceResolver79.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver79.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver79.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver79.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver79.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver79.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver79.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver79.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider79.Add(typeFromHandle158, new XamlTypeResolver(xmlNamespaceResolver79, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider79.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(642, 32)));
			object obj142 = markupExtension79.ProvideValue(xamlServiceProvider79);
			label33.Text = obj142;
			stackLayout7.Children.Add(label33);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension43.Mode = 1;
			bindingExtension43.Path = "GDPR_ShowPersonalyzed";
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			sfRadioButton8.SetBinding(ToggleButton.IsCheckedProperty, bindingBase43);
			translate52.Text = "GDPR_PersonalizedAds";
			IMarkupExtension markupExtension80 = translate52;
			XamlServiceProvider xamlServiceProvider80 = new XamlServiceProvider();
			Type typeFromHandle159 = typeof(IProvideValueTarget);
			object[] array80 = new object[0 + 7];
			array80[0] = sfRadioButton8;
			array80[1] = stackLayout6;
			array80[2] = stackLayout7;
			array80[3] = scrollView5;
			array80[4] = grid14;
			array80[5] = grid15;
			array80[6] = this;
			object obj143;
			xamlServiceProvider80.Add(typeFromHandle159, obj143 = new SimpleValueTargetProvider(array80, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider80.Add(typeof(IReferenceProvider), obj143);
			Type typeFromHandle160 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver80 = new XmlNamespaceResolver();
			xmlNamespaceResolver80.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver80.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver80.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver80.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver80.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver80.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver80.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver80.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider80.Add(typeFromHandle160, new XamlTypeResolver(xmlNamespaceResolver80, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider80.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(645, 112)));
			object obj144 = markupExtension80.ProvideValue(xamlServiceProvider80);
			sfRadioButton8.Text = obj144;
			stackLayout6.Children.Add(sfRadioButton8);
			bindingExtension44.Mode = 1;
			staticResourceExtension12.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension81 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider81 = new XamlServiceProvider();
			Type typeFromHandle161 = typeof(IProvideValueTarget);
			object[] array81 = new object[0 + 8];
			array81[0] = bindingExtension44;
			array81[1] = sfRadioButton9;
			array81[2] = stackLayout6;
			array81[3] = stackLayout7;
			array81[4] = scrollView5;
			array81[5] = grid14;
			array81[6] = grid15;
			array81[7] = this;
			object obj145;
			xamlServiceProvider81.Add(typeFromHandle161, obj145 = new SimpleValueTargetProvider(array81, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider81.Add(typeof(IReferenceProvider), obj145);
			Type typeFromHandle162 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver81 = new XmlNamespaceResolver();
			xmlNamespaceResolver81.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver81.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver81.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver81.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver81.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver81.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver81.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver81.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider81.Add(typeFromHandle162, new XamlTypeResolver(xmlNamespaceResolver81, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider81.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(646, 55)));
			object obj146 = markupExtension81.ProvideValue(xamlServiceProvider81);
			bindingExtension44.Converter = obj146;
			bindingExtension44.Path = "GDPR_ShowPersonalyzed";
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			sfRadioButton9.SetBinding(ToggleButton.IsCheckedProperty, bindingBase44);
			translate53.Text = "GDPR_NonPersonalizedAds";
			IMarkupExtension markupExtension82 = translate53;
			XamlServiceProvider xamlServiceProvider82 = new XamlServiceProvider();
			Type typeFromHandle163 = typeof(IProvideValueTarget);
			object[] array82 = new object[0 + 7];
			array82[0] = sfRadioButton9;
			array82[1] = stackLayout6;
			array82[2] = stackLayout7;
			array82[3] = scrollView5;
			array82[4] = grid14;
			array82[5] = grid15;
			array82[6] = this;
			object obj147;
			xamlServiceProvider82.Add(typeFromHandle163, obj147 = new SimpleValueTargetProvider(array82, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider82.Add(typeof(IReferenceProvider), obj147);
			Type typeFromHandle164 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver82 = new XmlNamespaceResolver();
			xmlNamespaceResolver82.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver82.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver82.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver82.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver82.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver82.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver82.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver82.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider82.Add(typeFromHandle164, new XamlTypeResolver(xmlNamespaceResolver82, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider82.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(646, 164)));
			object obj148 = markupExtension82.ProvideValue(xamlServiceProvider82);
			sfRadioButton9.Text = obj148;
			stackLayout6.Children.Add(sfRadioButton9);
			stackLayout7.Children.Add(stackLayout6);
			hyperLinkLabel2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel2.SetValue(HyperLinkLabel.NavigateUriProperty, "https://www.carscanner.info/privacy-policy/");
			translate54.Text = "ios_PrivacyPolicy";
			IMarkupExtension markupExtension83 = translate54;
			XamlServiceProvider xamlServiceProvider83 = new XamlServiceProvider();
			Type typeFromHandle165 = typeof(IProvideValueTarget);
			object[] array83 = new object[0 + 6];
			array83[0] = hyperLinkLabel2;
			array83[1] = stackLayout7;
			array83[2] = scrollView5;
			array83[3] = grid14;
			array83[4] = grid15;
			array83[5] = this;
			object obj149;
			xamlServiceProvider83.Add(typeFromHandle165, obj149 = new SimpleValueTargetProvider(array83, Label.TextProperty, nameScope));
			xamlServiceProvider83.Add(typeof(IReferenceProvider), obj149);
			Type typeFromHandle166 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver83 = new XmlNamespaceResolver();
			xmlNamespaceResolver83.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver83.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver83.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver83.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver83.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver83.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver83.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver83.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider83.Add(typeFromHandle166, new XamlTypeResolver(xmlNamespaceResolver83, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider83.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(653, 29)));
			object obj150 = markupExtension83.ProvideValue(xamlServiceProvider83);
			hyperLinkLabel2.Text = obj150;
			stackLayout7.Children.Add(hyperLinkLabel2);
			scrollView5.Content = stackLayout7;
			grid14.Children.Add(scrollView5);
			button6.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension17.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension84 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider84 = new XamlServiceProvider();
			Type typeFromHandle167 = typeof(IProvideValueTarget);
			object[] array84 = new object[0 + 4];
			array84[0] = button6;
			array84[1] = grid14;
			array84[2] = grid15;
			array84[3] = this;
			object obj151;
			xamlServiceProvider84.Add(typeFromHandle167, obj151 = new SimpleValueTargetProvider(array84, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider84.Add(typeof(IReferenceProvider), obj151);
			Type typeFromHandle168 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver84 = new XmlNamespaceResolver();
			xmlNamespaceResolver84.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver84.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver84.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver84.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver84.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver84.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver84.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver84.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider84.Add(typeFromHandle168, new XamlTypeResolver(xmlNamespaceResolver84, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider84.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(659, 21)));
			DynamicResource dynamicResource17 = markupExtension84.ProvideValue(xamlServiceProvider84);
			button6.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource17.Key);
			button6.Clicked += this.btnPrivacyNext_Clicked;
			translate55.Text = "ios_NEXT";
			IMarkupExtension markupExtension85 = translate55;
			XamlServiceProvider xamlServiceProvider85 = new XamlServiceProvider();
			Type typeFromHandle169 = typeof(IProvideValueTarget);
			object[] array85 = new object[0 + 4];
			array85[0] = button6;
			array85[1] = grid14;
			array85[2] = grid15;
			array85[3] = this;
			object obj152;
			xamlServiceProvider85.Add(typeFromHandle169, obj152 = new SimpleValueTargetProvider(array85, Button.TextProperty, nameScope));
			xamlServiceProvider85.Add(typeof(IReferenceProvider), obj152);
			Type typeFromHandle170 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver85 = new XmlNamespaceResolver();
			xmlNamespaceResolver85.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver85.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver85.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver85.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver85.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver85.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver85.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver85.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider85.Add(typeFromHandle170, new XamlTypeResolver(xmlNamespaceResolver85, typeof(WelcomePage1V3).GetTypeInfo().Assembly));
			xamlServiceProvider85.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(661, 21)));
			object obj153 = markupExtension85.ProvideValue(xamlServiceProvider85);
			button6.Text = obj153;
			button6.SetValue(Button.TextColorProperty, Color.White);
			grid14.Children.Add(button6);
			grid15.Children.Add(grid14);
			this.SetValue(ContentPage.ContentProperty, grid15);
		}

		// Token: 0x06003922 RID: 14626 RVA: 0x002D3FA4 File Offset: 0x002D21A4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<WelcomePage1V3>(this, typeof(WelcomePage1V3));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.lbFormattedTitle = NameScopeExtensions.FindByName<Label>(this, "lbFormattedTitle");
			this.lbPageTitle = NameScopeExtensions.FindByName<Span>(this, "lbPageTitle");
			this.lbPageTitleSeparator = NameScopeExtensions.FindByName<Span>(this, "lbPageTitleSeparator");
			this.lbPageSubtitle = NameScopeExtensions.FindByName<Span>(this, "lbPageSubtitle");
			this.progressBar = NameScopeExtensions.FindByName<ProgressBar>(this, "progressBar");
			this.gridEULA = NameScopeExtensions.FindByName<Grid>(this, "gridEULA");
			this.lbEULA = NameScopeExtensions.FindByName<Label>(this, "lbEULA");
			this.eulaActivityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "eulaActivityFrame");
			this.gridAdapter = NameScopeExtensions.FindByName<Grid>(this, "gridAdapter");
			this.iosELMLabel = NameScopeExtensions.FindByName<Label>(this, "iosELMLabel");
			this.droidELMLabel = NameScopeExtensions.FindByName<Label>(this, "droidELMLabel");
			this.btnAdapterNext = NameScopeExtensions.FindByName<Button>(this, "btnAdapterNext");
			this.gridInterface = NameScopeExtensions.FindByName<Grid>(this, "gridInterface");
			this.interfaceThemeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "interfaceThemeGroup");
			this.btnLight = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnLight");
			this.btnDark = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnDark");
			this.gridDashPreview = NameScopeExtensions.FindByName<Grid>(this, "gridDashPreview");
			this.dashThemeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "dashThemeGroup");
			this.btnCarScannerDash = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnCarScannerDash");
			this.btnLightDash = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnLightDash");
			this.btnDarkDash = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnDarkDash");
			this.btnInterfaceNext = NameScopeExtensions.FindByName<Button>(this, "btnInterfaceNext");
			this.gridProfileSelector = NameScopeExtensions.FindByName<Grid>(this, "gridProfileSelector");
			this.profileSelector = NameScopeExtensions.FindByName<ProfileSelectorV2>(this, "profileSelector");
			this.gridFuel = NameScopeExtensions.FindByName<Grid>(this, "gridFuel");
			this.panelCustomFuel = NameScopeExtensions.FindByName<StackLayout>(this, "panelCustomFuel");
			this.alwaysRecordFuelGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "alwaysRecordFuelGroup");
			this.entryL = NameScopeExtensions.FindByName<Entry>(this, "entryL");
			this.btnFuelNext = NameScopeExtensions.FindByName<Button>(this, "btnFuelNext");
			this.gridPrivacy = NameScopeExtensions.FindByName<Grid>(this, "gridPrivacy");
			this.panelAdsSelection = NameScopeExtensions.FindByName<StackLayout>(this, "panelAdsSelection");
			this.btnPrivacyNext = NameScopeExtensions.FindByName<Button>(this, "btnPrivacyNext");
		}

		// Token: 0x040022C4 RID: 8900
		private bool eulaUpdated;

		// Token: 0x040022C5 RID: 8901
		private DashboardItem dashItem;

		// Token: 0x040022C6 RID: 8902
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x040022C7 RID: 8903
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbFormattedTitle;

		// Token: 0x040022C8 RID: 8904
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageTitle;

		// Token: 0x040022C9 RID: 8905
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageTitleSeparator;

		// Token: 0x040022CA RID: 8906
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Span lbPageSubtitle;

		// Token: 0x040022CB RID: 8907
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProgressBar progressBar;

		// Token: 0x040022CC RID: 8908
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridEULA;

		// Token: 0x040022CD RID: 8909
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbEULA;

		// Token: 0x040022CE RID: 8910
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame eulaActivityFrame;

		// Token: 0x040022CF RID: 8911
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridAdapter;

		// Token: 0x040022D0 RID: 8912
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label iosELMLabel;

		// Token: 0x040022D1 RID: 8913
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label droidELMLabel;

		// Token: 0x040022D2 RID: 8914
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnAdapterNext;

		// Token: 0x040022D3 RID: 8915
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridInterface;

		// Token: 0x040022D4 RID: 8916
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup interfaceThemeGroup;

		// Token: 0x040022D5 RID: 8917
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnLight;

		// Token: 0x040022D6 RID: 8918
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnDark;

		// Token: 0x040022D7 RID: 8919
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridDashPreview;

		// Token: 0x040022D8 RID: 8920
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup dashThemeGroup;

		// Token: 0x040022D9 RID: 8921
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnCarScannerDash;

		// Token: 0x040022DA RID: 8922
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnLightDash;

		// Token: 0x040022DB RID: 8923
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnDarkDash;

		// Token: 0x040022DC RID: 8924
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnInterfaceNext;

		// Token: 0x040022DD RID: 8925
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridProfileSelector;

		// Token: 0x040022DE RID: 8926
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProfileSelectorV2 profileSelector;

		// Token: 0x040022DF RID: 8927
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridFuel;

		// Token: 0x040022E0 RID: 8928
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelCustomFuel;

		// Token: 0x040022E1 RID: 8929
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup alwaysRecordFuelGroup;

		// Token: 0x040022E2 RID: 8930
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryL;

		// Token: 0x040022E3 RID: 8931
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnFuelNext;

		// Token: 0x040022E4 RID: 8932
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridPrivacy;

		// Token: 0x040022E5 RID: 8933
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelAdsSelection;

		// Token: 0x040022E6 RID: 8934
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnPrivacyNext;

		// Token: 0x02000684 RID: 1668
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003923 RID: 14627 RVA: 0x002D41F3 File Offset: 0x002D23F3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003924 RID: 14628 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003925 RID: 14629 RVA: 0x002BF3A6 File Offset: 0x002BD5A6
			internal void <btnAgree_Clicked>b__6_0(object d, EventArgs args)
			{
				SharedSettings.Current.FirstConnectionAttempted = true;
			}

			// Token: 0x040022E7 RID: 8935
			public static readonly WelcomePage1V3.<>c <>9 = new WelcomePage1V3.<>c();

			// Token: 0x040022E8 RID: 8936
			public static EventHandler <>9__6_0;
		}

		// Token: 0x02000685 RID: 1669
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ProfileSelector_NextRequested>d__10 : IAsyncStateMachine
		{
			// Token: 0x06003926 RID: 14630 RVA: 0x002D4200 File Offset: 0x002D2400
			void IAsyncStateMachine.MoveNext()
			{
				WelcomePage1V3 welcomePage1V = this;
				try
				{
					welcomePage1V.lbPageSubtitle.Text = Translate.GetString("Settings_Control_FuelFlowItem.Content");
					welcomePage1V.gridProfileSelector.IsVisible = false;
					welcomePage1V.gridFuel.IsVisible = true;
					welcomePage1V.progressBar.Progress = 0.8;
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

			// Token: 0x06003927 RID: 14631 RVA: 0x002D4294 File Offset: 0x002D2494
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022E9 RID: 8937
			public int <>1__state;

			// Token: 0x040022EA RID: 8938
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022EB RID: 8939
			public WelcomePage1V3 <>4__this;
		}

		// Token: 0x02000686 RID: 1670
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetEULARus>d__3 : IAsyncStateMachine
		{
			// Token: 0x06003928 RID: 14632 RVA: 0x002D42A4 File Offset: 0x002D24A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V3 welcomePage1V = this;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, WelcomePage1V3.<SetEULARus>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x06003929 RID: 14633 RVA: 0x002D43BC File Offset: 0x002D25BC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022EC RID: 8940
			public int <>1__state;

			// Token: 0x040022ED RID: 8941
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022EE RID: 8942
			public WelcomePage1V3 <>4__this;

			// Token: 0x040022EF RID: 8943
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000687 RID: 1671
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPrivacyNext_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x0600392A RID: 14634 RVA: 0x002D43CC File Offset: 0x002D25CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage1V3 welcomePage1V = this;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, WelcomePage1V3.<btnPrivacyNext_Clicked>d__12>(ref taskAwaiter, ref this);
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

			// Token: 0x0600392B RID: 14635 RVA: 0x002D44AC File Offset: 0x002D26AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022F0 RID: 8944
			public int <>1__state;

			// Token: 0x040022F1 RID: 8945
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022F2 RID: 8946
			public WelcomePage1V3 <>4__this;

			// Token: 0x040022F3 RID: 8947
			private TaskAwaiter<bool> <>u__1;
		}
	}
}
