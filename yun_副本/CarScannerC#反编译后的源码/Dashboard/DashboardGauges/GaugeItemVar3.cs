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
	// Token: 0x020007AF RID: 1967
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml")]
	public class GaugeItemVar3 : ContentView
	{
		// Token: 0x06004403 RID: 17411 RVA: 0x0034E704 File Offset: 0x0034C904
		public GaugeItemVar3()
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

		// Token: 0x06004404 RID: 17412 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Frame_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06004405 RID: 17413 RVA: 0x0034E770 File Offset: 0x0034C970
		private async void Gauge_SizeChanged(object sender, EventArgs e)
		{
			if (PlatformHelper.IsiOS)
			{
				double num = ((base.Width < base.Height) ? base.Width : base.Height) / 150.0;
				this.gaugeScale.LabelFontSize = 11.0 * num;
				this.gaugeScale.RimThickness = 5.0 * num;
				this.markerPointer.MarkerWidth = 6.0 * num;
				this.markerPointer.MarkerHeight = 40.0 * num;
			}
			else
			{
				if (PlatformHelper.IsAndroid)
				{
					double num2 = ((base.Width < base.Height) ? base.Width : base.Height) / 250.0;
					this.markerPointer.EnableAnimation = true;
					this.rangePointer.EnableAnimation = true;
					this.markerPointer.MarkerWidth = 6.0 * num2;
					this.markerPointer.MarkerHeight = 50.0 * num2;
					this.minPointer.MarkerWidth = 12.0 * num2;
					this.minPointer.MarkerHeight = 12.0 * num2;
					this.maxPointer.MarkerWidth = 12.0 * num2;
					this.maxPointer.MarkerHeight = 12.0 * num2;
					this.circulargauge.Margin = new Thickness(0.0, 0.0, 0.0, this.circulargauge.Height / -4.0);
				}
				if (PlatformHelper.IsAndroid)
				{
					await Task.Delay(TimeSpan.FromSeconds(2.0));
					this.markerPointer.EnableAnimation = false;
					this.rangePointer.EnableAnimation = false;
					if (SharedSettings.Current.DashboardCircularGaugeAnimation)
					{
						await Task.Delay(TimeSpan.FromSeconds(2.0));
						this.markerPointer.EnableAnimation = true;
						this.rangePointer.EnableAnimation = true;
						this.rangePointer.AnimationDuration = 0.1;
						this.markerPointer.AnimationDuration = 0.1;
					}
				}
				else
				{
					this.rangePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
					this.markerPointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
				}
			}
		}

		// Token: 0x06004406 RID: 17414 RVA: 0x0034E7A8 File Offset: 0x0034C9A8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(GaugeItemVar3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/GaugeItemVar3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			LimitValueToVisibleMinimumAndMaximumConverter limitValueToVisibleMinimumAndMaximumConverter;
			VisualDiagnostics.RegisterSourceInfo(limitValueToVisibleMinimumAndMaximumConverter = new LimitValueToVisibleMinimumAndMaximumConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			GaugeMinMaxPointersColorConverter gaugeMinMaxPointersColorConverter;
			VisualDiagnostics.RegisterSourceInfo(gaugeMinMaxPointersColorConverter = new GaugeMinMaxPointersColorConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart excludeInfinityFromDoubleConverterAndLimitToRedLineStart;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverterAndLimitToRedLineStart = new ExcludeInfinityFromDoubleConverterAndLimitToRedLineStart(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish excludeInfinityFromDoubleConverterAndLimitToBlueLineFinish;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverterAndLimitToBlueLineFinish = new ExcludeInfinityFromDoubleConverterAndLimitToBlueLineFinish(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 36);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			Header header;
			VisualDiagnostics.RegisterSourceInfo(header = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 25);
			Header header2;
			VisualDiagnostics.RegisterSourceInfo(header2 = new Header(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 25);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 33);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 33);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			RangePointer rangePointer;
			VisualDiagnostics.RegisterSourceInfo(rangePointer = new RangePointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 30);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
			MarkerPointer markerPointer;
			VisualDiagnostics.RegisterSourceInfo(markerPointer = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 33);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 33);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 33);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 33);
			ReferenceExtension referenceExtension4;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension4 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 33);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 33);
			MarkerPointer markerPointer2;
			VisualDiagnostics.RegisterSourceInfo(markerPointer2 = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 30);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 33);
			ReferenceExtension referenceExtension5;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension5 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 33);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 33);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 33);
			ReferenceExtension referenceExtension6;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension6 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 33);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 33);
			MarkerPointer markerPointer3;
			VisualDiagnostics.RegisterSourceInfo(markerPointer3 = new MarkerPointer(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 30);
			Scale scale;
			VisualDiagnostics.RegisterSourceInfo(scale = new Scale(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 22);
			SfCircularGauge sfCircularGauge;
			VisualDiagnostics.RegisterSourceInfo(sfCircularGauge = new SfCircularGauge(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 17);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 17);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 17);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 17);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 31);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 26);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 14);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 17);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 17);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 17);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 26);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 31);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 14);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 17);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 17);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 17);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 26);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 14);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 17);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 17);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 26);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 278, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\GaugeItemVar3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("gaugeScale", scale);
			if (scale.StyleId == null)
			{
				scale.StyleId = "gaugeScale";
			}
			nameScope.RegisterName("rangePointer", rangePointer);
			if (rangePointer.StyleId == null)
			{
				rangePointer.StyleId = "rangePointer";
			}
			nameScope.RegisterName("markerPointer", markerPointer);
			if (markerPointer.StyleId == null)
			{
				markerPointer.StyleId = "markerPointer";
			}
			nameScope.RegisterName("minPointer", markerPointer2);
			if (markerPointer2.StyleId == null)
			{
				markerPointer2.StyleId = "minPointer";
			}
			nameScope.RegisterName("maxPointer", markerPointer3);
			if (markerPointer3.StyleId == null)
			{
				markerPointer3.StyleId = "maxPointer";
			}
			this.HackyBindedKey = this;
			this.circulargauge = sfCircularGauge;
			this.gaugeHeaderValue = header;
			this.gaugeHeaderTitle = header2;
			this.gaugeScale = scale;
			this.rangePointer = rangePointer;
			this.markerPointer = markerPointer;
			this.minPointer = markerPointer2;
			this.maxPointer = markerPointer3;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 17)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
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
			sfCircularGauge.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			sfCircularGauge.SizeChanged += this.Gauge_SizeChanged;
			sfCircularGauge.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "FontName";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			header.SetBinding(Header.FontFamilyProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "ValueTextColor";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			header.SetBinding(Header.ForegroundColorProperty, bindingBase6);
			header.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.35"));
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
			header2.SetValue(Header.PositionProperty, new PointTypeConverter().ConvertFromInvariantString("0.5,0.50"));
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
			bindingExtension12.Path = "Maximum";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			scale.SetBinding(Scale.EndValueProperty, bindingBase12);
			bindingExtension13.Path = "Interval";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			scale.SetBinding(Scale.IntervalProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "GaugeLabelColor";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			scale.SetBinding(Scale.LabelColorProperty, bindingBase14);
			scale.SetValue(Scale.MinorTicksPerIntervalProperty, 1.0);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "GaugeLabelNumberOfDecimalDigits";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			scale.SetBinding(Scale.NumberOfDecimalDigitsProperty, bindingBase15);
			scale.SetValue(Scale.ShowLabelsProperty, false);
			scale.SetValue(Scale.ShowRimProperty, false);
			scale.SetValue(Scale.ShowTicksProperty, false);
			scale.SetValue(Scale.StartAngleProperty, 180.0);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "Minimum";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			scale.SetBinding(Scale.StartValueProperty, bindingBase16);
			scale.SetValue(Scale.SweepAngleProperty, 180.0);
			rangePointer.SetValue(Pointer.EnableAnimationProperty, true);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "CiruclarGaugeWidth";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			rangePointer.SetBinding(RangePointer.ThicknessProperty, bindingBase17);
			rangePointer.SetValue(RangePointer.OffsetProperty, 1.0);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "GaugePointerColor";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			rangePointer.SetBinding(Pointer.ColorProperty, bindingBase18);
			bindingExtension19.Mode = 2;
			staticResourceExtension3.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 6];
			array3[0] = bindingExtension19;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 33)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension19.Converter = obj6;
			referenceExtension.Name = "HackyBindedKey";
			IMarkupExtension markupExtension4 = referenceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension19;
			array4[1] = rangePointer;
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 33)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension19.ConverterParameter = obj8;
			bindingExtension19.Path = "Model.FloatValue";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			rangePointer.SetBinding(Pointer.ValueProperty, bindingBase19);
			scale.GetValue(Scale.PointersProperty).Add(rangePointer);
			markerPointer.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer.SetValue(MarkerPointer.MarkerHeightProperty, 80.0);
			markerPointer.SetValue(MarkerPointer.MarkerShapeProperty, 5);
			markerPointer.SetValue(MarkerPointer.MarkerWidthProperty, 8.0);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "GaugePointerColor";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ColorProperty, bindingBase20);
			bindingExtension21.Mode = 2;
			staticResourceExtension4.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension21;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension21.Converter = obj10;
			referenceExtension2.Name = "HackyBindedKey";
			IMarkupExtension markupExtension6 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension21;
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 33)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension21.ConverterParameter = obj12;
			bindingExtension21.Path = "Model.FloatValue";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			markerPointer.SetBinding(Pointer.ValueProperty, bindingBase21);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer);
			markerPointer2.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer2.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension22.Mode = 2;
			staticResourceExtension5.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension22;
			array7[1] = markerPointer2;
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 33)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension22.Converter = obj14;
			referenceExtension3.Name = "HackyBindedKey";
			IMarkupExtension markupExtension8 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension22;
			array8[1] = markerPointer2;
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 33)));
			object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension22.ConverterParameter = obj16;
			bindingExtension22.Path = "MinMaxPointersColor";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ColorProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			staticResourceExtension6.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension23;
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 33)));
			object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension23.Converter = obj18;
			referenceExtension4.Name = "HackyBindedKey";
			IMarkupExtension markupExtension10 = referenceExtension4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension23;
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 33)));
			object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension23.ConverterParameter = obj20;
			bindingExtension23.Path = "Model.MinimumAchieved";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			markerPointer2.SetBinding(Pointer.ValueProperty, bindingBase23);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer2);
			markerPointer3.SetValue(Pointer.AnimationDurationProperty, 0.1);
			markerPointer3.SetValue(MarkerPointer.MarkerShapeProperty, 4);
			bindingExtension24.Mode = 2;
			staticResourceExtension7.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension24;
			array11[1] = markerPointer3;
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 33)));
			object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension24.Converter = obj22;
			referenceExtension5.Name = "HackyBindedKey";
			IMarkupExtension markupExtension12 = referenceExtension5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = bindingExtension24;
			array12[1] = markerPointer3;
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 33)));
			object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension24.ConverterParameter = obj24;
			bindingExtension24.Path = "MinMaxPointersColor";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			markerPointer3.SetBinding(Pointer.ColorProperty, bindingBase24);
			bindingExtension25.Mode = 2;
			staticResourceExtension8.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension25;
			array13[1] = markerPointer3;
			array13[2] = scale;
			array13[3] = sfCircularGauge;
			array13[4] = grid;
			array13[5] = this;
			object obj25;
			xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 33)));
			object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension25.Converter = obj26;
			referenceExtension6.Name = "HackyBindedKey";
			IMarkupExtension markupExtension14 = referenceExtension6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = bindingExtension25;
			array14[1] = markerPointer3;
			array14[2] = scale;
			array14[3] = sfCircularGauge;
			array14[4] = grid;
			array14[5] = this;
			object obj27;
			xamlServiceProvider14.Add(typeFromHandle27, obj27 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
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
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 33)));
			object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension25.ConverterParameter = obj28;
			bindingExtension25.Path = "Model.MaximumAchieved";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			markerPointer3.SetBinding(Pointer.ValueProperty, bindingBase25);
			scale.GetValue(Scale.PointersProperty).Add(markerPointer3);
			sfCircularGauge.GetValue(SfCircularGauge.ScalesProperty).Add(scale);
			grid.Children.Add(sfCircularGauge);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "UnitsFontSize";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsFontSize")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label.SetBinding(Label.FontSizeProperty, bindingBase26);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "Model.Units";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<string, bool>(model.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase27);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "UnitsTextColor";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsTextColor")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			label.SetBinding(Label.TextColorProperty, bindingBase28);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			grid.Children.Add(label);
			label2.SetValue(View.MarginProperty, new Thickness(0.0));
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = label2;
			array15[1] = grid;
			array15[2] = this;
			object obj29;
			xamlServiceProvider15.Add(typeFromHandle29, obj29 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
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
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(GaugeItemVar3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(220, 17)));
			DynamicResource dynamicResource = markupExtension15.ProvideValue(xamlServiceProvider15);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension29.Mode = 2;
			bindingExtension29.Source = sharedSettings;
			bindingExtension29.Path = "ShowPing";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase29);
			label2.SetValue(Label.TextColorProperty, Color.Red);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "Model.Ping";
			bindingExtension30.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						return new ValueTuple<long, bool>(model2.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase30);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, "ms");
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "MinMaxAvgFontSize";
			bindingExtension31.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase31);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension32.Mode = 2;
			bindingExtension32.Path = "ShowMinMax";
			bindingExtension32.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase32);
			bindingExtension33.Mode = 2;
			bindingExtension33.Path = "MinMaxAvgColor";
			bindingExtension33.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase33);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span3.SetValue(Span.TextProperty, "Max: ");
			formattedString2.Spans.Add(span3);
			bindingExtension34.Mode = 2;
			bindingExtension34.Path = "Model.MaximumAchievedText";
			bindingExtension34.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<string, bool>(model3.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			span4.SetBinding(Span.TextProperty, bindingBase34);
			formattedString2.Spans.Add(span4);
			label3.SetValue(Label.FormattedTextProperty, formattedString2);
			grid.Children.Add(label3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension35.Mode = 2;
			bindingExtension35.Path = "MinMaxAvgFontSize";
			bindingExtension35.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase35);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension36.Mode = 2;
			bindingExtension36.Path = "ShowMinMax";
			bindingExtension36.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase36);
			bindingExtension37.Mode = 2;
			bindingExtension37.Path = "MinMaxAvgColor";
			bindingExtension37.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase37);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span5.SetValue(Span.TextProperty, "Min: ");
			formattedString3.Spans.Add(span5);
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "Model.MinimumAchievedText";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase38);
			formattedString3.Spans.Add(span6);
			label4.SetValue(Label.FormattedTextProperty, formattedString3);
			grid.Children.Add(label4);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
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
			label5.SetBinding(Label.FontSizeProperty, bindingBase39);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension40.Mode = 2;
			bindingExtension40.Path = "ShowAvg";
			bindingExtension40.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase40);
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
			label5.SetBinding(Label.TextColorProperty, bindingBase41);
			label5.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span7.SetValue(Span.TextProperty, "Avg: ");
			formattedString4.Spans.Add(span7);
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "Model.AverageTextValue";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						return new ValueTuple<string, bool>(model5.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase42);
			formattedString4.Spans.Add(span8);
			label5.SetValue(Label.FormattedTextProperty, formattedString4);
			grid.Children.Add(label5);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x06004407 RID: 17415 RVA: 0x00352204 File Offset: 0x00350404
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<GaugeItemVar3>(this, typeof(GaugeItemVar3));
			this.HackyBindedKey = NameScopeExtensions.FindByName<ContentView>(this, "HackyBindedKey");
			this.circulargauge = NameScopeExtensions.FindByName<SfCircularGauge>(this, "circulargauge");
			this.gaugeHeaderValue = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderValue");
			this.gaugeHeaderTitle = NameScopeExtensions.FindByName<Header>(this, "gaugeHeaderTitle");
			this.gaugeScale = NameScopeExtensions.FindByName<Scale>(this, "gaugeScale");
			this.rangePointer = NameScopeExtensions.FindByName<RangePointer>(this, "rangePointer");
			this.markerPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "markerPointer");
			this.minPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "minPointer");
			this.maxPointer = NameScopeExtensions.FindByName<MarkerPointer>(this, "maxPointer");
		}

		// Token: 0x06004408 RID: 17416 RVA: 0x003522BC File Offset: 0x003504BC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__510(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004409 RID: 17417 RVA: 0x003522EC File Offset: 0x003504EC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__511(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x0600440A RID: 17418 RVA: 0x00352308 File Offset: 0x00350508
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__512(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600440B RID: 17419 RVA: 0x00352318 File Offset: 0x00350518
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__513(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x0600440C RID: 17420 RVA: 0x00352348 File Offset: 0x00350548
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__514(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600440D RID: 17421 RVA: 0x00352358 File Offset: 0x00350558
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__515(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600440E RID: 17422 RVA: 0x00352388 File Offset: 0x00350588
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__516(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600440F RID: 17423 RVA: 0x00352398 File Offset: 0x00350598
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__517(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004410 RID: 17424 RVA: 0x003523C8 File Offset: 0x003505C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__518(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004411 RID: 17425 RVA: 0x003523D8 File Offset: 0x003505D8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__519(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004412 RID: 17426 RVA: 0x00352408 File Offset: 0x00350608
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__520(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004413 RID: 17427 RVA: 0x00352418 File Offset: 0x00350618
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__521(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004414 RID: 17428 RVA: 0x00352448 File Offset: 0x00350648
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__523(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004415 RID: 17429 RVA: 0x00352458 File Offset: 0x00350658
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__524(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004416 RID: 17430 RVA: 0x00352488 File Offset: 0x00350688
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__525(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004417 RID: 17431 RVA: 0x00352498 File Offset: 0x00350698
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__526(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeLabelNumberOfDecimalDigits, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004418 RID: 17432 RVA: 0x003524C8 File Offset: 0x003506C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__527(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004419 RID: 17433 RVA: 0x003524D8 File Offset: 0x003506D8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__528(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600441A RID: 17434 RVA: 0x00352508 File Offset: 0x00350708
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__529(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600441B RID: 17435 RVA: 0x00352518 File Offset: 0x00350718
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__530(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600441C RID: 17436 RVA: 0x00352548 File Offset: 0x00350748
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__531(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x00352558 File Offset: 0x00350758
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__532(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.Units, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x00352590 File Offset: 0x00350790
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__533(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600441F RID: 17439 RVA: 0x003525A0 File Offset: 0x003507A0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__534(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004420 RID: 17440 RVA: 0x003525B4 File Offset: 0x003507B4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__535(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004421 RID: 17441 RVA: 0x003525E4 File Offset: 0x003507E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__536(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x003525F4 File Offset: 0x003507F4
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__537(DashboardItem A_0)
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

		// Token: 0x06004423 RID: 17443 RVA: 0x0035262C File Offset: 0x0035082C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__538(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0035263C File Offset: 0x0035083C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__539(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x00352650 File Offset: 0x00350850
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__540(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004426 RID: 17446 RVA: 0x00352680 File Offset: 0x00350880
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__541(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004427 RID: 17447 RVA: 0x00352690 File Offset: 0x00350890
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__542(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004428 RID: 17448 RVA: 0x003526C0 File Offset: 0x003508C0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__543(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004429 RID: 17449 RVA: 0x003526D0 File Offset: 0x003508D0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__544(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600442A RID: 17450 RVA: 0x00352700 File Offset: 0x00350900
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__545(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600442B RID: 17451 RVA: 0x00352710 File Offset: 0x00350910
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__546(DashboardItem A_0)
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

		// Token: 0x0600442C RID: 17452 RVA: 0x00352748 File Offset: 0x00350948
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__547(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600442D RID: 17453 RVA: 0x00352758 File Offset: 0x00350958
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__548(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600442E RID: 17454 RVA: 0x0035276C File Offset: 0x0035096C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__549(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600442F RID: 17455 RVA: 0x0035279C File Offset: 0x0035099C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__550(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004430 RID: 17456 RVA: 0x003527AC File Offset: 0x003509AC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__551(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004431 RID: 17457 RVA: 0x003527DC File Offset: 0x003509DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__552(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004432 RID: 17458 RVA: 0x003527EC File Offset: 0x003509EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__553(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004433 RID: 17459 RVA: 0x0035281C File Offset: 0x00350A1C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__554(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004434 RID: 17460 RVA: 0x0035282C File Offset: 0x00350A2C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__555(DashboardItem A_0)
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

		// Token: 0x06004435 RID: 17461 RVA: 0x00352864 File Offset: 0x00350A64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__556(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004436 RID: 17462 RVA: 0x00352874 File Offset: 0x00350A74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__557(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004437 RID: 17463 RVA: 0x00352888 File Offset: 0x00350A88
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__558(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004438 RID: 17464 RVA: 0x003528B8 File Offset: 0x00350AB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__559(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004439 RID: 17465 RVA: 0x003528C8 File Offset: 0x00350AC8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__560(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600443A RID: 17466 RVA: 0x003528F8 File Offset: 0x00350AF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__561(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600443B RID: 17467 RVA: 0x00352908 File Offset: 0x00350B08
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__562(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600443C RID: 17468 RVA: 0x00352938 File Offset: 0x00350B38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__563(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600443D RID: 17469 RVA: 0x00352948 File Offset: 0x00350B48
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__564(DashboardItem A_0)
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

		// Token: 0x0600443E RID: 17470 RVA: 0x00352980 File Offset: 0x00350B80
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__565(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600443F RID: 17471 RVA: 0x00352990 File Offset: 0x00350B90
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__566(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028BE RID: 10430
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView HackyBindedKey;

		// Token: 0x040028BF RID: 10431
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfCircularGauge circulargauge;

		// Token: 0x040028C0 RID: 10432
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderValue;

		// Token: 0x040028C1 RID: 10433
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Header gaugeHeaderTitle;

		// Token: 0x040028C2 RID: 10434
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Scale gaugeScale;

		// Token: 0x040028C3 RID: 10435
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RangePointer rangePointer;

		// Token: 0x040028C4 RID: 10436
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer markerPointer;

		// Token: 0x040028C5 RID: 10437
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer minPointer;

		// Token: 0x040028C6 RID: 10438
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private MarkerPointer maxPointer;

		// Token: 0x020007B0 RID: 1968
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Gauge_SizeChanged>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004440 RID: 17472 RVA: 0x003529A4 File Offset: 0x00350BA4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				GaugeItemVar3 gaugeItemVar = this;
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
							goto IL_02B6;
						}
						if (PlatformHelper.IsiOS)
						{
							double num3 = ((gaugeItemVar.Width < gaugeItemVar.Height) ? gaugeItemVar.Width : gaugeItemVar.Height) / 150.0;
							gaugeItemVar.gaugeScale.LabelFontSize = 11.0 * num3;
							gaugeItemVar.gaugeScale.RimThickness = 5.0 * num3;
							gaugeItemVar.markerPointer.MarkerWidth = 6.0 * num3;
							gaugeItemVar.markerPointer.MarkerHeight = 40.0 * num3;
							goto IL_0344;
						}
						if (PlatformHelper.IsAndroid)
						{
							double num4 = ((gaugeItemVar.Width < gaugeItemVar.Height) ? gaugeItemVar.Width : gaugeItemVar.Height) / 250.0;
							gaugeItemVar.markerPointer.EnableAnimation = true;
							gaugeItemVar.rangePointer.EnableAnimation = true;
							gaugeItemVar.markerPointer.MarkerWidth = 6.0 * num4;
							gaugeItemVar.markerPointer.MarkerHeight = 50.0 * num4;
							gaugeItemVar.minPointer.MarkerWidth = 12.0 * num4;
							gaugeItemVar.minPointer.MarkerHeight = 12.0 * num4;
							gaugeItemVar.maxPointer.MarkerWidth = 12.0 * num4;
							gaugeItemVar.maxPointer.MarkerHeight = 12.0 * num4;
							gaugeItemVar.circulargauge.Margin = new Thickness(0.0, 0.0, 0.0, gaugeItemVar.circulargauge.Height / -4.0);
						}
						if (!PlatformHelper.IsAndroid)
						{
							gaugeItemVar.rangePointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							gaugeItemVar.markerPointer.EnableAnimation = SharedSettings.Current.DashboardCircularGaugeAnimation;
							goto IL_0329;
						}
						taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItemVar3.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
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
					gaugeItemVar.markerPointer.EnableAnimation = false;
					gaugeItemVar.rangePointer.EnableAnimation = false;
					if (!SharedSettings.Current.DashboardCircularGaugeAnimation)
					{
						goto IL_0329;
					}
					taskAwaiter = Task.Delay(TimeSpan.FromSeconds(2.0)).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GaugeItemVar3.<Gauge_SizeChanged>d__2>(ref taskAwaiter, ref this);
						return;
					}
					IL_02B6:
					taskAwaiter.GetResult();
					gaugeItemVar.markerPointer.EnableAnimation = true;
					gaugeItemVar.rangePointer.EnableAnimation = true;
					gaugeItemVar.rangePointer.AnimationDuration = 0.1;
					gaugeItemVar.markerPointer.AnimationDuration = 0.1;
					IL_0329:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0344:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004441 RID: 17473 RVA: 0x00352D24 File Offset: 0x00350F24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040028C7 RID: 10439
			public int <>1__state;

			// Token: 0x040028C8 RID: 10440
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040028C9 RID: 10441
			public GaugeItemVar3 <>4__this;

			// Token: 0x040028CA RID: 10442
			private TaskAwaiter <>u__1;
		}
	}
}
