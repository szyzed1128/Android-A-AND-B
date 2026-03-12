using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000631 RID: 1585
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\LanguageSelectorPageV2.xaml")]
	public class LanguageSelectorPageV2 : ContentPage
	{
		// Token: 0x0600373A RID: 14138 RVA: 0x0029511D File Offset: 0x0029331D
		public LanguageSelectorPageV2()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x0029512C File Offset: 0x0029332C
		private async void btnApplyLanguage_Clicked(object sender, EventArgs e)
		{
			try
			{
				RegionInfo currentRegion = RegionInfo.CurrentRegion;
				SharedSettings.Current.Currency = currentRegion.CurrencySymbol;
				if (!currentRegion.IsMetric)
				{
					SharedSettings.Current.Use_km = false;
					SharedSettings.Current.UseLitersForVolume = false;
					SharedSettings.Current.Use_celcium = false;
					SharedSettings.Current.FuelConsumptionUnit = FuelConsumptionUnits.MilesPerGallon;
					if (currentRegion.TwoLetterISORegionName == "US")
					{
						SharedSettings.Current.UseUSGallon = true;
					}
					if (currentRegion.TwoLetterISORegionName == "UK")
					{
						SharedSettings.Current.UseLitersForVolume = true;
					}
				}
			}
			catch (Exception)
			{
				SharedSettings.Current.Currency = "$";
			}
			App.Instance.ChangeLanguageAndGoToWelcomePage();
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0029515C File Offset: 0x0029335C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LanguageSelectorPageV2.xaml",
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
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 83);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 35);
			int num = 0;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 22);
			int num2 = 1;
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 22);
			int num3 = 2;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			int num4 = 3;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 22);
			int num5 = 4;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			int num6 = 5;
			RadioCell radioCell6;
			VisualDiagnostics.RegisterSourceInfo(radioCell6 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 22);
			int num7 = 6;
			RadioCell radioCell7;
			VisualDiagnostics.RegisterSourceInfo(radioCell7 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 22);
			int num8 = 7;
			RadioCell radioCell8;
			VisualDiagnostics.RegisterSourceInfo(radioCell8 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			int num9 = 8;
			RadioCell radioCell9;
			VisualDiagnostics.RegisterSourceInfo(radioCell9 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 22);
			int num10 = 9;
			RadioCell radioCell10;
			VisualDiagnostics.RegisterSourceInfo(radioCell10 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			int num11 = 10;
			RadioCell radioCell11;
			VisualDiagnostics.RegisterSourceInfo(radioCell11 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			int num12 = 11;
			RadioCell radioCell12;
			VisualDiagnostics.RegisterSourceInfo(radioCell12 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
			int num13 = 12;
			RadioCell radioCell13;
			VisualDiagnostics.RegisterSourceInfo(radioCell13 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 22);
			int num14 = 13;
			RadioCell radioCell14;
			VisualDiagnostics.RegisterSourceInfo(radioCell14 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			int num15 = 14;
			RadioCell radioCell15;
			VisualDiagnostics.RegisterSourceInfo(radioCell15 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			int num16 = 15;
			RadioCell radioCell16;
			VisualDiagnostics.RegisterSourceInfo(radioCell16 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 22);
			int num17 = 16;
			RadioCell radioCell17;
			VisualDiagnostics.RegisterSourceInfo(radioCell17 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 22);
			int num18 = 17;
			RadioCell radioCell18;
			VisualDiagnostics.RegisterSourceInfo(radioCell18 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 22);
			int num19 = 18;
			RadioCell radioCell19;
			VisualDiagnostics.RegisterSourceInfo(radioCell19 = new RadioCell(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 21);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LanguageSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			this.settingsLayoutRoot = settingsView;
			this.SetValue(Page.TitleProperty, "Car Scanner ELM OBD2");
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*, Auto"));
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate.Text = "welcome_ChooseLanguage";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = section;
			array2[1] = settingsView;
			array2[2] = grid;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(23, 29)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			section.Title = obj3;
			bindingExtension.Path = "Language";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			translate2.Text = "Settings_Control_FuelScheme_Auto.Content";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = radioCell;
			array3[1] = section;
			array3[2] = settingsView;
			array3[3] = grid;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, CellBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(24, 35)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			radioCell.Title = obj5;
			radioCell.SetValue(RadioCell.ValueProperty, num);
			section.Add(radioCell);
			radioCell2.SetValue(CellBase.TitleProperty, "English");
			radioCell2.SetValue(RadioCell.ValueProperty, num2);
			section.Add(radioCell2);
			radioCell3.SetValue(CellBase.TitleProperty, "Русский");
			radioCell3.SetValue(RadioCell.ValueProperty, num3);
			section.Add(radioCell3);
			radioCell4.SetValue(CellBase.TitleProperty, "Deutsche");
			radioCell4.SetValue(RadioCell.ValueProperty, num4);
			section.Add(radioCell4);
			radioCell5.SetValue(CellBase.TitleProperty, "Türkçe");
			radioCell5.SetValue(RadioCell.ValueProperty, num5);
			section.Add(radioCell5);
			radioCell6.SetValue(CellBase.TitleProperty, "Español");
			radioCell6.SetValue(RadioCell.ValueProperty, num6);
			section.Add(radioCell6);
			radioCell7.SetValue(CellBase.TitleProperty, "Italiano");
			radioCell7.SetValue(RadioCell.ValueProperty, num7);
			section.Add(radioCell7);
			radioCell8.SetValue(CellBase.TitleProperty, "Française");
			radioCell8.SetValue(RadioCell.ValueProperty, num8);
			section.Add(radioCell8);
			radioCell9.SetValue(CellBase.TitleProperty, "Português");
			radioCell9.SetValue(RadioCell.ValueProperty, num9);
			section.Add(radioCell9);
			radioCell10.SetValue(CellBase.TitleProperty, "Polski");
			radioCell10.SetValue(RadioCell.ValueProperty, num10);
			section.Add(radioCell10);
			radioCell11.SetValue(CellBase.TitleProperty, "Český");
			radioCell11.SetValue(RadioCell.ValueProperty, num11);
			section.Add(radioCell11);
			radioCell12.SetValue(CellBase.TitleProperty, "한국어");
			radioCell12.SetValue(RadioCell.ValueProperty, num12);
			section.Add(radioCell12);
			radioCell13.SetValue(CellBase.TitleProperty, "简体中文");
			radioCell13.SetValue(RadioCell.ValueProperty, num13);
			section.Add(radioCell13);
			radioCell14.SetValue(CellBase.TitleProperty, "اللغة العربية");
			radioCell14.SetValue(RadioCell.ValueProperty, num14);
			section.Add(radioCell14);
			radioCell15.SetValue(CellBase.TitleProperty, "日本語");
			radioCell15.SetValue(RadioCell.ValueProperty, num15);
			section.Add(radioCell15);
			radioCell16.SetValue(CellBase.TitleProperty, "Svenska (beta)");
			radioCell16.SetValue(RadioCell.ValueProperty, num16);
			section.Add(radioCell16);
			radioCell17.SetValue(CellBase.TitleProperty, "Українська мова");
			radioCell17.SetValue(RadioCell.ValueProperty, num17);
			section.Add(radioCell17);
			radioCell18.SetValue(CellBase.TitleProperty, "Magyar");
			radioCell18.SetValue(RadioCell.ValueProperty, num18);
			section.Add(radioCell18);
			radioCell19.SetValue(CellBase.TitleProperty, "Български");
			radioCell19.SetValue(RadioCell.ValueProperty, num19);
			section.Add(radioCell19);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			frame.SetValue(Grid.RowProperty, 1);
			frame.SetValue(Layout.PaddingProperty, new Thickness(0.0, 1.0, 0.0, 1.0));
			dynamicResourceExtension2.Key = "SettingsHeaderTextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = frame;
			array4[1] = grid;
			array4[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Frame.BorderColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 17)));
			DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
			frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource2.Key);
			frame.SetValue(Frame.CornerRadiusProperty, 0f);
			dynamicResourceExtension3.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = button;
			array5[1] = frame;
			array5[2] = grid;
			array5[3] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 21)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			button.Clicked += this.btnApplyLanguage_Clicked;
			button.SetValue(Button.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension6 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = button;
			array6[1] = frame;
			array6[2] = grid;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Button.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LanguageSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 21)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button.FontSize = (double)obj9;
			button.SetValue(Button.TextProperty, "OK");
			button.SetValue(Button.TextColorProperty, Color.White);
			frame.SetValue(ContentView.ContentProperty, button);
			grid.Children.Add(frame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x00296454 File Offset: 0x00294654
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LanguageSelectorPageV2>(this, typeof(LanguageSelectorPageV2));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
		}

		// Token: 0x0400216B RID: 8555
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x02000632 RID: 1586
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyLanguage_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600373E RID: 14142 RVA: 0x00296478 File Offset: 0x00294678
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					try
					{
						RegionInfo currentRegion = RegionInfo.CurrentRegion;
						SharedSettings.Current.Currency = currentRegion.CurrencySymbol;
						if (!currentRegion.IsMetric)
						{
							SharedSettings.Current.Use_km = false;
							SharedSettings.Current.UseLitersForVolume = false;
							SharedSettings.Current.Use_celcium = false;
							SharedSettings.Current.FuelConsumptionUnit = FuelConsumptionUnits.MilesPerGallon;
							if (currentRegion.TwoLetterISORegionName == "US")
							{
								SharedSettings.Current.UseUSGallon = true;
							}
							if (currentRegion.TwoLetterISORegionName == "UK")
							{
								SharedSettings.Current.UseLitersForVolume = true;
							}
						}
					}
					catch (Exception)
					{
						SharedSettings.Current.Currency = "$";
					}
					App.Instance.ChangeLanguageAndGoToWelcomePage();
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600373F RID: 14143 RVA: 0x00296570 File Offset: 0x00294770
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400216C RID: 8556
			public int <>1__state;

			// Token: 0x0400216D RID: 8557
			public AsyncVoidMethodBuilder <>t__builder;
		}
	}
}
