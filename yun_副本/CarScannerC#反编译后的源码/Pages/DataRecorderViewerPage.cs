using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.UserControls;
using Syncfusion.SfChart.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x020005FD RID: 1533
	[XamlFilePath("DataRecorder\\DataRecorderViewerPage.xaml")]
	public class DataRecorderViewerPage : ContentPage
	{
		// Token: 0x0600364E RID: 13902 RVA: 0x002740A4 File Offset: 0x002722A4
		public DataRecorderViewerPage(IDataRecordContainer Recorder)
		{
			this.InitializeComponent();
			base.Title = Recorder.Title;
			this.zoomModeButton = new ToolbarItem("X", "", new Action(this.ChangeZoomMode), 0, 0);
			base.ToolbarItems.Add(this.zoomModeButton);
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			List<DataRecord> list = Recorder.Records.Where((DataRecord x) => x.IsVisible).ToList<DataRecord>();
			int num = 0;
			DependencyService.Get<IStatusBar>(0);
			foreach (DataRecord dataRecord in list)
			{
				if (dataRecord.Elements.Count > 0)
				{
					DataRecordElement dataRecordElement = dataRecord.Elements.Last<DataRecordElement>();
					if (dataRecordElement.Seconds > (double)num)
					{
						num = (int)dataRecordElement.Seconds;
					}
				}
			}
			int num2 = 0;
			double totalSeconds = Recorder.TimeStarted.TimeOfDay.TotalSeconds;
			foreach (DataRecord dataRecord2 in list)
			{
				DataRecordUC dataRecordUC = new DataRecordUC(totalSeconds)
				{
					BindingContext = dataRecord2,
					HeightRequest = 200.0
				};
				dataRecordUC.VisibleRangeUpdated += this.ChartControl_VisibleRangeUpdated;
				this.grid.RowDefinitions.Add(new RowDefinition
				{
					Height = GridLength.Star
				});
				this.grid.Children.Add(dataRecordUC);
				Grid.SetRow(dataRecordUC, num2);
				num2++;
			}
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x002742B0 File Offset: 0x002724B0
		private void ChartControl_VisibleRangeUpdated(DataRecordUC sender, double minimum, double maximum)
		{
			foreach (DataRecordUC dataRecordUC in this.grid.Children.Select((View x) => (DataRecordUC)x))
			{
				if (dataRecordUC != sender)
				{
					dataRecordUC.SetMinMax(minimum, maximum);
				}
			}
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x0027432C File Offset: 0x0027252C
		private void ChangeZoomMode()
		{
			ZoomMode zoomMode = 0;
			if (this.zoomModeButton.Text == "Y")
			{
				zoomMode = 0;
				this.zoomModeButton.Text = "X";
			}
			else if (this.zoomModeButton.Text == "X")
			{
				zoomMode = 2;
				this.zoomModeButton.Text = "XY";
			}
			else if (this.zoomModeButton.Text == "XY")
			{
				zoomMode = 1;
				this.zoomModeButton.Text = "Y";
			}
			foreach (View view in this.grid.Children)
			{
				if (view is DataRecordUC)
				{
					(view as DataRecordUC).SetZoomBehave(zoomMode);
				}
			}
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x0027440C File Offset: 0x0027260C
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DataViewer"), Translate.GetString("ios_DataViewer_Info"), "OK");
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06003654 RID: 13908 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06003655 RID: 13909 RVA: 0x00274430 File Offset: 0x00272630
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DataRecorderViewerPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/DataRecorderViewerPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\DataRecorderViewerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("grid", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "grid";
			}
			this.grid = grid;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Page_Appearing;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataRecorderViewerPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			scrollView.Content = grid;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06003656 RID: 13910 RVA: 0x0027486B File Offset: 0x00272A6B
		[CompilerGenerated]
		private void <.ctor>b__1_0()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06003657 RID: 13911 RVA: 0x00274875 File Offset: 0x00272A75
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DataRecorderViewerPage>(this, typeof(DataRecorderViewerPage));
			this.grid = NameScopeExtensions.FindByName<Grid>(this, "grid");
		}

		// Token: 0x04002085 RID: 8325
		private ToolbarItem zoomModeButton;

		// Token: 0x04002086 RID: 8326
		private List<DataRecordUC> controls = new List<DataRecordUC>();

		// Token: 0x04002087 RID: 8327
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid grid;

		// Token: 0x020005FE RID: 1534
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003658 RID: 13912 RVA: 0x00274899 File Offset: 0x00272A99
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003659 RID: 13913 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600365A RID: 13914 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <.ctor>b__1_1(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600365B RID: 13915 RVA: 0x002748A5 File Offset: 0x00272AA5
			internal DataRecordUC <ChartControl_VisibleRangeUpdated>b__2_0(View x)
			{
				return (DataRecordUC)x;
			}

			// Token: 0x04002088 RID: 8328
			public static readonly DataRecorderViewerPage.<>c <>9 = new DataRecorderViewerPage.<>c();

			// Token: 0x04002089 RID: 8329
			public static Func<DataRecord, bool> <>9__1_1;

			// Token: 0x0400208A RID: 8330
			public static Func<View, DataRecordUC> <>9__2_0;
		}
	}
}
