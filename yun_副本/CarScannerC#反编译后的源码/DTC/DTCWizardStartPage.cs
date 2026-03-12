using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x0200055F RID: 1375
	[XamlCompilation(2)]
	[XamlFilePath("DTC\\DTCWizardStartPage.xaml")]
	public class DTCWizardStartPage : ContentPage
	{
		// Token: 0x060032EB RID: 13035 RVA: 0x0023937A File Offset: 0x0023757A
		public DTCWizardStartPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060032EC RID: 13036 RVA: 0x00239388 File Offset: 0x00237588
		private async void BtnReadDTC_Clicked(object sender, EventArgs e)
		{
			this.btnReadDTC.IsEnabled = false;
			this.btnClearDTC.IsEnabled = false;
			DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Read);
			await base.Navigation.PushAsync(dtcwizardSetupPage);
			this.btnReadDTC.IsEnabled = true;
			this.btnClearDTC.IsEnabled = true;
		}

		// Token: 0x060032ED RID: 13037 RVA: 0x002393C0 File Offset: 0x002375C0
		private void BtnClearDTC_Clicked(object sender, EventArgs e)
		{
			this.btnReadDTC.IsEnabled = false;
			this.btnClearDTC.IsEnabled = false;
			DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Clear);
			base.Navigation.PushAsync(dtcwizardSetupPage);
			this.btnReadDTC.IsEnabled = true;
			this.btnClearDTC.IsEnabled = true;
		}

		// Token: 0x060032EE RID: 13038 RVA: 0x00239414 File Offset: 0x00237614
		private async void BtnSelectBrand_Clicked(object sender, EventArgs e)
		{
			this.btnSelectBrand.IsEnabled = false;
			ProfileSelectorV2Page profileSelectorV2Page = new ProfileSelectorV2Page();
			await base.Navigation.PushAsync(profileSelectorV2Page);
			this.btnSelectBrand.IsEnabled = true;
		}

		// Token: 0x060032EF RID: 13039 RVA: 0x0023944C File Offset: 0x0023764C
		private void ContentPage_Appearing(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(SharedSettings.Current.SelectedBrand))
			{
				this.panelSelectBrandWarning.IsVisible = true;
				this.panelSelectAction.IsVisible = false;
				return;
			}
			this.panelSelectBrandWarning.IsVisible = false;
			this.panelSelectAction.IsVisible = true;
		}

		// Token: 0x060032F0 RID: 13040 RVA: 0x0023949B File Offset: 0x0023769B
		private void ContentPage_Disappearing(object sender, EventArgs e)
		{
			App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
		}

		// Token: 0x060032F1 RID: 13041 RVA: 0x002394A8 File Offset: 0x002376A8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DTCWizardStartPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DTC/DTCWizardStartPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 24);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 22);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 28);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 21);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DTC\\DTCWizardStartPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelSelectBrandWarning", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelSelectBrandWarning";
			}
			nameScope.RegisterName("btnSelectBrand", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnSelectBrand";
			}
			nameScope.RegisterName("panelSelectAction", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelSelectAction";
			}
			nameScope.RegisterName("btnReadDTC", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnReadDTC";
			}
			nameScope.RegisterName("btnClearDTC", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnClearDTC";
			}
			this.panelSelectBrandWarning = stackLayout;
			this.btnSelectBrand = button;
			this.panelSelectAction = grid;
			this.btnReadDTC = button2;
			this.btnClearDTC = button3;
			translate.Text = "ios_MainPage_TileDtcErrors";
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(8, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.ContentPage_Appearing;
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.ContentPage_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			stackLayout2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			stackLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "ios_CarBrandNotSelectedWarning";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = stackLayout2;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(22, 24)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			stackLayout.Children.Add(label);
			button.Clicked += this.BtnSelectBrand_Clicked;
			translate3.Text = "ios_SelectCarBrand";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = button;
			array4[1] = stackLayout;
			array4[2] = stackLayout2;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(26, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button.Text = obj7;
			stackLayout.Children.Add(button);
			stackLayout2.Children.Add(stackLayout);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(Grid.ColumnProperty, 0);
			scrollView.SetValue(Grid.ColumnSpanProperty, 2);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			translate4.Text = "ios_MainPage_TileDtcErrorsInfoText";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label2;
			array5[1] = scrollView;
			array5[2] = grid;
			array5[3] = stackLayout2;
			array5[4] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 28)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label2.Text = obj9;
			scrollView.Content = label2;
			grid.Children.Add(scrollView);
			label3.SetValue(Grid.RowProperty, 1);
			label3.SetValue(Grid.ColumnProperty, 0);
			label3.SetValue(Grid.ColumnSpanProperty, 2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			translate5.Text = "ios_ChooseAction";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label3;
			array6[1] = grid;
			array6[2] = stackLayout2;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label3.Text = obj11;
			label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			grid.Children.Add(label3);
			button2.SetValue(Grid.RowProperty, 2);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.Clicked += this.BtnReadDTC_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			translate6.Text = "DtcPage_btnRead.Content";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = button2;
			array7[1] = grid;
			array7[2] = stackLayout2;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			button2.Text = obj13;
			grid.Children.Add(button2);
			button3.SetValue(Grid.RowProperty, 2);
			button3.SetValue(Grid.ColumnProperty, 1);
			button3.Clicked += this.BtnClearDTC_Clicked;
			button3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			translate7.Text = "DtcPage_btnClear.Content";
			IMarkupExtension markupExtension8 = translate7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = button3;
			array8[1] = grid;
			array8[2] = stackLayout2;
			array8[3] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCWizardStartPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 21)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button3.Text = obj15;
			grid.Children.Add(button3);
			stackLayout2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, stackLayout2);
		}

		// Token: 0x060032F2 RID: 13042 RVA: 0x0023A594 File Offset: 0x00238794
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DTCWizardStartPage>(this, typeof(DTCWizardStartPage));
			this.panelSelectBrandWarning = NameScopeExtensions.FindByName<StackLayout>(this, "panelSelectBrandWarning");
			this.btnSelectBrand = NameScopeExtensions.FindByName<Button>(this, "btnSelectBrand");
			this.panelSelectAction = NameScopeExtensions.FindByName<Grid>(this, "panelSelectAction");
			this.btnReadDTC = NameScopeExtensions.FindByName<Button>(this, "btnReadDTC");
			this.btnClearDTC = NameScopeExtensions.FindByName<Button>(this, "btnClearDTC");
		}

		// Token: 0x04001DE1 RID: 7649
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelSelectBrandWarning;

		// Token: 0x04001DE2 RID: 7650
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSelectBrand;

		// Token: 0x04001DE3 RID: 7651
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelSelectAction;

		// Token: 0x04001DE4 RID: 7652
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnReadDTC;

		// Token: 0x04001DE5 RID: 7653
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnClearDTC;

		// Token: 0x02000560 RID: 1376
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnReadDTC_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x060032F3 RID: 13043 RVA: 0x0023A608 File Offset: 0x00238808
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardStartPage dtcwizardStartPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dtcwizardStartPage.btnReadDTC.IsEnabled = false;
						dtcwizardStartPage.btnClearDTC.IsEnabled = false;
						DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Read);
						taskAwaiter = dtcwizardStartPage.Navigation.PushAsync(dtcwizardSetupPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardStartPage.<BtnReadDTC_Clicked>d__1>(ref taskAwaiter, ref this);
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
					dtcwizardStartPage.btnReadDTC.IsEnabled = true;
					dtcwizardStartPage.btnClearDTC.IsEnabled = true;
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

			// Token: 0x060032F4 RID: 13044 RVA: 0x0023A6FC File Offset: 0x002388FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DE6 RID: 7654
			public int <>1__state;

			// Token: 0x04001DE7 RID: 7655
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DE8 RID: 7656
			public DTCWizardStartPage <>4__this;

			// Token: 0x04001DE9 RID: 7657
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000561 RID: 1377
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnSelectBrand_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x060032F5 RID: 13045 RVA: 0x0023A70C File Offset: 0x0023890C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardStartPage dtcwizardStartPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dtcwizardStartPage.btnSelectBrand.IsEnabled = false;
						ProfileSelectorV2Page profileSelectorV2Page = new ProfileSelectorV2Page();
						taskAwaiter = dtcwizardStartPage.Navigation.PushAsync(profileSelectorV2Page).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardStartPage.<BtnSelectBrand_Clicked>d__3>(ref taskAwaiter, ref this);
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
					dtcwizardStartPage.btnSelectBrand.IsEnabled = true;
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

			// Token: 0x060032F6 RID: 13046 RVA: 0x0023A7E4 File Offset: 0x002389E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DEA RID: 7658
			public int <>1__state;

			// Token: 0x04001DEB RID: 7659
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DEC RID: 7660
			public DTCWizardStartPage <>4__this;

			// Token: 0x04001DED RID: 7661
			private TaskAwaiter <>u__1;
		}
	}
}
