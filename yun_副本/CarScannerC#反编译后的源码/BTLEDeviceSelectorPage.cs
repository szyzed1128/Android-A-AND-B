using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Plugin.BLE.Abstractions.Contracts;
using Syncfusion.ListView.XForms;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000158 RID: 344
	[XamlFilePath("Settings\\BTLEDeviceSelectorPage.xaml")]
	public class BTLEDeviceSelectorPage : ContentPage
	{
		// Token: 0x060014F9 RID: 5369 RVA: 0x00081928 File Offset: 0x0007FB28
		public BTLEDeviceSelectorPage()
		{
			BTLEDeviceSelectorPage.Instance = this;
			this.InitializeComponent();
			base.Disappearing += this.Page_Disappearing;
			this.model = new BTLEDeviceSelectorViewModel();
			if (!PlatformHelper.IsAndroid)
			{
				this.droidWarningLabel.IsVisible = false;
			}
			base.BindingContext = this.model;
			this.model.DiscoverDevices.Execute(null);
			PermissionStatus hasGPSPermission = this.HasGPSPermission;
			base.Appearing += this.BTLEDeviceSelectorPage_Appearing;
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x000819B0 File Offset: 0x0007FBB0
		private async void BTLEDeviceSelectorPage_Appearing(object sender, EventArgs e)
		{
			this.UpdateAndroid12BluetoothPermissionStatus();
			if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0) && !this.Droid12BTWarningShowed)
			{
				this.Droid12BTWarningShowed = true;
				PermissionStatus permissionStatus = await PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async();
				PermissionStatus btStatus = permissionStatus;
				if (btStatus != 3)
				{
					permissionStatus = await PlatformHelper.DroidService.RequestBluetoothPermissionAndroid12Async();
					btStatus = permissionStatus;
					if (btStatus != 3)
					{
						await base.DisplayAlert(Translate.GetString("droid_Android12BluetoothPermissionMissing_Title"), Translate.GetString("droid_NearbyDevicesExplanation") + "\n" + Translate.GetString("droid_Android12BluetoothPermissionMissing_Text"), "OK");
					}
					this.UpdateAndroid12BluetoothPermissionStatus();
					if (btStatus == 3)
					{
						this.model.DiscoverDevices.Execute(null);
					}
				}
			}
			await PermissionHelper.CheckLocationPermissionStatus(async delegate(PermissionStatus status)
			{
				this.HasGPSPermission = status;
				if (PlatformHelper.IsAndroid && this.HasGPSPermission != 3 && !SharedSettings.Current.BluetoothLocationWarningShowed)
				{
					SharedSettings.Current.BluetoothLocationWarningShowed = true;
					await base.DisplayAlert("Car Scanner", Translate.GetString("droid_LocationRequiredForBluetooth"), "OK");
					this.HasGPSPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
					if (this.HasGPSPermission == 3)
					{
						IBTLEDeviceSelectorViewModel ibtledeviceSelectorViewModel = this.model;
						if (ibtledeviceSelectorViewModel != null)
						{
							ICommand discoverDevices = ibtledeviceSelectorViewModel.DiscoverDevices;
							if (discoverDevices != null)
							{
								discoverDevices.Execute(this);
							}
						}
					}
				}
			});
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x000819E8 File Offset: 0x0007FBE8
		private async void UpdateAndroid12BluetoothPermissionStatus()
		{
			if (PlatformHelper.IsAndroid)
			{
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					PermissionStatus permissionStatus = await PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async();
					this.Android12HasBluetoothPermission = permissionStatus;
				}
				else
				{
					this.Android12HasBluetoothPermission = 3;
				}
			}
			else
			{
				this.Android12HasBluetoothPermission = 3;
			}
		}

		// Token: 0x17000F3C RID: 3900
		// (get) Token: 0x060014FC RID: 5372 RVA: 0x00081A1F File Offset: 0x0007FC1F
		// (set) Token: 0x060014FD RID: 5373 RVA: 0x00081A27 File Offset: 0x0007FC27
		public PermissionStatus Android12HasBluetoothPermission
		{
			get
			{
				return this._Android12HasBluetoothPermission;
			}
			set
			{
				this._Android12HasBluetoothPermission = value;
				this.OnPropertyChanged("Android12HasBluetoothPermission");
				this.OnPropertyChanged("DisplayPermissionsButton");
			}
		}

		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060014FE RID: 5374 RVA: 0x00081A46 File Offset: 0x0007FC46
		public bool DisplayPermissionsButton
		{
			get
			{
				return this.Android12HasBluetoothPermission != 3 || this.HasGPSPermission != 3;
			}
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00081A60 File Offset: 0x0007FC60
		private void Page_Disappearing(object sender, EventArgs e)
		{
			try
			{
				if (base.Navigation.ModalStack.LastOrDefault<Page>() == this)
				{
					base.Navigation.PopAsync();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06001500 RID: 5376 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001501 RID: 5377 RVA: 0x00081AA4 File Offset: 0x0007FCA4
		private async void btnDeviceTest_Clicked(object sender, EventArgs e)
		{
			IDevice device = (sender as MenuItem).BindingContext as IDevice;
			BTLEDeviceDescription deviceDescription = null;
			int num = 0;
			try
			{
				TaskAwaiter<bool> taskAwaiter = this.model.TestDevice(device).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					await base.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + this.model.ErrorMessage, "OK");
					return;
				}
				deviceDescription = this.model.DeviceForTest;
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception ex = (Exception)obj;
				await base.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + this.model.ErrorMessage, "OK");
			}
			obj = null;
			SharedSettings.Current.BTLEDeviceName = device.Name;
			SharedSettings.Current.BTLEDeviceID = device.Id.ToString();
			SharedSettings.Current.BTLEInputID = deviceDescription.InputID;
			SharedSettings.Current.BTLEOutputID = deviceDescription.OutputID;
			SharedSettings.Current.BTLEServiceID = deviceDescription.ServiceID;
			await base.Navigation.PopAsync();
		}

		// Token: 0x06001502 RID: 5378 RVA: 0x00081AE4 File Offset: 0x0007FCE4
		private async void btnDeviceInfo_Clicked(object sender, EventArgs e)
		{
			IDevice device = (sender as BindableObject).BindingContext as IDevice;
			string text = await this.model.GetDeviceInfo(device);
			await base.DisplayAlert("Info", text, "OK");
		}

		// Token: 0x06001503 RID: 5379 RVA: 0x00081B24 File Offset: 0x0007FD24
		private async void lvDevices_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			IDevice device = e.ItemData as IDevice;
			this.lvDevices.SelectedItem = null;
			IReadOnlyList<BTLEDeviceDescription> getKnownDeviceDefinitions = BTLEDeviceDescription.GetKnownDeviceDefinitions;
			if (device != null)
			{
				BTLEDeviceDescription deviceDescription = null;
				if (!string.IsNullOrEmpty(device.Name))
				{
					deviceDescription = getKnownDeviceDefinitions.FirstOrDefault((BTLEDeviceDescription x) => device.Name.ToLower().Contains(x.NamePart.ToLower()));
				}
				if (deviceDescription == null)
				{
					int num = 0;
					try
					{
						TaskAwaiter<bool> taskAwaiter = this.model.TestDevice(device).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							await base.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + this.model.ErrorMessage, "OK");
							return;
						}
						deviceDescription = this.model.DeviceForTest;
					}
					catch (Exception obj)
					{
						num = 1;
					}
					object obj;
					if (num == 1)
					{
						Exception ex = (Exception)obj;
						await base.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + this.model.ErrorMessage, "OK");
					}
					obj = null;
				}
				SharedSettings.Current.BTLEDeviceName = device.Name;
				SharedSettings.Current.BTLEDeviceID = device.Id.ToString();
				SharedSettings.Current.BTLEInputID = deviceDescription.InputID;
				SharedSettings.Current.BTLEOutputID = deviceDescription.OutputID;
				SharedSettings.Current.BTLEServiceID = deviceDescription.ServiceID;
				if (device.Name == "CAR2LS ScanX")
				{
					ScanXChecker.CheckAtDeviceSelection(device.Id.ToString(), device.Name);
				}
				else
				{
					if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated)
					{
						SharedSettings.Current.AdsProductPurchased = false;
						SimpleMainPage instance = SimpleMainPage.Instance;
						if (instance != null)
						{
							instance.UpdateMainButtons();
						}
					}
					SharedSettings.Current.WhitelistDeviceActivated = false;
				}
				if (WrongDeviceChecker.BAD_ELM327_NAMES.Contains(device.Name))
				{
					await base.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK");
				}
				await base.Navigation.PopAsync();
			}
		}

		// Token: 0x06001504 RID: 5380 RVA: 0x00026430 File Offset: 0x00024630
		private void btnCancel_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00081B63 File Offset: 0x0007FD63
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnCancel_Clicked(this, null);
				return true;
			}
			return true;
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00081B94 File Offset: 0x0007FD94
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			string text = Translate.GetString("ios_BTLE_Text");
			if (Device.RuntimePlatform == "Android")
			{
				text = Translate.GetString("droid_BTLE_Text");
			}
			base.DisplayAlert(Translate.GetString("ios_BTLE_Title"), text, "OK");
		}

		// Token: 0x06001507 RID: 5383 RVA: 0x0007CD3B File Offset: 0x0007AF3B
		private void BtnLocationPermissions_Clicked(object sender, EventArgs e)
		{
			PermissionHelper.OpenPermissionsSettings();
		}

		// Token: 0x17000F3E RID: 3902
		// (get) Token: 0x06001508 RID: 5384 RVA: 0x00081BE0 File Offset: 0x0007FDE0
		// (set) Token: 0x06001509 RID: 5385 RVA: 0x00081C35 File Offset: 0x0007FE35
		public PermissionStatus HasGPSPermission
		{
			get
			{
				if (PlatformHelper.IsiOS)
				{
					return 3;
				}
				if (PlatformHelper.IsAndroid)
				{
					if (!PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
					{
						return 3;
					}
					if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
					{
						return 3;
					}
				}
				if (this._HasGPSPermission == null)
				{
					PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
					{
						this.HasGPSPermission = status;
					});
				}
				return this._HasGPSPermission;
			}
			private set
			{
				this._HasGPSPermission = value;
				this.OnPropertyChanged("HasGPSPermission");
			}
		}

		// Token: 0x0600150A RID: 5386 RVA: 0x00081C4C File Offset: 0x0007FE4C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/BTLEDeviceSelectorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			GuidToStringConverter guidToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(guidToStringConverter = new GuidToStringConverter(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			EmptyStringToNonameConverter emptyStringToNonameConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToNonameConverter = new EmptyStringToNonameConverter(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			BTLENameToVisibleBoolConverter btlenameToVisibleBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(btlenameToVisibleBoolConverter = new BTLENameToVisibleBoolConverter(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			PermissionStatus permissionStatus = 0;
			PermissionStatus permissionStatus2 = 1;
			PermissionStatus permissionStatus3 = 2;
			PermissionStatus permissionStatus4 = 4;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 17);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 26);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 18);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 53);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 53);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 25);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 25);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 14);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 22);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 17);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 17);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 17);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 14);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 21);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 21);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 29);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 26);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 14);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 17);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 17);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("me", this);
			if (this.StyleId == null)
			{
				this.StyleId = "me";
			}
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("droid12WarningLabel", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "droid12WarningLabel";
			}
			nameScope.RegisterName("droidWarningLabel", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "droidWarningLabel";
			}
			nameScope.RegisterName("lvDevices", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lvDevices";
			}
			nameScope.RegisterName("LeftSwipeTemplate", dataTemplate);
			nameScope.RegisterName("btnLocationPermissions", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnLocationPermissions";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.me = this;
			this.gridButtons = grid;
			this.droid12WarningLabel = label2;
			this.droidWarningLabel = label3;
			this.lvDevices = sfListView;
			this.LeftSwipeTemplate = dataTemplate;
			this.btnLocationPermissions = button2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("GuidToStringConverter", guidToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EmptyStringToNonameConverter", emptyStringToNonameConverter);
			resourceDictionary.Add("BTLENameToVisibleBoolConverter", btlenameToVisibleBoolConverter);
			enumToBoolConverter.TrueValues.Add(permissionStatus);
			enumToBoolConverter.TrueValues.Add(permissionStatus2);
			enumToBoolConverter.TrueValues.Add(permissionStatus3);
			enumToBoolConverter.TrueValues.Add(permissionStatus4);
			IMarkupExtension<IValueConverter> markupExtension = enumToBoolConverter;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 2];
			array[0] = resourceDictionary;
			array[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, null, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("GPSStateNotGrantedToTrueConverter", valueConverter);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver2.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			onPlatform.Android = new Thickness(0.0);
			onPlatform.iOS = new Thickness(0.0);
			this.SetValue(Page.PaddingProperty, onPlatform);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnCancel_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver3.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Cancel";
			IMarkupExtension markupExtension4 = translate;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver4.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver5.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 17)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_BTLESelectDevice.Content";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = label;
			array6[1] = grid;
			array6[2] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver6.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 17)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.Text = obj8;
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension4.Key = "NB_info";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Button.ImageProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver7.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 17)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource4.Key);
			dynamicResourceExtension5.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = linkButton2;
			array8[1] = grid;
			array8[2] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver8.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 17)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource5.Key);
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "0";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on2);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform3.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			label2.SetValue(Grid.RowProperty, 1);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(Label.TextColorProperty, Color.Red);
			span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate3.Text = "droid_Android12BluetoothPermissionMissing_Title";
			IMarkupExtension markupExtension9 = translate3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label2;
			array9[3] = grid2;
			array9[4] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver9.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 53)));
			object obj12 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span.Text = obj12;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate4.Text = "droid_Android12BluetoothPermissionMissing_Text";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = span2;
			array10[1] = formattedString;
			array10[2] = label2;
			array10[3] = grid2;
			array10[4] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Span.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver10.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 53)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			span2.Text = obj14;
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			staticResourceExtension.Key = "GPSStateNotGrantedToTrueConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = bindingExtension;
			array11[1] = label2;
			array11[2] = grid2;
			array11[3] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver11.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 25)));
			object obj16 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension.Converter = obj16;
			bindingExtension.Mode = 2;
			bindingExtension.Path = "Android12HasBluetoothPermission";
			referenceExtension.Name = "me";
			IMarkupExtension markupExtension12 = referenceExtension;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = bindingExtension;
			array12[1] = label2;
			array12[2] = grid2;
			array12[3] = this;
			object obj17;
			xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver12.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 25)));
			object obj18 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension.Source = obj18;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid2.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 2);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate5.Text = "droid_LocationRequiredForBluetooth";
			IMarkupExtension markupExtension13 = translate5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = label3;
			array13[1] = grid2;
			array13[2] = this;
			object obj19;
			xamlServiceProvider13.Add(typeFromHandle25, obj19 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver13.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 17)));
			object obj20 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label3.Text = obj20;
			label3.SetValue(Label.TextColorProperty, Color.Red);
			staticResourceExtension2.Key = "GPSStateNotGrantedToTrueConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = bindingExtension2;
			array14[1] = label3;
			array14[2] = grid2;
			array14[3] = this;
			object obj21;
			xamlServiceProvider14.Add(typeFromHandle27, obj21 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver14.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 25)));
			object obj22 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension2.Converter = obj22;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "HasGPSPermission";
			referenceExtension2.Name = "me";
			IMarkupExtension markupExtension15 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = bindingExtension2;
			array15[1] = label3;
			array15[2] = grid2;
			array15[3] = this;
			object obj23;
			xamlServiceProvider15.Add(typeFromHandle29, obj23 = new SimpleValueTargetProvider(array15, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver15.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 25)));
			object obj24 = markupExtension15.ProvideValue(xamlServiceProvider15);
			bindingExtension2.Source = obj24;
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			grid2.Children.Add(label3);
			sfListView.SetValue(Grid.RowProperty, 3);
			sfListView.SetValue(SfListView.AllowSwipingProperty, true);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension3.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = bindingExtension3;
			array16[1] = sfListView;
			array16[2] = grid2;
			array16[3] = this;
			object obj25;
			xamlServiceProvider16.Add(typeFromHandle31, obj25 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver16.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 17)));
			object obj26 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension3.Converter = obj26;
			bindingExtension3.Path = "IsTestingDevice";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			sfListView.SetBinding(VisualElement.IsEnabledProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "IsBluetoothOn";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			sfListView.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.lvDevices_ItemTapped);
			bindingExtension5.Path = "DeviceList";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase5);
			sfListView.SetValue(SfListView.SelectionModeProperty, 3);
			IDataTemplate dataTemplate3 = dataTemplate;
			BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_80 <InitializeComponent>_anonXamlCDataTemplate_ = new BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_80();
			object[] array17 = new object[0 + 4];
			array17[0] = dataTemplate;
			array17[1] = sfListView;
			array17[2] = grid2;
			array17[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array17;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.LeftSwipeTemplateProperty, dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81 <InitializeComponent>_anonXamlCDataTemplate_2 = new BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81();
			object[] array18 = new object[0 + 4];
			array18[0] = dataTemplate2;
			array18[1] = sfListView;
			array18[2] = grid2;
			array18[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array18;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate2);
			grid2.Children.Add(sfListView);
			label4.SetValue(Grid.RowProperty, 3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension6.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 3];
			array19[0] = label4;
			array19[1] = grid2;
			array19[2] = this;
			object obj27;
			xamlServiceProvider17.Add(typeFromHandle33, obj27 = new SimpleValueTargetProvider(array19, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver17.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(228, 17)));
			DynamicResource dynamicResource6 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension6.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = bindingExtension6;
			array20[1] = label4;
			array20[2] = grid2;
			array20[3] = this;
			object obj28;
			xamlServiceProvider18.Add(typeFromHandle35, obj28 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver18.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(231, 17)));
			object obj29 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension6.Converter = obj29;
			bindingExtension6.Path = "IsBluetoothOn";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			translate6.Text = "ios_TurnOnBluetooth";
			IMarkupExtension markupExtension19 = translate6;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 3];
			array21[0] = label4;
			array21[1] = grid2;
			array21[2] = this;
			object obj30;
			xamlServiceProvider19.Add(typeFromHandle37, obj30 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver19.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 17)));
			object obj31 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label4.Text = obj31;
			label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid2.Children.Add(label4);
			stackLayout.SetValue(Grid.RowProperty, 4);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			button.SetValue(Grid.RowProperty, 2);
			bindingExtension7.Path = "DiscoverDevices";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			button.SetBinding(Button.CommandProperty, bindingBase7);
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension8.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = bindingExtension8;
			array22[1] = button;
			array22[2] = stackLayout;
			array22[3] = grid2;
			array22[4] = this;
			object obj32;
			xamlServiceProvider20.Add(typeFromHandle39, obj32 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver20.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 21)));
			object obj33 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension8.Converter = obj33;
			bindingExtension8.Path = "IsTestingDevice";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			button.SetBinding(VisualElement.IsEnabledProperty, bindingBase8);
			translate7.Text = "ios_Refresh";
			IMarkupExtension markupExtension21 = translate7;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = button;
			array23[1] = stackLayout;
			array23[2] = grid2;
			array23[3] = this;
			object obj34;
			xamlServiceProvider21.Add(typeFromHandle41, obj34 = new SimpleValueTargetProvider(array23, Button.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver21.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 21)));
			object obj35 = markupExtension21.ProvideValue(xamlServiceProvider21);
			button.Text = obj35;
			stackLayout.Children.Add(button);
			button2.Clicked += this.BtnLocationPermissions_Clicked;
			translate8.Text = "ios_Permissions";
			IMarkupExtension markupExtension22 = translate8;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = button2;
			array24[1] = stackLayout;
			array24[2] = grid2;
			array24[3] = this;
			object obj36;
			xamlServiceProvider22.Add(typeFromHandle43, obj36 = new SimpleValueTargetProvider(array24, Button.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver22.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(250, 21)));
			object obj37 = markupExtension22.ProvideValue(xamlServiceProvider22);
			button2.Text = obj37;
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "DisplayPermissionsButton";
			referenceExtension3.Name = "me";
			IMarkupExtension markupExtension23 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = bindingExtension9;
			array25[1] = button2;
			array25[2] = stackLayout;
			array25[3] = grid2;
			array25[4] = this;
			object obj38;
			xamlServiceProvider23.Add(typeFromHandle45, obj38 = new SimpleValueTargetProvider(array25, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver23.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(BTLEDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(256, 29)));
			object obj39 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension9.Source = obj39;
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			button2.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			stackLayout.Children.Add(button2);
			grid2.Children.Add(stackLayout);
			activityFrame.SetValue(Grid.RowProperty, 3);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "IsTestingDevice";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			activityFrame.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "TestingDeviceString";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			activityFrame.SetBinding(ActivityFrame.TextProperty, bindingBase11);
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x000850BC File Offset: 0x000832BC
		[CompilerGenerated]
		private async void <BTLEDeviceSelectorPage_Appearing>b__4_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
			if (PlatformHelper.IsAndroid && this.HasGPSPermission != 3 && !SharedSettings.Current.BluetoothLocationWarningShowed)
			{
				SharedSettings.Current.BluetoothLocationWarningShowed = true;
				await base.DisplayAlert("Car Scanner", Translate.GetString("droid_LocationRequiredForBluetooth"), "OK");
				this.HasGPSPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
				if (this.HasGPSPermission == 3)
				{
					IBTLEDeviceSelectorViewModel ibtledeviceSelectorViewModel = this.model;
					if (ibtledeviceSelectorViewModel != null)
					{
						ICommand discoverDevices = ibtledeviceSelectorViewModel.DiscoverDevices;
						if (discoverDevices != null)
						{
							discoverDevices.Execute(this);
						}
					}
				}
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x000850FB File Offset: 0x000832FB
		[CompilerGenerated]
		private void <get_HasGPSPermission>b__22_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
		}

		// Token: 0x0600150D RID: 5389 RVA: 0x00085104 File Offset: 0x00083304
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<BTLEDeviceSelectorPage>(this, typeof(BTLEDeviceSelectorPage));
			this.me = NameScopeExtensions.FindByName<ContentPage>(this, "me");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.droid12WarningLabel = NameScopeExtensions.FindByName<Label>(this, "droid12WarningLabel");
			this.droidWarningLabel = NameScopeExtensions.FindByName<Label>(this, "droidWarningLabel");
			this.lvDevices = NameScopeExtensions.FindByName<SfListView>(this, "lvDevices");
			this.LeftSwipeTemplate = NameScopeExtensions.FindByName<DataTemplate>(this, "LeftSwipeTemplate");
			this.btnLocationPermissions = NameScopeExtensions.FindByName<Button>(this, "btnLocationPermissions");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x0400055A RID: 1370
		public static BTLEDeviceSelectorPage Instance;

		// Token: 0x0400055B RID: 1371
		private IBTLEDeviceSelectorViewModel model;

		// Token: 0x0400055C RID: 1372
		private bool Droid12BTWarningShowed;

		// Token: 0x0400055D RID: 1373
		private PermissionStatus _Android12HasBluetoothPermission;

		// Token: 0x0400055E RID: 1374
		private PermissionStatus _HasGPSPermission;

		// Token: 0x0400055F RID: 1375
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage me;

		// Token: 0x04000560 RID: 1376
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000561 RID: 1377
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label droid12WarningLabel;

		// Token: 0x04000562 RID: 1378
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label droidWarningLabel;

		// Token: 0x04000563 RID: 1379
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvDevices;

		// Token: 0x04000564 RID: 1380
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private DataTemplate LeftSwipeTemplate;

		// Token: 0x04000565 RID: 1381
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnLocationPermissions;

		// Token: 0x04000566 RID: 1382
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000159 RID: 345
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<BTLEDeviceSelectorPage_Appearing>b__4_0>d : IAsyncStateMachine
		{
			// Token: 0x0600150E RID: 5390 RVA: 0x000851AC File Offset: 0x000833AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<PermissionStatus> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
							goto IL_011C;
						}
						btledeviceSelectorPage.HasGPSPermission = status;
						if (!PlatformHelper.IsAndroid || btledeviceSelectorPage.HasGPSPermission == 3 || SharedSettings.Current.BluetoothLocationWarningShowed)
						{
							goto IL_0151;
						}
						SharedSettings.Current.BluetoothLocationWarningShowed = true;
						taskAwaiter3 = btledeviceSelectorPage.DisplayAlert("Car Scanner", Translate.GetString("droid_LocationRequiredForBluetooth"), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<<BTLEDeviceSelectorPage_Appearing>b__4_0>d>(ref taskAwaiter3, ref this);
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
					taskAwaiter = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTLEDeviceSelectorPage.<<BTLEDeviceSelectorPage_Appearing>b__4_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_011C:
					PermissionStatus result = taskAwaiter.GetResult();
					btledeviceSelectorPage.HasGPSPermission = result;
					if (btledeviceSelectorPage.HasGPSPermission == 3)
					{
						IBTLEDeviceSelectorViewModel model = btledeviceSelectorPage.model;
						if (model != null)
						{
							ICommand discoverDevices = model.DiscoverDevices;
							if (discoverDevices != null)
							{
								discoverDevices.Execute(btledeviceSelectorPage);
							}
						}
					}
					IL_0151:;
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

			// Token: 0x0600150F RID: 5391 RVA: 0x00085354 File Offset: 0x00083554
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000567 RID: 1383
			public int <>1__state;

			// Token: 0x04000568 RID: 1384
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000569 RID: 1385
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x0400056A RID: 1386
			public PermissionStatus status;

			// Token: 0x0400056B RID: 1387
			private TaskAwaiter <>u__1;

			// Token: 0x0400056C RID: 1388
			private TaskAwaiter<PermissionStatus> <>u__2;
		}

		// Token: 0x0200015A RID: 346
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06001510 RID: 5392 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06001511 RID: 5393 RVA: 0x00085362 File Offset: 0x00083562
			internal bool <lvDevices_ItemTapped>b__0(BTLEDeviceDescription x)
			{
				return this.device.Name.ToLower().Contains(x.NamePart.ToLower());
			}

			// Token: 0x0400056D RID: 1389
			public IDevice device;
		}

		// Token: 0x0200015B RID: 347
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BTLEDeviceSelectorPage_Appearing>d__4 : IAsyncStateMachine
		{
			// Token: 0x06001512 RID: 5394 RVA: 0x00085384 File Offset: 0x00083584
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
						goto IL_011E;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01BD;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0243;
					}
					default:
						btledeviceSelectorPage.UpdateAndroid12BluetoothPermissionStatus();
						if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0) || btledeviceSelectorPage.Droid12BTWarningShowed)
						{
							goto IL_01E4;
						}
						btledeviceSelectorPage.Droid12BTWarningShowed = true;
						taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTLEDeviceSelectorPage.<BTLEDeviceSelectorPage_Appearing>d__4>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					PermissionStatus permissionStatus = taskAwaiter.GetResult();
					btStatus = permissionStatus;
					if (btStatus == 3)
					{
						goto IL_01E4;
					}
					taskAwaiter = PlatformHelper.DroidService.RequestBluetoothPermissionAndroid12Async().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTLEDeviceSelectorPage.<BTLEDeviceSelectorPage_Appearing>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_011E:
					permissionStatus = taskAwaiter.GetResult();
					btStatus = permissionStatus;
					if (btStatus == 3)
					{
						goto IL_01C4;
					}
					taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("droid_Android12BluetoothPermissionMissing_Title"), Translate.GetString("droid_NearbyDevicesExplanation") + "\n" + Translate.GetString("droid_Android12BluetoothPermissionMissing_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<BTLEDeviceSelectorPage_Appearing>d__4>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01BD:
					taskAwaiter3.GetResult();
					IL_01C4:
					btledeviceSelectorPage.UpdateAndroid12BluetoothPermissionStatus();
					if (btStatus == 3)
					{
						btledeviceSelectorPage.model.DiscoverDevices.Execute(null);
					}
					IL_01E4:
					taskAwaiter3 = PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
					{
						BTLEDeviceSelectorPage.<<BTLEDeviceSelectorPage_Appearing>b__4_0>d <<BTLEDeviceSelectorPage_Appearing>b__4_0>d;
						<<BTLEDeviceSelectorPage_Appearing>b__4_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<BTLEDeviceSelectorPage_Appearing>b__4_0>d.<>4__this = btledeviceSelectorPage;
						<<BTLEDeviceSelectorPage_Appearing>b__4_0>d.status = status;
						<<BTLEDeviceSelectorPage_Appearing>b__4_0>d.<>1__state = -1;
						<<BTLEDeviceSelectorPage_Appearing>b__4_0>d.<>t__builder.Start<BTLEDeviceSelectorPage.<<BTLEDeviceSelectorPage_Appearing>b__4_0>d>(ref <<BTLEDeviceSelectorPage_Appearing>b__4_0>d);
					}).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<BTLEDeviceSelectorPage_Appearing>d__4>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0243:
					taskAwaiter3.GetResult();
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

			// Token: 0x06001513 RID: 5395 RVA: 0x00085628 File Offset: 0x00083828
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400056E RID: 1390
			public int <>1__state;

			// Token: 0x0400056F RID: 1391
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000570 RID: 1392
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x04000571 RID: 1393
			private PermissionStatus <btStatus>5__2;

			// Token: 0x04000572 RID: 1394
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x04000573 RID: 1395
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200015C RID: 348
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateAndroid12BluetoothPermissionStatus>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001514 RID: 5396 RVA: 0x00085638 File Offset: 0x00083838
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							btledeviceSelectorPage.Android12HasBluetoothPermission = 3;
							goto IL_0098;
						}
						if (!PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							btledeviceSelectorPage.Android12HasBluetoothPermission = 3;
							goto IL_0098;
						}
						taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTLEDeviceSelectorPage.<UpdateAndroid12BluetoothPermissionStatus>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<PermissionStatus> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					PermissionStatus result = taskAwaiter.GetResult();
					btledeviceSelectorPage.Android12HasBluetoothPermission = result;
					IL_0098:;
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

			// Token: 0x06001515 RID: 5397 RVA: 0x0008571C File Offset: 0x0008391C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000574 RID: 1396
			public int <>1__state;

			// Token: 0x04000575 RID: 1397
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000576 RID: 1398
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x04000577 RID: 1399
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x0200015D RID: 349
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDeviceInfo_Clicked>d__15 : IAsyncStateMachine
		{
			// Token: 0x06001516 RID: 5398 RVA: 0x0008572C File Offset: 0x0008392C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00F2;
						}
						IDevice device = (sender as BindableObject).BindingContext as IDevice;
						taskAwaiter3 = btledeviceSelectorPage.model.GetDeviceInfo(device).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, BTLEDeviceSelectorPage.<btnDeviceInfo_Clicked>d__15>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter3.GetResult();
					taskAwaiter = btledeviceSelectorPage.DisplayAlert("Info", result, "OK").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<btnDeviceInfo_Clicked>d__15>(ref taskAwaiter, ref this);
						return;
					}
					IL_00F2:
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

			// Token: 0x06001517 RID: 5399 RVA: 0x00085870 File Offset: 0x00083A70
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000578 RID: 1400
			public int <>1__state;

			// Token: 0x04000579 RID: 1401
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400057A RID: 1402
			public object sender;

			// Token: 0x0400057B RID: 1403
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x0400057C RID: 1404
			private TaskAwaiter<string> <>u__1;

			// Token: 0x0400057D RID: 1405
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200015E RID: 350
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDeviceTest_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x06001518 RID: 5400 RVA: 0x00085880 File Offset: 0x00083A80
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter4;
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<Page> taskAwaiter5;
					switch (num2)
					{
					case 0:
					case 1:
						break;
					case 2:
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = -1;
						goto IL_0217;
					case 3:
					{
						TaskAwaiter<Page> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<Page>);
						num3 = -1;
						goto IL_02F6;
					}
					default:
						device = (sender as MenuItem).BindingContext as IDevice;
						deviceDescription = null;
						num = 0;
						break;
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter7;
						if (num2 != 0)
						{
							if (num2 == 1)
							{
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = -1;
								goto IL_0158;
							}
							taskAwaiter7 = btledeviceSelectorPage.model.TestDevice(device).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num3 = 0;
								taskAwaiter2 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEDeviceSelectorPage.<btnDeviceTest_Clicked>d__14>(ref taskAwaiter7, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter7 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num3 = -1;
						}
						if (taskAwaiter7.GetResult())
						{
							deviceDescription = btledeviceSelectorPage.model.DeviceForTest;
							goto IL_0179;
						}
						taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + btledeviceSelectorPage.model.ErrorMessage, "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num3 = 1;
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<btnDeviceTest_Clicked>d__14>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0158:
						taskAwaiter3.GetResult();
						goto IL_0327;
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					IL_0179:
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_021E;
					}
					Exception ex2 = (Exception)obj;
					taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + btledeviceSelectorPage.model.ErrorMessage, "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num3 = 2;
						taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<btnDeviceTest_Clicked>d__14>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0217:
					taskAwaiter3.GetResult();
					IL_021E:
					obj = null;
					SharedSettings.Current.BTLEDeviceName = device.Name;
					SharedSettings.Current.BTLEDeviceID = device.Id.ToString();
					SharedSettings.Current.BTLEInputID = deviceDescription.InputID;
					SharedSettings.Current.BTLEOutputID = deviceDescription.OutputID;
					SharedSettings.Current.BTLEServiceID = deviceDescription.ServiceID;
					taskAwaiter5 = btledeviceSelectorPage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num3 = 3;
						TaskAwaiter<Page> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, BTLEDeviceSelectorPage.<btnDeviceTest_Clicked>d__14>(ref taskAwaiter5, ref this);
						return;
					}
					IL_02F6:
					taskAwaiter5.GetResult();
				}
				catch (Exception ex3)
				{
					num3 = -2;
					device = null;
					deviceDescription = null;
					this.<>t__builder.SetException(ex3);
					return;
				}
				IL_0327:
				num3 = -2;
				device = null;
				deviceDescription = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001519 RID: 5401 RVA: 0x00085C0C File Offset: 0x00083E0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400057E RID: 1406
			public int <>1__state;

			// Token: 0x0400057F RID: 1407
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000580 RID: 1408
			public object sender;

			// Token: 0x04000581 RID: 1409
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x04000582 RID: 1410
			private IDevice <device>5__2;

			// Token: 0x04000583 RID: 1411
			private BTLEDeviceDescription <deviceDescription>5__3;

			// Token: 0x04000584 RID: 1412
			private object <>7__wrap3;

			// Token: 0x04000585 RID: 1413
			private int <>7__wrap4;

			// Token: 0x04000586 RID: 1414
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000587 RID: 1415
			private TaskAwaiter <>u__2;

			// Token: 0x04000588 RID: 1416
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x0200015F RID: 351
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lvDevices_ItemTapped>d__16 : IAsyncStateMachine
		{
			// Token: 0x0600151A RID: 5402 RVA: 0x00085C1C File Offset: 0x00083E1C
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				BTLEDeviceSelectorPage btledeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter4;
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<Page> taskAwaiter5;
					switch (num2)
					{
					case 0:
					case 1:
						break;
					case 2:
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = -1;
						goto IL_0294;
					case 3:
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = -1;
						goto IL_0441;
					case 4:
					{
						TaskAwaiter<Page> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<Page>);
						num3 = -1;
						goto IL_04A1;
					}
					default:
					{
						CS$<>8__locals1 = new BTLEDeviceSelectorPage.<>c__DisplayClass16_0();
						CS$<>8__locals1.device = e.ItemData as IDevice;
						btledeviceSelectorPage.lvDevices.SelectedItem = null;
						IReadOnlyList<BTLEDeviceDescription> getKnownDeviceDefinitions = BTLEDeviceDescription.GetKnownDeviceDefinitions;
						if (CS$<>8__locals1.device == null)
						{
							goto IL_04D2;
						}
						deviceDescription = null;
						if (!string.IsNullOrEmpty(CS$<>8__locals1.device.Name))
						{
							deviceDescription = getKnownDeviceDefinitions.FirstOrDefault((BTLEDeviceDescription x) => CS$<>8__locals1.device.Name.ToLower().Contains(x.NamePart.ToLower()));
						}
						if (deviceDescription != null)
						{
							goto IL_02A2;
						}
						num = 0;
						break;
					}
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter7;
						if (num2 != 0)
						{
							if (num2 == 1)
							{
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = -1;
								goto IL_01D2;
							}
							taskAwaiter7 = btledeviceSelectorPage.model.TestDevice(CS$<>8__locals1.device).GetAwaiter();
							if (!taskAwaiter7.IsCompleted)
							{
								num3 = 0;
								taskAwaiter2 = taskAwaiter7;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTLEDeviceSelectorPage.<lvDevices_ItemTapped>d__16>(ref taskAwaiter7, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter7 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num3 = -1;
						}
						if (taskAwaiter7.GetResult())
						{
							deviceDescription = btledeviceSelectorPage.model.DeviceForTest;
							goto IL_01F3;
						}
						taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + btledeviceSelectorPage.model.ErrorMessage, "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num3 = 1;
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<lvDevices_ItemTapped>d__16>(ref taskAwaiter3, ref this);
							return;
						}
						IL_01D2:
						taskAwaiter3.GetResult();
						goto IL_04D2;
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					IL_01F3:
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_029B;
					}
					Exception ex2 = (Exception)obj;
					taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("ios_BTLEDeviceNotSupportedTitle"), Translate.GetString("ios_BTLEDeviceNotSupportedText") + "\n" + btledeviceSelectorPage.model.ErrorMessage, "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num3 = 2;
						taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<lvDevices_ItemTapped>d__16>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0294:
					taskAwaiter3.GetResult();
					IL_029B:
					obj = null;
					IL_02A2:
					SharedSettings.Current.BTLEDeviceName = CS$<>8__locals1.device.Name;
					SharedSettings.Current.BTLEDeviceID = CS$<>8__locals1.device.Id.ToString();
					SharedSettings.Current.BTLEInputID = deviceDescription.InputID;
					SharedSettings.Current.BTLEOutputID = deviceDescription.OutputID;
					SharedSettings.Current.BTLEServiceID = deviceDescription.ServiceID;
					if (CS$<>8__locals1.device.Name == "CAR2LS ScanX")
					{
						ScanXChecker.CheckAtDeviceSelection(CS$<>8__locals1.device.Id.ToString(), CS$<>8__locals1.device.Name);
					}
					else
					{
						if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.WhitelistDeviceActivated)
						{
							SharedSettings.Current.AdsProductPurchased = false;
							SimpleMainPage instance = SimpleMainPage.Instance;
							if (instance != null)
							{
								instance.UpdateMainButtons();
							}
						}
						SharedSettings.Current.WhitelistDeviceActivated = false;
					}
					if (!WrongDeviceChecker.BAD_ELM327_NAMES.Contains(CS$<>8__locals1.device.Name))
					{
						goto IL_0448;
					}
					taskAwaiter3 = btledeviceSelectorPage.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num3 = 3;
						taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTLEDeviceSelectorPage.<lvDevices_ItemTapped>d__16>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0441:
					taskAwaiter3.GetResult();
					IL_0448:
					taskAwaiter5 = btledeviceSelectorPage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num3 = 4;
						TaskAwaiter<Page> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, BTLEDeviceSelectorPage.<lvDevices_ItemTapped>d__16>(ref taskAwaiter5, ref this);
						return;
					}
					IL_04A1:
					taskAwaiter5.GetResult();
				}
				catch (Exception ex3)
				{
					num3 = -2;
					CS$<>8__locals1 = null;
					deviceDescription = null;
					this.<>t__builder.SetException(ex3);
					return;
				}
				IL_04D2:
				num3 = -2;
				CS$<>8__locals1 = null;
				deviceDescription = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600151B RID: 5403 RVA: 0x00086150 File Offset: 0x00084350
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000589 RID: 1417
			public int <>1__state;

			// Token: 0x0400058A RID: 1418
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400058B RID: 1419
			public ItemTappedEventArgs e;

			// Token: 0x0400058C RID: 1420
			public BTLEDeviceSelectorPage <>4__this;

			// Token: 0x0400058D RID: 1421
			private BTLEDeviceSelectorPage.<>c__DisplayClass16_0 <>8__1;

			// Token: 0x0400058E RID: 1422
			private BTLEDeviceDescription <deviceDescription>5__2;

			// Token: 0x0400058F RID: 1423
			private object <>7__wrap2;

			// Token: 0x04000590 RID: 1424
			private int <>7__wrap3;

			// Token: 0x04000591 RID: 1425
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000592 RID: 1426
			private TaskAwaiter <>u__2;

			// Token: 0x04000593 RID: 1427
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x02000160 RID: 352
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_80
		{
			// Token: 0x0600151C RID: 5404 RVA: 0x00086160 File Offset: 0x00084360
			public <InitializeComponent>_anonXamlCDataTemplate_80()
			{
			}

			// Token: 0x0600151D RID: 5405 RVA: 0x00086174 File Offset: 0x00084374
			internal object LoadDataTemplate()
			{
				Button button;
				VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(button, nameScope);
				button.Clicked += this.root.btnDeviceInfo_Clicked;
				button.SetValue(Button.TextProperty, "Info");
				return button;
			}

			// Token: 0x04000594 RID: 1428
			internal object[] parentValues;

			// Token: 0x04000595 RID: 1429
			internal BTLEDeviceSelectorPage root;
		}

		// Token: 0x02000161 RID: 353
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_81
		{
			// Token: 0x0600151E RID: 5406 RVA: 0x000861E4 File Offset: 0x000843E4
			public <InitializeComponent>_anonXamlCDataTemplate_81()
			{
			}

			// Token: 0x0600151F RID: 5407 RVA: 0x000861F8 File Offset: 0x000843F8
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 31);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 34);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 37);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 37);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 34);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 37);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 37);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 37);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 30);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 33);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 33);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 30);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\BTLEDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				dynamicResourceExtension.Key = "SettingsCellBackground";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 31)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				stackLayout.SetValue(Grid.RowProperty, 0);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension markupExtension2 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = stackLayout;
				array4[2] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver2.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 37)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.FontSize = (double)obj3;
				staticResourceExtension2.Key = "EmptyStringToNonameConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension;
				array6[1] = label;
				array6[2] = stackLayout;
				array6[3] = grid;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver3.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 37)));
				object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension.Converter = obj5;
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				dynamicResourceExtension2.Key = "SettingsCellTitleColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = label;
				array8[1] = stackLayout;
				array8[2] = grid;
				object obj6;
				xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver4.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 37)));
				DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
				stackLayout.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(Grid.ColumnProperty, 0);
				staticResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension markupExtension5 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array9, 3, num5);
				object[] array10 = array9;
				array10[0] = label2;
				array10[1] = stackLayout;
				array10[2] = grid;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver5.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(178, 37)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label2.FontSize = (double)obj8;
				staticResourceExtension4.Key = "GuidToStringConverter";
				IMarkupExtension markupExtension6 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array11, 4, num6);
				object[] array12 = array11;
				array12[0] = bindingExtension2;
				array12[1] = label2;
				array12[2] = stackLayout;
				array12[3] = grid;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver6.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(179, 37)));
				object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
				bindingExtension2.Converter = obj10;
				bindingExtension2.Path = "Id";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				dynamicResourceExtension3.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array13, 3, num7);
				object[] array14 = array13;
				array14[0] = label2;
				array14[1] = stackLayout;
				array14[2] = grid;
				object obj11;
				xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array14, Label.TextColorProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver7.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 37)));
				DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label2.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
				stackLayout.Children.Add(label2);
				grid.Children.Add(stackLayout);
				image.SetValue(Grid.RowProperty, 0);
				image.SetValue(Grid.RowSpanProperty, 2);
				image.SetValue(Grid.ColumnProperty, 1);
				image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				staticResourceExtension5.Key = "BTLENameToVisibleBoolConverter";
				IMarkupExtension markupExtension8 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array15, 3, num8);
				object[] array16 = array15;
				array16[0] = bindingExtension3;
				array16[1] = image;
				array16[2] = grid;
				object obj12;
				xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver8.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 33)));
				object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
				bindingExtension3.Converter = obj13;
				bindingExtension3.Path = "Name";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				image.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
				image.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("elm327icon.png"));
				image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(image);
				frame.SetValue(Grid.RowProperty, 1);
				frame.SetValue(Grid.ColumnProperty, 0);
				frame.SetValue(Grid.ColumnSpanProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension4.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array17, 2, num9);
				object[] array18 = array17;
				array18[0] = frame;
				array18[1] = grid;
				object obj14;
				xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array18, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver9.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(BTLEDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_81).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 33)));
				DynamicResource dynamicResource4 = markupExtension9.ProvideValue(xamlServiceProvider9);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource4.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x04000596 RID: 1430
			internal object[] parentValues;

			// Token: 0x04000597 RID: 1431
			internal BTLEDeviceSelectorPage root;
		}
	}
}
