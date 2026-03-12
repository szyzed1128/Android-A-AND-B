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
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200064B RID: 1611
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\PIDSelectorV2.xaml")]
	public class PIDSelectorV2 : ContentPage
	{
		// Token: 0x060037CF RID: 14287 RVA: 0x002A0740 File Offset: 0x0029E940
		private PIDSelectorV2(IEnumerable<PID> pids, Func<PID, bool> additionalFilter, IPID preselectedPID = null)
		{
			this.InitializeComponent();
			this.lvPids.ItemsSource = this.FilteredPids;
			this.searchBar.BindingContext = this;
			if (pids == null)
			{
				this.allPids = App.OBDReader.CurrentCarData.LiveDataPIDs.Concat(CustomPIDViewModel.CurrentProfile.PidCollection.Concat(CustomPIDViewModel.CurrentCustom.PidCollection)).ToList<PID>();
			}
			else
			{
				this.allPids = new List<PID>(App.OBDReader.CurrentCarData.LiveDataPIDs.Count + CustomPIDViewModel.CurrentProfile.PidCollection.Count + CustomPIDViewModel.CurrentCustom.PidCollection.Count);
				this.allPids.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs);
				this.allPids.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
				this.allPids.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
			}
			if (additionalFilter != null)
			{
				this.allPids = new List<PID>(this.allPids.Where((PID x) => additionalFilter(x)));
			}
			this.lvPids.SelectedItem = preselectedPID;
			this.lvPids.ScrollTo(this.lvPids.SelectedItem, 2, false);
			this.UpdateFilter();
		}

		// Token: 0x17001389 RID: 5001
		// (get) Token: 0x060037D0 RID: 14288 RVA: 0x002A08C8 File Offset: 0x0029EAC8
		public SmartCollection<PID> FilteredPids
		{
			get
			{
				return this._FilteredPids;
			}
		}

		// Token: 0x1700138A RID: 5002
		// (get) Token: 0x060037D1 RID: 14289 RVA: 0x002A08D0 File Offset: 0x0029EAD0
		// (set) Token: 0x060037D2 RID: 14290 RVA: 0x002A08D8 File Offset: 0x0029EAD8
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

		// Token: 0x060037D3 RID: 14291 RVA: 0x002A08F4 File Offset: 0x0029EAF4
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

		// Token: 0x060037D4 RID: 14292 RVA: 0x002A0954 File Offset: 0x0029EB54
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

		// Token: 0x060037D5 RID: 14293 RVA: 0x002A09F4 File Offset: 0x0029EBF4
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				IEnumerable<PID> enumerable = this.Sort(this.allPids.Where((PID x) => x != null));
				this.FilteredPids.Reset(enumerable);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<PID> enumerable2 = this.allPids;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable2 = enumerable2.Where((PID x) => (x != null && !string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			IEnumerable<PID> enumerable3 = this.Sort(enumerable2);
			this.FilteredPids.Reset(enumerable3);
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x002A0AC8 File Offset: 0x0029ECC8
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

		// Token: 0x1700138B RID: 5003
		// (get) Token: 0x060037D7 RID: 14295 RVA: 0x002A0AFF File Offset: 0x0029ECFF
		// (set) Token: 0x060037D8 RID: 14296 RVA: 0x002A0B07 File Offset: 0x0029ED07
		public IPID SelectedPID
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedPID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedPID>k__BackingField = value;
			}
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x002A0B10 File Offset: 0x0029ED10
		private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			(sender as View).IsEnabled = false;
			this.SelectedPID = e.ItemData as PID;
			await base.Navigation.PopAsync(true);
			(sender as View).IsEnabled = true;
			this.ReleaseSemaphore();
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x002A0B58 File Offset: 0x0029ED58
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			(sender as VisualElement).IsEnabled = false;
			this.SelectedPID = null;
			await base.Navigation.PopAsync(true);
			this.ReleaseSemaphore();
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x002A0B98 File Offset: 0x0029ED98
		private void ReleaseSemaphore()
		{
			try
			{
				this.semaphore.Release();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x002A0BC8 File Offset: 0x0029EDC8
		protected override bool OnBackButtonPressed()
		{
			this.btnCancel_Clicked(this, EventArgs.Empty);
			return true;
		}

		// Token: 0x060037DD RID: 14301 RVA: 0x002A0BD7 File Offset: 0x0029EDD7
		public Task WaitSemaphoreAsync()
		{
			return this.semaphore.WaitAsync();
		}

		// Token: 0x060037DE RID: 14302 RVA: 0x002A0BE4 File Offset: 0x0029EDE4
		public static async Task<IPID> SelectPIDAsync(IPID preSelectedPid = null, Func<PID, bool> additionalFilter = null)
		{
			PIDSelectorV2 selector;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				selector = new PIDSelectorV2(null, additionalFilter, preSelectedPid);
			}
			else
			{
				selector = new PIDSelectorV2(LiveDataPIDModel._PIDCollection, additionalFilter, preSelectedPid);
			}
			await App.GetCurrentPage().Navigation.PushAsync(selector);
			await selector.semaphore.WaitAsync();
			return selector.SelectedPID;
		}

		// Token: 0x060037DF RID: 14303 RVA: 0x002A0C30 File Offset: 0x0029EE30
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(PIDSelectorV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/PIDSelectorV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 17);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
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
			this.gridButtons = grid;
			this.searchBar = entry;
			this.lvPids = sfListView;
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			on.Platform = new List<string>(1) { "Android" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "0,20,0,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			onPlatform2.Android = new Thickness(0.0, 0.0, 0.0, 0.0);
			onPlatform2.WinPhone = new Thickness(0.0);
			onPlatform2.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid.SetValue(View.MarginProperty, onPlatform2);
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
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.Text = obj10;
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			entry.SetValue(Grid.RowProperty, 1);
			entry.SetValue(Grid.ColumnProperty, 0);
			entry.SetValue(Grid.ColumnSpanProperty, 2);
			dynamicResourceExtension5.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = entry;
			array8[1] = grid2;
			array8[2] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Entry.FontSizeProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(PIDSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 17)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			entry.SetDynamicResource(Entry.FontSizeProperty, dynamicResource5.Key);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid2.Children.Add(entry);
			sfListView.SetValue(Grid.RowProperty, 2);
			sfListView.SetValue(Grid.ColumnProperty, 0);
			sfListView.SetValue(Grid.ColumnSpanProperty, 2);
			sfListView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Handle_ItemTapped);
			IDataTemplate dataTemplate2 = dataTemplate;
			PIDSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_71 <InitializeComponent>_anonXamlCDataTemplate_ = new PIDSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_71();
			object[] array9 = new object[0 + 4];
			array9[0] = dataTemplate;
			array9[1] = sfListView;
			array9[2] = grid2;
			array9[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(sfListView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060037E0 RID: 14304 RVA: 0x002A20B8 File Offset: 0x002A02B8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<PIDSelectorV2>(this, typeof(PIDSelectorV2));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lvPids = NameScopeExtensions.FindByName<SfListView>(this, "lvPids");
		}

		// Token: 0x040021CD RID: 8653
		private List<PID> allPids;

		// Token: 0x040021CE RID: 8654
		private SmartCollection<PID> _FilteredPids = new SmartCollection<PID>();

		// Token: 0x040021CF RID: 8655
		private string _Filter = "";

		// Token: 0x040021D0 RID: 8656
		private string filtertext = "";

		// Token: 0x040021D1 RID: 8657
		[CompilerGenerated]
		private IPID <SelectedPID>k__BackingField;

		// Token: 0x040021D2 RID: 8658
		private SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);

		// Token: 0x040021D3 RID: 8659
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040021D4 RID: 8660
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x040021D5 RID: 8661
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvPids;

		// Token: 0x0200064C RID: 1612
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060037E1 RID: 14305 RVA: 0x002A2109 File Offset: 0x002A0309
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060037E2 RID: 14306 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060037E3 RID: 14307 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__10_0(PID x)
			{
				return x.Name;
			}

			// Token: 0x060037E4 RID: 14308 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <Sort>b__10_1(PID x)
			{
				return x.Name;
			}

			// Token: 0x060037E5 RID: 14309 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <Sort>b__10_2(PID x)
			{
				return x.Id;
			}

			// Token: 0x060037E6 RID: 14310 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <UpdateFilter>b__11_0(PID x)
			{
				return x != null;
			}

			// Token: 0x040021D6 RID: 8662
			public static readonly PIDSelectorV2.<>c <>9 = new PIDSelectorV2.<>c();

			// Token: 0x040021D7 RID: 8663
			public static Func<PID, string> <>9__10_0;

			// Token: 0x040021D8 RID: 8664
			public static Func<PID, string> <>9__10_1;

			// Token: 0x040021D9 RID: 8665
			public static Func<PID, int> <>9__10_2;

			// Token: 0x040021DA RID: 8666
			public static Func<PID, bool> <>9__11_0;
		}

		// Token: 0x0200064D RID: 1613
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060037E7 RID: 14311 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060037E8 RID: 14312 RVA: 0x002A2125 File Offset: 0x002A0325
			internal bool <.ctor>b__0(PID x)
			{
				return this.additionalFilter(x);
			}

			// Token: 0x040021DB RID: 8667
			public Func<PID, bool> additionalFilter;
		}

		// Token: 0x0200064E RID: 1614
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x060037E9 RID: 14313 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x060037EA RID: 14314 RVA: 0x002A2134 File Offset: 0x002A0334
			internal bool <UpdateFilter>b__1(PID x)
			{
				return (x != null && !string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x040021DC RID: 8668
			public string word;
		}

		// Token: 0x0200064F RID: 1615
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ItemTapped>d__18 : IAsyncStateMachine
		{
			// Token: 0x060037EB RID: 14315 RVA: 0x002A2190 File Offset: 0x002A0390
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV2 pidselectorV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						(sender as View).IsEnabled = false;
						pidselectorV.SelectedPID = e.ItemData as PID;
						taskAwaiter = pidselectorV.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, PIDSelectorV2.<Handle_ItemTapped>d__18>(ref taskAwaiter, ref this);
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
					(sender as View).IsEnabled = true;
					pidselectorV.ReleaseSemaphore();
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

			// Token: 0x060037EC RID: 14316 RVA: 0x002A2288 File Offset: 0x002A0488
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021DD RID: 8669
			public int <>1__state;

			// Token: 0x040021DE RID: 8670
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021DF RID: 8671
			public object sender;

			// Token: 0x040021E0 RID: 8672
			public PIDSelectorV2 <>4__this;

			// Token: 0x040021E1 RID: 8673
			public ItemTappedEventArgs e;

			// Token: 0x040021E2 RID: 8674
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000650 RID: 1616
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectPIDAsync>d__24 : IAsyncStateMachine
		{
			// Token: 0x060037ED RID: 14317 RVA: 0x002A2298 File Offset: 0x002A0498
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				IPID selectedPID;
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
							goto IL_011B;
						}
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
						{
							selector = new PIDSelectorV2(null, additionalFilter, preSelectedPid);
						}
						else
						{
							selector = new PIDSelectorV2(LiveDataPIDModel._PIDCollection, additionalFilter, preSelectedPid);
						}
						taskAwaiter = App.GetCurrentPage().Navigation.PushAsync(selector).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV2.<SelectPIDAsync>d__24>(ref taskAwaiter, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV2.<SelectPIDAsync>d__24>(ref taskAwaiter, ref this);
						return;
					}
					IL_011B:
					taskAwaiter.GetResult();
					selectedPID = selector.SelectedPID;
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
				this.<>t__builder.SetResult(selectedPID);
			}

			// Token: 0x060037EE RID: 14318 RVA: 0x002A242C File Offset: 0x002A062C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021E3 RID: 8675
			public int <>1__state;

			// Token: 0x040021E4 RID: 8676
			public AsyncTaskMethodBuilder<IPID> <>t__builder;

			// Token: 0x040021E5 RID: 8677
			public Func<PID, bool> additionalFilter;

			// Token: 0x040021E6 RID: 8678
			public IPID preSelectedPid;

			// Token: 0x040021E7 RID: 8679
			private PIDSelectorV2 <selector>5__2;

			// Token: 0x040021E8 RID: 8680
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000651 RID: 1617
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__19 : IAsyncStateMachine
		{
			// Token: 0x060037EF RID: 14319 RVA: 0x002A243C File Offset: 0x002A063C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV2 pidselectorV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						(sender as VisualElement).IsEnabled = false;
						pidselectorV.SelectedPID = null;
						taskAwaiter = pidselectorV.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, PIDSelectorV2.<btnCancel_Clicked>d__19>(ref taskAwaiter, ref this);
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
					pidselectorV.ReleaseSemaphore();
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

			// Token: 0x060037F0 RID: 14320 RVA: 0x002A2514 File Offset: 0x002A0714
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021E9 RID: 8681
			public int <>1__state;

			// Token: 0x040021EA RID: 8682
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021EB RID: 8683
			public object sender;

			// Token: 0x040021EC RID: 8684
			public PIDSelectorV2 <>4__this;

			// Token: 0x040021ED RID: 8685
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000652 RID: 1618
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__13 : IAsyncStateMachine
		{
			// Token: 0x060037F1 RID: 14321 RVA: 0x002A2524 File Offset: 0x002A0724
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDSelectorV2 pidselectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						pidselectorV.filtertext = pidselectorV.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDSelectorV2.<searchBar_TextChanged>d__13>(ref taskAwaiter, ref this);
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
					if (pidselectorV.filtertext == pidselectorV.searchBar.Text)
					{
						try
						{
							pidselectorV.Filter = pidselectorV.filtertext;
							if (pidselectorV.lvPids.SelectedItem != null)
							{
								pidselectorV.lvPids.ScrollTo(pidselectorV.lvPids.SelectedItem, 1, true);
							}
							else
							{
								pidselectorV.lvPids.ScrollTo(pidselectorV.FilteredPids.FirstOrDefault<PID>(), 0, true);
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

			// Token: 0x060037F2 RID: 14322 RVA: 0x002A2664 File Offset: 0x002A0864
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021EE RID: 8686
			public int <>1__state;

			// Token: 0x040021EF RID: 8687
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021F0 RID: 8688
			public PIDSelectorV2 <>4__this;

			// Token: 0x040021F1 RID: 8689
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000653 RID: 1619
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_71
		{
			// Token: 0x060037F3 RID: 14323 RVA: 0x002A2674 File Offset: 0x002A0874
			public <InitializeComponent>_anonXamlCDataTemplate_71()
			{
			}

			// Token: 0x060037F4 RID: 14324 RVA: 0x002A2688 File Offset: 0x002A0888
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 38);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 38);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 34);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 37);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\PIDSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
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
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_71).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 37)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Name";
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
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_71).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 37)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				grid.Children.Add(frame);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040021F2 RID: 8690
			internal object[] parentValues;

			// Token: 0x040021F3 RID: 8691
			internal PIDSelectorV2 root;
		}
	}
}
