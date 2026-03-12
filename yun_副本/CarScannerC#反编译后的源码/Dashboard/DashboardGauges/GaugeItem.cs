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
	// Token: 0x020007AB RID: 1963
	[XamlFilePath("Dashboard\\DashboardGauges\\GaugeItem.xaml")]
	public class GaugeItem : ContentView
	{
		// Token: 0x06004389 RID: 17289 RVA: 0x003455A0 File Offset: 0x003437A0
		public GaugeItem()
		{
			this.InitializeComponent();
			if (PlatformHelper.IsiOS)
			{
				double num = 1.0;
				this.gaugeScale.LabelFontSize = 11.0 * num;
				this.gaugeScale.LabelOffset = 0.7;
				this.gaugePointer.Thickness = 3.0 * num;
				this.gaugePointer.KnobRadius = 6.0 * num;
				this.gaugePointer.Type = 1;
				this.gaugePointer.LengthFactor = 0.8;
				this.gaugePointer.TailLengthFactor = 0.15;
				this.gaugeScale.RimThickness = 5.0 * num;
				this.gaugeTick.Length = 10.0 * num;
				this.gaugeTick.Thickness = 3.0 * num;
				this.gaugeRedLineRange.Offset = 0.98;
				this.gaugePointer.EnableAnimation = true;
				this.gaugePointer.AnimationDuration = 0.15;
				this.gaugeRedLineRange.Thickness = this.gaugeTick.Length;
			}
		}

		// Token: 0x0600438A RID: 17290 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Frame_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600438B RID: 17291 RVA: 0x003456E4 File Offset: 0x003438E4
		private async void Gauge_SizeChanged(object sender, EventArgs e)
		{
			if (Device.RuntimePlatform == "iOS")
			{
				double num = ((base.Width < base.Height) ? base.Width : base.Height) / 150.0;
				this.gaugeScale.LabelFontSize = 11.0 * num;
				this.gaugeRedLineRange.Thickness = this.gaugeTick.Length;
				this.gaugePointer.Thickness = 3.0 * num;
				this.gaugePointer.KnobRadius = 6.0 * num;
				this.gaugeScale.RimThickness = 5.0 * num;
				this.gaugeRedLineRange.Thickness = this.gaugeTick.Length;
			}
			else
			{
				if (PlatformHelper.IsAndroid)
				{
					double num2 = ((base.Width < base.Height) ? base.Width : base.Height) / 250.0;
					this.gaugeScale.LabelFontSize = 20.0 * num2;
					this.gaugeScale.LabelOffset = 0.7;
					this.gaugePointer.Thickness = 5.0 * num2;
					this.gaugePointer.KnobRadius = 10.0 * num2;
					this.gaugePointer.Type = 1;
					this.gaugePointer.LengthFactor = 0.9;
					this.gaugePointer.TailLengthFactor = 0.15;
					this.gaugeScale.RimThickness = 5.0 * num2;
					this.gaugeTick.Length = 12.0 * num2;
					this.gaugeTick.Thickness = 3.0 * num2;
					this.gaugeSmallTick.Length = 6.0 * num2;
					this.gaugeSmallTick.Thickness = 1.5 * num2;
					this.gaugeBlueLineRange.Offset = 0.98;
					this.gaugeRedLineRange.Offset = 0.98;
					this.gaugePointer.EnableAnimation = true;
					this.minPointer.MarkerWidth = 12.0 * num2;
					this.minPointer.MarkerHeight = 12.0 * num2;
					this.maxPointer.MarkerWidth = 12.0 * num2;
					this.maxPointer.MarkerHeight = 12.0 * num2;
				}
				this.gaugeRedLineRange.Thickness = this.gaugeTick.Length;
				this.gaugeBlueLineRange.Thickness = this.gaugeTick.Length;
				if (PlatformHelper.IsAndroid)
				{
					await Task.Delay(TimeSpan.FromSeconds(2.0));
					this.gaugePointer.EnableAnimation = false;
					if (SharedSettings.Current.DashboardCircularGaugeAnimation)
					{
						await Task.Delay(TimeSpan.FromSeconds(2.0));
						this.gaugePointer.EnableAnimation = true;
						this.gaugePointer.AnimationDuration = 0.35;
					}
				}
				else
				{
					this.gaugePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
				}
			}
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0034571C File Offset: 0x0034391C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(GaugeItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/GaugeItem.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			LimitValueToVisibleMinimumAndMaximumConverter limitValueToVisibleMinimumAndMaximumConverter;
			VisualDiagnostics.RegisterSourceInfo(limitValueToVisibleMinimumAndMaximumConverter = new LimitValueToVisibleMinimumAndMaximumConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			GaugeMinMaxPointersColorConverter gaugeMinMaxPointersColorConverter;
			VisualDiagnostics.RegisterSourceInfo(gaugeMinMaxPointersColorConverter = new GaugeMinMaxPointersColorConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 36);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 25);
			Header header;
			VisualDiagnostics.RegisterSourceInfo(header = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			Header header2;
			VisualDiagnostics.RegisterSourceInfo(header2 = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			Header header3;
			VisualDiagnostics.RegisterSourceInfo(header3 = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 25);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 68);
			TickSettings tickSettings;
			VisualDiagnostics.RegisterSourceInfo(tickSettings = new TickSettings(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 30);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 73);
			TickSettings tickSettings2;
			VisualDiagnostics.RegisterSourceInfo(tickSettings2 = new TickSettings(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 30);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 33);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 33);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 33);
			Range range;
			VisualDiagnostics.RegisterSourceInfo(range = new Range(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 30);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 33);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 33);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 33);
			Range range2;
			VisualDiagnostics.RegisterSourceInfo(range2 = new Range(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 30);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 33);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 33);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			NeedlePointer needlePointer;
			VisualDiagnostics.RegisterSourceInfo(needlePointer = new NeedlePointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 30);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			MarkerPointer markerPointer;
			VisualDiagnostics.RegisterSourceInfo(markerPointer = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 30);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 33);
			ReferenceExtension referenceExtension4;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension4 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 33);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 33);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 33);
			ReferenceExtension referenceExtension5;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension5 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 33);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 33);
			MarkerPointer markerPointer2;
			VisualDiagnostics.RegisterSourceInfo(markerPointer2 = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 30);
			Scale scale;
			VisualDiagnostics.RegisterSourceInfo(scale = new Scale(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 22);
			SfCircularGauge sfCircularGauge;
			VisualDiagnostics.RegisterSourceInfo(sfCircularGauge = new SfCircularGauge(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 17);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 17);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 31);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 26);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 22);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 14);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 17);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 17);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 17);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 26);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 31);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 14);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 17);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 17);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 17);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 26);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 14);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 17);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 17);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 26);
			BindingExtension bindingExtension50;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension50 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\GaugeItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("gaugeTick", tickSettings);
			if (tickSettings.StyleId == null)
			{
				tickSettings.StyleId = "gaugeTick";
			}
			nameScope.RegisterName("gaugeSmallTick", tickSettings2);
			if (tickSettings2.StyleId == null)
			{
				tickSettings2.StyleId = "gaugeSmallTick";
			}
			nameScope.RegisterName("gaugeBlueLineRange", range);
			if (range.StyleId == null)
			{
				range.StyleId = "gaugeBlueLineRange";
			}
			nameScope.RegisterName("gaugeRedLineRange", range2);
			if (range2.StyleId == null)
			{
				range2.StyleId = "gaugeRedLineRange";
			}
			nameScope.RegisterName("gaugePointer", needlePointer);
			if (needlePointer.StyleId == null)
			{
				needlePointer.StyleId = "gaugePointer";
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
			this.gaugeTick = tickSettings;
			this.gaugeSmallTick = tickSettings2;
			this.gaugeBlueLineRange = range;
			this.gaugeRedLineRange = range2;
			this.gaugePointer = needlePointer;
			this.minPointer = markerPointer;
			this.maxPointer = markerPointer2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("LimitValueToVisibleMinimumAndMaximumConverter", limitValueToVisibleMinimumAndMaximumConverter);
			resourceDictionary.Add("GaugeMinMaxPointersColorConverter", gaugeMinMaxPointersColorConverter);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
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
			header.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.7"));
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
			header2.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.85"));
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
			header3.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.95"));
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
			scale.SetValue(Scale.MinorTicksPerIntervalProperty, 1.0);
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
			scale.SetValue(Scale.StartAngleProperty, 135.0);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "Minimum";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			scale.SetBinding(Scale.StartValueProperty, bindingBase20);
			scale.SetValue(Scale.SweepAngleProperty, 270.0);
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "GaugeTickColor";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeTickColor")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			tickSettings.SetBinding(TickSettings.ColorProperty, bindingBase21);
			scale.SetValue(Scale.MajorTickSettingsProperty, tickSettings);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "GaugeTickColor";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeTickColor")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			tickSettings2.SetBinding(TickSettings.ColorProperty, bindingBase22);
			scale.SetValue(Scale.MinorTickSettingsProperty, tickSettings2);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "GaugeBlueLineFinish";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			range.SetBinding(Range.EndValueProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "GaugeBlueLineStart";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			range.SetBinding(Range.StartValueProperty, bindingBase24);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "GaugeBlueLineColor";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			range.SetBinding(Range.ColorProperty, bindingBase25);
			scale.GetValue(Scale.RangesProperty).Add(range);
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "GaugeRedLineFinish";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			range2.SetBinding(Range.EndValueProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "GaugeRedLineStart";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			range2.SetBinding(Range.StartValueProperty, bindingBase27);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "GaugeRedLineColor";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			range2.SetBinding(Range.ColorProperty, bindingBase28);
			scale.GetValue(Scale.RangesProperty).Add(range2);
			needlePointer.SetValue(Pointer.EnableAnimationProperty, true);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "GaugeKnobColor";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			needlePointer.SetBinding(NeedlePointer.KnobColorProperty, bindingBase29);
			needlePointer.SetValue(NeedlePointer.KnobRadiusProperty, 10.0);
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "GaugePointerColor";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			needlePointer.SetBinding(NeedlePointer.TailColorProperty, bindingBase30);
			needlePointer.SetValue(NeedlePointer.ThicknessProperty, 7.0);
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "GaugePointerColor";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			needlePointer.SetBinding(Pointer.ColorProperty, bindingBase31);
			bindingExtension32.Mode = 2;
			staticResourceExtension3.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = bindingExtension32;
			array3[1] = needlePointer;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension32.Converter = obj6;
			referenceExtension.Name = "HackyBindedKey";
			IMarkupExtension markupExtension4 = referenceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension32;
			array4[1] = needlePointer;
			array4[2] = scale;
			array4[3] = sfCircularGauge;
			array4[4] = grid;
			array4[5] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension32.ConverterParameter = obj8;
			bindingExtension32.Path = "Model.FloatValue";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			needlePointer.SetBinding(Pointer.ValueProperty, bindingBase32);
			scale.GetValue(Scale.PointersProperty).Add(needlePointer);
			markerPointer.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension33.Mode = 2;
			staticResourceExtension4.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension33;
			array5[1] = markerPointer;
			array5[2] = scale;
			array5[3] = sfCircularGauge;
			array5[4] = grid;
			array5[5] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 33)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension33.Converter = obj10;
			referenceExtension2.Name = "HackyBindedKey";
			IMarkupExtension markupExtension6 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension33;
			array6[1] = markerPointer;
			array6[2] = scale;
			array6[3] = sfCircularGauge;
			array6[4] = grid;
			array6[5] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 33)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension33.ConverterParameter = obj12;
			bindingExtension33.Path = "MinMaxPointersColor";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ColorProperty, bindingBase33);
			bindingExtension34.Mode = 2;
			staticResourceExtension5.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension34;
			array7[1] = markerPointer;
			array7[2] = scale;
			array7[3] = sfCircularGauge;
			array7[4] = grid;
			array7[5] = this;
			object obj13;
			xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension34.Converter = obj14;
			referenceExtension3.Name = "HackyBindedKey";
			IMarkupExtension markupExtension8 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension34;
			array8[1] = markerPointer;
			array8[2] = scale;
			array8[3] = sfCircularGauge;
			array8[4] = grid;
			array8[5] = this;
			object obj15;
			xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension34.ConverterParameter = obj16;
			bindingExtension34.Path = "Model.MinimumAchieved";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ValueProperty, bindingBase34);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer);
			markerPointer2.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer2.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension35.Mode = 2;
			staticResourceExtension6.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension35;
			array9[1] = markerPointer2;
			array9[2] = scale;
			array9[3] = sfCircularGauge;
			array9[4] = grid;
			array9[5] = this;
			object obj17;
			xamlServiceProvider9.Add(typeFromHandle17, obj17 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 33)));
			object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension35.Converter = obj18;
			referenceExtension4.Name = "HackyBindedKey";
			IMarkupExtension markupExtension10 = referenceExtension4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension35;
			array10[1] = markerPointer2;
			array10[2] = scale;
			array10[3] = sfCircularGauge;
			array10[4] = grid;
			array10[5] = this;
			object obj19;
			xamlServiceProvider10.Add(typeFromHandle19, obj19 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 33)));
			object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension35.ConverterParameter = obj20;
			bindingExtension35.Path = "MinMaxPointersColor";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ColorProperty, bindingBase35);
			bindingExtension36.Mode = 2;
			staticResourceExtension7.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension36;
			array11[1] = markerPointer2;
			array11[2] = scale;
			array11[3] = sfCircularGauge;
			array11[4] = grid;
			array11[5] = this;
			object obj21;
			xamlServiceProvider11.Add(typeFromHandle21, obj21 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 33)));
			object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension36.Converter = obj22;
			referenceExtension5.Name = "HackyBindedKey";
			IMarkupExtension markupExtension12 = referenceExtension5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = bindingExtension36;
			array12[1] = markerPointer2;
			array12[2] = scale;
			array12[3] = sfCircularGauge;
			array12[4] = grid;
			array12[5] = this;
			object obj23;
			xamlServiceProvider12.Add(typeFromHandle23, obj23 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 33)));
			object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension36.ConverterParameter = obj24;
			bindingExtension36.Path = "Model.MaximumAchieved";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ValueProperty, bindingBase36);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer2);
			sfCircularGauge.GetValue(SfCircularGauge.ScalesProperty).Add(scale);
			grid.Children.Add(sfCircularGauge);
			label.SetValue(View.MarginProperty, new Thickness(0.0));
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = label;
			array13[1] = grid;
			array13[2] = this;
			object obj25;
			xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array13, Label.FontSizeProperty, nameScope));
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(GaugeItem).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(191, 17)));
			DynamicResource dynamicResource = markupExtension13.ProvideValue(xamlServiceProvider13);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension37.Mode = 2;
			bindingExtension37.Source = sharedSettings;
			bindingExtension37.Path = "ShowPing";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			label.SetBinding(VisualElement.IsVisibleProperty, bindingBase37);
			label.SetValue(Label.TextColorProperty, Color.Red);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "Model.Ping";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase38);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, "ms");
			formattedString.Spans.Add(span2);
			label.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "MinMaxAvgFontSize";
			bindingExtension39.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			label2.SetBinding(Label.FontSizeProperty, bindingBase39);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension40.Mode = 2;
			bindingExtension40.Path = "ShowMinMax";
			bindingExtension40.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase40);
			bindingExtension41.Mode = 2;
			bindingExtension41.Path = "MinMaxAvgColor";
			bindingExtension41.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			label2.SetBinding(Label.TextColorProperty, bindingBase41);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span3.SetValue(Span.TextProperty, "Max: ");
			formattedString2.Spans.Add(span3);
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "Model.MaximumAchievedText";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			span4.SetBinding(Span.TextProperty, bindingBase42);
			formattedString2.Spans.Add(span4);
			label2.SetValue(Label.FormattedTextProperty, formattedString2);
			grid.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension43.Mode = 2;
			bindingExtension43.Path = "MinMaxAvgFontSize";
			bindingExtension43.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase43);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension44.Mode = 2;
			bindingExtension44.Path = "ShowMinMax";
			bindingExtension44.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase44);
			bindingExtension45.Mode = 2;
			bindingExtension45.Path = "MinMaxAvgColor";
			bindingExtension45.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase45);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span5.SetValue(Span.TextProperty, "Min: ");
			formattedString3.Spans.Add(span5);
			bindingExtension46.Mode = 2;
			bindingExtension46.Path = "Model.MinimumAchievedText";
			bindingExtension46.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase46);
			formattedString3.Spans.Add(span6);
			label3.SetValue(Label.FormattedTextProperty, formattedString3);
			grid.Children.Add(label3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension47.Mode = 2;
			bindingExtension47.Path = "MinMaxAvgFontSize";
			bindingExtension47.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase47);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension48.Mode = 2;
			bindingExtension48.Path = "ShowAvg";
			bindingExtension48.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase48);
			bindingExtension49.Mode = 2;
			bindingExtension49.Path = "MinMaxAvgColor";
			bindingExtension49.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase49);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span7.SetValue(Span.TextProperty, "Avg: ");
			formattedString4.Spans.Add(span7);
			bindingExtension50.Mode = 2;
			bindingExtension50.Path = "Model.AverageTextValue";
			bindingExtension50.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase50 = bindingExtension50.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase50);
			formattedString4.Spans.Add(span8);
			label4.SetValue(Label.FormattedTextProperty, formattedString4);
			grid.Children.Add(label4);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x00349154 File Offset: 0x00347354
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<GaugeItem>(this, typeof(GaugeItem));
			this.HackyBindedKey = NameScopeExtensions.FindByName<ContentView>(this, "HackyBindedKey");
			this.circulargauge = NameScopeExtensions.FindByName<SfCircularGauge>(this, "circulargauge");
			this.gaugeHeaderValue = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderValue");
			this.gaugeHeaderTitle = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderTitle");
			this.gaugeHeaderUnits = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderUnits");
			this.gaugeScale = NameScopeExtensions.FindByName<Scale>(this, "gaugeScale");
			this.gaugeTick = NameScopeExtensions.FindByName<TickSettings>(this, "gaugeTick");
			this.gaugeSmallTick = NameScopeExtensions.FindByName<TickSettings>(this, "gaugeSmallTick");
			this.gaugeBlueLineRange = NameScopeExtensions.FindByName<Range>(this, "gaugeBlueLineRange");
			this.gaugeRedLineRange = NameScopeExtensions.FindByName<Range>(this, "gaugeRedLineRange");
			this.gaugePointer = NameScopeExtensions.FindByName<NeedlePointer>(this, "gaugePointer");
			this.minPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "minPointer");
			this.maxPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "maxPointer");
		}

		// Token: 0x0600438E RID: 17294 RVA: 0x00349250 File Offset: 0x00347450
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__400(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x00349280 File Offset: 0x00347480
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__401(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0034929C File Offset: 0x0034749C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__402(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004391 RID: 17297 RVA: 0x003492AC File Offset: 0x003474AC
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__403(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x003492DC File Offset: 0x003474DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__404(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x003492EC File Offset: 0x003474EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__405(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0034931C File Offset: 0x0034751C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__406(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0034932C File Offset: 0x0034752C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__407(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0034935C File Offset: 0x0034755C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__408(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0034936C File Offset: 0x0034756C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__409(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0034939C File Offset: 0x0034759C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__410(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x003493AC File Offset: 0x003475AC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__411(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x003493DC File Offset: 0x003475DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__413(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x003493EC File Offset: 0x003475EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__414(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0034941C File Offset: 0x0034761C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__415(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600439D RID: 17309 RVA: 0x0034942C File Offset: 0x0034762C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__416(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeLabelNumberOfDecimalDigits, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0034945C File Offset: 0x0034765C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__417(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0034946C File Offset: 0x0034766C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__418(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043A0 RID: 17312 RVA: 0x0034949C File Offset: 0x0034769C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__419(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043A1 RID: 17313 RVA: 0x003494AC File Offset: 0x003476AC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__420(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043A2 RID: 17314 RVA: 0x003494DC File Offset: 0x003476DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__421(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043A3 RID: 17315 RVA: 0x003494EC File Offset: 0x003476EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__422(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043A4 RID: 17316 RVA: 0x0034951C File Offset: 0x0034771C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__423(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x0034952C File Offset: 0x0034772C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__424(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x0034955C File Offset: 0x0034775C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__425(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043A7 RID: 17319 RVA: 0x0034956C File Offset: 0x0034776C
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__426(DashboardItem A_0)
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

		// Token: 0x060043A8 RID: 17320 RVA: 0x003495A4 File Offset: 0x003477A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__427(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x003495B4 File Offset: 0x003477B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__428(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x003495C8 File Offset: 0x003477C8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__429(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x003495F8 File Offset: 0x003477F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__430(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043AC RID: 17324 RVA: 0x00349608 File Offset: 0x00347808
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__431(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043AD RID: 17325 RVA: 0x00349638 File Offset: 0x00347838
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__432(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043AE RID: 17326 RVA: 0x00349648 File Offset: 0x00347848
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__433(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043AF RID: 17327 RVA: 0x00349678 File Offset: 0x00347878
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__434(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043B0 RID: 17328 RVA: 0x00349688 File Offset: 0x00347888
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__435(DashboardItem A_0)
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

		// Token: 0x060043B1 RID: 17329 RVA: 0x003496C0 File Offset: 0x003478C0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__436(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043B2 RID: 17330 RVA: 0x003496D0 File Offset: 0x003478D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__437(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043B3 RID: 17331 RVA: 0x003496E4 File Offset: 0x003478E4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__438(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043B4 RID: 17332 RVA: 0x00349714 File Offset: 0x00347914
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__439(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043B5 RID: 17333 RVA: 0x00349724 File Offset: 0x00347924
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__440(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043B6 RID: 17334 RVA: 0x00349754 File Offset: 0x00347954
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__441(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043B7 RID: 17335 RVA: 0x00349764 File Offset: 0x00347964
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__442(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043B8 RID: 17336 RVA: 0x00349794 File Offset: 0x00347994
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__443(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043B9 RID: 17337 RVA: 0x003497A4 File Offset: 0x003479A4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__444(DashboardItem A_0)
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

		// Token: 0x060043BA RID: 17338 RVA: 0x003497DC File Offset: 0x003479DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__445(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043BB RID: 17339 RVA: 0x003497EC File Offset: 0x003479EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__446(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060043BC RID: 17340 RVA: 0x00349800 File Offset: 0x00347A00
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__447(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060043BD RID: 17341 RVA: 0x00349830 File Offset: 0x00347A30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__448(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043BE RID: 17342 RVA: 0x00349840 File Offset: 0x00347A40
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__449(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x00349870 File Offset: 0x00347A70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__450(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043C0 RID: 17344 RVA: 0x00349880 File Offset: 0x00347A80
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__451(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x003498B0 File Offset: 0x00347AB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__452(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043C2 RID: 17346 RVA: 0x003498C0 File Offset: 0x00347AC0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__453(DashboardItem A_0)
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

		// Token: 0x060043C3 RID: 17347 RVA: 0x003498F8 File Offset: 0x00347AF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__454(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060043C4 RID: 17348 RVA: 0x00349908 File Offset: 0x00347B08
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__455(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0400289E RID: 10398
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView HackyBindedKey;

		// Token: 0x0400289F RID: 10399
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfCircularGauge circulargauge;

		// Token: 0x040028A0 RID: 10400
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderValue;

		// Token: 0x040028A1 RID: 10401
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderTitle;

		// Token: 0x040028A2 RID: 10402
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderUnits;

		// Token: 0x040028A3 RID: 10403
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Scale gaugeScale;

		// Token: 0x040028A4 RID: 10404
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private TickSettings gaugeTick;

		// Token: 0x040028A5 RID: 10405
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private TickSettings gaugeSmallTick;

		// Token: 0x040028A6 RID: 10406
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Range gaugeBlueLineRange;

		// Token: 0x040028A7 RID: 10407
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Range gaugeRedLineRange;

		// Token: 0x040028A8 RID: 10408
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NeedlePointer gaugePointer;

		// Token: 0x040028A9 RID: 10409
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer minPointer;

		// Token: 0x040028AA RID: 10410
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer maxPointer;

		// Token: 0x020007AC RID: 1964
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Gauge_SizeChanged>d__2 : IAsyncStateMachine
		{
			// Token: 0x060043C5 RID: 17349 RVA: 0x0034991C File Offset: 0x00347B1C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				GaugeItem gaugeItem = this;
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
							goto IL_03B4;
						}
						if (Device.RuntimePlatform == "iOS")
						{
							double num3 = ((gaugeItem.Width < gaugeItem.Height) ? gaugeItem.Width : gaugeItem.Height) / 150.0;
							gaugeItem.gaugeScale.LabelFontSize = 11.0 * num3;
							gaugeItem.gaugeRedLineRange.Thickness = gaugeItem.gaugeTick.Length;
							gaugeItem.gaugePointer.Thickness = 3.0 * num3;
							gaugeItem.gaugePointer.KnobRadius = 6.0 * num3;
							gaugeItem.gaugeScale.RimThickness = 5.0 * num3;
							gaugeItem.gaugeRedLineRange.Thickness = gaugeItem.gaugeTick.Length;
							goto IL_040D;
						}
						if (PlatformHelper.IsAndroid)
						{
							double num4 = ((gaugeItem.Width < gaugeItem.Height) ? gaugeItem.Width : gaugeItem.Height) / 250.0;
							gaugeItem.gaugeScale.LabelFontSize = 20.0 * num4;
							gaugeItem.gaugeScale.LabelOffset = 0.7;
							gaugeItem.gaugePointer.Thickness = 5.0 * num4;
							gaugeItem.gaugePointer.KnobRadius = 10.0 * num4;
							gaugeItem.gaugePointer.Type = 1;
							gaugeItem.gaugePointer.LengthFactor = 0.9;
							gaugeItem.gaugePointer.TailLengthFactor = 0.15;
							gaugeItem.gaugeScale.RimThickness = 5.0 * num4;
							gaugeItem.gaugeTick.Length = 12.0 * num4;
							gaugeItem.gaugeTick.Thickness = 3.0 * num4;
							gaugeItem.gaugeSmallTick.Length = 6.0 * num4;
							gaugeItem.gaugeSmallTick.Thickness = 1.5 * num4;
							gaugeItem.gaugeBlueLineRange.Offset = 0.98;
							gaugeItem.gaugeRedLineRange.Offset = 0.98;
							gaugeItem.gaugePointer.EnableAnimation = true;
							gaugeItem.minPointer.MarkerWidth = 12.0 * num4;
							gaugeItem.minPointer.MarkerHeight = 12.0 * num4;
							gaugeItem.maxPointer.MarkerWidth = 12.0 * num4;
							gaugeItem.maxPointer.MarkerHeight = 12.0 * num4;
						}
						gaugeItem.gaugeRedLineRange.Thickness = gaugeItem.gaugeTick.Length;
						gaugeItem.gaugeBlueLineRange.Thickness = gaugeItem.gaugeTick.Length;
						if (!PlatformHelper.IsAndroid)
						{
							gaugeItem.gaugePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							goto IL_03F2;
						}
						taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItem.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
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
					gaugeItem.gaugePointer.EnableAnimation = false;
					if (!SharedSettings.Current.DashboardCircularGaugeAnimation)
					{
						goto IL_03F2;
					}
					taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItem.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_03B4:
					taskAwaiter.GetResult();
					gaugeItem.gaugePointer.EnableAnimation = true;
					gaugeItem.gaugePointer.AnimationDuration = 0.35;
					IL_03F2:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_040D:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060043C6 RID: 17350 RVA: 0x00349D68 File Offset: 0x00347F68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040028AB RID: 10411
			public int <>1__state;

			// Token: 0x040028AC RID: 10412
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040028AD RID: 10413
			public GaugeItem <>4__this;

			// Token: 0x040028AE RID: 10414
			private TaskAwaiter <>u__1;
		}
	}
}
