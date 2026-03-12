using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000A7 RID: 167
	[XamlFilePath("Pages\\Dashboard\\DashboardEditorPage.xaml")]
	public class DashboardEditorPage : ContentPage
	{
		// Token: 0x0600034A RID: 842 RVA: 0x0001E492 File Offset: 0x0001C692
		public DashboardEditorPage()
		{
			this.InitializeComponent();
			this.lvItems.DragDropController.UpdateSource = true;
			if (PlatformHelper.IsiOS)
			{
				Page.SetPrefersHomeIndicatorAutoHidden(this, false);
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0001E4BF File Offset: 0x0001C6BF
		protected override bool OnBackButtonPressed()
		{
			this.btnBack_Clicked(this.btnBack, null);
			return true;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0001E4D0 File Offset: 0x0001C6D0
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			if (sender != null)
			{
				(sender as Button).IsEnabled = false;
			}
			if (this.activityFrame != null)
			{
				this.activityFrame.IsVisible = true;
			}
			if (this != null)
			{
				base.IsEnabled = false;
			}
			await Task.Run(delegate
			{
				DashboardPage dashboardPage = base.BindingContext as DashboardPage;
				if (dashboardPage != null && DashboardListViewModel.Current.Pages != null)
				{
					int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
					if (num >= 0)
					{
						ProxyPage proxyPage = new ProxyPage(dashboardPage);
						List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
						if (num < list.Count)
						{
							list[num] = proxyPage;
						}
						else
						{
							list.Add(proxyPage);
						}
						DashboardListViewModel.Current.SaveDashboardToSettings(list);
					}
				}
			});
			if (DashboardXamlPage.Instance != null)
			{
				DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
			}
			if (this.activityFrame != null)
			{
				this.activityFrame.IsVisible = false;
			}
			try
			{
				base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001E510 File Offset: 0x0001C710
		private async void btnDelPage_Clicked(object sender, EventArgs e)
		{
			if (DashboardListViewModel.Current.Pages.Count == 1)
			{
				await base.DisplayAlert(Translate.GetString("ios_DashboardEditor_LastPageTitle"), Translate.GetString("ios_DashboardEditor_LastPageText"), "OK");
			}
			else
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DashboardEditor_DelPageTitle"), Translate.GetString("ios_DashboardEditor_DelPageText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					DashboardPage dashboardPage = base.BindingContext as DashboardPage;
					DashboardListViewModel.Current.Pages.Remove(dashboardPage);
					base.Navigation.PopAsync();
				}
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x0001E548 File Offset: 0x0001C748
		private void btnEdit_Clicked(object sender, EventArgs e)
		{
			try
			{
				DashboardItem dashboardItem = (sender as Button).BindingContext as DashboardItem;
				if (App.UseLegacyUI)
				{
					if (PlatformHelper.IsiOS)
					{
						DashboardItemEditor dashboardItemEditor = new DashboardItemEditor(dashboardItem, dashboardItem.Width, dashboardItem.Height);
						this.lvItems.IsVisible = false;
						base.Navigation.PushAsync(dashboardItemEditor, true);
					}
				}
				else
				{
					DashboardItemEditorV3 dashboardItemEditorV = new DashboardItemEditorV3(dashboardItem, dashboardItem.Width, dashboardItem.Height);
					this.lvItems.IsVisible = false;
					base.Navigation.PushAsync(dashboardItemEditorV, true);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000350 RID: 848 RVA: 0x0001E5E8 File Offset: 0x0001C7E8
		private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e == null || e.ItemData == null || !(e.ItemData is DashboardItem))
			{
				return;
			}
			try
			{
				DashboardItem dashboardItem = (DashboardItem)e.ItemData;
				if (App.UseLegacyUI)
				{
					if (PlatformHelper.IsiOS)
					{
						DashboardItemEditor dashboardItemEditor = new DashboardItemEditor(dashboardItem, dashboardItem.Width, dashboardItem.Height);
						this.lvItems.IsVisible = false;
						base.Navigation.PushAsync(dashboardItemEditor, true);
					}
				}
				else
				{
					DashboardItemEditorV3 dashboardItemEditorV = new DashboardItemEditorV3(dashboardItem, dashboardItem.Width, dashboardItem.Height);
					this.lvItems.IsVisible = false;
					base.Navigation.PushAsync(dashboardItemEditorV, true);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000351 RID: 849 RVA: 0x0001E69C File Offset: 0x0001C89C
		private void btnUp_Clicked(object sender, EventArgs e)
		{
			DashboardItem dashboardItem = (sender as Button).BindingContext as DashboardItem;
			DashboardPage dashboardPage = base.BindingContext as DashboardPage;
			int num = dashboardPage.Items.IndexOf(dashboardItem);
			if (num > 0)
			{
				dashboardPage.Items.Move(num, num - 1);
			}
		}

		// Token: 0x06000352 RID: 850 RVA: 0x0001E6E8 File Offset: 0x0001C8E8
		private void btnDown_Clicked(object sender, EventArgs e)
		{
			DashboardItem dashboardItem = (sender as Button).BindingContext as DashboardItem;
			DashboardPage dashboardPage = base.BindingContext as DashboardPage;
			int num = dashboardPage.Items.IndexOf(dashboardItem);
			if (num < dashboardPage.Items.Count - 1)
			{
				dashboardPage.Items.Move(num, num + 1);
			}
		}

		// Token: 0x06000353 RID: 851 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06000354 RID: 852 RVA: 0x0001E740 File Offset: 0x0001C940
		private void Handle_Appearing(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = base.BindingContext as DashboardPage;
			this.lvItems.ItemsSource = null;
			this.lvItems.ItemsSource = dashboardPage.Items;
			this.lvItems.IsVisible = true;
		}

		// Token: 0x06000355 RID: 853 RVA: 0x0001E782 File Offset: 0x0001C982
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DashboardEditor"), Translate.GetString("ios_DashboardEditor_InfoText"), "OK");
		}

		// Token: 0x06000356 RID: 854 RVA: 0x0001E7A4 File Offset: 0x0001C9A4
		private void img_Tapped(object sender, EventArgs e)
		{
			DashboardPageTemplateEditor dashboardPageTemplateEditor = new DashboardPageTemplateEditor(base.BindingContext as DashboardPage);
			base.Navigation.PushAsync(dashboardPageTemplateEditor);
		}

		// Token: 0x06000357 RID: 855 RVA: 0x0001E7D0 File Offset: 0x0001C9D0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardEditorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardEditorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 21);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 21);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 30);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 30);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 22);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 17);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("btnDelPage", linkButton3);
			if (linkButton3.StyleId == null)
			{
				linkButton3.StyleId = "btnDelPage";
			}
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("lvItems", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lvItems";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.gridButtons = grid;
			this.btnBack = linkButton;
			this.btnDelPage = linkButton3;
			this.LayoutRoot = grid2;
			this.lvItems = sfListView;
			this.activityFrame = activityFrame;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 0);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Handle_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
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
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
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
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 17)));
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
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_DashboardEditor";
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
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			grid.Children.Add(nonScalableLabel);
			stackLayout.SetValue(Grid.ColumnProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension4.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = linkButton2;
			array6[1] = stackLayout;
			array6[2] = grid;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Button.ImageProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 21)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource4.Key);
			dynamicResourceExtension5.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = linkButton2;
			array7[1] = stackLayout;
			array7[2] = grid;
			array7[3] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 21)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource5.Key);
			stackLayout.Children.Add(linkButton2);
			linkButton3.SetValue(Grid.ColumnProperty, 2);
			linkButton3.Clicked += this.btnDelPage_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension6.Key = "NB_delete";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = linkButton3;
			array8[1] = stackLayout;
			array8[2] = grid;
			array8[3] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Button.ImageProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 21)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton3.SetDynamicResource(Button.ImageProperty, dynamicResource6.Key);
			dynamicResourceExtension7.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = linkButton3;
			array9[1] = stackLayout;
			array9[2] = grid;
			array9[3] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 21)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			linkButton3.SetDynamicResource(VisualElement.StyleProperty, dynamicResource7.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "0";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on2);
			linkButton3.SetValue(View.MarginProperty, onPlatform2);
			stackLayout.Children.Add(linkButton3);
			grid.Children.Add(stackLayout);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform3.WinPhone = new Thickness(0.0);
			onPlatform3.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			sfListView.SetValue(Grid.RowProperty, 0);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.SetValue(SfListView.DragStartModeProperty, 1);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Handle_ItemTapped);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "Items";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase);
			sfListView.SetValue(SfListView.SelectionBackgroundColorProperty, Color.Transparent);
			IDataTemplate dataTemplate3 = dataTemplate;
			DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35();
			object[] array10 = new object[0 + 4];
			array10[0] = dataTemplate;
			array10[1] = sfListView;
			array10[2] = grid2;
			array10[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array10;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.HeaderTemplateProperty, dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36 <InitializeComponent>_anonXamlCDataTemplate_2 = new DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36();
			object[] array11 = new object[0 + 4];
			array11[0] = dataTemplate2;
			array11[1] = sfListView;
			array11[2] = grid2;
			array11[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array11;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate2);
			grid2.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate3.Text = "ios_SavingDashboard";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = activityFrame;
			array12[1] = grid2;
			array12[2] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array12, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DashboardEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(215, 17)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			activityFrame.Text = obj13;
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06000358 RID: 856 RVA: 0x0001FF94 File Offset: 0x0001E194
		[CompilerGenerated]
		private void <btnBack_Clicked>b__3_0()
		{
			DashboardPage dashboardPage = base.BindingContext as DashboardPage;
			if (dashboardPage != null && DashboardListViewModel.Current.Pages != null)
			{
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				if (num >= 0)
				{
					ProxyPage proxyPage = new ProxyPage(dashboardPage);
					List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
					if (num < list.Count)
					{
						list[num] = proxyPage;
					}
					else
					{
						list.Add(proxyPage);
					}
					DashboardListViewModel.Current.SaveDashboardToSettings(list);
				}
			}
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00020034 File Offset: 0x0001E234
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardEditorPage>(this, typeof(DashboardEditorPage));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.btnDelPage = NameScopeExtensions.FindByName<LinkButton>(this, "btnDelPage");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.lvItems = NameScopeExtensions.FindByName<SfListView>(this, "lvItems");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04000221 RID: 545
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000222 RID: 546
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x04000223 RID: 547
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnDelPage;

		// Token: 0x04000224 RID: 548
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x04000225 RID: 549
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvItems;

		// Token: 0x04000226 RID: 550
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x020000A8 RID: 168
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600035A RID: 858 RVA: 0x000200B8 File Offset: 0x0001E2B8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600035B RID: 859 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600035C RID: 860 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <btnBack_Clicked>b__3_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x04000227 RID: 551
			public static readonly DashboardEditorPage.<>c <>9 = new DashboardEditorPage.<>c();

			// Token: 0x04000228 RID: 552
			public static Func<DashboardPage, ProxyPage> <>9__3_1;
		}

		// Token: 0x020000A9 RID: 169
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600035D RID: 861 RVA: 0x000200CC File Offset: 0x0001E2CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardEditorPage dashboardEditorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (sender != null)
						{
							(sender as Button).IsEnabled = false;
						}
						if (dashboardEditorPage.activityFrame != null)
						{
							dashboardEditorPage.activityFrame.IsVisible = true;
						}
						if (dashboardEditorPage != null)
						{
							dashboardEditorPage.IsEnabled = false;
						}
						taskAwaiter = Task.Run(delegate
						{
							DashboardPage dashboardPage = base.BindingContext as DashboardPage;
							if (dashboardPage != null && DashboardListViewModel.Current.Pages != null)
							{
								int num3 = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
								if (num3 >= 0)
								{
									ProxyPage proxyPage = new ProxyPage(dashboardPage);
									List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
									if (num3 < list.Count)
									{
										list[num3] = proxyPage;
									}
									else
									{
										list.Add(proxyPage);
									}
									DashboardListViewModel.Current.SaveDashboardToSettings(list);
								}
							}
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardEditorPage.<btnBack_Clicked>d__3>(ref taskAwaiter, ref this);
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
					if (DashboardXamlPage.Instance != null)
					{
						DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
					}
					if (dashboardEditorPage.activityFrame != null)
					{
						dashboardEditorPage.activityFrame.IsVisible = false;
					}
					try
					{
						dashboardEditorPage.Navigation.PopAsync();
					}
					catch (Exception)
					{
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

			// Token: 0x0600035E RID: 862 RVA: 0x00020208 File Offset: 0x0001E408
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000229 RID: 553
			public int <>1__state;

			// Token: 0x0400022A RID: 554
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400022B RID: 555
			public object sender;

			// Token: 0x0400022C RID: 556
			public DashboardEditorPage <>4__this;

			// Token: 0x0400022D RID: 557
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000AA RID: 170
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelPage_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600035F RID: 863 RVA: 0x00020218 File Offset: 0x0001E418
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardEditorPage dashboardEditorPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						TaskAwaiter<bool> taskAwaiter5;
						if (num != 1)
						{
							if (DashboardListViewModel.Current.Pages.Count == 1)
							{
								taskAwaiter3 = dashboardEditorPage.DisplayAlert(Translate.GetString("ios_DashboardEditor_LastPageTitle"), Translate.GetString("ios_DashboardEditor_LastPageText"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardEditorPage.<btnDelPage_Clicked>d__4>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0097;
							}
							else
							{
								taskAwaiter5 = dashboardEditorPage.DisplayAlert(Translate.GetString("ios_DashboardEditor_DelPageTitle"), Translate.GetString("ios_DashboardEditor_DelPageText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardEditorPage.<btnDelPage_Clicked>d__4>(ref taskAwaiter5, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						if (taskAwaiter5.GetResult())
						{
							DashboardPage dashboardPage = dashboardEditorPage.BindingContext as DashboardPage;
							DashboardListViewModel.Current.Pages.Remove(dashboardPage);
							dashboardEditorPage.Navigation.PopAsync();
							goto IL_0150;
						}
						goto IL_0150;
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					IL_0097:
					taskAwaiter3.GetResult();
					IL_0150:;
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

			// Token: 0x06000360 RID: 864 RVA: 0x000203C0 File Offset: 0x0001E5C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400022E RID: 558
			public int <>1__state;

			// Token: 0x0400022F RID: 559
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000230 RID: 560
			public DashboardEditorPage <>4__this;

			// Token: 0x04000231 RID: 561
			private TaskAwaiter <>u__1;

			// Token: 0x04000232 RID: 562
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020000AB RID: 171
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_35
		{
			// Token: 0x06000361 RID: 865 RVA: 0x000203D0 File Offset: 0x0001E5D0
			public <InitializeComponent>_anonXamlCDataTemplate_35()
			{
			}

			// Token: 0x06000362 RID: 866 RVA: 0x000203E4 File Offset: 0x0001E5E4
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 34);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 34);
				RowDefinition rowDefinition4;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 34);
				RowDefinition rowDefinition5;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 34);
				RowDefinition rowDefinition6;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 56);
				Entry entry;
				VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 30);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 33);
				TapGestureRecognizer tapGestureRecognizer;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 38);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 33);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 33);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 30);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 33);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 33);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 33);
				CheckBoxWithLabel checkBoxWithLabel;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel = new CheckBoxWithLabel(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 30);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 33);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 33);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 33);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 30);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 49);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 30);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid2, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("220"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
				rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
				rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
				grid.SetValue(Grid.RowProperty, 0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Mode = 1;
				bindingExtension.Path = "ios_DashboardPageTitle";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label);
				entry.SetValue(Grid.ColumnProperty, 1);
				bindingExtension2.Mode = 1;
				bindingExtension2.Path = "Title";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				entry.SetBinding(Entry.TextProperty, bindingBase2);
				grid.Children.Add(entry);
				grid2.Children.Add(grid);
				image.SetValue(Grid.RowProperty, 1);
				image.SetValue(View.MarginProperty, new Thickness(5.0));
				image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension3.Path = "PreviewFile";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				image.SetBinding(Image.SourceProperty, bindingBase3);
				tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
				tapGestureRecognizer.Tapped += this.root.img_Tapped;
				image.GestureRecognizers.Add(tapGestureRecognizer);
				grid2.Children.Add(image);
				label2.SetValue(Grid.RowProperty, 2);
				label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label2;
				array2[1] = grid2;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				translate.Text = "ios_DashboardChangeTemplateHint";
				IMarkupExtension markupExtension2 = translate;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid2;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 33)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.Text = obj3;
				grid2.Children.Add(label2);
				checkBoxWithLabel.SetValue(Grid.RowProperty, 3);
				bindingExtension4.Mode = 1;
				bindingExtension4.Path = "UpdateInBackground";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				checkBoxWithLabel.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase4);
				translate2.Text = "dash_UpdatePageInBackground";
				IMarkupExtension markupExtension3 = translate2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = checkBoxWithLabel;
				array6[1] = grid2;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, CheckBoxWithLabel.TextProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 33)));
				object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
				checkBoxWithLabel.Text = obj5;
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = checkBoxWithLabel;
				array8[1] = grid2;
				object obj6;
				xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array8, CheckBoxWithLabel.TextColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 33)));
				DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
				checkBoxWithLabel.SetDynamicResource(CheckBoxWithLabel.TextColorProperty, dynamicResource2.Key);
				grid2.Children.Add(checkBoxWithLabel);
				label3.SetValue(Grid.RowProperty, 4);
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "UpdateInBackground";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				translate3.Text = "dash_UpdatePageInBackgroundWarning";
				IMarkupExtension markupExtension5 = translate3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array9, 2, num5);
				object[] array10 = array9;
				array10[0] = label3;
				array10[1] = grid2;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 33)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label3.Text = obj8;
				dynamicResourceExtension3.Key = "RedTextColor";
				IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array11, 2, num6);
				object[] array12 = array11;
				array12[0] = label3;
				array12[1] = grid2;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, Label.TextColorProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 33)));
				DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
				label3.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
				grid2.Children.Add(label3);
				label4.SetValue(Grid.RowProperty, 5);
				translate4.Text = "ios_DashboardPageItems";
				IMarkupExtension markupExtension7 = translate4;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array13, 2, num7);
				object[] array14 = array13;
				array14[0] = label4;
				array14[1] = grid2;
				object obj10;
				xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_35).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 49)));
				object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label4.Text = obj11;
				grid2.Children.Add(label4);
				return grid2;
			}

			// Token: 0x04000233 RID: 563
			internal object[] parentValues;

			// Token: 0x04000234 RID: 564
			internal DashboardEditorPage root;
		}

		// Token: 0x020000AC RID: 172
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_36
		{
			// Token: 0x06000363 RID: 867 RVA: 0x000215C8 File Offset: 0x0001F7C8
			public <InitializeComponent>_anonXamlCDataTemplate_36()
			{
			}

			// Token: 0x06000364 RID: 868 RVA: 0x000215DC File Offset: 0x0001F7DC
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 34);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 34);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 67);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 67);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 37);
				LinkButton linkButton;
				VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 34);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 37);
				LinkButton linkButton2;
				VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 34);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 37);
				LinkButton linkButton3;
				VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 34);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 30);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("1.5"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				stackLayout.SetValue(Grid.RowProperty, 0);
				stackLayout.SetValue(Grid.ColumnProperty, 0);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "PlacePlusOne";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				stackLayout.Children.Add(label);
				label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "ProxyPidName";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout.Children.Add(label2);
				grid.Children.Add(stackLayout);
				stackLayout2.SetValue(Grid.RowProperty, 0);
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
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = linkButton;
				array2[1] = stackLayout2;
				array2[2] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Button.ImageProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(182, 37)));
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
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = linkButton2;
				array4[1] = stackLayout2;
				array4[2] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Button.ImageProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 37)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource2.Key);
				linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout2.Children.Add(linkButton2);
				linkButton3.SetValue(Grid.ColumnProperty, 1);
				linkButton3.SetValue(View.MarginProperty, new Thickness(0.0));
				linkButton3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
				linkButton3.SetValue(Button.BorderColorProperty, Color.Transparent);
				linkButton3.Clicked += this.root.btnEdit_Clicked;
				dynamicResourceExtension3.Key = "TC_settings";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = linkButton3;
				array6[1] = stackLayout2;
				array6[2] = grid;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Button.ImageProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 37)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				linkButton3.SetDynamicResource(Button.ImageProperty, dynamicResource3.Key);
				stackLayout2.Children.Add(linkButton3);
				grid.Children.Add(stackLayout2);
				frame.SetValue(Grid.RowProperty, 1);
				frame.SetValue(Grid.ColumnProperty, 0);
				frame.SetValue(Grid.ColumnSpanProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension4.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = frame;
				array8[1] = grid;
				object obj4;
				xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array8, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardEditorPage.<InitializeComponent>_anonXamlCDataTemplate_36).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 33)));
				DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource4.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x04000235 RID: 565
			internal object[] parentValues;

			// Token: 0x04000236 RID: 566
			internal DashboardEditorPage root;
		}
	}
}
