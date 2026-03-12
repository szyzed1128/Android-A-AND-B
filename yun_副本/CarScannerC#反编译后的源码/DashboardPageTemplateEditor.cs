using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000B5 RID: 181
	[XamlFilePath("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml")]
	public class DashboardPageTemplateEditor : ContentPage
	{
		// Token: 0x06000385 RID: 901 RVA: 0x000263AC File Offset: 0x000245AC
		public DashboardPageTemplateEditor(DashboardPage page)
		{
			this.InitializeComponent();
			if (PlatformHelper.IsiOS)
			{
				Page.SetPrefersHomeIndicatorAutoHidden(this, false);
			}
			this.page = page;
			DescriptionPage descriptionPage = StaticLists.DashboardPageTypes.FirstOrDefault((DescriptionPage x) => x.PreviewFile == this.page.PreviewFile);
			this.lv.SelectedItem = descriptionPage;
			try
			{
				this.lv.ScrollTo(descriptionPage, 2, false);
			}
			catch
			{
			}
		}

		// Token: 0x06000386 RID: 902 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00026420 File Offset: 0x00024620
		protected override bool OnBackButtonPressed()
		{
			this.btnBack_Clicked(this.btnBack, null);
			return true;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00026430 File Offset: 0x00024630
		private void btnBack_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00026440 File Offset: 0x00024640
		private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			DescriptionPage descriptionPage = e.Item as DescriptionPage;
			DashboardPage newPage = DashboardPage.GetDashboardPageFromDashboardType(descriptionPage.DashboardType);
			newPage.CreateGrid();
			if (newPage is Dash_CustomPage)
			{
				int num = DashboardListViewModel.Current.Pages.IndexOf(this.page);
				if (num >= 0 && num < DashboardListViewModel.Current.Pages.Count)
				{
					List<ProxyItem> list = this.page.Items.Select((DashboardItem x) => new ProxyItem(x)).ToList<ProxyItem>();
					newPage.Title = this.page.Title;
					ProxyPage proxyPage = new ProxyPage(newPage);
					proxyPage.Items = list;
					List<ProxyPage> list2 = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
					list2[num] = proxyPage;
					DashboardListViewModel.Current.SaveDashboardToSettings(list2);
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
					if (page != null)
					{
						page.BindingContext = DashboardListViewModel.Current.Pages[num];
					}
					await base.Navigation.PopAsync();
				}
			}
			else
			{
				bool flag = true;
				if (newPage.ItemsCount < this.page.ItemsCount)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_TemplateSmaller_Title"), Translate.GetString("ios_TemplateSmaller_Text"), Translate.GetString("ios_Yes"), Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					flag = taskAwaiter.GetResult();
				}
				if (flag)
				{
					int num2 = DashboardListViewModel.Current.Pages.IndexOf(this.page);
					if (num2 >= 0 && num2 < DashboardListViewModel.Current.Pages.Count)
					{
						int num3 = ((newPage.ItemsCount < this.page.ItemsCount) ? newPage.ItemsCount : this.page.ItemsCount);
						for (int i = 0; i < num3; i++)
						{
							newPage.Items[i] = this.page.Items[i];
						}
						newPage.Title = this.page.Title;
						ProxyPage proxyPage2 = new ProxyPage(newPage);
						List<ProxyPage> list3 = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
						list3[num2] = proxyPage2;
						DashboardListViewModel.Current.SaveDashboardToSettings(list3);
						DashboardListViewModel.Current.LoadDashboardFromSettings();
						Page page2 = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
						if (page2 != null)
						{
							page2.BindingContext = DashboardListViewModel.Current.Pages[num2];
						}
						await base.Navigation.PopAsync();
					}
				}
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x000226CC File Offset: 0x000208CC
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_ChooseDashboardPageType"), Translate.GetString("ios_ChooseDashboardPageType_InfoText"), "OK");
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00026480 File Offset: 0x00024680
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardPageTemplateEditor.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 17);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 17);
			List<DescriptionPage> dashboardPageTypes;
			VisualDiagnostics.RegisterSourceInfo(dashboardPageTypes = StaticLists.DashboardPageTypes, new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(1), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("btnBack", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnBack";
			}
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			this.gridButtons = grid;
			this.btnBack = linkButton;
			this.LayoutRoot = grid2;
			this.lv = listView;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 20.0, 5.0, 5.0));
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 0);
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
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
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
			linkButton.Clicked += this.btnBack_Clicked;
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
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(28, 17)));
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
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			onPlatform.iOS = true;
			onPlatform.Android = true;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = nonScalableLabel;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_ChooseDashboardPageType";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
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
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension4.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton2;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Button.ImageProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource4.Key);
			dynamicResourceExtension5.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource5.Key);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform2.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform2.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform2);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			listView.SetValue(Grid.RowProperty, 1);
			dynamicResourceExtension6.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = listView;
			array8[1] = grid2;
			array8[2] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardPageTemplateEditor).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 17)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			listView.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemTapped += this.Handle_ItemTapped;
			bindingExtension.Source = dashboardPageTypes;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			DashboardPageTemplateEditor.<InitializeComponent>_anonXamlCDataTemplate_43 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardPageTemplateEditor.<InitializeComponent>_anonXamlCDataTemplate_43();
			object[] array9 = new object[0 + 4];
			array9[0] = dataTemplate;
			array9[1] = listView;
			array9[2] = grid2;
			array9[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00027673 File Offset: 0x00025873
		[CompilerGenerated]
		private bool <.ctor>b__0_0(DescriptionPage x)
		{
			return x.PreviewFile == this.page.PreviewFile;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0002768C File Offset: 0x0002588C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardPageTemplateEditor>(this, typeof(DashboardPageTemplateEditor));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
		}

		// Token: 0x0400025C RID: 604
		private DashboardPage page;

		// Token: 0x0400025D RID: 605
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x0400025E RID: 606
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x0400025F RID: 607
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x04000260 RID: 608
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x020000B6 RID: 182
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600038E RID: 910 RVA: 0x000276EE File Offset: 0x000258EE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600038F RID: 911 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06000390 RID: 912 RVA: 0x000276FA File Offset: 0x000258FA
			internal ProxyItem <Handle_ItemTapped>b__5_0(DashboardItem x)
			{
				return new ProxyItem(x);
			}

			// Token: 0x06000391 RID: 913 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <Handle_ItemTapped>b__5_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x06000392 RID: 914 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <Handle_ItemTapped>b__5_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x06000393 RID: 915 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <Handle_ItemTapped>b__5_3(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x06000394 RID: 916 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <Handle_ItemTapped>b__5_4(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x04000261 RID: 609
			public static readonly DashboardPageTemplateEditor.<>c <>9 = new DashboardPageTemplateEditor.<>c();

			// Token: 0x04000262 RID: 610
			public static Func<DashboardItem, ProxyItem> <>9__5_0;

			// Token: 0x04000263 RID: 611
			public static Func<DashboardPage, ProxyPage> <>9__5_1;

			// Token: 0x04000264 RID: 612
			public static Func<Page, bool> <>9__5_2;

			// Token: 0x04000265 RID: 613
			public static Func<DashboardPage, ProxyPage> <>9__5_3;

			// Token: 0x04000266 RID: 614
			public static Func<Page, bool> <>9__5_4;
		}

		// Token: 0x020000B7 RID: 183
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ItemTapped>d__5 : IAsyncStateMachine
		{
			// Token: 0x06000395 RID: 917 RVA: 0x00027710 File Offset: 0x00025910
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardPageTemplateEditor dashboardPageTemplateEditor = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					bool flag;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<Page> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Page>);
						num2 = -1;
						break;
					}
					case 1:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0297;
					case 2:
					{
						TaskAwaiter<Page> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_0469;
					}
					default:
					{
						DescriptionPage descriptionPage = e.Item as DescriptionPage;
						newPage = DashboardPage.GetDashboardPageFromDashboardType(descriptionPage.DashboardType);
						newPage.CreateGrid();
						if (newPage is Dash_CustomPage)
						{
							int num3 = DashboardListViewModel.Current.Pages.IndexOf(dashboardPageTemplateEditor.page);
							if (num3 < 0 || num3 >= DashboardListViewModel.Current.Pages.Count)
							{
								goto IL_0493;
							}
							List<ProxyItem> list = dashboardPageTemplateEditor.page.Items.Select((DashboardItem x) => new ProxyItem(x)).ToList<ProxyItem>();
							newPage.Title = dashboardPageTemplateEditor.page.Title;
							ProxyPage proxyPage = new ProxyPage(newPage);
							proxyPage.Items = list;
							List<ProxyPage> list2 = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
							list2[num3] = proxyPage;
							DashboardListViewModel.Current.SaveDashboardToSettings(list2);
							DashboardListViewModel.Current.LoadDashboardFromSettings();
							Page page = dashboardPageTemplateEditor.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
							if (page != null)
							{
								page.BindingContext = DashboardListViewModel.Current.Pages[num3];
							}
							taskAwaiter3 = dashboardPageTemplateEditor.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardPageTemplateEditor.<Handle_ItemTapped>d__5>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							flag = true;
							if (newPage.ItemsCount >= dashboardPageTemplateEditor.page.ItemsCount)
							{
								goto IL_02A8;
							}
							taskAwaiter5 = dashboardPageTemplateEditor.DisplayAlert(Translate.GetString("ios_TemplateSmaller_Title"), Translate.GetString("ios_TemplateSmaller_Text"), Translate.GetString("ios_Yes"), Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardPageTemplateEditor.<Handle_ItemTapped>d__5>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0297;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_0471;
					IL_0297:
					flag = taskAwaiter5.GetResult();
					IL_02A8:
					if (!flag)
					{
						goto IL_0471;
					}
					int num4 = DashboardListViewModel.Current.Pages.IndexOf(dashboardPageTemplateEditor.page);
					if (num4 < 0 || num4 >= DashboardListViewModel.Current.Pages.Count)
					{
						goto IL_0493;
					}
					int num5 = ((newPage.ItemsCount < dashboardPageTemplateEditor.page.ItemsCount) ? newPage.ItemsCount : dashboardPageTemplateEditor.page.ItemsCount);
					for (int i = 0; i < num5; i++)
					{
						newPage.Items[i] = dashboardPageTemplateEditor.page.Items[i];
					}
					newPage.Title = dashboardPageTemplateEditor.page.Title;
					ProxyPage proxyPage2 = new ProxyPage(newPage);
					List<ProxyPage> list3 = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
					list3[num4] = proxyPage2;
					DashboardListViewModel.Current.SaveDashboardToSettings(list3);
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					Page page2 = dashboardPageTemplateEditor.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
					if (page2 != null)
					{
						page2.BindingContext = DashboardListViewModel.Current.Pages[num4];
					}
					taskAwaiter3 = dashboardPageTemplateEditor.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardPageTemplateEditor.<Handle_ItemTapped>d__5>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0469:
					taskAwaiter3.GetResult();
					IL_0471:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					newPage = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0493:
				num2 = -2;
				newPage = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06000396 RID: 918 RVA: 0x00027BE8 File Offset: 0x00025DE8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000267 RID: 615
			public int <>1__state;

			// Token: 0x04000268 RID: 616
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000269 RID: 617
			public ItemTappedEventArgs e;

			// Token: 0x0400026A RID: 618
			public DashboardPageTemplateEditor <>4__this;

			// Token: 0x0400026B RID: 619
			private DashboardPage <newPage>5__2;

			// Token: 0x0400026C RID: 620
			private TaskAwaiter<Page> <>u__1;

			// Token: 0x0400026D RID: 621
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020000B8 RID: 184
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_43
		{
			// Token: 0x06000397 RID: 919 RVA: 0x00027BF8 File Offset: 0x00025DF8
			public <InitializeComponent>_anonXamlCDataTemplate_43()
			{
			}

			// Token: 0x06000398 RID: 920 RVA: 0x00027C0C File Offset: 0x00025E0C
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 37);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 34);
				ContentView contentView;
				VisualDiagnostics.RegisterSourceInfo(contentView = new ContentView(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardPageTemplateEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				image.SetValue(View.MarginProperty, new Thickness(10.0));
				image.SetValue(VisualElement.HeightRequestProperty, 150.0);
				image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				image.SetValue(VisualElement.MinimumHeightRequestProperty, 150.0);
				bindingExtension.Path = "PreviewFile";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				image.SetBinding(Image.SourceProperty, bindingBase);
				contentView.SetValue(ContentView.ContentProperty, image);
				viewCell.View = contentView;
				return viewCell;
			}

			// Token: 0x0400026E RID: 622
			internal object[] parentValues;

			// Token: 0x0400026F RID: 623
			internal DashboardPageTemplateEditor root;
		}
	}
}
