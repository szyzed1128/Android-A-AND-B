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
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x0200028D RID: 653
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml")]
	public class SettingsConnectionPageV3 : ContentPage
	{
		// Token: 0x06001FEC RID: 8172 RVA: 0x00164E78 File Offset: 0x00163078
		public SettingsConnectionPageV3()
		{
			try
			{
				this.InitializeComponent();
				base.BindingContext = SharedSettings.Current;
				base.Appearing += this.SettingsConnectionExpertV3_Appearing;
				base.Disappearing += this.SettingsConnectionExpertV3_Disappearing;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x00164ED8 File Offset: 0x001630D8
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

		// Token: 0x06001FEE RID: 8174 RVA: 0x00164F04 File Offset: 0x00163104
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

		// Token: 0x06001FEF RID: 8175 RVA: 0x00164F3C File Offset: 0x0016313C
		private async void btnSelectDevice_Clicked(object sender, EventArgs e)
		{
			if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE)
			{
				if (PlatformHelper.IsiOS)
				{
					try
					{
						if (PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0) && PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
						{
							await base.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK");
							PlatformHelper.CommonService.OpenPermissionsSettings();
							return;
						}
					}
					catch (Exception)
					{
					}
				}
				await base.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true);
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
			{
				await base.Navigation.PushAsync(new BTDeviceSelectorPage(), true);
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus)
			{
				await base.Navigation.PushAsync(new BTDeviceSelectorPage(), true);
			}
		}

		// Token: 0x06001FF0 RID: 8176 RVA: 0x00164F74 File Offset: 0x00163174
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

		// Token: 0x06001FF1 RID: 8177 RVA: 0x000E2964 File Offset: 0x000E0B64
		private void BtnDroidBackgroundTrobleshooting_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				if (PlatformHelper.DroidService.PowerHacks_CanShowPowerManagerActivity())
				{
					PlatformHelper.DroidService.PowerHacks_LaunchPowerManagerActivity();
					return;
				}
				Device.OpenUri(new Uri("https://dontkillmyapp.com?app=Car Scanner ELM OBD2"));
			}
		}

		// Token: 0x06001FF2 RID: 8178 RVA: 0x00164FC4 File Offset: 0x001631C4
		private async void btnExportLog_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				try
				{
					string text2 = await SharedSettings.Current.GetSettingsReport();
					string text = text2;
					await App.OBDReader.DebugWrite("\r\n[///**** CONTACT DEVELOPER REPORT:");
					await App.OBDReader.DebugWrite(text);
					await App.OBDReader.DebugWrite("\r\n[***/// END OF CONTACT DEVELOPER REPORT]");
					text = null;
				}
				catch (Exception)
				{
				}
				try
				{
					PCLDebugStream.CurrentInstance.Close();
				}
				catch (Exception)
				{
				}
				try
				{
					string text = PCLDebugStream.GetFilepath();
					if (PlatformHelper.IsiOS)
					{
						await Share.RequestAsync(new ShareFileRequest(new ShareFile(text)));
					}
					if (PlatformHelper.IsAndroid)
					{
						await Share.RequestAsync(new ShareFileRequest(new ShareFile(text)));
					}
					text = null;
					return;
				}
				catch (Exception ex)
				{
					base.DisplayAlert("Error", ex.Message, "OK");
					return;
				}
			}
			base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK");
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x00164FFC File Offset: 0x001631FC
		private void btnAdvancedConnection_Clicked(object sender, EventArgs e)
		{
			SettingsConnectionExpertV3 settingsConnectionExpertV = new SettingsConnectionExpertV3();
			base.Navigation.PushAsync(settingsConnectionExpertV);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x0016501C File Offset: 0x0016321C
		private void btnConnectionGuide_Clicked(object sender, EventArgs e)
		{
			switch (SharedSettings.Current.ConnectionType)
			{
			case ConnectionTypes.WiFi:
				Device.OpenUri(new Uri("https://www.carscanner.info/wifi/"));
				return;
			case ConnectionTypes.BluetoothLE:
				Device.OpenUri(new Uri("https://www.carscanner.info/ios-bt4/"));
				return;
			case ConnectionTypes.Bluetooth:
				Device.OpenUri(new Uri("https://www.carscanner.info/android-bluetooth/"));
				return;
			default:
				return;
			}
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x00165076 File Offset: 0x00163276
		private void CellWiFiTroubleshooting_Tapped(object sender, EventArgs e)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/ios-wifi-troubleshooting/");
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x00165083 File Offset: 0x00163283
		private void btnRecomendations_Tapped(object sender, EventArgs e)
		{
			if (App.CurrentLanguageCode == "ru" || App.CurrentLanguageCode == "RU")
			{
				Launcher.TryOpenAsync("https://www.carscanner.info/ru/choosing-obdii-adapter/");
				return;
			}
			Launcher.TryOpenAsync("https://www.carscanner.info/choosing-obdii-adapter/");
		}

		// Token: 0x06001FF7 RID: 8183 RVA: 0x001650BE File Offset: 0x001632BE
		private void ELMErrors_Tapped(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new ELMFailPage());
		}

		// Token: 0x06001FF8 RID: 8184 RVA: 0x001650D4 File Offset: 0x001632D4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsConnectionPageV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			ConnectionTypeToWiFiVisibleBoolConverter connectionTypeToWiFiVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverter = new ConnectionTypeToWiFiVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ConnectionTypeToBTLEVisibleBoolConverter connectionTypeToBTLEVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTLEVisibleBoolConverter = new ConnectionTypeToBTLEVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ConnectionTypeToBTVisibleBoolConverter connectionTypeToBTVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverter = new ConnectionTypeToBTVisibleBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ConnectionTypeToMFIBluetoothConverter connectionTypeToMFIBluetoothConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToMFIBluetoothConverter = new ConnectionTypeToMFIBluetoothConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ConnectionTypeToIntConverter connectionTypeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToIntConverter = new ConnectionTypeToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			EmptyConverterParameterToNotSelectedStringConverter emptyConverterParameterToNotSelectedStringConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyConverterParameterToNotSelectedStringConverter = new EmptyConverterParameterToNotSelectedStringConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			EmptyStringToConvertererParameterConverter emptyStringToConvertererParameterConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToConvertererParameterConverter = new EmptyStringToConvertererParameterConverter(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			ConnectionTypes connectionTypes = ConnectionTypes.WiFi;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			ConnectionTypes connectionTypes2 = ConnectionTypes.Bluetooth;
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 30);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 30);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 26);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 18);
			ConnectionTypes connectionTypes3 = ConnectionTypes.BluetoothLE;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			ConnectionTypes connectionTypes4 = ConnectionTypes.MFI_OBDLinkMXPlus;
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 26);
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 21);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 21);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 21);
			EntryCell entryCell;
			VisualDiagnostics.RegisterSourceInfo(entryCell = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 21);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 21);
			EntryCell entryCell2;
			VisualDiagnostics.RegisterSourceInfo(entryCell2 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 49);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 107);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 49);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 112);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 21);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 26);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 17);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 25);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 25);
			OnPlatform<string> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<string>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 22);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 21);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 21);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 39);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 30);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 26);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 49);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 116);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 14);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 17);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 17);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 17);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 25);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 25);
			OnPlatform<string> onPlatform5;
			VisualDiagnostics.RegisterSourceInfo(onPlatform5 = new OnPlatform<string>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 22);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 21);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 21);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 21);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 21);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 39);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 30);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 30);
			MultiBinding multiBinding2;
			VisualDiagnostics.RegisterSourceInfo(multiBinding2 = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 26);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 14);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 17);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 17);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 17);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 17);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 21);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 21);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 21);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 39);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 30);
			MultiBinding multiBinding3;
			VisualDiagnostics.RegisterSourceInfo(multiBinding3 = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 26);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 14);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 22);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 18);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 21);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 14);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 25);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 21);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 14);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 25);
			On on5;
			VisualDiagnostics.RegisterSourceInfo(on5 = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 26);
			On on6;
			VisualDiagnostics.RegisterSourceInfo(on6 = new On(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 26);
			OnPlatform<bool> onPlatform6;
			VisualDiagnostics.RegisterSourceInfo(onPlatform6 = new OnPlatform<bool>(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 22);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 49);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 109);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 18);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 21);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 18);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 21);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 21);
			ButtonCell buttonCell5;
			VisualDiagnostics.RegisterSourceInfo(buttonCell5 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 14);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 25);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 21);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 21);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 21);
			EntryCell entryCell3;
			VisualDiagnostics.RegisterSourceInfo(entryCell3 = new EntryCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 14);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 25);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 49);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 120);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 18);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 21);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 21);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 14);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 21);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 21);
			ButtonCell buttonCell6;
			VisualDiagnostics.RegisterSourceInfo(buttonCell6 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 18);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 21);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 21);
			ButtonCell buttonCell7;
			VisualDiagnostics.RegisterSourceInfo(buttonCell7 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 18);
			Section section11;
			VisualDiagnostics.RegisterSourceInfo(section11 = new Section(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsConnectionPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("connectionTypeSection", section);
			if (section.StyleId == null)
			{
				section.StyleId = "connectionTypeSection";
			}
			nameScope.RegisterName("cellWiFi", radioCell);
			if (radioCell.StyleId == null)
			{
				radioCell.StyleId = "cellWiFi";
			}
			nameScope.RegisterName("cellBT", radioCell2);
			if (radioCell2.StyleId == null)
			{
				radioCell2.StyleId = "cellBT";
			}
			nameScope.RegisterName("wifiPanel", section2);
			if (section2.StyleId == null)
			{
				section2.StyleId = "wifiPanel";
			}
			nameScope.RegisterName("bluetoothLEPanel", section3);
			if (section3.StyleId == null)
			{
				section3.StyleId = "bluetoothLEPanel";
			}
			nameScope.RegisterName("bluetooth2Panel", section4);
			if (section4.StyleId == null)
			{
				section4.StyleId = "bluetooth2Panel";
			}
			nameScope.RegisterName("bluetoothMFIPanel", section5);
			if (section5.StyleId == null)
			{
				section5.StyleId = "bluetoothMFIPanel";
			}
			this.settingsLayoutRoot = settingsView;
			this.connectionTypeSection = section;
			this.cellWiFi = radioCell;
			this.cellBT = radioCell2;
			this.wifiPanel = section2;
			this.bluetoothLEPanel = section3;
			this.bluetooth2Panel = section4;
			this.bluetoothMFIPanel = section5;
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
			resourceDictionary.Add("EmptyConverterParameterToNotSelectedStringConverter", emptyConverterParameterToNotSelectedStringConverter);
			resourceDictionary.Add("EmptyStringToConvertererParameterConverter", emptyStringToConvertererParameterConverter);
			translate.Text = "Settings_Adapter";
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
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
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
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbConnectionType.Text";
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
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			bindingExtension.Path = "ConnectionType";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
				}
				return default(ValueTuple<ConnectionTypes, bool>);
			}, delegate(SharedSettings A_0, ConnectionTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ConnectionType = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectionType")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			radioCell.SetValue(CellBase.TitleProperty, "Wi-Fi");
			radioCell.SetValue(RadioCell.ValueProperty, connectionTypes);
			section.Add(radioCell);
			radioCell2.SetValue(CellBase.TitleProperty, "Bluetooth");
			radioCell2.SetValue(RadioCell.ValueProperty, connectionTypes2);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform.Platforms.Add(on2);
			radioCell2.SetValue(CellBase.IsVisibleProperty, onPlatform);
			section.Add(radioCell2);
			radioCell3.SetValue(CellBase.TitleProperty, "Bluetooth LE (4.0+)");
			radioCell3.SetValue(RadioCell.ValueProperty, connectionTypes3);
			section.Add(radioCell3);
			radioCell4.SetValue(CellBase.TitleProperty, "Bluetooth MFi");
			radioCell4.SetValue(CellBase.DescriptionProperty, "OBDLink MX+, vLinker FS, vLinker MS");
			radioCell4.SetValue(RadioCell.ValueProperty, connectionTypes4);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "True";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "False";
			onPlatform2.Platforms.Add(on4);
			radioCell4.SetValue(CellBase.IsVisibleProperty, onPlatform2);
			section.Add(radioCell4);
			settingsView.Root.Add(section);
			section2.SetValue(SectionBase.TitleProperty, "Wi-Fi");
			translate3.Text = "ios_DefaultWiFi_Servers";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = section2;
			array4[1] = settingsView;
			array4[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Section.FooterTextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 17)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			section2.FooterText = obj7;
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = bindingExtension2;
			array5[1] = section2;
			array5[2] = settingsView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 17)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension2.Converter = obj9;
			bindingExtension2.Path = "ConnectionType";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase2);
			translate4.Text = "Settings_Control_tbWiFiServer.Text";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = entryCell;
			array6[1] = section2;
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
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			entryCell.Title = obj11;
			entryCell.SetValue(EntryCell.PlaceholderProperty, "192.168.0.10");
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "WiFiServer";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.WiFiServer, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.WiFiServer = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "WiFiServer")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			entryCell.SetBinding(EntryCell.ValueTextProperty, bindingBase3);
			staticResourceExtension2.Key = "BaseFontSize++";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = entryCell;
			array7[1] = section2;
			array7[2] = settingsView;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, EntryCell.ValueTextFontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entryCell.ValueTextFontSize = (double)obj13;
			section2.Add(entryCell);
			translate5.Text = "Settings_Control_tbWiFiPort.Text";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = entryCell2;
			array8[1] = section2;
			array8[2] = settingsView;
			array8[3] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, CellBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 21)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			entryCell2.Title = obj15;
			entryCell2.SetValue(EntryCell.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			entryCell2.SetValue(EntryCell.MaxLengthProperty, 5);
			entryCell2.SetValue(EntryCell.PlaceholderProperty, "35000");
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "WiFiPort";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.WiFiPort, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.WiFiPort = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "WiFiPort")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entryCell2.SetBinding(EntryCell.ValueTextProperty, bindingBase4);
			section2.Add(entryCell2);
			translate6.Text = "ios_Settings_NoWiFiWarning";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = settingsCheckBoxCellPatched;
			array9[1] = section2;
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
			xmlNamespaceResolver9.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 49)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			settingsCheckBoxCellPatched.Title = obj17;
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "NoWiFiWarning";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.NoWiFiWarning, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.NoWiFiWarning = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "NoWiFiWarning")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase5);
			section2.Add(settingsCheckBoxCellPatched);
			translate7.Text = "ios_TryConnectToLastWiFiNetwork";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = settingsCheckBoxCellPatched2;
			array10[1] = section2;
			array10[2] = settingsView;
			array10[3] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, CellBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 49)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			settingsCheckBoxCellPatched2.Title = obj19;
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "TryConnectToLastWiFiNetwork";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.TryConnectToLastWiFiNetwork, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.TryConnectToLastWiFiNetwork = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "TryConnectToLastWiFiNetwork")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase6);
			section2.Add(settingsCheckBoxCellPatched2);
			translate8.Text = "ios_WiFiTroubleshooting";
			IMarkupExtension markupExtension11 = translate8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = buttonCell;
			array11[1] = section2;
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
			xmlNamespaceResolver11.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 21)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			buttonCell.Title = obj21;
			buttonCell.Tapped += this.CellWiFiTroubleshooting_Tapped;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = buttonCell;
			array12[1] = section2;
			array12[2] = settingsView;
			array12[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 21)));
			DynamicResource dynamicResource2 = markupExtension12.ProvideValue(xamlServiceProvider12);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			onPlatform3.Default = false;
			onPlatform3.iOS = true;
			buttonCell.SetValue(CellBase.IsVisibleProperty, onPlatform3);
			section2.Add(buttonCell);
			settingsView.Root.Add(section2);
			translate9.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension13 = translate9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = section3;
			array13[1] = settingsView;
			array13[2] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 17)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			section3.Title = obj24;
			bindingExtension7.Mode = 2;
			staticResourceExtension3.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = bindingExtension7;
			array14[1] = section3;
			array14[2] = settingsView;
			array14[3] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 17)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension7.Converter = obj26;
			bindingExtension7.Path = "ConnectionType";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase7);
			translate10.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension15 = translate10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = onPlatform4;
			array15[1] = section3;
			array15[2] = settingsView;
			array15[3] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 25)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			onPlatform4.Android = obj28;
			translate11.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension16 = translate11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = onPlatform4;
			array16[1] = section3;
			array16[2] = settingsView;
			array16[3] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array16, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 25)));
			object obj30 = markupExtension16.ProvideValue(xamlServiceProvider16);
			onPlatform4.iOS = obj30;
			section3.SetValue(Section.FooterTextProperty, onPlatform4);
			translate12.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension17 = translate12;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = labelCell;
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
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 21)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			labelCell.Title = obj32;
			translate13.Text = "settings_TapToSelectConnectionProfile";
			IMarkupExtension markupExtension18 = translate13;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = labelCell;
			array18[1] = section3;
			array18[2] = settingsView;
			array18[3] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle35, obj33 = new SimpleValueTargetProvider(array18, CellBase.DescriptionProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(110, 21)));
			object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
			labelCell.Description = obj34;
			labelCell.SetValue(CellBase.DescriptionFontAttributesProperty, new FontAttributes?(2));
			labelCell.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = labelCell;
			array19[1] = section3;
			array19[2] = settingsView;
			array19[3] = this;
			object obj35;
			xamlServiceProvider19.Add(typeFromHandle37, obj35 = new SimpleValueTargetProvider(array19, LabelCell.ValueTextColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 21)));
			DynamicResource dynamicResource3 = markupExtension19.ProvideValue(xamlServiceProvider19);
			labelCell.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource3.Key);
			staticResourceExtension4.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = multiBinding;
			array20[1] = labelCell;
			array20[2] = section3;
			array20[3] = settingsView;
			array20[4] = this;
			object obj36;
			xamlServiceProvider20.Add(typeFromHandle39, obj36 = new SimpleValueTargetProvider(array20, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 39)));
			object obj37 = markupExtension20.ProvideValue(xamlServiceProvider20);
			multiBinding.Converter = obj37;
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "BTLEDeviceName";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase8);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "BTLEDeviceID";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase9);
			labelCell.SetBinding(LabelCell.ValueTextProperty, multiBinding);
			section3.Add(labelCell);
			translate14.Text = "ios_SearchForBTLEIfConnectionFailed";
			IMarkupExtension markupExtension21 = translate14;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = settingsCheckBoxCellPatched3;
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
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 49)));
			object obj39 = markupExtension21.ProvideValue(xamlServiceProvider21);
			settingsCheckBoxCellPatched3.Title = obj39;
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "SearchForBTLEIfConnectionFailed";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SearchForBTLEIfConnectionFailed, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SearchForBTLEIfConnectionFailed = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SearchForBTLEIfConnectionFailed")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase10);
			section3.Add(settingsCheckBoxCellPatched3);
			settingsView.Root.Add(section3);
			translate15.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension22 = translate15;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 3];
			array22[0] = section4;
			array22[1] = settingsView;
			array22[2] = this;
			object obj40;
			xamlServiceProvider22.Add(typeFromHandle43, obj40 = new SimpleValueTargetProvider(array22, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 17)));
			object obj41 = markupExtension22.ProvideValue(xamlServiceProvider22);
			section4.Title = obj41;
			bindingExtension11.Mode = 2;
			staticResourceExtension5.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension23 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = bindingExtension11;
			array23[1] = section4;
			array23[2] = settingsView;
			array23[3] = this;
			object obj42;
			xamlServiceProvider23.Add(typeFromHandle45, obj42 = new SimpleValueTargetProvider(array23, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 17)));
			object obj43 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension11.Converter = obj43;
			bindingExtension11.Path = "ConnectionType";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase11);
			translate16.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension24 = translate16;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = onPlatform5;
			array24[1] = section4;
			array24[2] = settingsView;
			array24[3] = this;
			object obj44;
			xamlServiceProvider24.Add(typeFromHandle47, obj44 = new SimpleValueTargetProvider(array24, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 25)));
			object obj45 = markupExtension24.ProvideValue(xamlServiceProvider24);
			onPlatform5.Android = obj45;
			translate17.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension25 = translate17;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = onPlatform5;
			array25[1] = section4;
			array25[2] = settingsView;
			array25[3] = this;
			object obj46;
			xamlServiceProvider25.Add(typeFromHandle49, obj46 = new SimpleValueTargetProvider(array25, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 25)));
			object obj47 = markupExtension25.ProvideValue(xamlServiceProvider25);
			onPlatform5.iOS = obj47;
			section4.SetValue(Section.FooterTextProperty, onPlatform5);
			translate18.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension26 = translate18;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = labelCell2;
			array26[1] = section4;
			array26[2] = settingsView;
			array26[3] = this;
			object obj48;
			xamlServiceProvider26.Add(typeFromHandle51, obj48 = new SimpleValueTargetProvider(array26, CellBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 21)));
			object obj49 = markupExtension26.ProvideValue(xamlServiceProvider26);
			labelCell2.Title = obj49;
			translate19.Text = "settings_TapToSelectConnectionProfile";
			IMarkupExtension markupExtension27 = translate19;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = labelCell2;
			array27[1] = section4;
			array27[2] = settingsView;
			array27[3] = this;
			object obj50;
			xamlServiceProvider27.Add(typeFromHandle53, obj50 = new SimpleValueTargetProvider(array27, CellBase.DescriptionProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 21)));
			object obj51 = markupExtension27.ProvideValue(xamlServiceProvider27);
			labelCell2.Description = obj51;
			labelCell2.SetValue(CellBase.DescriptionFontAttributesProperty, new FontAttributes?(2));
			dynamicResourceExtension4.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = labelCell2;
			array28[1] = section4;
			array28[2] = settingsView;
			array28[3] = this;
			object obj52;
			xamlServiceProvider28.Add(typeFromHandle55, obj52 = new SimpleValueTargetProvider(array28, CellBase.HintFontSizeProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 21)));
			DynamicResource dynamicResource4 = markupExtension28.ProvideValue(xamlServiceProvider28);
			labelCell2.SetDynamicResource(CellBase.HintFontSizeProperty, dynamicResource4.Key);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "BTDeviceID";
			bindingExtension12.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.BTDeviceID, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "BTDeviceID")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			labelCell2.SetBinding(CellBase.HintTextProperty, bindingBase12);
			labelCell2.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = labelCell2;
			array29[1] = section4;
			array29[2] = settingsView;
			array29[3] = this;
			object obj53;
			xamlServiceProvider29.Add(typeFromHandle57, obj53 = new SimpleValueTargetProvider(array29, LabelCell.ValueTextColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 21)));
			DynamicResource dynamicResource5 = markupExtension29.ProvideValue(xamlServiceProvider29);
			labelCell2.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource5.Key);
			staticResourceExtension6.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension30 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = multiBinding2;
			array30[1] = labelCell2;
			array30[2] = section4;
			array30[3] = settingsView;
			array30[4] = this;
			object obj54;
			xamlServiceProvider30.Add(typeFromHandle59, obj54 = new SimpleValueTargetProvider(array30, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 39)));
			object obj55 = markupExtension30.ProvideValue(xamlServiceProvider30);
			multiBinding2.Converter = obj55;
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "BTDeviceName";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "BTDeviceID";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase14);
			labelCell2.SetBinding(LabelCell.ValueTextProperty, multiBinding2);
			section4.Add(labelCell2);
			settingsView.Root.Add(section4);
			translate20.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension31 = translate20;
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
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 17)));
			object obj57 = markupExtension31.ProvideValue(xamlServiceProvider31);
			section5.Title = obj57;
			translate21.Text = "ios_BT_MFI_Text";
			IMarkupExtension markupExtension32 = translate21;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 3];
			array32[0] = section5;
			array32[1] = settingsView;
			array32[2] = this;
			object obj58;
			xamlServiceProvider32.Add(typeFromHandle63, obj58 = new SimpleValueTargetProvider(array32, Section.FooterTextProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 17)));
			object obj59 = markupExtension32.ProvideValue(xamlServiceProvider32);
			section5.FooterText = obj59;
			bindingExtension15.Mode = 2;
			staticResourceExtension7.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = bindingExtension15;
			array33[1] = section5;
			array33[2] = settingsView;
			array33[3] = this;
			object obj60;
			xamlServiceProvider33.Add(typeFromHandle65, obj60 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 17)));
			object obj61 = markupExtension33.ProvideValue(xamlServiceProvider33);
			bindingExtension15.Converter = obj61;
			bindingExtension15.Path = "ConnectionType";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, ConnectionTypes>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase15);
			translate22.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension34 = translate22;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = labelCell3;
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
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 21)));
			object obj63 = markupExtension34.ProvideValue(xamlServiceProvider34);
			labelCell3.Title = obj63;
			translate23.Text = "settings_TapToSelectConnectionProfile";
			IMarkupExtension markupExtension35 = translate23;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = labelCell3;
			array35[1] = section5;
			array35[2] = settingsView;
			array35[3] = this;
			object obj64;
			xamlServiceProvider35.Add(typeFromHandle69, obj64 = new SimpleValueTargetProvider(array35, CellBase.DescriptionProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 21)));
			object obj65 = markupExtension35.ProvideValue(xamlServiceProvider35);
			labelCell3.Description = obj65;
			labelCell3.SetValue(CellBase.DescriptionFontAttributesProperty, new FontAttributes?(2));
			labelCell3.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension36 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = labelCell3;
			array36[1] = section5;
			array36[2] = settingsView;
			array36[3] = this;
			object obj66;
			xamlServiceProvider36.Add(typeFromHandle71, obj66 = new SimpleValueTargetProvider(array36, LabelCell.ValueTextColorProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 21)));
			DynamicResource dynamicResource6 = markupExtension36.ProvideValue(xamlServiceProvider36);
			labelCell3.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource6.Key);
			staticResourceExtension8.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension37 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 5];
			array37[0] = multiBinding3;
			array37[1] = labelCell3;
			array37[2] = section5;
			array37[3] = settingsView;
			array37[4] = this;
			object obj67;
			xamlServiceProvider37.Add(typeFromHandle73, obj67 = new SimpleValueTargetProvider(array37, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 39)));
			object obj68 = markupExtension37.ProvideValue(xamlServiceProvider37);
			multiBinding3.Converter = obj68;
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "BTDeviceName";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			multiBinding3.Bindings.Add(bindingBase16);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "BTDeviceID";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			multiBinding3.Bindings.Add(bindingBase17);
			labelCell3.SetBinding(LabelCell.ValueTextProperty, multiBinding3);
			section5.Add(labelCell3);
			settingsView.Root.Add(section5);
			section6.SetValue(Section.HeaderHeightProperty, 0.0);
			stackLayout.SetValue(VisualElement.HeightRequestProperty, 0.0);
			section6.SetValue(Section.HeaderViewProperty, stackLayout);
			translate24.Text = "ios_ConnectionGuide";
			IMarkupExtension markupExtension38 = translate24;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = buttonCell2;
			array38[1] = section6;
			array38[2] = settingsView;
			array38[3] = this;
			object obj69;
			xamlServiceProvider38.Add(typeFromHandle75, obj69 = new SimpleValueTargetProvider(array38, CellBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(186, 21)));
			object obj70 = markupExtension38.ProvideValue(xamlServiceProvider38);
			buttonCell2.Title = obj70;
			buttonCell2.Tapped += this.btnConnectionGuide_Clicked;
			dynamicResourceExtension7.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension39 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 4];
			array39[0] = buttonCell2;
			array39[1] = section6;
			array39[2] = settingsView;
			array39[3] = this;
			object obj71;
			xamlServiceProvider39.Add(typeFromHandle77, obj71 = new SimpleValueTargetProvider(array39, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 21)));
			DynamicResource dynamicResource7 = markupExtension39.ProvideValue(xamlServiceProvider39);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource7.Key);
			section6.Add(buttonCell2);
			translate25.Text = "settings_AdapterRecomendations";
			IMarkupExtension markupExtension40 = translate25;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 4];
			array40[0] = buttonCell3;
			array40[1] = section6;
			array40[2] = settingsView;
			array40[3] = this;
			object obj72;
			xamlServiceProvider40.Add(typeFromHandle79, obj72 = new SimpleValueTargetProvider(array40, CellBase.TitleProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 21)));
			object obj73 = markupExtension40.ProvideValue(xamlServiceProvider40);
			buttonCell3.Title = obj73;
			buttonCell3.Tapped += this.btnRecomendations_Tapped;
			dynamicResourceExtension8.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension41 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = buttonCell3;
			array41[1] = section6;
			array41[2] = settingsView;
			array41[3] = this;
			object obj74;
			xamlServiceProvider41.Add(typeFromHandle81, obj74 = new SimpleValueTargetProvider(array41, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 21)));
			DynamicResource dynamicResource8 = markupExtension41.ProvideValue(xamlServiceProvider41);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource8.Key);
			section6.Add(buttonCell3);
			settingsView.Root.Add(section6);
			translate26.Text = "ios_AdvancedConnectionSettings_Label";
			IMarkupExtension markupExtension42 = translate26;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 3];
			array42[0] = section7;
			array42[1] = settingsView;
			array42[2] = this;
			object obj75;
			xamlServiceProvider42.Add(typeFromHandle83, obj75 = new SimpleValueTargetProvider(array42, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 25)));
			object obj76 = markupExtension42.ProvideValue(xamlServiceProvider42);
			section7.Title = obj76;
			translate27.Text = "ios_AdvancedConnectionSettings";
			IMarkupExtension markupExtension43 = translate27;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = buttonCell4;
			array43[1] = section7;
			array43[2] = settingsView;
			array43[3] = this;
			object obj77;
			xamlServiceProvider43.Add(typeFromHandle85, obj77 = new SimpleValueTargetProvider(array43, CellBase.TitleProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(224, 21)));
			object obj78 = markupExtension43.ProvideValue(xamlServiceProvider43);
			buttonCell4.Title = obj78;
			buttonCell4.Tapped += this.btnAdvancedConnection_Clicked;
			dynamicResourceExtension9.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension44 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = buttonCell4;
			array44[1] = section7;
			array44[2] = settingsView;
			array44[3] = this;
			object obj79;
			xamlServiceProvider44.Add(typeFromHandle87, obj79 = new SimpleValueTargetProvider(array44, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(226, 21)));
			DynamicResource dynamicResource9 = markupExtension44.ProvideValue(xamlServiceProvider44);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource9.Key);
			section7.Add(buttonCell4);
			settingsView.Root.Add(section7);
			translate28.Text = "settings_Connection_BackgroundWork";
			IMarkupExtension markupExtension45 = translate28;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 3];
			array45[0] = section8;
			array45[1] = settingsView;
			array45[2] = this;
			object obj80;
			xamlServiceProvider45.Add(typeFromHandle89, obj80 = new SimpleValueTargetProvider(array45, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(229, 25)));
			object obj81 = markupExtension45.ProvideValue(xamlServiceProvider45);
			section8.Title = obj81;
			on5.Platform = new List<string>(1) { "iOS" };
			on5.Value = "False";
			onPlatform6.Platforms.Add(on5);
			on6.Platform = new List<string>(1) { "Android" };
			on6.Value = "True";
			onPlatform6.Platforms.Add(on6);
			section8.SetValue(Section.IsVisibleProperty, onPlatform6);
			translate29.Text = "droid_StartBackgroundService";
			IMarkupExtension markupExtension46 = translate29;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 4];
			array46[0] = settingsCheckBoxCellPatched4;
			array46[1] = section8;
			array46[2] = settingsView;
			array46[3] = this;
			object obj82;
			xamlServiceProvider46.Add(typeFromHandle91, obj82 = new SimpleValueTargetProvider(array46, CellBase.TitleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(236, 49)));
			object obj83 = markupExtension46.ProvideValue(xamlServiceProvider46);
			settingsCheckBoxCellPatched4.Title = obj83;
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "AndroidStartBackgroundService";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidStartBackgroundService, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidStartBackgroundService = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidStartBackgroundService")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase18);
			section8.Add(settingsCheckBoxCellPatched4);
			translate30.Text = "DroidDisplayConnectionStatusInStatusBar";
			IMarkupExtension markupExtension47 = translate30;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = settingsCheckBoxCellPatched5;
			array47[1] = section8;
			array47[2] = settingsView;
			array47[3] = this;
			object obj84;
			xamlServiceProvider47.Add(typeFromHandle93, obj84 = new SimpleValueTargetProvider(array47, CellBase.TitleProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj84);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(238, 21)));
			object obj85 = markupExtension47.ProvideValue(xamlServiceProvider47);
			settingsCheckBoxCellPatched5.Title = obj85;
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "DroidDisplayConnectionStatusInStatusBar";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DroidDisplayConnectionStatusInStatusBar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DroidDisplayConnectionStatusInStatusBar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DroidDisplayConnectionStatusInStatusBar")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase19);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "AndroidStartBackgroundService";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidStartBackgroundService, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidStartBackgroundService")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CellBase.IsVisibleProperty, bindingBase20);
			section8.Add(settingsCheckBoxCellPatched5);
			translate31.Text = "droid_BackgroundTroubleshooting";
			IMarkupExtension markupExtension48 = translate31;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = buttonCell5;
			array48[1] = section8;
			array48[2] = settingsView;
			array48[3] = this;
			object obj86;
			xamlServiceProvider48.Add(typeFromHandle95, obj86 = new SimpleValueTargetProvider(array48, CellBase.TitleProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj86);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 21)));
			object obj87 = markupExtension48.ProvideValue(xamlServiceProvider48);
			buttonCell5.Title = obj87;
			buttonCell5.Tapped += this.BtnDroidBackgroundTrobleshooting_Clicked;
			dynamicResourceExtension10.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension49 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 4];
			array49[0] = buttonCell5;
			array49[1] = section8;
			array49[2] = settingsView;
			array49[3] = this;
			object obj88;
			xamlServiceProvider49.Add(typeFromHandle97, obj88 = new SimpleValueTargetProvider(array49, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 21)));
			DynamicResource dynamicResource10 = markupExtension49.ProvideValue(xamlServiceProvider49);
			buttonCell5.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource10.Key);
			section8.Add(buttonCell5);
			settingsView.Root.Add(section8);
			translate32.Text = "settings_StopConnectionAttemptsAfterFailsMinutes";
			IMarkupExtension markupExtension50 = translate32;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 3];
			array50[0] = section9;
			array50[1] = settingsView;
			array50[2] = this;
			object obj89;
			xamlServiceProvider50.Add(typeFromHandle99, obj89 = new SimpleValueTargetProvider(array50, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(247, 25)));
			object obj90 = markupExtension50.ProvideValue(xamlServiceProvider50);
			section9.Title = obj90;
			entryCell3.SetValue(CellBase.TitleProperty, "");
			translate33.Text = "settings_StopConnectionAttemptsAfterFailsMinutes_Hint";
			IMarkupExtension markupExtension51 = translate33;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 4];
			array51[0] = entryCell3;
			array51[1] = section9;
			array51[2] = settingsView;
			array51[3] = this;
			object obj91;
			xamlServiceProvider51.Add(typeFromHandle101, obj91 = new SimpleValueTargetProvider(array51, CellBase.HintTextProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(250, 21)));
			object obj92 = markupExtension51.ProvideValue(xamlServiceProvider51);
			entryCell3.HintText = obj92;
			bindingExtension21.Mode = 1;
			staticResourceExtension9.Key = "IntToStringConverter";
			IMarkupExtension markupExtension52 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 5];
			array52[0] = bindingExtension21;
			array52[1] = entryCell3;
			array52[2] = section9;
			array52[3] = settingsView;
			array52[4] = this;
			object obj93;
			xamlServiceProvider52.Add(typeFromHandle103, obj93 = new SimpleValueTargetProvider(array52, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj93);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(251, 21)));
			object obj94 = markupExtension52.ProvideValue(xamlServiceProvider52);
			bindingExtension21.Converter = obj94;
			bindingExtension21.Path = "StopConnectionAttemptsAfterFailsMinutes";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.StopConnectionAttemptsAfterFailsMinutes, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.StopConnectionAttemptsAfterFailsMinutes = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "StopConnectionAttemptsAfterFailsMinutes")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			entryCell3.SetBinding(EntryCell.ValueTextProperty, bindingBase21);
			section9.Add(entryCell3);
			settingsView.Root.Add(section9);
			translate34.Text = "settings_AutomaticConnection";
			IMarkupExtension markupExtension53 = translate34;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 3];
			array53[0] = section10;
			array53[1] = settingsView;
			array53[2] = this;
			object obj95;
			xamlServiceProvider53.Add(typeFromHandle105, obj95 = new SimpleValueTargetProvider(array53, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj95);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(254, 25)));
			object obj96 = markupExtension53.ProvideValue(xamlServiceProvider53);
			section10.Title = obj96;
			translate35.Text = "Settings_Control_ConnectOnLaunch.Header";
			IMarkupExtension markupExtension54 = translate35;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 4];
			array54[0] = settingsCheckBoxCellPatched6;
			array54[1] = section10;
			array54[2] = settingsView;
			array54[3] = this;
			object obj97;
			xamlServiceProvider54.Add(typeFromHandle107, obj97 = new SimpleValueTargetProvider(array54, CellBase.TitleProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj97);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(255, 49)));
			object obj98 = markupExtension54.ProvideValue(xamlServiceProvider54);
			settingsCheckBoxCellPatched6.Title = obj98;
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "ConnectOnLaunch";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ConnectOnLaunch, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ConnectOnLaunch = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectOnLaunch")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase22);
			section10.Add(settingsCheckBoxCellPatched6);
			translate36.Text = "OpenDashboardOnLaunch";
			IMarkupExtension markupExtension55 = translate36;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 4];
			array55[0] = settingsCheckBoxCellPatched7;
			array55[1] = section10;
			array55[2] = settingsView;
			array55[3] = this;
			object obj99;
			xamlServiceProvider55.Add(typeFromHandle109, obj99 = new SimpleValueTargetProvider(array55, CellBase.TitleProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj99);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(257, 21)));
			object obj100 = markupExtension55.ProvideValue(xamlServiceProvider55);
			settingsCheckBoxCellPatched7.Title = obj100;
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "OpenDashboardOnLaunch";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.OpenDashboardOnLaunch, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.OpenDashboardOnLaunch = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "OpenDashboardOnLaunch")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "ConnectOnLaunch";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ConnectOnLaunch, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ConnectOnLaunch")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CellBase.IsVisibleProperty, bindingBase24);
			section10.Add(settingsCheckBoxCellPatched7);
			settingsView.Root.Add(section10);
			translate37.Text = "ios_ExportLog";
			IMarkupExtension markupExtension56 = translate37;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 4];
			array56[0] = buttonCell6;
			array56[1] = section11;
			array56[2] = settingsView;
			array56[3] = this;
			object obj101;
			xamlServiceProvider56.Add(typeFromHandle111, obj101 = new SimpleValueTargetProvider(array56, CellBase.TitleProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj101);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 21)));
			object obj102 = markupExtension56.ProvideValue(xamlServiceProvider56);
			buttonCell6.Title = obj102;
			buttonCell6.Tapped += this.btnExportLog_Clicked;
			dynamicResourceExtension11.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension57 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 4];
			array57[0] = buttonCell6;
			array57[1] = section11;
			array57[2] = settingsView;
			array57[3] = this;
			object obj103;
			xamlServiceProvider57.Add(typeFromHandle113, obj103 = new SimpleValueTargetProvider(array57, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj103);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(270, 21)));
			DynamicResource dynamicResource11 = markupExtension57.ProvideValue(xamlServiceProvider57);
			buttonCell6.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource11.Key);
			section11.Add(buttonCell6);
			translate38.Text = "settings_ELM_Errors";
			IMarkupExtension markupExtension58 = translate38;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 4];
			array58[0] = buttonCell7;
			array58[1] = section11;
			array58[2] = settingsView;
			array58[3] = this;
			object obj104;
			xamlServiceProvider58.Add(typeFromHandle115, obj104 = new SimpleValueTargetProvider(array58, CellBase.TitleProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj104);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(273, 21)));
			object obj105 = markupExtension58.ProvideValue(xamlServiceProvider58);
			buttonCell7.Title = obj105;
			buttonCell7.Tapped += this.ELMErrors_Tapped;
			dynamicResourceExtension12.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension59 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 4];
			array59[0] = buttonCell7;
			array59[1] = section11;
			array59[2] = settingsView;
			array59[3] = this;
			object obj106;
			xamlServiceProvider59.Add(typeFromHandle117, obj106 = new SimpleValueTargetProvider(array59, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj106);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider59.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver59, typeof(SettingsConnectionPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 21)));
			DynamicResource dynamicResource12 = markupExtension59.ProvideValue(xamlServiceProvider59);
			buttonCell7.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource12.Key);
			section11.Add(buttonCell7);
			settingsView.Root.Add(section11);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06001FF9 RID: 8185 RVA: 0x0016CA70 File Offset: 0x0016AC70
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsConnectionPageV3>(this, typeof(SettingsConnectionPageV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.connectionTypeSection = NameScopeExtensions.FindByName<Section>(this, "connectionTypeSection");
			this.cellWiFi = NameScopeExtensions.FindByName<RadioCell>(this, "cellWiFi");
			this.cellBT = NameScopeExtensions.FindByName<RadioCell>(this, "cellBT");
			this.wifiPanel = NameScopeExtensions.FindByName<Section>(this, "wifiPanel");
			this.bluetoothLEPanel = NameScopeExtensions.FindByName<Section>(this, "bluetoothLEPanel");
			this.bluetooth2Panel = NameScopeExtensions.FindByName<Section>(this, "bluetooth2Panel");
			this.bluetoothMFIPanel = NameScopeExtensions.FindByName<Section>(this, "bluetoothMFIPanel");
		}

		// Token: 0x06001FFA RID: 8186 RVA: 0x0016CB18 File Offset: 0x0016AD18
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1827(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06001FFB RID: 8187 RVA: 0x0016CB48 File Offset: 0x0016AD48
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1828(SharedSettings A_0, ConnectionTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ConnectionType = A_1;
				return;
			}
		}

		// Token: 0x06001FFC RID: 8188 RVA: 0x0016CB64 File Offset: 0x0016AD64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1829(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FFD RID: 8189 RVA: 0x0016CB74 File Offset: 0x0016AD74
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1830(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06001FFE RID: 8190 RVA: 0x0016CBA4 File Offset: 0x0016ADA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1831(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001FFF RID: 8191 RVA: 0x0016CBB4 File Offset: 0x0016ADB4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1832(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.WiFiServer, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06002000 RID: 8192 RVA: 0x0016CBE4 File Offset: 0x0016ADE4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1833(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.WiFiServer = A_1;
				return;
			}
		}

		// Token: 0x06002001 RID: 8193 RVA: 0x0016CC00 File Offset: 0x0016AE00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1834(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002002 RID: 8194 RVA: 0x0016CC10 File Offset: 0x0016AE10
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1835(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.WiFiPort, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06002003 RID: 8195 RVA: 0x0016CC40 File Offset: 0x0016AE40
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1836(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.WiFiPort = A_1;
				return;
			}
		}

		// Token: 0x06002004 RID: 8196 RVA: 0x0016CC5C File Offset: 0x0016AE5C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1837(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002005 RID: 8197 RVA: 0x0016CC6C File Offset: 0x0016AE6C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1838(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.NoWiFiWarning, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002006 RID: 8198 RVA: 0x0016CC9C File Offset: 0x0016AE9C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1839(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.NoWiFiWarning = A_1;
				return;
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x0016CCB8 File Offset: 0x0016AEB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1840(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0016CCC8 File Offset: 0x0016AEC8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1841(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.TryConnectToLastWiFiNetwork, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x0016CCF8 File Offset: 0x0016AEF8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1842(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.TryConnectToLastWiFiNetwork = A_1;
				return;
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0016CD14 File Offset: 0x0016AF14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1843(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x0016CD24 File Offset: 0x0016AF24
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1844(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x0016CD54 File Offset: 0x0016AF54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1845(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x0016CD64 File Offset: 0x0016AF64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1846(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SearchForBTLEIfConnectionFailed, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0016CD94 File Offset: 0x0016AF94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1847(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SearchForBTLEIfConnectionFailed = A_1;
				return;
			}
		}

		// Token: 0x0600200F RID: 8207 RVA: 0x0016CDB0 File Offset: 0x0016AFB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1848(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002010 RID: 8208 RVA: 0x0016CDC0 File Offset: 0x0016AFC0
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1849(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x0016CDF0 File Offset: 0x0016AFF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1850(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x0016CE00 File Offset: 0x0016B000
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1851(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.BTDeviceID, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x0016CE30 File Offset: 0x0016B030
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1852(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x0016CE40 File Offset: 0x0016B040
		[CompilerGenerated]
		private static ValueTuple<ConnectionTypes, bool> <InitializeComponent>typedBindingsM__1853(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ConnectionTypes, bool>(A_0.ConnectionType, true);
			}
			return default(ValueTuple<ConnectionTypes, bool>);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0016CE70 File Offset: 0x0016B070
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1854(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x0016CE80 File Offset: 0x0016B080
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1855(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidStartBackgroundService, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x0016CEB0 File Offset: 0x0016B0B0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1856(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidStartBackgroundService = A_1;
				return;
			}
		}

		// Token: 0x06002018 RID: 8216 RVA: 0x0016CECC File Offset: 0x0016B0CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1857(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002019 RID: 8217 RVA: 0x0016CEDC File Offset: 0x0016B0DC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1858(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DroidDisplayConnectionStatusInStatusBar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600201A RID: 8218 RVA: 0x0016CF0C File Offset: 0x0016B10C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1859(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DroidDisplayConnectionStatusInStatusBar = A_1;
				return;
			}
		}

		// Token: 0x0600201B RID: 8219 RVA: 0x0016CF28 File Offset: 0x0016B128
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1860(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600201C RID: 8220 RVA: 0x0016CF38 File Offset: 0x0016B138
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1861(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidStartBackgroundService, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600201D RID: 8221 RVA: 0x0016CF68 File Offset: 0x0016B168
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1862(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600201E RID: 8222 RVA: 0x0016CF78 File Offset: 0x0016B178
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1863(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.StopConnectionAttemptsAfterFailsMinutes, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600201F RID: 8223 RVA: 0x0016CFA8 File Offset: 0x0016B1A8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1864(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.StopConnectionAttemptsAfterFailsMinutes = A_1;
				return;
			}
		}

		// Token: 0x06002020 RID: 8224 RVA: 0x0016CFC4 File Offset: 0x0016B1C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1865(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002021 RID: 8225 RVA: 0x0016CFD4 File Offset: 0x0016B1D4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1866(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ConnectOnLaunch, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002022 RID: 8226 RVA: 0x0016D004 File Offset: 0x0016B204
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1867(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ConnectOnLaunch = A_1;
				return;
			}
		}

		// Token: 0x06002023 RID: 8227 RVA: 0x0016D020 File Offset: 0x0016B220
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1868(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002024 RID: 8228 RVA: 0x0016D030 File Offset: 0x0016B230
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1869(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.OpenDashboardOnLaunch, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002025 RID: 8229 RVA: 0x0016D060 File Offset: 0x0016B260
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1870(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.OpenDashboardOnLaunch = A_1;
				return;
			}
		}

		// Token: 0x06002026 RID: 8230 RVA: 0x0016D07C File Offset: 0x0016B27C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1871(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002027 RID: 8231 RVA: 0x0016D08C File Offset: 0x0016B28C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1872(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ConnectOnLaunch, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002028 RID: 8232 RVA: 0x0016D0BC File Offset: 0x0016B2BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1873(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000F59 RID: 3929
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000F5A RID: 3930
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section connectionTypeSection;

		// Token: 0x04000F5B RID: 3931
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioCell cellWiFi;

		// Token: 0x04000F5C RID: 3932
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioCell cellBT;

		// Token: 0x04000F5D RID: 3933
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section wifiPanel;

		// Token: 0x04000F5E RID: 3934
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetoothLEPanel;

		// Token: 0x04000F5F RID: 3935
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetooth2Panel;

		// Token: 0x04000F60 RID: 3936
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetoothMFIPanel;

		// Token: 0x0200028E RID: 654
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnExportLog_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06002029 RID: 8233 RVA: 0x0016D0CC File Offset: 0x0016B2CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionPageV3 settingsConnectionPageV = this;
				try
				{
					if (num > 3)
					{
						if (num - 4 <= 1)
						{
							goto IL_01FA;
						}
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							goto IL_031E;
						}
					}
					TaskAwaiter taskAwaiter4;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							break;
						}
						case 1:
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0108;
						case 2:
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0170;
						case 3:
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_01D7;
						default:
							taskAwaiter = SharedSettings.Current.GetSettingsReport().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						string result = taskAwaiter.GetResult();
						text = result;
						taskAwaiter3 = App.OBDReader.DebugWrite("\r\n[///**** CONTACT DEVELOPER REPORT:").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0108:
						taskAwaiter3.GetResult();
						taskAwaiter3 = App.OBDReader.DebugWrite(text).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 2);
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0170:
						taskAwaiter3.GetResult();
						taskAwaiter3 = App.OBDReader.DebugWrite("\r\n[***/// END OF CONTACT DEVELOPER REPORT]").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_01D7:
						taskAwaiter3.GetResult();
						text = null;
					}
					catch (Exception)
					{
					}
					try
					{
						PCLDebugStream.CurrentInstance.Close();
					}
					catch (Exception)
					{
					}
					IL_01FA:
					try
					{
						TaskAwaiter taskAwaiter3;
						if (num != 4)
						{
							if (num == 5)
							{
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_02F2;
							}
							text = PCLDebugStream.GetFilepath();
							if (!PlatformHelper.IsiOS)
							{
								goto IL_0285;
							}
							taskAwaiter3 = Share.RequestAsync(new ShareFileRequest(new ShareFile(text))).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 4);
								taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter3.GetResult();
						IL_0285:
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_02F9;
						}
						taskAwaiter3 = Share.RequestAsync(new ShareFileRequest(new ShareFile(text))).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 5);
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnExportLog_Clicked>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						IL_02F2:
						taskAwaiter3.GetResult();
						IL_02F9:
						text = null;
						goto IL_0339;
					}
					catch (Exception ex)
					{
						settingsConnectionPageV.DisplayAlert("Error", ex.Message, "OK");
						goto IL_0339;
					}
					IL_031E:
					settingsConnectionPageV.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK");
					IL_0339:;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600202A RID: 8234 RVA: 0x0016D4A4 File Offset: 0x0016B6A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F61 RID: 3937
			public int <>1__state;

			// Token: 0x04000F62 RID: 3938
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F63 RID: 3939
			public SettingsConnectionPageV3 <>4__this;

			// Token: 0x04000F64 RID: 3940
			private string <text>5__2;

			// Token: 0x04000F65 RID: 3941
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04000F66 RID: 3942
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200028F RID: 655
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectDevice_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600202B RID: 8235 RVA: 0x0016D4B4 File Offset: 0x0016B6B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionPageV3 settingsConnectionPageV = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
						break;
					case 1:
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0145;
					case 2:
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01BD;
					case 3:
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_022F;
					default:
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE)
						{
							if (!PlatformHelper.IsiOS)
							{
								goto IL_00E6;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth)
						{
							taskAwaiter = settingsConnectionPageV.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnSelectDevice_Clicked>d__3>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01BD;
						}
						else
						{
							if (SharedSettings.Current.ConnectionType != ConnectionTypes.MFI_OBDLinkMXPlus)
							{
								goto IL_0236;
							}
							taskAwaiter = settingsConnectionPageV.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnSelectDevice_Clicked>d__3>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_022F;
						}
						break;
					}
					try
					{
						if (num != 0)
						{
							if (!PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0) || !PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
							{
								goto IL_00E6;
							}
							taskAwaiter = settingsConnectionPageV.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnSelectDevice_Clicked>d__3>(ref taskAwaiter, ref this);
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
						PlatformHelper.CommonService.OpenPermissionsSettings();
						goto IL_024F;
					}
					catch (Exception)
					{
					}
					IL_00E6:
					taskAwaiter = settingsConnectionPageV.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPageV3.<btnSelectDevice_Clicked>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0145:
					taskAwaiter.GetResult();
					goto IL_0236;
					IL_01BD:
					taskAwaiter.GetResult();
					goto IL_0236;
					IL_022F:
					taskAwaiter.GetResult();
					IL_0236:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_024F:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600202C RID: 8236 RVA: 0x0016D758 File Offset: 0x0016B958
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F67 RID: 3943
			public int <>1__state;

			// Token: 0x04000F68 RID: 3944
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F69 RID: 3945
			public SettingsConnectionPageV3 <>4__this;

			// Token: 0x04000F6A RID: 3946
			private TaskAwaiter <>u__1;
		}
	}
}
