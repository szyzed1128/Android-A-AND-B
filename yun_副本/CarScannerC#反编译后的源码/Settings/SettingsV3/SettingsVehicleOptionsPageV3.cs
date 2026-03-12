using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002B2 RID: 690
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml")]
	public class SettingsVehicleOptionsPageV3 : ContentPage
	{
		// Token: 0x060021BE RID: 8638 RVA: 0x0019A2E8 File Offset: 0x001984E8
		public SettingsVehicleOptionsPageV3()
		{
			try
			{
				this.InitializeComponent();
				base.Appearing += this.SettingsVehicleOptionsPageV3_Appearing;
				base.Disappearing += this.SettingsPage_Disappearing;
				base.BindingContext = SharedSettings.Current;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0019A348 File Offset: 0x00198548
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

		// Token: 0x060021C0 RID: 8640 RVA: 0x0019A374 File Offset: 0x00198574
		private void SettingsVehicleOptionsPageV3_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
			PermissionStatus hasGPSPermission = this.HasGPSPermission;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x0007CD3B File Offset: 0x0007AF3B
		private void BtnLocationPermissions_Clicked(object sender, EventArgs e)
		{
			PermissionHelper.OpenPermissionsSettings();
		}

		// Token: 0x170010E4 RID: 4324
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x0019A390 File Offset: 0x00198590
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x0019A3BC File Offset: 0x001985BC
		public PermissionStatus HasGPSPermission
		{
			get
			{
				if (this._HasGPSPermission == null)
				{
					PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
					{
						this.HasGPSPermission = status;
					});
				}
				return this._HasGPSPermission.GetValueOrDefault();
			}
			private set
			{
				this._HasGPSPermission = new PermissionStatus?(value);
				this.OnPropertyChanged("HasGPSPermission");
			}
		}

		// Token: 0x060021C4 RID: 8644 RVA: 0x0019A3D8 File Offset: 0x001985D8
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

		// Token: 0x060021C5 RID: 8645 RVA: 0x0019A428 File Offset: 0x00198628
		private async void CellGPSPermissionStatus_Tapped(object sender, EventArgs e)
		{
			this.cellGPSPermissionStatus.IsEnabled = false;
			await PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
			{
				this.HasGPSPermission = status;
			});
			this.cellGPSPermissionStatus.IsEnabled = true;
		}

		// Token: 0x060021C6 RID: 8646 RVA: 0x0019A460 File Offset: 0x00198660
		private async void btnSpeedCalibrationAuto_Clicked(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.UseGPS)
			{
				await base.DisplayAlert(Translate.GetString("Settings_EnableGPSSpeedFirst_Title"), Translate.GetString("Settings_EnableGPSSpeedFirst_Text"), "OK");
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("Settings_AutoSpeedCalibration_Title"), Translate.GetString("Settings_AutoSpeedCalibration_Text"), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					SharedSettings.Current.SpeedCorrectionFactor = 1.0;
					SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
					SharedSettings.Current.SpeedCalibrationTaskPending = true;
				}
			}
		}

		// Token: 0x060021C7 RID: 8647 RVA: 0x0010D0E3 File Offset: 0x0010B2E3
		private void btnCancelSpeedCalibrationAuto_Clicked(object sender, EventArgs e)
		{
			SpeedCalibrationModelV2.Instance = null;
			SharedSettings.Current.SpeedCalibrationTaskPending = false;
		}

		// Token: 0x060021C8 RID: 8648 RVA: 0x0019A498 File Offset: 0x00198698
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsVehicleOptionsPageV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			EmptyConverterParameterToNotSelectedStringConverter emptyConverterParameterToNotSelectedStringConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyConverterParameterToNotSelectedStringConverter = new EmptyConverterParameterToNotSelectedStringConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			PermissionStatus permissionStatus = 0;
			PermissionStatus permissionStatus2 = 1;
			PermissionStatus permissionStatus3 = 2;
			PermissionStatus permissionStatus4 = 4;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			PermissionStatusToColorConverter permissionStatusToColorConverter;
			VisualDiagnostics.RegisterSourceInfo(permissionStatusToColorConverter = new PermissionStatusToColorConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			PermissionStatusToStringConverter permissionStatusToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(permissionStatusToStringConverter = new PermissionStatusToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			MultiBooleanToTrueConverter multiBooleanToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(multiBooleanToTrueConverter = new MultiBooleanToTrueConverter(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 59);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 91);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 31);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 106);
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 31);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 100);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 67);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 21);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 21);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 49);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 91);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 21);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 21);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 21);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 21);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 21);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 21);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 21);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 21);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 49);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 98);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 39);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 30);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 14);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 25);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 49);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 112);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 49);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 99);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 18);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 49);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 111);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 14);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 89);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 31);
			BoostCalculationMethods boostCalculationMethods = BoostCalculationMethods.Auto;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 31);
			BoostCalculationMethods boostCalculationMethods2 = BoostCalculationMethods.MAF;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 31);
			BoostCalculationMethods boostCalculationMethods3 = BoostCalculationMethods.MAP;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 31);
			BoostCalculationMethods boostCalculationMethods4 = BoostCalculationMethods.LOAD_ABS;
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 14);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 25);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 49);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 120);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 18);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 21);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 18);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 97);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched8;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched8 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 14);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 25);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 75);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 28);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 14);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 25);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 95);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 28);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 28);
			NumericValidationBehavior numericValidationBehavior;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior = new NumericValidationBehavior(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 30);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 22);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsVehicleOptionsPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("me", this);
			if (this.StyleId == null)
			{
				this.StyleId = "me";
			}
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("btnSpeedCalibrationAuto", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnSpeedCalibrationAuto";
			}
			nameScope.RegisterName("btnCancelSpeedCalibrationAuto", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnCancelSpeedCalibrationAuto";
			}
			nameScope.RegisterName("cellGPSPermissionStatus", labelCell);
			if (labelCell.StyleId == null)
			{
				labelCell.StyleId = "cellGPSPermissionStatus";
			}
			this.me = this;
			this.settingsLayoutRoot = settingsView;
			this.btnSpeedCalibrationAuto = buttonCell;
			this.btnCancelSpeedCalibrationAuto = buttonCell2;
			this.cellGPSPermissionStatus = labelCell;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("EmptyConverterParameterToNotSelectedStringConverter", emptyConverterParameterToNotSelectedStringConverter);
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("GPSStateNotGrantedToTrueConverter", valueConverter);
			resourceDictionary.Add("PermissionStatusToColorConverter", permissionStatusToColorConverter);
			resourceDictionary.Add("PermissionStatusToStringConverter", permissionStatusToStringConverter);
			resourceDictionary.Add("MultiBooleanToTrueConverter", multiBooleanToTrueConverter);
			translate.Text = "Settings_Control_VehicleOptions.Header";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Page.TitleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver2.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.Title = obj3;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver3.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 5)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbFuelTankCapacity.Text";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = section;
			array4[1] = settingsView;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver4.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 25)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			section.Title = obj6;
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension.Mode = 1;
			bindingExtension.Path = "FuelTankCapacity";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.FuelTankCapacity, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelTankCapacity = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelTankCapacity")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase);
			customCell.SetValue(CustomCell.ContentProperty, numericEntryV);
			section.Add(customCell);
			settingsView.Root.Add(section);
			translate3.Text = "Settings_Control_tbDisplayAFR.Text";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = section2;
			array5[1] = settingsView;
			array5[2] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver5.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			section2.Title = obj8;
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "ShowAirFuelBasedOnStoichiometric";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowAirFuelBasedOnStoichiometric, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowAirFuelBasedOnStoichiometric = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowAirFuelBasedOnStoichiometric")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section2.SetBinding(RadioCell.SelectedValueProperty, bindingBase2);
			translate4.Text = "Settings_Control_AFR_Stoichiometric.Content";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = radioCell;
			array6[1] = section2;
			array6[2] = settingsView;
			array6[3] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, CellBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver6.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 31)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			radioCell.Title = obj10;
			staticResourceExtension.Key = "TrueValue";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = radioCell;
			array7[1] = section2;
			array7[2] = settingsView;
			array7[3] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver7.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 106)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			radioCell.SetValue(RadioCell.ValueProperty, obj12);
			section2.Add(radioCell);
			translate5.Text = "Settings_Control_AFR_RawValue.Content";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = radioCell2;
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
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver8.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 31)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			radioCell2.Title = obj14;
			staticResourceExtension2.Key = "FalseValue";
			IMarkupExtension markupExtension9 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = radioCell2;
			array9[1] = section2;
			array9[2] = settingsView;
			array9[3] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver9.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 100)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			radioCell2.SetValue(RadioCell.ValueProperty, obj16);
			section2.Add(radioCell2);
			settingsView.Root.Add(section2);
			translate6.Text = "ios_SpeedCorrectionFactor";
			IMarkupExtension markupExtension10 = translate6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = section3;
			array10[1] = settingsView;
			array10[2] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver10.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 25)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			section3.Title = obj18;
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV2.SetValue(NumericEntryV3.DoubleFormatProperty, "0.#########");
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "SpeedCorrectionFactor";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.SpeedCorrectionFactor, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.SpeedCorrectionFactor = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SpeedCorrectionFactor")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase3);
			customCell2.SetValue(CustomCell.ContentProperty, numericEntryV2);
			section3.Add(customCell2);
			translate7.Text = "Settings_AutoSpeedCalibration";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = buttonCell;
			array11[1] = section3;
			array11[2] = settingsView;
			array11[3] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, CellBase.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver11.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 21)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			buttonCell.Title = obj20;
			bindingExtension4.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = bindingExtension4;
			array12[1] = buttonCell;
			array12[2] = section3;
			array12[3] = settingsView;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver12.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 21)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension4.Converter = obj22;
			bindingExtension4.Path = "SpeedCalibrationTaskPending";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SpeedCalibrationTaskPending, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SpeedCalibrationTaskPending")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			buttonCell.SetBinding(CellBase.IsVisibleProperty, bindingBase4);
			buttonCell.Tapped += this.btnSpeedCalibrationAuto_Clicked;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = buttonCell;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver13.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 21)));
			DynamicResource dynamicResource2 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section3.Add(buttonCell);
			translate8.Text = "Settings_CancelAutoSpeedCalibration";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = buttonCell2;
			array14[1] = section3;
			array14[2] = settingsView;
			array14[3] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array14, CellBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver14.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 21)));
			object obj25 = markupExtension14.ProvideValue(xamlServiceProvider14);
			buttonCell2.Title = obj25;
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "SpeedCalibrationTaskPending";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SpeedCalibrationTaskPending, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SpeedCalibrationTaskPending")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			buttonCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase5);
			buttonCell2.Tapped += this.btnCancelSpeedCalibrationAuto_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = buttonCell2;
			array15[1] = section3;
			array15[2] = settingsView;
			array15[3] = this;
			object obj26;
			xamlServiceProvider15.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array15, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver15.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			DynamicResource dynamicResource3 = markupExtension15.ProvideValue(xamlServiceProvider15);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section3.Add(buttonCell2);
			settingsView.Root.Add(section3);
			section4.SetValue(SectionBase.TitleProperty, "GPS");
			translate9.Text = "ios_UseGPS";
			IMarkupExtension markupExtension16 = translate9;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = settingsCheckBoxCellPatched;
			array16[1] = section4;
			array16[2] = settingsView;
			array16[3] = this;
			object obj27;
			xamlServiceProvider16.Add(typeFromHandle31, obj27 = new SimpleValueTargetProvider(array16, CellBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver16.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 49)));
			object obj28 = markupExtension16.ProvideValue(xamlServiceProvider16);
			settingsCheckBoxCellPatched.Title = obj28;
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "UseGPS";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseGPS, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseGPS = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseGPS")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase6);
			section4.Add(settingsCheckBoxCellPatched);
			translate10.Text = "settings_GPSPermissionStatus";
			IMarkupExtension markupExtension17 = translate10;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = labelCell;
			array17[1] = section4;
			array17[2] = settingsView;
			array17[3] = this;
			object obj29;
			xamlServiceProvider17.Add(typeFromHandle33, obj29 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver17.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 21)));
			object obj30 = markupExtension17.ProvideValue(xamlServiceProvider17);
			labelCell.Title = obj30;
			referenceExtension.Name = "me";
			IMarkupExtension markupExtension18 = referenceExtension;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = labelCell;
			array18[1] = section4;
			array18[2] = settingsView;
			array18[3] = this;
			object obj31;
			xamlServiceProvider18.Add(typeFromHandle35, obj31 = new SimpleValueTargetProvider(array18, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver18.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 21)));
			object obj32 = markupExtension18.ProvideValue(xamlServiceProvider18);
			labelCell.SetValue(BindableObject.BindingContextProperty, obj32);
			bindingExtension7.Mode = 2;
			bindingExtension7.Source = sharedSettings;
			bindingExtension7.Path = "UseGPS";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			labelCell.SetBinding(CellBase.IsVisibleProperty, bindingBase7);
			labelCell.Tapped += this.CellGPSPermissionStatus_Tapped;
			bindingExtension8.Mode = 2;
			staticResourceExtension4.Key = "PermissionStatusToStringConverter";
			IMarkupExtension markupExtension19 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = bindingExtension8;
			array19[1] = labelCell;
			array19[2] = section4;
			array19[3] = settingsView;
			array19[4] = this;
			object obj33;
			xamlServiceProvider19.Add(typeFromHandle37, obj33 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver19.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 21)));
			object obj34 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension8.Converter = obj34;
			bindingExtension8.Path = "HasGPSPermission";
			bindingExtension8.TypedBinding = new TypedBinding<SettingsVehicleOptionsPageV3, PermissionStatus>(delegate(SettingsVehicleOptionsPageV3 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
				}
				return default(ValueTuple<PermissionStatus, bool>);
			}, null, new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>[]
			{
				new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>((SettingsVehicleOptionsPageV3 A_0) => A_0, "HasGPSPermission")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase8);
			bindingExtension9.Mode = 2;
			staticResourceExtension5.Key = "PermissionStatusToColorConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = bindingExtension9;
			array20[1] = labelCell;
			array20[2] = section4;
			array20[3] = settingsView;
			array20[4] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle39, obj35 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver20.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 21)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension9.Converter = obj36;
			bindingExtension9.Path = "HasGPSPermission";
			bindingExtension9.TypedBinding = new TypedBinding<SettingsVehicleOptionsPageV3, PermissionStatus>(delegate(SettingsVehicleOptionsPageV3 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
				}
				return default(ValueTuple<PermissionStatus, bool>);
			}, null, new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>[]
			{
				new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>((SettingsVehicleOptionsPageV3 A_0) => A_0, "HasGPSPermission")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			labelCell.SetBinding(LabelCell.ValueTextColorProperty, bindingBase9);
			section4.Add(labelCell);
			translate11.Text = "ios_Permissions";
			IMarkupExtension markupExtension21 = translate11;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = buttonCell3;
			array21[1] = section4;
			array21[2] = settingsView;
			array21[3] = this;
			object obj37;
			xamlServiceProvider21.Add(typeFromHandle41, obj37 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver21.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 21)));
			object obj38 = markupExtension21.ProvideValue(xamlServiceProvider21);
			buttonCell3.Title = obj38;
			referenceExtension2.Name = "me";
			IMarkupExtension markupExtension22 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = buttonCell3;
			array22[1] = section4;
			array22[2] = settingsView;
			array22[3] = this;
			object obj39;
			xamlServiceProvider22.Add(typeFromHandle43, obj39 = new SimpleValueTargetProvider(array22, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver22.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 21)));
			object obj40 = markupExtension22.ProvideValue(xamlServiceProvider22);
			buttonCell3.SetValue(BindableObject.BindingContextProperty, obj40);
			bindingExtension10.Mode = 2;
			staticResourceExtension6.Key = "GPSStateNotGrantedToTrueConverter";
			IMarkupExtension markupExtension23 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = bindingExtension10;
			array23[1] = buttonCell3;
			array23[2] = section4;
			array23[3] = settingsView;
			array23[4] = this;
			object obj41;
			xamlServiceProvider23.Add(typeFromHandle45, obj41 = new SimpleValueTargetProvider(array23, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver23.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 21)));
			object obj42 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension10.Converter = obj42;
			bindingExtension10.Path = "HasGPSPermission";
			bindingExtension10.TypedBinding = new TypedBinding<SettingsVehicleOptionsPageV3, PermissionStatus>(delegate(SettingsVehicleOptionsPageV3 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
				}
				return default(ValueTuple<PermissionStatus, bool>);
			}, null, new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>[]
			{
				new Tuple<Func<SettingsVehicleOptionsPageV3, object>, string>((SettingsVehicleOptionsPageV3 A_0) => A_0, "HasGPSPermission")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			buttonCell3.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			buttonCell3.Tapped += this.BtnLocationPermissions_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = buttonCell3;
			array24[1] = section4;
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
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver24.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 21)));
			DynamicResource dynamicResource4 = markupExtension24.ProvideValue(xamlServiceProvider24);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section4.Add(buttonCell3);
			translate12.Text = "records_RecordGPS";
			IMarkupExtension markupExtension25 = translate12;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = settingsCheckBoxCellPatched2;
			array25[1] = section4;
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
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver25.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 49)));
			object obj45 = markupExtension25.ProvideValue(xamlServiceProvider25);
			settingsCheckBoxCellPatched2.Title = obj45;
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "RecordLocationData";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.RecordLocationData, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.RecordLocationData = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "RecordLocationData")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase11);
			staticResourceExtension7.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension26 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 5];
			array26[0] = multiBinding;
			array26[1] = settingsCheckBoxCellPatched2;
			array26[2] = section4;
			array26[3] = settingsView;
			array26[4] = this;
			object obj46;
			xamlServiceProvider26.Add(typeFromHandle51, obj46 = new SimpleValueTargetProvider(array26, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver26.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 39)));
			object obj47 = markupExtension26.ProvideValue(xamlServiceProvider26);
			multiBinding.Converter = obj47;
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "UseGPS";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "ShowExperimental";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase13);
			settingsCheckBoxCellPatched2.SetBinding(CellBase.IsVisibleProperty, multiBinding);
			section4.Add(settingsCheckBoxCellPatched2);
			settingsView.Root.Add(section4);
			translate13.Text = "ios_MainPage_TileDtcErrors";
			IMarkupExtension markupExtension27 = translate13;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 3];
			array27[0] = section5;
			array27[1] = settingsView;
			array27[2] = this;
			object obj48;
			xamlServiceProvider27.Add(typeFromHandle53, obj48 = new SimpleValueTargetProvider(array27, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver27.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 25)));
			object obj49 = markupExtension27.ProvideValue(xamlServiceProvider27);
			section5.Title = obj49;
			translate14.Text = "ios_HideDTCWithUncomplitedTests";
			IMarkupExtension markupExtension28 = translate14;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = settingsCheckBoxCellPatched3;
			array28[1] = section5;
			array28[2] = settingsView;
			array28[3] = this;
			object obj50;
			xamlServiceProvider28.Add(typeFromHandle55, obj50 = new SimpleValueTargetProvider(array28, CellBase.TitleProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver28.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 49)));
			object obj51 = markupExtension28.ProvideValue(xamlServiceProvider28);
			settingsCheckBoxCellPatched3.Title = obj51;
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "HideDTCWithUncomplitedTests";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HideDTCWithUncomplitedTests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.HideDTCWithUncomplitedTests = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "HideDTCWithUncomplitedTests")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase14);
			section5.Add(settingsCheckBoxCellPatched3);
			translate15.Text = "ios_SkipArchiveDTC";
			IMarkupExtension markupExtension29 = translate15;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = settingsCheckBoxCellPatched4;
			array29[1] = section5;
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
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver29.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 49)));
			object obj53 = markupExtension29.ProvideValue(xamlServiceProvider29);
			settingsCheckBoxCellPatched4.Title = obj53;
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "HideArchiveDTC";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HideArchiveDTC, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.HideArchiveDTC = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "HideArchiveDTC")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase15);
			section5.Add(settingsCheckBoxCellPatched4);
			translate16.Text = "settings_VWTPOpenSessionForDTC";
			IMarkupExtension markupExtension30 = translate16;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = settingsCheckBoxCellPatched5;
			array30[1] = section5;
			array30[2] = settingsView;
			array30[3] = this;
			object obj54;
			xamlServiceProvider30.Add(typeFromHandle59, obj54 = new SimpleValueTargetProvider(array30, CellBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver30.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 49)));
			object obj55 = markupExtension30.ProvideValue(xamlServiceProvider30);
			settingsCheckBoxCellPatched5.Title = obj55;
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "VWTP20OpenSessionForDTCOperations";
			bindingExtension16.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.VWTP20OpenSessionForDTCOperations, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.VWTP20OpenSessionForDTCOperations = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VWTP20OpenSessionForDTCOperations")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase16);
			section5.Add(settingsCheckBoxCellPatched5);
			settingsView.Root.Add(section5);
			translate17.Text = "ios_SelectBoostCalculationMethod";
			IMarkupExtension markupExtension31 = translate17;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 3];
			array31[0] = section6;
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
			xmlNamespaceResolver31.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver31.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 25)));
			object obj57 = markupExtension31.ProvideValue(xamlServiceProvider31);
			section6.Title = obj57;
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "BoostCalculationMethod";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, BoostCalculationMethods>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<BoostCalculationMethods, bool>(A_0.BoostCalculationMethod, true);
				}
				return default(ValueTuple<BoostCalculationMethods, bool>);
			}, delegate(SharedSettings A_0, BoostCalculationMethods A_1)
			{
				if (A_0 != null)
				{
					A_0.BoostCalculationMethod = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "BoostCalculationMethod")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			section6.SetBinding(RadioCell.SelectedValueProperty, bindingBase17);
			translate18.Text = "Settings_Control_FuelScheme_Auto.Content";
			IMarkupExtension markupExtension32 = translate18;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = radioCell3;
			array32[1] = section6;
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
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver32.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 31)));
			object obj59 = markupExtension32.ProvideValue(xamlServiceProvider32);
			radioCell3.Title = obj59;
			radioCell3.SetValue(RadioCell.ValueProperty, boostCalculationMethods);
			section6.Add(radioCell3);
			translate19.Text = "Settings_Control_FuelScheme_MAF.Content";
			IMarkupExtension markupExtension33 = translate19;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = radioCell4;
			array33[1] = section6;
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
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver33.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 31)));
			object obj61 = markupExtension33.ProvideValue(xamlServiceProvider33);
			radioCell4.Title = obj61;
			radioCell4.SetValue(RadioCell.ValueProperty, boostCalculationMethods2);
			section6.Add(radioCell4);
			translate20.Text = "Settings_Control_FuelScheme_MAP.Content";
			IMarkupExtension markupExtension34 = translate20;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = radioCell5;
			array34[1] = section6;
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
			xmlNamespaceResolver34.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver34.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 31)));
			object obj63 = markupExtension34.ProvideValue(xamlServiceProvider34);
			radioCell5.Title = obj63;
			radioCell5.SetValue(RadioCell.ValueProperty, boostCalculationMethods3);
			section6.Add(radioCell5);
			translate21.Text = "Settings_Control_FuelScheme_AbsLOAD.Content";
			IMarkupExtension markupExtension35 = translate21;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = radioCell6;
			array35[1] = section6;
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
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver35.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 31)));
			object obj65 = markupExtension35.ProvideValue(xamlServiceProvider35);
			radioCell6.Title = obj65;
			radioCell6.SetValue(RadioCell.ValueProperty, boostCalculationMethods4);
			section6.Add(radioCell6);
			settingsView.Root.Add(section6);
			translate22.Text = "coding_Group_Other";
			IMarkupExtension markupExtension36 = translate22;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 3];
			array36[0] = section7;
			array36[1] = settingsView;
			array36[2] = this;
			object obj66;
			xamlServiceProvider36.Add(typeFromHandle71, obj66 = new SimpleValueTargetProvider(array36, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver36.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 25)));
			object obj67 = markupExtension36.ProvideValue(xamlServiceProvider36);
			section7.Title = obj67;
			translate23.Text = "ios_IgnoreSupportedFlagForPIDs0166_0183";
			IMarkupExtension markupExtension37 = translate23;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = settingsCheckBoxCellPatched6;
			array37[1] = section7;
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
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver37.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 49)));
			object obj69 = markupExtension37.ProvideValue(xamlServiceProvider37);
			settingsCheckBoxCellPatched6.Title = obj69;
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "IgnoreSupportedFlagForPIDs0166_0183";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IgnoreSupportedFlagForPIDs0166_0183, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.IgnoreSupportedFlagForPIDs0166_0183 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "IgnoreSupportedFlagForPIDs0166_0183")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase18);
			section7.Add(settingsCheckBoxCellPatched6);
			translate24.Text = "Settings_Control_tbRPMFix.Text";
			IMarkupExtension markupExtension38 = translate24;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = settingsCheckBoxCellPatched7;
			array38[1] = section7;
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
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver38.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 21)));
			object obj71 = markupExtension38.ProvideValue(xamlServiceProvider38);
			settingsCheckBoxCellPatched7.Title = obj71;
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "UseRPMFix";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseRPMFix, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseRPMFix = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseRPMFix")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase19);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "ShowExperimental";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CellBase.IsVisibleProperty, bindingBase20);
			section7.Add(settingsCheckBoxCellPatched7);
			settingsCheckBoxCellPatched8.SetValue(CellBase.TitleProperty, "Speed PID 2-byte length (default: OFF!)");
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "SpeedPID2Bytes";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SpeedPID2Bytes, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SpeedPID2Bytes = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SpeedPID2Bytes")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CheckboxCell.CheckedProperty, bindingBase21);
			section7.Add(settingsCheckBoxCellPatched8);
			settingsView.Root.Add(section7);
			translate25.Text = "ios_SkipHeadersDTC";
			IMarkupExtension markupExtension39 = translate25;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 3];
			array39[0] = section8;
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
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver39.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 25)));
			object obj73 = markupExtension39.ProvideValue(xamlServiceProvider39);
			section8.Title = obj73;
			bindingExtension22.Path = "ShowExperimental";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			section8.SetBinding(Section.IsVisibleProperty, bindingBase22);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "SkipHeadersDTC";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.SkipHeadersDTC, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.SkipHeadersDTC = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SkipHeadersDTC")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase23);
			customCell3.SetValue(CustomCell.ContentProperty, entry);
			section8.Add(customCell3);
			settingsView.Root.Add(section8);
			translate26.Text = "settings_FramesToCalculateAcceleration";
			IMarkupExtension markupExtension40 = translate26;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 3];
			array40[0] = section9;
			array40[1] = settingsView;
			array40[2] = this;
			object obj74;
			xamlServiceProvider40.Add(typeFromHandle79, obj74 = new SimpleValueTargetProvider(array40, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver40.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(181, 25)));
			object obj75 = markupExtension40.ProvideValue(xamlServiceProvider40);
			section9.Title = obj75;
			bindingExtension24.Path = "ShowExperimental";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
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
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			section9.SetBinding(Section.IsVisibleProperty, bindingBase24);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension25.Mode = 1;
			staticResourceExtension8.Key = "IntToStringConverter";
			IMarkupExtension markupExtension41 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 6];
			array41[0] = bindingExtension25;
			array41[1] = entry2;
			array41[2] = customCell4;
			array41[3] = section9;
			array41[4] = settingsView;
			array41[5] = this;
			object obj76;
			xamlServiceProvider41.Add(typeFromHandle81, obj76 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("setV3", "clr-namespace:CarScannerXamarinForms.Settings.SettingsV3");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xmlNamespaceResolver41.Add("xess", "clr-namespace:Xamarin.Essentials;assembly=Xamarin.Essentials");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsVehicleOptionsPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 28)));
			object obj77 = markupExtension41.ProvideValue(xamlServiceProvider41);
			bindingExtension25.Converter = obj77;
			bindingExtension25.Path = "AccelerationItems";
			bindingExtension25.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.AccelerationItems, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.AccelerationItems = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationItems")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase25);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumValueProperty, 2.0);
			((ICollection<Behavior>)entry2.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior);
			customCell4.SetValue(CustomCell.ContentProperty, entry2);
			section9.Add(customCell4);
			settingsView.Root.Add(section9);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x060021C9 RID: 8649 RVA: 0x001A0292 File Offset: 0x0019E492
		[CompilerGenerated]
		private void <get_HasGPSPermission>b__5_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
		}

		// Token: 0x060021CA RID: 8650 RVA: 0x001A0292 File Offset: 0x0019E492
		[CompilerGenerated]
		private void <CellGPSPermissionStatus_Tapped>b__9_0(PermissionStatus status)
		{
			this.HasGPSPermission = status;
		}

		// Token: 0x060021CB RID: 8651 RVA: 0x001A029C File Offset: 0x0019E49C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsVehicleOptionsPageV3>(this, typeof(SettingsVehicleOptionsPageV3));
			this.me = NameScopeExtensions.FindByName<ContentPage>(this, "me");
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.btnSpeedCalibrationAuto = NameScopeExtensions.FindByName<ButtonCell>(this, "btnSpeedCalibrationAuto");
			this.btnCancelSpeedCalibrationAuto = NameScopeExtensions.FindByName<ButtonCell>(this, "btnCancelSpeedCalibrationAuto");
			this.cellGPSPermissionStatus = NameScopeExtensions.FindByName<LabelCell>(this, "cellGPSPermissionStatus");
		}

		// Token: 0x060021CC RID: 8652 RVA: 0x001A0310 File Offset: 0x0019E510
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2125(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.FuelTankCapacity, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060021CD RID: 8653 RVA: 0x001A0340 File Offset: 0x0019E540
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2126(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelTankCapacity = A_1;
				return;
			}
		}

		// Token: 0x060021CE RID: 8654 RVA: 0x001A035C File Offset: 0x0019E55C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2127(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021CF RID: 8655 RVA: 0x001A036C File Offset: 0x0019E56C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2128(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAirFuelBasedOnStoichiometric, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x001A039C File Offset: 0x0019E59C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2129(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowAirFuelBasedOnStoichiometric = A_1;
				return;
			}
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x001A03B8 File Offset: 0x0019E5B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2130(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021D2 RID: 8658 RVA: 0x001A03C8 File Offset: 0x0019E5C8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2131(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.SpeedCorrectionFactor, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x001A03F8 File Offset: 0x0019E5F8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2132(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.SpeedCorrectionFactor = A_1;
				return;
			}
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x001A0414 File Offset: 0x0019E614
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2133(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021D5 RID: 8661 RVA: 0x001A0424 File Offset: 0x0019E624
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2134(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SpeedCalibrationTaskPending, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021D6 RID: 8662 RVA: 0x001A0454 File Offset: 0x0019E654
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2135(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021D7 RID: 8663 RVA: 0x001A0464 File Offset: 0x0019E664
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2136(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SpeedCalibrationTaskPending, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021D8 RID: 8664 RVA: 0x001A0494 File Offset: 0x0019E694
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2137(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021D9 RID: 8665 RVA: 0x001A04A4 File Offset: 0x0019E6A4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2138(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseGPS, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021DA RID: 8666 RVA: 0x001A04D4 File Offset: 0x0019E6D4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2139(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseGPS = A_1;
				return;
			}
		}

		// Token: 0x060021DB RID: 8667 RVA: 0x001A04F0 File Offset: 0x0019E6F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2140(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021DC RID: 8668 RVA: 0x001A0500 File Offset: 0x0019E700
		[CompilerGenerated]
		private static ValueTuple<PermissionStatus, bool> <InitializeComponent>typedBindingsM__2141(SettingsVehicleOptionsPageV3 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
			}
			return default(ValueTuple<PermissionStatus, bool>);
		}

		// Token: 0x060021DD RID: 8669 RVA: 0x001A0530 File Offset: 0x0019E730
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2142(SettingsVehicleOptionsPageV3 A_0)
		{
			return A_0;
		}

		// Token: 0x060021DE RID: 8670 RVA: 0x001A0540 File Offset: 0x0019E740
		[CompilerGenerated]
		private static ValueTuple<PermissionStatus, bool> <InitializeComponent>typedBindingsM__2143(SettingsVehicleOptionsPageV3 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
			}
			return default(ValueTuple<PermissionStatus, bool>);
		}

		// Token: 0x060021DF RID: 8671 RVA: 0x001A0570 File Offset: 0x0019E770
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2144(SettingsVehicleOptionsPageV3 A_0)
		{
			return A_0;
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x001A0580 File Offset: 0x0019E780
		[CompilerGenerated]
		private static ValueTuple<PermissionStatus, bool> <InitializeComponent>typedBindingsM__2145(SettingsVehicleOptionsPageV3 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<PermissionStatus, bool>(A_0.HasGPSPermission, true);
			}
			return default(ValueTuple<PermissionStatus, bool>);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x001A05B0 File Offset: 0x0019E7B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2146(SettingsVehicleOptionsPageV3 A_0)
		{
			return A_0;
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x001A05C0 File Offset: 0x0019E7C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2147(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.RecordLocationData, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x001A05F0 File Offset: 0x0019E7F0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2148(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.RecordLocationData = A_1;
				return;
			}
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x001A060C File Offset: 0x0019E80C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2149(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021E5 RID: 8677 RVA: 0x001A061C File Offset: 0x0019E81C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2150(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HideDTCWithUncomplitedTests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021E6 RID: 8678 RVA: 0x001A064C File Offset: 0x0019E84C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2151(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.HideDTCWithUncomplitedTests = A_1;
				return;
			}
		}

		// Token: 0x060021E7 RID: 8679 RVA: 0x001A0668 File Offset: 0x0019E868
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2152(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021E8 RID: 8680 RVA: 0x001A0678 File Offset: 0x0019E878
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2153(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HideArchiveDTC, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021E9 RID: 8681 RVA: 0x001A06A8 File Offset: 0x0019E8A8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2154(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.HideArchiveDTC = A_1;
				return;
			}
		}

		// Token: 0x060021EA RID: 8682 RVA: 0x001A06C4 File Offset: 0x0019E8C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2155(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021EB RID: 8683 RVA: 0x001A06D4 File Offset: 0x0019E8D4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2156(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.VWTP20OpenSessionForDTCOperations, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021EC RID: 8684 RVA: 0x001A0704 File Offset: 0x0019E904
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2157(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.VWTP20OpenSessionForDTCOperations = A_1;
				return;
			}
		}

		// Token: 0x060021ED RID: 8685 RVA: 0x001A0720 File Offset: 0x0019E920
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2158(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021EE RID: 8686 RVA: 0x001A0730 File Offset: 0x0019E930
		[CompilerGenerated]
		private static ValueTuple<BoostCalculationMethods, bool> <InitializeComponent>typedBindingsM__2159(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<BoostCalculationMethods, bool>(A_0.BoostCalculationMethod, true);
			}
			return default(ValueTuple<BoostCalculationMethods, bool>);
		}

		// Token: 0x060021EF RID: 8687 RVA: 0x001A0760 File Offset: 0x0019E960
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2160(SharedSettings A_0, BoostCalculationMethods A_1)
		{
			if (A_0 != null)
			{
				A_0.BoostCalculationMethod = A_1;
				return;
			}
		}

		// Token: 0x060021F0 RID: 8688 RVA: 0x001A077C File Offset: 0x0019E97C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2161(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021F1 RID: 8689 RVA: 0x001A078C File Offset: 0x0019E98C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2162(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IgnoreSupportedFlagForPIDs0166_0183, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021F2 RID: 8690 RVA: 0x001A07BC File Offset: 0x0019E9BC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2163(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.IgnoreSupportedFlagForPIDs0166_0183 = A_1;
				return;
			}
		}

		// Token: 0x060021F3 RID: 8691 RVA: 0x001A07D8 File Offset: 0x0019E9D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2164(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021F4 RID: 8692 RVA: 0x001A07E8 File Offset: 0x0019E9E8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2165(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseRPMFix, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021F5 RID: 8693 RVA: 0x001A0818 File Offset: 0x0019EA18
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2166(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseRPMFix = A_1;
				return;
			}
		}

		// Token: 0x060021F6 RID: 8694 RVA: 0x001A0834 File Offset: 0x0019EA34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2167(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021F7 RID: 8695 RVA: 0x001A0844 File Offset: 0x0019EA44
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2168(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021F8 RID: 8696 RVA: 0x001A0874 File Offset: 0x0019EA74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2169(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021F9 RID: 8697 RVA: 0x001A0884 File Offset: 0x0019EA84
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2170(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SpeedPID2Bytes, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021FA RID: 8698 RVA: 0x001A08B4 File Offset: 0x0019EAB4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2171(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SpeedPID2Bytes = A_1;
				return;
			}
		}

		// Token: 0x060021FB RID: 8699 RVA: 0x001A08D0 File Offset: 0x0019EAD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2172(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021FC RID: 8700 RVA: 0x001A08E0 File Offset: 0x0019EAE0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2173(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021FD RID: 8701 RVA: 0x001A0910 File Offset: 0x0019EB10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2174(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x060021FE RID: 8702 RVA: 0x001A092C File Offset: 0x0019EB2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2175(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021FF RID: 8703 RVA: 0x001A093C File Offset: 0x0019EB3C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2176(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.SkipHeadersDTC, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06002200 RID: 8704 RVA: 0x001A096C File Offset: 0x0019EB6C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2177(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.SkipHeadersDTC = A_1;
				return;
			}
		}

		// Token: 0x06002201 RID: 8705 RVA: 0x001A0988 File Offset: 0x0019EB88
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2178(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002202 RID: 8706 RVA: 0x001A0998 File Offset: 0x0019EB98
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2179(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowExperimental, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002203 RID: 8707 RVA: 0x001A09C8 File Offset: 0x0019EBC8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2180(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowExperimental = A_1;
				return;
			}
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x001A09E4 File Offset: 0x0019EBE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2181(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x001A09F4 File Offset: 0x0019EBF4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2182(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.AccelerationItems, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x001A0A24 File Offset: 0x0019EC24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2183(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.AccelerationItems = A_1;
				return;
			}
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x001A0A40 File Offset: 0x0019EC40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2184(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0400101A RID: 4122
		private PermissionStatus? _HasGPSPermission;

		// Token: 0x0400101B RID: 4123
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage me;

		// Token: 0x0400101C RID: 4124
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x0400101D RID: 4125
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnSpeedCalibrationAuto;

		// Token: 0x0400101E RID: 4126
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnCancelSpeedCalibrationAuto;

		// Token: 0x0400101F RID: 4127
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell cellGPSPermissionStatus;

		// Token: 0x020002B3 RID: 691
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CellGPSPermissionStatus_Tapped>d__9 : IAsyncStateMachine
		{
			// Token: 0x06002208 RID: 8712 RVA: 0x001A0A50 File Offset: 0x0019EC50
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsVehicleOptionsPageV3 settingsVehicleOptionsPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsVehicleOptionsPageV.cellGPSPermissionStatus.IsEnabled = false;
						taskAwaiter = PermissionHelper.CheckLocationPermissionStatus(delegate(PermissionStatus status)
						{
							base.HasGPSPermission = status;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsVehicleOptionsPageV3.<CellGPSPermissionStatus_Tapped>d__9>(ref taskAwaiter, ref this);
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
					settingsVehicleOptionsPageV.cellGPSPermissionStatus.IsEnabled = true;
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

			// Token: 0x06002209 RID: 8713 RVA: 0x001A0B28 File Offset: 0x0019ED28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001020 RID: 4128
			public int <>1__state;

			// Token: 0x04001021 RID: 4129
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001022 RID: 4130
			public SettingsVehicleOptionsPageV3 <>4__this;

			// Token: 0x04001023 RID: 4131
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002B4 RID: 692
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSpeedCalibrationAuto_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x0600220A RID: 8714 RVA: 0x001A0B38 File Offset: 0x0019ED38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsVehicleOptionsPageV3 settingsVehicleOptionsPageV = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					if (num != 0)
					{
						TaskAwaiter<bool> taskAwaiter5;
						if (num != 1)
						{
							if (!SharedSettings.Current.UseGPS)
							{
								taskAwaiter3 = settingsVehicleOptionsPageV.DisplayAlert(Translate.GetString("Settings_EnableGPSSpeedFirst_Title"), Translate.GetString("Settings_EnableGPSSpeedFirst_Text"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsVehicleOptionsPageV3.<btnSpeedCalibrationAuto_Clicked>d__10>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0091;
							}
							else
							{
								taskAwaiter5 = settingsVehicleOptionsPageV.DisplayAlert(Translate.GetString("Settings_AutoSpeedCalibration_Title"), Translate.GetString("Settings_AutoSpeedCalibration_Text"), "OK", Translate.GetString("ios_Cancel")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsVehicleOptionsPageV3.<btnSpeedCalibrationAuto_Clicked>d__10>(ref taskAwaiter5, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						if (taskAwaiter5.GetResult())
						{
							SharedSettings.Current.SpeedCorrectionFactor = 1.0;
							SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
							SharedSettings.Current.SpeedCalibrationTaskPending = true;
						}
						goto IL_015D;
					}
					taskAwaiter3 = taskAwaiter4;
					taskAwaiter4 = default(TaskAwaiter);
					num2 = -1;
					IL_0091:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_015D:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600220B RID: 8715 RVA: 0x001A0CD4 File Offset: 0x0019EED4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001024 RID: 4132
			public int <>1__state;

			// Token: 0x04001025 RID: 4133
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001026 RID: 4134
			public SettingsVehicleOptionsPageV3 <>4__this;

			// Token: 0x04001027 RID: 4135
			private TaskAwaiter <>u__1;

			// Token: 0x04001028 RID: 4136
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
