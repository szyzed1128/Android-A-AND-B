using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006B5 RID: 1717
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\Dashboard\\DashboardItemTypeSelector.xaml")]
	public class DashboardItemTypeSelector : ContentPage
	{
		// Token: 0x06003ADF RID: 15071 RVA: 0x0030F386 File Offset: 0x0030D586
		public DashboardItemTypeSelector(IPID pid)
		{
			this.InitializeComponent();
			this.Init(pid);
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x0030F3B4 File Offset: 0x0030D5B4
		private async void Init(IPID pid)
		{
			DashboardItemTypeSelector.<>c__DisplayClass2_0 CS$<>8__locals1 = new DashboardItemTypeSelector.<>c__DisplayClass2_0();
			CS$<>8__locals1.pid = pid;
			CS$<>8__locals1.<>4__this = this;
			double num = ((DeviceDisplay.MainDisplayInfo.Width > DeviceDisplay.MainDisplayInfo.Height) ? DeviceDisplay.MainDisplayInfo.Height : DeviceDisplay.MainDisplayInfo.Width);
			num /= DeviceDisplay.MainDisplayInfo.Density;
			CS$<>8__locals1.size = Math.Round(num / 2.5, 0);
			await Task.Run(delegate
			{
				DashboardItemTypeSelector.<>c__DisplayClass2_0.<<Init>b__0>d <<Init>b__0>d;
				<<Init>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Init>b__0>d.<>4__this = CS$<>8__locals1;
				<<Init>b__0>d.<>1__state = -1;
				<<Init>b__0>d.<>t__builder.Start<DashboardItemTypeSelector.<>c__DisplayClass2_0.<<Init>b__0>d>(ref <<Init>b__0>d);
				return <<Init>b__0>d.<>t__builder.Task;
			});
			MainThread.BeginInvokeOnMainThread(delegate
			{
				foreach (DashboardItem dashboardItem in CS$<>8__locals1.<>4__this.items)
				{
					Frame frame = new Frame
					{
						BorderColor = (Color)Application.Current.Resources["BackgroundColor"],
						WidthRequest = CS$<>8__locals1.size,
						HeightRequest = CS$<>8__locals1.size,
						Padding = new Thickness(3.0)
					};
					TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.Tapped += CS$<>8__locals1.<>4__this.Tap_Tapped;
					frame.GestureRecognizers.Add(tapGestureRecognizer);
					frame.Content = dashboardItem;
					CS$<>8__locals1.<>4__this.uniGrid.Children.Add(frame);
				}
				CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
			});
			if (!App.OBDSimulator.IsActive)
			{
				List<OBDRequest> list = new List<OBDRequest>(4);
				this.items.FirstOrDefault<DashboardItem>().Model.GetRequests(list, null, "");
				App.OBDReader.ReplaceQueue(list);
			}
		}

		// Token: 0x06003AE1 RID: 15073 RVA: 0x0030F3F4 File Offset: 0x0030D5F4
		private async void Tap_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			Frame frame;
			DashboardItem dashboardItem;
			if (sender is Frame)
			{
				frame = (Frame)sender;
				dashboardItem = (DashboardItem)frame.Content;
			}
			else
			{
				dashboardItem = (DashboardItem)sender;
				frame = (Frame)dashboardItem.Parent;
			}
			this.itemTypeSelector.IsEnabled = false;
			if (SharedSettings.Current.DashboardAnimation)
			{
				(frame.Parent as Layout).RaiseChild(frame);
				ViewExtensions.ScaleXTo(frame, 1.2, 250U, null);
			}
			frame.Content = null;
			this.SelectedItemType = new DashboardItemTypes?(dashboardItem.ItemType);
			try
			{
				foreach (DashboardItem dashboardItem2 in this.items)
				{
					if (dashboardItem2.GestureRecognizers.Count > 0)
					{
						TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem2.GestureRecognizers[dashboardItem2.GestureRecognizers.Count - 1];
						tapGestureRecognizer.Tapped -= this.Tap_Tapped;
						dashboardItem2.GestureRecognizers.Remove(tapGestureRecognizer);
					}
					dashboardItem2.Stop();
				}
			}
			catch (Exception)
			{
			}
			try
			{
				foreach (View view in this.uniGrid.Children)
				{
					Frame frame2 = (Frame)view;
					if (frame2.GestureRecognizers.Count > 0)
					{
						((TapGestureRecognizer)frame2.GestureRecognizers[0]).Tapped -= this.Tap_Tapped;
					}
					frame2.GestureRecognizers.Clear();
				}
			}
			catch (Exception)
			{
			}
			base.IsEnabled = true;
			try
			{
				await base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
			try
			{
				this.semaphore.Release();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003AE2 RID: 15074 RVA: 0x0030F433 File Offset: 0x0030D633
		protected override bool OnBackButtonPressed()
		{
			this.btnCancel_Clicked(this, null);
			return true;
		}

		// Token: 0x06003AE3 RID: 15075 RVA: 0x0030F440 File Offset: 0x0030D640
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			try
			{
				foreach (DashboardItem dashboardItem in this.items)
				{
					if (dashboardItem != null)
					{
						dashboardItem.Stop();
						TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[0];
						tapGestureRecognizer.Tapped -= this.Tap_Tapped;
						dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
						Frame frame = (Frame)dashboardItem.Parent;
						tapGestureRecognizer = (TapGestureRecognizer)frame.GestureRecognizers[0];
						tapGestureRecognizer.Tapped -= this.Tap_Tapped;
						frame.GestureRecognizers.Remove(tapGestureRecognizer);
					}
				}
				this.items.Clear();
				this.uniGrid.Children.Clear();
				this.SelectedItemType = null;
			}
			catch (Exception)
			{
			}
			try
			{
				await base.Navigation.PopAsync(true);
			}
			catch (Exception)
			{
			}
			try
			{
				this.semaphore.Release();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x0030F478 File Offset: 0x0030D678
		public static async Task<DashboardItemTypes?> SelectDashboardItemType(IPID pid)
		{
			DashboardItemTypeSelector selector = new DashboardItemTypeSelector(pid);
			await App.GetCurrentPage().Navigation.PushAsync(selector);
			await selector.semaphore.WaitAsync();
			return selector.SelectedItemType;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x0030F4BC File Offset: 0x0030D6BC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardItemTypeSelector.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			UniformGrid uniformGrid;
			VisualDiagnostics.RegisterSourceInfo(uniformGrid = new UniformGrid(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardItemTypeSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("thisPage", this);
			if (this.StyleId == null)
			{
				this.StyleId = "thisPage";
			}
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("lbNavbarTitle", nonScalableLabel);
			if (nonScalableLabel.StyleId == null)
			{
				nonScalableLabel.StyleId = "lbNavbarTitle";
			}
			nameScope.RegisterName("itemTypeSelector", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "itemTypeSelector";
			}
			nameScope.RegisterName("uniGrid", uniformGrid);
			if (uniformGrid.StyleId == null)
			{
				uniformGrid.StyleId = "uniGrid";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.thisPage = this;
			this.gridButtons = grid;
			this.lbNavbarTitle = nonScalableLabel;
			this.itemTypeSelector = scrollView;
			this.uniGrid = uniformGrid;
			this.activityFrame = activityFrame;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "SettingsBackground";
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
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid.SetValue(View.MarginProperty, onPlatform);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnCancel_Clicked;
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
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
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
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
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
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_DashboardItemEditor_DisplayType";
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
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardItemTypeSelector).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(nonScalableLabel);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*"));
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			scrollView.Content = uniformGrid;
			grid2.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x003102D0 File Offset: 0x0030E4D0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardItemTypeSelector>(this, typeof(DashboardItemTypeSelector));
			this.thisPage = NameScopeExtensions.FindByName<ContentPage>(this, "thisPage");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.lbNavbarTitle = NameScopeExtensions.FindByName<NonScalableLabel>(this, "lbNavbarTitle");
			this.itemTypeSelector = NameScopeExtensions.FindByName<ScrollView>(this, "itemTypeSelector");
			this.uniGrid = NameScopeExtensions.FindByName<UniformGrid>(this, "uniGrid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x040023EC RID: 9196
		private List<DashboardItem> items = new List<DashboardItem>();

		// Token: 0x040023ED RID: 9197
		private DashboardItemTypes? SelectedItemType;

		// Token: 0x040023EE RID: 9198
		private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

		// Token: 0x040023EF RID: 9199
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage thisPage;

		// Token: 0x040023F0 RID: 9200
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040023F1 RID: 9201
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel lbNavbarTitle;

		// Token: 0x040023F2 RID: 9202
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView itemTypeSelector;

		// Token: 0x040023F3 RID: 9203
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private UniformGrid uniGrid;

		// Token: 0x040023F4 RID: 9204
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x020006B6 RID: 1718
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06003AE7 RID: 15079 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06003AE8 RID: 15080 RVA: 0x00310354 File Offset: 0x0030E554
			internal async Task <Init>b__0()
			{
				DashboardItemTypeSelector.<>c__DisplayClass2_1 CS$<>8__locals1 = new DashboardItemTypeSelector.<>c__DisplayClass2_1();
				CS$<>8__locals1.CS$<>8__locals1 = this;
				if (this.pid is CustomPID && (this.pid as CustomPID).IsAction)
				{
					DashboardItem dashboardItem = new DashboardItem
					{
						ItemType = DashboardItemTypes.Action,
						PID_Id = this.pid.Id,
						Minimum = this.pid.Minimum,
						Maximum = this.pid.Maximum,
						CustomName = this.pid.ShortName,
						DesiredHeight = this.size,
						DesiredWidth = this.size,
						WidthRequest = this.size,
						HeightRequest = this.size,
						LinearOrientationHorizontal = false
					};
					this.<>4__this.items.Add(dashboardItem);
				}
				else
				{
					DashboardItem dashboardItem2 = new DashboardItem
					{
						ItemType = DashboardItemTypes.Text,
						PID_Id = this.pid.Id,
						Minimum = this.pid.Minimum,
						Maximum = this.pid.Maximum,
						CustomName = this.pid.ShortName,
						DesiredHeight = this.size,
						DesiredWidth = this.size,
						WidthRequest = this.size,
						HeightRequest = this.size,
						ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
					};
					DashboardItem dashboardItem3 = new DashboardItem
					{
						ItemType = DashboardItemTypes.TextHorizontal,
						PID_Id = this.pid.Id,
						Minimum = this.pid.Minimum,
						Maximum = this.pid.Maximum,
						CustomName = this.pid.ShortName,
						DesiredHeight = this.size,
						DesiredWidth = this.size,
						WidthRequest = this.size,
						HeightRequest = this.size,
						ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5,
						UnitsFontSize = Device.GetNamedSize(4, typeof(Label))
					};
					this.<>4__this.items.Add(dashboardItem2);
					this.<>4__this.items.Add(dashboardItem3);
					if (this.pid is IPIDFloatValue)
					{
						DashboardItem dashboardItem4 = new DashboardItem
						{
							ItemType = DashboardItemTypes.Chart,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size
						};
						this.<>4__this.items.Add(dashboardItem4);
						DashboardItem dashboardItem5 = new DashboardItem
						{
							ItemType = DashboardItemTypes.Gauge,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size,
							ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5
						};
						this.<>4__this.items.Add(dashboardItem5);
						DashboardItem dashboardItem6 = new DashboardItem
						{
							ItemType = DashboardItemTypes.GaugeVar2,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size,
							ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
						};
						this.<>4__this.items.Add(dashboardItem6);
						DashboardItem dashboardItem7 = new DashboardItem
						{
							ItemType = DashboardItemTypes.GaugeVar3,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size,
							ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
						};
						this.<>4__this.items.Add(dashboardItem7);
						DashboardItem dashboardItem8 = new DashboardItem
						{
							ItemType = DashboardItemTypes.LinearGauge,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size,
							LinearOrientationHorizontal = true,
							ShowValue = false
						};
						this.<>4__this.items.Add(dashboardItem8);
						DashboardItem dashboardItem9 = new DashboardItem
						{
							ItemType = DashboardItemTypes.LinearGauge,
							PID_Id = this.pid.Id,
							Minimum = this.pid.Minimum,
							Maximum = this.pid.Maximum,
							CustomName = this.pid.ShortName,
							DesiredHeight = this.size,
							DesiredWidth = this.size,
							WidthRequest = this.size,
							HeightRequest = this.size,
							LinearOrientationHorizontal = false,
							ShowValue = false
						};
						this.<>4__this.items.Add(dashboardItem9);
						switch (SharedSettings.Current.DashboardTheme)
						{
						case 0:
						case 2:
							dashboardItem8.GaugePointerColor = Color.Orange;
							dashboardItem9.GaugePointerColor = Color.Orange;
							dashboardItem6.GaugePointerColor = Color.Orange;
							dashboardItem7.GaugePointerColor = Color.Orange;
							break;
						case 1:
							dashboardItem8.GaugePointerColor = Color.Red;
							dashboardItem9.GaugePointerColor = Color.Red;
							dashboardItem6.GaugePointerColor = Color.Red;
							dashboardItem7.GaugePointerColor = Color.Red;
							break;
						}
					}
				}
				CS$<>8__locals1.model = new LiveDataPIDModel();
				await Task.WhenAll(this.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemTypeSelector.<>c__DisplayClass2_2
				{
					CS$<>8__locals2 = CS$<>8__locals1,
					item = item
				}.<Init>b__3))));
			}

			// Token: 0x06003AE9 RID: 15081 RVA: 0x00310398 File Offset: 0x0030E598
			internal void <Init>b__1()
			{
				foreach (DashboardItem dashboardItem in this.<>4__this.items)
				{
					Frame frame = new Frame
					{
						BorderColor = (Color)Application.Current.Resources["BackgroundColor"],
						WidthRequest = this.size,
						HeightRequest = this.size,
						Padding = new Thickness(3.0)
					};
					TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
					tapGestureRecognizer.Tapped += this.<>4__this.Tap_Tapped;
					frame.GestureRecognizers.Add(tapGestureRecognizer);
					frame.Content = dashboardItem;
					this.<>4__this.uniGrid.Children.Add(frame);
				}
				this.<>4__this.activityFrame.IsVisible = false;
			}

			// Token: 0x040023F5 RID: 9205
			public IPID pid;

			// Token: 0x040023F6 RID: 9206
			public double size;

			// Token: 0x040023F7 RID: 9207
			public DashboardItemTypeSelector <>4__this;

			// Token: 0x020006B7 RID: 1719
			[StructLayout(LayoutKind.Auto)]
			private struct <<Init>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06003AEA RID: 15082 RVA: 0x00310498 File Offset: 0x0030E698
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					DashboardItemTypeSelector.<>c__DisplayClass2_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							DashboardItemTypeSelector.<>c__DisplayClass2_1 CS$<>8__locals2 = new DashboardItemTypeSelector.<>c__DisplayClass2_1();
							CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
							if (CS$<>8__locals1.pid is CustomPID && (CS$<>8__locals1.pid as CustomPID).IsAction)
							{
								DashboardItem dashboardItem = new DashboardItem
								{
									ItemType = DashboardItemTypes.Action,
									PID_Id = CS$<>8__locals1.pid.Id,
									Minimum = CS$<>8__locals1.pid.Minimum,
									Maximum = CS$<>8__locals1.pid.Maximum,
									CustomName = CS$<>8__locals1.pid.ShortName,
									DesiredHeight = CS$<>8__locals1.size,
									DesiredWidth = CS$<>8__locals1.size,
									WidthRequest = CS$<>8__locals1.size,
									HeightRequest = CS$<>8__locals1.size,
									LinearOrientationHorizontal = false
								};
								CS$<>8__locals1.<>4__this.items.Add(dashboardItem);
							}
							else
							{
								DashboardItem dashboardItem2 = new DashboardItem
								{
									ItemType = DashboardItemTypes.Text,
									PID_Id = CS$<>8__locals1.pid.Id,
									Minimum = CS$<>8__locals1.pid.Minimum,
									Maximum = CS$<>8__locals1.pid.Maximum,
									CustomName = CS$<>8__locals1.pid.ShortName,
									DesiredHeight = CS$<>8__locals1.size,
									DesiredWidth = CS$<>8__locals1.size,
									WidthRequest = CS$<>8__locals1.size,
									HeightRequest = CS$<>8__locals1.size,
									ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
								};
								DashboardItem dashboardItem3 = new DashboardItem
								{
									ItemType = DashboardItemTypes.TextHorizontal,
									PID_Id = CS$<>8__locals1.pid.Id,
									Minimum = CS$<>8__locals1.pid.Minimum,
									Maximum = CS$<>8__locals1.pid.Maximum,
									CustomName = CS$<>8__locals1.pid.ShortName,
									DesiredHeight = CS$<>8__locals1.size,
									DesiredWidth = CS$<>8__locals1.size,
									WidthRequest = CS$<>8__locals1.size,
									HeightRequest = CS$<>8__locals1.size,
									ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5,
									UnitsFontSize = Device.GetNamedSize(4, typeof(Label))
								};
								CS$<>8__locals1.<>4__this.items.Add(dashboardItem2);
								CS$<>8__locals1.<>4__this.items.Add(dashboardItem3);
								if (CS$<>8__locals1.pid is IPIDFloatValue)
								{
									DashboardItem dashboardItem4 = new DashboardItem
									{
										ItemType = DashboardItemTypes.Chart,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem4);
									DashboardItem dashboardItem5 = new DashboardItem
									{
										ItemType = DashboardItemTypes.Gauge,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size,
										ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem5);
									DashboardItem dashboardItem6 = new DashboardItem
									{
										ItemType = DashboardItemTypes.GaugeVar2,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size,
										ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem6);
									DashboardItem dashboardItem7 = new DashboardItem
									{
										ItemType = DashboardItemTypes.GaugeVar3,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size,
										ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 2.0
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem7);
									DashboardItem dashboardItem8 = new DashboardItem
									{
										ItemType = DashboardItemTypes.LinearGauge,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size,
										LinearOrientationHorizontal = true,
										ShowValue = false
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem8);
									DashboardItem dashboardItem9 = new DashboardItem
									{
										ItemType = DashboardItemTypes.LinearGauge,
										PID_Id = CS$<>8__locals1.pid.Id,
										Minimum = CS$<>8__locals1.pid.Minimum,
										Maximum = CS$<>8__locals1.pid.Maximum,
										CustomName = CS$<>8__locals1.pid.ShortName,
										DesiredHeight = CS$<>8__locals1.size,
										DesiredWidth = CS$<>8__locals1.size,
										WidthRequest = CS$<>8__locals1.size,
										HeightRequest = CS$<>8__locals1.size,
										LinearOrientationHorizontal = false,
										ShowValue = false
									};
									CS$<>8__locals1.<>4__this.items.Add(dashboardItem9);
									switch (SharedSettings.Current.DashboardTheme)
									{
									case 0:
									case 2:
										dashboardItem8.GaugePointerColor = Color.Orange;
										dashboardItem9.GaugePointerColor = Color.Orange;
										dashboardItem6.GaugePointerColor = Color.Orange;
										dashboardItem7.GaugePointerColor = Color.Orange;
										break;
									case 1:
										dashboardItem8.GaugePointerColor = Color.Red;
										dashboardItem9.GaugePointerColor = Color.Red;
										dashboardItem6.GaugePointerColor = Color.Red;
										dashboardItem7.GaugePointerColor = Color.Red;
										break;
									}
								}
							}
							CS$<>8__locals2.model = new LiveDataPIDModel();
							taskAwaiter = Task.WhenAll(CS$<>8__locals1.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemTypeSelector.<>c__DisplayClass2_2
							{
								CS$<>8__locals2 = CS$<>8__locals2,
								item = item
							}.<Init>b__3)))).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemTypeSelector.<>c__DisplayClass2_0.<<Init>b__0>d>(ref taskAwaiter, ref this);
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

				// Token: 0x06003AEB RID: 15083 RVA: 0x00310C5C File Offset: 0x0030EE5C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040023F8 RID: 9208
				public int <>1__state;

				// Token: 0x040023F9 RID: 9209
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040023FA RID: 9210
				public DashboardItemTypeSelector.<>c__DisplayClass2_0 <>4__this;

				// Token: 0x040023FB RID: 9211
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x020006B8 RID: 1720
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x06003AEC RID: 15084 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x06003AED RID: 15085 RVA: 0x00310C6A File Offset: 0x0030EE6A
			internal Task <Init>b__2(DashboardItem item)
			{
				return Task.Run(new Action(new DashboardItemTypeSelector.<>c__DisplayClass2_2
				{
					CS$<>8__locals2 = this,
					item = item
				}.<Init>b__3));
			}

			// Token: 0x040023FC RID: 9212
			public LiveDataPIDModel model;

			// Token: 0x040023FD RID: 9213
			public DashboardItemTypeSelector.<>c__DisplayClass2_0 CS$<>8__locals1;
		}

		// Token: 0x020006B9 RID: 1721
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_2
		{
			// Token: 0x06003AEE RID: 15086 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_2()
			{
			}

			// Token: 0x06003AEF RID: 15087 RVA: 0x00310C90 File Offset: 0x0030EE90
			internal void <Init>b__3()
			{
				this.item.SelectAndAddControl();
				this.item.Model = this.CS$<>8__locals2.model;
				this.item.Start();
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Tap_Tapped;
				this.item.GestureRecognizers.Add(tapGestureRecognizer);
			}

			// Token: 0x040023FE RID: 9214
			public DashboardItem item;

			// Token: 0x040023FF RID: 9215
			public DashboardItemTypeSelector.<>c__DisplayClass2_1 CS$<>8__locals2;
		}

		// Token: 0x020006BA RID: 1722
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Init>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003AF0 RID: 15088 RVA: 0x00310D04 File Offset: 0x0030EF04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemTypeSelector dashboardItemTypeSelector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new DashboardItemTypeSelector.<>c__DisplayClass2_0();
						CS$<>8__locals1.pid = pid;
						CS$<>8__locals1.<>4__this = this;
						double num3 = ((DeviceDisplay.MainDisplayInfo.Width > DeviceDisplay.MainDisplayInfo.Height) ? DeviceDisplay.MainDisplayInfo.Height : DeviceDisplay.MainDisplayInfo.Width);
						num3 /= DeviceDisplay.MainDisplayInfo.Density;
						CS$<>8__locals1.size = Math.Round(num3 / 2.5, 0);
						taskAwaiter = Task.Run(delegate
						{
							DashboardItemTypeSelector.<>c__DisplayClass2_0.<<Init>b__0>d <<Init>b__0>d;
							<<Init>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Init>b__0>d.<>4__this = CS$<>8__locals1;
							<<Init>b__0>d.<>1__state = -1;
							<<Init>b__0>d.<>t__builder.Start<DashboardItemTypeSelector.<>c__DisplayClass2_0.<<Init>b__0>d>(ref <<Init>b__0>d);
							return <<Init>b__0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemTypeSelector.<Init>d__2>(ref taskAwaiter, ref this);
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
					MainThread.BeginInvokeOnMainThread(delegate
					{
						foreach (DashboardItem dashboardItem in CS$<>8__locals1.<>4__this.items)
						{
							Frame frame = new Frame
							{
								BorderColor = (Color)Application.Current.Resources["BackgroundColor"],
								WidthRequest = CS$<>8__locals1.size,
								HeightRequest = CS$<>8__locals1.size,
								Padding = new Thickness(3.0)
							};
							TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
							tapGestureRecognizer.Tapped += CS$<>8__locals1.<>4__this.Tap_Tapped;
							frame.GestureRecognizers.Add(tapGestureRecognizer);
							frame.Content = dashboardItem;
							CS$<>8__locals1.<>4__this.uniGrid.Children.Add(frame);
						}
						CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
					});
					if (!App.OBDSimulator.IsActive)
					{
						List<OBDRequest> list = new List<OBDRequest>(4);
						dashboardItemTypeSelector.items.FirstOrDefault<DashboardItem>().Model.GetRequests(list, null, "");
						App.OBDReader.ReplaceQueue(list);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003AF1 RID: 15089 RVA: 0x00310ED0 File Offset: 0x0030F0D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002400 RID: 9216
			public int <>1__state;

			// Token: 0x04002401 RID: 9217
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002402 RID: 9218
			public IPID pid;

			// Token: 0x04002403 RID: 9219
			public DashboardItemTypeSelector <>4__this;

			// Token: 0x04002404 RID: 9220
			private DashboardItemTypeSelector.<>c__DisplayClass2_0 <>8__1;

			// Token: 0x04002405 RID: 9221
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006BB RID: 1723
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectDashboardItemType>d__8 : IAsyncStateMachine
		{
			// Token: 0x06003AF2 RID: 15090 RVA: 0x00310EE0 File Offset: 0x0030F0E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemTypes? selectedItemType;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00E7;
						}
						selector = new DashboardItemTypeSelector(pid);
						taskAwaiter = App.GetCurrentPage().Navigation.PushAsync(selector).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemTypeSelector.<SelectDashboardItemType>d__8>(ref taskAwaiter, ref this);
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
					taskAwaiter = selector.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemTypeSelector.<SelectDashboardItemType>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E7:
					taskAwaiter.GetResult();
					selectedItemType = selector.SelectedItemType;
				}
				catch (Exception ex)
				{
					num2 = -2;
					selector = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				selector = null;
				this.<>t__builder.SetResult(selectedItemType);
			}

			// Token: 0x06003AF3 RID: 15091 RVA: 0x00311034 File Offset: 0x0030F234
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002406 RID: 9222
			public int <>1__state;

			// Token: 0x04002407 RID: 9223
			public AsyncTaskMethodBuilder<DashboardItemTypes?> <>t__builder;

			// Token: 0x04002408 RID: 9224
			public IPID pid;

			// Token: 0x04002409 RID: 9225
			private DashboardItemTypeSelector <selector>5__2;

			// Token: 0x0400240A RID: 9226
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006BC RID: 1724
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Tap_Tapped>d__5 : IAsyncStateMachine
		{
			// Token: 0x06003AF4 RID: 15092 RVA: 0x00311044 File Offset: 0x0030F244
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemTypeSelector dashboardItemTypeSelector = this;
				try
				{
					if (num != 0)
					{
						dashboardItemTypeSelector.IsEnabled = false;
						Frame frame;
						DashboardItem dashboardItem;
						if (sender is Frame)
						{
							frame = (Frame)sender;
							dashboardItem = (DashboardItem)frame.Content;
						}
						else
						{
							dashboardItem = (DashboardItem)sender;
							frame = (Frame)dashboardItem.Parent;
						}
						dashboardItemTypeSelector.itemTypeSelector.IsEnabled = false;
						if (SharedSettings.Current.DashboardAnimation)
						{
							(frame.Parent as Layout).RaiseChild(frame);
							ViewExtensions.ScaleXTo(frame, 1.2, 250U, null);
						}
						frame.Content = null;
						dashboardItemTypeSelector.SelectedItemType = new DashboardItemTypes?(dashboardItem.ItemType);
						try
						{
							List<DashboardItem>.Enumerator enumerator = dashboardItemTypeSelector.items.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardItem dashboardItem2 = enumerator.Current;
									if (dashboardItem2.GestureRecognizers.Count > 0)
									{
										TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem2.GestureRecognizers[dashboardItem2.GestureRecognizers.Count - 1];
										tapGestureRecognizer.Tapped -= dashboardItemTypeSelector.Tap_Tapped;
										dashboardItem2.GestureRecognizers.Remove(tapGestureRecognizer);
									}
									dashboardItem2.Stop();
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
						}
						catch (Exception)
						{
						}
						try
						{
							IEnumerator<View> enumerator2 = dashboardItemTypeSelector.uniGrid.Children.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									View view = enumerator2.Current;
									Frame frame2 = (Frame)view;
									if (frame2.GestureRecognizers.Count > 0)
									{
										((TapGestureRecognizer)frame2.GestureRecognizers[0]).Tapped -= dashboardItemTypeSelector.Tap_Tapped;
									}
									frame2.GestureRecognizers.Clear();
								}
							}
							finally
							{
								if (num < 0 && enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						catch (Exception)
						{
						}
						dashboardItemTypeSelector.IsEnabled = true;
					}
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = dashboardItemTypeSelector.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemTypeSelector.<Tap_Tapped>d__5>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
					try
					{
						dashboardItemTypeSelector.semaphore.Release();
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

			// Token: 0x06003AF5 RID: 15093 RVA: 0x00311378 File Offset: 0x0030F578
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400240B RID: 9227
			public int <>1__state;

			// Token: 0x0400240C RID: 9228
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400240D RID: 9229
			public DashboardItemTypeSelector <>4__this;

			// Token: 0x0400240E RID: 9230
			public object sender;

			// Token: 0x0400240F RID: 9231
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x020006BD RID: 1725
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06003AF6 RID: 15094 RVA: 0x00311388 File Offset: 0x0030F588
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemTypeSelector dashboardItemTypeSelector = this;
				try
				{
					if (num != 0)
					{
						dashboardItemTypeSelector.IsEnabled = false;
						try
						{
							List<DashboardItem>.Enumerator enumerator = dashboardItemTypeSelector.items.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardItem dashboardItem = enumerator.Current;
									if (dashboardItem != null)
									{
										dashboardItem.Stop();
										TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[0];
										tapGestureRecognizer.Tapped -= dashboardItemTypeSelector.Tap_Tapped;
										dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
										Frame frame = (Frame)dashboardItem.Parent;
										tapGestureRecognizer = (TapGestureRecognizer)frame.GestureRecognizers[0];
										tapGestureRecognizer.Tapped -= dashboardItemTypeSelector.Tap_Tapped;
										frame.GestureRecognizers.Remove(tapGestureRecognizer);
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							dashboardItemTypeSelector.items.Clear();
							dashboardItemTypeSelector.uniGrid.Children.Clear();
							dashboardItemTypeSelector.SelectedItemType = null;
						}
						catch (Exception)
						{
						}
					}
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = dashboardItemTypeSelector.Navigation.PopAsync(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemTypeSelector.<btnCancel_Clicked>d__7>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
					try
					{
						dashboardItemTypeSelector.semaphore.Release();
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

			// Token: 0x06003AF7 RID: 15095 RVA: 0x003115B8 File Offset: 0x0030F7B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002410 RID: 9232
			public int <>1__state;

			// Token: 0x04002411 RID: 9233
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002412 RID: 9234
			public DashboardItemTypeSelector <>4__this;

			// Token: 0x04002413 RID: 9235
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
