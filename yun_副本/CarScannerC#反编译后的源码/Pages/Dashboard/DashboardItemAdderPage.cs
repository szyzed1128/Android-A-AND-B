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
	// Token: 0x02000688 RID: 1672
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\Dashboard\\DashboardItemAdderPage.xaml")]
	public class DashboardItemAdderPage : ContentPage
	{
		// Token: 0x0600392C RID: 14636 RVA: 0x002D44BC File Offset: 0x002D26BC
		public DashboardItemAdderPage(DashboardPage page, Point touchPoint)
		{
			this.InitializeComponent();
			this.Page = page;
			this.TouchPoint = touchPoint;
			this.lvPids.ItemsSource = this.FilteredPids;
			this.searchBar.BindingContext = this;
			this.UpdateFilter();
		}

		// Token: 0x17001393 RID: 5011
		// (get) Token: 0x0600392D RID: 14637 RVA: 0x002D4532 File Offset: 0x002D2732
		public SmartCollection<PID> FilteredPids
		{
			get
			{
				return this._FilteredPids;
			}
		}

		// Token: 0x17001394 RID: 5012
		// (get) Token: 0x0600392E RID: 14638 RVA: 0x002D453A File Offset: 0x002D273A
		// (set) Token: 0x0600392F RID: 14639 RVA: 0x002D4542 File Offset: 0x002D2742
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

		// Token: 0x06003930 RID: 14640 RVA: 0x002D455C File Offset: 0x002D275C
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

		// Token: 0x06003931 RID: 14641 RVA: 0x002D45BC File Offset: 0x002D27BC
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

		// Token: 0x06003932 RID: 14642 RVA: 0x002D465C File Offset: 0x002D285C
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

		// Token: 0x06003933 RID: 14643 RVA: 0x002D4758 File Offset: 0x002D2958
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

		// Token: 0x06003934 RID: 14644 RVA: 0x002D4790 File Offset: 0x002D2990
		private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			DashboardItemAdderPage.<>c__DisplayClass16_0 CS$<>8__locals1 = new DashboardItemAdderPage.<>c__DisplayClass16_0();
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
				DashboardItemAdderPage.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d <<Handle_ItemTapped>b__0>d;
				<<Handle_ItemTapped>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<Handle_ItemTapped>b__0>d.<>4__this = CS$<>8__locals1;
				<<Handle_ItemTapped>b__0>d.<>1__state = -1;
				<<Handle_ItemTapped>b__0>d.<>t__builder.Start<DashboardItemAdderPage.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref <<Handle_ItemTapped>b__0>d);
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

		// Token: 0x06003935 RID: 14645 RVA: 0x002D47D0 File Offset: 0x002D29D0
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
			try
			{
				foreach (DashboardItem dashboardItem in this.items)
				{
					if (dashboardItem.GestureRecognizers.Count > 0)
					{
						TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[dashboardItem.GestureRecognizers.Count - 1];
						tapGestureRecognizer.Tapped -= this.Tap_Tapped;
						dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
					}
					dashboardItem.Stop();
				}
			}
			catch (Exception)
			{
			}
			foreach (View view in this.uniGrid.Children)
			{
				Frame frame = (Frame)view;
				if (frame.GestureRecognizers.Count > 0)
				{
					((TapGestureRecognizer)frame.GestureRecognizers[0]).Tapped -= this.Tap_Tapped;
				}
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

		// Token: 0x06003936 RID: 14646 RVA: 0x002D480F File Offset: 0x002D2A0F
		protected override bool OnBackButtonPressed()
		{
			this.btnCancel_Clicked(this, null);
			return true;
		}

		// Token: 0x06003937 RID: 14647 RVA: 0x002D481C File Offset: 0x002D2A1C
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			if (sender != null && sender is VisualElement)
			{
				(sender as VisualElement).IsEnabled = false;
			}
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
			}
			(sender as VisualElement).IsEnabled = true;
		}

		// Token: 0x06003938 RID: 14648 RVA: 0x002D485C File Offset: 0x002D2A5C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardItemAdderPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardItemAdderPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 26);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 14);
			UniformGrid uniformGrid;
			VisualDiagnostics.RegisterSourceInfo(uniformGrid = new UniformGrid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardItemAdderPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 17)));
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
			DashboardItemAdderPage.<InitializeComponent>_anonXamlCDataTemplate_37 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardItemAdderPage.<InitializeComponent>_anonXamlCDataTemplate_37();
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

		// Token: 0x06003939 RID: 14649 RVA: 0x002D5DA0 File Offset: 0x002D3FA0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardItemAdderPage>(this, typeof(DashboardItemAdderPage));
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

		// Token: 0x040022F4 RID: 8948
		private DashboardPage Page;

		// Token: 0x040022F5 RID: 8949
		private Point TouchPoint;

		// Token: 0x040022F6 RID: 8950
		private SmartCollection<PID> _FilteredPids = new SmartCollection<PID>();

		// Token: 0x040022F7 RID: 8951
		private string _Filter = "";

		// Token: 0x040022F8 RID: 8952
		private string filtertext = "";

		// Token: 0x040022F9 RID: 8953
		private List<DashboardItem> items = new List<DashboardItem>();

		// Token: 0x040022FA RID: 8954
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage thisPage;

		// Token: 0x040022FB RID: 8955
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040022FC RID: 8956
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel lbNavbarTitle;

		// Token: 0x040022FD RID: 8957
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton sortButton;

		// Token: 0x040022FE RID: 8958
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040022FF RID: 8959
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridSelector;

		// Token: 0x04002300 RID: 8960
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002301 RID: 8961
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvPids;

		// Token: 0x04002302 RID: 8962
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView itemTypeSelector;

		// Token: 0x04002303 RID: 8963
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private UniformGrid uniGrid;

		// Token: 0x04002304 RID: 8964
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000689 RID: 1673
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600393A RID: 14650 RVA: 0x002D5E79 File Offset: 0x002D4079
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600393B RID: 14651 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600393C RID: 14652 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__11_0(PID x)
			{
				return x.Name;
			}

			// Token: 0x0600393D RID: 14653 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__11_1(PID x)
			{
				return x.Name;
			}

			// Token: 0x0600393E RID: 14654 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <Sort>b__11_2(PID x)
			{
				return x.Id;
			}

			// Token: 0x0600393F RID: 14655 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <UpdateFilter>b__12_0(PID x)
			{
				return x != null;
			}

			// Token: 0x06003940 RID: 14656 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <UpdateFilter>b__12_1(PID x)
			{
				return x != null;
			}

			// Token: 0x04002305 RID: 8965
			public static readonly DashboardItemAdderPage.<>c <>9 = new DashboardItemAdderPage.<>c();

			// Token: 0x04002306 RID: 8966
			public static Func<PID, string> <>9__11_0;

			// Token: 0x04002307 RID: 8967
			public static Func<PID, string> <>9__11_1;

			// Token: 0x04002308 RID: 8968
			public static Func<PID, int> <>9__11_2;

			// Token: 0x04002309 RID: 8969
			public static Func<PID, bool> <>9__12_0;

			// Token: 0x0400230A RID: 8970
			public static Func<PID, bool> <>9__12_1;
		}

		// Token: 0x0200068A RID: 1674
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06003941 RID: 14657 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06003942 RID: 14658 RVA: 0x002D5E88 File Offset: 0x002D4088
			internal bool <UpdateFilter>b__2(PID x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x0400230B RID: 8971
			public string word;
		}

		// Token: 0x0200068B RID: 1675
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06003943 RID: 14659 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06003944 RID: 14660 RVA: 0x002D5EE0 File Offset: 0x002D40E0
			internal async Task <Handle_ItemTapped>b__0()
			{
				DashboardItemAdderPage.<>c__DisplayClass16_1 CS$<>8__locals1 = new DashboardItemAdderPage.<>c__DisplayClass16_1();
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
				await Task.WhenAll(this.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemAdderPage.<>c__DisplayClass16_2
				{
					CS$<>8__locals2 = CS$<>8__locals1,
					item = item
				}.<Handle_ItemTapped>b__3))));
			}

			// Token: 0x06003945 RID: 14661 RVA: 0x002D5F24 File Offset: 0x002D4124
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

			// Token: 0x0400230C RID: 8972
			public PID pid;

			// Token: 0x0400230D RID: 8973
			public double size;

			// Token: 0x0400230E RID: 8974
			public DashboardItemAdderPage <>4__this;

			// Token: 0x0200068C RID: 1676
			[StructLayout(LayoutKind.Auto)]
			private struct <<Handle_ItemTapped>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06003946 RID: 14662 RVA: 0x002D6024 File Offset: 0x002D4224
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					DashboardItemAdderPage.<>c__DisplayClass16_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							DashboardItemAdderPage.<>c__DisplayClass16_1 CS$<>8__locals2 = new DashboardItemAdderPage.<>c__DisplayClass16_1();
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
							taskAwaiter = Task.WhenAll(CS$<>8__locals1.<>4__this.items.Select((DashboardItem item) => Task.Run(new Action(new DashboardItemAdderPage.<>c__DisplayClass16_2
							{
								CS$<>8__locals2 = CS$<>8__locals2,
								item = item
							}.<Handle_ItemTapped>b__3)))).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPage.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref taskAwaiter, ref this);
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

				// Token: 0x06003947 RID: 14663 RVA: 0x002D67E8 File Offset: 0x002D49E8
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400230F RID: 8975
				public int <>1__state;

				// Token: 0x04002310 RID: 8976
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04002311 RID: 8977
				public DashboardItemAdderPage.<>c__DisplayClass16_0 <>4__this;

				// Token: 0x04002312 RID: 8978
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x0200068D RID: 1677
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_1
		{
			// Token: 0x06003948 RID: 14664 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_1()
			{
			}

			// Token: 0x06003949 RID: 14665 RVA: 0x002D67F6 File Offset: 0x002D49F6
			internal Task <Handle_ItemTapped>b__2(DashboardItem item)
			{
				return Task.Run(new Action(new DashboardItemAdderPage.<>c__DisplayClass16_2
				{
					CS$<>8__locals2 = this,
					item = item
				}.<Handle_ItemTapped>b__3));
			}

			// Token: 0x04002313 RID: 8979
			public LiveDataPIDModel model;

			// Token: 0x04002314 RID: 8980
			public DashboardItemAdderPage.<>c__DisplayClass16_0 CS$<>8__locals1;
		}

		// Token: 0x0200068E RID: 1678
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_2
		{
			// Token: 0x0600394A RID: 14666 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_2()
			{
			}

			// Token: 0x0600394B RID: 14667 RVA: 0x002D681C File Offset: 0x002D4A1C
			internal void <Handle_ItemTapped>b__3()
			{
				this.item.SelectAndAddControl();
				this.item.Model = this.CS$<>8__locals2.model;
				this.item.Start();
				TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
				tapGestureRecognizer.Tapped += this.CS$<>8__locals2.CS$<>8__locals1.<>4__this.Tap_Tapped;
				this.item.GestureRecognizers.Add(tapGestureRecognizer);
			}

			// Token: 0x04002315 RID: 8981
			public DashboardItem item;

			// Token: 0x04002316 RID: 8982
			public DashboardItemAdderPage.<>c__DisplayClass16_1 CS$<>8__locals2;
		}

		// Token: 0x0200068F RID: 1679
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ItemTapped>d__16 : IAsyncStateMachine
		{
			// Token: 0x0600394C RID: 14668 RVA: 0x002D6890 File Offset: 0x002D4A90
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPage dashboardItemAdderPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new DashboardItemAdderPage.<>c__DisplayClass16_0();
						CS$<>8__locals1.<>4__this = this;
						dashboardItemAdderPage.activityFrame.IsVisible = true;
						CS$<>8__locals1.pid = e.ItemData as PID;
						double num3 = ((dashboardItemAdderPage.Width > dashboardItemAdderPage.Height) ? dashboardItemAdderPage.Height : dashboardItemAdderPage.Width);
						CS$<>8__locals1.size = Math.Round(num3 / 2.5, 0);
						dashboardItemAdderPage.itemTypeSelector.IsVisible = true;
						dashboardItemAdderPage.gridSelector.IsVisible = false;
						dashboardItemAdderPage.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_DisplayType");
						dashboardItemAdderPage.sortButton.IsVisible = false;
						taskAwaiter = Task.Run(delegate
						{
							DashboardItemAdderPage.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d <<Handle_ItemTapped>b__0>d;
							<<Handle_ItemTapped>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<Handle_ItemTapped>b__0>d.<>4__this = CS$<>8__locals1;
							<<Handle_ItemTapped>b__0>d.<>1__state = -1;
							<<Handle_ItemTapped>b__0>d.<>t__builder.Start<DashboardItemAdderPage.<>c__DisplayClass16_0.<<Handle_ItemTapped>b__0>d>(ref <<Handle_ItemTapped>b__0>d);
							return <<Handle_ItemTapped>b__0>d.<>t__builder.Task;
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPage.<Handle_ItemTapped>d__16>(ref taskAwaiter, ref this);
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
						dashboardItemAdderPage.items.FirstOrDefault<DashboardItem>().Model.GetRequests(list, null, "");
						App.OBDReader.ReplaceQueue(list);
					}
					if (CS$<>8__locals1.pid is CustomPID && (CS$<>8__locals1.pid as CustomPID).IsAction)
					{
						dashboardItemAdderPage.Tap_Tapped(dashboardItemAdderPage.items.FirstOrDefault<DashboardItem>(), EventArgs.Empty);
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

			// Token: 0x0600394D RID: 14669 RVA: 0x002D6ABC File Offset: 0x002D4CBC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002317 RID: 8983
			public int <>1__state;

			// Token: 0x04002318 RID: 8984
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002319 RID: 8985
			public DashboardItemAdderPage <>4__this;

			// Token: 0x0400231A RID: 8986
			public ItemTappedEventArgs e;

			// Token: 0x0400231B RID: 8987
			private DashboardItemAdderPage.<>c__DisplayClass16_0 <>8__1;

			// Token: 0x0400231C RID: 8988
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000690 RID: 1680
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Tap_Tapped>d__17 : IAsyncStateMachine
		{
			// Token: 0x0600394E RID: 14670 RVA: 0x002D6ACC File Offset: 0x002D4CCC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPage dashboardItemAdderPage = this;
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
						dashboardItemAdderPage.IsEnabled = false;
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
						dashboardItemAdderPage.itemTypeSelector.IsEnabled = false;
						custPage = (Dash_CustomPage)dashboardItemAdderPage.Page;
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DashboardItemAdderPage.<Tap_Tapped>d__17>(ref taskAwaiter3, ref this);
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
					taskAwaiter = dashboardItemAdderPage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemAdderPage.<Tap_Tapped>d__17>(ref taskAwaiter, ref this);
						return;
					}
					IL_019D:
					taskAwaiter.GetResult();
					f.Content = null;
					item.PositionX = dashboardItemAdderPage.TouchPoint.X;
					item.PositionY = dashboardItemAdderPage.TouchPoint.Y;
					custPage.AddNewItem(item);
					try
					{
						List<DashboardItem>.Enumerator enumerator = dashboardItemAdderPage.items.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DashboardItem dashboardItem = enumerator.Current;
								if (dashboardItem.GestureRecognizers.Count > 0)
								{
									TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[dashboardItem.GestureRecognizers.Count - 1];
									tapGestureRecognizer.Tapped -= dashboardItemAdderPage.Tap_Tapped;
									dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
								}
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
					}
					catch (Exception)
					{
					}
					IEnumerator<View> enumerator2 = dashboardItemAdderPage.uniGrid.Children.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							View view = enumerator2.Current;
							Frame frame = (Frame)view;
							if (frame.GestureRecognizers.Count > 0)
							{
								((TapGestureRecognizer)frame.GestureRecognizers[0]).Tapped -= dashboardItemAdderPage.Tap_Tapped;
							}
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
					dashboardItemAdderPage.IsEnabled = true;
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

			// Token: 0x0600394F RID: 14671 RVA: 0x002D6EFC File Offset: 0x002D50FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400231D RID: 8989
			public int <>1__state;

			// Token: 0x0400231E RID: 8990
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400231F RID: 8991
			public DashboardItemAdderPage <>4__this;

			// Token: 0x04002320 RID: 8992
			public object sender;

			// Token: 0x04002321 RID: 8993
			private Frame <f>5__2;

			// Token: 0x04002322 RID: 8994
			private DashboardItem <item>5__3;

			// Token: 0x04002323 RID: 8995
			private Dash_CustomPage <custPage>5__4;

			// Token: 0x04002324 RID: 8996
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002325 RID: 8997
			private TaskAwaiter<Page> <>u__2;
		}

		// Token: 0x02000691 RID: 1681
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__19 : IAsyncStateMachine
		{
			// Token: 0x06003950 RID: 14672 RVA: 0x002D6F0C File Offset: 0x002D510C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPage dashboardItemAdderPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						if (sender != null && sender is VisualElement)
						{
							(sender as VisualElement).IsEnabled = false;
						}
						if (!dashboardItemAdderPage.gridSelector.IsVisible)
						{
							dashboardItemAdderPage.gridSelector.IsVisible = true;
							dashboardItemAdderPage.itemTypeSelector.IsVisible = false;
							dashboardItemAdderPage.sortButton.IsVisible = true;
							dashboardItemAdderPage.lbNavbarTitle.Text = Translate.GetString("ios_DashboardItemEditor_Sensor");
							List<DashboardItem>.Enumerator enumerator = dashboardItemAdderPage.items.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardItem dashboardItem = enumerator.Current;
									if (dashboardItem != null)
									{
										dashboardItem.Stop();
										TapGestureRecognizer tapGestureRecognizer = (TapGestureRecognizer)dashboardItem.GestureRecognizers[0];
										tapGestureRecognizer.Tapped -= dashboardItemAdderPage.Tap_Tapped;
										dashboardItem.GestureRecognizers.Remove(tapGestureRecognizer);
										Frame frame = (Frame)dashboardItem.Parent;
										tapGestureRecognizer = (TapGestureRecognizer)frame.GestureRecognizers[0];
										tapGestureRecognizer.Tapped -= dashboardItemAdderPage.Tap_Tapped;
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
							dashboardItemAdderPage.items.Clear();
							dashboardItemAdderPage.uniGrid.Children.Clear();
							goto IL_01B9;
						}
						taskAwaiter = dashboardItemAdderPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardItemAdderPage.<btnCancel_Clicked>d__19>(ref taskAwaiter, ref this);
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
					IL_01B9:
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

			// Token: 0x06003951 RID: 14673 RVA: 0x002D7148 File Offset: 0x002D5348
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002326 RID: 8998
			public int <>1__state;

			// Token: 0x04002327 RID: 8999
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002328 RID: 9000
			public object sender;

			// Token: 0x04002329 RID: 9001
			public DashboardItemAdderPage <>4__this;

			// Token: 0x0400232A RID: 9002
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000692 RID: 1682
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__14 : IAsyncStateMachine
		{
			// Token: 0x06003952 RID: 14674 RVA: 0x002D7158 File Offset: 0x002D5358
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemAdderPage dashboardItemAdderPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardItemAdderPage.filtertext = dashboardItemAdderPage.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemAdderPage.<searchBar_TextChanged>d__14>(ref taskAwaiter, ref this);
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
					if (dashboardItemAdderPage.filtertext == dashboardItemAdderPage.searchBar.Text)
					{
						try
						{
							dashboardItemAdderPage.Filter = dashboardItemAdderPage.filtertext;
							if (dashboardItemAdderPage.lvPids.SelectedItem != null)
							{
								dashboardItemAdderPage.lvPids.ScrollTo(dashboardItemAdderPage.lvPids.SelectedItem, 1, true);
							}
							else
							{
								dashboardItemAdderPage.lvPids.ScrollTo(dashboardItemAdderPage.FilteredPids.FirstOrDefault<PID>(), 0, true);
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

			// Token: 0x06003953 RID: 14675 RVA: 0x002D7298 File Offset: 0x002D5498
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400232B RID: 9003
			public int <>1__state;

			// Token: 0x0400232C RID: 9004
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400232D RID: 9005
			public DashboardItemAdderPage <>4__this;

			// Token: 0x0400232E RID: 9006
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000693 RID: 1683
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_37
		{
			// Token: 0x06003954 RID: 14676 RVA: 0x002D72A8 File Offset: 0x002D54A8
			public <InitializeComponent>_anonXamlCDataTemplate_37()
			{
			}

			// Token: 0x06003955 RID: 14677 RVA: 0x002D72BC File Offset: 0x002D54BC
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 42);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 41);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 41);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardItemAdderPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 30);
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemAdderPage.<InitializeComponent>_anonXamlCDataTemplate_37).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 41)));
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
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemAdderPage.<InitializeComponent>_anonXamlCDataTemplate_37).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06003956 RID: 14678 RVA: 0x002D7928 File Offset: 0x002D5B28
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1089(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003957 RID: 14679 RVA: 0x002D7958 File Offset: 0x002D5B58
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1090(PID A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}

			// Token: 0x06003958 RID: 14680 RVA: 0x002D7974 File Offset: 0x002D5B74
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1091(PID A_0)
			{
				return A_0;
			}

			// Token: 0x0400232F RID: 9007
			internal object[] parentValues;

			// Token: 0x04002330 RID: 9008
			internal DashboardItemAdderPage root;
		}
	}
}
