using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
	// Token: 0x020001BD RID: 445
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml")]
	public class WelcomePage2 : ContentPage
	{
		// Token: 0x06001748 RID: 5960 RVA: 0x000AC938 File Offset: 0x000AAB38
		public WelcomePage2()
		{
			this.InitializeComponent();
			if (SharedSettings.Current.FirstConnectionAttempted && (SharedSettings.Current.ConnectionType != ConnectionTypes.Bluetooth || !string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID)) && (SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE || !string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID)))
			{
				this.panelOptions.IsVisible = false;
				this.panelProfileSelector.IsVisible = true;
			}
			if (false)
			{
				this.connectionTypeGroup.Children.Clear();
			}
			else
			{
				this.LayoutRootConnection.Children.Remove(this.connectionTypePicker);
			}
			base.BindingContext = SharedSettings.Current;
		}

		// Token: 0x06001749 RID: 5961 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600174A RID: 5962 RVA: 0x000AC9E8 File Offset: 0x000AABE8
		private async void Handle_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600174B RID: 5963 RVA: 0x000ACA18 File Offset: 0x000AAC18
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				if (this.panelProfileSelector.IsVisible)
				{
					this.profileSelector.GoBack();
				}
				else
				{
					base.Navigation.PopAsync();
				}
			}
			return true;
		}

		// Token: 0x0600174C RID: 5964 RVA: 0x000ACA70 File Offset: 0x000AAC70
		private async void btnOptionsNext_Clicked(object sender, EventArgs e)
		{
			if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
			{
				await this.DroidAskForPermissions();
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
			{
				await base.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK");
			}
			else if ((SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
			{
				await base.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK");
			}
			if (string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
			{
				this.panelOptions.IsVisible = false;
				this.panelProfileSelector.IsVisible = true;
			}
			else
			{
				this.FinishSetupAndStartConnection();
			}
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x000ACAA8 File Offset: 0x000AACA8
		private async void btnSelectDevice_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				await this.DroidAskForPermissions();
			}
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
				this.activityFramebtle.IsVisible = true;
				if (sender is Button)
				{
					(sender as Button).IsEnabled = false;
				}
				await Task.Delay(100);
				await base.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true);
				this.activityFramebtle.IsVisible = false;
				if (sender is Button)
				{
					(sender as Button).IsEnabled = true;
				}
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus)
			{
				this.activityFramebt2.IsVisible = true;
				if (sender is Button)
				{
					(sender as Button).IsEnabled = false;
				}
				await Task.Delay(100);
				await base.Navigation.PushAsync(new BTDeviceSelectorPage(), true);
				this.activityFramebt2.IsVisible = false;
				if (sender is Button)
				{
					(sender as Button).IsEnabled = true;
				}
			}
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x000ACAE8 File Offset: 0x000AACE8
		private async Task DroidAskForPermissions()
		{
			TaskAwaiter<PermissionStatus> taskAwaiter2;
			if (PlatformHelper.IsAndroid && !this.PermissionWarningShowed)
			{
				TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter.GetResult() != 3)
				{
					bool flag = await base.DisplayAlert(Translate.GetString("droid_AskLocationPermissionTitle"), Translate.GetString("droid_AskLocationPermissionText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No"));
					this.PermissionWarningShowed = true;
					if (flag)
					{
						await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
					}
				}
			}
			if (PlatformHelper.IsiOS)
			{
				TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				bool flag2 = taskAwaiter.GetResult() == 3;
				if (!flag2)
				{
					taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationAlways>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					flag2 = taskAwaiter.GetResult() == 3;
				}
				if (!flag2)
				{
					string text = Translate.GetString("droid_AskLocationPermissionText");
					try
					{
						List<string> list = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
						list.RemoveAt(1);
						text = "";
						foreach (string text2 in list)
						{
							text = text + text2 + "\n";
						}
					}
					catch (Exception)
					{
					}
					TaskAwaiter<bool> taskAwaiter3 = base.DisplayAlert(Translate.GetString("droid_AskLocationPermissionTitle"), text, Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						await taskAwaiter3;
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter3.GetResult())
					{
						Permissions.RequestAsync<Permissions.LocationWhenInUse>();
					}
				}
			}
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x000ACB2C File Offset: 0x000AAD2C
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
				Device.OpenUri(new Uri("https://www.carscanner.info/configuring/"));
				return;
			}
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000ACB98 File Offset: 0x000AAD98
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

		// Token: 0x06001751 RID: 5969 RVA: 0x000ACC28 File Offset: 0x000AAE28
		private void ProfileSelector_WindowCloseRequested(object sender, bool profileSelected)
		{
			if (profileSelected)
			{
				this.FinishSetupAndStartConnection();
				return;
			}
			this.panelProfileSelector.IsVisible = false;
			this.panelOptions.IsVisible = true;
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000ACC4C File Offset: 0x000AAE4C
		private void FinishSetupAndStartConnection()
		{
			SharedSettings.Current.FirstConnectionAttempted = true;
			SimpleMainPage.Instance.StartConnection(true);
			base.Navigation.PopAsync();
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000ACC74 File Offset: 0x000AAE74
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(WelcomePage2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/WelcomePage2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			ConnectionTypeToWiFiVisibleBoolConverter connectionTypeToWiFiVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToWiFiVisibleBoolConverter = new ConnectionTypeToWiFiVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ConnectionTypeToBTLEVisibleBoolConverter connectionTypeToBTLEVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTLEVisibleBoolConverter = new ConnectionTypeToBTLEVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ConnectionTypeToBTVisibleBoolConverter connectionTypeToBTVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToBTVisibleBoolConverter = new ConnectionTypeToBTVisibleBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ConnectionTypeToMFIBluetoothConverter connectionTypeToMFIBluetoothConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToMFIBluetoothConverter = new ConnectionTypeToMFIBluetoothConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ConnectionTypeToIntConverter connectionTypeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(connectionTypeToIntConverter = new ConnectionTypeToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 25);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 36);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 30);
			List<string> connectionPickerItems;
			VisualDiagnostics.RegisterSourceInfo(connectionPickerItems = StaticLists.ConnectionPickerItems, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 33);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 33);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 33);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 33);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 37);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 37);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 37);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 37);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 37);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 34);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 37);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 37);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 37);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 37);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 37);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 34);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 37);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 37);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 37);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 37);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 37);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 46);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 46);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 42);
			SfRadioButton sfRadioButton3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton3 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 34);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 37);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 37);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 37);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 37);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 37);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 46);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 46);
			OnPlatform<bool> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 42);
			SfRadioButton sfRadioButton4;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton4 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 34);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 33);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 33);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 40);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 65);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 34);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 40);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 34);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 37);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 34);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 40);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 84);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 34);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 30);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 33);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 37);
			On on5;
			VisualDiagnostics.RegisterSourceInfo(on5 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 46);
			On on6;
			VisualDiagnostics.RegisterSourceInfo(on6 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 46);
			OnPlatform<bool> onPlatform5;
			VisualDiagnostics.RegisterSourceInfo(onPlatform5 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 42);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 34);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 40);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 34);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 37);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 34);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 37);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 34);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 37);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 34);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 30);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 33);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 40);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 34);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 37);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 34);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 37);
			ActivityFrame activityFrame2;
			VisualDiagnostics.RegisterSourceInfo(activityFrame2 = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 34);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 37);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 34);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 45);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 45);
			OnPlatform<string> onPlatform6;
			VisualDiagnostics.RegisterSourceInfo(onPlatform6 = new OnPlatform<string>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 42);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 34);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 30);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 33);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 40);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 37);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 34);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 37);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 34);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 30);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 33);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 30);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 48);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 113);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 30);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 25);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 22);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 22);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 21);
			ProfileSelectorV2 profileSelectorV;
			VisualDiagnostics.RegisterSourceInfo(profileSelectorV = new ProfileSelectorV2(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\WelcomePage2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelOptions", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "panelOptions";
			}
			nameScope.RegisterName("LayoutRootConnection", stackLayout5);
			if (stackLayout5.StyleId == null)
			{
				stackLayout5.StyleId = "LayoutRootConnection";
			}
			nameScope.RegisterName("connectionTypePicker", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "connectionTypePicker";
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
			nameScope.RegisterName("activityFramebtle", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFramebtle";
			}
			nameScope.RegisterName("bluetoothMFIPanel", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "bluetoothMFIPanel";
			}
			nameScope.RegisterName("activityFramebt2", activityFrame2);
			if (activityFrame2.StyleId == null)
			{
				activityFrame2.StyleId = "activityFramebt2";
			}
			nameScope.RegisterName("bluetooth2Panel", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "bluetooth2Panel";
			}
			nameScope.RegisterName("panelProfileSelector", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelProfileSelector";
			}
			nameScope.RegisterName("profileSelector", profileSelectorV);
			if (profileSelectorV.StyleId == null)
			{
				profileSelectorV.StyleId = "profileSelector";
			}
			this.panelOptions = scrollView;
			this.LayoutRootConnection = stackLayout5;
			this.connectionTypePicker = picker;
			this.connectionTypeGroup = sfRadioGroup;
			this.btnWiFi = sfRadioButton;
			this.btnBluetoothLE = sfRadioButton2;
			this.btnBluetooth = sfRadioButton3;
			this.btnMFI = sfRadioButton4;
			this.wifiPanel = stackLayout;
			this.bluetoothLEPanel = stackLayout2;
			this.activityFramebtle = activityFrame;
			this.bluetoothMFIPanel = stackLayout3;
			this.activityFramebt2 = activityFrame2;
			this.bluetooth2Panel = stackLayout4;
			this.panelProfileSelector = grid;
			this.profileSelector = profileSelectorV;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ConnectionTypeToWiFiVisibleBoolConverter", connectionTypeToWiFiVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTLEVisibleBoolConverter", connectionTypeToBTLEVisibleBoolConverter);
			resourceDictionary.Add("ConnectionTypeToBTVisibleBoolConverter", connectionTypeToBTVisibleBoolConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("ConnectionTypeToMFIBluetoothConverter", connectionTypeToMFIBluetoothConverter);
			resourceDictionary.Add("ConnectionTypeToIntConverter", connectionTypeToIntConverter);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, false);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(0.0);
			onPlatform.iOS = new Thickness(10.0, 20.0, 10.0, 10.0);
			this.SetValue(Page.PaddingProperty, onPlatform);
			onPlatform2.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform2.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform2);
			scrollView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.LineBreakModeProperty, 1);
			translate.Text = "ios_MostImportantSettings";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 5];
			array2[0] = label;
			array2[1] = stackLayout6;
			array2[2] = scrollView;
			array2[3] = grid2;
			array2[4] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 25)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			stackLayout6.Children.Add(label);
			frame.SetValue(Layout.PaddingProperty, new Thickness(10.0));
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = frame;
			array3[1] = stackLayout6;
			array3[2] = scrollView;
			array3[3] = grid2;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			frame.SetValue(Frame.CornerRadiusProperty, 0f);
			frame.SetValue(Frame.HasShadowProperty, false);
			frame.SetValue(Frame.OutlineColorProperty, Color.Transparent);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "Settings_Control_tbConnectionType.Text";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 7];
			array4[0] = label2;
			array4[1] = stackLayout5;
			array4[2] = frame;
			array4[3] = stackLayout6;
			array4[4] = scrollView;
			array4[5] = grid2;
			array4[6] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 36)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label2.Text = obj6;
			stackLayout5.Children.Add(label2);
			bindingExtension.Source = connectionPickerItems;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase);
			bindingExtension2.Mode = 1;
			staticResourceExtension.Key = "ConnectionTypeToIntConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 8];
			array5[0] = bindingExtension2;
			array5[1] = picker;
			array5[2] = stackLayout5;
			array5[3] = frame;
			array5[4] = stackLayout6;
			array5[5] = scrollView;
			array5[6] = grid2;
			array5[7] = this;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 33)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension2.Converter = obj8;
			bindingExtension2.Path = "ConnectionType";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase2);
			stackLayout5.Children.Add(picker);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 8];
			array6[0] = sfRadioButton;
			array6[1] = sfRadioGroup;
			array6[2] = stackLayout5;
			array6[3] = frame;
			array6[4] = stackLayout6;
			array6[5] = scrollView;
			array6[6] = grid2;
			array6[7] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 37)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfRadioButton.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource3.Key);
			bindingExtension3.Mode = 2;
			staticResourceExtension2.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 9];
			array7[0] = bindingExtension3;
			array7[1] = sfRadioButton;
			array7[2] = sfRadioGroup;
			array7[3] = stackLayout5;
			array7[4] = frame;
			array7[5] = stackLayout6;
			array7[6] = scrollView;
			array7[7] = grid2;
			array7[8] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 37)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension3.Converter = obj11;
			bindingExtension3.Path = "ConnectionType";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			sfRadioButton.SetBinding(ToggleButton.IsCheckedProperty, bindingBase3);
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton.SetValue(ToggleButton.TextProperty, "Wi-Fi");
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = sfRadioButton;
			array8[1] = sfRadioGroup;
			array8[2] = stackLayout5;
			array8[3] = frame;
			array8[4] = stackLayout6;
			array8[5] = scrollView;
			array8[6] = grid2;
			array8[7] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 37)));
			DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
			sfRadioButton.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource4.Key);
			dynamicResourceExtension5.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = sfRadioButton;
			array9[1] = sfRadioGroup;
			array9[2] = stackLayout5;
			array9[3] = frame;
			array9[4] = stackLayout6;
			array9[5] = scrollView;
			array9[6] = grid2;
			array9[7] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array9, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 37)));
			DynamicResource dynamicResource5 = markupExtension9.ProvideValue(xamlServiceProvider9);
			sfRadioButton.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource5.Key);
			sfRadioGroup.Children.Add(sfRadioButton);
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 8];
			array10[0] = sfRadioButton2;
			array10[1] = sfRadioGroup;
			array10[2] = stackLayout5;
			array10[3] = frame;
			array10[4] = stackLayout6;
			array10[5] = scrollView;
			array10[6] = grid2;
			array10[7] = this;
			object obj14;
			xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array10, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 37)));
			DynamicResource dynamicResource6 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfRadioButton2.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource6.Key);
			bindingExtension4.Mode = 2;
			staticResourceExtension3.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 9];
			array11[0] = bindingExtension4;
			array11[1] = sfRadioButton2;
			array11[2] = sfRadioGroup;
			array11[3] = stackLayout5;
			array11[4] = frame;
			array11[5] = stackLayout6;
			array11[6] = scrollView;
			array11[7] = grid2;
			array11[8] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 37)));
			object obj16 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension4.Converter = obj16;
			bindingExtension4.Path = "ConnectionType";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			sfRadioButton2.SetBinding(ToggleButton.IsCheckedProperty, bindingBase4);
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton2.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton2.SetValue(ToggleButton.TextProperty, "Bluetooth LE (4.0+)");
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 8];
			array12[0] = sfRadioButton2;
			array12[1] = sfRadioGroup;
			array12[2] = stackLayout5;
			array12[3] = frame;
			array12[4] = stackLayout6;
			array12[5] = scrollView;
			array12[6] = grid2;
			array12[7] = this;
			object obj17;
			xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array12, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 37)));
			DynamicResource dynamicResource7 = markupExtension12.ProvideValue(xamlServiceProvider12);
			sfRadioButton2.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource7.Key);
			dynamicResourceExtension8.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 8];
			array13[0] = sfRadioButton2;
			array13[1] = sfRadioGroup;
			array13[2] = stackLayout5;
			array13[3] = frame;
			array13[4] = stackLayout6;
			array13[5] = scrollView;
			array13[6] = grid2;
			array13[7] = this;
			object obj18;
			xamlServiceProvider13.Add(typeFromHandle25, obj18 = new SimpleValueTargetProvider(array13, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 37)));
			DynamicResource dynamicResource8 = markupExtension13.ProvideValue(xamlServiceProvider13);
			sfRadioButton2.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource8.Key);
			sfRadioGroup.Children.Add(sfRadioButton2);
			dynamicResourceExtension9.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 8];
			array14[0] = sfRadioButton3;
			array14[1] = sfRadioGroup;
			array14[2] = stackLayout5;
			array14[3] = frame;
			array14[4] = stackLayout6;
			array14[5] = scrollView;
			array14[6] = grid2;
			array14[7] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 37)));
			DynamicResource dynamicResource9 = markupExtension14.ProvideValue(xamlServiceProvider14);
			sfRadioButton3.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource9.Key);
			bindingExtension5.Mode = 2;
			staticResourceExtension4.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension15 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 9];
			array15[0] = bindingExtension5;
			array15[1] = sfRadioButton3;
			array15[2] = sfRadioGroup;
			array15[3] = stackLayout5;
			array15[4] = frame;
			array15[5] = stackLayout6;
			array15[6] = scrollView;
			array15[7] = grid2;
			array15[8] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle29, obj20 = new SimpleValueTargetProvider(array15, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 37)));
			object obj21 = markupExtension15.ProvideValue(xamlServiceProvider15);
			bindingExtension5.Converter = obj21;
			bindingExtension5.Path = "ConnectionType";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			sfRadioButton3.SetBinding(ToggleButton.IsCheckedProperty, bindingBase5);
			sfRadioButton3.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton3.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton3.SetValue(ToggleButton.TextProperty, "Bluetooth");
			dynamicResourceExtension10.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 8];
			array16[0] = sfRadioButton3;
			array16[1] = sfRadioGroup;
			array16[2] = stackLayout5;
			array16[3] = frame;
			array16[4] = stackLayout6;
			array16[5] = scrollView;
			array16[6] = grid2;
			array16[7] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array16, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 37)));
			DynamicResource dynamicResource10 = markupExtension16.ProvideValue(xamlServiceProvider16);
			sfRadioButton3.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource10.Key);
			dynamicResourceExtension11.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 8];
			array17[0] = sfRadioButton3;
			array17[1] = sfRadioGroup;
			array17[2] = stackLayout5;
			array17[3] = frame;
			array17[4] = stackLayout6;
			array17[5] = scrollView;
			array17[6] = grid2;
			array17[7] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array17, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 37)));
			DynamicResource dynamicResource11 = markupExtension17.ProvideValue(xamlServiceProvider17);
			sfRadioButton3.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource11.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform3.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform3.Platforms.Add(on2);
			sfRadioButton3.SetValue(VisualElement.IsVisibleProperty, onPlatform3);
			sfRadioGroup.Children.Add(sfRadioButton3);
			dynamicResourceExtension12.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 8];
			array18[0] = sfRadioButton4;
			array18[1] = sfRadioGroup;
			array18[2] = stackLayout5;
			array18[3] = frame;
			array18[4] = stackLayout6;
			array18[5] = scrollView;
			array18[6] = grid2;
			array18[7] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle35, obj24 = new SimpleValueTargetProvider(array18, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 37)));
			DynamicResource dynamicResource12 = markupExtension18.ProvideValue(xamlServiceProvider18);
			sfRadioButton4.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource12.Key);
			bindingExtension6.Mode = 2;
			staticResourceExtension5.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension19 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 9];
			array19[0] = bindingExtension6;
			array19[1] = sfRadioButton4;
			array19[2] = sfRadioGroup;
			array19[3] = stackLayout5;
			array19[4] = frame;
			array19[5] = stackLayout6;
			array19[6] = scrollView;
			array19[7] = grid2;
			array19[8] = this;
			object obj25;
			xamlServiceProvider19.Add(typeFromHandle37, obj25 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 37)));
			object obj26 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension6.Converter = obj26;
			bindingExtension6.Path = "ConnectionType";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			sfRadioButton4.SetBinding(ToggleButton.IsCheckedProperty, bindingBase6);
			sfRadioButton4.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton4.StateChanged += this.btnConnectionType_StateChanged;
			sfRadioButton4.SetValue(ToggleButton.TextProperty, "Bluetooth MFi (OBDLink MX+, vLinker FS)");
			dynamicResourceExtension13.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 8];
			array20[0] = sfRadioButton4;
			array20[1] = sfRadioGroup;
			array20[2] = stackLayout5;
			array20[3] = frame;
			array20[4] = stackLayout6;
			array20[5] = scrollView;
			array20[6] = grid2;
			array20[7] = this;
			object obj27;
			xamlServiceProvider20.Add(typeFromHandle39, obj27 = new SimpleValueTargetProvider(array20, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 37)));
			DynamicResource dynamicResource13 = markupExtension20.ProvideValue(xamlServiceProvider20);
			sfRadioButton4.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource13.Key);
			dynamicResourceExtension14.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 8];
			array21[0] = sfRadioButton4;
			array21[1] = sfRadioGroup;
			array21[2] = stackLayout5;
			array21[3] = frame;
			array21[4] = stackLayout6;
			array21[5] = scrollView;
			array21[6] = grid2;
			array21[7] = this;
			object obj28;
			xamlServiceProvider21.Add(typeFromHandle41, obj28 = new SimpleValueTargetProvider(array21, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 37)));
			DynamicResource dynamicResource14 = markupExtension21.ProvideValue(xamlServiceProvider21);
			sfRadioButton4.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource14.Key);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "true";
			onPlatform4.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "false";
			onPlatform4.Platforms.Add(on4);
			sfRadioButton4.SetValue(VisualElement.IsVisibleProperty, onPlatform4);
			sfRadioGroup.Children.Add(sfRadioButton4);
			stackLayout5.Children.Add(sfRadioGroup);
			bindingExtension7.Mode = 2;
			staticResourceExtension6.Key = "ConnectionTypeToWiFiVisibleBoolConverter";
			IMarkupExtension markupExtension22 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 8];
			array22[0] = bindingExtension7;
			array22[1] = stackLayout;
			array22[2] = stackLayout5;
			array22[3] = frame;
			array22[4] = stackLayout6;
			array22[5] = scrollView;
			array22[6] = grid2;
			array22[7] = this;
			object obj29;
			xamlServiceProvider22.Add(typeFromHandle43, obj29 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 33)));
			object obj30 = markupExtension22.ProvideValue(xamlServiceProvider22);
			bindingExtension7.Converter = obj30;
			bindingExtension7.Path = "ConnectionType";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate3.Text = "Settings_Control_tbWiFiServer.Text";
			IMarkupExtension markupExtension23 = translate3;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 8];
			array23[0] = label3;
			array23[1] = stackLayout;
			array23[2] = stackLayout5;
			array23[3] = frame;
			array23[4] = stackLayout6;
			array23[5] = scrollView;
			array23[6] = grid2;
			array23[7] = this;
			object obj31;
			xamlServiceProvider23.Add(typeFromHandle45, obj31 = new SimpleValueTargetProvider(array23, Label.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 40)));
			object obj32 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label3.Text = obj32;
			stackLayout.Children.Add(label3);
			entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "WiFiServer";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase8);
			stackLayout.Children.Add(entry);
			translate4.Text = "Settings_Control_tbWiFiPort.Text";
			IMarkupExtension markupExtension24 = translate4;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 8];
			array24[0] = label4;
			array24[1] = stackLayout;
			array24[2] = stackLayout5;
			array24[3] = frame;
			array24[4] = stackLayout6;
			array24[5] = scrollView;
			array24[6] = grid2;
			array24[7] = this;
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
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 40)));
			object obj34 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label4.Text = obj34;
			stackLayout.Children.Add(label4);
			entry2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			entry2.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "WiFiPort";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase9);
			stackLayout.Children.Add(entry2);
			dynamicResourceExtension15.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 8];
			array25[0] = label5;
			array25[1] = stackLayout;
			array25[2] = stackLayout5;
			array25[3] = frame;
			array25[4] = stackLayout6;
			array25[5] = scrollView;
			array25[6] = grid2;
			array25[7] = this;
			object obj35;
			xamlServiceProvider25.Add(typeFromHandle49, obj35 = new SimpleValueTargetProvider(array25, Label.FontSizeProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 40)));
			DynamicResource dynamicResource15 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource15.Key);
			translate5.Text = "ios_DefaultWiFi_Servers";
			IMarkupExtension markupExtension26 = translate5;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 8];
			array26[0] = label5;
			array26[1] = stackLayout;
			array26[2] = stackLayout5;
			array26[3] = frame;
			array26[4] = stackLayout6;
			array26[5] = scrollView;
			array26[6] = grid2;
			array26[7] = this;
			object obj36;
			xamlServiceProvider26.Add(typeFromHandle51, obj36 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 84)));
			object obj37 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label5.Text = obj37;
			stackLayout.Children.Add(label5);
			stackLayout5.Children.Add(stackLayout);
			bindingExtension10.Mode = 2;
			staticResourceExtension7.Key = "ConnectionTypeToBTLEVisibleBoolConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 8];
			array27[0] = bindingExtension10;
			array27[1] = stackLayout2;
			array27[2] = stackLayout5;
			array27[3] = frame;
			array27[4] = stackLayout6;
			array27[5] = scrollView;
			array27[6] = grid2;
			array27[7] = this;
			object obj38;
			xamlServiceProvider27.Add(typeFromHandle53, obj38 = new SimpleValueTargetProvider(array27, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 33)));
			object obj39 = markupExtension27.ProvideValue(xamlServiceProvider27);
			bindingExtension10.Converter = obj39;
			bindingExtension10.Path = "ConnectionType";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label6.SetValue(Label.LineBreakModeProperty, 1);
			translate6.Text = "droid_BTLE_Warning";
			IMarkupExtension markupExtension28 = translate6;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 8];
			array28[0] = label6;
			array28[1] = stackLayout2;
			array28[2] = stackLayout5;
			array28[3] = frame;
			array28[4] = stackLayout6;
			array28[5] = scrollView;
			array28[6] = grid2;
			array28[7] = this;
			object obj40;
			xamlServiceProvider28.Add(typeFromHandle55, obj40 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 37)));
			object obj41 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label6.Text = obj41;
			label6.SetValue(Label.TextColorProperty, Color.Red);
			on5.Platform = new List<string>(1) { "iOS" };
			on5.Value = "false";
			onPlatform5.Platforms.Add(on5);
			on6.Platform = new List<string>(1) { "Android" };
			on6.Value = "true";
			onPlatform5.Platforms.Add(on6);
			label6.SetValue(VisualElement.IsVisibleProperty, onPlatform5);
			stackLayout2.Children.Add(label6);
			translate7.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension29 = translate7;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 8];
			array29[0] = label7;
			array29[1] = stackLayout2;
			array29[2] = stackLayout5;
			array29[3] = frame;
			array29[4] = stackLayout6;
			array29[5] = scrollView;
			array29[6] = grid2;
			array29[7] = this;
			object obj42;
			xamlServiceProvider29.Add(typeFromHandle57, obj42 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 40)));
			object obj43 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label7.Text = obj43;
			stackLayout2.Children.Add(label7);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label8.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "BTLEDeviceName";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label8.SetBinding(Label.TextProperty, bindingBase11);
			stackLayout2.Children.Add(label8);
			activityFrame.SetValue(Grid.RowProperty, 2);
			activityFrame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate8.Text = "ios_PleaseWait";
			IMarkupExtension markupExtension30 = translate8;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 8];
			array30[0] = activityFrame;
			array30[1] = stackLayout2;
			array30[2] = stackLayout5;
			array30[3] = frame;
			array30[4] = stackLayout6;
			array30[5] = scrollView;
			array30[6] = grid2;
			array30[7] = this;
			object obj44;
			xamlServiceProvider30.Add(typeFromHandle59, obj44 = new SimpleValueTargetProvider(array30, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 37)));
			object obj45 = markupExtension30.ProvideValue(xamlServiceProvider30);
			activityFrame.Text = obj45;
			stackLayout2.Children.Add(activityFrame);
			button.Clicked += this.btnSelectDevice_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate9.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension31 = translate9;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 8];
			array31[0] = button;
			array31[1] = stackLayout2;
			array31[2] = stackLayout5;
			array31[3] = frame;
			array31[4] = stackLayout6;
			array31[5] = scrollView;
			array31[6] = grid2;
			array31[7] = this;
			object obj46;
			xamlServiceProvider31.Add(typeFromHandle61, obj46 = new SimpleValueTargetProvider(array31, Button.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 37)));
			object obj47 = markupExtension31.ProvideValue(xamlServiceProvider31);
			button.Text = obj47;
			stackLayout2.Children.Add(button);
			stackLayout5.Children.Add(stackLayout2);
			bindingExtension12.Mode = 2;
			staticResourceExtension8.Key = "ConnectionTypeToBTVisibleBoolConverter";
			IMarkupExtension markupExtension32 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 8];
			array32[0] = bindingExtension12;
			array32[1] = stackLayout3;
			array32[2] = stackLayout5;
			array32[3] = frame;
			array32[4] = stackLayout6;
			array32[5] = scrollView;
			array32[6] = grid2;
			array32[7] = this;
			object obj48;
			xamlServiceProvider32.Add(typeFromHandle63, obj48 = new SimpleValueTargetProvider(array32, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 33)));
			object obj49 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension12.Converter = obj49;
			bindingExtension12.Path = "ConnectionType";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate10.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension33 = translate10;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 8];
			array33[0] = label9;
			array33[1] = stackLayout3;
			array33[2] = stackLayout5;
			array33[3] = frame;
			array33[4] = stackLayout6;
			array33[5] = scrollView;
			array33[6] = grid2;
			array33[7] = this;
			object obj50;
			xamlServiceProvider33.Add(typeFromHandle65, obj50 = new SimpleValueTargetProvider(array33, Label.TextProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 40)));
			object obj51 = markupExtension33.ProvideValue(xamlServiceProvider33);
			label9.Text = obj51;
			stackLayout3.Children.Add(label9);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label10.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "BTDeviceName";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label10.SetBinding(Label.TextProperty, bindingBase13);
			stackLayout3.Children.Add(label10);
			activityFrame2.SetValue(Grid.RowProperty, 2);
			activityFrame2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			activityFrame2.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate11.Text = "ios_PleaseWait";
			IMarkupExtension markupExtension34 = translate11;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 8];
			array34[0] = activityFrame2;
			array34[1] = stackLayout3;
			array34[2] = stackLayout5;
			array34[3] = frame;
			array34[4] = stackLayout6;
			array34[5] = scrollView;
			array34[6] = grid2;
			array34[7] = this;
			object obj52;
			xamlServiceProvider34.Add(typeFromHandle67, obj52 = new SimpleValueTargetProvider(array34, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(193, 37)));
			object obj53 = markupExtension34.ProvideValue(xamlServiceProvider34);
			activityFrame2.Text = obj53;
			stackLayout3.Children.Add(activityFrame2);
			button2.Clicked += this.btnSelectDevice_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate12.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension35 = translate12;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 8];
			array35[0] = button2;
			array35[1] = stackLayout3;
			array35[2] = stackLayout5;
			array35[3] = frame;
			array35[4] = stackLayout6;
			array35[5] = scrollView;
			array35[6] = grid2;
			array35[7] = this;
			object obj54;
			xamlServiceProvider35.Add(typeFromHandle69, obj54 = new SimpleValueTargetProvider(array35, Button.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 37)));
			object obj55 = markupExtension35.ProvideValue(xamlServiceProvider35);
			button2.Text = obj55;
			stackLayout3.Children.Add(button2);
			translate13.Text = "android_BT2_BT4";
			IMarkupExtension markupExtension36 = translate13;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 9];
			array36[0] = onPlatform6;
			array36[1] = label11;
			array36[2] = stackLayout3;
			array36[3] = stackLayout5;
			array36[4] = frame;
			array36[5] = stackLayout6;
			array36[6] = scrollView;
			array36[7] = grid2;
			array36[8] = this;
			object obj56;
			xamlServiceProvider36.Add(typeFromHandle71, obj56 = new SimpleValueTargetProvider(array36, typeof(OnPlatform<string>).GetRuntimeProperty("Android"), nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(202, 45)));
			object obj57 = markupExtension36.ProvideValue(xamlServiceProvider36);
			onPlatform6.Android = obj57;
			translate14.Text = "ios_BLE_Experimental";
			IMarkupExtension markupExtension37 = translate14;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 9];
			array37[0] = onPlatform6;
			array37[1] = label11;
			array37[2] = stackLayout3;
			array37[3] = stackLayout5;
			array37[4] = frame;
			array37[5] = stackLayout6;
			array37[6] = scrollView;
			array37[7] = grid2;
			array37[8] = this;
			object obj58;
			xamlServiceProvider37.Add(typeFromHandle73, obj58 = new SimpleValueTargetProvider(array37, typeof(OnPlatform<string>).GetRuntimeProperty("iOS"), nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 45)));
			object obj59 = markupExtension37.ProvideValue(xamlServiceProvider37);
			onPlatform6.iOS = obj59;
			label11.SetValue(Label.TextProperty, onPlatform6);
			stackLayout3.Children.Add(label11);
			stackLayout5.Children.Add(stackLayout3);
			bindingExtension14.Mode = 2;
			staticResourceExtension9.Key = "ConnectionTypeToMFIBluetoothConverter";
			IMarkupExtension markupExtension38 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 8];
			array38[0] = bindingExtension14;
			array38[1] = stackLayout4;
			array38[2] = stackLayout5;
			array38[3] = frame;
			array38[4] = stackLayout6;
			array38[5] = scrollView;
			array38[6] = grid2;
			array38[7] = this;
			object obj60;
			xamlServiceProvider38.Add(typeFromHandle75, obj60 = new SimpleValueTargetProvider(array38, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 33)));
			object obj61 = markupExtension38.ProvideValue(xamlServiceProvider38);
			bindingExtension14.Converter = obj61;
			bindingExtension14.Path = "ConnectionType";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			translate15.Text = "ios_BTLEDeviceName.Text";
			IMarkupExtension markupExtension39 = translate15;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 8];
			array39[0] = label12;
			array39[1] = stackLayout4;
			array39[2] = stackLayout5;
			array39[3] = frame;
			array39[4] = stackLayout6;
			array39[5] = scrollView;
			array39[6] = grid2;
			array39[7] = this;
			object obj62;
			xamlServiceProvider39.Add(typeFromHandle77, obj62 = new SimpleValueTargetProvider(array39, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 40)));
			object obj63 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label12.Text = obj63;
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
			translate16.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension40 = translate16;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 8];
			array40[0] = button3;
			array40[1] = stackLayout4;
			array40[2] = stackLayout5;
			array40[3] = frame;
			array40[4] = stackLayout6;
			array40[5] = scrollView;
			array40[6] = grid2;
			array40[7] = this;
			object obj64;
			xamlServiceProvider40.Add(typeFromHandle79, obj64 = new SimpleValueTargetProvider(array40, Button.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(221, 37)));
			object obj65 = markupExtension40.ProvideValue(xamlServiceProvider40);
			button3.Text = obj65;
			stackLayout4.Children.Add(button3);
			stackLayout5.Children.Add(stackLayout4);
			linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.btnConnectionGuide_Clicked;
			translate17.Text = "ios_ConnectionGuide";
			IMarkupExtension markupExtension41 = translate17;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 7];
			array41[0] = linkButton;
			array41[1] = stackLayout5;
			array41[2] = frame;
			array41[3] = stackLayout6;
			array41[4] = scrollView;
			array41[5] = grid2;
			array41[6] = this;
			object obj66;
			xamlServiceProvider41.Add(typeFromHandle81, obj66 = new SimpleValueTargetProvider(array41, Button.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(236, 33)));
			object obj67 = markupExtension41.ProvideValue(xamlServiceProvider41);
			linkButton.Text = obj67;
			stackLayout5.Children.Add(linkButton);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "AndroidStartBackgroundService";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase16);
			translate18.Text = "droid_StartBackgroundService";
			IMarkupExtension markupExtension42 = translate18;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 7];
			array42[0] = labelSwitch;
			array42[1] = stackLayout5;
			array42[2] = frame;
			array42[3] = stackLayout6;
			array42[4] = scrollView;
			array42[5] = grid2;
			array42[6] = this;
			object obj68;
			xamlServiceProvider42.Add(typeFromHandle83, obj68 = new SimpleValueTargetProvider(array42, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(239, 113)));
			object obj69 = markupExtension42.ProvideValue(xamlServiceProvider42);
			labelSwitch.Text = obj69;
			stackLayout5.Children.Add(labelSwitch);
			frame.SetValue(ContentView.ContentProperty, stackLayout5);
			stackLayout6.Children.Add(frame);
			button4.SetValue(VisualElement.BackgroundColorProperty, Color.Green);
			button4.Clicked += this.btnOptionsNext_Clicked;
			translate19.Text = "ios_NEXT";
			IMarkupExtension markupExtension43 = translate19;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 5];
			array43[0] = button4;
			array43[1] = stackLayout6;
			array43[2] = scrollView;
			array43[3] = grid2;
			array43[4] = this;
			object obj70;
			xamlServiceProvider43.Add(typeFromHandle85, obj70 = new SimpleValueTargetProvider(array43, Button.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(265, 25)));
			object obj71 = markupExtension43.ProvideValue(xamlServiceProvider43);
			button4.Text = obj71;
			button4.SetValue(Button.TextColorProperty, Color.White);
			stackLayout6.Children.Add(button4);
			scrollView.Content = stackLayout6;
			grid2.Children.Add(scrollView);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			profileSelectorV.SetValue(Grid.RowProperty, 0);
			profileSelectorV.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			profileSelectorV.SetValue(ProfileSelectorV2.BackButtonVisibleProperty, false);
			profileSelectorV.SetValue(ProfileSelectorV2.CreateBackItemProperty, true);
			translate20.Text = "ios_ChooseCarBrand";
			IMarkupExtension markupExtension44 = translate20;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = profileSelectorV;
			array44[1] = grid;
			array44[2] = grid2;
			array44[3] = this;
			object obj72;
			xamlServiceProvider44.Add(typeFromHandle87, obj72 = new SimpleValueTargetProvider(array44, ProfileSelectorV2.TitleTextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(WelcomePage2).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(284, 21)));
			object obj73 = markupExtension44.ProvideValue(xamlServiceProvider44);
			profileSelectorV.TitleText = obj73;
			profileSelectorV.SetValue(ProfileSelectorV2.TitleVisibleProperty, true);
			profileSelectorV.WindowCloseRequested += this.ProfileSelector_WindowCloseRequested;
			grid.Children.Add(profileSelectorV);
			grid2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x000B2958 File Offset: 0x000B0B58
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<WelcomePage2>(this, typeof(WelcomePage2));
			this.panelOptions = NameScopeExtensions.FindByName<ScrollView>(this, "panelOptions");
			this.LayoutRootConnection = NameScopeExtensions.FindByName<StackLayout>(this, "LayoutRootConnection");
			this.connectionTypePicker = NameScopeExtensions.FindByName<Picker>(this, "connectionTypePicker");
			this.connectionTypeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "connectionTypeGroup");
			this.btnWiFi = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnWiFi");
			this.btnBluetoothLE = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnBluetoothLE");
			this.btnBluetooth = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnBluetooth");
			this.btnMFI = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnMFI");
			this.wifiPanel = NameScopeExtensions.FindByName<StackLayout>(this, "wifiPanel");
			this.bluetoothLEPanel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetoothLEPanel");
			this.activityFramebtle = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFramebtle");
			this.bluetoothMFIPanel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetoothMFIPanel");
			this.activityFramebt2 = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFramebt2");
			this.bluetooth2Panel = NameScopeExtensions.FindByName<StackLayout>(this, "bluetooth2Panel");
			this.panelProfileSelector = NameScopeExtensions.FindByName<Grid>(this, "panelProfileSelector");
			this.profileSelector = NameScopeExtensions.FindByName<ProfileSelectorV2>(this, "profileSelector");
		}

		// Token: 0x04000A24 RID: 2596
		private bool PermissionWarningShowed;

		// Token: 0x04000A25 RID: 2597
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView panelOptions;

		// Token: 0x04000A26 RID: 2598
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout LayoutRootConnection;

		// Token: 0x04000A27 RID: 2599
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker connectionTypePicker;

		// Token: 0x04000A28 RID: 2600
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup connectionTypeGroup;

		// Token: 0x04000A29 RID: 2601
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnWiFi;

		// Token: 0x04000A2A RID: 2602
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnBluetoothLE;

		// Token: 0x04000A2B RID: 2603
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnBluetooth;

		// Token: 0x04000A2C RID: 2604
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnMFI;

		// Token: 0x04000A2D RID: 2605
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout wifiPanel;

		// Token: 0x04000A2E RID: 2606
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetoothLEPanel;

		// Token: 0x04000A2F RID: 2607
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFramebtle;

		// Token: 0x04000A30 RID: 2608
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetoothMFIPanel;

		// Token: 0x04000A31 RID: 2609
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFramebt2;

		// Token: 0x04000A32 RID: 2610
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout bluetooth2Panel;

		// Token: 0x04000A33 RID: 2611
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelProfileSelector;

		// Token: 0x04000A34 RID: 2612
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProfileSelectorV2 profileSelector;

		// Token: 0x020001BE RID: 446
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DroidAskForPermissions>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001755 RID: 5973 RVA: 0x000B2A88 File Offset: 0x000B0C88
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2 welcomePage = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter5;
					TaskAwaiter<bool> taskAwaiter6;
					switch (num)
					{
					case 0:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						break;
					case 1:
						taskAwaiter6 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_011D;
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						goto IL_0180;
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						goto IL_01E5;
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num = (num2 = -1);
						goto IL_0248;
					case 5:
						taskAwaiter6 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_0347;
					default:
						if (!PlatformHelper.IsAndroid || welcomePage.PermissionWarningShowed)
						{
							goto IL_0188;
						}
						taskAwaiter5 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter5, ref this);
							return;
						}
						break;
					}
					if (taskAwaiter5.GetResult() == 3)
					{
						goto IL_0188;
					}
					taskAwaiter6 = welcomePage.DisplayAlert(Translate.GetString("droid_AskLocationPermissionTitle"), Translate.GetString("droid_AskLocationPermissionText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 1);
						taskAwaiter4 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter6, ref this);
						return;
					}
					IL_011D:
					bool result = taskAwaiter6.GetResult();
					welcomePage.PermissionWarningShowed = true;
					if (!result)
					{
						goto IL_0188;
					}
					taskAwaiter5 = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 2);
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0180:
					taskAwaiter5.GetResult();
					IL_0188:
					if (!PlatformHelper.IsiOS)
					{
						goto IL_0356;
					}
					taskAwaiter5 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 3);
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter5, ref this);
						return;
					}
					IL_01E5:
					bool flag = taskAwaiter5.GetResult() == 3;
					if (flag)
					{
						goto IL_0254;
					}
					taskAwaiter5 = Permissions.CheckStatusAsync<Permissions.LocationAlways>().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 4);
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0248:
					flag = taskAwaiter5.GetResult() == 3;
					IL_0254:
					if (flag)
					{
						goto IL_0356;
					}
					string text = Translate.GetString("droid_AskLocationPermissionText");
					try
					{
						List<string> list = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
						list.RemoveAt(1);
						text = "";
						List<string>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								string text2 = enumerator.Current;
								text = text + text2 + "\n";
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
					}
					catch (Exception)
					{
					}
					taskAwaiter6 = welcomePage.DisplayAlert(Translate.GetString("droid_AskLocationPermissionTitle"), text, Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num = (num2 = 5);
						taskAwaiter4 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, WelcomePage2.<DroidAskForPermissions>d__7>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0347:
					if (taskAwaiter6.GetResult())
					{
						Permissions.RequestAsync<Permissions.LocationWhenInUse>();
					}
					IL_0356:;
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

			// Token: 0x06001756 RID: 5974 RVA: 0x000B2E68 File Offset: 0x000B1068
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A35 RID: 2613
			public int <>1__state;

			// Token: 0x04000A36 RID: 2614
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000A37 RID: 2615
			public WelcomePage2 <>4__this;

			// Token: 0x04000A38 RID: 2616
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x04000A39 RID: 2617
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020001BF RID: 447
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06001757 RID: 5975 RVA: 0x000B2E78 File Offset: 0x000B1078
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06001758 RID: 5976 RVA: 0x000B2EC4 File Offset: 0x000B10C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A3A RID: 2618
			public int <>1__state;

			// Token: 0x04000A3B RID: 2619
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x020001C0 RID: 448
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOptionsNext_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001759 RID: 5977 RVA: 0x000B2ED4 File Offset: 0x000B10D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2 welcomePage = this;
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
						goto IL_011A;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C1;
					}
					default:
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.WiFi)
						{
							taskAwaiter = welcomePage.DroidAskForPermissions().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnOptionsNext_Clicked>d__5>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
						{
							taskAwaiter = welcomePage.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle"), Translate.GetString("ios_NoBTLE_DeviceSelectedText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnOptionsNext_Clicked>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_011A;
						}
						else
						{
							if ((SharedSettings.Current.ConnectionType != ConnectionTypes.Bluetooth && SharedSettings.Current.ConnectionType != ConnectionTypes.MFI_OBDLinkMXPlus) || !string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
							{
								goto IL_01C8;
							}
							taskAwaiter = welcomePage.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle"), Translate.GetString("ios_NoBT_DeviceSelectedText"), "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnOptionsNext_Clicked>d__5>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01C1;
						}
						break;
					}
					taskAwaiter.GetResult();
					goto IL_01C8;
					IL_011A:
					taskAwaiter.GetResult();
					goto IL_01C8;
					IL_01C1:
					taskAwaiter.GetResult();
					IL_01C8:
					if (string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
					{
						welcomePage.panelOptions.IsVisible = false;
						welcomePage.panelProfileSelector.IsVisible = true;
					}
					else
					{
						welcomePage.FinishSetupAndStartConnection();
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

			// Token: 0x0600175A RID: 5978 RVA: 0x000B3124 File Offset: 0x000B1324
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A3C RID: 2620
			public int <>1__state;

			// Token: 0x04000A3D RID: 2621
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A3E RID: 2622
			public WelcomePage2 <>4__this;

			// Token: 0x04000A3F RID: 2623
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001C1 RID: 449
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectDevice_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600175B RID: 5979 RVA: 0x000B3134 File Offset: 0x000B1334
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				WelcomePage2 welcomePage = this;
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
						num = (num2 = -1);
						break;
					}
					case 1:
						IL_00A8:
						try
						{
							if (num != 1)
							{
								if (!PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0) || !PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
								{
									goto IL_0151;
								}
								taskAwaiter = welcomePage.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle"), Translate.GetString("ios_NoBluetoothPermissionText"), "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
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
							PlatformHelper.CommonService.OpenPermissionsSettings();
							goto IL_03B8;
						}
						catch (Exception)
						{
						}
						goto IL_0151;
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01D0;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0236;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_030B;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_036E;
					}
					default:
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_008E;
						}
						taskAwaiter = welcomePage.DroidAskForPermissions().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_008E:
					if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE)
					{
						if (PlatformHelper.IsiOS)
						{
							goto IL_00A8;
						}
					}
					else
					{
						if (SharedSettings.Current.ConnectionType != ConnectionTypes.Bluetooth && SharedSettings.Current.ConnectionType != ConnectionTypes.MFI_OBDLinkMXPlus)
						{
							goto IL_039F;
						}
						welcomePage.activityFramebt2.IsVisible = true;
						if (sender is Button)
						{
							(sender as Button).IsEnabled = false;
						}
						taskAwaiter = Task.Delay(100).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_030B;
					}
					IL_0151:
					welcomePage.activityFramebtle.IsVisible = true;
					if (sender is Button)
					{
						(sender as Button).IsEnabled = false;
					}
					taskAwaiter = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
						return;
					}
					IL_01D0:
					taskAwaiter.GetResult();
					taskAwaiter = welcomePage.Navigation.PushAsync(new BTLEDeviceSelectorPage(), true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
						return;
					}
					IL_0236:
					taskAwaiter.GetResult();
					welcomePage.activityFramebtle.IsVisible = false;
					if (sender is Button)
					{
						(sender as Button).IsEnabled = true;
						goto IL_039F;
					}
					goto IL_039F;
					IL_030B:
					taskAwaiter.GetResult();
					taskAwaiter = welcomePage.Navigation.PushAsync(new BTDeviceSelectorPage(), true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, WelcomePage2.<btnSelectDevice_Clicked>d__6>(ref taskAwaiter, ref this);
						return;
					}
					IL_036E:
					taskAwaiter.GetResult();
					welcomePage.activityFramebt2.IsVisible = false;
					if (sender is Button)
					{
						(sender as Button).IsEnabled = true;
					}
					IL_039F:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_03B8:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600175C RID: 5980 RVA: 0x000B3540 File Offset: 0x000B1740
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A40 RID: 2624
			public int <>1__state;

			// Token: 0x04000A41 RID: 2625
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A42 RID: 2626
			public WelcomePage2 <>4__this;

			// Token: 0x04000A43 RID: 2627
			public object sender;

			// Token: 0x04000A44 RID: 2628
			private TaskAwaiter <>u__1;
		}
	}
}
