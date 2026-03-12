using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000642 RID: 1602
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\LiveMultiChartPage.xaml")]
	public class LiveMultiChartPage : ContentPage
	{
		// Token: 0x060037A9 RID: 14249 RVA: 0x0029C6C0 File Offset: 0x0029A8C0
		public LiveMultiChartPage(List<PID> pids)
		{
			this.InitializeComponent();
			if (Device.Idiom == 2)
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["tabletTemplate"];
			}
			else
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["mobileTemplate"];
			}
			new Label().BindingContextChanged += this.Lb_BindingContextChanged;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_list.png", delegate
			{
				this.Legend.IsVisible = !this.Legend.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("list", "", delegate
			{
				this.chart.IsVisible = !this.chart.IsVisible;
				this.lv.IsVisible = !this.chart.IsVisible;
				this.zoomModeButton.IsVisible = this.chart.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			this.models = new List<LiveDataPIDModel>(pids.Count);
			List<OBDRequest> list = new List<OBDRequest>(pids.Count);
			foreach (PID pid in pids)
			{
				LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
				liveDataPIDModel.Mode = LiveDataModes.Dashboard;
				liveDataPIDModel.SelectedPID = pid;
				this.models.Add(liveDataPIDModel);
				liveDataPIDModel.GetRequests(list, null, "");
				XyDataSeries xyDataSeries = null;
				if (PlatformHelper.IsiOS)
				{
					xyDataSeries = new FastLineSeries();
					xyDataSeries.EnableAnimation = true;
				}
				else if (PlatformHelper.IsAndroid)
				{
					if (SharedSettings.Current.AndroidChartRenderingSafeMode)
					{
						xyDataSeries = new FastLineSeries
						{
							StrokeDashArray = new double[] { 1.0, 2.0, 3.0 }
						};
					}
					else
					{
						xyDataSeries = new FastLineSeries();
					}
					xyDataSeries.EnableAnimation = false;
				}
				xyDataSeries.AnimationDuration = 0.4;
				xyDataSeries.ItemsSource = liveDataPIDModel.Values;
				xyDataSeries.XBindingPath = "SecondsAdded";
				xyDataSeries.YBindingPath = "Value";
				xyDataSeries.EnableDataPointSelection = true;
				xyDataSeries.Label = pid.ShortName;
				xyDataSeries.IsVisibleOnLegend = true;
				xyDataSeries.ShowTrackballInfo = true;
				xyDataSeries.BindingContext = liveDataPIDModel;
				BindableObjectExtensions.SetBinding(xyDataSeries, ChartSeries.ColorProperty, "ChartLineColor", 2, null, null);
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				this.chart.Series.Add(xyDataSeries);
			}
			this.lv.ItemsSource = this.models;
			if (SharedSettings.Current.AlwaysRecordFuelConsumption)
			{
				PID pid2 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
				if (pid2 != null)
				{
					LiveDataPIDModel.GetRequests(pid2, list, null, "");
				}
			}
			List<OBDRequest> requestsForDelegate = new List<OBDRequest>(list);
			MainAppRequestProducer.Delegate = delegate(List<OBDRequest> delegateRequests)
			{
				delegateRequests.AddRange(requestsForDelegate);
			};
			RequestProducerStatic.UpdateOBDReaderRequests();
			if (SharedSettings.Current.MultiChartPauseHidden)
			{
				foreach (ChartSeries chartSeries in this.chart.Series)
				{
					chartSeries.PropertyChanged += this.Lineseries_PropertyChanged;
				}
			}
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x0029CA90 File Offset: 0x0029AC90
		private void Chart_SizeChanged(object sender, EventArgs e)
		{
			foreach (Grid grid in this.legendGrids)
			{
				grid.WidthRequest = this.chart.Width;
				grid.MinimumWidthRequest = this.chart.Width;
			}
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0029CAFC File Offset: 0x0029ACFC
		private void Lb_BindingContextChanged(object sender, EventArgs e)
		{
			Label label = sender as Label;
			object bindingContext = (label.BindingContext as ChartLegendItem).Series.BindingContext;
			label.BindingContextChanged -= this.Lb_BindingContextChanged;
			BindableObjectExtensions.SetBinding(label, Label.TextProperty, "TextValue", 2, null, null);
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0029CB4C File Offset: 0x0029AD4C
		private void spanValue_ContextChanged(object sender, EventArgs e)
		{
			Span span = sender as Span;
			LiveDataPIDModel liveDataPIDModel = (span.BindingContext as ChartLegendItem).Series.BindingContext as LiveDataPIDModel;
			span.BindingContextChanged -= this.spanValue_ContextChanged;
			span.BindingContext = liveDataPIDModel;
			BindableObjectExtensions.SetBinding(span, Span.TextProperty, "TextValue", 2, null, null);
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x0029CBA5 File Offset: 0x0029ADA5
		private void legendGrid_BindingContextChanged(object sender, EventArgs e)
		{
			this.legendGrids.Add((Grid)sender);
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x0029CBB8 File Offset: 0x0029ADB8
		private void spanUnits_ContextChanged(object sender, EventArgs e)
		{
			Span span = sender as Span;
			LiveDataPIDModel liveDataPIDModel = (span.BindingContext as ChartLegendItem).Series.BindingContext as LiveDataPIDModel;
			span.BindingContextChanged -= this.spanUnits_ContextChanged;
			span.BindingContext = liveDataPIDModel;
			BindableObjectExtensions.SetBinding(span, Span.TextProperty, "Units", 4, null, null);
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0029CC14 File Offset: 0x0029AE14
		private void Lineseries_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == ChartSeries.IsVisibleProperty.PropertyName)
			{
				ChartSeries[] array = this.chart.Series.Where((ChartSeries x) => x.IsVisible).ToArray<ChartSeries>();
				List<OBDRequest> list = new List<OBDRequest>(array.Length);
				ChartSeries[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					(array2[i].BindingContext as LiveDataPIDModel).GetRequests(list, null, "");
				}
				if (SharedSettings.Current.AlwaysRecordFuelConsumption)
				{
					PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
					if (pid != null)
					{
						LiveDataPIDModel.GetRequests(pid, list, null, "");
					}
				}
				list = list.OrderBy((OBDRequest x) => x.Header).ToList<OBDRequest>();
				IEnumerable<OBDRequest> enumerable = OBDRequestQueueOptimizer.Optimize(list);
				App.OBDReader.ReplaceQueue(enumerable);
			}
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x002967AC File Offset: 0x002949AC
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MultiChartTitle"), Translate.GetString("ios_MultiChart_Info"), "OK");
		}

		// Token: 0x060037B1 RID: 14257 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060037B2 RID: 14258 RVA: 0x0029CD28 File Offset: 0x0029AF28
		private void ChangeZoomMode()
		{
			if (this.zoomBehave.ZoomMode == null)
			{
				this.zoomBehave.ZoomMode = 2;
				this.zoomModeButton.Text = "XY";
				return;
			}
			if (this.zoomBehave.ZoomMode == 2)
			{
				this.zoomBehave.ZoomMode = 1;
				this.zoomModeButton.Text = "Y";
				return;
			}
			if (this.zoomBehave.ZoomMode == 1)
			{
				this.zoomBehave.ZoomMode = 0;
				this.zoomModeButton.Text = "X";
			}
		}

		// Token: 0x060037B3 RID: 14259 RVA: 0x0029CDB4 File Offset: 0x0029AFB4
		private void zoomModeButton_Clicked(object sender, EventArgs e)
		{
			this.ChangeZoomMode();
		}

		// Token: 0x060037B4 RID: 14260 RVA: 0x0029CDBC File Offset: 0x0029AFBC
		private void numAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			int num;
			if (int.TryParse(e.LabelContent, out num))
			{
				int num2 = num / 60;
				num %= 60;
				e.LabelContent = num2.ToString(CultureInfo.InvariantCulture) + ":" + num.ToString("00", CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x060037B5 RID: 14261 RVA: 0x0029CE10 File Offset: 0x0029B010
		private void NumericalAxisActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
			double num = (double)e.ActualMaximum - (double)SharedSettings.Current.LiveDataShowTime;
			e.ActualMinimum = num;
		}

		// Token: 0x060037B6 RID: 14262 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x060037B7 RID: 14263 RVA: 0x0029CE44 File Offset: 0x0029B044
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveMultiChartPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LiveMultiChartPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			Color red = Color.Red;
			Color green = Color.Green;
			Color blue = Color.Blue;
			Color orange = Color.Orange;
			Color pink = Color.Pink;
			Color color = new Color(0.6000000238418579, 0.06666667014360428, 0.0941176488995552, 1.0);
			Color color2 = new Color(0.6000000238418579, 0.3294117748737335, 0.47058823704719543, 1.0);
			Color color3 = new Color(0.5882353186607361, 0.6000000238418579, 0.3294117748737335, 1.0);
			Color color4 = new Color(0.6000000238418579, 0.3607843220233917, 0.3294117748737335, 1.0);
			Color color5 = new Color(0.5803921818733215, 0.6000000238418579, 0.529411792755127, 1.0);
			Color color6 = new Color(0.08627451211214066, 0.6117647290229797, 0.800000011920929, 1.0);
			Color color7 = new Color(0.6823529601097107, 0.800000011920929, 0.08627451211214066, 1.0);
			Color color8 = new Color(0.08627451211214066, 0.800000011920929, 0.4901960790157318, 1.0);
			Color color9 = new Color(0.6470588445663452, 0.08627451211214066, 0.800000011920929, 1.0);
			Color color10 = new Color(0.08627451211214066, 0.7764706015586853, 0.800000011920929, 1.0);
			Color color11 = new Color(0.08627451211214066, 0.3607843220233917, 0.800000011920929, 1.0);
			Color color12 = new Color(0.800000011920929, 0.46666666865348816, 0.1764705926179886, 1.0);
			Color color13 = new Color(0.43921568989753723, 0.5372549295425415, 0.800000011920929, 1.0);
			Color color14 = new Color(0.800000011920929, 0.6549019813537598, 0.529411792755127, 1.0);
			Color color15 = new Color(0.7098039388656616, 0.529411792755127, 0.800000011920929, 1.0);
			Color color16 = new Color(1.0, 0.3176470696926117, 0.10980392247438431, 1.0);
			Color color17 = new Color(1.0, 0.10980392247438431, 0.3607843220233917, 1.0);
			Color color18 = new Color(0.10980392247438431, 0.24313725531101227, 1.0, 1.0);
			Color color19 = new Color(1.0, 0.21960784494876862, 0.7137255072593689, 1.0);
			Color color20 = new Color(0.41960784792900085, 1.0, 0.3294117748737335, 1.0);
			Color color21 = new Color(1.0, 0.8313725590705872, 0.43921568989753723, 1.0);
			Color color22 = new Color(1.0, 0.6549019813537598, 0.5490196347236633, 1.0);
			Color color23 = new Color(1.0, 0.5490196347236633, 0.6784313917160034, 1.0);
			Color color24 = new Color(1.0, 0.7686274647712708, 0.7803921699523926, 1.0);
			Color color25 = new Color(0.8274509906768799, 1.0, 0.7686274647712708, 1.0);
			Color color26 = new Color(0.9921568632125854, 1.0, 0.8784313797950745, 1.0);
			Color color27 = new Color(0.9921568632125854, 1.0, 0.9882352948188782, 1.0);
			Color darkOrange = Color.DarkOrange;
			Color darkGreen = Color.DarkGreen;
			Color darkRed = Color.DarkRed;
			ChartColorCollection chartColorCollection;
			VisualDiagnostics.RegisterSourceInfo(chartColorCollection = new ChartColorCollection(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 44);
			ChartColorModel chartColorModel;
			VisualDiagnostics.RegisterSourceInfo(chartColorModel = new ChartColorModel(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 25);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 22);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 22);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 30);
			ChartLegend chartLegend;
			VisualDiagnostics.RegisterSourceInfo(chartLegend = new ChartLegend(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 14);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 14);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("chart", sfChart);
			if (sfChart.StyleId == null)
			{
				sfChart.StyleId = "chart";
			}
			nameScope.RegisterName("xaxis", numericalAxis);
			if (numericalAxis.StyleId == null)
			{
				numericalAxis.StyleId = "xaxis";
			}
			nameScope.RegisterName("zoomBehave", chartZoomPanBehavior);
			if (chartZoomPanBehavior.StyleId == null)
			{
				chartZoomPanBehavior.StyleId = "zoomBehave";
			}
			nameScope.RegisterName("Legend", chartLegend);
			if (chartLegend.StyleId == null)
			{
				chartLegend.StyleId = "Legend";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("zoomModeButton", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "zoomModeButton";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.zoomBehave = chartZoomPanBehavior;
			this.Legend = chartLegend;
			this.lv = listView;
			this.zoomModeButton = linkButton;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			chartColorCollection.Add(red);
			chartColorCollection.Add(green);
			chartColorCollection.Add(blue);
			chartColorCollection.Add(orange);
			chartColorCollection.Add(pink);
			chartColorCollection.Add(color);
			chartColorCollection.Add(color2);
			chartColorCollection.Add(color3);
			chartColorCollection.Add(color4);
			chartColorCollection.Add(color5);
			chartColorCollection.Add(color6);
			chartColorCollection.Add(color7);
			chartColorCollection.Add(color8);
			chartColorCollection.Add(color9);
			chartColorCollection.Add(color10);
			chartColorCollection.Add(color11);
			chartColorCollection.Add(color12);
			chartColorCollection.Add(color13);
			chartColorCollection.Add(color14);
			chartColorCollection.Add(color15);
			chartColorCollection.Add(color16);
			chartColorCollection.Add(color17);
			chartColorCollection.Add(color18);
			chartColorCollection.Add(color19);
			chartColorCollection.Add(color20);
			chartColorCollection.Add(color21);
			chartColorCollection.Add(color22);
			chartColorCollection.Add(color23);
			chartColorCollection.Add(color24);
			chartColorCollection.Add(color25);
			chartColorCollection.Add(color26);
			chartColorCollection.Add(color27);
			chartColorCollection.Add(darkOrange);
			chartColorCollection.Add(darkGreen);
			chartColorCollection.Add(darkRed);
			resourceDictionary.Add("Colors", chartColorCollection);
			IDataTemplate dataTemplate4 = dataTemplate;
			LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_66 <InitializeComponent>_anonXamlCDataTemplate_ = new LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_66();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("mobileTemplate", dataTemplate);
			IDataTemplate dataTemplate5 = dataTemplate2;
			LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_67 <InitializeComponent>_anonXamlCDataTemplate_2 = new LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_67();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			resourceDictionary.Add("tabletTemplate", dataTemplate2);
			translate.Text = "ios_MultiChartTitle";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array3, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 1];
			array4[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			sfChart.SetValue(Grid.RowProperty, 0);
			sfChart.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = sfChart;
			array5[1] = grid;
			array5[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array5, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = sfChart;
			array6[1] = grid;
			array6[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			sfChart.SizeChanged += this.Chart_SizeChanged;
			staticResourceExtension.Key = "Colors";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = chartColorModel;
			array7[1] = sfChart;
			array7[2] = grid;
			array7[3] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array7, ChartColorModel.CustomBrushesProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 44)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			chartColorModel.CustomBrushes = obj7;
			chartColorModel.SetValue(ChartColorModel.PaletteProperty, 5);
			sfChart.SetValue(SfChart.ColorModelProperty, chartColorModel);
			numericalAxis.ActualRangeChanged += this.NumericalAxisActualRangeChanged;
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, false);
			numericalAxis.SetValue(NumericalAxis.IntervalProperty, new double?(5.0));
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			staticResourceExtension2.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension6 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = numericalAxis;
			array8[1] = sfChart;
			array8[2] = grid;
			array8[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array8, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 25)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			numericalAxis.MajorGridLineStyle = obj9;
			staticResourceExtension3.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = numericalAxis;
			array9[1] = sfChart;
			array9[2] = grid;
			array9[3] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array9, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 25)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			numericalAxis.MajorTickStyle = obj11;
			dynamicResourceExtension4.Key = "GaugeLabelColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = chartAxisLabelStyle;
			array10[1] = numericalAxis;
			array10[2] = sfChart;
			array10[3] = grid;
			array10[4] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array10, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 56)));
			DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource4.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 0);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 0);
			numericalAxis2.LabelCreated += this.valueAxis_LabelCreated;
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension4.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = numericalAxis2;
			array11[1] = sfChart;
			array11[2] = grid;
			array11[3] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array11, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 25)));
			object obj14 = markupExtension9.ProvideValue(xamlServiceProvider9);
			numericalAxis2.MajorGridLineStyle = obj14;
			staticResourceExtension5.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension10 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = numericalAxis2;
			array12[1] = sfChart;
			array12[2] = grid;
			array12[3] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array12, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 25)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			numericalAxis2.MajorTickStyle = obj16;
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			dynamicResourceExtension5.Key = "GaugeLabelColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = chartAxisLabelStyle2;
			array13[1] = numericalAxis2;
			array13[2] = sfChart;
			array13[3] = grid;
			array13[4] = this;
			object obj17;
			xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array13, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LiveMultiChartPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(177, 56)));
			DynamicResource dynamicResource5 = markupExtension11.ProvideValue(xamlServiceProvider11);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource5.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableDoubleTapProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnablePanningProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableZoomingProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.ZoomModeProperty, 0);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartZoomPanBehavior);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.LabelDisplayModeProperty, 1);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLabelProperty, true);
			chartTrackballBehavior.SetValue(ChartTrackballBehavior.ShowLineProperty, true);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartTrackballBehavior);
			chartLegend.SetValue(ChartLegend.DockPositionProperty, 3);
			chartLegend.SetValue(ChartLegend.IsVisibleProperty, true);
			chartLegend.SetValue(ChartLegend.OrientationProperty, 2);
			chartLegend.SetValue(ChartLegend.OverflowModeProperty, 1);
			chartLegend.SetValue(ChartLegend.ToggleSeriesVisibilityProperty, true);
			IDataTemplate dataTemplate6 = dataTemplate3;
			LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_68 <InitializeComponent>_anonXamlCDataTemplate_3 = new LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_68();
			object[] array14 = new object[0 + 5];
			array14[0] = dataTemplate3;
			array14[1] = chartLegend;
			array14[2] = sfChart;
			array14[3] = grid;
			array14[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array14;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			chartLegend.SetValue(ChartLegend.ItemTemplateProperty, dataTemplate3);
			sfChart.SetValue(SfChart.LegendProperty, chartLegend);
			grid.Children.Add(sfChart);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			listView.SetValue(ListView.SelectionModeProperty, 0);
			grid.Children.Add(listView);
			linkButton.SetValue(Grid.RowProperty, 0);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(5.0));
			linkButton.Clicked += this.zoomModeButton_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			linkButton.SetValue(Button.TextProperty, "X");
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid.Children.Add(linkButton);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x060037B8 RID: 14264 RVA: 0x0029EF69 File Offset: 0x0029D169
		[CompilerGenerated]
		private void <.ctor>b__1_0()
		{
			this.Legend.IsVisible = !this.Legend.IsVisible;
		}

		// Token: 0x060037B9 RID: 14265 RVA: 0x0029EF84 File Offset: 0x0029D184
		[CompilerGenerated]
		private void <.ctor>b__1_1()
		{
			this.chart.IsVisible = !this.chart.IsVisible;
			this.lv.IsVisible = !this.chart.IsVisible;
			this.zoomModeButton.IsVisible = this.chart.IsVisible;
		}

		// Token: 0x060037BA RID: 14266 RVA: 0x0029EFD9 File Offset: 0x0029D1D9
		[CompilerGenerated]
		private void <.ctor>b__1_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x060037BB RID: 14267 RVA: 0x0029EFE4 File Offset: 0x0029D1E4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveMultiChartPage>(this, typeof(LiveMultiChartPage));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
			this.Legend = NameScopeExtensions.FindByName<ChartLegend>(this, "Legend");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.zoomModeButton = NameScopeExtensions.FindByName<LinkButton>(this, "zoomModeButton");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x040021B3 RID: 8627
		private List<LiveDataPIDModel> models = new List<LiveDataPIDModel>();

		// Token: 0x040021B4 RID: 8628
		private List<Grid> legendGrids = new List<Grid>();

		// Token: 0x040021B5 RID: 8629
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x040021B6 RID: 8630
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x040021B7 RID: 8631
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x040021B8 RID: 8632
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartLegend Legend;

		// Token: 0x040021B9 RID: 8633
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x040021BA RID: 8634
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton zoomModeButton;

		// Token: 0x040021BB RID: 8635
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000643 RID: 1603
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060037BC RID: 14268 RVA: 0x0029F079 File Offset: 0x0029D279
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060037BD RID: 14269 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060037BE RID: 14270 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <.ctor>b__1_4(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x060037BF RID: 14271 RVA: 0x00298DE4 File Offset: 0x00296FE4
			internal bool <Lineseries_PropertyChanged>b__8_0(ChartSeries x)
			{
				return x.IsVisible;
			}

			// Token: 0x060037C0 RID: 14272 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <Lineseries_PropertyChanged>b__8_2(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x060037C1 RID: 14273 RVA: 0x001DF55B File Offset: 0x001DD75B
			internal string <Lineseries_PropertyChanged>b__8_1(OBDRequest x)
			{
				return x.Header;
			}

			// Token: 0x040021BC RID: 8636
			public static readonly LiveMultiChartPage.<>c <>9 = new LiveMultiChartPage.<>c();

			// Token: 0x040021BD RID: 8637
			public static Func<PID, bool> <>9__1_4;

			// Token: 0x040021BE RID: 8638
			public static Func<ChartSeries, bool> <>9__8_0;

			// Token: 0x040021BF RID: 8639
			public static Func<PID, bool> <>9__8_2;

			// Token: 0x040021C0 RID: 8640
			public static Func<OBDRequest, string> <>9__8_1;
		}

		// Token: 0x02000644 RID: 1604
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x060037C2 RID: 14274 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x060037C3 RID: 14275 RVA: 0x0029F085 File Offset: 0x0029D285
			internal void <.ctor>b__3(List<OBDRequest> delegateRequests)
			{
				delegateRequests.AddRange(this.requestsForDelegate);
			}

			// Token: 0x040021C1 RID: 8641
			public List<OBDRequest> requestsForDelegate;
		}

		// Token: 0x02000645 RID: 1605
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_66
		{
			// Token: 0x060037C4 RID: 14276 RVA: 0x0029F094 File Offset: 0x0029D294
			public <InitializeComponent>_anonXamlCDataTemplate_66()
			{
			}

			// Token: 0x060037C5 RID: 14277 RVA: 0x0029F0A8 File Offset: 0x0029D2A8
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 30);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 30);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 45);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 26);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 43);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 38);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 38);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 43);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 38);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 26);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 29);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 29);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 43);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 38);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 38);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 34);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				label3.SetValue(Grid.RowProperty, 0);
				label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize---";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label3;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_66).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "Settings.ShowPing";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				label3.SetValue(Label.TextColorProperty, Color.Red);
				label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "Ping";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				span4.SetBinding(Span.TextProperty, bindingBase5);
				formattedString2.Spans.Add(span4);
				span5.SetValue(Span.TextProperty, "ms");
				formattedString2.Spans.Add(span5);
				label3.SetValue(Label.FormattedTextProperty, formattedString2);
				grid.Children.Add(label3);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040021C2 RID: 8642
			internal object[] parentValues;

			// Token: 0x040021C3 RID: 8643
			internal LiveMultiChartPage root;
		}

		// Token: 0x02000646 RID: 1606
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_67
		{
			// Token: 0x060037C6 RID: 14278 RVA: 0x0029F82C File Offset: 0x0029DA2C
			public <InitializeComponent>_anonXamlCDataTemplate_67()
			{
			}

			// Token: 0x060037C7 RID: 14279 RVA: 0x0029F840 File Offset: 0x0029DA40
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 30);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 29);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 26);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 43);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 38);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 38);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 43);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 38);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_67).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "SelectedPID.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_67).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 29)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TextValue";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040021C4 RID: 8644
			internal object[] parentValues;

			// Token: 0x040021C5 RID: 8645
			internal LiveMultiChartPage root;
		}

		// Token: 0x02000647 RID: 1607
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_68
		{
			// Token: 0x060037C8 RID: 14280 RVA: 0x0029FF10 File Offset: 0x0029E110
			public <InitializeComponent>_anonXamlCDataTemplate_68()
			{
			}

			// Token: 0x060037C9 RID: 14281 RVA: 0x0029FF24 File Offset: 0x0029E124
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 41);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 60);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 55);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 50);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 50);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 50);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 50);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 50);
				Span span6;
				VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveMultiChartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				grid.BindingContextChanged += this.root.legendGrid_BindingContextChanged;
				grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
				grid.SetValue(VisualElement.WidthRequestProperty, 200.0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				boxView.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Path = "IconColor";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				boxView.SetBinding(VisualElement.BackgroundColorProperty, bindingBase);
				boxView.SetValue(VisualElement.HeightRequestProperty, 10.0);
				boxView.SetValue(VisualElement.WidthRequestProperty, 10.0);
				bindingExtension2.Path = "IconColor";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				boxView.SetBinding(BoxView.ColorProperty, bindingBase2);
				grid.Children.Add(boxView);
				label.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension.Key = "BaseFontSize-";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveMultiChartPage.<InitializeComponent>_anonXamlCDataTemplate_68).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(233, 60)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.FontSize = (double)obj2;
				bindingExtension3.Mode = 4;
				bindingExtension3.Path = "Label";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " [");
				formattedString.Spans.Add(span2);
				span3.BindingContextChanged += this.root.spanValue_ContextChanged;
				formattedString.Spans.Add(span3);
				span4.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span4);
				span5.BindingContextChanged += this.root.spanUnits_ContextChanged;
				formattedString.Spans.Add(span5);
				span6.SetValue(Span.TextProperty, "]");
				formattedString.Spans.Add(span6);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label);
				return grid;
			}

			// Token: 0x040021C6 RID: 8646
			internal object[] parentValues;

			// Token: 0x040021C7 RID: 8647
			internal LiveMultiChartPage root;
		}
	}
}
