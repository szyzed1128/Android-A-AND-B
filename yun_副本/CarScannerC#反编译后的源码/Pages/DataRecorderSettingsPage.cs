using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x020005EF RID: 1519
	[XamlFilePath("DataRecorder\\DataRecorderSettingsPage.xaml")]
	public class DataRecorderSettingsPage : ContentPage
	{
		// Token: 0x0600360C RID: 13836 RVA: 0x0026E76C File Offset: 0x0026C96C
		public DataRecorderSettingsPage()
		{
			this.InitializeComponent();
			base.BindingContextChanged += this.DataRecorderSettingsPage_BindingContextChanged;
			this.btnOK.TextColor = Color.White;
			this.btnOK.BackgroundColor = Color.DarkGray;
			this.btnOK.IsEnabled = false;
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_check"], delegate
			{
				foreach (DataRecord dataRecord in (base.BindingContext as IDataRecordContainer).Records)
				{
					dataRecord.IsVisible = true;
				}
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_uncheck"], delegate
			{
				foreach (DataRecord dataRecord2 in (base.BindingContext as IDataRecordContainer).Records)
				{
					dataRecord2.IsVisible = false;
				}
			}, 0, 0));
		}

		// Token: 0x0600360D RID: 13837 RVA: 0x0026E868 File Offset: 0x0026CA68
		private void DataRecorderSettingsPage_BindingContextChanged(object sender, EventArgs e)
		{
			if (base.BindingContext == null)
			{
				this.PidList.Clear();
				this.UpdateFilter();
				return;
			}
			this.PidList.Clear();
			foreach (DataRecord dataRecord in ((IDataRecordContainer)base.BindingContext).Records)
			{
				this.PidList.Add(dataRecord);
			}
			this.UpdateFilter();
		}

		// Token: 0x17001378 RID: 4984
		// (get) Token: 0x0600360E RID: 13838 RVA: 0x0026E8F8 File Offset: 0x0026CAF8
		public SmartCollection<DataRecord> FilteredPidList
		{
			get
			{
				return this._FilteredPidList;
			}
		}

		// Token: 0x0600360F RID: 13839 RVA: 0x0026E900 File Offset: 0x0026CB00
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

		// Token: 0x17001379 RID: 4985
		// (get) Token: 0x06003610 RID: 13840 RVA: 0x0026E937 File Offset: 0x0026CB37
		// (set) Token: 0x06003611 RID: 13841 RVA: 0x0026E93F File Offset: 0x0026CB3F
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

		// Token: 0x06003612 RID: 13842 RVA: 0x0026E95C File Offset: 0x0026CB5C
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				this.FilteredPidList.Reset(this.PidList);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<DataRecord> enumerable = this.PidList;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable = enumerable.Where((DataRecord x) => (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			this.FilteredPidList.Reset(enumerable);
		}

		// Token: 0x06003613 RID: 13843 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06003614 RID: 13844 RVA: 0x0026E9F4 File Offset: 0x0026CBF4
		private void Page_Appearing(object sender, EventArgs e)
		{
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06003615 RID: 13845 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06003616 RID: 13846 RVA: 0x0026EA04 File Offset: 0x0026CC04
		private void Handle_Toggled(object sender, ToggledEventArgs e)
		{
			if ((base.BindingContext as IDataRecordContainer).Records.Any((DataRecord x) => x.IsVisible))
			{
				this.btnOK.IsEnabled = true;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.Green;
				return;
			}
			this.btnOK.IsEnabled = false;
			this.btnOK.TextColor = Color.White;
			this.btnOK.BackgroundColor = Color.DarkGray;
		}

		// Token: 0x06003617 RID: 13847 RVA: 0x0026EAA0 File Offset: 0x0026CCA0
		private async Task GoToMultiChart()
		{
			IDataRecordContainer rec = base.BindingContext as IDataRecordContainer;
			bool show = false;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				show = true;
			}
			else
			{
				int num = rec.Records.Count((DataRecord x) => x.IsVisible);
				if (num <= 2)
				{
					show = true;
				}
				else
				{
					string text = Translate.GetString("ios_Limit2");
					text = string.Format(text, num.ToString());
					await base.DisplayAlert("Car Scanner Pro", text, "OK");
				}
			}
			if (show)
			{
				DataRecorderViewerPage dataRecorderViewerPage = new DataRecorderViewerPage(rec);
				await base.Navigation.PushAsync(dataRecorderViewerPage);
			}
		}

		// Token: 0x06003618 RID: 13848 RVA: 0x0026EAE4 File Offset: 0x0026CCE4
		private async Task GoToOneChart()
		{
			IDataRecordContainer rec = base.BindingContext as IDataRecordContainer;
			bool show = false;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				show = true;
			}
			else
			{
				int num = rec.Records.Count((DataRecord x) => x.IsVisible);
				if (num <= 2)
				{
					show = true;
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("ios_DataRecorderFreeLimitTitle"), string.Format(Translate.GetString("ios_DataRecorderFreeLimitText"), num), "OK");
				}
			}
			if (show)
			{
				DataRecorderSingleChartViewerPage dataRecorderSingleChartViewerPage = new DataRecorderSingleChartViewerPage(rec);
				await base.Navigation.PushAsync(dataRecorderSingleChartViewerPage);
			}
		}

		// Token: 0x06003619 RID: 13849 RVA: 0x0026EB27 File Offset: 0x0026CD27
		private void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				DataRecord dataRecord = e.SelectedItem as DataRecord;
				dataRecord.IsVisible = !dataRecord.IsVisible;
				this.lv.SelectedItem = null;
			}
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x0026EB58 File Offset: 0x0026CD58
		private async void btnDTC_Clicked(object sender, EventArgs e)
		{
			this.btnDTC.IsEnabled = false;
			IDataRecordContainer dataRecordContainer = base.BindingContext as IDataRecordContainer;
			foreach (DTCItemV2 dtcitemV in dataRecordContainer.DTCs)
			{
				if (dtcitemV.Descriptions.Count == 0 && !dtcitemV.IsVAG)
				{
					dtcitemV.LoadDescription();
				}
			}
			DataRecorderDTCPage dataRecorderDTCPage = new DataRecorderDTCPage(dataRecordContainer.Title, dataRecordContainer.DTCs);
			await base.Navigation.PushAsync(dataRecorderDTCPage);
			this.btnDTC.IsEnabled = true;
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0026EB90 File Offset: 0x0026CD90
		private async void btnSpeedTest_Clicked(object sender, EventArgs e)
		{
			DataRecorderSpeedTestPage dataRecorderSpeedTestPage = new DataRecorderSpeedTestPage(base.BindingContext as IDataRecordContainer);
			await base.Navigation.PushAsync(dataRecorderSpeedTestPage);
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x0026EBC8 File Offset: 0x0026CDC8
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			IDataRecordContainer rec = base.BindingContext as IDataRecordContainer;
			bool show = false;
			if (SharedSettings.Current.AdsProductPurchased)
			{
				show = true;
			}
			else
			{
				int num = rec.Records.Count((DataRecord x) => x.IsVisible);
				if (num <= 2)
				{
					show = true;
				}
				else
				{
					string text = Translate.GetString("ios_Limit2");
					text = string.Format(text, num.ToString());
					await base.DisplayAlert("Car Scanner Pro", text, "OK");
				}
			}
			if (show)
			{
				this.btnOK.IsEnabled = false;
				string separateChart = Translate.GetString("ios_ShowRecordOneChart.Content");
				string multiChart = Translate.GetString("ios_ShowRecordMultiCharts.Content");
				string map = Translate.GetString("records_MapWithChart");
				string alertResult = null;
				if (rec.HasGeolocationData)
				{
					alertResult = await this.DisplayActionSheetCustom("", Translate.GetString("btnCancel.Content"), null, new string[] { separateChart, multiChart, map });
				}
				else
				{
					alertResult = await this.DisplayActionSheetCustom("", Translate.GetString("btnCancel.Content"), null, new string[] { separateChart, multiChart });
				}
				this.activityFrame.IsVisible = true;
				await Task.Delay(100);
				if (alertResult == separateChart)
				{
					await this.GoToOneChart();
				}
				else if (alertResult == multiChart)
				{
					await this.GoToMultiChart();
				}
				else if (alertResult == map)
				{
					MapWithMultiChartV2 mapWithMultiChartV = new MapWithMultiChartV2(rec, (rec as DataRecordContainer).Positions);
					await base.Navigation.PushAsync(mapWithMultiChartV);
				}
				this.activityFrame.IsVisible = false;
				this.btnOK.IsEnabled = true;
				separateChart = null;
				multiChart = null;
				map = null;
				alertResult = null;
			}
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0026EC00 File Offset: 0x0026CE00
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DataRecorderSettingsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/DataRecorderSettingsPage.xaml",
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
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 22);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 14);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 21);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			nameScope.RegisterName("gridButtons", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridButtons";
			}
			nameScope.RegisterName("btnDTC", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnDTC";
			}
			nameScope.RegisterName("btnSpeedTest", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnSpeedTest";
			}
			this.page = this;
			this.searchBar = entry;
			this.lv = listView;
			this.activityFrame = activityFrame;
			this.btnOK = button;
			this.gridButtons = grid2;
			this.btnDTC = button2;
			this.btnSpeedTest = button3;
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 5.0, 5.0, 5.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Page_Appearing;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataRecorderSettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid3.Children.Add(entry);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension2 = referenceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = listView;
			array2[1] = grid3;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DataRecorderSettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			listView.SetValue(BindableObject.BindingContextProperty, obj3);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Handle_ItemSelected;
			bindingExtension2.Path = "FilteredPidList";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase2);
			IDataTemplate dataTemplate2 = dataTemplate;
			DataRecorderSettingsPage.<InitializeComponent>_anonXamlCDataTemplate_17 <InitializeComponent>_anonXamlCDataTemplate_ = new DataRecorderSettingsPage.<InitializeComponent>_anonXamlCDataTemplate_17();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid3;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid3.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid3.Children.Add(activityFrame);
			grid.SetValue(Grid.RowProperty, 2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.SetValue(Grid.ColumnSpanProperty, 2);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(VisualElement.IsEnabledProperty, false);
			button.SetValue(Button.TextProperty, "OK");
			grid.Children.Add(button);
			grid3.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 3);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.SetValue(VisualElement.BackgroundColorProperty, Color.Green);
			button2.Clicked += this.btnDTC_Clicked;
			bindingExtension3.Path = "HasDTC";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			button2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			translate.Text = "ios_MainPage_TileDtcErrors";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = button2;
			array4[1] = grid2;
			array4[2] = grid3;
			array4[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DataRecorderSettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			button2.Text = obj5;
			button2.SetValue(Button.TextColorProperty, Color.White);
			grid2.Children.Add(button2);
			button3.SetValue(Grid.ColumnProperty, 1);
			button3.SetValue(VisualElement.BackgroundColorProperty, Color.Green);
			button3.Clicked += this.btnSpeedTest_Clicked;
			bindingExtension4.Path = "HasSpeedTests";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			button3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			translate2.Text = "ios_MainPage_TileSpeedTest";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = button3;
			array5[1] = grid2;
			array5[2] = grid3;
			array5[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DataRecorderSettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button3.Text = obj7;
			button3.SetValue(Button.TextColorProperty, Color.White);
			grid2.Children.Add(button3);
			grid3.Children.Add(grid2);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0026FBBC File Offset: 0x0026DDBC
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			foreach (DataRecord dataRecord in (base.BindingContext as IDataRecordContainer).Records)
			{
				dataRecord.IsVisible = true;
			}
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0026FC18 File Offset: 0x0026DE18
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			foreach (DataRecord dataRecord in (base.BindingContext as IDataRecordContainer).Records)
			{
				dataRecord.IsVisible = false;
			}
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0026FC74 File Offset: 0x0026DE74
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DataRecorderSettingsPage>(this, typeof(DataRecorderSettingsPage));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnDTC = NameScopeExtensions.FindByName<Button>(this, "btnDTC");
			this.btnSpeedTest = NameScopeExtensions.FindByName<Button>(this, "btnSpeedTest");
		}

		// Token: 0x04002040 RID: 8256
		private ObservableCollection<DataRecord> PidList = new ObservableCollection<DataRecord>();

		// Token: 0x04002041 RID: 8257
		private SmartCollection<DataRecord> _FilteredPidList = new SmartCollection<DataRecord>();

		// Token: 0x04002042 RID: 8258
		private string _Filter = "";

		// Token: 0x04002043 RID: 8259
		private string filtertext = "";

		// Token: 0x04002044 RID: 8260
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002045 RID: 8261
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002046 RID: 8262
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002047 RID: 8263
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04002048 RID: 8264
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x04002049 RID: 8265
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x0400204A RID: 8266
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDTC;

		// Token: 0x0400204B RID: 8267
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSpeedTest;

		// Token: 0x020005F0 RID: 1520
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003621 RID: 13857 RVA: 0x0026FD1A File Offset: 0x0026DF1A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003622 RID: 13858 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003623 RID: 13859 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <Handle_Toggled>b__16_0(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003624 RID: 13860 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <GoToMultiChart>b__17_0(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003625 RID: 13861 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <GoToOneChart>b__18_0(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x06003626 RID: 13862 RVA: 0x0026FD26 File Offset: 0x0026DF26
			internal bool <btnOK_Clicked>b__22_0(DataRecord x)
			{
				return x.IsVisible;
			}

			// Token: 0x0400204C RID: 8268
			public static readonly DataRecorderSettingsPage.<>c <>9 = new DataRecorderSettingsPage.<>c();

			// Token: 0x0400204D RID: 8269
			public static Func<DataRecord, bool> <>9__16_0;

			// Token: 0x0400204E RID: 8270
			public static Func<DataRecord, bool> <>9__17_0;

			// Token: 0x0400204F RID: 8271
			public static Func<DataRecord, bool> <>9__18_0;

			// Token: 0x04002050 RID: 8272
			public static Func<DataRecord, bool> <>9__22_0;
		}

		// Token: 0x020005F1 RID: 1521
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x06003627 RID: 13863 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x06003628 RID: 13864 RVA: 0x0026FD30 File Offset: 0x0026DF30
			internal bool <UpdateFilter>b__0(DataRecord x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002051 RID: 8273
			public string word;
		}

		// Token: 0x020005F2 RID: 1522
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GoToMultiChart>d__17 : IAsyncStateMachine
		{
			// Token: 0x06003629 RID: 13865 RVA: 0x0026FD88 File Offset: 0x0026DF88
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
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
							goto IL_017D;
						}
						rec = dataRecorderSettingsPage.BindingContext as IDataRecordContainer;
						show = false;
						if (SharedSettings.Current.AdsProductPurchased)
						{
							show = true;
							goto IL_010D;
						}
						int num3 = rec.Records.Count((DataRecord x) => x.IsVisible);
						if (num3 <= 2)
						{
							show = true;
							goto IL_010D;
						}
						string text = Translate.GetString("ios_Limit2");
						text = string.Format(text, num3.ToString());
						taskAwaiter = dataRecorderSettingsPage.DisplayAlert("Car Scanner Pro", text, "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<GoToMultiChart>d__17>(ref taskAwaiter, ref this);
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
					IL_010D:
					if (!show)
					{
						goto IL_0184;
					}
					DataRecorderViewerPage dataRecorderViewerPage = new DataRecorderViewerPage(rec);
					taskAwaiter = dataRecorderSettingsPage.Navigation.PushAsync(dataRecorderViewerPage).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<GoToMultiChart>d__17>(ref taskAwaiter, ref this);
						return;
					}
					IL_017D:
					taskAwaiter.GetResult();
					IL_0184:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					rec = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				rec = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600362A RID: 13866 RVA: 0x0026FF74 File Offset: 0x0026E174
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002052 RID: 8274
			public int <>1__state;

			// Token: 0x04002053 RID: 8275
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002054 RID: 8276
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x04002055 RID: 8277
			private IDataRecordContainer <rec>5__2;

			// Token: 0x04002056 RID: 8278
			private bool <show>5__3;

			// Token: 0x04002057 RID: 8279
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005F3 RID: 1523
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GoToOneChart>d__18 : IAsyncStateMachine
		{
			// Token: 0x0600362B RID: 13867 RVA: 0x0026FF84 File Offset: 0x0026E184
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
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
							goto IL_0174;
						}
						rec = dataRecorderSettingsPage.BindingContext as IDataRecordContainer;
						show = false;
						if (SharedSettings.Current.AdsProductPurchased)
						{
							show = true;
							goto IL_0107;
						}
						int num3 = rec.Records.Count((DataRecord x) => x.IsVisible);
						if (num3 <= 2)
						{
							show = true;
							goto IL_0107;
						}
						taskAwaiter = dataRecorderSettingsPage.DisplayAlert(Translate.GetString("ios_DataRecorderFreeLimitTitle"), string.Format(Translate.GetString("ios_DataRecorderFreeLimitText"), num3), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<GoToOneChart>d__18>(ref taskAwaiter, ref this);
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
					IL_0107:
					if (!show)
					{
						goto IL_017B;
					}
					DataRecorderSingleChartViewerPage dataRecorderSingleChartViewerPage = new DataRecorderSingleChartViewerPage(rec);
					taskAwaiter = dataRecorderSettingsPage.Navigation.PushAsync(dataRecorderSingleChartViewerPage).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<GoToOneChart>d__18>(ref taskAwaiter, ref this);
						return;
					}
					IL_0174:
					taskAwaiter.GetResult();
					IL_017B:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					rec = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				rec = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600362C RID: 13868 RVA: 0x00270164 File Offset: 0x0026E364
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002058 RID: 8280
			public int <>1__state;

			// Token: 0x04002059 RID: 8281
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400205A RID: 8282
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x0400205B RID: 8283
			private IDataRecordContainer <rec>5__2;

			// Token: 0x0400205C RID: 8284
			private bool <show>5__3;

			// Token: 0x0400205D RID: 8285
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005F4 RID: 1524
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDTC_Clicked>d__20 : IAsyncStateMachine
		{
			// Token: 0x0600362D RID: 13869 RVA: 0x00270174 File Offset: 0x0026E374
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dataRecorderSettingsPage.btnDTC.IsEnabled = false;
						IDataRecordContainer dataRecordContainer = dataRecorderSettingsPage.BindingContext as IDataRecordContainer;
						List<DTCItemV2>.Enumerator enumerator = dataRecordContainer.DTCs.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								DTCItemV2 dtcitemV = enumerator.Current;
								if (dtcitemV.Descriptions.Count == 0 && !dtcitemV.IsVAG)
								{
									dtcitemV.LoadDescription();
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
						DataRecorderDTCPage dataRecorderDTCPage = new DataRecorderDTCPage(dataRecordContainer.Title, dataRecordContainer.DTCs);
						taskAwaiter = dataRecorderSettingsPage.Navigation.PushAsync(dataRecorderDTCPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnDTC_Clicked>d__20>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					dataRecorderSettingsPage.btnDTC.IsEnabled = true;
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

			// Token: 0x0600362E RID: 13870 RVA: 0x002702CC File Offset: 0x0026E4CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400205E RID: 8286
			public int <>1__state;

			// Token: 0x0400205F RID: 8287
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002060 RID: 8288
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x04002061 RID: 8289
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005F5 RID: 1525
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__22 : IAsyncStateMachine
		{
			// Token: 0x0600362F RID: 13871 RVA: 0x002702DC File Offset: 0x0026E4DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0208;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_029D;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0312;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0383;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03F9;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0496;
					}
					default:
					{
						rec = dataRecorderSettingsPage.BindingContext as IDataRecordContainer;
						show = false;
						if (SharedSettings.Current.AdsProductPurchased)
						{
							show = true;
							goto IL_0122;
						}
						int num3 = rec.Records.Count((DataRecord x) => x.IsVisible);
						if (num3 <= 2)
						{
							show = true;
							goto IL_0122;
						}
						string text = Translate.GetString("ios_Limit2");
						text = string.Format(text, num3.ToString());
						taskAwaiter = dataRecorderSettingsPage.DisplayAlert("Car Scanner Pro", text, "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					IL_0122:
					if (!show)
					{
						goto IL_04D1;
					}
					dataRecorderSettingsPage.btnOK.IsEnabled = false;
					separateChart = Translate.GetString("ios_ShowRecordOneChart.Content");
					multiChart = Translate.GetString("ios_ShowRecordMultiCharts.Content");
					map = Translate.GetString("records_MapWithChart");
					alertResult = null;
					if (rec.HasGeolocationData)
					{
						taskAwaiter3 = dataRecorderSettingsPage.DisplayActionSheetCustom("", Translate.GetString("btnCancel.Content"), null, new string[] { separateChart, multiChart, map }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = dataRecorderSettingsPage.DisplayActionSheetCustom("", Translate.GetString("btnCancel.Content"), null, new string[] { separateChart, multiChart }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_029D;
					}
					IL_0208:
					string text2 = taskAwaiter3.GetResult();
					alertResult = text2;
					goto IL_02AE;
					IL_029D:
					text2 = taskAwaiter3.GetResult();
					alertResult = text2;
					IL_02AE:
					dataRecorderSettingsPage.activityFrame.IsVisible = true;
					taskAwaiter = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter, ref this);
						return;
					}
					IL_0312:
					taskAwaiter.GetResult();
					if (alertResult == separateChart)
					{
						taskAwaiter = dataRecorderSettingsPage.GoToOneChart().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter, ref this);
							return;
						}
					}
					else if (alertResult == multiChart)
					{
						taskAwaiter = dataRecorderSettingsPage.GoToMultiChart().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_03F9;
					}
					else
					{
						if (!(alertResult == map))
						{
							goto IL_049D;
						}
						MapWithMultiChartV2 mapWithMultiChartV = new MapWithMultiChartV2(rec, (rec as DataRecordContainer).Positions);
						taskAwaiter = dataRecorderSettingsPage.Navigation.PushAsync(mapWithMultiChartV).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnOK_Clicked>d__22>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0496;
					}
					IL_0383:
					taskAwaiter.GetResult();
					goto IL_049D;
					IL_03F9:
					taskAwaiter.GetResult();
					goto IL_049D;
					IL_0496:
					taskAwaiter.GetResult();
					IL_049D:
					dataRecorderSettingsPage.activityFrame.IsVisible = false;
					dataRecorderSettingsPage.btnOK.IsEnabled = true;
					separateChart = null;
					multiChart = null;
					map = null;
					alertResult = null;
					IL_04D1:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					rec = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				rec = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003630 RID: 13872 RVA: 0x00270814 File Offset: 0x0026EA14
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002062 RID: 8290
			public int <>1__state;

			// Token: 0x04002063 RID: 8291
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002064 RID: 8292
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x04002065 RID: 8293
			private IDataRecordContainer <rec>5__2;

			// Token: 0x04002066 RID: 8294
			private bool <show>5__3;

			// Token: 0x04002067 RID: 8295
			private TaskAwaiter <>u__1;

			// Token: 0x04002068 RID: 8296
			private string <separateChart>5__4;

			// Token: 0x04002069 RID: 8297
			private string <multiChart>5__5;

			// Token: 0x0400206A RID: 8298
			private string <map1>5__6;

			// Token: 0x0400206B RID: 8299
			private string <alertResult>5__7;

			// Token: 0x0400206C RID: 8300
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x020005F6 RID: 1526
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSpeedTest_Clicked>d__21 : IAsyncStateMachine
		{
			// Token: 0x06003631 RID: 13873 RVA: 0x00270824 File Offset: 0x0026EA24
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						DataRecorderSpeedTestPage dataRecorderSpeedTestPage = new DataRecorderSpeedTestPage(dataRecorderSettingsPage.BindingContext as IDataRecordContainer);
						taskAwaiter = dataRecorderSettingsPage.Navigation.PushAsync(dataRecorderSpeedTestPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<btnSpeedTest_Clicked>d__21>(ref taskAwaiter, ref this);
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

			// Token: 0x06003632 RID: 13874 RVA: 0x002708F0 File Offset: 0x0026EAF0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400206D RID: 8301
			public int <>1__state;

			// Token: 0x0400206E RID: 8302
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400206F RID: 8303
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x04002070 RID: 8304
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005F7 RID: 1527
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__6 : IAsyncStateMachine
		{
			// Token: 0x06003633 RID: 13875 RVA: 0x00270900 File Offset: 0x0026EB00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataRecorderSettingsPage dataRecorderSettingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dataRecorderSettingsPage.filtertext = dataRecorderSettingsPage.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataRecorderSettingsPage.<searchBar_TextChanged>d__6>(ref taskAwaiter, ref this);
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
					if (dataRecorderSettingsPage.filtertext == dataRecorderSettingsPage.searchBar.Text)
					{
						try
						{
							dataRecorderSettingsPage.Filter = dataRecorderSettingsPage.filtertext;
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

			// Token: 0x06003634 RID: 13876 RVA: 0x002709FC File Offset: 0x0026EBFC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002071 RID: 8305
			public int <>1__state;

			// Token: 0x04002072 RID: 8306
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002073 RID: 8307
			public DataRecorderSettingsPage <>4__this;

			// Token: 0x04002074 RID: 8308
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005F8 RID: 1528
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_17
		{
			// Token: 0x06003635 RID: 13877 RVA: 0x00270A0C File Offset: 0x0026EC0C
			public <InitializeComponent>_anonXamlCDataTemplate_17()
			{
			}

			// Token: 0x06003636 RID: 13878 RVA: 0x00270A20 File Offset: 0x0026EC20
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 37);
				Switch @switch;
				VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("DataRecorder\\DataRecorderSettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(View.MarginProperty, new Thickness(5.0));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				@switch.SetValue(Grid.ColumnProperty, 1);
				@switch.SetValue(View.MarginProperty, new Thickness(5.0));
				@switch.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				bindingExtension2.Mode = 1;
				bindingExtension2.Path = "IsVisible";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				@switch.SetBinding(Switch.IsToggledProperty, bindingBase2);
				@switch.Toggled += this.root.Handle_Toggled;
				@switch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				grid.Children.Add(@switch);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04002075 RID: 8309
			internal object[] parentValues;

			// Token: 0x04002076 RID: 8310
			internal DataRecorderSettingsPage root;
		}
	}
}
