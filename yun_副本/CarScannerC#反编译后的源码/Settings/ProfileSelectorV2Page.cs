using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Reflection;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200020A RID: 522
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\ProfileSelectorV2Page.xaml")]
	public class ProfileSelectorV2Page : ContentPage
	{
		// Token: 0x06001A7B RID: 6779 RVA: 0x00124828 File Offset: 0x00122A28
		public ProfileSelectorV2Page()
		{
			this.InitializeComponent();
			this.CreateProfileSelector();
			ContentView contentView = (ContentView)this.profileSelector;
			base.Content = contentView;
			this.profileSelector.BackButtonVisible = true;
			this.profileSelector.BackButtonVisible = false;
			this.profileSelector.CreateBackItem = true;
			this.profileSelector.PropertyChanged += this.ProfileSelector_PropertyChanged;
			this.profileSelector.TitleText = Translate.GetString("ios_ChooseCarBrand");
			this.profileSelector.TitleVisible = false;
			this.profileSelector.WindowCloseRequested += this.ProfileSelector_WindowCloseRequested;
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x001248CD File Offset: 0x00122ACD
		private void CreateProfileSelector()
		{
			if (App.UseLegacyUI)
			{
				if (PlatformHelper.IsiOS)
				{
					this.profileSelector = new ProfileSelectorV2();
					return;
				}
			}
			else
			{
				this.profileSelector = new ProfileSelectorV3();
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00026430 File Offset: 0x00024630
		private void ProfileSelector_WindowCloseRequested(object sender, bool profileSelected)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x001248F4 File Offset: 0x00122AF4
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.profileSelector.GoBack();
				return true;
			}
			return true;
		}

		// Token: 0x06001A7F RID: 6783 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001A80 RID: 6784 RVA: 0x00124926 File Offset: 0x00122B26
		private void LinkButton_Clicked(object sender, EventArgs e)
		{
			this.profileSelector.GoBack();
		}

		// Token: 0x06001A81 RID: 6785 RVA: 0x00124933 File Offset: 0x00122B33
		private void ProfileSelector_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "TitleText")
			{
				this.lbTitle.Text = this.profileSelector.TitleText;
			}
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x00124960 File Offset: 0x00122B60
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/ProfileSelectorV2Page.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\ProfileSelectorV2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("lbTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbTitle";
			}
			this.gridButtons = grid;
			this.lbTitle = label;
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
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.LinkButton_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Cancel";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.LineBreakModeProperty, 4);
			translate2.Text = "ios_ChooseCarBrand";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj7;
			dynamicResourceExtension4.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = label;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ProfileSelectorV2Page).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x00125490 File Offset: 0x00123690
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ProfileSelectorV2Page>(this, typeof(ProfileSelectorV2Page));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.lbTitle = NameScopeExtensions.FindByName<Label>(this, "lbTitle");
		}

		// Token: 0x04000BBA RID: 3002
		private IProfileSelector profileSelector;

		// Token: 0x04000BBB RID: 3003
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000BBC RID: 3004
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbTitle;
	}
}
