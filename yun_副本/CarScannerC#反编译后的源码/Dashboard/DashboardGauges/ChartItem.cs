using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.PlatformAdapters;
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
	// Token: 0x020007A9 RID: 1961
	[XamlFilePath("Dashboard\\DashboardGauges\\ChartItem.xaml")]
	public class ChartItem : ContentView
	{
		// Token: 0x06004309 RID: 17161 RVA: 0x0033FB82 File Offset: 0x0033DD82
		public ChartItem(ChartItemTypes chartType)
		{
			this.InitializeComponent();
			base.BindingContextChanged += this.ChartItem_BindingContextChanged;
			this.UpdateSeries(chartType);
		}

		// Token: 0x0600430A RID: 17162 RVA: 0x0033FBA9 File Offset: 0x0033DDA9
		private void ChartItem_BindingContextChanged(object sender, EventArgs e)
		{
			this._dashItem = base.BindingContext as DashboardItem;
		}

		// Token: 0x0600430B RID: 17163 RVA: 0x0033FBBC File Offset: 0x0033DDBC
		public void UpdateSeries(ChartItemTypes chartType)
		{
			if (this.fastLineSeries != null)
			{
				if (this.chart.Series.Contains(this.fastLineSeries))
				{
					this.chart.Series.Remove(this.fastLineSeries);
				}
				this.fastLineSeries = null;
			}
			switch (chartType)
			{
			case ChartItemTypes.FastLine:
				if (PlatformHelper.IsiOS)
				{
					this.fastLineSeries = new FastLineSeries();
					this.fastLineSeries.EnableAnimation = true;
				}
				else if (PlatformHelper.IsAndroid)
				{
					this.fastLineSeries = new FastLineSeries();
					this.fastLineSeries.EnableAnimation = false;
				}
				break;
			case ChartItemTypes.Area:
				this.fastLineSeries = new AreaSeries();
				break;
			case ChartItemTypes.SplineLine:
				this.fastLineSeries = new SplineSeries
				{
					SplineType = 1
				};
				break;
			case ChartItemTypes.SplineArea:
				this.fastLineSeries = new SplineAreaSeries
				{
					SplineType = 1
				};
				break;
			}
			this.fastLineSeries.AnimationDuration = 0.35;
			BindableObjectExtensions.SetBinding(this.fastLineSeries, ChartSeries.ItemsSourceProperty, "Model.Values", 2, null, null);
			this.fastLineSeries.XBindingPath = "SecondsAdded";
			this.fastLineSeries.YBindingPath = "Value";
			BindableObjectExtensions.SetBinding(this.fastLineSeries, ChartSeries.ColorProperty, "ChartLineColor", 2, null, null);
			BindableObjectExtensions.SetBinding(this.fastLineSeries, ChartSeries.StrokeWidthProperty, "ChartLineWidth", 2, null, null);
			this.chart.Series.Add(this.fastLineSeries);
		}

		// Token: 0x0600430C RID: 17164 RVA: 0x0033FD24 File Offset: 0x0033DF24
		private void NumericalAxisActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
			if (this._dashItem != null)
			{
				double num = (double)e.ActualMaximum - (double)this._dashItem.LiveDataShowTime;
				e.ActualMinimum = num;
				return;
			}
			double num2 = (double)e.ActualMaximum - (double)SharedSettings.Current.LiveDataShowTime;
			e.ActualMinimum = num2;
		}

		// Token: 0x0600430D RID: 17165 RVA: 0x0033FD84 File Offset: 0x0033DF84
		private void numAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			int num;
			if (int.TryParse(e.LabelContent, NumberStyles.Any, CultureInfo.InvariantCulture, out num))
			{
				int num2 = num / 60;
				num %= 60;
				e.LabelContent = num2.ToString(CultureInfo.InvariantCulture) + ":" + num.ToString("00", CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x0033FDE4 File Offset: 0x0033DFE4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ChartItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/ChartItem.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 21);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 21);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 33);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 33);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 33);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 26);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 22);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 25);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 14);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 17);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 29);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 29);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 29);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 29);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 26);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 26);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 29);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 29);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 29);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 17);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 17);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 26);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 14);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 17);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 17);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 17);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 26);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 31);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 14);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 17);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 17);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 17);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 26);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 31);
			Span span11;
			VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 26);
			FormattedString formattedString5;
			VisualDiagnostics.RegisterSourceInfo(formattedString5 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 22);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 14);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 17);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 17);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 17);
			Span span12;
			VisualDiagnostics.RegisterSourceInfo(span12 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 26);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 31);
			Span span13;
			VisualDiagnostics.RegisterSourceInfo(span13 = new Span(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 26);
			FormattedString formattedString6;
			VisualDiagnostics.RegisterSourceInfo(formattedString6 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 22);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\ChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ChartItem).GetTypeInfo().Assembly));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ChartItem).GetTypeInfo().Assembly));
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
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension8.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = bindingExtension8;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 21)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension8.Converter = obj6;
			bindingExtension8.Path = "ChartValuePositionCenter";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "FontName";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span.SetBinding(Span.FontFamilyProperty, bindingBase9);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "ValueFontSize";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			span.SetBinding(Span.FontSizeProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "Model.TextValue";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "TextValue")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase11);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "ValueTextColor";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			span.SetBinding(Span.TextColorProperty, bindingBase12);
			formattedString.Spans.Add(span);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "UnitsFontSize";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			span2.SetBinding(Span.FontSizeProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "Model.Units";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						return new ValueTuple<string, bool>(model2.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase14);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "UnitsTextColor";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			span2.SetBinding(Span.TextColorProperty, bindingBase15);
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label2);
			grid2.Children.Add(grid);
			sfChart.SetValue(Grid.RowProperty, 1);
			sfChart.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(View.VerticalOptionsProperty, LayoutOptions.FillAndExpand);
			numericalAxis.ActualRangeChanged += this.NumericalAxisActualRangeChanged;
			numericalAxis.SetValue(NumericalAxis.IntervalProperty, new double?(5.0));
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 25)));
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 25)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			numericalAxis.MajorTickStyle = obj10;
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "GaugeLabelColor";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			chartAxisLabelStyle.SetBinding(ChartLabelStyle.TextColorProperty, bindingBase16);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 2);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 1);
			bindingExtension17.Path = "Interval";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.IntervalProperty, bindingBase17);
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 25)));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 25)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			numericalAxis2.MajorTickStyle = obj14;
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "Maximum";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MaximumProperty, bindingBase18);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "Minimum";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MinimumProperty, bindingBase19);
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "GaugeLabelColor";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			chartAxisLabelStyle2.SetBinding(ChartLabelStyle.TextColorProperty, bindingBase20);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			grid2.Children.Add(sfChart);
			label3.SetValue(Grid.RowProperty, 1);
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
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "FontName";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			span3.SetBinding(Span.FontFamilyProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "ValueFontSize";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			span3.SetBinding(Span.FontSizeProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "Model.TextValue";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase24);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "ValueTextColor";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			span3.SetBinding(Span.TextColorProperty, bindingBase25);
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, "\n");
			formattedString2.Spans.Add(span4);
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
			span5.SetBinding(Span.FontSizeProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "Model.Units";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase27);
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
			span5.SetBinding(Span.TextColorProperty, bindingBase28);
			formattedString2.Spans.Add(span5);
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(ChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 17)));
			DynamicResource dynamicResource = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension29.Mode = 2;
			bindingExtension29.Source = sharedSettings;
			bindingExtension29.Path = "ShowPing";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase29);
			label4.SetValue(Label.TextColorProperty, Color.Red);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "Model.Ping";
			bindingExtension30.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase30);
			formattedString3.Spans.Add(span6);
			span7.SetValue(Span.TextProperty, "ms");
			formattedString3.Spans.Add(span7);
			label4.SetValue(Label.FormattedTextProperty, formattedString3);
			grid2.Children.Add(label4);
			label5.SetValue(Grid.RowProperty, 1);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
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
			label5.SetBinding(Label.FontSizeProperty, bindingBase31);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
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
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase32);
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
			label5.SetBinding(Label.TextColorProperty, bindingBase33);
			label5.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span8.SetValue(Span.TextProperty, "Max: ");
			formattedString4.Spans.Add(span8);
			bindingExtension34.Mode = 2;
			bindingExtension34.Path = "Model.MaximumAchievedText";
			bindingExtension34.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			span9.SetBinding(Span.TextProperty, bindingBase34);
			formattedString4.Spans.Add(span9);
			label5.SetValue(Label.FormattedTextProperty, formattedString4);
			grid2.Children.Add(label5);
			label6.SetValue(Grid.RowProperty, 1);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
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
			label6.SetBinding(Label.FontSizeProperty, bindingBase35);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
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
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase36);
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
			label6.SetBinding(Label.TextColorProperty, bindingBase37);
			label6.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span10.SetValue(Span.TextProperty, "Min: ");
			formattedString5.Spans.Add(span10);
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "Model.MinimumAchievedText";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			span11.SetBinding(Span.TextProperty, bindingBase38);
			formattedString5.Spans.Add(span11);
			label6.SetValue(Label.FormattedTextProperty, formattedString5);
			grid2.Children.Add(label6);
			label7.SetValue(Grid.RowProperty, 1);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
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
			label7.SetBinding(Label.FontSizeProperty, bindingBase39);
			label7.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
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
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase40);
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
			label7.SetBinding(Label.TextColorProperty, bindingBase41);
			label7.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span12.SetValue(Span.TextProperty, "Avg: ");
			formattedString6.Spans.Add(span12);
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "Model.AverageTextValue";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			span13.SetBinding(Span.TextProperty, bindingBase42);
			formattedString6.Spans.Add(span13);
			label7.SetValue(Label.FormattedTextProperty, formattedString6);
			grid2.Children.Add(label7);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x003431B2 File Offset: 0x003413B2
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ChartItem>(this, typeof(ChartItem));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x003431D8 File Offset: 0x003413D8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__278(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x00343208 File Offset: 0x00341408
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__279(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00343224 File Offset: 0x00341424
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__280(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00343234 File Offset: 0x00341434
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__281(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x06004315 RID: 17173 RVA: 0x00343264 File Offset: 0x00341464
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__282(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004316 RID: 17174 RVA: 0x00343274 File Offset: 0x00341474
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__283(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004317 RID: 17175 RVA: 0x003432A4 File Offset: 0x003414A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__284(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004318 RID: 17176 RVA: 0x003432B4 File Offset: 0x003414B4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__285(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004319 RID: 17177 RVA: 0x003432E4 File Offset: 0x003414E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__286(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x003432F4 File Offset: 0x003414F4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__287(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600431B RID: 17179 RVA: 0x00343324 File Offset: 0x00341524
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__288(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600431C RID: 17180 RVA: 0x00343334 File Offset: 0x00341534
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__289(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600431D RID: 17181 RVA: 0x00343364 File Offset: 0x00341564
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__290(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600431E RID: 17182 RVA: 0x00343374 File Offset: 0x00341574
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__291(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600431F RID: 17183 RVA: 0x003433A4 File Offset: 0x003415A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__292(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004320 RID: 17184 RVA: 0x003433B4 File Offset: 0x003415B4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__293(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004321 RID: 17185 RVA: 0x003433E4 File Offset: 0x003415E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__294(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004322 RID: 17186 RVA: 0x003433F4 File Offset: 0x003415F4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__295(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004323 RID: 17187 RVA: 0x00343424 File Offset: 0x00341624
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__296(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004324 RID: 17188 RVA: 0x00343434 File Offset: 0x00341634
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__297(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004325 RID: 17189 RVA: 0x00343464 File Offset: 0x00341664
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__298(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004326 RID: 17190 RVA: 0x00343474 File Offset: 0x00341674
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__299(DashboardItem A_0)
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

		// Token: 0x06004327 RID: 17191 RVA: 0x003434AC File Offset: 0x003416AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__300(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004328 RID: 17192 RVA: 0x003434BC File Offset: 0x003416BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__301(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004329 RID: 17193 RVA: 0x003434D0 File Offset: 0x003416D0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__302(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600432A RID: 17194 RVA: 0x00343500 File Offset: 0x00341700
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__303(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600432B RID: 17195 RVA: 0x00343510 File Offset: 0x00341710
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__304(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600432C RID: 17196 RVA: 0x00343540 File Offset: 0x00341740
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__305(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600432D RID: 17197 RVA: 0x00343550 File Offset: 0x00341750
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__306(DashboardItem A_0)
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

		// Token: 0x0600432E RID: 17198 RVA: 0x00343588 File Offset: 0x00341788
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__307(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600432F RID: 17199 RVA: 0x00343598 File Offset: 0x00341798
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__308(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004330 RID: 17200 RVA: 0x003435AC File Offset: 0x003417AC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__309(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004331 RID: 17201 RVA: 0x003435DC File Offset: 0x003417DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__310(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004332 RID: 17202 RVA: 0x003435EC File Offset: 0x003417EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__311(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004333 RID: 17203 RVA: 0x0034361C File Offset: 0x0034181C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__312(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004334 RID: 17204 RVA: 0x0034362C File Offset: 0x0034182C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__313(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004335 RID: 17205 RVA: 0x0034365C File Offset: 0x0034185C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__315(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004336 RID: 17206 RVA: 0x0034366C File Offset: 0x0034186C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__316(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x0034369C File Offset: 0x0034189C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__317(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x003436AC File Offset: 0x003418AC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__318(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x003436DC File Offset: 0x003418DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__319(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x003436EC File Offset: 0x003418EC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__320(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x0034371C File Offset: 0x0034191C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__321(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x0034372C File Offset: 0x0034192C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__322(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600433D RID: 17213 RVA: 0x0034375C File Offset: 0x0034195C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__323(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600433E RID: 17214 RVA: 0x0034376C File Offset: 0x0034196C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__324(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600433F RID: 17215 RVA: 0x0034379C File Offset: 0x0034199C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__325(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x003437AC File Offset: 0x003419AC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__326(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x003437DC File Offset: 0x003419DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__327(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004342 RID: 17218 RVA: 0x003437EC File Offset: 0x003419EC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__328(DashboardItem A_0)
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

		// Token: 0x06004343 RID: 17219 RVA: 0x00343824 File Offset: 0x00341A24
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__329(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004344 RID: 17220 RVA: 0x00343834 File Offset: 0x00341A34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__330(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004345 RID: 17221 RVA: 0x00343848 File Offset: 0x00341A48
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__331(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004346 RID: 17222 RVA: 0x00343878 File Offset: 0x00341A78
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__332(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004347 RID: 17223 RVA: 0x00343888 File Offset: 0x00341A88
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__333(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004348 RID: 17224 RVA: 0x003438B8 File Offset: 0x00341AB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__334(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004349 RID: 17225 RVA: 0x003438C8 File Offset: 0x00341AC8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__335(DashboardItem A_0)
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

		// Token: 0x0600434A RID: 17226 RVA: 0x00343900 File Offset: 0x00341B00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__336(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600434B RID: 17227 RVA: 0x00343910 File Offset: 0x00341B10
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__337(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600434C RID: 17228 RVA: 0x00343924 File Offset: 0x00341B24
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__338(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600434D RID: 17229 RVA: 0x00343954 File Offset: 0x00341B54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__339(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600434E RID: 17230 RVA: 0x00343964 File Offset: 0x00341B64
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__340(DashboardItem A_0)
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

		// Token: 0x0600434F RID: 17231 RVA: 0x0034399C File Offset: 0x00341B9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__341(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004350 RID: 17232 RVA: 0x003439AC File Offset: 0x00341BAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__342(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004351 RID: 17233 RVA: 0x003439C0 File Offset: 0x00341BC0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__343(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004352 RID: 17234 RVA: 0x003439F0 File Offset: 0x00341BF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__344(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004353 RID: 17235 RVA: 0x00343A00 File Offset: 0x00341C00
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__345(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004354 RID: 17236 RVA: 0x00343A30 File Offset: 0x00341C30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__346(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004355 RID: 17237 RVA: 0x00343A40 File Offset: 0x00341C40
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__347(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004356 RID: 17238 RVA: 0x00343A70 File Offset: 0x00341C70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__348(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004357 RID: 17239 RVA: 0x00343A80 File Offset: 0x00341C80
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__349(DashboardItem A_0)
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

		// Token: 0x06004358 RID: 17240 RVA: 0x00343AB8 File Offset: 0x00341CB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__350(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004359 RID: 17241 RVA: 0x00343AC8 File Offset: 0x00341CC8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__351(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600435A RID: 17242 RVA: 0x00343ADC File Offset: 0x00341CDC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__352(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600435B RID: 17243 RVA: 0x00343B0C File Offset: 0x00341D0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__353(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600435C RID: 17244 RVA: 0x00343B1C File Offset: 0x00341D1C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__354(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600435D RID: 17245 RVA: 0x00343B4C File Offset: 0x00341D4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__355(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600435E RID: 17246 RVA: 0x00343B5C File Offset: 0x00341D5C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__356(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600435F RID: 17247 RVA: 0x00343B8C File Offset: 0x00341D8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__357(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004360 RID: 17248 RVA: 0x00343B9C File Offset: 0x00341D9C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__358(DashboardItem A_0)
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

		// Token: 0x06004361 RID: 17249 RVA: 0x00343BD4 File Offset: 0x00341DD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__359(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004362 RID: 17250 RVA: 0x00343BE4 File Offset: 0x00341DE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__360(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004363 RID: 17251 RVA: 0x00343BF8 File Offset: 0x00341DF8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__361(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004364 RID: 17252 RVA: 0x00343C28 File Offset: 0x00341E28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__362(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004365 RID: 17253 RVA: 0x00343C38 File Offset: 0x00341E38
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__363(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004366 RID: 17254 RVA: 0x00343C68 File Offset: 0x00341E68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__364(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004367 RID: 17255 RVA: 0x00343C78 File Offset: 0x00341E78
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__365(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004368 RID: 17256 RVA: 0x00343CA8 File Offset: 0x00341EA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__366(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004369 RID: 17257 RVA: 0x00343CB8 File Offset: 0x00341EB8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__367(DashboardItem A_0)
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

		// Token: 0x0600436A RID: 17258 RVA: 0x00343CF0 File Offset: 0x00341EF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__368(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600436B RID: 17259 RVA: 0x00343D00 File Offset: 0x00341F00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__369(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0400289B RID: 10395
		private XyDataSeries fastLineSeries;

		// Token: 0x0400289C RID: 10396
		private DashboardItem _dashItem;

		// Token: 0x0400289D RID: 10397
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;
	}
}
