using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200067C RID: 1660
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\WelcomePage2V3.xaml")]
	public class WelcomePage2V3 : ContentPage
	{
		// Token: 0x060038FA RID: 14586 RVA: 0x002C067E File Offset: 0x002BE87E
		public WelcomePage2V3()
		{
			this.InitializeComponent();
			this.InitializePermissionRequestAsync();
		}

		// Token: 0x060038FB RID: 14587 RVA: 0x002C0694 File Offset: 0x002BE894
		private async ValueTask InitializePermissionRequestAsync()
		{
			if (PlatformHelper.IsAndroid)
			{
				bool flag = PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0);
				if (flag)
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					flag = taskAwaiter.GetResult() != 3;
				}
				if (flag)
				{
					this.gridDroidPermissions.DisplayGPS = false;
					this.gridDroidPermissions.DisplayBluetooth = true;
					this.gridDroidPermissions.IsVisible = true;
					this.gridMain.IsVisible = false;
				}
				else if (PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0) && !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					this.gridDroidPermissions.DisplayBluetooth = false;
					this.gridDroidPermissions.DisplayGPS = true;
					this.gridDroidPermissions.IsVisible = true;
					this.gridMain.IsVisible = false;
				}
			}
		}

		// Token: 0x060038FC RID: 14588 RVA: 0x002C06D7 File Offset: 0x002BE8D7
		private void WelcomePage2V3_Appearing(object sender, EventArgs e)
		{
			this.gridDroidPermissions.UpdateCurrentStatus();
		}

		// Token: 0x060038FD RID: 14589 RVA: 0x002C06E8 File Offset: 0x002BE8E8
		private async Task FinishAndConnect()
		{
			SharedSettings.Current.FirstConnectionAttempted = true;
			await base.Navigation.PopAsync();
			await SimpleMainPage.Instance.StartConnection(true);
		}

		// Token: 0x060038FE RID: 14590 RVA: 0x002C072B File Offset: 0x002BE92B
		private void gridDroidPermissions_NextClicked(object sender, EventArgs e)
		{
			this.gridMain.IsVisible = true;
			this.gridDroidPermissions.IsVisible = false;
			this.DroidConnectionSettingsDetector();
		}

		// Token: 0x060038FF RID: 14591 RVA: 0x002C074C File Offset: 0x002BE94C
		private async Task DroidConnectionSettingsDetector()
		{
			if (PlatformHelper.IsAndroid)
			{
				ConnectionDetector connectionDetector = new ConnectionDetector();
				connectionDetector.FinishedWithSuccess += delegate(object d, EventArgs args)
				{
					SharedSettings.Current.FirstConnectionAttempted = false;
				};
				await connectionDetector.DetectAndSetSettings();
			}
		}

		// Token: 0x06003900 RID: 14592 RVA: 0x002C0788 File Offset: 0x002BE988
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
							PlatformHelper.IOSService.OpenSystemSettings();
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

		// Token: 0x06003901 RID: 14593 RVA: 0x002C07C0 File Offset: 0x002BE9C0
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

		// Token: 0x06003902 RID: 14594 RVA: 0x002C0810 File Offset: 0x002BEA10
		private void btnConnectionGuide_Clicked(object sender, EventArgs e)
		{
			switch (SharedSettings.Current.ConnectionType)
			{
			case ConnectionTypes.WiFi:
				Launcher.TryOpenAsync("https://www.carscanner.info/wifi/");
				return;
			case ConnectionTypes.BluetoothLE:
				Launcher.TryOpenAsync("https://www.carscanner.info/ios-bt4/");
				return;
			case ConnectionTypes.Bluetooth:
				Launcher.TryOpenAsync("https://www.carscanner.info/android-bluetooth/");
				return;
			default:
				return;
			}
		}

		// Token: 0x06003903 RID: 14595 RVA: 0x002C0860 File Offset: 0x002BEA60
		private async void btnNext_Clicked(object sender, EventArgs e)
		{
			if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
			{
				await base.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK");
				this.btnSelectDevice_Clicked(sender, e);
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
			{
				await base.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK");
				this.btnSelectDevice_Clicked(sender, e);
			}
			else if (string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
			{
				await base.DisplayAlert(Translate.GetString("welcome_ChooseConnectionProfileFirst_Title"), Translate.GetString("welcome_ChooseConnectionProfileFirst_Text"), "OK");
				this.settingsLayoutRoot.ScrollToBottom = true;
			}
			else
			{
				this.FinishAndConnect();
			}
		}

		// Token: 0x06003904 RID: 14596 RVA: 0x002C08A8 File Offset: 0x002BEAA8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(WelcomePage2V3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/WelcomePage2V3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			ConnectionTypeToWiFiVisibleBoolConverter connectionTypeToWiFiVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverter = new ConnectionTypeToWiFiVisibleBoolConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ConnectionTypeToBTLEVisibleBoolConverter connectionTypeToBTLEVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTLEVisibleBoolConverter = new ConnectionTypeToBTLEVisibleBoolConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ConnectionTypeToBTVisibleBoolConverter connectionTypeToBTVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverter = new ConnectionTypeToBTVisibleBoolConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ConnectionTypeToMFIBluetoothConverter connectionTypeToMFIBluetoothConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToMFIBluetoothConverter = new ConnectionTypeToMFIBluetoothConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ConnectionTypeToIntConverter connectionTypeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToIntConverter = new ConnectionTypeToIntConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			EmptyConverterParameterToNotSelectedStringConverter emptyConverterParameterToNotSelectedStringConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyConverterParameterToNotSelectedStringConverter = new EmptyConverterParameterToNotSelectedStringConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			EmptyStringToConvertererParameterConverter emptyStringToConvertererParameterConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToConvertererParameterConverter = new EmptyStringToConvertererParameterConverter(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			PermissionsRequestControl permissionsRequestControl;
			VisualDiagnostics.RegisterSourceInfo(permissionsRequestControl = new PermissionsRequestControl(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			ConnectionTypes connectionTypes = ConnectionTypes.WiFi;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 26);
			ConnectionTypes connectionTypes2 = ConnectionTypes.Bluetooth;
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 38);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 38);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 34);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 26);
			ConnectionTypes connectionTypes3 = ConnectionTypes.BluetoothLE;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			ConnectionTypes connectionTypes4 = ConnectionTypes.MFI_OBDLinkMXPlus;
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 38);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 38);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 34);
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 26);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 29);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 29);
			EntryCell entryCell;
			VisualDiagnostics.RegisterSourceInfo(entryCell = new EntryCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 26);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 29);
			EntryCell entryCell2;
			VisualDiagnostics.RegisterSourceInfo(entryCell2 = new EntryCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 26);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 25);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 25);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 33);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 33);
			OnPlatform<string> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<string>(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 30);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 29);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 29);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 47);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 38);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 38);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 34);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 25);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 33);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			OnPlatform<string> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<string>(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 30);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 29);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 47);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 38);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 38);
			MultiBinding multiBinding2;
			VisualDiagnostics.RegisterSourceInfo(multiBinding2 = new MultiBinding(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 34);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 26);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 22);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 25);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 29);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 29);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 47);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 38);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 38);
			MultiBinding multiBinding3;
			VisualDiagnostics.RegisterSourceInfo(multiBinding3 = new MultiBinding(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 34);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 26);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 33);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 37);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 37);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 34);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 37);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 37);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 55);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 46);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 46);
			MultiBinding multiBinding4;
			VisualDiagnostics.RegisterSourceInfo(multiBinding4 = new MultiBinding(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 42);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 34);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 37);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 37);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 34);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 30);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 26);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 22);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 21);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\WelcomePage2V3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("gridDroidPermissions", permissionsRequestControl);
			if (permissionsRequestControl.StyleId == null)
			{
				permissionsRequestControl.StyleId = "gridDroidPermissions";
			}
			nameScope.RegisterName("gridMain", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridMain";
			}
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
			this.LayoutRoot = grid2;
			this.gridDroidPermissions = permissionsRequestControl;
			this.gridMain = grid;
			this.settingsLayoutRoot = settingsView;
			this.connectionTypeSection = section;
			this.wifiPanel = section2;
			this.bluetoothLEPanel = section3;
			this.bluetooth2Panel = section4;
			this.bluetoothMFIPanel = section5;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ConnectionTypeToWiFiVisibleBoolConverter", connectionTypeToWiFiVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTLEVisibleBoolConverter", connectionTypeToBTLEVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTVisibleBoolConverter", connectionTypeToBTVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToMFIBluetoothConverter", connectionTypeToMFIBluetoothConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("ConnectionTypeToIntConverter", connectionTypeToIntConverter);
			resourceDictionary.Add("EmptyConverterParameterToNotSelectedStringConverter", emptyConverterParameterToNotSelectedStringConverter);
			resourceDictionary.Add("EmptyStringToConvertererParameterConverter", emptyStringToConvertererParameterConverter);
			translate.Text = "Settings_Control_tbConnection.Text";
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
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.WelcomePage2V3_Appearing;
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
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			permissionsRequestControl.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			permissionsRequestControl.NextClicked += this.gridDroidPermissions_NextClicked;
			grid2.Children.Add(permissionsRequestControl);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbConnectionType.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = section;
			array3[1] = settingsView;
			array3[2] = grid;
			array3[3] = grid2;
			array3[4] = this;
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
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			bindingExtension.Path = "ConnectionType";
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
			radioCell4.SetValue(CellBase.DescriptionProperty, "OBDLink MX+, vLinker FS");
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
			object[] array4 = new object[0 + 5];
			array4[0] = section2;
			array4[1] = settingsView;
			array4[2] = grid;
			array4[3] = grid2;
			array4[4] = this;
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
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 25)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			section2.FooterText = obj7;
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension2;
			array5[1] = section2;
			array5[2] = settingsView;
			array5[3] = grid;
			array5[4] = grid2;
			array5[5] = this;
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
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 25)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension2.Converter = obj9;
			bindingExtension2.Path = "ConnectionType";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase2);
			translate4.Text = "Settings_Control_tbWiFiServer.Text";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = entryCell;
			array6[1] = section2;
			array6[2] = settingsView;
			array6[3] = grid;
			array6[4] = grid2;
			array6[5] = this;
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
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 29)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			entryCell.Title = obj11;
			entryCell.SetValue(EntryCell.PlaceholderProperty, "192.168.0.10");
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "WiFiServer";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			entryCell.SetBinding(EntryCell.ValueTextProperty, bindingBase3);
			staticResourceExtension2.Key = "BaseFontSize++";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = entryCell;
			array7[1] = section2;
			array7[2] = settingsView;
			array7[3] = grid;
			array7[4] = grid2;
			array7[5] = this;
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
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 29)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entryCell.ValueTextFontSize = (double)obj13;
			section2.Add(entryCell);
			translate5.Text = "Settings_Control_tbWiFiPort.Text";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = entryCell2;
			array8[1] = section2;
			array8[2] = settingsView;
			array8[3] = grid;
			array8[4] = grid2;
			array8[5] = this;
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
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 29)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			entryCell2.Title = obj15;
			entryCell2.SetValue(EntryCell.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			entryCell2.SetValue(EntryCell.MaxLengthProperty, 5);
			entryCell2.SetValue(EntryCell.PlaceholderProperty, "35000");
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "WiFiPort";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entryCell2.SetBinding(EntryCell.ValueTextProperty, bindingBase4);
			section2.Add(entryCell2);
			settingsView.Root.Add(section2);
			translate6.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = section3;
			array9[1] = settingsView;
			array9[2] = grid;
			array9[3] = grid2;
			array9[4] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, SectionBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 25)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			section3.Title = obj17;
			bindingExtension5.Mode = 2;
			staticResourceExtension3.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension5;
			array10[1] = section3;
			array10[2] = settingsView;
			array10[3] = grid;
			array10[4] = grid2;
			array10[5] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 25)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension5.Converter = obj19;
			bindingExtension5.Path = "ConnectionType";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase5);
			translate7.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = onPlatform3;
			array11[1] = section3;
			array11[2] = settingsView;
			array11[3] = grid;
			array11[4] = grid2;
			array11[5] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array11, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
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
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 33)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			onPlatform3.Android = obj21;
			translate8.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = onPlatform3;
			array12[1] = section3;
			array12[2] = settingsView;
			array12[3] = grid;
			array12[4] = grid2;
			array12[5] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
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
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 33)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			onPlatform3.iOS = obj23;
			section3.SetValue(Section.FooterTextProperty, onPlatform3);
			translate9.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension13 = translate9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = labelCell;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = grid;
			array13[4] = grid2;
			array13[5] = this;
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
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 29)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			labelCell.Title = obj25;
			labelCell.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = labelCell;
			array14[1] = section3;
			array14[2] = settingsView;
			array14[3] = grid;
			array14[4] = grid2;
			array14[5] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle27, obj26 = new SimpleValueTargetProvider(array14, LabelCell.ValueTextColorProperty, nameScope));
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
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 29)));
			DynamicResource dynamicResource2 = markupExtension14.ProvideValue(xamlServiceProvider14);
			labelCell.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource2.Key);
			staticResourceExtension4.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension15 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = multiBinding;
			array15[1] = labelCell;
			array15[2] = section3;
			array15[3] = settingsView;
			array15[4] = grid;
			array15[5] = grid2;
			array15[6] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 47)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			multiBinding.Converter = obj28;
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "BTLEDeviceName";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase6);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "BTLEDeviceID";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase7);
			labelCell.SetBinding(LabelCell.ValueTextProperty, multiBinding);
			section3.Add(labelCell);
			settingsView.Root.Add(section3);
			translate10.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension16 = translate10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = section4;
			array16[1] = settingsView;
			array16[2] = grid;
			array16[3] = grid2;
			array16[4] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array16, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 25)));
			object obj30 = markupExtension16.ProvideValue(xamlServiceProvider16);
			section4.Title = obj30;
			bindingExtension8.Mode = 2;
			staticResourceExtension5.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension17 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = bindingExtension8;
			array17[1] = section4;
			array17[2] = settingsView;
			array17[3] = grid;
			array17[4] = grid2;
			array17[5] = this;
			object obj31;
			xamlServiceProvider17.Add(typeFromHandle33, obj31 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 25)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension8.Converter = obj32;
			bindingExtension8.Path = "ConnectionType";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase8);
			translate11.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension18 = translate11;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = onPlatform4;
			array18[1] = section4;
			array18[2] = settingsView;
			array18[3] = grid;
			array18[4] = grid2;
			array18[5] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle35, obj33 = new SimpleValueTargetProvider(array18, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 33)));
			object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
			onPlatform4.Android = obj34;
			translate12.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension19 = translate12;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 6];
			array19[0] = onPlatform4;
			array19[1] = section4;
			array19[2] = settingsView;
			array19[3] = grid;
			array19[4] = grid2;
			array19[5] = this;
			object obj35;
			xamlServiceProvider19.Add(typeFromHandle37, obj35 = new SimpleValueTargetProvider(array19, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 33)));
			object obj36 = markupExtension19.ProvideValue(xamlServiceProvider19);
			onPlatform4.iOS = obj36;
			section4.SetValue(Section.FooterTextProperty, onPlatform4);
			translate13.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension20 = translate13;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = labelCell2;
			array20[1] = section4;
			array20[2] = settingsView;
			array20[3] = grid;
			array20[4] = grid2;
			array20[5] = this;
			object obj37;
			xamlServiceProvider20.Add(typeFromHandle39, obj37 = new SimpleValueTargetProvider(array20, CellBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 29)));
			object obj38 = markupExtension20.ProvideValue(xamlServiceProvider20);
			labelCell2.Title = obj38;
			labelCell2.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = labelCell2;
			array21[1] = section4;
			array21[2] = settingsView;
			array21[3] = grid;
			array21[4] = grid2;
			array21[5] = this;
			object obj39;
			xamlServiceProvider21.Add(typeFromHandle41, obj39 = new SimpleValueTargetProvider(array21, LabelCell.ValueTextColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 29)));
			DynamicResource dynamicResource3 = markupExtension21.ProvideValue(xamlServiceProvider21);
			labelCell2.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource3.Key);
			staticResourceExtension6.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension22 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = multiBinding2;
			array22[1] = labelCell2;
			array22[2] = section4;
			array22[3] = settingsView;
			array22[4] = grid;
			array22[5] = grid2;
			array22[6] = this;
			object obj40;
			xamlServiceProvider22.Add(typeFromHandle43, obj40 = new SimpleValueTargetProvider(array22, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 47)));
			object obj41 = markupExtension22.ProvideValue(xamlServiceProvider22);
			multiBinding2.Converter = obj41;
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "BTDeviceName";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase9);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "BTDeviceID";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase10);
			labelCell2.SetBinding(LabelCell.ValueTextProperty, multiBinding2);
			section4.Add(labelCell2);
			settingsView.Root.Add(section4);
			translate14.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension23 = translate14;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = section5;
			array23[1] = settingsView;
			array23[2] = grid;
			array23[3] = grid2;
			array23[4] = this;
			object obj42;
			xamlServiceProvider23.Add(typeFromHandle45, obj42 = new SimpleValueTargetProvider(array23, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 25)));
			object obj43 = markupExtension23.ProvideValue(xamlServiceProvider23);
			section5.Title = obj43;
			bindingExtension11.Mode = 2;
			staticResourceExtension7.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension24 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = bindingExtension11;
			array24[1] = section5;
			array24[2] = settingsView;
			array24[3] = grid;
			array24[4] = grid2;
			array24[5] = this;
			object obj44;
			xamlServiceProvider24.Add(typeFromHandle47, obj44 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 25)));
			object obj45 = markupExtension24.ProvideValue(xamlServiceProvider24);
			bindingExtension11.Converter = obj45;
			bindingExtension11.Path = "ConnectionType";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase11);
			translate15.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension25 = translate15;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = labelCell3;
			array25[1] = section5;
			array25[2] = settingsView;
			array25[3] = grid;
			array25[4] = grid2;
			array25[5] = this;
			object obj46;
			xamlServiceProvider25.Add(typeFromHandle49, obj46 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 29)));
			object obj47 = markupExtension25.ProvideValue(xamlServiceProvider25);
			labelCell3.Title = obj47;
			labelCell3.Tapped += this.btnSelectDevice_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = labelCell3;
			array26[1] = section5;
			array26[2] = settingsView;
			array26[3] = grid;
			array26[4] = grid2;
			array26[5] = this;
			object obj48;
			xamlServiceProvider26.Add(typeFromHandle51, obj48 = new SimpleValueTargetProvider(array26, LabelCell.ValueTextColorProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 29)));
			DynamicResource dynamicResource4 = markupExtension26.ProvideValue(xamlServiceProvider26);
			labelCell3.SetDynamicResource(LabelCell.ValueTextColorProperty, dynamicResource4.Key);
			staticResourceExtension8.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 7];
			array27[0] = multiBinding3;
			array27[1] = labelCell3;
			array27[2] = section5;
			array27[3] = settingsView;
			array27[4] = grid;
			array27[5] = grid2;
			array27[6] = this;
			object obj49;
			xamlServiceProvider27.Add(typeFromHandle53, obj49 = new SimpleValueTargetProvider(array27, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 47)));
			object obj50 = markupExtension27.ProvideValue(xamlServiceProvider27);
			multiBinding3.Converter = obj50;
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "BTDeviceName";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			multiBinding3.Bindings.Add(bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "BTDeviceID";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			multiBinding3.Bindings.Add(bindingBase13);
			labelCell3.SetBinding(LabelCell.ValueTextProperty, multiBinding3);
			section5.Add(labelCell3);
			settingsView.Root.Add(section5);
			translate16.Text = "settings_ConnectionProfile";
			IMarkupExtension markupExtension28 = translate16;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = section6;
			array28[1] = settingsView;
			array28[2] = grid;
			array28[3] = grid2;
			array28[4] = this;
			object obj51;
			xamlServiceProvider28.Add(typeFromHandle55, obj51 = new SimpleValueTargetProvider(array28, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 33)));
			object obj52 = markupExtension28.ProvideValue(xamlServiceProvider28);
			section6.Title = obj52;
			customCell.Tapped += this.btnProfileSelector_Clicked;
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			staticResourceExtension9.Key = "BaseFontSize";
			IMarkupExtension markupExtension29 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 8];
			array29[0] = label;
			array29[1] = stackLayout;
			array29[2] = customCell;
			array29[3] = section6;
			array29[4] = settingsView;
			array29[5] = grid;
			array29[6] = grid2;
			array29[7] = this;
			object obj53;
			xamlServiceProvider29.Add(typeFromHandle57, obj53 = new SimpleValueTargetProvider(array29, Label.FontSizeProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 37)));
			object obj54 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label.FontSize = (double)obj54;
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate17.Text = "Settings_Control_tbChooseProfile.Text";
			IMarkupExtension markupExtension30 = translate17;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 8];
			array30[0] = label;
			array30[1] = stackLayout;
			array30[2] = customCell;
			array30[3] = section6;
			array30[4] = settingsView;
			array30[5] = grid;
			array30[6] = grid2;
			array30[7] = this;
			object obj55;
			xamlServiceProvider30.Add(typeFromHandle59, obj55 = new SimpleValueTargetProvider(array30, Label.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 37)));
			object obj56 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label.Text = obj56;
			stackLayout.Children.Add(label);
			staticResourceExtension10.Key = "BaseFontSize++";
			IMarkupExtension markupExtension31 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 8];
			array31[0] = label2;
			array31[1] = stackLayout;
			array31[2] = customCell;
			array31[3] = section6;
			array31[4] = settingsView;
			array31[5] = grid;
			array31[6] = grid2;
			array31[7] = this;
			object obj57;
			xamlServiceProvider31.Add(typeFromHandle61, obj57 = new SimpleValueTargetProvider(array31, Label.FontSizeProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 37)));
			object obj58 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label2.FontSize = (double)obj58;
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension32 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 8];
			array32[0] = label2;
			array32[1] = stackLayout;
			array32[2] = customCell;
			array32[3] = section6;
			array32[4] = settingsView;
			array32[5] = grid;
			array32[6] = grid2;
			array32[7] = this;
			object obj59;
			xamlServiceProvider32.Add(typeFromHandle63, obj59 = new SimpleValueTargetProvider(array32, Label.TextColorProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 37)));
			DynamicResource dynamicResource5 = markupExtension32.ProvideValue(xamlServiceProvider32);
			label2.SetDynamicResource(Label.TextColorProperty, dynamicResource5.Key);
			staticResourceExtension11.Key = "EmptyConverterParameterToNotSelectedStringConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 9];
			array33[0] = multiBinding4;
			array33[1] = label2;
			array33[2] = stackLayout;
			array33[3] = customCell;
			array33[4] = section6;
			array33[5] = settingsView;
			array33[6] = grid;
			array33[7] = grid2;
			array33[8] = this;
			object obj60;
			xamlServiceProvider33.Add(typeFromHandle65, obj60 = new SimpleValueTargetProvider(array33, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(177, 55)));
			object obj61 = markupExtension33.ProvideValue(xamlServiceProvider33);
			multiBinding4.Converter = obj61;
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "BrandAndProfile";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			multiBinding4.Bindings.Add(bindingBase14);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "SelectedProfileV2Name";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			multiBinding4.Bindings.Add(bindingBase15);
			label2.SetBinding(Label.TextProperty, multiBinding4);
			stackLayout.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension12.Key = "BaseFontSize";
			IMarkupExtension markupExtension34 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 8];
			array34[0] = label3;
			array34[1] = stackLayout;
			array34[2] = customCell;
			array34[3] = section6;
			array34[4] = settingsView;
			array34[5] = grid;
			array34[6] = grid2;
			array34[7] = this;
			object obj62;
			xamlServiceProvider34.Add(typeFromHandle67, obj62 = new SimpleValueTargetProvider(array34, Label.FontSizeProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(185, 37)));
			object obj63 = markupExtension34.ProvideValue(xamlServiceProvider34);
			label3.FontSize = (double)obj63;
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate18.Text = "settings_TapToSelectConnectionProfile";
			IMarkupExtension markupExtension35 = translate18;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 8];
			array35[0] = label3;
			array35[1] = stackLayout;
			array35[2] = customCell;
			array35[3] = section6;
			array35[4] = settingsView;
			array35[5] = grid;
			array35[6] = grid2;
			array35[7] = this;
			object obj64;
			xamlServiceProvider35.Add(typeFromHandle69, obj64 = new SimpleValueTargetProvider(array35, Label.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 37)));
			object obj65 = markupExtension35.ProvideValue(xamlServiceProvider35);
			label3.Text = obj65;
			stackLayout.Children.Add(label3);
			customCell.SetValue(CustomCell.ContentProperty, stackLayout);
			section6.Add(customCell);
			settingsView.Root.Add(section6);
			grid.Children.Add(settingsView);
			button.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension6.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension36 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = button;
			array36[1] = grid;
			array36[2] = grid2;
			array36[3] = this;
			object obj66;
			xamlServiceProvider36.Add(typeFromHandle71, obj66 = new SimpleValueTargetProvider(array36, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(220, 21)));
			DynamicResource dynamicResource6 = markupExtension36.ProvideValue(xamlServiceProvider36);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			button.Clicked += this.btnNext_Clicked;
			translate19.Text = "ios_NEXT";
			IMarkupExtension markupExtension37 = translate19;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = button;
			array37[1] = grid;
			array37[2] = grid2;
			array37[3] = this;
			object obj67;
			xamlServiceProvider37.Add(typeFromHandle73, obj67 = new SimpleValueTargetProvider(array37, Button.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(WelcomePage2V3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 21)));
			object obj68 = markupExtension37.ProvideValue(xamlServiceProvider37);
			button.Text = obj68;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			grid2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06003905 RID: 14597 RVA: 0x002C55DC File Offset: 0x002C37DC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<WelcomePage2V3>(this, typeof(WelcomePage2V3));
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.gridDroidPermissions = NameScopeExtensions.FindByName<PermissionsRequestControl>(this, "gridDroidPermissions");
			this.gridMain = NameScopeExtensions.FindByName<Grid>(this, "gridMain");
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.connectionTypeSection = NameScopeExtensions.FindByName<Section>(this, "connectionTypeSection");
			this.wifiPanel = NameScopeExtensions.FindByName<Section>(this, "wifiPanel");
			this.bluetoothLEPanel = NameScopeExtensions.FindByName<Section>(this, "bluetoothLEPanel");
			this.bluetooth2Panel = NameScopeExtensions.FindByName<Section>(this, "bluetooth2Panel");
			this.bluetoothMFIPanel = NameScopeExtensions.FindByName<Section>(this, "bluetoothMFIPanel");
		}

		// Token: 0x040022A3 RID: 8867
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040022A4 RID: 8868
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private PermissionsRequestControl gridDroidPermissions;

		// Token: 0x040022A5 RID: 8869
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridMain;

		// Token: 0x040022A6 RID: 8870
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x040022A7 RID: 8871
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section connectionTypeSection;

		// Token: 0x040022A8 RID: 8872
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section wifiPanel;

		// Token: 0x040022A9 RID: 8873
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetoothLEPanel;

		// Token: 0x040022AA RID: 8874
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetooth2Panel;

		// Token: 0x040022AB RID: 8875
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section bluetoothMFIPanel;

		// Token: 0x0200067D RID: 1661
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003906 RID: 14598 RVA: 0x002C5693 File Offset: 0x002C3893
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003907 RID: 14599 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003908 RID: 14600 RVA: 0x002C569F File Offset: 0x002C389F
			internal void <DroidConnectionSettingsDetector>b__5_0(object d, EventArgs args)
			{
				SharedSettings.Current.FirstConnectionAttempted = false;
			}

			// Token: 0x040022AC RID: 8876
			public static readonly WelcomePage2V3.<>c <>9 = new WelcomePage2V3.<>c();

			// Token: 0x040022AD RID: 8877
			public static EventHandler <>9__5_0;
		}

		// Token: 0x0200067E RID: 1662
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DroidConnectionSettingsDetector>d__5 : IAsyncStateMachine
		{
			// Token: 0x06003909 RID: 14601 RVA: 0x002C56AC File Offset: 0x002C38AC
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
							SharedSettings.Current.FirstConnectionAttempted = false;
						};
						taskAwaiter = connectionDetector.DetectAndSetSettings().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<DroidConnectionSettingsDetector>d__5>(ref taskAwaiter, ref this);
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

			// Token: 0x0600390A RID: 14602 RVA: 0x002C578C File Offset: 0x002C398C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022AE RID: 8878
			public int <>1__state;

			// Token: 0x040022AF RID: 8879
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040022B0 RID: 8880
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200067F RID: 1663
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <FinishAndConnect>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600390B RID: 14603 RVA: 0x002C579C File Offset: 0x002C399C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2V3 welcomePage2V = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Page> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00DA;
						}
						SharedSettings.Current.FirstConnectionAttempted = true;
						taskAwaiter3 = welcomePage2V.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, WelcomePage2V3.<FinishAndConnect>d__3>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = SimpleMainPage.Instance.StartConnection(true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<FinishAndConnect>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_00DA:
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

			// Token: 0x0600390C RID: 14604 RVA: 0x002C58C8 File Offset: 0x002C3AC8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022B1 RID: 8881
			public int <>1__state;

			// Token: 0x040022B2 RID: 8882
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040022B3 RID: 8883
			public WelcomePage2V3 <>4__this;

			// Token: 0x040022B4 RID: 8884
			private TaskAwaiter<Page> <>u__1;

			// Token: 0x040022B5 RID: 8885
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000680 RID: 1664
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <InitializePermissionRequestAsync>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600390D RID: 14605 RVA: 0x002C58D8 File Offset: 0x002C3AD8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2V3 welcomePage2V = this;
				try
				{
					bool flag;
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_0106;
						}
						flag = PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0);
						if (!flag)
						{
							goto IL_008D;
						}
						taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, WelcomePage2V3.<InitializePermissionRequestAsync>d__1>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					flag = taskAwaiter3.GetResult() != 3;
					IL_008D:
					if (flag)
					{
						welcomePage2V.gridDroidPermissions.DisplayGPS = false;
						welcomePage2V.gridDroidPermissions.DisplayBluetooth = true;
						welcomePage2V.gridDroidPermissions.IsVisible = true;
						welcomePage2V.gridMain.IsVisible = false;
					}
					else if (PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0) && !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
					{
						welcomePage2V.gridDroidPermissions.DisplayBluetooth = false;
						welcomePage2V.gridDroidPermissions.DisplayGPS = true;
						welcomePage2V.gridDroidPermissions.IsVisible = true;
						welcomePage2V.gridMain.IsVisible = false;
					}
					IL_0106:;
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

			// Token: 0x0600390E RID: 14606 RVA: 0x002C5A2C File Offset: 0x002C3C2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022B6 RID: 8886
			public int <>1__state;

			// Token: 0x040022B7 RID: 8887
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040022B8 RID: 8888
			public WelcomePage2V3 <>4__this;

			// Token: 0x040022B9 RID: 8889
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x02000681 RID: 1665
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnNext_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600390F RID: 14607 RVA: 0x002C5A3C File Offset: 0x002C3C3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2V3 welcomePage2V = this;
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
						goto IL_0160;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01F9;
					}
					default:
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
						{
							taskAwaiter = welcomePage2V.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnNext_Clicked>d__9>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
						{
							taskAwaiter = welcomePage2V.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnNext_Clicked>d__9>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0160;
						}
						else
						{
							if (!string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
							{
								welcomePage2V.FinishAndConnect();
								goto IL_022E;
							}
							taskAwaiter = welcomePage2V.DisplayAlert(Translate.GetString("welcome_ChooseConnectionProfileFirst_Title"), Translate.GetString("welcome_ChooseConnectionProfileFirst_Text"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnNext_Clicked>d__9>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01F9;
						}
						break;
					}
					taskAwaiter.GetResult();
					welcomePage2V.btnSelectDevice_Clicked(sender, e);
					goto IL_022E;
					IL_0160:
					taskAwaiter.GetResult();
					welcomePage2V.btnSelectDevice_Clicked(sender, e);
					goto IL_022E;
					IL_01F9:
					taskAwaiter.GetResult();
					welcomePage2V.settingsLayoutRoot.ScrollToBottom = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_022E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003910 RID: 14608 RVA: 0x002C5CA8 File Offset: 0x002C3EA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022BA RID: 8890
			public int <>1__state;

			// Token: 0x040022BB RID: 8891
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022BC RID: 8892
			public WelcomePage2V3 <>4__this;

			// Token: 0x040022BD RID: 8893
			public object sender;

			// Token: 0x040022BE RID: 8894
			public EventArgs e;

			// Token: 0x040022BF RID: 8895
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000682 RID: 1666
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectDevice_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06003911 RID: 14609 RVA: 0x002C5CB8 File Offset: 0x002C3EB8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2V3 welcomePage2V = this;
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
							taskAwaiter = welcomePage2V.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
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
							taskAwaiter = welcomePage2V.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 3;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
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
							taskAwaiter = welcomePage2V.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
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
						PlatformHelper.IOSService.OpenSystemSettings();
						goto IL_024F;
					}
					catch (Exception)
					{
					}
					IL_00E6:
					taskAwaiter = welcomePage2V.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2V3.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
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

			// Token: 0x06003912 RID: 14610 RVA: 0x002C5F5C File Offset: 0x002C415C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040022C0 RID: 8896
			public int <>1__state;

			// Token: 0x040022C1 RID: 8897
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040022C2 RID: 8898
			public WelcomePage2V3 <>4__this;

			// Token: 0x040022C3 RID: 8899
			private TaskAwaiter <>u__1;
		}
	}
}
