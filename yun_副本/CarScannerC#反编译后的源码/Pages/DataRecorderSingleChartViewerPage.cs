using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2.PIDS;
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
	// Token: 0x020005F9 RID: 1529
	[XamlFilePath("DataRecorder\\DataRecorderSingleChartViewerPage.xaml")]
	public class DataRecorderSingleChartViewerPage : ContentPage
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x00270D14 File Offset: 0x0026EF14
		public DataRecorderSingleChartViewerPage(IDataRecordContainer Recorder)
		{
			this.InitializeComponent();
			base.Title = Recorder.Title;
			this.zoomModeButton = new ToolbarItem("X", "", delegate
			{
				this.ChangeZoomMode();
			}, 0, 0);
			base.ToolbarItems.Add(this.zoomModeButton);
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_list.png", delegate
			{
				this.Legend.IsVisible = !this.Legend.IsVisible;
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			List<DataRecord> list = Recorder.Records.Where((DataRecord x) => x.IsVisible).ToList<DataRecord>();
			if (list.Select((DataRecord x) => x.Units).Distinct<UnitsHelper.Units>().Count<UnitsHelper.Units>() == 2)
			{
				this.BuildInterfaceFor2Units(Recorder, list);
				return;
			}
			this.BuildInterfaceForManyUnits(Recorder, list);
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x00270E44 File Offset: 0x0026F044
		private void BuildInterfaceForManyUnits(IDataRecordContainer Recorder, List<DataRecord> visibleCollection)
		{
			foreach (DataRecord dataRecord in visibleCollection)
			{
				FastLineSeries fastLineSeries = new FastLineSeries
				{
					ItemsSource = dataRecord.Elements,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					EnableDataPointSelection = true,
					Label = dataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecord.Units) + "]",
					IsVisibleOnLegend = true,
					ShowTrackballInfo = true
				};
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				FastScatterSeries fastScatterSeries = new FastScatterSeries
				{
					ShapeType = 2,
					ItemsSource = dataRecord.Elements,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					IsVisibleOnLegend = false,
					ShowTrackballInfo = false,
					EnableDataPointSelection = false,
					Label = dataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecord.Units) + "]",
					EnableAntiAliasing = false,
					EnableAnimation = false,
					EnableTooltip = false,
					IsVisible = false
				};
				if (dataRecord.Elements.Count < 10)
				{
					fastScatterSeries.IsVisible = true;
				}
				this.chart.Series.Add(fastScatterSeries);
				this.chart.Series.Add(fastLineSeries);
			}
			this.xaxis.Minimum = new double?(Recorder.TimeStarted.TimeOfDay.TotalSeconds);
		}

		// Token: 0x06003639 RID: 13881 RVA: 0x00271004 File Offset: 0x0026F204
		private void BuildInterfaceFor2Units(IDataRecordContainer Recorder, List<DataRecord> visibleCollection)
		{
			UnitsHelper.Units[] array = visibleCollection.Select((DataRecord x) => x.Units).Distinct<UnitsHelper.Units>().ToArray<UnitsHelper.Units>();
			UnitsHelper.Units leftUnit = array[0];
			UnitsHelper.Units rightUnit = array[1];
			List<DataRecord> list = visibleCollection.Where((DataRecord x) => x.Units == leftUnit).ToList<DataRecord>();
			visibleCollection.Where((DataRecord x) => x.Units == rightUnit).ToList<DataRecord>();
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
				}
			};
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
			this.chart.SecondaryAxis = numericalAxis;
			this.chart.Axes.Add(numericalAxis2);
			if (this.chart.Series == null)
			{
				this.chart.Series = new ChartSeriesCollection();
			}
			foreach (DataRecord dataRecord in visibleCollection)
			{
				FastLineSeries fastLineSeries = new FastLineSeries
				{
					ItemsSource = dataRecord.Elements,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					EnableDataPointSelection = true,
					Label = dataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecord.Units) + "]",
					IsVisibleOnLegend = true,
					ShowTrackballInfo = true
				};
				if (list.Contains(dataRecord))
				{
					fastLineSeries.YAxis = numericalAxis;
				}
				else
				{
					fastLineSeries.YAxis = numericalAxis2;
				}
				if (this.chart.Series == null)
				{
					this.chart.Series = new ChartSeriesCollection();
				}
				this.chart.Series.Add(fastLineSeries);
				FastScatterSeries fastScatterSeries = new FastScatterSeries
				{
					ShapeType = 2,
					ItemsSource = dataRecord.Elements,
					YBindingPath = "Value",
					XBindingPath = "Seconds",
					IsVisibleOnLegend = false,
					ShowTrackballInfo = false,
					EnableDataPointSelection = false,
					Label = dataRecord.ShortName + " [" + UnitsHelper.GetCaption(dataRecord.Units) + "]",
					EnableAnimation = false,
					EnableAntiAliasing = false,
					EnableTooltip = false,
					IsVisible = false
				};
				if (dataRecord.Elements.Count < 10)
				{
					fastScatterSeries.IsVisible = true;
				}
				this.chart.Series.Add(fastScatterSeries);
			}
			this.xaxis.Minimum = new double?(Recorder.TimeStarted.TimeOfDay.TotalSeconds);
		}

		// Token: 0x0600363A RID: 13882 RVA: 0x002713FC File Offset: 0x0026F5FC
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

		// Token: 0x0600363B RID: 13883 RVA: 0x00271488 File Offset: 0x0026F688
		private void zoomModeButton_Clicked(object sender, EventArgs e)
		{
			this.ChangeZoomMode();
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x00271490 File Offset: 0x0026F690
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DataViewer"), Translate.GetString("ios_DataViewerOne_Info"), "OK");
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x002714B4 File Offset: 0x0026F6B4
		private void Handle_SizeChanged(object sender, EventArgs e)
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

		// Token: 0x0600363E RID: 13886 RVA: 0x00271598 File Offset: 0x0026F798
		private void numAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			double num;
			if (double.TryParse(e.LabelContent, out num))
			{
				double num2 = this.xaxis.VisibleMaximum - this.xaxis.VisibleMinimum;
				string text;
				if (num2 > 300.0)
				{
					text = "hh\\:mm";
				}
				else if (num2 <= 300.0 && num2 > 60.0)
				{
					text = "hh\\:mm\\:ss";
				}
				else if (num2 > 2.0 && num2 <= 60.0)
				{
					text = "mm\\:ss\\.ff";
				}
				else
				{
					text = "ss\\.fff";
				}
				string text2 = TimeSpan.FromSeconds(num).ToString(text);
				e.LabelContent = text2;
			}
		}

		// Token: 0x0600363F RID: 13887 RVA: 0x00271644 File Offset: 0x0026F844
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/DataRecorderSingleChartViewerPage.xaml",
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
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			Color red = Color.Red;
			Color red2 = Color.Red;
			Color green = Color.Green;
			Color green2 = Color.Green;
			Color blue = Color.Blue;
			Color blue2 = Color.Blue;
			Color orange = Color.Orange;
			Color orange2 = Color.Orange;
			Color color = new Color(0.6000000238418579, 0.06666667014360428, 0.0941176488995552, 1.0);
			Color color2 = new Color(0.6000000238418579, 0.06666667014360428, 0.0941176488995552, 1.0);
			Color color3 = new Color(0.6000000238418579, 0.3294117748737335, 0.47058823704719543, 1.0);
			Color color4 = new Color(0.6000000238418579, 0.3294117748737335, 0.47058823704719543, 1.0);
			Color color5 = new Color(0.5882353186607361, 0.6000000238418579, 0.3294117748737335, 1.0);
			Color color6 = new Color(0.5882353186607361, 0.6000000238418579, 0.3294117748737335, 1.0);
			Color color7 = new Color(0.6000000238418579, 0.3607843220233917, 0.3294117748737335, 1.0);
			Color color8 = new Color(0.6000000238418579, 0.3607843220233917, 0.3294117748737335, 1.0);
			Color color9 = new Color(0.5803921818733215, 0.6000000238418579, 0.529411792755127, 1.0);
			Color color10 = new Color(0.5803921818733215, 0.6000000238418579, 0.529411792755127, 1.0);
			Color color11 = new Color(0.08627451211214066, 0.6117647290229797, 0.800000011920929, 1.0);
			Color color12 = new Color(0.08627451211214066, 0.6117647290229797, 0.800000011920929, 1.0);
			Color color13 = new Color(0.6823529601097107, 0.800000011920929, 0.08627451211214066, 1.0);
			Color color14 = new Color(0.6823529601097107, 0.800000011920929, 0.08627451211214066, 1.0);
			Color pink = Color.Pink;
			Color pink2 = Color.Pink;
			Color color15 = new Color(0.08627451211214066, 0.800000011920929, 0.4901960790157318, 1.0);
			Color color16 = new Color(0.08627451211214066, 0.800000011920929, 0.4901960790157318, 1.0);
			Color color17 = new Color(0.6470588445663452, 0.08627451211214066, 0.800000011920929, 1.0);
			Color color18 = new Color(0.6470588445663452, 0.08627451211214066, 0.800000011920929, 1.0);
			Color color19 = new Color(0.08627451211214066, 0.7764706015586853, 0.800000011920929, 1.0);
			Color color20 = new Color(0.08627451211214066, 0.7764706015586853, 0.800000011920929, 1.0);
			Color color21 = new Color(0.08627451211214066, 0.3607843220233917, 0.800000011920929, 1.0);
			Color color22 = new Color(0.08627451211214066, 0.3607843220233917, 0.800000011920929, 1.0);
			Color color23 = new Color(0.800000011920929, 0.46666666865348816, 0.1764705926179886, 1.0);
			Color color24 = new Color(0.800000011920929, 0.46666666865348816, 0.1764705926179886, 1.0);
			Color color25 = new Color(0.43921568989753723, 0.5372549295425415, 0.800000011920929, 1.0);
			Color color26 = new Color(0.43921568989753723, 0.5372549295425415, 0.800000011920929, 1.0);
			Color color27 = new Color(0.800000011920929, 0.6549019813537598, 0.529411792755127, 1.0);
			Color color28 = new Color(0.800000011920929, 0.6549019813537598, 0.529411792755127, 1.0);
			Color color29 = new Color(0.7098039388656616, 0.529411792755127, 0.800000011920929, 1.0);
			Color color30 = new Color(0.7098039388656616, 0.529411792755127, 0.800000011920929, 1.0);
			Color color31 = new Color(1.0, 0.3176470696926117, 0.10980392247438431, 1.0);
			Color color32 = new Color(1.0, 0.3176470696926117, 0.10980392247438431, 1.0);
			Color color33 = new Color(0.10980392247438431, 0.24313725531101227, 1.0, 1.0);
			Color color34 = new Color(0.10980392247438431, 0.24313725531101227, 1.0, 1.0);
			Color color35 = new Color(1.0, 0.21960784494876862, 0.7137255072593689, 1.0);
			Color color36 = new Color(1.0, 0.21960784494876862, 0.7137255072593689, 1.0);
			Color color37 = new Color(0.41960784792900085, 1.0, 0.3294117748737335, 1.0);
			Color color38 = new Color(0.41960784792900085, 1.0, 0.3294117748737335, 1.0);
			Color color39 = new Color(1.0, 0.10980392247438431, 0.3607843220233917, 1.0);
			Color color40 = new Color(1.0, 0.10980392247438431, 0.3607843220233917, 1.0);
			Color color41 = new Color(1.0, 0.8313725590705872, 0.43921568989753723, 1.0);
			Color color42 = new Color(1.0, 0.8313725590705872, 0.43921568989753723, 1.0);
			Color color43 = new Color(1.0, 0.5490196347236633, 0.6784313917160034, 1.0);
			Color color44 = new Color(1.0, 0.5490196347236633, 0.6784313917160034, 1.0);
			Color color45 = new Color(0.8274509906768799, 1.0, 0.7686274647712708, 1.0);
			Color color46 = new Color(0.8274509906768799, 1.0, 0.7686274647712708, 1.0);
			Color color47 = new Color(1.0, 0.7686274647712708, 0.7803921699523926, 1.0);
			Color color48 = new Color(1.0, 0.7686274647712708, 0.7803921699523926, 1.0);
			Color color49 = new Color(1.0, 0.6549019813537598, 0.5490196347236633, 1.0);
			Color color50 = new Color(1.0, 0.6549019813537598, 0.5490196347236633, 1.0);
			Color color51 = new Color(0.9921568632125854, 1.0, 0.8784313797950745, 1.0);
			Color color52 = new Color(0.9921568632125854, 1.0, 0.8784313797950745, 1.0);
			Color darkOrange = Color.DarkOrange;
			Color darkOrange2 = Color.DarkOrange;
			Color darkGreen = Color.DarkGreen;
			Color darkGreen2 = Color.DarkGreen;
			Color color53 = new Color(0.9921568632125854, 1.0, 0.9882352948188782, 1.0);
			Color color54 = new Color(0.9921568632125854, 1.0, 0.9882352948188782, 1.0);
			Color darkRed = Color.DarkRed;
			Color darkRed2 = Color.DarkRed;
			ChartColorCollection chartColorCollection;
			VisualDiagnostics.RegisterSourceInfo(chartColorCollection = new ChartColorCollection(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 44);
			ChartColorModel chartColorModel;
			VisualDiagnostics.RegisterSourceInfo(chartColorModel = new ChartColorModel(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 30);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 51);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 25);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 30);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 51);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 22);
			ChartTrackballBehavior chartTrackballBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTrackballBehavior = new ChartTrackballBehavior(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 30);
			ChartLegend chartLegend;
			VisualDiagnostics.RegisterSourceInfo(chartLegend = new ChartLegend(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("yaxis", numericalAxis2);
			if (numericalAxis2.StyleId == null)
			{
				numericalAxis2.StyleId = "yaxis";
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
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.yaxis = numericalAxis2;
			this.zoomBehave = chartZoomPanBehavior;
			this.Legend = chartLegend;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			chartColorCollection.Add(red);
			chartColorCollection.Add(red2);
			chartColorCollection.Add(green);
			chartColorCollection.Add(green2);
			chartColorCollection.Add(blue);
			chartColorCollection.Add(blue2);
			chartColorCollection.Add(orange);
			chartColorCollection.Add(orange2);
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
			chartColorCollection.Add(pink);
			chartColorCollection.Add(pink2);
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
			chartColorCollection.Add(color28);
			chartColorCollection.Add(color29);
			chartColorCollection.Add(color30);
			chartColorCollection.Add(color31);
			chartColorCollection.Add(color32);
			chartColorCollection.Add(color33);
			chartColorCollection.Add(color34);
			chartColorCollection.Add(color35);
			chartColorCollection.Add(color36);
			chartColorCollection.Add(color37);
			chartColorCollection.Add(color38);
			chartColorCollection.Add(color39);
			chartColorCollection.Add(color40);
			chartColorCollection.Add(color41);
			chartColorCollection.Add(color42);
			chartColorCollection.Add(color43);
			chartColorCollection.Add(color44);
			chartColorCollection.Add(color45);
			chartColorCollection.Add(color46);
			chartColorCollection.Add(color47);
			chartColorCollection.Add(color48);
			chartColorCollection.Add(color49);
			chartColorCollection.Add(color50);
			chartColorCollection.Add(color51);
			chartColorCollection.Add(color52);
			chartColorCollection.Add(darkOrange);
			chartColorCollection.Add(darkOrange2);
			chartColorCollection.Add(darkGreen);
			chartColorCollection.Add(darkGreen2);
			chartColorCollection.Add(color53);
			chartColorCollection.Add(color54);
			chartColorCollection.Add(darkRed);
			chartColorCollection.Add(darkRed2);
			resourceDictionary.Add("Colors", chartColorCollection);
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			sfChart.SetValue(Grid.RowProperty, 0);
			sfChart.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = sfChart;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = sfChart;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 17)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			staticResourceExtension.Key = "Colors";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = chartColorModel;
			array4[1] = sfChart;
			array4[2] = grid;
			array4[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, ChartColorModel.CustomBrushesProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 44)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			chartColorModel.CustomBrushes = obj5;
			chartColorModel.SetValue(ChartColorModel.PaletteProperty, 5);
			sfChart.SetValue(SfChart.ColorModelProperty, chartColorModel);
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, true);
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			staticResourceExtension2.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = numericalAxis;
			array5[1] = sfChart;
			array5[2] = grid;
			array5[3] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 25)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			numericalAxis.MajorGridLineStyle = obj7;
			staticResourceExtension3.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension6 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = numericalAxis;
			array6[1] = sfChart;
			array6[2] = grid;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 25)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			numericalAxis.MajorTickStyle = obj9;
			dynamicResourceExtension4.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = chartAxisLabelStyle;
			array7[1] = numericalAxis;
			array7[2] = sfChart;
			array7[3] = grid;
			array7[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 56)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource4.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension5.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = chartLineStyle;
			array8[1] = numericalAxis;
			array8[2] = sfChart;
			array8[3] = grid;
			array8[4] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 51)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource5.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 0);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 0);
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension4.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = numericalAxis2;
			array9[1] = sfChart;
			array9[2] = grid;
			array9[3] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 25)));
			object obj13 = markupExtension9.ProvideValue(xamlServiceProvider9);
			numericalAxis2.MajorGridLineStyle = obj13;
			staticResourceExtension5.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension10 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = numericalAxis2;
			array10[1] = sfChart;
			array10[2] = grid;
			array10[3] = this;
			object obj14;
			xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array10, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 25)));
			object obj15 = markupExtension10.ProvideValue(xamlServiceProvider10);
			numericalAxis2.MajorTickStyle = obj15;
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			dynamicResourceExtension6.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = chartAxisLabelStyle2;
			array11[1] = numericalAxis2;
			array11[2] = sfChart;
			array11[3] = grid;
			array11[4] = this;
			object obj16;
			xamlServiceProvider11.Add(typeFromHandle21, obj16 = new SimpleValueTargetProvider(array11, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 56)));
			DynamicResource dynamicResource6 = markupExtension11.ProvideValue(xamlServiceProvider11);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource6.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension7.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = chartLineStyle2;
			array12[1] = numericalAxis2;
			array12[2] = sfChart;
			array12[3] = grid;
			array12[4] = this;
			object obj17;
			xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array12, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DataRecorderSingleChartViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 51)));
			DynamicResource dynamicResource7 = markupExtension12.ProvideValue(xamlServiceProvider12);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource7.Key);
			numericalAxis2.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle2);
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
			IDataTemplate dataTemplate2 = dataTemplate;
			DataRecorderSingleChartViewerPage.<InitializeComponent>_anonXamlCDataTemplate_18 <InitializeComponent>_anonXamlCDataTemplate_ = new DataRecorderSingleChartViewerPage.<InitializeComponent>_anonXamlCDataTemplate_18();
			object[] array13 = new object[0 + 5];
			array13[0] = dataTemplate;
			array13[1] = chartLegend;
			array13[2] = sfChart;
			array13[3] = grid;
			array13[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array13;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			chartLegend.SetValue(ChartLegend.ItemTemplateProperty, dataTemplate);
			sfChart.SetValue(SfChart.LegendProperty, chartLegend);
			grid.Children.Add(sfChart);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003640 RID: 13888 RVA: 0x00271488 File Offset: 0x0026F688
		[CompilerGenerated]
		private void <.ctor>b__1_0()
		{
			this.ChangeZoomMode();
		}

		// Token: 0x06003641 RID: 13889 RVA: 0x00273B60 File Offset: 0x00271D60
		[CompilerGenerated]
		private void <.ctor>b__1_1()
		{
			this.Legend.IsVisible = !this.Legend.IsVisible;
		}

		// Token: 0x06003642 RID: 13890 RVA: 0x00273B7B File Offset: 0x00271D7B
		[CompilerGenerated]
		private void <.ctor>b__1_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x00273B88 File Offset: 0x00271D88
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DataRecorderSingleChartViewerPage>(this, typeof(DataRecorderSingleChartViewerPage));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.yaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "yaxis");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
			this.Legend = NameScopeExtensions.FindByName<ChartLegend>(this, "Legend");
		}

		// Token: 0x04002077 RID: 8311
		private ToolbarItem zoomModeButton;

		// Token: 0x04002078 RID: 8312
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x04002079 RID: 8313
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x0400207A RID: 8314
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis yaxis;

		// Token: 0x0400207B RID: 8315
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x0400207C RID: 8316
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartLegend Legend;

		// Token: 0x020005FA RID: 1530
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003644 RID: 13892 RVA: 0x00273BFB File Offset: 0x00271DFB
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003645 RID: 13893 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003646 RID: 13894 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <.ctor>b__1_3(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003647 RID: 13895 RVA: 0x00273C07 File Offset: 0x00271E07
			internal UnitsHelper.Units <.ctor>b__1_4(DataRecord x)
			{
				return x.Units;
			}

			// Token: 0x06003648 RID: 13896 RVA: 0x00273C07 File Offset: 0x00271E07
			internal UnitsHelper.Units <BuildInterfaceFor2Units>b__3_0(DataRecord x)
			{
				return x.Units;
			}

			// Token: 0x0400207D RID: 8317
			public static readonly DataRecorderSingleChartViewerPage.<>c <>9 = new DataRecorderSingleChartViewerPage.<>c();

			// Token: 0x0400207E RID: 8318
			public static Func<DataRecord, bool> <>9__1_3;

			// Token: 0x0400207F RID: 8319
			public static Func<DataRecord, UnitsHelper.Units> <>9__1_4;

			// Token: 0x04002080 RID: 8320
			public static Func<DataRecord, UnitsHelper.Units> <>9__3_0;
		}

		// Token: 0x020005FB RID: 1531
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06003649 RID: 13897 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x0600364A RID: 13898 RVA: 0x00273C0F File Offset: 0x00271E0F
			internal bool <BuildInterfaceFor2Units>b__1(DataRecord x)
			{
				return x.Units == this.leftUnit;
			}

			// Token: 0x0600364B RID: 13899 RVA: 0x00273C1F File Offset: 0x00271E1F
			internal bool <BuildInterfaceFor2Units>b__2(DataRecord x)
			{
				return x.Units == this.rightUnit;
			}

			// Token: 0x04002081 RID: 8321
			public UnitsHelper.Units leftUnit;

			// Token: 0x04002082 RID: 8322
			public UnitsHelper.Units rightUnit;
		}

		// Token: 0x020005FC RID: 1532
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_18
		{
			// Token: 0x0600364C RID: 13900 RVA: 0x00273C30 File Offset: 0x00271E30
			public <InitializeComponent>_anonXamlCDataTemplate_18()
			{
			}

			// Token: 0x0600364D RID: 13901 RVA: 0x00273C44 File Offset: 0x00271E44
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 41);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 41);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecorderSingleChartViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataRecorderSingleChartViewerPage.<InitializeComponent>_anonXamlCDataTemplate_18).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(209, 41)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.FontSize = (double)obj2;
				bindingExtension3.Mode = 4;
				bindingExtension3.Path = "Label";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase3);
				grid.Children.Add(label);
				return grid;
			}

			// Token: 0x04002083 RID: 8323
			internal object[] parentValues;

			// Token: 0x04002084 RID: 8324
			internal DataRecorderSingleChartViewerPage root;
		}
	}
}
