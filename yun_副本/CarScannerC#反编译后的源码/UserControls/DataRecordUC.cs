using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005C2 RID: 1474
	[XamlFilePath("DataRecorder\\DataRecordUC.xaml")]
	public class DataRecordUC : ContentView
	{
		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06003522 RID: 13602 RVA: 0x0026166C File Offset: 0x0025F86C
		// (remove) Token: 0x06003523 RID: 13603 RVA: 0x002616A4 File Offset: 0x0025F8A4
		public event VisibleRangeUpdated VisibleRangeUpdated
		{
			[CompilerGenerated]
			add
			{
				VisibleRangeUpdated visibleRangeUpdated = this.VisibleRangeUpdated;
				VisibleRangeUpdated visibleRangeUpdated2;
				do
				{
					visibleRangeUpdated2 = visibleRangeUpdated;
					VisibleRangeUpdated visibleRangeUpdated3 = (VisibleRangeUpdated)Delegate.Combine(visibleRangeUpdated2, value);
					visibleRangeUpdated = Interlocked.CompareExchange<VisibleRangeUpdated>(ref this.VisibleRangeUpdated, visibleRangeUpdated3, visibleRangeUpdated2);
				}
				while (visibleRangeUpdated != visibleRangeUpdated2);
			}
			[CompilerGenerated]
			remove
			{
				VisibleRangeUpdated visibleRangeUpdated = this.VisibleRangeUpdated;
				VisibleRangeUpdated visibleRangeUpdated2;
				do
				{
					visibleRangeUpdated2 = visibleRangeUpdated;
					VisibleRangeUpdated visibleRangeUpdated3 = (VisibleRangeUpdated)Delegate.Remove(visibleRangeUpdated2, value);
					visibleRangeUpdated = Interlocked.CompareExchange<VisibleRangeUpdated>(ref this.VisibleRangeUpdated, visibleRangeUpdated3, visibleRangeUpdated2);
				}
				while (visibleRangeUpdated != visibleRangeUpdated2);
			}
		}

		// Token: 0x06003524 RID: 13604 RVA: 0x002616DC File Offset: 0x0025F8DC
		public DataRecordUC(double time_started_seconds)
		{
			this.InitializeComponent();
			FastLineSeries fastLineSeries = new FastLineSeries
			{
				YBindingPath = "Value",
				XBindingPath = "Seconds",
				Color = (Color)Application.Current.Resources["ChartLineColor"],
				EnableAnimation = false,
				EnableDataPointSelection = true,
				EnableTooltip = true
			};
			fastLineSeries.SetDynamicResource(ChartSeries.ColorProperty, "Color");
			BindableObjectExtensions.SetBinding(fastLineSeries, ChartSeries.ItemsSourceProperty, "Elements", 0, null, null);
			this.xaxis.Minimum = new double?(time_started_seconds);
			this.chart.Series.Add(fastLineSeries);
		}

		// Token: 0x06003525 RID: 13605 RVA: 0x002617A7 File Offset: 0x0025F9A7
		public void SetZoomBehave(ZoomMode zm)
		{
			this.zoomBehave.ZoomMode = zm;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x002617B8 File Offset: 0x0025F9B8
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

		// Token: 0x06003527 RID: 13607 RVA: 0x00261862 File Offset: 0x0025FA62
		public void SetMinMax(double minimum, double maximum)
		{
			this.zoomBehave.ZoomByRange(this.xaxis, minimum, maximum);
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x000027D4 File Offset: 0x000009D4
		private void NumericalAxisActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
		{
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x00261878 File Offset: 0x0025FA78
		private async void Handle_Scroll(object sender, ChartScrollEventArgs e)
		{
			if (e.Axis == this.xaxis)
			{
				ulong num = this.last_task + 1UL;
				this.last_task = num;
				ulong temp = num;
				await Task.Delay(500);
				if (this.last_task == temp)
				{
					VisibleRangeUpdated visibleRangeUpdated = this.VisibleRangeUpdated;
					if (visibleRangeUpdated != null)
					{
						visibleRangeUpdated(this, this.xaxis.VisibleMinimum, this.xaxis.VisibleMaximum);
					}
				}
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_ZoomStart(object sender, ChartZoomStartEventArgs e)
		{
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_ZoomDelta(object sender, ChartZoomDeltaEventArgs e)
		{
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x002618B8 File Offset: 0x0025FAB8
		private async void Handle_ZoomEnd(object sender, ChartZoomEventArgs e)
		{
			if (e.Axis == this.xaxis)
			{
				ulong num = this.last_task + 1UL;
				this.last_task = num;
				ulong temp = num;
				await Task.Delay(500);
				if (this.last_task == temp)
				{
					VisibleRangeUpdated visibleRangeUpdated = this.VisibleRangeUpdated;
					if (visibleRangeUpdated != null)
					{
						visibleRangeUpdated(this, this.xaxis.VisibleMinimum, this.xaxis.VisibleMaximum);
					}
				}
			}
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x002618F8 File Offset: 0x0025FAF8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DataRecordUC).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/DataRecordUC.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 22);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 25);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 56);
			ChartAxisLabelStyle chartAxisLabelStyle;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle = new ChartAxisLabelStyle(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 30);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 51);
			ChartLineStyle chartLineStyle;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle = new ChartLineStyle(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 30);
			NumericalAxis numericalAxis;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis = new NumericalAxis(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 56);
			ChartAxisLabelStyle chartAxisLabelStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartAxisLabelStyle2 = new ChartAxisLabelStyle(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 30);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 51);
			ChartLineStyle chartLineStyle2;
			VisualDiagnostics.RegisterSourceInfo(chartLineStyle2 = new ChartLineStyle(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 30);
			NumericalAxis numericalAxis2;
			VisualDiagnostics.RegisterSourceInfo(numericalAxis2 = new NumericalAxis(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 22);
			ChartZoomPanBehavior chartZoomPanBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartZoomPanBehavior = new ChartZoomPanBehavior(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 22);
			ChartSelectionBehavior chartSelectionBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartSelectionBehavior = new ChartSelectionBehavior(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
			ChartTooltipBehavior chartTooltipBehavior;
			VisualDiagnostics.RegisterSourceInfo(chartTooltipBehavior = new ChartTooltipBehavior(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
			SfChart sfChart;
			VisualDiagnostics.RegisterSourceInfo(sfChart = new SfChart(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\DataRecordUC.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.chart = sfChart;
			this.xaxis = numericalAxis;
			this.zoomBehave = chartZoomPanBehavior;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.ColumnProperty, 0);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ShortName";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			stackLayout.SetValue(Grid.ColumnProperty, 1);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "UnitsToStringConverter";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 6];
			array[0] = bindingExtension2;
			array[1] = label2;
			array[2] = stackLayout;
			array[3] = grid;
			array[4] = grid2;
			array[5] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 25)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			bindingExtension2.Converter = obj2;
			bindingExtension2.Path = "Units";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase2);
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label2);
			grid.Children.Add(stackLayout);
			grid2.Children.Add(grid);
			sfChart.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = sfChart;
			array2[1] = grid2;
			array2[2] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, SfChart.AreaBackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 17)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			sfChart.SetDynamicResource(SfChart.AreaBackgroundColorProperty, dynamicResource.Key);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = sfChart;
			array3[1] = grid2;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfChart.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			sfChart.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			sfChart.Scroll += this.Handle_Scroll;
			sfChart.ZoomDelta += this.Handle_ZoomDelta;
			sfChart.ZoomEnd += this.Handle_ZoomEnd;
			sfChart.ZoomStart += this.Handle_ZoomStart;
			numericalAxis.ActualRangeChanged += this.NumericalAxisActualRangeChanged;
			numericalAxis.SetValue(RangeAxisBase.EdgeLabelsVisibilityModeProperty, 0);
			numericalAxis.SetValue(ChartAxis.EnableAutoIntervalOnZoomingProperty, true);
			numericalAxis.LabelCreated += this.numAxis_LabelCreated;
			numericalAxis.SetValue(ChartAxis.LabelsIntersectActionProperty, 2);
			staticResourceExtension2.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension4 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = numericalAxis;
			array4[1] = sfChart;
			array4[2] = grid2;
			array4[3] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 25)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			numericalAxis.MajorGridLineStyle = obj6;
			staticResourceExtension3.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension5 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = numericalAxis;
			array5[1] = sfChart;
			array5[2] = grid2;
			array5[3] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 25)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			numericalAxis.MajorTickStyle = obj8;
			dynamicResourceExtension3.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = chartAxisLabelStyle;
			array6[1] = numericalAxis;
			array6[2] = sfChart;
			array6[3] = grid2;
			array6[4] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 56)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			chartAxisLabelStyle.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource3.Key);
			numericalAxis.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle);
			dynamicResourceExtension4.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = chartLineStyle;
			array7[1] = numericalAxis;
			array7[2] = sfChart;
			array7[3] = grid2;
			array7[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 51)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			chartLineStyle.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource4.Key);
			numericalAxis.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle);
			sfChart.SetValue(SfChart.PrimaryAxisProperty, numericalAxis);
			staticResourceExtension4.Key = "DefaultChartGirdLineStyle";
			IMarkupExtension markupExtension8 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = numericalAxis2;
			array8[1] = sfChart;
			array8[2] = grid2;
			array8[3] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, ChartAxis.MajorGridLineStyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 25)));
			object obj12 = markupExtension8.ProvideValue(xamlServiceProvider8);
			numericalAxis2.MajorGridLineStyle = obj12;
			staticResourceExtension5.Key = "DefaultChartGridTickStyle";
			IMarkupExtension markupExtension9 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = numericalAxis2;
			array9[1] = sfChart;
			array9[2] = grid2;
			array9[3] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array9, ChartAxis.MajorTickStyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 25)));
			object obj14 = markupExtension9.ProvideValue(xamlServiceProvider9);
			numericalAxis2.MajorTickStyle = obj14;
			numericalAxis2.SetValue(NumericalAxis.RangePaddingProperty, 0);
			dynamicResourceExtension5.Key = "ChartLabelColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = chartAxisLabelStyle2;
			array10[1] = numericalAxis2;
			array10[2] = sfChart;
			array10[3] = grid2;
			array10[4] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array10, ChartLabelStyle.TextColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 56)));
			DynamicResource dynamicResource5 = markupExtension10.ProvideValue(xamlServiceProvider10);
			chartAxisLabelStyle2.SetDynamicResource(ChartLabelStyle.TextColorProperty, dynamicResource5.Key);
			numericalAxis2.SetValue(ChartAxis.LabelStyleProperty, chartAxisLabelStyle2);
			dynamicResourceExtension6.Key = "ChartStrokeColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = chartLineStyle2;
			array11[1] = numericalAxis2;
			array11[2] = sfChart;
			array11[3] = grid2;
			array11[4] = this;
			object obj16;
			xamlServiceProvider11.Add(typeFromHandle21, obj16 = new SimpleValueTargetProvider(array11, ChartLineStyle.StrokeColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DataRecordUC).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 51)));
			DynamicResource dynamicResource6 = markupExtension11.ProvideValue(xamlServiceProvider11);
			chartLineStyle2.SetDynamicResource(ChartLineStyle.StrokeColorProperty, dynamicResource6.Key);
			numericalAxis2.SetValue(ChartAxis.AxisLineStyleProperty, chartLineStyle2);
			sfChart.SetValue(SfChart.SecondaryAxisProperty, numericalAxis2);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableDoubleTapProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnablePanningProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableSelectionZoomingProperty, false);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.EnableZoomingProperty, true);
			chartZoomPanBehavior.SetValue(ChartZoomPanBehavior.ZoomModeProperty, 0);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartZoomPanBehavior);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartSelectionBehavior);
			sfChart.GetValue(SfChart.ChartBehaviorsProperty).Add(chartTooltipBehavior);
			grid2.Children.Add(sfChart);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x00262FA8 File Offset: 0x002611A8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DataRecordUC>(this, typeof(DataRecordUC));
			this.chart = NameScopeExtensions.FindByName<SfChart>(this, "chart");
			this.xaxis = NameScopeExtensions.FindByName<NumericalAxis>(this, "xaxis");
			this.zoomBehave = NameScopeExtensions.FindByName<ChartZoomPanBehavior>(this, "zoomBehave");
		}

		// Token: 0x04001F9B RID: 8091
		[CompilerGenerated]
		private VisibleRangeUpdated VisibleRangeUpdated;

		// Token: 0x04001F9C RID: 8092
		private Tuple<double, double> rangeInvokeTuple = new Tuple<double, double>(0.0, 0.0);

		// Token: 0x04001F9D RID: 8093
		private ulong last_task;

		// Token: 0x04001F9E RID: 8094
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfChart chart;

		// Token: 0x04001F9F RID: 8095
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericalAxis xaxis;

		// Token: 0x04001FA0 RID: 8096
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ChartZoomPanBehavior zoomBehave;

		// Token: 0x020005C3 RID: 1475
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Scroll>d__10 : IAsyncStateMachine
		{
			// Token: 0x0600352F RID: 13615 RVA: 0x00262FFC File Offset: 0x002611FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecordUC dataRecordUC = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (e.Axis != dataRecordUC.xaxis)
						{
							goto IL_00D4;
						}
						DataRecordUC dataRecordUC2 = dataRecordUC;
						ulong num3 = dataRecordUC.last_task + 1UL;
						dataRecordUC2.last_task = num3;
						temp = num3;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecordUC.<Handle_Scroll>d__10>(ref taskAwaiter, ref this);
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
					if (dataRecordUC.last_task == temp)
					{
						VisibleRangeUpdated visibleRangeUpdated = dataRecordUC.VisibleRangeUpdated;
						if (visibleRangeUpdated != null)
						{
							visibleRangeUpdated(dataRecordUC, dataRecordUC.xaxis.VisibleMinimum, dataRecordUC.xaxis.VisibleMaximum);
						}
					}
					IL_00D4:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003530 RID: 13616 RVA: 0x0026311C File Offset: 0x0026131C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FA1 RID: 8097
			public int <>1__state;

			// Token: 0x04001FA2 RID: 8098
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001FA3 RID: 8099
			public ChartScrollEventArgs e;

			// Token: 0x04001FA4 RID: 8100
			public DataRecordUC <>4__this;

			// Token: 0x04001FA5 RID: 8101
			private ulong <temp>5__2;

			// Token: 0x04001FA6 RID: 8102
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005C4 RID: 1476
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ZoomEnd>d__13 : IAsyncStateMachine
		{
			// Token: 0x06003531 RID: 13617 RVA: 0x0026312C File Offset: 0x0026132C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecordUC dataRecordUC = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (e.Axis != dataRecordUC.xaxis)
						{
							goto IL_00D4;
						}
						DataRecordUC dataRecordUC2 = dataRecordUC;
						ulong num3 = dataRecordUC.last_task + 1UL;
						dataRecordUC2.last_task = num3;
						temp = num3;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecordUC.<Handle_ZoomEnd>d__13>(ref taskAwaiter, ref this);
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
					if (dataRecordUC.last_task == temp)
					{
						VisibleRangeUpdated visibleRangeUpdated = dataRecordUC.VisibleRangeUpdated;
						if (visibleRangeUpdated != null)
						{
							visibleRangeUpdated(dataRecordUC, dataRecordUC.xaxis.VisibleMinimum, dataRecordUC.xaxis.VisibleMaximum);
						}
					}
					IL_00D4:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003532 RID: 13618 RVA: 0x0026324C File Offset: 0x0026144C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001FA7 RID: 8103
			public int <>1__state;

			// Token: 0x04001FA8 RID: 8104
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001FA9 RID: 8105
			public ChartZoomEventArgs e;

			// Token: 0x04001FAA RID: 8106
			public DataRecordUC <>4__this;

			// Token: 0x04001FAB RID: 8107
			private ulong <temp>5__2;

			// Token: 0x04001FAC RID: 8108
			private TaskAwaiter <>u__1;
		}
	}
}
