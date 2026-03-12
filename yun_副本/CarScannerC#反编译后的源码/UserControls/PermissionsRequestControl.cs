using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005D1 RID: 1489
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\PermissionsRequestControl.xaml")]
	public class PermissionsRequestControl : ContentView
	{
		// Token: 0x06003597 RID: 13719 RVA: 0x0026634D File Offset: 0x0026454D
		public PermissionsRequestControl()
		{
			this.InitializeComponent();
		}

		// Token: 0x1700136A RID: 4970
		// (get) Token: 0x06003598 RID: 13720 RVA: 0x0026637F File Offset: 0x0026457F
		// (set) Token: 0x06003599 RID: 13721 RVA: 0x00266387 File Offset: 0x00264587
		public bool DisplayBluetooth
		{
			get
			{
				return this._DisplayBluetooth;
			}
			set
			{
				if (this._DisplayBluetooth != value)
				{
					this._DisplayBluetooth = value;
					this.cellBluetooth.IsVisible = this._DisplayBluetooth;
				}
			}
		}

		// Token: 0x1700136B RID: 4971
		// (get) Token: 0x0600359A RID: 13722 RVA: 0x002663AA File Offset: 0x002645AA
		// (set) Token: 0x0600359B RID: 13723 RVA: 0x002663B2 File Offset: 0x002645B2
		public bool DisplayGPS
		{
			get
			{
				return this._DisplayGPS;
			}
			set
			{
				if (this._DisplayGPS != value)
				{
					this._DisplayGPS = value;
					this.cellGPS.IsVisible = this._DisplayGPS;
				}
			}
		}

		// Token: 0x14000035 RID: 53
		// (add) Token: 0x0600359C RID: 13724 RVA: 0x002663D8 File Offset: 0x002645D8
		// (remove) Token: 0x0600359D RID: 13725 RVA: 0x00266410 File Offset: 0x00264610
		public event EventHandler NextClicked
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.NextClicked;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.NextClicked, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.NextClicked;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.NextClicked, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x0600359E RID: 13726 RVA: 0x00266448 File Offset: 0x00264648
		public async ValueTask UpdateCurrentStatus()
		{
			await this.AndroidUpdatePermissionsStatus();
		}

		// Token: 0x0600359F RID: 13727 RVA: 0x0026648C File Offset: 0x0026468C
		private async void CellDroidBtPermission_Tapped(object sender, EventArgs e)
		{
			await this.RequestAndroid12BluetoothPermission();
			await this.AndroidUpdatePermissionsStatus();
			if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
			{
				TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<PermissionStatus> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter.GetResult() != 3)
				{
					PermissionHelper.OpenPermissionsSettings();
				}
			}
		}

		// Token: 0x060035A0 RID: 13728 RVA: 0x002664C4 File Offset: 0x002646C4
		private async void CellDroidLocationPermission_Tapped(object sender, EventArgs e)
		{
			await this.RequestAndroidGPSPermission();
			await this.AndroidUpdatePermissionsStatus();
		}

		// Token: 0x060035A1 RID: 13729 RVA: 0x002664FC File Offset: 0x002646FC
		private async Task AndroidRequestRequiredPermissions()
		{
			if (this.DisplayBluetooth)
			{
				await this.RequestAndroid12BluetoothPermission();
			}
			if (this.DisplayGPS)
			{
				await this.RequestAndroidGPSPermission();
			}
		}

		// Token: 0x060035A2 RID: 13730 RVA: 0x00266540 File Offset: 0x00264740
		private async Task RequestAndroidGPSPermission()
		{
			if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
			{
				TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<PermissionStatus> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter.GetResult() != 3)
				{
					await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
				}
			}
		}

		// Token: 0x060035A3 RID: 13731 RVA: 0x0026657C File Offset: 0x0026477C
		private async Task RequestAndroid12BluetoothPermission()
		{
			if (PlatformHelper.IsAndroid && PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
			{
				TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<PermissionStatus> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter.GetResult() != 3)
				{
					await PlatformHelper.DroidService.RequestBluetoothPermissionAndroid12Async();
				}
			}
		}

		// Token: 0x060035A4 RID: 13732 RVA: 0x002665B8 File Offset: 0x002647B8
		private async ValueTask AndroidUpdatePermissionsStatus()
		{
			if (PlatformHelper.IsAndroid)
			{
				TaskAwaiter<PermissionStatus> taskAwaiter2;
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() == 3)
					{
						this.cellDroidBtPermission.Text = this.grantedSign;
					}
					else
					{
						this.cellDroidBtPermission.Text = this.restrictedSign;
					}
				}
				else
				{
					this.cellDroidBtPermission.Text = this.grantedSign;
				}
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
				{
					TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
					}
					if (taskAwaiter.GetResult() == 3)
					{
						this.cellDroidLocationPermission.Text = this.grantedSign;
					}
					else
					{
						this.cellDroidLocationPermission.Text = this.restrictedSign;
					}
				}
				else
				{
					this.cellDroidLocationPermission.Text = this.grantedSign;
				}
			}
		}

		// Token: 0x060035A5 RID: 13733 RVA: 0x002665FC File Offset: 0x002647FC
		private async void btnDroidPermissionsNext_Clicked(object sender, EventArgs e)
		{
			await this.AndroidRequestRequiredPermissions();
			EventHandler nextClicked = this.NextClicked;
			if (nextClicked != null)
			{
				nextClicked(this, EventArgs.Empty);
			}
		}

		// Token: 0x060035A6 RID: 13734 RVA: 0x0007CD3B File Offset: 0x0007AF3B
		private void ButtonCell_Tapped(object sender, EventArgs e)
		{
			PermissionHelper.OpenPermissionsSettings();
		}

		// Token: 0x060035A7 RID: 13735 RVA: 0x00266634 File Offset: 0x00264834
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(PermissionsRequestControl).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/PermissionsRequestControl.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 29);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 33);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 30);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 33);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 26);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 22);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 33);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 33);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 30);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 33);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 30);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 26);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 25);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 25);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 17);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\PermissionsRequestControl.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridDroidPermissions", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "gridDroidPermissions";
			}
			nameScope.RegisterName("setDroidPermissions", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "setDroidPermissions";
			}
			nameScope.RegisterName("cellBluetooth", customCell);
			if (customCell.StyleId == null)
			{
				customCell.StyleId = "cellBluetooth";
			}
			nameScope.RegisterName("cellDroidBtPermission", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "cellDroidBtPermission";
			}
			nameScope.RegisterName("cellGPS", customCell2);
			if (customCell2.StyleId == null)
			{
				customCell2.StyleId = "cellGPS";
			}
			nameScope.RegisterName("cellDroidLocationPermission", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "cellDroidLocationPermission";
			}
			nameScope.RegisterName("btnDroidPermissionsNext", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnDroidPermissionsNext";
			}
			this.gridDroidPermissions = grid3;
			this.setDroidPermissions = settingsView;
			this.cellBluetooth = customCell;
			this.cellDroidBtPermission = label2;
			this.cellGPS = customCell2;
			this.cellDroidLocationPermission = label4;
			this.btnDroidPermissionsNext = button;
			grid3.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView.SetValue(Grid.RowProperty, 0);
			dynamicResourceExtension.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 3];
			array[0] = settingsView;
			array[1] = grid3;
			array[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, SettingsView.CellTitleFontSizeProperty, nameScope));
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
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 17)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			settingsView.SetDynamicResource(SettingsView.CellTitleFontSizeProperty, dynamicResource.Key);
			dynamicResourceExtension2.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = settingsView;
			array2[1] = grid3;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, SettingsView.CellValueTextFontSizeProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
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
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(20, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			settingsView.SetDynamicResource(SettingsView.CellValueTextFontSizeProperty, dynamicResource2.Key);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderFontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = settingsView;
			array3[1] = grid3;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, SettingsView.HeaderFontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
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
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(23, 17)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			settingsView.SetDynamicResource(SettingsView.HeaderFontSizeProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = settingsView;
			array4[1] = grid3;
			array4[2] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, SettingsView.SeparatorColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
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
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(24, 17)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			settingsView.SetDynamicResource(SettingsView.SeparatorColorProperty, dynamicResource4.Key);
			translate.Text = "droid_PermissionsExplanation12";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = section;
			array5[1] = settingsView;
			array5[2] = grid3;
			array5[3] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array5, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
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
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 29)));
			object obj6 = markupExtension5.ProvideValue(xamlServiceProvider5);
			section.Title = obj6;
			customCell.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("bluetooth48.png"));
			customCell.Tapped += this.CellDroidBtPermission_Tapped;
			grid.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, auto"));
			label.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension5.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 7];
			array6[0] = label;
			array6[1] = grid;
			array6[2] = customCell;
			array6[3] = section;
			array6[4] = settingsView;
			array6[5] = grid3;
			array6[6] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
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
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 33)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
			translate2.Text = "droid_NearbyDevicesExplanation";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = label;
			array7[1] = grid;
			array7[2] = customCell;
			array7[3] = section;
			array7[4] = settingsView;
			array7[5] = grid3;
			array7[6] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
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
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 33)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label.Text = obj9;
			grid.Children.Add(label);
			label2.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension6.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 7];
			array8[0] = label2;
			array8[1] = grid;
			array8[2] = customCell;
			array8[3] = section;
			array8[4] = settingsView;
			array8[5] = grid3;
			array8[6] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
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
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 33)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
			label2.SetValue(Label.TextProperty, "");
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label2);
			customCell.SetValue(CustomCell.ContentProperty, grid);
			section.Add(customCell);
			customCell2.SetValue(CellBase.IconSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("gps48.png"));
			customCell2.Tapped += this.CellDroidLocationPermission_Tapped;
			grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, auto"));
			label3.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension7.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = label3;
			array9[1] = grid2;
			array9[2] = customCell2;
			array9[3] = section;
			array9[4] = settingsView;
			array9[5] = grid3;
			array9[6] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
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
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 33)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			translate3.Text = "droid_LocationDevicesExplanation";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = label3;
			array10[1] = grid2;
			array10[2] = customCell2;
			array10[3] = section;
			array10[4] = settingsView;
			array10[5] = grid3;
			array10[6] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
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
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 33)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label3.Text = obj13;
			grid2.Children.Add(label3);
			label4.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension8.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = label4;
			array11[1] = grid2;
			array11[2] = customCell2;
			array11[3] = section;
			array11[4] = settingsView;
			array11[5] = grid3;
			array11[6] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array11, Label.FontSizeProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
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
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 33)));
			DynamicResource dynamicResource8 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label4.SetValue(Label.TextProperty, "");
			label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid2.Children.Add(label4);
			customCell2.SetValue(CustomCell.ContentProperty, grid2);
			section.Add(customCell2);
			translate4.Text = "ios_Permissions";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = buttonCell;
			array12[1] = section;
			array12[2] = settingsView;
			array12[3] = grid3;
			array12[4] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
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
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 25)));
			object obj16 = markupExtension12.ProvideValue(xamlServiceProvider12);
			buttonCell.Title = obj16;
			buttonCell.Tapped += this.ButtonCell_Tapped;
			dynamicResourceExtension9.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = buttonCell;
			array13[1] = section;
			array13[2] = settingsView;
			array13[3] = grid3;
			array13[4] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array13, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
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
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 25)));
			DynamicResource dynamicResource9 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource9.Key);
			dynamicResourceExtension10.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = buttonCell;
			array14[1] = section;
			array14[2] = settingsView;
			array14[3] = grid3;
			array14[4] = this;
			object obj18;
			xamlServiceProvider14.Add(typeFromHandle27, obj18 = new SimpleValueTargetProvider(array14, CellBase.TitleFontSizeProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj18);
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
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 25)));
			DynamicResource dynamicResource10 = markupExtension14.ProvideValue(xamlServiceProvider14);
			buttonCell.SetDynamicResource(CellBase.TitleFontSizeProperty, dynamicResource10.Key);
			section.Add(buttonCell);
			translate5.Text = "droid_TapNextToGrantPermissions";
			IMarkupExtension markupExtension15 = translate5;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = labelCell;
			array15[1] = section;
			array15[2] = settingsView;
			array15[3] = grid3;
			array15[4] = this;
			object obj19;
			xamlServiceProvider15.Add(typeFromHandle29, obj19 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj19);
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
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 25)));
			object obj20 = markupExtension15.ProvideValue(xamlServiceProvider15);
			labelCell.Title = obj20;
			labelCell.SetValue(CellBase.IsVisibleProperty, true);
			labelCell.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section.Add(labelCell);
			settingsView.Root.Add(section);
			grid3.Children.Add(settingsView);
			button.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension11.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = button;
			array16[1] = grid3;
			array16[2] = this;
			object obj21;
			xamlServiceProvider16.Add(typeFromHandle31, obj21 = new SimpleValueTargetProvider(array16, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj21);
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
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 17)));
			DynamicResource dynamicResource11 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource11.Key);
			button.Clicked += this.btnDroidPermissionsNext_Clicked;
			translate6.Text = "ios_NEXT";
			IMarkupExtension markupExtension17 = translate6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 3];
			array17[0] = button;
			array17[1] = grid3;
			array17[2] = this;
			object obj22;
			xamlServiceProvider17.Add(typeFromHandle33, obj22 = new SimpleValueTargetProvider(array17, Button.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj22);
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
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(PermissionsRequestControl).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 17)));
			object obj23 = markupExtension17.ProvideValue(xamlServiceProvider17);
			button.Text = obj23;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid3.Children.Add(button);
			this.SetValue(ContentView.ContentProperty, grid3);
		}

		// Token: 0x060035A8 RID: 13736 RVA: 0x002687D4 File Offset: 0x002669D4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<PermissionsRequestControl>(this, typeof(PermissionsRequestControl));
			this.gridDroidPermissions = NameScopeExtensions.FindByName<Grid>(this, "gridDroidPermissions");
			this.setDroidPermissions = NameScopeExtensions.FindByName<SettingsView>(this, "setDroidPermissions");
			this.cellBluetooth = NameScopeExtensions.FindByName<CustomCell>(this, "cellBluetooth");
			this.cellDroidBtPermission = NameScopeExtensions.FindByName<Label>(this, "cellDroidBtPermission");
			this.cellGPS = NameScopeExtensions.FindByName<CustomCell>(this, "cellGPS");
			this.cellDroidLocationPermission = NameScopeExtensions.FindByName<Label>(this, "cellDroidLocationPermission");
			this.btnDroidPermissionsNext = NameScopeExtensions.FindByName<Button>(this, "btnDroidPermissionsNext");
		}

		// Token: 0x04001FD8 RID: 8152
		private bool _DisplayBluetooth = true;

		// Token: 0x04001FD9 RID: 8153
		private bool _DisplayGPS = true;

		// Token: 0x04001FDA RID: 8154
		[CompilerGenerated]
		private EventHandler NextClicked;

		// Token: 0x04001FDB RID: 8155
		private string grantedSign = "✅";

		// Token: 0x04001FDC RID: 8156
		private string restrictedSign = "❌";

		// Token: 0x04001FDD RID: 8157
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridDroidPermissions;

		// Token: 0x04001FDE RID: 8158
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView setDroidPermissions;

		// Token: 0x04001FDF RID: 8159
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell cellBluetooth;

		// Token: 0x04001FE0 RID: 8160
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label cellDroidBtPermission;

		// Token: 0x04001FE1 RID: 8161
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell cellGPS;

		// Token: 0x04001FE2 RID: 8162
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label cellDroidLocationPermission;

		// Token: 0x04001FE3 RID: 8163
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDroidPermissionsNext;

		// Token: 0x020005D2 RID: 1490
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AndroidRequestRequiredPermissions>d__17 : IAsyncStateMachine
		{
			// Token: 0x060035A9 RID: 13737 RVA: 0x0026886C File Offset: 0x00266A6C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00D4;
						}
						if (!permissionsRequestControl.DisplayBluetooth)
						{
							goto IL_007B;
						}
						taskAwaiter = permissionsRequestControl.RequestAndroid12BluetoothPermission().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PermissionsRequestControl.<AndroidRequestRequiredPermissions>d__17>(ref taskAwaiter, ref this);
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
					IL_007B:
					if (!permissionsRequestControl.DisplayGPS)
					{
						goto IL_00DB;
					}
					taskAwaiter = permissionsRequestControl.RequestAndroidGPSPermission().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PermissionsRequestControl.<AndroidRequestRequiredPermissions>d__17>(ref taskAwaiter, ref this);
						return;
					}
					IL_00D4:
					taskAwaiter.GetResult();
					IL_00DB:;
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

			// Token: 0x060035AA RID: 13738 RVA: 0x00268990 File Offset: 0x00266B90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FE4 RID: 8164
			public int <>1__state;

			// Token: 0x04001FE5 RID: 8165
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001FE6 RID: 8166
			public PermissionsRequestControl <>4__this;

			// Token: 0x04001FE7 RID: 8167
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005D3 RID: 1491
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AndroidUpdatePermissionsStatus>d__20 : IAsyncStateMachine
		{
			// Token: 0x060035AB RID: 13739 RVA: 0x002689A0 File Offset: 0x00266BA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
							goto IL_0128;
						}
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_0169;
						}
						if (!PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							permissionsRequestControl.cellDroidBtPermission.Text = permissionsRequestControl.grantedSign;
							goto IL_00C8;
						}
						taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<AndroidUpdatePermissionsStatus>d__20>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						permissionsRequestControl.cellDroidBtPermission.Text = permissionsRequestControl.grantedSign;
					}
					else
					{
						permissionsRequestControl.cellDroidBtPermission.Text = permissionsRequestControl.restrictedSign;
					}
					IL_00C8:
					if (!PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
					{
						permissionsRequestControl.cellDroidLocationPermission.Text = permissionsRequestControl.grantedSign;
						goto IL_0169;
					}
					taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<AndroidUpdatePermissionsStatus>d__20>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0128:
					if (taskAwaiter3.GetResult() == 3)
					{
						permissionsRequestControl.cellDroidLocationPermission.Text = permissionsRequestControl.grantedSign;
					}
					else
					{
						permissionsRequestControl.cellDroidLocationPermission.Text = permissionsRequestControl.restrictedSign;
					}
					IL_0169:;
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

			// Token: 0x060035AC RID: 13740 RVA: 0x00268B60 File Offset: 0x00266D60
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FE8 RID: 8168
			public int <>1__state;

			// Token: 0x04001FE9 RID: 8169
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x04001FEA RID: 8170
			public PermissionsRequestControl <>4__this;

			// Token: 0x04001FEB RID: 8171
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x020005D4 RID: 1492
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CellDroidBtPermission_Tapped>d__15 : IAsyncStateMachine
		{
			// Token: 0x060035AD RID: 13741 RVA: 0x00268B70 File Offset: 0x00266D70
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					ValueTaskAwaiter valueTaskAwaiter;
					TaskAwaiter<PermissionStatus> taskAwaiter5;
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
						ValueTaskAwaiter valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter);
						num2 = -1;
						goto IL_00D3;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
						goto IL_0143;
					default:
						taskAwaiter3 = permissionsRequestControl.RequestAndroid12BluetoothPermission().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PermissionsRequestControl.<CellDroidBtPermission_Tapped>d__15>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					valueTaskAwaiter = permissionsRequestControl.AndroidUpdatePermissionsStatus().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 1;
						ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, PermissionsRequestControl.<CellDroidBtPermission_Tapped>d__15>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_00D3:
					valueTaskAwaiter.GetResult();
					if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
					{
						goto IL_0152;
					}
					taskAwaiter5 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<CellDroidBtPermission_Tapped>d__15>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0143:
					if (taskAwaiter5.GetResult() != 3)
					{
						PermissionHelper.OpenPermissionsSettings();
					}
					IL_0152:;
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

			// Token: 0x060035AE RID: 13742 RVA: 0x00268D1C File Offset: 0x00266F1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FEC RID: 8172
			public int <>1__state;

			// Token: 0x04001FED RID: 8173
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001FEE RID: 8174
			public PermissionsRequestControl <>4__this;

			// Token: 0x04001FEF RID: 8175
			private TaskAwaiter <>u__1;

			// Token: 0x04001FF0 RID: 8176
			private ValueTaskAwaiter <>u__2;

			// Token: 0x04001FF1 RID: 8177
			private TaskAwaiter<PermissionStatus> <>u__3;
		}

		// Token: 0x020005D5 RID: 1493
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CellDroidLocationPermission_Tapped>d__16 : IAsyncStateMachine
		{
			// Token: 0x060035AF RID: 13743 RVA: 0x00268D2C File Offset: 0x00266F2C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					ValueTaskAwaiter valueTaskAwaiter;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							ValueTaskAwaiter valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter);
							num2 = -1;
							goto IL_00C8;
						}
						taskAwaiter = permissionsRequestControl.RequestAndroidGPSPermission().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PermissionsRequestControl.<CellDroidLocationPermission_Tapped>d__16>(ref taskAwaiter, ref this);
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
					valueTaskAwaiter = permissionsRequestControl.AndroidUpdatePermissionsStatus().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 1;
						ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, PermissionsRequestControl.<CellDroidLocationPermission_Tapped>d__16>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_00C8:
					valueTaskAwaiter.GetResult();
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

			// Token: 0x060035B0 RID: 13744 RVA: 0x00268E48 File Offset: 0x00267048
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FF2 RID: 8178
			public int <>1__state;

			// Token: 0x04001FF3 RID: 8179
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001FF4 RID: 8180
			public PermissionsRequestControl <>4__this;

			// Token: 0x04001FF5 RID: 8181
			private TaskAwaiter <>u__1;

			// Token: 0x04001FF6 RID: 8182
			private ValueTaskAwaiter <>u__2;
		}

		// Token: 0x020005D6 RID: 1494
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestAndroid12BluetoothPermission>d__19 : IAsyncStateMachine
		{
			// Token: 0x060035B1 RID: 13745 RVA: 0x00268E58 File Offset: 0x00267058
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
							goto IL_00DF;
						}
						if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(31, 0))
						{
							goto IL_00E9;
						}
						taskAwaiter3 = PlatformHelper.DroidService.GetBluetoothStatusAndroid12Async().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<RequestAndroid12BluetoothPermission>d__19>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						goto IL_00E9;
					}
					taskAwaiter3 = PlatformHelper.DroidService.RequestBluetoothPermissionAndroid12Async().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<RequestAndroid12BluetoothPermission>d__19>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00DF:
					taskAwaiter3.GetResult();
					IL_00E9:;
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

			// Token: 0x060035B2 RID: 13746 RVA: 0x00268F8C File Offset: 0x0026718C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FF7 RID: 8183
			public int <>1__state;

			// Token: 0x04001FF8 RID: 8184
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001FF9 RID: 8185
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x020005D7 RID: 1495
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RequestAndroidGPSPermission>d__18 : IAsyncStateMachine
		{
			// Token: 0x060035B3 RID: 13747 RVA: 0x00268F9C File Offset: 0x0026719C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num2 = -1;
							goto IL_00D5;
						}
						if (!PlatformHelper.IsAndroid || !PlatformHelper.IsPlatformVersionNewerOrEqual(23, 0))
						{
							goto IL_00DD;
						}
						taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<RequestAndroidGPSPermission>d__18>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						goto IL_00DD;
					}
					taskAwaiter3 = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, PermissionsRequestControl.<RequestAndroidGPSPermission>d__18>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00D5:
					taskAwaiter3.GetResult();
					IL_00DD:;
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

			// Token: 0x060035B4 RID: 13748 RVA: 0x002690C4 File Offset: 0x002672C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FFA RID: 8186
			public int <>1__state;

			// Token: 0x04001FFB RID: 8187
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001FFC RID: 8188
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x020005D8 RID: 1496
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentStatus>d__12 : IAsyncStateMachine
		{
			// Token: 0x060035B5 RID: 13749 RVA: 0x002690D4 File Offset: 0x002672D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					ValueTaskAwaiter valueTaskAwaiter;
					if (num != 0)
					{
						valueTaskAwaiter = permissionsRequestControl.AndroidUpdatePermissionsStatus().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, PermissionsRequestControl.<UpdateCurrentStatus>d__12>(ref valueTaskAwaiter, ref this);
							return;
						}
					}
					else
					{
						ValueTaskAwaiter valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter);
						num2 = -1;
					}
					valueTaskAwaiter.GetResult();
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

			// Token: 0x060035B6 RID: 13750 RVA: 0x0026918C File Offset: 0x0026738C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FFD RID: 8189
			public int <>1__state;

			// Token: 0x04001FFE RID: 8190
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x04001FFF RID: 8191
			public PermissionsRequestControl <>4__this;

			// Token: 0x04002000 RID: 8192
			private ValueTaskAwaiter <>u__1;
		}

		// Token: 0x020005D9 RID: 1497
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDroidPermissionsNext_Clicked>d__21 : IAsyncStateMachine
		{
			// Token: 0x060035B7 RID: 13751 RVA: 0x0026919C File Offset: 0x0026739C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PermissionsRequestControl permissionsRequestControl = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = permissionsRequestControl.AndroidRequestRequiredPermissions().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PermissionsRequestControl.<btnDroidPermissionsNext_Clicked>d__21>(ref taskAwaiter, ref this);
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
					EventHandler nextClicked = permissionsRequestControl.NextClicked;
					if (nextClicked != null)
					{
						nextClicked(permissionsRequestControl, EventArgs.Empty);
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

			// Token: 0x060035B8 RID: 13752 RVA: 0x00269268 File Offset: 0x00267468
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002001 RID: 8193
			public int <>1__state;

			// Token: 0x04002002 RID: 8194
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002003 RID: 8195
			public PermissionsRequestControl <>4__this;

			// Token: 0x04002004 RID: 8196
			private TaskAwaiter <>u__1;
		}
	}
}
