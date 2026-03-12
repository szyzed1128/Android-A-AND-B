using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000D4 RID: 212
	[XamlFilePath("Pages\\FreezeFramePage.xaml")]
	public class FreezeFramePage : ContentPage
	{
		// Token: 0x0600040C RID: 1036 RVA: 0x00037324 File Offset: 0x00035524
		public FreezeFramePage()
		{
			this.defaultViewModel = new FreezeFrameViewModel(App.OBDReader);
			this.InitializeComponent();
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			base.BindingContext = this.DefaultViewModel;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0003737F File Offset: 0x0003557F
		private void Handle_Appearing(object sender, EventArgs e)
		{
			if (!this.wasLoaded)
			{
				this.wasLoaded = true;
				this.LoadFirstFF();
			}
			if (!SharedSettings.Current.InfoShowed_FreezeFrame)
			{
				SharedSettings.Current.InfoShowed_FreezeFrame = true;
				this.btnInfo_Clicked(null, null);
			}
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000373B5 File Offset: 0x000355B5
		private void btnBack_Clicked(object sender, EventArgs e)
		{
			App.OBDReader.ClearRequestQueue();
			base.Navigation.PopAsync();
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x000373CE File Offset: 0x000355CE
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x000373FB File Offset: 0x000355FB
		public FreezeFrameViewModel DefaultViewModel
		{
			get
			{
				return this.defaultViewModel;
			}
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00037404 File Offset: 0x00035604
		private async void LoadFirstFF()
		{
			this.btnPrev.IsEnabled = false;
			this.btnNext.IsEnabled = false;
			this.activityFrame.IsVisible = true;
			this.DefaultViewModel.FreezeFrameNumber = 0;
			await this.DefaultViewModel.GetFreezeFrame();
			this.btnPrev.IsEnabled = true;
			this.btnNext.IsEnabled = true;
			this.activityFrame.IsVisible = false;
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x0003743C File Offset: 0x0003563C
		private async void ChangeFF(int num)
		{
			this.activityFrame.IsVisible = true;
			this.btnPrev.IsEnabled = false;
			this.btnNext.IsEnabled = false;
			this.DefaultViewModel.FreezeFrameNumber = num;
			this.tbFrameNumber.Text = num.ToString();
			await this.DefaultViewModel.GetFreezeFrame();
			this.btnPrev.IsEnabled = true;
			this.btnNext.IsEnabled = true;
			this.activityFrame.IsVisible = false;
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x0003747C File Offset: 0x0003567C
		private void btnPrev_Clicked(object sender, EventArgs e)
		{
			int num = this.DefaultViewModel.FreezeFrameNumber - 1;
			if (num < 0)
			{
				num = 0;
			}
			this.ChangeFF(num);
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x000374A4 File Offset: 0x000356A4
		private void btnNext_Clicked(object sender, EventArgs e)
		{
			int num = this.DefaultViewModel.FreezeFrameNumber + 1;
			if (num > 255)
			{
				num = 255;
			}
			this.ChangeFF(num);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000374D4 File Offset: 0x000356D4
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileFreezeFrame"), Translate.GetString("ios_FreezeFrame_InfoText"), "OK");
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x000374F8 File Offset: 0x000356F8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FreezeFramePage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/FreezeFramePage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 17);
			LiveDataListDataTemplateSelector liveDataListDataTemplateSelector;
			VisualDiagnostics.RegisterSourceInfo(liveDataListDataTemplateSelector = new LiveDataListDataTemplateSelector(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 21);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 21);
			NonScalableLabel nonScalableLabel2;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel2 = new NonScalableLabel(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 21);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 14);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 17);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 26);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 22);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 18);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 17);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("btnPrev", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnPrev";
			}
			nameScope.RegisterName("tbFrameNumber", nonScalableLabel2);
			if (nonScalableLabel2.StyleId == null)
			{
				nonScalableLabel2.StyleId = "tbFrameNumber";
			}
			nameScope.RegisterName("btnNext", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "btnNext";
			}
			nameScope.RegisterName("listView", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "listView";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.btnPrev = linkButton2;
			this.tbFrameNumber = nonScalableLabel2;
			this.btnNext = linkButton3;
			this.listView = listView;
			this.activityFrame = activityFrame;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			IDataTemplate dataTemplate3 = dataTemplate;
			FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_55 <InitializeComponent>_anonXamlCDataTemplate_ = new FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_55();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("MobileDataTemplatePro", dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_56 <InitializeComponent>_anonXamlCDataTemplate_2 = new FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_56();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			resourceDictionary.Add("TabletDataTemplatePro", dataTemplate2);
			staticResourceExtension.Key = "MobileDataTemplatePro";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = liveDataListDataTemplateSelector;
			array3[1] = resourceDictionary;
			array3[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array3, typeof(LiveDataListDataTemplateSelector).GetRuntimeProperty("MobileTemplate"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			liveDataListDataTemplateSelector.MobileTemplate = obj2;
			staticResourceExtension2.Key = "TabletDataTemplatePro";
			IMarkupExtension markupExtension2 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = liveDataListDataTemplateSelector;
			array4[1] = resourceDictionary;
			array4[2] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, typeof(LiveDataListDataTemplateSelector).GetRuntimeProperty("TabletTemplate"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			liveDataListDataTemplateSelector.TabletTemplate = obj4;
			resourceDictionary.Add("LiveDataListDataTemplateSelector", liveDataListDataTemplateSelector);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 1];
			array5[0] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array5, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,20,5,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton;
			array6[1] = grid;
			array6[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array6, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 17)));
			DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton;
			array7[1] = grid;
			array7[2] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 17)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			linkButton.Text = obj8;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 2);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension3.Key = "NavigationBarNonScalableLabel";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = nonScalableLabel;
			array8[1] = grid;
			array8[2] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 17)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_MainPage_TileFreezeFrame";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = nonScalableLabel;
			array9[1] = grid;
			array9[2] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 17)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			nonScalableLabel.Text = obj11;
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(nonScalableLabel);
			stackLayout.SetValue(Grid.ColumnProperty, 3);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnPrev_Clicked;
			dynamicResourceExtension4.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = linkButton2;
			array10[1] = stackLayout;
			array10[2] = grid;
			array10[3] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array10, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 21)));
			DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			linkButton2.SetValue(Button.TextProperty, "<<");
			stackLayout.Children.Add(linkButton2);
			nonScalableLabel2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			dynamicResourceExtension5.Key = "NavigationBarNonScalableLabel";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = nonScalableLabel2;
			array11[1] = stackLayout;
			array11[2] = grid;
			array11[3] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array11, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 21)));
			DynamicResource dynamicResource5 = markupExtension9.ProvideValue(xamlServiceProvider9);
			nonScalableLabel2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource5.Key);
			nonScalableLabel2.SetValue(Label.TextProperty, "0");
			nonScalableLabel2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(nonScalableLabel2);
			linkButton3.SetValue(Grid.ColumnProperty, 2);
			linkButton3.Clicked += this.btnNext_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension6.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = linkButton3;
			array12[1] = stackLayout;
			array12[2] = grid;
			array12[3] = this;
			object obj14;
			xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array12, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 21)));
			DynamicResource dynamicResource6 = markupExtension10.ProvideValue(xamlServiceProvider10);
			linkButton3.SetDynamicResource(VisualElement.StyleProperty, dynamicResource6.Key);
			linkButton3.SetValue(Button.TextProperty, ">>");
			stackLayout.Children.Add(linkButton3);
			grid.Children.Add(stackLayout);
			linkButton4.SetValue(Grid.ColumnProperty, 5);
			linkButton4.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton4.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton4.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton4.Clicked += this.btnInfo_Clicked;
			linkButton4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension7.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = linkButton4;
			array13[1] = grid;
			array13[2] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array13, Button.ImageProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 17)));
			DynamicResource dynamicResource7 = markupExtension11.ProvideValue(xamlServiceProvider11);
			linkButton4.SetDynamicResource(Button.ImageProperty, dynamicResource7.Key);
			linkButton4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "0";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on4);
			linkButton4.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton4);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform3.WinPhone = new Thickness(0.0);
			onPlatform3.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			staticResourceExtension3.Key = "LiveDataListDataTemplateSelector";
			IMarkupExtension markupExtension12 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = listView;
			array14[1] = grid2;
			array14[2] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array14, ItemsView<Cell>.ItemTemplateProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(FreezeFramePage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(237, 17)));
			object obj17 = markupExtension12.ProvideValue(xamlServiceProvider12);
			listView.ItemTemplate = obj17;
			bindingExtension.Path = "FFPIDS";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			complexAdView.SetValue(Grid.RowProperty, 2);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0, -5.0, -5.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid2.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06000418 RID: 1048 RVA: 0x00039468 File Offset: 0x00037668
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FreezeFramePage>(this, typeof(FreezeFramePage));
			this.btnPrev = NameScopeExtensions.FindByName<LinkButton>(this, "btnPrev");
			this.tbFrameNumber = NameScopeExtensions.FindByName<NonScalableLabel>(this, "tbFrameNumber");
			this.btnNext = NameScopeExtensions.FindByName<LinkButton>(this, "btnNext");
			this.listView = NameScopeExtensions.FindByName<ListView>(this, "listView");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x040002E4 RID: 740
		private bool wasLoaded;

		// Token: 0x040002E5 RID: 741
		private FreezeFrameViewModel defaultViewModel;

		// Token: 0x040002E6 RID: 742
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPrev;

		// Token: 0x040002E7 RID: 743
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel tbFrameNumber;

		// Token: 0x040002E8 RID: 744
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnNext;

		// Token: 0x040002E9 RID: 745
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView listView;

		// Token: 0x040002EA RID: 746
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x040002EB RID: 747
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020000D5 RID: 213
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ChangeFF>d__10 : IAsyncStateMachine
		{
			// Token: 0x06000419 RID: 1049 RVA: 0x000394EC File Offset: 0x000376EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FreezeFramePage freezeFramePage = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						freezeFramePage.activityFrame.IsVisible = true;
						freezeFramePage.btnPrev.IsEnabled = false;
						freezeFramePage.btnNext.IsEnabled = false;
						freezeFramePage.DefaultViewModel.FreezeFrameNumber = num;
						freezeFramePage.tbFrameNumber.Text = num.ToString();
						taskAwaiter = freezeFramePage.DefaultViewModel.GetFreezeFrame().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, FreezeFramePage.<ChangeFF>d__10>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					freezeFramePage.btnPrev.IsEnabled = true;
					freezeFramePage.btnNext.IsEnabled = true;
					freezeFramePage.activityFrame.IsVisible = false;
					RequestProducerStatic.UpdateOBDReaderRequests();
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

			// Token: 0x0600041A RID: 1050 RVA: 0x0003961C File Offset: 0x0003781C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002EC RID: 748
			public int <>1__state;

			// Token: 0x040002ED RID: 749
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002EE RID: 750
			public FreezeFramePage <>4__this;

			// Token: 0x040002EF RID: 751
			public int num;

			// Token: 0x040002F0 RID: 752
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020000D6 RID: 214
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadFirstFF>d__9 : IAsyncStateMachine
		{
			// Token: 0x0600041B RID: 1051 RVA: 0x0003962C File Offset: 0x0003782C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FreezeFramePage freezeFramePage = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						freezeFramePage.btnPrev.IsEnabled = false;
						freezeFramePage.btnNext.IsEnabled = false;
						freezeFramePage.activityFrame.IsVisible = true;
						freezeFramePage.DefaultViewModel.FreezeFrameNumber = 0;
						taskAwaiter = freezeFramePage.DefaultViewModel.GetFreezeFrame().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, FreezeFramePage.<LoadFirstFF>d__9>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					freezeFramePage.btnPrev.IsEnabled = true;
					freezeFramePage.btnNext.IsEnabled = true;
					freezeFramePage.activityFrame.IsVisible = false;
					RequestProducerStatic.UpdateOBDReaderRequests();
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

			// Token: 0x0600041C RID: 1052 RVA: 0x00039740 File Offset: 0x00037940
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040002F1 RID: 753
			public int <>1__state;

			// Token: 0x040002F2 RID: 754
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040002F3 RID: 755
			public FreezeFramePage <>4__this;

			// Token: 0x040002F4 RID: 756
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020000D7 RID: 215
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_55
		{
			// Token: 0x0600041D RID: 1053 RVA: 0x00039750 File Offset: 0x00037950
			public <InitializeComponent>_anonXamlCDataTemplate_55()
			{
			}

			// Token: 0x0600041E RID: 1054 RVA: 0x00039764 File Offset: 0x00037964
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 30);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 30);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 26);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 33);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 30);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 33);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 30);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				grid.SetValue(Layout.PaddingProperty, new Thickness(0.0, 5.0, 0.0, 5.0));
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				stackLayout.SetValue(Grid.RowProperty, 1);
				stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Value";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout.Children.Add(label2);
				label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label3.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase3);
				stackLayout.Children.Add(label3);
				grid.Children.Add(stackLayout);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040002F5 RID: 757
			internal object[] parentValues;

			// Token: 0x040002F6 RID: 758
			internal FreezeFramePage root;
		}

		// Token: 0x020000D8 RID: 216
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_56
		{
			// Token: 0x0600041F RID: 1055 RVA: 0x00039B90 File Offset: 0x00037D90
			public <InitializeComponent>_anonXamlCDataTemplate_56()
			{
			}

			// Token: 0x06000420 RID: 1056 RVA: 0x00039BA4 File Offset: 0x00037DA4
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 30);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 29);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 33);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 33);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 30);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 33);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 33);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\FreezeFramePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 18);
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
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_56).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				stackLayout.SetValue(Grid.ColumnProperty, 1);
				stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = stackLayout;
				array4[2] = grid;
				array4[3] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_56).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 33)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Value";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout.Children.Add(label2);
				dynamicResourceExtension3.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label3;
				array6[1] = stackLayout;
				array6[2] = grid;
				array6[3] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(FreezeFramePage.<InitializeComponent>_anonXamlCDataTemplate_56).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 33)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
				label3.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Units";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase3);
				stackLayout.Children.Add(label3);
				grid.Children.Add(stackLayout);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040002F7 RID: 759
			internal object[] parentValues;

			// Token: 0x040002F8 RID: 760
			internal FreezeFramePage root;
		}
	}
}
