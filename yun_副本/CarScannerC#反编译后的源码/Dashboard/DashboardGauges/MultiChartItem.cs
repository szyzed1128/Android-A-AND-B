using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
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
	// Token: 0x020007B4 RID: 1972
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\MultiChartItem.xaml")]
	public class MultiChartItem : ContentView
	{
		// Token: 0x0600450E RID: 17678 RVA: 0x0035CABF File Offset: 0x0035ACBF
		public MultiChartItem()
		{
			this.InitializeComponent();
			base.BindingContextChanged += this.ChartItem_BindingContextChanged;
		}

		// Token: 0x0600450F RID: 17679 RVA: 0x0035CAE0 File Offset: 0x0035ACE0
		private void ChartItem_BindingContextChanged(object sender, EventArgs e)
		{
			if (this._dashItem == base.BindingContext)
			{
				return;
			}
			if (base.BindingContext == null && this._dashItem != null)
			{
				if (this._dashItem.Model != null)
				{
					MultiplePIDViewModel multiplePIDViewModel = this._dashItem.Model as MultiplePIDViewModel;
					if (multiplePIDViewModel != null)
					{
						multiplePIDViewModel.InternalModels.CollectionChanged -= this.InternalModels_CollectionChanged;
					}
				}
				this.chart.Series.Clear();
			}
			this._dashItem = base.BindingContext as DashboardItem;
			if (this._dashItem != null && this._dashItem.Model != null)
			{
				MultiplePIDViewModel multiplePIDViewModel2 = this._dashItem.Model as MultiplePIDViewModel;
				if (multiplePIDViewModel2 != null)
				{
					multiplePIDViewModel2.InternalModels.CollectionChanged -= this.InternalModels_CollectionChanged;
					multiplePIDViewModel2.InternalModels.CollectionChanged += this.InternalModels_CollectionChanged;
				}
			}
		}

		// Token: 0x06004510 RID: 17680 RVA: 0x0035CBBC File Offset: 0x0035ADBC
		private void InternalModels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedAction action = e.Action;
			if (action != NotifyCollectionChangedAction.Add)
			{
				if (action != NotifyCollectionChangedAction.Remove)
				{
					goto IL_00DD;
				}
			}
			else
			{
				using (IEnumerator enumerator = e.NewItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						XyDataSeries xyDataSeries = this.ModelToSeries(obj as LiveDataPIDModel);
						this.chart.Series.Add(xyDataSeries);
					}
					return;
				}
			}
			using (IEnumerator enumerator = e.NewItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object item = enumerator.Current;
					ChartSeries chartSeries = this.chart.Series.FirstOrDefault((ChartSeries x) => x.BindingContext == item);
					if (chartSeries != null)
					{
						this.chart.Series.Remove(chartSeries);
					}
				}
				return;
			}
			IL_00DD:
			this.chart.Series.Clear();
			ObservableCollection<LiveDataPIDModel> observableCollection = sender as ObservableCollection<LiveDataPIDModel>;
			if (observableCollection != null)
			{
				foreach (LiveDataPIDModel liveDataPIDModel in observableCollection)
				{
					XyDataSeries xyDataSeries2 = this.ModelToSeries(liveDataPIDModel);
					this.chart.Series.Add(xyDataSeries2);
				}
			}
		}

		// Token: 0x06004511 RID: 17681 RVA: 0x0035CD34 File Offset: 0x0035AF34
		private XyDataSeries ModelToSeries(LiveDataPIDModel model)
		{
			FastLineSeries fastLineSeries = new FastLineSeries();
			if (PlatformHelper.IsiOS)
			{
				fastLineSeries.EnableAnimation = true;
			}
			else if (PlatformHelper.IsAndroid)
			{
				fastLineSeries.EnableAnimation = false;
			}
			fastLineSeries.AnimationDuration = 0.35;
			BindableObjectExtensions.SetBinding(fastLineSeries, ChartSeries.ItemsSourceProperty, "Values", 2, null, null);
			fastLineSeries.XBindingPath = "SecondsAdded";
			fastLineSeries.YBindingPath = "Value";
			fastLineSeries.StrokeWidth = 3.0;
			fastLineSeries.Color = Color.Yellow;
			fastLineSeries.ItemsSource = model.Values;
			fastLineSeries.BindingContext = model;
			return fastLineSeries;
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x0035CDCC File Offset: 0x0035AFCC
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

		// Token: 0x06004513 RID: 17683 RVA: 0x0035CE2C File Offset: 0x0035B02C
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

		// Token: 0x06004514 RID: 17684 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0035CE8C File Offset: 0x0035B08C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MultiChartItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/MultiChartItem.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 21);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 21);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 33);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 33);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 33);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 26);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 25);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 25);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 14);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 17);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 29);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 29);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 29);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 29);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 26);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 26);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 29);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 29);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 29);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 17);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 17);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 26);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 14);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 17);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 17);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 17);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 26);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 31);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 14);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 17);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 17);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 17);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 26);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 31);
			Span span11;
			VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 26);
			FormattedString formattedString5;
			VisualDiagnostics.RegisterSourceInfo(formattedString5 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 22);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 14);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 17);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 17);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 17);
			Span span12;
			VisualDiagnostics.RegisterSourceInfo(span12 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 26);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 31);
			Span span13;
			VisualDiagnostics.RegisterSourceInfo(span13 = new Span(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 26);
			FormattedString formattedString6;
			VisualDiagnostics.RegisterSourceInfo(formattedString6 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 22);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\MultiChartItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 17)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 17)));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 21)));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 25)));
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 25)));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 25)));
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(MultiChartItem).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 17)));
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

		// Token: 0x06004516 RID: 17686 RVA: 0x0036023F File Offset: 0x0035E43F
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MultiChartItem>(this, typeof(MultiChartItem));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00360264 File Offset: 0x0035E464
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__762(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00360294 File Offset: 0x0035E494
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__763(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x06004519 RID: 17689 RVA: 0x003602B0 File Offset: 0x0035E4B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__764(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x003602C0 File Offset: 0x0035E4C0
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__765(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x003602F0 File Offset: 0x0035E4F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__766(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x00360300 File Offset: 0x0035E500
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__767(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00360330 File Offset: 0x0035E530
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__768(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00360340 File Offset: 0x0035E540
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__769(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x00360370 File Offset: 0x0035E570
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__770(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x00360380 File Offset: 0x0035E580
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__771(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x003603B0 File Offset: 0x0035E5B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__772(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x003603C0 File Offset: 0x0035E5C0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__773(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x003603F0 File Offset: 0x0035E5F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__774(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004524 RID: 17700 RVA: 0x00360400 File Offset: 0x0035E600
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__775(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x00360430 File Offset: 0x0035E630
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__776(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004526 RID: 17702 RVA: 0x00360440 File Offset: 0x0035E640
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__777(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004527 RID: 17703 RVA: 0x00360470 File Offset: 0x0035E670
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__778(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004528 RID: 17704 RVA: 0x00360480 File Offset: 0x0035E680
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__779(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004529 RID: 17705 RVA: 0x003604B0 File Offset: 0x0035E6B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__780(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600452A RID: 17706 RVA: 0x003604C0 File Offset: 0x0035E6C0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__781(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600452B RID: 17707 RVA: 0x003604F0 File Offset: 0x0035E6F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__782(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600452C RID: 17708 RVA: 0x00360500 File Offset: 0x0035E700
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__783(DashboardItem A_0)
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

		// Token: 0x0600452D RID: 17709 RVA: 0x00360538 File Offset: 0x0035E738
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__784(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600452E RID: 17710 RVA: 0x00360548 File Offset: 0x0035E748
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__785(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x0036055C File Offset: 0x0035E75C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__786(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x0036058C File Offset: 0x0035E78C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__787(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0036059C File Offset: 0x0035E79C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__788(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x003605CC File Offset: 0x0035E7CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__789(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x003605DC File Offset: 0x0035E7DC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__790(DashboardItem A_0)
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

		// Token: 0x06004534 RID: 17716 RVA: 0x00360614 File Offset: 0x0035E814
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__791(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00360624 File Offset: 0x0035E824
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__792(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004536 RID: 17718 RVA: 0x00360638 File Offset: 0x0035E838
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__793(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004537 RID: 17719 RVA: 0x00360668 File Offset: 0x0035E868
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__794(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x00360678 File Offset: 0x0035E878
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__795(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004539 RID: 17721 RVA: 0x003606A8 File Offset: 0x0035E8A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__796(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600453A RID: 17722 RVA: 0x003606B8 File Offset: 0x0035E8B8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__797(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600453B RID: 17723 RVA: 0x003606E8 File Offset: 0x0035E8E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__799(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600453C RID: 17724 RVA: 0x003606F8 File Offset: 0x0035E8F8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__800(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600453D RID: 17725 RVA: 0x00360728 File Offset: 0x0035E928
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__801(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600453E RID: 17726 RVA: 0x00360738 File Offset: 0x0035E938
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__802(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600453F RID: 17727 RVA: 0x00360768 File Offset: 0x0035E968
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__803(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004540 RID: 17728 RVA: 0x00360778 File Offset: 0x0035E978
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__804(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004541 RID: 17729 RVA: 0x003607A8 File Offset: 0x0035E9A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__805(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004542 RID: 17730 RVA: 0x003607B8 File Offset: 0x0035E9B8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__806(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004543 RID: 17731 RVA: 0x003607E8 File Offset: 0x0035E9E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__807(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004544 RID: 17732 RVA: 0x003607F8 File Offset: 0x0035E9F8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__808(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x00360828 File Offset: 0x0035EA28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__809(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004546 RID: 17734 RVA: 0x00360838 File Offset: 0x0035EA38
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__810(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004547 RID: 17735 RVA: 0x00360868 File Offset: 0x0035EA68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__811(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004548 RID: 17736 RVA: 0x00360878 File Offset: 0x0035EA78
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__812(DashboardItem A_0)
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

		// Token: 0x06004549 RID: 17737 RVA: 0x003608B0 File Offset: 0x0035EAB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__813(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600454A RID: 17738 RVA: 0x003608C0 File Offset: 0x0035EAC0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__814(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600454B RID: 17739 RVA: 0x003608D4 File Offset: 0x0035EAD4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__815(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600454C RID: 17740 RVA: 0x00360904 File Offset: 0x0035EB04
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__816(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600454D RID: 17741 RVA: 0x00360914 File Offset: 0x0035EB14
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__817(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600454E RID: 17742 RVA: 0x00360944 File Offset: 0x0035EB44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__818(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600454F RID: 17743 RVA: 0x00360954 File Offset: 0x0035EB54
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__819(DashboardItem A_0)
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

		// Token: 0x06004550 RID: 17744 RVA: 0x0036098C File Offset: 0x0035EB8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__820(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004551 RID: 17745 RVA: 0x0036099C File Offset: 0x0035EB9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__821(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004552 RID: 17746 RVA: 0x003609B0 File Offset: 0x0035EBB0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__822(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004553 RID: 17747 RVA: 0x003609E0 File Offset: 0x0035EBE0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__823(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004554 RID: 17748 RVA: 0x003609F0 File Offset: 0x0035EBF0
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__824(DashboardItem A_0)
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

		// Token: 0x06004555 RID: 17749 RVA: 0x00360A28 File Offset: 0x0035EC28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__825(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004556 RID: 17750 RVA: 0x00360A38 File Offset: 0x0035EC38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__826(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004557 RID: 17751 RVA: 0x00360A4C File Offset: 0x0035EC4C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__827(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004558 RID: 17752 RVA: 0x00360A7C File Offset: 0x0035EC7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__828(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x00360A8C File Offset: 0x0035EC8C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__829(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600455A RID: 17754 RVA: 0x00360ABC File Offset: 0x0035ECBC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__830(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600455B RID: 17755 RVA: 0x00360ACC File Offset: 0x0035ECCC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__831(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x00360AFC File Offset: 0x0035ECFC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__832(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x00360B0C File Offset: 0x0035ED0C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__833(DashboardItem A_0)
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

		// Token: 0x0600455E RID: 17758 RVA: 0x00360B44 File Offset: 0x0035ED44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__834(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600455F RID: 17759 RVA: 0x00360B54 File Offset: 0x0035ED54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__835(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004560 RID: 17760 RVA: 0x00360B68 File Offset: 0x0035ED68
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__836(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004561 RID: 17761 RVA: 0x00360B98 File Offset: 0x0035ED98
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__837(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004562 RID: 17762 RVA: 0x00360BA8 File Offset: 0x0035EDA8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__838(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004563 RID: 17763 RVA: 0x00360BD8 File Offset: 0x0035EDD8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__839(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004564 RID: 17764 RVA: 0x00360BE8 File Offset: 0x0035EDE8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__840(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004565 RID: 17765 RVA: 0x00360C18 File Offset: 0x0035EE18
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__841(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004566 RID: 17766 RVA: 0x00360C28 File Offset: 0x0035EE28
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__842(DashboardItem A_0)
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

		// Token: 0x06004567 RID: 17767 RVA: 0x00360C60 File Offset: 0x0035EE60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__843(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004568 RID: 17768 RVA: 0x00360C70 File Offset: 0x0035EE70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__844(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004569 RID: 17769 RVA: 0x00360C84 File Offset: 0x0035EE84
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__845(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600456A RID: 17770 RVA: 0x00360CB4 File Offset: 0x0035EEB4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__846(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600456B RID: 17771 RVA: 0x00360CC4 File Offset: 0x0035EEC4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__847(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600456C RID: 17772 RVA: 0x00360CF4 File Offset: 0x0035EEF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__848(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600456D RID: 17773 RVA: 0x00360D04 File Offset: 0x0035EF04
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__849(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600456E RID: 17774 RVA: 0x00360D34 File Offset: 0x0035EF34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__850(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600456F RID: 17775 RVA: 0x00360D44 File Offset: 0x0035EF44
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__851(DashboardItem A_0)
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

		// Token: 0x06004570 RID: 17776 RVA: 0x00360D7C File Offset: 0x0035EF7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__852(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004571 RID: 17777 RVA: 0x00360D8C File Offset: 0x0035EF8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__853(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028D4 RID: 10452
		private DashboardItem _dashItem;

		// Token: 0x040028D5 RID: 10453
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x020007B5 RID: 1973
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06004572 RID: 17778 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06004573 RID: 17779 RVA: 0x00360D9F File Offset: 0x0035EF9F
			internal bool <InternalModels_CollectionChanged>b__0(ChartSeries x)
			{
				return x.BindingContext == this.item;
			}

			// Token: 0x040028D6 RID: 10454
			public object item;
		}
	}
}
