using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
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
	// Token: 0x020007B2 RID: 1970
	[XamlFilePath("Dashboard\\DashboardGauges\\LinearProgressItem.xaml")]
	public class LinearProgressItem : ContentView
	{
		// Token: 0x06004471 RID: 17521 RVA: 0x00354DEB File Offset: 0x00352FEB
		public LinearProgressItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x06004472 RID: 17522 RVA: 0x00354DFC File Offset: 0x00352FFC
		private void SfLinearGauge_SizeChanged(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				double num = ((base.Width < base.Height) ? base.Width : base.Height) / 250.0;
				this.gaugeTick.Length = 8.0 * num;
				this.gaugeTick.Thickness = 2.0 * num;
				this.gaugeSmallTick.Length = 3.0 * num;
				this.gaugeSmallTick.Thickness = 1.0 * num;
			}
		}

		// Token: 0x06004473 RID: 17523 RVA: 0x00354E90 File Offset: 0x00353090
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LinearProgressItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/LinearProgressItem.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			DoubleToPercentConverter doubleToPercentConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToPercentConverter = new DoubleToPercentConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			FloatItemToLineBreakModeConverter floatItemToLineBreakModeConverter;
			VisualDiagnostics.RegisterSourceInfo(floatItemToLineBreakModeConverter = new FloatItemToLineBreakModeConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			BoolToSFGaugeOrientationConverter boolToSFGaugeOrientationConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToSFGaugeOrientationConverter = new BoolToSFGaugeOrientationConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			LimitValueToVisibleMinimumAndMaximumConverter limitValueToVisibleMinimumAndMaximumConverter;
			VisualDiagnostics.RegisterSourceInfo(limitValueToVisibleMinimumAndMaximumConverter = new LimitValueToVisibleMinimumAndMaximumConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			FalseToVerticalMarginConverter falseToVerticalMarginConverter;
			VisualDiagnostics.RegisterSourceInfo(falseToVerticalMarginConverter = new FalseToVerticalMarginConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			GaugeMinMaxPointersColorConverter gaugeMinMaxPointersColorConverter;
			VisualDiagnostics.RegisterSourceInfo(gaugeMinMaxPointersColorConverter = new GaugeMinMaxPointersColorConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 49);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 17);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 17);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 17);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 25);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 33);
			LinearTickSettings linearTickSettings;
			VisualDiagnostics.RegisterSourceInfo(linearTickSettings = new LinearTickSettings(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 33);
			LinearTickSettings linearTickSettings2;
			VisualDiagnostics.RegisterSourceInfo(linearTickSettings2 = new LinearTickSettings(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 30);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 33);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 33);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 33);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 41);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 50);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 50);
			OnPlatform<BindingMode> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<BindingMode>(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 46);
			double num = 75.0;
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 38);
			BarPointer barPointer;
			VisualDiagnostics.RegisterSourceInfo(barPointer = new BarPointer(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 30);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 33);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
			SymbolPointer symbolPointer;
			VisualDiagnostics.RegisterSourceInfo(symbolPointer = new SymbolPointer(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 30);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 33);
			ReferenceExtension referenceExtension4;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension4 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 33);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 33);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 33);
			ReferenceExtension referenceExtension5;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension5 = new ReferenceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 33);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 33);
			SymbolPointer symbolPointer2;
			VisualDiagnostics.RegisterSourceInfo(symbolPointer2 = new SymbolPointer(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
			LinearScale linearScale;
			VisualDiagnostics.RegisterSourceInfo(linearScale = new LinearScale(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 22);
			SfLinearGauge sfLinearGauge;
			VisualDiagnostics.RegisterSourceInfo(sfLinearGauge = new SfLinearGauge(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 14);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 17);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 17);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 17);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 29);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 29);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 29);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 29);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 26);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 29);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 29);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 29);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 17);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 17);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 31);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 26);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 14);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 17);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 17);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 17);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 26);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 14);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 17);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 265, 17);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 26);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 14);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 17);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 17);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 17);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 26);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 31);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 26);
			FormattedString formattedString5;
			VisualDiagnostics.RegisterSourceInfo(formattedString5 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 22);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\LinearProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("HackyBindedKey", this);
			if (this.StyleId == null)
			{
				this.StyleId = "HackyBindedKey";
			}
			nameScope.RegisterName("layoutGrid", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "layoutGrid";
			}
			nameScope.RegisterName("linearGauge", sfLinearGauge);
			if (sfLinearGauge.StyleId == null)
			{
				sfLinearGauge.StyleId = "linearGauge";
			}
			nameScope.RegisterName("gaugeScale", linearScale);
			if (linearScale.StyleId == null)
			{
				linearScale.StyleId = "gaugeScale";
			}
			nameScope.RegisterName("gaugeTick", linearTickSettings);
			if (linearTickSettings.StyleId == null)
			{
				linearTickSettings.StyleId = "gaugeTick";
			}
			nameScope.RegisterName("gaugeSmallTick", linearTickSettings2);
			if (linearTickSettings2.StyleId == null)
			{
				linearTickSettings2.StyleId = "gaugeSmallTick";
			}
			nameScope.RegisterName("gaugePointer", barPointer);
			if (barPointer.StyleId == null)
			{
				barPointer.StyleId = "gaugePointer";
			}
			this.HackyBindedKey = this;
			this.layoutGrid = grid;
			this.linearGauge = sfLinearGauge;
			this.gaugeScale = linearScale;
			this.gaugeTick = linearTickSettings;
			this.gaugeSmallTick = linearTickSettings2;
			this.gaugePointer = barPointer;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("DoubleToPercentConverter", doubleToPercentConverter);
			resourceDictionary.Add("FloatItemToLineBreakModeConverter", floatItemToLineBreakModeConverter);
			resourceDictionary.Add("BoolToSFGaugeOrientationConverter", boolToSFGaugeOrientationConverter);
			resourceDictionary.Add("LimitValueToVisibleMinimumAndMaximumConverter", limitValueToVisibleMinimumAndMaximumConverter);
			resourceDictionary.Add("FalseToVerticalMarginConverter", falseToVerticalMarginConverter);
			resourceDictionary.Add("GaugeMinMaxPointersColorConverter", gaugeMinMaxPointersColorConverter);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ShowDefaultBackground";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 17)));
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
			label.SetValue(Grid.RowProperty, 0);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "TitleFontSize";
			bindingExtension5.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleFontSize")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label.SetBinding(Label.FontSizeProperty, bindingBase5);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.MaxLinesProperty, 2);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "PIDName";
			bindingExtension6.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PIDName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PIDName")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase6);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "TitleTextColor";
			bindingExtension7.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleTextColor")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label.SetBinding(Label.TextColorProperty, bindingBase7);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			grid.Children.Add(label);
			sfLinearGauge.SetValue(Grid.RowProperty, 0);
			staticResourceExtension3.Key = "FalseToVerticalMarginConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = bindingExtension8;
			array3[1] = sfLinearGauge;
			array3[2] = grid;
			array3[3] = this;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 17)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension8.Converter = obj6;
			referenceExtension.Name = "linearGauge";
			IMarkupExtension markupExtension4 = referenceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = bindingExtension8;
			array4[1] = sfLinearGauge;
			array4[2] = grid;
			array4[3] = this;
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 17)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension8.ConverterParameter = obj8;
			bindingExtension8.Path = "LinearOrientationHorizontal";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LinearOrientationHorizontal = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearOrientationHorizontal")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			sfLinearGauge.SetBinding(View.MarginProperty, bindingBase8);
			staticResourceExtension4.Key = "BoolToSFGaugeOrientationConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = bindingExtension9;
			array5[1] = sfLinearGauge;
			array5[2] = grid;
			array5[3] = this;
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 17)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension9.Converter = obj10;
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "LinearOrientationHorizontal";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearOrientationHorizontal")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			sfLinearGauge.SetBinding(SfLinearGauge.OrientationProperty, bindingBase9);
			sfLinearGauge.SizeChanged += this.SfLinearGauge_SizeChanged;
			sfLinearGauge.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			bindingExtension10.Path = "Interval";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			linearScale.SetBinding(LinearScale.IntervalProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "GaugeLabelColor";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			linearScale.SetBinding(LinearScale.LabelColorProperty, bindingBase11);
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
			linearScale.SetBinding(LinearScale.MaximumValueProperty, bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "Minimum";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			linearScale.SetBinding(LinearScale.MinimumValueProperty, bindingBase13);
			linearScale.SetValue(LinearScale.MinorTicksPerIntervalProperty, 1.0);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "GaugeRimColor";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			linearScale.SetBinding(LinearScale.ScaleBarColorProperty, bindingBase14);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "LinearScaleSize";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearScaleSize")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			linearScale.SetBinding(LinearScale.ScaleBarSizeProperty, bindingBase15);
			linearScale.SetValue(LinearScale.ShowLabelsProperty, true);
			linearTickSettings.SetValue(LinearTickSettings.ThicknessProperty, 1.0);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "GaugeTickColor";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			linearTickSettings.SetBinding(LinearTickSettings.ColorProperty, bindingBase16);
			linearScale.SetValue(LinearScale.MajorTickSettingsProperty, linearTickSettings);
			linearTickSettings2.SetValue(LinearTickSettings.ThicknessProperty, 1.0);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "GaugeTickColor";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			linearTickSettings2.SetBinding(LinearTickSettings.ColorProperty, bindingBase17);
			linearScale.SetValue(LinearScale.MinorTickSettingsProperty, linearTickSettings2);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "GaugePointerColor";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			barPointer.SetBinding(LinearPointer.ColorProperty, bindingBase18);
			bindingExtension19.Mode = 2;
			staticResourceExtension5.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension19;
			array6[1] = barPointer;
			array6[2] = linearScale;
			array6[3] = sfLinearGauge;
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 33)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension19.Converter = obj12;
			bindingExtension19.Path = "Model.FloatValue";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			barPointer.SetBinding(LinearPointer.ValueProperty, bindingBase19);
			staticResourceExtension6.Key = "DoubleToPercentConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension20;
			array7[1] = barPointer;
			array7[2] = linearScale;
			array7[3] = sfLinearGauge;
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 41)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension20.Converter = obj14;
			bindingExtension20.Mode = 4;
			bindingExtension20.Path = "LinearScaleSize";
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "OneTime";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "OneWay";
			onPlatform.Platforms.Add(on2);
			bindingExtension20.Mode = onPlatform;
			bindingExtension20.ConverterParameter = num;
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			barPointer.SetBinding(LinearPointer.ThicknessProperty, bindingBase20);
			linearScale.GetValue(LinearScale.PointersProperty).Add(barPointer);
			symbolPointer.SetValue(LinearPointer.EnableAnimationProperty, true);
			bindingExtension21.Mode = 2;
			staticResourceExtension7.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension21;
			array8[1] = symbolPointer;
			array8[2] = linearScale;
			array8[3] = sfLinearGauge;
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 33)));
			object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension21.Converter = obj16;
			referenceExtension2.Name = "HackyBindedKey";
			IMarkupExtension markupExtension9 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension21;
			array9[1] = symbolPointer;
			array9[2] = linearScale;
			array9[3] = sfLinearGauge;
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 33)));
			object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension21.ConverterParameter = obj18;
			bindingExtension21.Path = "MinMaxPointersColor";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			symbolPointer.SetBinding(LinearPointer.ColorProperty, bindingBase21);
			bindingExtension22.Mode = 2;
			staticResourceExtension8.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension22;
			array10[1] = symbolPointer;
			array10[2] = linearScale;
			array10[3] = sfLinearGauge;
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension22.Converter = obj20;
			referenceExtension3.Name = "HackyBindedKey";
			IMarkupExtension markupExtension11 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension22;
			array11[1] = symbolPointer;
			array11[2] = linearScale;
			array11[3] = sfLinearGauge;
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
			object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension22.ConverterParameter = obj22;
			bindingExtension22.Path = "Model.MinimumAchieved";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			symbolPointer.SetBinding(LinearPointer.ValueProperty, bindingBase22);
			linearScale.GetValue(LinearScale.PointersProperty).Add(symbolPointer);
			symbolPointer2.SetValue(LinearPointer.EnableAnimationProperty, true);
			bindingExtension23.Mode = 2;
			staticResourceExtension9.Key = "GaugeMinMaxPointersColorConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = bindingExtension23;
			array12[1] = symbolPointer2;
			array12[2] = linearScale;
			array12[3] = sfLinearGauge;
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 33)));
			object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension23.Converter = obj24;
			referenceExtension4.Name = "HackyBindedKey";
			IMarkupExtension markupExtension13 = referenceExtension4;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension23;
			array13[1] = symbolPointer2;
			array13[2] = linearScale;
			array13[3] = sfLinearGauge;
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 33)));
			object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension23.ConverterParameter = obj26;
			bindingExtension23.Path = "MinMaxPointersColor";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			symbolPointer2.SetBinding(LinearPointer.ColorProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			staticResourceExtension10.Key = "LimitValueToVisibleMinimumAndMaximumConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = bindingExtension24;
			array14[1] = symbolPointer2;
			array14[2] = linearScale;
			array14[3] = sfLinearGauge;
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
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 33)));
			object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension24.Converter = obj28;
			referenceExtension5.Name = "HackyBindedKey";
			IMarkupExtension markupExtension15 = referenceExtension5;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = bindingExtension24;
			array15[1] = symbolPointer2;
			array15[2] = linearScale;
			array15[3] = sfLinearGauge;
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
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 33)));
			object obj30 = markupExtension15.ProvideValue(xamlServiceProvider15);
			bindingExtension24.ConverterParameter = obj30;
			bindingExtension24.Path = "Model.MaximumAchieved";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			symbolPointer2.SetBinding(LinearPointer.ValueProperty, bindingBase24);
			linearScale.GetValue(LinearScale.PointersProperty).Add(symbolPointer2);
			sfLinearGauge.GetValue(SfLinearGauge.ScalesProperty).Add(linearScale);
			grid.Children.Add(sfLinearGauge);
			label2.SetValue(Grid.RowProperty, 0);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "ShowValue";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowValue")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase25);
			staticResourceExtension11.Key = "FloatItemToLineBreakModeConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = bindingExtension26;
			array16[1] = label2;
			array16[2] = grid;
			array16[3] = this;
			object obj31;
			xamlServiceProvider16.Add(typeFromHandle31, obj31 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 17)));
			object obj32 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension26.Converter = obj32;
			bindingExtension26.Path = "Model.ChartVisible";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<bool, bool>(model.ChartVisible, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						model2.ChartVisible = A_1;
						return;
					}
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "ChartVisible")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label2.SetBinding(Label.LineBreakModeProperty, bindingBase26);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "FontName";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.FontName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FontName")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			span.SetBinding(Span.FontFamilyProperty, bindingBase27);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "ValueFontSize";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFontSize")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			span.SetBinding(Span.FontSizeProperty, bindingBase28);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "Model.TextValue";
			bindingExtension29.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<string, bool>(model3.TextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "TextValue")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase29);
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "ValueTextColor";
			bindingExtension30.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueTextColor")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			span.SetBinding(Span.TextColorProperty, bindingBase30);
			formattedString.Spans.Add(span);
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "UnitsFontSize";
			bindingExtension31.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			span2.SetBinding(Span.FontSizeProperty, bindingBase31);
			bindingExtension32.Mode = 2;
			bindingExtension32.Path = "Model.Units";
			bindingExtension32.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase32);
			bindingExtension33.Mode = 2;
			bindingExtension33.Path = "UnitsTextColor";
			bindingExtension33.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			span2.SetBinding(Span.TextColorProperty, bindingBase33);
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label2);
			label3.SetValue(View.MarginProperty, new Thickness(0.0));
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 3];
			array17[0] = label3;
			array17[1] = grid;
			array17[2] = this;
			object obj33;
			xamlServiceProvider17.Add(typeFromHandle33, obj33 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver17.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(LinearProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(229, 17)));
			DynamicResource dynamicResource = markupExtension17.ProvideValue(xamlServiceProvider17);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension34.Mode = 2;
			bindingExtension34.Source = sharedSettings;
			bindingExtension34.Path = "ShowPing";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase34);
			label3.SetValue(Label.TextColorProperty, Color.Red);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension35.Mode = 2;
			bindingExtension35.Path = "Model.Ping";
			bindingExtension35.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						return new ValueTuple<long, bool>(model5.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase35);
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, "ms");
			formattedString2.Spans.Add(span4);
			label3.SetValue(Label.FormattedTextProperty, formattedString2);
			grid.Children.Add(label3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension36.Mode = 2;
			bindingExtension36.Path = "MinMaxAvgFontSize";
			bindingExtension36.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase36);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension37.Mode = 2;
			bindingExtension37.Path = "ShowMinMax";
			bindingExtension37.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase37);
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "MinMaxAvgColor";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase38);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span5.SetValue(Span.TextProperty, "Max: ");
			formattedString3.Spans.Add(span5);
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "Model.MaximumAchievedText";
			bindingExtension39.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model6 = A_0.Model;
					if (model6 != null)
					{
						return new ValueTuple<string, bool>(model6.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase39);
			formattedString3.Spans.Add(span6);
			label4.SetValue(Label.FormattedTextProperty, formattedString3);
			grid.Children.Add(label4);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension40.Mode = 2;
			bindingExtension40.Path = "MinMaxAvgFontSize";
			bindingExtension40.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			label5.SetBinding(Label.FontSizeProperty, bindingBase40);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension41.Mode = 2;
			bindingExtension41.Path = "ShowMinMax";
			bindingExtension41.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase41);
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "MinMaxAvgColor";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			label5.SetBinding(Label.TextColorProperty, bindingBase42);
			label5.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span7.SetValue(Span.TextProperty, "Min: ");
			formattedString4.Spans.Add(span7);
			bindingExtension43.Mode = 2;
			bindingExtension43.Path = "Model.MinimumAchievedText";
			bindingExtension43.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model7 = A_0.Model;
					if (model7 != null)
					{
						return new ValueTuple<string, bool>(model7.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase43);
			formattedString4.Spans.Add(span8);
			label5.SetValue(Label.FormattedTextProperty, formattedString4);
			grid.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension44.Mode = 2;
			bindingExtension44.Path = "MinMaxAvgFontSize";
			bindingExtension44.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			label6.SetBinding(Label.FontSizeProperty, bindingBase44);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension45.Mode = 2;
			bindingExtension45.Path = "ShowAvg";
			bindingExtension45.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase45);
			bindingExtension46.Mode = 2;
			bindingExtension46.Path = "MinMaxAvgColor";
			bindingExtension46.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			label6.SetBinding(Label.TextColorProperty, bindingBase46);
			label6.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span9.SetValue(Span.TextProperty, "Avg: ");
			formattedString5.Spans.Add(span9);
			bindingExtension47.Mode = 2;
			bindingExtension47.Path = "Model.AverageTextValue";
			bindingExtension47.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model8 = A_0.Model;
					if (model8 != null)
					{
						return new ValueTuple<string, bool>(model8.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			span10.SetBinding(Span.TextProperty, bindingBase47);
			formattedString5.Spans.Add(span10);
			label6.SetValue(Label.FormattedTextProperty, formattedString5);
			grid.Children.Add(label6);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x06004474 RID: 17524 RVA: 0x00359200 File Offset: 0x00357400
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LinearProgressItem>(this, typeof(LinearProgressItem));
			this.HackyBindedKey = NameScopeExtensions.FindByName<ContentView>(this, "HackyBindedKey");
			this.layoutGrid = NameScopeExtensions.FindByName<Grid>(this, "layoutGrid");
			this.linearGauge = NameScopeExtensions.FindByName<SfLinearGauge>(this, "linearGauge");
			this.gaugeScale = NameScopeExtensions.FindByName<LinearScale>(this, "gaugeScale");
			this.gaugeTick = NameScopeExtensions.FindByName<LinearTickSettings>(this, "gaugeTick");
			this.gaugeSmallTick = NameScopeExtensions.FindByName<LinearTickSettings>(this, "gaugeSmallTick");
			this.gaugePointer = NameScopeExtensions.FindByName<BarPointer>(this, "gaugePointer");
		}

		// Token: 0x06004475 RID: 17525 RVA: 0x00359298 File Offset: 0x00357498
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__611(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004476 RID: 17526 RVA: 0x003592C8 File Offset: 0x003574C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__612(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004477 RID: 17527 RVA: 0x003592D8 File Offset: 0x003574D8
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__613(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x06004478 RID: 17528 RVA: 0x00359308 File Offset: 0x00357508
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__614(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004479 RID: 17529 RVA: 0x00359318 File Offset: 0x00357518
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__615(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600447A RID: 17530 RVA: 0x00359348 File Offset: 0x00357548
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__616(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600447B RID: 17531 RVA: 0x00359358 File Offset: 0x00357558
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__617(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600447C RID: 17532 RVA: 0x00359388 File Offset: 0x00357588
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__618(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600447D RID: 17533 RVA: 0x00359398 File Offset: 0x00357598
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__619(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600447E RID: 17534 RVA: 0x003593C8 File Offset: 0x003575C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__620(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600447F RID: 17535 RVA: 0x003593D8 File Offset: 0x003575D8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__621(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004480 RID: 17536 RVA: 0x00359408 File Offset: 0x00357608
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__622(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x00359418 File Offset: 0x00357618
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__623(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004482 RID: 17538 RVA: 0x00359448 File Offset: 0x00357648
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__624(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004483 RID: 17539 RVA: 0x00359458 File Offset: 0x00357658
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__625(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x00359488 File Offset: 0x00357688
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__626(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LinearOrientationHorizontal = A_1;
				return;
			}
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x003594A4 File Offset: 0x003576A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__627(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x003594B4 File Offset: 0x003576B4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__628(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x003594E4 File Offset: 0x003576E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__629(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x003594F4 File Offset: 0x003576F4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__630(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x00359524 File Offset: 0x00357724
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__632(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x00359534 File Offset: 0x00357734
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__633(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600448B RID: 17547 RVA: 0x00359564 File Offset: 0x00357764
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__634(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600448C RID: 17548 RVA: 0x00359574 File Offset: 0x00357774
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__635(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600448D RID: 17549 RVA: 0x003595A4 File Offset: 0x003577A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__636(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600448E RID: 17550 RVA: 0x003595B4 File Offset: 0x003577B4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__637(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600448F RID: 17551 RVA: 0x003595E4 File Offset: 0x003577E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__638(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004490 RID: 17552 RVA: 0x003595F4 File Offset: 0x003577F4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__639(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004491 RID: 17553 RVA: 0x00359624 File Offset: 0x00357824
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__640(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004492 RID: 17554 RVA: 0x00359634 File Offset: 0x00357834
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__641(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x00359664 File Offset: 0x00357864
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__642(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004494 RID: 17556 RVA: 0x00359674 File Offset: 0x00357874
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__643(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004495 RID: 17557 RVA: 0x003596A4 File Offset: 0x003578A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__644(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004496 RID: 17558 RVA: 0x003596B4 File Offset: 0x003578B4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__645(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004497 RID: 17559 RVA: 0x003596E4 File Offset: 0x003578E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__646(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004498 RID: 17560 RVA: 0x003596F4 File Offset: 0x003578F4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__647(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004499 RID: 17561 RVA: 0x00359724 File Offset: 0x00357924
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__648(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600449A RID: 17562 RVA: 0x00359734 File Offset: 0x00357934
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__649(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.ChartVisible, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0035976C File Offset: 0x0035796C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__650(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					model.ChartVisible = A_1;
					return;
				}
			}
		}

		// Token: 0x0600449C RID: 17564 RVA: 0x00359794 File Offset: 0x00357994
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__651(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600449D RID: 17565 RVA: 0x003597A4 File Offset: 0x003579A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__652(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600449E RID: 17566 RVA: 0x003597B8 File Offset: 0x003579B8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__653(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600449F RID: 17567 RVA: 0x003597E8 File Offset: 0x003579E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__654(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044A0 RID: 17568 RVA: 0x003597F8 File Offset: 0x003579F8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__655(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044A1 RID: 17569 RVA: 0x00359828 File Offset: 0x00357A28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__656(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044A2 RID: 17570 RVA: 0x00359838 File Offset: 0x00357A38
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__657(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.TextValue, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060044A3 RID: 17571 RVA: 0x00359870 File Offset: 0x00357A70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__658(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044A4 RID: 17572 RVA: 0x00359880 File Offset: 0x00357A80
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__659(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044A5 RID: 17573 RVA: 0x00359894 File Offset: 0x00357A94
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__660(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044A6 RID: 17574 RVA: 0x003598C4 File Offset: 0x00357AC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__661(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044A7 RID: 17575 RVA: 0x003598D4 File Offset: 0x00357AD4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__662(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044A8 RID: 17576 RVA: 0x00359904 File Offset: 0x00357B04
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__663(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044A9 RID: 17577 RVA: 0x00359914 File Offset: 0x00357B14
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__664(DashboardItem A_0)
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

		// Token: 0x060044AA RID: 17578 RVA: 0x0035994C File Offset: 0x00357B4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__665(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x0035995C File Offset: 0x00357B5C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__666(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00359970 File Offset: 0x00357B70
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__667(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x003599A0 File Offset: 0x00357BA0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__668(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x003599B0 File Offset: 0x00357BB0
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__669(DashboardItem A_0)
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

		// Token: 0x060044AF RID: 17583 RVA: 0x003599E8 File Offset: 0x00357BE8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__670(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x003599F8 File Offset: 0x00357BF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__671(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00359A0C File Offset: 0x00357C0C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__672(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x00359A3C File Offset: 0x00357C3C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__673(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00359A4C File Offset: 0x00357C4C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__674(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044B4 RID: 17588 RVA: 0x00359A7C File Offset: 0x00357C7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__675(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044B5 RID: 17589 RVA: 0x00359A8C File Offset: 0x00357C8C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__676(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044B6 RID: 17590 RVA: 0x00359ABC File Offset: 0x00357CBC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__677(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044B7 RID: 17591 RVA: 0x00359ACC File Offset: 0x00357CCC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__678(DashboardItem A_0)
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

		// Token: 0x060044B8 RID: 17592 RVA: 0x00359B04 File Offset: 0x00357D04
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__679(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x00359B14 File Offset: 0x00357D14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__680(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044BA RID: 17594 RVA: 0x00359B28 File Offset: 0x00357D28
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__681(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044BB RID: 17595 RVA: 0x00359B58 File Offset: 0x00357D58
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__682(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044BC RID: 17596 RVA: 0x00359B68 File Offset: 0x00357D68
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__683(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044BD RID: 17597 RVA: 0x00359B98 File Offset: 0x00357D98
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__684(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044BE RID: 17598 RVA: 0x00359BA8 File Offset: 0x00357DA8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__685(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x00359BD8 File Offset: 0x00357DD8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__686(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044C0 RID: 17600 RVA: 0x00359BE8 File Offset: 0x00357DE8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__687(DashboardItem A_0)
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

		// Token: 0x060044C1 RID: 17601 RVA: 0x00359C20 File Offset: 0x00357E20
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__688(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044C2 RID: 17602 RVA: 0x00359C30 File Offset: 0x00357E30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__689(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044C3 RID: 17603 RVA: 0x00359C44 File Offset: 0x00357E44
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__690(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x00359C74 File Offset: 0x00357E74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__691(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x00359C84 File Offset: 0x00357E84
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__692(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00359CB4 File Offset: 0x00357EB4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__693(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x00359CC4 File Offset: 0x00357EC4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__694(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00359CF4 File Offset: 0x00357EF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__695(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x00359D04 File Offset: 0x00357F04
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__696(DashboardItem A_0)
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

		// Token: 0x060044CA RID: 17610 RVA: 0x00359D3C File Offset: 0x00357F3C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__697(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044CB RID: 17611 RVA: 0x00359D4C File Offset: 0x00357F4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__698(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028CC RID: 10444
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView HackyBindedKey;

		// Token: 0x040028CD RID: 10445
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutGrid;

		// Token: 0x040028CE RID: 10446
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfLinearGauge linearGauge;

		// Token: 0x040028CF RID: 10447
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinearScale gaugeScale;

		// Token: 0x040028D0 RID: 10448
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinearTickSettings gaugeTick;

		// Token: 0x040028D1 RID: 10449
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinearTickSettings gaugeSmallTick;

		// Token: 0x040028D2 RID: 10450
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BarPointer gaugePointer;
	}
}
