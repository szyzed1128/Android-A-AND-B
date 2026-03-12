using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001CD RID: 461
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml")]
	public class SettingsConnectionPage : ContentPage
	{
		// Token: 0x060018EE RID: 6382 RVA: 0x000E2704 File Offset: 0x000E0904
		public SettingsConnectionPage()
		{
			this.InitializeComponent();
			base.BindingContext = SharedSettings.Current;
			if (PlatformHelper.IsAndroid && !PlatformHelper.IsPlatformVersionNewerOrEqual(26, 0))
			{
				this.switchDisplayStatusInNotification.IsVisible = false;
			}
			if (PlatformHelper.IsiOS && Device.Idiom == 2)
			{
				this.btnShareLog.IsVisible = false;
			}
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x000E2760 File Offset: 0x000E0960
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

		// Token: 0x060018F0 RID: 6384 RVA: 0x000E27B0 File Offset: 0x000E09B0
		private void btnAdvancedConnection_Clicked(object sender, EventArgs e)
		{
			SettingsConnectionExpertPage settingsConnectionExpertPage = new SettingsConnectionExpertPage();
			base.Navigation.PushAsync(settingsConnectionExpertPage);
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x000E27D0 File Offset: 0x000E09D0
		private async void btnClearLog_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				PCLDebugStream.CurrentInstance.Clear();
			}
			else
			{
				await base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK");
			}
		}

		// Token: 0x060018F2 RID: 6386 RVA: 0x000E2808 File Offset: 0x000E0A08
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

		// Token: 0x060018F3 RID: 6387 RVA: 0x000E2840 File Offset: 0x000E0A40
		private async void btnExportLog_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				try
				{
					PCLDebugStream.CurrentInstance.Close();
				}
				catch (Exception)
				{
				}
				try
				{
					await Share.RequestAsync(new ShareFileRequest(new ShareFile(PCLDebugStream.GetFilepath())));
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

		// Token: 0x060018F4 RID: 6388 RVA: 0x000E2878 File Offset: 0x000E0A78
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

		// Token: 0x060018F5 RID: 6389 RVA: 0x000E28D4 File Offset: 0x000E0AD4
		private void btnConnectionType_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (this.btnWiFi.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.ConnectionType = ConnectionTypes.WiFi;
				return;
			}
			if (this.btnBluetooth.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.ConnectionType = ConnectionTypes.Bluetooth;
				return;
			}
			if (this.btnBluetoothLE.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.ConnectionType = ConnectionTypes.BluetoothLE;
				return;
			}
			if (this.btnMFI.IsChecked.GetValueOrDefault())
			{
				SharedSettings.Current.ConnectionType = ConnectionTypes.MFI_OBDLinkMXPlus;
			}
		}

		// Token: 0x060018F6 RID: 6390 RVA: 0x000E2964 File Offset: 0x000E0B64
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

		// Token: 0x060018F7 RID: 6391 RVA: 0x000E2994 File Offset: 0x000E0B94
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsConnectionPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsConnectionPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			ConnectionTypeToWiFiVisibleBoolConverter connectionTypeToWiFiVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverter = new ConnectionTypeToWiFiVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 26);
			ConnectionTypeToBTLEVisibleBoolConverter connectionTypeToBTLEVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTLEVisibleBoolConverter = new ConnectionTypeToBTLEVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 26);
			ConnectionTypeToBTVisibleBoolConverter connectionTypeToBTVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverter = new ConnectionTypeToBTVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 26);
			ConnectionTypeToMFIBluetoothConverter connectionTypeToMFIBluetoothConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToMFIBluetoothConverter = new ConnectionTypeToMFIBluetoothConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 26);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 26);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 26);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 26);
			ConnectionTypeToIntConverter connectionTypeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToIntConverter = new ConnectionTypeToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 26);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 24);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 25);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 25);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 25);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 25);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 22);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 25);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 25);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 34);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 34);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 30);
			SfRadioButton sfRadioButton3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton3 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 25);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 34);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 34);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 30);
			SfRadioButton sfRadioButton4;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton4 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 22);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 18);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 21);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 28);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 22);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 53);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 28);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 22);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 25);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 22);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 28);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 70);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 40);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 89);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 40);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 103);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 21);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 25);
			On on5;
			VisualDiagnostics.RegisterSourceInfo(on5 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
			On on6;
			VisualDiagnostics.RegisterSourceInfo(on6 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 30);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 22);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 28);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 22);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 25);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 22);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 33);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 33);
			OnPlatform<string> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<string>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 30);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 18);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 21);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 21);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 28);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 22);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 25);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 22);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 22);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 33);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 33);
			OnPlatform<string> onPlatform5;
			VisualDiagnostics.RegisterSourceInfo(onPlatform5 = new OnPlatform<string>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 30);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 18);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 21);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 21);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 28);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 22);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 25);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 22);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 22);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 21);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 24);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 18);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 21);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 62);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 18);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 24);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 18);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 65);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 18);
			OnPlatform<bool> onPlatform6;
			VisualDiagnostics.RegisterSourceInfo(onPlatform6 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 44);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 109);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 26);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 29);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 29);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 29);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 26);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 29);
			On on7;
			VisualDiagnostics.RegisterSourceInfo(on7 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 38);
			On on8;
			VisualDiagnostics.RegisterSourceInfo(on8 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 38);
			OnPlatform<bool> onPlatform7;
			VisualDiagnostics.RegisterSourceInfo(onPlatform7 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 34);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 26);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 22);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 40);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 91);
			LabelSwitch labelSwitch5;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch5 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 22);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 25);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 25);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 25);
			LabelSwitch labelSwitch6;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch6 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 22);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 25);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 22);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 18);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsConnectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("LayoutRoot", stackLayout7);
			if (stackLayout7.StyleId == null)
			{
				stackLayout7.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("connectionTypeGroup", sfRadioGroup);
			if (sfRadioGroup.StyleId == null)
			{
				sfRadioGroup.StyleId = "connectionTypeGroup";
			}
			nameScope.RegisterName("btnWiFi", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "btnWiFi";
			}
			nameScope.RegisterName("btnBluetoothLE", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "btnBluetoothLE";
			}
			nameScope.RegisterName("btnBluetooth", sfRadioButton3);
			if (sfRadioButton3.StyleId == null)
			{
				sfRadioButton3.StyleId = "btnBluetooth";
			}
			nameScope.RegisterName("btnMFI", sfRadioButton4);
			if (sfRadioButton4.StyleId == null)
			{
				sfRadioButton4.StyleId = "btnMFI";
			}
			nameScope.RegisterName("wifiPanel", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "wifiPanel";
			}
			nameScope.RegisterName("bluetoothLEPanel", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "bluetoothLEPanel";
			}
			nameScope.RegisterName("bluetooth2Panel", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "bluetooth2Panel";
			}
			nameScope.RegisterName("bluetoothMFIPanel", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "bluetoothMFIPanel";
			}
			nameScope.RegisterName("switchDisplayStatusInNotification", labelSwitch4);
			if (labelSwitch4.StyleId == null)
			{
				labelSwitch4.StyleId = "switchDisplayStatusInNotification";
			}
			nameScope.RegisterName("btnDroidBackgroundTrobleshooting", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnDroidBackgroundTrobleshooting";
			}
			nameScope.RegisterName("btnShareLog", button7);
			if (button7.StyleId == null)
			{
				button7.StyleId = "btnShareLog";
			}
			this.LayoutRoot = stackLayout7;
			this.connectionTypeGroup = sfRadioGroup;
			this.btnWiFi = sfRadioButton;
			this.btnBluetoothLE = sfRadioButton2;
			this.btnBluetooth = sfRadioButton3;
			this.btnMFI = sfRadioButton4;
			this.wifiPanel = stackLayout;
			this.bluetoothLEPanel = stackLayout2;
			this.bluetooth2Panel = stackLayout3;
			this.bluetoothMFIPanel = stackLayout4;
			this.switchDisplayStatusInNotification = labelSwitch4;
			this.btnDroidBackgroundTrobleshooting = button6;
			this.btnShareLog = button7;
			stackLayout7.Resources = resourceDictionary;
			resourceDictionary.Add("ConnectionTypeToWiFiVisibleBoolConverter", connectionTypeToWiFiVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTLEVisibleBoolConverter", connectionTypeToBTLEVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTVisibleBoolConverter", connectionTypeToBTVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToMFIBluetoothConverter", connectionTypeToMFIBluetoothConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("ConnectionTypeToIntConverter", connectionTypeToIntConverter);
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("xf", "Xamarin.Forms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("xf", "Xamarin.Forms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			scrollView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout7.Resources = resourceDictionary;
			translate2.Text = "Settings_Control_tbConnectionType.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label;
			array3[1] = stackLayout7;
			array3[2] = scrollView;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("xf", "Xamarin.Forms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 24)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			stackLayout7.Children.Add(label);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension2.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = sfRadioButton;
			array4[1] = sfRadioGroup;
			array4[2] = stackLayout7;
			array4[3] = scrollView;
			array4[4] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("xf", "Xamarin.Forms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 25)));
			DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfRadioButton.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource2.Key);
			bindingExtension.Mode = 2;
			staticResourceExtension.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension;
			array5[1] = sfRadioButton;
			array5[2] = sfRadioGroup;
			array5[3] = stackLayout7;
			array5[4] = scrollView;
			array5[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("xf", "Xamarin.Forms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 25)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension.Converter = obj8;
			bindingExtension.Path = "ConnectionType";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfRadioButton.SetBinding(ToggleButton.IsCheckedProperty, bindingBase);
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton.SetValue(ToggleButton.TextProperty, "Wi-Fi");
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = sfRadioButton;
			array6[1] = sfRadioGroup;
			array6[2] = stackLayout7;
			array6[3] = scrollView;
			array6[4] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("xf", "Xamarin.Forms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 25)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfRadioButton.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = sfRadioButton;
			array7[1] = sfRadioGroup;
			array7[2] = stackLayout7;
			array7[3] = scrollView;
			array7[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("xf", "Xamarin.Forms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 25)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			sfRadioButton.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource4.Key);
			sfRadioGroup.Children.Add(sfRadioButton);
			dynamicResourceExtension5.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = sfRadioButton2;
			array8[1] = sfRadioGroup;
			array8[2] = stackLayout7;
			array8[3] = scrollView;
			array8[4] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("xf", "Xamarin.Forms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 25)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			sfRadioButton2.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource5.Key);
			bindingExtension2.Mode = 2;
			staticResourceExtension2.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension2;
			array9[1] = sfRadioButton2;
			array9[2] = sfRadioGroup;
			array9[3] = stackLayout7;
			array9[4] = scrollView;
			array9[5] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("xf", "Xamarin.Forms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 25)));
			object obj13 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension2.Converter = obj13;
			bindingExtension2.Path = "ConnectionType";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			sfRadioButton2.SetBinding(ToggleButton.IsCheckedProperty, bindingBase2);
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton2.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton2.SetValue(ToggleButton.TextProperty, "Bluetooth LE (4.0+)");
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = sfRadioButton2;
			array10[1] = sfRadioGroup;
			array10[2] = stackLayout7;
			array10[3] = scrollView;
			array10[4] = this;
			object obj14;
			xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array10, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("xf", "Xamarin.Forms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 25)));
			DynamicResource dynamicResource6 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfRadioButton2.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource6.Key);
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = sfRadioButton2;
			array11[1] = sfRadioGroup;
			array11[2] = stackLayout7;
			array11[3] = scrollView;
			array11[4] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("xf", "Xamarin.Forms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 25)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			sfRadioButton2.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource7.Key);
			sfRadioGroup.Children.Add(sfRadioButton2);
			dynamicResourceExtension8.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = sfRadioButton3;
			array12[1] = sfRadioGroup;
			array12[2] = stackLayout7;
			array12[3] = scrollView;
			array12[4] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("xf", "Xamarin.Forms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 25)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			sfRadioButton3.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource8.Key);
			bindingExtension3.Mode = 2;
			staticResourceExtension3.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension3;
			array13[1] = sfRadioButton3;
			array13[2] = sfRadioGroup;
			array13[3] = stackLayout7;
			array13[4] = scrollView;
			array13[5] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("xf", "Xamarin.Forms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 25)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension3.Converter = obj18;
			bindingExtension3.Path = "ConnectionType";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			sfRadioButton3.SetBinding(ToggleButton.IsCheckedProperty, bindingBase3);
			sfRadioButton3.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton3.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton3.SetValue(ToggleButton.TextProperty, "Bluetooth");
			dynamicResourceExtension9.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = sfRadioButton3;
			array14[1] = sfRadioGroup;
			array14[2] = stackLayout7;
			array14[3] = scrollView;
			array14[4] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("xf", "Xamarin.Forms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 25)));
			DynamicResource dynamicResource9 = markupExtension14.ProvideValue(xamlServiceProvider14);
			sfRadioButton3.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource9.Key);
			dynamicResourceExtension10.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = sfRadioButton3;
			array15[1] = sfRadioGroup;
			array15[2] = stackLayout7;
			array15[3] = scrollView;
			array15[4] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle29, obj20 = new SimpleValueTargetProvider(array15, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("xf", "Xamarin.Forms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 25)));
			DynamicResource dynamicResource10 = markupExtension15.ProvideValue(xamlServiceProvider15);
			sfRadioButton3.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource10.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform.Platforms.Add(on2);
			sfRadioButton3.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			sfRadioGroup.Children.Add(sfRadioButton3);
			dynamicResourceExtension11.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = sfRadioButton4;
			array16[1] = sfRadioGroup;
			array16[2] = stackLayout7;
			array16[3] = scrollView;
			array16[4] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle31, obj21 = new SimpleValueTargetProvider(array16, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("xf", "Xamarin.Forms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 25)));
			DynamicResource dynamicResource11 = markupExtension16.ProvideValue(xamlServiceProvider16);
			sfRadioButton4.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource11.Key);
			bindingExtension4.Mode = 2;
			staticResourceExtension4.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension17 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = bindingExtension4;
			array17[1] = sfRadioButton4;
			array17[2] = sfRadioGroup;
			array17[3] = stackLayout7;
			array17[4] = scrollView;
			array17[5] = this;
			object obj22;
			xamlServiceProvider17.Add(typeFromHandle33, obj22 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("xf", "Xamarin.Forms");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 25)));
			object obj23 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension4.Converter = obj23;
			bindingExtension4.Path = "ConnectionType";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			sfRadioButton4.SetBinding(ToggleButton.IsCheckedProperty, bindingBase4);
			sfRadioButton4.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton4.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton4.SetValue(ToggleButton.TextProperty, "Bluetooth MFi (OBDLink MX+, vLinker FS)");
			dynamicResourceExtension12.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = sfRadioButton4;
			array18[1] = sfRadioGroup;
			array18[2] = stackLayout7;
			array18[3] = scrollView;
			array18[4] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle35, obj24 = new SimpleValueTargetProvider(array18, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("xf", "Xamarin.Forms");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 25)));
			DynamicResource dynamicResource12 = markupExtension18.ProvideValue(xamlServiceProvider18);
			sfRadioButton4.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource12.Key);
			dynamicResourceExtension13.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = sfRadioButton4;
			array19[1] = sfRadioGroup;
			array19[2] = stackLayout7;
			array19[3] = scrollView;
			array19[4] = this;
			object obj25;
			xamlServiceProvider19.Add(typeFromHandle37, obj25 = new SimpleValueTargetProvider(array19, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("xf", "Xamarin.Forms");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
			DynamicResource dynamicResource13 = markupExtension19.ProvideValue(xamlServiceProvider19);
			sfRadioButton4.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource13.Key);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "true";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "false";
			onPlatform2.Platforms.Add(on4);
			sfRadioButton4.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			sfRadioGroup.Children.Add(sfRadioButton4);
			stackLayout7.Children.Add(sfRadioGroup);
			bindingExtension5.Mode = 2;
			staticResourceExtension5.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = bindingExtension5;
			array20[1] = stackLayout;
			array20[2] = stackLayout7;
			array20[3] = scrollView;
			array20[4] = this;
			object obj26;
			xamlServiceProvider20.Add(typeFromHandle39, obj26 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("xf", "Xamarin.Forms");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 21)));
			object obj27 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension5.Converter = obj27;
			bindingExtension5.Path = "ConnectionType";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate3.Text = "Settings_Control_tbWiFiServer.Text";
			IMarkupExtension markupExtension21 = translate3;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 5];
			array21[0] = label2;
			array21[1] = stackLayout;
			array21[2] = stackLayout7;
			array21[3] = scrollView;
			array21[4] = this;
			object obj28;
			xamlServiceProvider21.Add(typeFromHandle41, obj28 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("xf", "Xamarin.Forms");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 28)));
			object obj29 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label2.Text = obj29;
			stackLayout.Children.Add(label2);
			entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "WiFiServer";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase6);
			stackLayout.Children.Add(entry);
			translate4.Text = "Settings_Control_tbWiFiPort.Text";
			IMarkupExtension markupExtension22 = translate4;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = label3;
			array22[1] = stackLayout;
			array22[2] = stackLayout7;
			array22[3] = scrollView;
			array22[4] = this;
			object obj30;
			xamlServiceProvider22.Add(typeFromHandle43, obj30 = new SimpleValueTargetProvider(array22, Label.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("xf", "Xamarin.Forms");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 28)));
			object obj31 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label3.Text = obj31;
			stackLayout.Children.Add(label3);
			entry2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			entry2.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "WiFiPort";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase7);
			stackLayout.Children.Add(entry2);
			dynamicResourceExtension14.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = label4;
			array23[1] = stackLayout;
			array23[2] = stackLayout7;
			array23[3] = scrollView;
			array23[4] = this;
			object obj32;
			xamlServiceProvider23.Add(typeFromHandle45, obj32 = new SimpleValueTargetProvider(array23, Label.FontSizeProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("xf", "Xamarin.Forms");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 28)));
			DynamicResource dynamicResource14 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource14.Key);
			translate5.Text = "ios_DefaultWiFi_Servers";
			IMarkupExtension markupExtension24 = translate5;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = label4;
			array24[1] = stackLayout;
			array24[2] = stackLayout7;
			array24[3] = scrollView;
			array24[4] = this;
			object obj33;
			xamlServiceProvider24.Add(typeFromHandle47, obj33 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("xf", "Xamarin.Forms");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 70)));
			object obj34 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label4.Text = obj34;
			stackLayout.Children.Add(label4);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "NoWiFiWarning";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase8);
			translate6.Text = "ios_Settings_NoWiFiWarning";
			IMarkupExtension markupExtension25 = translate6;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = labelSwitch;
			array25[1] = stackLayout;
			array25[2] = stackLayout7;
			array25[3] = scrollView;
			array25[4] = this;
			object obj35;
			xamlServiceProvider25.Add(typeFromHandle49, obj35 = new SimpleValueTargetProvider(array25, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("xf", "Xamarin.Forms");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 89)));
			object obj36 = markupExtension25.ProvideValue(xamlServiceProvider25);
			labelSwitch.Text = obj36;
			stackLayout.Children.Add(labelSwitch);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "TryConnectToLastWiFiNetwork";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase9);
			translate7.Text = "ios_TryConnectToLastWiFiNetwork";
			IMarkupExtension markupExtension26 = translate7;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 5];
			array26[0] = labelSwitch2;
			array26[1] = stackLayout;
			array26[2] = stackLayout7;
			array26[3] = scrollView;
			array26[4] = this;
			object obj37;
			xamlServiceProvider26.Add(typeFromHandle51, obj37 = new SimpleValueTargetProvider(array26, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("xf", "Xamarin.Forms");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 103)));
			object obj38 = markupExtension26.ProvideValue(xamlServiceProvider26);
			labelSwitch2.Text = obj38;
			stackLayout.Children.Add(labelSwitch2);
			stackLayout7.Children.Add(stackLayout);
			bindingExtension10.Mode = 2;
			staticResourceExtension6.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = bindingExtension10;
			array27[1] = stackLayout2;
			array27[2] = stackLayout7;
			array27[3] = scrollView;
			array27[4] = this;
			object obj39;
			xamlServiceProvider27.Add(typeFromHandle53, obj39 = new SimpleValueTargetProvider(array27, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("xf", "Xamarin.Forms");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 21)));
			object obj40 = markupExtension27.ProvideValue(xamlServiceProvider27);
			bindingExtension10.Converter = obj40;
			bindingExtension10.Path = "ConnectionType";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label5.SetValue(Label.LineBreakModeProperty, 1);
			translate8.Text = "droid_BTLE_Warning";
			IMarkupExtension markupExtension28 = translate8;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = label5;
			array28[1] = stackLayout2;
			array28[2] = stackLayout7;
			array28[3] = scrollView;
			array28[4] = this;
			object obj41;
			xamlServiceProvider28.Add(typeFromHandle55, obj41 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("xf", "Xamarin.Forms");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 25)));
			object obj42 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label5.Text = obj42;
			label5.SetValue(Label.TextColorProperty, Color.Red);
			on5.Platform = new List<string>(1) { "iOS" };
			on5.Value = "false";
			onPlatform3.Platforms.Add(on5);
			on6.Platform = new List<string>(1) { "Android" };
			on6.Value = "true";
			onPlatform3.Platforms.Add(on6);
			label5.SetValue(VisualElement.IsVisibleProperty, onPlatform3);
			stackLayout2.Children.Add(label5);
			translate9.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension29 = translate9;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 5];
			array29[0] = label6;
			array29[1] = stackLayout2;
			array29[2] = stackLayout7;
			array29[3] = scrollView;
			array29[4] = this;
			object obj43;
			xamlServiceProvider29.Add(typeFromHandle57, obj43 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("xf", "Xamarin.Forms");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 28)));
			object obj44 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label6.Text = obj44;
			stackLayout2.Children.Add(label6);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label7.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "BTLEDeviceName";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label7.SetBinding(Label.TextProperty, bindingBase11);
			stackLayout2.Children.Add(label7);
			button.Clicked += this.btnSelectDevice_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate10.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension30 = translate10;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = button;
			array30[1] = stackLayout2;
			array30[2] = stackLayout7;
			array30[3] = scrollView;
			array30[4] = this;
			object obj45;
			xamlServiceProvider30.Add(typeFromHandle59, obj45 = new SimpleValueTargetProvider(array30, Button.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("xf", "Xamarin.Forms");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 25)));
			object obj46 = markupExtension30.ProvideValue(xamlServiceProvider30);
			button.Text = obj46;
			stackLayout2.Children.Add(button);
			translate11.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension31 = translate11;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = onPlatform4;
			array31[1] = label8;
			array31[2] = stackLayout2;
			array31[3] = stackLayout7;
			array31[4] = scrollView;
			array31[5] = this;
			object obj47;
			xamlServiceProvider31.Add(typeFromHandle61, obj47 = new SimpleValueTargetProvider(array31, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("xf", "Xamarin.Forms");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 33)));
			object obj48 = markupExtension31.ProvideValue(xamlServiceProvider31);
			onPlatform4.Android = obj48;
			translate12.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension32 = translate12;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = onPlatform4;
			array32[1] = label8;
			array32[2] = stackLayout2;
			array32[3] = stackLayout7;
			array32[4] = scrollView;
			array32[5] = this;
			object obj49;
			xamlServiceProvider32.Add(typeFromHandle63, obj49 = new SimpleValueTargetProvider(array32, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("xf", "Xamarin.Forms");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 33)));
			object obj50 = markupExtension32.ProvideValue(xamlServiceProvider32);
			onPlatform4.iOS = obj50;
			label8.SetValue(Label.TextProperty, onPlatform4);
			stackLayout2.Children.Add(label8);
			stackLayout7.Children.Add(stackLayout2);
			bindingExtension12.Mode = 2;
			staticResourceExtension7.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 5];
			array33[0] = bindingExtension12;
			array33[1] = stackLayout3;
			array33[2] = stackLayout7;
			array33[3] = scrollView;
			array33[4] = this;
			object obj51;
			xamlServiceProvider33.Add(typeFromHandle65, obj51 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("xf", "Xamarin.Forms");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 21)));
			object obj52 = markupExtension33.ProvideValue(xamlServiceProvider33);
			bindingExtension12.Converter = obj52;
			bindingExtension12.Path = "ConnectionType";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate13.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension34 = translate13;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 5];
			array34[0] = label9;
			array34[1] = stackLayout3;
			array34[2] = stackLayout7;
			array34[3] = scrollView;
			array34[4] = this;
			object obj53;
			xamlServiceProvider34.Add(typeFromHandle67, obj53 = new SimpleValueTargetProvider(array34, Label.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("xf", "Xamarin.Forms");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 28)));
			object obj54 = markupExtension34.ProvideValue(xamlServiceProvider34);
			label9.Text = obj54;
			stackLayout3.Children.Add(label9);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label10.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "BTDeviceName";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label10.SetBinding(Label.TextProperty, bindingBase13);
			stackLayout3.Children.Add(label10);
			button2.Clicked += this.btnSelectDevice_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate14.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension35 = translate14;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 5];
			array35[0] = button2;
			array35[1] = stackLayout3;
			array35[2] = stackLayout7;
			array35[3] = scrollView;
			array35[4] = this;
			object obj55;
			xamlServiceProvider35.Add(typeFromHandle69, obj55 = new SimpleValueTargetProvider(array35, Button.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("xf", "Xamarin.Forms");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 25)));
			object obj56 = markupExtension35.ProvideValue(xamlServiceProvider35);
			button2.Text = obj56;
			stackLayout3.Children.Add(button2);
			translate15.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension36 = translate15;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 6];
			array36[0] = onPlatform5;
			array36[1] = label11;
			array36[2] = stackLayout3;
			array36[3] = stackLayout7;
			array36[4] = scrollView;
			array36[5] = this;
			object obj57;
			xamlServiceProvider36.Add(typeFromHandle71, obj57 = new SimpleValueTargetProvider(array36, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("xf", "Xamarin.Forms");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 33)));
			object obj58 = markupExtension36.ProvideValue(xamlServiceProvider36);
			onPlatform5.Android = obj58;
			translate16.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension37 = translate16;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 6];
			array37[0] = onPlatform5;
			array37[1] = label11;
			array37[2] = stackLayout3;
			array37[3] = stackLayout7;
			array37[4] = scrollView;
			array37[5] = this;
			object obj59;
			xamlServiceProvider37.Add(typeFromHandle73, obj59 = new SimpleValueTargetProvider(array37, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("xf", "Xamarin.Forms");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 33)));
			object obj60 = markupExtension37.ProvideValue(xamlServiceProvider37);
			onPlatform5.iOS = obj60;
			label11.SetValue(Label.TextProperty, onPlatform5);
			stackLayout3.Children.Add(label11);
			stackLayout7.Children.Add(stackLayout3);
			bindingExtension14.Mode = 2;
			staticResourceExtension8.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension38 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 5];
			array38[0] = bindingExtension14;
			array38[1] = stackLayout4;
			array38[2] = stackLayout7;
			array38[3] = scrollView;
			array38[4] = this;
			object obj61;
			xamlServiceProvider38.Add(typeFromHandle75, obj61 = new SimpleValueTargetProvider(array38, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("xf", "Xamarin.Forms");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 21)));
			object obj62 = markupExtension38.ProvideValue(xamlServiceProvider38);
			bindingExtension14.Converter = obj62;
			bindingExtension14.Path = "ConnectionType";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			translate17.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension39 = translate17;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 5];
			array39[0] = label12;
			array39[1] = stackLayout4;
			array39[2] = stackLayout7;
			array39[3] = scrollView;
			array39[4] = this;
			object obj63;
			xamlServiceProvider39.Add(typeFromHandle77, obj63 = new SimpleValueTargetProvider(array39, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("xf", "Xamarin.Forms");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 28)));
			object obj64 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label12.Text = obj64;
			stackLayout4.Children.Add(label12);
			label13.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label13.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "BTDeviceName";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label13.SetBinding(Label.TextProperty, bindingBase15);
			stackLayout4.Children.Add(label13);
			button3.Clicked += this.btnSelectDevice_Clicked;
			button3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate18.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension40 = translate18;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 5];
			array40[0] = button3;
			array40[1] = stackLayout4;
			array40[2] = stackLayout7;
			array40[3] = scrollView;
			array40[4] = this;
			object obj65;
			xamlServiceProvider40.Add(typeFromHandle79, obj65 = new SimpleValueTargetProvider(array40, Button.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("xf", "Xamarin.Forms");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 25)));
			object obj66 = markupExtension40.ProvideValue(xamlServiceProvider40);
			button3.Text = obj66;
			stackLayout4.Children.Add(button3);
			stackLayout7.Children.Add(stackLayout4);
			linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.btnConnectionGuide_Clicked;
			translate19.Text = "ios_ConnectionGuide";
			IMarkupExtension markupExtension41 = translate19;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = linkButton;
			array41[1] = stackLayout7;
			array41[2] = scrollView;
			array41[3] = this;
			object obj67;
			xamlServiceProvider41.Add(typeFromHandle81, obj67 = new SimpleValueTargetProvider(array41, Button.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("xf", "Xamarin.Forms");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(207, 21)));
			object obj68 = markupExtension41.ProvideValue(xamlServiceProvider41);
			linkButton.Text = obj68;
			stackLayout7.Children.Add(linkButton);
			translate20.Text = "Settings_Control_tbChooseProfile.Text";
			IMarkupExtension markupExtension42 = translate20;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = label14;
			array42[1] = stackLayout7;
			array42[2] = scrollView;
			array42[3] = this;
			object obj69;
			xamlServiceProvider42.Add(typeFromHandle83, obj69 = new SimpleValueTargetProvider(array42, Label.TextProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("xf", "Xamarin.Forms");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(210, 24)));
			object obj70 = markupExtension42.ProvideValue(xamlServiceProvider42);
			label14.Text = obj70;
			stackLayout7.Children.Add(label14);
			label15.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label15.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label15.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "BrandAndProfile";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			label15.SetBinding(Label.TextProperty, bindingBase16);
			stackLayout7.Children.Add(label15);
			button4.Clicked += this.btnProfileSelector_Clicked;
			translate21.Text = "Settings_Control_btnSelectProfile.Content";
			IMarkupExtension markupExtension43 = translate21;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = button4;
			array43[1] = stackLayout7;
			array43[2] = scrollView;
			array43[3] = this;
			object obj71;
			xamlServiceProvider43.Add(typeFromHandle85, obj71 = new SimpleValueTargetProvider(array43, Button.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("xf", "Xamarin.Forms");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(216, 62)));
			object obj72 = markupExtension43.ProvideValue(xamlServiceProvider43);
			button4.Text = obj72;
			stackLayout7.Children.Add(button4);
			translate22.Text = "ios_AdvancedConnectionSettings_Label";
			IMarkupExtension markupExtension44 = translate22;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = label16;
			array44[1] = stackLayout7;
			array44[2] = scrollView;
			array44[3] = this;
			object obj73;
			xamlServiceProvider44.Add(typeFromHandle87, obj73 = new SimpleValueTargetProvider(array44, Label.TextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("xf", "Xamarin.Forms");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(219, 24)));
			object obj74 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label16.Text = obj74;
			stackLayout7.Children.Add(label16);
			button5.Clicked += this.btnAdvancedConnection_Clicked;
			translate23.Text = "ios_AdvancedConnectionSettings";
			IMarkupExtension markupExtension45 = translate23;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 4];
			array45[0] = button5;
			array45[1] = stackLayout7;
			array45[2] = scrollView;
			array45[3] = this;
			object obj75;
			xamlServiceProvider45.Add(typeFromHandle89, obj75 = new SimpleValueTargetProvider(array45, Button.TextProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("xf", "Xamarin.Forms");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(220, 65)));
			object obj76 = markupExtension45.ProvideValue(xamlServiceProvider45);
			button5.Text = obj76;
			stackLayout7.Children.Add(button5);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			onPlatform6.Android = true;
			onPlatform6.Default = false;
			onPlatform6.iOS = false;
			stackLayout5.SetValue(VisualElement.IsVisibleProperty, onPlatform6);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "AndroidStartBackgroundService";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase17);
			translate24.Text = "droid_StartBackgroundService";
			IMarkupExtension markupExtension46 = translate24;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 6];
			array46[0] = labelSwitch3;
			array46[1] = stackLayout5;
			array46[2] = stackLayout6;
			array46[3] = stackLayout7;
			array46[4] = scrollView;
			array46[5] = this;
			object obj77;
			xamlServiceProvider46.Add(typeFromHandle91, obj77 = new SimpleValueTargetProvider(array46, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("xf", "Xamarin.Forms");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(234, 109)));
			object obj78 = markupExtension46.ProvideValue(xamlServiceProvider46);
			labelSwitch3.Text = obj78;
			stackLayout5.Children.Add(labelSwitch3);
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "DroidDisplayConnectionStatusInStatusBar";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase18);
			bindingExtension19.Path = "AndroidStartBackgroundService";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			labelSwitch4.SetBinding(VisualElement.IsVisibleProperty, bindingBase19);
			translate25.Text = "DroidDisplayConnectionStatusInStatusBar";
			IMarkupExtension markupExtension47 = translate25;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 6];
			array47[0] = labelSwitch4;
			array47[1] = stackLayout5;
			array47[2] = stackLayout6;
			array47[3] = stackLayout7;
			array47[4] = scrollView;
			array47[5] = this;
			object obj79;
			xamlServiceProvider47.Add(typeFromHandle93, obj79 = new SimpleValueTargetProvider(array47, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver47.Add("xf", "Xamarin.Forms");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(239, 29)));
			object obj80 = markupExtension47.ProvideValue(xamlServiceProvider47);
			labelSwitch4.Text = obj80;
			stackLayout5.Children.Add(labelSwitch4);
			button6.Clicked += this.BtnDroidBackgroundTrobleshooting_Clicked;
			translate26.Text = "droid_BackgroundTroubleshooting";
			IMarkupExtension markupExtension48 = translate26;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 6];
			array48[0] = button6;
			array48[1] = stackLayout5;
			array48[2] = stackLayout6;
			array48[3] = stackLayout7;
			array48[4] = scrollView;
			array48[5] = this;
			object obj81;
			xamlServiceProvider48.Add(typeFromHandle95, obj81 = new SimpleValueTargetProvider(array48, Button.TextProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver48.Add("xf", "Xamarin.Forms");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(243, 29)));
			object obj82 = markupExtension48.ProvideValue(xamlServiceProvider48);
			button6.Text = obj82;
			on7.Platform = new List<string>(1) { "iOS" };
			on7.Value = "False";
			onPlatform7.Platforms.Add(on7);
			on8.Platform = new List<string>(1) { "Android" };
			on8.Value = "True";
			onPlatform7.Platforms.Add(on8);
			button6.SetValue(VisualElement.IsVisibleProperty, onPlatform7);
			stackLayout5.Children.Add(button6);
			stackLayout6.Children.Add(stackLayout5);
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "ConnectOnLaunch";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			labelSwitch5.SetBinding(LabelSwitch.IsToggledProperty, bindingBase20);
			translate27.Text = "Settings_Control_ConnectOnLaunch.Header";
			IMarkupExtension markupExtension49 = translate27;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 5];
			array49[0] = labelSwitch5;
			array49[1] = stackLayout6;
			array49[2] = stackLayout7;
			array49[3] = scrollView;
			array49[4] = this;
			object obj83;
			xamlServiceProvider49.Add(typeFromHandle97, obj83 = new SimpleValueTargetProvider(array49, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver49.Add("xf", "Xamarin.Forms");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(257, 91)));
			object obj84 = markupExtension49.ProvideValue(xamlServiceProvider49);
			labelSwitch5.Text = obj84;
			stackLayout6.Children.Add(labelSwitch5);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "OpenDashboardOnLaunch";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			labelSwitch6.SetBinding(LabelSwitch.IsToggledProperty, bindingBase21);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "ConnectOnLaunch";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			labelSwitch6.SetBinding(VisualElement.IsVisibleProperty, bindingBase22);
			translate28.Text = "OpenDashboardOnLaunch";
			IMarkupExtension markupExtension50 = translate28;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 5];
			array50[0] = labelSwitch6;
			array50[1] = stackLayout6;
			array50[2] = stackLayout7;
			array50[3] = scrollView;
			array50[4] = this;
			object obj85;
			xamlServiceProvider50.Add(typeFromHandle99, obj85 = new SimpleValueTargetProvider(array50, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver50.Add("xf", "Xamarin.Forms");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 25)));
			object obj86 = markupExtension50.ProvideValue(xamlServiceProvider50);
			labelSwitch6.Text = obj86;
			stackLayout6.Children.Add(labelSwitch6);
			button7.Clicked += this.btnExportLog_Clicked;
			translate29.Text = "ios_ExportLog";
			IMarkupExtension markupExtension51 = translate29;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 5];
			array51[0] = button7;
			array51[1] = stackLayout6;
			array51[2] = stackLayout7;
			array51[3] = scrollView;
			array51[4] = this;
			object obj87;
			xamlServiceProvider51.Add(typeFromHandle101, obj87 = new SimpleValueTargetProvider(array51, Button.TextProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver51.Add("xf", "Xamarin.Forms");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsConnectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(274, 25)));
			object obj88 = markupExtension51.ProvideValue(xamlServiceProvider51);
			button7.Text = obj88;
			stackLayout6.Children.Add(button7);
			stackLayout7.Children.Add(stackLayout6);
			scrollView.Content = stackLayout7;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x060018F8 RID: 6392 RVA: 0x000E913C File Offset: 0x000E733C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsConnectionPage>(this, typeof(SettingsConnectionPage));
			this.LayoutRoot = NameScopeExtensions.FindByName<StackLayout>(this, "LayoutRoot");
			this.connectionTypeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "connectionTypeGroup");
			this.btnWiFi = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnWiFi");
			this.btnBluetoothLE = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnBluetoothLE");
			this.btnBluetooth = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnBluetooth");
			this.btnMFI = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnMFI");
			this.wifiPanel = NameScopeExtensions.FindByName<StackLayout>(this, "wifiPanel");
			this.bluetoothLEPanel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetoothLEPanel");
			this.bluetooth2Panel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetooth2Panel");
			this.bluetoothMFIPanel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetoothMFIPanel");
			this.switchDisplayStatusInNotification = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchDisplayStatusInNotification");
			this.btnDroidBackgroundTrobleshooting = NameScopeExtensions.FindByName<Button>(this, "btnDroidBackgroundTrobleshooting");
			this.btnShareLog = NameScopeExtensions.FindByName<Button>(this, "btnShareLog");
		}

		// Token: 0x04000A98 RID: 2712
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout LayoutRoot;

		// Token: 0x04000A99 RID: 2713
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup connectionTypeGroup;

		// Token: 0x04000A9A RID: 2714
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnWiFi;

		// Token: 0x04000A9B RID: 2715
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnBluetoothLE;

		// Token: 0x04000A9C RID: 2716
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnBluetooth;

		// Token: 0x04000A9D RID: 2717
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnMFI;

		// Token: 0x04000A9E RID: 2718
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout wifiPanel;

		// Token: 0x04000A9F RID: 2719
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetoothLEPanel;

		// Token: 0x04000AA0 RID: 2720
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetooth2Panel;

		// Token: 0x04000AA1 RID: 2721
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetoothMFIPanel;

		// Token: 0x04000AA2 RID: 2722
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchDisplayStatusInNotification;

		// Token: 0x04000AA3 RID: 2723
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDroidBackgroundTrobleshooting;

		// Token: 0x04000AA4 RID: 2724
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnShareLog;

		// Token: 0x020001CE RID: 462
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnClearLog_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x060018F9 RID: 6393 RVA: 0x000E9238 File Offset: 0x000E7438
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionPage settingsConnectionPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
						{
							PCLDebugStream.CurrentInstance.Clear();
							goto IL_0095;
						}
						taskAwaiter = settingsConnectionPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnClearLog_Clicked>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x060018FA RID: 6394 RVA: 0x000E9318 File Offset: 0x000E7518
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AA5 RID: 2725
			public int <>1__state;

			// Token: 0x04000AA6 RID: 2726
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AA7 RID: 2727
			public SettingsConnectionPage <>4__this;

			// Token: 0x04000AA8 RID: 2728
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001CF RID: 463
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnExportLog_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x060018FB RID: 6395 RVA: 0x000E9328 File Offset: 0x000E7528
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionPage settingsConnectionPage = this;
				try
				{
					if (num != 0)
					{
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							goto IL_00B8;
						}
						try
						{
							PCLDebugStream.CurrentInstance.Close();
						}
						catch (Exception)
						{
						}
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(PCLDebugStream.GetFilepath()))).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnExportLog_Clicked>d__5>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
						goto IL_00D3;
					}
					catch (Exception ex)
					{
						settingsConnectionPage.DisplayAlert("Error", ex.Message, "OK");
						goto IL_00D3;
					}
					IL_00B8:
					settingsConnectionPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK");
					IL_00D3:;
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

			// Token: 0x060018FC RID: 6396 RVA: 0x000E9460 File Offset: 0x000E7660
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AA9 RID: 2729
			public int <>1__state;

			// Token: 0x04000AAA RID: 2730
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AAB RID: 2731
			public SettingsConnectionPage <>4__this;

			// Token: 0x04000AAC RID: 2732
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001D0 RID: 464
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectDevice_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x060018FD RID: 6397 RVA: 0x000E9470 File Offset: 0x000E7670
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsConnectionPage settingsConnectionPage = this;
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
							taskAwaiter = settingsConnectionPage.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnSelectDevice_Clicked>d__4>(ref taskAwaiter, ref this);
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
							taskAwaiter = settingsConnectionPage.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnSelectDevice_Clicked>d__4>(ref taskAwaiter, ref this);
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
							taskAwaiter = settingsConnectionPage.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnSelectDevice_Clicked>d__4>(ref taskAwaiter, ref this);
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
					taskAwaiter = settingsConnectionPage.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsConnectionPage.<btnSelectDevice_Clicked>d__4>(ref taskAwaiter, ref this);
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

			// Token: 0x060018FE RID: 6398 RVA: 0x000E9714 File Offset: 0x000E7914
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000AAD RID: 2733
			public int <>1__state;

			// Token: 0x04000AAE RID: 2734
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000AAF RID: 2735
			public SettingsConnectionPage <>4__this;

			// Token: 0x04000AB0 RID: 2736
			private TaskAwaiter <>u__1;
		}
	}
}
