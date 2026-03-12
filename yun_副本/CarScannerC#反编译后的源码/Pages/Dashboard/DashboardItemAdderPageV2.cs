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
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.ListView.XForms;
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
	// Token: 0x02000694 RID: 1684
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml")]
	public class DashboardItemAdderPageV2 : ContentPage
	{
		// Token: 0x06003959 RID: 14681 RVA: 0x002D7984 File Offset: 0x002D5B84
		public DashboardItemAdderPageV2(DashboardPage page, Point touchPoint)
		{
			this.InitializeComponent();
			this.Page = page;
			this.TouchPoint = touchPoint;
			this.lvPids.ItemsSource = this.FilteredPids;
			this.searchBar.BindingContext = this;
			this.UpdateFilter();
		}

		// Token: 0x17001395 RID: 5013
		// (get) Token: 0x0600395A RID: 14682 RVA: 0x002D79FA File Offset: 0x002D5BFA
		public SmartCollection<PID> FilteredPids
		{
			get
			{
				return this._FilteredPids;
			}
		}

		// Token: 0x17001396 RID: 5014
		// (get) Token: 0x0600395B RID: 14683 RVA: 0x002D7A02 File Offset: 0x002D5C02
		// (set) Token: 0x0600395C RID: 14684 RVA: 0x002D7A0A File Offset: 0x002D5C0A
		public string Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				this._Filter = value;
				this.OnPropertyChanged("Filter");
				this.UpdateFilter();
			}
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x002D7A24 File Offset: 0x002D5C24
		private void btnSort_Clicked(object sender, EventArgs e)
		{
			if (SharedSettings.Current.PIDSortingMode == SharedSettings.PIDSortingModes.Id)
			{
				SharedSettings.Current.PIDSortingMode = SharedSettings.PIDSortingModes.NameAsc;
			}
			else if (SharedSettings.Current.PIDSortingMode == SharedSettings.PIDSortingModes.NameAsc)
			{
				SharedSettings.Current.PIDSortingMode = SharedSettings.PIDSortingModes.NameDesc;
			}
			else if (SharedSettings.Current.PIDSortingMode == SharedSettings.PIDSortingModes.NameDesc)
			{
				SharedSettings.Current.PIDSortingMode = SharedSettings.PIDSortingModes.Id;
			}
			this.UpdateFilter();
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x002D7A84 File Offset: 0x002D5C84
		private IEnumerable<PID> Sort(IEnumerable<PID> collection)
		{
			switch (SharedSettings.Current.PIDSortingMode)
			{
			case SharedSettings.PIDSortingModes.NameAsc:
				return collection.OrderBy((PID x) => x.Name);
			case SharedSettings.PIDSortingModes.NameDesc:
				return collection.OrderByDescending((PID x) => x.Name);
			}
			return collection.OrderBy((PID x) => x.Id);
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x002D7B24 File Offset: 0x002D5D24
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				IEnumerable<PID> enumerable = this.Sort(LiveDataPIDModel._PIDCollection);
				enumerable = enumerable.Where((PID x) => x != null);
				this.FilteredPids.Reset(enumerable);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<PID> enumerable2 = LiveDataPIDModel._PIDCollection;
			enumerable2 = enumerable2.Where((PID x) => x != null);
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable2 = enumerable2.Where((PID x) => (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			IEnumerable<PID> enumerable3 = this.Sort(enumerable2);
			this.FilteredPids.Reset(enumerable3);
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x002D7C20 File Offset: 0x002D5E20
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filtertext = this.searchBar.Text;
			await Task.Delay(500);
			if (this.filtertext == this.searchBar.Text)
			{
				try
				{
					this.Filter = this.filtertext;
					if (this.lvPids.SelectedItem != null)
					{
						this.lvPids.ScrollTo(this.lvPids.SelectedItem, 1, true);
					}
					else
					{
						this.lvPids.ScrollTo(this.FilteredPids.FirstOrDefault<PID>(), 0, true);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x002D7C58 File Offset: 0x002D5E58
		private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			DashboardItemAdderPageV2.<>c__DisplayClass16_0 CS$<>8__locals1 = new DashboardItemAdderPageV2.<>c__DisplayClass16_0();
			CS$<>8__locals1.<>4__this = this;
			this.activityFrame.IsVisible = true;
			CS$<>8__locals1.pid = e.ItemData as PID;
			double num = ((base.Width > base.Height) ? base.Height : base.Width);
			CS$<>8__locals1.size = Math.Round(num / 2.5, 0);
			this.itemTypeSelector.IsVisible = true;
			this.gridSelector.IsVisible = false;
			this.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_DisplayType");
			this.sortButton.IsVisible = false;
			await Task.Run(delegate
			{
				DashboardItemAdderPageV2.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d <<Handle_ItemTapped>b__0>d;
				<<Handle_ItemTapped>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Handle_ItemTapped>b__0>d.<>4__this = CS$<>8__locals1;
				<<Handle_ItemTapped>b__0>d.<>1__state = -1;
				<<Handle_ItemTapped>b__0>d.<>t__builder.Start<DashboardItemAdderPageV2.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref <<Handle_ItemTapped>b__0>d);
				return <<Handle_ItemTapped>b__0>d.<>t__builder.Task;
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
			if (CS$<>8__locals1.pid is CustomPID && (CS$<>8__locals1.pid as CustomPID).IsAction)
			{
				this.Tap_Tapped(this.items.FirstOrDefault<DashboardItem>(), EventArgs.Empty);
			}
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x002D7C98 File Offset: 0x002D5E98
		private async void Tap_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			Frame f = null;
			DashboardItem item = null;
			if (sender is Frame)
			{
				f = (Frame)sender;
				item = (DashboardItem)f.Content;
			}
			else
			{
				item = (DashboardItem)sender;
				f = (Frame)item.Parent;
			}
			this.itemTypeSelector.IsEnabled = false;
			Dash_CustomPage custPage = (Dash_CustomPage)this.Page;
			if (SharedSettings.Current.DashboardAnimation)
			{
				(f.Parent as Layout).RaiseChild(f);
				await ViewExtensions.ScaleXTo(f, 1.2, 250U, null);
			}
			await base.Navigation.PopAsync();
			f.Content = null;
			item.PositionX = this.TouchPoint.X;
			item.PositionY = this.TouchPoint.Y;
			custPage.AddNewItem(item);
			foreach (DashboardItem dashboardItem in this.items)
			{
				TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[dashboardItem.GestureRecognizers.Count - 1];
				tapGestureRecognizer.Tapped -= this.Tap_Tapped;
				dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
				dashboardItem.Stop();
			}
			foreach (View view in this.uniGrid.Children)
			{
				Frame frame = (Frame)view;
				((TapGestureRecognizer)frame.GestureRecognizers[0]).Tapped -= this.Tap_Tapped;
				frame.GestureRecognizers.Clear();
			}
			DashboardListViewModel.Current.SaveDashboardToSettings();
			try
			{
				DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Stop();
				DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Start();
			}
			catch (Exception)
			{
			}
			base.IsEnabled = true;
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x002D7CD7 File Offset: 0x002D5ED7
		protected override bool OnBackButtonPressed()
		{
			this.btnCancel_Clicked(this, null);
			return true;
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x002D7CE4 File Offset: 0x002D5EE4
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			(sender as VisualElement).IsEnabled = false;
			if (this.gridSelector.IsVisible)
			{
				await base.Navigation.PopAsync(true);
			}
			else
			{
				this.gridSelector.IsVisible = true;
				this.itemTypeSelector.IsVisible = false;
				this.sortButton.IsVisible = true;
				this.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_Sensor");
				foreach (DashboardItem dashboardItem in this.items)
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
				this.items.Clear();
				this.uniGrid.Children.Clear();
			}
			(sender as VisualElement).IsEnabled = true;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x002D7D24 File Offset: 0x002D5F24
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardItemAdderPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			UniformGrid uniformGrid;
			VisualDiagnostics.RegisterSourceInfo(uniformGrid = new UniformGrid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("sortButton", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "sortButton";
			}
			nameScope.RegisterName("LayoutRoot", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("gridSelector", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridSelector";
			}
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("lvPids", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lvPids";
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
			this.sortButton = linkButton2;
			this.LayoutRoot = grid3;
			this.gridSelector = grid2;
			this.searchBar = entry;
			this.lvPids = sfListView;
			this.itemTypeSelector = scrollView;
			this.uniGrid = uniformGrid;
			this.activityFrame = activityFrame;
			this.SetValue(Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.UseSafeAreaProperty, true);
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
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 5)));
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
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
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
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
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
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_DashboardItemEditor_Sensor";
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
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnSort_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension4.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton2;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			translate3.Text = "ios_Sort";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardItemAdderPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.Text = obj10;
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(Grid.RowProperty, 0);
			grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto, *"));
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(Grid.ColumnProperty, 0);
			entry.SetValue(Grid.ColumnSpanProperty, 2);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid2.Children.Add(entry);
			sfListView.SetValue(Grid.RowProperty, 1);
			sfListView.SetValue(Grid.ColumnProperty, 0);
			sfListView.SetValue(Grid.ColumnSpanProperty, 2);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Handle_ItemTapped);
			IDataTemplate dataTemplate2 = dataTemplate;
			DashboardItemAdderPageV2.<InitializeComponent>_anonXamlCDataTemplate_38 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardItemAdderPageV2.<InitializeComponent>_anonXamlCDataTemplate_38();
			object[] array8 = new object[0 + 5];
			array8[0] = dataTemplate;
			array8[1] = sfListView;
			array8[2] = grid2;
			array8[3] = grid3;
			array8[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array8;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(sfListView);
			grid3.Children.Add(grid2);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			scrollView.Content = uniformGrid;
			grid3.Children.Add(scrollView);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid3.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x002D9268 File Offset: 0x002D7468
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardItemAdderPageV2>(this, typeof(DashboardItemAdderPageV2));
			this.thisPage = NameScopeExtensions.FindByName<ContentPage>(this, "thisPage");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.lbNavbarTitle = NameScopeExtensions.FindByName<NonScalableLabel>(this, "lbNavbarTitle");
			this.sortButton = NameScopeExtensions.FindByName<LinkButton>(this, "sortButton");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.gridSelector = NameScopeExtensions.FindByName<Grid>(this, "gridSelector");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lvPids = NameScopeExtensions.FindByName<SfListView>(this, "lvPids");
			this.itemTypeSelector = NameScopeExtensions.FindByName<ScrollView>(this, "itemTypeSelector");
			this.uniGrid = NameScopeExtensions.FindByName<UniformGrid>(this, "uniGrid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002331 RID: 9009
		private DashboardPage Page;

		// Token: 0x04002332 RID: 9010
		private Point TouchPoint;

		// Token: 0x04002333 RID: 9011
		private SmartCollection<PID> _FilteredPids = new SmartCollection<PID>();

		// Token: 0x04002334 RID: 9012
		private string _Filter = "";

		// Token: 0x04002335 RID: 9013
		private string filtertext = "";

		// Token: 0x04002336 RID: 9014
		private List<DashboardItem> items = new List<DashboardItem>();

		// Token: 0x04002337 RID: 9015
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage thisPage;

		// Token: 0x04002338 RID: 9016
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04002339 RID: 9017
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel lbNavbarTitle;

		// Token: 0x0400233A RID: 9018
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton sortButton;

		// Token: 0x0400233B RID: 9019
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x0400233C RID: 9020
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridSelector;

		// Token: 0x0400233D RID: 9021
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x0400233E RID: 9022
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvPids;

		// Token: 0x0400233F RID: 9023
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView itemTypeSelector;

		// Token: 0x04002340 RID: 9024
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private UniformGrid uniGrid;

		// Token: 0x04002341 RID: 9025
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000695 RID: 1685
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003967 RID: 14695 RVA: 0x002D9341 File Offset: 0x002D7541
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003968 RID: 14696 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003969 RID: 14697 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__11_0(PID x)
			{
				return x.Name;
			}

			// Token: 0x0600396A RID: 14698 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__11_1(PID x)
			{
				return x.Name;
			}

			// Token: 0x0600396B RID: 14699 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <Sort>b__11_2(PID x)
			{
				return x.Id;
			}

			// Token: 0x0600396C RID: 14700 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <UpdateFilter>b__12_0(PID x)
			{
				return x != null;
			}

			// Token: 0x0600396D RID: 14701 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <UpdateFilter>b__12_1(PID x)
			{
				return x != null;
			}

			// Token: 0x04002342 RID: 9026
			public static readonly DashboardItemAdderPageV2.<>c <>9 = new DashboardItemAdderPageV2.<>c();

			// Token: 0x04002343 RID: 9027
			public static Func<PID, string> <>9__11_0;

			// Token: 0x04002344 RID: 9028
			public static Func<PID, string> <>9__11_1;

			// Token: 0x04002345 RID: 9029
			public static Func<PID, int> <>9__11_2;

			// Token: 0x04002346 RID: 9030
			public static Func<PID, bool> <>9__12_0;

			// Token: 0x04002347 RID: 9031
			public static Func<PID, bool> <>9__12_1;
		}

		// Token: 0x02000696 RID: 1686
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600396E RID: 14702 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x0600396F RID: 14703 RVA: 0x002D9350 File Offset: 0x002D7550
			internal bool <UpdateFilter>b__2(PID x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002348 RID: 9032
			public string word;
		}

		// Token: 0x02000697 RID: 1687
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06003970 RID: 14704 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06003971 RID: 14705 RVA: 0x002D93A8 File Offset: 0x002D75A8
			internal async Task <Handle_ItemTapped>b__0()
			{
				DashboardItemAdderPageV2.<>c__DisplayClass16_1 CS$<>8__locals1 = new DashboardItemAdderPageV2.<>c__DisplayClass16_1();
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
				await Task.WhenAll(this.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemAdderPageV2.<>c__DisplayClass16_2
				{
					CS$<>8__locals2 = CS$<>8__locals1,
					item = item
				}.<Handle_ItemTapped>b__3))));
			}

			// Token: 0x06003972 RID: 14706 RVA: 0x002D93EC File Offset: 0x002D75EC
			internal void <Handle_ItemTapped>b__1()
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

			// Token: 0x04002349 RID: 9033
			public PID pid;

			// Token: 0x0400234A RID: 9034
			public double size;

			// Token: 0x0400234B RID: 9035
			public DashboardItemAdderPageV2 <>4__this;

			// Token: 0x02000698 RID: 1688
			[StructLayout(LayoutKind.Auto)]
			private struct <<Handle_ItemTapped>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06003973 RID: 14707 RVA: 0x002D94EC File Offset: 0x002D76EC
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					DashboardItemAdderPageV2.<>c__DisplayClass16_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							DashboardItemAdderPageV2.<>c__DisplayClass16_1 CS$<>8__locals2 = new DashboardItemAdderPageV2.<>c__DisplayClass16_1();
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
							taskAwaiter = Task.WhenAll(CS$<>8__locals1.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemAdderPageV2.<>c__DisplayClass16_2
							{
								CS$<>8__locals2 = CS$<>8__locals2,
								item = item
							}.<Handle_ItemTapped>b__3)))).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPageV2.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref taskAwaiter, ref this);
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

				// Token: 0x06003974 RID: 14708 RVA: 0x002D9CB0 File Offset: 0x002D7EB0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400234C RID: 9036
				public int <>1__state;

				// Token: 0x0400234D RID: 9037
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x0400234E RID: 9038
				public DashboardItemAdderPageV2.<>c__DisplayClass16_0 <>4__this;

				// Token: 0x0400234F RID: 9039
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x02000699 RID: 1689
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_1
		{
			// Token: 0x06003975 RID: 14709 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_1()
			{
			}

			// Token: 0x06003976 RID: 14710 RVA: 0x002D9CBE File Offset: 0x002D7EBE
			internal Task <Handle_ItemTapped>b__2(DashboardItem item)
			{
				return Task.Run(new Action(new DashboardItemAdderPageV2.<>c__DisplayClass16_2
				{
					CS$<>8__locals2 = this,
					item = item
				}.<Handle_ItemTapped>b__3));
			}

			// Token: 0x04002350 RID: 9040
			public LiveDataPIDModel model;

			// Token: 0x04002351 RID: 9041
			public DashboardItemAdderPageV2.<>c__DisplayClass16_0 CS$<>8__locals1;
		}

		// Token: 0x0200069A RID: 1690
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_2
		{
			// Token: 0x06003977 RID: 14711 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_2()
			{
			}

			// Token: 0x06003978 RID: 14712 RVA: 0x002D9CE4 File Offset: 0x002D7EE4
			internal void <Handle_ItemTapped>b__3()
			{
				this.item.SelectAndAddControl();
				this.item.Model = this.CS$<>8__locals2.model;
				this.item.Start();
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Tap_Tapped;
				this.item.GestureRecognizers.Add(tapGestureRecognizer);
			}

			// Token: 0x04002352 RID: 9042
			public DashboardItem item;

			// Token: 0x04002353 RID: 9043
			public DashboardItemAdderPageV2.<>c__DisplayClass16_1 CS$<>8__locals2;
		}

		// Token: 0x0200069B RID: 1691
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ItemTapped>d__16 : IAsyncStateMachine
		{
			// Token: 0x06003979 RID: 14713 RVA: 0x002D9D58 File Offset: 0x002D7F58
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPageV2 dashboardItemAdderPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new DashboardItemAdderPageV2.<>c__DisplayClass16_0();
						CS$<>8__locals1.<>4__this = this;
						dashboardItemAdderPageV.activityFrame.IsVisible = true;
						CS$<>8__locals1.pid = e.ItemData as PID;
						double num3 = ((dashboardItemAdderPageV.Width > dashboardItemAdderPageV.Height) ? dashboardItemAdderPageV.Height : dashboardItemAdderPageV.Width);
						CS$<>8__locals1.size = Math.Round(num3 / 2.5, 0);
						dashboardItemAdderPageV.itemTypeSelector.IsVisible = true;
						dashboardItemAdderPageV.gridSelector.IsVisible = false;
						dashboardItemAdderPageV.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_DisplayType");
						dashboardItemAdderPageV.sortButton.IsVisible = false;
						taskAwaiter = Task.Run(delegate
						{
							DashboardItemAdderPageV2.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d <<Handle_ItemTapped>b__0>d;
							<<Handle_ItemTapped>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Handle_ItemTapped>b__0>d.<>4__this = CS$<>8__locals1;
							<<Handle_ItemTapped>b__0>d.<>1__state = -1;
							<<Handle_ItemTapped>b__0>d.<>t__builder.Start<DashboardItemAdderPageV2.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref <<Handle_ItemTapped>b__0>d);
							return <<Handle_ItemTapped>b__0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPageV2.<Handle_ItemTapped>d__16>(ref taskAwaiter, ref this);
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
						dashboardItemAdderPageV.items.FirstOrDefault<DashboardItem>().Model.GetRequests(list, null, "");
						App.OBDReader.ReplaceQueue(list);
					}
					if (CS$<>8__locals1.pid is CustomPID && (CS$<>8__locals1.pid as CustomPID).IsAction)
					{
						dashboardItemAdderPageV.Tap_Tapped(dashboardItemAdderPageV.items.FirstOrDefault<DashboardItem>(), EventArgs.Empty);
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

			// Token: 0x0600397A RID: 14714 RVA: 0x002D9F84 File Offset: 0x002D8184
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002354 RID: 9044
			public int <>1__state;

			// Token: 0x04002355 RID: 9045
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002356 RID: 9046
			public DashboardItemAdderPageV2 <>4__this;

			// Token: 0x04002357 RID: 9047
			public ItemTappedEventArgs e;

			// Token: 0x04002358 RID: 9048
			private DashboardItemAdderPageV2.<>c__DisplayClass16_0 <>8__1;

			// Token: 0x04002359 RID: 9049
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200069C RID: 1692
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Tap_Tapped>d__17 : IAsyncStateMachine
		{
			// Token: 0x0600397B RID: 14715 RVA: 0x002D9F94 File Offset: 0x002D8194
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPageV2 dashboardItemAdderPageV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
							num = (num2 = -1);
							goto IL_019D;
						}
						dashboardItemAdderPageV.IsEnabled = false;
						f = null;
						item = null;
						if (sender is Frame)
						{
							f = (Frame)sender;
							item = (DashboardItem)f.Content;
						}
						else
						{
							item = (DashboardItem)sender;
							f = (Frame)item.Parent;
						}
						dashboardItemAdderPageV.itemTypeSelector.IsEnabled = false;
						custPage = (Dash_CustomPage)dashboardItemAdderPageV.Page;
						if (!SharedSettings.Current.DashboardAnimation)
						{
							goto IL_0144;
						}
						(f.Parent as Layout).RaiseChild(f);
						taskAwaiter3 = ViewExtensions.ScaleXTo(f, 1.2, 250U, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardItemAdderPageV2.<Tap_Tapped>d__17>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
					}
					taskAwaiter3.GetResult();
					IL_0144:
					taskAwaiter = dashboardItemAdderPageV.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemAdderPageV2.<Tap_Tapped>d__17>(ref taskAwaiter, ref this);
						return;
					}
					IL_019D:
					taskAwaiter.GetResult();
					f.Content = null;
					item.PositionX = dashboardItemAdderPageV.TouchPoint.X;
					item.PositionY = dashboardItemAdderPageV.TouchPoint.Y;
					custPage.AddNewItem(item);
					List<DashboardItem>.Enumerator enumerator = dashboardItemAdderPageV.items.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DashboardItem dashboardItem = enumerator.Current;
							TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[dashboardItem.GestureRecognizers.Count - 1];
							tapGestureRecognizer.Tapped -= dashboardItemAdderPageV.Tap_Tapped;
							dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
							dashboardItem.Stop();
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					IEnumerator<View> enumerator2 = dashboardItemAdderPageV.uniGrid.Children.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							View view = enumerator2.Current;
							Frame frame = (Frame)view;
							((TapGestureRecognizer)frame.GestureRecognizers[0]).Tapped -= dashboardItemAdderPageV.Tap_Tapped;
							frame.GestureRecognizers.Clear();
						}
					}
					finally
					{
						if (num < 0 && enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					DashboardListViewModel.Current.SaveDashboardToSettings();
					try
					{
						DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Stop();
						DashboardXamlPage.Instance.Pages[DashboardXamlPage.Instance.CurrentPage].Start();
					}
					catch (Exception)
					{
					}
					dashboardItemAdderPageV.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					f = null;
					item = null;
					custPage = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				f = null;
				item = null;
				custPage = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600397C RID: 14716 RVA: 0x002DA384 File Offset: 0x002D8584
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400235A RID: 9050
			public int <>1__state;

			// Token: 0x0400235B RID: 9051
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400235C RID: 9052
			public DashboardItemAdderPageV2 <>4__this;

			// Token: 0x0400235D RID: 9053
			public object sender;

			// Token: 0x0400235E RID: 9054
			private Frame <f>5__2;

			// Token: 0x0400235F RID: 9055
			private DashboardItem <item>5__3;

			// Token: 0x04002360 RID: 9056
			private Dash_CustomPage <custPage>5__4;

			// Token: 0x04002361 RID: 9057
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002362 RID: 9058
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x0200069D RID: 1693
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__19 : IAsyncStateMachine
		{
			// Token: 0x0600397D RID: 14717 RVA: 0x002DA394 File Offset: 0x002D8594
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPageV2 dashboardItemAdderPageV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						(sender as VisualElement).IsEnabled = false;
						if (!dashboardItemAdderPageV.gridSelector.IsVisible)
						{
							dashboardItemAdderPageV.gridSelector.IsVisible = true;
							dashboardItemAdderPageV.itemTypeSelector.IsVisible = false;
							dashboardItemAdderPageV.sortButton.IsVisible = true;
							dashboardItemAdderPageV.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_Sensor");
							List<DashboardItem>.Enumerator enumerator = dashboardItemAdderPageV.items.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardItem dashboardItem = enumerator.Current;
									dashboardItem.Stop();
									TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[0];
									tapGestureRecognizer.Tapped -= dashboardItemAdderPageV.Tap_Tapped;
									dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
									Frame frame = (Frame)dashboardItem.Parent;
									tapGestureRecognizer = (TapGestureRecognizer)frame.GestureRecognizers[0];
									tapGestureRecognizer.Tapped -= dashboardItemAdderPageV.Tap_Tapped;
									frame.GestureRecognizers.Remove(tapGestureRecognizer);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							dashboardItemAdderPageV.items.Clear();
							dashboardItemAdderPageV.uniGrid.Children.Clear();
							goto IL_0196;
						}
						taskAwaiter = dashboardItemAdderPageV.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemAdderPageV2.<btnCancel_Clicked>d__19>(ref taskAwaiter, ref this);
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
					IL_0196:
					(sender as VisualElement).IsEnabled = true;
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

			// Token: 0x0600397E RID: 14718 RVA: 0x002DA5AC File Offset: 0x002D87AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002363 RID: 9059
			public int <>1__state;

			// Token: 0x04002364 RID: 9060
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002365 RID: 9061
			public object sender;

			// Token: 0x04002366 RID: 9062
			public DashboardItemAdderPageV2 <>4__this;

			// Token: 0x04002367 RID: 9063
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x0200069E RID: 1694
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__14 : IAsyncStateMachine
		{
			// Token: 0x0600397F RID: 14719 RVA: 0x002DA5BC File Offset: 0x002D87BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPageV2 dashboardItemAdderPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardItemAdderPageV.filtertext = dashboardItemAdderPageV.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPageV2.<searchBar_TextChanged>d__14>(ref taskAwaiter, ref this);
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
					if (dashboardItemAdderPageV.filtertext == dashboardItemAdderPageV.searchBar.Text)
					{
						try
						{
							dashboardItemAdderPageV.Filter = dashboardItemAdderPageV.filtertext;
							if (dashboardItemAdderPageV.lvPids.SelectedItem != null)
							{
								dashboardItemAdderPageV.lvPids.ScrollTo(dashboardItemAdderPageV.lvPids.SelectedItem, 1, true);
							}
							else
							{
								dashboardItemAdderPageV.lvPids.ScrollTo(dashboardItemAdderPageV.FilteredPids.FirstOrDefault<PID>(), 0, true);
							}
						}
						catch
						{
						}
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

			// Token: 0x06003980 RID: 14720 RVA: 0x002DA6FC File Offset: 0x002D88FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002368 RID: 9064
			public int <>1__state;

			// Token: 0x04002369 RID: 9065
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400236A RID: 9066
			public DashboardItemAdderPageV2 <>4__this;

			// Token: 0x0400236B RID: 9067
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200069F RID: 1695
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_38
		{
			// Token: 0x06003981 RID: 14721 RVA: 0x002DA70C File Offset: 0x002D890C
			public <InitializeComponent>_anonXamlCDataTemplate_38()
			{
			}

			// Token: 0x06003982 RID: 14722 RVA: 0x002DA720 File Offset: 0x002D8920
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 42);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 41);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 41);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardItemAdderPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize++";
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
				xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemAdderPageV2.<InitializeComponent>_anonXamlCDataTemplate_38).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Name";
				bindingExtension.TypedBinding = new TypedBinding<PID, string>(delegate(PID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(PID A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Name = A_1;
						return;
					}
				}, new Tuple<Func<PID, object>, string>[]
				{
					new Tuple<Func<PID, object>, string>((PID A_0) => A_0, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				frame.SetValue(Grid.RowProperty, 1);
				frame.SetValue(Grid.ColumnProperty, 0);
				frame.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension2.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = frame;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
				xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemAdderPageV2.<InitializeComponent>_anonXamlCDataTemplate_38).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06003983 RID: 14723 RVA: 0x002DAD8C File Offset: 0x002D8F8C
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1092(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003984 RID: 14724 RVA: 0x002DADBC File Offset: 0x002D8FBC
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1093(PID A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}

			// Token: 0x06003985 RID: 14725 RVA: 0x002DADD8 File Offset: 0x002D8FD8
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1094(PID A_0)
			{
				return A_0;
			}

			// Token: 0x0400236C RID: 9068
			internal object[] parentValues;

			// Token: 0x0400236D RID: 9069
			internal DashboardItemAdderPageV2 root;
		}
	}
}
