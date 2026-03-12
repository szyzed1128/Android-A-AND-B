using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x0200028A RID: 650
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml")]
	public class SettingsConnectionExpertV3 : ContentPage
	{
		// Token: 0x06001EF6 RID: 7926 RVA: 0x00153ECB File Offset: 0x001520CB
		public SettingsConnectionExpertV3()
		{
			this.InitializeComponent();
			base.BindingContext = SharedSettings.Current;
			base.Appearing += this.SettingsConnectionExpertV3_Appearing;
			base.Disappearing += this.SettingsConnectionExpertV3_Disappearing;
		}

		// Token: 0x06001EF7 RID: 7927 RVA: 0x00153F08 File Offset: 0x00152108
		private void SettingsConnectionExpertV3_Disappearing(object sender, EventArgs e)
		{
			try
			{
				base.BindingContext = null;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001EF8 RID: 7928 RVA: 0x00153F34 File Offset: 0x00152134
		private void SettingsConnectionExpertV3_Appearing(object sender, EventArgs e)
		{
			try
			{
				if (base.BindingContext == null)
				{
					base.BindingContext = SharedSettings.Current;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001EF9 RID: 7929 RVA: 0x00153F6C File Offset: 0x0015216C
		private async void btnForceMode22Learning_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				this.btnForceMode22Learning.IsEnabled = false;
				await OBDRequestQueueOptimizer.ForceLearnOptimizationDictionary();
				this.btnForceMode22Learning.IsEnabled = true;
			}
			else
			{
				await base.DisplayAlert("Please connect to your car ECU first!", "Connection required!", "OK");
			}
		}

		// Token: 0x06001EFA RID: 7930 RVA: 0x000D7C73 File Offset: 0x000D5E73
		private void btnClearOptimizationData_Clicked(object sender, EventArgs e)
		{
			OBDRequestQueueOptimizer.ClearOptimizationDictionary();
		}

		// Token: 0x06001EFB RID: 7931 RVA: 0x00153FA4 File Offset: 0x001521A4
		private async void cellInitStringEditor_Tapped(object sender, EventArgs e)
		{
			await base.Navigation.PushAsync(new InitSequenceEditorPage());
		}

		// Token: 0x06001EFC RID: 7932 RVA: 0x00153FDC File Offset: 0x001521DC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsConnectionExpertV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			ConnectionTypeToWiFiVisibleBoolConverter connectionTypeToWiFiVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverter = new ConnectionTypeToWiFiVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ConnectionTypeToBTLEVisibleBoolConverter connectionTypeToBTLEVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTLEVisibleBoolConverter = new ConnectionTypeToBTLEVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ConnectionTypeToBTVisibleBoolConverter connectionTypeToBTVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverter = new ConnectionTypeToBTVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ConnectionTypeToMFIBluetoothConverter connectionTypeToMFIBluetoothConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToMFIBluetoothConverter = new ConnectionTypeToMFIBluetoothConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ConnectionTypeToIntConverter connectionTypeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToIntConverter = new ConnectionTypeToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			AndroidWiFiConnectionModesToIntConverter androidWiFiConnectionModesToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(androidWiFiConnectionModesToIntConverter = new AndroidWiFiConnectionModesToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			EnumToTranslationTitleConverter enumToTranslationTitleConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToTranslationTitleConverter = new EnumToTranslationTitleConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid connectionTypeToWiFiVisibleBoolConverterOnlyAndroid;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverterOnlyAndroid = new ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid connectionTypeToBTVisibleBoolConverterOnlyForAndroid;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverterOnlyForAndroid = new ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			IntToStringInItemsConverter intToStringInItemsConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringInItemsConverter = new IntToStringInItemsConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 93);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 31);
			bool flag = true;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 31);
			bool flag2 = false;
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 21);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 115);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 115);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 32);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 82);
			List<string> protocols;
			VisualDiagnostics.RegisterSourceInfo(protocols = StaticLists.Protocols, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 82);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 82);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 26);
			List<string> protocols2;
			VisualDiagnostics.RegisterSourceInfo(protocols2 = StaticLists.Protocols, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 29);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 29);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 49);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 105);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 105);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 14);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 96);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 32);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 82);
			List<string> adaptiveTimingsList;
			VisualDiagnostics.RegisterSourceInfo(adaptiveTimingsList = StaticLists.AdaptiveTimingsList, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 82);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 82);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 26);
			List<string> adaptiveTimingsList2;
			VisualDiagnostics.RegisterSourceInfo(adaptiveTimingsList2 = StaticLists.AdaptiveTimingsList, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 29);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 29);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 26);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker2;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker2 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 85);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 32);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 82);
			List<string> atstlist;
			VisualDiagnostics.RegisterSourceInfo(atstlist = StaticLists.ATSTList, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 82);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 82);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 26);
			List<string> atstlist2;
			VisualDiagnostics.RegisterSourceInfo(atstlist2 = StaticLists.ATSTList, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 29);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 26);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker3;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker3 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 14);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 25);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 97);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 97);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 25);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 25);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 25);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 14);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 40);
			EntryCell entryCell;
			VisualDiagnostics.RegisterSourceInfo(entryCell = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 14);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 25);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 40);
			EntryCell entryCell2;
			VisualDiagnostics.RegisterSourceInfo(entryCell2 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 14);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 25);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 40);
			EntryCell entryCell3;
			VisualDiagnostics.RegisterSourceInfo(entryCell3 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 14);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 25);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 49);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 116);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 18);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 49);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 118);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 49);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 137);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 18);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 49);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 120);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 21);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 21);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 21);
			NumberPickerCell numberPickerCell;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 21);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 21);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched8;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched8 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 21);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 21);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 21);
			NumberPickerCell numberPickerCell2;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell2 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 21);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 21);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched9;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched9 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 18);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 21);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched10;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched10 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 49);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 120);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched11;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched11 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 18);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 21);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 18);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 21);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 18);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 49);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 113);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched12;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched12 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 18);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 49);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 107);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched13;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched13 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 14);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 25);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 49);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 120);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched14;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched14 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 18);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 49);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 111);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched15;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched15 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 18);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 21);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 21);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched16;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched16 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 18);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 49);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 107);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched17;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched17 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 18);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 49);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 112);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched18;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched18 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 18);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 49);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 113);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched19;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched19 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 18);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 49);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 131);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched20;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched20 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 18);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 49);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 110);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched21;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched21 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 18);
			BindingExtension bindingExtension50;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension50 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 108);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched22;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched22 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 18);
			BindingExtension bindingExtension51;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension51 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 133);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched23;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched23 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 18);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 21);
			BindingExtension bindingExtension52;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension52 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched24;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched24 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 18);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 21);
			BindingExtension bindingExtension53;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension53 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched25;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched25 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 18);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 21);
			BindingExtension bindingExtension54;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension54 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched26;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched26 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 14);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 25);
			BindingExtension bindingExtension55;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension55 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 89);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 31);
			FlowControlOverrides flowControlOverrides = FlowControlOverrides.Off;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 18);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 31);
			FlowControlOverrides flowControlOverrides2 = FlowControlOverrides.ForceOnFor7Ex;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 18);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 31);
			FlowControlOverrides flowControlOverrides3 = FlowControlOverrides.ForceOffFor7Ex;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 18);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 31);
			FlowControlOverrides flowControlOverrides4 = FlowControlOverrides.ForceOnForAll;
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 18);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 31);
			FlowControlOverrides flowControlOverrides5 = FlowControlOverrides.ForceOffForAll;
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 18);
			Section section11;
			VisualDiagnostics.RegisterSourceInfo(section11 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 14);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 21);
			BindingExtension bindingExtension56;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension56 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 21);
			BindingExtension bindingExtension57;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension57 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched27;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched27 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 18);
			Translate translate49;
			VisualDiagnostics.RegisterSourceInfo(translate49 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 49);
			BindingExtension bindingExtension58;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension58 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 99);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched28;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched28 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 18);
			Section section12;
			VisualDiagnostics.RegisterSourceInfo(section12 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 14);
			Translate translate50;
			VisualDiagnostics.RegisterSourceInfo(translate50 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 25);
			BindingExtension bindingExtension59;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension59 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 40);
			EntryCell entryCell4;
			VisualDiagnostics.RegisterSourceInfo(entryCell4 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 18);
			Section section13;
			VisualDiagnostics.RegisterSourceInfo(section13 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 14);
			Translate translate51;
			VisualDiagnostics.RegisterSourceInfo(translate51 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 49);
			BindingExtension bindingExtension60;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension60 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 122);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched29;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched29 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 18);
			Translate translate52;
			VisualDiagnostics.RegisterSourceInfo(translate52 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 300, 21);
			BindingExtension bindingExtension61;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension61 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 303, 21);
			NumberPickerCell numberPickerCell3;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell3 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 18);
			BindingExtension bindingExtension62;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension62 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 77);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched30;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched30 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 18);
			Translate translate53;
			VisualDiagnostics.RegisterSourceInfo(translate53 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 49);
			BindingExtension bindingExtension63;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension63 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 102);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched31;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched31 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 18);
			Translate translate54;
			VisualDiagnostics.RegisterSourceInfo(translate54 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 308, 21);
			Translate translate55;
			VisualDiagnostics.RegisterSourceInfo(translate55 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 21);
			BindingExtension bindingExtension64;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension64 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 21);
			NumberPickerCell numberPickerCell4;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell4 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 307, 18);
			Section section14;
			VisualDiagnostics.RegisterSourceInfo(section14 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 14);
			Translate translate56;
			VisualDiagnostics.RegisterSourceInfo(translate56 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 25);
			BindingExtension bindingExtension65;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension65 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 98);
			Translate translate57;
			VisualDiagnostics.RegisterSourceInfo(translate57 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 31);
			ReadPartialErrorActions readPartialErrorActions = ReadPartialErrorActions.FullReset;
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 18);
			Translate translate58;
			VisualDiagnostics.RegisterSourceInfo(translate58 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 31);
			ReadPartialErrorActions readPartialErrorActions2 = ReadPartialErrorActions.Ignore;
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 18);
			Translate translate59;
			VisualDiagnostics.RegisterSourceInfo(translate59 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 31);
			ReadPartialErrorActions readPartialErrorActions3 = ReadPartialErrorActions.ResetConnection;
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 18);
			Section section15;
			VisualDiagnostics.RegisterSourceInfo(section15 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 14);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 40);
			BindingExtension bindingExtension66;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension66 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 40);
			EntryCell entryCell5;
			VisualDiagnostics.RegisterSourceInfo(entryCell5 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 18);
			Section section16;
			VisualDiagnostics.RegisterSourceInfo(section16 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 14);
			Translate translate60;
			VisualDiagnostics.RegisterSourceInfo(translate60 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 328, 25);
			BindingExtension bindingExtension67;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension67 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 328, 86);
			BindingExtension bindingExtension68;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension68 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 25);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 329, 18);
			Section section17;
			VisualDiagnostics.RegisterSourceInfo(section17 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 328, 14);
			BindingExtension bindingExtension69;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension69 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 64);
			BindingExtension bindingExtension70;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension70 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 25);
			BindingExtension bindingExtension71;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension71 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 25);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 18);
			Section section18;
			VisualDiagnostics.RegisterSourceInfo(section18 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 14);
			Translate translate61;
			VisualDiagnostics.RegisterSourceInfo(translate61 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 25);
			BindingExtension bindingExtension72;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension72 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 94);
			BindingExtension bindingExtension73;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension73 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 352, 25);
			BindingExtension bindingExtension74;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension74 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 25);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 22);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 18);
			Section section19;
			VisualDiagnostics.RegisterSourceInfo(section19 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 14);
			Translate translate62;
			VisualDiagnostics.RegisterSourceInfo(translate62 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 17);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 17);
			BindingExtension bindingExtension75;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension75 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 17);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 32);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 367, 37);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 50);
			string text = "Auto";
			string text2 = "Modern (bind)";
			string text3 = "Legacy (bind)";
			string text4 = "Modern (don't bind)";
			string text5 = "Legacy (don't bind)";
			ArrayExtension arrayExtension;
			(arrayExtension = new ArrayExtension()).Type = typeFromHandle;
			arrayExtension.Items.Add(text);
			arrayExtension.Items.Add(text2);
			arrayExtension.Items.Add(text3);
			arrayExtension.Items.Add(text4);
			arrayExtension.Items.Add(text5);
			string[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new string[] { text, text2, text3, text4, text5 }, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 42);
			BindingExtension bindingExtension76;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension76 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 366, 34);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 26);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 51);
			BindingExtension bindingExtension77;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension77 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 51);
			Type typeFromHandle2;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle2 = typeof(string), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 384, 42);
			string text6 = "Auto";
			string text7 = "Modern (bind)";
			string text8 = "Legacy (bind)";
			string text9 = "Modern (don't bind)";
			string text10 = "Legacy (don't bind)";
			ArrayExtension arrayExtension2;
			(arrayExtension2 = new ArrayExtension()).Type = typeFromHandle2;
			arrayExtension2.Items.Add(text6);
			arrayExtension2.Items.Add(text7);
			arrayExtension2.Items.Add(text8);
			arrayExtension2.Items.Add(text9);
			arrayExtension2.Items.Add(text10);
			string[] array2;
			VisualDiagnostics.RegisterSourceInfo(array2 = new string[] { text6, text7, text8, text9, text10 }, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 384, 34);
			Picker picker4;
			VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 26);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker4;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker4 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 362, 18);
			Section section20;
			VisualDiagnostics.RegisterSourceInfo(section20 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 14);
			Translate translate63;
			VisualDiagnostics.RegisterSourceInfo(translate63 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 400, 17);
			StaticResourceExtension staticResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 17);
			BindingExtension bindingExtension78;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension78 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 17);
			StaticResourceExtension staticResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 32);
			StaticResourceExtension staticResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension19 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 82);
			List<string> androidConnectionMethods;
			VisualDiagnostics.RegisterSourceInfo(androidConnectionMethods = StaticLists.AndroidConnectionMethods, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 82);
			BindingExtension bindingExtension79;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension79 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 82);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 26);
			List<string> androidConnectionMethods2;
			VisualDiagnostics.RegisterSourceInfo(androidConnectionMethods2 = StaticLists.AndroidConnectionMethods, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 408, 29);
			BindingExtension bindingExtension80;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension80 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 408, 29);
			BindingExtension bindingExtension81;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension81 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 409, 29);
			Picker picker5;
			VisualDiagnostics.RegisterSourceInfo(picker5 = new Picker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 406, 26);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 404, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker5;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker5 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 403, 18);
			Translate translate64;
			VisualDiagnostics.RegisterSourceInfo(translate64 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 49);
			BindingExtension bindingExtension82;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension82 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 112);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched32;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched32 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 412, 18);
			Translate translate65;
			VisualDiagnostics.RegisterSourceInfo(translate65 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 49);
			BindingExtension bindingExtension83;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension83 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 105);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched33;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched33 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 18);
			Translate translate66;
			VisualDiagnostics.RegisterSourceInfo(translate66 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 49);
			BindingExtension bindingExtension84;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension84 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 109);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched34;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched34 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 18);
			Section section21;
			VisualDiagnostics.RegisterSourceInfo(section21 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 398, 14);
			StaticResourceExtension staticResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension20 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 421, 17);
			BindingExtension bindingExtension85;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension85 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 421, 17);
			Translate translate67;
			VisualDiagnostics.RegisterSourceInfo(translate67 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 422, 49);
			BindingExtension bindingExtension86;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension86 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 422, 116);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched35;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched35 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 422, 18);
			Section section22;
			VisualDiagnostics.RegisterSourceInfo(section22 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 418, 14);
			BindingExtension bindingExtension87;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension87 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 58);
			EntryCell entryCell6;
			VisualDiagnostics.RegisterSourceInfo(entryCell6 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 18);
			StaticResourceExtension staticResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension21 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 65);
			BindingExtension bindingExtension88;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension88 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 65);
			EntryCell entryCell7;
			VisualDiagnostics.RegisterSourceInfo(entryCell7 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 18);
			StaticResourceExtension staticResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension22 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 55);
			BindingExtension bindingExtension89;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension89 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 55);
			EntryCell entryCell8;
			VisualDiagnostics.RegisterSourceInfo(entryCell8 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 18);
			Section section23;
			VisualDiagnostics.RegisterSourceInfo(section23 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsConnectionExpertV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("DefaultConnectionPanel", section2);
			if (section2.StyleId == null)
			{
				section2.StyleId = "DefaultConnectionPanel";
			}
			nameScope.RegisterName("btnForceMode22Learning", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnForceMode22Learning";
			}
			nameScope.RegisterName("btnClearOptimizationData", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnClearOptimizationData";
			}
			nameScope.RegisterName("wifiPanel", section20);
			if (section20.StyleId == null)
			{
				section20.StyleId = "wifiPanel";
			}
			nameScope.RegisterName("bluetooth2Panel", section21);
			if (section21.StyleId == null)
			{
				section21.StyleId = "bluetooth2Panel";
			}
			nameScope.RegisterName("bluetoothLEPanel", section22);
			if (section22.StyleId == null)
			{
				section22.StyleId = "bluetoothLEPanel";
			}
			this.settingsLayoutRoot = settingsView;
			this.DefaultConnectionPanel = section2;
			this.btnForceMode22Learning = buttonCell;
			this.btnClearOptimizationData = buttonCell2;
			this.wifiPanel = section20;
			this.bluetooth2Panel = section21;
			this.bluetoothLEPanel = section22;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ConnectionTypeToWiFiVisibleBoolConverter", connectionTypeToWiFiVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTLEVisibleBoolConverter", connectionTypeToBTLEVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTVisibleBoolConverter", connectionTypeToBTVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToMFIBluetoothConverter", connectionTypeToMFIBluetoothConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("ConnectionTypeToIntConverter", connectionTypeToIntConverter);
			resourceDictionary.Add("AndroidWiFiConnectionModesToIntConverter", androidWiFiConnectionModesToIntConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("EnumToTranslationTitleConverter", enumToTranslationTitleConverter);
			resourceDictionary.Add("ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid", connectionTypeToWiFiVisibleBoolConverterOnlyAndroid);
			resourceDictionary.Add("ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid", connectionTypeToBTVisibleBoolConverterOnlyForAndroid);
			resourceDictionary.Add("IntToStringInItemsConverter", intToStringInItemsConverter);
			translate.Text = "Settings_Control_tbConnection.Text";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle3, obj = new SimpleValueTargetProvider(array3, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 1];
			array4[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbInitSequence.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = section;
			array5[1] = settingsView;
			array5[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array5, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			bindingExtension.Mode = 1;
			bindingExtension.Path = "UseDefaultInit";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseDefaultInit = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			translate3.Text = "Settings_Control_InitSequenceDefault.Content";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = radioCell;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array6, CellBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 31)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			radioCell.Title = obj7;
			radioCell.SetValue(RadioCell.ValueProperty, flag);
			section.Add(radioCell);
			translate4.Text = "Settings_Control_InitSequenceCustom.Content";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = radioCell2;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array7, CellBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 31)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			radioCell2.Title = obj9;
			radioCell2.SetValue(RadioCell.ValueProperty, flag2);
			section.Add(radioCell2);
			translate5.Text = "Settings_Control_UseOBD2.Header";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = settingsCheckBoxCellPatched;
			array8[1] = section;
			array8[2] = settingsView;
			array8[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array8, CellBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			settingsCheckBoxCellPatched.Title = obj11;
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "UseOBD2";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseOBD2, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseOBD2 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseOBD2")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase2);
			bindingExtension3.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = bindingExtension3;
			array9[1] = settingsCheckBoxCellPatched;
			array9[2] = section;
			array9[3] = settingsView;
			array9[4] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension3.Converter = obj13;
			bindingExtension3.Path = "UseDefaultInit";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CellBase.IsVisibleProperty, bindingBase3);
			section.Add(settingsCheckBoxCellPatched);
			settingsCheckBoxCellPatched2.SetValue(CellBase.TitleProperty, "Decode positive response to confirm successful connection");
			bindingExtension4.Mode = 1;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = bindingExtension4;
			array10[1] = settingsCheckBoxCellPatched2;
			array10[2] = section;
			array10[3] = settingsView;
			array10[4] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 115)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension4.Converter = obj15;
			bindingExtension4.Path = "CheckOnlyPositiveResponseMarker";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CheckOnlyPositiveResponseMarker, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CheckOnlyPositiveResponseMarker = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CheckOnlyPositiveResponseMarker")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase4);
			section.Add(settingsCheckBoxCellPatched2);
			settingsView.Root.Add(section);
			translate6.Text = "Settings_Control_tbECUProtocol.Text";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = section2;
			array11[1] = settingsView;
			array11[2] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array11, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 17)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			section2.Title = obj17;
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "UseDefaultInit";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase5);
			staticResourceExtension3.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label;
			array12[1] = grid;
			array12[2] = settingsCustomCellForPicker;
			array12[3] = section2;
			array12[4] = settingsView;
			array12[5] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle21, obj18 = new SimpleValueTargetProvider(array12, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 32)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label.Style = obj19;
			staticResourceExtension4.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = bindingExtension6;
			array13[1] = label;
			array13[2] = grid;
			array13[3] = settingsCustomCellForPicker;
			array13[4] = section2;
			array13[5] = settingsView;
			array13[6] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle23, obj20 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 82)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension6.Converter = obj21;
			bindingExtension6.ConverterParameter = protocols;
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "ProtocolNumber";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ProtocolNumber, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ProtocolNumber")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase6);
			grid.Children.Add(label);
			picker.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension7.Source = protocols2;
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase7);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "ProtocolNumber";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ProtocolNumber, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ProtocolNumber = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ProtocolNumber")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase8);
			grid.Children.Add(picker);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, grid);
			section2.Add(settingsCustomCellForPicker);
			translate7.Text = "ios_ForceOnlyOneProtocol";
			IMarkupExtension markupExtension12 = translate7;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = settingsCheckBoxCellPatched3;
			array14[1] = section2;
			array14[2] = settingsView;
			array14[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle25, obj22 = new SimpleValueTargetProvider(array14, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 49)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			settingsCheckBoxCellPatched3.Title = obj23;
			bindingExtension9.Mode = 1;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = bindingExtension9;
			array15[1] = settingsCheckBoxCellPatched3;
			array15[2] = section2;
			array15[3] = settingsView;
			array15[4] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array15, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 105)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension9.Converter = obj25;
			bindingExtension9.Path = "ForceOnlyOneProtocol";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ForceOnlyOneProtocol, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ForceOnlyOneProtocol = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ForceOnlyOneProtocol")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase9);
			section2.Add(settingsCheckBoxCellPatched3);
			settingsView.Root.Add(section2);
			translate8.Text = "Settings_Control_tbAdaptiveTimings.Text";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = section3;
			array16[1] = settingsView;
			array16[2] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array16, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 25)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			section3.Title = obj27;
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "UseDefaultInit";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase10);
			staticResourceExtension6.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension15 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = label2;
			array17[1] = grid2;
			array17[2] = settingsCustomCellForPicker2;
			array17[3] = section3;
			array17[4] = settingsView;
			array17[5] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle31, obj28 = new SimpleValueTargetProvider(array17, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 32)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label2.Style = obj29;
			staticResourceExtension7.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = bindingExtension11;
			array18[1] = label2;
			array18[2] = grid2;
			array18[3] = settingsCustomCellForPicker2;
			array18[4] = section3;
			array18[5] = settingsView;
			array18[6] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle33, obj30 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 82)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension11.Converter = obj31;
			bindingExtension11.ConverterParameter = adaptiveTimingsList;
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "AdaptiveTimings";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.AdaptiveTimings, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AdaptiveTimings")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase11);
			grid2.Children.Add(label2);
			picker2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension12.Source = adaptiveTimingsList2;
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			picker2.SetBinding(Picker.ItemsSourceProperty, bindingBase12);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "AdaptiveTimings";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.AdaptiveTimings, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.AdaptiveTimings = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AdaptiveTimings")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase13);
			grid2.Children.Add(picker2);
			settingsCustomCellForPicker2.SetValue(CustomCell.ContentProperty, grid2);
			section3.Add(settingsCustomCellForPicker2);
			settingsView.Root.Add(section3);
			translate9.Text = "Settings_Control_tbATST.Text";
			IMarkupExtension markupExtension17 = translate9;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 3];
			array19[0] = section4;
			array19[1] = settingsView;
			array19[2] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle35, obj32 = new SimpleValueTargetProvider(array19, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 25)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			section4.Title = obj33;
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "UseDefaultInit";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase14);
			staticResourceExtension8.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension18 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = label3;
			array20[1] = grid3;
			array20[2] = settingsCustomCellForPicker3;
			array20[3] = section4;
			array20[4] = settingsView;
			array20[5] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle37, obj34 = new SimpleValueTargetProvider(array20, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 32)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label3.Style = obj35;
			staticResourceExtension9.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension19 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = bindingExtension15;
			array21[1] = label3;
			array21[2] = grid3;
			array21[3] = settingsCustomCellForPicker3;
			array21[4] = section4;
			array21[5] = settingsView;
			array21[6] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle39, obj36 = new SimpleValueTargetProvider(array21, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 82)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension15.Converter = obj37;
			bindingExtension15.ConverterParameter = atstlist;
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "ATSTIdx";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ATSTIdx, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ATSTIdx")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase15);
			grid3.Children.Add(label3);
			picker3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension16.Source = atstlist2;
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			picker3.SetBinding(Picker.ItemsSourceProperty, bindingBase16);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "ATSTIdx";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ATSTIdx, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ATSTIdx = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ATSTIdx")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase17);
			grid3.Children.Add(picker3);
			settingsCustomCellForPicker3.SetValue(CustomCell.ContentProperty, grid3);
			section4.Add(settingsCustomCellForPicker3);
			settingsView.Root.Add(section4);
			translate10.Text = "Settings_Control_tbCustomInitString.Text";
			IMarkupExtension markupExtension20 = translate10;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 3];
			array22[0] = section5;
			array22[1] = settingsView;
			array22[2] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle41, obj38 = new SimpleValueTargetProvider(array22, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 25)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			section5.Title = obj39;
			bindingExtension18.Mode = 2;
			staticResourceExtension10.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension21 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = bindingExtension18;
			array23[1] = section5;
			array23[2] = settingsView;
			array23[3] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle43, obj40 = new SimpleValueTargetProvider(array23, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 97)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension18.Converter = obj41;
			bindingExtension18.Path = "UseDefaultInit";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseDefaultInit")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase18);
			customCell.SetValue(CustomCell.IsMeasureOnceProperty, false);
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			customCell.Tapped += this.cellInitStringEditor_Tapped;
			staticResourceExtension11.Key = "BaseFontSize++";
			IMarkupExtension markupExtension22 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = label4;
			array24[1] = customCell;
			array24[2] = section5;
			array24[3] = settingsView;
			array24[4] = this;
			object obj42;
			xamlServiceProvider22.Add(typeFromHandle45, obj42 = new SimpleValueTargetProvider(array24, Label.FontSizeProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 25)));
			object obj43 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label4.FontSize = (double)obj43;
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "CustomInitString";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.CustomInitString, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CustomInitString")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			label4.SetBinding(Label.TextProperty, bindingBase19);
			dynamicResourceExtension2.Key = "SettingsCellValueTextColor";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = label4;
			array25[1] = customCell;
			array25[2] = section5;
			array25[3] = settingsView;
			array25[4] = this;
			object obj44;
			xamlServiceProvider23.Add(typeFromHandle47, obj44 = new SimpleValueTargetProvider(array25, Label.TextColorProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 25)));
			DynamicResource dynamicResource2 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label4.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
			customCell.SetValue(CustomCell.ContentProperty, label4);
			section5.Add(customCell);
			settingsView.Root.Add(section5);
			translate11.Text = "Settings_Control_tbCustomHeader.Text";
			IMarkupExtension markupExtension24 = translate11;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 3];
			array26[0] = section6;
			array26[1] = settingsView;
			array26[2] = this;
			object obj45;
			xamlServiceProvider24.Add(typeFromHandle49, obj45 = new SimpleValueTargetProvider(array26, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 25)));
			object obj46 = markupExtension24.ProvideValue(xamlServiceProvider24);
			section6.Title = obj46;
			entryCell.SetValue(CellBase.TitleProperty, "");
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "DefaultFunctionalHeader";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.DefaultFunctionalHeader, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.DefaultFunctionalHeader = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DefaultFunctionalHeader")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			entryCell.SetBinding(EntryCell.ValueTextProperty, bindingBase20);
			section6.Add(entryCell);
			settingsView.Root.Add(section6);
			translate12.Text = "Settings_Control_tbCustomPingPID.Text";
			IMarkupExtension markupExtension25 = translate12;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 3];
			array27[0] = section7;
			array27[1] = settingsView;
			array27[2] = this;
			object obj47;
			xamlServiceProvider25.Add(typeFromHandle51, obj47 = new SimpleValueTargetProvider(array27, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 25)));
			object obj48 = markupExtension25.ProvideValue(xamlServiceProvider25);
			section7.Title = obj48;
			entryCell2.SetValue(CellBase.TitleProperty, "");
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "DetectECUConnectionPID";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.DetectECUConnectionPID, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.DetectECUConnectionPID = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DetectECUConnectionPID")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			entryCell2.SetBinding(EntryCell.ValueTextProperty, bindingBase21);
			section7.Add(entryCell2);
			settingsView.Root.Add(section7);
			translate13.Text = "Settings_Control_tbTesterPresentPID.Text";
			IMarkupExtension markupExtension26 = translate13;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 3];
			array28[0] = section8;
			array28[1] = settingsView;
			array28[2] = this;
			object obj49;
			xamlServiceProvider26.Add(typeFromHandle53, obj49 = new SimpleValueTargetProvider(array28, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 25)));
			object obj50 = markupExtension26.ProvideValue(xamlServiceProvider26);
			section8.Title = obj50;
			entryCell3.SetValue(CellBase.TitleProperty, "");
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "TesterPresentCommand";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TesterPresentCommand, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.TesterPresentCommand = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "TesterPresentCommand")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			entryCell3.SetBinding(EntryCell.ValueTextProperty, bindingBase22);
			section8.Add(entryCell3);
			settingsView.Root.Add(section8);
			translate14.Text = "settings_Optimizations";
			IMarkupExtension markupExtension27 = translate14;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 3];
			array29[0] = section9;
			array29[1] = settingsView;
			array29[2] = this;
			object obj51;
			xamlServiceProvider27.Add(typeFromHandle55, obj51 = new SimpleValueTargetProvider(array29, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(179, 25)));
			object obj52 = markupExtension27.ProvideValue(xamlServiceProvider27);
			section9.Title = obj52;
			translate15.Text = "settings_ATCommandStateOptimization";
			IMarkupExtension markupExtension28 = translate15;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = settingsCheckBoxCellPatched4;
			array30[1] = section9;
			array30[2] = settingsView;
			array30[3] = this;
			object obj53;
			xamlServiceProvider28.Add(typeFromHandle57, obj53 = new SimpleValueTargetProvider(array30, CellBase.TitleProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 49)));
			object obj54 = markupExtension28.ProvideValue(xamlServiceProvider28);
			settingsCheckBoxCellPatched4.Title = obj54;
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "ATCommandStateOptimization";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ATCommandStateOptimization, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ATCommandStateOptimization = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ATCommandStateOptimization")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase23);
			section9.Add(settingsCheckBoxCellPatched4);
			translate16.Text = "ios_ExpectedResponseCountOptimization";
			IMarkupExtension markupExtension29 = translate16;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = settingsCheckBoxCellPatched5;
			array31[1] = section9;
			array31[2] = settingsView;
			array31[3] = this;
			object obj55;
			xamlServiceProvider29.Add(typeFromHandle59, obj55 = new SimpleValueTargetProvider(array31, CellBase.TitleProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(181, 49)));
			object obj56 = markupExtension29.ProvideValue(xamlServiceProvider29);
			settingsCheckBoxCellPatched5.Title = obj56;
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "ExpectedResponseCountOptimization";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ExpectedResponseCountOptimization, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ExpectedResponseCountOptimization = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ExpectedResponseCountOptimization")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase24);
			section9.Add(settingsCheckBoxCellPatched5);
			translate17.Text = "ios_ExpectedResponseCountOptimizationAlways1ForKWPMode01";
			IMarkupExtension markupExtension30 = translate17;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = settingsCheckBoxCellPatched6;
			array32[1] = section9;
			array32[2] = settingsView;
			array32[3] = this;
			object obj57;
			xamlServiceProvider30.Add(typeFromHandle61, obj57 = new SimpleValueTargetProvider(array32, CellBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 49)));
			object obj58 = markupExtension30.ProvideValue(xamlServiceProvider30);
			settingsCheckBoxCellPatched6.Title = obj58;
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "ExpectedResponseCountOptimizationAlways1ForKWPMode01";
			bindingExtension25.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ExpectedResponseCountOptimizationAlways1ForKWPMode01, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ExpectedResponseCountOptimizationAlways1ForKWPMode01 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ExpectedResponseCountOptimizationAlways1ForKWPMode01")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase25);
			section9.Add(settingsCheckBoxCellPatched6);
			translate18.Text = "Settings_Control_tbCANOptimization.Text";
			IMarkupExtension markupExtension31 = translate18;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = settingsCheckBoxCellPatched7;
			array33[1] = section9;
			array33[2] = settingsView;
			array33[3] = this;
			object obj59;
			xamlServiceProvider31.Add(typeFromHandle63, obj59 = new SimpleValueTargetProvider(array33, CellBase.TitleProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 49)));
			object obj60 = markupExtension31.ProvideValue(xamlServiceProvider31);
			settingsCheckBoxCellPatched7.Title = obj60;
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "CANOptimizeRequests";
			bindingExtension26.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CANOptimizeRequests = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase26);
			section9.Add(settingsCheckBoxCellPatched7);
			translate19.Text = "Settings_Control_tbCANOptimizeMaxInRequest.Text";
			IMarkupExtension markupExtension32 = translate19;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = numberPickerCell;
			array34[1] = section9;
			array34[2] = settingsView;
			array34[3] = this;
			object obj61;
			xamlServiceProvider32.Add(typeFromHandle65, obj61 = new SimpleValueTargetProvider(array34, CellBase.TitleProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(185, 21)));
			object obj62 = markupExtension32.ProvideValue(xamlServiceProvider32);
			numberPickerCell.Title = obj62;
			numberPickerCell.SetValue(CellBase.DescriptionProperty, "OBDII $01");
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "CANOptimizeRequests";
			bindingExtension27.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			numberPickerCell.SetBinding(CellBase.IsVisibleProperty, bindingBase27);
			numberPickerCell.SetValue(NumberPickerCell.MaxProperty, 6);
			numberPickerCell.SetValue(NumberPickerCell.MinProperty, 2);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "CANOptimizeMaxInRequest";
			bindingExtension28.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.CANOptimizeMaxInRequest, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.CANOptimizeMaxInRequest = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeMaxInRequest")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			numberPickerCell.SetBinding(NumberPickerCell.NumberProperty, bindingBase28);
			section9.Add(numberPickerCell);
			translate20.Text = "ios_CANOptimizeMode22";
			IMarkupExtension markupExtension33 = translate20;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = settingsCheckBoxCellPatched8;
			array35[1] = section9;
			array35[2] = settingsView;
			array35[3] = this;
			object obj63;
			xamlServiceProvider33.Add(typeFromHandle67, obj63 = new SimpleValueTargetProvider(array35, CellBase.TitleProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(195, 21)));
			object obj64 = markupExtension33.ProvideValue(xamlServiceProvider33);
			settingsCheckBoxCellPatched8.Title = obj64;
			bindingExtension29.Mode = 1;
			bindingExtension29.Path = "CANOptimizeMode22";
			bindingExtension29.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CANOptimizeMode22 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeMode22")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CheckboxCell.CheckedProperty, bindingBase29);
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "CANOptimizeRequests";
			bindingExtension30.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CellBase.IsVisibleProperty, bindingBase30);
			section9.Add(settingsCheckBoxCellPatched8);
			translate21.Text = "Settings_Control_tbCANOptimizeMaxInRequest.Text";
			IMarkupExtension markupExtension34 = translate21;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = numberPickerCell2;
			array36[1] = section9;
			array36[2] = settingsView;
			array36[3] = this;
			object obj65;
			xamlServiceProvider34.Add(typeFromHandle69, obj65 = new SimpleValueTargetProvider(array36, CellBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 21)));
			object obj66 = markupExtension34.ProvideValue(xamlServiceProvider34);
			numberPickerCell2.Title = obj66;
			numberPickerCell2.SetValue(CellBase.DescriptionProperty, "UDS $22");
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "CANOptimizeMode22";
			bindingExtension31.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeMode22")
			});
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			numberPickerCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase31);
			numberPickerCell2.SetValue(NumberPickerCell.MaxProperty, 8);
			numberPickerCell2.SetValue(NumberPickerCell.MinProperty, 2);
			bindingExtension32.Mode = 1;
			bindingExtension32.Path = "CANOptimizeMode22MaxInRequest";
			bindingExtension32.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.CANOptimizeMode22MaxInRequest, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.CANOptimizeMode22MaxInRequest = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeMode22MaxInRequest")
			});
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			numberPickerCell2.SetBinding(NumberPickerCell.NumberProperty, bindingBase32);
			section9.Add(numberPickerCell2);
			translate22.Text = "ios_CANOptimizeMode22SelfLearningMode";
			IMarkupExtension markupExtension35 = translate22;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = settingsCheckBoxCellPatched9;
			array37[1] = section9;
			array37[2] = settingsView;
			array37[3] = this;
			object obj67;
			xamlServiceProvider35.Add(typeFromHandle71, obj67 = new SimpleValueTargetProvider(array37, CellBase.TitleProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(208, 21)));
			object obj68 = markupExtension35.ProvideValue(xamlServiceProvider35);
			settingsCheckBoxCellPatched9.Title = obj68;
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "CANOptimizeMode22SelfLearningMode";
			bindingExtension33.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22SelfLearningMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CANOptimizeMode22SelfLearningMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeMode22SelfLearningMode")
			});
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			settingsCheckBoxCellPatched9.SetBinding(CheckboxCell.CheckedProperty, bindingBase33);
			bindingExtension34.Mode = 2;
			bindingExtension34.Path = "CANOptimizeRequests";
			bindingExtension34.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			settingsCheckBoxCellPatched9.SetBinding(CellBase.IsVisibleProperty, bindingBase34);
			section9.Add(settingsCheckBoxCellPatched9);
			translate23.Text = "settings_AutomaticReoptimization";
			IMarkupExtension markupExtension36 = translate23;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = settingsCheckBoxCellPatched10;
			array38[1] = section9;
			array38[2] = settingsView;
			array38[3] = this;
			object obj69;
			xamlServiceProvider36.Add(typeFromHandle73, obj69 = new SimpleValueTargetProvider(array38, CellBase.TitleProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			object obj70 = markupExtension36.ProvideValue(xamlServiceProvider36);
			settingsCheckBoxCellPatched10.Title = obj70;
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "AutomaticReoptimization";
			bindingExtension35.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticReoptimization, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AutomaticReoptimization = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticReoptimization")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			settingsCheckBoxCellPatched10.SetBinding(CheckboxCell.CheckedProperty, bindingBase35);
			bindingExtension36.Mode = 2;
			bindingExtension36.Path = "CANOptimizeRequests";
			bindingExtension36.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			settingsCheckBoxCellPatched10.SetBinding(CellBase.IsVisibleProperty, bindingBase36);
			section9.Add(settingsCheckBoxCellPatched10);
			translate24.Text = "Settings_CANRequestSegmentationSTNLevel";
			IMarkupExtension markupExtension37 = translate24;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 4];
			array39[0] = settingsCheckBoxCellPatched11;
			array39[1] = section9;
			array39[2] = settingsView;
			array39[3] = this;
			object obj71;
			xamlServiceProvider37.Add(typeFromHandle75, obj71 = new SimpleValueTargetProvider(array39, CellBase.TitleProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 49)));
			object obj72 = markupExtension37.ProvideValue(xamlServiceProvider37);
			settingsCheckBoxCellPatched11.Title = obj72;
			bindingExtension37.Mode = 1;
			bindingExtension37.Path = "CANRequestSegmentationSTNLevel";
			bindingExtension37.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANRequestSegmentationSTNLevel, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CANRequestSegmentationSTNLevel = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANRequestSegmentationSTNLevel")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			settingsCheckBoxCellPatched11.SetBinding(CheckboxCell.CheckedProperty, bindingBase37);
			section9.Add(settingsCheckBoxCellPatched11);
			translate25.Text = "ios_btnForceMode22Learning";
			IMarkupExtension markupExtension38 = translate25;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 4];
			array40[0] = buttonCell;
			array40[1] = section9;
			array40[2] = settingsView;
			array40[3] = this;
			object obj73;
			xamlServiceProvider38.Add(typeFromHandle77, obj73 = new SimpleValueTargetProvider(array40, CellBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(226, 21)));
			object obj74 = markupExtension38.ProvideValue(xamlServiceProvider38);
			buttonCell.Title = obj74;
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "CANOptimizeRequests";
			bindingExtension38.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			buttonCell.SetBinding(CellBase.IsVisibleProperty, bindingBase38);
			buttonCell.Tapped += this.btnForceMode22Learning_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension39 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = buttonCell;
			array41[1] = section9;
			array41[2] = settingsView;
			array41[3] = this;
			object obj75;
			xamlServiceProvider39.Add(typeFromHandle79, obj75 = new SimpleValueTargetProvider(array41, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(229, 21)));
			DynamicResource dynamicResource3 = markupExtension39.ProvideValue(xamlServiceProvider39);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section9.Add(buttonCell);
			translate26.Text = "ios_btnClearOptimizationData";
			IMarkupExtension markupExtension40 = translate26;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = buttonCell2;
			array42[1] = section9;
			array42[2] = settingsView;
			array42[3] = this;
			object obj76;
			xamlServiceProvider40.Add(typeFromHandle81, obj76 = new SimpleValueTargetProvider(array42, CellBase.TitleProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 21)));
			object obj77 = markupExtension40.ProvideValue(xamlServiceProvider40);
			buttonCell2.Title = obj77;
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "CANOptimizeRequests";
			bindingExtension39.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CANOptimizeRequests")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			buttonCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase39);
			buttonCell2.Tapped += this.btnClearOptimizationData_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension41 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = buttonCell2;
			array43[1] = section9;
			array43[2] = settingsView;
			array43[3] = this;
			object obj78;
			xamlServiceProvider41.Add(typeFromHandle83, obj78 = new SimpleValueTargetProvider(array43, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(235, 21)));
			DynamicResource dynamicResource4 = markupExtension41.ProvideValue(xamlServiceProvider41);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section9.Add(buttonCell2);
			translate27.Text = "ios_DisableOptimizationIfItFails";
			IMarkupExtension markupExtension42 = translate27;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = settingsCheckBoxCellPatched12;
			array44[1] = section9;
			array44[2] = settingsView;
			array44[3] = this;
			object obj79;
			xamlServiceProvider42.Add(typeFromHandle85, obj79 = new SimpleValueTargetProvider(array44, CellBase.TitleProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(236, 49)));
			object obj80 = markupExtension42.ProvideValue(xamlServiceProvider42);
			settingsCheckBoxCellPatched12.Title = obj80;
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "DisableOptimizationIfItFails";
			bindingExtension40.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DisableOptimizationIfItFails, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DisableOptimizationIfItFails = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DisableOptimizationIfItFails")
			});
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			settingsCheckBoxCellPatched12.SetBinding(CheckboxCell.CheckedProperty, bindingBase40);
			section9.Add(settingsCheckBoxCellPatched12);
			translate28.Text = "settings_ATCRAOptimization";
			IMarkupExtension markupExtension43 = translate28;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 4];
			array45[0] = settingsCheckBoxCellPatched13;
			array45[1] = section9;
			array45[2] = settingsView;
			array45[3] = this;
			object obj81;
			xamlServiceProvider43.Add(typeFromHandle87, obj81 = new SimpleValueTargetProvider(array45, CellBase.TitleProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(237, 49)));
			object obj82 = markupExtension43.ProvideValue(xamlServiceProvider43);
			settingsCheckBoxCellPatched13.Title = obj82;
			bindingExtension41.Mode = 1;
			bindingExtension41.Path = "ATCRAOptimization";
			bindingExtension41.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ATCRAOptimization, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ATCRAOptimization = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ATCRAOptimization")
			});
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			settingsCheckBoxCellPatched13.SetBinding(CheckboxCell.CheckedProperty, bindingBase41);
			section9.Add(settingsCheckBoxCellPatched13);
			settingsView.Root.Add(section9);
			translate29.Text = "settings_CompatibilityPatches";
			IMarkupExtension markupExtension44 = translate29;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 3];
			array46[0] = section10;
			array46[1] = settingsView;
			array46[2] = this;
			object obj83;
			xamlServiceProvider44.Add(typeFromHandle89, obj83 = new SimpleValueTargetProvider(array46, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(241, 25)));
			object obj84 = markupExtension44.ProvideValue(xamlServiceProvider44);
			section10.Title = obj84;
			translate30.Text = "Settings_Control_tbPingECUWhenIdle.Text";
			IMarkupExtension markupExtension45 = translate30;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = settingsCheckBoxCellPatched14;
			array47[1] = section10;
			array47[2] = settingsView;
			array47[3] = this;
			object obj85;
			xamlServiceProvider45.Add(typeFromHandle91, obj85 = new SimpleValueTargetProvider(array47, CellBase.TitleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 49)));
			object obj86 = markupExtension45.ProvideValue(xamlServiceProvider45);
			settingsCheckBoxCellPatched14.Title = obj86;
			bindingExtension42.Mode = 1;
			bindingExtension42.Path = "AlwaysPingECU";
			bindingExtension42.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AlwaysPingECU, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AlwaysPingECU = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AlwaysPingECU")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			settingsCheckBoxCellPatched14.SetBinding(CheckboxCell.CheckedProperty, bindingBase42);
			section10.Add(settingsCheckBoxCellPatched14);
			translate31.Text = "Settings_Control_MUT2Mode.Text";
			IMarkupExtension markupExtension46 = translate31;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = settingsCheckBoxCellPatched15;
			array48[1] = section10;
			array48[2] = settingsView;
			array48[3] = this;
			object obj87;
			xamlServiceProvider46.Add(typeFromHandle93, obj87 = new SimpleValueTargetProvider(array48, CellBase.TitleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(243, 49)));
			object obj88 = markupExtension46.ProvideValue(xamlServiceProvider46);
			settingsCheckBoxCellPatched15.Title = obj88;
			bindingExtension43.Mode = 1;
			bindingExtension43.Path = "DecodeMUT2Compatible";
			bindingExtension43.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DecodeMUT2Compatible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DecodeMUT2Compatible = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DecodeMUT2Compatible")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			settingsCheckBoxCellPatched15.SetBinding(CheckboxCell.CheckedProperty, bindingBase43);
			section10.Add(settingsCheckBoxCellPatched15);
			translate32.Text = "ios_Settings_DaihatsuMode";
			IMarkupExtension markupExtension47 = translate32;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 4];
			array49[0] = settingsCheckBoxCellPatched16;
			array49[1] = section10;
			array49[2] = settingsView;
			array49[3] = this;
			object obj89;
			xamlServiceProvider47.Add(typeFromHandle95, obj89 = new SimpleValueTargetProvider(array49, CellBase.TitleProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 21)));
			object obj90 = markupExtension47.ProvideValue(xamlServiceProvider47);
			settingsCheckBoxCellPatched16.Title = obj90;
			bindingExtension44.Mode = 1;
			bindingExtension44.Path = "DaihatsuKLine";
			bindingExtension44.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DaihatsuKLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DaihatsuKLine = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DaihatsuKLine")
			});
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			settingsCheckBoxCellPatched16.SetBinding(CheckboxCell.CheckedProperty, bindingBase44);
			translate33.Text = "ios_Settings_DaihatsuWarning";
			IMarkupExtension markupExtension48 = translate33;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 4];
			array50[0] = settingsCheckBoxCellPatched16;
			array50[1] = section10;
			array50[2] = settingsView;
			array50[3] = this;
			object obj91;
			xamlServiceProvider48.Add(typeFromHandle97, obj91 = new SimpleValueTargetProvider(array50, CellBase.DescriptionProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(247, 21)));
			object obj92 = markupExtension48.ProvideValue(xamlServiceProvider48);
			settingsCheckBoxCellPatched16.Description = obj92;
			section10.Add(settingsCheckBoxCellPatched16);
			translate34.Text = "ios_KWPConcatResponseLines";
			IMarkupExtension markupExtension49 = translate34;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 4];
			array51[0] = settingsCheckBoxCellPatched17;
			array51[1] = section10;
			array51[2] = settingsView;
			array51[3] = this;
			object obj93;
			xamlServiceProvider49.Add(typeFromHandle99, obj93 = new SimpleValueTargetProvider(array51, CellBase.TitleProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj93);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 49)));
			object obj94 = markupExtension49.ProvideValue(xamlServiceProvider49);
			settingsCheckBoxCellPatched17.Title = obj94;
			bindingExtension45.Mode = 1;
			bindingExtension45.Path = "KWPConcatResponseLines";
			bindingExtension45.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.KWPConcatResponseLines, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.KWPConcatResponseLines = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "KWPConcatResponseLines")
			});
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			settingsCheckBoxCellPatched17.SetBinding(CheckboxCell.CheckedProperty, bindingBase45);
			section10.Add(settingsCheckBoxCellPatched17);
			translate35.Text = "settings_ForceFlowControlCoding";
			IMarkupExtension markupExtension50 = translate35;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 4];
			array52[0] = settingsCheckBoxCellPatched18;
			array52[1] = section10;
			array52[2] = settingsView;
			array52[3] = this;
			object obj95;
			xamlServiceProvider50.Add(typeFromHandle101, obj95 = new SimpleValueTargetProvider(array52, CellBase.TitleProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj95);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver50.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider50.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 49)));
			object obj96 = markupExtension50.ProvideValue(xamlServiceProvider50);
			settingsCheckBoxCellPatched18.Title = obj96;
			bindingExtension46.Mode = 1;
			bindingExtension46.Path = "ForceUseManualFlowControlForCodingOperations";
			bindingExtension46.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ForceUseManualFlowControlForCodingOperations, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ForceUseManualFlowControlForCodingOperations = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ForceUseManualFlowControlForCodingOperations")
			});
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			settingsCheckBoxCellPatched18.SetBinding(CheckboxCell.CheckedProperty, bindingBase46);
			section10.Add(settingsCheckBoxCellPatched18);
			translate36.Text = "settings_ForceFlowControlReading";
			IMarkupExtension markupExtension51 = translate36;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 4];
			array53[0] = settingsCheckBoxCellPatched19;
			array53[1] = section10;
			array53[2] = settingsView;
			array53[3] = this;
			object obj97;
			xamlServiceProvider51.Add(typeFromHandle103, obj97 = new SimpleValueTargetProvider(array53, CellBase.TitleProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj97);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver51.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider51.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(250, 49)));
			object obj98 = markupExtension51.ProvideValue(xamlServiceProvider51);
			settingsCheckBoxCellPatched19.Title = obj98;
			bindingExtension47.Mode = 1;
			bindingExtension47.Path = "ForceUseManualFlowControlWhileReadingData";
			bindingExtension47.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ForceUseManualFlowControlWhileReadingData, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ForceUseManualFlowControlWhileReadingData = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ForceUseManualFlowControlWhileReadingData")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			settingsCheckBoxCellPatched19.SetBinding(CheckboxCell.CheckedProperty, bindingBase47);
			section10.Add(settingsCheckBoxCellPatched19);
			translate37.Text = "Settings_SendTesterPresentWhileLongUploadTimeVag5f";
			IMarkupExtension markupExtension52 = translate37;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 4];
			array54[0] = settingsCheckBoxCellPatched20;
			array54[1] = section10;
			array54[2] = settingsView;
			array54[3] = this;
			object obj99;
			xamlServiceProvider52.Add(typeFromHandle105, obj99 = new SimpleValueTargetProvider(array54, CellBase.TitleProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj99);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver52.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver52.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider52.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver52, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(251, 49)));
			object obj100 = markupExtension52.ProvideValue(xamlServiceProvider52);
			settingsCheckBoxCellPatched20.Title = obj100;
			bindingExtension48.Mode = 1;
			bindingExtension48.Path = "SendTesterPresentWhileLongUploadTimeVag5f";
			bindingExtension48.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SendTesterPresentWhileLongUploadTimeVag5f, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SendTesterPresentWhileLongUploadTimeVag5f = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SendTesterPresentWhileLongUploadTimeVag5f")
			});
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			settingsCheckBoxCellPatched20.SetBinding(CheckboxCell.CheckedProperty, bindingBase48);
			section10.Add(settingsCheckBoxCellPatched20);
			translate38.Text = "Settings_ReplaceATTAWithATCER";
			IMarkupExtension markupExtension53 = translate38;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 4];
			array55[0] = settingsCheckBoxCellPatched21;
			array55[1] = section10;
			array55[2] = settingsView;
			array55[3] = this;
			object obj101;
			xamlServiceProvider53.Add(typeFromHandle107, obj101 = new SimpleValueTargetProvider(array55, CellBase.TitleProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj101);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver53.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver53.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider53.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver53, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(252, 49)));
			object obj102 = markupExtension53.ProvideValue(xamlServiceProvider53);
			settingsCheckBoxCellPatched21.Title = obj102;
			bindingExtension49.Mode = 1;
			bindingExtension49.Path = "ReplaceATTAWithATCER";
			bindingExtension49.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ReplaceATTAWithATCER, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ReplaceATTAWithATCER = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ReplaceATTAWithATCER")
			});
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			settingsCheckBoxCellPatched21.SetBinding(CheckboxCell.CheckedProperty, bindingBase49);
			section10.Add(settingsCheckBoxCellPatched21);
			settingsCheckBoxCellPatched22.SetValue(CellBase.TitleProperty, "Use service-only positive response for ISO 15765-4");
			bindingExtension50.Mode = 1;
			bindingExtension50.Path = "UseServiceResponseForPositiveResponseMarker";
			bindingExtension50.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseServiceResponseForPositiveResponseMarker, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseServiceResponseForPositiveResponseMarker = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseServiceResponseForPositiveResponseMarker")
			});
			BindingBase bindingBase50 = bindingExtension50.ProvideValue(null);
			settingsCheckBoxCellPatched22.SetBinding(CheckboxCell.CheckedProperty, bindingBase50);
			section10.Add(settingsCheckBoxCellPatched22);
			settingsCheckBoxCellPatched23.SetValue(CellBase.TitleProperty, "Concact response lines for faulty device, splitting each byte at a new line");
			bindingExtension51.Mode = 1;
			bindingExtension51.Path = "KWPConcatResponseLines";
			bindingExtension51.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.KWPConcatResponseLines, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.KWPConcatResponseLines = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "KWPConcatResponseLines")
			});
			BindingBase bindingBase51 = bindingExtension51.ProvideValue(null);
			settingsCheckBoxCellPatched23.SetBinding(CheckboxCell.CheckedProperty, bindingBase51);
			section10.Add(settingsCheckBoxCellPatched23);
			translate39.Text = "settings_AddNissanConsult3Pids";
			IMarkupExtension markupExtension54 = translate39;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 4];
			array56[0] = settingsCheckBoxCellPatched24;
			array56[1] = section10;
			array56[2] = settingsView;
			array56[3] = this;
			object obj103;
			xamlServiceProvider54.Add(typeFromHandle109, obj103 = new SimpleValueTargetProvider(array56, CellBase.TitleProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj103);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver54.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver54.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider54.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver54, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(256, 21)));
			object obj104 = markupExtension54.ProvideValue(xamlServiceProvider54);
			settingsCheckBoxCellPatched24.Title = obj104;
			bindingExtension52.Mode = 1;
			bindingExtension52.Path = "AddNissanConsult3Pids";
			bindingExtension52.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AddNissanConsult3Pids, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AddNissanConsult3Pids = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AddNissanConsult3Pids")
			});
			BindingBase bindingBase52 = bindingExtension52.ProvideValue(null);
			settingsCheckBoxCellPatched24.SetBinding(CheckboxCell.CheckedProperty, bindingBase52);
			settingsCheckBoxCellPatched24.SetValue(CellBase.DescriptionProperty, "Nissan, Infiniti");
			section10.Add(settingsCheckBoxCellPatched24);
			translate40.Text = "settings_NissanConsult3OpenCloseSession";
			IMarkupExtension markupExtension55 = translate40;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 4];
			array57[0] = settingsCheckBoxCellPatched25;
			array57[1] = section10;
			array57[2] = settingsView;
			array57[3] = this;
			object obj105;
			xamlServiceProvider55.Add(typeFromHandle111, obj105 = new SimpleValueTargetProvider(array57, CellBase.TitleProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj105);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver55.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver55.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider55.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver55, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(260, 21)));
			object obj106 = markupExtension55.ProvideValue(xamlServiceProvider55);
			settingsCheckBoxCellPatched25.Title = obj106;
			bindingExtension53.Mode = 1;
			bindingExtension53.Path = "NissanConsult3OpenCloseSession";
			bindingExtension53.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.NissanConsult3OpenCloseSession, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.NissanConsult3OpenCloseSession = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "NissanConsult3OpenCloseSession")
			});
			BindingBase bindingBase53 = bindingExtension53.ProvideValue(null);
			settingsCheckBoxCellPatched25.SetBinding(CheckboxCell.CheckedProperty, bindingBase53);
			settingsCheckBoxCellPatched25.SetValue(CellBase.DescriptionProperty, "Nissan, Infiniti");
			section10.Add(settingsCheckBoxCellPatched25);
			translate41.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension56 = translate41;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 4];
			array58[0] = settingsCheckBoxCellPatched26;
			array58[1] = section10;
			array58[2] = settingsView;
			array58[3] = this;
			object obj107;
			xamlServiceProvider56.Add(typeFromHandle113, obj107 = new SimpleValueTargetProvider(array58, CellBase.TitleProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj107);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver56.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver56.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider56.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver56, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(265, 21)));
			object obj108 = markupExtension56.ProvideValue(xamlServiceProvider56);
			settingsCheckBoxCellPatched26.Title = obj108;
			bindingExtension54.Mode = 1;
			bindingExtension54.Path = "IgnoreCodingErrors";
			bindingExtension54.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IgnoreCodingErrors, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.IgnoreCodingErrors = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "IgnoreCodingErrors")
			});
			BindingBase bindingBase54 = bindingExtension54.ProvideValue(null);
			settingsCheckBoxCellPatched26.SetBinding(CheckboxCell.CheckedProperty, bindingBase54);
			settingsCheckBoxCellPatched26.SetValue(CellBase.DescriptionProperty, "VAG (VW, Audi, Skoda, Seat)");
			section10.Add(settingsCheckBoxCellPatched26);
			settingsView.Root.Add(section10);
			translate42.Text = "settings_FlowControlOverrideMode";
			IMarkupExtension markupExtension57 = translate42;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 3];
			array59[0] = section11;
			array59[1] = settingsView;
			array59[2] = this;
			object obj109;
			xamlServiceProvider57.Add(typeFromHandle115, obj109 = new SimpleValueTargetProvider(array59, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj109);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver57.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver57.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider57.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver57, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(271, 25)));
			object obj110 = markupExtension57.ProvideValue(xamlServiceProvider57);
			section11.Title = obj110;
			bindingExtension55.Mode = 1;
			bindingExtension55.Path = "FlowControlOverrideMode";
			bindingExtension55.TypedBinding = new TypedBinding<SharedSettings, FlowControlOverrides>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FlowControlOverrides, bool>(A_0.FlowControlOverrideMode, true);
				}
				return default(ValueTuple<FlowControlOverrides, bool>);
			}, delegate(SharedSettings A_0, FlowControlOverrides A_1)
			{
				if (A_0 != null)
				{
					A_0.FlowControlOverrideMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FlowControlOverrideMode")
			});
			BindingBase bindingBase55 = bindingExtension55.ProvideValue(null);
			section11.SetBinding(RadioCell.SelectedValueProperty, bindingBase55);
			translate43.Text = "FlowControlOverrides.Off";
			IMarkupExtension markupExtension58 = translate43;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 4];
			array60[0] = radioCell3;
			array60[1] = section11;
			array60[2] = settingsView;
			array60[3] = this;
			object obj111;
			xamlServiceProvider58.Add(typeFromHandle117, obj111 = new SimpleValueTargetProvider(array60, CellBase.TitleProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj111);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver58.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver58.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider58.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver58, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 31)));
			object obj112 = markupExtension58.ProvideValue(xamlServiceProvider58);
			radioCell3.Title = obj112;
			radioCell3.SetValue(RadioCell.ValueProperty, flowControlOverrides);
			section11.Add(radioCell3);
			translate44.Text = "FlowControlOverrides.ForceOnFor7Ex";
			IMarkupExtension markupExtension59 = translate44;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle119 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 4];
			array61[0] = radioCell4;
			array61[1] = section11;
			array61[2] = settingsView;
			array61[3] = this;
			object obj113;
			xamlServiceProvider59.Add(typeFromHandle119, obj113 = new SimpleValueTargetProvider(array61, CellBase.TitleProperty, nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj113);
			Type typeFromHandle120 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver59.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver59.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider59.Add(typeFromHandle120, new XamlTypeResolver(xmlNamespaceResolver59, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(273, 31)));
			object obj114 = markupExtension59.ProvideValue(xamlServiceProvider59);
			radioCell4.Title = obj114;
			radioCell4.SetValue(RadioCell.ValueProperty, flowControlOverrides2);
			section11.Add(radioCell4);
			translate45.Text = "FlowControlOverrides.ForceOffFor7Ex";
			IMarkupExtension markupExtension60 = translate45;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle121 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 4];
			array62[0] = radioCell5;
			array62[1] = section11;
			array62[2] = settingsView;
			array62[3] = this;
			object obj115;
			xamlServiceProvider60.Add(typeFromHandle121, obj115 = new SimpleValueTargetProvider(array62, CellBase.TitleProperty, nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj115);
			Type typeFromHandle122 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver60.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver60.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider60.Add(typeFromHandle122, new XamlTypeResolver(xmlNamespaceResolver60, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(274, 31)));
			object obj116 = markupExtension60.ProvideValue(xamlServiceProvider60);
			radioCell5.Title = obj116;
			radioCell5.SetValue(RadioCell.ValueProperty, flowControlOverrides3);
			section11.Add(radioCell5);
			translate46.Text = "FlowControlOverrides.ForceOnForAll";
			IMarkupExtension markupExtension61 = translate46;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle123 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 4];
			array63[0] = radioCell6;
			array63[1] = section11;
			array63[2] = settingsView;
			array63[3] = this;
			object obj117;
			xamlServiceProvider61.Add(typeFromHandle123, obj117 = new SimpleValueTargetProvider(array63, CellBase.TitleProperty, nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj117);
			Type typeFromHandle124 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver61.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver61.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider61.Add(typeFromHandle124, new XamlTypeResolver(xmlNamespaceResolver61, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 31)));
			object obj118 = markupExtension61.ProvideValue(xamlServiceProvider61);
			radioCell6.Title = obj118;
			radioCell6.SetValue(RadioCell.ValueProperty, flowControlOverrides4);
			section11.Add(radioCell6);
			translate47.Text = "FlowControlOverrides.ForceOffForAll";
			IMarkupExtension markupExtension62 = translate47;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle125 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 4];
			array64[0] = radioCell7;
			array64[1] = section11;
			array64[2] = settingsView;
			array64[3] = this;
			object obj119;
			xamlServiceProvider62.Add(typeFromHandle125, obj119 = new SimpleValueTargetProvider(array64, CellBase.TitleProperty, nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj119);
			Type typeFromHandle126 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver62.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver62.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider62.Add(typeFromHandle126, new XamlTypeResolver(xmlNamespaceResolver62, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 31)));
			object obj120 = markupExtension62.ProvideValue(xamlServiceProvider62);
			radioCell7.Title = obj120;
			radioCell7.SetValue(RadioCell.ValueProperty, flowControlOverrides5);
			section11.Add(radioCell7);
			settingsView.Root.Add(section11);
			section12.SetValue(SectionBase.TitleProperty, "OBDII");
			translate48.Text = "Settings_Control_PerformSensorsScanByTesting.Text";
			IMarkupExtension markupExtension63 = translate48;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle127 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 4];
			array65[0] = settingsCheckBoxCellPatched27;
			array65[1] = section12;
			array65[2] = settingsView;
			array65[3] = this;
			object obj121;
			xamlServiceProvider63.Add(typeFromHandle127, obj121 = new SimpleValueTargetProvider(array65, CellBase.TitleProperty, nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj121);
			Type typeFromHandle128 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver63.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver63.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider63.Add(typeFromHandle128, new XamlTypeResolver(xmlNamespaceResolver63, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(282, 21)));
			object obj122 = markupExtension63.ProvideValue(xamlServiceProvider63);
			settingsCheckBoxCellPatched27.Title = obj122;
			bindingExtension56.Mode = 1;
			bindingExtension56.Path = "PerformSensorsScanByTesting";
			bindingExtension56.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PerformSensorsScanByTesting, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PerformSensorsScanByTesting = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "PerformSensorsScanByTesting")
			});
			BindingBase bindingBase56 = bindingExtension56.ProvideValue(null);
			settingsCheckBoxCellPatched27.SetBinding(CheckboxCell.CheckedProperty, bindingBase56);
			bindingExtension57.Mode = 2;
			bindingExtension57.Path = "ShowExperimental";
			bindingExtension57.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase57 = bindingExtension57.ProvideValue(null);
			settingsCheckBoxCellPatched27.SetBinding(CellBase.IsVisibleProperty, bindingBase57);
			section12.Add(settingsCheckBoxCellPatched27);
			translate49.Text = "ios_RequestECUInfo";
			IMarkupExtension markupExtension64 = translate49;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle129 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 4];
			array66[0] = settingsCheckBoxCellPatched28;
			array66[1] = section12;
			array66[2] = settingsView;
			array66[3] = this;
			object obj123;
			xamlServiceProvider64.Add(typeFromHandle129, obj123 = new SimpleValueTargetProvider(array66, CellBase.TitleProperty, nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj123);
			Type typeFromHandle130 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver64.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver64.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider64.Add(typeFromHandle130, new XamlTypeResolver(xmlNamespaceResolver64, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(285, 49)));
			object obj124 = markupExtension64.ProvideValue(xamlServiceProvider64);
			settingsCheckBoxCellPatched28.Title = obj124;
			bindingExtension58.Mode = 1;
			bindingExtension58.Path = "RequestECUInfo";
			bindingExtension58.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.RequestECUInfo, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.RequestECUInfo = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "RequestECUInfo")
			});
			BindingBase bindingBase58 = bindingExtension58.ProvideValue(null);
			settingsCheckBoxCellPatched28.SetBinding(CheckboxCell.CheckedProperty, bindingBase58);
			section12.Add(settingsCheckBoxCellPatched28);
			settingsView.Root.Add(section12);
			translate50.Text = "settings_Connection_Mode01Prefix";
			IMarkupExtension markupExtension65 = translate50;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle131 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 3];
			array67[0] = section13;
			array67[1] = settingsView;
			array67[2] = this;
			object obj125;
			xamlServiceProvider65.Add(typeFromHandle131, obj125 = new SimpleValueTargetProvider(array67, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj125);
			Type typeFromHandle132 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver65.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver65.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider65.Add(typeFromHandle132, new XamlTypeResolver(xmlNamespaceResolver65, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(288, 25)));
			object obj126 = markupExtension65.ProvideValue(xamlServiceProvider65);
			section13.Title = obj126;
			entryCell4.SetValue(CellBase.TitleProperty, "");
			bindingExtension59.Mode = 1;
			bindingExtension59.Path = "Mode01Prefix";
			bindingExtension59.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Mode01Prefix, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Mode01Prefix = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Mode01Prefix")
			});
			BindingBase bindingBase59 = bindingExtension59.ProvideValue(null);
			entryCell4.SetBinding(EntryCell.ValueTextProperty, bindingBase59);
			section13.Add(entryCell4);
			settingsView.Root.Add(section13);
			section14.SetValue(SectionBase.TitleProperty, "ELM327");
			translate51.Text = "Settings_Control_ShowBadELMWarning.Header";
			IMarkupExtension markupExtension66 = translate51;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle133 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 4];
			array68[0] = settingsCheckBoxCellPatched29;
			array68[1] = section14;
			array68[2] = settingsView;
			array68[3] = this;
			object obj127;
			xamlServiceProvider66.Add(typeFromHandle133, obj127 = new SimpleValueTargetProvider(array68, CellBase.TitleProperty, nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj127);
			Type typeFromHandle134 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver66.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver66.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider66.Add(typeFromHandle134, new XamlTypeResolver(xmlNamespaceResolver66, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(298, 49)));
			object obj128 = markupExtension66.ProvideValue(xamlServiceProvider66);
			settingsCheckBoxCellPatched29.Title = obj128;
			bindingExtension60.Mode = 1;
			bindingExtension60.Path = "ShowBadELMWarning";
			bindingExtension60.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowBadELMWarning, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowBadELMWarning = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowBadELMWarning")
			});
			BindingBase bindingBase60 = bindingExtension60.ProvideValue(null);
			settingsCheckBoxCellPatched29.SetBinding(CheckboxCell.CheckedProperty, bindingBase60);
			section14.Add(settingsCheckBoxCellPatched29);
			translate52.Text = "Settings_Control_tbIOTimeout.Text";
			IMarkupExtension markupExtension67 = translate52;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle135 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 4];
			array69[0] = numberPickerCell3;
			array69[1] = section14;
			array69[2] = settingsView;
			array69[3] = this;
			object obj129;
			xamlServiceProvider67.Add(typeFromHandle135, obj129 = new SimpleValueTargetProvider(array69, CellBase.TitleProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj129);
			Type typeFromHandle136 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver67.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver67.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider67.Add(typeFromHandle136, new XamlTypeResolver(xmlNamespaceResolver67, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(300, 21)));
			object obj130 = markupExtension67.ProvideValue(xamlServiceProvider67);
			numberPickerCell3.Title = obj130;
			numberPickerCell3.SetValue(NumberPickerCell.MaxProperty, 60);
			numberPickerCell3.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension61.Mode = 1;
			bindingExtension61.Path = "IOTimeout";
			bindingExtension61.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.IOTimeout, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.IOTimeout = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "IOTimeout")
			});
			BindingBase bindingBase61 = bindingExtension61.ProvideValue(null);
			numberPickerCell3.SetBinding(NumberPickerCell.NumberProperty, bindingBase61);
			section14.Add(numberPickerCell3);
			settingsCheckBoxCellPatched30.SetValue(CellBase.TitleProperty, "ELM327 send ATZ;ATE");
			bindingExtension62.Mode = 1;
			bindingExtension62.Path = "SendATZATE";
			bindingExtension62.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SendATZATE, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SendATZATE = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SendATZATE")
			});
			BindingBase bindingBase62 = bindingExtension62.ProvideValue(null);
			settingsCheckBoxCellPatched30.SetBinding(CheckboxCell.CheckedProperty, bindingBase62);
			section14.Add(settingsCheckBoxCellPatched30);
			translate53.Text = "ios_NoDelayConnection";
			IMarkupExtension markupExtension68 = translate53;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle137 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 4];
			array70[0] = settingsCheckBoxCellPatched31;
			array70[1] = section14;
			array70[2] = settingsView;
			array70[3] = this;
			object obj131;
			xamlServiceProvider68.Add(typeFromHandle137, obj131 = new SimpleValueTargetProvider(array70, CellBase.TitleProperty, nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj131);
			Type typeFromHandle138 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver68.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver68.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider68.Add(typeFromHandle138, new XamlTypeResolver(xmlNamespaceResolver68, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(305, 49)));
			object obj132 = markupExtension68.ProvideValue(xamlServiceProvider68);
			settingsCheckBoxCellPatched31.Title = obj132;
			bindingExtension63.Mode = 1;
			bindingExtension63.Path = "NoDelayELM327Init";
			bindingExtension63.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.NoDelayELM327Init, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.NoDelayELM327Init = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "NoDelayELM327Init")
			});
			BindingBase bindingBase63 = bindingExtension63.ProvideValue(null);
			settingsCheckBoxCellPatched31.SetBinding(CheckboxCell.CheckedProperty, bindingBase63);
			section14.Add(settingsCheckBoxCellPatched31);
			translate54.Text = "settings_ELM327ConnectionAttempts";
			IMarkupExtension markupExtension69 = translate54;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle139 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 4];
			array71[0] = numberPickerCell4;
			array71[1] = section14;
			array71[2] = settingsView;
			array71[3] = this;
			object obj133;
			xamlServiceProvider69.Add(typeFromHandle139, obj133 = new SimpleValueTargetProvider(array71, CellBase.TitleProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj133);
			Type typeFromHandle140 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver69.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver69.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider69.Add(typeFromHandle140, new XamlTypeResolver(xmlNamespaceResolver69, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(308, 21)));
			object obj134 = markupExtension69.ProvideValue(xamlServiceProvider69);
			numberPickerCell4.Title = obj134;
			translate55.Text = "settings_ELM327ConnectionAttempts_Hint";
			IMarkupExtension markupExtension70 = translate55;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle141 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 4];
			array72[0] = numberPickerCell4;
			array72[1] = section14;
			array72[2] = settingsView;
			array72[3] = this;
			object obj135;
			xamlServiceProvider70.Add(typeFromHandle141, obj135 = new SimpleValueTargetProvider(array72, CellBase.DescriptionProperty, nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj135);
			Type typeFromHandle142 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver70.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver70.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider70.Add(typeFromHandle142, new XamlTypeResolver(xmlNamespaceResolver70, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(309, 21)));
			object obj136 = markupExtension70.ProvideValue(xamlServiceProvider70);
			numberPickerCell4.Description = obj136;
			bindingExtension64.Mode = 1;
			bindingExtension64.Path = "ELM327ConnectionAttempts";
			bindingExtension64.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ELM327ConnectionAttempts, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ELM327ConnectionAttempts = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ELM327ConnectionAttempts")
			});
			BindingBase bindingBase64 = bindingExtension64.ProvideValue(null);
			numberPickerCell4.SetBinding(NumberPickerCell.NumberProperty, bindingBase64);
			section14.Add(numberPickerCell4);
			settingsView.Root.Add(section14);
			translate56.Text = "SettingsConnection_ReadPartialErrorAction";
			IMarkupExtension markupExtension71 = translate56;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle143 = typeof(IProvideValueTarget);
			object[] array73 = new object[0 + 3];
			array73[0] = section15;
			array73[1] = settingsView;
			array73[2] = this;
			object obj137;
			xamlServiceProvider71.Add(typeFromHandle143, obj137 = new SimpleValueTargetProvider(array73, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj137);
			Type typeFromHandle144 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver71.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver71.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider71.Add(typeFromHandle144, new XamlTypeResolver(xmlNamespaceResolver71, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(313, 25)));
			object obj138 = markupExtension71.ProvideValue(xamlServiceProvider71);
			section15.Title = obj138;
			bindingExtension65.Mode = 1;
			bindingExtension65.Path = "ReadPartialErrorAction";
			bindingExtension65.TypedBinding = new TypedBinding<SharedSettings, ReadPartialErrorActions>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ReadPartialErrorActions, bool>(A_0.ReadPartialErrorAction, true);
				}
				return default(ValueTuple<ReadPartialErrorActions, bool>);
			}, delegate(SharedSettings A_0, ReadPartialErrorActions A_1)
			{
				if (A_0 != null)
				{
					A_0.ReadPartialErrorAction = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ReadPartialErrorAction")
			});
			BindingBase bindingBase65 = bindingExtension65.ProvideValue(null);
			section15.SetBinding(RadioCell.SelectedValueProperty, bindingBase65);
			translate57.Text = "ReadPartialErrorActions.FullReset";
			IMarkupExtension markupExtension72 = translate57;
			XamlServiceProvider xamlServiceProvider72 = new XamlServiceProvider();
			Type typeFromHandle145 = typeof(IProvideValueTarget);
			object[] array74 = new object[0 + 4];
			array74[0] = radioCell8;
			array74[1] = section15;
			array74[2] = settingsView;
			array74[3] = this;
			object obj139;
			xamlServiceProvider72.Add(typeFromHandle145, obj139 = new SimpleValueTargetProvider(array74, CellBase.TitleProperty, nameScope));
			xamlServiceProvider72.Add(typeof(IReferenceProvider), obj139);
			Type typeFromHandle146 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver72 = new XmlNamespaceResolver();
			xmlNamespaceResolver72.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver72.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver72.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver72.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver72.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver72.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver72.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver72.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver72.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider72.Add(typeFromHandle146, new XamlTypeResolver(xmlNamespaceResolver72, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider72.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 31)));
			object obj140 = markupExtension72.ProvideValue(xamlServiceProvider72);
			radioCell8.Title = obj140;
			radioCell8.SetValue(RadioCell.ValueProperty, readPartialErrorActions);
			section15.Add(radioCell8);
			translate58.Text = "ReadPartialErrorActions.Ignore";
			IMarkupExtension markupExtension73 = translate58;
			XamlServiceProvider xamlServiceProvider73 = new XamlServiceProvider();
			Type typeFromHandle147 = typeof(IProvideValueTarget);
			object[] array75 = new object[0 + 4];
			array75[0] = radioCell9;
			array75[1] = section15;
			array75[2] = settingsView;
			array75[3] = this;
			object obj141;
			xamlServiceProvider73.Add(typeFromHandle147, obj141 = new SimpleValueTargetProvider(array75, CellBase.TitleProperty, nameScope));
			xamlServiceProvider73.Add(typeof(IReferenceProvider), obj141);
			Type typeFromHandle148 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver73 = new XmlNamespaceResolver();
			xmlNamespaceResolver73.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver73.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver73.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver73.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver73.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver73.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver73.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver73.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver73.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider73.Add(typeFromHandle148, new XamlTypeResolver(xmlNamespaceResolver73, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider73.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(315, 31)));
			object obj142 = markupExtension73.ProvideValue(xamlServiceProvider73);
			radioCell9.Title = obj142;
			radioCell9.SetValue(RadioCell.ValueProperty, readPartialErrorActions2);
			section15.Add(radioCell9);
			translate59.Text = "ReadPartialErrorActions.ResetConnection";
			IMarkupExtension markupExtension74 = translate59;
			XamlServiceProvider xamlServiceProvider74 = new XamlServiceProvider();
			Type typeFromHandle149 = typeof(IProvideValueTarget);
			object[] array76 = new object[0 + 4];
			array76[0] = radioCell10;
			array76[1] = section15;
			array76[2] = settingsView;
			array76[3] = this;
			object obj143;
			xamlServiceProvider74.Add(typeFromHandle149, obj143 = new SimpleValueTargetProvider(array76, CellBase.TitleProperty, nameScope));
			xamlServiceProvider74.Add(typeof(IReferenceProvider), obj143);
			Type typeFromHandle150 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver74 = new XmlNamespaceResolver();
			xmlNamespaceResolver74.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver74.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver74.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver74.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver74.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver74.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver74.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver74.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver74.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider74.Add(typeFromHandle150, new XamlTypeResolver(xmlNamespaceResolver74, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider74.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 31)));
			object obj144 = markupExtension74.ProvideValue(xamlServiceProvider74);
			radioCell10.Title = obj144;
			radioCell10.SetValue(RadioCell.ValueProperty, readPartialErrorActions3);
			section15.Add(radioCell10);
			settingsView.Root.Add(section15);
			section16.SetValue(SectionBase.TitleProperty, "NO DATA Limit before reconnect:");
			entryCell5.SetValue(CellBase.TitleProperty, "");
			bindingExtension66.Mode = 1;
			staticResourceExtension12.Key = "IntToStringConverter";
			IMarkupExtension markupExtension75 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider75 = new XamlServiceProvider();
			Type typeFromHandle151 = typeof(IProvideValueTarget);
			object[] array77 = new object[0 + 5];
			array77[0] = bindingExtension66;
			array77[1] = entryCell5;
			array77[2] = section16;
			array77[3] = settingsView;
			array77[4] = this;
			object obj145;
			xamlServiceProvider75.Add(typeFromHandle151, obj145 = new SimpleValueTargetProvider(array77, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider75.Add(typeof(IReferenceProvider), obj145);
			Type typeFromHandle152 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver75 = new XmlNamespaceResolver();
			xmlNamespaceResolver75.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver75.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver75.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver75.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver75.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver75.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver75.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver75.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver75.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider75.Add(typeFromHandle152, new XamlTypeResolver(xmlNamespaceResolver75, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider75.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(320, 40)));
			object obj146 = markupExtension75.ProvideValue(xamlServiceProvider75);
			bindingExtension66.Converter = obj146;
			bindingExtension66.Path = "NoDataLimit";
			bindingExtension66.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.NoDataLimit, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.NoDataLimit = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "NoDataLimit")
			});
			BindingBase bindingBase66 = bindingExtension66.ProvideValue(null);
			entryCell5.SetBinding(EntryCell.ValueTextProperty, bindingBase66);
			section16.Add(entryCell5);
			settingsView.Root.Add(section16);
			translate60.Text = "Settings_Control_tbDelay.Text";
			IMarkupExtension markupExtension76 = translate60;
			XamlServiceProvider xamlServiceProvider76 = new XamlServiceProvider();
			Type typeFromHandle153 = typeof(IProvideValueTarget);
			object[] array78 = new object[0 + 3];
			array78[0] = section17;
			array78[1] = settingsView;
			array78[2] = this;
			object obj147;
			xamlServiceProvider76.Add(typeFromHandle153, obj147 = new SimpleValueTargetProvider(array78, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider76.Add(typeof(IReferenceProvider), obj147);
			Type typeFromHandle154 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver76 = new XmlNamespaceResolver();
			xmlNamespaceResolver76.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver76.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver76.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver76.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver76.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver76.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver76.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver76.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver76.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider76.Add(typeFromHandle154, new XamlTypeResolver(xmlNamespaceResolver76, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider76.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(328, 25)));
			object obj148 = markupExtension76.ProvideValue(xamlServiceProvider76);
			section17.Title = obj148;
			bindingExtension67.Path = "ShowExperimental";
			bindingExtension67.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase67 = bindingExtension67.ProvideValue(null);
			section17.SetBinding(Section.IsVisibleProperty, bindingBase67);
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension68.Mode = 1;
			bindingExtension68.Path = "SendDelay";
			bindingExtension68.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SendDelay, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.SendDelay = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SendDelay")
			});
			BindingBase bindingBase68 = bindingExtension68.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase68);
			customCell2.SetValue(CustomCell.ContentProperty, numericEntryV);
			section17.Add(customCell2);
			settingsView.Root.Add(section17);
			section18.SetValue(SectionBase.TitleProperty, "Dataset upload max block size:");
			bindingExtension69.Path = "ShowExperimental";
			bindingExtension69.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase69 = bindingExtension69.ProvideValue(null);
			section18.SetBinding(Section.IsVisibleProperty, bindingBase69);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV2.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			bindingExtension70.Path = "ShowExperimental";
			bindingExtension70.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase70 = bindingExtension70.ProvideValue(null);
			numericEntryV2.SetBinding(VisualElement.IsVisibleProperty, bindingBase70);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension71.Mode = 1;
			bindingExtension71.Path = "DatasetUploadMaxBlockSize";
			bindingExtension71.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DatasetUploadMaxBlockSize, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.DatasetUploadMaxBlockSize = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DatasetUploadMaxBlockSize")
			});
			BindingBase bindingBase71 = bindingExtension71.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase71);
			customCell3.SetValue(CustomCell.ContentProperty, numericEntryV2);
			section18.Add(customCell3);
			settingsView.Root.Add(section18);
			translate61.Text = "settings_SendTesterPresentWhileUpload";
			IMarkupExtension markupExtension77 = translate61;
			XamlServiceProvider xamlServiceProvider77 = new XamlServiceProvider();
			Type typeFromHandle155 = typeof(IProvideValueTarget);
			object[] array79 = new object[0 + 3];
			array79[0] = section19;
			array79[1] = settingsView;
			array79[2] = this;
			object obj149;
			xamlServiceProvider77.Add(typeFromHandle155, obj149 = new SimpleValueTargetProvider(array79, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider77.Add(typeof(IReferenceProvider), obj149);
			Type typeFromHandle156 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver77 = new XmlNamespaceResolver();
			xmlNamespaceResolver77.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver77.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver77.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver77.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver77.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver77.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver77.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver77.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver77.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider77.Add(typeFromHandle156, new XamlTypeResolver(xmlNamespaceResolver77, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider77.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(348, 25)));
			object obj150 = markupExtension77.ProvideValue(xamlServiceProvider77);
			section19.Title = obj150;
			bindingExtension72.Path = "ShowExperimental";
			bindingExtension72.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase72 = bindingExtension72.ProvideValue(null);
			section19.SetBinding(Section.IsVisibleProperty, bindingBase72);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV3.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			bindingExtension73.Path = "ShowExperimental";
			bindingExtension73.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowExperimental = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowExperimental")
			});
			BindingBase bindingBase73 = bindingExtension73.ProvideValue(null);
			numericEntryV3.SetBinding(VisualElement.IsVisibleProperty, bindingBase73);
			numericEntryV3.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension74.Mode = 1;
			bindingExtension74.Path = "SendTesterPresentWhileLongUploadTimeMs";
			bindingExtension74.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SendTesterPresentWhileLongUploadTimeMs, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.SendTesterPresentWhileLongUploadTimeMs = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SendTesterPresentWhileLongUploadTimeMs")
			});
			BindingBase bindingBase74 = bindingExtension74.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase74);
			customCell4.SetValue(CustomCell.ContentProperty, numericEntryV3);
			section19.Add(customCell4);
			settingsView.Root.Add(section19);
			translate62.Text = "droid_WiFiMode";
			IMarkupExtension markupExtension78 = translate62;
			XamlServiceProvider xamlServiceProvider78 = new XamlServiceProvider();
			Type typeFromHandle157 = typeof(IProvideValueTarget);
			object[] array80 = new object[0 + 3];
			array80[0] = section20;
			array80[1] = settingsView;
			array80[2] = this;
			object obj151;
			xamlServiceProvider78.Add(typeFromHandle157, obj151 = new SimpleValueTargetProvider(array80, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider78.Add(typeof(IReferenceProvider), obj151);
			Type typeFromHandle158 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver78 = new XmlNamespaceResolver();
			xmlNamespaceResolver78.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver78.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver78.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver78.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver78.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver78.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver78.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver78.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver78.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider78.Add(typeFromHandle158, new XamlTypeResolver(xmlNamespaceResolver78, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider78.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(360, 17)));
			object obj152 = markupExtension78.ProvideValue(xamlServiceProvider78);
			section20.Title = obj152;
			bindingExtension75.Mode = 2;
			staticResourceExtension13.Key = "ConnectionTypeToWiFiVisibleBoolConverterOnlyAndroid";
			IMarkupExtension markupExtension79 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider79 = new XamlServiceProvider();
			Type typeFromHandle159 = typeof(IProvideValueTarget);
			object[] array81 = new object[0 + 4];
			array81[0] = bindingExtension75;
			array81[1] = section20;
			array81[2] = settingsView;
			array81[3] = this;
			object obj153;
			xamlServiceProvider79.Add(typeFromHandle159, obj153 = new SimpleValueTargetProvider(array81, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider79.Add(typeof(IReferenceProvider), obj153);
			Type typeFromHandle160 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver79 = new XmlNamespaceResolver();
			xmlNamespaceResolver79.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver79.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver79.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver79.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver79.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver79.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver79.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver79.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver79.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider79.Add(typeFromHandle160, new XamlTypeResolver(xmlNamespaceResolver79, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider79.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(361, 17)));
			object obj154 = markupExtension79.ProvideValue(xamlServiceProvider79);
			bindingExtension75.Converter = obj154;
			bindingExtension75.Path = "ConnectionType";
			bindingExtension75.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
				}
				return default(ValueTuple<ConnectionTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectionType")
			});
			BindingBase bindingBase75 = bindingExtension75.ProvideValue(null);
			section20.SetBinding(Section.IsVisibleProperty, bindingBase75);
			staticResourceExtension14.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension80 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider80 = new XamlServiceProvider();
			Type typeFromHandle161 = typeof(IProvideValueTarget);
			object[] array82 = new object[0 + 6];
			array82[0] = label5;
			array82[1] = grid4;
			array82[2] = settingsCustomCellForPicker4;
			array82[3] = section20;
			array82[4] = settingsView;
			array82[5] = this;
			object obj155;
			xamlServiceProvider80.Add(typeFromHandle161, obj155 = new SimpleValueTargetProvider(array82, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider80.Add(typeof(IReferenceProvider), obj155);
			Type typeFromHandle162 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver80 = new XmlNamespaceResolver();
			xmlNamespaceResolver80.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver80.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver80.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver80.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver80.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver80.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver80.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver80.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver80.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider80.Add(typeFromHandle162, new XamlTypeResolver(xmlNamespaceResolver80, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider80.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(364, 32)));
			object obj156 = markupExtension80.ProvideValue(xamlServiceProvider80);
			label5.Style = obj156;
			staticResourceExtension15.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension81 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider81 = new XamlServiceProvider();
			Type typeFromHandle163 = typeof(IProvideValueTarget);
			object[] array83 = new object[0 + 7];
			array83[0] = bindingExtension76;
			array83[1] = label5;
			array83[2] = grid4;
			array83[3] = settingsCustomCellForPicker4;
			array83[4] = section20;
			array83[5] = settingsView;
			array83[6] = this;
			object obj157;
			xamlServiceProvider81.Add(typeFromHandle163, obj157 = new SimpleValueTargetProvider(array83, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider81.Add(typeof(IReferenceProvider), obj157);
			Type typeFromHandle164 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver81 = new XmlNamespaceResolver();
			xmlNamespaceResolver81.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver81.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver81.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver81.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver81.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver81.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver81.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver81.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver81.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider81.Add(typeFromHandle164, new XamlTypeResolver(xmlNamespaceResolver81, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider81.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(367, 37)));
			object obj158 = markupExtension81.ProvideValue(xamlServiceProvider81);
			bindingExtension76.Converter = obj158;
			bindingExtension76.Mode = 2;
			bindingExtension76.Path = "AndroidWiFiMode";
			bindingExtension76.ConverterParameter = array;
			bindingExtension76.TypedBinding = new TypedBinding<SharedSettings, AndroidWiFiConnectionModes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<AndroidWiFiConnectionModes, bool>(A_0.AndroidWiFiMode, true);
				}
				return default(ValueTuple<AndroidWiFiConnectionModes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidWiFiMode")
			});
			BindingBase bindingBase76 = bindingExtension76.ProvideValue(null);
			label5.SetBinding(Label.TextProperty, bindingBase76);
			grid4.Children.Add(label5);
			picker4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension77.Mode = 1;
			staticResourceExtension16.Key = "AndroidWiFiConnectionModesToIntConverter";
			IMarkupExtension markupExtension82 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider82 = new XamlServiceProvider();
			Type typeFromHandle165 = typeof(IProvideValueTarget);
			object[] array84 = new object[0 + 7];
			array84[0] = bindingExtension77;
			array84[1] = picker4;
			array84[2] = grid4;
			array84[3] = settingsCustomCellForPicker4;
			array84[4] = section20;
			array84[5] = settingsView;
			array84[6] = this;
			object obj159;
			xamlServiceProvider82.Add(typeFromHandle165, obj159 = new SimpleValueTargetProvider(array84, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider82.Add(typeof(IReferenceProvider), obj159);
			Type typeFromHandle166 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver82 = new XmlNamespaceResolver();
			xmlNamespaceResolver82.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver82.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver82.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver82.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver82.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver82.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver82.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver82.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver82.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider82.Add(typeFromHandle166, new XamlTypeResolver(xmlNamespaceResolver82, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider82.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(382, 51)));
			object obj160 = markupExtension82.ProvideValue(xamlServiceProvider82);
			bindingExtension77.Converter = obj160;
			bindingExtension77.Path = "AndroidWiFiMode";
			bindingExtension77.TypedBinding = new TypedBinding<SharedSettings, AndroidWiFiConnectionModes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<AndroidWiFiConnectionModes, bool>(A_0.AndroidWiFiMode, true);
				}
				return default(ValueTuple<AndroidWiFiConnectionModes, bool>);
			}, delegate(SharedSettings A_0, AndroidWiFiConnectionModes A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidWiFiMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidWiFiMode")
			});
			BindingBase bindingBase77 = bindingExtension77.ProvideValue(null);
			picker4.SetBinding(Picker.SelectedIndexProperty, bindingBase77);
			picker4.SetValue(Picker.ItemsSourceProperty, array2);
			grid4.Children.Add(picker4);
			settingsCustomCellForPicker4.SetValue(CustomCell.ContentProperty, grid4);
			section20.Add(settingsCustomCellForPicker4);
			settingsView.Root.Add(section20);
			translate63.Text = "droid_BT_Method";
			IMarkupExtension markupExtension83 = translate63;
			XamlServiceProvider xamlServiceProvider83 = new XamlServiceProvider();
			Type typeFromHandle167 = typeof(IProvideValueTarget);
			object[] array85 = new object[0 + 3];
			array85[0] = section21;
			array85[1] = settingsView;
			array85[2] = this;
			object obj161;
			xamlServiceProvider83.Add(typeFromHandle167, obj161 = new SimpleValueTargetProvider(array85, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider83.Add(typeof(IReferenceProvider), obj161);
			Type typeFromHandle168 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver83 = new XmlNamespaceResolver();
			xmlNamespaceResolver83.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver83.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver83.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver83.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver83.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver83.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver83.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver83.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver83.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider83.Add(typeFromHandle168, new XamlTypeResolver(xmlNamespaceResolver83, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider83.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(400, 17)));
			object obj162 = markupExtension83.ProvideValue(xamlServiceProvider83);
			section21.Title = obj162;
			bindingExtension78.Mode = 2;
			staticResourceExtension17.Key = "ConnectionTypeToBTVisibleBoolConverterOnlyForAndroid";
			IMarkupExtension markupExtension84 = staticResourceExtension17;
			XamlServiceProvider xamlServiceProvider84 = new XamlServiceProvider();
			Type typeFromHandle169 = typeof(IProvideValueTarget);
			object[] array86 = new object[0 + 4];
			array86[0] = bindingExtension78;
			array86[1] = section21;
			array86[2] = settingsView;
			array86[3] = this;
			object obj163;
			xamlServiceProvider84.Add(typeFromHandle169, obj163 = new SimpleValueTargetProvider(array86, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider84.Add(typeof(IReferenceProvider), obj163);
			Type typeFromHandle170 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver84 = new XmlNamespaceResolver();
			xmlNamespaceResolver84.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver84.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver84.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver84.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver84.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver84.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver84.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver84.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver84.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider84.Add(typeFromHandle170, new XamlTypeResolver(xmlNamespaceResolver84, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider84.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(401, 17)));
			object obj164 = markupExtension84.ProvideValue(xamlServiceProvider84);
			bindingExtension78.Converter = obj164;
			bindingExtension78.Path = "ConnectionType";
			bindingExtension78.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
				}
				return default(ValueTuple<ConnectionTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectionType")
			});
			BindingBase bindingBase78 = bindingExtension78.ProvideValue(null);
			section21.SetBinding(Section.IsVisibleProperty, bindingBase78);
			staticResourceExtension18.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension85 = staticResourceExtension18;
			XamlServiceProvider xamlServiceProvider85 = new XamlServiceProvider();
			Type typeFromHandle171 = typeof(IProvideValueTarget);
			object[] array87 = new object[0 + 6];
			array87[0] = label6;
			array87[1] = grid5;
			array87[2] = settingsCustomCellForPicker5;
			array87[3] = section21;
			array87[4] = settingsView;
			array87[5] = this;
			object obj165;
			xamlServiceProvider85.Add(typeFromHandle171, obj165 = new SimpleValueTargetProvider(array87, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider85.Add(typeof(IReferenceProvider), obj165);
			Type typeFromHandle172 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver85 = new XmlNamespaceResolver();
			xmlNamespaceResolver85.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver85.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver85.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver85.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver85.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver85.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver85.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver85.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver85.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider85.Add(typeFromHandle172, new XamlTypeResolver(xmlNamespaceResolver85, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider85.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(405, 32)));
			object obj166 = markupExtension85.ProvideValue(xamlServiceProvider85);
			label6.Style = obj166;
			staticResourceExtension19.Key = "IntToStringInItemsConverter";
			IMarkupExtension markupExtension86 = staticResourceExtension19;
			XamlServiceProvider xamlServiceProvider86 = new XamlServiceProvider();
			Type typeFromHandle173 = typeof(IProvideValueTarget);
			object[] array88 = new object[0 + 7];
			array88[0] = bindingExtension79;
			array88[1] = label6;
			array88[2] = grid5;
			array88[3] = settingsCustomCellForPicker5;
			array88[4] = section21;
			array88[5] = settingsView;
			array88[6] = this;
			object obj167;
			xamlServiceProvider86.Add(typeFromHandle173, obj167 = new SimpleValueTargetProvider(array88, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider86.Add(typeof(IReferenceProvider), obj167);
			Type typeFromHandle174 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver86 = new XmlNamespaceResolver();
			xmlNamespaceResolver86.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver86.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver86.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver86.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver86.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver86.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver86.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver86.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver86.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider86.Add(typeFromHandle174, new XamlTypeResolver(xmlNamespaceResolver86, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider86.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(405, 82)));
			object obj168 = markupExtension86.ProvideValue(xamlServiceProvider86);
			bindingExtension79.Converter = obj168;
			bindingExtension79.ConverterParameter = androidConnectionMethods;
			bindingExtension79.Path = "AndroidBluetoothConnectionMethod";
			bindingExtension79.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.AndroidBluetoothConnectionMethod, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidBluetoothConnectionMethod = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidBluetoothConnectionMethod")
			});
			BindingBase bindingBase79 = bindingExtension79.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase79);
			grid5.Children.Add(label6);
			picker5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension80.Source = androidConnectionMethods2;
			BindingBase bindingBase80 = bindingExtension80.ProvideValue(null);
			picker5.SetBinding(Picker.ItemsSourceProperty, bindingBase80);
			bindingExtension81.Mode = 1;
			bindingExtension81.Path = "AndroidBluetoothConnectionMethod";
			bindingExtension81.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.AndroidBluetoothConnectionMethod, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidBluetoothConnectionMethod = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidBluetoothConnectionMethod")
			});
			BindingBase bindingBase81 = bindingExtension81.ProvideValue(null);
			picker5.SetBinding(Picker.SelectedIndexProperty, bindingBase81);
			grid5.Children.Add(picker5);
			settingsCustomCellForPicker5.SetValue(CustomCell.ContentProperty, grid5);
			section21.Add(settingsCustomCellForPicker5);
			translate64.Text = "DroidAskForBluetoothPermissions";
			IMarkupExtension markupExtension87 = translate64;
			XamlServiceProvider xamlServiceProvider87 = new XamlServiceProvider();
			Type typeFromHandle175 = typeof(IProvideValueTarget);
			object[] array89 = new object[0 + 4];
			array89[0] = settingsCheckBoxCellPatched32;
			array89[1] = section21;
			array89[2] = settingsView;
			array89[3] = this;
			object obj169;
			xamlServiceProvider87.Add(typeFromHandle175, obj169 = new SimpleValueTargetProvider(array89, CellBase.TitleProperty, nameScope));
			xamlServiceProvider87.Add(typeof(IReferenceProvider), obj169);
			Type typeFromHandle176 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver87 = new XmlNamespaceResolver();
			xmlNamespaceResolver87.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver87.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver87.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver87.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver87.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver87.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver87.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver87.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver87.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider87.Add(typeFromHandle176, new XamlTypeResolver(xmlNamespaceResolver87, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider87.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(412, 49)));
			object obj170 = markupExtension87.ProvideValue(xamlServiceProvider87);
			settingsCheckBoxCellPatched32.Title = obj170;
			bindingExtension82.Mode = 1;
			bindingExtension82.Path = "DroidAskForBluetoothPermissions";
			bindingExtension82.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DroidAskForBluetoothPermissions, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DroidAskForBluetoothPermissions = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DroidAskForBluetoothPermissions")
			});
			BindingBase bindingBase82 = bindingExtension82.ProvideValue(null);
			settingsCheckBoxCellPatched32.SetBinding(CheckboxCell.CheckedProperty, bindingBase82);
			section21.Add(settingsCheckBoxCellPatched32);
			translate65.Text = "droid_TryTurnOnBluetooth";
			IMarkupExtension markupExtension88 = translate65;
			XamlServiceProvider xamlServiceProvider88 = new XamlServiceProvider();
			Type typeFromHandle177 = typeof(IProvideValueTarget);
			object[] array90 = new object[0 + 4];
			array90[0] = settingsCheckBoxCellPatched33;
			array90[1] = section21;
			array90[2] = settingsView;
			array90[3] = this;
			object obj171;
			xamlServiceProvider88.Add(typeFromHandle177, obj171 = new SimpleValueTargetProvider(array90, CellBase.TitleProperty, nameScope));
			xamlServiceProvider88.Add(typeof(IReferenceProvider), obj171);
			Type typeFromHandle178 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver88 = new XmlNamespaceResolver();
			xmlNamespaceResolver88.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver88.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver88.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver88.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver88.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver88.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver88.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver88.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver88.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider88.Add(typeFromHandle178, new XamlTypeResolver(xmlNamespaceResolver88, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider88.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(413, 49)));
			object obj172 = markupExtension88.ProvideValue(xamlServiceProvider88);
			settingsCheckBoxCellPatched33.Title = obj172;
			bindingExtension83.Mode = 1;
			bindingExtension83.Path = "DroidTryTurnOnBluetooth";
			bindingExtension83.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DroidTryTurnOnBluetooth, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DroidTryTurnOnBluetooth = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DroidTryTurnOnBluetooth")
			});
			BindingBase bindingBase83 = bindingExtension83.ProvideValue(null);
			settingsCheckBoxCellPatched33.SetBinding(CheckboxCell.CheckedProperty, bindingBase83);
			section21.Add(settingsCheckBoxCellPatched33);
			translate66.Text = "settings_CheckBluetoothState";
			IMarkupExtension markupExtension89 = translate66;
			XamlServiceProvider xamlServiceProvider89 = new XamlServiceProvider();
			Type typeFromHandle179 = typeof(IProvideValueTarget);
			object[] array91 = new object[0 + 4];
			array91[0] = settingsCheckBoxCellPatched34;
			array91[1] = section21;
			array91[2] = settingsView;
			array91[3] = this;
			object obj173;
			xamlServiceProvider89.Add(typeFromHandle179, obj173 = new SimpleValueTargetProvider(array91, CellBase.TitleProperty, nameScope));
			xamlServiceProvider89.Add(typeof(IReferenceProvider), obj173);
			Type typeFromHandle180 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver89 = new XmlNamespaceResolver();
			xmlNamespaceResolver89.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver89.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver89.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver89.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver89.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver89.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver89.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver89.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver89.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider89.Add(typeFromHandle180, new XamlTypeResolver(xmlNamespaceResolver89, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider89.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(414, 49)));
			object obj174 = markupExtension89.ProvideValue(xamlServiceProvider89);
			settingsCheckBoxCellPatched34.Title = obj174;
			bindingExtension84.Mode = 1;
			bindingExtension84.Path = "CheckBluetoothTurnedOn";
			bindingExtension84.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.CheckBluetoothTurnedOn, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.CheckBluetoothTurnedOn = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CheckBluetoothTurnedOn")
			});
			BindingBase bindingBase84 = bindingExtension84.ProvideValue(null);
			settingsCheckBoxCellPatched34.SetBinding(CheckboxCell.CheckedProperty, bindingBase84);
			section21.Add(settingsCheckBoxCellPatched34);
			settingsView.Root.Add(section21);
			section22.SetValue(SectionBase.TitleProperty, "Bluetooth LE (4.0)");
			bindingExtension85.Mode = 2;
			staticResourceExtension20.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension90 = staticResourceExtension20;
			XamlServiceProvider xamlServiceProvider90 = new XamlServiceProvider();
			Type typeFromHandle181 = typeof(IProvideValueTarget);
			object[] array92 = new object[0 + 4];
			array92[0] = bindingExtension85;
			array92[1] = section22;
			array92[2] = settingsView;
			array92[3] = this;
			object obj175;
			xamlServiceProvider90.Add(typeFromHandle181, obj175 = new SimpleValueTargetProvider(array92, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider90.Add(typeof(IReferenceProvider), obj175);
			Type typeFromHandle182 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver90 = new XmlNamespaceResolver();
			xmlNamespaceResolver90.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver90.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver90.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver90.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver90.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver90.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver90.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver90.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver90.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider90.Add(typeFromHandle182, new XamlTypeResolver(xmlNamespaceResolver90, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider90.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(421, 17)));
			object obj176 = markupExtension90.ProvideValue(xamlServiceProvider90);
			bindingExtension85.Converter = obj176;
			bindingExtension85.Path = "ConnectionType";
			bindingExtension85.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
				}
				return default(ValueTuple<ConnectionTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectionType")
			});
			BindingBase bindingBase85 = bindingExtension85.ProvideValue(null);
			section22.SetBinding(Section.IsVisibleProperty, bindingBase85);
			translate67.Text = "settings_BTLEShowDevicesWithoutName";
			IMarkupExtension markupExtension91 = translate67;
			XamlServiceProvider xamlServiceProvider91 = new XamlServiceProvider();
			Type typeFromHandle183 = typeof(IProvideValueTarget);
			object[] array93 = new object[0 + 4];
			array93[0] = settingsCheckBoxCellPatched35;
			array93[1] = section22;
			array93[2] = settingsView;
			array93[3] = this;
			object obj177;
			xamlServiceProvider91.Add(typeFromHandle183, obj177 = new SimpleValueTargetProvider(array93, CellBase.TitleProperty, nameScope));
			xamlServiceProvider91.Add(typeof(IReferenceProvider), obj177);
			Type typeFromHandle184 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver91 = new XmlNamespaceResolver();
			xmlNamespaceResolver91.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver91.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver91.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver91.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver91.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver91.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver91.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver91.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver91.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider91.Add(typeFromHandle184, new XamlTypeResolver(xmlNamespaceResolver91, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider91.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(422, 49)));
			object obj178 = markupExtension91.ProvideValue(xamlServiceProvider91);
			settingsCheckBoxCellPatched35.Title = obj178;
			bindingExtension86.Mode = 1;
			bindingExtension86.Path = "BTLEShowDevicesWithoutName";
			bindingExtension86.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.BTLEShowDevicesWithoutName, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.BTLEShowDevicesWithoutName = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "BTLEShowDevicesWithoutName")
			});
			BindingBase bindingBase86 = bindingExtension86.ProvideValue(null);
			settingsCheckBoxCellPatched35.SetBinding(CheckboxCell.CheckedProperty, bindingBase86);
			section22.Add(settingsCheckBoxCellPatched35);
			settingsView.Root.Add(section22);
			section23.SetValue(SectionBase.TitleProperty, "VW TP2.0");
			entryCell6.SetValue(CellBase.TitleProperty, "Channel setup ATST");
			bindingExtension87.Mode = 1;
			bindingExtension87.Path = "VWTP_ChannelSetupATST";
			bindingExtension87.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.VWTP_ChannelSetupATST, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.VWTP_ChannelSetupATST = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VWTP_ChannelSetupATST")
			});
			BindingBase bindingBase87 = bindingExtension87.ProvideValue(null);
			entryCell6.SetBinding(EntryCell.ValueTextProperty, bindingBase87);
			section23.Add(entryCell6);
			entryCell7.SetValue(CellBase.TitleProperty, "Active connection timeout");
			bindingExtension88.Mode = 1;
			staticResourceExtension21.Key = "IntToStringConverter";
			IMarkupExtension markupExtension92 = staticResourceExtension21;
			XamlServiceProvider xamlServiceProvider92 = new XamlServiceProvider();
			Type typeFromHandle185 = typeof(IProvideValueTarget);
			object[] array94 = new object[0 + 5];
			array94[0] = bindingExtension88;
			array94[1] = entryCell7;
			array94[2] = section23;
			array94[3] = settingsView;
			array94[4] = this;
			object obj179;
			xamlServiceProvider92.Add(typeFromHandle185, obj179 = new SimpleValueTargetProvider(array94, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider92.Add(typeof(IReferenceProvider), obj179);
			Type typeFromHandle186 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver92 = new XmlNamespaceResolver();
			xmlNamespaceResolver92.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver92.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver92.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver92.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver92.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver92.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver92.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver92.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver92.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider92.Add(typeFromHandle186, new XamlTypeResolver(xmlNamespaceResolver92, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider92.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(429, 65)));
			object obj180 = markupExtension92.ProvideValue(xamlServiceProvider92);
			bindingExtension88.Converter = obj180;
			bindingExtension88.Path = "VWTP_ActiveConnectionTestTimeout";
			bindingExtension88.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VWTP_ActiveConnectionTestTimeout, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VWTP_ActiveConnectionTestTimeout = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VWTP_ActiveConnectionTestTimeout")
			});
			BindingBase bindingBase88 = bindingExtension88.ProvideValue(null);
			entryCell7.SetBinding(EntryCell.ValueTextProperty, bindingBase88);
			section23.Add(entryCell7);
			entryCell8.SetValue(CellBase.TitleProperty, "Send A3 period:");
			bindingExtension89.Mode = 1;
			staticResourceExtension22.Key = "IntToStringConverter";
			IMarkupExtension markupExtension93 = staticResourceExtension22;
			XamlServiceProvider xamlServiceProvider93 = new XamlServiceProvider();
			Type typeFromHandle187 = typeof(IProvideValueTarget);
			object[] array95 = new object[0 + 5];
			array95[0] = bindingExtension89;
			array95[1] = entryCell8;
			array95[2] = section23;
			array95[3] = settingsView;
			array95[4] = this;
			object obj181;
			xamlServiceProvider93.Add(typeFromHandle187, obj181 = new SimpleValueTargetProvider(array95, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider93.Add(typeof(IReferenceProvider), obj181);
			Type typeFromHandle188 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver93 = new XmlNamespaceResolver();
			xmlNamespaceResolver93.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver93.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver93.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver93.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver93.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver93.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver93.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver93.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver93.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider93.Add(typeFromHandle188, new XamlTypeResolver(xmlNamespaceResolver93, typeof(SettingsConnectionExpertV3).GetTypeInfo().Assembly));
			xamlServiceProvider93.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(430, 55)));
			object obj182 = markupExtension93.ProvideValue(xamlServiceProvider93);
			bindingExtension89.Converter = obj182;
			bindingExtension89.Path = "VWTP_SendConnectionConfirmationPeriod";
			bindingExtension89.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VWTP_SendConnectionConfirmationPeriod, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VWTP_SendConnectionConfirmationPeriod = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VWTP_SendConnectionConfirmationPeriod")
			});
			BindingBase bindingBase89 = bindingExtension89.ProvideValue(null);
			entryCell8.SetBinding(EntryCell.ValueTextProperty, bindingBase89);
			section23.Add(entryCell8);
			settingsView.Root.Add(section23);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06001EFD RID: 7933 RVA: 0x00162F78 File Offset: 0x00161178
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsConnectionExpertV3>(this, typeof(SettingsConnectionExpertV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.DefaultConnectionPanel = NameScopeExtensions.FindByName<Section>(this, "DefaultConnectionPanel");
			this.btnForceMode22Learning = NameScopeExtensions.FindByName<ButtonCell>(this, "btnForceMode22Learning");
			this.btnClearOptimizationData = NameScopeExtensions.FindByName<ButtonCell>(this, "btnClearOptimizationData");
			this.wifiPanel = NameScopeExtensions.FindByName<Section>(this, "wifiPanel");
			this.bluetooth2Panel = NameScopeExtensions.FindByName<Section>(this, "bluetooth2Panel");
			this.bluetoothLEPanel = NameScopeExtensions.FindByName<Section>(this, "bluetoothLEPanel");
		}

		// Token: 0x06001EFE RID: 7934 RVA: 0x00163010 File Offset: 0x00161210
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1593(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001EFF RID: 7935 RVA: 0x00163040 File Offset: 0x00161240
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1594(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseDefaultInit = A_1;
				return;
			}
		}

		// Token: 0x06001F00 RID: 7936 RVA: 0x0016305C File Offset: 0x0016125C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1595(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x0016306C File Offset: 0x0016126C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1596(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseOBD2, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x0016309C File Offset: 0x0016129C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1597(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseOBD2 = A_1;
				return;
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x001630B8 File Offset: 0x001612B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1598(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F04 RID: 7940 RVA: 0x001630C8 File Offset: 0x001612C8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1599(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F05 RID: 7941 RVA: 0x001630F8 File Offset: 0x001612F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1600(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F06 RID: 7942 RVA: 0x00163108 File Offset: 0x00161308
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1601(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CheckOnlyPositiveResponseMarker, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F07 RID: 7943 RVA: 0x00163138 File Offset: 0x00161338
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1602(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CheckOnlyPositiveResponseMarker = A_1;
				return;
			}
		}

		// Token: 0x06001F08 RID: 7944 RVA: 0x00163154 File Offset: 0x00161354
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1603(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F09 RID: 7945 RVA: 0x00163164 File Offset: 0x00161364
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1604(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F0A RID: 7946 RVA: 0x00163194 File Offset: 0x00161394
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1605(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F0B RID: 7947 RVA: 0x001631A4 File Offset: 0x001613A4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1606(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ProtocolNumber, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F0C RID: 7948 RVA: 0x001631D4 File Offset: 0x001613D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1607(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F0D RID: 7949 RVA: 0x001631E4 File Offset: 0x001613E4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1608(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ProtocolNumber, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F0E RID: 7950 RVA: 0x00163214 File Offset: 0x00161414
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1609(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ProtocolNumber = A_1;
				return;
			}
		}

		// Token: 0x06001F0F RID: 7951 RVA: 0x00163230 File Offset: 0x00161430
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1610(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F10 RID: 7952 RVA: 0x00163240 File Offset: 0x00161440
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1611(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ForceOnlyOneProtocol, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F11 RID: 7953 RVA: 0x00163270 File Offset: 0x00161470
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1612(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ForceOnlyOneProtocol = A_1;
				return;
			}
		}

		// Token: 0x06001F12 RID: 7954 RVA: 0x0016328C File Offset: 0x0016148C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1613(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F13 RID: 7955 RVA: 0x0016329C File Offset: 0x0016149C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1614(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F14 RID: 7956 RVA: 0x001632CC File Offset: 0x001614CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1615(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F15 RID: 7957 RVA: 0x001632DC File Offset: 0x001614DC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1616(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.AdaptiveTimings, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F16 RID: 7958 RVA: 0x0016330C File Offset: 0x0016150C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1617(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F17 RID: 7959 RVA: 0x0016331C File Offset: 0x0016151C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1618(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.AdaptiveTimings, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F18 RID: 7960 RVA: 0x0016334C File Offset: 0x0016154C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1619(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.AdaptiveTimings = A_1;
				return;
			}
		}

		// Token: 0x06001F19 RID: 7961 RVA: 0x00163368 File Offset: 0x00161568
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1620(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F1A RID: 7962 RVA: 0x00163378 File Offset: 0x00161578
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1621(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F1B RID: 7963 RVA: 0x001633A8 File Offset: 0x001615A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1622(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F1C RID: 7964 RVA: 0x001633B8 File Offset: 0x001615B8
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1623(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ATSTIdx, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F1D RID: 7965 RVA: 0x001633E8 File Offset: 0x001615E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1624(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F1E RID: 7966 RVA: 0x001633F8 File Offset: 0x001615F8
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1625(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ATSTIdx, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F1F RID: 7967 RVA: 0x00163428 File Offset: 0x00161628
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1626(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ATSTIdx = A_1;
				return;
			}
		}

		// Token: 0x06001F20 RID: 7968 RVA: 0x00163444 File Offset: 0x00161644
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1627(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F21 RID: 7969 RVA: 0x00163454 File Offset: 0x00161654
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1628(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseDefaultInit, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F22 RID: 7970 RVA: 0x00163484 File Offset: 0x00161684
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1629(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F23 RID: 7971 RVA: 0x00163494 File Offset: 0x00161694
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1630(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.CustomInitString, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001F24 RID: 7972 RVA: 0x001634C4 File Offset: 0x001616C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1631(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x001634D4 File Offset: 0x001616D4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1632(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.DefaultFunctionalHeader, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x00163504 File Offset: 0x00161704
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1633(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.DefaultFunctionalHeader = A_1;
				return;
			}
		}

		// Token: 0x06001F27 RID: 7975 RVA: 0x00163520 File Offset: 0x00161720
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1634(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x00163530 File Offset: 0x00161730
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1635(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.DetectECUConnectionPID, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x00163560 File Offset: 0x00161760
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1636(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.DetectECUConnectionPID = A_1;
				return;
			}
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x0016357C File Offset: 0x0016177C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1637(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x0016358C File Offset: 0x0016178C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1638(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.TesterPresentCommand, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x001635BC File Offset: 0x001617BC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1639(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.TesterPresentCommand = A_1;
				return;
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x001635D8 File Offset: 0x001617D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1640(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x001635E8 File Offset: 0x001617E8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1641(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ATCommandStateOptimization, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00163618 File Offset: 0x00161818
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1642(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ATCommandStateOptimization = A_1;
				return;
			}
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x00163634 File Offset: 0x00161834
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1643(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x00163644 File Offset: 0x00161844
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1644(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ExpectedResponseCountOptimization, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x00163674 File Offset: 0x00161874
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1645(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ExpectedResponseCountOptimization = A_1;
				return;
			}
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x00163690 File Offset: 0x00161890
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1646(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x001636A0 File Offset: 0x001618A0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1647(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ExpectedResponseCountOptimizationAlways1ForKWPMode01, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x001636D0 File Offset: 0x001618D0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1648(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ExpectedResponseCountOptimizationAlways1ForKWPMode01 = A_1;
				return;
			}
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x001636EC File Offset: 0x001618EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1649(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x001636FC File Offset: 0x001618FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1650(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x0016372C File Offset: 0x0016192C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1651(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CANOptimizeRequests = A_1;
				return;
			}
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x00163748 File Offset: 0x00161948
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1652(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x00163758 File Offset: 0x00161958
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1653(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x00163788 File Offset: 0x00161988
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1654(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x00163798 File Offset: 0x00161998
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1655(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.CANOptimizeMaxInRequest, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F3D RID: 7997 RVA: 0x001637C8 File Offset: 0x001619C8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1656(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.CANOptimizeMaxInRequest = A_1;
				return;
			}
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x001637E4 File Offset: 0x001619E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1657(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x001637F4 File Offset: 0x001619F4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1658(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x00163824 File Offset: 0x00161A24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1659(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CANOptimizeMode22 = A_1;
				return;
			}
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x00163840 File Offset: 0x00161A40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1660(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x00163850 File Offset: 0x00161A50
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1661(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x00163880 File Offset: 0x00161A80
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1662(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x00163890 File Offset: 0x00161A90
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1663(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x001638C0 File Offset: 0x00161AC0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1664(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x001638D0 File Offset: 0x00161AD0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1665(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.CANOptimizeMode22MaxInRequest, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x00163900 File Offset: 0x00161B00
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1666(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.CANOptimizeMode22MaxInRequest = A_1;
				return;
			}
		}

		// Token: 0x06001F48 RID: 8008 RVA: 0x0016391C File Offset: 0x00161B1C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1667(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x0016392C File Offset: 0x00161B2C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1668(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeMode22SelfLearningMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x0016395C File Offset: 0x00161B5C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1669(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CANOptimizeMode22SelfLearningMode = A_1;
				return;
			}
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x00163978 File Offset: 0x00161B78
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1670(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x00163988 File Offset: 0x00161B88
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1671(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x001639B8 File Offset: 0x00161BB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1672(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F4E RID: 8014 RVA: 0x001639C8 File Offset: 0x00161BC8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1673(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticReoptimization, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F4F RID: 8015 RVA: 0x001639F8 File Offset: 0x00161BF8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1674(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AutomaticReoptimization = A_1;
				return;
			}
		}

		// Token: 0x06001F50 RID: 8016 RVA: 0x00163A14 File Offset: 0x00161C14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1675(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F51 RID: 8017 RVA: 0x00163A24 File Offset: 0x00161C24
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1676(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F52 RID: 8018 RVA: 0x00163A54 File Offset: 0x00161C54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1677(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F53 RID: 8019 RVA: 0x00163A64 File Offset: 0x00161C64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1678(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANRequestSegmentationSTNLevel, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F54 RID: 8020 RVA: 0x00163A94 File Offset: 0x00161C94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1679(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CANRequestSegmentationSTNLevel = A_1;
				return;
			}
		}

		// Token: 0x06001F55 RID: 8021 RVA: 0x00163AB0 File Offset: 0x00161CB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1680(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F56 RID: 8022 RVA: 0x00163AC0 File Offset: 0x00161CC0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1681(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F57 RID: 8023 RVA: 0x00163AF0 File Offset: 0x00161CF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1682(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F58 RID: 8024 RVA: 0x00163B00 File Offset: 0x00161D00
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1683(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CANOptimizeRequests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F59 RID: 8025 RVA: 0x00163B30 File Offset: 0x00161D30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1684(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F5A RID: 8026 RVA: 0x00163B40 File Offset: 0x00161D40
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1685(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DisableOptimizationIfItFails, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F5B RID: 8027 RVA: 0x00163B70 File Offset: 0x00161D70
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1686(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DisableOptimizationIfItFails = A_1;
				return;
			}
		}

		// Token: 0x06001F5C RID: 8028 RVA: 0x00163B8C File Offset: 0x00161D8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1687(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F5D RID: 8029 RVA: 0x00163B9C File Offset: 0x00161D9C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1688(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ATCRAOptimization, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F5E RID: 8030 RVA: 0x00163BCC File Offset: 0x00161DCC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1689(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ATCRAOptimization = A_1;
				return;
			}
		}

		// Token: 0x06001F5F RID: 8031 RVA: 0x00163BE8 File Offset: 0x00161DE8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1690(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F60 RID: 8032 RVA: 0x00163BF8 File Offset: 0x00161DF8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1691(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AlwaysPingECU, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F61 RID: 8033 RVA: 0x00163C28 File Offset: 0x00161E28
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1692(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AlwaysPingECU = A_1;
				return;
			}
		}

		// Token: 0x06001F62 RID: 8034 RVA: 0x00163C44 File Offset: 0x00161E44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1693(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F63 RID: 8035 RVA: 0x00163C54 File Offset: 0x00161E54
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1694(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DecodeMUT2Compatible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F64 RID: 8036 RVA: 0x00163C84 File Offset: 0x00161E84
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1695(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DecodeMUT2Compatible = A_1;
				return;
			}
		}

		// Token: 0x06001F65 RID: 8037 RVA: 0x00163CA0 File Offset: 0x00161EA0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1696(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F66 RID: 8038 RVA: 0x00163CB0 File Offset: 0x00161EB0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1697(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DaihatsuKLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F67 RID: 8039 RVA: 0x00163CE0 File Offset: 0x00161EE0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1698(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DaihatsuKLine = A_1;
				return;
			}
		}

		// Token: 0x06001F68 RID: 8040 RVA: 0x00163CFC File Offset: 0x00161EFC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1699(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F69 RID: 8041 RVA: 0x00163D0C File Offset: 0x00161F0C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1700(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.KWPConcatResponseLines, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F6A RID: 8042 RVA: 0x00163D3C File Offset: 0x00161F3C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1701(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.KWPConcatResponseLines = A_1;
				return;
			}
		}

		// Token: 0x06001F6B RID: 8043 RVA: 0x00163D58 File Offset: 0x00161F58
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1702(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F6C RID: 8044 RVA: 0x00163D68 File Offset: 0x00161F68
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1703(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ForceUseManualFlowControlForCodingOperations, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F6D RID: 8045 RVA: 0x00163D98 File Offset: 0x00161F98
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1704(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ForceUseManualFlowControlForCodingOperations = A_1;
				return;
			}
		}

		// Token: 0x06001F6E RID: 8046 RVA: 0x00163DB4 File Offset: 0x00161FB4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1705(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F6F RID: 8047 RVA: 0x00163DC4 File Offset: 0x00161FC4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1706(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ForceUseManualFlowControlWhileReadingData, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F70 RID: 8048 RVA: 0x00163DF4 File Offset: 0x00161FF4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1707(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ForceUseManualFlowControlWhileReadingData = A_1;
				return;
			}
		}

		// Token: 0x06001F71 RID: 8049 RVA: 0x00163E10 File Offset: 0x00162010
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1708(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F72 RID: 8050 RVA: 0x00163E20 File Offset: 0x00162020
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1709(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SendTesterPresentWhileLongUploadTimeVag5f, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F73 RID: 8051 RVA: 0x00163E50 File Offset: 0x00162050
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1710(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SendTesterPresentWhileLongUploadTimeVag5f = A_1;
				return;
			}
		}

		// Token: 0x06001F74 RID: 8052 RVA: 0x00163E6C File Offset: 0x0016206C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1711(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F75 RID: 8053 RVA: 0x00163E7C File Offset: 0x0016207C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1712(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ReplaceATTAWithATCER, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F76 RID: 8054 RVA: 0x00163EAC File Offset: 0x001620AC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1713(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ReplaceATTAWithATCER = A_1;
				return;
			}
		}

		// Token: 0x06001F77 RID: 8055 RVA: 0x00163EC8 File Offset: 0x001620C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1714(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F78 RID: 8056 RVA: 0x00163ED8 File Offset: 0x001620D8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1715(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseServiceResponseForPositiveResponseMarker, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F79 RID: 8057 RVA: 0x00163F08 File Offset: 0x00162108
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1716(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseServiceResponseForPositiveResponseMarker = A_1;
				return;
			}
		}

		// Token: 0x06001F7A RID: 8058 RVA: 0x00163F24 File Offset: 0x00162124
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1717(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F7B RID: 8059 RVA: 0x00163F34 File Offset: 0x00162134
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1718(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.KWPConcatResponseLines, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F7C RID: 8060 RVA: 0x00163F64 File Offset: 0x00162164
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1719(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.KWPConcatResponseLines = A_1;
				return;
			}
		}

		// Token: 0x06001F7D RID: 8061 RVA: 0x00163F80 File Offset: 0x00162180
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1720(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x00163F90 File Offset: 0x00162190
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1721(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AddNissanConsult3Pids, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x00163FC0 File Offset: 0x001621C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1722(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AddNissanConsult3Pids = A_1;
				return;
			}
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x00163FDC File Offset: 0x001621DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1723(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x00163FEC File Offset: 0x001621EC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1724(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.NissanConsult3OpenCloseSession, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x0016401C File Offset: 0x0016221C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1725(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.NissanConsult3OpenCloseSession = A_1;
				return;
			}
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x00164038 File Offset: 0x00162238
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1726(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x00164048 File Offset: 0x00162248
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1727(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IgnoreCodingErrors, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F85 RID: 8069 RVA: 0x00164078 File Offset: 0x00162278
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1728(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.IgnoreCodingErrors = A_1;
				return;
			}
		}

		// Token: 0x06001F86 RID: 8070 RVA: 0x00164094 File Offset: 0x00162294
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1729(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F87 RID: 8071 RVA: 0x001640A4 File Offset: 0x001622A4
		[CompilerGenerated]
		private static ValueTuple<FlowControlOverrides, bool> <InitializeComponent>typedBindingsM__1730(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FlowControlOverrides, bool>(A_0.FlowControlOverrideMode, true);
			}
			return default(ValueTuple<FlowControlOverrides, bool>);
		}

		// Token: 0x06001F88 RID: 8072 RVA: 0x001640D4 File Offset: 0x001622D4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1731(SharedSettings A_0, FlowControlOverrides A_1)
		{
			if (A_0 != null)
			{
				A_0.FlowControlOverrideMode = A_1;
				return;
			}
		}

		// Token: 0x06001F89 RID: 8073 RVA: 0x001640F0 File Offset: 0x001622F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1732(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x00164100 File Offset: 0x00162300
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1733(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PerformSensorsScanByTesting, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x00164130 File Offset: 0x00162330
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1734(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PerformSensorsScanByTesting = A_1;
				return;
			}
		}

		// Token: 0x06001F8C RID: 8076 RVA: 0x0016414C File Offset: 0x0016234C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1735(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F8D RID: 8077 RVA: 0x0016415C File Offset: 0x0016235C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1736(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x0016418C File Offset: 0x0016238C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1737(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x0016419C File Offset: 0x0016239C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1738(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.RequestECUInfo, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F90 RID: 8080 RVA: 0x001641CC File Offset: 0x001623CC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1739(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.RequestECUInfo = A_1;
				return;
			}
		}

		// Token: 0x06001F91 RID: 8081 RVA: 0x001641E8 File Offset: 0x001623E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1740(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x001641F8 File Offset: 0x001623F8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1741(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Mode01Prefix, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x00164228 File Offset: 0x00162428
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1742(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Mode01Prefix = A_1;
				return;
			}
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00164244 File Offset: 0x00162444
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1743(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x00164254 File Offset: 0x00162454
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1744(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowBadELMWarning, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x00164284 File Offset: 0x00162484
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1745(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowBadELMWarning = A_1;
				return;
			}
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x001642A0 File Offset: 0x001624A0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1746(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x001642B0 File Offset: 0x001624B0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1747(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.IOTimeout, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x001642E0 File Offset: 0x001624E0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1748(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.IOTimeout = A_1;
				return;
			}
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x001642FC File Offset: 0x001624FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1749(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x0016430C File Offset: 0x0016250C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1750(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SendATZATE, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x0016433C File Offset: 0x0016253C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1751(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SendATZATE = A_1;
				return;
			}
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x00164358 File Offset: 0x00162558
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1752(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x00164368 File Offset: 0x00162568
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1753(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.NoDelayELM327Init, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x00164398 File Offset: 0x00162598
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1754(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.NoDelayELM327Init = A_1;
				return;
			}
		}

		// Token: 0x06001FA0 RID: 8096 RVA: 0x001643B4 File Offset: 0x001625B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1755(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FA1 RID: 8097 RVA: 0x001643C4 File Offset: 0x001625C4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1756(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ELM327ConnectionAttempts, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FA2 RID: 8098 RVA: 0x001643F4 File Offset: 0x001625F4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1757(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ELM327ConnectionAttempts = A_1;
				return;
			}
		}

		// Token: 0x06001FA3 RID: 8099 RVA: 0x00164410 File Offset: 0x00162610
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1758(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FA4 RID: 8100 RVA: 0x00164420 File Offset: 0x00162620
		[CompilerGenerated]
		private static ValueTuple<ReadPartialErrorActions, bool> <InitializeComponent>typedBindingsM__1759(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ReadPartialErrorActions, bool>(A_0.ReadPartialErrorAction, true);
			}
			return default(ValueTuple<ReadPartialErrorActions, bool>);
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x00164450 File Offset: 0x00162650
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1760(SharedSettings A_0, ReadPartialErrorActions A_1)
		{
			if (A_0 != null)
			{
				A_0.ReadPartialErrorAction = A_1;
				return;
			}
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x0016446C File Offset: 0x0016266C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1761(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x0016447C File Offset: 0x0016267C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1762(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.NoDataLimit, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x001644AC File Offset: 0x001626AC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1763(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.NoDataLimit = A_1;
				return;
			}
		}

		// Token: 0x06001FA9 RID: 8105 RVA: 0x001644C8 File Offset: 0x001626C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1764(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x001644D8 File Offset: 0x001626D8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1765(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x00164508 File Offset: 0x00162708
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1766(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x00164524 File Offset: 0x00162724
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1767(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FAD RID: 8109 RVA: 0x00164534 File Offset: 0x00162734
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1768(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.SendDelay, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FAE RID: 8110 RVA: 0x00164564 File Offset: 0x00162764
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1769(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.SendDelay = A_1;
				return;
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x00164580 File Offset: 0x00162780
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1770(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x00164590 File Offset: 0x00162790
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1771(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x001645C0 File Offset: 0x001627C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1772(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06001FB2 RID: 8114 RVA: 0x001645DC File Offset: 0x001627DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1773(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FB3 RID: 8115 RVA: 0x001645EC File Offset: 0x001627EC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1774(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FB4 RID: 8116 RVA: 0x0016461C File Offset: 0x0016281C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1775(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x00164638 File Offset: 0x00162838
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1776(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FB6 RID: 8118 RVA: 0x00164648 File Offset: 0x00162848
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1777(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DatasetUploadMaxBlockSize, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FB7 RID: 8119 RVA: 0x00164678 File Offset: 0x00162878
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1778(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.DatasetUploadMaxBlockSize = A_1;
				return;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x00164694 File Offset: 0x00162894
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1779(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x001646A4 File Offset: 0x001628A4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1780(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x001646D4 File Offset: 0x001628D4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1781(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06001FBB RID: 8123 RVA: 0x001646F0 File Offset: 0x001628F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1782(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x00164700 File Offset: 0x00162900
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1783(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00164730 File Offset: 0x00162930
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1784(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x0016474C File Offset: 0x0016294C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1785(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FBF RID: 8127 RVA: 0x0016475C File Offset: 0x0016295C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1786(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.SendTesterPresentWhileLongUploadTimeMs, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FC0 RID: 8128 RVA: 0x0016478C File Offset: 0x0016298C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1787(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.SendTesterPresentWhileLongUploadTimeMs = A_1;
				return;
			}
		}

		// Token: 0x06001FC1 RID: 8129 RVA: 0x001647A8 File Offset: 0x001629A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1788(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FC2 RID: 8130 RVA: 0x001647B8 File Offset: 0x001629B8
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1789(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x001647E8 File Offset: 0x001629E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1790(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x001647F8 File Offset: 0x001629F8
		[CompilerGenerated]
		private static ValueTuple<AndroidWiFiConnectionModes, bool> <InitializeComponent>typedBindingsM__1791(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<AndroidWiFiConnectionModes, bool>(A_0.AndroidWiFiMode, true);
			}
			return default(ValueTuple<AndroidWiFiConnectionModes, bool>);
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x00164828 File Offset: 0x00162A28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1792(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x00164838 File Offset: 0x00162A38
		[CompilerGenerated]
		private static ValueTuple<AndroidWiFiConnectionModes, bool> <InitializeComponent>typedBindingsM__1793(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<AndroidWiFiConnectionModes, bool>(A_0.AndroidWiFiMode, true);
			}
			return default(ValueTuple<AndroidWiFiConnectionModes, bool>);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x00164868 File Offset: 0x00162A68
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1794(SharedSettings A_0, AndroidWiFiConnectionModes A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidWiFiMode = A_1;
				return;
			}
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x00164884 File Offset: 0x00162A84
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1795(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x00164894 File Offset: 0x00162A94
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1796(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x001648C4 File Offset: 0x00162AC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1797(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x001648D4 File Offset: 0x00162AD4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1798(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.AndroidBluetoothConnectionMethod, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x00164904 File Offset: 0x00162B04
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1799(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidBluetoothConnectionMethod = A_1;
				return;
			}
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x00164920 File Offset: 0x00162B20
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1800(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x00164930 File Offset: 0x00162B30
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1801(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.AndroidBluetoothConnectionMethod, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x00164960 File Offset: 0x00162B60
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1802(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidBluetoothConnectionMethod = A_1;
				return;
			}
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x0016497C File Offset: 0x00162B7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1803(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0016498C File Offset: 0x00162B8C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1804(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DroidAskForBluetoothPermissions, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x001649BC File Offset: 0x00162BBC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1805(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DroidAskForBluetoothPermissions = A_1;
				return;
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x001649D8 File Offset: 0x00162BD8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1806(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x001649E8 File Offset: 0x00162BE8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1807(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DroidTryTurnOnBluetooth, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x00164A18 File Offset: 0x00162C18
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1808(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DroidTryTurnOnBluetooth = A_1;
				return;
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00164A34 File Offset: 0x00162C34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1809(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00164A44 File Offset: 0x00162C44
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1810(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.CheckBluetoothTurnedOn, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x00164A74 File Offset: 0x00162C74
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1811(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.CheckBluetoothTurnedOn = A_1;
				return;
			}
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x00164A90 File Offset: 0x00162C90
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1812(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00164AA0 File Offset: 0x00162CA0
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1813(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x00164AD0 File Offset: 0x00162CD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1814(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x00164AE0 File Offset: 0x00162CE0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1815(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.BTLEShowDevicesWithoutName, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x00164B10 File Offset: 0x00162D10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1816(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.BTLEShowDevicesWithoutName = A_1;
				return;
			}
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x00164B2C File Offset: 0x00162D2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1817(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FDF RID: 8159 RVA: 0x00164B3C File Offset: 0x00162D3C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1818(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.VWTP_ChannelSetupATST, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x00164B6C File Offset: 0x00162D6C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1819(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.VWTP_ChannelSetupATST = A_1;
				return;
			}
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x00164B88 File Offset: 0x00162D88
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1820(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00164B98 File Offset: 0x00162D98
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1821(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VWTP_ActiveConnectionTestTimeout, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x00164BC8 File Offset: 0x00162DC8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1822(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VWTP_ActiveConnectionTestTimeout = A_1;
				return;
			}
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x00164BE4 File Offset: 0x00162DE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1823(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x00164BF4 File Offset: 0x00162DF4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1824(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VWTP_SendConnectionConfirmationPeriod, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x00164C24 File Offset: 0x00162E24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1825(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VWTP_SendConnectionConfirmationPeriod = A_1;
				return;
			}
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00164C40 File Offset: 0x00162E40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1826(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000F4A RID: 3914
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000F4B RID: 3915
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section DefaultConnectionPanel;

		// Token: 0x04000F4C RID: 3916
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnForceMode22Learning;

		// Token: 0x04000F4D RID: 3917
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnClearOptimizationData;

		// Token: 0x04000F4E RID: 3918
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section wifiPanel;

		// Token: 0x04000F4F RID: 3919
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetooth2Panel;

		// Token: 0x04000F50 RID: 3920
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetoothLEPanel;

		// Token: 0x0200028B RID: 651
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnForceMode22Learning_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x06001FE8 RID: 8168 RVA: 0x00164C50 File Offset: 0x00162E50
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionExpertV3 settingsConnectionExpertV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
							{
								settingsConnectionExpertV.btnForceMode22Learning.IsEnabled = false;
								taskAwaiter = OBDRequestQueueOptimizer.ForceLearnOptimizationDictionary().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionExpertV3.<btnForceMode22Learning_Clicked>d__3>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0084;
							}
							else
							{
								taskAwaiter = settingsConnectionExpertV.DisplayAlert("Please connect to your car ECU first!", "Connection required!", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionExpertV3.<btnForceMode22Learning_Clicked>d__3>(ref taskAwaiter, ref this);
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
						goto IL_0100;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_0084:
					taskAwaiter.GetResult();
					settingsConnectionExpertV.btnForceMode22Learning.IsEnabled = true;
					IL_0100:;
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

			// Token: 0x06001FE9 RID: 8169 RVA: 0x00164D9C File Offset: 0x00162F9C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F51 RID: 3921
			public int <>1__state;

			// Token: 0x04000F52 RID: 3922
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F53 RID: 3923
			public SettingsConnectionExpertV3 <>4__this;

			// Token: 0x04000F54 RID: 3924
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200028C RID: 652
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <cellInitStringEditor_Tapped>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001FEA RID: 8170 RVA: 0x00164DAC File Offset: 0x00162FAC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionExpertV3 settingsConnectionExpertV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = settingsConnectionExpertV.Navigation.PushAsync(new InitSequenceEditorPage()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionExpertV3.<cellInitStringEditor_Tapped>d__5>(ref taskAwaiter, ref this);
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
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001FEB RID: 8171 RVA: 0x00164E68 File Offset: 0x00163068
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F55 RID: 3925
			public int <>1__state;

			// Token: 0x04000F56 RID: 3926
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F57 RID: 3927
			public SettingsConnectionExpertV3 <>4__this;

			// Token: 0x04000F58 RID: 3928
			private TaskAwaiter <>u__1;
		}
	}
}
