using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002B1 RID: 689
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\SettingsUnitsV3.xaml")]
	public class SettingsUnitsV3 : ContentPage
	{
		// Token: 0x06002199 RID: 8601 RVA: 0x001950E6 File Offset: 0x001932E6
		public SettingsUnitsV3()
		{
			this.InitializeComponent();
			base.BindingContext = SharedSettings.Current;
			base.Appearing += this.SettingsPage_Appearing;
			base.Disappearing += this.SettingsPage_Disappearing;
		}

		// Token: 0x0600219A RID: 8602 RVA: 0x00195124 File Offset: 0x00193324
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

		// Token: 0x0600219B RID: 8603 RVA: 0x0016D7E4 File Offset: 0x0016B9E4
		private void SettingsPage_Appearing(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				base.BindingContext = SharedSettings.Current;
			}
		}

		// Token: 0x0600219C RID: 8604 RVA: 0x00195150 File Offset: 0x00193350
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsUnitsV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/SettingsUnitsV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			IntToStringInItemsConverter intToStringInItemsConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringInItemsConverter = new IntToStringInItemsConverter(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 111);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 31);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 112);
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 31);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 113);
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 73);
			FuelConsumptionUnits fuelConsumptionUnits = FuelConsumptionUnits.LitersPer100km;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			FuelConsumptionUnits fuelConsumptionUnits2 = FuelConsumptionUnits.KmPerLiter;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			FuelConsumptionUnits fuelConsumptionUnits3 = FuelConsumptionUnits.MilesPerGallon;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 72);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 31);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 73);
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 31);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 74);
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 31);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 104);
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 31);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 105);
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 100);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 31);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 109);
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 31);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 110);
			RadioCell radioCell11;
			VisualDiagnostics.RegisterSourceInfo(radioCell11 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 96);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 31);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 105);
			RadioCell radioCell12;
			VisualDiagnostics.RegisterSourceInfo(radioCell12 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 31);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 106);
			RadioCell radioCell13;
			VisualDiagnostics.RegisterSourceInfo(radioCell13 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 103);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 31);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 112);
			RadioCell radioCell14;
			VisualDiagnostics.RegisterSourceInfo(radioCell14 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 18);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 31);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 113);
			RadioCell radioCell15;
			VisualDiagnostics.RegisterSourceInfo(radioCell15 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 14);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 25);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 104);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 41);
			RadioCell radioCell16;
			VisualDiagnostics.RegisterSourceInfo(radioCell16 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 18);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 47);
			RadioCell radioCell17;
			VisualDiagnostics.RegisterSourceInfo(radioCell17 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 14);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 97);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 42);
			RadioCell radioCell18;
			VisualDiagnostics.RegisterSourceInfo(radioCell18 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 18);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 42);
			RadioCell radioCell19;
			VisualDiagnostics.RegisterSourceInfo(radioCell19 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 14);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 98);
			StaticResourceExtension staticResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 42);
			RadioCell radioCell20;
			VisualDiagnostics.RegisterSourceInfo(radioCell20 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			StaticResourceExtension staticResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 46);
			RadioCell radioCell21;
			VisualDiagnostics.RegisterSourceInfo(radioCell21 = new RadioCell(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\SettingsUnitsV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			this.settingsLayoutRoot = settingsView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("IntToStringInItemsConverter", intToStringInItemsConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "settings_Units";
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
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "Settings_Control_tbSpeedDistanceTemperatureVolume.Text";
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
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			bindingExtension.Mode = 1;
			bindingExtension.Path = "Use_km";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_km, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_km = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_km")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			translate3.Text = "Settings_Control_ToggleSpeedAndDistance.OnContent";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = radioCell;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, CellBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 31)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			radioCell.Title = obj7;
			staticResourceExtension.Key = "TrueValue";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = radioCell;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, RadioCell.ValueProperty, nameScope));
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
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 112)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			radioCell.SetValue(RadioCell.ValueProperty, obj9);
			section.Add(radioCell);
			translate4.Text = "Settings_Control_ToggleSpeedAndDistance.OffContent";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = radioCell2;
			array6[1] = section;
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
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 31)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			radioCell2.Title = obj11;
			staticResourceExtension2.Key = "FalseValue";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = radioCell2;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, RadioCell.ValueProperty, nameScope));
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
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 113)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			radioCell2.SetValue(RadioCell.ValueProperty, obj13);
			section.Add(radioCell2);
			settingsView.Root.Add(section);
			translate5.Text = "ios_UnitsForFuel";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = section2;
			array8[1] = settingsView;
			array8[2] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, SectionBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 25)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			section2.Title = obj15;
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "FuelConsumptionUnit";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, FuelConsumptionUnits>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
				}
				return default(ValueTuple<FuelConsumptionUnits, bool>);
			}, delegate(SharedSettings A_0, FuelConsumptionUnits A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelConsumptionUnit = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelConsumptionUnit")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section2.SetBinding(RadioCell.SelectedValueProperty, bindingBase2);
			radioCell3.SetValue(CellBase.TitleProperty, "l/100km");
			radioCell3.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits);
			section2.Add(radioCell3);
			radioCell4.SetValue(CellBase.TitleProperty, "km/L");
			radioCell4.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits2);
			section2.Add(radioCell4);
			radioCell5.SetValue(CellBase.TitleProperty, "MPG");
			radioCell5.SetValue(RadioCell.ValueProperty, fuelConsumptionUnits3);
			section2.Add(radioCell5);
			settingsView.Root.Add(section2);
			translate6.Text = "ios_VolumeUnits";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = section3;
			array9[1] = settingsView;
			array9[2] = this;
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
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 25)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			section3.Title = obj17;
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "UseLitersForVolume";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseLitersForVolume = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			section3.SetBinding(RadioCell.SelectedValueProperty, bindingBase3);
			translate7.Text = "ios_Liters";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = radioCell6;
			array10[1] = section3;
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
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 31)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			radioCell6.Title = obj19;
			staticResourceExtension3.Key = "TrueValue";
			IMarkupExtension markupExtension11 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = radioCell6;
			array11[1] = section3;
			array11[2] = settingsView;
			array11[3] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array11, RadioCell.ValueProperty, nameScope));
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
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 73)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			radioCell6.SetValue(RadioCell.ValueProperty, obj21);
			section3.Add(radioCell6);
			translate8.Text = "ios_Gallons";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = radioCell7;
			array12[1] = section3;
			array12[2] = settingsView;
			array12[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 31)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			radioCell7.Title = obj23;
			staticResourceExtension4.Key = "FalseValue";
			IMarkupExtension markupExtension13 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = radioCell7;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle25, obj24 = new SimpleValueTargetProvider(array13, RadioCell.ValueProperty, nameScope));
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
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 74)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			radioCell7.SetValue(RadioCell.ValueProperty, obj25);
			section3.Add(radioCell7);
			settingsView.Root.Add(section3);
			translate9.Text = "Settings_Control_ToggleUSGallon.Header";
			IMarkupExtension markupExtension14 = translate9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = section4;
			array14[1] = settingsView;
			array14[2] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle27, obj26 = new SimpleValueTargetProvider(array14, SectionBase.TitleProperty, nameScope));
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
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 17)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			section4.Title = obj27;
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "UseUSGallon";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseUSGallon = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseUSGallon")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			section4.SetBinding(RadioCell.SelectedValueProperty, bindingBase4);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "ShowUSGallonSelector";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowUSGallonSelector")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase5);
			translate10.Text = "Settings_Control_ToggleUSGallon.OnContent";
			IMarkupExtension markupExtension15 = translate10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = radioCell8;
			array15[1] = section4;
			array15[2] = settingsView;
			array15[3] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle29, obj28 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
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
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 31)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			radioCell8.Title = obj29;
			staticResourceExtension5.Key = "TrueValue";
			IMarkupExtension markupExtension16 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = radioCell8;
			array16[1] = section4;
			array16[2] = settingsView;
			array16[3] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle31, obj30 = new SimpleValueTargetProvider(array16, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
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
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 104)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			radioCell8.SetValue(RadioCell.ValueProperty, obj31);
			section4.Add(radioCell8);
			translate11.Text = "Settings_Control_ToggleUSGallon.OffContent";
			IMarkupExtension markupExtension17 = translate11;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = radioCell9;
			array17[1] = section4;
			array17[2] = settingsView;
			array17[3] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle33, obj32 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
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
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 31)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			radioCell9.Title = obj33;
			staticResourceExtension6.Key = "FalseValue";
			IMarkupExtension markupExtension18 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = radioCell9;
			array18[1] = section4;
			array18[2] = settingsView;
			array18[3] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle35, obj34 = new SimpleValueTargetProvider(array18, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
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
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 105)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			radioCell9.SetValue(RadioCell.ValueProperty, obj35);
			section4.Add(radioCell9);
			settingsView.Root.Add(section4);
			translate12.Text = "Settings_Control_TogglePressureUnits.Header";
			IMarkupExtension markupExtension19 = translate12;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 3];
			array19[0] = section5;
			array19[1] = settingsView;
			array19[2] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle37, obj36 = new SimpleValueTargetProvider(array19, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
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
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 25)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			section5.Title = obj37;
			bindingExtension6.Path = "Pressure_use_kpa";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Pressure_use_kpa = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Pressure_use_kpa")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			section5.SetBinding(RadioCell.SelectedValueProperty, bindingBase6);
			translate13.Text = "Settings_Control_TogglePressureUnits.OnContent";
			IMarkupExtension markupExtension20 = translate13;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = radioCell10;
			array20[1] = section5;
			array20[2] = settingsView;
			array20[3] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle39, obj38 = new SimpleValueTargetProvider(array20, CellBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
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
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 31)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			radioCell10.Title = obj39;
			staticResourceExtension7.Key = "TrueValue";
			IMarkupExtension markupExtension21 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = radioCell10;
			array21[1] = section5;
			array21[2] = settingsView;
			array21[3] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle41, obj40 = new SimpleValueTargetProvider(array21, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
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
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 109)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			radioCell10.SetValue(RadioCell.ValueProperty, obj41);
			section5.Add(radioCell10);
			translate14.Text = "Settings_Control_TogglePressureUnits.OffContent";
			IMarkupExtension markupExtension22 = translate14;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = radioCell11;
			array22[1] = section5;
			array22[2] = settingsView;
			array22[3] = this;
			object obj42;
			xamlServiceProvider22.Add(typeFromHandle43, obj42 = new SimpleValueTargetProvider(array22, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj42);
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
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 31)));
			object obj43 = markupExtension22.ProvideValue(xamlServiceProvider22);
			radioCell11.Title = obj43;
			staticResourceExtension8.Key = "FalseValue";
			IMarkupExtension markupExtension23 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = radioCell11;
			array23[1] = section5;
			array23[2] = settingsView;
			array23[3] = this;
			object obj44;
			xamlServiceProvider23.Add(typeFromHandle45, obj44 = new SimpleValueTargetProvider(array23, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj44);
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
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 110)));
			object obj45 = markupExtension23.ProvideValue(xamlServiceProvider23);
			radioCell11.SetValue(RadioCell.ValueProperty, obj45);
			section5.Add(radioCell11);
			settingsView.Root.Add(section5);
			translate15.Text = "Settings_Control_ToggleFlowUnits.Header";
			IMarkupExtension markupExtension24 = translate15;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 3];
			array24[0] = section6;
			array24[1] = settingsView;
			array24[2] = this;
			object obj46;
			xamlServiceProvider24.Add(typeFromHandle47, obj46 = new SimpleValueTargetProvider(array24, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj46);
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
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 25)));
			object obj47 = markupExtension24.ProvideValue(xamlServiceProvider24);
			section6.Title = obj47;
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Flow_use_grams_sec";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Flow_use_grams_sec = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Flow_use_grams_sec")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			section6.SetBinding(RadioCell.SelectedValueProperty, bindingBase7);
			translate16.Text = "Settings_Control_ToggleFlowUnits.OnContent";
			IMarkupExtension markupExtension25 = translate16;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = radioCell12;
			array25[1] = section6;
			array25[2] = settingsView;
			array25[3] = this;
			object obj48;
			xamlServiceProvider25.Add(typeFromHandle49, obj48 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj48);
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
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 31)));
			object obj49 = markupExtension25.ProvideValue(xamlServiceProvider25);
			radioCell12.Title = obj49;
			staticResourceExtension9.Key = "TrueValue";
			IMarkupExtension markupExtension26 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = radioCell12;
			array26[1] = section6;
			array26[2] = settingsView;
			array26[3] = this;
			object obj50;
			xamlServiceProvider26.Add(typeFromHandle51, obj50 = new SimpleValueTargetProvider(array26, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj50);
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
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 105)));
			object obj51 = markupExtension26.ProvideValue(xamlServiceProvider26);
			radioCell12.SetValue(RadioCell.ValueProperty, obj51);
			section6.Add(radioCell12);
			translate17.Text = "Settings_Control_ToggleFlowUnits.OffContent";
			IMarkupExtension markupExtension27 = translate17;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = radioCell13;
			array27[1] = section6;
			array27[2] = settingsView;
			array27[3] = this;
			object obj52;
			xamlServiceProvider27.Add(typeFromHandle53, obj52 = new SimpleValueTargetProvider(array27, CellBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj52);
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
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 31)));
			object obj53 = markupExtension27.ProvideValue(xamlServiceProvider27);
			radioCell13.Title = obj53;
			staticResourceExtension10.Key = "FalseValue";
			IMarkupExtension markupExtension28 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = radioCell13;
			array28[1] = section6;
			array28[2] = settingsView;
			array28[3] = this;
			object obj54;
			xamlServiceProvider28.Add(typeFromHandle55, obj54 = new SimpleValueTargetProvider(array28, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj54);
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
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 106)));
			object obj55 = markupExtension28.ProvideValue(xamlServiceProvider28);
			radioCell13.SetValue(RadioCell.ValueProperty, obj55);
			section6.Add(radioCell13);
			settingsView.Root.Add(section6);
			translate18.Text = "Settings_Control_ToggleTemperatureUnits.Header";
			IMarkupExtension markupExtension29 = translate18;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 3];
			array29[0] = section7;
			array29[1] = settingsView;
			array29[2] = this;
			object obj56;
			xamlServiceProvider29.Add(typeFromHandle57, obj56 = new SimpleValueTargetProvider(array29, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj56);
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
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 25)));
			object obj57 = markupExtension29.ProvideValue(xamlServiceProvider29);
			section7.Title = obj57;
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Use_celcium";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_celcium = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_celcium")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			section7.SetBinding(RadioCell.SelectedValueProperty, bindingBase8);
			translate19.Text = "Settings_Control_ToggleTemperatureUnits.OnContent";
			IMarkupExtension markupExtension30 = translate19;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = radioCell14;
			array30[1] = section7;
			array30[2] = settingsView;
			array30[3] = this;
			object obj58;
			xamlServiceProvider30.Add(typeFromHandle59, obj58 = new SimpleValueTargetProvider(array30, CellBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj58);
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
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 31)));
			object obj59 = markupExtension30.ProvideValue(xamlServiceProvider30);
			radioCell14.Title = obj59;
			staticResourceExtension11.Key = "TrueValue";
			IMarkupExtension markupExtension31 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = radioCell14;
			array31[1] = section7;
			array31[2] = settingsView;
			array31[3] = this;
			object obj60;
			xamlServiceProvider31.Add(typeFromHandle61, obj60 = new SimpleValueTargetProvider(array31, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj60);
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
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 112)));
			object obj61 = markupExtension31.ProvideValue(xamlServiceProvider31);
			radioCell14.SetValue(RadioCell.ValueProperty, obj61);
			section7.Add(radioCell14);
			translate20.Text = "Settings_Control_ToggleTemperatureUnits.OffContent";
			IMarkupExtension markupExtension32 = translate20;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = radioCell15;
			array32[1] = section7;
			array32[2] = settingsView;
			array32[3] = this;
			object obj62;
			xamlServiceProvider32.Add(typeFromHandle63, obj62 = new SimpleValueTargetProvider(array32, CellBase.TitleProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj62);
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
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 31)));
			object obj63 = markupExtension32.ProvideValue(xamlServiceProvider32);
			radioCell15.Title = obj63;
			staticResourceExtension12.Key = "FalseValue";
			IMarkupExtension markupExtension33 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = radioCell15;
			array33[1] = section7;
			array33[2] = settingsView;
			array33[3] = this;
			object obj64;
			xamlServiceProvider33.Add(typeFromHandle65, obj64 = new SimpleValueTargetProvider(array33, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj64);
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
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 113)));
			object obj65 = markupExtension33.ProvideValue(xamlServiceProvider33);
			radioCell15.SetValue(RadioCell.ValueProperty, obj65);
			section7.Add(radioCell15);
			settingsView.Root.Add(section7);
			translate21.Text = "Settings_Control_ToggleAccelerationUnits.Header";
			IMarkupExtension markupExtension34 = translate21;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 3];
			array34[0] = section8;
			array34[1] = settingsView;
			array34[2] = this;
			object obj66;
			xamlServiceProvider34.Add(typeFromHandle67, obj66 = new SimpleValueTargetProvider(array34, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj66);
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
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 25)));
			object obj67 = markupExtension34.ProvideValue(xamlServiceProvider34);
			section8.Title = obj67;
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "AccelerationUseG";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AccelerationUseG = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationUseG")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			section8.SetBinding(RadioCell.SelectedValueProperty, bindingBase9);
			radioCell16.SetValue(CellBase.TitleProperty, "g");
			staticResourceExtension13.Key = "TrueValue";
			IMarkupExtension markupExtension35 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = radioCell16;
			array35[1] = section8;
			array35[2] = settingsView;
			array35[3] = this;
			object obj68;
			xamlServiceProvider35.Add(typeFromHandle69, obj68 = new SimpleValueTargetProvider(array35, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj68);
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
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 41)));
			object obj69 = markupExtension35.ProvideValue(xamlServiceProvider35);
			radioCell16.SetValue(RadioCell.ValueProperty, obj69);
			section8.Add(radioCell16);
			radioCell17.SetValue(CellBase.TitleProperty, "m/sec^2");
			staticResourceExtension14.Key = "FalseValue";
			IMarkupExtension markupExtension36 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = radioCell17;
			array36[1] = section8;
			array36[2] = settingsView;
			array36[3] = this;
			object obj70;
			xamlServiceProvider36.Add(typeFromHandle71, obj70 = new SimpleValueTargetProvider(array36, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj70);
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
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 47)));
			object obj71 = markupExtension36.ProvideValue(xamlServiceProvider36);
			radioCell17.SetValue(RadioCell.ValueProperty, obj71);
			section8.Add(radioCell17);
			settingsView.Root.Add(section8);
			translate22.Text = "Settings_Control_TogglePowerUnits.Header";
			IMarkupExtension markupExtension37 = translate22;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 3];
			array37[0] = section9;
			array37[1] = settingsView;
			array37[2] = this;
			object obj72;
			xamlServiceProvider37.Add(typeFromHandle73, obj72 = new SimpleValueTargetProvider(array37, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj72);
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
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 25)));
			object obj73 = markupExtension37.ProvideValue(xamlServiceProvider37);
			section9.Title = obj73;
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "UseHoursePower";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseHoursePower = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseHoursePower")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			section9.SetBinding(RadioCell.SelectedValueProperty, bindingBase10);
			radioCell18.SetValue(CellBase.TitleProperty, "hp");
			staticResourceExtension15.Key = "TrueValue";
			IMarkupExtension markupExtension38 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = radioCell18;
			array38[1] = section9;
			array38[2] = settingsView;
			array38[3] = this;
			object obj74;
			xamlServiceProvider38.Add(typeFromHandle75, obj74 = new SimpleValueTargetProvider(array38, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 42)));
			object obj75 = markupExtension38.ProvideValue(xamlServiceProvider38);
			radioCell18.SetValue(RadioCell.ValueProperty, obj75);
			section9.Add(radioCell18);
			radioCell19.SetValue(CellBase.TitleProperty, "kW");
			staticResourceExtension16.Key = "FalseValue";
			IMarkupExtension markupExtension39 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 4];
			array39[0] = radioCell19;
			array39[1] = section9;
			array39[2] = settingsView;
			array39[3] = this;
			object obj76;
			xamlServiceProvider39.Add(typeFromHandle77, obj76 = new SimpleValueTargetProvider(array39, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 42)));
			object obj77 = markupExtension39.ProvideValue(xamlServiceProvider39);
			radioCell19.SetValue(RadioCell.ValueProperty, obj77);
			section9.Add(radioCell19);
			settingsView.Root.Add(section9);
			translate23.Text = "Settings_Control_ToggleTorqueUnits.Header";
			IMarkupExtension markupExtension40 = translate23;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 3];
			array40[0] = section10;
			array40[1] = settingsView;
			array40[2] = this;
			object obj78;
			xamlServiceProvider40.Add(typeFromHandle79, obj78 = new SimpleValueTargetProvider(array40, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 25)));
			object obj79 = markupExtension40.ProvideValue(xamlServiceProvider40);
			section10.Title = obj79;
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "UseNmForTorque";
			bindingExtension11.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseNmForTorque = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseNmForTorque")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section10.SetBinding(RadioCell.SelectedValueProperty, bindingBase11);
			radioCell20.SetValue(CellBase.TitleProperty, "Nm");
			staticResourceExtension17.Key = "TrueValue";
			IMarkupExtension markupExtension41 = staticResourceExtension17;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = radioCell20;
			array41[1] = section10;
			array41[2] = settingsView;
			array41[3] = this;
			object obj80;
			xamlServiceProvider41.Add(typeFromHandle81, obj80 = new SimpleValueTargetProvider(array41, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 42)));
			object obj81 = markupExtension41.ProvideValue(xamlServiceProvider41);
			radioCell20.SetValue(RadioCell.ValueProperty, obj81);
			section10.Add(radioCell20);
			radioCell21.SetValue(CellBase.TitleProperty, "ft*lbs");
			staticResourceExtension18.Key = "FalseValue";
			IMarkupExtension markupExtension42 = staticResourceExtension18;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = radioCell21;
			array42[1] = section10;
			array42[2] = settingsView;
			array42[3] = this;
			object obj82;
			xamlServiceProvider42.Add(typeFromHandle83, obj82 = new SimpleValueTargetProvider(array42, RadioCell.ValueProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
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
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsUnitsV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 46)));
			object obj83 = markupExtension42.ProvideValue(xamlServiceProvider42);
			radioCell21.SetValue(RadioCell.ValueProperty, obj83);
			section10.Add(radioCell21);
			settingsView.Root.Add(section10);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x0600219D RID: 8605 RVA: 0x00199EEC File Offset: 0x001980EC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsUnitsV3>(this, typeof(SettingsUnitsV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
		}

		// Token: 0x0600219E RID: 8606 RVA: 0x00199F10 File Offset: 0x00198110
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2093(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_km, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x00199F40 File Offset: 0x00198140
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2094(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_km = A_1;
				return;
			}
		}

		// Token: 0x060021A0 RID: 8608 RVA: 0x00199F5C File Offset: 0x0019815C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2095(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021A1 RID: 8609 RVA: 0x00199F6C File Offset: 0x0019816C
		[CompilerGenerated]
		private static ValueTuple<FuelConsumptionUnits, bool> <InitializeComponent>typedBindingsM__2096(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
			}
			return default(ValueTuple<FuelConsumptionUnits, bool>);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x00199F9C File Offset: 0x0019819C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2097(SharedSettings A_0, FuelConsumptionUnits A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelConsumptionUnit = A_1;
				return;
			}
		}

		// Token: 0x060021A3 RID: 8611 RVA: 0x00199FB8 File Offset: 0x001981B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2098(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021A4 RID: 8612 RVA: 0x00199FC8 File Offset: 0x001981C8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2099(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021A5 RID: 8613 RVA: 0x00199FF8 File Offset: 0x001981F8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2100(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseLitersForVolume = A_1;
				return;
			}
		}

		// Token: 0x060021A6 RID: 8614 RVA: 0x0019A014 File Offset: 0x00198214
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2101(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x0019A024 File Offset: 0x00198224
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2102(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x0019A054 File Offset: 0x00198254
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2103(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseUSGallon = A_1;
				return;
			}
		}

		// Token: 0x060021A9 RID: 8617 RVA: 0x0019A070 File Offset: 0x00198270
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2104(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021AA RID: 8618 RVA: 0x0019A080 File Offset: 0x00198280
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2105(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021AB RID: 8619 RVA: 0x0019A0B0 File Offset: 0x001982B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2106(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021AC RID: 8620 RVA: 0x0019A0C0 File Offset: 0x001982C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2107(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021AD RID: 8621 RVA: 0x0019A0F0 File Offset: 0x001982F0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2108(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Pressure_use_kpa = A_1;
				return;
			}
		}

		// Token: 0x060021AE RID: 8622 RVA: 0x0019A10C File Offset: 0x0019830C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2109(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021AF RID: 8623 RVA: 0x0019A11C File Offset: 0x0019831C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2110(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021B0 RID: 8624 RVA: 0x0019A14C File Offset: 0x0019834C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2111(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Flow_use_grams_sec = A_1;
				return;
			}
		}

		// Token: 0x060021B1 RID: 8625 RVA: 0x0019A168 File Offset: 0x00198368
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2112(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021B2 RID: 8626 RVA: 0x0019A178 File Offset: 0x00198378
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2113(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021B3 RID: 8627 RVA: 0x0019A1A8 File Offset: 0x001983A8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2114(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_celcium = A_1;
				return;
			}
		}

		// Token: 0x060021B4 RID: 8628 RVA: 0x0019A1C4 File Offset: 0x001983C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2115(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021B5 RID: 8629 RVA: 0x0019A1D4 File Offset: 0x001983D4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2116(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021B6 RID: 8630 RVA: 0x0019A204 File Offset: 0x00198404
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2117(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AccelerationUseG = A_1;
				return;
			}
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0019A220 File Offset: 0x00198420
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2118(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021B8 RID: 8632 RVA: 0x0019A230 File Offset: 0x00198430
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2119(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x0019A260 File Offset: 0x00198460
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2120(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseHoursePower = A_1;
				return;
			}
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0019A27C File Offset: 0x0019847C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2121(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x0019A28C File Offset: 0x0019848C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2122(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x0019A2BC File Offset: 0x001984BC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2123(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseNmForTorque = A_1;
				return;
			}
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x0019A2D8 File Offset: 0x001984D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2124(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04001019 RID: 4121
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;
	}
}
