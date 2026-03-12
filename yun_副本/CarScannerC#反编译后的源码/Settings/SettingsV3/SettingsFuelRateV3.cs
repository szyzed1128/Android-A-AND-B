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
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x0200029E RID: 670
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsFuelRateV3.xaml")]
	public class SettingsFuelRateV3 : ContentPage
	{
		// Token: 0x06002076 RID: 8310 RVA: 0x001732EC File Offset: 0x001714EC
		public SettingsFuelRateV3()
		{
			try
			{
				this.InitializeComponent();
				base.Appearing += this.SettingsPage_Appearing;
				base.Disappearing += this.SettingsPage_Disappearing;
				base.BindingContext = SharedSettings.Current;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002077 RID: 8311 RVA: 0x0017334C File Offset: 0x0017154C
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

		// Token: 0x06002078 RID: 8312 RVA: 0x0016D7E4 File Offset: 0x0016B9E4
		private void SettingsPage_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
		}

		// Token: 0x06002079 RID: 8313 RVA: 0x000ED9AA File Offset: 0x000EBBAA
		private void btnCalibration_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsFuelCalibrationPage());
		}

		// Token: 0x0600207A RID: 8314 RVA: 0x00173378 File Offset: 0x00171578
		private async void btnResetStatistics_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetStatsTitle"), Translate.GetString("ios_ResetStatsText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				DriveCycleViewModel.Current.Reset();
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x001733B0 File Offset: 0x001715B0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsFuelRateV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsFuelRateV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ECUInitializationPickerConverter ecuinitializationPickerConverter;
			VisualDiagnostics.RegisterSourceInfo(ecuinitializationPickerConverter = new ECUInitializationPickerConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			NissanProtocolNumberToVisibilityConverter nissanProtocolNumberToVisibilityConverter;
			VisualDiagnostics.RegisterSourceInfo(nissanProtocolNumberToVisibilityConverter = new NissanProtocolNumberToVisibilityConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			DTCReadingModeToIntConverter dtcreadingModeToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcreadingModeToIntConverter = new DTCReadingModeToIntConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			InjectorPidIdToPIDConverter injectorPidIdToPIDConverter;
			VisualDiagnostics.RegisterSourceInfo(injectorPidIdToPIDConverter = new InjectorPidIdToPIDConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ZeroConsumptionPIDIdtoPIDConverter zeroConsumptionPIDIdtoPIDConverter;
			VisualDiagnostics.RegisterSourceInfo(zeroConsumptionPIDIdtoPIDConverter = new ZeroConsumptionPIDIdtoPIDConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			CustomFuelToTrueConverter customFuelToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(customFuelToTrueConverter = new CustomFuelToTrueConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			MultiBooleanToTrueConverter multiBooleanToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(multiBooleanToTrueConverter = new MultiBooleanToTrueConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DecimalFuelPriceForLitreToStringConverter decimalFuelPriceForLitreToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(decimalFuelPriceForLitreToStringConverter = new DecimalFuelPriceForLitreToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes = FuelFlowCalculationSchemes.Auto;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes2 = FuelFlowCalculationSchemes.LOAD_ABS;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes3 = FuelFlowCalculationSchemes.MAP;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes4 = FuelFlowCalculationSchemes.Injector;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes5 = FuelFlowCalculationSchemes.CycleConsumption;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes6 = FuelFlowCalculationSchemes.Auto;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes7 = FuelFlowCalculationSchemes.LOAD_ABS;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes8 = FuelFlowCalculationSchemes.MAP;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes9 = FuelFlowCalculationSchemes.MAF;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes10 = FuelFlowCalculationSchemes.Injector;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes11 = FuelFlowCalculationSchemes.CycleConsumption;
			EnumToBoolConverter enumToBoolConverter2;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter2 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes12 = FuelFlowCalculationSchemes.LOAD_ABS;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes13 = FuelFlowCalculationSchemes.LOAD_ABS;
			EnumToBoolConverter enumToBoolConverter3;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter3 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes14 = FuelFlowCalculationSchemes.MAP;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes15 = FuelFlowCalculationSchemes.MAP;
			EnumToBoolConverter enumToBoolConverter4;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter4 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes16 = FuelFlowCalculationSchemes.MAF;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes17 = FuelFlowCalculationSchemes.MAF;
			EnumToBoolConverter enumToBoolConverter5;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter5 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes18 = FuelFlowCalculationSchemes.Injector;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes19 = FuelFlowCalculationSchemes.Injector;
			EnumToBoolConverter enumToBoolConverter6;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter6 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes20 = FuelFlowCalculationSchemes.CycleConsumption;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes21 = FuelFlowCalculationSchemes.CycleConsumption;
			EnumToBoolConverter enumToBoolConverter7;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter7 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 14);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes22 = FuelFlowCalculationSchemes.FuelRate;
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes23 = FuelFlowCalculationSchemes.FuelRate;
			EnumToBoolConverter enumToBoolConverter8;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter8 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 14);
			FuelTypes fuelTypes = FuelTypes.Gasoline;
			FuelTypes fuelTypes2 = FuelTypes.Diesel;
			FuelTypes fuelTypes3 = FuelTypes.Ethanol;
			FuelTypes fuelTypes4 = FuelTypes.Methanol;
			FuelTypes fuelTypes5 = FuelTypes.Propan;
			FuelTypes fuelTypes6 = FuelTypes.Methan;
			FuelTypes fuelTypes7 = FuelTypes.FlexFuelOBDII;
			FuelTypes fuelTypes8 = FuelTypes.Custom;
			EnumToBoolConverter enumToBoolConverter9;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter9 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 14);
			FuelTypes fuelTypes9 = FuelTypes.EvNoFuel;
			FuelTypes fuelTypes10 = FuelTypes.EvNoFuel;
			EnumToBoolConverter enumToBoolConverter10;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter10 = new EnumToBoolConverter(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 67);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 31);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 100);
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 31);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 95);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 21);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 92);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 31);
			FuelTypes fuelTypes11 = FuelTypes.Gasoline;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 31);
			FuelTypes fuelTypes12 = FuelTypes.Diesel;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 31);
			FuelTypes fuelTypes13 = FuelTypes.EvNoFuel;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 31);
			FuelTypes fuelTypes14 = FuelTypes.Ethanol;
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 31);
			FuelTypes fuelTypes15 = FuelTypes.Methanol;
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 31);
			FuelTypes fuelTypes16 = FuelTypes.Propan;
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 18);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 31);
			FuelTypes fuelTypes17 = FuelTypes.Methan;
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 31);
			FuelTypes fuelTypes18 = FuelTypes.FlexFuelOBDII;
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 31);
			FuelTypes fuelTypes19 = FuelTypes.Custom;
			RadioCell radioCell11;
			VisualDiagnostics.RegisterSourceInfo(radioCell11 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 21);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 14);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 73);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 73);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 57);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 14);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 78);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 78);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 57);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 14);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 83);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 28);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 28);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 18);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 28);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 22);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 18);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 21);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 21);
			NumberPickerCell numberPickerCell;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 18);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 14);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 83);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 83);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 28);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 28);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 22);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 18);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 28);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 22);
			CustomCell customCell6;
			VisualDiagnostics.RegisterSourceInfo(customCell6 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 18);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 32);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 18);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 21);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 21);
			NumberPickerCell numberPickerCell2;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell2 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 18);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 14);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 106);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes24 = FuelFlowCalculationSchemes.Auto;
			RadioCell radioCell12;
			VisualDiagnostics.RegisterSourceInfo(radioCell12 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes25 = FuelFlowCalculationSchemes.MAF;
			RadioCell radioCell13;
			VisualDiagnostics.RegisterSourceInfo(radioCell13 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 18);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes26 = FuelFlowCalculationSchemes.LOAD_ABS;
			RadioCell radioCell14;
			VisualDiagnostics.RegisterSourceInfo(radioCell14 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 18);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes27 = FuelFlowCalculationSchemes.MAP;
			RadioCell radioCell15;
			VisualDiagnostics.RegisterSourceInfo(radioCell15 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 18);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes28 = FuelFlowCalculationSchemes.FuelRate;
			RadioCell radioCell16;
			VisualDiagnostics.RegisterSourceInfo(radioCell16 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 18);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes29 = FuelFlowCalculationSchemes.Injector;
			RadioCell radioCell17;
			VisualDiagnostics.RegisterSourceInfo(radioCell17 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 18);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 31);
			FuelFlowCalculationSchemes fuelFlowCalculationSchemes30 = FuelFlowCalculationSchemes.CycleConsumption;
			RadioCell radioCell18;
			VisualDiagnostics.RegisterSourceInfo(radioCell18 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 18);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 21);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 21);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 18);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 21);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 21);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 21);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched3;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched3 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 18);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 39);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 30);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 30);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 33);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 30);
			MultiBinding multiBinding;
			VisualDiagnostics.RegisterSourceInfo(multiBinding = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 26);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 34);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 32);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 26);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 29);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 29);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 29);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 22);
			CustomCell customCell7;
			VisualDiagnostics.RegisterSourceInfo(customCell7 = new CustomCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 18);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 49);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 128);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 39);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 30);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 30);
			MultiBinding multiBinding2;
			VisualDiagnostics.RegisterSourceInfo(multiBinding2 = new MultiBinding(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 26);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched4;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched4 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 14);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 25);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 99);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 99);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 29);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 74);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(double), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 38);
			double num = 0.1;
			double num2 = 0.2;
			double num3 = 0.3;
			double num4 = 0.4;
			double num5 = 0.5;
			double num6 = 0.6;
			double num7 = 0.7;
			double num8 = 0.8;
			double num9 = 0.9;
			double num10 = 1.0;
			double num11 = 1.1;
			double num12 = 1.2;
			double num13 = 1.3;
			double num14 = 1.4;
			double num15 = 1.5;
			double num16 = 1.6;
			double num17 = 1.7;
			double num18 = 1.8;
			double num19 = 1.9;
			double num20 = 2.0;
			double num21 = 2.1;
			double num22 = 2.2;
			double num23 = 2.3;
			double num24 = 2.4;
			double num25 = 2.5;
			double num26 = 2.6;
			double num27 = 2.7;
			double num28 = 2.8;
			double num29 = 2.9;
			double num30 = 3.0;
			double num31 = 3.1;
			double num32 = 3.2;
			double num33 = 3.3;
			double num34 = 3.4;
			double num35 = 3.5;
			double num36 = 3.6;
			double num37 = 3.7;
			double num38 = 3.8;
			double num39 = 3.9;
			double num40 = 4.0;
			double num41 = 4.1;
			double num42 = 4.2;
			double num43 = 4.3;
			double num44 = 4.4;
			double num45 = 4.5;
			double num46 = 4.6;
			double num47 = 4.7;
			double num48 = 4.8;
			double num49 = 4.9;
			double num50 = 5.0;
			double num51 = 5.1;
			double num52 = 5.2;
			double num53 = 5.3;
			double num54 = 5.4;
			double num55 = 5.5;
			double num56 = 5.6;
			double num57 = 5.7;
			double num58 = 5.8;
			double num59 = 5.9;
			double num60 = 6.0;
			double num61 = 6.1;
			double num62 = 6.2;
			double num63 = 6.3;
			double num64 = 6.4;
			double num65 = 6.5;
			double num66 = 6.6;
			double num67 = 6.7;
			double num68 = 6.8;
			double num69 = 6.9;
			double num70 = 7.0;
			double num71 = 7.1;
			double num72 = 7.2;
			double num73 = 7.3;
			double num74 = 7.4;
			double num75 = 7.5;
			double num76 = 7.6;
			double num77 = 7.7;
			double num78 = 7.8;
			double num79 = 7.9;
			double num80 = 8.0;
			double num81 = 8.1;
			double num82 = 8.2;
			double num83 = 8.3;
			double num84 = 8.4;
			double num85 = 8.5;
			double num86 = 8.6;
			double num87 = 8.7;
			double num88 = 8.8;
			double num89 = 8.9;
			double num90 = 9.0;
			double num91 = 9.1;
			double num92 = 9.2;
			double num93 = 9.3;
			double num94 = 9.4;
			double num95 = 9.5;
			double num96 = 9.6;
			double num97 = 9.7;
			double num98 = 9.8;
			double num99 = 9.9;
			double num100 = 10.0;
			double num101 = 10.1;
			double num102 = 10.2;
			double num103 = 10.3;
			double num104 = 10.4;
			double num105 = 10.5;
			double num106 = 10.6;
			double num107 = 10.7;
			double num108 = 10.8;
			double num109 = 10.9;
			double num110 = 11.0;
			double num111 = 11.1;
			double num112 = 11.2;
			double num113 = 11.3;
			double num114 = 11.4;
			double num115 = 11.5;
			double num116 = 11.6;
			double num117 = 11.7;
			double num118 = 11.8;
			double num119 = 11.9;
			double num120 = 12.0;
			double num121 = 12.1;
			double num122 = 12.2;
			double num123 = 12.3;
			double num124 = 12.4;
			double num125 = 12.5;
			double num126 = 12.6;
			double num127 = 12.7;
			double num128 = 12.8;
			double num129 = 12.9;
			ArrayExtension arrayExtension;
			(arrayExtension = new ArrayExtension()).Type = typeFromHandle;
			arrayExtension.Items.Add(num);
			arrayExtension.Items.Add(num2);
			arrayExtension.Items.Add(num3);
			arrayExtension.Items.Add(num4);
			arrayExtension.Items.Add(num5);
			arrayExtension.Items.Add(num6);
			arrayExtension.Items.Add(num7);
			arrayExtension.Items.Add(num8);
			arrayExtension.Items.Add(num9);
			arrayExtension.Items.Add(num10);
			arrayExtension.Items.Add(num11);
			arrayExtension.Items.Add(num12);
			arrayExtension.Items.Add(num13);
			arrayExtension.Items.Add(num14);
			arrayExtension.Items.Add(num15);
			arrayExtension.Items.Add(num16);
			arrayExtension.Items.Add(num17);
			arrayExtension.Items.Add(num18);
			arrayExtension.Items.Add(num19);
			arrayExtension.Items.Add(num20);
			arrayExtension.Items.Add(num21);
			arrayExtension.Items.Add(num22);
			arrayExtension.Items.Add(num23);
			arrayExtension.Items.Add(num24);
			arrayExtension.Items.Add(num25);
			arrayExtension.Items.Add(num26);
			arrayExtension.Items.Add(num27);
			arrayExtension.Items.Add(num28);
			arrayExtension.Items.Add(num29);
			arrayExtension.Items.Add(num30);
			arrayExtension.Items.Add(num31);
			arrayExtension.Items.Add(num32);
			arrayExtension.Items.Add(num33);
			arrayExtension.Items.Add(num34);
			arrayExtension.Items.Add(num35);
			arrayExtension.Items.Add(num36);
			arrayExtension.Items.Add(num37);
			arrayExtension.Items.Add(num38);
			arrayExtension.Items.Add(num39);
			arrayExtension.Items.Add(num40);
			arrayExtension.Items.Add(num41);
			arrayExtension.Items.Add(num42);
			arrayExtension.Items.Add(num43);
			arrayExtension.Items.Add(num44);
			arrayExtension.Items.Add(num45);
			arrayExtension.Items.Add(num46);
			arrayExtension.Items.Add(num47);
			arrayExtension.Items.Add(num48);
			arrayExtension.Items.Add(num49);
			arrayExtension.Items.Add(num50);
			arrayExtension.Items.Add(num51);
			arrayExtension.Items.Add(num52);
			arrayExtension.Items.Add(num53);
			arrayExtension.Items.Add(num54);
			arrayExtension.Items.Add(num55);
			arrayExtension.Items.Add(num56);
			arrayExtension.Items.Add(num57);
			arrayExtension.Items.Add(num58);
			arrayExtension.Items.Add(num59);
			arrayExtension.Items.Add(num60);
			arrayExtension.Items.Add(num61);
			arrayExtension.Items.Add(num62);
			arrayExtension.Items.Add(num63);
			arrayExtension.Items.Add(num64);
			arrayExtension.Items.Add(num65);
			arrayExtension.Items.Add(num66);
			arrayExtension.Items.Add(num67);
			arrayExtension.Items.Add(num68);
			arrayExtension.Items.Add(num69);
			arrayExtension.Items.Add(num70);
			arrayExtension.Items.Add(num71);
			arrayExtension.Items.Add(num72);
			arrayExtension.Items.Add(num73);
			arrayExtension.Items.Add(num74);
			arrayExtension.Items.Add(num75);
			arrayExtension.Items.Add(num76);
			arrayExtension.Items.Add(num77);
			arrayExtension.Items.Add(num78);
			arrayExtension.Items.Add(num79);
			arrayExtension.Items.Add(num80);
			arrayExtension.Items.Add(num81);
			arrayExtension.Items.Add(num82);
			arrayExtension.Items.Add(num83);
			arrayExtension.Items.Add(num84);
			arrayExtension.Items.Add(num85);
			arrayExtension.Items.Add(num86);
			arrayExtension.Items.Add(num87);
			arrayExtension.Items.Add(num88);
			arrayExtension.Items.Add(num89);
			arrayExtension.Items.Add(num90);
			arrayExtension.Items.Add(num91);
			arrayExtension.Items.Add(num92);
			arrayExtension.Items.Add(num93);
			arrayExtension.Items.Add(num94);
			arrayExtension.Items.Add(num95);
			arrayExtension.Items.Add(num96);
			arrayExtension.Items.Add(num97);
			arrayExtension.Items.Add(num98);
			arrayExtension.Items.Add(num99);
			arrayExtension.Items.Add(num100);
			arrayExtension.Items.Add(num101);
			arrayExtension.Items.Add(num102);
			arrayExtension.Items.Add(num103);
			arrayExtension.Items.Add(num104);
			arrayExtension.Items.Add(num105);
			arrayExtension.Items.Add(num106);
			arrayExtension.Items.Add(num107);
			arrayExtension.Items.Add(num108);
			arrayExtension.Items.Add(num109);
			arrayExtension.Items.Add(num110);
			arrayExtension.Items.Add(num111);
			arrayExtension.Items.Add(num112);
			arrayExtension.Items.Add(num113);
			arrayExtension.Items.Add(num114);
			arrayExtension.Items.Add(num115);
			arrayExtension.Items.Add(num116);
			arrayExtension.Items.Add(num117);
			arrayExtension.Items.Add(num118);
			arrayExtension.Items.Add(num119);
			arrayExtension.Items.Add(num120);
			arrayExtension.Items.Add(num121);
			arrayExtension.Items.Add(num122);
			arrayExtension.Items.Add(num123);
			arrayExtension.Items.Add(num124);
			arrayExtension.Items.Add(num125);
			arrayExtension.Items.Add(num126);
			arrayExtension.Items.Add(num127);
			arrayExtension.Items.Add(num128);
			arrayExtension.Items.Add(num129);
			double[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new double[]
			{
				num, num2, num3, num4, num5, num6, num7, num8, num9, num10,
				num11, num12, num13, num14, num15, num16, num17, num18, num19, num20,
				num21, num22, num23, num24, num25, num26, num27, num28, num29, num30,
				num31, num32, num33, num34, num35, num36, num37, num38, num39, num40,
				num41, num42, num43, num44, num45, num46, num47, num48, num49, num50,
				num51, num52, num53, num54, num55, num56, num57, num58, num59, num60,
				num61, num62, num63, num64, num65, num66, num67, num68, num69, num70,
				num71, num72, num73, num74, num75, num76, num77, num78, num79, num80,
				num81, num82, num83, num84, num85, num86, num87, num88, num89, num90,
				num91, num92, num93, num94, num95, num96, num97, num98, num99, num100,
				num101, num102, num103, num104, num105, num106, num107, num108, num109, num110,
				num111, num112, num113, num114, num115, num116, num117, num118, num119, num120,
				num121, num122, num123, num124, num125, num126, num127, num128, num129
			}, new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 30);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 18);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 21);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 431, 21);
			NumberPickerCell numberPickerCell3;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell3 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 18);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 49);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 99);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched5;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched5 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 18);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 21);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 21);
			NumberPickerCell numberPickerCell4;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell4 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 14);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 55);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 55);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 442, 21);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 445, 21);
			NumberPickerCell numberPickerCell5;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell5 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 441, 18);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 448, 21);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 451, 21);
			NumberPickerCell numberPickerCell6;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell6 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 447, 18);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 454, 21);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 457, 21);
			NumberPickerCell numberPickerCell7;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell7 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 453, 18);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 21);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 21);
			NumberPickerCell numberPickerCell8;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell8 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 459, 18);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 466, 21);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 469, 21);
			NumberPickerCell numberPickerCell9;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell9 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 18);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 472, 21);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 475, 21);
			NumberPickerCell numberPickerCell10;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell10 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 18);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 478, 21);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 481, 21);
			NumberPickerCell numberPickerCell11;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell11 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 477, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 14);
			Translate translate49;
			VisualDiagnostics.RegisterSourceInfo(translate49 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 25);
			Translate translate50;
			VisualDiagnostics.RegisterSourceInfo(translate50 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 49);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 104);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched6;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched6 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 18);
			Translate translate51;
			VisualDiagnostics.RegisterSourceInfo(translate51 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 49);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 97);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched7;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched7 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 492, 18);
			Translate translate52;
			VisualDiagnostics.RegisterSourceInfo(translate52 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 49);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 101);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched8;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched8 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 18);
			Translate translate53;
			VisualDiagnostics.RegisterSourceInfo(translate53 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 49);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 108);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched9;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched9 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsFuelRateV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("zeroConsPanel", settingsCheckBoxCellPatched2);
			if (settingsCheckBoxCellPatched2.StyleId == null)
			{
				settingsCheckBoxCellPatched2.StyleId = "zeroConsPanel";
			}
			this.settingsLayoutRoot = settingsView;
			this.zeroConsPanel = settingsCheckBoxCellPatched2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ECUInitializationPickerConverter", ecuinitializationPickerConverter);
			resourceDictionary.Add("NissanProtocolNumberToVisibilityConverter", nissanProtocolNumberToVisibilityConverter);
			resourceDictionary.Add("DTCReadingModeToIntConverter", dtcreadingModeToIntConverter);
			resourceDictionary.Add("InjectorPidIdToPIDConverter", injectorPidIdToPIDConverter);
			resourceDictionary.Add("ZeroConsumptionPIDIdtoPIDConverter", zeroConsumptionPIDIdtoPIDConverter);
			resourceDictionary.Add("CustomFuelToTrueConverter", customFuelToTrueConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("MultiBooleanToTrueConverter", multiBooleanToTrueConverter);
			resourceDictionary.Add("DecimalFuelPriceForLitreToStringConverter", decimalFuelPriceForLitreToStringConverter);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes2);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes3);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes4);
			enumToBoolConverter.TrueValues.Add(fuelFlowCalculationSchemes5);
			IMarkupExtension<IValueConverter> markupExtension = enumToBoolConverter;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = resourceDictionary;
			array2[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, null, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("EngineDisplacementToVisibleTrueConverter", valueConverter);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes6);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes7);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes8);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes9);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes10);
			enumToBoolConverter2.TrueValues.Add(fuelFlowCalculationSchemes11);
			IMarkupExtension<IValueConverter> markupExtension2 = enumToBoolConverter2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 2];
			array3[0] = resourceDictionary;
			array3[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle4, obj2 = new SimpleValueTargetProvider(array3, null, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 14)));
			IValueConverter valueConverter2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			resourceDictionary.Add("ZeroConsumptionToVisibleTrueConverter", valueConverter2);
			enumToBoolConverter3.TrueValues.Add(fuelFlowCalculationSchemes12);
			enumToBoolConverter3.TrueValues.Add(fuelFlowCalculationSchemes13);
			IMarkupExtension<IValueConverter> markupExtension3 = enumToBoolConverter3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 2];
			array4[0] = resourceDictionary;
			array4[1] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle6, obj3 = new SimpleValueTargetProvider(array4, null, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 14)));
			IValueConverter valueConverter3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			resourceDictionary.Add("FuelSchemeLOAD_ABSToTrueConverter", valueConverter3);
			enumToBoolConverter4.TrueValues.Add(fuelFlowCalculationSchemes14);
			enumToBoolConverter4.TrueValues.Add(fuelFlowCalculationSchemes15);
			IMarkupExtension<IValueConverter> markupExtension4 = enumToBoolConverter4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 2];
			array5[0] = resourceDictionary;
			array5[1] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle8, obj4 = new SimpleValueTargetProvider(array5, null, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 14)));
			IValueConverter valueConverter4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			resourceDictionary.Add("FuelSchemeMAPToTrueConverter", valueConverter4);
			enumToBoolConverter5.TrueValues.Add(fuelFlowCalculationSchemes16);
			enumToBoolConverter5.TrueValues.Add(fuelFlowCalculationSchemes17);
			IMarkupExtension<IValueConverter> markupExtension5 = enumToBoolConverter5;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 2];
			array6[0] = resourceDictionary;
			array6[1] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle10, obj5 = new SimpleValueTargetProvider(array6, null, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 14)));
			IValueConverter valueConverter5 = markupExtension5.ProvideValue(xamlServiceProvider5);
			resourceDictionary.Add("FuelSchemeMAFToTrueConverter", valueConverter5);
			enumToBoolConverter6.TrueValues.Add(fuelFlowCalculationSchemes18);
			enumToBoolConverter6.TrueValues.Add(fuelFlowCalculationSchemes19);
			IMarkupExtension<IValueConverter> markupExtension6 = enumToBoolConverter6;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 2];
			array7[0] = resourceDictionary;
			array7[1] = this;
			object obj6;
			xamlServiceProvider6.Add(typeFromHandle12, obj6 = new SimpleValueTargetProvider(array7, null, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 14)));
			IValueConverter valueConverter6 = markupExtension6.ProvideValue(xamlServiceProvider6);
			resourceDictionary.Add("FuelSchemeInjectorToTrueConverter", valueConverter6);
			enumToBoolConverter7.TrueValues.Add(fuelFlowCalculationSchemes20);
			enumToBoolConverter7.TrueValues.Add(fuelFlowCalculationSchemes21);
			IMarkupExtension<IValueConverter> markupExtension7 = enumToBoolConverter7;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 2];
			array8[0] = resourceDictionary;
			array8[1] = this;
			object obj7;
			xamlServiceProvider7.Add(typeFromHandle14, obj7 = new SimpleValueTargetProvider(array8, null, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 14)));
			IValueConverter valueConverter7 = markupExtension7.ProvideValue(xamlServiceProvider7);
			resourceDictionary.Add("FuelSchemeCycleConsumptionToTrueConverter", valueConverter7);
			enumToBoolConverter8.TrueValues.Add(fuelFlowCalculationSchemes22);
			enumToBoolConverter8.TrueValues.Add(fuelFlowCalculationSchemes23);
			IMarkupExtension<IValueConverter> markupExtension8 = enumToBoolConverter8;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 2];
			array9[0] = resourceDictionary;
			array9[1] = this;
			object obj8;
			xamlServiceProvider8.Add(typeFromHandle16, obj8 = new SimpleValueTargetProvider(array9, null, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 14)));
			IValueConverter valueConverter8 = markupExtension8.ProvideValue(xamlServiceProvider8);
			resourceDictionary.Add("FuelSchemeFuelRateToTrueConverter", valueConverter8);
			enumToBoolConverter9.TrueValues.Add(fuelTypes);
			enumToBoolConverter9.TrueValues.Add(fuelTypes2);
			enumToBoolConverter9.TrueValues.Add(fuelTypes3);
			enumToBoolConverter9.TrueValues.Add(fuelTypes4);
			enumToBoolConverter9.TrueValues.Add(fuelTypes5);
			enumToBoolConverter9.TrueValues.Add(fuelTypes6);
			enumToBoolConverter9.TrueValues.Add(fuelTypes7);
			enumToBoolConverter9.TrueValues.Add(fuelTypes8);
			IMarkupExtension<IValueConverter> markupExtension9 = enumToBoolConverter9;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 2];
			array10[0] = resourceDictionary;
			array10[1] = this;
			object obj9;
			xamlServiceProvider9.Add(typeFromHandle18, obj9 = new SimpleValueTargetProvider(array10, null, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 14)));
			IValueConverter valueConverter9 = markupExtension9.ProvideValue(xamlServiceProvider9);
			resourceDictionary.Add("CombustionEngineFuelTypeToTrueConverter", valueConverter9);
			enumToBoolConverter10.TrueValues.Add(fuelTypes9);
			enumToBoolConverter10.TrueValues.Add(fuelTypes10);
			IMarkupExtension<IValueConverter> markupExtension10 = enumToBoolConverter10;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 2];
			array11[0] = resourceDictionary;
			array11[1] = this;
			object obj10;
			xamlServiceProvider10.Add(typeFromHandle20, obj10 = new SimpleValueTargetProvider(array11, null, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 14)));
			IValueConverter valueConverter10 = markupExtension10.ProvideValue(xamlServiceProvider10);
			resourceDictionary.Add("EvNoFuelToTrueConverter", valueConverter10);
			translate.Text = "Settings_Control_tbFuelFlowItem.Text";
			IMarkupExtension markupExtension11 = translate;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 1];
			array12[0] = this;
			object obj11;
			xamlServiceProvider11.Add(typeFromHandle22, obj11 = new SimpleValueTargetProvider(array12, Page.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			object obj12 = markupExtension11.ProvideValue(xamlServiceProvider11);
			this.Title = obj12;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 1];
			array13[0] = this;
			object obj13;
			xamlServiceProvider12.Add(typeFromHandle24, obj13 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension12.ProvideValue(xamlServiceProvider12);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "ios_Image8";
			IMarkupExtension markupExtension13 = translate2;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = section;
			array14[1] = settingsView;
			array14[2] = this;
			object obj14;
			xamlServiceProvider13.Add(typeFromHandle26, obj14 = new SimpleValueTargetProvider(array14, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 25)));
			object obj15 = markupExtension13.ProvideValue(xamlServiceProvider13);
			section.Title = obj15;
			bindingExtension.Mode = 1;
			bindingExtension.Path = "AlwaysRecordFuelConsumption";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AlwaysRecordFuelConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AlwaysRecordFuelConsumption")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			translate3.Text = "ios_RecordFuelConsumptionWhenSelected";
			IMarkupExtension markupExtension14 = translate3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = radioCell;
			array15[1] = section;
			array15[2] = settingsView;
			array15[3] = this;
			object obj16;
			xamlServiceProvider14.Add(typeFromHandle28, obj16 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 31)));
			object obj17 = markupExtension14.ProvideValue(xamlServiceProvider14);
			radioCell.Title = obj17;
			staticResourceExtension.Key = "FalseValue";
			IMarkupExtension markupExtension15 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = radioCell;
			array16[1] = section;
			array16[2] = settingsView;
			array16[3] = this;
			object obj18;
			xamlServiceProvider15.Add(typeFromHandle30, obj18 = new SimpleValueTargetProvider(array16, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 100)));
			object obj19 = markupExtension15.ProvideValue(xamlServiceProvider15);
			radioCell.SetValue(RadioCell.ValueProperty, obj19);
			section.Add(radioCell);
			translate4.Text = "Settings_tbAlwaysRecordFuel.Text";
			IMarkupExtension markupExtension16 = translate4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = radioCell2;
			array17[1] = section;
			array17[2] = settingsView;
			array17[3] = this;
			object obj20;
			xamlServiceProvider16.Add(typeFromHandle32, obj20 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 31)));
			object obj21 = markupExtension16.ProvideValue(xamlServiceProvider16);
			radioCell2.Title = obj21;
			staticResourceExtension2.Key = "TrueValue";
			IMarkupExtension markupExtension17 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = radioCell2;
			array18[1] = section;
			array18[2] = settingsView;
			array18[3] = this;
			object obj22;
			xamlServiceProvider17.Add(typeFromHandle34, obj22 = new SimpleValueTargetProvider(array18, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 95)));
			object obj23 = markupExtension17.ProvideValue(xamlServiceProvider17);
			radioCell2.SetValue(RadioCell.ValueProperty, obj23);
			section.Add(radioCell2);
			translate5.Text = "Settings_tbAlwaysRecordFuelWarning.Text";
			IMarkupExtension markupExtension18 = translate5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = labelCell;
			array19[1] = section;
			array19[2] = settingsView;
			array19[3] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle36, obj24 = new SimpleValueTargetProvider(array19, CellBase.TitleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 21)));
			object obj25 = markupExtension18.ProvideValue(xamlServiceProvider18);
			labelCell.Title = obj25;
			bindingExtension2.Path = "AlwaysRecordFuelConsumption";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AlwaysRecordFuelConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AlwaysRecordFuelConsumption")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelCell.SetBinding(CellBase.IsVisibleProperty, bindingBase2);
			labelCell.SetValue(CellBase.TitleColorProperty, Color.Red);
			section.Add(labelCell);
			settingsView.Root.Add(section);
			translate6.Text = "Settings_Control_tbVehicleFuel.Text";
			IMarkupExtension markupExtension19 = translate6;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 3];
			array20[0] = section2;
			array20[1] = settingsView;
			array20[2] = this;
			object obj26;
			xamlServiceProvider19.Add(typeFromHandle38, obj26 = new SimpleValueTargetProvider(array20, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(137, 25)));
			object obj27 = markupExtension19.ProvideValue(xamlServiceProvider19);
			section2.Title = obj27;
			bindingExtension3.Path = "FuelType";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, delegate(SharedSettings A_0, FuelTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelType = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			section2.SetBinding(RadioCell.SelectedValueProperty, bindingBase3);
			translate7.Text = "Settings_Control_Gasoline.Content";
			IMarkupExtension markupExtension20 = translate7;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = radioCell3;
			array21[1] = section2;
			array21[2] = settingsView;
			array21[3] = this;
			object obj28;
			xamlServiceProvider20.Add(typeFromHandle40, obj28 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 31)));
			object obj29 = markupExtension20.ProvideValue(xamlServiceProvider20);
			radioCell3.Title = obj29;
			radioCell3.SetValue(RadioCell.ValueProperty, fuelTypes11);
			section2.Add(radioCell3);
			translate8.Text = "Settings_Control_Diesel.Content";
			IMarkupExtension markupExtension21 = translate8;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = radioCell4;
			array22[1] = section2;
			array22[2] = settingsView;
			array22[3] = this;
			object obj30;
			xamlServiceProvider21.Add(typeFromHandle42, obj30 = new SimpleValueTargetProvider(array22, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 31)));
			object obj31 = markupExtension21.ProvideValue(xamlServiceProvider21);
			radioCell4.Title = obj31;
			radioCell4.SetValue(RadioCell.ValueProperty, fuelTypes12);
			section2.Add(radioCell4);
			translate9.Text = "fuelType_EV";
			IMarkupExtension markupExtension22 = translate9;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = radioCell5;
			array23[1] = section2;
			array23[2] = settingsView;
			array23[3] = this;
			object obj32;
			xamlServiceProvider22.Add(typeFromHandle44, obj32 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 31)));
			object obj33 = markupExtension22.ProvideValue(xamlServiceProvider22);
			radioCell5.Title = obj33;
			radioCell5.SetValue(RadioCell.ValueProperty, fuelTypes13);
			section2.Add(radioCell5);
			translate10.Text = "ios_Ethanol";
			IMarkupExtension markupExtension23 = translate10;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = radioCell6;
			array24[1] = section2;
			array24[2] = settingsView;
			array24[3] = this;
			object obj34;
			xamlServiceProvider23.Add(typeFromHandle46, obj34 = new SimpleValueTargetProvider(array24, CellBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 31)));
			object obj35 = markupExtension23.ProvideValue(xamlServiceProvider23);
			radioCell6.Title = obj35;
			radioCell6.SetValue(RadioCell.ValueProperty, fuelTypes14);
			section2.Add(radioCell6);
			translate11.Text = "ios_Methanol";
			IMarkupExtension markupExtension24 = translate11;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = radioCell7;
			array25[1] = section2;
			array25[2] = settingsView;
			array25[3] = this;
			object obj36;
			xamlServiceProvider24.Add(typeFromHandle48, obj36 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 31)));
			object obj37 = markupExtension24.ProvideValue(xamlServiceProvider24);
			radioCell7.Title = obj37;
			radioCell7.SetValue(RadioCell.ValueProperty, fuelTypes15);
			section2.Add(radioCell7);
			translate12.Text = "ios_Propan";
			IMarkupExtension markupExtension25 = translate12;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = radioCell8;
			array26[1] = section2;
			array26[2] = settingsView;
			array26[3] = this;
			object obj38;
			xamlServiceProvider25.Add(typeFromHandle50, obj38 = new SimpleValueTargetProvider(array26, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 31)));
			object obj39 = markupExtension25.ProvideValue(xamlServiceProvider25);
			radioCell8.Title = obj39;
			radioCell8.SetValue(RadioCell.ValueProperty, fuelTypes16);
			section2.Add(radioCell8);
			translate13.Text = "ios_Methan";
			IMarkupExtension markupExtension26 = translate13;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = radioCell9;
			array27[1] = section2;
			array27[2] = settingsView;
			array27[3] = this;
			object obj40;
			xamlServiceProvider26.Add(typeFromHandle52, obj40 = new SimpleValueTargetProvider(array27, CellBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 31)));
			object obj41 = markupExtension26.ProvideValue(xamlServiceProvider26);
			radioCell9.Title = obj41;
			radioCell9.SetValue(RadioCell.ValueProperty, fuelTypes17);
			section2.Add(radioCell9);
			translate14.Text = "ios_FlexFuelOBDII";
			IMarkupExtension markupExtension27 = translate14;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = radioCell10;
			array28[1] = section2;
			array28[2] = settingsView;
			array28[3] = this;
			object obj42;
			xamlServiceProvider27.Add(typeFromHandle54, obj42 = new SimpleValueTargetProvider(array28, CellBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 31)));
			object obj43 = markupExtension27.ProvideValue(xamlServiceProvider27);
			radioCell10.Title = obj43;
			radioCell10.SetValue(RadioCell.ValueProperty, fuelTypes18);
			section2.Add(radioCell10);
			translate15.Text = "ios_Custom";
			IMarkupExtension markupExtension28 = translate15;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = radioCell11;
			array29[1] = section2;
			array29[2] = settingsView;
			array29[3] = this;
			object obj44;
			xamlServiceProvider28.Add(typeFromHandle56, obj44 = new SimpleValueTargetProvider(array29, CellBase.TitleProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 31)));
			object obj45 = markupExtension28.ProvideValue(xamlServiceProvider28);
			radioCell11.Title = obj45;
			radioCell11.SetValue(RadioCell.ValueProperty, fuelTypes19);
			section2.Add(radioCell11);
			settingsCheckBoxCellPatched.SetValue(CellBase.TitleProperty, "Invert Battery Power value sign (+/-)");
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "EV_Power_InvertValue";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.EV_Power_InvertValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.EV_Power_InvertValue = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "EV_Power_InvertValue")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase4);
			bindingExtension5.Mode = 2;
			staticResourceExtension3.Key = "EvNoFuelToTrueConverter";
			IMarkupExtension markupExtension29 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = bindingExtension5;
			array30[1] = settingsCheckBoxCellPatched;
			array30[2] = section2;
			array30[3] = settingsView;
			array30[4] = this;
			object obj46;
			xamlServiceProvider29.Add(typeFromHandle58, obj46 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 21)));
			object obj47 = markupExtension29.ProvideValue(xamlServiceProvider29);
			bindingExtension5.Converter = obj47;
			bindingExtension5.Path = "FuelType";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CellBase.IsVisibleProperty, bindingBase5);
			section2.Add(settingsCheckBoxCellPatched);
			settingsView.Root.Add(section2);
			translate16.Text = "ios_CustomFuelAF";
			IMarkupExtension markupExtension30 = translate16;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 3];
			array31[0] = section3;
			array31[1] = settingsView;
			array31[2] = this;
			object obj48;
			xamlServiceProvider30.Add(typeFromHandle60, obj48 = new SimpleValueTargetProvider(array31, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 25)));
			object obj49 = markupExtension30.ProvideValue(xamlServiceProvider30);
			section3.Title = obj49;
			bindingExtension6.Mode = 2;
			staticResourceExtension4.Key = "CustomFuelToTrueConverter";
			IMarkupExtension markupExtension31 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = bindingExtension6;
			array32[1] = section3;
			array32[2] = settingsView;
			array32[3] = this;
			object obj50;
			xamlServiceProvider31.Add(typeFromHandle62, obj50 = new SimpleValueTargetProvider(array32, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 73)));
			object obj51 = markupExtension31.ProvideValue(xamlServiceProvider31);
			bindingExtension6.Converter = obj51;
			bindingExtension6.Path = "FuelType";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase6);
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "CustomFuelAF";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CustomFuelAF, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomFuelAF = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CustomFuelAF")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase7);
			customCell.SetValue(CustomCell.ContentProperty, numericEntryV);
			section3.Add(customCell);
			settingsView.Root.Add(section3);
			translate17.Text = "ios_CustomFuelDensity";
			IMarkupExtension markupExtension32 = translate17;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle64 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 3];
			array33[0] = section4;
			array33[1] = settingsView;
			array33[2] = this;
			object obj52;
			xamlServiceProvider32.Add(typeFromHandle64, obj52 = new SimpleValueTargetProvider(array33, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle65 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle65, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 25)));
			object obj53 = markupExtension32.ProvideValue(xamlServiceProvider32);
			section4.Title = obj53;
			bindingExtension8.Mode = 2;
			staticResourceExtension5.Key = "CustomFuelToTrueConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle66 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = bindingExtension8;
			array34[1] = section4;
			array34[2] = settingsView;
			array34[3] = this;
			object obj54;
			xamlServiceProvider33.Add(typeFromHandle66, obj54 = new SimpleValueTargetProvider(array34, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle67 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle67, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 78)));
			object obj55 = markupExtension33.ProvideValue(xamlServiceProvider33);
			bindingExtension8.Converter = obj55;
			bindingExtension8.Path = "FuelType";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, FuelTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
				}
				return default(ValueTuple<FuelTypes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelType")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase8);
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 0.0001);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "CustomFuelDensity";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CustomFuelDensity, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomFuelDensity = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "CustomFuelDensity")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase9);
			customCell2.SetValue(CustomCell.ContentProperty, numericEntryV2);
			section4.Add(customCell2);
			settingsView.Root.Add(section4);
			translate18.Text = "Settings_tbFuelPriceL.Text";
			IMarkupExtension markupExtension34 = translate18;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle68 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 3];
			array35[0] = section5;
			array35[1] = settingsView;
			array35[2] = this;
			object obj56;
			xamlServiceProvider34.Add(typeFromHandle68, obj56 = new SimpleValueTargetProvider(array35, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle69 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle69, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 25)));
			object obj57 = markupExtension34.ProvideValue(xamlServiceProvider34);
			section5.Title = obj57;
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "UseLitersForVolume";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase10);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension11.Mode = 1;
			staticResourceExtension6.Key = "DecimalFuelPriceForLitreToStringConverter";
			IMarkupExtension markupExtension35 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle70 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 6];
			array36[0] = bindingExtension11;
			array36[1] = entry;
			array36[2] = customCell3;
			array36[3] = section5;
			array36[4] = settingsView;
			array36[5] = this;
			object obj58;
			xamlServiceProvider35.Add(typeFromHandle70, obj58 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle71 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle71, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 28)));
			object obj59 = markupExtension35.ProvideValue(xamlServiceProvider35);
			bindingExtension11.Converter = obj59;
			bindingExtension11.Path = "FuelPriceForLitre";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, decimal>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
				}
				return default(ValueTuple<decimal, bool>);
			}, delegate(SharedSettings A_0, decimal A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelPriceForLitre = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelPriceForLitre")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase11);
			customCell3.SetValue(CustomCell.ContentProperty, entry);
			section5.Add(customCell3);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "Currency";
			bindingExtension12.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Currency, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Currency = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Currency")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase12);
			customCell4.SetValue(CustomCell.ContentProperty, entry2);
			section5.Add(customCell4);
			translate19.Text = "ios_ResetStatsBtn";
			IMarkupExtension markupExtension36 = translate19;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle72 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = buttonCell;
			array37[1] = section5;
			array37[2] = settingsView;
			array37[3] = this;
			object obj60;
			xamlServiceProvider36.Add(typeFromHandle72, obj60 = new SimpleValueTargetProvider(array37, CellBase.TitleProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle73 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle73, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(178, 21)));
			object obj61 = markupExtension36.ProvideValue(xamlServiceProvider36);
			buttonCell.Title = obj61;
			buttonCell.Tapped += this.btnResetStatistics_Clicked;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension37 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle74 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = buttonCell;
			array38[1] = section5;
			array38[2] = settingsView;
			array38[3] = this;
			object obj62;
			xamlServiceProvider37.Add(typeFromHandle74, obj62 = new SimpleValueTargetProvider(array38, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle75 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle75, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 21)));
			DynamicResource dynamicResource2 = markupExtension37.ProvideValue(xamlServiceProvider37);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section5.Add(buttonCell);
			translate20.Text = "ios_MergeDriveCyclesTime";
			IMarkupExtension markupExtension38 = translate20;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle76 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 4];
			array39[0] = numberPickerCell;
			array39[1] = section5;
			array39[2] = settingsView;
			array39[3] = this;
			object obj63;
			xamlServiceProvider38.Add(typeFromHandle76, obj63 = new SimpleValueTargetProvider(array39, CellBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle77 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle77, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 21)));
			object obj64 = markupExtension38.ProvideValue(xamlServiceProvider38);
			numberPickerCell.Title = obj64;
			numberPickerCell.SetValue(NumberPickerCell.MaxProperty, 3600);
			numberPickerCell.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "MergeDriveCyclesTime";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.MergeDriveCyclesTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.MergeDriveCyclesTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "MergeDriveCyclesTime")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			numberPickerCell.SetBinding(NumberPickerCell.NumberProperty, bindingBase13);
			section5.Add(numberPickerCell);
			translate21.Text = "Settings_Control_tbFuelCalibration.Text";
			IMarkupExtension markupExtension39 = translate21;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle78 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 4];
			array40[0] = buttonCell2;
			array40[1] = section5;
			array40[2] = settingsView;
			array40[3] = this;
			object obj65;
			xamlServiceProvider39.Add(typeFromHandle78, obj65 = new SimpleValueTargetProvider(array40, CellBase.TitleProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle79 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle79, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 21)));
			object obj66 = markupExtension39.ProvideValue(xamlServiceProvider39);
			buttonCell2.Title = obj66;
			buttonCell2.Tapped += this.btnCalibration_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension40 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle80 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = buttonCell2;
			array41[1] = section5;
			array41[2] = settingsView;
			array41[3] = this;
			object obj67;
			xamlServiceProvider40.Add(typeFromHandle80, obj67 = new SimpleValueTargetProvider(array41, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle81 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle81, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 21)));
			DynamicResource dynamicResource3 = markupExtension40.ProvideValue(xamlServiceProvider40);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section5.Add(buttonCell2);
			settingsView.Root.Add(section5);
			translate22.Text = "Settings_tbFuelPriceG.Text";
			IMarkupExtension markupExtension41 = translate22;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle82 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 3];
			array42[0] = section6;
			array42[1] = settingsView;
			array42[2] = this;
			object obj68;
			xamlServiceProvider41.Add(typeFromHandle82, obj68 = new SimpleValueTargetProvider(array42, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle83 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle83, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 25)));
			object obj69 = markupExtension41.ProvideValue(xamlServiceProvider41);
			section6.Title = obj69;
			bindingExtension14.Mode = 2;
			staticResourceExtension7.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension42 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle84 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 4];
			array43[0] = bindingExtension14;
			array43[1] = section6;
			array43[2] = settingsView;
			array43[3] = this;
			object obj70;
			xamlServiceProvider42.Add(typeFromHandle84, obj70 = new SimpleValueTargetProvider(array43, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle85 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle85, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 83)));
			object obj71 = markupExtension42.ProvideValue(xamlServiceProvider42);
			bindingExtension14.Converter = obj71;
			bindingExtension14.Path = "UseLitersForVolume";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			section6.SetBinding(Section.IsVisibleProperty, bindingBase14);
			customCell5.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension15.Mode = 1;
			staticResourceExtension8.Key = "DecimalFuelPriceForLitreToStringConverter";
			IMarkupExtension markupExtension43 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle86 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 6];
			array44[0] = bindingExtension15;
			array44[1] = entry3;
			array44[2] = customCell5;
			array44[3] = section6;
			array44[4] = settingsView;
			array44[5] = this;
			object obj72;
			xamlServiceProvider43.Add(typeFromHandle86, obj72 = new SimpleValueTargetProvider(array44, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle87 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle87, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(194, 28)));
			object obj73 = markupExtension43.ProvideValue(xamlServiceProvider43);
			bindingExtension15.Converter = obj73;
			bindingExtension15.Path = "FuelPriceForLitre";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, decimal>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
				}
				return default(ValueTuple<decimal, bool>);
			}, delegate(SharedSettings A_0, decimal A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelPriceForLitre = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelPriceForLitre")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase15);
			customCell5.SetValue(CustomCell.ContentProperty, entry3);
			section6.Add(customCell5);
			customCell6.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "Currency";
			bindingExtension16.TypedBinding = new TypedBinding<SharedSettings, string>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Currency, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(SharedSettings A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Currency = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Currency")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase16);
			customCell6.SetValue(CustomCell.ContentProperty, entry4);
			section6.Add(customCell6);
			translate23.Text = "ios_ResetStatsBtn";
			IMarkupExtension markupExtension44 = translate23;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle88 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 4];
			array45[0] = buttonCell3;
			array45[1] = section6;
			array45[2] = settingsView;
			array45[3] = this;
			object obj74;
			xamlServiceProvider44.Add(typeFromHandle88, obj74 = new SimpleValueTargetProvider(array45, CellBase.TitleProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle89 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle89, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 32)));
			object obj75 = markupExtension44.ProvideValue(xamlServiceProvider44);
			buttonCell3.Title = obj75;
			buttonCell3.Tapped += this.btnResetStatistics_Clicked;
			section6.Add(buttonCell3);
			translate24.Text = "ios_MergeDriveCyclesTime";
			IMarkupExtension markupExtension45 = translate24;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle90 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 4];
			array46[0] = numberPickerCell2;
			array46[1] = section6;
			array46[2] = settingsView;
			array46[3] = this;
			object obj76;
			xamlServiceProvider45.Add(typeFromHandle90, obj76 = new SimpleValueTargetProvider(array46, CellBase.TitleProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle91 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle91, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(201, 21)));
			object obj77 = markupExtension45.ProvideValue(xamlServiceProvider45);
			numberPickerCell2.Title = obj77;
			numberPickerCell2.SetValue(NumberPickerCell.MaxProperty, 3600);
			numberPickerCell2.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "MergeDriveCyclesTime";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.MergeDriveCyclesTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.MergeDriveCyclesTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "MergeDriveCyclesTime")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			numberPickerCell2.SetBinding(NumberPickerCell.NumberProperty, bindingBase17);
			section6.Add(numberPickerCell2);
			translate25.Text = "Settings_Control_tbFuelCalibration.Text";
			IMarkupExtension markupExtension46 = translate25;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle92 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = buttonCell4;
			array47[1] = section6;
			array47[2] = settingsView;
			array47[3] = this;
			object obj78;
			xamlServiceProvider46.Add(typeFromHandle92, obj78 = new SimpleValueTargetProvider(array47, CellBase.TitleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle93 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle93, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 21)));
			object obj79 = markupExtension46.ProvideValue(xamlServiceProvider46);
			buttonCell4.Title = obj79;
			buttonCell4.Tapped += this.btnCalibration_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension47 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle94 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = buttonCell4;
			array48[1] = section6;
			array48[2] = settingsView;
			array48[3] = this;
			object obj80;
			xamlServiceProvider47.Add(typeFromHandle94, obj80 = new SimpleValueTargetProvider(array48, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle95 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle95, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(208, 21)));
			DynamicResource dynamicResource4 = markupExtension47.ProvideValue(xamlServiceProvider47);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section6.Add(buttonCell4);
			settingsView.Root.Add(section6);
			translate26.Text = "Settings_Control_tbFuelRateCalculationScheme.Text";
			IMarkupExtension markupExtension48 = translate26;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle96 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 3];
			array49[0] = section7;
			array49[1] = settingsView;
			array49[2] = this;
			object obj81;
			xamlServiceProvider48.Add(typeFromHandle96, obj81 = new SimpleValueTargetProvider(array49, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle97 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle97, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 25)));
			object obj82 = markupExtension48.ProvideValue(xamlServiceProvider48);
			section7.Title = obj82;
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "FuelFlowCalculationScheme";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, FuelFlowCalculationSchemes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
				}
				return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
			}, delegate(SharedSettings A_0, FuelFlowCalculationSchemes A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelFlowCalculationScheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelFlowCalculationScheme")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			section7.SetBinding(RadioCell.SelectedValueProperty, bindingBase18);
			translate27.Text = "Settings_Control_FuelScheme_Auto.Content";
			IMarkupExtension markupExtension49 = translate27;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle98 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 4];
			array50[0] = radioCell12;
			array50[1] = section7;
			array50[2] = settingsView;
			array50[3] = this;
			object obj83;
			xamlServiceProvider49.Add(typeFromHandle98, obj83 = new SimpleValueTargetProvider(array50, CellBase.TitleProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle99 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle99, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(212, 31)));
			object obj84 = markupExtension49.ProvideValue(xamlServiceProvider49);
			radioCell12.Title = obj84;
			radioCell12.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes24);
			section7.Add(radioCell12);
			translate28.Text = "Settings_Control_FuelScheme_MAF.Content";
			IMarkupExtension markupExtension50 = translate28;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle100 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 4];
			array51[0] = radioCell13;
			array51[1] = section7;
			array51[2] = settingsView;
			array51[3] = this;
			object obj85;
			xamlServiceProvider50.Add(typeFromHandle100, obj85 = new SimpleValueTargetProvider(array51, CellBase.TitleProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle101 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver50.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider50.Add(typeFromHandle101, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 31)));
			object obj86 = markupExtension50.ProvideValue(xamlServiceProvider50);
			radioCell13.Title = obj86;
			radioCell13.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes25);
			section7.Add(radioCell13);
			translate29.Text = "Settings_Control_FuelScheme_AbsLOAD.Content";
			IMarkupExtension markupExtension51 = translate29;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle102 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 4];
			array52[0] = radioCell14;
			array52[1] = section7;
			array52[2] = settingsView;
			array52[3] = this;
			object obj87;
			xamlServiceProvider51.Add(typeFromHandle102, obj87 = new SimpleValueTargetProvider(array52, CellBase.TitleProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle103 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver51.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider51.Add(typeFromHandle103, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 31)));
			object obj88 = markupExtension51.ProvideValue(xamlServiceProvider51);
			radioCell14.Title = obj88;
			radioCell14.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes26);
			section7.Add(radioCell14);
			translate30.Text = "Settings_Control_FuelScheme_MAP.Content";
			IMarkupExtension markupExtension52 = translate30;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle104 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 4];
			array53[0] = radioCell15;
			array53[1] = section7;
			array53[2] = settingsView;
			array53[3] = this;
			object obj89;
			xamlServiceProvider52.Add(typeFromHandle104, obj89 = new SimpleValueTargetProvider(array53, CellBase.TitleProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle105 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver52.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider52.Add(typeFromHandle105, new XamlTypeResolver(xmlNamespaceResolver52, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(215, 31)));
			object obj90 = markupExtension52.ProvideValue(xamlServiceProvider52);
			radioCell15.Title = obj90;
			radioCell15.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes27);
			section7.Add(radioCell15);
			translate31.Text = "Settings_Control_FuelScheme_FuelFlowPID.Content";
			IMarkupExtension markupExtension53 = translate31;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle106 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 4];
			array54[0] = radioCell16;
			array54[1] = section7;
			array54[2] = settingsView;
			array54[3] = this;
			object obj91;
			xamlServiceProvider53.Add(typeFromHandle106, obj91 = new SimpleValueTargetProvider(array54, CellBase.TitleProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle107 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver53.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider53.Add(typeFromHandle107, new XamlTypeResolver(xmlNamespaceResolver53, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(216, 31)));
			object obj92 = markupExtension53.ProvideValue(xamlServiceProvider53);
			radioCell16.Title = obj92;
			radioCell16.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes28);
			section7.Add(radioCell16);
			translate32.Text = "Settings_Control_FuelScheme_Injector.Content";
			IMarkupExtension markupExtension54 = translate32;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle108 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 4];
			array55[0] = radioCell17;
			array55[1] = section7;
			array55[2] = settingsView;
			array55[3] = this;
			object obj93;
			xamlServiceProvider54.Add(typeFromHandle108, obj93 = new SimpleValueTargetProvider(array55, CellBase.TitleProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj93);
			Type typeFromHandle109 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver54.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider54.Add(typeFromHandle109, new XamlTypeResolver(xmlNamespaceResolver54, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 31)));
			object obj94 = markupExtension54.ProvideValue(xamlServiceProvider54);
			radioCell17.Title = obj94;
			radioCell17.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes29);
			section7.Add(radioCell17);
			translate33.Text = "PID_Cycle_Consumption";
			IMarkupExtension markupExtension55 = translate33;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle110 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 4];
			array56[0] = radioCell18;
			array56[1] = section7;
			array56[2] = settingsView;
			array56[3] = this;
			object obj95;
			xamlServiceProvider55.Add(typeFromHandle110, obj95 = new SimpleValueTargetProvider(array56, CellBase.TitleProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj95);
			Type typeFromHandle111 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver55.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider55.Add(typeFromHandle111, new XamlTypeResolver(xmlNamespaceResolver55, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 31)));
			object obj96 = markupExtension55.ProvideValue(xamlServiceProvider55);
			radioCell18.Title = obj96;
			radioCell18.SetValue(RadioCell.ValueProperty, fuelFlowCalculationSchemes30);
			section7.Add(radioCell18);
			translate34.Text = "Settings_Control_DetectZeroConsumptionSwitch.Text";
			IMarkupExtension markupExtension56 = translate34;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle112 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 4];
			array57[0] = settingsCheckBoxCellPatched2;
			array57[1] = section7;
			array57[2] = settingsView;
			array57[3] = this;
			object obj97;
			xamlServiceProvider56.Add(typeFromHandle112, obj97 = new SimpleValueTargetProvider(array57, CellBase.TitleProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj97);
			Type typeFromHandle113 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver56.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider56.Add(typeFromHandle113, new XamlTypeResolver(xmlNamespaceResolver56, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(240, 21)));
			object obj98 = markupExtension56.ProvideValue(xamlServiceProvider56);
			settingsCheckBoxCellPatched2.Title = obj98;
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "DetectZeroFuelConsumption";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DetectZeroFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DetectZeroFuelConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DetectZeroFuelConsumption")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase19);
			bindingExtension20.Mode = 2;
			staticResourceExtension9.Key = "ZeroConsumptionToVisibleTrueConverter";
			IMarkupExtension markupExtension57 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle114 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 5];
			array58[0] = bindingExtension20;
			array58[1] = settingsCheckBoxCellPatched2;
			array58[2] = section7;
			array58[3] = settingsView;
			array58[4] = this;
			object obj99;
			xamlServiceProvider57.Add(typeFromHandle114, obj99 = new SimpleValueTargetProvider(array58, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj99);
			Type typeFromHandle115 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver57.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider57.Add(typeFromHandle115, new XamlTypeResolver(xmlNamespaceResolver57, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 21)));
			object obj100 = markupExtension57.ProvideValue(xamlServiceProvider57);
			bindingExtension20.Converter = obj100;
			bindingExtension20.Path = "FuelFlowCalculationScheme";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, FuelFlowCalculationSchemes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
				}
				return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelFlowCalculationScheme")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(Cell.IsEnabledProperty, bindingBase20);
			section7.Add(settingsCheckBoxCellPatched2);
			translate35.Text = "Settings_Control_UseCustomPIDForZeroConsumption.Text";
			IMarkupExtension markupExtension58 = translate35;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle116 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 4];
			array59[0] = settingsCheckBoxCellPatched3;
			array59[1] = section7;
			array59[2] = settingsView;
			array59[3] = this;
			object obj101;
			xamlServiceProvider58.Add(typeFromHandle116, obj101 = new SimpleValueTargetProvider(array59, CellBase.TitleProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj101);
			Type typeFromHandle117 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver58.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider58.Add(typeFromHandle117, new XamlTypeResolver(xmlNamespaceResolver58, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 21)));
			object obj102 = markupExtension58.ProvideValue(xamlServiceProvider58);
			settingsCheckBoxCellPatched3.Title = obj102;
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "UseCustomPIDForZeroConsumption";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomPIDForZeroConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseCustomPIDForZeroConsumption = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseCustomPIDForZeroConsumption")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(CheckboxCell.CheckedProperty, bindingBase21);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "DetectZeroFuelConsumption";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DetectZeroFuelConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DetectZeroFuelConsumption")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			settingsCheckBoxCellPatched3.SetBinding(Cell.IsEnabledProperty, bindingBase22);
			section7.Add(settingsCheckBoxCellPatched3);
			staticResourceExtension10.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension59 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle118 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 5];
			array60[0] = multiBinding;
			array60[1] = customCell7;
			array60[2] = section7;
			array60[3] = settingsView;
			array60[4] = this;
			object obj103;
			xamlServiceProvider59.Add(typeFromHandle118, obj103 = new SimpleValueTargetProvider(array60, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj103);
			Type typeFromHandle119 = typeof(IXamlTypeResolver);
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
			xmlNamespaceResolver59.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider59.Add(typeFromHandle119, new XamlTypeResolver(xmlNamespaceResolver59, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 39)));
			object obj104 = markupExtension59.ProvideValue(xamlServiceProvider59);
			multiBinding.Converter = obj104;
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "DetectZeroFuelConsumption";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase23);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "UseCustomPIDForZeroConsumption";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase24);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "IsVisible";
			referenceExtension.Name = "zeroConsPanel";
			IMarkupExtension markupExtension60 = referenceExtension;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle120 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 6];
			array61[0] = bindingExtension25;
			array61[1] = multiBinding;
			array61[2] = customCell7;
			array61[3] = section7;
			array61[4] = settingsView;
			array61[5] = this;
			object obj105;
			xamlServiceProvider60.Add(typeFromHandle120, obj105 = new SimpleValueTargetProvider(array61, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj105);
			Type typeFromHandle121 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver60.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver60.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver60.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider60.Add(typeFromHandle121, new XamlTypeResolver(xmlNamespaceResolver60, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(255, 33)));
			object obj106 = markupExtension60.ProvideValue(xamlServiceProvider60);
			bindingExtension25.Source = obj106;
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			multiBinding.Bindings.Add(bindingBase25);
			customCell7.SetBinding(Cell.IsEnabledProperty, multiBinding);
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "UseCustomPIDForZeroConsumption";
			bindingExtension26.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomPIDForZeroConsumption, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseCustomPIDForZeroConsumption")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsEnabledProperty, bindingBase26);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate36.Text = "Settings_Control_tbChooseZeroConsumptionPID.Text";
			IMarkupExtension markupExtension61 = translate36;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle122 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 6];
			array62[0] = label;
			array62[1] = stackLayout;
			array62[2] = customCell7;
			array62[3] = section7;
			array62[4] = settingsView;
			array62[5] = this;
			object obj107;
			xamlServiceProvider61.Add(typeFromHandle122, obj107 = new SimpleValueTargetProvider(array62, Label.TextProperty, nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj107);
			Type typeFromHandle123 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver61.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver61.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver61.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider61.Add(typeFromHandle123, new XamlTypeResolver(xmlNamespaceResolver61, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(259, 32)));
			object obj108 = markupExtension61.ProvideValue(xamlServiceProvider61);
			label.Text = obj108;
			stackLayout.Children.Add(label);
			bindingExtension27.Path = "Name";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			picker.ItemDisplayBinding = bindingBase27;
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "ZeroConsumptionPIDCollection";
			bindingExtension28.TypedBinding = new TypedBinding<SharedSettings, List<PID>>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<List<PID>, bool>(A_0.ZeroConsumptionPIDCollection, true);
				}
				return default(ValueTuple<List<PID>, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ZeroConsumptionPIDCollection")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase28);
			bindingExtension29.Mode = 1;
			staticResourceExtension11.Key = "ZeroConsumptionPIDIdtoPIDConverter";
			IMarkupExtension markupExtension62 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle124 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 7];
			array63[0] = bindingExtension29;
			array63[1] = picker;
			array63[2] = stackLayout;
			array63[3] = customCell7;
			array63[4] = section7;
			array63[5] = settingsView;
			array63[6] = this;
			object obj109;
			xamlServiceProvider62.Add(typeFromHandle124, obj109 = new SimpleValueTargetProvider(array63, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj109);
			Type typeFromHandle125 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver62.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver62.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver62.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider62.Add(typeFromHandle125, new XamlTypeResolver(xmlNamespaceResolver62, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 29)));
			object obj110 = markupExtension62.ProvideValue(xamlServiceProvider62);
			bindingExtension29.Converter = obj110;
			bindingExtension29.Path = "ZeroConsumptionPIDId";
			bindingExtension29.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ZeroConsumptionPIDId, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ZeroConsumptionPIDId = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ZeroConsumptionPIDId")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase29);
			stackLayout.Children.Add(picker);
			customCell7.SetValue(CustomCell.ContentProperty, stackLayout);
			section7.Add(customCell7);
			translate37.Text = "Settings_Control_ZeroConsumptionTreatValue.Text";
			IMarkupExtension markupExtension63 = translate37;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle126 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 4];
			array64[0] = settingsCheckBoxCellPatched4;
			array64[1] = section7;
			array64[2] = settingsView;
			array64[3] = this;
			object obj111;
			xamlServiceProvider63.Add(typeFromHandle126, obj111 = new SimpleValueTargetProvider(array64, CellBase.TitleProperty, nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj111);
			Type typeFromHandle127 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver63.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver63.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver63.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider63.Add(typeFromHandle127, new XamlTypeResolver(xmlNamespaceResolver63, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(266, 49)));
			object obj112 = markupExtension63.ProvideValue(xamlServiceProvider63);
			settingsCheckBoxCellPatched4.Title = obj112;
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "ZeroConsumptionWhenZero";
			bindingExtension30.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ZeroConsumptionWhenZero, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ZeroConsumptionWhenZero = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ZeroConsumptionWhenZero")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			settingsCheckBoxCellPatched4.SetBinding(CheckboxCell.CheckedProperty, bindingBase30);
			staticResourceExtension12.Key = "MultiBooleanToTrueConverter";
			IMarkupExtension markupExtension64 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle128 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 5];
			array65[0] = multiBinding2;
			array65[1] = settingsCheckBoxCellPatched4;
			array65[2] = section7;
			array65[3] = settingsView;
			array65[4] = this;
			object obj113;
			xamlServiceProvider64.Add(typeFromHandle128, obj113 = new SimpleValueTargetProvider(array65, typeof(MultiBinding).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj113);
			Type typeFromHandle129 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver64.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver64.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver64.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider64.Add(typeFromHandle129, new XamlTypeResolver(xmlNamespaceResolver64, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 39)));
			object obj114 = markupExtension64.ProvideValue(xamlServiceProvider64);
			multiBinding2.Converter = obj114;
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "DetectZeroFuelConsumption";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase31);
			bindingExtension32.Mode = 2;
			bindingExtension32.Path = "UseCustomPIDForZeroConsumption";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			multiBinding2.Bindings.Add(bindingBase32);
			settingsCheckBoxCellPatched4.SetBinding(Cell.IsEnabledProperty, multiBinding2);
			section7.Add(settingsCheckBoxCellPatched4);
			settingsView.Root.Add(section7);
			translate38.Text = "Settings_Control_tbEngineDisplacement.Text";
			IMarkupExtension markupExtension65 = translate38;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle130 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 3];
			array66[0] = section8;
			array66[1] = settingsView;
			array66[2] = this;
			object obj115;
			xamlServiceProvider65.Add(typeFromHandle130, obj115 = new SimpleValueTargetProvider(array66, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj115);
			Type typeFromHandle131 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver65.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver65.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver65.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider65.Add(typeFromHandle131, new XamlTypeResolver(xmlNamespaceResolver65, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 25)));
			object obj116 = markupExtension65.ProvideValue(xamlServiceProvider65);
			section8.Title = obj116;
			bindingExtension33.Mode = 2;
			staticResourceExtension13.Key = "EngineDisplacementToVisibleTrueConverter";
			IMarkupExtension markupExtension66 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle132 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 4];
			array67[0] = bindingExtension33;
			array67[1] = section8;
			array67[2] = settingsView;
			array67[3] = this;
			object obj117;
			xamlServiceProvider66.Add(typeFromHandle132, obj117 = new SimpleValueTargetProvider(array67, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj117);
			Type typeFromHandle133 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver66.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver66.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver66.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider66.Add(typeFromHandle133, new XamlTypeResolver(xmlNamespaceResolver66, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 99)));
			object obj118 = markupExtension66.ProvideValue(xamlServiceProvider66);
			bindingExtension33.Converter = obj118;
			bindingExtension33.Path = "FuelFlowCalculationScheme";
			bindingExtension33.TypedBinding = new TypedBinding<SharedSettings, FuelFlowCalculationSchemes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
				}
				return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelFlowCalculationScheme")
			});
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			section8.SetBinding(Section.IsVisibleProperty, bindingBase33);
			dynamicResourceExtension5.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension67 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle134 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 5];
			array68[0] = picker2;
			array68[1] = settingsCustomCellForPicker;
			array68[2] = section8;
			array68[3] = settingsView;
			array68[4] = this;
			object obj119;
			xamlServiceProvider67.Add(typeFromHandle134, obj119 = new SimpleValueTargetProvider(array68, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj119);
			Type typeFromHandle135 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver67.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver67.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver67.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider67.Add(typeFromHandle135, new XamlTypeResolver(xmlNamespaceResolver67, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(288, 29)));
			DynamicResource dynamicResource5 = markupExtension67.ProvideValue(xamlServiceProvider67);
			picker2.SetDynamicResource(Picker.FontSizeProperty, dynamicResource5.Key);
			bindingExtension34.Mode = 1;
			bindingExtension34.Path = "EngineDisplacement";
			bindingExtension34.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.EngineDisplacement, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.EngineDisplacement = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "EngineDisplacement")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedItemProperty, bindingBase34);
			picker2.SetValue(Picker.ItemsSourceProperty, array);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, picker2);
			section8.Add(settingsCustomCellForPicker);
			translate39.Text = "Settings_Control_tbNumberOfCylinders.Text";
			IMarkupExtension markupExtension68 = translate39;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle136 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 4];
			array69[0] = numberPickerCell3;
			array69[1] = section8;
			array69[2] = settingsView;
			array69[3] = this;
			object obj120;
			xamlServiceProvider68.Add(typeFromHandle136, obj120 = new SimpleValueTargetProvider(array69, CellBase.TitleProperty, nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj120);
			Type typeFromHandle137 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver68.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver68.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver68.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider68.Add(typeFromHandle137, new XamlTypeResolver(xmlNamespaceResolver68, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(428, 21)));
			object obj121 = markupExtension68.ProvideValue(xamlServiceProvider68);
			numberPickerCell3.Title = obj121;
			numberPickerCell3.SetValue(NumberPickerCell.MaxProperty, 16);
			numberPickerCell3.SetValue(NumberPickerCell.MinProperty, 1);
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "EngineCylinders";
			bindingExtension35.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.EngineCylinders, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.EngineCylinders = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "EngineCylinders")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			numberPickerCell3.SetBinding(NumberPickerCell.NumberProperty, bindingBase35);
			section8.Add(numberPickerCell3);
			translate40.Text = "sett_FuelHybridCar";
			IMarkupExtension markupExtension69 = translate40;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle138 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 4];
			array70[0] = settingsCheckBoxCellPatched5;
			array70[1] = section8;
			array70[2] = settingsView;
			array70[3] = this;
			object obj122;
			xamlServiceProvider69.Add(typeFromHandle138, obj122 = new SimpleValueTargetProvider(array70, CellBase.TitleProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj122);
			Type typeFromHandle139 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver69.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver69.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver69.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider69.Add(typeFromHandle139, new XamlTypeResolver(xmlNamespaceResolver69, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(432, 49)));
			object obj123 = markupExtension69.ProvideValue(xamlServiceProvider69);
			settingsCheckBoxCellPatched5.Title = obj123;
			bindingExtension36.Mode = 1;
			bindingExtension36.Path = "FuelHybridCar";
			bindingExtension36.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.FuelHybridCar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelHybridCar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelHybridCar")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			settingsCheckBoxCellPatched5.SetBinding(CheckboxCell.CheckedProperty, bindingBase36);
			section8.Add(settingsCheckBoxCellPatched5);
			translate41.Text = "Settings_Control_tbInjectorPerformance.Text";
			IMarkupExtension markupExtension70 = translate41;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle140 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 4];
			array71[0] = numberPickerCell4;
			array71[1] = section8;
			array71[2] = settingsView;
			array71[3] = this;
			object obj124;
			xamlServiceProvider70.Add(typeFromHandle140, obj124 = new SimpleValueTargetProvider(array71, CellBase.TitleProperty, nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj124);
			Type typeFromHandle141 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver70.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver70.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver70.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider70.Add(typeFromHandle141, new XamlTypeResolver(xmlNamespaceResolver70, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(434, 21)));
			object obj125 = markupExtension70.ProvideValue(xamlServiceProvider70);
			numberPickerCell4.Title = obj125;
			numberPickerCell4.SetValue(NumberPickerCell.MaxProperty, 5000);
			numberPickerCell4.SetValue(NumberPickerCell.MinProperty, 1);
			bindingExtension37.Mode = 1;
			bindingExtension37.Path = "InjectorFlow";
			bindingExtension37.TypedBinding = new TypedBinding<SharedSettings, double>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.InjectorFlow, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(SharedSettings A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.InjectorFlow = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "InjectorFlow")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			numberPickerCell4.SetBinding(NumberPickerCell.NumberProperty, bindingBase37);
			section8.Add(numberPickerCell4);
			settingsView.Root.Add(section8);
			section9.SetValue(SectionBase.TitleProperty, "Volumetric efficiency");
			bindingExtension38.Mode = 2;
			staticResourceExtension14.Key = "FuelSchemeMAPToTrueConverter";
			IMarkupExtension markupExtension71 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle142 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 4];
			array72[0] = bindingExtension38;
			array72[1] = section9;
			array72[2] = settingsView;
			array72[3] = this;
			object obj126;
			xamlServiceProvider71.Add(typeFromHandle142, obj126 = new SimpleValueTargetProvider(array72, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj126);
			Type typeFromHandle143 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver71.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver71.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver71.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider71.Add(typeFromHandle143, new XamlTypeResolver(xmlNamespaceResolver71, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(440, 55)));
			object obj127 = markupExtension71.ProvideValue(xamlServiceProvider71);
			bindingExtension38.Converter = obj127;
			bindingExtension38.Path = "FuelFlowCalculationScheme";
			bindingExtension38.TypedBinding = new TypedBinding<SharedSettings, FuelFlowCalculationSchemes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
				}
				return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelFlowCalculationScheme")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			section9.SetBinding(Section.IsVisibleProperty, bindingBase38);
			translate42.Text = "Settings_Control_VE1000.Text";
			IMarkupExtension markupExtension72 = translate42;
			XamlServiceProvider xamlServiceProvider72 = new XamlServiceProvider();
			Type typeFromHandle144 = typeof(IProvideValueTarget);
			object[] array73 = new object[0 + 4];
			array73[0] = numberPickerCell5;
			array73[1] = section9;
			array73[2] = settingsView;
			array73[3] = this;
			object obj128;
			xamlServiceProvider72.Add(typeFromHandle144, obj128 = new SimpleValueTargetProvider(array73, CellBase.TitleProperty, nameScope));
			xamlServiceProvider72.Add(typeof(IReferenceProvider), obj128);
			Type typeFromHandle145 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver72 = new XmlNamespaceResolver();
			xmlNamespaceResolver72.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver72.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver72.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver72.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver72.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver72.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver72.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver72.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver72.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver72.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider72.Add(typeFromHandle145, new XamlTypeResolver(xmlNamespaceResolver72, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider72.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(442, 21)));
			object obj129 = markupExtension72.ProvideValue(xamlServiceProvider72);
			numberPickerCell5.Title = obj129;
			numberPickerCell5.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell5.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension39.Mode = 1;
			bindingExtension39.Path = "VE1000";
			bindingExtension39.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE1000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE1000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE1000")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			numberPickerCell5.SetBinding(NumberPickerCell.NumberProperty, bindingBase39);
			numberPickerCell5.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell5);
			translate43.Text = "Settings_Control_VE2000.Text";
			IMarkupExtension markupExtension73 = translate43;
			XamlServiceProvider xamlServiceProvider73 = new XamlServiceProvider();
			Type typeFromHandle146 = typeof(IProvideValueTarget);
			object[] array74 = new object[0 + 4];
			array74[0] = numberPickerCell6;
			array74[1] = section9;
			array74[2] = settingsView;
			array74[3] = this;
			object obj130;
			xamlServiceProvider73.Add(typeFromHandle146, obj130 = new SimpleValueTargetProvider(array74, CellBase.TitleProperty, nameScope));
			xamlServiceProvider73.Add(typeof(IReferenceProvider), obj130);
			Type typeFromHandle147 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver73 = new XmlNamespaceResolver();
			xmlNamespaceResolver73.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver73.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver73.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver73.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver73.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver73.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver73.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver73.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver73.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver73.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider73.Add(typeFromHandle147, new XamlTypeResolver(xmlNamespaceResolver73, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider73.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(448, 21)));
			object obj131 = markupExtension73.ProvideValue(xamlServiceProvider73);
			numberPickerCell6.Title = obj131;
			numberPickerCell6.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell6.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "VE2000";
			bindingExtension40.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE2000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE2000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE2000")
			});
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			numberPickerCell6.SetBinding(NumberPickerCell.NumberProperty, bindingBase40);
			numberPickerCell6.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell6);
			translate44.Text = "Settings_Control_VE3000.Text";
			IMarkupExtension markupExtension74 = translate44;
			XamlServiceProvider xamlServiceProvider74 = new XamlServiceProvider();
			Type typeFromHandle148 = typeof(IProvideValueTarget);
			object[] array75 = new object[0 + 4];
			array75[0] = numberPickerCell7;
			array75[1] = section9;
			array75[2] = settingsView;
			array75[3] = this;
			object obj132;
			xamlServiceProvider74.Add(typeFromHandle148, obj132 = new SimpleValueTargetProvider(array75, CellBase.TitleProperty, nameScope));
			xamlServiceProvider74.Add(typeof(IReferenceProvider), obj132);
			Type typeFromHandle149 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver74 = new XmlNamespaceResolver();
			xmlNamespaceResolver74.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver74.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver74.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver74.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver74.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver74.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver74.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver74.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver74.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver74.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider74.Add(typeFromHandle149, new XamlTypeResolver(xmlNamespaceResolver74, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider74.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(454, 21)));
			object obj133 = markupExtension74.ProvideValue(xamlServiceProvider74);
			numberPickerCell7.Title = obj133;
			numberPickerCell7.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell7.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension41.Mode = 1;
			bindingExtension41.Path = "VE3000";
			bindingExtension41.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE3000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE3000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE3000")
			});
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			numberPickerCell7.SetBinding(NumberPickerCell.NumberProperty, bindingBase41);
			numberPickerCell7.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell7);
			translate45.Text = "Settings_Control_VE4000.Text";
			IMarkupExtension markupExtension75 = translate45;
			XamlServiceProvider xamlServiceProvider75 = new XamlServiceProvider();
			Type typeFromHandle150 = typeof(IProvideValueTarget);
			object[] array76 = new object[0 + 4];
			array76[0] = numberPickerCell8;
			array76[1] = section9;
			array76[2] = settingsView;
			array76[3] = this;
			object obj134;
			xamlServiceProvider75.Add(typeFromHandle150, obj134 = new SimpleValueTargetProvider(array76, CellBase.TitleProperty, nameScope));
			xamlServiceProvider75.Add(typeof(IReferenceProvider), obj134);
			Type typeFromHandle151 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver75 = new XmlNamespaceResolver();
			xmlNamespaceResolver75.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver75.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver75.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver75.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver75.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver75.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver75.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver75.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver75.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver75.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider75.Add(typeFromHandle151, new XamlTypeResolver(xmlNamespaceResolver75, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider75.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(460, 21)));
			object obj135 = markupExtension75.ProvideValue(xamlServiceProvider75);
			numberPickerCell8.Title = obj135;
			numberPickerCell8.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell8.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension42.Mode = 1;
			bindingExtension42.Path = "VE4000";
			bindingExtension42.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE4000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE4000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE4000")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			numberPickerCell8.SetBinding(NumberPickerCell.NumberProperty, bindingBase42);
			numberPickerCell8.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell8);
			translate46.Text = "Settings_Control_VE5000.Text";
			IMarkupExtension markupExtension76 = translate46;
			XamlServiceProvider xamlServiceProvider76 = new XamlServiceProvider();
			Type typeFromHandle152 = typeof(IProvideValueTarget);
			object[] array77 = new object[0 + 4];
			array77[0] = numberPickerCell9;
			array77[1] = section9;
			array77[2] = settingsView;
			array77[3] = this;
			object obj136;
			xamlServiceProvider76.Add(typeFromHandle152, obj136 = new SimpleValueTargetProvider(array77, CellBase.TitleProperty, nameScope));
			xamlServiceProvider76.Add(typeof(IReferenceProvider), obj136);
			Type typeFromHandle153 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver76 = new XmlNamespaceResolver();
			xmlNamespaceResolver76.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver76.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver76.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver76.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver76.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver76.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver76.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver76.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver76.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver76.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider76.Add(typeFromHandle153, new XamlTypeResolver(xmlNamespaceResolver76, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider76.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(466, 21)));
			object obj137 = markupExtension76.ProvideValue(xamlServiceProvider76);
			numberPickerCell9.Title = obj137;
			numberPickerCell9.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell9.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension43.Mode = 1;
			bindingExtension43.Path = "VE5000";
			bindingExtension43.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE5000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE5000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE5000")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			numberPickerCell9.SetBinding(NumberPickerCell.NumberProperty, bindingBase43);
			numberPickerCell9.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell9);
			translate47.Text = "Settings_Control_VE6000.Text";
			IMarkupExtension markupExtension77 = translate47;
			XamlServiceProvider xamlServiceProvider77 = new XamlServiceProvider();
			Type typeFromHandle154 = typeof(IProvideValueTarget);
			object[] array78 = new object[0 + 4];
			array78[0] = numberPickerCell10;
			array78[1] = section9;
			array78[2] = settingsView;
			array78[3] = this;
			object obj138;
			xamlServiceProvider77.Add(typeFromHandle154, obj138 = new SimpleValueTargetProvider(array78, CellBase.TitleProperty, nameScope));
			xamlServiceProvider77.Add(typeof(IReferenceProvider), obj138);
			Type typeFromHandle155 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver77 = new XmlNamespaceResolver();
			xmlNamespaceResolver77.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver77.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver77.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver77.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver77.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver77.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver77.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver77.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver77.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver77.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider77.Add(typeFromHandle155, new XamlTypeResolver(xmlNamespaceResolver77, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider77.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(472, 21)));
			object obj139 = markupExtension77.ProvideValue(xamlServiceProvider77);
			numberPickerCell10.Title = obj139;
			numberPickerCell10.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell10.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension44.Mode = 1;
			bindingExtension44.Path = "VE6000";
			bindingExtension44.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE6000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE6000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE6000")
			});
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			numberPickerCell10.SetBinding(NumberPickerCell.NumberProperty, bindingBase44);
			numberPickerCell10.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell10);
			translate48.Text = "Settings_Control_VE7000.Text";
			IMarkupExtension markupExtension78 = translate48;
			XamlServiceProvider xamlServiceProvider78 = new XamlServiceProvider();
			Type typeFromHandle156 = typeof(IProvideValueTarget);
			object[] array79 = new object[0 + 4];
			array79[0] = numberPickerCell11;
			array79[1] = section9;
			array79[2] = settingsView;
			array79[3] = this;
			object obj140;
			xamlServiceProvider78.Add(typeFromHandle156, obj140 = new SimpleValueTargetProvider(array79, CellBase.TitleProperty, nameScope));
			xamlServiceProvider78.Add(typeof(IReferenceProvider), obj140);
			Type typeFromHandle157 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver78 = new XmlNamespaceResolver();
			xmlNamespaceResolver78.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver78.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver78.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver78.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver78.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver78.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver78.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver78.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver78.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver78.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider78.Add(typeFromHandle157, new XamlTypeResolver(xmlNamespaceResolver78, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider78.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(478, 21)));
			object obj141 = markupExtension78.ProvideValue(xamlServiceProvider78);
			numberPickerCell11.Title = obj141;
			numberPickerCell11.SetValue(NumberPickerCell.MaxProperty, 150);
			numberPickerCell11.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension45.Mode = 1;
			bindingExtension45.Path = "VE7000";
			bindingExtension45.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.VE7000, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.VE7000 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "VE7000")
			});
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			numberPickerCell11.SetBinding(NumberPickerCell.NumberProperty, bindingBase45);
			numberPickerCell11.SetValue(NumberPickerCell.UnitProperty, "%");
			section9.Add(numberPickerCell11);
			settingsView.Root.Add(section9);
			translate49.Text = "settings_CompatibilityPatches";
			IMarkupExtension markupExtension79 = translate49;
			XamlServiceProvider xamlServiceProvider79 = new XamlServiceProvider();
			Type typeFromHandle158 = typeof(IProvideValueTarget);
			object[] array80 = new object[0 + 3];
			array80[0] = section10;
			array80[1] = settingsView;
			array80[2] = this;
			object obj142;
			xamlServiceProvider79.Add(typeFromHandle158, obj142 = new SimpleValueTargetProvider(array80, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider79.Add(typeof(IReferenceProvider), obj142);
			Type typeFromHandle159 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver79 = new XmlNamespaceResolver();
			xmlNamespaceResolver79.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver79.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver79.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver79.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver79.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver79.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver79.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver79.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver79.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver79.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider79.Add(typeFromHandle159, new XamlTypeResolver(xmlNamespaceResolver79, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider79.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(490, 25)));
			object obj143 = markupExtension79.ProvideValue(xamlServiceProvider79);
			section10.Title = obj143;
			translate50.Text = "ios_FilterWrongAFValues";
			IMarkupExtension markupExtension80 = translate50;
			XamlServiceProvider xamlServiceProvider80 = new XamlServiceProvider();
			Type typeFromHandle160 = typeof(IProvideValueTarget);
			object[] array81 = new object[0 + 4];
			array81[0] = settingsCheckBoxCellPatched6;
			array81[1] = section10;
			array81[2] = settingsView;
			array81[3] = this;
			object obj144;
			xamlServiceProvider80.Add(typeFromHandle160, obj144 = new SimpleValueTargetProvider(array81, CellBase.TitleProperty, nameScope));
			xamlServiceProvider80.Add(typeof(IReferenceProvider), obj144);
			Type typeFromHandle161 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver80 = new XmlNamespaceResolver();
			xmlNamespaceResolver80.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver80.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver80.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver80.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver80.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver80.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver80.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver80.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver80.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver80.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider80.Add(typeFromHandle161, new XamlTypeResolver(xmlNamespaceResolver80, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider80.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(491, 49)));
			object obj145 = markupExtension80.ProvideValue(xamlServiceProvider80);
			settingsCheckBoxCellPatched6.Title = obj145;
			bindingExtension46.Mode = 1;
			bindingExtension46.Path = "FilterWrongAFValues";
			bindingExtension46.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.FilterWrongAFValues, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.FilterWrongAFValues = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FilterWrongAFValues")
			});
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			settingsCheckBoxCellPatched6.SetBinding(CheckboxCell.CheckedProperty, bindingBase46);
			section10.Add(settingsCheckBoxCellPatched6);
			translate51.Text = "ios_FilterRPM300";
			IMarkupExtension markupExtension81 = translate51;
			XamlServiceProvider xamlServiceProvider81 = new XamlServiceProvider();
			Type typeFromHandle162 = typeof(IProvideValueTarget);
			object[] array82 = new object[0 + 4];
			array82[0] = settingsCheckBoxCellPatched7;
			array82[1] = section10;
			array82[2] = settingsView;
			array82[3] = this;
			object obj146;
			xamlServiceProvider81.Add(typeFromHandle162, obj146 = new SimpleValueTargetProvider(array82, CellBase.TitleProperty, nameScope));
			xamlServiceProvider81.Add(typeof(IReferenceProvider), obj146);
			Type typeFromHandle163 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver81 = new XmlNamespaceResolver();
			xmlNamespaceResolver81.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver81.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver81.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver81.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver81.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver81.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver81.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver81.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver81.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver81.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider81.Add(typeFromHandle163, new XamlTypeResolver(xmlNamespaceResolver81, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider81.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(492, 49)));
			object obj147 = markupExtension81.ProvideValue(xamlServiceProvider81);
			settingsCheckBoxCellPatched7.Title = obj147;
			bindingExtension47.Mode = 1;
			bindingExtension47.Path = "FilterRPM300";
			bindingExtension47.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.FilterRPM300, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.FilterRPM300 = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FilterRPM300")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			settingsCheckBoxCellPatched7.SetBinding(CheckboxCell.CheckedProperty, bindingBase47);
			section10.Add(settingsCheckBoxCellPatched7);
			translate52.Text = "settings_UseFixedAFR";
			IMarkupExtension markupExtension82 = translate52;
			XamlServiceProvider xamlServiceProvider82 = new XamlServiceProvider();
			Type typeFromHandle164 = typeof(IProvideValueTarget);
			object[] array83 = new object[0 + 4];
			array83[0] = settingsCheckBoxCellPatched8;
			array83[1] = section10;
			array83[2] = settingsView;
			array83[3] = this;
			object obj148;
			xamlServiceProvider82.Add(typeFromHandle164, obj148 = new SimpleValueTargetProvider(array83, CellBase.TitleProperty, nameScope));
			xamlServiceProvider82.Add(typeof(IReferenceProvider), obj148);
			Type typeFromHandle165 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver82 = new XmlNamespaceResolver();
			xmlNamespaceResolver82.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver82.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver82.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver82.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver82.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver82.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver82.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver82.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver82.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver82.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider82.Add(typeFromHandle165, new XamlTypeResolver(xmlNamespaceResolver82, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider82.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(493, 49)));
			object obj149 = markupExtension82.ProvideValue(xamlServiceProvider82);
			settingsCheckBoxCellPatched8.Title = obj149;
			bindingExtension48.Mode = 1;
			bindingExtension48.Path = "FuelFlowUseFixedAFR";
			bindingExtension48.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.FuelFlowUseFixedAFR, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelFlowUseFixedAFR = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelFlowUseFixedAFR")
			});
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			settingsCheckBoxCellPatched8.SetBinding(CheckboxCell.CheckedProperty, bindingBase48);
			section10.Add(settingsCheckBoxCellPatched8);
			translate53.Text = "settings_DefaultSearchOrder";
			IMarkupExtension markupExtension83 = translate53;
			XamlServiceProvider xamlServiceProvider83 = new XamlServiceProvider();
			Type typeFromHandle166 = typeof(IProvideValueTarget);
			object[] array84 = new object[0 + 4];
			array84[0] = settingsCheckBoxCellPatched9;
			array84[1] = section10;
			array84[2] = settingsView;
			array84[3] = this;
			object obj150;
			xamlServiceProvider83.Add(typeFromHandle166, obj150 = new SimpleValueTargetProvider(array84, CellBase.TitleProperty, nameScope));
			xamlServiceProvider83.Add(typeof(IReferenceProvider), obj150);
			Type typeFromHandle167 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver83 = new XmlNamespaceResolver();
			xmlNamespaceResolver83.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver83.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver83.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver83.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver83.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver83.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver83.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver83.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver83.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver83.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider83.Add(typeFromHandle167, new XamlTypeResolver(xmlNamespaceResolver83, typeof(SettingsFuelRateV3).GetTypeInfo().Assembly));
			xamlServiceProvider83.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(494, 49)));
			object obj151 = markupExtension83.ProvideValue(xamlServiceProvider83);
			settingsCheckBoxCellPatched9.Title = obj151;
			bindingExtension49.Mode = 1;
			bindingExtension49.Path = "SensorsSearchOrderDefault";
			bindingExtension49.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SensorsSearchOrderDefault, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SensorsSearchOrderDefault = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SensorsSearchOrderDefault")
			});
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			settingsCheckBoxCellPatched9.SetBinding(CheckboxCell.CheckedProperty, bindingBase49);
			section10.Add(settingsCheckBoxCellPatched9);
			settingsView.Root.Add(section10);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x00180E34 File Offset: 0x0017F034
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsFuelRateV3>(this, typeof(SettingsFuelRateV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.zeroConsPanel = NameScopeExtensions.FindByName<SettingsCheckBoxCellPatched>(this, "zeroConsPanel");
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00180E6C File Offset: 0x0017F06C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1904(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00180E9C File Offset: 0x0017F09C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1905(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AlwaysRecordFuelConsumption = A_1;
				return;
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00180EB8 File Offset: 0x0017F0B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1906(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00180EC8 File Offset: 0x0017F0C8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1907(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AlwaysRecordFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00180EF8 File Offset: 0x0017F0F8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1908(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AlwaysRecordFuelConsumption = A_1;
				return;
			}
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00180F14 File Offset: 0x0017F114
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1909(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00180F24 File Offset: 0x0017F124
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1910(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00180F54 File Offset: 0x0017F154
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1911(SharedSettings A_0, FuelTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelType = A_1;
				return;
			}
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x00180F70 File Offset: 0x0017F170
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1912(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00180F80 File Offset: 0x0017F180
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1913(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.EV_Power_InvertValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x00180FB0 File Offset: 0x0017F1B0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1914(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.EV_Power_InvertValue = A_1;
				return;
			}
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00180FCC File Offset: 0x0017F1CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1915(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x00180FDC File Offset: 0x0017F1DC
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1916(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x0600208A RID: 8330 RVA: 0x0018100C File Offset: 0x0017F20C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1917(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600208B RID: 8331 RVA: 0x0018101C File Offset: 0x0017F21C
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1918(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x0600208C RID: 8332 RVA: 0x0018104C File Offset: 0x0017F24C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1919(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0018105C File Offset: 0x0017F25C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1920(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CustomFuelAF, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0018108C File Offset: 0x0017F28C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1921(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomFuelAF = A_1;
				return;
			}
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x001810A8 File Offset: 0x0017F2A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1922(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002090 RID: 8336 RVA: 0x001810B8 File Offset: 0x0017F2B8
		[CompilerGenerated]
		private static ValueTuple<FuelTypes, bool> <InitializeComponent>typedBindingsM__1923(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelTypes, bool>(A_0.FuelType, true);
			}
			return default(ValueTuple<FuelTypes, bool>);
		}

		// Token: 0x06002091 RID: 8337 RVA: 0x001810E8 File Offset: 0x0017F2E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1924(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002092 RID: 8338 RVA: 0x001810F8 File Offset: 0x0017F2F8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1925(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CustomFuelDensity, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06002093 RID: 8339 RVA: 0x00181128 File Offset: 0x0017F328
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1926(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomFuelDensity = A_1;
				return;
			}
		}

		// Token: 0x06002094 RID: 8340 RVA: 0x00181144 File Offset: 0x0017F344
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1927(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002095 RID: 8341 RVA: 0x00181154 File Offset: 0x0017F354
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1928(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06002096 RID: 8342 RVA: 0x00181184 File Offset: 0x0017F384
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1929(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06002097 RID: 8343 RVA: 0x00181194 File Offset: 0x0017F394
		[CompilerGenerated]
		private static ValueTuple<decimal, bool> <InitializeComponent>typedBindingsM__1930(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
			}
			return default(ValueTuple<decimal, bool>);
		}

		// Token: 0x06002098 RID: 8344 RVA: 0x001811C4 File Offset: 0x0017F3C4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1931(SharedSettings A_0, decimal A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelPriceForLitre = A_1;
				return;
			}
		}

		// Token: 0x06002099 RID: 8345 RVA: 0x001811E0 File Offset: 0x0017F3E0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1932(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600209A RID: 8346 RVA: 0x001811F0 File Offset: 0x0017F3F0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1933(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Currency, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600209B RID: 8347 RVA: 0x00181220 File Offset: 0x0017F420
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1934(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Currency = A_1;
				return;
			}
		}

		// Token: 0x0600209C RID: 8348 RVA: 0x0018123C File Offset: 0x0017F43C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1935(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600209D RID: 8349 RVA: 0x0018124C File Offset: 0x0017F44C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1936(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.MergeDriveCyclesTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600209E RID: 8350 RVA: 0x0018127C File Offset: 0x0017F47C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1937(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.MergeDriveCyclesTime = A_1;
				return;
			}
		}

		// Token: 0x0600209F RID: 8351 RVA: 0x00181298 File Offset: 0x0017F498
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1938(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020A0 RID: 8352 RVA: 0x001812A8 File Offset: 0x0017F4A8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1939(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020A1 RID: 8353 RVA: 0x001812D8 File Offset: 0x0017F4D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1940(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020A2 RID: 8354 RVA: 0x001812E8 File Offset: 0x0017F4E8
		[CompilerGenerated]
		private static ValueTuple<decimal, bool> <InitializeComponent>typedBindingsM__1941(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<decimal, bool>(A_0.FuelPriceForLitre, true);
			}
			return default(ValueTuple<decimal, bool>);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x00181318 File Offset: 0x0017F518
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1942(SharedSettings A_0, decimal A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelPriceForLitre = A_1;
				return;
			}
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x00181334 File Offset: 0x0017F534
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1943(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020A5 RID: 8357 RVA: 0x00181344 File Offset: 0x0017F544
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1944(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Currency, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060020A6 RID: 8358 RVA: 0x00181374 File Offset: 0x0017F574
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1945(SharedSettings A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Currency = A_1;
				return;
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x00181390 File Offset: 0x0017F590
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1946(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x001813A0 File Offset: 0x0017F5A0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1947(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.MergeDriveCyclesTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x001813D0 File Offset: 0x0017F5D0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1948(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.MergeDriveCyclesTime = A_1;
				return;
			}
		}

		// Token: 0x060020AA RID: 8362 RVA: 0x001813EC File Offset: 0x0017F5EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1949(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x001813FC File Offset: 0x0017F5FC
		[CompilerGenerated]
		private static ValueTuple<FuelFlowCalculationSchemes, bool> <InitializeComponent>typedBindingsM__1950(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
			}
			return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x0018142C File Offset: 0x0017F62C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1951(SharedSettings A_0, FuelFlowCalculationSchemes A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelFlowCalculationScheme = A_1;
				return;
			}
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00181448 File Offset: 0x0017F648
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1952(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x00181458 File Offset: 0x0017F658
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1953(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DetectZeroFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x00181488 File Offset: 0x0017F688
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1954(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DetectZeroFuelConsumption = A_1;
				return;
			}
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x001814A4 File Offset: 0x0017F6A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1955(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x001814B4 File Offset: 0x0017F6B4
		[CompilerGenerated]
		private static ValueTuple<FuelFlowCalculationSchemes, bool> <InitializeComponent>typedBindingsM__1956(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
			}
			return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x001814E4 File Offset: 0x0017F6E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1957(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x001814F4 File Offset: 0x0017F6F4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1958(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomPIDForZeroConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x00181524 File Offset: 0x0017F724
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1959(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseCustomPIDForZeroConsumption = A_1;
				return;
			}
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x00181540 File Offset: 0x0017F740
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1960(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x00181550 File Offset: 0x0017F750
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1961(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DetectZeroFuelConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x00181580 File Offset: 0x0017F780
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1962(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x00181590 File Offset: 0x0017F790
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1963(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomPIDForZeroConsumption, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020B9 RID: 8377 RVA: 0x001815C0 File Offset: 0x0017F7C0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1964(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020BA RID: 8378 RVA: 0x001815D0 File Offset: 0x0017F7D0
		[CompilerGenerated]
		private static ValueTuple<List<PID>, bool> <InitializeComponent>typedBindingsM__1965(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<List<PID>, bool>(A_0.ZeroConsumptionPIDCollection, true);
			}
			return default(ValueTuple<List<PID>, bool>);
		}

		// Token: 0x060020BB RID: 8379 RVA: 0x00181600 File Offset: 0x0017F800
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1966(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020BC RID: 8380 RVA: 0x00181610 File Offset: 0x0017F810
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1967(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ZeroConsumptionPIDId, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020BD RID: 8381 RVA: 0x00181640 File Offset: 0x0017F840
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1968(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ZeroConsumptionPIDId = A_1;
				return;
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x0018165C File Offset: 0x0017F85C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1969(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x0018166C File Offset: 0x0017F86C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1970(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ZeroConsumptionWhenZero, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x0018169C File Offset: 0x0017F89C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1971(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ZeroConsumptionWhenZero = A_1;
				return;
			}
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x001816B8 File Offset: 0x0017F8B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1972(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020C2 RID: 8386 RVA: 0x001816C8 File Offset: 0x0017F8C8
		[CompilerGenerated]
		private static ValueTuple<FuelFlowCalculationSchemes, bool> <InitializeComponent>typedBindingsM__1973(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
			}
			return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
		}

		// Token: 0x060020C3 RID: 8387 RVA: 0x001816F8 File Offset: 0x0017F8F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1974(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020C4 RID: 8388 RVA: 0x00181708 File Offset: 0x0017F908
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1975(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.EngineDisplacement, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060020C5 RID: 8389 RVA: 0x00181738 File Offset: 0x0017F938
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1976(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.EngineDisplacement = A_1;
				return;
			}
		}

		// Token: 0x060020C6 RID: 8390 RVA: 0x00181754 File Offset: 0x0017F954
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1977(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020C7 RID: 8391 RVA: 0x00181764 File Offset: 0x0017F964
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1978(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.EngineCylinders, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020C8 RID: 8392 RVA: 0x00181794 File Offset: 0x0017F994
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1979(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.EngineCylinders = A_1;
				return;
			}
		}

		// Token: 0x060020C9 RID: 8393 RVA: 0x001817B0 File Offset: 0x0017F9B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1980(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020CA RID: 8394 RVA: 0x001817C0 File Offset: 0x0017F9C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1981(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.FuelHybridCar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020CB RID: 8395 RVA: 0x001817F0 File Offset: 0x0017F9F0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1982(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelHybridCar = A_1;
				return;
			}
		}

		// Token: 0x060020CC RID: 8396 RVA: 0x0018180C File Offset: 0x0017FA0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1983(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020CD RID: 8397 RVA: 0x0018181C File Offset: 0x0017FA1C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1984(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.InjectorFlow, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060020CE RID: 8398 RVA: 0x0018184C File Offset: 0x0017FA4C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1985(SharedSettings A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.InjectorFlow = A_1;
				return;
			}
		}

		// Token: 0x060020CF RID: 8399 RVA: 0x00181868 File Offset: 0x0017FA68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1986(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020D0 RID: 8400 RVA: 0x00181878 File Offset: 0x0017FA78
		[CompilerGenerated]
		private static ValueTuple<FuelFlowCalculationSchemes, bool> <InitializeComponent>typedBindingsM__1987(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelFlowCalculationSchemes, bool>(A_0.FuelFlowCalculationScheme, true);
			}
			return default(ValueTuple<FuelFlowCalculationSchemes, bool>);
		}

		// Token: 0x060020D1 RID: 8401 RVA: 0x001818A8 File Offset: 0x0017FAA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1988(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020D2 RID: 8402 RVA: 0x001818B8 File Offset: 0x0017FAB8
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1989(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE1000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020D3 RID: 8403 RVA: 0x001818E8 File Offset: 0x0017FAE8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1990(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE1000 = A_1;
				return;
			}
		}

		// Token: 0x060020D4 RID: 8404 RVA: 0x00181904 File Offset: 0x0017FB04
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1991(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020D5 RID: 8405 RVA: 0x00181914 File Offset: 0x0017FB14
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1992(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE2000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020D6 RID: 8406 RVA: 0x00181944 File Offset: 0x0017FB44
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1993(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE2000 = A_1;
				return;
			}
		}

		// Token: 0x060020D7 RID: 8407 RVA: 0x00181960 File Offset: 0x0017FB60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1994(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020D8 RID: 8408 RVA: 0x00181970 File Offset: 0x0017FB70
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1995(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE3000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020D9 RID: 8409 RVA: 0x001819A0 File Offset: 0x0017FBA0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1996(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE3000 = A_1;
				return;
			}
		}

		// Token: 0x060020DA RID: 8410 RVA: 0x001819BC File Offset: 0x0017FBBC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1997(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x001819CC File Offset: 0x0017FBCC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1998(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE4000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x001819FC File Offset: 0x0017FBFC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1999(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE4000 = A_1;
				return;
			}
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00181A18 File Offset: 0x0017FC18
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2000(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020DE RID: 8414 RVA: 0x00181A28 File Offset: 0x0017FC28
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2001(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE5000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00181A58 File Offset: 0x0017FC58
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2002(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE5000 = A_1;
				return;
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x00181A74 File Offset: 0x0017FC74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2003(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x00181A84 File Offset: 0x0017FC84
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2004(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE6000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x00181AB4 File Offset: 0x0017FCB4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2005(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE6000 = A_1;
				return;
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x00181AD0 File Offset: 0x0017FCD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2006(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x00181AE0 File Offset: 0x0017FCE0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2007(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.VE7000, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x00181B10 File Offset: 0x0017FD10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2008(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.VE7000 = A_1;
				return;
			}
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x00181B2C File Offset: 0x0017FD2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2009(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x00181B3C File Offset: 0x0017FD3C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2010(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.FilterWrongAFValues, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x00181B6C File Offset: 0x0017FD6C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2011(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.FilterWrongAFValues = A_1;
				return;
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x00181B88 File Offset: 0x0017FD88
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2012(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x00181B98 File Offset: 0x0017FD98
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2013(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.FilterRPM300, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x00181BC8 File Offset: 0x0017FDC8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2014(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.FilterRPM300 = A_1;
				return;
			}
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x00181BE4 File Offset: 0x0017FDE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2015(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x00181BF4 File Offset: 0x0017FDF4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2016(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.FuelFlowUseFixedAFR, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x00181C24 File Offset: 0x0017FE24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2017(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelFlowUseFixedAFR = A_1;
				return;
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x00181C40 File Offset: 0x0017FE40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2018(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x00181C50 File Offset: 0x0017FE50
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2019(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SensorsSearchOrderDefault, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x00181C80 File Offset: 0x0017FE80
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2020(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SensorsSearchOrderDefault = A_1;
				return;
			}
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x00181C9C File Offset: 0x0017FE9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2021(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000FB0 RID: 4016
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04000FB1 RID: 4017
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsCheckBoxCellPatched zeroConsPanel;

		// Token: 0x0200029F RID: 671
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetStatistics_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x060020F3 RID: 8435 RVA: 0x00181CAC File Offset: 0x0017FEAC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsFuelRateV3 settingsFuelRateV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsFuelRateV.DisplayAlert(Translate.GetString("ios_ResetStatsTitle"), Translate.GetString("ios_ResetStatsText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsFuelRateV3.<btnResetStatistics_Clicked>d__4>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						DriveCycleViewModel.Current.Reset();
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

			// Token: 0x060020F4 RID: 8436 RVA: 0x00181D90 File Offset: 0x0017FF90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000FB2 RID: 4018
			public int <>1__state;

			// Token: 0x04000FB3 RID: 4019
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000FB4 RID: 4020
			public SettingsFuelRateV3 <>4__this;

			// Token: 0x04000FB5 RID: 4021
			private TaskAwaiter<bool> <>u__1;
		}
	}
}
