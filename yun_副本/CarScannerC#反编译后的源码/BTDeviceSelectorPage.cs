using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Input;
using CarScannerXamarinForms.Bluetooth2;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
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
	// Token: 0x0200014E RID: 334
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\BTDeviceSelectorPage.xaml")]
	public class BTDeviceSelectorPage : ContentPage
	{
		// Token: 0x060014C5 RID: 5317 RVA: 0x0007CB04 File Offset: 0x0007AD04
		public BTDeviceSelectorPage()
		{
			this.InitializeComponent();
			base.Disappearing += this.Page_Disappearing;
			if (PlatformHelper.IsiOS)
			{
				this.droidWarningLabel.IsVisible = false;
			}
			this.model = new BTDeviceSelectorViewModel();
			base.BindingContext = this.model;
			this.model.DiscoverDevices.Execute(null);
			PermissionStatus hasGPSPermission = this.HasGPSPermission;
			base.Appearing += this.BTDeviceSelectorPage_Appearing;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x0007CB84 File Offset: 0x0007AD84
		private async void BTDeviceSelectorPage_Appearing(object sender, EventArgs e)
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
						BTDeviceSelectorViewModel btdeviceSelectorViewModel = this.model;
						if (btdeviceSelectorViewModel != null)
						{
							ICommand discoverDevices = btdeviceSelectorViewModel.DiscoverDevices;
							if (discoverDevices != null)
							{
								discoverDevices.Execute(this);
							}
						}
					}
				}
			});
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x0007CBBC File Offset: 0x0007ADBC
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

		// Token: 0x060014C8 RID: 5320 RVA: 0x00026430 File Offset: 0x00024630
		private void btnCancel_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x0007CC00 File Offset: 0x0007AE00
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnCancel_Clicked(this, null);
				return true;
			}
			return true;
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x0007CC30 File Offset: 0x0007AE30
		private async void btnDevicePair_Clicked(object sender, EventArgs e)
		{
			IBluetooth2Device device = (sender as MenuItem).BindingContext as IBluetooth2Device;
			await device.Pair();
			if (device.Paired)
			{
				base.DisplayAlert("Device pairing status:", "paired", "OK");
			}
			else
			{
				base.DisplayAlert("Device pairing status:", "not paired", "OK");
			}
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0007CC70 File Offset: 0x0007AE70
		private async void btnForceSelect_Clicked(object sender, EventArgs e)
		{
			IBluetooth2Device bluetooth2Device = (sender as MenuItem).BindingContext as IBluetooth2Device;
			SharedSettings.Current.BTDeviceID = bluetooth2Device.Id;
			SharedSettings.Current.BTDeviceName = bluetooth2Device.Name;
			base.Navigation.PopAsync();
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x0007CCB0 File Offset: 0x0007AEB0
		private async void lvDevices_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			IBluetooth2Device bluetooth2Device = e.ItemData as IBluetooth2Device;
			await this.SelectDevice(bluetooth2Device);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x0007CCF0 File Offset: 0x0007AEF0
		private async Task SelectDevice(IBluetooth2Device device)
		{
			if (device != null)
			{
				if (!device.Paired)
				{
					SharedSettings.Current.BTDeviceID = device.Id;
					SharedSettings.Current.BTDeviceName = device.Name;
					device.Pair();
					if (WrongDeviceChecker.BAD_ELM327_NAMES.Contains(device.Name))
					{
						await base.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK");
					}
					base.Navigation.PopAsync();
				}
				else
				{
					SharedSettings.Current.BTDeviceID = device.Id;
					SharedSettings.Current.BTDeviceName = device.Name;
					if (WrongDeviceChecker.BAD_ELM327_NAMES.Contains(device.Name))
					{
						await base.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK");
					}
					base.Navigation.PopAsync();
				}
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
				if (PlatformHelper.IsAndroid && device.Name == "CAR2LS ScanX")
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Bluetooth / Bluetooth LE (4.0)", string.Format(Translate.GetString("settings_WhitelistDeviceWrongBluetoothTypeSelected"), device.Name), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						SharedSettings.Current.ConnectionType = ConnectionTypes.BluetoothLE;
						base.Navigation.PopAsync();
					}
				}
			}
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x0007CD3B File Offset: 0x0007AF3B
		private void BtnLocationPermissions_Clicked(object sender, EventArgs e)
		{
			PermissionHelper.OpenPermissionsSettings();
		}

		// Token: 0x17000F31 RID: 3889
		// (get) Token: 0x060014D0 RID: 5328 RVA: 0x0007CD42 File Offset: 0x0007AF42
		public bool DisplayPermissionsButton
		{
			get
			{
				return this.Android12HasBluetoothPermission != 3 || this.HasGPSPermission != 3;
			}
		}

		// Token: 0x17000F32 RID: 3890
		// (get) Token: 0x060014D1 RID: 5329 RVA: 0x0007CD59 File Offset: 0x0007AF59
		// (set) Token: 0x060014D2 RID: 5330 RVA: 0x0007CD61 File Offset: 0x0007AF61
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

		// Token: 0x060014D3 RID: 5331 RVA: 0x0007CD80 File Offset: 0x0007AF80
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

		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x060014D4 RID: 5332 RVA: 0x0007CDB8 File Offset: 0x0007AFB8
		// (set) Token: 0x060014D5 RID: 5333 RVA: 0x0007CE0D File Offset: 0x0007B00D
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
				this.OnPropertyChanged("DisplayPermissionsButton");
			}
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x0007CE2C File Offset: 0x0007B02C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/BTDeviceSelectorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			GuidToStringConverter guidToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(guidToStringConverter = new GuidToStringConverter(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			EmptyStringToNonameConverter emptyStringToNonameConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToNonameConverter = new EmptyStringToNonameConverter(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			PermissionStatus permissionStatus = 0;
			PermissionStatus permissionStatus2 = 1;
			PermissionStatus permissionStatus3 = 2;
			PermissionStatus permissionStatus4 = 4;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 18);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 53);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 53);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 25);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 25);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 14);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 17);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 17);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 17);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 14);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 21);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 21);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 29);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 26);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("btnLocationPermissions", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnLocationPermissions";
			}
			this.me = this;
			this.gridButtons = grid;
			this.droid12WarningLabel = label2;
			this.droidWarningLabel = label3;
			this.lvDevices = sfListView;
			this.btnLocationPermissions = button2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("GuidToStringConverter", guidToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EmptyStringToNonameConverter", emptyStringToNonameConverter);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 14)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 17)));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 17)));
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 17)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.Text = obj8;
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform2.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform2.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform2);
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
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = span;
			array7[1] = formattedString;
			array7[2] = label2;
			array7[3] = grid2;
			array7[4] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Span.TextProperty, nameScope));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 53)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			span.Text = obj10;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate4.Text = "droid_Android12BluetoothPermissionMissing_Text";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = span2;
			array8[1] = formattedString;
			array8[2] = label2;
			array8[3] = grid2;
			array8[4] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver8.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 53)));
			object obj12 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span2.Text = obj12;
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			staticResourceExtension.Key = "GPSStateNotGrantedToTrueConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = bindingExtension;
			array9[1] = label2;
			array9[2] = grid2;
			array9[3] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver9.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 25)));
			object obj14 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension.Converter = obj14;
			bindingExtension.Mode = 2;
			bindingExtension.Path = "Android12HasBluetoothPermission";
			referenceExtension.Name = "me";
			IMarkupExtension markupExtension10 = referenceExtension;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = bindingExtension;
			array10[1] = label2;
			array10[2] = grid2;
			array10[3] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver10.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 25)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension.Source = obj16;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid2.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 2);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate5.Text = "droid_LocationRequiredForBluetooth";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = label3;
			array11[1] = grid2;
			array11[2] = this;
			object obj17;
			xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver11.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 17)));
			object obj18 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label3.Text = obj18;
			label3.SetValue(Label.TextColorProperty, Color.Red);
			staticResourceExtension2.Key = "GPSStateNotGrantedToTrueConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = bindingExtension2;
			array12[1] = label3;
			array12[2] = grid2;
			array12[3] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver12.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 25)));
			object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension2.Converter = obj20;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "HasGPSPermission";
			referenceExtension2.Name = "me";
			IMarkupExtension markupExtension13 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = bindingExtension2;
			array13[1] = label3;
			array13[2] = grid2;
			array13[3] = this;
			object obj21;
			xamlServiceProvider13.Add(typeFromHandle25, obj21 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver13.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 25)));
			object obj22 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension2.Source = obj22;
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			grid2.Children.Add(label3);
			sfListView.SetValue(Grid.RowProperty, 3);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension3.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = bindingExtension3;
			array14[1] = sfListView;
			array14[2] = grid2;
			array14[3] = this;
			object obj23;
			xamlServiceProvider14.Add(typeFromHandle27, obj23 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver14.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 17)));
			object obj24 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension3.Converter = obj24;
			bindingExtension3.Path = "IsPairing";
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
			IDataTemplate dataTemplate2 = dataTemplate;
			BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79 <InitializeComponent>_anonXamlCDataTemplate_ = new BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79();
			object[] array15 = new object[0 + 4];
			array15[0] = dataTemplate;
			array15[1] = sfListView;
			array15[2] = grid2;
			array15[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array15;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(sfListView);
			label4.SetValue(Grid.RowProperty, 3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = label4;
			array16[1] = grid2;
			array16[2] = this;
			object obj25;
			xamlServiceProvider15.Add(typeFromHandle29, obj25 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver15.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 17)));
			DynamicResource dynamicResource4 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension6.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = bindingExtension6;
			array17[1] = label4;
			array17[2] = grid2;
			array17[3] = this;
			object obj26;
			xamlServiceProvider16.Add(typeFromHandle31, obj26 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver16.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 17)));
			object obj27 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension6.Converter = obj27;
			bindingExtension6.Path = "IsBluetoothOn";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			translate6.Text = "ios_TurnOnBluetooth";
			IMarkupExtension markupExtension17 = translate6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 3];
			array18[0] = label4;
			array18[1] = grid2;
			array18[2] = this;
			object obj28;
			xamlServiceProvider17.Add(typeFromHandle33, obj28 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver17.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(207, 17)));
			object obj29 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label4.Text = obj29;
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
			IMarkupExtension markupExtension18 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = bindingExtension8;
			array19[1] = button;
			array19[2] = stackLayout;
			array19[3] = grid2;
			array19[4] = this;
			object obj30;
			xamlServiceProvider18.Add(typeFromHandle35, obj30 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver18.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(219, 21)));
			object obj31 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension8.Converter = obj31;
			bindingExtension8.Path = "IsPairing";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			button.SetBinding(VisualElement.IsEnabledProperty, bindingBase8);
			translate7.Text = "ios_Refresh";
			IMarkupExtension markupExtension19 = translate7;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = button;
			array20[1] = stackLayout;
			array20[2] = grid2;
			array20[3] = this;
			object obj32;
			xamlServiceProvider19.Add(typeFromHandle37, obj32 = new SimpleValueTargetProvider(array20, Button.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver19.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(220, 21)));
			object obj33 = markupExtension19.ProvideValue(xamlServiceProvider19);
			button.Text = obj33;
			stackLayout.Children.Add(button);
			button2.Clicked += this.BtnLocationPermissions_Clicked;
			translate8.Text = "ios_Permissions";
			IMarkupExtension markupExtension20 = translate8;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = button2;
			array21[1] = stackLayout;
			array21[2] = grid2;
			array21[3] = this;
			object obj34;
			xamlServiceProvider20.Add(typeFromHandle39, obj34 = new SimpleValueTargetProvider(array21, Button.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver20.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(225, 21)));
			object obj35 = markupExtension20.ProvideValue(xamlServiceProvider20);
			button2.Text = obj35;
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "DisplayPermissionsButton";
			referenceExtension3.Name = "me";
			IMarkupExtension markupExtension21 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = bindingExtension9;
			array22[1] = button2;
			array22[2] = stackLayout;
			array22[3] = grid2;
			array22[4] = this;
			object obj36;
			xamlServiceProvider21.Add(typeFromHandle41, obj36 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver21.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(BTDeviceSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(230, 29)));
			object obj37 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension9.Source = obj37;
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			button2.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			stackLayout.Children.Add(button2);
			grid2.Children.Add(stackLayout);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x0007FBF8 File Offset: 0x0007DDF8
		[CompilerGenerated]
		private async void <BTDeviceSelectorPage_Appearing>b__2_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
			if (PlatformHelper.IsAndroid && this.HasGPSPermission != 3 && !SharedSettings.Current.BluetoothLocationWarningShowed)
			{
				SharedSettings.Current.BluetoothLocationWarningShowed = true;
				await base.DisplayAlert("Car Scanner", Translate.GetString("droid_LocationRequiredForBluetooth"), "OK");
				this.HasGPSPermission = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
				if (this.HasGPSPermission == 3)
				{
					BTDeviceSelectorViewModel btdeviceSelectorViewModel = this.model;
					if (btdeviceSelectorViewModel != null)
					{
						ICommand discoverDevices = btdeviceSelectorViewModel.DiscoverDevices;
						if (discoverDevices != null)
						{
							discoverDevices.Execute(this);
						}
					}
				}
			}
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0007FC37 File Offset: 0x0007DE37
		[CompilerGenerated]
		private void <get_HasGPSPermission>b__21_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x0007FC40 File Offset: 0x0007DE40
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<BTDeviceSelectorPage>(this, typeof(BTDeviceSelectorPage));
			this.me = NameScopeExtensions.FindByName<ContentPage>(this, "me");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.droid12WarningLabel = NameScopeExtensions.FindByName<Label>(this, "droid12WarningLabel");
			this.droidWarningLabel = NameScopeExtensions.FindByName<Label>(this, "droidWarningLabel");
			this.lvDevices = NameScopeExtensions.FindByName<SfListView>(this, "lvDevices");
			this.btnLocationPermissions = NameScopeExtensions.FindByName<Button>(this, "btnLocationPermissions");
		}

		// Token: 0x04000525 RID: 1317
		private bool Droid12BTWarningShowed;

		// Token: 0x04000526 RID: 1318
		private BTDeviceSelectorViewModel model;

		// Token: 0x04000527 RID: 1319
		private PermissionStatus _Android12HasBluetoothPermission;

		// Token: 0x04000528 RID: 1320
		private PermissionStatus _HasGPSPermission;

		// Token: 0x04000529 RID: 1321
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage me;

		// Token: 0x0400052A RID: 1322
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x0400052B RID: 1323
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label droid12WarningLabel;

		// Token: 0x0400052C RID: 1324
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label droidWarningLabel;

		// Token: 0x0400052D RID: 1325
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvDevices;

		// Token: 0x0400052E RID: 1326
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnLocationPermissions;

		// Token: 0x0200014F RID: 335
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<BTDeviceSelectorPage_Appearing>b__2_0>d : IAsyncStateMachine
		{
			// Token: 0x060014DA RID: 5338 RVA: 0x0007FCC4 File Offset: 0x0007DEC4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
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
						btdeviceSelectorPage.HasGPSPermission = status;
						if (!PlatformHelper.IsAndroid || btdeviceSelectorPage.HasGPSPermission == 3 || SharedSettings.Current.BluetoothLocationWarningShowed)
						{
							goto IL_0151;
						}
						SharedSettings.Current.BluetoothLocationWarningShowed = true;
						taskAwaiter3 = btdeviceSelectorPage.DisplayAlert("Car Scanner", Translate.GetString("droid_LocationRequiredForBluetooth"), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<<BTDeviceSelectorPage_Appearing>b__2_0>d>(ref taskAwaiter3, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTDeviceSelectorPage.<<BTDeviceSelectorPage_Appearing>b__2_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_011C:
					PermissionStatus result = taskAwaiter.GetResult();
					btdeviceSelectorPage.HasGPSPermission = result;
					if (btdeviceSelectorPage.HasGPSPermission == 3)
					{
						BTDeviceSelectorViewModel model = btdeviceSelectorPage.model;
						if (model != null)
						{
							ICommand discoverDevices = model.DiscoverDevices;
							if (discoverDevices != null)
							{
								discoverDevices.Execute(btdeviceSelectorPage);
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

			// Token: 0x060014DB RID: 5339 RVA: 0x0007FE6C File Offset: 0x0007E06C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400052F RID: 1327
			public int <>1__state;

			// Token: 0x04000530 RID: 1328
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000531 RID: 1329
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x04000532 RID: 1330
			public PermissionStatus status;

			// Token: 0x04000533 RID: 1331
			private TaskAwaiter <>u__1;

			// Token: 0x04000534 RID: 1332
			private TaskAwaiter<PermissionStatus> <>u__2;
		}

		// Token: 0x02000150 RID: 336
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BTDeviceSelectorPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x060014DC RID: 5340 RVA: 0x0007FE7C File Offset: 0x0007E07C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
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
						btdeviceSelectorPage.UpdateAndroid12BluetoothPermissionStatus();
						if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0) || btdeviceSelectorPage.Droid12BTWarningShowed)
						{
							goto IL_01E4;
						}
						btdeviceSelectorPage.Droid12BTWarningShowed = true;
						taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTDeviceSelectorPage.<BTDeviceSelectorPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTDeviceSelectorPage.<BTDeviceSelectorPage_Appearing>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_011E:
					permissionStatus = taskAwaiter.GetResult();
					btStatus = permissionStatus;
					if (btStatus == 3)
					{
						goto IL_01C4;
					}
					taskAwaiter3 = btdeviceSelectorPage.DisplayAlert(Translate.GetString("droid_Android12BluetoothPermissionMissing_Title"), Translate.GetString("droid_NearbyDevicesExplanation") + "\n" + Translate.GetString("droid_Android12BluetoothPermissionMissing_Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<BTDeviceSelectorPage_Appearing>d__2>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01BD:
					taskAwaiter3.GetResult();
					IL_01C4:
					btdeviceSelectorPage.UpdateAndroid12BluetoothPermissionStatus();
					if (btStatus == 3)
					{
						btdeviceSelectorPage.model.DiscoverDevices.Execute(null);
					}
					IL_01E4:
					taskAwaiter3 = PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
					{
						BTDeviceSelectorPage.<<BTDeviceSelectorPage_Appearing>b__2_0>d <<BTDeviceSelectorPage_Appearing>b__2_0>d;
						<<BTDeviceSelectorPage_Appearing>b__2_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<BTDeviceSelectorPage_Appearing>b__2_0>d.<>4__this = btdeviceSelectorPage;
						<<BTDeviceSelectorPage_Appearing>b__2_0>d.status = status;
						<<BTDeviceSelectorPage_Appearing>b__2_0>d.<>1__state = -1;
						<<BTDeviceSelectorPage_Appearing>b__2_0>d.<>t__builder.Start<BTDeviceSelectorPage.<<BTDeviceSelectorPage_Appearing>b__2_0>d>(ref <<BTDeviceSelectorPage_Appearing>b__2_0>d);
					}).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<BTDeviceSelectorPage_Appearing>d__2>(ref taskAwaiter3, ref this);
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

			// Token: 0x060014DD RID: 5341 RVA: 0x00080120 File Offset: 0x0007E320
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000535 RID: 1333
			public int <>1__state;

			// Token: 0x04000536 RID: 1334
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000537 RID: 1335
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x04000538 RID: 1336
			private PermissionStatus <btStatus>5__2;

			// Token: 0x04000539 RID: 1337
			private TaskAwaiter<PermissionStatus> <>u__1;

			// Token: 0x0400053A RID: 1338
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000151 RID: 337
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectDevice>d__11 : IAsyncStateMachine
		{
			// Token: 0x060014DE RID: 5342 RVA: 0x00080130 File Offset: 0x0007E330
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
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
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01BD;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_02B1;
					default:
						if (device == null)
						{
							goto IL_02EC;
						}
						if (!device.Paired)
						{
							SharedSettings.Current.BTDeviceID = device.Id;
							SharedSettings.Current.BTDeviceName = device.Name;
							device.Pair();
							if (!WrongDeviceChecker.BAD_ELM327_NAMES.Contains(device.Name))
							{
								goto IL_00FE;
							}
							taskAwaiter3 = btdeviceSelectorPage.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<SelectDevice>d__11>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							SharedSettings.Current.BTDeviceID = device.Id;
							SharedSettings.Current.BTDeviceName = device.Name;
							if (!WrongDeviceChecker.BAD_ELM327_NAMES.Contains(device.Name))
							{
								goto IL_01C4;
							}
							taskAwaiter3 = btdeviceSelectorPage.DisplayAlert(Translate.GetString("BT_BadDeviceSelected_Title"), Translate.GetString("BT_BadDeviceSelected_Text"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<SelectDevice>d__11>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01BD;
						}
						break;
					}
					taskAwaiter3.GetResult();
					IL_00FE:
					btdeviceSelectorPage.Navigation.PopAsync();
					goto IL_01D0;
					IL_01BD:
					taskAwaiter3.GetResult();
					IL_01C4:
					btdeviceSelectorPage.Navigation.PopAsync();
					IL_01D0:
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
					if (!PlatformHelper.IsAndroid || !(device.Name == "CAR2LS ScanX"))
					{
						goto IL_02D1;
					}
					taskAwaiter5 = btdeviceSelectorPage.DisplayAlert("Bluetooth / Bluetooth LE (4.0)", string.Format(Translate.GetString("settings_WhitelistDeviceWrongBluetoothTypeSelected"), device.Name), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, BTDeviceSelectorPage.<SelectDevice>d__11>(ref taskAwaiter5, ref this);
						return;
					}
					IL_02B1:
					if (taskAwaiter5.GetResult())
					{
						SharedSettings.Current.ConnectionType = ConnectionTypes.BluetoothLE;
						btdeviceSelectorPage.Navigation.PopAsync();
					}
					IL_02D1:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02EC:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060014DF RID: 5343 RVA: 0x00080458 File Offset: 0x0007E658
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400053B RID: 1339
			public int <>1__state;

			// Token: 0x0400053C RID: 1340
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400053D RID: 1341
			public IBluetooth2Device device;

			// Token: 0x0400053E RID: 1342
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x0400053F RID: 1343
			private TaskAwaiter <>u__1;

			// Token: 0x04000540 RID: 1344
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000152 RID: 338
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateAndroid12BluetoothPermissionStatus>d__19 : IAsyncStateMachine
		{
			// Token: 0x060014E0 RID: 5344 RVA: 0x00080468 File Offset: 0x0007E668
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							btdeviceSelectorPage.Android12HasBluetoothPermission = 3;
							goto IL_0098;
						}
						if (!PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							btdeviceSelectorPage.Android12HasBluetoothPermission = 3;
							goto IL_0098;
						}
						taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<PermissionStatus> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, BTDeviceSelectorPage.<UpdateAndroid12BluetoothPermissionStatus>d__19>(ref taskAwaiter, ref this);
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
					btdeviceSelectorPage.Android12HasBluetoothPermission = result;
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

			// Token: 0x060014E1 RID: 5345 RVA: 0x0008054C File Offset: 0x0007E74C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000541 RID: 1345
			public int <>1__state;

			// Token: 0x04000542 RID: 1346
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000543 RID: 1347
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x04000544 RID: 1348
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x02000153 RID: 339
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDevicePair_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x060014E2 RID: 5346 RVA: 0x0008055C File Offset: 0x0007E75C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						device = (sender as MenuItem).BindingContext as IBluetooth2Device;
						taskAwaiter = device.Pair().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<btnDevicePair_Clicked>d__8>(ref taskAwaiter, ref this);
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
					if (device.Paired)
					{
						btdeviceSelectorPage.DisplayAlert("Device pairing status:", "paired", "OK");
					}
					else
					{
						btdeviceSelectorPage.DisplayAlert("Device pairing status:", "not paired", "OK");
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					device = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				device = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060014E3 RID: 5347 RVA: 0x0008067C File Offset: 0x0007E87C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000545 RID: 1349
			public int <>1__state;

			// Token: 0x04000546 RID: 1350
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000547 RID: 1351
			public object sender;

			// Token: 0x04000548 RID: 1352
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x04000549 RID: 1353
			private IBluetooth2Device <device>5__2;

			// Token: 0x0400054A RID: 1354
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000154 RID: 340
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnForceSelect_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x060014E4 RID: 5348 RVA: 0x0008068C File Offset: 0x0007E88C
			void IAsyncStateMachine.MoveNext()
			{
				BTDeviceSelectorPage btdeviceSelectorPage = this;
				try
				{
					IBluetooth2Device bluetooth2Device = (sender as MenuItem).BindingContext as IBluetooth2Device;
					SharedSettings.Current.BTDeviceID = bluetooth2Device.Id;
					SharedSettings.Current.BTDeviceName = bluetooth2Device.Name;
					btdeviceSelectorPage.Navigation.PopAsync();
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

			// Token: 0x060014E5 RID: 5349 RVA: 0x00080720 File Offset: 0x0007E920
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400054B RID: 1355
			public int <>1__state;

			// Token: 0x0400054C RID: 1356
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400054D RID: 1357
			public object sender;

			// Token: 0x0400054E RID: 1358
			public BTDeviceSelectorPage <>4__this;
		}

		// Token: 0x02000155 RID: 341
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lvDevices_ItemTapped>d__10 : IAsyncStateMachine
		{
			// Token: 0x060014E6 RID: 5350 RVA: 0x00080730 File Offset: 0x0007E930
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				BTDeviceSelectorPage btdeviceSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						IBluetooth2Device bluetooth2Device = e.ItemData as IBluetooth2Device;
						taskAwaiter = btdeviceSelectorPage.SelectDevice(bluetooth2Device).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, BTDeviceSelectorPage.<lvDevices_ItemTapped>d__10>(ref taskAwaiter, ref this);
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

			// Token: 0x060014E7 RID: 5351 RVA: 0x000807F8 File Offset: 0x0007E9F8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400054F RID: 1359
			public int <>1__state;

			// Token: 0x04000550 RID: 1360
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000551 RID: 1361
			public ItemTappedEventArgs e;

			// Token: 0x04000552 RID: 1362
			public BTDeviceSelectorPage <>4__this;

			// Token: 0x04000553 RID: 1363
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000156 RID: 342
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_79
		{
			// Token: 0x060014E8 RID: 5352 RVA: 0x00080808 File Offset: 0x0007EA08
			public <InitializeComponent>_anonXamlCDataTemplate_79()
			{
			}

			// Token: 0x060014E9 RID: 5353 RVA: 0x0008081C File Offset: 0x0007EA1C
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 31);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 34);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 37);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 37);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 34);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 37);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 37);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 30);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 33);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 30);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\BTDeviceSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 26);
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 31)));
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
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 37)));
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
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 37)));
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
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 37)));
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
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 37)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label2.FontSize = (double)obj8;
				bindingExtension2.Path = "Id";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				dynamicResourceExtension3.Key = "SettingsCellValueTextColor";
				IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array11, 3, num6);
				object[] array12 = array11;
				array12[0] = label2;
				array12[1] = stackLayout;
				array12[2] = grid;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, Label.TextColorProperty, nameScope));
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
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 37)));
				DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
				label2.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
				stackLayout.Children.Add(label2);
				grid.Children.Add(stackLayout);
				image.SetValue(Grid.RowProperty, 0);
				image.SetValue(Grid.RowSpanProperty, 2);
				image.SetValue(Grid.ColumnProperty, 1);
				image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				bindingExtension3.Path = "IsValid";
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
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array13, 2, num7);
				object[] array14 = array13;
				array14[0] = frame;
				array14[1] = grid;
				object obj10;
				xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array14, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xmlNamespaceResolver7.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(BTDeviceSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_79).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 33)));
				DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource4.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x04000554 RID: 1364
			internal object[] parentValues;

			// Token: 0x04000555 RID: 1365
			internal BTDeviceSelectorPage root;
		}
	}
}
