using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200019F RID: 415
	[XamlFilePath("UserControls\\LiveDataChartUC.xaml")]
	public class LiveDataChartUC : ContentView
	{
		// Token: 0x06001674 RID: 5748 RVA: 0x000A06D0 File Offset: 0x0009E8D0
		public LiveDataChartUC()
		{
			this.InitializeComponent();
			XyDataSeries xyDataSeries = null;
			switch (SharedSettings.Current.ChartDisplayStyle)
			{
			case ChartItemTypes.FastLine:
				xyDataSeries = new FastLineSeries();
				xyDataSeries.EnableAnimation = !PlatformHelper.IsAndroid;
				break;
			case ChartItemTypes.Area:
				xyDataSeries = new AreaSeries();
				break;
			case ChartItemTypes.SplineLine:
				xyDataSeries = new SplineSeries
				{
					SplineType = 1
				};
				break;
			case ChartItemTypes.SplineArea:
				xyDataSeries = new SplineAreaSeries
				{
					SplineType = 1
				};
				break;
			}
			xyDataSeries.AnimationDuration = 0.35;
			BindableObjectExtensions.SetBinding(xyDataSeries, ChartSeries.ItemsSourceProperty, "Values", 2, null, null);
			xyDataSeries.XBindingPath = "SecondsAdded";
			xyDataSeries.YBindingPath = "Value";
			BindableObjectExtensions.SetBinding(xyDataSeries, ChartSeries.ColorProperty, "ChartLineColor", 2, null, null);
			xyDataSeries.SetDynamicResource(ChartSeries.ColorProperty, "ChartLineColor");
			this.chart.Series.Add(xyDataSeries);
		}

		// Token: 0x06001675 RID: 5749 RVA: 0x000A07B4 File Offset: 0x0009E9B4
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

		// Token: 0x06001676 RID: 5750 RVA: 0x000A0808 File Offset: 0x0009EA08
		private async void Frame_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			LiveDataPIDModel model = (LiveDataPIDModel)base.BindingContext;
			IPID ipid = await PIDSelector.SelectPIDAsync(model.SelectedPID, null);
			if (ipid != null)
			{
				model.SelectedPID = ipid;
			}
			base.IsEnabled = true;
		}

		// Token: 0x06001677 RID: 5751 RVA: 0x000A0840 File Offset: 0x0009EA40
		private void NumericalAxisActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
			double num = (double)e.ActualMaximum - (double)SharedSettings.Current.LiveDataShowTime;
			e.ActualMinimum = num;
		}

		// Token: 0x06001678 RID: 5752 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_BindingContextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001679 RID: 5753 RVA: 0x000A0871 File Offset: 0x0009EA71
		private void valueAxis_LabelCreated(object sender, ChartAxisLabelEventArgs e)
		{
			if (e.Position < 0.0 && e.LabelContent != null && !e.LabelContent.StartsWith('-'))
			{
				e.LabelContent = "-" + e.LabelContent;
			}
		}

		// Token: 0x0600167A RID: 5754 RVA: 0x000A08B4 File Offset: 0x0009EAB4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataChartUC).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/LiveDataChartUC.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 21);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 22);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 25);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 14);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 17);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 17);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 14);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 17);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 17);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 30);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 51);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 25);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 30);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 51);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 14);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 17);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 21);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 21);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 18);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 21);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 21);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 21);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 21);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 18);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 14);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 17);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 21);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 21);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 18);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 21);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 21);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 21);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 18);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 14);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 17);
			DynamicResourceExtension dynamicResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension18 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 21);
			DynamicResourceExtension dynamicResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension19 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 21);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 18);
			DynamicResourceExtension dynamicResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension20 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 21);
			DynamicResourceExtension dynamicResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension21 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 21);
			DynamicResourceExtension dynamicResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension22 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 21);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 18);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 14);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 17);
			DynamicResourceExtension dynamicResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension23 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 21);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 21);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 18);
			DynamicResourceExtension dynamicResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension24 = new DynamicResourceExtension(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 21);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 18);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\LiveDataChartUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.BindingContextChanged += this.Handle_BindingContextChanged;
			this.Resources = resourceDictionary;
			grid2.SetValue(Grid.ColumnSpacingProperty, 0.0);
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 2);
			tapGestureRecognizer.Tapped += this.Frame_Tapped;
			grid2.GestureRecognizers.Add(tapGestureRecognizer);
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(Grid.ColumnSpacingProperty, 5.0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "IsFloatPID";
			bindingExtension.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "IsFloatPID")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			grid.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.Frame_Tapped;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "SelectedPID.ShortName";
			bindingExtension2.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					IPID selectedPID = A_0.SelectedPID;
					if (selectedPID != null)
					{
						return new ValueTuple<string, bool>(selectedPID.ShortName, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "SelectedPID"),
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.SelectedPID, "ShortName")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			linkButton.SetBinding(Button.TextProperty, bindingBase2);
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			stackLayout.SetValue(Grid.ColumnProperty, 1);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "TextValue";
			bindingExtension3.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "TextValue")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase3);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "Units";
			bindingExtension4.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Units, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Units")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label2);
			grid.Children.Add(stackLayout);
			grid2.Children.Add(grid);
			linkButton2.SetValue(Grid.RowProperty, 0);
			linkButton2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton2.Clicked += this.Frame_Tapped;
			dynamicResourceExtension.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 3];
			array[0] = linkButton2;
			array[1] = grid2;
			array[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Button.FontSizeProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 17)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			linkButton2.SetDynamicResource(Button.FontSizeProperty, dynamicResource.Key);
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			bindingExtension5.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension2 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = bindingExtension5;
			array2[1] = linkButton2;
			array2[2] = grid2;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 17)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			bindingExtension5.Converter = obj3;
			bindingExtension5.Path = "IsFloatPID";
			bindingExtension5.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "IsFloatPID")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			linkButton2.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "SelectedPID.ShortName";
			bindingExtension6.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					IPID selectedPID2 = A_0.SelectedPID;
					if (selectedPID2 != null)
					{
						return new ValueTuple<string, bool>(selectedPID2.ShortName, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "SelectedPID"),
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.SelectedPID, "ShortName")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			linkButton2.SetBinding(Button.TextProperty, bindingBase6);
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid2.Children.Add(linkButton2);
			label3.SetValue(Grid.RowProperty, 1);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension7.Mode = 2;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = bindingExtension7;
			array3[1] = label3;
			array3[2] = grid2;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 17)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension7.Converter = obj5;
			bindingExtension7.Path = "IsFloatPID";
			bindingExtension7.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "IsFloatPID")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			label3.SetValue(Label.LineBreakModeProperty, 1);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "TextValue";
			bindingExtension8.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.TextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "TextValue")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase8);
			grid2.Children.Add(label3);
			button.SetValue(Grid.RowProperty, 1);
			button.SetValue(View.MarginProperty, new Thickness(0.0));
			bindingExtension9.Path = "Action";
			bindingExtension9.TypedBinding = new TypedBinding<LiveDataPIDModel, ICommand>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ICommand, bool>(A_0.Action, true);
				}
				return default(ValueTuple<ICommand, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Action")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			button.SetBinding(Button.CommandProperty, bindingBase9);
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "IsAction";
			bindingExtension10.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsAction, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "IsAction")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate.Text = "ios_LaunchAction";
			IMarkupExtension markupExtension4 = translate;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = button;
			array4[1] = grid2;
			array4[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 17)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button.Text = obj7;
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			grid2.Children.Add(button);
			sfChart.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = sfChart;
			array5[1] = grid2;
			array5[2] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 17)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = sfChart;
			array6[1] = grid2;
			array6[2] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 17)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			sfChart.SetValue(SfChart.ChartPaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "ChartVisible";
			bindingExtension11.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "ChartVisible")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			sfChart.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			numericalAxis.ActualRangeChanged += this.NumericalAxisActualRangeChanged;
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, true);
			numericalAxis.SetValue(NumericalAxis.IntervalProperty, new double?(5.0));
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			staticResourceExtension3.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = numericalAxis;
			array7[1] = sfChart;
			array7[2] = grid2;
			array7[3] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 25)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			numericalAxis.MajorGridLineStyle = obj11;
			staticResourceExtension4.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension8 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = numericalAxis;
			array8[1] = sfChart;
			array8[2] = grid2;
			array8[3] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 25)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			numericalAxis.MajorTickStyle = obj13;
			dynamicResourceExtension4.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = chartAxisLabelStyle;
			array9[1] = numericalAxis;
			array9[2] = sfChart;
			array9[3] = grid2;
			array9[4] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array9, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 56)));
			DynamicResource dynamicResource4 = markupExtension9.ProvideValue(xamlServiceProvider9);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource4.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension5.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = chartLineStyle;
			array10[1] = numericalAxis;
			array10[2] = sfChart;
			array10[3] = grid2;
			array10[4] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array10, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 51)));
			DynamicResource dynamicResource5 = markupExtension10.ProvideValue(xamlServiceProvider10);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource5.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			numericalAxis2.SetValue(ChartAxis.EdgeLabelsDrawingModeProperty, 2);
			numericalAxis2.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 1);
			bindingExtension12.Path = "Interval";
			bindingExtension12.TypedBinding = new TypedBinding<LiveDataPIDModel, double>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Interval, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(LiveDataPIDModel A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Interval = A_1;
					return;
				}
			}, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Interval")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.IntervalProperty, bindingBase12);
			numericalAxis2.LabelCreated += this.valueAxis_LabelCreated;
			numericalAxis2.SetValue(ChartAxis.LabelsIntersectActionProperty, 0);
			staticResourceExtension5.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension11 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = numericalAxis2;
			array11[1] = sfChart;
			array11[2] = grid2;
			array11[3] = this;
			object obj16;
			xamlServiceProvider11.Add(typeFromHandle21, obj16 = new SimpleValueTargetProvider(array11, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 25)));
			object obj17 = markupExtension11.ProvideValue(xamlServiceProvider11);
			numericalAxis2.MajorGridLineStyle = obj17;
			staticResourceExtension6.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension12 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = numericalAxis2;
			array12[1] = sfChart;
			array12[2] = grid2;
			array12[3] = this;
			object obj18;
			xamlServiceProvider12.Add(typeFromHandle23, obj18 = new SimpleValueTargetProvider(array12, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 25)));
			object obj19 = markupExtension12.ProvideValue(xamlServiceProvider12);
			numericalAxis2.MajorTickStyle = obj19;
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "Maximum";
			bindingExtension13.TypedBinding = new TypedBinding<LiveDataPIDModel, double>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MaximumProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "Minimum";
			bindingExtension14.TypedBinding = new TypedBinding<LiveDataPIDModel, double>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			numericalAxis2.SetBinding(NumericalAxis.MinimumProperty, bindingBase14);
			numericalAxis2.SetValue(RangeAxisBase.MinorTicksPerIntervalProperty, 1);
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 2);
			dynamicResourceExtension6.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = chartAxisLabelStyle2;
			array13[1] = numericalAxis2;
			array13[2] = sfChart;
			array13[3] = grid2;
			array13[4] = this;
			object obj20;
			xamlServiceProvider13.Add(typeFromHandle25, obj20 = new SimpleValueTargetProvider(array13, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 56)));
			DynamicResource dynamicResource6 = markupExtension13.ProvideValue(xamlServiceProvider13);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource6.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension7.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = chartLineStyle2;
			array14[1] = numericalAxis2;
			array14[2] = sfChart;
			array14[3] = grid2;
			array14[4] = this;
			object obj21;
			xamlServiceProvider14.Add(typeFromHandle27, obj21 = new SimpleValueTargetProvider(array14, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 51)));
			DynamicResource dynamicResource7 = markupExtension14.ProvideValue(xamlServiceProvider14);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource7.Key);
			numericalAxis2.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableDoubleTapProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnablePanningProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableZoomingProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.MaximumZoomLevelProperty, 1f);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.ZoomModeProperty, 0);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartZoomPanBehavior);
			grid2.Children.Add(sfChart);
			stackLayout2.SetValue(Grid.RowProperty, 1);
			stackLayout2.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "ShowMinMaxValues";
			bindingExtension15.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "ShowMinMaxValues")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout2.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension8.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = label4;
			array15[1] = stackLayout2;
			array15[2] = grid2;
			array15[3] = this;
			object obj22;
			xamlServiceProvider15.Add(typeFromHandle29, obj22 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 21)));
			DynamicResource dynamicResource8 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label4.SetValue(Label.TextProperty, "Max: ");
			dynamicResourceExtension9.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = label4;
			array16[1] = stackLayout2;
			array16[2] = grid2;
			array16[3] = this;
			object obj23;
			xamlServiceProvider16.Add(typeFromHandle31, obj23 = new SimpleValueTargetProvider(array16, Label.TextColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(186, 21)));
			DynamicResource dynamicResource9 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label4.SetDynamicResource(Label.TextColorProperty, dynamicResource9.Key);
			stackLayout2.Children.Add(label4);
			dynamicResourceExtension10.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = label5;
			array17[1] = stackLayout2;
			array17[2] = grid2;
			array17[3] = this;
			object obj24;
			xamlServiceProvider17.Add(typeFromHandle33, obj24 = new SimpleValueTargetProvider(array17, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 21)));
			DynamicResource dynamicResource10 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label5.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension11.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = label5;
			array18[1] = stackLayout2;
			array18[2] = grid2;
			array18[3] = this;
			object obj25;
			xamlServiceProvider18.Add(typeFromHandle35, obj25 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 21)));
			DynamicResource dynamicResource11 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource11.Key);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "MaximumAchievedText";
			bindingExtension16.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.MaximumAchievedText, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "MaximumAchievedText")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			label5.SetBinding(Label.TextProperty, bindingBase16);
			dynamicResourceExtension12.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = label5;
			array19[1] = stackLayout2;
			array19[2] = grid2;
			array19[3] = this;
			object obj26;
			xamlServiceProvider19.Add(typeFromHandle37, obj26 = new SimpleValueTargetProvider(array19, Label.TextColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 21)));
			DynamicResource dynamicResource12 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label5.SetDynamicResource(Label.TextColorProperty, dynamicResource12.Key);
			stackLayout2.Children.Add(label5);
			grid2.Children.Add(stackLayout2);
			stackLayout3.SetValue(Grid.RowProperty, 1);
			stackLayout3.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "ShowMinMaxValues";
			bindingExtension17.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "ShowMinMaxValues")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout3.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout3.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension13.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = label6;
			array20[1] = stackLayout3;
			array20[2] = grid2;
			array20[3] = this;
			object obj27;
			xamlServiceProvider20.Add(typeFromHandle39, obj27 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 21)));
			DynamicResource dynamicResource13 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource13.Key);
			label6.SetValue(Label.TextProperty, "Min: ");
			dynamicResourceExtension14.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = label6;
			array21[1] = stackLayout3;
			array21[2] = grid2;
			array21[3] = this;
			object obj28;
			xamlServiceProvider21.Add(typeFromHandle41, obj28 = new SimpleValueTargetProvider(array21, Label.TextColorProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(207, 21)));
			DynamicResource dynamicResource14 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label6.SetDynamicResource(Label.TextColorProperty, dynamicResource14.Key);
			stackLayout3.Children.Add(label6);
			dynamicResourceExtension15.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = label7;
			array22[1] = stackLayout3;
			array22[2] = grid2;
			array22[3] = this;
			object obj29;
			xamlServiceProvider22.Add(typeFromHandle43, obj29 = new SimpleValueTargetProvider(array22, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(209, 21)));
			DynamicResource dynamicResource15 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label7.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource15.Key);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension16.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = label7;
			array23[1] = stackLayout3;
			array23[2] = grid2;
			array23[3] = this;
			object obj30;
			xamlServiceProvider23.Add(typeFromHandle45, obj30 = new SimpleValueTargetProvider(array23, Label.FontSizeProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 21)));
			DynamicResource dynamicResource16 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource16.Key);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "MinimumAchievedText";
			bindingExtension18.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.MinimumAchievedText, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "MinimumAchievedText")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label7.SetBinding(Label.TextProperty, bindingBase18);
			dynamicResourceExtension17.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = label7;
			array24[1] = stackLayout3;
			array24[2] = grid2;
			array24[3] = this;
			object obj31;
			xamlServiceProvider24.Add(typeFromHandle47, obj31 = new SimpleValueTargetProvider(array24, Label.TextColorProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			DynamicResource dynamicResource17 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label7.SetDynamicResource(Label.TextColorProperty, dynamicResource17.Key);
			stackLayout3.Children.Add(label7);
			grid2.Children.Add(stackLayout3);
			stackLayout4.SetValue(Grid.RowProperty, 1);
			stackLayout4.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "ChartShowAverageValue";
			bindingExtension19.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "ChartShowAverageValue")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase19);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout4.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension18.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension18;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = label8;
			array25[1] = stackLayout4;
			array25[2] = grid2;
			array25[3] = this;
			object obj32;
			xamlServiceProvider25.Add(typeFromHandle49, obj32 = new SimpleValueTargetProvider(array25, Label.FontSizeProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(226, 21)));
			DynamicResource dynamicResource18 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource18.Key);
			label8.SetValue(Label.TextProperty, "Avg: ");
			dynamicResourceExtension19.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension26 = dynamicResourceExtension19;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = label8;
			array26[1] = stackLayout4;
			array26[2] = grid2;
			array26[3] = this;
			object obj33;
			xamlServiceProvider26.Add(typeFromHandle51, obj33 = new SimpleValueTargetProvider(array26, Label.TextColorProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(228, 21)));
			DynamicResource dynamicResource19 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label8.SetDynamicResource(Label.TextColorProperty, dynamicResource19.Key);
			stackLayout4.Children.Add(label8);
			dynamicResourceExtension20.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension20;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 4];
			array27[0] = label9;
			array27[1] = stackLayout4;
			array27[2] = grid2;
			array27[3] = this;
			object obj34;
			xamlServiceProvider27.Add(typeFromHandle53, obj34 = new SimpleValueTargetProvider(array27, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(230, 21)));
			DynamicResource dynamicResource20 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label9.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource20.Key);
			label9.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension21.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension21;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = label9;
			array28[1] = stackLayout4;
			array28[2] = grid2;
			array28[3] = this;
			object obj35;
			xamlServiceProvider28.Add(typeFromHandle55, obj35 = new SimpleValueTargetProvider(array28, Label.FontSizeProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 21)));
			DynamicResource dynamicResource21 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource21.Key);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "AverageTextValue";
			bindingExtension20.TypedBinding = new TypedBinding<LiveDataPIDModel, string>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.AverageTextValue, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "AverageTextValue")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label9.SetBinding(Label.TextProperty, bindingBase20);
			dynamicResourceExtension22.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension22;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 4];
			array29[0] = label9;
			array29[1] = stackLayout4;
			array29[2] = grid2;
			array29[3] = this;
			object obj36;
			xamlServiceProvider29.Add(typeFromHandle57, obj36 = new SimpleValueTargetProvider(array29, Label.TextColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(234, 21)));
			DynamicResource dynamicResource22 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label9.SetDynamicResource(Label.TextColorProperty, dynamicResource22.Key);
			stackLayout4.Children.Add(label9);
			grid2.Children.Add(stackLayout4);
			stackLayout5.SetValue(Grid.RowProperty, 1);
			stackLayout5.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "Settings.ShowPing";
			bindingExtension21.TypedBinding = new TypedBinding<LiveDataPIDModel, bool>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					SharedSettings settings = A_0.Settings;
					if (settings != null)
					{
						return new ValueTuple<bool, bool>(settings.ShowPing, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Settings"),
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0.Settings, "ShowPing")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			stackLayout5.SetBinding(VisualElement.IsVisibleProperty, bindingBase21);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout5.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout5.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension23.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension23;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = label10;
			array30[1] = stackLayout5;
			array30[2] = grid2;
			array30[3] = this;
			object obj37;
			xamlServiceProvider30.Add(typeFromHandle59, obj37 = new SimpleValueTargetProvider(array30, Label.FontSizeProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 21)));
			DynamicResource dynamicResource23 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource23.Key);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "Ping";
			bindingExtension22.TypedBinding = new TypedBinding<LiveDataPIDModel, long>(delegate(LiveDataPIDModel A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<long, bool>(A_0.Ping, true);
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<LiveDataPIDModel, object>, string>[]
			{
				new Tuple<Func<LiveDataPIDModel, object>, string>((LiveDataPIDModel A_0) => A_0, "Ping")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label10.SetBinding(Label.TextProperty, bindingBase22);
			label10.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout5.Children.Add(label10);
			label11.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension24.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension31 = dynamicResourceExtension24;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = label11;
			array31[1] = stackLayout5;
			array31[2] = grid2;
			array31[3] = this;
			object obj38;
			xamlServiceProvider31.Add(typeFromHandle61, obj38 = new SimpleValueTargetProvider(array31, Label.FontSizeProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("viewmodels", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(LiveDataChartUC).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(253, 21)));
			DynamicResource dynamicResource24 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label11.SetDynamicResource(Label.FontSizeProperty, dynamicResource24.Key);
			label11.SetValue(Label.TextProperty, "ms");
			label11.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout5.Children.Add(label11);
			grid2.Children.Add(stackLayout5);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x0600167B RID: 5755 RVA: 0x000A4EA6 File Offset: 0x000A30A6
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataChartUC>(this, typeof(LiveDataChartUC));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
		}

		// Token: 0x0600167C RID: 5756 RVA: 0x000A4ECC File Offset: 0x000A30CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2185(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600167D RID: 5757 RVA: 0x000A4EFC File Offset: 0x000A30FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2186(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600167E RID: 5758 RVA: 0x000A4F0C File Offset: 0x000A310C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2187(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				IPID selectedPID = A_0.SelectedPID;
				if (selectedPID != null)
				{
					return new ValueTuple<string, bool>(selectedPID.ShortName, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600167F RID: 5759 RVA: 0x000A4F44 File Offset: 0x000A3144
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2188(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001680 RID: 5760 RVA: 0x000A4F54 File Offset: 0x000A3154
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2189(LiveDataPIDModel A_0)
		{
			return A_0.SelectedPID;
		}

		// Token: 0x06001681 RID: 5761 RVA: 0x000A4F68 File Offset: 0x000A3168
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2190(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.TextValue, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001682 RID: 5762 RVA: 0x000A4F98 File Offset: 0x000A3198
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2191(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001683 RID: 5763 RVA: 0x000A4FA8 File Offset: 0x000A31A8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2192(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Units, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001684 RID: 5764 RVA: 0x000A4FD8 File Offset: 0x000A31D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2193(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001685 RID: 5765 RVA: 0x000A4FE8 File Offset: 0x000A31E8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2194(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001686 RID: 5766 RVA: 0x000A5018 File Offset: 0x000A3218
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2195(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001687 RID: 5767 RVA: 0x000A5028 File Offset: 0x000A3228
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2196(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				IPID selectedPID = A_0.SelectedPID;
				if (selectedPID != null)
				{
					return new ValueTuple<string, bool>(selectedPID.ShortName, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001688 RID: 5768 RVA: 0x000A5060 File Offset: 0x000A3260
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2197(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001689 RID: 5769 RVA: 0x000A5070 File Offset: 0x000A3270
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2198(LiveDataPIDModel A_0)
		{
			return A_0.SelectedPID;
		}

		// Token: 0x0600168A RID: 5770 RVA: 0x000A5084 File Offset: 0x000A3284
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2199(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsFloatPID, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600168B RID: 5771 RVA: 0x000A50B4 File Offset: 0x000A32B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2200(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600168C RID: 5772 RVA: 0x000A50C4 File Offset: 0x000A32C4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2201(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.TextValue, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600168D RID: 5773 RVA: 0x000A50F4 File Offset: 0x000A32F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2202(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600168E RID: 5774 RVA: 0x000A5104 File Offset: 0x000A3304
		[CompilerGenerated]
		private static ValueTuple<ICommand, bool> <InitializeComponent>typedBindingsM__2203(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ICommand, bool>(A_0.Action, true);
			}
			return default(ValueTuple<ICommand, bool>);
		}

		// Token: 0x0600168F RID: 5775 RVA: 0x000A5134 File Offset: 0x000A3334
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2205(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001690 RID: 5776 RVA: 0x000A5144 File Offset: 0x000A3344
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2206(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.IsAction, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001691 RID: 5777 RVA: 0x000A5174 File Offset: 0x000A3374
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2207(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001692 RID: 5778 RVA: 0x000A5184 File Offset: 0x000A3384
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2208(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001693 RID: 5779 RVA: 0x000A51B4 File Offset: 0x000A33B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2209(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001694 RID: 5780 RVA: 0x000A51C4 File Offset: 0x000A33C4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2210(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001695 RID: 5781 RVA: 0x000A51F4 File Offset: 0x000A33F4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2211(LiveDataPIDModel A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Interval = A_1;
				return;
			}
		}

		// Token: 0x06001696 RID: 5782 RVA: 0x000A5210 File Offset: 0x000A3410
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2212(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001697 RID: 5783 RVA: 0x000A5220 File Offset: 0x000A3420
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2213(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001698 RID: 5784 RVA: 0x000A5250 File Offset: 0x000A3450
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2214(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x06001699 RID: 5785 RVA: 0x000A5260 File Offset: 0x000A3460
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2215(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600169A RID: 5786 RVA: 0x000A5290 File Offset: 0x000A3490
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2216(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600169B RID: 5787 RVA: 0x000A52A0 File Offset: 0x000A34A0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2217(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600169C RID: 5788 RVA: 0x000A52D0 File Offset: 0x000A34D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2218(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600169D RID: 5789 RVA: 0x000A52E0 File Offset: 0x000A34E0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2219(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.MaximumAchievedText, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600169E RID: 5790 RVA: 0x000A5310 File Offset: 0x000A3510
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2220(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0600169F RID: 5791 RVA: 0x000A5320 File Offset: 0x000A3520
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2221(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060016A0 RID: 5792 RVA: 0x000A5350 File Offset: 0x000A3550
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2222(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x000A5360 File Offset: 0x000A3560
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2223(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.MinimumAchievedText, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x000A5390 File Offset: 0x000A3590
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2224(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000A53A0 File Offset: 0x000A35A0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2225(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x000A53D0 File Offset: 0x000A35D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2226(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000A53E0 File Offset: 0x000A35E0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2227(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.AverageTextValue, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000A5410 File Offset: 0x000A3610
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2228(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x000A5420 File Offset: 0x000A3620
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2229(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				SharedSettings settings = A_0.Settings;
				if (settings != null)
				{
					return new ValueTuple<bool, bool>(settings.ShowPing, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x000A5458 File Offset: 0x000A3658
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2230(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x060016A9 RID: 5801 RVA: 0x000A5468 File Offset: 0x000A3668
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2231(LiveDataPIDModel A_0)
		{
			return A_0.Settings;
		}

		// Token: 0x060016AA RID: 5802 RVA: 0x000A547C File Offset: 0x000A367C
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__2232(LiveDataPIDModel A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<long, bool>(A_0.Ping, true);
			}
			return default(ValueTuple<long, bool>);
		}

		// Token: 0x060016AB RID: 5803 RVA: 0x000A54AC File Offset: 0x000A36AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2233(LiveDataPIDModel A_0)
		{
			return A_0;
		}

		// Token: 0x0400069B RID: 1691
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x020001A0 RID: 416
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Frame_Tapped>d__2 : IAsyncStateMachine
		{
			// Token: 0x060016AC RID: 5804 RVA: 0x000A54BC File Offset: 0x000A36BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataChartUC liveDataChartUC = this;
				try
				{
					TaskAwaiter<IPID> taskAwaiter;
					if (num != 0)
					{
						liveDataChartUC.IsEnabled = false;
						model = (LiveDataPIDModel)liveDataChartUC.BindingContext;
						taskAwaiter = PIDSelector.SelectPIDAsync(model.SelectedPID, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<IPID> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, LiveDataChartUC.<Frame_Tapped>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<IPID> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<IPID>);
						num2 = -1;
					}
					IPID result = taskAwaiter.GetResult();
					if (result != null)
					{
						model.SelectedPID = result;
					}
					liveDataChartUC.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					model = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				model = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060016AD RID: 5805 RVA: 0x000A55B8 File Offset: 0x000A37B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400069C RID: 1692
			public int <>1__state;

			// Token: 0x0400069D RID: 1693
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400069E RID: 1694
			public LiveDataChartUC <>4__this;

			// Token: 0x0400069F RID: 1695
			private LiveDataPIDModel <model>5__2;

			// Token: 0x040006A0 RID: 1696
			private TaskAwaiter<IPID> <>u__1;
		}
	}
}
