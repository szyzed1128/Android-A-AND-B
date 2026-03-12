using System;
using System.CodeDom.Compiler;
using System.Reflection;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000603 RID: 1539
	[XamlFilePath("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml")]
	public class FuelStatisticsDatePeriodSelectorPage : ContentPage
	{
		// Token: 0x0600366A RID: 13930 RVA: 0x002783C4 File Offset: 0x002765C4
		public FuelStatisticsDatePeriodSelectorPage(DriveCycleViewModel model)
		{
			this.InitializeComponent();
			this.model = model;
			if (model.PeriodStartDate.Date == DateTime.MinValue)
			{
				this.dateStart.Date = DateTimeNowHelper.NowSafe.Date;
			}
			else
			{
				this.dateStart.Date = model.PeriodStartDate.Date;
			}
			if (model.PeriodEndDate == DateTime.MaxValue)
			{
				this.dateEnd.Date = DateTimeNowHelper.NowSafe.Date;
				return;
			}
			this.dateEnd.Date = model.PeriodEndDate.Date;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x00278478 File Offset: 0x00276678
		private void btnOK_Clicked(object sender, EventArgs e)
		{
			DateTime date = this.dateEnd.Date;
			date = new DateTime(date.Ticks + TimeSpan.FromSeconds(86399.0).Ticks);
			this.model.SetCustomPeriod(this.dateStart.Date, date);
			base.Navigation.PopAsync();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x002784DC File Offset: 0x002766DC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DriveCycles/FuelStatisticsDatePeriodSelectorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 26);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 26);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 25);
			DatePicker datePicker;
			VisualDiagnostics.RegisterSourceInfo(datePicker = new DatePicker(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			DatePicker datePicker2;
			VisualDiagnostics.RegisterSourceInfo(datePicker2 = new DatePicker(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DriveCycles\\FuelStatisticsDatePeriodSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("dateStart", datePicker);
			if (datePicker.StyleId == null)
			{
				datePicker.StyleId = "dateStart";
			}
			nameScope.RegisterName("dateEnd", datePicker2);
			if (datePicker2.StyleId == null)
			{
				datePicker2.StyleId = "dateEnd";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			this.dateStart = datePicker;
			this.dateEnd = datePicker2;
			this.btnOK = button;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			translate.Text = "ios_SelectPeriod";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0, 5.0, 5.0));
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			datePicker.SetValue(Grid.RowProperty, 0);
			datePicker.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = datePicker;
			array3[1] = grid;
			array3[2] = stackLayout;
			array3[3] = scrollView;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			datePicker.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			datePicker.SetValue(DatePicker.FormatProperty, "d");
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = datePicker;
			array4[1] = grid;
			array4[2] = stackLayout;
			array4[3] = scrollView;
			array4[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, DatePicker.TextColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 25)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			datePicker.SetDynamicResource(DatePicker.TextColorProperty, dynamicResource3.Key);
			grid.Children.Add(datePicker);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = scrollView;
			array5[4] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 25)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.TextProperty, "-");
			grid.Children.Add(label);
			datePicker2.SetValue(Grid.RowProperty, 0);
			datePicker2.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension5.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = datePicker2;
			array6[1] = grid;
			array6[2] = stackLayout;
			array6[3] = scrollView;
			array6[4] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 25)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			datePicker2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource5.Key);
			datePicker2.SetValue(DatePicker.FormatProperty, "d");
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = datePicker2;
			array7[1] = grid;
			array7[2] = stackLayout;
			array7[3] = scrollView;
			array7[4] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, DatePicker.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 25)));
			DynamicResource dynamicResource6 = markupExtension7.ProvideValue(xamlServiceProvider7);
			datePicker2.SetDynamicResource(DatePicker.TextColorProperty, dynamicResource6.Key);
			grid.Children.Add(datePicker2);
			stackLayout.Children.Add(grid);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			button.SetValue(Button.TextProperty, "OK");
			dynamicResourceExtension7.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = button;
			array8[1] = stackLayout;
			array8[2] = scrollView;
			array8[3] = this;
			object obj9;
			xamlServiceProvider8.Add(typeFromHandle15, obj9 = new SimpleValueTargetProvider(array8, Button.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(FuelStatisticsDatePeriodSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 21)));
			DynamicResource dynamicResource7 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button.SetDynamicResource(Button.TextColorProperty, dynamicResource7.Key);
			stackLayout.Children.Add(button);
			scrollView.Content = stackLayout;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x00279604 File Offset: 0x00277804
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FuelStatisticsDatePeriodSelectorPage>(this, typeof(FuelStatisticsDatePeriodSelectorPage));
			this.dateStart = NameScopeExtensions.FindByName<DatePicker>(this, "dateStart");
			this.dateEnd = NameScopeExtensions.FindByName<DatePicker>(this, "dateEnd");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
		}

		// Token: 0x0400209A RID: 8346
		private DriveCycleViewModel model;

		// Token: 0x0400209B RID: 8347
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private DatePicker dateStart;

		// Token: 0x0400209C RID: 8348
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private DatePicker dateEnd;

		// Token: 0x0400209D RID: 8349
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;
	}
}
