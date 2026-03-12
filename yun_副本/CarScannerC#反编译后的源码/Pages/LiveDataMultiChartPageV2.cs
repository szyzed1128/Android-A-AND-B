using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.ListView.XForms;
using Syncfusion.SfChart.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000633 RID: 1587
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\LiveDataMultiChartPageV2.xaml")]
	public class LiveDataMultiChartPageV2 : ContentPage
	{
		// Token: 0x06003740 RID: 14144 RVA: 0x00296580 File Offset: 0x00294780
		public LiveDataMultiChartPageV2(List<IPIDFloatValue> pids, Page selectorPage)
		{
			this.InitializeComponent();
			this.selectorPage = selectorPage;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			if (Device.Idiom == 2)
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["tabletTemplate"];
			}
			else
			{
				this.lv.ItemTemplate = (DataTemplate)base.Resources["mobileTemplate"];
			}
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_list.png", delegate
			{
				this.Legend.IsVisible = !this.Legend.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("list", "", delegate
			{
				this.chart.IsVisible = !this.chart.IsVisible;
				this.lv.IsVisible = !this.chart.IsVisible;
			}, 0, 0));
			this.zoomModeButton = new ToolbarItem("X", "", delegate
			{
				this.ChangeZoomMode();
			}, 0, 0);
			base.ToolbarItems.Add(this.zoomModeButton);
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			this.BuildCharts(pids);
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x002966E4 File Offset: 0x002948E4
		private void spanUnits_ContextChanged(object sender, EventArgs e)
		{
			Span span = sender as Span;
			LiveDataPIDModel liveDataPIDModel = (span.BindingContext as ChartLegendItem).Series.BindingContext as LiveDataPIDModel;
			span.BindingContextChanged -= this.spanUnits_ContextChanged;
			span.BindingContext = liveDataPIDModel;
			BindableObjectExtensions.SetBinding(span, Span.TextProperty, "Units", 4, null, null);
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x00296740 File Offset: 0x00294940
		private void spanValue_ContextChanged(object sender, EventArgs e)
		{
			Span span = sender as Span;
			LiveDataPIDModel liveDataPIDModel = (span.BindingContext as ChartLegendItem).Series.BindingContext as LiveDataPIDModel;
			span.BindingContextChanged -= this.spanValue_ContextChanged;
			span.BindingContext = liveDataPIDModel;
			BindableObjectExtensions.SetBinding(span, Span.TextProperty, "TextValue", 2, null, null);
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x00296799 File Offset: 0x00294999
		private void legendGrid_BindingContextChanged(object sender, EventArgs e)
		{
			this.legendGrids.Add((Grid)sender);
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x002967AC File Offset: 0x002949AC
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MultiChartTitle"), Translate.GetString("ios_MultiChart_Info"), "OK");
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x002967CE File Offset: 0x002949CE
		private void zoomModeButton_Clicked(object sender, EventArgs e)
		{
			this.ChangeZoomMode();
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x002967D8 File Offset: 0x002949D8
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

		// Token: 0x06003747 RID: 14151 RVA: 0x00296864 File Offset: 0x00294A64
		private void BuildCharts(List<IPIDFloatValue> pids)
		{
			UnitsHelper.Units[] array = pids.Select((IPIDFloatValue x) => x.Units).Distinct<UnitsHelper.Units>().ToArray<UnitsHelper.Units>();
			UnitsHelper.Units leftUnit = array[0];
			UnitsHelper.Units rightUnit = array[1];
			List<IPIDFloatValue> list = pids.Where((IPIDFloatValue x) => x.Units == leftUnit).ToList<IPIDFloatValue>();
			List<IPIDFloatValue> list2 = pids.Where((IPIDFloatValue x) => x.Units == rightUnit).ToList<IPIDFloatValue>();
			ChartAxisTitle chartAxisTitle = new ChartAxisTitle
			{
				Text = UnitsHelper.GetCaption(leftUnit),
				Margin = new Thickness(1.0, 0.0, 0.0, 0.0),
				FontAttributes = 1
			};
			chartAxisTitle.SetDynamicResource(ChartAxisTitle.TextColorProperty, "ButtonBackgroundColor");
			ChartAxisTitle chartAxisTitle2 = new ChartAxisTitle
			{
				Text = UnitsHelper.GetCaption(rightUnit),
				Margin = new Thickness(0.0, 0.0, 1.0, 0.0),
				FontAttributes = 1
			};
			chartAxisTitle2.SetDynamicResource(ChartAxisTitle.TextColorProperty, "ButtonBackgroundColor");
			NumericalAxis numericalAxis = new NumericalAxis
			{
				EdgeLabelsDrawingMode = 0,
				EdgeLabelsVisibilityMode = 0,
				LabelsIntersectAction = 0,
				RangePadding = 2,
				OpposedPosition = false,
				Title = chartAxisTitle,
				TickPosition = 0,
				LabelStyle = new ChartAxisLabelStyle
				{
					LabelFormat = "0.###"
				},
				MajorGridLineStyle = (ChartLineStyle)Application.Current.Resources["DefaultChartGirdLineStyle"],
				MajorTickStyle = (ChartAxisTickStyle)Application.Current.Resources["DefaultChartGridTickStyle"]
			};
			numericalAxis.LabelCreated += this.valueAxis_LabelCreated;
			NumericalAxis numericalAxis2 = new NumericalAxis
			{
				EdgeLabelsDrawingMode = 0,
				EdgeLabelsVisibilityMode = 0,
				LabelsIntersectAction = 0,
				RangePadding = 2,
				OpposedPosition = true,
				Title = chartAxisTitle2,
				TickPosition = 0,
				LabelStyle = new ChartAxisLabelStyle
				{
					LabelFormat = "0.###"
				}
			};
			numericalAxis.LabelCreated += this.valueAxis_LabelCreated;
			this.chart.SecondaryAxis = numericalAxis;
			this.chart.Axes.Add(numericalAxis2);
			if (this.chart.Series == null)
			{
				this.chart.Series = new ChartSeriesCollection();
			}
			List<ValueTuple<LiveDataPIDModel, XyDataSeries>> list3 = this.CreateModelsAndSeries(list, numericalAxis);
			List<ValueTuple<LiveDataPIDModel, XyDataSeries>> list4 = this.CreateModelsAndSeries(list2, numericalAxis2);
			List<LiveDataPIDModel> list5 = new List<LiveDataPIDModel>(pids.Count);
			list5.AddRange(list3.Select((ValueTuple<LiveDataPIDModel, XyDataSeries> x) => x.Item1));
			list5.AddRange(list4.Select((ValueTuple<LiveDataPIDModel, XyDataSeries> x) => x.Item1));
			this.lv.ItemsSource = list5;
			List<XyDataSeries> list6 = new List<XyDataSeries>(pids.Count);
			list6.AddRange(list3.Select((ValueTuple<LiveDataPIDModel, XyDataSeries> x) => x.Item2));
			list6.AddRange(list4.Select((ValueTuple<LiveDataPIDModel, XyDataSeries> x) => x.Item2));
			foreach (XyDataSeries xyDataSeries in list6)
			{
				this.chart.Series.Add(xyDataSeries);
			}
			List<OBDRequest> list7 = new List<OBDRequest>(pids.Count);
			foreach (LiveDataPIDModel liveDataPIDModel in list5)
			{
				liveDataPIDModel.GetRequests(list7, null, "");
			}
			LiveDataMultiChartSelector liveDataMultiChartSelector = this.selectorPage as LiveDataMultiChartSelector;
			if (liveDataMultiChartSelector != null)
			{
				liveDataMultiChartSelector.CacheModelsList.AddRange(list5);
			}
			else
			{
				LiveDataMultiChartSelectorPageV3 liveDataMultiChartSelectorPageV = this.selectorPage as LiveDataMultiChartSelectorPageV3;
				if (liveDataMultiChartSelectorPageV != null)
				{
					liveDataMultiChartSelectorPageV.CacheModelsList.AddRange(list5);
				}
			}
			List<OBDRequest> requestsForDelegate = new List<OBDRequest>(list7);
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

		// Token: 0x06003748 RID: 14152 RVA: 0x00296D30 File Offset: 0x00294F30
		private List<ValueTuple<LiveDataPIDModel, XyDataSeries>> CreateModelsAndSeries(List<IPIDFloatValue> pids, NumericalAxis yAxis)
		{
			List<ValueTuple<LiveDataPIDModel, XyDataSeries>> list = new List<ValueTuple<LiveDataPIDModel, XyDataSeries>>(pids.Count);
			foreach (IPIDFloatValue ipidfloatValue in pids)
			{
				LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
				liveDataPIDModel.Mode = LiveDataModes.Dashboard;
				liveDataPIDModel.SelectedPID = ipidfloatValue;
				XyDataSeries xyDataSeries = null;
				ChartItemTypes chartDisplayStyle = SharedSettings.Current.ChartDisplayStyle;
				if (chartDisplayStyle > ChartItemTypes.Area)
				{
					if (chartDisplayStyle - ChartItemTypes.SplineLine <= 1)
					{
						xyDataSeries = new SplineSeries
						{
							SplineType = 1
						};
					}
				}
				else if (PlatformHelper.IsiOS)
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
				xyDataSeries.ItemsSource = liveDataPIDModel.Values;
				xyDataSeries.YAxis = yAxis;
				xyDataSeries.XBindingPath = "SecondsAdded";
				xyDataSeries.YBindingPath = "Value";
				xyDataSeries.EnableDataPointSelection = true;
				xyDataSeries.Label = ipidfloatValue.ShortName;
				xyDataSeries.SetBinding(ChartSeries.LabelProperty, new Binding("TextValue", 2, new LiveDataPidModelToStringNameValueUnits(), liveDataPIDModel, null, null));
				xyDataSeries.IsVisibleOnLegend = true;
				xyDataSeries.ShowTrackballInfo = true;
				xyDataSeries.BindingContext = liveDataPIDModel;
				BindableObjectExtensions.SetBinding(xyDataSeries, ChartSeries.ColorProperty, "ChartLineColor", 2, null, null);
				list.Add(new ValueTuple<LiveDataPIDModel, XyDataSeries>(liveDataPIDModel, xyDataSeries));
			}
			return list;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x00296ED4 File Offset: 0x002950D4
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

		// Token: 0x0600374B RID: 14155 RVA: 0x00296FE8 File Offset: 0x002951E8
		private void NumericalAxisActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
			double num = (double)e.ActualMaximum - (double)SharedSettings.Current.LiveDataShowTime;
			e.ActualMinimum = num;
		}

		// Token: 0x0600374C RID: 14156 RVA: 0x0029701C File Offset: 0x0029521C
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

		// Token: 0x0600374D RID: 14157 RVA: 0x00297070 File Offset: 0x00295270
		private void ContentPage_SizeChanged(object sender, EventArgs e)
		{
			switch (DeviceDisplay.MainDisplayInfo.Rotation)
			{
			case 0:
			case 1:
			case 3:
				this.Legend.DockPosition = 3;
				this.Legend.BackgroundColor = (Color)Application.Current.Resources["BackgroundColor"];
				this.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width;
				return;
			case 2:
			case 4:
				this.Legend.DockPosition = 4;
				this.Legend.BackgroundColor = ((Color)Application.Current.Resources["BackgroundColor"]).MultiplyAlpha(0.5);
				this.Legend.MaxWidth = DeviceDisplay.MainDisplayInfo.Width * 0.25;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00297154 File Offset: 0x00295354
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LiveDataMultiChartPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			LiveDataPidModelToStringNameValueUnits liveDataPidModelToStringNameValueUnits;
			VisualDiagnostics.RegisterSourceInfo(liveDataPidModelToStringNameValueUnits = new LiveDataPidModelToStringNameValueUnits(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
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
			VisualDiagnostics.RegisterSourceInfo(chartColorCollection = new ChartColorCollection(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 44);
			ChartColorModel chartColorModel;
			VisualDiagnostics.RegisterSourceInfo(chartColorModel = new ChartColorModel(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 22);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 30);
			ChartLegend chartLegend;
			VisualDiagnostics.RegisterSourceInfo(chartLegend = new ChartLegend(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 22);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 14);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("Legend", chartLegend);
			if (chartLegend.StyleId == null)
			{
				chartLegend.StyleId = "Legend";
			}
			nameScope.RegisterName("zoomBehave", chartZoomPanBehavior);
			if (chartZoomPanBehavior.StyleId == null)
			{
				chartZoomPanBehavior.StyleId = "zoomBehave";
			}
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.Legend = chartLegend;
			this.zoomBehave = chartZoomPanBehavior;
			this.lv = sfListView;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			resourceDictionary.Add("LiveDataPidModelToStringNameValueUnits", liveDataPidModelToStringNameValueUnits);
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
			LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_62 <InitializeComponent>_anonXamlCDataTemplate_ = new LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_62();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("mobileTemplate", dataTemplate);
			IDataTemplate dataTemplate5 = dataTemplate2;
			LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_63 <InitializeComponent>_anonXamlCDataTemplate_2 = new LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_63();
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.ContentPage_SizeChanged;
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
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
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(137, 17)));
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
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
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
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 44)));
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
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 25)));
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
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 25)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			numericalAxis.MajorTickStyle = obj11;
			dynamicResourceExtension4.Key = "ChartLabelColor";
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
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LiveDataMultiChartPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(155, 56)));
			DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource4.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			chartLegend.SetValue(ChartLegend.DockPositionProperty, 3);
			chartLegend.SetValue(ChartLegend.IsVisibleProperty, true);
			chartLegend.SetValue(ChartLegend.OrientationProperty, 2);
			chartLegend.SetValue(ChartLegend.OverflowModeProperty, 1);
			chartLegend.SetValue(ChartLegend.ToggleSeriesVisibilityProperty, true);
			IDataTemplate dataTemplate6 = dataTemplate3;
			LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_64 <InitializeComponent>_anonXamlCDataTemplate_3 = new LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_64();
			object[] array11 = new object[0 + 5];
			array11[0] = dataTemplate3;
			array11[1] = chartLegend;
			array11[2] = sfChart;
			array11[3] = grid;
			array11[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array11;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			chartLegend.SetValue(ChartLegend.ItemTemplateProperty, dataTemplate3);
			sfChart.SetValue(SfChart.LegendProperty, chartLegend);
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
			grid.Children.Add(sfChart);
			sfListView.SetValue(Grid.RowProperty, 0);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			sfListView.SetValue(SfListView.SelectionModeProperty, 3);
			grid.Children.Add(sfListView);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x00298CE1 File Offset: 0x00296EE1
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.Legend.IsVisible = !this.Legend.IsVisible;
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x00298CFC File Offset: 0x00296EFC
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.chart.IsVisible = !this.chart.IsVisible;
			this.lv.IsVisible = !this.chart.IsVisible;
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x002967CE File Offset: 0x002949CE
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.ChangeZoomMode();
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x00298D30 File Offset: 0x00296F30
		[CompilerGenerated]
		private void <.ctor>b__0_3()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x00298D3C File Offset: 0x00296F3C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataMultiChartPageV2>(this, typeof(LiveDataMultiChartPageV2));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.Legend = NameScopeExtensions.FindByName<ChartLegend>(this, "Legend");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400216E RID: 8558
		private List<Grid> legendGrids = new List<Grid>();

		// Token: 0x0400216F RID: 8559
		private Page selectorPage;

		// Token: 0x04002170 RID: 8560
		private ToolbarItem zoomModeButton;

		// Token: 0x04002171 RID: 8561
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x04002172 RID: 8562
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x04002173 RID: 8563
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartLegend Legend;

		// Token: 0x04002174 RID: 8564
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x04002175 RID: 8565
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x04002176 RID: 8566
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000634 RID: 1588
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003754 RID: 14164 RVA: 0x00298DC0 File Offset: 0x00296FC0
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003755 RID: 14165 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003756 RID: 14166 RVA: 0x00298DCC File Offset: 0x00296FCC
			internal UnitsHelper.Units <BuildCharts>b__10_0(IPIDFloatValue x)
			{
				return x.Units;
			}

			// Token: 0x06003757 RID: 14167 RVA: 0x00298DD4 File Offset: 0x00296FD4
			internal LiveDataPIDModel <BuildCharts>b__10_3(ValueTuple<LiveDataPIDModel, XyDataSeries> x)
			{
				return x.Item1;
			}

			// Token: 0x06003758 RID: 14168 RVA: 0x00298DD4 File Offset: 0x00296FD4
			internal LiveDataPIDModel <BuildCharts>b__10_4(ValueTuple<LiveDataPIDModel, XyDataSeries> x)
			{
				return x.Item1;
			}

			// Token: 0x06003759 RID: 14169 RVA: 0x00298DDC File Offset: 0x00296FDC
			internal XyDataSeries <BuildCharts>b__10_5(ValueTuple<LiveDataPIDModel, XyDataSeries> x)
			{
				return x.Item2;
			}

			// Token: 0x0600375A RID: 14170 RVA: 0x00298DDC File Offset: 0x00296FDC
			internal XyDataSeries <BuildCharts>b__10_6(ValueTuple<LiveDataPIDModel, XyDataSeries> x)
			{
				return x.Item2;
			}

			// Token: 0x0600375B RID: 14171 RVA: 0x00298DE4 File Offset: 0x00296FE4
			internal bool <Lineseries_PropertyChanged>b__13_0(ChartSeries x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600375C RID: 14172 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <Lineseries_PropertyChanged>b__13_2(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x0600375D RID: 14173 RVA: 0x001DF55B File Offset: 0x001DD75B
			internal string <Lineseries_PropertyChanged>b__13_1(OBDRequest x)
			{
				return x.Header;
			}

			// Token: 0x04002177 RID: 8567
			public static readonly LiveDataMultiChartPageV2.<>c <>9 = new LiveDataMultiChartPageV2.<>c();

			// Token: 0x04002178 RID: 8568
			public static Func<IPIDFloatValue, UnitsHelper.Units> <>9__10_0;

			// Token: 0x04002179 RID: 8569
			public static Func<ValueTuple<LiveDataPIDModel, XyDataSeries>, LiveDataPIDModel> <>9__10_3;

			// Token: 0x0400217A RID: 8570
			public static Func<ValueTuple<LiveDataPIDModel, XyDataSeries>, LiveDataPIDModel> <>9__10_4;

			// Token: 0x0400217B RID: 8571
			public static Func<ValueTuple<LiveDataPIDModel, XyDataSeries>, XyDataSeries> <>9__10_5;

			// Token: 0x0400217C RID: 8572
			public static Func<ValueTuple<LiveDataPIDModel, XyDataSeries>, XyDataSeries> <>9__10_6;

			// Token: 0x0400217D RID: 8573
			public static Func<ChartSeries, bool> <>9__13_0;

			// Token: 0x0400217E RID: 8574
			public static Func<PID, bool> <>9__13_2;

			// Token: 0x0400217F RID: 8575
			public static Func<OBDRequest, string> <>9__13_1;
		}

		// Token: 0x02000635 RID: 1589
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x0600375E RID: 14174 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x0600375F RID: 14175 RVA: 0x00298DEC File Offset: 0x00296FEC
			internal bool <BuildCharts>b__1(IPIDFloatValue x)
			{
				return x.Units == this.leftUnit;
			}

			// Token: 0x06003760 RID: 14176 RVA: 0x00298DFC File Offset: 0x00296FFC
			internal bool <BuildCharts>b__2(IPIDFloatValue x)
			{
				return x.Units == this.rightUnit;
			}

			// Token: 0x06003761 RID: 14177 RVA: 0x00298E0C File Offset: 0x0029700C
			internal void <BuildCharts>b__7(List<OBDRequest> delegateRequests)
			{
				delegateRequests.AddRange(this.requestsForDelegate);
			}

			// Token: 0x04002180 RID: 8576
			public UnitsHelper.Units leftUnit;

			// Token: 0x04002181 RID: 8577
			public UnitsHelper.Units rightUnit;

			// Token: 0x04002182 RID: 8578
			public List<OBDRequest> requestsForDelegate;
		}

		// Token: 0x02000636 RID: 1590
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_62
		{
			// Token: 0x06003762 RID: 14178 RVA: 0x00298E1C File Offset: 0x0029701C
			public <InitializeComponent>_anonXamlCDataTemplate_62()
			{
			}

			// Token: 0x06003763 RID: 14179 RVA: 0x00298E30 File Offset: 0x00297030
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 26);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 39);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 34);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 34);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 30);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
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
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label3;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_62).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 25)));
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
				return grid;
			}

			// Token: 0x04002183 RID: 8579
			internal object[] parentValues;

			// Token: 0x04002184 RID: 8580
			internal LiveDataMultiChartPageV2 root;
		}

		// Token: 0x02000637 RID: 1591
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_63
		{
			// Token: 0x06003764 RID: 14180 RVA: 0x00299588 File Offset: 0x00297788
			public <InitializeComponent>_anonXamlCDataTemplate_63()
			{
			}

			// Token: 0x06003765 RID: 14181 RVA: 0x0029959C File Offset: 0x0029779C
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 26);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 25);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 25);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 25);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 39);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 34);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 39);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 34);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
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
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_63).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 25)));
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
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_63).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 25)));
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
				return grid;
			}

			// Token: 0x04002185 RID: 8581
			internal object[] parentValues;

			// Token: 0x04002186 RID: 8582
			internal LiveDataMultiChartPageV2 root;
		}

		// Token: 0x02000638 RID: 1592
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_64
		{
			// Token: 0x06003766 RID: 14182 RVA: 0x00299C4C File Offset: 0x00297E4C
			public <InitializeComponent>_anonXamlCDataTemplate_64()
			{
			}

			// Token: 0x06003767 RID: 14183 RVA: 0x00299C60 File Offset: 0x00297E60
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 41);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 41);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 34);
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
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartPageV2.<InitializeComponent>_anonXamlCDataTemplate_64).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 41)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.FontSize = (double)obj2;
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Label";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase3);
				grid.Children.Add(label);
				return grid;
			}

			// Token: 0x04002187 RID: 8583
			internal object[] parentValues;

			// Token: 0x04002188 RID: 8584
			internal LiveDataMultiChartPageV2 root;
		}
	}
}
