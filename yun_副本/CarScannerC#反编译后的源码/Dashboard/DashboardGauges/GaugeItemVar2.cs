using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.SfGauge.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Dashboard.DashboardGauges
{
	// Token: 0x020007AD RID: 1965
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml")]
	public class GaugeItemVar2 : ContentView
	{
		// Token: 0x060043C7 RID: 17351 RVA: 0x00349D78 File Offset: 0x00347F78
		public GaugeItemVar2()
		{
			this.InitializeComponent();
			if (PlatformHelper.IsiOS)
			{
				double num = 1.0;
				this.gaugeScale.LabelFontSize = 11.0 * num;
				this.gaugeScale.LabelOffset = 0.7;
				this.gaugeScale.RimThickness = 5.0 * num;
			}
		}

		// Token: 0x060043C8 RID: 17352 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Frame_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060043C9 RID: 17353 RVA: 0x00349DE4 File Offset: 0x00347FE4
		private async void Gauge_SizeChanged(object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == "iOS")
			{
				if (PlatformHelper.IsiOS)
				{
					double num = ((base.Width < base.Height) ? base.Width : base.Height) / 150.0;
					this.gaugeScale.LabelFontSize = 11.0 * num;
					this.gaugeScale.RimThickness = 5.0 * num;
					return;
				}
			}
			else if (PlatformHelper.IsAndroid)
			{
				double num2 = ((base.Width < base.Height) ? base.Width : base.Height) / 250.0;
				this.gaugeScale.LabelFontSize = 15.0 * num2;
				this.gaugeScale.LabelOffset = 0.85;
				this.minPointer.MarkerWidth = 12.0 * num2;
				this.minPointer.MarkerHeight = 12.0 * num2;
				this.maxPointer.MarkerWidth = 12.0 * num2;
				this.maxPointer.MarkerHeight = 12.0 * num2;
				this.rangePointer.EnableAnimation = true;
				this.redLinePointer.EnableAnimation = true;
				this.blueLinePointer.EnableAnimation = true;
			}
			if (PlatformHelper.IsAndroid)
			{
				await Task.Delay(TimeSpan.FromSeconds(2.0));
				this.rangePointer.EnableAnimation = false;
				this.redLinePointer.EnableAnimation = false;
				this.blueLinePointer.EnableAnimation = false;
				if (SharedSettings.Current.DashboardCircularGaugeAnimation)
				{
					await Task.Delay(TimeSpan.FromSeconds(2.0));
					this.rangePointer.EnableAnimation = true;
					this.redLinePointer.EnableAnimation = true;
					this.blueLinePointer.EnableAnimation = true;
					this.rangePointer.AnimationDuration = 0.25;
					this.redLinePointer.AnimationDuration = 0.3;
					this.blueLinePointer.AnimationDuration = 0.35;
				}
			}
			else
			{
				this.rangePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
				this.redLinePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
				this.blueLinePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
			}
		}

		// Token: 0x060043CA RID: 17354 RVA: 0x00349E1C File Offset: 0x0034801C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(GaugeItemVar2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/GaugeItemVar2.xaml",
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
			ExcludeInfinityFromDoubleConverter excludeInfinityFromDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			LimitValueToVisibleMinimumAndMaximumConverter limitValueToVisibleMinimumAndMaximumConverter;
			VisualDiagnostics.RegisterSourceInfo(limitValueToVisibleMinimumAndMaximumConverter = new LimitValueToVisibleMinimumAndMaximumConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			GaugeMinMaxPointersColorConverter gaugeMinMaxPointersColorConverter;
			VisualDiagnostics.RegisterSourceInfo(gaugeMinMaxPointersColorConverter = new GaugeMinMaxPointersColorConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart excludeInfinityFromDoubleConverterAndLimitToRedLineStart;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverterAndLimitToRedLineStart = new ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish excludeInfinityFromDoubleConverterAndLimitToBlueLineFinish;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverterAndLimitToBlueLineFinish = new ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 36);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			Header header;
			VisualDiagnostics.RegisterSourceInfo(header = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			Header header2;
			VisualDiagnostics.RegisterSourceInfo(header2 = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			Header header3;
			VisualDiagnostics.RegisterSourceInfo(header3 = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 22);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 25);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 25);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 33);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 33);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 33);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 33);
			RangePointer rangePointer;
			VisualDiagnostics.RegisterSourceInfo(rangePointer = new RangePointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 33);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			RangePointer rangePointer2;
			VisualDiagnostics.RegisterSourceInfo(rangePointer2 = new RangePointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 30);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 33);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			RangePointer rangePointer3;
			VisualDiagnostics.RegisterSourceInfo(rangePointer3 = new RangePointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 30);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 33);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 33);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 33);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 33);
			ReferenceExtension referenceExtension4;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension4 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 33);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 33);
			MarkerPointer markerPointer;
			VisualDiagnostics.RegisterSourceInfo(markerPointer = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 30);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 33);
			ReferenceExtension referenceExtension5;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension5 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 33);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 33);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 33);
			ReferenceExtension referenceExtension6;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension6 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 33);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 33);
			MarkerPointer markerPointer2;
			VisualDiagnostics.RegisterSourceInfo(markerPointer2 = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 30);
			Scale scale;
			VisualDiagnostics.RegisterSourceInfo(scale = new Scale(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			SfCircularGauge sfCircularGauge;
			VisualDiagnostics.RegisterSourceInfo(sfCircularGauge = new SfCircularGauge(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 17);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 17);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 31);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 26);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 22);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 14);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 17);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 17);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 17);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 26);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 31);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 14);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 17);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 17);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 17);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 26);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 14);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 17);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 17);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 26);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\GaugeItemVar2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("HackyBindedKey", this);
			if (this.StyleId == null)
			{
				this.StyleId = "HackyBindedKey";
			}
			nameScope.RegisterName("circulargauge", sfCircularGauge);
			if (sfCircularGauge.StyleId == null)
			{
				sfCircularGauge.StyleId = "circulargauge";
			}
			nameScope.RegisterName("gaugeHeaderValue", header);
			if (header.StyleId == null)
			{
				header.StyleId = "gaugeHeaderValue";
			}
			nameScope.RegisterName("gaugeHeaderTitle", header2);
			if (header2.StyleId == null)
			{
				header2.StyleId = "gaugeHeaderTitle";
			}
			nameScope.RegisterName("gaugeHeaderUnits", header3);
			if (header3.StyleId == null)
			{
				header3.StyleId = "gaugeHeaderUnits";
			}
			nameScope.RegisterName("gaugeScale", scale);
			if (scale.StyleId == null)
			{
				scale.StyleId = "gaugeScale";
			}
			nameScope.RegisterName("redLinePointer", rangePointer);
			if (rangePointer.StyleId == null)
			{
				rangePointer.StyleId = "redLinePointer";
			}
			nameScope.RegisterName("rangePointer", rangePointer2);
			if (rangePointer2.StyleId == null)
			{
				rangePointer2.StyleId = "rangePointer";
			}
			nameScope.RegisterName("blueLinePointer", rangePointer3);
			if (rangePointer3.StyleId == null)
			{
				rangePointer3.StyleId = "blueLinePointer";
			}
			nameScope.RegisterName("minPointer", markerPointer);
			if (markerPointer.StyleId == null)
			{
				markerPointer.StyleId = "minPointer";
			}
			nameScope.RegisterName("maxPointer", markerPointer2);
			if (markerPointer2.StyleId == null)
			{
				markerPointer2.StyleId = "maxPointer";
			}
			this.HackyBindedKey = this;
			this.circulargauge = sfCircularGauge;
			this.gaugeHeaderValue = header;
			this.gaugeHeaderTitle = header2;
			this.gaugeHeaderUnits = header3;
			this.gaugeScale = scale;
			this.redLinePointer = rangePointer;
			this.rangePointer = rangePointer2;
			this.blueLinePointer = rangePointer3;
			this.minPointer = markerPointer;
			this.maxPointer = markerPointer2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("LimitValueToVisibleMinimumAndMaximumConverter", limitValueToVisibleMinimumAndMaximumConverter);
			resourceDictionary.Add("GaugeMinMaxPointersColorConverter", gaugeMinMaxPointersColorConverter);
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart", excludeInfinityFromDoubleConverterAndLimitToRedLineStart);
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish", excludeInfinityFromDoubleConverterAndLimitToBlueLineFinish);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			bindingExtension.Path = "ShowDefaultBackground";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowDefaultBackground = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			gaugeBackground.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid.Children.Add(gaugeBackground);
			boxView.SetValue(Grid.RowProperty, 0);
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "CornerRadiusToThicknessConverter";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = bindingExtension2;
			array[1] = boxView;
			array[2] = grid;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			bindingExtension2.Converter = obj2;
			bindingExtension2.Path = "CornerRadius";
			bindingExtension2.TypedBinding = new TypedBinding<DashboardItem, Thickness>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
				}
				return default(ValueTuple<Thickness, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CornerRadius")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			boxView.SetBinding(BoxView.CornerRadiusProperty, bindingBase2);
			boxView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			bindingExtension3.Mode = 2;
			staticResourceExtension2.Key = "TransparentColorToFalseConverter";
			IMarkupExtension markupExtension2 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = bindingExtension3;
			array2[1] = boxView;
			array2[2] = grid;
			array2[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			bindingExtension3.Converter = obj4;
			bindingExtension3.Path = "IndicatorBackgroundColor";
			bindingExtension3.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "IndicatorBackgroundColor")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			boxView.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "IndicatorBackgroundColor";
			bindingExtension4.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "IndicatorBackgroundColor")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			boxView.SetBinding(BoxView.ColorProperty, bindingBase4);
			grid.Children.Add(boxView);
			sfCircularGauge.SetValue(Grid.RowProperty, 0);
			sfCircularGauge.SetValue(Grid.ColumnProperty, 0);
			sfCircularGauge.SetValue(View.MarginProperty, new Thickness(0.0));
			sfCircularGauge.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			sfCircularGauge.SizeChanged += this.Gauge_SizeChanged;
			sfCircularGauge.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "FontName";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			header.SetBinding(Header.FontFamilyProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "ValueTextColor";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			header.SetBinding(Header.ForegroundColorProperty, bindingBase6);
			header.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.45"));
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "Model.TextValue";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			header.SetBinding(Header.TextProperty, bindingBase7);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "ValueFontSize";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			header.SetBinding(Header.TextSizeProperty, bindingBase8);
			sfCircularGauge.Headers.Add(header);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "TitleTextColor";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			header2.SetBinding(Header.ForegroundColorProperty, bindingBase9);
			header2.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.60"));
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "PIDName";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			header2.SetBinding(Header.TextProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "TitleFontSize";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			header2.SetBinding(Header.TextSizeProperty, bindingBase11);
			sfCircularGauge.Headers.Add(header2);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "UnitsTextColor";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			header3.SetBinding(Header.ForegroundColorProperty, bindingBase12);
			header3.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.70"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "Model.Units";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			header3.SetBinding(Header.TextProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "UnitsFontSize";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			header3.SetBinding(Header.TextSizeProperty, bindingBase14);
			sfCircularGauge.Headers.Add(header3);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "Maximum";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			scale.SetBinding(Scale.EndValueProperty, bindingBase15);
			bindingExtension16.Path = "Interval";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Interval, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Interval")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			scale.SetBinding(Scale.IntervalProperty, bindingBase16);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "GaugeLabelColor";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeLabelColor")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			scale.SetBinding(Scale.LabelColorProperty, bindingBase17);
			scale.SetValue(Scale.MinorTicksPerIntervalProperty, 0.0);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "GaugeLabelNumberOfDecimalDigits";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeLabelNumberOfDecimalDigits, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeLabelNumberOfDecimalDigits")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			scale.SetBinding(Scale.NumberOfDecimalDigitsProperty, bindingBase18);
			scale.SetValue(Scale.RadiusFactorProperty, 1.0);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "GaugeRimColor";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRimColor")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			scale.SetBinding(Scale.RimColorProperty, bindingBase19);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "CiruclarGaugeWidth";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CiruclarGaugeWidth")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			scale.SetBinding(Scale.RimThicknessProperty, bindingBase20);
			scale.SetValue(Scale.ShowFirstLabelProperty, true);
			scale.SetValue(Scale.ShowLabelsProperty, false);
			scale.SetValue(Scale.ShowLastLabelProperty, true);
			scale.SetValue(Scale.ShowTicksProperty, false);
			scale.SetValue(Scale.StartAngleProperty, 135.0);
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "Minimum";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			scale.SetBinding(Scale.StartValueProperty, bindingBase21);
			scale.SetValue(Scale.SweepAngleProperty, 270.0);
			rangePointer.SetValue(Pointer.EnableAnimationProperty, true);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "CiruclarGaugeWidth";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			rangePointer.SetBinding(RangePointer.ThicknessProperty, bindingBase22);
			rangePointer.SetValue(RangePointer.OffsetProperty, 1.0);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "GaugeRedLineColor";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			rangePointer.SetBinding(Pointer.ColorProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			staticResourceExtension3.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = bindingExtension24;
			array3[1] = rangePointer;
			array3[2] = scale;
			array3[3] = sfCircularGauge;
			array3[4] = grid;
			array3[5] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 33)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension24.Converter = obj6;
			bindingExtension24.Path = "Model.FloatValue";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			rangePointer.SetBinding(Pointer.ValueProperty, bindingBase24);
			scale.GetValue(Scale.PointersProperty).Add(rangePointer);
			rangePointer2.SetValue(Pointer.EnableAnimationProperty, true);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "CiruclarGaugeWidth";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			rangePointer2.SetBinding(RangePointer.ThicknessProperty, bindingBase25);
			rangePointer2.SetValue(RangePointer.OffsetProperty, 1.0);
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "GaugePointerColor";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			rangePointer2.SetBinding(Pointer.ColorProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			staticResourceExtension4.Key = "ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart";
			IMarkupExtension markupExtension4 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension27;
			array4[1] = rangePointer2;
			array4[2] = scale;
			array4[3] = sfCircularGauge;
			array4[4] = grid;
			array4[5] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension27.Converter = obj8;
			referenceExtension.Name = "HackyBindedKey";
			IMarkupExtension markupExtension5 = referenceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension27;
			array5[1] = rangePointer2;
			array5[2] = scale;
			array5[3] = sfCircularGauge;
			array5[4] = grid;
			array5[5] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension27.ConverterParameter = obj10;
			bindingExtension27.Path = "Model.FloatValue";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			rangePointer2.SetBinding(Pointer.ValueProperty, bindingBase27);
			scale.GetValue(Scale.PointersProperty).Add(rangePointer2);
			rangePointer3.SetValue(Pointer.EnableAnimationProperty, true);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "CiruclarGaugeWidth";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			rangePointer3.SetBinding(RangePointer.ThicknessProperty, bindingBase28);
			rangePointer3.SetValue(RangePointer.OffsetProperty, 1.0);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "GaugeBlueLineColor";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			rangePointer3.SetBinding(Pointer.ColorProperty, bindingBase29);
			bindingExtension30.Mode = 2;
			staticResourceExtension5.Key = "ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish";
			IMarkupExtension markupExtension6 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension30;
			array6[1] = rangePointer3;
			array6[2] = scale;
			array6[3] = sfCircularGauge;
			array6[4] = grid;
			array6[5] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension30.Converter = obj12;
			referenceExtension2.Name = "HackyBindedKey";
			IMarkupExtension markupExtension7 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension30;
			array7[1] = rangePointer3;
			array7[2] = scale;
			array7[3] = sfCircularGauge;
			array7[4] = grid;
			array7[5] = this;
			object obj13;
			xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension30.ConverterParameter = obj14;
			bindingExtension30.Path = "Model.FloatValue";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			rangePointer3.SetBinding(Pointer.ValueProperty, bindingBase30);
			scale.GetValue(Scale.PointersProperty).Add(rangePointer3);
			markerPointer.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension31.Mode = 2;
			staticResourceExtension6.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension31;
			array8[1] = markerPointer;
			array8[2] = scale;
			array8[3] = sfCircularGauge;
			array8[4] = grid;
			array8[5] = this;
			object obj15;
			xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 33)));
			object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension31.Converter = obj16;
			referenceExtension3.Name = "HackyBindedKey";
			IMarkupExtension markupExtension9 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension31;
			array9[1] = markerPointer;
			array9[2] = scale;
			array9[3] = sfCircularGauge;
			array9[4] = grid;
			array9[5] = this;
			object obj17;
			xamlServiceProvider9.Add(typeFromHandle17, obj17 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver9.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 33)));
			object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension31.ConverterParameter = obj18;
			bindingExtension31.Path = "MinMaxPointersColor";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ColorProperty, bindingBase31);
			bindingExtension32.Mode = 2;
			staticResourceExtension7.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension32;
			array10[1] = markerPointer;
			array10[2] = scale;
			array10[3] = sfCircularGauge;
			array10[4] = grid;
			array10[5] = this;
			object obj19;
			xamlServiceProvider10.Add(typeFromHandle19, obj19 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver10.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 33)));
			object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension32.Converter = obj20;
			referenceExtension4.Name = "HackyBindedKey";
			IMarkupExtension markupExtension11 = referenceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension32;
			array11[1] = markerPointer;
			array11[2] = scale;
			array11[3] = sfCircularGauge;
			array11[4] = grid;
			array11[5] = this;
			object obj21;
			xamlServiceProvider11.Add(typeFromHandle21, obj21 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver11.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 33)));
			object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension32.ConverterParameter = obj22;
			bindingExtension32.Path = "Model.MinimumAchieved";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ValueProperty, bindingBase32);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer);
			markerPointer2.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer2.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension33.Mode = 2;
			staticResourceExtension8.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = bindingExtension33;
			array12[1] = markerPointer2;
			array12[2] = scale;
			array12[3] = sfCircularGauge;
			array12[4] = grid;
			array12[5] = this;
			object obj23;
			xamlServiceProvider12.Add(typeFromHandle23, obj23 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver12.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(155, 33)));
			object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension33.Converter = obj24;
			referenceExtension5.Name = "HackyBindedKey";
			IMarkupExtension markupExtension13 = referenceExtension5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension33;
			array13[1] = markerPointer2;
			array13[2] = scale;
			array13[3] = sfCircularGauge;
			array13[4] = grid;
			array13[5] = this;
			object obj25;
			xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver13.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(155, 33)));
			object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension33.ConverterParameter = obj26;
			bindingExtension33.Path = "MinMaxPointersColor";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ColorProperty, bindingBase33);
			bindingExtension34.Mode = 2;
			staticResourceExtension9.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = bindingExtension34;
			array14[1] = markerPointer2;
			array14[2] = scale;
			array14[3] = sfCircularGauge;
			array14[4] = grid;
			array14[5] = this;
			object obj27;
			xamlServiceProvider14.Add(typeFromHandle27, obj27 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver14.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 33)));
			object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension34.Converter = obj28;
			referenceExtension6.Name = "HackyBindedKey";
			IMarkupExtension markupExtension15 = referenceExtension6;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = bindingExtension34;
			array15[1] = markerPointer2;
			array15[2] = scale;
			array15[3] = sfCircularGauge;
			array15[4] = grid;
			array15[5] = this;
			object obj29;
			xamlServiceProvider15.Add(typeFromHandle29, obj29 = new SimpleValueTargetProvider(array15, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver15.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 33)));
			object obj30 = markupExtension15.ProvideValue(xamlServiceProvider15);
			bindingExtension34.ConverterParameter = obj30;
			bindingExtension34.Path = "Model.MaximumAchieved";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ValueProperty, bindingBase34);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer2);
			sfCircularGauge.GetValue(SfCircularGauge.ScalesProperty).Add(scale);
			grid.Children.Add(sfCircularGauge);
			label.SetValue(View.MarginProperty, new Thickness(0.0));
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = label;
			array16[1] = grid;
			array16[2] = this;
			object obj31;
			xamlServiceProvider16.Add(typeFromHandle31, obj31 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver16.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(GaugeItemVar2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 17)));
			DynamicResource dynamicResource = markupExtension16.ProvideValue(xamlServiceProvider16);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension35.Mode = 2;
			bindingExtension35.Source = sharedSettings;
			bindingExtension35.Path = "ShowPing";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			label.SetBinding(VisualElement.IsVisibleProperty, bindingBase35);
			label.SetValue(Label.TextColorProperty, Color.Red);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension36.Mode = 2;
			bindingExtension36.Path = "Model.Ping";
			bindingExtension36.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<long, bool>(model.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase36);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, "ms");
			formattedString.Spans.Add(span2);
			label.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension37.Mode = 2;
			bindingExtension37.Path = "MinMaxAvgFontSize";
			bindingExtension37.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			label2.SetBinding(Label.FontSizeProperty, bindingBase37);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "ShowMinMax";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMax")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase38);
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "MinMaxAvgColor";
			bindingExtension39.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			label2.SetBinding(Label.TextColorProperty, bindingBase39);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span3.SetValue(Span.TextProperty, "Max: ");
			formattedString2.Spans.Add(span3);
			bindingExtension40.Mode = 2;
			bindingExtension40.Path = "Model.MaximumAchievedText";
			bindingExtension40.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						return new ValueTuple<string, bool>(model2.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			span4.SetBinding(Span.TextProperty, bindingBase40);
			formattedString2.Spans.Add(span4);
			label2.SetValue(Label.FormattedTextProperty, formattedString2);
			grid.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension41.Mode = 2;
			bindingExtension41.Path = "MinMaxAvgFontSize";
			bindingExtension41.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase41);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "ShowMinMax";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMax")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase42);
			bindingExtension43.Mode = 2;
			bindingExtension43.Path = "MinMaxAvgColor";
			bindingExtension43.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase43);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span5.SetValue(Span.TextProperty, "Min: ");
			formattedString3.Spans.Add(span5);
			bindingExtension44.Mode = 2;
			bindingExtension44.Path = "Model.MinimumAchievedText";
			bindingExtension44.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<string, bool>(model3.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase44);
			formattedString3.Spans.Add(span6);
			label3.SetValue(Label.FormattedTextProperty, formattedString3);
			grid.Children.Add(label3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension45.Mode = 2;
			bindingExtension45.Path = "MinMaxAvgFontSize";
			bindingExtension45.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase45);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension46.Mode = 2;
			bindingExtension46.Path = "ShowAvg";
			bindingExtension46.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowAvg")
			});
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase46);
			bindingExtension47.Mode = 2;
			bindingExtension47.Path = "MinMaxAvgColor";
			bindingExtension47.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase47);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span7.SetValue(Span.TextProperty, "Avg: ");
			formattedString4.Spans.Add(span7);
			bindingExtension48.Mode = 2;
			bindingExtension48.Path = "Model.AverageTextValue";
			bindingExtension48.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase48);
			formattedString4.Spans.Add(span8);
			label4.SetValue(Label.FormattedTextProperty, formattedString4);
			grid.Children.Add(label4);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0034DC1C File Offset: 0x0034BE1C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<GaugeItemVar2>(this, typeof(GaugeItemVar2));
			this.HackyBindedKey = NameScopeExtensions.FindByName<ContentView>(this, "HackyBindedKey");
			this.circulargauge = NameScopeExtensions.FindByName<SfCircularGauge>(this, "circulargauge");
			this.gaugeHeaderValue = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderValue");
			this.gaugeHeaderTitle = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderTitle");
			this.gaugeHeaderUnits = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderUnits");
			this.gaugeScale = NameScopeExtensions.FindByName<Scale>(this, "gaugeScale");
			this.redLinePointer = NameScopeExtensions.FindByName<RangePointer>(this, "redLinePointer");
			this.rangePointer = NameScopeExtensions.FindByName<RangePointer>(this, "rangePointer");
			this.blueLinePointer = NameScopeExtensions.FindByName<RangePointer>(this, "blueLinePointer");
			this.minPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "minPointer");
			this.maxPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "maxPointer");
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0034DCF8 File Offset: 0x0034BEF8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__456(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0034DD28 File Offset: 0x0034BF28
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__457(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0034DD44 File Offset: 0x0034BF44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__458(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0034DD54 File Offset: 0x0034BF54
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__459(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0034DD84 File Offset: 0x0034BF84
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__460(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043D1 RID: 17361 RVA: 0x0034DD94 File Offset: 0x0034BF94
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__461(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043D2 RID: 17362 RVA: 0x0034DDC4 File Offset: 0x0034BFC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__462(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0034DDD4 File Offset: 0x0034BFD4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__463(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043D4 RID: 17364 RVA: 0x0034DE04 File Offset: 0x0034C004
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__464(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043D5 RID: 17365 RVA: 0x0034DE14 File Offset: 0x0034C014
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__465(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043D6 RID: 17366 RVA: 0x0034DE44 File Offset: 0x0034C044
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__466(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043D7 RID: 17367 RVA: 0x0034DE54 File Offset: 0x0034C054
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__467(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0034DE84 File Offset: 0x0034C084
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__469(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0034DE94 File Offset: 0x0034C094
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__470(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0034DEC4 File Offset: 0x0034C0C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__471(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0034DED4 File Offset: 0x0034C0D4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__472(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeLabelNumberOfDecimalDigits, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043DC RID: 17372 RVA: 0x0034DF04 File Offset: 0x0034C104
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__473(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0034DF14 File Offset: 0x0034C114
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__474(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043DE RID: 17374 RVA: 0x0034DF44 File Offset: 0x0034C144
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__475(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043DF RID: 17375 RVA: 0x0034DF54 File Offset: 0x0034C154
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__476(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043E0 RID: 17376 RVA: 0x0034DF84 File Offset: 0x0034C184
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__477(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043E1 RID: 17377 RVA: 0x0034DF94 File Offset: 0x0034C194
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__478(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043E2 RID: 17378 RVA: 0x0034DFC4 File Offset: 0x0034C1C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__479(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043E3 RID: 17379 RVA: 0x0034DFD4 File Offset: 0x0034C1D4
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__480(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<long, bool>(model.Ping, true);
				}
			}
			return default(ValueTuple<long, bool>);
		}

		// Token: 0x060043E4 RID: 17380 RVA: 0x0034E00C File Offset: 0x0034C20C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__481(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043E5 RID: 17381 RVA: 0x0034E01C File Offset: 0x0034C21C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__482(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043E6 RID: 17382 RVA: 0x0034E030 File Offset: 0x0034C230
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__483(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043E7 RID: 17383 RVA: 0x0034E060 File Offset: 0x0034C260
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__484(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043E8 RID: 17384 RVA: 0x0034E070 File Offset: 0x0034C270
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__485(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0034E0A0 File Offset: 0x0034C2A0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__486(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043EA RID: 17386 RVA: 0x0034E0B0 File Offset: 0x0034C2B0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__487(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043EB RID: 17387 RVA: 0x0034E0E0 File Offset: 0x0034C2E0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__488(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043EC RID: 17388 RVA: 0x0034E0F0 File Offset: 0x0034C2F0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__489(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.MaximumAchievedText, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060043ED RID: 17389 RVA: 0x0034E128 File Offset: 0x0034C328
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__490(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043EE RID: 17390 RVA: 0x0034E138 File Offset: 0x0034C338
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__491(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043EF RID: 17391 RVA: 0x0034E14C File Offset: 0x0034C34C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__492(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043F0 RID: 17392 RVA: 0x0034E17C File Offset: 0x0034C37C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__493(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043F1 RID: 17393 RVA: 0x0034E18C File Offset: 0x0034C38C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__494(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043F2 RID: 17394 RVA: 0x0034E1BC File Offset: 0x0034C3BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__495(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0034E1CC File Offset: 0x0034C3CC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__496(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0034E1FC File Offset: 0x0034C3FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__497(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0034E20C File Offset: 0x0034C40C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__498(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.MinimumAchievedText, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0034E244 File Offset: 0x0034C444
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__499(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0034E254 File Offset: 0x0034C454
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__500(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043F8 RID: 17400 RVA: 0x0034E268 File Offset: 0x0034C468
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__501(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043F9 RID: 17401 RVA: 0x0034E298 File Offset: 0x0034C498
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__502(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043FA RID: 17402 RVA: 0x0034E2A8 File Offset: 0x0034C4A8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__503(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043FB RID: 17403 RVA: 0x0034E2D8 File Offset: 0x0034C4D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__504(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043FC RID: 17404 RVA: 0x0034E2E8 File Offset: 0x0034C4E8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__505(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043FD RID: 17405 RVA: 0x0034E318 File Offset: 0x0034C518
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__506(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043FE RID: 17406 RVA: 0x0034E328 File Offset: 0x0034C528
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__507(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.AverageTextValue, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060043FF RID: 17407 RVA: 0x0034E360 File Offset: 0x0034C560
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__508(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004400 RID: 17408 RVA: 0x0034E370 File Offset: 0x0034C570
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__509(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028AF RID: 10415
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView HackyBindedKey;

		// Token: 0x040028B0 RID: 10416
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfCircularGauge circulargauge;

		// Token: 0x040028B1 RID: 10417
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderValue;

		// Token: 0x040028B2 RID: 10418
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderTitle;

		// Token: 0x040028B3 RID: 10419
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderUnits;

		// Token: 0x040028B4 RID: 10420
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Scale gaugeScale;

		// Token: 0x040028B5 RID: 10421
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RangePointer redLinePointer;

		// Token: 0x040028B6 RID: 10422
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RangePointer rangePointer;

		// Token: 0x040028B7 RID: 10423
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RangePointer blueLinePointer;

		// Token: 0x040028B8 RID: 10424
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer minPointer;

		// Token: 0x040028B9 RID: 10425
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer maxPointer;

		// Token: 0x020007AE RID: 1966
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Gauge_SizeChanged>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004401 RID: 17409 RVA: 0x0034E384 File Offset: 0x0034C584
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				GaugeItemVar2 gaugeItemVar = this;
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
							goto IL_0271;
						}
						if (Device.RuntimePlatform == "iOS")
						{
							if (PlatformHelper.IsiOS)
							{
								double num3 = ((gaugeItemVar.Width < gaugeItemVar.Height) ? gaugeItemVar.Width : gaugeItemVar.Height) / 150.0;
								gaugeItemVar.gaugeScale.LabelFontSize = 11.0 * num3;
								gaugeItemVar.gaugeScale.RimThickness = 5.0 * num3;
								goto IL_0334;
							}
						}
						else if (PlatformHelper.IsAndroid)
						{
							double num4 = ((gaugeItemVar.Width < gaugeItemVar.Height) ? gaugeItemVar.Width : gaugeItemVar.Height) / 250.0;
							gaugeItemVar.gaugeScale.LabelFontSize = 15.0 * num4;
							gaugeItemVar.gaugeScale.LabelOffset = 0.85;
							gaugeItemVar.minPointer.MarkerWidth = 12.0 * num4;
							gaugeItemVar.minPointer.MarkerHeight = 12.0 * num4;
							gaugeItemVar.maxPointer.MarkerWidth = 12.0 * num4;
							gaugeItemVar.maxPointer.MarkerHeight = 12.0 * num4;
							gaugeItemVar.rangePointer.EnableAnimation = true;
							gaugeItemVar.redLinePointer.EnableAnimation = true;
							gaugeItemVar.blueLinePointer.EnableAnimation = true;
						}
						if (!PlatformHelper.IsAndroid)
						{
							gaugeItemVar.rangePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							gaugeItemVar.redLinePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							gaugeItemVar.blueLinePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							goto IL_0319;
						}
						taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItemVar2.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
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
					gaugeItemVar.rangePointer.EnableAnimation = false;
					gaugeItemVar.redLinePointer.EnableAnimation = false;
					gaugeItemVar.blueLinePointer.EnableAnimation = false;
					if (!SharedSettings.Current.DashboardCircularGaugeAnimation)
					{
						goto IL_0319;
					}
					taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItemVar2.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_0271:
					taskAwaiter.GetResult();
					gaugeItemVar.rangePointer.EnableAnimation = true;
					gaugeItemVar.redLinePointer.EnableAnimation = true;
					gaugeItemVar.blueLinePointer.EnableAnimation = true;
					gaugeItemVar.rangePointer.AnimationDuration = 0.25;
					gaugeItemVar.redLinePointer.AnimationDuration = 0.3;
					gaugeItemVar.blueLinePointer.AnimationDuration = 0.35;
					IL_0319:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0334:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004402 RID: 17410 RVA: 0x0034E6F4 File Offset: 0x0034C8F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040028BA RID: 10426
			public int <>1__state;

			// Token: 0x040028BB RID: 10427
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040028BC RID: 10428
			public GaugeItemVar2 <>4__this;

			// Token: 0x040028BD RID: 10429
			private TaskAwaiter <>u__1;
		}
	}
}
