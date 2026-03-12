using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006C5 RID: 1733
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml")]
	public class LiveDataMultiChartSelectorPageV3 : ContentPage
	{
		// Token: 0x06003B17 RID: 15127 RVA: 0x0031262C File Offset: 0x0031082C
		public LiveDataMultiChartSelectorPageV3()
		{
			this.InitializeComponent();
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			this.btnOK.IsEnabled = false;
			this.btnOK.TextColor = Color.White;
			this.btnOK.BackgroundColor = Color.DarkGray;
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_checked_checkbox", delegate
			{
				foreach (PIDProxy pidproxy in this.PidList)
				{
					pidproxy.IsVisible = true;
				}
				this.SetSelectedCount();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_unchecked_checkbox", delegate
			{
				foreach (PIDProxy pidproxy2 in this.PidList)
				{
					pidproxy2.IsVisible = false;
				}
				this.SetSelectedCount();
			}, 0, 0));
			this.GeneratePidList();
			this.lvSettingsRoot.BindingContext = this;
		}

		// Token: 0x06003B18 RID: 15128 RVA: 0x00312730 File Offset: 0x00310930
		private void ResetFilteredPidListWithIENumerable(IEnumerable<PIDProxy> collection)
		{
			this._FilteredPidList.Clear();
			foreach (PIDProxy pidproxy in collection)
			{
				this._FilteredPidList.Add(pidproxy);
			}
			this.lvSection.ItemsSource = this.FilteredPidList;
		}

		// Token: 0x1700139B RID: 5019
		// (get) Token: 0x06003B19 RID: 15129 RVA: 0x0031279C File Offset: 0x0031099C
		// (set) Token: 0x06003B1A RID: 15130 RVA: 0x003127A4 File Offset: 0x003109A4
		public List<LiveDataPIDModel> CacheModelsList
		{
			[CompilerGenerated]
			get
			{
				return this.<CacheModelsList>k__BackingField;
			}
			[CompilerGenerated]
			internal set
			{
				this.<CacheModelsList>k__BackingField = value;
			}
		} = new List<LiveDataPIDModel>();

		// Token: 0x1700139C RID: 5020
		// (get) Token: 0x06003B1B RID: 15131 RVA: 0x003127AD File Offset: 0x003109AD
		public ObservableCollection<PIDProxy> FilteredPidList
		{
			get
			{
				return this._FilteredPidList;
			}
		}

		// Token: 0x06003B1C RID: 15132 RVA: 0x003127B8 File Offset: 0x003109B8
		private async void Page_Appearing(object sender, EventArgs e)
		{
			await App.OBDReader.ClearRequestQueue();
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x06003B1D RID: 15133 RVA: 0x003127E8 File Offset: 0x003109E8
		private void SettingsCheckBoxCellPatched_Tapped(object sender, EventArgs e)
		{
			if (this.PidList.Any((PIDProxy x) => x.IsVisible))
			{
				this.btnOK.IsEnabled = true;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.Green;
			}
			else
			{
				this.btnOK.IsEnabled = false;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.DarkGray;
			}
			this.SetSelectedCount();
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06003B1E RID: 15134 RVA: 0x0031288C File Offset: 0x00310A8C
		private void GeneratePidList()
		{
			foreach (PID pid in LiveDataPIDModel._PIDCollection)
			{
				if (pid is IPIDFloatValue)
				{
					this.PidList.Add(new PIDProxy(pid));
				}
			}
			this.ResetFilteredPidListWithIENumerable(this.PidList);
			if (!string.IsNullOrEmpty(SharedSettings.Current.MultiPidsSelected))
			{
				string[] array = SharedSettings.Current.MultiPidsSelected.Split(new char[] { ';' });
				new List<int>(array.Length);
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					int id = -1;
					if (int.TryParse(text, out id))
					{
						PIDProxy pidproxy = this.PidList.FirstOrDefault((PIDProxy x) => x.Pid.Id == id);
						if (pidproxy != null)
						{
							pidproxy.IsVisible = true;
						}
						this.btnOK.IsEnabled = true;
						this.btnOK.TextColor = Color.White;
						this.btnOK.BackgroundColor = Color.Green;
					}
				}
			}
			this.SetSelectedCount();
		}

		// Token: 0x06003B1F RID: 15135 RVA: 0x003129B4 File Offset: 0x00310BB4
		private void SetSelectedCount()
		{
			this.labelSelectedCount.Text = this.PidList.Count((PIDProxy x) => x.IsVisible).ToString();
		}

		// Token: 0x06003B20 RID: 15136 RVA: 0x00312A00 File Offset: 0x00310C00
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			this.btnOK.IsEnabled = false;
			List<PID> list = (from x in this.PidList
				where x.IsVisible
				select x.Pid).ToList<PID>();
			if (list.Count > 0)
			{
				if (list.Count > 2 && !SharedSettings.Current.AdsProductPurchased)
				{
					string text = Translate.GetString("ios_Limit2");
					text = string.Format(text, this.PidList.Count((PIDProxy x) => x.IsVisible).ToString());
					base.DisplayAlert("Car Scanner Pro", text, "OK");
					this.btnOK.IsEnabled = true;
					return;
				}
				this.SaveSelected();
				if ((from x in list
					select (IPIDFloatValue)x into x
					select x.Units).Distinct<UnitsHelper.Units>().Count<UnitsHelper.Units>() == 2)
				{
					LiveDataMultiChartPageV2 liveDataMultiChartPageV = new LiveDataMultiChartPageV2(list.Cast<IPIDFloatValue>().ToList<IPIDFloatValue>(), this);
					await base.Navigation.PushAsync(liveDataMultiChartPageV);
				}
				else
				{
					LiveMultiChartPage liveMultiChartPage = new LiveMultiChartPage(list);
					await base.Navigation.PushAsync(liveMultiChartPage);
				}
			}
			this.btnOK.IsEnabled = true;
		}

		// Token: 0x06003B21 RID: 15137 RVA: 0x00312A38 File Offset: 0x00310C38
		private void SaveSelected()
		{
			PIDProxy[] array = this.PidList.Where((PIDProxy x) => x.IsVisible).ToArray<PIDProxy>();
			StringBuilder stringBuilder = new StringBuilder(array.Length * 2);
			foreach (PIDProxy pidproxy in array)
			{
				stringBuilder.Append(pidproxy.Pid.Id);
				stringBuilder.Append(";");
			}
			SharedSettings.Current.MultiPidsSelected = stringBuilder.ToString();
		}

		// Token: 0x06003B22 RID: 15138 RVA: 0x00312AC0 File Offset: 0x00310CC0
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filtertext = this.searchBar.Text;
			await Task.Delay(500);
			if (this.filtertext == this.searchBar.Text)
			{
				try
				{
					this.Filter = this.filtertext;
				}
				catch
				{
				}
			}
		}

		// Token: 0x1700139D RID: 5021
		// (get) Token: 0x06003B23 RID: 15139 RVA: 0x00312AF7 File Offset: 0x00310CF7
		// (set) Token: 0x06003B24 RID: 15140 RVA: 0x00312AFF File Offset: 0x00310CFF
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

		// Token: 0x06003B25 RID: 15141 RVA: 0x00312B1C File Offset: 0x00310D1C
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				this.Sort(this.PidList);
				this.ResetFilteredPidListWithIENumerable(this.PidList);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<PIDProxy> enumerable = this.PidList;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable = enumerable.Where((PIDProxy x) => (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			this.Sort(enumerable);
			this.ResetFilteredPidListWithIENumerable(this.PidList);
		}

		// Token: 0x06003B26 RID: 15142 RVA: 0x00312BC4 File Offset: 0x00310DC4
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

		// Token: 0x06003B27 RID: 15143 RVA: 0x00312C24 File Offset: 0x00310E24
		private IEnumerable<PIDProxy> Sort(IEnumerable<PIDProxy> collection)
		{
			switch (SharedSettings.Current.PIDSortingMode)
			{
			case SharedSettings.PIDSortingModes.NameAsc:
				return collection.OrderBy((PIDProxy x) => x.Pid.Name);
			case SharedSettings.PIDSortingModes.NameDesc:
				return collection.OrderByDescending((PIDProxy x) => x.Pid.Name);
			}
			return from x in collection
				orderby false, x.Pid.Id
				select x;
		}

		// Token: 0x06003B28 RID: 15144 RVA: 0x00312CE8 File Offset: 0x00310EE8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/LiveDataMultiChartSelectorPageV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 17);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("lvSettingsRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "lvSettingsRoot";
			}
			nameScope.RegisterName("lvSection", section);
			if (section.StyleId == null)
			{
				section.StyleId = "lvSection";
			}
			nameScope.RegisterName("labelSelectedCount", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelSelectedCount";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.page = this;
			this.searchBar = entry;
			this.lvSettingsRoot = settingsView;
			this.lvSection = section;
			this.labelSelectedCount = label2;
			this.btnOK = button;
			this.ad = complexAdView;
			translate.Text = "ios_MultiChartSelectorTitle";
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(NavigationPage.HideNavigationBarSeparatorProperty, true);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Page_Appearing;
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto, *, Auto, Auto, Auto"));
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid.Children.Add(entry);
			settingsView.SetValue(Grid.RowProperty, 1);
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension3 = referenceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = settingsView;
			array3[1] = grid;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 17)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			settingsView.SetValue(BindableObject.BindingContextProperty, obj5);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			referenceExtension2.Name = "page";
			IMarkupExtension markupExtension4 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = section;
			array4[1] = settingsView;
			array4[2] = grid;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			section.SetValue(BindableObject.BindingContextProperty, obj7);
			bindingExtension.Path = "FilteredPidList";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(Section.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			LiveDataMultiChartSelectorPageV3.<InitializeComponent>_anonXamlCDataTemplate_49 <InitializeComponent>_anonXamlCDataTemplate_ = new LiveDataMultiChartSelectorPageV3.<InitializeComponent>_anonXamlCDataTemplate_49();
			object[] array5 = new object[0 + 5];
			array5[0] = dataTemplate;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = grid;
			array5[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section.SetValue(Section.ItemTemplateProperty, dataTemplate);
			labelCell.SetValue(CellBase.TitleProperty, "TEST");
			section.Add(labelCell);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			stackLayout.SetValue(Grid.RowProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label;
			array6[1] = stackLayout;
			array6[2] = grid;
			array6[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 21)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			translate2.Text = "ios_MultiChartSelectedCount";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = label;
			array7[1] = stackLayout;
			array7[2] = grid;
			array7[3] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 21)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.Text = obj10;
			stackLayout.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = label2;
			array8[1] = stackLayout;
			array8[2] = grid;
			array8[3] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LiveDataMultiChartSelectorPageV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 21)));
			DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label2.SetValue(Label.TextProperty, "0");
			stackLayout.Children.Add(label2);
			grid.Children.Add(stackLayout);
			button.SetValue(Grid.RowProperty, 3);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(VisualElement.IsEnabledProperty, false);
			button.SetValue(Button.TextProperty, "OK");
			grid.Children.Add(button);
			complexAdView.SetValue(Grid.RowProperty, 4);
			complexAdView.SetValue(View.MarginProperty, new Thickness(0.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003B29 RID: 15145 RVA: 0x00313DCC File Offset: 0x00311FCC
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			foreach (PIDProxy pidproxy in this.PidList)
			{
				pidproxy.IsVisible = true;
			}
			this.SetSelectedCount();
		}

		// Token: 0x06003B2A RID: 15146 RVA: 0x00313E20 File Offset: 0x00312020
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			foreach (PIDProxy pidproxy in this.PidList)
			{
				pidproxy.IsVisible = false;
			}
			this.SetSelectedCount();
		}

		// Token: 0x06003B2B RID: 15147 RVA: 0x00313E74 File Offset: 0x00312074
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataMultiChartSelectorPageV3>(this, typeof(LiveDataMultiChartSelectorPageV3));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lvSettingsRoot = NameScopeExtensions.FindByName<SettingsView>(this, "lvSettingsRoot");
			this.lvSection = NameScopeExtensions.FindByName<Section>(this, "lvSection");
			this.labelSelectedCount = NameScopeExtensions.FindByName<Label>(this, "labelSelectedCount");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400242C RID: 9260
		[CompilerGenerated]
		private List<LiveDataPIDModel> <CacheModelsList>k__BackingField;

		// Token: 0x0400242D RID: 9261
		private ObservableCollection<PIDProxy> PidList = new ObservableCollection<PIDProxy>();

		// Token: 0x0400242E RID: 9262
		private ObservableCollection<PIDProxy> _FilteredPidList = new SmartCollection<PIDProxy>();

		// Token: 0x0400242F RID: 9263
		private string _Filter = "";

		// Token: 0x04002430 RID: 9264
		private string filtertext = "";

		// Token: 0x04002431 RID: 9265
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002432 RID: 9266
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002433 RID: 9267
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lvSettingsRoot;

		// Token: 0x04002434 RID: 9268
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section lvSection;

		// Token: 0x04002435 RID: 9269
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelSelectedCount;

		// Token: 0x04002436 RID: 9270
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x04002437 RID: 9271
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020006C6 RID: 1734
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003B2C RID: 15148 RVA: 0x00313F09 File Offset: 0x00312109
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003B2D RID: 15149 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003B2E RID: 15150 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <SettingsCheckBoxCellPatched_Tapped>b__11_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003B2F RID: 15151 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <SetSelectedCount>b__13_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003B30 RID: 15152 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <btnOK_Clicked>b__14_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003B31 RID: 15153 RVA: 0x0029B90D File Offset: 0x00299B0D
			internal PID <btnOK_Clicked>b__14_1(PIDProxy x)
			{
				return x.Pid;
			}

			// Token: 0x06003B32 RID: 15154 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <btnOK_Clicked>b__14_2(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003B33 RID: 15155 RVA: 0x0004C724 File Offset: 0x0004A924
			internal IPIDFloatValue <btnOK_Clicked>b__14_3(PID x)
			{
				return (IPIDFloatValue)x;
			}

			// Token: 0x06003B34 RID: 15156 RVA: 0x00298DCC File Offset: 0x00296FCC
			internal UnitsHelper.Units <btnOK_Clicked>b__14_4(IPIDFloatValue x)
			{
				return x.Units;
			}

			// Token: 0x06003B35 RID: 15157 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <SaveSelected>b__15_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003B36 RID: 15158 RVA: 0x0029B8F3 File Offset: 0x00299AF3
			internal string <Sort>b__24_0(PIDProxy x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003B37 RID: 15159 RVA: 0x0029B8F3 File Offset: 0x00299AF3
			internal string <Sort>b__24_1(PIDProxy x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003B38 RID: 15160 RVA: 0x00002076 File Offset: 0x00000276
			internal bool <Sort>b__24_2(PIDProxy x)
			{
				return false;
			}

			// Token: 0x06003B39 RID: 15161 RVA: 0x0029B900 File Offset: 0x00299B00
			internal int <Sort>b__24_3(PIDProxy x)
			{
				return x.Pid.Id;
			}

			// Token: 0x04002438 RID: 9272
			public static readonly LiveDataMultiChartSelectorPageV3.<>c <>9 = new LiveDataMultiChartSelectorPageV3.<>c();

			// Token: 0x04002439 RID: 9273
			public static Func<PIDProxy, bool> <>9__11_0;

			// Token: 0x0400243A RID: 9274
			public static Func<PIDProxy, bool> <>9__13_0;

			// Token: 0x0400243B RID: 9275
			public static Func<PIDProxy, bool> <>9__14_0;

			// Token: 0x0400243C RID: 9276
			public static Func<PIDProxy, PID> <>9__14_1;

			// Token: 0x0400243D RID: 9277
			public static Func<PIDProxy, bool> <>9__14_2;

			// Token: 0x0400243E RID: 9278
			public static Func<PID, IPIDFloatValue> <>9__14_3;

			// Token: 0x0400243F RID: 9279
			public static Func<IPIDFloatValue, UnitsHelper.Units> <>9__14_4;

			// Token: 0x04002440 RID: 9280
			public static Func<PIDProxy, bool> <>9__15_0;

			// Token: 0x04002441 RID: 9281
			public static Func<PIDProxy, string> <>9__24_0;

			// Token: 0x04002442 RID: 9282
			public static Func<PIDProxy, string> <>9__24_1;

			// Token: 0x04002443 RID: 9283
			public static Func<PIDProxy, bool> <>9__24_2;

			// Token: 0x04002444 RID: 9284
			public static Func<PIDProxy, int> <>9__24_3;
		}

		// Token: 0x020006C7 RID: 1735
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06003B3A RID: 15162 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06003B3B RID: 15163 RVA: 0x00313F15 File Offset: 0x00312115
			internal bool <GeneratePidList>b__0(PIDProxy x)
			{
				return x.Pid.Id == this.id;
			}

			// Token: 0x04002445 RID: 9285
			public int id;
		}

		// Token: 0x020006C8 RID: 1736
		[CompilerGenerated]
		private sealed class <>c__DisplayClass22_0
		{
			// Token: 0x06003B3C RID: 15164 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass22_0()
			{
			}

			// Token: 0x06003B3D RID: 15165 RVA: 0x00313F2C File Offset: 0x0031212C
			internal bool <UpdateFilter>b__0(PIDProxy x)
			{
				return (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002446 RID: 9286
			public string word;
		}

		// Token: 0x020006C9 RID: 1737
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Appearing>d__10 : IAsyncStateMachine
		{
			// Token: 0x06003B3E RID: 15166 RVA: 0x00313F98 File Offset: 0x00312198
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = App.OBDReader.ClearRequestQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelectorPageV3.<Page_Appearing>d__10>(ref taskAwaiter, ref this);
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

			// Token: 0x06003B3F RID: 15167 RVA: 0x0031404C File Offset: 0x0031224C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002447 RID: 9287
			public int <>1__state;

			// Token: 0x04002448 RID: 9288
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002449 RID: 9289
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006CA RID: 1738
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x06003B40 RID: 15168 RVA: 0x0031405C File Offset: 0x0031225C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataMultiChartSelectorPageV3 liveDataMultiChartSelectorPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							liveDataMultiChartSelectorPageV.btnOK.IsEnabled = false;
							List<PID> list = (from x in liveDataMultiChartSelectorPageV.PidList
								where x.IsVisible
								select x.Pid).ToList<PID>();
							if (list.Count <= 0)
							{
								goto IL_0244;
							}
							if (list.Count > 2 && !SharedSettings.Current.AdsProductPurchased)
							{
								string text = Translate.GetString("ios_Limit2");
								text = string.Format(text, liveDataMultiChartSelectorPageV.PidList.Count((PIDProxy x) => x.IsVisible).ToString());
								liveDataMultiChartSelectorPageV.DisplayAlert("Car Scanner Pro", text, "OK");
								liveDataMultiChartSelectorPageV.btnOK.IsEnabled = true;
								goto IL_026B;
							}
							liveDataMultiChartSelectorPageV.SaveSelected();
							if ((from x in list
								select (IPIDFloatValue)x into x
								select x.Units).Distinct<UnitsHelper.Units>().Count<UnitsHelper.Units>() == 2)
							{
								LiveDataMultiChartPageV2 liveDataMultiChartPageV = new LiveDataMultiChartPageV2(list.Cast<IPIDFloatValue>().ToList<IPIDFloatValue>(), liveDataMultiChartSelectorPageV);
								taskAwaiter = liveDataMultiChartSelectorPageV.Navigation.PushAsync(liveDataMultiChartPageV).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelectorPageV3.<btnOK_Clicked>d__14>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_01D1;
							}
							else
							{
								LiveMultiChartPage liveMultiChartPage = new LiveMultiChartPage(list);
								taskAwaiter = liveDataMultiChartSelectorPageV.Navigation.PushAsync(liveMultiChartPage).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelectorPageV3.<btnOK_Clicked>d__14>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0244;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_01D1:
					taskAwaiter.GetResult();
					IL_0244:
					liveDataMultiChartSelectorPageV.btnOK.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_026B:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003B41 RID: 15169 RVA: 0x00314304 File Offset: 0x00312504
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400244A RID: 9290
			public int <>1__state;

			// Token: 0x0400244B RID: 9291
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400244C RID: 9292
			public LiveDataMultiChartSelectorPageV3 <>4__this;

			// Token: 0x0400244D RID: 9293
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006CB RID: 1739
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__16 : IAsyncStateMachine
		{
			// Token: 0x06003B42 RID: 15170 RVA: 0x00314314 File Offset: 0x00312514
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataMultiChartSelectorPageV3 liveDataMultiChartSelectorPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						liveDataMultiChartSelectorPageV.filtertext = liveDataMultiChartSelectorPageV.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelectorPageV3.<searchBar_TextChanged>d__16>(ref taskAwaiter, ref this);
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
					if (liveDataMultiChartSelectorPageV.filtertext == liveDataMultiChartSelectorPageV.searchBar.Text)
					{
						try
						{
							liveDataMultiChartSelectorPageV.Filter = liveDataMultiChartSelectorPageV.filtertext;
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

			// Token: 0x06003B43 RID: 15171 RVA: 0x00314410 File Offset: 0x00312610
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400244E RID: 9294
			public int <>1__state;

			// Token: 0x0400244F RID: 9295
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002450 RID: 9296
			public LiveDataMultiChartSelectorPageV3 <>4__this;

			// Token: 0x04002451 RID: 9297
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006CC RID: 1740
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_49
		{
			// Token: 0x06003B44 RID: 15172 RVA: 0x00314420 File Offset: 0x00312620
			public <InitializeComponent>_anonXamlCDataTemplate_49()
			{
			}

			// Token: 0x06003B45 RID: 15173 RVA: 0x00314434 File Offset: 0x00312634
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 33);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 33);
				CheckboxCell checkboxCell;
				VisualDiagnostics.RegisterSourceInfo(checkboxCell = new CheckboxCell(), new Uri("Pages\\Dashboard\\LiveDataMultiChartSelectorPageV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(checkboxCell, nameScope);
				bindingExtension.Path = "Pid.Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				checkboxCell.SetBinding(CellBase.TitleProperty, bindingBase);
				bindingExtension2.Mode = 1;
				bindingExtension2.Path = "IsVisible";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				checkboxCell.SetBinding(CheckboxCell.CheckedProperty, bindingBase2);
				checkboxCell.Tapped += this.root.SettingsCheckBoxCellPatched_Tapped;
				return checkboxCell;
			}

			// Token: 0x04002452 RID: 9298
			internal object[] parentValues;

			// Token: 0x04002453 RID: 9299
			internal LiveDataMultiChartSelectorPageV3 root;
		}
	}
}
