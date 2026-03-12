using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000205 RID: 517
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\MainPageConfigurationScreen.xaml")]
	public class MainPageConfigurationScreen : ContentPage
	{
		// Token: 0x06001A69 RID: 6761 RVA: 0x00123039 File Offset: 0x00121239
		public MainPageConfigurationScreen()
		{
			this.InitializeComponent();
			this.model = new MainPageConfigurationModel();
			this.lv.ItemsSource = this.model.Configuration;
			this.switchShowCoding.BindingContext = SharedSettings.Current;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00123078 File Offset: 0x00121278
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem == null)
			{
				return;
			}
			MainPageConfigurationProxyItem mainPageConfigurationProxyItem = (MainPageConfigurationProxyItem)this.lv.SelectedItem;
			mainPageConfigurationProxyItem.IsVisible = !mainPageConfigurationProxyItem.IsVisible;
			this.lv.SelectedItem = null;
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x001230B2 File Offset: 0x001212B2
		private void MainPageConfigurationScreen_Disappearing(object sender, EventArgs e)
		{
			this.model.Save();
			SimpleMainPage.Instance.UpdateMainButtons();
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x001230CC File Offset: 0x001212CC
		private void btnUp_Clicked(object sender, EventArgs e)
		{
			MainPageConfigurationProxyItem mainPageConfigurationProxyItem = (MainPageConfigurationProxyItem)(sender as Button).BindingContext;
			int num = this.model.Configuration.IndexOf(mainPageConfigurationProxyItem);
			if (num > 0)
			{
				this.model.Configuration.Move(num, num - 1);
			}
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00123114 File Offset: 0x00121314
		private void btnDown_Clicked(object sender, EventArgs e)
		{
			MainPageConfigurationProxyItem mainPageConfigurationProxyItem = (MainPageConfigurationProxyItem)(sender as Button).BindingContext;
			int num = this.model.Configuration.IndexOf(mainPageConfigurationProxyItem);
			if (num < this.model.Configuration.Count - 1)
			{
				this.model.Configuration.Move(num, num + 1);
			}
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x001230B2 File Offset: 0x001212B2
		private void btnApply_Clicked(object sender, EventArgs e)
		{
			this.model.Save();
			SimpleMainPage.Instance.UpdateMainButtons();
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x0012316D File Offset: 0x0012136D
		private void btnReset_Clicked(object sender, EventArgs e)
		{
			this.model.ResetToDefault();
			SimpleMainPage.Instance.UpdateMainButtons();
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00123184 File Offset: 0x00121384
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MainPageConfigurationScreen).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/MainPageConfigurationScreen.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("switchShowCoding", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "switchShowCoding";
			}
			nameScope.RegisterName("btnReset", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnReset";
			}
			this.lv = listView;
			this.switchShowCoding = labelSwitch;
			this.btnReset = button;
			this.Resources = resourceDictionary;
			translate.Text = "ios_MainScreen";
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MainPageConfigurationScreen).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MainPageConfigurationScreen).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.MainPageConfigurationScreen_Disappearing;
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(5.0, 5.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			listView.SetValue(Grid.RowProperty, 0);
			listView.ItemSelected += this.Lv_ItemSelected;
			IDataTemplate dataTemplate2 = dataTemplate;
			MainPageConfigurationScreen.<InitializeComponent>_anonXamlCDataTemplate_86 <InitializeComponent>_anonXamlCDataTemplate_ = new MainPageConfigurationScreen.<InitializeComponent>_anonXamlCDataTemplate_86();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(listView);
			labelSwitch.SetValue(Grid.RowProperty, 1);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "ShowCodingAndService";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
			translate2.Text = "coding_ShowCodingIfAvailable";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = labelSwitch;
			array4[1] = grid;
			array4[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MainPageConfigurationScreen).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 17)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			labelSwitch.Text = obj5;
			grid.Children.Add(labelSwitch);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.btnReset_Clicked;
			translate3.Text = "ios_Reset";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = button;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MainPageConfigurationScreen).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 17)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button.Text = obj7;
			grid.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00123B9C File Offset: 0x00121D9C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MainPageConfigurationScreen>(this, typeof(MainPageConfigurationScreen));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.switchShowCoding = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchShowCoding");
			this.btnReset = NameScopeExtensions.FindByName<Button>(this, "btnReset");
		}

		// Token: 0x04000BAE RID: 2990
		private MainPageConfigurationModel model;

		// Token: 0x04000BAF RID: 2991
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04000BB0 RID: 2992
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchShowCoding;

		// Token: 0x04000BB1 RID: 2993
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnReset;

		// Token: 0x02000206 RID: 518
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_86
		{
			// Token: 0x06001A72 RID: 6770 RVA: 0x00123BF0 File Offset: 0x00121DF0
			public <InitializeComponent>_anonXamlCDataTemplate_86()
			{
			}

			// Token: 0x06001A73 RID: 6771 RVA: 0x00123C04 File Offset: 0x00121E04
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 34);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 41);
				LinkButton linkButton;
				VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 41);
				LinkButton linkButton2;
				VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 56);
				CheckSwitch checkSwitch;
				VisualDiagnostics.RegisterSourceInfo(checkSwitch = new CheckSwitch(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 38);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\MainPageConfigurationScreen.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				stackLayout.SetValue(Grid.ColumnProperty, 0);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Title";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout.Children.Add(label);
				grid.Children.Add(stackLayout);
				stackLayout2.SetValue(Grid.ColumnProperty, 1);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
				linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
				linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
				linkButton.Clicked += this.root.btnUp_Clicked;
				dynamicResourceExtension.Key = "TC_up_image";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array, 4, num);
				object[] array2 = array;
				array2[0] = linkButton;
				array2[1] = stackLayout2;
				array2[2] = grid;
				array2[3] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Button.ImageProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MainPageConfigurationScreen.<InitializeComponent>_anonXamlCDataTemplate_86).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				linkButton.SetDynamicResource(Button.ImageProperty, dynamicResource.Key);
				linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout2.Children.Add(linkButton);
				linkButton2.SetValue(View.MarginProperty, new Thickness(0.0));
				linkButton2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
				linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
				linkButton2.Clicked += this.root.btnDown_Clicked;
				dynamicResourceExtension2.Key = "TC_down_image";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = linkButton2;
				array4[1] = stackLayout2;
				array4[2] = grid;
				array4[3] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Button.ImageProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MainPageConfigurationScreen.<InitializeComponent>_anonXamlCDataTemplate_86).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource2.Key);
				linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout2.Children.Add(linkButton2);
				bindingExtension2.Mode = 1;
				bindingExtension2.Path = "IsVisible";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				checkSwitch.SetBinding(CheckSwitch.IsToggledProperty, bindingBase2);
				checkSwitch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout2.Children.Add(checkSwitch);
				grid.Children.Add(stackLayout2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000BB2 RID: 2994
			internal object[] parentValues;

			// Token: 0x04000BB3 RID: 2995
			internal MainPageConfigurationScreen root;
		}
	}
}
