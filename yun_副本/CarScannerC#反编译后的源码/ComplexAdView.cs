using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000197 RID: 407
	[XamlFilePath("UserControls\\ComplexAdView.xaml")]
	public class ComplexAdView : ContentView
	{
		// Token: 0x17000F61 RID: 3937
		// (get) Token: 0x06001634 RID: 5684 RVA: 0x0009DE68 File Offset: 0x0009C068
		public static string RokodilERID
		{
			get
			{
				switch (PlatformHelper.AppMarket)
				{
				case Markets.AppStore:
					return " erid: LjN8KFCtC";
				case Markets.GooglePlay:
					return " erid: LjN8KH5QH";
				case Markets.HMS:
					return " erid: LjN8KK6Hs";
				case Markets.Rustore:
					return " erid: LjN8KKmFj";
				case Markets.RUS:
					return " erid: LjN8KLmhX";
				default:
					return " erid: LjN8KH5QH";
				}
			}
		}

		// Token: 0x06001635 RID: 5685 RVA: 0x0009DEBA File Offset: 0x0009C0BA
		public ComplexAdView()
		{
			this.InitializeComponent();
			this.ad.PropertyChanged += this.Ad_PropertyChanged;
		}

		// Token: 0x06001636 RID: 5686 RVA: 0x0009DEE0 File Offset: 0x0009C0E0
		private async void Ad_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "AdsLoaded" && !this.ad.AdsLoaded && SharedSettings.Current.DisplayRokodilV2)
			{
				bool flag = RuDetector.IsRuLanguage() && RuDetector.IsRuLocale();
				if (flag)
				{
					flag = await RuDetector.IsRuIP();
				}
				if (flag)
				{
					this.rokodilGrid.IsVisible = true;
					this.label.IsVisible = false;
				}
			}
			if (e.PropertyName == "AdsLoaded" && this.ad.AdsLoaded && this.rokodilGrid.IsVisible)
			{
				this.rokodilGrid.IsVisible = false;
				this.label.IsVisible = true;
			}
			if (e.PropertyName == "AdsLoaded" && this.ad.AdsLoaded)
			{
				this.adsWarningAdmob.IsVisible = true;
				this.label.IsVisible = false;
			}
		}

		// Token: 0x06001637 RID: 5687 RVA: 0x0009DF20 File Offset: 0x0009C120
		private async void User_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			if (!this.ad.AdsLoaded)
			{
				if (this.rokodilGrid.IsVisible)
				{
					string text = SharedSettings.Current.RokodilURL;
					if (PlatformHelper.AppMarket == Markets.GooglePlay)
					{
						text += "?LjN8KH5QH";
					}
					else if (PlatformHelper.AppMarket == Markets.Sideload)
					{
						text = "?LjN8KH5QH";
					}
					if (PlatformHelper.AppMarket == Markets.Rustore)
					{
						text += "?LjN8KKmFj";
					}
					if (PlatformHelper.AppMarket == Markets.HMS)
					{
						text += "?LjN8KK6Hs";
					}
					if (PlatformHelper.AppMarket == Markets.RUS)
					{
						text += "?LjN8KLmhX";
					}
					if (PlatformHelper.AppMarket == Markets.AppStore)
					{
						text += "?LjN8KFCtC";
					}
					else
					{
						text += "?LjN8KH5QH";
					}
					await Launcher.TryOpenAsync(text);
				}
				else
				{
					Page inAppPage = InAppManager.GetInAppPage();
					if (inAppPage != null)
					{
						Page currentPage = App.GetCurrentPage();
						if (currentPage != null)
						{
							try
							{
								await currentPage.Navigation.PushAsync(inAppPage);
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
			base.IsEnabled = true;
		}

		// Token: 0x06001638 RID: 5688 RVA: 0x0009DF58 File Offset: 0x0009C158
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ComplexAdView).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/ComplexAdView.xaml",
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
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 18);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 17);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 35);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 30);
			string rokodilERID;
			VisualDiagnostics.RegisterSourceInfo(rokodilERID = ComplexAdView.RokodilERID, new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 35);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 26);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 17);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 17);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 14);
			AdMobView adMobView;
			VisualDiagnostics.RegisterSourceInfo(adMobView = new AdMobView(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\ComplexAdView.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("tapGesture", tapGestureRecognizer);
			if (tapGestureRecognizer.StyleId == null)
			{
				tapGestureRecognizer.StyleId = "tapGesture";
			}
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			nameScope.RegisterName("rokodilGrid", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "rokodilGrid";
			}
			nameScope.RegisterName("adsWarning", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "adsWarning";
			}
			nameScope.RegisterName("adsWarningAdmob", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "adsWarningAdmob";
			}
			nameScope.RegisterName("ad", adMobView);
			if (adMobView.StyleId == null)
			{
				adMobView.StyleId = "ad";
			}
			this.tapGesture = tapGestureRecognizer;
			this.label = label;
			this.rokodilGrid = grid;
			this.adsWarning = label3;
			this.adsWarningAdmob = label4;
			this.ad = adMobView;
			grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("1,auto,auto"));
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			tapGestureRecognizer.Tapped += this.User_Tapped;
			grid2.GestureRecognizers.Add(tapGestureRecognizer);
			frame.SetValue(Grid.RowProperty, 0);
			frame.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			dynamicResourceExtension.Key = "ListViewSeparatorColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 3];
			array[0] = frame;
			array[1] = grid2;
			array[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Frame.BorderColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 17)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource.Key);
			grid2.Children.Add(frame);
			label.SetValue(Grid.RowProperty, 1);
			label.SetValue(Grid.RowSpanProperty, 2);
			label.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension2.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = label;
			array2[1] = grid2;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.InputTransparentProperty, true);
			label.SetValue(Label.MaxLinesProperty, 2);
			translate.Text = "ios_UpgradeToPro";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = label;
			array3[1] = grid2;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj4;
			label.SetValue(Label.TextColorProperty, new Color(0.0, 0.47843137383461, 1.0, 1.0));
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid2.Children.Add(label);
			grid.SetValue(Grid.RowProperty, 1);
			grid.SetValue(Grid.RowSpanProperty, 2);
			grid.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension3.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = grid;
			array4[1] = grid2;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label2.SetValue(Grid.RowProperty, 0);
			label2.SetValue(Grid.ColumnProperty, 0);
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = label2;
			array5[1] = grid;
			array5[2] = grid2;
			array5[3] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 21)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label2.FontSize = (double)obj7;
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(VisualElement.InputTransparentProperty, true);
			label2.SetValue(Label.MaxLinesProperty, 2);
			label2.SetValue(Label.TextProperty, "Качественный адаптер Rokodil ScanX");
			label2.SetValue(Label.TextColorProperty, new Color(0.0, 0.47843137383461, 1.0, 1.0));
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 0);
			label3.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension4.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label3;
			array6[1] = grid;
			array6[2] = grid2;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 21)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label3.SetValue(VisualElement.InputTransparentProperty, true);
			label3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			dynamicResourceExtension5.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = label3;
			array7[1] = grid;
			array7[2] = grid2;
			array7[3] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Label.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 21)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource5.Key);
			label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			translate2.Text = "ads_Caption";
			IMarkupExtension markupExtension8 = translate2;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = span;
			array8[1] = formattedString;
			array8[2] = label3;
			array8[3] = grid;
			array8[4] = grid2;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 35)));
			object obj11 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj11;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, rokodilERID);
			formattedString.Spans.Add(span2);
			label3.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label3);
			grid2.Children.Add(grid);
			label4.SetValue(Grid.RowProperty, 1);
			label4.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension6.Key = "BaseFontSize---";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = label4;
			array9[1] = grid2;
			array9[2] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 17)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label4.SetValue(VisualElement.InputTransparentProperty, true);
			label4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label4.SetValue(Label.MaxLinesProperty, 1);
			translate3.Text = "ads_Caption";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = label4;
			array10[1] = grid2;
			array10[2] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 17)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label4.Text = obj14;
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = label4;
			array11[1] = grid2;
			array11[2] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array11, Label.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(ComplexAdView).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 17)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label4.SetDynamicResource(Label.TextColorProperty, dynamicResource7.Key);
			label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			grid2.Children.Add(label4);
			adMobView.SetValue(Grid.RowProperty, 2);
			adMobView.SetValue(Grid.ColumnProperty, 0);
			adMobView.SetValue(VisualElement.HeightRequestProperty, 50.0);
			adMobView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			adMobView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid2.Children.Add(adMobView);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x0009F4BC File Offset: 0x0009D6BC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ComplexAdView>(this, typeof(ComplexAdView));
			this.tapGesture = NameScopeExtensions.FindByName<TapGestureRecognizer>(this, "tapGesture");
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
			this.rokodilGrid = NameScopeExtensions.FindByName<Grid>(this, "rokodilGrid");
			this.adsWarning = NameScopeExtensions.FindByName<Label>(this, "adsWarning");
			this.adsWarningAdmob = NameScopeExtensions.FindByName<Label>(this, "adsWarningAdmob");
			this.ad = NameScopeExtensions.FindByName<AdMobView>(this, "ad");
		}

		// Token: 0x04000677 RID: 1655
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private TapGestureRecognizer tapGesture;

		// Token: 0x04000678 RID: 1656
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;

		// Token: 0x04000679 RID: 1657
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid rokodilGrid;

		// Token: 0x0400067A RID: 1658
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label adsWarning;

		// Token: 0x0400067B RID: 1659
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label adsWarningAdmob;

		// Token: 0x0400067C RID: 1660
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private AdMobView ad;

		// Token: 0x02000198 RID: 408
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Ad_PropertyChanged>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600163A RID: 5690 RVA: 0x0009F540 File Offset: 0x0009D740
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ComplexAdView complexAdView = this;
				try
				{
					bool flag;
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						if (!(e.PropertyName == "AdsLoaded") || complexAdView.ad.AdsLoaded || !SharedSettings.Current.DisplayRokodilV2)
						{
							goto IL_00D6;
						}
						flag = RuDetector.IsRuLanguage() && RuDetector.IsRuLocale();
						if (!flag)
						{
							goto IL_00BB;
						}
						taskAwaiter = RuDetector.IsRuIP().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ComplexAdView.<Ad_PropertyChanged>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					flag = taskAwaiter.GetResult();
					IL_00BB:
					if (flag)
					{
						complexAdView.rokodilGrid.IsVisible = true;
						complexAdView.label.IsVisible = false;
					}
					IL_00D6:
					if (e.PropertyName == "AdsLoaded" && complexAdView.ad.AdsLoaded && complexAdView.rokodilGrid.IsVisible)
					{
						complexAdView.rokodilGrid.IsVisible = false;
						complexAdView.label.IsVisible = true;
					}
					if (e.PropertyName == "AdsLoaded" && complexAdView.ad.AdsLoaded)
					{
						complexAdView.adsWarningAdmob.IsVisible = true;
						complexAdView.label.IsVisible = false;
					}
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

			// Token: 0x0600163B RID: 5691 RVA: 0x0009F6F4 File Offset: 0x0009D8F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400067D RID: 1661
			public int <>1__state;

			// Token: 0x0400067E RID: 1662
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400067F RID: 1663
			public PropertyChangedEventArgs e;

			// Token: 0x04000680 RID: 1664
			public ComplexAdView <>4__this;

			// Token: 0x04000681 RID: 1665
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000199 RID: 409
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <User_Tapped>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600163C RID: 5692 RVA: 0x0009F704 File Offset: 0x0009D904
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ComplexAdView complexAdView = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					if (num != 0)
					{
						Page inAppPage;
						Page currentPage;
						if (num != 1)
						{
							complexAdView.IsEnabled = false;
							if (complexAdView.ad.AdsLoaded)
							{
								goto IL_01B2;
							}
							if (complexAdView.rokodilGrid.IsVisible)
							{
								string text = SharedSettings.Current.RokodilURL;
								if (PlatformHelper.AppMarket == Markets.GooglePlay)
								{
									text += "?LjN8KH5QH";
								}
								else if (PlatformHelper.AppMarket == Markets.Sideload)
								{
									text = "?LjN8KH5QH";
								}
								if (PlatformHelper.AppMarket == Markets.Rustore)
								{
									text += "?LjN8KKmFj";
								}
								if (PlatformHelper.AppMarket == Markets.HMS)
								{
									text += "?LjN8KK6Hs";
								}
								if (PlatformHelper.AppMarket == Markets.RUS)
								{
									text += "?LjN8KLmhX";
								}
								if (PlatformHelper.AppMarket == Markets.AppStore)
								{
									text += "?LjN8KFCtC";
								}
								else
								{
									text += "?LjN8KH5QH";
								}
								taskAwaiter = Launcher.TryOpenAsync(text).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ComplexAdView.<User_Tapped>d__4>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0122;
							}
							else
							{
								inAppPage = InAppManager.GetInAppPage();
								if (inAppPage == null)
								{
									goto IL_01B2;
								}
								currentPage = App.GetCurrentPage();
								if (currentPage == null)
								{
									goto IL_01B2;
								}
							}
						}
						try
						{
							TaskAwaiter taskAwaiter3;
							if (num != 1)
							{
								taskAwaiter3 = currentPage.Navigation.PushAsync(inAppPage).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ComplexAdView.<User_Tapped>d__4>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num2 = -1;
							}
							taskAwaiter3.GetResult();
						}
						catch (Exception)
						{
						}
						goto IL_01B2;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
					num2 = -1;
					IL_0122:
					taskAwaiter.GetResult();
					IL_01B2:
					complexAdView.IsEnabled = true;
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

			// Token: 0x0600163D RID: 5693 RVA: 0x0009F92C File Offset: 0x0009DB2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000682 RID: 1666
			public int <>1__state;

			// Token: 0x04000683 RID: 1667
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000684 RID: 1668
			public ComplexAdView <>4__this;

			// Token: 0x04000685 RID: 1669
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000686 RID: 1670
			private TaskAwaiter <>u__2;
		}
	}
}
