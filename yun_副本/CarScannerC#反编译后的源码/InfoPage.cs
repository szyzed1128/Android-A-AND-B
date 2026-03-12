using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using MR.Gestures;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000175 RID: 373
	[XamlFilePath("Settings\\InfoPage.xaml")]
	public class InfoPage : ContentPage
	{
		// Token: 0x06001568 RID: 5480 RVA: 0x000934C0 File Offset: 0x000916C0
		public InfoPage()
		{
			this.InitializeComponent();
			base.BindingContext = SharedSettings.Current;
			if (PlatformHelper.AppMarket == Markets.RUS)
			{
				this.webLinkLabel.Text = "Ru.CarScanner.Info";
				this.webLinkLabel.NavigateUri = "https://ru.carscanner.info";
			}
			base.Title = App.AppTitle;
			if (SharedSettings.Current.DeveloperMode)
			{
				base.Title += " :)";
			}
			this.labelBuildVersion.Text = App.Version + "/" + App.Build;
			if (PlatformHelper.AppMarket == Markets.Sideload)
			{
				this.labelBuildVersion.Text = App.Version + "/" + App.Build + "/SL";
			}
		}

		// Token: 0x06001569 RID: 5481 RVA: 0x00093584 File Offset: 0x00091784
		private void MrImage_LongPressing(object sender, LongPressEventArgs e)
		{
			if (SharedSettings.Current.ShowExperimental && e.Duration > 5000L && !SharedSettings.Current.DeveloperMode)
			{
				SharedSettings.Current.DeveloperMode = true;
				base.Title += " :)";
			}
		}

		// Token: 0x0600156A RID: 5482 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600156B RID: 5483 RVA: 0x000935D8 File Offset: 0x000917D8
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x0600156C RID: 5484 RVA: 0x00093610 File Offset: 0x00091810
		private async void adsSettingsButton_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				SharedSettings sharedSettings = SharedSettings.Current;
				bool flag = await base.DisplayAlert(Translate.GetString("GDPR_Consent_Title"), Translate.GetString("GDPR_Consent_Text"), Translate.GetString("GDPR_PersonalizedAds"), Translate.GetString("GDPR_NonPersonalizedAds"));
				sharedSettings.GDPR_ShowPersonalyzed = flag;
				sharedSettings = null;
			}
			if (PlatformHelper.IsiOS)
			{
				if (PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
				{
					PlatformHelper.CommonService.OpenPermissionsSettings();
				}
				else
				{
					SharedSettings sharedSettings = SharedSettings.Current;
					bool flag = await base.DisplayAlert(Translate.GetString("GDPR_Consent_Title"), Translate.GetString("GDPR_Consent_Text"), Translate.GetString("GDPR_NonPersonalizedAds"), Translate.GetString("GDPR_PersonalizedAds"));
					sharedSettings.GDPR_ShowPersonalyzed = !flag;
					sharedSettings = null;
				}
			}
		}

		// Token: 0x0600156D RID: 5485 RVA: 0x00093647 File Offset: 0x00091847
		private void Image_Tapped(object sender, TapEventArgs e)
		{
			this.SetDevMode();
		}

		// Token: 0x0600156E RID: 5486 RVA: 0x00093650 File Offset: 0x00091850
		private void SetDevMode()
		{
			if (SharedSettings.Current.AdsProductPurchased && SharedSettings.Current.ShowExperimental && !SharedSettings.Current.DeveloperMode)
			{
				this.tapcounter++;
				if (this.tapcounter >= 10)
				{
					SharedSettings.Current.DeveloperMode = true;
					base.Title += " :)";
				}
			}
		}

		// Token: 0x0600156F RID: 5487 RVA: 0x000936BA File Offset: 0x000918BA
		private void Image_DoubleTapped(object sender, TapEventArgs e)
		{
			if (PlatformHelper.IsAndroid && PlatformHelper.AppMarket == Markets.Sideload && !SharedSettings.Current.AdsProductPurchased)
			{
				this.panelKeyInput.IsVisible = true;
			}
			this.SetDevMode();
			this.SetDevMode();
		}

		// Token: 0x06001570 RID: 5488 RVA: 0x000936F0 File Offset: 0x000918F0
		private async void BtnKeyActivation_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.IsAndroid)
			{
				this.panelKeyInput.IsVisible = false;
				await PlatformHelper.DroidService.CustomKeyActivator_CheckKeyAsync(this.entryKey.Text, delegate
				{
					SharedSettings.Current.AdsProductPurchased = true;
					base.Navigation.PopToRootAsync();
				}, delegate
				{
				});
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x00093727 File Offset: 0x00091927
		private void btnImportOldSettings_Clicked(object sender, EventArgs e)
		{
			SettingsMigrator.Migrate();
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0009372E File Offset: 0x0009192E
		private void BtnSideloadUpdate_Clicked(object sender, EventArgs e)
		{
			if (PlatformHelper.AppMarket == Markets.Sideload)
			{
				Device.OpenUri(new Uri("https://www.carscanner.info/get-apk"));
			}
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x00093748 File Offset: 0x00091948
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(InfoPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/InfoPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 25);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			HyperLinkLabel hyperLinkLabel;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel = new HyperLinkLabel(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 30);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 30);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 26);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 22);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 40);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 92);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 22);
			HyperLinkLabel hyperLinkLabel2;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel2 = new HyperLinkLabel(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 25);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 34);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 34);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
			HyperLinkLabel hyperLinkLabel3;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel3 = new HyperLinkLabel(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 25);
			HyperLinkLabel hyperLinkLabel4;
			VisualDiagnostics.RegisterSourceInfo(hyperLinkLabel4 = new HyperLinkLabel(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\InfoPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("mrImage", image);
			if (image.StyleId == null)
			{
				image.StyleId = "mrImage";
			}
			nameScope.RegisterName("tbTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "tbTitle";
			}
			nameScope.RegisterName("webLinkLabel", hyperLinkLabel);
			if (hyperLinkLabel.StyleId == null)
			{
				hyperLinkLabel.StyleId = "webLinkLabel";
			}
			nameScope.RegisterName("labelBuildVersion", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "labelBuildVersion";
			}
			nameScope.RegisterName("id", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "id";
			}
			nameScope.RegisterName("panelKeyInput", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelKeyInput";
			}
			nameScope.RegisterName("entryKey", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryKey";
			}
			nameScope.RegisterName("btnKeyActivation", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnKeyActivation";
			}
			nameScope.RegisterName("adsSettingsButton", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "adsSettingsButton";
			}
			nameScope.RegisterName("iosTos", hyperLinkLabel3);
			if (hyperLinkLabel3.StyleId == null)
			{
				hyperLinkLabel3.StyleId = "iosTos";
			}
			nameScope.RegisterName("btnImportOldSettings", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnImportOldSettings";
			}
			this.LayoutRoot = grid2;
			this.mrImage = image;
			this.tbTitle = label;
			this.webLinkLabel = hyperLinkLabel;
			this.labelBuildVersion = label4;
			this.id = label5;
			this.panelKeyInput = grid;
			this.entryKey = entry;
			this.btnKeyActivation = button;
			this.adsSettingsButton = button2;
			this.iosTos = hyperLinkLabel3;
			this.btnImportOldSettings = button3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "SettingsPage_itemHelpInfo.Content";
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
			xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 20.0, 5.0, 5.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "SettingsBackground";
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
			xmlNamespaceResolver2.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			scrollView.SetValue(Grid.RowProperty, 1);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(Image.AspectProperty, 0);
			image.DoubleTapped += this.Image_DoubleTapped;
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			image.LongPressed += this.MrImage_LongPressing;
			image.LongPressing += this.MrImage_LongPressing;
			dynamicResourceExtension2.Key = "LogoImage";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = image;
			array3[1] = stackLayout;
			array3[2] = scrollView;
			array3[3] = grid2;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Image.SourceProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource2.Key);
			image.Tapped += this.Image_Tapped;
			image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout.Children.Add(image);
			dynamicResourceExtension3.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = scrollView;
			array4[3] = grid2;
			array4[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 25)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.LineBreakModeProperty, 0);
			stackLayout.Children.Add(label);
			hyperLinkLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel.SetValue(HyperLinkLabel.NavigateUriProperty, "http://carscanner.info");
			hyperLinkLabel.SetValue(Label.TextProperty, "CarScanner.Info");
			stackLayout.Children.Add(hyperLinkLabel);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label2.SetValue(Label.LineBreakModeProperty, 0);
			label2.SetValue(Label.TextProperty, "Copyright (c) 0vZ 2016-2023");
			stackLayout.Children.Add(label2);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(Label.TextProperty, "Version/Build:");
			stackLayout.Children.Add(label3);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label4);
			label5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout.Children.Add(label5);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			entry.SetValue(Grid.ColumnProperty, 0);
			grid.Children.Add(entry);
			button.SetValue(Grid.ColumnProperty, 1);
			button.Clicked += this.BtnKeyActivation_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			grid.Children.Add(button);
			stackLayout.Children.Add(grid);
			button2.Clicked += this.adsSettingsButton_Clicked;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension;
			array5[1] = button2;
			array5[2] = stackLayout;
			array5[3] = scrollView;
			array5[4] = grid2;
			array5[5] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 25)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension.Converter = obj7;
			bindingExtension.Path = "AdsProductPurchased";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			button2.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			translate2.Text = "GDPR_ChangeAdsSettings";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = button2;
			array6[1] = stackLayout;
			array6[2] = scrollView;
			array6[3] = grid2;
			array6[4] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 25)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button2.Text = obj9;
			stackLayout.Children.Add(button2);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "ShowExperimental";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase2);
			translate3.Text = "ios_ShowExperimentalFeatures";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = labelSwitch;
			array7[1] = stackLayout;
			array7[2] = scrollView;
			array7[3] = grid2;
			array7[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 92)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			labelSwitch.Text = obj11;
			stackLayout.Children.Add(labelSwitch);
			hyperLinkLabel2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel2.SetValue(HyperLinkLabel.NavigateUriProperty, "http://www.icons8.com");
			hyperLinkLabel2.SetValue(Label.TextProperty, "Icons from Icons8");
			stackLayout.Children.Add(hyperLinkLabel2);
			hyperLinkLabel3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel3.SetValue(HyperLinkLabel.NavigateUriProperty, "https://www.carscanner.info/ios-tos/");
			translate4.Text = "ios_TermsOfUse";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = hyperLinkLabel3;
			array8[1] = stackLayout;
			array8[2] = scrollView;
			array8[3] = grid2;
			array8[4] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 25)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			hyperLinkLabel3.Text = obj13;
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "True";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "False";
			onPlatform.Platforms.Add(on2);
			hyperLinkLabel3.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			stackLayout.Children.Add(hyperLinkLabel3);
			hyperLinkLabel4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			hyperLinkLabel4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			hyperLinkLabel4.SetValue(HyperLinkLabel.NavigateUriProperty, "https://www.carscanner.info/privacy-policy/");
			translate5.Text = "ios_PrivacyPolicy";
			IMarkupExtension markupExtension9 = translate5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = hyperLinkLabel4;
			array9[1] = stackLayout;
			array9[2] = scrollView;
			array9[3] = grid2;
			array9[4] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 25)));
			object obj15 = markupExtension9.ProvideValue(xamlServiceProvider9);
			hyperLinkLabel4.Text = obj15;
			stackLayout.Children.Add(hyperLinkLabel4);
			button3.Clicked += this.btnImportOldSettings_Clicked;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension3;
			array10[1] = button3;
			array10[2] = stackLayout;
			array10[3] = scrollView;
			array10[4] = grid2;
			array10[5] = this;
			object obj16;
			xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(InfoPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 25)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension3.Converter = obj17;
			bindingExtension3.Path = "MigratedToSettingsV2";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			button3.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			button3.SetValue(Button.TextProperty, "Import settings from old version(before 1.27.0)");
			stackLayout.Children.Add(button3);
			scrollView.Content = stackLayout;
			grid2.Children.Add(scrollView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x00095328 File Offset: 0x00093528
		[CompilerGenerated]
		private void <BtnKeyActivation_Clicked>b__9_0()
		{
			SharedSettings.Current.AdsProductPurchased = true;
			base.Navigation.PopToRootAsync();
		}

		// Token: 0x06001575 RID: 5493 RVA: 0x00095344 File Offset: 0x00093544
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<InfoPage>(this, typeof(InfoPage));
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.mrImage = NameScopeExtensions.FindByName<Image>(this, "mrImage");
			this.tbTitle = NameScopeExtensions.FindByName<Label>(this, "tbTitle");
			this.webLinkLabel = NameScopeExtensions.FindByName<HyperLinkLabel>(this, "webLinkLabel");
			this.labelBuildVersion = NameScopeExtensions.FindByName<Label>(this, "labelBuildVersion");
			this.id = NameScopeExtensions.FindByName<Label>(this, "id");
			this.panelKeyInput = NameScopeExtensions.FindByName<Grid>(this, "panelKeyInput");
			this.entryKey = NameScopeExtensions.FindByName<Entry>(this, "entryKey");
			this.btnKeyActivation = NameScopeExtensions.FindByName<Button>(this, "btnKeyActivation");
			this.adsSettingsButton = NameScopeExtensions.FindByName<Button>(this, "adsSettingsButton");
			this.iosTos = NameScopeExtensions.FindByName<HyperLinkLabel>(this, "iosTos");
			this.btnImportOldSettings = NameScopeExtensions.FindByName<Button>(this, "btnImportOldSettings");
		}

		// Token: 0x040005E5 RID: 1509
		private int tapcounter;

		// Token: 0x040005E6 RID: 1510
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040005E7 RID: 1511
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image mrImage;

		// Token: 0x040005E8 RID: 1512
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label tbTitle;

		// Token: 0x040005E9 RID: 1513
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private HyperLinkLabel webLinkLabel;

		// Token: 0x040005EA RID: 1514
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelBuildVersion;

		// Token: 0x040005EB RID: 1515
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label id;

		// Token: 0x040005EC RID: 1516
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelKeyInput;

		// Token: 0x040005ED RID: 1517
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryKey;

		// Token: 0x040005EE RID: 1518
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnKeyActivation;

		// Token: 0x040005EF RID: 1519
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button adsSettingsButton;

		// Token: 0x040005F0 RID: 1520
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private HyperLinkLabel iosTos;

		// Token: 0x040005F1 RID: 1521
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnImportOldSettings;

		// Token: 0x02000176 RID: 374
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001576 RID: 5494 RVA: 0x0009542E File Offset: 0x0009362E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001577 RID: 5495 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001578 RID: 5496 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <BtnKeyActivation_Clicked>b__9_1()
			{
			}

			// Token: 0x040005F2 RID: 1522
			public static readonly InfoPage.<>c <>9 = new InfoPage.<>c();

			// Token: 0x040005F3 RID: 1523
			public static Action <>9__9_1;
		}

		// Token: 0x02000177 RID: 375
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnKeyActivation_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06001579 RID: 5497 RVA: 0x0009543C File Offset: 0x0009363C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InfoPage infoPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_00BC;
						}
						infoPage.panelKeyInput.IsVisible = false;
						taskAwaiter = PlatformHelper.DroidService.CustomKeyActivator_CheckKeyAsync(infoPage.entryKey.Text, delegate
						{
							SharedSettings.Current.AdsProductPurchased = true;
							base.Navigation.PopToRootAsync();
						}, delegate
						{
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, InfoPage.<BtnKeyActivation_Clicked>d__9>(ref taskAwaiter, ref this);
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
					IL_00BC:;
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

			// Token: 0x0600157A RID: 5498 RVA: 0x00095544 File Offset: 0x00093744
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005F4 RID: 1524
			public int <>1__state;

			// Token: 0x040005F5 RID: 1525
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005F6 RID: 1526
			public InfoPage <>4__this;

			// Token: 0x040005F7 RID: 1527
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000178 RID: 376
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <adsSettingsButton_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600157B RID: 5499 RVA: 0x00095554 File Offset: 0x00093754
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InfoPage infoPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_016B;
						}
						if (!PlatformHelper.IsAndroid)
						{
							goto IL_00C4;
						}
						sharedSettings = SharedSettings.Current;
						taskAwaiter = infoPage.DisplayAlert(Translate.GetString("GDPR_Consent_Title"), Translate.GetString("GDPR_Consent_Text"), Translate.GetString("GDPR_PersonalizedAds"), Translate.GetString("GDPR_NonPersonalizedAds")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InfoPage.<adsSettingsButton_Clicked>d__4>(ref taskAwaiter, ref this);
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
					bool flag = taskAwaiter.GetResult();
					sharedSettings.GDPR_ShowPersonalyzed = flag;
					sharedSettings = null;
					IL_00C4:
					if (!PlatformHelper.IsiOS)
					{
						goto IL_0189;
					}
					if (PlatformHelper.IsPlatformVersionNewerOrEqual(14, 0))
					{
						PlatformHelper.CommonService.OpenPermissionsSettings();
						goto IL_0189;
					}
					sharedSettings = SharedSettings.Current;
					taskAwaiter = infoPage.DisplayAlert(Translate.GetString("GDPR_Consent_Title"), Translate.GetString("GDPR_Consent_Text"), Translate.GetString("GDPR_NonPersonalizedAds"), Translate.GetString("GDPR_PersonalizedAds")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, InfoPage.<adsSettingsButton_Clicked>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_016B:
					flag = taskAwaiter.GetResult();
					sharedSettings.GDPR_ShowPersonalyzed = !flag;
					sharedSettings = null;
					IL_0189:;
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

			// Token: 0x0600157C RID: 5500 RVA: 0x00095734 File Offset: 0x00093934
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005F8 RID: 1528
			public int <>1__state;

			// Token: 0x040005F9 RID: 1529
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005FA RID: 1530
			public InfoPage <>4__this;

			// Token: 0x040005FB RID: 1531
			private SharedSettings <>7__wrap1;

			// Token: 0x040005FC RID: 1532
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000179 RID: 377
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600157D RID: 5501 RVA: 0x00095744 File Offset: 0x00093944
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				InfoPage infoPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = infoPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, InfoPage.<btnBack_Clicked>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
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

			// Token: 0x0600157E RID: 5502 RVA: 0x00095800 File Offset: 0x00093A00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005FD RID: 1533
			public int <>1__state;

			// Token: 0x040005FE RID: 1534
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005FF RID: 1535
			public InfoPage <>4__this;

			// Token: 0x04000600 RID: 1536
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
