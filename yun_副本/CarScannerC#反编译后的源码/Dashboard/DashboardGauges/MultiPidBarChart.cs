using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Dashboard.DashboardGauges
{
	// Token: 0x020007B6 RID: 1974
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml")]
	public class MultiPidBarChart : ContentView
	{
		// Token: 0x06004574 RID: 17780 RVA: 0x00360DAF File Offset: 0x0035EFAF
		public MultiPidBarChart()
		{
			this.InitializeComponent();
			base.BindingContextChanged += this.ChartItem_BindingContextChanged;
		}

		// Token: 0x06004575 RID: 17781 RVA: 0x00360DCF File Offset: 0x0035EFCF
		private void ChartItem_BindingContextChanged(object sender, EventArgs e)
		{
			this._dashItem = base.BindingContext as DashboardItem;
		}

		// Token: 0x06004576 RID: 17782 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x06004577 RID: 17783 RVA: 0x00360DE2 File Offset: 0x0035EFE2
		private void xAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.LabelContent != null && e.LabelContent.StartsWith("-"))
			{
				e.LabelContent = "";
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x00360E0C File Offset: 0x0035F00C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MultiPidBarChart).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/MultiPidBarChart.xaml",
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
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 21);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 21);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 21);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 30);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 35);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 35);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 30);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 35);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 22);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 22);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 25);
			ColumnSeries columnSeries;
			VisualDiagnostics.RegisterSourceInfo(columnSeries = new ColumnSeries(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 14);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 17);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 17);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 26);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 26);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 26);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 31);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 26);
			Span span11;
			VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 26);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 31);
			Span span12;
			VisualDiagnostics.RegisterSourceInfo(span12 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 17);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 17);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 31);
			Span span13;
			VisualDiagnostics.RegisterSourceInfo(span13 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 26);
			Span span14;
			VisualDiagnostics.RegisterSourceInfo(span14 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\MultiPidBarChart.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("chart", sfChart);
			if (sfChart.StyleId == null)
			{
				sfChart.StyleId = "chart";
			}
			this.chart = sfChart;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.Resources = resourceDictionary;
			grid2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			grid2.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			gaugeBackground.SetValue(Grid.RowSpanProperty, 2);
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
			grid2.Children.Add(gaugeBackground);
			boxView.SetValue(Grid.RowProperty, 0);
			boxView.SetValue(Grid.RowSpanProperty, 2);
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "CornerRadiusToThicknessConverter";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = bindingExtension2;
			array[1] = boxView;
			array[2] = grid2;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 17)));
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
			array2[2] = grid2;
			array2[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 17)));
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
			grid2.Children.Add(boxView);
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.ColumnProperty, 0);
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
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
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
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			label2.SetValue(Grid.ColumnProperty, 1);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "MinMaxAvgFontSize";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label2.SetBinding(Label.FontSizeProperty, bindingBase8);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension9.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = bindingExtension9;
			array3[1] = label2;
			array3[2] = grid;
			array3[3] = grid2;
			array3[4] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 21)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension9.Converter = obj6;
			bindingExtension9.Path = "ChartValuePositionCenter";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartValuePositionCenter")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "MinMaxAvgColor";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label2.SetBinding(Label.TextColorProperty, bindingBase10);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span.SetValue(Span.TextProperty, "Min=");
			formattedString.Spans.Add(span);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "Model.MinimumAchievedText";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase11);
			formattedString.Spans.Add(span2);
			span3.SetValue(Span.TextProperty, " Max=");
			formattedString.Spans.Add(span3);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "Model.MaximumAchievedText";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			span4.SetBinding(Span.TextProperty, bindingBase12);
			formattedString.Spans.Add(span4);
			span5.SetValue(Span.TextProperty, " Avg=");
			formattedString.Spans.Add(span5);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "Model.AverageTextValue";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<string, bool>(model3.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase13);
			formattedString.Spans.Add(span6);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label2);
			grid2.Children.Add(grid);
			sfChart.SetValue(Grid.RowProperty, 1);
			sfChart.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			numericalAxis.LabelCreated += this.xAxis_LabelCreated;
			staticResourceExtension4.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension4 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = numericalAxis;
			array4[1] = sfChart;
			array4[2] = grid2;
			array4[3] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 25)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			numericalAxis.MajorGridLineStyle = obj8;
			staticResourceExtension5.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension5 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = numericalAxis;
			array5[1] = sfChart;
			array5[2] = grid2;
			array5[3] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array5, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 25)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			numericalAxis.MajorTickStyle = obj10;
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
			chartAxisLabelStyle.SetBinding(ChartLabelStyle.TextColorProperty, bindingBase14);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 2);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 1);
			bindingExtension15.Path = "Interval";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.IntervalProperty, bindingBase15);
			numericalAxis2.LabelCreated += this.valueAxis_LabelCreated;
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension6.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension6 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = numericalAxis2;
			array6[1] = sfChart;
			array6[2] = grid2;
			array6[3] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array6, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 25)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			numericalAxis2.MajorGridLineStyle = obj12;
			staticResourceExtension7.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension7 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = numericalAxis2;
			array7[1] = sfChart;
			array7[2] = grid2;
			array7[3] = this;
			object obj13;
			xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array7, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 25)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			numericalAxis2.MajorTickStyle = obj14;
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "Maximum";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MaximumProperty, bindingBase16);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "Minimum";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MinimumProperty, bindingBase17);
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "GaugeLabelColor";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			chartAxisLabelStyle2.SetBinding(ChartLabelStyle.TextColorProperty, bindingBase18);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			bindingExtension19.Path = "Model.Values";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, SmartCollection<DoubleValueItem>>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<SmartCollection<DoubleValueItem>, bool>(model4.Values, true);
					}
				}
				return default(ValueTuple<SmartCollection<DoubleValueItem>, bool>);
			}, delegate(DashboardItem A_0, SmartCollection<DoubleValueItem> A_1)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						model5.Values = A_1;
						return;
					}
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Values")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			columnSeries.SetBinding(ChartSeries.ItemsSourceProperty, bindingBase19);
			columnSeries.SetValue(ChartSeries.XBindingPathProperty, "SecondsAdded");
			columnSeries.SetValue(XyDataSeries.YBindingPathProperty, "Value");
			sfChart.GetValue(SfChart.SeriesProperty).Add(columnSeries);
			grid2.Children.Add(sfChart);
			label3.SetValue(Grid.RowProperty, 1);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "MinMaxAvgFontSize";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase20);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "ChartValuePositionCenter";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartValuePositionCenter")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase21);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "MinMaxAvgColor";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase22);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			span7.SetValue(Span.TextProperty, "Min=");
			formattedString2.Spans.Add(span7);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "Model.MinimumAchievedText";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model6 = A_0.Model;
					if (model6 != null)
					{
						return new ValueTuple<string, bool>(model6.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase23);
			formattedString2.Spans.Add(span8);
			span9.SetValue(Span.TextProperty, " Max=");
			formattedString2.Spans.Add(span9);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "Model.MaximumAchievedText";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model7 = A_0.Model;
					if (model7 != null)
					{
						return new ValueTuple<string, bool>(model7.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			span10.SetBinding(Span.TextProperty, bindingBase24);
			formattedString2.Spans.Add(span10);
			span11.SetValue(Span.TextProperty, " Avg=");
			formattedString2.Spans.Add(span11);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "Model.AverageTextValue";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			span12.SetBinding(Span.TextProperty, bindingBase25);
			formattedString2.Spans.Add(span12);
			label3.SetValue(Label.FormattedTextProperty, formattedString2);
			grid2.Children.Add(label3);
			label4.SetValue(Grid.RowProperty, 1);
			label4.SetValue(View.MarginProperty, new Thickness(0.0));
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = label4;
			array8[1] = grid2;
			array8[2] = this;
			object obj15;
			xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(MultiPidBarChart).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(191, 17)));
			DynamicResource dynamicResource = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension26.Mode = 2;
			bindingExtension26.Source = sharedSettings;
			bindingExtension26.Path = "ShowPing";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase26);
			label4.SetValue(Label.TextColorProperty, Color.Red);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "Model.Ping";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model9 = A_0.Model;
					if (model9 != null)
					{
						return new ValueTuple<long, bool>(model9.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			span13.SetBinding(Span.TextProperty, bindingBase27);
			formattedString3.Spans.Add(span13);
			span14.SetValue(Span.TextProperty, "ms");
			formattedString3.Spans.Add(span14);
			label4.SetValue(Label.FormattedTextProperty, formattedString3);
			grid2.Children.Add(label4);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x00363761 File Offset: 0x00361961
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MultiPidBarChart>(this, typeof(MultiPidBarChart));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x00363788 File Offset: 0x00361988
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__854(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600457B RID: 17787 RVA: 0x003637B8 File Offset: 0x003619B8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__855(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x0600457C RID: 17788 RVA: 0x003637D4 File Offset: 0x003619D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__856(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600457D RID: 17789 RVA: 0x003637E4 File Offset: 0x003619E4
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__857(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x0600457E RID: 17790 RVA: 0x00363814 File Offset: 0x00361A14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__858(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600457F RID: 17791 RVA: 0x00363824 File Offset: 0x00361A24
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__859(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004580 RID: 17792 RVA: 0x00363854 File Offset: 0x00361A54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__860(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004581 RID: 17793 RVA: 0x00363864 File Offset: 0x00361A64
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__861(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004582 RID: 17794 RVA: 0x00363894 File Offset: 0x00361A94
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__862(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004583 RID: 17795 RVA: 0x003638A4 File Offset: 0x00361AA4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__863(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004584 RID: 17796 RVA: 0x003638D4 File Offset: 0x00361AD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__864(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004585 RID: 17797 RVA: 0x003638E4 File Offset: 0x00361AE4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__865(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004586 RID: 17798 RVA: 0x00363914 File Offset: 0x00361B14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__866(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004587 RID: 17799 RVA: 0x00363924 File Offset: 0x00361B24
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__867(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004588 RID: 17800 RVA: 0x00363954 File Offset: 0x00361B54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__868(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x00363964 File Offset: 0x00361B64
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__869(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600458A RID: 17802 RVA: 0x00363994 File Offset: 0x00361B94
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__870(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600458B RID: 17803 RVA: 0x003639A4 File Offset: 0x00361BA4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__871(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600458C RID: 17804 RVA: 0x003639D4 File Offset: 0x00361BD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__872(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600458D RID: 17805 RVA: 0x003639E4 File Offset: 0x00361BE4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__873(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600458E RID: 17806 RVA: 0x00363A14 File Offset: 0x00361C14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__874(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600458F RID: 17807 RVA: 0x00363A24 File Offset: 0x00361C24
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__875(DashboardItem A_0)
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

		// Token: 0x06004590 RID: 17808 RVA: 0x00363A5C File Offset: 0x00361C5C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__876(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004591 RID: 17809 RVA: 0x00363A6C File Offset: 0x00361C6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__877(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004592 RID: 17810 RVA: 0x00363A80 File Offset: 0x00361C80
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__878(DashboardItem A_0)
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

		// Token: 0x06004593 RID: 17811 RVA: 0x00363AB8 File Offset: 0x00361CB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__879(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004594 RID: 17812 RVA: 0x00363AC8 File Offset: 0x00361CC8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__880(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x00363ADC File Offset: 0x00361CDC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__881(DashboardItem A_0)
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

		// Token: 0x06004596 RID: 17814 RVA: 0x00363B14 File Offset: 0x00361D14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__882(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x00363B24 File Offset: 0x00361D24
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__883(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x00363B38 File Offset: 0x00361D38
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__884(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004599 RID: 17817 RVA: 0x00363B68 File Offset: 0x00361D68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__885(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600459A RID: 17818 RVA: 0x00363B78 File Offset: 0x00361D78
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__886(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600459B RID: 17819 RVA: 0x00363BA8 File Offset: 0x00361DA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__888(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600459C RID: 17820 RVA: 0x00363BB8 File Offset: 0x00361DB8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__889(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600459D RID: 17821 RVA: 0x00363BE8 File Offset: 0x00361DE8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__890(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00363BF8 File Offset: 0x00361DF8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__891(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x00363C28 File Offset: 0x00361E28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__892(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00363C38 File Offset: 0x00361E38
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__893(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x00363C68 File Offset: 0x00361E68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__894(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00363C78 File Offset: 0x00361E78
		[CompilerGenerated]
		private static ValueTuple<SmartCollection<DoubleValueItem>, bool> <InitializeComponent>typedBindingsM__895(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<SmartCollection<DoubleValueItem>, bool>(model.Values, true);
				}
			}
			return default(ValueTuple<SmartCollection<DoubleValueItem>, bool>);
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x00363CB0 File Offset: 0x00361EB0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__896(DashboardItem A_0, SmartCollection<DoubleValueItem> A_1)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					model.Values = A_1;
					return;
				}
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x00363CD8 File Offset: 0x00361ED8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__897(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x00363CE8 File Offset: 0x00361EE8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__898(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x00363CFC File Offset: 0x00361EFC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__899(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x00363D2C File Offset: 0x00361F2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__900(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00363D3C File Offset: 0x00361F3C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__901(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00363D6C File Offset: 0x00361F6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__902(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x00363D7C File Offset: 0x00361F7C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__903(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x00363DAC File Offset: 0x00361FAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__904(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00363DBC File Offset: 0x00361FBC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__905(DashboardItem A_0)
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

		// Token: 0x060045AD RID: 17837 RVA: 0x00363DF4 File Offset: 0x00361FF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__906(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045AE RID: 17838 RVA: 0x00363E04 File Offset: 0x00362004
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__907(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045AF RID: 17839 RVA: 0x00363E18 File Offset: 0x00362018
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__908(DashboardItem A_0)
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

		// Token: 0x060045B0 RID: 17840 RVA: 0x00363E50 File Offset: 0x00362050
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__909(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045B1 RID: 17841 RVA: 0x00363E60 File Offset: 0x00362060
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__910(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045B2 RID: 17842 RVA: 0x00363E74 File Offset: 0x00362074
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__911(DashboardItem A_0)
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

		// Token: 0x060045B3 RID: 17843 RVA: 0x00363EAC File Offset: 0x003620AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__912(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045B4 RID: 17844 RVA: 0x00363EBC File Offset: 0x003620BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__913(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045B5 RID: 17845 RVA: 0x00363ED0 File Offset: 0x003620D0
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__914(DashboardItem A_0)
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

		// Token: 0x060045B6 RID: 17846 RVA: 0x00363F08 File Offset: 0x00362108
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__915(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045B7 RID: 17847 RVA: 0x00363F18 File Offset: 0x00362118
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__916(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028D7 RID: 10455
		private DashboardItem _dashItem;

		// Token: 0x040028D8 RID: 10456
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;
	}
}
