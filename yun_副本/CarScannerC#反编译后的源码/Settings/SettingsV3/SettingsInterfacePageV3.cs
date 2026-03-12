using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002A6 RID: 678
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml")]
	public class SettingsInterfacePageV3 : ContentPage
	{
		// Token: 0x0600211E RID: 8478 RVA: 0x00185740 File Offset: 0x00183940
		public SettingsInterfacePageV3()
		{
			this.InitializeComponent();
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				this.sectionLanguage.IsVisible = false;
			}
			base.BindingContext = SharedSettings.Current;
			base.Appearing += this.SettingsPage_Appearing;
			base.Disappearing += this.SettingsPage_Disappearing;
			if (PlatformHelper.IsiOS)
			{
				this.carPlayRefreshRateSection.Title = Translate.GetString("settings_AppleCarPlayRefreshInterval");
			}
			else if (PlatformHelper.IsAndroid)
			{
				this.carPlayRefreshRateSection.Title = Translate.GetString("settings_AndroidAutoRefreshInterval");
			}
			if (SharedSettings.Current.ShowExperimental && CarPlayManager.Instance.IsConnected)
			{
				this.carPlayRefreshRateSection.IsVisible = true;
				return;
			}
			this.carPlayRefreshRateSection.IsVisible = false;
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x00185808 File Offset: 0x00183A08
		private void SettingsPage_Disappearing(object sender, EventArgs e)
		{
			try
			{
				base.BindingContext = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x0016D7E4 File Offset: 0x0016B9E4
		private void SettingsPage_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x00185834 File Offset: 0x00183A34
		private async void btnApplyLanguage_Clicked(object sender, EventArgs e)
		{
			if (App.OBDSimulator.IsActive)
			{
				App.OBDSimulator.Stop();
			}
			if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
			{
				await App.OBDReader.Disconnect("btnApplyLanguage_Clicked");
			}
			App.Instance.ChangeLanguage();
			SharedSettings.Current.LanguageChanged = false;
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000E988B File Offset: 0x000E7A8B
		private void DarkThemeSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			base.ApplyBindings();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x00185864 File Offset: 0x00183A64
		private async void btnMainScreenConfiguration_Clicked(object sender, EventArgs e)
		{
			(sender as Cell).IsEnabled = false;
			await base.Navigation.PushAsync(new MainPageConfigurationScreen());
			(sender as Cell).IsEnabled = true;
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x001858A4 File Offset: 0x00183AA4
		private void btnFontSizePlus_Tapped(object sender, EventArgs e)
		{
			SharedSettings sharedSettings = SharedSettings.Current;
			int fontSizePatch = sharedSettings.FontSizePatch;
			sharedSettings.FontSizePatch = fontSizePatch + 1;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x001858C8 File Offset: 0x00183AC8
		private void btnFontSizeMinus_Tapped(object sender, EventArgs e)
		{
			SharedSettings sharedSettings = SharedSettings.Current;
			int fontSizePatch = sharedSettings.FontSizePatch;
			sharedSettings.FontSizePatch = fontSizePatch - 1;
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x001858EC File Offset: 0x00183AEC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsInterfacePageV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			IntToStringInItemsConverter intToStringInItemsConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringInItemsConverter = new IntToStringInItemsConverter(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 50);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 32);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 82);
			List<string> languageList;
			VisualDiagnostics.RegisterSourceInfo(languageList = StaticLists.LanguageList, new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 82);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 82);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 26);
			List<string> languageList2;
			VisualDiagnostics.RegisterSourceInfo(languageList2 = StaticLists.LanguageList, new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 25);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 75);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 21);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 21);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 21);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 21);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 21);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 49);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 113);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 49);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 117);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 31);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 86);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 21);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 49);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 111);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 14);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 81);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 31);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 99);
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 31);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 102);
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 14);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 86);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 31);
			int num = 0;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 31);
			int num2 = 1;
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 31);
			int num3 = 2;
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 18);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 49);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 129);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 49);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 104);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 18);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 49);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 118);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 18);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 49);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 109);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 30);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 30);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched8;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched8 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 14);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 25);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 47);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 47);
			NumericValidationBehavior numericValidationBehavior;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior = new NumericValidationBehavior(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 30);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 14);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 76);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 31);
			ChartItemTypes chartItemTypes = ChartItemTypes.FastLine;
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 18);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 31);
			ChartItemTypes chartItemTypes2 = ChartItemTypes.SplineLine;
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 18);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 31);
			ChartItemTypes chartItemTypes3 = ChartItemTypes.Area;
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 18);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 31);
			ChartItemTypes chartItemTypes4 = ChartItemTypes.SplineArea;
			RadioCell radioCell11;
			VisualDiagnostics.RegisterSourceInfo(radioCell11 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 14);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 25);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 116);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 31);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 125);
			RadioCell radioCell12;
			VisualDiagnostics.RegisterSourceInfo(radioCell12 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 18);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 31);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 126);
			RadioCell radioCell13;
			VisualDiagnostics.RegisterSourceInfo(radioCell13 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 14);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 25);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 105);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 31);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 105);
			RadioCell radioCell14;
			VisualDiagnostics.RegisterSourceInfo(radioCell14 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 18);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 31);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 104);
			RadioCell radioCell15;
			VisualDiagnostics.RegisterSourceInfo(radioCell15 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 14);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 25);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 47);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 47);
			NumericValidationBehavior numericValidationBehavior2;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior2 = new NumericValidationBehavior(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 30);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 14);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 47);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 47);
			NumericValidationBehavior numericValidationBehavior3;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior3 = new NumericValidationBehavior(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 30);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 18);
			Section section11;
			VisualDiagnostics.RegisterSourceInfo(section11 = new Section(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsInterfacePageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("sectionLanguage", section);
			if (section.StyleId == null)
			{
				section.StyleId = "sectionLanguage";
			}
			nameScope.RegisterName("btnFontSizePlus", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnFontSizePlus";
			}
			nameScope.RegisterName("btnFontSizeMinus", buttonCell3);
			if (buttonCell3.StyleId == null)
			{
				buttonCell3.StyleId = "btnFontSizeMinus";
			}
			nameScope.RegisterName("btnMainScreenConfiguration", buttonCell4);
			if (buttonCell4.StyleId == null)
			{
				buttonCell4.StyleId = "btnMainScreenConfiguration";
			}
			nameScope.RegisterName("carPlayRefreshRateSection", section11);
			if (section11.StyleId == null)
			{
				section11.StyleId = "carPlayRefreshRateSection";
			}
			this.settingsLayoutRoot = settingsView;
			this.sectionLanguage = section;
			this.btnFontSizePlus = buttonCell2;
			this.btnFontSizeMinus = buttonCell3;
			this.btnMainScreenConfiguration = buttonCell4;
			this.carPlayRefreshRateSection = section11;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("IntToStringInItemsConverter", intToStringInItemsConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "settings_Interface";
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
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
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
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbLanguage.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = section;
			array3[1] = settingsView;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 50)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			staticResourceExtension.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = settingsCustomCellForPicker;
			array4[3] = section;
			array4[4] = settingsView;
			array4[5] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 32)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.Style = obj7;
			bindingExtension.Mode = 1;
			staticResourceExtension2.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 7];
			array5[0] = bindingExtension;
			array5[1] = label;
			array5[2] = grid;
			array5[3] = settingsCustomCellForPicker;
			array5[4] = section;
			array5[5] = settingsView;
			array5[6] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 82)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension.Converter = obj9;
			bindingExtension.ConverterParameter = languageList;
			bindingExtension.Path = "Language";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Language, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.Language = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Language")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			grid.Children.Add(label);
			picker.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension2.Source = languageList2;
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase2);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "Language";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Language, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.Language = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Language")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase3);
			grid.Children.Add(picker);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, grid);
			section.Add(settingsCustomCellForPicker);
			buttonCell.SetValue(CellBase.TitleProperty, "OK");
			buttonCell.Tapped += this.btnApplyLanguage_Clicked;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = buttonCell;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 21)));
			DynamicResource dynamicResource2 = markupExtension6.ProvideValue(xamlServiceProvider6);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section.Add(buttonCell);
			settingsView.Root.Add(section);
			translate3.Text = "ios_InterfaceTheme";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = section2;
			array7[1] = settingsView;
			array7[2] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 25)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			section2.Title = obj12;
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "DarkMode";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			section2.SetBinding(RadioCell.SelectedValueProperty, bindingBase4);
			translate4.Text = "ios_AutomaticallySwitchTheme";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = settingsCheckBoxCellPatched;
			array8[1] = section2;
			array8[2] = settingsView;
			array8[3] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, CellBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 21)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			settingsCheckBoxCellPatched.Title = obj14;
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "AutomaticallySwitchTheme";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "AutomaticallySwitchThemeAvailable";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CellBase.IsVisibleProperty, bindingBase6);
			section2.Add(settingsCheckBoxCellPatched);
			translate5.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension9 = translate5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = radioCell;
			array9[1] = section2;
			array9[2] = settingsView;
			array9[3] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, CellBase.TitleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 21)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			radioCell.Title = obj16;
			bindingExtension7.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = bindingExtension7;
			array10[1] = radioCell;
			array10[2] = section2;
			array10[3] = settingsView;
			array10[4] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 21)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension7.Converter = obj18;
			bindingExtension7.Path = "AutomaticallySwitchTheme";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			radioCell.SetBinding(CellBase.IsVisibleProperty, bindingBase7);
			staticResourceExtension4.Key = "FalseValue";
			IMarkupExtension markupExtension11 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = radioCell;
			array11[1] = section2;
			array11[2] = settingsView;
			array11[3] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 21)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			radioCell.SetValue(RadioCell.ValueProperty, obj20);
			section2.Add(radioCell);
			translate6.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = radioCell2;
			array12[1] = section2;
			array12[2] = settingsView;
			array12[3] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 21)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			radioCell2.Title = obj22;
			bindingExtension8.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = bindingExtension8;
			array13[1] = radioCell2;
			array13[2] = section2;
			array13[3] = settingsView;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 21)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension8.Converter = obj24;
			bindingExtension8.Path = "AutomaticallySwitchTheme";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			radioCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase8);
			staticResourceExtension6.Key = "TrueValue";
			IMarkupExtension markupExtension14 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = radioCell2;
			array14[1] = section2;
			array14[2] = settingsView;
			array14[3] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 21)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			radioCell2.SetValue(RadioCell.ValueProperty, obj26);
			section2.Add(radioCell2);
			settingsView.Root.Add(section2);
			translate7.Text = "settings_Common";
			IMarkupExtension markupExtension15 = translate7;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = section3;
			array15[1] = settingsView;
			array15[2] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 25)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			section3.Title = obj28;
			translate8.Text = "Settings_Control_tbShowPing.Text";
			IMarkupExtension markupExtension16 = translate8;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = settingsCheckBoxCellPatched2;
			array16[1] = section3;
			array16[2] = settingsView;
			array16[3] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array16, CellBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 49)));
			object obj30 = markupExtension16.ProvideValue(xamlServiceProvider16);
			settingsCheckBoxCellPatched2.Title = obj30;
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "ShowPing";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowPing, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowPing = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowPing")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase9);
			section3.Add(settingsCheckBoxCellPatched2);
			translate9.Text = "settings_ShowConnectionStatusOverlay";
			IMarkupExtension markupExtension17 = translate9;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = settingsCheckBoxCellPatched3;
			array17[1] = section3;
			array17[2] = settingsView;
			array17[3] = this;
			object obj31;
			xamlServiceProvider17.Add(typeFromHandle33, obj31 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 49)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			settingsCheckBoxCellPatched3.Title = obj32;
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "ShowConnectionStatusOverlay";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowConnectionStatusOverlay, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowConnectionStatusOverlay = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowConnectionStatusOverlay")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase10);
			section3.Add(settingsCheckBoxCellPatched3);
			translate10.Text = "Settings_AdjustFontSize";
			IMarkupExtension markupExtension18 = translate10;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = labelCell;
			array18[1] = section3;
			array18[2] = settingsView;
			array18[3] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle35, obj33 = new SimpleValueTargetProvider(array18, CellBase.TitleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 31)));
			object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
			labelCell.Title = obj34;
			bindingExtension11.Mode = 2;
			bindingExtension11.StringFormat = "{0:+#;-#;0}";
			bindingExtension11.Path = "FontSizePatch";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.FontSizePatch, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FontSizePatch")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase11);
			section3.Add(labelCell);
			translate11.Text = "Settings_IncreaseFont";
			IMarkupExtension markupExtension19 = translate11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = buttonCell2;
			array19[1] = section3;
			array19[2] = settingsView;
			array19[3] = this;
			object obj35;
			xamlServiceProvider19.Add(typeFromHandle37, obj35 = new SimpleValueTargetProvider(array19, CellBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 21)));
			object obj36 = markupExtension19.ProvideValue(xamlServiceProvider19);
			buttonCell2.Title = obj36;
			buttonCell2.Tapped += this.btnFontSizePlus_Tapped;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = buttonCell2;
			array20[1] = section3;
			array20[2] = settingsView;
			array20[3] = this;
			object obj37;
			xamlServiceProvider20.Add(typeFromHandle39, obj37 = new SimpleValueTargetProvider(array20, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 21)));
			DynamicResource dynamicResource3 = markupExtension20.ProvideValue(xamlServiceProvider20);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section3.Add(buttonCell2);
			translate12.Text = "Settings_DecreaseFont";
			IMarkupExtension markupExtension21 = translate12;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = buttonCell3;
			array21[1] = section3;
			array21[2] = settingsView;
			array21[3] = this;
			object obj38;
			xamlServiceProvider21.Add(typeFromHandle41, obj38 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 21)));
			object obj39 = markupExtension21.ProvideValue(xamlServiceProvider21);
			buttonCell3.Title = obj39;
			buttonCell3.Tapped += this.btnFontSizeMinus_Tapped;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = buttonCell3;
			array22[1] = section3;
			array22[2] = settingsView;
			array22[3] = this;
			object obj40;
			xamlServiceProvider22.Add(typeFromHandle43, obj40 = new SimpleValueTargetProvider(array22, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 21)));
			DynamicResource dynamicResource4 = markupExtension22.ProvideValue(xamlServiceProvider22);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section3.Add(buttonCell3);
			translate13.Text = "ios_MainScreen";
			IMarkupExtension markupExtension23 = translate13;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = buttonCell4;
			array23[1] = section3;
			array23[2] = settingsView;
			array23[3] = this;
			object obj41;
			xamlServiceProvider23.Add(typeFromHandle45, obj41 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 21)));
			object obj42 = markupExtension23.ProvideValue(xamlServiceProvider23);
			buttonCell4.Title = obj42;
			buttonCell4.Tapped += this.btnMainScreenConfiguration_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = buttonCell4;
			array24[1] = section3;
			array24[2] = settingsView;
			array24[3] = this;
			object obj43;
			xamlServiceProvider24.Add(typeFromHandle47, obj43 = new SimpleValueTargetProvider(array24, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 21)));
			DynamicResource dynamicResource5 = markupExtension24.ProvideValue(xamlServiceProvider24);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section3.Add(buttonCell4);
			translate14.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension25 = translate14;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = settingsCheckBoxCellPatched4;
			array25[1] = section3;
			array25[2] = settingsView;
			array25[3] = this;
			object obj44;
			xamlServiceProvider25.Add(typeFromHandle49, obj44 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 49)));
			object obj45 = markupExtension25.ProvideValue(xamlServiceProvider25);
			settingsCheckBoxCellPatched4.Title = obj45;
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "AndroidRecolorNavBar";
			bindingExtension12.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase12);
			onPlatform.Android = true;
			onPlatform.iOS = false;
			settingsCheckBoxCellPatched4.SetValue(CellBase.IsVisibleProperty, onPlatform);
			section3.Add(settingsCheckBoxCellPatched4);
			settingsView.Root.Add(section3);
			translate15.Text = "settings_PIDSelectorType";
			IMarkupExtension markupExtension26 = translate15;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 3];
			array26[0] = section4;
			array26[1] = settingsView;
			array26[2] = this;
			object obj46;
			xamlServiceProvider26.Add(typeFromHandle51, obj46 = new SimpleValueTargetProvider(array26, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 25)));
			object obj47 = markupExtension26.ProvideValue(xamlServiceProvider26);
			section4.Title = obj47;
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "PIDSelectorWithValuePreview";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PIDSelectorWithValuePreview, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PIDSelectorWithValuePreview = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "PIDSelectorWithValuePreview")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			section4.SetBinding(RadioCell.SelectedValueProperty, bindingBase13);
			translate16.Text = "settings_PIDSelectorWithValuePreview";
			IMarkupExtension markupExtension27 = translate16;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = radioCell3;
			array27[1] = section4;
			array27[2] = settingsView;
			array27[3] = this;
			object obj48;
			xamlServiceProvider27.Add(typeFromHandle53, obj48 = new SimpleValueTargetProvider(array27, CellBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 31)));
			object obj49 = markupExtension27.ProvideValue(xamlServiceProvider27);
			radioCell3.Title = obj49;
			staticResourceExtension7.Key = "TrueValue";
			IMarkupExtension markupExtension28 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = radioCell3;
			array28[1] = section4;
			array28[2] = settingsView;
			array28[3] = this;
			object obj50;
			xamlServiceProvider28.Add(typeFromHandle55, obj50 = new SimpleValueTargetProvider(array28, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 99)));
			object obj51 = markupExtension28.ProvideValue(xamlServiceProvider28);
			radioCell3.SetValue(RadioCell.ValueProperty, obj51);
			section4.Add(radioCell3);
			translate17.Text = "settings_PIDSelectorWithourValuePreview";
			IMarkupExtension markupExtension29 = translate17;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = radioCell4;
			array29[1] = section4;
			array29[2] = settingsView;
			array29[3] = this;
			object obj52;
			xamlServiceProvider29.Add(typeFromHandle57, obj52 = new SimpleValueTargetProvider(array29, CellBase.TitleProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 31)));
			object obj53 = markupExtension29.ProvideValue(xamlServiceProvider29);
			radioCell4.Title = obj53;
			staticResourceExtension8.Key = "FalseValue";
			IMarkupExtension markupExtension30 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = radioCell4;
			array30[1] = section4;
			array30[2] = settingsView;
			array30[3] = this;
			object obj54;
			xamlServiceProvider30.Add(typeFromHandle59, obj54 = new SimpleValueTargetProvider(array30, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 102)));
			object obj55 = markupExtension30.ProvideValue(xamlServiceProvider30);
			radioCell4.SetValue(RadioCell.ValueProperty, obj55);
			section4.Add(radioCell4);
			settingsView.Root.Add(section4);
			translate18.Text = "Settings_Control_tbChartsMode";
			IMarkupExtension markupExtension31 = translate18;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 3];
			array31[0] = section5;
			array31[1] = settingsView;
			array31[2] = this;
			object obj56;
			xamlServiceProvider31.Add(typeFromHandle61, obj56 = new SimpleValueTargetProvider(array31, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 25)));
			object obj57 = markupExtension31.ProvideValue(xamlServiceProvider31);
			section5.Title = obj57;
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "ChartsView";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ChartsView, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartsView = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartsView")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			section5.SetBinding(RadioCell.SelectedValueProperty, bindingBase14);
			translate19.Text = "Settings_Control_tbChartsMode_Ask";
			IMarkupExtension markupExtension32 = translate19;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = radioCell5;
			array32[1] = section5;
			array32[2] = settingsView;
			array32[3] = this;
			object obj58;
			xamlServiceProvider32.Add(typeFromHandle63, obj58 = new SimpleValueTargetProvider(array32, CellBase.TitleProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 31)));
			object obj59 = markupExtension32.ProvideValue(xamlServiceProvider32);
			radioCell5.Title = obj59;
			radioCell5.SetValue(RadioCell.ValueProperty, num);
			section5.Add(radioCell5);
			translate20.Text = "ios_LiveDataMode_Combined";
			IMarkupExtension markupExtension33 = translate20;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = radioCell6;
			array33[1] = section5;
			array33[2] = settingsView;
			array33[3] = this;
			object obj60;
			xamlServiceProvider33.Add(typeFromHandle65, obj60 = new SimpleValueTargetProvider(array33, CellBase.TitleProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 31)));
			object obj61 = markupExtension33.ProvideValue(xamlServiceProvider33);
			radioCell6.Title = obj61;
			radioCell6.SetValue(RadioCell.ValueProperty, num2);
			section5.Add(radioCell6);
			translate21.Text = "ios_LiveDataMode_Separate";
			IMarkupExtension markupExtension34 = translate21;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = radioCell7;
			array34[1] = section5;
			array34[2] = settingsView;
			array34[3] = this;
			object obj62;
			xamlServiceProvider34.Add(typeFromHandle67, obj62 = new SimpleValueTargetProvider(array34, CellBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 31)));
			object obj63 = markupExtension34.ProvideValue(xamlServiceProvider34);
			radioCell7.Title = obj63;
			radioCell7.SetValue(RadioCell.ValueProperty, num3);
			section5.Add(radioCell7);
			translate22.Text = "Settings_Control_ToggleShowAverageOnChart.Header";
			IMarkupExtension markupExtension35 = translate22;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = settingsCheckBoxCellPatched5;
			array35[1] = section5;
			array35[2] = settingsView;
			array35[3] = this;
			object obj64;
			xamlServiceProvider35.Add(typeFromHandle69, obj64 = new SimpleValueTargetProvider(array35, CellBase.TitleProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 49)));
			object obj65 = markupExtension35.ProvideValue(xamlServiceProvider35);
			settingsCheckBoxCellPatched5.Title = obj65;
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "ChartShowAverageValue";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartShowAverageValue = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartShowAverageValue")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase15);
			section5.Add(settingsCheckBoxCellPatched5);
			translate23.Text = "Dashboard_DisplayMinMax";
			IMarkupExtension markupExtension36 = translate23;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = settingsCheckBoxCellPatched6;
			array36[1] = section5;
			array36[2] = settingsView;
			array36[3] = this;
			object obj66;
			xamlServiceProvider36.Add(typeFromHandle71, obj66 = new SimpleValueTargetProvider(array36, CellBase.TitleProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 49)));
			object obj67 = markupExtension36.ProvideValue(xamlServiceProvider36);
			settingsCheckBoxCellPatched6.Title = obj67;
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "ShowMinMaxValues";
			bindingExtension16.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowMinMaxValues = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowMinMaxValues")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase16);
			section5.Add(settingsCheckBoxCellPatched6);
			translate24.Text = "ios_Settings_MultiChart_PauseRequests";
			IMarkupExtension markupExtension37 = translate24;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = settingsCheckBoxCellPatched7;
			array37[1] = section5;
			array37[2] = settingsView;
			array37[3] = this;
			object obj68;
			xamlServiceProvider37.Add(typeFromHandle73, obj68 = new SimpleValueTargetProvider(array37, CellBase.TitleProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 49)));
			object obj69 = markupExtension37.ProvideValue(xamlServiceProvider37);
			settingsCheckBoxCellPatched7.Title = obj69;
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "MultiChartPauseHidden";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.MultiChartPauseHidden, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.MultiChartPauseHidden = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "MultiChartPauseHidden")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase17);
			section5.Add(settingsCheckBoxCellPatched7);
			translate25.Text = "droid_ChartRenderingSafeMode";
			IMarkupExtension markupExtension38 = translate25;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = settingsCheckBoxCellPatched8;
			array38[1] = section5;
			array38[2] = settingsView;
			array38[3] = this;
			object obj70;
			xamlServiceProvider38.Add(typeFromHandle75, obj70 = new SimpleValueTargetProvider(array38, CellBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 49)));
			object obj71 = markupExtension38.ProvideValue(xamlServiceProvider38);
			settingsCheckBoxCellPatched8.Title = obj71;
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "AndroidChartRenderingSafeMode";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidChartRenderingSafeMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidChartRenderingSafeMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidChartRenderingSafeMode")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CheckboxCell.CheckedProperty, bindingBase18);
			on.Platform = new List<string>(1) { "Android" };
			on.Value = "True";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "False";
			onPlatform2.Platforms.Add(on2);
			settingsCheckBoxCellPatched8.SetValue(CellBase.IsVisibleProperty, onPlatform2);
			section5.Add(settingsCheckBoxCellPatched8);
			settingsView.Root.Add(section5);
			translate26.Text = "Settings_Control_tbChartVisibleTime.Text";
			IMarkupExtension markupExtension39 = translate26;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 3];
			array39[0] = section6;
			array39[1] = settingsView;
			array39[2] = this;
			object obj72;
			xamlServiceProvider39.Add(typeFromHandle77, obj72 = new SimpleValueTargetProvider(array39, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 25)));
			object obj73 = markupExtension39.ProvideValue(xamlServiceProvider39);
			section6.Title = obj73;
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			entry.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension19.Mode = 1;
			staticResourceExtension9.Key = "IntToStringConverter";
			IMarkupExtension markupExtension40 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 6];
			array40[0] = bindingExtension19;
			array40[1] = entry;
			array40[2] = customCell;
			array40[3] = section6;
			array40[4] = settingsView;
			array40[5] = this;
			object obj74;
			xamlServiceProvider40.Add(typeFromHandle79, obj74 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 47)));
			object obj75 = markupExtension40.ProvideValue(xamlServiceProvider40);
			bindingExtension19.Converter = obj75;
			bindingExtension19.Path = "LiveDataShowTime";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.LiveDataShowTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "LiveDataShowTime")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase19);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumValueProperty, 1.0);
			((ICollection<Behavior>)entry.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior);
			customCell.SetValue(CustomCell.ContentProperty, entry);
			section6.Add(customCell);
			settingsView.Root.Add(section6);
			translate27.Text = "Settings_ChartStyle";
			IMarkupExtension markupExtension41 = translate27;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 3];
			array41[0] = section7;
			array41[1] = settingsView;
			array41[2] = this;
			object obj76;
			xamlServiceProvider41.Add(typeFromHandle81, obj76 = new SimpleValueTargetProvider(array41, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 25)));
			object obj77 = markupExtension41.ProvideValue(xamlServiceProvider41);
			section7.Title = obj77;
			bindingExtension20.Path = "ChartDisplayStyle";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, ChartItemTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ChartItemTypes, bool>(A_0.ChartDisplayStyle, true);
				}
				return default(ValueTuple<ChartItemTypes, bool>);
			}, delegate(SharedSettings A_0, ChartItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartDisplayStyle = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartDisplayStyle")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			section7.SetBinding(RadioCell.SelectedValueProperty, bindingBase20);
			translate28.Text = "Settings_ChartStyle_FastLine";
			IMarkupExtension markupExtension42 = translate28;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = radioCell8;
			array42[1] = section7;
			array42[2] = settingsView;
			array42[3] = this;
			object obj78;
			xamlServiceProvider42.Add(typeFromHandle83, obj78 = new SimpleValueTargetProvider(array42, CellBase.TitleProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 31)));
			object obj79 = markupExtension42.ProvideValue(xamlServiceProvider42);
			radioCell8.Title = obj79;
			radioCell8.SetValue(RadioCell.ValueProperty, chartItemTypes);
			section7.Add(radioCell8);
			translate29.Text = "Settings_ChartStyle_Spline";
			IMarkupExtension markupExtension43 = translate29;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = radioCell9;
			array43[1] = section7;
			array43[2] = settingsView;
			array43[3] = this;
			object obj80;
			xamlServiceProvider43.Add(typeFromHandle85, obj80 = new SimpleValueTargetProvider(array43, CellBase.TitleProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 31)));
			object obj81 = markupExtension43.ProvideValue(xamlServiceProvider43);
			radioCell9.Title = obj81;
			radioCell9.SetValue(RadioCell.ValueProperty, chartItemTypes2);
			section7.Add(radioCell9);
			translate30.Text = "Settings_ChartStyle_Area";
			IMarkupExtension markupExtension44 = translate30;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = radioCell10;
			array44[1] = section7;
			array44[2] = settingsView;
			array44[3] = this;
			object obj82;
			xamlServiceProvider44.Add(typeFromHandle87, obj82 = new SimpleValueTargetProvider(array44, CellBase.TitleProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(163, 31)));
			object obj83 = markupExtension44.ProvideValue(xamlServiceProvider44);
			radioCell10.Title = obj83;
			radioCell10.SetValue(RadioCell.ValueProperty, chartItemTypes3);
			section7.Add(radioCell10);
			translate31.Text = "Settings_ChartStyle_SplineArea";
			IMarkupExtension markupExtension45 = translate31;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 4];
			array45[0] = radioCell11;
			array45[1] = section7;
			array45[2] = settingsView;
			array45[3] = this;
			object obj84;
			xamlServiceProvider45.Add(typeFromHandle89, obj84 = new SimpleValueTargetProvider(array45, CellBase.TitleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj84);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 31)));
			object obj85 = markupExtension45.ProvideValue(xamlServiceProvider45);
			radioCell11.Title = obj85;
			radioCell11.SetValue(RadioCell.ValueProperty, chartItemTypes4);
			section7.Add(radioCell11);
			settingsView.Root.Add(section7);
			translate32.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.Header";
			IMarkupExtension markupExtension46 = translate32;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 3];
			array46[0] = section8;
			array46[1] = settingsView;
			array46[2] = this;
			object obj86;
			xamlServiceProvider46.Add(typeFromHandle91, obj86 = new SimpleValueTargetProvider(array46, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj86);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 25)));
			object obj87 = markupExtension46.ProvideValue(xamlServiceProvider46);
			section8.Title = obj87;
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "SetChartMinMaxOnlyVisibleArea";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SetChartMinMaxOnlyVisibleArea = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SetChartMinMaxOnlyVisibleArea")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			section8.SetBinding(RadioCell.SelectedValueProperty, bindingBase21);
			translate33.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OnContent";
			IMarkupExtension markupExtension47 = translate33;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = radioCell12;
			array47[1] = section8;
			array47[2] = settingsView;
			array47[3] = this;
			object obj88;
			xamlServiceProvider47.Add(typeFromHandle93, obj88 = new SimpleValueTargetProvider(array47, CellBase.TitleProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 31)));
			object obj89 = markupExtension47.ProvideValue(xamlServiceProvider47);
			radioCell12.Title = obj89;
			staticResourceExtension10.Key = "TrueValue";
			IMarkupExtension markupExtension48 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = radioCell12;
			array48[1] = section8;
			array48[2] = settingsView;
			array48[3] = this;
			object obj90;
			xamlServiceProvider48.Add(typeFromHandle95, obj90 = new SimpleValueTargetProvider(array48, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj90);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver48.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 125)));
			object obj91 = markupExtension48.ProvideValue(xamlServiceProvider48);
			radioCell12.SetValue(RadioCell.ValueProperty, obj91);
			section8.Add(radioCell12);
			translate34.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OffContent";
			IMarkupExtension markupExtension49 = translate34;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 4];
			array49[0] = radioCell13;
			array49[1] = section8;
			array49[2] = settingsView;
			array49[3] = this;
			object obj92;
			xamlServiceProvider49.Add(typeFromHandle97, obj92 = new SimpleValueTargetProvider(array49, CellBase.TitleProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj92);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver49.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 31)));
			object obj93 = markupExtension49.ProvideValue(xamlServiceProvider49);
			radioCell13.Title = obj93;
			staticResourceExtension11.Key = "FalseValue";
			IMarkupExtension markupExtension50 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 4];
			array50[0] = radioCell13;
			array50[1] = section8;
			array50[2] = settingsView;
			array50[3] = this;
			object obj94;
			xamlServiceProvider50.Add(typeFromHandle99, obj94 = new SimpleValueTargetProvider(array50, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj94);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver50.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver50.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 126)));
			object obj95 = markupExtension50.ProvideValue(xamlServiceProvider50);
			radioCell13.SetValue(RadioCell.ValueProperty, obj95);
			section8.Add(radioCell13);
			settingsView.Root.Add(section8);
			translate35.Text = "settings_LiveDataListPageUpdateOnlyVisible_Title";
			IMarkupExtension markupExtension51 = translate35;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 3];
			array51[0] = section9;
			array51[1] = settingsView;
			array51[2] = this;
			object obj96;
			xamlServiceProvider51.Add(typeFromHandle101, obj96 = new SimpleValueTargetProvider(array51, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj96);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver51.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver51.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 25)));
			object obj97 = markupExtension51.ProvideValue(xamlServiceProvider51);
			section9.Title = obj97;
			bindingExtension22.Path = "LiveDataListPageUpdateOnlyVisible";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LiveDataListPageUpdateOnlyVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LiveDataListPageUpdateOnlyVisible = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "LiveDataListPageUpdateOnlyVisible")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			section9.SetBinding(RadioCell.SelectedValueProperty, bindingBase22);
			translate36.Text = "settings_LiveDataListPageUpdateOnlyVisible";
			IMarkupExtension markupExtension52 = translate36;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 4];
			array52[0] = radioCell14;
			array52[1] = section9;
			array52[2] = settingsView;
			array52[3] = this;
			object obj98;
			xamlServiceProvider52.Add(typeFromHandle103, obj98 = new SimpleValueTargetProvider(array52, CellBase.TitleProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj98);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver52.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver52.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver52.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 31)));
			object obj99 = markupExtension52.ProvideValue(xamlServiceProvider52);
			radioCell14.Title = obj99;
			staticResourceExtension12.Key = "TrueValue";
			IMarkupExtension markupExtension53 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 4];
			array53[0] = radioCell14;
			array53[1] = section9;
			array53[2] = settingsView;
			array53[3] = this;
			object obj100;
			xamlServiceProvider53.Add(typeFromHandle105, obj100 = new SimpleValueTargetProvider(array53, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj100);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver53.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver53.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver53.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 105)));
			object obj101 = markupExtension53.ProvideValue(xamlServiceProvider53);
			radioCell14.SetValue(RadioCell.ValueProperty, obj101);
			section9.Add(radioCell14);
			translate37.Text = "settings_LiveDataListPageUpdateAllSensors";
			IMarkupExtension markupExtension54 = translate37;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 4];
			array54[0] = radioCell15;
			array54[1] = section9;
			array54[2] = settingsView;
			array54[3] = this;
			object obj102;
			xamlServiceProvider54.Add(typeFromHandle107, obj102 = new SimpleValueTargetProvider(array54, CellBase.TitleProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj102);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver54.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver54.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver54.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 31)));
			object obj103 = markupExtension54.ProvideValue(xamlServiceProvider54);
			radioCell15.Title = obj103;
			staticResourceExtension13.Key = "FalseValue";
			IMarkupExtension markupExtension55 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 4];
			array55[0] = radioCell15;
			array55[1] = section9;
			array55[2] = settingsView;
			array55[3] = this;
			object obj104;
			xamlServiceProvider55.Add(typeFromHandle109, obj104 = new SimpleValueTargetProvider(array55, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj104);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver55.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver55.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver55.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 104)));
			object obj105 = markupExtension55.ProvideValue(xamlServiceProvider55);
			radioCell15.SetValue(RadioCell.ValueProperty, obj105);
			section9.Add(radioCell15);
			settingsView.Root.Add(section9);
			translate38.Text = "settings_DataRecordLineSplitterTime";
			IMarkupExtension markupExtension56 = translate38;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 3];
			array56[0] = section10;
			array56[1] = settingsView;
			array56[2] = this;
			object obj106;
			xamlServiceProvider56.Add(typeFromHandle111, obj106 = new SimpleValueTargetProvider(array56, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj106);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver56.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver56.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver56.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(179, 25)));
			object obj107 = markupExtension56.ProvideValue(xamlServiceProvider56);
			section10.Title = obj107;
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			entry2.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension23.Mode = 1;
			staticResourceExtension14.Key = "IntToStringConverter";
			IMarkupExtension markupExtension57 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 6];
			array57[0] = bindingExtension23;
			array57[1] = entry2;
			array57[2] = customCell2;
			array57[3] = section10;
			array57[4] = settingsView;
			array57[5] = this;
			object obj108;
			xamlServiceProvider57.Add(typeFromHandle113, obj108 = new SimpleValueTargetProvider(array57, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj108);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver57.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver57.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver57.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(181, 47)));
			object obj109 = markupExtension57.ProvideValue(xamlServiceProvider57);
			bindingExtension23.Converter = obj109;
			bindingExtension23.Path = "DataRecordLineSplitterTime";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.DataRecordLineSplitterTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DataRecordLineSplitterTime")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase23);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MinimumValueProperty, 0.0);
			((ICollection<Behavior>)entry2.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior2);
			customCell2.SetValue(CustomCell.ContentProperty, entry2);
			section10.Add(customCell2);
			settingsView.Root.Add(section10);
			section11.SetValue(SectionBase.TitleProperty, "CarPlay/AndroidAuto refresh rate");
			section11.SetValue(Section.IsVisibleProperty, false);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			entry3.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension24.Mode = 1;
			staticResourceExtension15.Key = "IntToStringConverter";
			IMarkupExtension markupExtension58 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 6];
			array58[0] = bindingExtension24;
			array58[1] = entry3;
			array58[2] = customCell3;
			array58[3] = section11;
			array58[4] = settingsView;
			array58[5] = this;
			object obj110;
			xamlServiceProvider58.Add(typeFromHandle115, obj110 = new SimpleValueTargetProvider(array58, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj110);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver58.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver58.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver58.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(SettingsInterfacePageV3).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(198, 47)));
			object obj111 = markupExtension58.ProvideValue(xamlServiceProvider58);
			bindingExtension24.Converter = obj111;
			bindingExtension24.Path = "CarPlayUpdateInterval";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.CarPlayUpdateInterval, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.CarPlayUpdateInterval = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CarPlayUpdateInterval")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase24);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MinimumValueProperty, 0.0);
			((ICollection<Behavior>)entry3.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior3);
			customCell3.SetValue(CustomCell.ContentProperty, entry3);
			section11.Add(customCell3);
			settingsView.Root.Add(section11);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x0018D3E4 File Offset: 0x0018B5E4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsInterfacePageV3>(this, typeof(SettingsInterfacePageV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.sectionLanguage = NameScopeExtensions.FindByName<Section>(this, "sectionLanguage");
			this.btnFontSizePlus = NameScopeExtensions.FindByName<ButtonCell>(this, "btnFontSizePlus");
			this.btnFontSizeMinus = NameScopeExtensions.FindByName<ButtonCell>(this, "btnFontSizeMinus");
			this.btnMainScreenConfiguration = NameScopeExtensions.FindByName<ButtonCell>(this, "btnMainScreenConfiguration");
			this.carPlayRefreshRateSection = NameScopeExtensions.FindByName<Section>(this, "carPlayRefreshRateSection");
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x0018D468 File Offset: 0x0018B668
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2028(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.Language, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002129 RID: 8489 RVA: 0x0018D498 File Offset: 0x0018B698
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2029(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.Language = A_1;
				return;
			}
		}

		// Token: 0x0600212A RID: 8490 RVA: 0x0018D4B4 File Offset: 0x0018B6B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2030(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600212B RID: 8491 RVA: 0x0018D4C4 File Offset: 0x0018B6C4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2031(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.Language, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600212C RID: 8492 RVA: 0x0018D4F4 File Offset: 0x0018B6F4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2032(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.Language = A_1;
				return;
			}
		}

		// Token: 0x0600212D RID: 8493 RVA: 0x0018D510 File Offset: 0x0018B710
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2033(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600212E RID: 8494 RVA: 0x0018D520 File Offset: 0x0018B720
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2034(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DarkMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600212F RID: 8495 RVA: 0x0018D550 File Offset: 0x0018B750
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2035(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DarkMode = A_1;
				return;
			}
		}

		// Token: 0x06002130 RID: 8496 RVA: 0x0018D56C File Offset: 0x0018B76C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2036(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002131 RID: 8497 RVA: 0x0018D57C File Offset: 0x0018B77C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2037(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002132 RID: 8498 RVA: 0x0018D5AC File Offset: 0x0018B7AC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2038(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AutomaticallySwitchTheme = A_1;
				return;
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x0018D5C8 File Offset: 0x0018B7C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2039(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x0018D5D8 File Offset: 0x0018B7D8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2040(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchThemeAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0018D608 File Offset: 0x0018B808
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2041(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x0018D618 File Offset: 0x0018B818
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2042(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x0018D648 File Offset: 0x0018B848
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2043(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x0018D658 File Offset: 0x0018B858
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2044(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x0018D688 File Offset: 0x0018B888
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2045(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x0018D698 File Offset: 0x0018B898
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2046(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowPing, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x0018D6C8 File Offset: 0x0018B8C8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2047(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowPing = A_1;
				return;
			}
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0018D6E4 File Offset: 0x0018B8E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2048(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x0018D6F4 File Offset: 0x0018B8F4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2049(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowConnectionStatusOverlay, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x0018D724 File Offset: 0x0018B924
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2050(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowConnectionStatusOverlay = A_1;
				return;
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0018D740 File Offset: 0x0018B940
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2051(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0018D750 File Offset: 0x0018B950
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2052(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.FontSizePatch, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x0018D780 File Offset: 0x0018B980
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2053(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0018D790 File Offset: 0x0018B990
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2054(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x0018D7C0 File Offset: 0x0018B9C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2055(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidRecolorNavBar = A_1;
				return;
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0018D7DC File Offset: 0x0018B9DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2056(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x0018D7EC File Offset: 0x0018B9EC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2057(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PIDSelectorWithValuePreview, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x0018D81C File Offset: 0x0018BA1C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2058(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PIDSelectorWithValuePreview = A_1;
				return;
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0018D838 File Offset: 0x0018BA38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2059(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x0018D848 File Offset: 0x0018BA48
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2060(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ChartsView, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0018D878 File Offset: 0x0018BA78
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2061(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartsView = A_1;
				return;
			}
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x0018D894 File Offset: 0x0018BA94
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2062(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0018D8A4 File Offset: 0x0018BAA4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2063(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x0018D8D4 File Offset: 0x0018BAD4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2064(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartShowAverageValue = A_1;
				return;
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x0018D8F0 File Offset: 0x0018BAF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2065(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x0018D900 File Offset: 0x0018BB00
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2066(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x0018D930 File Offset: 0x0018BB30
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2067(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowMinMaxValues = A_1;
				return;
			}
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0018D94C File Offset: 0x0018BB4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2068(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0018D95C File Offset: 0x0018BB5C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2069(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.MultiChartPauseHidden, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x0018D98C File Offset: 0x0018BB8C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2070(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.MultiChartPauseHidden = A_1;
				return;
			}
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0018D9A8 File Offset: 0x0018BBA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2071(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x0018D9B8 File Offset: 0x0018BBB8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2072(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidChartRenderingSafeMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x0018D9E8 File Offset: 0x0018BBE8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2073(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidChartRenderingSafeMode = A_1;
				return;
			}
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0018DA04 File Offset: 0x0018BC04
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2074(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x0018DA14 File Offset: 0x0018BC14
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2075(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x0018DA44 File Offset: 0x0018BC44
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2076(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.LiveDataShowTime = A_1;
				return;
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x0018DA60 File Offset: 0x0018BC60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2077(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0018DA70 File Offset: 0x0018BC70
		[CompilerGenerated]
		private static ValueTuple<ChartItemTypes, bool> <InitializeComponent>typedBindingsM__2078(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ChartItemTypes, bool>(A_0.ChartDisplayStyle, true);
			}
			return default(ValueTuple<ChartItemTypes, bool>);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x0018DAA0 File Offset: 0x0018BCA0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2079(SharedSettings A_0, ChartItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartDisplayStyle = A_1;
				return;
			}
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x0018DABC File Offset: 0x0018BCBC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2080(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x0018DACC File Offset: 0x0018BCCC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2081(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x0018DAFC File Offset: 0x0018BCFC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2082(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SetChartMinMaxOnlyVisibleArea = A_1;
				return;
			}
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x0018DB18 File Offset: 0x0018BD18
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2083(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x0018DB28 File Offset: 0x0018BD28
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2084(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LiveDataListPageUpdateOnlyVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x0018DB58 File Offset: 0x0018BD58
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2085(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LiveDataListPageUpdateOnlyVisible = A_1;
				return;
			}
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x0018DB74 File Offset: 0x0018BD74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2086(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x0018DB84 File Offset: 0x0018BD84
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2087(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x0018DBB4 File Offset: 0x0018BDB4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2088(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.DataRecordLineSplitterTime = A_1;
				return;
			}
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x0018DBD0 File Offset: 0x0018BDD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2089(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002166 RID: 8550 RVA: 0x0018DBE0 File Offset: 0x0018BDE0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2090(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.CarPlayUpdateInterval, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002167 RID: 8551 RVA: 0x0018DC10 File Offset: 0x0018BE10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2091(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.CarPlayUpdateInterval = A_1;
				return;
			}
		}

		// Token: 0x06002168 RID: 8552 RVA: 0x0018DC2C File Offset: 0x0018BE2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2092(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000FDC RID: 4060
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000FDD RID: 4061
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section sectionLanguage;

		// Token: 0x04000FDE RID: 4062
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnFontSizePlus;

		// Token: 0x04000FDF RID: 4063
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnFontSizeMinus;

		// Token: 0x04000FE0 RID: 4064
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnMainScreenConfiguration;

		// Token: 0x04000FE1 RID: 4065
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section carPlayRefreshRateSection;

		// Token: 0x020002A7 RID: 679
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyLanguage_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002169 RID: 8553 RVA: 0x0018DC3C File Offset: 0x0018BE3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (App.OBDSimulator.IsActive)
						{
							App.OBDSimulator.Stop();
						}
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
						{
							goto IL_008D;
						}
						taskAwaiter = App.OBDReader.Disconnect("btnApplyLanguage_Clicked").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsInterfacePageV3.<btnApplyLanguage_Clicked>d__3>(ref taskAwaiter, ref this);
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
					IL_008D:
					App.Instance.ChangeLanguage();
					SharedSettings.Current.LanguageChanged = false;
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

			// Token: 0x0600216A RID: 8554 RVA: 0x0018DD28 File Offset: 0x0018BF28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FE2 RID: 4066
			public int <>1__state;

			// Token: 0x04000FE3 RID: 4067
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FE4 RID: 4068
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002A8 RID: 680
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnMainScreenConfiguration_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600216B RID: 8555 RVA: 0x0018DD38 File Offset: 0x0018BF38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsInterfacePageV3 settingsInterfacePageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						(sender as Cell).IsEnabled = false;
						taskAwaiter = settingsInterfacePageV.Navigation.PushAsync(new MainPageConfigurationScreen()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsInterfacePageV3.<btnMainScreenConfiguration_Clicked>d__5>(ref taskAwaiter, ref this);
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
					(sender as Cell).IsEnabled = true;
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

			// Token: 0x0600216C RID: 8556 RVA: 0x0018DE18 File Offset: 0x0018C018
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FE5 RID: 4069
			public int <>1__state;

			// Token: 0x04000FE6 RID: 4070
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FE7 RID: 4071
			public object sender;

			// Token: 0x04000FE8 RID: 4072
			public SettingsInterfacePageV3 <>4__this;

			// Token: 0x04000FE9 RID: 4073
			private TaskAwaiter <>u__1;
		}
	}
}
