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
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
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
	// Token: 0x02000639 RID: 1593
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\LiveDataMultiChartSelector.xaml")]
	public class LiveDataMultiChartSelector : ContentPage
	{
		// Token: 0x06003768 RID: 14184 RVA: 0x0029A0E8 File Offset: 0x002982E8
		public LiveDataMultiChartSelector()
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
			this.lv.BindingContext = this;
		}

		// Token: 0x06003769 RID: 14185 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600376A RID: 14186 RVA: 0x0029A1EC File Offset: 0x002983EC
		private async void Page_Appearing(object sender, EventArgs e)
		{
			MainAppRequestProducer.Delegate = null;
			RequestProducerStatic.UpdateOBDReaderRequests();
			foreach (LiveDataPIDModel liveDataPIDModel in this.CacheModelsList)
			{
				liveDataPIDModel.ClearResources();
			}
			this.CacheModelsList.Clear();
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x0600376B RID: 14187 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600376C RID: 14188 RVA: 0x0029A224 File Offset: 0x00298424
		private void GeneratePidList()
		{
			foreach (PID pid in LiveDataPIDModel._PIDCollection)
			{
				if (pid is IPIDFloatValue)
				{
					this.PidList.Add(new PIDProxy(pid));
				}
			}
			this.FilteredPidList.Reset(this.PidList);
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

		// Token: 0x0600376D RID: 14189 RVA: 0x0029A354 File Offset: 0x00298554
		private void SetSelectedCount()
		{
			this.labelSelectedCount.Text = this.PidList.Count((PIDProxy x) => x.IsVisible).ToString();
		}

		// Token: 0x0600376E RID: 14190 RVA: 0x0029A3A0 File Offset: 0x002985A0
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

		// Token: 0x17001383 RID: 4995
		// (get) Token: 0x0600376F RID: 14191 RVA: 0x0029A3D7 File Offset: 0x002985D7
		// (set) Token: 0x06003770 RID: 14192 RVA: 0x0029A3DF File Offset: 0x002985DF
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

		// Token: 0x06003771 RID: 14193 RVA: 0x0029A3FC File Offset: 0x002985FC
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				IEnumerable<PIDProxy> enumerable = this.Sort(this.PidList);
				this.FilteredPidList.Reset(enumerable);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<PIDProxy> enumerable2 = this.PidList;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable2 = enumerable2.Where((PIDProxy x) => (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			IEnumerable<PIDProxy> enumerable3 = this.Sort(enumerable2);
			this.FilteredPidList.Reset(enumerable3);
		}

		// Token: 0x06003772 RID: 14194 RVA: 0x0029A4AC File Offset: 0x002986AC
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

		// Token: 0x06003773 RID: 14195 RVA: 0x0029A50C File Offset: 0x0029870C
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

		// Token: 0x06003774 RID: 14196 RVA: 0x0029A5D0 File Offset: 0x002987D0
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

		// Token: 0x06003775 RID: 14197 RVA: 0x0029A658 File Offset: 0x00298858
		private void Handle_Toggled(object sender, ToggledEventArgs e)
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
		}

		// Token: 0x06003776 RID: 14198 RVA: 0x0029A6F4 File Offset: 0x002988F4
		private void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.ItemData != null)
			{
				PIDProxy pidproxy = e.ItemData as PIDProxy;
				pidproxy.IsVisible = !pidproxy.IsVisible;
				this.lv.SelectedItem = null;
			}
			PlatformHelper.CommonService.HideKeyboard();
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
		}

		// Token: 0x17001384 RID: 4996
		// (get) Token: 0x06003777 RID: 14199 RVA: 0x0029A7C4 File Offset: 0x002989C4
		public SmartCollection<PIDProxy> FilteredPidList
		{
			get
			{
				return this._FilteredPidList;
			}
		}

		// Token: 0x17001385 RID: 4997
		// (get) Token: 0x06003778 RID: 14200 RVA: 0x0029A7CC File Offset: 0x002989CC
		// (set) Token: 0x06003779 RID: 14201 RVA: 0x0029A7D4 File Offset: 0x002989D4
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

		// Token: 0x0600377A RID: 14202 RVA: 0x0029A7E0 File Offset: 0x002989E0
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

		// Token: 0x0600377B RID: 14203 RVA: 0x0029A817 File Offset: 0x00298A17
		private void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				PIDProxy pidproxy = e.SelectedItem as PIDProxy;
				pidproxy.IsVisible = !pidproxy.IsVisible;
				this.lv.SelectedItem = null;
			}
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x0600377C RID: 14204 RVA: 0x0029A850 File Offset: 0x00298A50
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LiveDataMultiChartSelector.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
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
			this.searchBar = entry;
			this.lv = sfListView;
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid.Children.Add(entry);
			sfListView.SetValue(Grid.RowProperty, 1);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Lv_ItemTapped);
			bindingExtension.Path = "FilteredPidList";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase);
			sfListView.SetValue(SfListView.SelectionBackgroundColorProperty, Color.Transparent);
			sfListView.SetValue(SfListView.SelectionModeProperty, 0);
			IDataTemplate dataTemplate2 = dataTemplate;
			LiveDataMultiChartSelector.<InitializeComponent>_anonXamlCDataTemplate_65 <InitializeComponent>_anonXamlCDataTemplate_ = new LiveDataMultiChartSelector.<InitializeComponent>_anonXamlCDataTemplate_65();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = sfListView;
			array3[2] = grid;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(sfListView);
			stackLayout.SetValue(Grid.RowProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = grid;
			array4[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 21)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			translate2.Text = "ios_MultiChartSelectedCount";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = label;
			array5[1] = stackLayout;
			array5[2] = grid;
			array5[3] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 21)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.Text = obj6;
			stackLayout.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label2;
			array6[1] = stackLayout;
			array6[2] = grid;
			array6[3] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LiveDataMultiChartSelector).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 21)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
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

		// Token: 0x0600377D RID: 14205 RVA: 0x0029B7C4 File Offset: 0x002999C4
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			foreach (PIDProxy pidproxy in this.PidList)
			{
				pidproxy.IsVisible = true;
			}
			this.SetSelectedCount();
		}

		// Token: 0x0600377E RID: 14206 RVA: 0x0029B818 File Offset: 0x00299A18
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			foreach (PIDProxy pidproxy in this.PidList)
			{
				pidproxy.IsVisible = false;
			}
			this.SetSelectedCount();
		}

		// Token: 0x0600377F RID: 14207 RVA: 0x0029B86C File Offset: 0x00299A6C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LiveDataMultiChartSelector>(this, typeof(LiveDataMultiChartSelector));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.labelSelectedCount = NameScopeExtensions.FindByName<Label>(this, "labelSelectedCount");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x04002189 RID: 8585
		private string _Filter = "";

		// Token: 0x0400218A RID: 8586
		private string filtertext = "";

		// Token: 0x0400218B RID: 8587
		private ObservableCollection<PIDProxy> PidList = new ObservableCollection<PIDProxy>();

		// Token: 0x0400218C RID: 8588
		private SmartCollection<PIDProxy> _FilteredPidList = new SmartCollection<PIDProxy>();

		// Token: 0x0400218D RID: 8589
		[CompilerGenerated]
		private List<LiveDataPIDModel> <CacheModelsList>k__BackingField;

		// Token: 0x0400218E RID: 8590
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x0400218F RID: 8591
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x04002190 RID: 8592
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelSelectedCount;

		// Token: 0x04002191 RID: 8593
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x04002192 RID: 8594
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x0200063A RID: 1594
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003780 RID: 14208 RVA: 0x0029B8DF File Offset: 0x00299ADF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003781 RID: 14209 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003782 RID: 14210 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <SetSelectedCount>b__5_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003783 RID: 14211 RVA: 0x0029B8F3 File Offset: 0x00299AF3
			internal string <Sort>b__14_0(PIDProxy x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003784 RID: 14212 RVA: 0x0029B8F3 File Offset: 0x00299AF3
			internal string <Sort>b__14_1(PIDProxy x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003785 RID: 14213 RVA: 0x00002076 File Offset: 0x00000276
			internal bool <Sort>b__14_2(PIDProxy x)
			{
				return false;
			}

			// Token: 0x06003786 RID: 14214 RVA: 0x0029B900 File Offset: 0x00299B00
			internal int <Sort>b__14_3(PIDProxy x)
			{
				return x.Pid.Id;
			}

			// Token: 0x06003787 RID: 14215 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <SaveSelected>b__15_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003788 RID: 14216 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <Handle_Toggled>b__16_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003789 RID: 14217 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <Lv_ItemTapped>b__17_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600378A RID: 14218 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <btnOK_Clicked>b__26_0(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600378B RID: 14219 RVA: 0x0029B90D File Offset: 0x00299B0D
			internal PID <btnOK_Clicked>b__26_1(PIDProxy x)
			{
				return x.Pid;
			}

			// Token: 0x0600378C RID: 14220 RVA: 0x0029B8EB File Offset: 0x00299AEB
			internal bool <btnOK_Clicked>b__26_2(PIDProxy x)
			{
				return x.IsVisible;
			}

			// Token: 0x0600378D RID: 14221 RVA: 0x0004C724 File Offset: 0x0004A924
			internal IPIDFloatValue <btnOK_Clicked>b__26_3(PID x)
			{
				return (IPIDFloatValue)x;
			}

			// Token: 0x0600378E RID: 14222 RVA: 0x00298DCC File Offset: 0x00296FCC
			internal UnitsHelper.Units <btnOK_Clicked>b__26_4(IPIDFloatValue x)
			{
				return x.Units;
			}

			// Token: 0x04002193 RID: 8595
			public static readonly LiveDataMultiChartSelector.<>c <>9 = new LiveDataMultiChartSelector.<>c();

			// Token: 0x04002194 RID: 8596
			public static Func<PIDProxy, bool> <>9__5_0;

			// Token: 0x04002195 RID: 8597
			public static Func<PIDProxy, string> <>9__14_0;

			// Token: 0x04002196 RID: 8598
			public static Func<PIDProxy, string> <>9__14_1;

			// Token: 0x04002197 RID: 8599
			public static Func<PIDProxy, bool> <>9__14_2;

			// Token: 0x04002198 RID: 8600
			public static Func<PIDProxy, int> <>9__14_3;

			// Token: 0x04002199 RID: 8601
			public static Func<PIDProxy, bool> <>9__15_0;

			// Token: 0x0400219A RID: 8602
			public static Func<PIDProxy, bool> <>9__16_0;

			// Token: 0x0400219B RID: 8603
			public static Func<PIDProxy, bool> <>9__17_0;

			// Token: 0x0400219C RID: 8604
			public static Func<PIDProxy, bool> <>9__26_0;

			// Token: 0x0400219D RID: 8605
			public static Func<PIDProxy, PID> <>9__26_1;

			// Token: 0x0400219E RID: 8606
			public static Func<PIDProxy, bool> <>9__26_2;

			// Token: 0x0400219F RID: 8607
			public static Func<PID, IPIDFloatValue> <>9__26_3;

			// Token: 0x040021A0 RID: 8608
			public static Func<IPIDFloatValue, UnitsHelper.Units> <>9__26_4;
		}

		// Token: 0x0200063B RID: 1595
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x0600378F RID: 14223 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06003790 RID: 14224 RVA: 0x0029B918 File Offset: 0x00299B18
			internal bool <UpdateFilter>b__0(PIDProxy x)
			{
				return (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x040021A1 RID: 8609
			public string word;
		}

		// Token: 0x0200063C RID: 1596
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06003791 RID: 14225 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06003792 RID: 14226 RVA: 0x0029B984 File Offset: 0x00299B84
			internal bool <GeneratePidList>b__0(PIDProxy x)
			{
				return x.Pid.Id == this.id;
			}

			// Token: 0x040021A2 RID: 8610
			public int id;
		}

		// Token: 0x0200063D RID: 1597
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003793 RID: 14227 RVA: 0x0029B99C File Offset: 0x00299B9C
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				LiveDataMultiChartSelector liveDataMultiChartSelector = this;
				try
				{
					MainAppRequestProducer.Delegate = null;
					RequestProducerStatic.UpdateOBDReaderRequests();
					List<LiveDataPIDModel>.Enumerator enumerator = liveDataMultiChartSelector.CacheModelsList.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							LiveDataPIDModel liveDataPIDModel = enumerator.Current;
							liveDataPIDModel.ClearResources();
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					liveDataMultiChartSelector.CacheModelsList.Clear();
					RequestProducerStatic.UpdateOBDReaderRequests();
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003794 RID: 14228 RVA: 0x0029BA54 File Offset: 0x00299C54
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021A3 RID: 8611
			public int <>1__state;

			// Token: 0x040021A4 RID: 8612
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021A5 RID: 8613
			public LiveDataMultiChartSelector <>4__this;
		}

		// Token: 0x0200063E RID: 1598
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__26 : IAsyncStateMachine
		{
			// Token: 0x06003795 RID: 14229 RVA: 0x0029BA64 File Offset: 0x00299C64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataMultiChartSelector liveDataMultiChartSelector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							liveDataMultiChartSelector.btnOK.IsEnabled = false;
							List<PID> list = (from x in liveDataMultiChartSelector.PidList
								where x.IsVisible
								select x.Pid).ToList<PID>();
							if (list.Count <= 0)
							{
								goto IL_0244;
							}
							if (list.Count > 2 && !SharedSettings.Current.AdsProductPurchased)
							{
								string text = Translate.GetString("ios_Limit2");
								text = string.Format(text, liveDataMultiChartSelector.PidList.Count((PIDProxy x) => x.IsVisible).ToString());
								liveDataMultiChartSelector.DisplayAlert("Car Scanner Pro", text, "OK");
								liveDataMultiChartSelector.btnOK.IsEnabled = true;
								goto IL_026B;
							}
							liveDataMultiChartSelector.SaveSelected();
							if ((from x in list
								select (IPIDFloatValue)x into x
								select x.Units).Distinct<UnitsHelper.Units>().Count<UnitsHelper.Units>() == 2)
							{
								LiveDataMultiChartPageV2 liveDataMultiChartPageV = new LiveDataMultiChartPageV2(list.Cast<IPIDFloatValue>().ToList<IPIDFloatValue>(), liveDataMultiChartSelector);
								taskAwaiter = liveDataMultiChartSelector.Navigation.PushAsync(liveDataMultiChartPageV).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelector.<btnOK_Clicked>d__26>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_01D1;
							}
							else
							{
								LiveMultiChartPage liveMultiChartPage = new LiveMultiChartPage(list);
								taskAwaiter = liveDataMultiChartSelector.Navigation.PushAsync(liveMultiChartPage).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelector.<btnOK_Clicked>d__26>(ref taskAwaiter, ref this);
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
					liveDataMultiChartSelector.btnOK.IsEnabled = true;
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

			// Token: 0x06003796 RID: 14230 RVA: 0x0029BD0C File Offset: 0x00299F0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021A6 RID: 8614
			public int <>1__state;

			// Token: 0x040021A7 RID: 8615
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021A8 RID: 8616
			public LiveDataMultiChartSelector <>4__this;

			// Token: 0x040021A9 RID: 8617
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200063F RID: 1599
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__6 : IAsyncStateMachine
		{
			// Token: 0x06003797 RID: 14231 RVA: 0x0029BD1C File Offset: 0x00299F1C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LiveDataMultiChartSelector liveDataMultiChartSelector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						liveDataMultiChartSelector.filtertext = liveDataMultiChartSelector.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LiveDataMultiChartSelector.<searchBar_TextChanged>d__6>(ref taskAwaiter, ref this);
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
					if (liveDataMultiChartSelector.filtertext == liveDataMultiChartSelector.searchBar.Text)
					{
						try
						{
							liveDataMultiChartSelector.Filter = liveDataMultiChartSelector.filtertext;
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

			// Token: 0x06003798 RID: 14232 RVA: 0x0029BE18 File Offset: 0x0029A018
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040021AA RID: 8618
			public int <>1__state;

			// Token: 0x040021AB RID: 8619
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040021AC RID: 8620
			public LiveDataMultiChartSelector <>4__this;

			// Token: 0x040021AD RID: 8621
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000640 RID: 1600
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_65
		{
			// Token: 0x06003799 RID: 14233 RVA: 0x0029BE28 File Offset: 0x0029A028
			public <InitializeComponent>_anonXamlCDataTemplate_65()
			{
			}

			// Token: 0x0600379A RID: 14234 RVA: 0x0029BE3C File Offset: 0x0029A03C
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 34);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 34);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 33);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 33);
				CheckSwitch checkSwitch;
				VisualDiagnostics.RegisterSourceInfo(checkSwitch = new CheckSwitch(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LiveDataMultiChartSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0));
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Pid.Name";
				bindingExtension.TypedBinding = new TypedBinding<PIDProxy, string>(delegate(PIDProxy A_0)
				{
					if (A_0 != null)
					{
						PID pid = A_0.Pid;
						if (pid != null)
						{
							return new ValueTuple<string, bool>(pid.Name, true);
						}
					}
					return default(ValueTuple<string, bool>);
				}, delegate(PIDProxy A_0, string A_1)
				{
					if (A_0 != null)
					{
						PID pid2 = A_0.Pid;
						if (pid2 != null)
						{
							pid2.Name = A_1;
							return;
						}
					}
				}, new Tuple<Func<PIDProxy, object>, string>[]
				{
					new Tuple<Func<PIDProxy, object>, string>((PIDProxy A_0) => A_0, "Pid"),
					new Tuple<Func<PIDProxy, object>, string>((PIDProxy A_0) => A_0.Pid, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				grid.Children.Add(label);
				checkSwitch.SetValue(Grid.RowProperty, 0);
				checkSwitch.SetValue(Grid.ColumnProperty, 1);
				checkSwitch.SetValue(View.MarginProperty, new Thickness(5.0));
				checkSwitch.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				bindingExtension2.Mode = 1;
				bindingExtension2.Path = "IsVisible";
				bindingExtension2.TypedBinding = new TypedBinding<PIDProxy, bool>(delegate(PIDProxy A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.IsVisible, true);
					}
					return default(ValueTuple<bool, bool>);
				}, delegate(PIDProxy A_0, bool A_1)
				{
					if (A_0 != null)
					{
						A_0.IsVisible = A_1;
						return;
					}
				}, new Tuple<Func<PIDProxy, object>, string>[]
				{
					new Tuple<Func<PIDProxy, object>, string>((PIDProxy A_0) => A_0, "IsVisible")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				checkSwitch.SetBinding(CheckSwitch.IsToggledProperty, bindingBase2);
				checkSwitch.Toggled += this.root.Handle_Toggled;
				checkSwitch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				grid.Children.Add(checkSwitch);
				frame.SetValue(Grid.RowProperty, 1);
				frame.SetValue(Grid.ColumnProperty, 0);
				frame.SetValue(Grid.ColumnSpanProperty, 2);
				frame.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
				dynamicResourceExtension.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = frame;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("pages", "clr-namespace:CarScannerXamarinForms.Pages");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LiveDataMultiChartSelector.<InitializeComponent>_anonXamlCDataTemplate_65).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource.Key);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x0600379B RID: 14235 RVA: 0x0029C510 File Offset: 0x0029A710
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1375(PIDProxy A_0)
			{
				if (A_0 != null)
				{
					PID pid = A_0.Pid;
					if (pid != null)
					{
						return new ValueTuple<string, bool>(pid.Name, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600379C RID: 14236 RVA: 0x0029C548 File Offset: 0x0029A748
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1376(PIDProxy A_0, string A_1)
			{
				if (A_0 != null)
				{
					PID pid = A_0.Pid;
					if (pid != null)
					{
						pid.Name = A_1;
						return;
					}
				}
			}

			// Token: 0x0600379D RID: 14237 RVA: 0x0029C570 File Offset: 0x0029A770
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1377(PIDProxy A_0)
			{
				return A_0;
			}

			// Token: 0x0600379E RID: 14238 RVA: 0x0029C580 File Offset: 0x0029A780
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1378(PIDProxy A_0)
			{
				return A_0.Pid;
			}

			// Token: 0x0600379F RID: 14239 RVA: 0x0029C594 File Offset: 0x0029A794
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1379(PIDProxy A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x060037A0 RID: 14240 RVA: 0x0029C5C4 File Offset: 0x0029A7C4
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1380(PIDProxy A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.IsVisible = A_1;
					return;
				}
			}

			// Token: 0x060037A1 RID: 14241 RVA: 0x0029C5E0 File Offset: 0x0029A7E0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1381(PIDProxy A_0)
			{
				return A_0;
			}

			// Token: 0x040021AE RID: 8622
			internal object[] parentValues;

			// Token: 0x040021AF RID: 8623
			internal LiveDataMultiChartSelector root;
		}
	}
}
